Public Class clsMatch
    Private strHomeTeamName As String = "HOME TEAM"
    Private strAwayTeamName As String = "AWAY TEAM"
    Private colHomeTextColour As Color = Color.White
    Private colAwayTextColour As Color = Color.White
    Private colHomeBackColour As Color = Color.Red
    Private colAwayBackColour As Color = Color.Green

    Public Property HomeTextColour() As Color
        Get
            Return colHomeTextColour
        End Get
        Set(ByVal value As Color)
            colHomeTextColour = value
        End Set
    End Property

    Public Property AwayTextColour() As Color
        Get
            Return colAwayTextColour
        End Get
        Set(ByVal value As Color)
            colAwayTextColour = value
        End Set
    End Property
    Public Property HomeBackColour() As Color
        Get
            Return colHomeBackColour
        End Get
        Set(ByVal value As Color)
            colHomeBackColour = value
        End Set
    End Property

    Public Property AwayBackColour() As Color
        Get
            Return colAwayBackColour
        End Get
        Set(ByVal value As Color)
            colAwayBackColour = value
        End Set
    End Property

    Public Property AwayTeamName() As String
        Get
            Return strAwayTeamName
        End Get
        Set(ByVal value As String)
            strAwayTeamName = value
        End Set
    End Property

    Public Property HomeTeamName() As String
        Get
            Return strHomeTeamName
        End Get
        Set(ByVal value As String)
            strHomeTeamName = value
        End Set
    End Property


End Class
