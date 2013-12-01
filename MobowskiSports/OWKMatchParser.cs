using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Mobowski.Core.Sports
{
	public class OWKMatchParser : IParser<Match>
	{
		/*
            "program_id":"1135411",
            "game_id":"18322",
            "year":"2013",
            "home_team_name":"Haarlem A1",
            "htc_id":"NCX23F2",
            "away_team_name":"Aurora A1",
            "atc_id":"NCX14B2",
            "class_name":"A-jeugd 2e klasse",
            "date":"2013-11-16",
            "time":"17:50",
            "field":"Kenn. West",
            "facility_name":"Kennemer Sportcentrum Haarlem",
            "facility_id":"NCX67N0",
            "match_officials":"J.T. Baas (Scheidsrechter)",
            "poule_name":"A2I",
            "home_team_id":"32703",
            "away_team_id":"29261",
            "date_short":"16 nov"
		*/

		#region IParser implementation

		public Match Parse (object data)
		{
			var json = (JObject)data;

			var match = new Match ();
			match.Referee = (string)json ["match_officials"];
			match.Field = (string)json ["field"];
			match.HostTeam = (string)json ["home_team_name"];
			match.GuestTeam = (string)json ["away_team_name"];
			match.OtherInfo = (string)json ["facility_name"];

			var dateString = String.Format ("{0} {1}", json ["date"], json ["time"]);
			match.TimeStart = dateString.ToDate ("yyyy-MM-dd HH:mm");			          

			return match;
		}

		#endregion

	}
}

