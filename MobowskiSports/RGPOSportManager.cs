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
		public RGPOSportManager (ClubBase club, ICacheController cacheController) : base (club, cacheController)
		{
		}

		#region implemented abstract members of SportManagerBase

		public override List<Team> RetrieveTeams () {
			var teams = new List<Team> ();
			var rgpoClub = (RGPOClub)Club;

			using (var client = new RGPOWebClient (this, rgpoClub)) {
				var doc = client.LoadTeamsXml ();

				var parser = new RGPOTeamParser ();
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
			var rgpoClub = (RGPOClub)Club;

			using (var client = new RGPOWebClient (this, rgpoClub)) {
				var doc = client.LoadMatchesXml ();

				var parser = new RGPOMatchParser ();
				var nodes = doc.SelectNodes ("//wedstrijd");
				foreach (var node in nodes) {
					var match = parser.Parse (node);
					matches.Add (match);
				}
			}

			return matches;
		}

		public override List<Match> RetrieveMatches (Team team) {
			var matches = new List<Match> ();

			matches = RetrieveMatches ();

			// remove all matches that are not played by the chosen team ...
			var predicate = new Predicate<Match> ((Match match) => {
				var isHostTeam = match.HostTeam.Equals (team.Name);
				var isGuestTeam = match.GuestTeam.Equals (team.Name);
				return (!isHostTeam && !isGuestTeam);
			});
			matches.RemoveAll (predicate);

			return matches;			
		}

		public override List<Standing> RetrieveStandings (Team team) {
			var standings = new List<Standing> ();
			var rgpoClub = (RGPOClub)Club;

			using (var client = new RGPOWebClient (this, rgpoClub)) {
				var doc = client.LoadStandingsXml (team);

				var parser = new RGPOStandingParser ();
				var nodes = doc.SelectNodes ("//ranglijst/ranglijstitem");
				foreach (var node in nodes) {
					var standing = parser.Parse (node);
					standings.Add (standing);
				}
			}

			return standings;
		}

		public override List<Result> RetrieveResults () {
			// RGPO has no call to retrieve results for a club, so we return an empty list.
			return new List<Result> (); 
		}

		public override List<Result> RetrieveResults (Team team) {
			var results = new List<Result> ();
			var rgpoClub = (RGPOClub)Club;

			if (rgpoClub.HasKVNBSource) {
				using (var client = new RGPOWebClient (this, rgpoClub)) {
					var doc = client.LoadPouleMatchesXml (team);

					var parser = new RGPOResultParser ();
					var nodes = doc.SelectNodes ("//poule[@type='R']/wedstrijd");
					foreach (var node in nodes) {
						var result = parser.Parse (node);
						results.Add (result);
					}
				}
			} else {
				using (var client = new RGPOWebClient (this, rgpoClub)) {
					var doc = client.LoadResultsXml (team);

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

