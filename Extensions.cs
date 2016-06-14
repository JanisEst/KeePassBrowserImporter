using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace KeePassBrowserImporter
{
	public static class Extensions
	{
		[DllImport("user32.dll")]
		private extern static bool DestroyIcon(IntPtr handle);

		/// <summary>A Bitmap extension method that converts a bmp to an icon.</summary>
		/// <param name="bmp">The bmp to act on.</param>
		/// <returns>The new icon.</returns>
		public static Icon ToIcon(this Bitmap bmp)
		{
			var hicon = bmp.GetHicon();

			var icon = Icon.FromHandle(hicon);
			icon = icon.Clone() as Icon;

			DestroyIcon(hicon);

			return icon;
		}
	}
}
