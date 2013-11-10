using System;
using System.Xml;

namespace Mobowski.Core.Sports
{
	public class Standing
	{
		public string Team { get; internal set; }

		public int Ranking { get; internal set; }

		public int MatchesPlayed { get; internal set; }

		public int MatchesWon { get; internal set; }

		public int MatchesDraw { get; internal set; }

		public int MatchesLost { get; internal set; }

		public int Points { get; internal set; }

		public int GoalsFor { get; internal set; }

		public int GoalsAgainst { get; internal set; }

		public int PointsDeduced { get; internal set; }

		internal Standing (IParser<Standing> parser, object data)
		{
			try {
				parser.Parse (this, data);
			} catch (Exception ex) {
				throw new Exception ("failed to parse Standing", ex);
			}
		}

		public override string ToString ()
		{
			return string.Format ("[Standing: Team={0}, Ranking={1}, MatchesPlayed={2}, MatchesWon={3}, MatchesDraw={4}, MatchesLost={5}, Points={6}, GoalsFor={7}, GoalsAgainst={8}, PointsDeduced={9}]", Team, Ranking, MatchesPlayed, MatchesWon, MatchesDraw, MatchesLost, Points, GoalsFor, GoalsAgainst, PointsDeduced);
		}
	}
}

