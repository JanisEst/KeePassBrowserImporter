using HtmlAgilityPack;
using KeePassLib;
using KeePassLib.Interfaces;
using KeePassLib.Security;
using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace KeePassBrowserImporter
{
	public static class PwDatabaseExtension
	{
		/// <summary>
		/// A PwDatabase extension method that creates a website entry.
		/// </summary>
		/// <param name="pd">The database to act on</param>
		/// <param name="group">The group to insert new entries into</param>
		/// <param name="host">The host</param>
		/// <param name="username">The username</param>
		/// <param name="password">The password</param>
		/// <param name="creationSettings">Settings used while creating the entry</param>
		/// <param name="logger">The logger</param>
		public static void CreateWebsiteEntry(this PwDatabase pd, PwGroup group, EntryInfo entry, CreationSettings creationSettings, IStatusLogger logger)
		{
			Contract.Requires(group != null);
			Contract.Requires(entry != null);
			Contract.Requires(creationSettings != null);
			Contract.Requires(logger != null);

			logger.SetText(string.Format("{0} - {1}", entry.Username, entry.Hostname), LogStatusType.Info);

			var pe = new PwEntry(true, true);
			group.AddEntry(pe, true);

			pe.Strings.Set(PwDefs.TitleField, new ProtectedString(pd.MemoryProtection.ProtectTitle, entry.Hostname));
			pe.Strings.Set(PwDefs.UserNameField, new ProtectedString(pd.MemoryProtection.ProtectUserName, entry.Username));
			pe.Strings.Set(PwDefs.PasswordField, new ProtectedString(pd.MemoryProtection.ProtectPassword, entry.Password));
			pe.Strings.Set(PwDefs.UrlField, new ProtectedString(pd.MemoryProtection.ProtectUrl, entry.Hostname));

			if (creationSettings.UseDates)
			{
				pe.CreationTime = entry.Created;
				pe.LastModificationTime = entry.Modified;
			}

			if (!string.IsNullOrEmpty(entry.Hostname) && (creationSettings.ExtractIcon || creationSettings.ExtractTitle))
			{
				try
				{
					string content;
					using (var client = new WebClientEx())
					{
						content = client.DownloadString(entry.Hostname);

						var document = new HtmlDocument();
						document.LoadHtml(content);

						if (creationSettings.ExtractTitle)
						{
							var title = document.DocumentNode.SelectSingleNode("/html/head/title");
							if (title != null)
							{
								pe.Strings.Set(PwDefs.TitleField, new ProtectedString(pd.MemoryProtection.ProtectTitle, HttpUtility.HtmlDecode(title.InnerText.Trim())));
							}
						}

						if (creationSettings.ExtractIcon)
						{
							string iconUrl = null;
							foreach (var prio in new string[] { "shortcut icon", "apple-touch-icon", "icon" })
							{
								//iconUrl = document.DocumentNode.SelectNodes("/html/head/link").Where(l => prio == l.Attributes["rel"]?.Value).LastOrDefault()?.Attributes["href"]?.Value;
								var node = document.DocumentNode.SelectNodes("/html/head/link").Where(l => l.GetAttributeValue("rel", string.Empty) == prio).LastOrDefault();
								if (node != null)
								{
									iconUrl = node.GetAttributeValue("href", string.Empty);
								}
								
								if (!string.IsNullOrEmpty(iconUrl))
								{
									break;
								}
							}

							if (!string.IsNullOrEmpty(iconUrl))
							{
								if (!iconUrl.StartsWith("http://") && !iconUrl.StartsWith("https://"))
								{
									iconUrl = entry.Hostname.TrimEnd('/') + '/' + iconUrl.TrimStart('/');
								}

								using (var s = client.OpenRead(iconUrl))
								{
									var icon = Image.FromStream(s);
									if (icon.Width > 16 || icon.Height > 16)
									{
										icon = icon.GetThumbnailImage(16, 16, null, IntPtr.Zero);
									}

									using (var ms = new MemoryStream())
									{
										icon.Save(ms, ImageFormat.Png);

										var data = ms.ToArray();

										var createNewIcon = true;
										foreach (var item in pd.CustomIcons)
										{
											if (KeePassLib.Utility.MemUtil.ArraysEqual(data, item.ImageDataPng))
											{
												pe.CustomIconUuid = item.Uuid;

												createNewIcon = false;

												break;
											}
										}

										if (createNewIcon)
										{
											var pwci = new PwCustomIcon(new PwUuid(true), data);
											pd.CustomIcons.Add(pwci);
											pe.CustomIconUuid = pwci.Uuid;
										}
									}
								}

								pd.UINeedsIconUpdate = true;
							}
						}
					}
				}
				catch
				{

				}
			}
		}
	}
}
