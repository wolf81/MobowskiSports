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

		public override Task<List<Team>> RetrieveTeamsAsync (ClubBase club)
		{
			return Task.Run (() => {
				var teams = new List<Team> ();

				using (var client = new WebClient()) {
					var mcnClub = (MCNClub)club;
					var url = _teamUrl + mcnClub.Identifier;
					var doc = client.LoadXml (url);

					var parser = new MCNTeamParser ();
					var nodes = doc.SelectNodes ("//team");
					foreach (var node in nodes) {
						var team = new Team (parser, node);
						teams.Add (team);
					}
				}

				return teams;
			});
		}

		public override Task<List<Match>> RetrieveMatchesAsync (ClubBase club)
		{
			return Task.Run (() => {
				var matches = new List<Match> ();

				using (var client = new WebClient()) {
					var mcnClub = (MCNClub)club;
					var url = _matchUrl + mcnClub.Identifier;
					var doc = client.LoadXml (url);

					var parser = new MCNMatchParser ();
					var nodes = doc.SelectNodes ("//wedstrijden/wedstrijd");
					foreach (var node in nodes) {
						var match = new Match (parser, node);
						matches.Add (match);
					}
				}

				return matches;
			});
		}

		public override Task<List<Match>> RetrieveMatchesAsync (ClubBase club, Team team)
		{
			return Task.Run (() => {
				var matches = new List<Match> ();

				using (var client = new WebClient()) {
					var mcnClub = (MCNClub)club;
					var url = _matchUrl + mcnClub.Identifier + "/periode,/team/" + HttpUtility.UrlEncode (team.Name);
					var doc = client.LoadXml (url);

					var parser = new MCNMatchParser ();
					var nodes = doc.SelectNodes ("//wedstrijden/wedstrijd");
					foreach (var node in nodes) {
						var match = new Match (parser, node);
						matches.Add (match);
					}
				}

				return matches;
			});
		}

		public override Task<List<Standing>> RetrieveStandingsAsync (ClubBase club, Team team)
		{
			return Task.Run (() => {
				var standings = new List<Standing> ();

				using (var client = new WebClient()) {
					var mcnClub = (MCNClub)club;
					var encTeam = HttpUtility.UrlEncode (team.Name);
					var url = String.Format ("{0}{1}/team/{2}?layout=stand&stand=1&format=xml", _standingUrl, mcnClub.Identifier, encTeam);
					var doc = client.LoadXml (url);

					var parser = new MCNStandingParser ();
					var nodes = doc.SelectNodes ("//table/tbody/tr");
					foreach (var node in nodes) {
						var standing = new Standing (parser, node);
						standings.Add (standing);
					}
				}

				return standings;
			});
		}

		public override Task<List<Result>> RetrieveResultsAsync (ClubBase club)
		{
			return Task.Run (() => {
				var results = new List<Result> ();

				using (var client = new WebClient()) {
					var mcnClub = (MCNClub)club;
					var url = _resultClubUrl + mcnClub.Identifier;
					var doc = client.LoadXml (url);

					var parser = new MCNResultParser (MCNResultParser.ParseMode.Club);
					var nodes = doc.SelectNodes ("//wedstrijd");
					foreach (var node in nodes) {
						var result = new Result (parser, node);
						results.Add (result);
					}
				}

				return results;
			});
		}

		public override Task<List<Result>> RetrieveResultsAsync (ClubBase club, Team team)
		{
			return Task.Run (() => {
				var results = new List<Result> ();

				using (var client = new WebClient()) {
					var mcnClub = (MCNClub)club;
					var encTeam = HttpUtility.UrlEncode (team.Name);
					var url = String.Format ("{0}{1}/team/{2}?layout=uitslagen&format=xml", _resultsTeamUrl, mcnClub.Identifier, encTeam);
					var doc = client.LoadXml (url);

					var parser = new MCNResultParser (MCNResultParser.ParseMode.Team);
					var nodes = doc.SelectNodes ("//table/tbody/tr");
					foreach (var node in nodes) {
						var result = new Result (parser, node);
						results.Add (result);
					}
				}

				return results;
			});
		}

		#endregion

	}
}

