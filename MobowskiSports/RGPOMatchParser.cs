using System;
using System.Xml;

namespace Mobowski.Core.Sports
{
	internal class RGPOMatchParser : IParser<Match>
	{

		#region IParser implementation

		public Match Parse (object data)
		{
			var match = new Match ();
			var node = (XmlNode)data;
			XmlNode testNode = null;

			try {
				match.IsCancelled = false; // RGPO provides no data wheter the match is cancelled.
				match.Referee = node.SelectSingleNode ("scheidsrechter").InnerText;
				match.Field = node.SelectSingleNode ("veld").InnerText;
				match.Type = node.SelectSingleNode ("wedstrijd_type").InnerText;
				match.OtherInfo = node.SelectSingleNode ("info").InnerText;

				match.HostClub = node.SelectSingleNode ("vereniging_thuis").InnerText;
				match.HostTeam = node.SelectSingleNode ("team_thuis").InnerText;
				match.HostTeamLockerRoom = node.SelectSingleNode ("team_thuis_kleedkamer").InnerText;

				match.GuestClub = node.SelectSingleNode ("vereniging_uit").InnerText;
				match.GuestTeam = node.SelectSingleNode ("team_uit").InnerText;
				match.GuestTeamLockerRoom = node.SelectSingleNode ("team_uit_kleedkamer").InnerText;

				match.TimeDepart = null; // RGPO provides no data regarding the departure time.

				// parse start date
				testNode = node.SelectSingleNode ("datum_US");
				if (testNode.InnerText != null) {
					var dateString = testNode.InnerText;

					// parse start time
					testNode = node.SelectSingleNode ("aanvang");
					if (testNode.InnerText != null) {
						dateString += testNode.InnerText;
						match.TimeStart = dateString.ToDate ("yyyy-MM-ddHH:mm:ss");
					} else {
						match.TimeStart = dateString.ToDate ("yyyy-MM-dd");
					}
				}				
			} catch (Exception ex) {
				throw new Exception ("failed to parse RGPO match", ex);		
			}

			return match;
		}

		#endregion

	}
}
