using System;
using System.Xml;

namespace Mobowski.Core.Sports
{
	public class Match
	{
		public bool IsCancelled { get; internal set; }

		public DateTime? TimeDepart { get; internal set; }

		public DateTime? TimeStart { get; internal set; }

		public string Type { get; internal set; }

		public string Referee { get; internal set; }

		public string HostClub { get; internal set; }

		public string HostTeam { get; internal set; }

		public string GuestClub { get; internal set; }

		public string GuestTeam { get; internal set; }

		public string Field { get; internal set; }

		public string HostTeamLockerRoom { get; internal set; }

		public string GuestTeamLockerRoom { get; internal set; }

		public string OtherInfo { get; internal set; }

		internal Match (IParser<Match> parser, object data)
		{
			try {
				parser.Parse (this, data);
			} catch (Exception ex) {
				throw new Exception ("failed to parse Match", ex);
			}
		}

		public override string ToString ()
		{
			return string.Format ("[Match: IsCancelled={0}, TimeDepart={1}, TimeStart={2}, Type={3}, Referee={4}, HostClub={5}, HostTeam={6}, GuestClub={7}, GuestTeam={8}, Field={9}, HostTeamLockerRoom={10}, GuestTeamLockerRoom={11}, OtherInfo={12}]", IsCancelled, TimeDepart, TimeStart, Type, Referee, HostClub, HostTeam, GuestClub, GuestTeam, Field, HostTeamLockerRoom, GuestTeamLockerRoom, OtherInfo);
		}
	}
}

