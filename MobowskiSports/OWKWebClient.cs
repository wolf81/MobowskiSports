using System;
using System.Net;

namespace Mobowski.Core.Sports
{
	public class OWKWebClient : WebClient
	{
		public OWKWebClient () : base ()
		{
			this.Headers [HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
		}
	}
}

