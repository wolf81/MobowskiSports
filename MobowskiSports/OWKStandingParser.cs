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

		#region IParser implementation

		public Standing Parse (object data)
		{
			Standing standing = new Standing ();

			try {
				JToken json = (JToken)data;
				standing.Team = (string)json.SelectToken ("team_name");
			} catch (Exception ex) {
				throw ex;
			}

			return standing;
		}

		#endregion

	}
}
