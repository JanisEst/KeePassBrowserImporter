using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using KeePass.DataExchange;
using KeePass.Resources;
using KeePassLib;
using KeePassLib.Interfaces;

namespace KeePassBrowserImporter
{
	internal class BrowserProvider : FileFormatProvider
	{
		public override bool SupportsImport { get { return true; } }
		public override bool SupportsExport { get { return false; } }

		public override string FormatName { get { return "Generic Browser Importer"; } }
		public override bool RequiresFile { get { return false; } }
		public override string ApplicationGroup { get { return KPRes.Browser; } }

		public override Image SmallIcon { get { return Properties.Resources.B16x16_Combined; } }

		public override bool ImportAppendsToRootGroupOnly { get { return false; } }

		public override void Import(PwDatabase pwStorage, Stream sInput,
			IStatusLogger slLogger)
		{
			//Debugger.Launch();

			var pf = new ProviderForm(pwStorage);
			if (pf.ShowDialog() == DialogResult.OK)
			{
				var provider = pf.SelectedProvider;
				var parms = new ImportParameter
				{
					Logger = slLogger,

					Database = pwStorage,
					Group = pf.SelectedGroup ?? pwStorage.RootGroup,

					Profile = pf.SelectedProfile,
					CustomProfilePath = pf.CustomProfilePath,

					Password = pf.MasterPassword,

					CreationSettings = new CreationSettings
					{
						ExtractTitle = pf.ExtractTitle,
						ExtractIcon = pf.ExtractIcon,
						UseDates = pf.UseDates
					}
				};

				//could take some time if the webpages need to get crawled
				var task = Task.Factory.StartNew(() => provider.ImportCredentials(parms));

				while (!task.IsCompleted)
				{
					Thread.Sleep(100);
					Application.DoEvents();
				}

				if (task.IsFaulted)
				{
					throw new Exception($"Error while importing credentials from browser. Some credentials may have been imported.\n\n{task.Exception?.Message}", task.Exception == null ? null : task.Exception.InnerException);
				}
			}
		}
	}
}
