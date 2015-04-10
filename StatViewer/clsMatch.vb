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
    Private strHomeTimeSinceScore As String = "0:00"
    Private strAwayTimeSinceScore As String = "0:00"
    Private strHomePossession1 As String = ""
    Private strHomePossession2 As String = ""
    Private strHomePossessionTotal As String = ""
    Private strAwayPossession1 As String = ""
    Private strAwayPossession2 As String = ""
    Private strAwayPossessionTotal As String = ""
    Private iPeriod As Integer = 0
    Public Property Period() As Integer
        Get
            Return iPeriod
        End Get
        Set(ByVal value As Integer)
            iPeriod = value
        End Set
    End Property


    Public Property AwayPossessionTotal() As String
        Get
            Return strAwayPossessionTotal
        End Get
        Set(ByVal value As String)
            strAwayPossessionTotal = value
        End Set
    End Property


    Public Property AwayPossession2() As String
        Get
            Return strAwayPossession2
        End Get
        Set(ByVal value As String)
            strAwayPossession2 = value
        End Set
    End Property


    Public Property AwayPossession1() As String
        Get
            Return strAwayPossession1
        End Get
        Set(ByVal value As String)
            strAwayPossession1 = value
        End Set
    End Property


    Public Property HomePossessionTotal() As String
        Get
            Return strHomePossessionTotal
        End Get
        Set(ByVal value As String)
            strHomePossessionTotal = value
        End Set
    End Property


    Public Property HomePossession2() As String
        Get
            Return strHomePossession2
        End Get
        Set(ByVal value As String)
            strHomePossession2 = value
        End Set
    End Property


    Public Property HomePossession1() As String
        Get
            Return strHomePossession1
        End Get
        Set(ByVal value As String)
            strHomePossession1 = value
        End Set
    End Property



    Public Property AwayTimeSinceScore() As String
        Get
            Return strAwayTimeSinceScore
        End Get
        Set(ByVal value As String)
            strAwayTimeSinceScore = value
        End Set
    End Property


    Public Property HomeTimeSinceScore() As String
        Get
            Return strHomeTimeSinceScore
        End Get
        Set(ByVal value As String)
            strHomeTimeSinceScore = value
        End Set
    End Property


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
