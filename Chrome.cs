using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KeePassBrowserImporter
{
	internal class Chrome : ChromiumBase
	{
		public Chrome()
			: base(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Google\Chrome\User Data"))
		{
		}

		public override bool SupportsMultipleProfiles { get { return true; } }

		/// <summary>Gets the folders which contain the "Login Data" file.</summary>
		public override IEnumerable<string> GetProfiles()
		{
			var profileDir = new DirectoryInfo(ProfilePath);
			if (profileDir.Exists)
			{
				foreach (var dir in profileDir.EnumerateDirectories())
				{
					if (dir.EnumerateFiles().Any(f => f.Name == "Login Data"))
					{
						yield return dir.Name;
					}
				}
			}
		}
	}
}
