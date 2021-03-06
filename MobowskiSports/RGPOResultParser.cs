using System;
using System.Xml;
using System.Globalization;

namespace Mobowski.Core.Sports
{
	internal class RGPOResultParser : IParser<Result>
	{

		#region IParser implementation

		public Result Parse (object data)
		{
			var result = new Result ();
			var node = (XmlNode)data;
			XmlNode testNode;

			try {
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

				testNode = node.SelectSingleNode("datum");
				if (testNode.InnerText != null && testNode.InnerText.Length > 0) {
					CultureInfo culture = new CultureInfo("nl-NL");
					result.Date = testNode.InnerText.ToDate("d-MMM", culture) ?? testNode.InnerText.ToDate("dd-MM-yyyy");
				}
			} catch (Exception ex) {
				throw new Exception ("failed to parse RGPO result", ex);		
			}

			return result;
		}

		#endregion

	}
}
