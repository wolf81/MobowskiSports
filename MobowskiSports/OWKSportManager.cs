using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Web;

namespace Mobowski.Core.Sports
{
	public class OWKSportManager : SportManagerBase
	{
		private const string _baseUrl = "http://www.knkv.nl/kcp/";
		private const string _postData = "file=json&f=get_data&full=0";

		public OWKSportManager (ClubBase club) : base (club)
		{
		}

		#region implemented abstract members of SportManagerBase

		public override List<Team> RetrieveTeams ()
		{
			var teams = new List<Team> ();

			using (var client = new OWKWebClient ()) {
				var postData = String.Format ("{0}&t=teams", _postData);
				var jsonString = client.UploadString (_baseUrl, postData);
				var json = (JObject)JToken.Parse (jsonString);
				var parser = new OWKTeamParser ();

				var keys = json.Properties ().Select (p => p.Name).ToList ();
				foreach (var key in keys) {
					var teamsJson = (JArray)json [key] ["v"];

					foreach (var teamJson in teamsJson) {
						var team = parser.Parse (teamJson);
						teams.Add (team);
					}
				}
			}

			return teams;
		}

		public override List<Match> RetrieveMatches ()
		{
			var matches = new List<Match> ();

			using (var client = new OWKWebClient ()) {
				var postData = String.Format ("{0}&t=program", _postData);
				var jsonString = client.UploadString (_baseUrl, postData);                
				var json = (JObject)JToken.Parse (jsonString);
				var parser = new OWKMatchParser ();

				var keys = json.Properties ().Select (p => p.Name).ToList ();
				foreach (var key in keys) {
					var matchesJson = json [key] ["items"];

					foreach (var matchJson in matchesJson) {
						var match = parser.Parse (matchJson);
						matches.Add (match);
					}
				}
			}

			return matches;
		}

		public override List<Match> RetrieveMatches (Team team)
		{
			// TODO: remember to filter on team here ...

			var matches = new List<Match> ();

			using (var client = new OWKWebClient ()) {
				var postData = String.Format ("{0}&t=program&t_id={1}", _postData, team.Identifier);
				var jsonString = client.UploadString (_baseUrl, postData); 
				var json = (JObject)JToken.Parse (jsonString);
				var parser = new OWKMatchParser ();

				var keys = json.Properties ().Select (p => p.Name).ToList ();
				foreach (var key in keys) {
					var matchesJson = json [key] ["items"];

					foreach (var matchJson in matchesJson) {
						var match = parser.Parse (matchJson);
						matches.Add (match);
					}
				}
			}

			return matches;
		}

		public override List<Standing> RetrieveStandings (Team team)
		{
			// PLEASE NOTE: due to a limitation in the app we just parse the first standing here. Until we implement
			//	poules, we just have to assume the first standing is the correct one.

			var standings = new List<Standing> ();

			using (var client = new OWKWebClient ()) {
				var postData = String.Format ("{0}&t=standing&t_id={1}", _postData, team.Identifier);
				var jsonString = client.UploadString (_baseUrl, postData); 
				var json = (JArray)JToken.Parse (jsonString);
				var parser = new OWKStandingParser ();

				var standingsJson = json != null && json.Count > 0 ? json [0] : null;

				foreach (var standingJson in standingsJson["lines"]) {
					var standing = parser.Parse (standingJson);
					standings.Add (standing);
				}
			}

			return standings;
		}

		public override List<Result> RetrieveResults ()
		{
			var results = new List<Result> ();

			using (var client = new OWKWebClient ()) {
				var postData = String.Format ("{0}&t=result", _postData);
				var jsonString = client.UploadString (_baseUrl, postData); 
				var json = (JObject)JToken.Parse (jsonString);
				var parser = new OWKResultParser ();

				var keys = json.Properties ().Select (p => p.Name).ToList ();
				foreach (var key in keys) {
					var matchesJson = json [key] ["items"];

					foreach (var matchJson in matchesJson) {
						var match = parser.Parse (matchJson);
						results.Add (match);
					}
				}
			}

			return results;
		}

		public override List<Result> RetrieveResults (Team team)
		{
			var results = new List<Result> ();

			using (var client = new OWKWebClient ()) {
				var postData = String.Format ("{0}&t=result&t_id={1}", _postData, team.Identifier);
				var jsonString = client.UploadString (_baseUrl, postData); 
				var json = (JObject)JToken.Parse (jsonString);
				var parser = new OWKResultParser ();

				var keys = json.Properties ().Select (p => p.Name).ToList ();
				foreach (var key in keys) {
					var matchesJson = json [key] ["items"];

					foreach (var matchJson in matchesJson) {
						var match = parser.Parse (matchJson);
						results.Add (match);
					}
				}
			}

			return results;
		}

		#endregion

	}
}

