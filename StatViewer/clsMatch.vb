Public Class clsMatch
    Private strHomeTeamName As String = "HOME TEAM"
    Private strAwayTeamName As String = "AWAY TEAM"
    Private colHomeTextColour As Color = Color.White
    Private colAwayTextColour As Color = Color.White
    Private colHomeBackColour As Color = Color.Red
    Private colAwayBackColour As Color = Color.Green
    Private cStat(20) As clsSVStat
    Private strHomeScoreline As String = "0-00"
    Private strAwayScoreline As String = "0-00"
    Private strMatchClock As String = "00:00"
    Public Property MatchClock() As String
        Get
            Return strMatchClock
        End Get
        Set(ByVal value As String)
            strMatchClock = value
        End Set
    End Property


    Public Property AwayScoreline() As String
        Get
            Return strAwayScoreline
        End Get
        Set(ByVal value As String)
            strAwayScoreline = value
        End Set
    End Property

    Public Property HomeScoreline() As String
        Get
            Return strHomeScoreline
        End Get
        Set(ByVal value As String)
            strHomeScoreline = value
        End Set
    End Property


    Public Property Stat() As clsSVStat()
        Get
            Return cStat
        End Get
        Set(ByVal value As clsSVStat())
            cStat = value
        End Set
    End Property
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
