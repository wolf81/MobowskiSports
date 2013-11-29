using System;
using System.Xml;

namespace Mobowski.Core.Sports
{
	internal class MCNMatchParser : IParser<Match>
	{

		#region IParser implementation

		public Match Parse (object data)
		{
			var match = new Match ();
			var node = (XmlNode)data;
			XmlNode testNode = null;
			string testString = null;

			try {
				testString = node.AttributeValue ("afgelast");
				match.IsCancelled = (testString != null && testString.Equals ("ja")); 
				match.Referee = node.NodeValue ("scheidsrechter");
				match.Field = node.NodeValue ("veld");
				match.Type = node.NodeValue ("soort");

				testString = node.NodeValue ("opmerkingen");
				match.OtherInfo = (testString != null) ? testString : null; // TODO: trim?

				testNode = node.SelectSingleNode ("thuisteam");
				match.HostClub = null;
				match.HostTeam = testNode.InnerText != null ? testNode.InnerText : null;
				match.HostTeamLockerRoom = testNode.AttributeValue ("kleedkamer");

				testNode = node.SelectSingleNode ("uitteam");
				match.GuestClub = null;
				match.GuestTeam = testNode.InnerText != null ? testNode.InnerText : null;
				match.GuestTeamLockerRoom = testNode.AttributeValue ("kleedkamer");

				var dateString = node.NodeValue ("datum");
				if (dateString != null) {
					testNode = node.SelectSingleNode ("aanvang");

					// parse start time
					if (testNode != null && testNode.InnerText != null) { 
						var timeString = dateString + testNode.InnerText;
						match.TimeStart = timeString.ToDate ("yyyy-MM-ddHH:mm");
					} else {
						match.TimeStart = dateString.ToDate ("yyyy-MM-dd");
					}

					// parse departure time
					testNode = testNode.Attributes ["aanwezig"];
					if (testNode != null && testNode.InnerText != null) {
						var timeString = dateString + testNode.InnerText;
						match.TimeDepart = timeString.ToDate ("yyyy-MM-ddHH:mm");
					}
				}
			} catch (Exception ex) {
				throw ex;				
			}

			return match;
		}

		#endregion

	}
}
