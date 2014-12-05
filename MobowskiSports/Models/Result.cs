using System;
using System.Xml;

namespace Mobowski.Core.Sports
{
	public class Result
	{
		public DateTime? Date { get; internal set; }

		public string HomeTeam { get; internal set; }

		public string GuestTeam { get; internal set; }

		public int? HomeTeamScore { get; internal set; }

		public int? GuestTeamScore { get; internal set; }

		internal Result ()
		{
		}

		public override string ToString ()
		{
			return string.Format ("[Result: Date={0}, HomeTeam={1}, GuestTeam={2}, HomeTeamScore={3}, GuestTeamScore={4}]", Date, HomeTeam, GuestTeam, HomeTeamScore, GuestTeamScore);
		}
	}
}

