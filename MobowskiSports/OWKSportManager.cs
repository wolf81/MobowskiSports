using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Mobowski.Core.Sports
{
	public class OWKSportManager : SportManagerBase
	{

		#region implemented abstract members of SportManagerBase

		public override Task<List<Team>> RetrieveTeamsAsync (ClubBase club)
		{
			throw new NotImplementedException ();
		}

		public override Task<List<Match>> RetrieveMatchesAsync (ClubBase club)
		{
			throw new NotImplementedException ();
		}

		public override Task<List<Match>> RetrieveMatchesAsync (ClubBase club, Team team)
		{
			throw new NotImplementedException ();
		}

		public override Task<List<Standing>> RetrieveStandingsAsync (ClubBase club, Team team)
		{
			throw new NotImplementedException ();
		}

		public override Task<List<Result>> RetrieveResultsAsync (ClubBase club)
		{
			throw new NotImplementedException ();
		}

		public override Task<List<Result>> RetrieveResultsAsync (ClubBase club, Team team)
		{
			throw new NotImplementedException ();
		}

		#endregion

	}
}

