using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Web;
//using ErrorLogging;

//using ErrorLogging;
namespace Mobowski.Core.Sports
{
	public class OWKSportManager : SportManagerBase
	{
        public OWKSportManager(ClubBase club, ICacheController cacheController) : base(club, cacheController)
		{
		}

		#region implemented abstract members of SportManagerBase


        private string Left(string str, int count) {
            if (str.Length > count) {
                return str.Substring(0, count);
            } else {
                return str;
            }
        }

        private string RetrieveAndOrStore(OWKWebClient client, string identifier) {
            var returnValue = "";

            var jsonString = client.CacheRetrieve(identifier);

            if (jsonString == null) {
                jsonString = client.UploadString(identifier);
                //ErrorLogging.ErrorLog.WriteError("Got from UploadString");

                if (jsonString != null) {
                    client.CacheInsert(identifier, jsonString);
                }
            } else {
                //ErrorLogging.ErrorLog.WriteError("Got from Cache: " + Left(jsonString, 30));
            }

            if (jsonString != null) {
                returnValue = jsonString;
            } else {
                //ErrorLogging.ErrorLog.WriteError("JSONSTRING == NULL");
            }

            if (returnValue.IndexOf("alert") > 0) {
                //ErrorLogging.ErrorLog.WriteError(jsonString);
            }


            return returnValue;
        }


		public override List<Team> RetrieveTeams ()
		{
			var teams = new List<Team> ();
			var owkClub = (OWKClub)Club;

			using (var client = new OWKWebClient (this, owkClub)) {
                var jsonString = RetrieveAndOrStore(client, "t=teams");

//				ErrorLog.WriteError ("got json: " + jsonString);

        		var json = (JObject)JToken.Parse (jsonString);
				var parser = new OWKTeamParser ();

                var keys = json.Properties().Select(p => p.Name).ToList();
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
			var owkClub = (OWKClub)Club;

			using (var client = new OWKWebClient (this, owkClub)) {
                var jsonString = RetrieveAndOrStore(client, "t=program");
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
			var owkClub = (OWKClub)Club;

			using (var client = new OWKWebClient (this, owkClub)) {
                var jsonString = RetrieveAndOrStore(client, "t=program&t_id=" + team.Identifier);
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
			var owkClub = (OWKClub)Club;

			using (var client = new OWKWebClient (this, owkClub)) {
                var jsonString = RetrieveAndOrStore(client, "t=standing&t_id=" + team.Identifier);
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
			var owkClub = (OWKClub)Club;

			using (var client = new OWKWebClient (this, owkClub)) {
                var jsonString = RetrieveAndOrStore(client, "t=result");
//				ErrorLog.WriteError ("got json: " + jsonString);

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
			var owkClub = (OWKClub)Club;

			using (var client = new OWKWebClient (this, owkClub)) {
                var jsonString = RetrieveAndOrStore(client, "t=result&t_id=" + team.Identifier);
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

