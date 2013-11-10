using System;
using System.Net;

namespace Mobowski.Core
{
	/// <summary>
	/// The standard WebClient doesn't support cookies out-of-the-box. This subclass fixes the 
	/// cookie issue, whenever it's required.
	/// </summary>
	public class CookieAwareWebClient : WebClient
	{
		private CookieContainer _cc = new CookieContainer ();
		private string _lastPage = null;

		protected override WebRequest GetWebRequest (Uri address)
		{
			var r = base.GetWebRequest (address);
			if (r is HttpWebRequest) {
				var wr = (HttpWebRequest)r;
				wr.CookieContainer = _cc;
				if (_lastPage != null) {
					wr.Referer = _lastPage;
				}
			}
			_lastPage = address.ToString ();

			return r;
		}
	}
}

