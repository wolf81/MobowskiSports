using System;
using System.IO;
using System.Net;
using System.Web;
using System.Xml;

namespace Mobowski.Core.Sports
{
	public class MCNWebClient : WebClient
	{
		private MCNClub _club;
		private const string _teamUrl = "http://mijnclub.nu/clubs/teams/xml/";
		private const string _matchUrl = "http://mijnclub.nu/clubs/speelschema/xml/";
		private const string _resultClubUrl = "http://mijnclub.nu/clubs/uitslagen/xml/";
		private const string _resultsTeamUrl = "http://mijnclub.nu/clubs/teams/embed/";
		private const string _standingUrl = "http://mijnclub.nu/clubs/teams/embed/";

		public MCNWebClient (MCNClub club) : base ()
		{
			_club = club;
		}

		private XmlDocument LoadXml (string url)
		{
			var result = new XmlDocument ();

			try {
				var data = DownloadData (url);
				var stream = new MemoryStream (data);
				result.Load (stream);
			} catch (Exception ex) {
				throw new Exception ("failed to load XML", ex);
			}

			return result;
		}

		public XmlDocument LoadTeamsXml ()
		{
			return LoadXml (_teamUrl + _club.Identifier);
		}

		public XmlDocument LoadResultsXml ()
		{
			return LoadXml (_resultClubUrl + _club.Identifier);
		}

		public XmlDocument LoadMatchesXml ()
		{
			return LoadXml (_matchUrl + _club.Identifier);
		}

		public XmlDocument LoadResultsXml (Team team)
		{
			var encTeam = HttpUtility.UrlEncode (team.Name);
			var url = String.Format ("{0}{1}/team/{2}?layout=uitslagen&format=xml", _resultsTeamUrl, _club.Identifier, encTeam);
			return LoadXml (url);
		}

		public XmlDocument LoadStandingsXml (Team team)
		{
			var encTeam = HttpUtility.UrlEncode (team.Name);
			var url = String.Format ("{0}{1}/team/{2}?layout=stand&stand=1&format=xml", _standingUrl, _club.Identifier, encTeam);
			return LoadXml (url);
		}

		public XmlDocument LoadMatchesXml (Team team)
		{
			var url = _matchUrl + _club.Identifier + "/periode,/team/" + HttpUtility.UrlEncode (team.Name);
			return LoadXml (url);
		}
	}
}

