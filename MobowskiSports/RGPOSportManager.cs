using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Mobowski.Core.Sports
{
	public class RGPOSportManager : SportManagerBase
	{
		private const string _challengeUrl = "http://www.wedstrijdprogramma.com/api.php";
		private const string _teamUrl = "http://www.wedstrijdprogramma.com/api.php?action=teams";
		private const string _matchUrl = "http://www.wedstrijdprogramma.com/api.php?action=wedstrijden";
		private const string _rankingUrl = "http://www.wedstrijdprogramma.com/api.php?action=standen";
		private const string _rankingsKnvbUrl = "http://www.wedstrijdprogramma.com/api.php?action=standen2";
		// TODO: implement a way to retrieve club info, since mobowski currently doesn't send all required data
		private const string _clubUrl = "http://www.wedstrijdprogramma.com/api.php?action=vereniging";

		/// <summary>
		/// Gets the challenge response, this value is required to access other data from the RGPO 
		/// webservice. Every request will always first get a challenge response, which is to be 
		/// used for the next request.
		/// </summary>
		/// <returns>The challenge response.</returns>
		/// <param name="client">Client.</param>
		private static string GetChallengeResponse (CookieAwareWebClient client)
		{
			string response = null;

			var data = client.DownloadData (_challengeUrl);
			var stream = new MemoryStream (data);
			var doc = new XmlDocument ();
			doc.Load (stream);

			var node = doc.SelectSingleNode ("//challenge");
			response = (node != null) ? node.InnerText : null;

			return response;
		}

		internal static Task<RGPOClub> RetrieveClub (string referer)
		{
			return Task.Run (() => {
				RGPOClub club = null;

				using (var client = new CookieAwareWebClient ()) {
					var response = GetChallengeResponse (client);

					if (response != null) {
						var url = String.Format ("{0}&response={1}", _clubUrl, response);
						var doc = client.LoadXml (url);

						var xpath = String.Format ("//vereniging[referer='{0}']", referer);
						var node = doc.SelectSingleNode (xpath);
						if (node != null) {
							var id = Convert.ToInt32 (node.SelectSingleNode ("id").InnerText);
							var isKnvbSource = Convert.ToInt32 (node.SelectSingleNode ("KNVBdataservice").InnerText);

							var parameters = new Dictionary<string, object> ();
							parameters.Add ("Identifier", id);
							parameters.Add ("Referer", referer);
							parameters.Add ("IsKNVBSource", (isKnvbSource == 1));

							club = new RGPOClub (parameters);
						}
					}
				}

				return club;
			});
		}

		#region implemented abstract members of SportManagerBase

		public override List<Team> RetrieveTeams (ClubBase club)
		{
			var teams = new List<Team> ();

			using (var client = new CookieAwareWebClient ()) {
				var response = GetChallengeResponse (client);

				if (response != null) {
					var rgpoClub = (RGPOClub)club;
					var url = String.Format ("{0}&vereniging_id={1}&response={2}", _teamUrl, rgpoClub.Identifier, response);
					var doc = client.LoadXml (url);

					var parser = new RGPOTeamParser ();
					var nodes = doc.SelectNodes ("//team");
					foreach (var node in nodes) {
						var team = parser.Parse (node);
						teams.Add (team);
					}
				}
			}

			return teams;
		}

		public override List<Match> RetrieveMatches (ClubBase club)
		{
			var matches = new List<Match> ();

			using (var client = new CookieAwareWebClient ()) {
				var response = GetChallengeResponse (client);

				if (response != null) {
					var rgpoClub = (RGPOClub)club;
					var url = String.Format ("{0}&vereniging_id={1}&response={2}", _matchUrl, rgpoClub.Identifier, response);
					var doc = client.LoadXml (url);

					var parser = new RGPOMatchParser ();
					var nodes = doc.SelectNodes ("//wedstrijd");
					foreach (var node in nodes) {
						var match = parser.Parse (node);
						matches.Add (match);
					}
				}
			}

			return matches;
		}

		public override List<Match> RetrieveMatches (ClubBase club, Team team)
		{
			var matches = RetrieveMatches (club);

			// remove all matches that are not played by the chosen team ...
			var predicate = new Predicate<Match> ((Match match) => {
				var isHostTeam = match.HostTeam.Equals (team.Name);
				var isGuestTeam = match.GuestTeam.Equals (team.Name);
				return (!isHostTeam && !isGuestTeam);
			});
			matches.RemoveAll (predicate);

			return matches;
		}

		public override List<Standing> RetrieveStandings (ClubBase club, Team team)
		{
			var standings = new List<Standing> ();

			using (var client = new CookieAwareWebClient ()) {
				var response = GetChallengeResponse (client);

				if (response != null) {
					var rgpoClub = (RGPOClub)club;
					var baseUrl = rgpoClub.HasKVNBSource ? _rankingsKnvbUrl : _rankingUrl;
					var url = String.Format ("{0}&vereniging_id={1}&team_id={2}&response={3}", baseUrl, rgpoClub.Identifier, team.Identifier, response);
					var doc = client.LoadXml (url);

					var parser = new RGPOStandingParser ();
					var nodes = doc.SelectNodes ("//ranglijst/ranglijstitem");
					foreach (var node in nodes) {
						var standing = parser.Parse (node);
						standings.Add (standing);
					}
				}
			}

			return standings;
		}

		public override List<Result> RetrieveResults (ClubBase club)
		{
			// RGPO has no call to retrieve results for a club, so we return an empty list.
			return new List<Result> (); 
		}

		public override List<Result> RetrieveResults (ClubBase club, Team team)
		{
			var results = new List<Result> ();

			using (var client = new CookieAwareWebClient ()) {
				var response = GetChallengeResponse (client);

				if (response != null) {
					var rgpoClub = (RGPOClub)club;
					var baseUrl = rgpoClub.HasKVNBSource ? _rankingsKnvbUrl : _rankingUrl;
					var url = String.Format ("{0}&vereniging_id={1}&team_id={2}&response={3}", baseUrl, rgpoClub.Identifier, team.Identifier, response);
					var doc = client.LoadXml (url);

					var parser = new RGPOResultParser ();
					var nodes = doc.SelectNodes ("//wedstrijden/wedstrijd");
					foreach (var node in nodes) {
						// only parse results from the past

						var result = parser.Parse (node);
						if (result.GuestTeamScore != null && result.HomeTeamScore != null) {
							results.Add (result);
						}
					}
				}
			}

			return results;
		}

		#endregion

	}
}

