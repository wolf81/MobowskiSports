using System;
using System.Xml;

namespace Mobowski.Core.Sports
{
	internal class RGPOResultParser : IParser<Result>
	{

		#region IParser implementation

		public void Parse (Result result, object data)
		{
			var node = (XmlNode)data;
			XmlNode testNode;

			result.HomeTeam = (string)node.SelectSingleNode ("team_thuis").InnerText;
			result.GuestTeam = (string)node.SelectSingleNode ("team_uit").InnerText;

			testNode = node.SelectSingleNode ("score_thuis");
			if (testNode.InnerText != null && testNode.InnerText.Length > 0) {
				result.HomeTeamScore = Convert.ToInt32 (testNode.InnerText);
			}

			testNode = node.SelectSingleNode ("score_uit");
			if (testNode.InnerText != null && testNode.InnerText.Length > 0) {
				result.GuestTeamScore = Convert.ToInt32 (testNode.InnerText);
			}
		}

		#endregion

	}
}