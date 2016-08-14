using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeePassLib;
using System.Diagnostics.Contracts;
using System.IO;
using System.Data;

namespace KeePassBrowserImporter
{
	internal abstract class ChromiumBase : IBrowserImporter
	{
		private string path;
		public string ProfilePath { get { return path; } }

		public ChromiumBase(string path)
		{
			Contract.Requires(!string.IsNullOrEmpty(path));

			this.path = path;
		}

		public override bool SupportsProfiles { get { return true; } }

		public override string GetProfilePath(string profile)
		{
			return Path.Combine(ProfilePath, profile);
		}

		public override bool IsAvailable { get { return Directory.Exists(path); } }

		public override bool UsesMasterPassword { get { return false; } }

		/// <summary>
		/// Import credentials from the "Login Data" file.
		/// </summary>
		/// <exception cref="FileNotFoundException">Thrown when the database is not present.</exception>
		/// <param name="param">The parameters for the import</param>
		public override void ImportCredentials(ImportParameter param)
		{
			var currentProfilePath = !string.IsNullOrEmpty(param.CustomProfilePath)
				? param.CustomProfilePath
				: !string.IsNullOrEmpty(param.Profile)
					? Path.Combine(ProfilePath, param.Profile)
					: ProfilePath;
			if (!Directory.Exists(currentProfilePath))
			{
				throw new ProfileNotFoundException(currentProfilePath);
			}

			var loginDataPath = Path.Combine(currentProfilePath, "Login Data");
			if (!File.Exists(loginDataPath))
			{
				throw new ProfileNotFoundException(loginDataPath);
			}

			using (var db = new DBHandler(loginDataPath))
			{
				DataTable dt;
				db.Query(out dt, "SELECT origin_url, username_value, password_value, date_created FROM logins");

				foreach (var row in dt.AsEnumerable())
				{
					var date = DateUtils.FromChromiumTime((long)row["date_created"]);

					var entry = new EntryInfo
					{
						Hostname = row["origin_url"] as string,
						Username = row["username_value"] as string,
						Password = Encoding.UTF8.GetString(Cryptography.DecryptUserData(row["password_value"] as byte[])),
						Created = date,
						Modified = date
					};

					param.Database.CreateWebsiteEntry(
						param.Group,
						entry,
						param.CreationSettings,
						param.Logger
					);
				}
			}
		}
	}
}
