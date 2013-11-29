using System;
using System.Xml;

namespace Mobowski.Core.Sports
{
	internal class RGPOStandingParser : IParser<Standing>
	{

		#region IParser implementation

		public Standing Parse (object data)
		{
			var standing = new Standing ();
			var node = (XmlNode)data;

			try {
				standing.Team = (string)node.Attributes ["team"].InnerText;
				standing.Ranking = Convert.ToInt32 (node.Attributes ["positie"].InnerText);
				standing.MatchesPlayed = Convert.ToInt32 (node.SelectSingleNode ("GS").InnerText);
				standing.MatchesWon = Convert.ToInt32 (node.SelectSingleNode ("WN").InnerText);
				standing.MatchesDraw = Convert.ToInt32 (node.SelectSingleNode ("GL").InnerText);
				standing.MatchesLost = Convert.ToInt32 (node.SelectSingleNode ("VL").InnerText);
				standing.Points = Convert.ToInt32 (node.SelectSingleNode ("PT").InnerText);
				standing.GoalsFor = Convert.ToInt32 (node.SelectSingleNode ("VR").InnerText);
				standing.GoalsAgainst = Convert.ToInt32 (node.SelectSingleNode ("TG").InnerText);
				standing.PointsDeduced = Convert.ToInt32 (node.SelectSingleNode ("PM").InnerText);	
			} catch (Exception ex) {
				throw ex;
			}

			return standing;
		}

		#endregion

	}
}
