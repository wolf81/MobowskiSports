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
		public abstract Task<List<Team>> RetrieveTeamsAsync (ClubBase club);

		public abstract Task<List<Match>> RetrieveMatchesAsync (ClubBase club);

		public abstract Task<List<Match>> RetrieveMatchesAsync (ClubBase club, Team team);

		public abstract Task<List<Standing>> RetrieveStandingsAsync (ClubBase club, Team team);

		public abstract Task<List<Result>> RetrieveResultsAsync (ClubBase club);

		public abstract Task<List<Result>> RetrieveResultsAsync (ClubBase club, Team team);
	}

	/// <summary>
	/// Returns a SportManager based on the chosen provider.
	/// </summary>
	public abstract class SportManagerFactory
	{
		public static SportManagerBase Create (ClubBase club)
		{
			switch (club.Provider) {
			case SportDataProvider.RGPO:
				return new RGPOSportManager ();
			case SportDataProvider.MCN:
				return new MCNSportManager ();
			case SportDataProvider.OWK:
				return new OWKSportManager ();
			}

			return null;
		}
	}
}

