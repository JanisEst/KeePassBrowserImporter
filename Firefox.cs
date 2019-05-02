using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using KeePass.DataExchange;
using KeePassLib.Utility;

namespace KeePassBrowserImporter
{
	internal class Firefox : IBrowserImporter
	{
		#region Extern

		private const string KERNEL32_DLL = "kernel32.dll";

		[DllImport(KERNEL32_DLL)]
		private static extern bool SetDllDirectory(string lpPathName);

		[DllImport(KERNEL32_DLL, SetLastError = true)]
		private static extern IntPtr LoadLibrary(string fileName);

		[DllImport(KERNEL32_DLL)]
		private static extern void FreeLibrary(IntPtr handle);

		private enum SECStatus
		{
			WouldBlock = -2,
			Failure = -1,
			Success = 0
		}

		private enum SECItemType
		{
			Buffer,
			ClearDataBuffer,
			CipherDataBuffer,
			DERCertBuffer,
			EncodedCertBuffer,
			DERNameBuffer,
			EncodedNameBuffer,
			AsciiNameString,
			AsciiString,
			DEROID
		}

		private struct SECItem
		{
			public SECItemType Type;
			public IntPtr Data;
			public int Length;
		};

		private const string NSS3_DLL = "nss3.dll";

		[DllImport(NSS3_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern SECStatus NSS_Init(string configdir);

		[DllImport(NSS3_DLL, CallingConvention = CallingConvention.Cdecl)]
		private static extern SECStatus NSS_Shutdown();

		[DllImport(NSS3_DLL, CallingConvention = CallingConvention.Cdecl)]
		private static extern SECStatus PK11_Authenticate(IntPtr slot, bool loadCerts, IntPtr context);

		[DllImport(NSS3_DLL, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern SECStatus PK11_CheckUserPassword(IntPtr slot, string pw);

		[DllImport(NSS3_DLL, CallingConvention = CallingConvention.Cdecl)]
		private static extern void PK11_FreeSlot(IntPtr slot);

		[DllImport(NSS3_DLL, CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr PK11_GetInternalKeySlot();

		[DllImport(NSS3_DLL, CallingConvention = CallingConvention.Cdecl)]
		private static extern SECStatus PK11SDR_Decrypt(ref SECItem data, ref SECItem result, IntPtr context);

		[DllImport(NSS3_DLL, CallingConvention = CallingConvention.Cdecl)]
		private static extern void SECITEM_FreeItem(ref SECItem item, bool freeit);

		#endregion

		public class NetworkSecurityServicesException : Exception
		{
			public NetworkSecurityServicesException(string message)
				: base(message)
			{

			}
		}

		private readonly string profileDirectory;

		/// <summary>Constructs the object and builds the profile path.</summary>
		public Firefox()
		{
			profileDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Mozilla\Firefox\Profiles");
		}

		public override bool SupportsProfiles { get { return true; } }

		public override bool SupportsMultipleProfiles { get { return true; } }

		/// <summary>Gets all subdirectories in the profiles directory.</summary>
		public override IEnumerable<string> GetProfiles()
		{
			if (Directory.Exists(profileDirectory))
			{
				return Directory.EnumerateDirectories(profileDirectory).Select(s => Path.GetFileName(s));
			}
			return Enumerable.Empty<string>();
		}

		public override string GetProfilePath(string profile)
		{
			return Path.Combine(profileDirectory, profile);
		}

		public override bool UsesMasterPassword { get { return true; } }

		/// <summary>
		/// Import the credentials with the Network Security Services methods.
		/// </summary>
		/// <exception cref="ProfileNotFoundException">Thrown when the requested profile is not present.</exception>
		/// <exception cref="NetworkSecurityServicesException">Thrown when a Network Security Services error condition occurs.</exception>
		/// <param name="param">The parameters for the import</param>
		public override void ImportCredentials(ImportParameter param)
		{
			var currentProfilePath = !string.IsNullOrEmpty(param.CustomProfilePath)
				? param.CustomProfilePath
				: Path.Combine(profileDirectory, param.Profile);
			if (!Directory.Exists(currentProfilePath))
			{
				throw new ProfileNotFoundException(currentProfilePath);
			}

			var pluginPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var nativeLibraryPath = Path.Combine(pluginPath, Environment.Is64BitProcess ? "x64" : "x86");

			SetDllDirectory(nativeLibraryPath);

			var nss3Handle = LoadLibrary(Path.Combine(nativeLibraryPath, NSS3_DLL));
			if (nss3Handle == IntPtr.Zero)
			{
				throw new Win32Exception(string.Format("Could not load file: {0}", Path.Combine(nativeLibraryPath, NSS3_DLL)));
			}

			var slot = IntPtr.Zero;
			try
			{
				if (NSS_Init(currentProfilePath) != SECStatus.Success)
				{
					throw new NetworkSecurityServicesException("NSS_Init failed: Wrong profile path?\n\n" + currentProfilePath);
				}

				if ((slot = PK11_GetInternalKeySlot()) == IntPtr.Zero)
				{
					throw new NetworkSecurityServicesException("PK11_GetInternalKeySlot failed");
				}

				if (PK11_CheckUserPassword(slot, param.Password) != SECStatus.Success)
				{
					throw new NetworkSecurityServicesException("PK11_CheckUserPassword failed: Wrong Master Password?");
				}

				if (PK11_Authenticate(slot, false, IntPtr.Zero) != SECStatus.Success)
				{
					throw new NetworkSecurityServicesException("PK11_Authenticate failed");
				}

				var exceptions = new List<Exception>();
				ImportCredentialsImpl(param, () => ReadLoginsFile(currentProfilePath), ref exceptions);
				ImportCredentialsImpl(param, () => ReadSignonsFile(currentProfilePath), ref exceptions);

				if (exceptions.Any())
				{
					var combinedMessages = string.Join("\n\n", exceptions.Select(StrUtil.FormatException));

					throw new Exception(combinedMessages);
				}
			}
			finally
			{
				if (slot != IntPtr.Zero)
				{
					PK11_FreeSlot(slot);
				}

				NSS_Shutdown();

				if (nss3Handle != IntPtr.Zero)
				{
					FreeLibrary(nss3Handle);
				}
			}
		}

		/// <summary>
		/// Imports the credentials provided by the <see cref="importFunc"/>.
		/// </summary>
		/// <param name="param">The parameters for the import</param>
		/// <param name="importFunc">The provider for the credential entries.</param>
		/// <param name="occuredExceptions">List with occured exceptions.</param>
		private static void ImportCredentialsImpl(ImportParameter param, Func<IEnumerable<EntryInfo>> importFunc, ref List<Exception> occuredExceptions)
		{
			try
			{
				foreach (var entry in importFunc())
				{
					param.Database.CreateWebsiteEntry(
						param.Group,
						entry,
						param.CreationSettings,
						param.Logger
					);
				}
			}
			catch (Exception ex)
			{
				occuredExceptions.Add(ex);
			}
		}

		/// <summary>
		/// Enumerates the entries of the signons.sqlite file.
		/// </summary>
		/// <param name="profilePath">Path of the profile folder</param>
		/// <returns></returns>
		private static IEnumerable<EntryInfo> ReadSignonsFile(string profilePath)
		{
			var entries = new List<EntryInfo>();

			var dbPath = Path.Combine(profilePath, "signons.sqlite");
			if (File.Exists(dbPath))
			{
				try
				{
					using (var db = new DBHandler(dbPath))
					{
						DataTable dt;
						db.Query(out dt, "SELECT hostname, encryptedUsername, encryptedPassword, timeCreated, timePasswordChanged FROM moz_logins");

						foreach (var row in dt.AsEnumerable())
						{
							try
							{
								entries.Add(new EntryInfo
								{
									Hostname = (row["hostname"] as string).Trim(),
									Username = PK11_Decrypt(row["encryptedUsername"] as string).Trim(),
									Password = PK11_Decrypt(row["encryptedPassword"] as string),
									Created = DateUtils.FromUnixTimeMilliseconds((long)row["timeCreated"]),
									Modified = DateUtils.FromUnixTimeMilliseconds((long)row["timePasswordChanged"])
								});
							}
							catch
							{
								// Skip faulty entries
							}
						}
					}
				}
				catch (DbException ex)
				{
					throw new Exception(string.Format("Error while using the browsers login database. It may help to close all running instances of the browser.\n\n{0}", StrUtil.FormatException(ex)), ex);
				}
			}

			return entries;
		}

		/// <summary>
		/// Enumerates the entries of the logins.json file.
		/// </summary>
		/// <param name="profilePath">Path of the profile folder</param>
		/// <returns></returns>
		private static IEnumerable<EntryInfo> ReadLoginsFile(string profilePath)
		{
			var entries = new List<EntryInfo>();

			var path = Path.Combine(profilePath, "logins.json");
			if (File.Exists(path))
			{
				var root = new JsonObject(new CharStream(File.ReadAllText(path)));

				var logins = root.GetValueArray<JsonObject>("logins");
				foreach (var item in logins)
				{
					try
					{
						entries.Add(new EntryInfo
						{
							Hostname = item.GetValue<string>("hostname").Trim(),
							Username = PK11_Decrypt(item.GetValue<string>("encryptedUsername").Trim()),
							Password = PK11_Decrypt(item.GetValue<string>("encryptedPassword")),
							Created = DateUtils.FromUnixTimeMilliseconds(item.GetValue("timeCreated", 0L)),
							Modified = DateUtils.FromUnixTimeMilliseconds(item.GetValue("timePasswordChanged", 0L))
						});
					}
					catch
					{
						// Skip faulty entries
					}
				}
			}

			return entries;
		}

		/// <summary>
		/// Helper to decrypt ciphered text.
		/// </summary>
		/// <param name="cipheredText">The ciphered text</param>
		/// <returns>A decrypted string</returns>
		private static string PK11_Decrypt(string cipheredText)
		{
			var reply = default(SECItem);

			var rawData = Convert.FromBase64String(cipheredText);

			var handle = GCHandle.Alloc(rawData, GCHandleType.Pinned);
			try
			{
				var request = new SECItem
				{
					Data = handle.AddrOfPinnedObject(),
					Length = rawData.Length
				};

				PK11SDR_Decrypt(ref request, ref reply, IntPtr.Zero);

				var tmp = new byte[reply.Length];
				Marshal.Copy(reply.Data, tmp, 0, reply.Length);
				return System.Text.Encoding.UTF8.GetString(tmp);
			}
			finally
			{
				if (handle.IsAllocated)
				{
					handle.Free();
				}
				if (reply.Data != IntPtr.Zero)
				{
					try
					{
						SECITEM_FreeItem(ref reply, false);
					}
					catch
					{
						
					}
				}
			}
		}
	}
}
