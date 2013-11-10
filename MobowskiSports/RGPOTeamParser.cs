using System;
using System.Xml;

namespace Mobowski.Core.Sports
{
	internal class RGPOTeamParser : IParser<Team>
	{

		#region IParser implementation

		public void Parse (Team team, object data)
		{
			var node = (XmlNode)data;

			team.Name = (string)node.SelectSingleNode ("naam").InnerText;
			team.Identifier = Convert.ToInt32 (node.SelectSingleNode ("id").InnerText);
		}

		#endregion

	}
}
