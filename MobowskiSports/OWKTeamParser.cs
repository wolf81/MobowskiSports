using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Mobowski.Core.Sports
{
	class OWKTeamParser : IParser<Team>
	{

		#region IParser implementation

		public Team Parse (object data)
		{
			var team = new Team ();

			try {
				var json = (JObject)data;

				team.Identifier = (int)json ["team_id"];
				team.Name = (string)json ["team_name"];
			} catch (Exception ex) {
				throw new Exception ("failed to parse OWK team", ex);
			}

			return team;
		}

		#endregion

	}
}
