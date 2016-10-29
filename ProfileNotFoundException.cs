using System;

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
