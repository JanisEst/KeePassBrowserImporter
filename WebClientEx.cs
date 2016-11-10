using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

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

		/// <summary>Downloads the string and uses the encoding provided by the html headers to interpret it.</summary>
		public string DownloadStringAwareOfEncoding(string address)
		{
			var rawData = DownloadData(address);
			var encoding = GetEncodingFromHeaders(ResponseHeaders, Encoding.UTF8);
			return encoding.GetString(rawData);
		}

		/// <summary>Gets the encoding from the response headers.</summary>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="responseHeaders"/> are null.</exception>
		/// <param name="responseHeaders">The response headers.</param>
		/// <param name="defaultEncoding">(Optional) The default encoding.</param>
		/// <returns>The encoding from headers or <paramref name="defaultEncoding"/> if it is not valid.</returns>
		public static Encoding GetEncodingFromHeaders(NameValueCollection responseHeaders, Encoding defaultEncoding = null)
		{
			if (responseHeaders == null)
			{
				throw new ArgumentNullException();
			}

			var contentType = responseHeaders["Content-Type"];
			if (contentType == null)
			{
				return defaultEncoding;
			}

			var contentTypeParts = contentType.Split(';');
			if (contentTypeParts.Length <= 1)
			{
				return defaultEncoding;
			}

			var charsetPart = contentTypeParts.Skip(1).FirstOrDefault(p => p.TrimStart().StartsWith("charset", StringComparison.InvariantCultureIgnoreCase));
			if (charsetPart == null)
			{
				return defaultEncoding;
			}

			var charsetPartParts = charsetPart.Split('=');
			if (charsetPartParts.Length != 2)
			{
				return defaultEncoding;
			}

			var charsetName = charsetPartParts[1].Trim();
			if (charsetName == string.Empty)
			{
				return defaultEncoding;
			}

			try
			{
				return Encoding.GetEncoding(charsetName);
			}
			catch (ArgumentException)
			{
				return defaultEncoding;
			}
		}
	}
}
