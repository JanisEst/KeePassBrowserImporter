using KeePassLib;
using KeePassLib.Interfaces;
using System.Collections.Generic;

namespace KeePassBrowserImporter
{
	public class CreationSettings
	{
		public bool ExtractTitle;
		public bool ExtractIcon;
		public bool UseDates;
	}

	public class ImportParameter
	{
		public IStatusLogger Logger;

		public PwDatabase Database;
		public PwGroup Group;

		public string Profile;
		public string CustomProfilePath;

		public string Password;

		public CreationSettings CreationSettings;
	}

	public abstract class IBrowserImporter
	{
		/// <summary>Checks if the importer is available.</summary>
		public abstract bool IsAvailable { get; }

		/// <summary>Checks if the importer supports profiles.</summary>
		public abstract bool SupportsProfiles { get; }

		/// <summary>Checks if the importer supports multiple profiles.</summary>
		public abstract bool SupportsMultipleProfiles { get; }

		/// <summary>Gets all availables profiles.</summary>
		public abstract IEnumerable<string> GetProfiles();

		/// <summary>Gets the path of the given profile.</summary>
		public abstract string GetProfilePath(string profile);

		/// <summary>Checks if the importer uses a master password.</summary>
		public abstract bool UsesMasterPassword { get; }

		/// <summary>Import the credentials from the browser.</summary>
		public abstract void ImportCredentials(ImportParameter param);
	}
}
