using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace KeePassBrowserImporter
{
	public class Cryptography
	{
		#region Extern

		[DllImport("crypt32.dll")]
		private static extern bool CryptUnprotectData(
			DATA_BLOB pCipherText,
			IntPtr pszDescription,
			DATA_BLOB pEntropy,
			IntPtr pReserved,
			IntPtr pPrompt,
			int dwFlags,
			DATA_BLOB pPlainText
		);

		[StructLayout(LayoutKind.Sequential)]
		internal class DATA_BLOB
		{
			public int cbData;
			public IntPtr pbData;

			public static DATA_BLOB CreateFrom(byte[] data)
			{
				var blob = new DATA_BLOB();

				if (data == null)
				{
					data = new byte[0];
				}

				blob.pbData = Marshal.AllocHGlobal(data.Length);

				if (blob.pbData == IntPtr.Zero)
				{
					throw new Exception();
				}

				blob.cbData = data.Length;

				Marshal.Copy(data, 0, blob.pbData, data.Length);

				return blob;
			}
		}

		#endregion

		/// <summary>
		/// Decrypt the provided data using CryptUnprotectData.
		/// </summary>
		/// <param name="data">The data to decrypt</param>
		/// <returns>The decrypted data</returns>
		public static byte[] DecryptUserData(byte[] data)
		{
			return DecryptUserData(data, null);
		}

		/// <summary>
		/// Decrypt the provided data using CryptUnprotectData.
		/// </summary>
		/// <param name="data">The data to decrypt</param>
		/// <param name="entropy">The entropy to use (can be null)</param>
		/// <returns>The decrypted data</returns>
		public static byte[] DecryptUserData(byte[] data, byte[] entropy)
		{
			Contract.Requires(data != null);

			var result = new byte[0];

			var plain = new DATA_BLOB();
			if (CryptUnprotectData(DATA_BLOB.CreateFrom(data), IntPtr.Zero, entropy != null ? DATA_BLOB.CreateFrom(entropy) : null, IntPtr.Zero, IntPtr.Zero, 0, plain))
			{
				result = new byte[plain.cbData];

				Marshal.Copy(plain.pbData, result, 0, plain.cbData);
			}

			return result;
		}
	}
}
