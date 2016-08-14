using System.Diagnostics.Contracts;
using System.Drawing;

using KeePass.Plugins;
using KeePass.DataExchange;

namespace KeePassBrowserImporter
{
	public class KeePassBrowserImporterExt : Plugin
	{
		private IPluginHost host = null;
		private FileFormatProvider provider = null;

		public override Image SmallIcon
		{
			get { return Properties.Resources.B16x16_Combined; }
		}

		public override string UpdateUrl
		{
			get { return "https://github.com/KN4CK3R/KeePassBrowserImporter/raw/master/keepass.version"; }
		}

		public override bool Initialize(IPluginHost host)
		{
			Contract.Requires(host != null);

			//System.Diagnostics.Debugger.Launch();

			this.host = host;

			provider = new BrowserProvider();

			host.FileFormatPool.Add(provider);

			return true;
		}

		public override void Terminate()
		{
			if (host != null)
			{
				host.FileFormatPool.Remove(provider);
			}
		}
	}
}
