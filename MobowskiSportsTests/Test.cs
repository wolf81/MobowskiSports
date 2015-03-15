using System;
using NUnit.Framework;
using System.Collections.Generic;
using Mobowski.Core.Sports;

namespace MobowskiSportsTests {
	[TestFixture ()]
	public class Test {
		public Test () {
		}

		private MockCacheController CacheController {
			get {
				return new MockCacheController ();
			}
		}

		private MCNClub GetMCNClub () {
			var parameters = new Dictionary<string, object> ();
			parameters.Add ("Identifier", "BBBJ19X"); // BBHW92L
			return new MCNClub (parameters);
		}

		private Team GetMCNTeam () {
			var club = GetMCNClub ();
			var manager = SportManagerFactory.Create (club, this.CacheController);
			var teams = manager.RetrieveTeams ();
			return (teams != null && teams.Count > 0) ? teams [0] : null;
		}

		private RGPOClub GetRGPOClub () {
			return RGPOClub.CreateClub ("www.aswh.nl");
		}

		private Team GetRGPOTeam () {
			var club = GetRGPOClub ();
			var manager = SportManagerFactory.Create (club, this.CacheController);
			var teams = manager.RetrieveTeams ();
			return (teams != null && teams.Count > 0) ? teams [11] : null;
		}

		[Test ()]
		public void TestRGPOClub () {
			var club = RGPOClub.CreateClub ("www.candia66.nl");
			Assert.IsTrue (club != null && club.Identifier == 44);
		}

		[Test ()]
		public void TestRGPOResults () {
			var club = GetRGPOClub ();
			var manager = SportManagerFactory.Create (club, this.CacheController);
			var team = GetRGPOTeam ();
			var matches = manager.RetrieveResults (team);

			Console.WriteLine ("{0}", matches);

			Assert.IsTrue (matches != null);
		}

		[Test ()]
		public void TestRGPOMatches () {
			var club = GetRGPOClub ();
			var manager = SportManagerFactory.Create (club, this.CacheController);
			var matches = manager.RetrieveMatches ();
			Assert.IsTrue (matches != null && matches.Count > 0);
		}

		[Test ()]
		public void TestRGPOMatchesForTeam () {
			Team team = null;

			var club = GetRGPOClub ();
			var manager = SportManagerFactory.Create (club, this.CacheController);
			var teams = manager.RetrieveTeams ();
			if (teams != null && teams.Count > 0) {
				team = teams [22];
			}
			var matches = manager.RetrieveMatches (team);

			Assert.IsTrue (matches != null);
		}

		[Test ()]
		public void TestRGPOPouleResults () {
			var club = GetRGPOClub ();
			var manager = SportManagerFactory.Create (club, this.CacheController);
			var team = GetRGPOTeam ();
			var matches = manager.RetrieveResults (team);

			Console.WriteLine ("{0}", matches);

			Assert.IsTrue (matches != null);
		}

		[Test ()]
		public void TestRGPOStandings () {
			var club = GetRGPOClub ();
			var manager = SportManagerFactory.Create (club, this.CacheController);
			var team = GetRGPOTeam ();
			var standings = manager.RetrieveStandings (team);

			Console.WriteLine ("{0}", standings);

			Assert.IsTrue (standings != null);
		}

		[Test ()]
		public void TestMCNTeams () {
			var club = GetMCNClub ();
			var manager = SportManagerFactory.Create (club, this.CacheController);
			var teams = manager.RetrieveTeams ();
			Assert.IsTrue (teams != null && teams.Count > 0);
		}

		[Test ()]
		public void TestMCNMatchesForClub () {
			var club = GetMCNClub ();
			var manager = SportManagerFactory.Create (club, this.CacheController);
			var matches = manager.RetrieveMatches ();
			Assert.IsTrue (matches != null && matches.Count > 0);
		}

		[Test ()]
		public void TestMCNMatchesForTeam () {
			Team team = null;

			var club = GetMCNClub ();
			var manager = SportManagerFactory.Create (club, this.CacheController);
			var teams = manager.RetrieveTeams ();
			if (teams != null && teams.Count > 0) {
				team = teams [0];
			}
			var matches = manager.RetrieveMatches (team);
			Assert.IsTrue (team != null);
		}

		[Test ()]
		public void TestMCNStandings () {
			var club = GetMCNClub ();
			var manager = SportManagerFactory.Create (club, this.CacheController);
			var team = GetMCNTeam ();
			var standings = manager.RetrieveStandings (team);
			Assert.IsTrue (standings != null && standings.Count > 0);
		}

		[Test ()]
		public void TestMCNResults () {
			var club = GetMCNClub ();
			var manager = SportManagerFactory.Create (club, this.CacheController);
			var team = GetMCNTeam ();
			var results = manager.RetrieveResults();
			Assert.IsTrue (results != null && results.Count > 0);
		}

		[Test ()]
		public void TestMCNResultsForTeam () {
			var club = GetMCNClub ();
			var manager = SportManagerFactory.Create (club, this.CacheController);
			var team = GetMCNTeam ();
			var results = manager.RetrieveResults (team);
			Assert.IsTrue (results != null && results.Count > 0);
		}


		[Test ()] 
		public void TestOWKTeams () {
			var club = new OWKClub (null);
			var manager = SportManagerFactory.Create (club, this.CacheController);
			var teams = manager.RetrieveTeams ();
			Assert.IsTrue (teams != null && teams.Count > 0);
		}

		[Test ()]
		public void TestOWKMatches () {
			var club = new OWKClub (null);
			var manager = SportManagerFactory.Create (club, this.CacheController);
			var matches = manager.RetrieveMatches ();
			Assert.IsTrue (matches != null && matches.Count > 0);
		}

		[Test ()]
		public void TestOWKStandings () {
			var club = new OWKClub (null);
			var manager = SportManagerFactory.Create (club, this.CacheController);
			var team = GetMCNTeam ();
			var standings = manager.RetrieveStandings (team);
			Assert.IsTrue (standings != null && standings.Count > 0);
		}

		[Test ()]
		public void TestOWKResults () {
			var club = new OWKClub (null);
			var manager = SportManagerFactory.Create (club, this.CacheController);
			var results = manager.RetrieveResults ();
			Assert.IsTrue (results != null && results.Count > 0);
		}
	}
}

