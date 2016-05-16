using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KeePassBrowserImporter
{
	internal class Opera : ChromiumBase
	{
		public Opera()
			: base(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Opera Software\Opera Stable"))
		{
		}

		public override bool SupportsMultipleProfiles { get { return false; } }

		public override IEnumerable<string> GetProfiles()
		{
			return Enumerable.Empty<string>();
		}
	}
}
