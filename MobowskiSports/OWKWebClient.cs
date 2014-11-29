using System;
using System.Net;
using ErrorLogging;

namespace Mobowski.Core.Sports
{
    internal class OWKWebClient : CachingWebClient
	{
		private const string _baseUrl = "http://www.knkv.nl/kcp/";
		private const string _postData = "file=json&f=get_data&full=1";

        public OWKWebClient(SportManagerBase sportManager, OWKClub club) : base(sportManager)
		{
			this.Headers [HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
			this.BaseAddress = String.Format ("{0}{1}", _baseUrl, club.Code + "/json/");
		}

        public string CacheRetrieve(string postData) {
            if ((SportManager == null) || (SportManager.CacheController == null)) { return null; }

            var url = BaseAddress + string.Format("{0}&{1}", _postData, postData);
            //ErrorLog.WriteError("CacheRetrieve: " + url);
            var returnValue = SportManager.CacheController.RetrieveDataFromCache(url);

            if (returnValue == "") {
                returnValue = null;
            }

            return returnValue;
        }

        public bool CacheInsert(string postData, string content) {
            if ((SportManager == null) || (SportManager.CacheController == null)) { return false; }
            var url = BaseAddress + string.Format("{0}&{1}", _postData, postData);
            return SportManager.CacheController.StoreDataInCache(url, content);
        }


		public string UploadString (string postData) {
	        postData = String.Format ("{0}&{1}", _postData, postData);
			return UploadString (BaseAddress, postData);
		}
	}
}

