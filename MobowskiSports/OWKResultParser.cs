using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Mobowski.Core.Sports
{
	public class OWKResultParser : IParser<Result>
	{
		/*
            "program_id":"1128251",
            "game_id":"11162",
            "year":"2013",
            "home_team_name":"Phoenix 1",
            "htc_id":"NCX20F3",
            "away_team_name":"Haarlem 1",
            "home_score":"32",
            "away_score":"16",
            "date":"2013-11-10",
            "time":"14:45",
            "match_officials":"Sangers, M (NoordWest)",
            "poule_name":"1F",
            "home_team_id":"31379",
            "away_team_id":"31685",
            "date_short":"10 nov"
		 */

		#region IParser implementation

		public Result Parse (object data)
		{
			var result = new Result ();

			try {
				var json = (JObject)data;
				result.HomeTeam = (string)json ["home_team_name"];
				result.GuestTeam = (string)json ["away_team_name"];
				result.HomeTeamScore = (int)json ["home_score"];
				result.GuestTeamScore = (int)json ["away_score"];
			} catch (Exception ex) {
				throw new Exception ("failed to parse OWK result", ex);
			}

			return result;
		}

		#endregion

	}
}

