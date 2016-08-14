using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;

using FileTime = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace KeePassBrowserImporter
{
	class InternetExplorer : IBrowserImporter
	{
		#region Extern

		#region Autocomplete Passwords

		[StructLayout(LayoutKind.Sequential)]
		private struct STATURL
		{
			public int cbSize;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pwcsUrl;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pwcsTitle;
			public FileTime ftLastVisited;
			public FileTime ftLastUpdated;
			public FileTime ftExpires;
			public int dwFlags;
		}

		[ComImport]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("3C374A42-BAE4-11CF-BF7D-00AA006946EE")]
		private interface IEnumSTATURL
		{
			void Next(int celt, ref STATURL rgelt, out int pceltFetched);
		}

		[ComImport]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("3C374A41-BAE4-11CF-BF7D-00AA006946EE")]
		private interface IUrlHistoryStg
		{
			void AddUrl(string pocsUrl, string pocsTitle, int dwFlags);
			void DeleteUrl(string pocsUrl, int dwFlags);
			void QueryUrl([MarshalAs(UnmanagedType.LPWStr)] string pocsUrl, int dwFlags, ref STATURL lpSTATURL);
			void BindToObject([In] string pocsUrl, [In] int riid, IntPtr ppvOut);
			object EnumUrls
			{
				[return: MarshalAs(UnmanagedType.IUnknown)]
				get;
			}
		}

		[ComImport]
		[Guid("3C374A40-BAE4-11CF-BF7D-00AA006946EE")]
		private class CUrlHistory
		{
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct IESecretInfoHeader
		{
			public int IdHeader;
			public int Size;
			public int TotalSecrets;
			public int Unknown00;
			public int id4;
			public int Unknown01;
		};

		[StructLayout(LayoutKind.Sequential)]
		private struct IEAutoCompleteSecretHeader
		{
			public int Size;
			public int SecretInfoSize;
			public int SecretSize;
			public IESecretInfoHeader IESecretHeader;
		};

		[StructLayout(LayoutKind.Sequential)]
		private struct SecretEntry
		{
			public int Offset;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			public byte[] SecretId;
			public int Length;
		};

		#endregion

		#region HTTP Basic Authentication Passwords

		private enum CredentialType
		{
			Generic = 1,
			DomainPassword,
			DomainCertificate,
			DomainVisiblePassword,
			GenericCertificate,
			DomainExtended,
			Maximum,
			MaximumEx = Maximum + 1000,
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct Credential
		{
			public uint Flags;
			public CredentialType Type;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string TargetName;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string Comment;
			public FileTime LastWritten;
			public uint CredentialBlobSize;
			public IntPtr CredentialBlob;
			public uint Persist;
			public uint AttributeCount;
			public IntPtr Attributes;
			public IntPtr TargetAlias;
			public IntPtr UserName;
		}

		private const string ADVAPI32_DLL = "advapi32.dll";

		[DllImport(ADVAPI32_DLL, CharSet = CharSet.Unicode)]
		private static extern bool CredEnumerate(string filter, int flag, out int count, out IntPtr pCredentials);

		[DllImport(ADVAPI32_DLL)]
		private static extern bool CredFree(IntPtr cred);

		#endregion

		#region Vault Passwords (Win7+)

		private enum VAULT_ELEMENT_TYPE : uint
		{
			Boolean = 0,
			Short = 1,
			UnsignedShort = 2,
			Integer = 3,
			UnsignedInteger = 4,
			Double = 5,
			Guid = 6,
			String = 7,
			ByteArray = 8,
			TimeStamp = 9,
			ProtectedArray = 0xA,
			Attribute = 0xB,
			Sid = 0xC,
			Last = 0xD,
			Undefined = 0xFFFFFFFF
		};

		private enum VAULT_SCHEMA_ELEMENT_ID
		{
			Illegal = 0,
			Resource = 1,
			Identity = 2,
			Authenticator = 3,
			Tag = 4,
			PackageSid = 5,
			AppStart = 0x64,
			AppEnd = 0x2710
		};

		[StructLayout(LayoutKind.Sequential)]
		private struct VAULT_VARIANT
		{
			public VAULT_ELEMENT_TYPE Type;
			public uint Unknown00;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string String;
		};

		[StructLayout(LayoutKind.Sequential)]
		private struct VAULT_ITEM_ELEMENT
		{
			public VAULT_SCHEMA_ELEMENT_ID SchemaElementId;
			public uint Unknown00;
			public VAULT_VARIANT ItemValue;
		};

		[StructLayout(LayoutKind.Sequential)]
		private struct VAULT_ITEM_W7
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			public /*GUID*/byte[] SchemaId;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pszCredentialFriendlyName;
			public /*VAULT_ITEM_ELEMENT*/IntPtr pResourceElement;
			public /*VAULT_ITEM_ELEMENT*/IntPtr pIdentityElement;
			public /*VAULT_ITEM_ELEMENT*/IntPtr pAuthenticatorElement;
			public FileTime LastModified;
			public uint dwFlags;
			public uint dwPropertiesCount;
			public /*VAULT_ITEM_ELEMENT*/IntPtr pPropertyElements;
		};

		[StructLayout(LayoutKind.Sequential)]
		private struct VAULT_ITEM_W8
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			public /*GUID*/byte[] SchemaId;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pszCredentialFriendlyName;
			public /*VAULT_ITEM_ELEMENT*/IntPtr pResourceElement;
			public /*VAULT_ITEM_ELEMENT*/IntPtr pIdentityElement;
			public /*VAULT_ITEM_ELEMENT*/IntPtr pAuthenticatorElement;
			public /*VAULT_ITEM_ELEMENT*/IntPtr pPackageSid;
			public FileTime LastModified;
			public uint dwFlags;
			public uint dwPropertiesCount;
			public /*VAULT_ITEM_ELEMENT*/IntPtr pPropertyElements;
		};

		private const string VAULTCLI_DLL = "vaultcli.dll";

		[DllImport(VAULTCLI_DLL, CallingConvention = CallingConvention.StdCall)]
		private static extern uint VaultEnumerateVaults(uint dwFlags, out int VaultsCount, out IntPtr ppVaultGuids);

		[DllImport(VAULTCLI_DLL, CallingConvention = CallingConvention.StdCall)]
		private static extern uint VaultEnumerateItems(IntPtr pVaultHandle, uint dwFlags, out int ItemsCount, out IntPtr ppItems);

		[DllImport(VAULTCLI_DLL, CallingConvention = CallingConvention.StdCall, EntryPoint = "VaultGetItem")]
		private static extern uint VaultGetItem7(IntPtr pVaultHandle, IntPtr pSchemaId, IntPtr pResource, IntPtr pIdentity, IntPtr hwndOwner, uint dwFlags, out IntPtr ppItems);

		[DllImport(VAULTCLI_DLL, CallingConvention = CallingConvention.StdCall, EntryPoint = "VaultGetItem")]
		private static extern uint VaultGetItem8(IntPtr pVaultHandle, IntPtr pSchemaId, IntPtr pResource, IntPtr pIdentity, IntPtr pPackageSid, IntPtr hwndOwner, uint dwFlags, out IntPtr ppItems);

		[DllImport(VAULTCLI_DLL, CallingConvention = CallingConvention.StdCall)]
		private static extern uint VaultOpenVault(IntPtr pVaultId, uint dwFlags, out IntPtr pVaultHandle);

		[DllImport(VAULTCLI_DLL, CallingConvention = CallingConvention.StdCall)]
		private static extern uint VaultCloseVault(IntPtr pVaultHandle);

		[DllImport(VAULTCLI_DLL, CallingConvention = CallingConvention.StdCall)]
		private static extern uint VaultFree(IntPtr pMemory);

		#endregion

		#endregion

		public override bool IsAvailable { get { return Environment.OSVersion.Platform == PlatformID.Win32NT; } }

		public override bool SupportsProfiles { get { return false; } }

		public override bool SupportsMultipleProfiles { get { return false; } }

		public override IEnumerable<string> GetProfiles()
		{
			throw new NotImplementedException();
		}

		public override string GetProfilePath(string profile)
		{
			throw new NotImplementedException();
		}

		public override bool UsesMasterPassword { get { return false; } }

		public override void ImportCredentials(ImportParameter param)
		{
			foreach (var entry in ReadRegistryPasswords().Union(ReadCredentialStorePasswords()).Union(ReadVaultPasswords()))
			{
				param.Database.CreateWebsiteEntry(
					param.Group,
					entry,
					param.CreationSettings,
					param.Logger
				);
			}
		}

		/// <summary>
		/// Gets all stored history items.
		/// </summary>
		/// <returns>The history items</returns>
		private List<string> GetHistoryItems()
		{
			var urlHistory = new CUrlHistory();
			var urlHistoryStg = (IUrlHistoryStg)urlHistory;

			var history = new List<string>();

			var enumrator = (IEnumSTATURL)urlHistoryStg.EnumUrls;

			while (true)
			{
				var staturl = new STATURL();
				int index;
				enumrator.Next(1, ref staturl, out index);
				if (index == 0)
				{
					break;
				}

				var url = staturl.pwcsUrl;
				var qi = staturl.pwcsUrl.IndexOf('?');
				if (qi != -1)
				{
					url = url.Substring(0, qi);
				}

				history.Add((url + '\0').ToLower());
			}

			var typedUrls = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Internet Explorer\TypedURLs");
			if (typedUrls != null)
			{
				foreach (var name in typedUrls.GetValueNames())
				{
					var url = typedUrls.GetValue(name, string.Empty) as string;
					if (url != null)
					{
						history.Add((url + '\0').ToLower());
					}
				}
			}

			return history;
		}

		/// <summary>
		/// Enumerates the auto complete passwords stored in the registry.
		/// </summary>
		private IEnumerable<EntryInfo> ReadRegistryPasswords()
		{
			var hashToUrl = new Dictionary<string, string>();
			using (var sha1 = new SHA1Managed())
			{
				foreach (var url in GetHistoryItems())
				{
					var hash = sha1.ComputeHash(Encoding.Unicode.GetBytes(url));

					var sb = new StringBuilder();

					byte checksum = 0;
					foreach (var x in hash)
					{
						checksum += x;

						sb.AppendFormat("{0:X2}", x);
					}
					sb.AppendFormat("{0:X2}", checksum);

					hashToUrl[sb.ToString()] = url;
				}
			}

			var storage2 = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Internet Explorer\IntelliForms\Storage2");
			if (storage2 != null)
			{
				foreach (var hash in storage2.GetValueNames())
				{
					string url;
					if (hashToUrl.TryGetValue(hash, out url))
					{
						var data = storage2.GetValue(hash, null) as byte[];
						if (data != null)
						{
							data = Cryptography.DecryptUserData(data, Encoding.Unicode.GetBytes(url));
							if (data.Length > 0)
							{
								var autoCompleteHeader = FromBinaryData<IEAutoCompleteSecretHeader>(data, 0);
								if (data.Length >= autoCompleteHeader.Size + autoCompleteHeader.SecretInfoSize + autoCompleteHeader.SecretSize)
								{
									var totalSecrets = autoCompleteHeader.IESecretHeader.TotalSecrets / 2;

									var offset = Marshal.SizeOf(typeof(IEAutoCompleteSecretHeader));
									var secretOffset = autoCompleteHeader.Size + autoCompleteHeader.SecretInfoSize;

									for (var i = 0; i < autoCompleteHeader.IESecretHeader.TotalSecrets; i += 2)
									{
										var entry = FromBinaryData<SecretEntry>(data, offset);
										var username = BytePtrToStringUni(data, secretOffset + entry.Offset);

										offset += Marshal.SizeOf(typeof(SecretEntry));

										entry = FromBinaryData<SecretEntry>(data, offset);
										var password = BytePtrToStringUni(data, secretOffset + entry.Offset);

										offset += Marshal.SizeOf(typeof(SecretEntry));

										yield return new EntryInfo
										{
											Hostname = url.Trim('\0').Trim(),
											Username = username,
											Password = password,
											Created = DateTime.Now,
											Modified = DateTime.Now
										};
									}
								}
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Enumerates the HTTP Basic Authentication passwords stored in the credential store.
		/// </summary>
		private IEnumerable<EntryInfo> ReadCredentialStorePasswords()
		{
			int count;
			IntPtr credentials = IntPtr.Zero;
			try
			{
				if (CredEnumerate(null, 0, out count, out credentials))
				{
					var entropy = new byte[]
					{
						0x84, 0x01, 0x88, 0x01, 0x94, 0x01, 0xC8, 0x00,
						0xE0, 0x00, 0xD8, 0x00, 0xE4, 0x00, 0x98, 0x01,
						0xB4, 0x00, 0xE4, 0x00, 0x88, 0x01, 0xD0, 0x00,
						0xDC, 0x00, 0xB4, 0x00, 0xD0, 0x00, 0x8C, 0x01,
						0x90, 0x01, 0xE4, 0x00, 0xB4, 0x00, 0x84, 0x01,
						0xCC, 0x00, 0xD4, 0x00, 0xE0, 0x00, 0xB4, 0x00,
						0x8C, 0x01, 0xC8, 0x00, 0xC8, 0x00, 0xE4, 0x00,
						0xC0, 0x00, 0xD0, 0x00, 0x90, 0x01, 0x88, 0x01,
						0x84, 0x01, 0xDC, 0x00, 0x98, 0x01, 0xDC, 0x00,
						0x00, 0x00
					};

					for (int i = 0; i < count; i++)
					{
						var credPtr = Marshal.ReadIntPtr(credentials, i * Marshal.SizeOf(typeof(IntPtr)));
						var cred = (Credential)Marshal.PtrToStructure(credPtr, typeof(Credential));
						if (cred.Type == CredentialType.Generic && cred.CredentialBlobSize > 0)
						{
							var blob = new byte[cred.CredentialBlobSize];
							Marshal.Copy(cred.CredentialBlob, blob, 0, blob.Length);

							var data = Cryptography.DecryptUserData(blob, entropy);
							if (data.Length > 0)
							{
								var credStr = Encoding.Unicode.GetString(data).TrimEnd('\0');
								var splitOffset = credStr.IndexOf(':');
								if (splitOffset != -1)
								{
									var date = DateUtils.FromFileTime(cred.LastWritten);

									yield return new EntryInfo
									{
										Hostname = cred.TargetName,
										Username = credStr.Substring(0, splitOffset),
										Password = credStr.Substring(splitOffset + 1),
										Created = date,
										Modified = date
									};
								}
							}
						}
					}
				}
			}
			finally
			{
				if (credentials != IntPtr.Zero)
				{
					CredFree(credentials);
				}
			}
		}

		/// <summary>
		/// Enumerates the passwords stored in the password vault.
		/// </summary>
		private IEnumerable<EntryInfo> ReadVaultPasswords()
		{
			if (!(Environment.OSVersion.Version.Major > 6 || (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor >= 1)))
			{
				yield break;
			}

			var Vault_WebCredential_ID = Guid.Parse("3CCD5499-87A8-4B10-A215-608888DD3B55");

			const uint ERROR_SUCCESS = 0;
			const uint VAULT_ENUMERATE_ALL_ITEMS = 512;

			bool isWin8 = Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor > 1;

			int vaultCount;
			IntPtr vaultGuids = IntPtr.Zero;
			try
			{
				if (VaultEnumerateVaults(0, out vaultCount, out vaultGuids) == ERROR_SUCCESS)
				{
					for (var i = 0; i < vaultCount; ++i)
					{
						IntPtr vault = IntPtr.Zero;
						try
						{
							var vaultIdPtr = vaultGuids + i * Marshal.SizeOf(typeof(Guid));
							if (VaultOpenVault(vaultIdPtr, 0, out vault) == ERROR_SUCCESS)
							{
								int itemCount;
								IntPtr items = IntPtr.Zero;
								try
								{
									if (VaultEnumerateItems(vault, VAULT_ENUMERATE_ALL_ITEMS, out itemCount, out items) == ERROR_SUCCESS)
									{
										for (var j = 0; j < itemCount; ++j)
										{
											string host;
											string username;
											string password = string.Empty;
											DateTime date = DateTime.Now;
											if (isWin8)
											{
												var itemPtr = items + i * Marshal.SizeOf(typeof(VAULT_ITEM_W8));
												var item = (VAULT_ITEM_W8)Marshal.PtrToStructure(itemPtr, typeof(VAULT_ITEM_W8));
												if (!Vault_WebCredential_ID.Equals(new Guid(item.SchemaId)))
												{
													continue;
												}

												var resourceElement = (VAULT_ITEM_ELEMENT)Marshal.PtrToStructure(item.pResourceElement, typeof(VAULT_ITEM_ELEMENT));
												host = resourceElement.ItemValue.String;
												var identityElement = (VAULT_ITEM_ELEMENT)Marshal.PtrToStructure(item.pIdentityElement, typeof(VAULT_ITEM_ELEMENT));
												username = identityElement.ItemValue.String;

												if (item.dwPropertiesCount > 0)
												{
													IntPtr propertyPtr;
													if (VaultGetItem8(vault, itemPtr, item.pResourceElement, item.pIdentityElement, IntPtr.Zero, IntPtr.Zero, 0, out propertyPtr) == ERROR_SUCCESS)
													{
														var property = (VAULT_ITEM_W8)Marshal.PtrToStructure(propertyPtr, typeof(VAULT_ITEM_W8));

														date = DateUtils.FromFileTime(property.LastModified);

														var authenticatorElement = (VAULT_ITEM_ELEMENT)Marshal.PtrToStructure(property.pAuthenticatorElement, typeof(VAULT_ITEM_ELEMENT));
														password = authenticatorElement.ItemValue.String;

														VaultFree(propertyPtr);
													}
												}
											}
											else
											{
												var itemPtr = items + i * Marshal.SizeOf(typeof(VAULT_ITEM_W7));
												var item = (VAULT_ITEM_W7)Marshal.PtrToStructure(itemPtr, typeof(VAULT_ITEM_W7));
												if (!Vault_WebCredential_ID.Equals(new Guid(item.SchemaId)))
												{
													continue;
												}

												var resourceElement = (VAULT_ITEM_ELEMENT)Marshal.PtrToStructure(item.pResourceElement, typeof(VAULT_ITEM_ELEMENT));
												host = resourceElement.ItemValue.String;
												var identityElement = (VAULT_ITEM_ELEMENT)Marshal.PtrToStructure(item.pIdentityElement, typeof(VAULT_ITEM_ELEMENT));
												username = identityElement.ItemValue.String;

												if (item.dwPropertiesCount > 0)
												{
													IntPtr propertyPtr;
													if (VaultGetItem7(vault, itemPtr, item.pResourceElement, item.pIdentityElement, IntPtr.Zero, 0, out propertyPtr) == ERROR_SUCCESS)
													{
														var property = (VAULT_ITEM_W7)Marshal.PtrToStructure(propertyPtr, typeof(VAULT_ITEM_W7));

														date = DateUtils.FromFileTime(property.LastModified);

														var authenticatorElement = (VAULT_ITEM_ELEMENT)Marshal.PtrToStructure(property.pAuthenticatorElement, typeof(VAULT_ITEM_ELEMENT));
														password = authenticatorElement.ItemValue.String;

														VaultFree(propertyPtr);
													}
												}
											}

											yield return new EntryInfo
											{
												Hostname = host,
												Username = username,
												Password = password,
												Created = date,
												Modified = date
											};
										}
									}
								}
								finally
								{
									if (items != IntPtr.Zero)
									{
										VaultFree(items);
									}
								}
							}
						}
						finally
						{
							if (vault != IntPtr.Zero)
							{
								VaultCloseVault(vault);
							}
						}
					}
				}
			}
			finally
			{
				if (vaultGuids != IntPtr.Zero)
				{
					VaultFree(vaultGuids);
				}
			}
			yield break;
		}

		/// <summary>
		/// Maps a structure from binary data.
		/// </summary>
		/// <param name="data">The binary data</param>
		/// <param name="offset">The offset</param>
		/// <returns></returns>
		private static T FromBinaryData<T>(byte[] data, int offset)
		{
			Contract.Requires(data != null);
			Contract.Requires(data.Length >= offset + Marshal.SizeOf(typeof(T)));

			var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
			var result = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject() + offset, typeof(T));
			handle.Free();

			return result;
		}

		/// <summary>
		/// Reads an unicode string from binary data.
		/// </summary>
		/// <param name="data">The binary data</param>
		/// <param name="offset">The offset</param>
		/// <returns></returns>
		private static string BytePtrToStringUni(byte[] data, int offset)
		{
			Contract.Requires(data != null);
			Contract.Requires(offset >= 0);
			Contract.Requires(data.Length >= offset);

			var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
			var result = Marshal.PtrToStringUni(handle.AddrOfPinnedObject() + offset);
			handle.Free();

			return result;
		}
	}
}
