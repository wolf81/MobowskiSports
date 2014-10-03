using System;
using System.Net;
using System.Web;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Mobowski.Core.Sports {
	public class MCNSportManager : SportManagerBase {
		public MCNSportManager (ClubBase club) : base (club) {
		}

		#region implemented abstract members of SportManagerBase

		public override List<Team> RetrieveTeams () {
			var teams = new List<Team> ();

			using (var client = new MCNWebClient ((MCNClub)Club)) {
				var doc = client.LoadTeamsXml ();

				var parser = new MCNTeamParser ();
				var nodes = doc.SelectNodes ("//team");
				foreach (var node in nodes) {
					var team = parser.Parse (node);
					teams.Add (team);
				}
			}

			return teams;
		}

		public override List<Match> RetrieveMatches () {
			var matches = new List<Match> ();

			using (var client = new MCNWebClient ((MCNClub)Club)) {
				var doc = client.LoadMatchesXml ();

				var parser = new MCNMatchParser ();
				var nodes = doc.SelectNodes ("//wedstrijden/wedstrijd");
				foreach (var node in nodes) {
					var match = parser.Parse (node);
					matches.Add (match);
				}
			}

			return matches;
		}

		public override List<Match> RetrieveMatches (Team team) {
			var matches = new List<Match> ();

			using (var client = new MCNWebClient ((MCNClub)Club)) {
				var doc = client.LoadMatchesXml (team);

				var parser = new MCNMatchParser ();
				var nodes = doc.SelectNodes ("//wedstrijden/wedstrijd");
				foreach (var node in nodes) {
					var match = parser.Parse (node);
					matches.Add (match);
				}
			}

			return matches;
		}

		public override List<Standing> RetrieveStandings (Team team) {
			var standings = new List<Standing> ();

			using (var client = new MCNWebClient ((MCNClub)Club)) {
				var doc = client.LoadStandingsXml (team);

				var parser = new MCNStandingParser ();
				var nodes = doc.SelectNodes ("//table/tbody/tr");
				foreach (var node in nodes) {
					var standing = parser.Parse (node);
					standings.Add (standing);
				}
			}

			return standings;
		}

		public override List<Result> RetrieveResults () {
			var results = new List<Result> ();

			using (var client = new MCNWebClient ((MCNClub)Club)) {
				var doc = client.LoadResultsXml ();

				var parser = new MCNResultParser (MCNResultParser.ParseMode.Club);
				var nodes = doc.SelectNodes ("//wedstrijd");
				foreach (var node in nodes) {
					var result = parser.Parse (node);
					results.Add (result);
				}
			}

			return results;
		}

		public override List<Result> RetrieveResults (Team team) {
			var results = new List<Result> ();

			using (var client = new MCNWebClient ((MCNClub)Club)) {
				var doc = client.LoadPouleResultsXml (team);

				var parser = new MCNResultParser (MCNResultParser.ParseMode.Team);
				var nodes = doc.SelectNodes ("//table/tbody/tr");
				foreach (var node in nodes) {
					var result = parser.Parse (node);
					results.Add (result);
				}
			}

			return results;
		}

		#endregion

	}
}

