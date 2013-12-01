using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Mobowski.Core.Sports
{
	class OWKTeamParser : IParser<Team>
	{

		#region IParser implementation

		public Team Parse (object data)
		{
			var json = (JObject)data;

			var team = new Team ();
			team.Identifier = (int)json ["team_id"];
			team.Name = (string)json ["team_name"];
			return team;
		}

		#endregion

	}
}
