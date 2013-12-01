using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Mobowski.Core.Sports
{
	class OWKStandingParser : IParser<Standing>
	{
		/*
            "position":"1",
            "team_name":"BEP",
            "played":"7",
            "points":"13",
            "won":"6",
            "lost":"0",
            "draw":"1",
            "sport":"KORFBALL-VE-WK",
            "serie":"",
            "goals_for":"98",
            "goals_against":"84",
            "difference":"14",
            "penalties":"0"
		 */

		#region IParser implementation

		public Standing Parse (object data)
		{
			Standing standing = new Standing ();

			try {
				JToken json = (JToken)data;
				standing.Team = (string)json ["team_name"];
				standing.Ranking = (int)json ["position"];
				standing.MatchesPlayed = (int)json ["played"];
				standing.Points = (int)json ["points"];
				standing.MatchesWon = (int)json ["won"];
				standing.MatchesLost = (int)json ["lost"];
				standing.MatchesDraw = (int)json ["draw"];
				standing.GoalsFor = (int)json ["goals_for"];
				standing.GoalsAgainst = (int)json ["goals_against"];
//				standing.PointsDeduced = (int)json ["penalties"];

			} catch (Exception ex) {
				throw new Exception ("failed to parse OWK standing", ex);
			}

			return standing;
		}

		#endregion

	}
}
