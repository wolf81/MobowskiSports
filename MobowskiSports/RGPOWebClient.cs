using System;
using System.IO;
using System.Xml;
using System.Net;

namespace Mobowski.Core.Sports {
	/// <summary>
	/// The standard WebClient doesn't support cookies out-of-the-box. This subclass fixes the 
	/// cookie issue, whenever it's required.
	/// </summary>
	internal class RGPOWebClient : CachingWebClient
	{
		// cookie handling
		private CookieContainer _cc = new CookieContainer ();
		private string _lastPage = null;
		private RGPOClub _club = null;
		//
		// rpgo specific members
		private const string _challengeUrl = "http://www.wedstrijdprogramma.com/api.php";
		private const string _teamUrl = "http://www.wedstrijdprogramma.com/api.php?action=teams";
		private const string _matchUrl = "http://www.wedstrijdprogramma.com/api.php?action=wedstrijden";
		private const string _standingsUrl = "http://www.wedstrijdprogramma.com/api.php?action=standen";
		private const string _standingsKvnbUrl = "http://www.wedstrijdprogramma.com/api.php?action=standen2";
		// KNVB data source required for poule matches!
		private const string _pouleMatchUrl = "http://www.wedstrijdprogramma.com/api.php?action=poule";
		private const string _clubUrl = "http://www.wedstrijdprogramma.com/api.php?action=vereniging";
			
		// http://www.wedstrijdprogramma.com/api.php?action=wedstrijden&vereniging_id=68&team_id=102781&response=1744502599

    public RGPOWebClient (SportManagerBase sportManager, RGPOClub club) : base(sportManager)
		{
			_club = club;
		}

		protected override WebRequest GetWebRequest (Uri address) {
			// we've overridden GetWebRequest() so we can handle cookies correctly. The default WebClient
			//	doesn't handle cookies properly, but we need cookies to make use of RGPO's challenge/response
			//	system.
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

		/// <summary>
		/// Gets the challenge response, this value is required to access other data from the RGPO 
		/// webservice. Every request will always first get a challenge response, which is to be 
		/// used for the next request.
		/// </summary>
		/// <returns>The challenge response.</returns>
		/// <param name="client">Client.</param>
		private  string GetChallengeResponse () {
			string response = null;

			var data = DownloadData (_challengeUrl);
			var stream = new MemoryStream (data);
			var doc = new XmlDocument ();
			doc.Load (stream);

			var node = doc.SelectSingleNode ("//challenge");
			response = (node != null) ? node.InnerText : null;

			return response;
		}

		/// <summary>
		/// Attempt to retrieve the XML data from the RGPO webservice. Will add the challenge response to 
		/// the url.
		/// </summary>
		/// <returns>The xml.</returns>
		/// <param name="url">URL.</param>
		private XmlDocument LoadXml (string url) {
			XmlDocument result = null;
            byte[] data = null;

            if ((SportManager != null) && (SportManager.CacheController != null)) {
                data = SportManager.CacheController.RetrieveByteDataFromCache(url);
                if (data != null) {
                    result = new XmlDocument();
                    var stream = new MemoryStream(data);
                    result.Load(stream);
                }
            }

            if (data == null) {
                var response = GetChallengeResponse();
                
                if (response != null) {
                    result = new XmlDocument();
                    
                    try {
                        var requesturi = url + String.Format("&response={0}", response);

                        //url += String.Format("&response={0}", response);
                        data = DownloadData(requesturi);

                        if ((SportManager != null) && (SportManager.CacheController != null)) {
                            SportManager.CacheController.StoreByteDataInCache(url, data);
                        }

                        var stream = new MemoryStream(data);
                        result.Load(stream);
                    } catch (Exception ex) {
                        throw new Exception("failed to load XML", ex);
                    }
                }
            }
 
			return result;
		}

		public static XmlDocument LoadClubsXml () {
			XmlDocument result = null;

            // due to the current architecture it's not possible to cache the Clubs.
			using (var client = new RGPOWebClient (null, null)) {
				result = client.LoadXml (_clubUrl);
			}

			return result;
		}

		public XmlDocument LoadPouleMatchesXml (Team team) {
			var url = String.Format ("{0}&vereniging_id={1}&team_id={2}", _pouleMatchUrl, _club.Identifier, team.Identifier);
			return LoadXml (url);
		}

		public XmlDocument LoadTeamsXml () {
			var url = String.Format ("{0}&vereniging_id={1}", _teamUrl, _club.Identifier);
			return LoadXml (url);
		}

		public XmlDocument LoadResultsXml (Team team) {
			var baseUrl = _club.HasKVNBSource ? _standingsKvnbUrl : _standingsUrl;
			var url = String.Format ("{0}&vereniging_id={1}&team_id={2}", baseUrl, _club.Identifier, team.Identifier);
			return LoadXml (url);
		}

		public XmlDocument LoadStandingsXml (Team team) {
			var baseUrl = _club.HasKVNBSource ? _standingsKvnbUrl : _standingsUrl;
			var url = String.Format ("{0}&vereniging_id={1}&team_id={2}", baseUrl, _club.Identifier, team.Identifier);
			return LoadXml (url);
		}

		public XmlDocument LoadMatchesXml () {
			var url = String.Format ("{0}&vereniging_id={1}", _matchUrl, _club.Identifier);
			return LoadXml (url);
		}

		public XmlDocument LoadMatchesXml(Team team) {
			var url = String.Format ("{0}&vereniging_id={1}&team_id={2}", _matchUrl, _club.Identifier, team.Identifier);
			return LoadXml (url);
		}
	}
}

