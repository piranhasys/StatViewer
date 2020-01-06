Imports System.Collections.Generic

Public Class clsBettingBoard

    Public Property RaceCode As String = ""
    Public Property CourseTime As String = ""
    Public Property RaceName As String = ""
    Public Property Changes1Text As String = ""
    Public Property Changes2Text As String = "" 'NR
    Public Property Changes3Text As String = "" 'JC
    Public Property Changes4Text As String = ""
    Public Property CourseLogo As String = ""
    Public Property SponsorLogo As String = ""
    Public Property BettingRowList As New List(Of clsBettingBoardRow)

    Public Sub New()
        _RaceCode = ""
        _CourseTime = ""
        _RaceName = ""
        _Changes1Text = ""
        _Changes2Text = ""
        _Changes3Text = ""
        _Changes4Text = ""
        _CourseLogo = ""
        _SponsorLogo = ""
        _BettingRowList = New List(Of clsBettingBoardRow)
    End Sub
    Public ReadOnly Property ChangesCount As Integer
        Get
            Dim returnValue As Integer = 0
            If _Changes1Text.Trim <> "" Then returnValue += 1
            If _Changes2Text.Trim <> "" Then returnValue += 1
            If _Changes3Text.Trim <> "" Then returnValue += 1
            If _Changes4Text.Trim <> "" Then returnValue += 1
            Return returnValue
        End Get
    End Property
End Class
