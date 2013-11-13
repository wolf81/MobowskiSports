Imports Mobowski.Core.Sports

Public Class Test
    Inherits System.Web.UI.Page

    Public strOutput As String = ""

    Protected Async Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Dim parameters = New Dictionary(Of String, Object)
    parameters.Add("Code", "bla")

    Dim club = New OWKClub(parameters)
    Dim manager = SportManagerFactory.Create(club)
    Dim result = Await manager.RetrieveTeamsAsync(club)

    Console.WriteLine(result)

    'Dim objClub = Await RGPOClub.RetrieveClub("www.aswh.nl")
    'Dim objManager = SportManagerFactory.Create(objClub)
    'Dim colTeams = Await objManager.RetrieveTeamsAsync(objClub)
    'Dim objTeam = colTeams(0)

    'strOutput = objTeam.Name
    'Console.WriteLine(objTeam.Name)




    End Sub

End Class