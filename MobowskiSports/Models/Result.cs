using System;
using System.Xml;

namespace Mobowski.Core.Sports
{
	public class Result
	{
		public string HomeTeam { get; internal set; }

		public string GuestTeam { get; internal set; }

		public int? HomeTeamScore { get; internal set; }

		public int? GuestTeamScore { get; internal set; }

		internal Result (IParser<Result> parser, object data)
		{
			try {
				parser.Parse (this, data);
			} catch (Exception ex) {
				throw new Exception ("failed to parse Result", ex);
			}
		}

		public override string ToString ()
		{
			return string.Format ("[Result: HomeTeam={0}, GuestTeam={1}, HomeTeamScore={2}, GuestTeamScore={3}]", HomeTeam, GuestTeam, HomeTeamScore, GuestTeamScore);
		}
	}
}

