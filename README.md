MobowskiSports
==============

**PLEASE NOTE:** The information below isn't completely up to date anymore, e.g. the Async calls have been removed, due to the webservice front-end having issues with this approach. I will update this guide in the nearby future.

A basic API for Mobowski to retrieve sport club information from several webservices. Currently support is build-in for both RGPO (wedstrijdprogramma.com) and MCN (mijnclub.nu).

How to use the API
------------------

The API is pretty easy to use:
1. Create a club object.
2. Use the club object to construct a manager from the SportManagerFactory
3. Use the manager to retrieve specific information like matches, teams, results & standings

Create a club object
--------------------

In order to use most webservices, we should be able to create a club in the following way:

    Dictionary<string, object> parameters;
    parameters = new Dictionary<string, object> ();
    parameters.Add ("Identifier", "BBHW92L");
    var mcnClub = new MCNClub (parameters);

In order to create a club for RGPO, we have to use a different approach though:

    var rgpoClub = await RGPOClub.RetrieveClub ("www.vvsteenwijk.nl");

Use the club object to construct a manager from the SportManagerFactory
-----------------------------------------------------------------------

    var manager = SportManagerFactory.Create (club);

That's all :P

Use the manager to retrieve specific information like matches, teams, results & standings
-----------------------------------------------------------------------------------------

We use the Task Parallel Library of Microsoft, so results might be achieved in several ways. Async / await is pretty easy to use and won't block the current thread:

    // any of the following won't block the main thread, but will still wait for the result before using it
    // in subsequent code.
    var teams = await manager.RetrieveTeamsAsync (club);
    var matches = await manager.RetrieveMatchesAsync (club, team);
    var standings = await manager.RetrieveStandingsAsync (club, team);
    var resultsClub = await manager.RetrieveResultsAsync (club);
    var resultsTeam = await manager.RetrieveResultsAsync (club, team);
    
Another blocking way to retrieve the results, might be like this:

    // will block the main thread
    var teams = manager.RetrieveTeamsAsync (club).Result; 

For other approaches (like performing requests in parallel), [check Microsoft's documentation on the Task Parallel Library][0].

[0]: http://msdn.microsoft.com/en-us/library/dd460717(v=vs.110).aspx


