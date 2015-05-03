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
    Private strActionAreaA1 As String = ""
    Private strActionAreaB1 As String = ""
    Private strActionAreaC1 As String = ""
    Private strActionAreaA2 As String = ""
    Private strActionAreaB2 As String = ""
    Private strActionAreaC2 As String = ""

    Private strTimesLevel As String = ""

    Private strTimeInPlay1 As String = ""
    Private strMatchTime1 As String = ""

    Private strTimeInPlay2 As String = ""
    Private strMatchTime2 As String = ""

    Private strTimeInPlayTotal As String = ""
    Private strMatchTimeTotal As String = ""
    Public Property TimesLevel() As String
        Get
            Return strTimesLevel
        End Get
        Set(ByVal value As String)
            strTimesLevel = value
        End Set
    End Property


    Public Property MatchTime1() As String
        Get
            Return strMatchTime1
        End Get
        Set(ByVal value As String)
            strMatchTime1 = value
        End Set
    End Property


    Public Property TimeInPlay1() As String
        Get
            Return strTimeInPlay1
        End Get
        Set(ByVal value As String)
            strTimeInPlay1 = value
        End Set
    End Property

    Public Property MatchTime2() As String
        Get
            Return strMatchTime2
        End Get
        Set(ByVal value As String)
            strMatchTime2 = value
        End Set
    End Property


    Public Property TimeInPlay2() As String
        Get
            Return strTimeInPlay2
        End Get
        Set(ByVal value As String)
            strTimeInPlay2 = value
        End Set
    End Property

    Public Property MatchTimeTotal() As String
        Get
            Return strMatchTimeTotal
        End Get
        Set(ByVal value As String)
            strMatchTimeTotal = value
        End Set
    End Property


    Public Property TimeInPlayTotal() As String
        Get
            Return strTimeInPlayTotal
        End Get
        Set(ByVal value As String)
            strTimeInPlayTotal = value
        End Set
    End Property

    Public Property ActionAreaC1() As String
        Get
            Return strActionAreaC1
        End Get
        Set(ByVal value As String)
            strActionAreaC1 = value
        End Set
    End Property


    Public Property ActionAreaB1() As String
        Get
            Return strActionAreaB1
        End Get
        Set(ByVal value As String)
            strActionAreaB1 = value
        End Set
    End Property


    Public Property ActionAreaA1() As String
        Get
            Return strActionAreaA1
        End Get
        Set(ByVal value As String)
            strActionAreaA1 = value
        End Set
    End Property
    Public Property ActionAreaC2() As String
        Get
            Return strActionAreaC2
        End Get
        Set(ByVal value As String)
            strActionAreaC2 = value
        End Set
    End Property


    Public Property ActionAreaB2() As String
        Get
            Return strActionAreaB2
        End Get
        Set(ByVal value As String)
            strActionAreaB2 = value
        End Set
    End Property


    Public Property ActionAreaA2() As String
        Get
            Return strActionAreaA2
        End Get
        Set(ByVal value As String)
            strActionAreaA2 = value
        End Set
    End Property




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

    Public ReadOnly Property TimeInPlayText1() As String
        Get
            If strMatchTime1 <> "" Then
                Return strTimeInPlay1 + " / " + strMatchTime1
            Else
                Return ""
            End If
        End Get
    End Property
    Public ReadOnly Property TimeInPlayText2() As String
        Get
            If strMatchTime2 <> "" Then
                Return strTimeInPlay2 + " / " + strMatchTime2
            Else
                Return ""
            End If
        End Get
    End Property
    Public ReadOnly Property TimeInPlayTextTotal() As String
        Get
            If strMatchTimeTotal <> "" Then
                Return strTimeInPlayTotal + " / " + strMatchTimeTotal
            Else
                Return ""
            End If
        End Get
    End Property


End Class
