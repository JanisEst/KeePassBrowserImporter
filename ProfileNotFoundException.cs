using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeePassBrowserImporter
{
	public class ProfileNotFoundException : Exception
	{
		public ProfileNotFoundException(string path)
			: base("Profile not found. Used path:\n\n" + path)
		{

		}
	}
}
