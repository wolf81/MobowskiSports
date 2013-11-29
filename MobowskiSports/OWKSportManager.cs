using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Mobowski.Core.Sports
{
	public class OWKSportManager : SportManagerBase
	{
		private const string _baseUrl = "http://mobowski.zapto.org/temp/onsweb.txt";

		#region implemented abstract members of SportManagerBase

		public override Task<List<Team>> RetrieveTeamsAsync (ClubBase club)
		{
			return Task.Run (() => {
				var teams = new List<Team> ();

				using (var client = new WebClient()) {
					var owClub = (OWKClub)club;
					var url = _baseUrl;




				}

				return teams;
			});
		}

		public override Task<List<Match>> RetrieveMatchesAsync (ClubBase club)
		{
			throw new NotImplementedException ();
		}

		public override Task<List<Match>> RetrieveMatchesAsync (ClubBase club, Team team)
		{
			throw new NotImplementedException ();
		}

		public override Task<List<Standing>> RetrieveStandingsAsync (ClubBase club, Team team)
		{
			return Task.Run (() => {
				var standings = new List<Standing> ();

				using (var client = new WebClient()) {
					client.Headers.Add (HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");

					var url = _baseUrl;
					var jsonString = client.DownloadString (url);
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
			throw new NotImplementedException ();
		}

		public override Task<List<Result>> RetrieveResultsAsync (ClubBase club, Team team)
		{
			throw new NotImplementedException ();
		}

		#endregion

	}
}

