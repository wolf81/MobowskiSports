using NUnit.Framework;
using System;
using System.Collections.Generic;
using Mobowski.Core.Sports;

namespace Mobowski.Core.Sports
{
	[TestFixture ()]
	public class Test
	{
		private MCNClub GetMCNClub ()
		{

			var parameters = new Dictionary<string, object> ();
			parameters.Add ("Identifier", "BBHW92L");
			return new MCNClub (parameters);
		}

		[Test ()]
		public void TestRGPOClub ()
		{
			var club = RGPOClub.RetrieveClub ("www.ajax.nl").Result;
			Assert.True (club != null && club.Identifier == 62);
		}

		[Test ()]
		public void TestMCNTeams ()
		{
			var club = GetMCNClub ();
			var manager = SportManagerFactory.Create (club);
			var teams = manager.RetrieveTeamsAsync (club).Result;
			Assert.True (teams != null && teams.Count > 0);
		}

		[Test ()]
		public void TestMCNMatchesForClub ()
		{
			var club = GetMCNClub ();
			var manager = SportManagerFactory.Create (club);
			var matches = manager.RetrieveMatchesAsync (club).Result;
			Assert.True (matches != null && matches.Count > 0);
		}

		[Test ()]
		public void TestMCNMatchesForTeam ()
		{
			Team team = null;

			var club = GetMCNClub ();
			var manager = SportManagerFactory.Create (club);
			var teams = manager.RetrieveTeamsAsync (club).Result;
			if (teams != null && teams.Count > 0) {
				team = teams [0];
			}
			Assert.True (team != null);
		}

		[Test ()]
		public void TestMCNStandings ()
		{
			var club = GetMCNClub ();
			var manager = SportManagerFactory.Create (club);
			var standings = manager.RetrieveStandingsAsync (club, null);
		}
	}
}

