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
		private const string _teamsUrl = "http://localhost:8000/teams.json";
		private const string _matchesUrl = "http://localhost:8000/matches.json";
		private const string _standingsUrl = "http://localhost:8000/standings.json";
		private const string _resultsUrl = "http://localhost:8000/results.json";

		#region implemented abstract members of SportManagerBase

		public override Task<List<Team>> RetrieveTeamsAsync (ClubBase club)
		{
			return Task.Run (() => {
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
			});
		}

		public override Task<List<Match>> RetrieveMatchesAsync (ClubBase club)
		{
			return Task.Run (() => {
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
			});
		}

		public override Task<List<Match>> RetrieveMatchesAsync (ClubBase club, Team team)
		{
			// TODO: remember to filter on team here ...

			return Task.Run (() => {
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
			});
		}

		public override Task<List<Standing>> RetrieveStandingsAsync (ClubBase club, Team team)
		{
			return Task.Run (() => {
				var standings = new List<Standing> ();

				using (var client = new WebClient ()) {
					var jsonString = client.DownloadString (_standingsUrl);
					var json = (JArray)JToken.Parse (jsonString);
					var parser = new OWKStandingParser ();

					// filer op team en haal eerste poule binnen: t_id = team.Identifier

					foreach (var item in json) {
						var lines = item.SelectToken ("lines");

						// haal eerste hieruit            
						foreach (var line in lines) {
							var standing = parser.Parse (line);
							standings.Add (standing);
							break;
						}
					}
				}

				return standings;
			});
		}

		public override Task<List<Result>> RetrieveResultsAsync (ClubBase club)
		{
			return Task.Run (() => {
				var teams = new List<Result> ();

				using (var client = new WebClient ()) {
					var jsonString = client.DownloadString (_resultsUrl);
					var json = JToken.Parse (jsonString);
					var parser = new OWKResultParser ();
				}

				return teams;
			});
		}

		public override Task<List<Result>> RetrieveResultsAsync (ClubBase club, Team team)
		{
			return Task.Run (() => {
				var teams = new List<Result> ();

				using (var client = new WebClient ()) {
					var jsonString = client.DownloadString (_resultsUrl);
					var json = JToken.Parse (jsonString);
					var parser = new OWKResultParser ();
				}

				return teams;
			});
		}

		#endregion

	}
}

