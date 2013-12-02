using System;
using System.Net;
using System.Web;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Mobowski.Core.Sports
{
	public class MCNSportManager : SportManagerBase
	{
		private const string _teamUrl = "http://mijnclub.nu/clubs/teams/xml/";
		private const string _matchUrl = "http://mijnclub.nu/clubs/speelschema/xml/";
		private const string _resultClubUrl = "http://mijnclub.nu/clubs/uitslagen/xml/";
		private const string _resultsTeamUrl = "http://mijnclub.nu/clubs/teams/embed/";
		private const string _standingUrl = "http://mijnclub.nu/clubs/teams/embed/";

		#region implemented abstract members of SportManagerBase

		public override List<Team> RetrieveTeams (ClubBase club)
		{
			var teams = new List<Team> ();

			using (var client = new WebClient ()) {
				var mcnClub = (MCNClub)club;
				var url = _teamUrl + mcnClub.Identifier;
				var doc = client.LoadXml (url);

				var parser = new MCNTeamParser ();
				var nodes = doc.SelectNodes ("//team");
				foreach (var node in nodes) {
					var team = parser.Parse (node);
					teams.Add (team);
				}
			}

			return teams;
		}

		public override List<Match> RetrieveMatches (ClubBase club)
		{
			var matches = new List<Match> ();

			using (var client = new WebClient ()) {
				var mcnClub = (MCNClub)club;
				var url = _matchUrl + mcnClub.Identifier;
				var doc = client.LoadXml (url);

				var parser = new MCNMatchParser ();
				var nodes = doc.SelectNodes ("//wedstrijden/wedstrijd");
				foreach (var node in nodes) {
					var match = parser.Parse (node);
					matches.Add (match);
				}
			}

			return matches;
		}

		public override List<Match> RetrieveMatches (ClubBase club, Team team)
		{
			var matches = new List<Match> ();

			using (var client = new WebClient ()) {
				var mcnClub = (MCNClub)club;
				var url = _matchUrl + mcnClub.Identifier + "/periode,/team/" + HttpUtility.UrlEncode (team.Name);
				var doc = client.LoadXml (url);

				var parser = new MCNMatchParser ();
				var nodes = doc.SelectNodes ("//wedstrijden/wedstrijd");
				foreach (var node in nodes) {
					var match = parser.Parse (node);
					matches.Add (match);
				}
			}

			return matches;
		}

		public override List<Standing> RetrieveStandings (ClubBase club, Team team)
		{
			var standings = new List<Standing> ();

			using (var client = new WebClient ()) {
				var mcnClub = (MCNClub)club;
				var encTeam = HttpUtility.UrlEncode (team.Name);
				var url = String.Format ("{0}{1}/team/{2}?layout=stand&stand=1&format=xml", _standingUrl, mcnClub.Identifier, encTeam);
				var doc = client.LoadXml (url);

				var parser = new MCNStandingParser ();
				var nodes = doc.SelectNodes ("//table/tbody/tr");
				foreach (var node in nodes) {
					var standing = parser.Parse (node);
					standings.Add (standing);
				}
			}

			return standings;
		}

		public override List<Result> RetrieveResults (ClubBase club)
		{
			var results = new List<Result> ();

			using (var client = new WebClient ()) {
				var mcnClub = (MCNClub)club;
				var url = _resultClubUrl + mcnClub.Identifier;
				var doc = client.LoadXml (url);

				var parser = new MCNResultParser (MCNResultParser.ParseMode.Club);
				var nodes = doc.SelectNodes ("//wedstrijd");
				foreach (var node in nodes) {
					var result = parser.Parse (node);
					results.Add (result);
				}
			}

			return results;
		}

		public override List<Result> RetrieveResults (ClubBase club, Team team)
		{
			var results = new List<Result> ();

			using (var client = new WebClient ()) {
				var mcnClub = (MCNClub)club;
				var encTeam = HttpUtility.UrlEncode (team.Name);
				var url = String.Format ("{0}{1}/team/{2}?layout=uitslagen&format=xml", _resultsTeamUrl, mcnClub.Identifier, encTeam);
				var doc = client.LoadXml (url);

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

