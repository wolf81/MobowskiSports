using System;
using System.Net;

namespace Mobowski.Core.Sports
{
	public class OWKWebClient : WebClient
	{
		private const string _baseUrl = "http://www.knkv.nl/kcp/";
		private const string _postData = "file=json&f=get_data&full=0";

		public OWKWebClient (OWKClub club) : base ()
		{
			this.Headers [HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
			this.BaseAddress = String.Format ("{0}{1}", _baseUrl, club.Code);
		}

		public string UploadString (string postData)
		{
			postData = String.Format ("{0}&{1}", _postData, postData);
			return UploadString (BaseAddress, postData);
		}
	}
}

