using System;
using System.Xml;

namespace Mobowski.Core.Sports
{
	internal class MCNTeamParser : IParser<Team>
	{

		#region IParser implementation

		public Team Parse (object data)
		{
			var team = new Team ();
			var node = (XmlNode)data;

			try {
				team.Name = (string)node.SelectSingleNode ("naam").InnerText;
				team.Identifier = Convert.ToInt32 (node.Attributes ["id"].InnerText);
			} catch (Exception ex) {
				throw new Exception ("failed to parse MCN team", ex);		
			}

			return team;
		}

		#endregion

	}
}
