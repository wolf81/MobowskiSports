using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Mobowski.Core.Sports
{
	public class OWKSportManager : SportManagerBase
	{
		private const string _teamsUrl = "http://www.mobowski.com/temp/teams.txt";
		private const string _matchesUrl = "http://www.mobowski.com/temp/matches.txt";
		private const string _standingsUrl = "http://www.mobowski.com/temp/standings.txt";
		private const string _resultsUrl = "http://www.mobowski.com/temp/results.txt";

		public OWKSportManager (ClubBase club) : base (club)
		{
		}

		#region implemented abstract members of SportManagerBase

		public override List<Team> RetrieveTeams ()
		{
			var teams = new List<Team> ();

			using (var client = new WebClient ()) {
				var jsonString = client.DownloadString (_teamsUrl);
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

			using (var client = new WebClient ()) {
				var jsonString = client.DownloadString (_matchesUrl);
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

			using (var client = new WebClient ()) {
				var jsonString = client.DownloadString (_matchesUrl);
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

			using (var client = new WebClient ()) {
				var jsonString = client.DownloadString (_standingsUrl);
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

			using (var client = new WebClient ()) {
				var jsonString = client.DownloadString (_resultsUrl);
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

			using (var client = new WebClient ()) {
				var jsonString = client.DownloadString (_resultsUrl);
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

