using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mobowski.Core.Sports
{
	/// <summary>
	/// The source which we use to retrieve sport data.
	/// </summary>
	internal enum SportDataProvider
	{
		RGPO = 0,
		// wedstrijdprogramma.com
		MCN,
		// mijnclub.nu
		OWK,
		// onsweb.nl
	}

	/// <summary>
	/// Sport manager.
	/// </summary>
	public abstract class SportManagerBase
	{
		public ClubBase Club { get; private set; }

        public ICacheController CacheController { get; private set; }

        public SportManagerBase(ClubBase club, ICacheController cacheController) {
            Club = club;
            CacheController = cacheController;
        }

		public Team RetrieveTeam (int identifier)
		{
			Team selectedTeam = null;
			var teams = RetrieveTeams ();
			foreach (var team in teams) {
				if (team.Identifier == identifier) {
					selectedTeam = team;
					break;
				}
			}
			return selectedTeam;
		}

		public abstract List<Team> RetrieveTeams ();

		public abstract List<Match> RetrieveMatches ();

		public abstract List<Match> RetrieveMatches (Team team);

		public abstract List<Standing> RetrieveStandings (Team team);

		public abstract List<Result> RetrieveResults ();

		public abstract List<Result> RetrieveResults (Team team);
	}

	/// <summary>
	/// Returns a SportManager based on the chosen provider.
	/// </summary>
	public abstract class SportManagerFactory
	{
        public static SportManagerBase Create(ClubBase club, ICacheController cacheController)
		{
			switch (club.Provider) {
			case SportDataProvider.RGPO:
				return new RGPOSportManager (club, cacheController);
			case SportDataProvider.MCN:
				return new MCNSportManager (club, cacheController);
			case SportDataProvider.OWK:
				return new OWKSportManager (club, cacheController);
			}

			return null;
		}
	}
}

