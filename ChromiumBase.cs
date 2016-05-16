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

		public override bool IsAvailable { get { return Directory.Exists(path); } }

		public override bool UsesMasterPassword { get { return false; } }

		/// <summary>
		/// Import credentials from the "Login Data" file.
		/// </summary>
		/// <exception cref="FileNotFoundException">Thrown when the database is not present.</exception>
		/// <param name="param">The parameters for the import</param>
		public override void ImportCredentials(ImportParameter param)
		{
			var currentProfilePath = !string.IsNullOrEmpty(param.Profile) ? Path.Combine(ProfilePath, param.Profile) : ProfilePath;
			if (!Directory.Exists(currentProfilePath))
			{
				throw new FileNotFoundException(currentProfilePath);
			}

			var loginDataPath = Path.Combine(currentProfilePath, "Login Data");
			if (!File.Exists(loginDataPath))
			{
				throw new FileNotFoundException(loginDataPath);
			}

			using (var db = new DBHandler(loginDataPath))
			{
				DataTable dt;
				db.Query(out dt, "SELECT origin_url, username_value, password_value FROM logins");

				foreach (var row in dt.AsEnumerable())
				{
					param.Database.CreateWebsiteEntry(
						param.Group,
						row["origin_url"] as string,
						row["username_value"] as string,
						Encoding.UTF8.GetString(Cryptography.DecryptUserData(row["password_value"] as byte[])),
						param.ExtractTitle,
						param.ExtractIcon,
						param.Logger
					);
				}
			}
		}
	}
}
