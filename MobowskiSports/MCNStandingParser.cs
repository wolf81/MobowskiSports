using System;
using System.Xml;

namespace Mobowski.Core.Sports
{
	internal class MCNStandingParser : IParser<Standing>
	{

		#region IParser implementation

		public void Parse (Standing standing, object data)
		{
			var node = (XmlNode)data;

			// we need to start from the current node '.', otherwise search begins from the root 
			//	document, always returning the first found value (first node in nodeList)
			standing.Team = node.NodeValue ("./td[@class='team']");
			standing.Ranking = Convert.ToInt32 (node.SelectSingleNode ("./td[@class='nr']").InnerText);
			standing.MatchesPlayed = Convert.ToInt32 (node.SelectSingleNode ("./td[@class='played']").InnerText);
			standing.MatchesWon = Convert.ToInt32 (node.SelectSingleNode ("./td[@class='wins']").InnerText);
			standing.MatchesDraw = Convert.ToInt32 (node.SelectSingleNode ("./td[@class='draws']").InnerText);
			standing.MatchesLost = Convert.ToInt32 (node.SelectSingleNode ("./td[@class='losses']").InnerText);
			standing.Points = Convert.ToInt32 (node.SelectSingleNode ("./td[@class='points']").InnerText);
			standing.GoalsFor = Convert.ToInt32 (node.SelectSingleNode ("./td[@class='for']").InnerText);
			standing.GoalsAgainst = Convert.ToInt32 (node.SelectSingleNode ("./td[@class='against']").InnerText);
			standing.PointsDeduced = Convert.ToInt32 (node.SelectSingleNode ("./td[@class='penaltypoints']").InnerText);
		}

		#endregion

	}
}
