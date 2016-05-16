using System;
using System.Net;

namespace KeePassBrowserImporter
{
	public class WebClientEx : WebClient
	{
		private CookieContainer cookies = new CookieContainer();

		/// <summary>
		/// Builds a WebRequest which does look like a normal request from Firefox.
		/// </summary>
		protected override WebRequest GetWebRequest(Uri address)
		{
			var request = base.GetWebRequest(address) as HttpWebRequest;

			request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:46.0) Gecko/20100101 Firefox/46.0";
			request.CookieContainer = cookies;
			request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
			request.Headers.Add(HttpRequestHeader.AcceptLanguage, "*");
			request.Timeout = 10000;

			return request;
		}
	}
}
