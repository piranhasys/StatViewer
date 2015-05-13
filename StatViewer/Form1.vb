Imports System.Net.Sockets
Imports System.Text
Imports System.Data.SqlClient
Imports System.Net
Imports System.IO
Imports System.Net.DnsPermissionAttribute
Imports System.Security.Permissions
Imports System.Runtime.InteropServices
Imports System.Collections.Generic

Public Class Form1
    Public bConnectedToServer As Boolean = False
    Dim dtLastPAData As Date = Now
    Dim dtLastPAHeartbeat As Date = Now
    Dim dtLastLocalHeartbeat As Date = Now
    Dim dtLastRemoteHeartbeat As Date = Now
    Private client As TcpClient
    Const READ_BUFFER_SIZE As Integer = 8192 * 2
    Private readBuffer(READ_BUFFER_SIZE) As Byte
    Private Const OLECMDID_OPTICAL_ZOOM As Integer = 63
    Private Const OLECMDEXECOPT_DONTPROMPTUSER As Integer = 2
    Private fontSize As Integer = 0
    Private displayStyle As Integer = 0 '0=web, 1=RTE GAA
    Private left As Integer = 0
    Private top As Integer = 0
    Private lastPageName As String = ""
    Private thisMatch As New clsMatch

    Private Sub Form1_Activated(sender As Object, e As EventArgs) Handles Me.Activated
    End Sub

    Private Sub Form1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'initial HP commit
        For incStat As Integer = 0 To RBTeamStats.GetUpperBound(0)
            RBTeamStats(incStat) = New clsRBTeamStat
        Next
        dtLastPAData = Now.AddDays(-1)
        timerCheckConnections.Start()
        fontSize = My.Settings.FontSize
        left = My.Settings.Left
        top = My.Settings.Top
        displayStyle = My.Settings.DisplayType
        Select Case displayStyle
            Case 0
            Case Else
                For incStat As Integer = 0 To thisMatch.Stat.GetUpperBound(0)
                    thisMatch.Stat(incStat) = New clsSVStat
                Next
        End Select
        ' WebBrowser1.ScriptErrorsSuppressed = True
        SetupDisplay()

    End Sub
    Sub SetupDisplay()
        Select Case displayStyle
            Case 1
                PanelRTE.BringToFront()
                LoadRTEStaticData()
                ShowRTEStaticData()
                ShowRTETeamData()
            Case Else
                PanelRTE.SendToBack()
                RepositionBrowser()
        End Select
        'RacingToolStripMenuItem.Checked = (displayStyle = 0)
        'RTEGAAToolStripMenuItem.Checked = (displayStyle = 1)

    End Sub
    Sub LoadRTEStaticData()
        Dim inc As Integer
        Dim strTextLine As String = ""
        Dim strFilename As String = My.Settings.DataFilename
        Dim inputFile As System.IO.StreamReader
        Dim TempArray() As String
        Try
           
            If Not System.IO.File.Exists(strFilename) Then
                Exit Sub
            Else
                inputFile = System.IO.File.OpenText(strFilename)
                thisMatch.HomeTeamName = inputFile.ReadLine.ToUpper
                thisMatch.AwayTeamName = inputFile.ReadLine.ToUpper
                thisMatch.HomeScoreline = inputFile.ReadLine
                thisMatch.AwayScoreline = inputFile.ReadLine
                thisMatch.HomeBackColour = Color.FromArgb(Val(inputFile.ReadLine))
                thisMatch.HomeTextColour = Color.FromArgb(Val(inputFile.ReadLine))
                thisMatch.AwayBackColour = Color.FromArgb(Val(inputFile.ReadLine))
                thisMatch.AwayTextColour = Color.FromArgb(Val(inputFile.ReadLine))

                For incStat As Integer = 1 To 20
                    thisMatch.Stat(incStat).AssignData(inputFile.ReadLine)
                Next


                inputFile.Close()
            End If
        Catch ex As Exception
            ' MessageBox.Show(ex.ToString, "Error reading stat names")
        End Try

    End Sub
    Delegate Sub ShowRTEStaticDataCallback()
    Sub ShowRTEStaticData()
        If lablHomeTeam.InvokeRequired Then
            Dim d As New ShowRTEStaticDataCallback(AddressOf ShowRTEStaticData)
            Me.Invoke(d, New Object() {})
        Else
            lablHomeTeam.Text = thisMatch.HomeTeamName
            lablHomeTeam.BackColor = thisMatch.HomeBackColour
            lablHomeTeam.ForeColor = thisMatch.HomeTextColour
            lablAwayTeam.Text = thisMatch.AwayTeamName
            lablAwayTeam.BackColor = thisMatch.AwayBackColour
            lablAwayTeam.ForeColor = thisMatch.AwayTextColour

            lablHomeTeam2.Text = thisMatch.HomeTeamName
            lablHomeTeam2.BackColor = thisMatch.HomeBackColour
            lablHomeTeam2.ForeColor = thisMatch.HomeTextColour
            lablAwayTeam2.Text = thisMatch.AwayTeamName
            lablAwayTeam2.BackColor = thisMatch.AwayBackColour
            lablAwayTeam2.ForeColor = thisMatch.AwayTextColour
        End If
    End Sub
    Delegate Sub ShowRTETeamDataCallback()
    Sub ShowRTETeamData()
        If lablStatAway01.InvokeRequired Then
            Dim d As New ShowRTETeamDataCallback(AddressOf ShowRTETeamData)
            Me.Invoke(d, New Object() {})
        Else
            lablHomeScoreline.Text = thisMatch.HomeScoreline
            lablAwayScoreline.Text = thisMatch.AwayScoreline

            lablStatName01.Text = thisMatch.Stat(1).Name
            lablStatName02.Text = thisMatch.Stat(2).Name
            lablStatName03.Text = thisMatch.Stat(3).Name
            lablStatName04.Text = thisMatch.Stat(4).Name
            lablStatName05.Text = thisMatch.Stat(5).Name
            lablStatName06.Text = thisMatch.Stat(6).Name
            lablStatName07.Text = thisMatch.Stat(7).Name
            lablStatName08.Text = thisMatch.Stat(8).Name
            lablStatName09.Text = thisMatch.Stat(9).Name
            lablStatName10.Text = thisMatch.Stat(10).Name
            lablStatName11.Text = thisMatch.Stat(11).Name
            lablStatName12.Text = thisMatch.Stat(12).Name
            lablStatName13.Text = thisMatch.Stat(13).Name
            lablStatName14.Text = thisMatch.Stat(14).Name
            lablStatName15.Text = thisMatch.Stat(15).Name
            lablStatName16.Text = thisMatch.Stat(16).Name
            lablStatName17.Text = thisMatch.Stat(17).Name
            lablStatName18.Text = thisMatch.Stat(18).Name
            lablStatName19.Text = thisMatch.Stat(19).Name
            lablStatName20.Text = thisMatch.Stat(20).Name

            lablStatHome01.Text = thisMatch.Stat(1).HomeNum
            lablStatHome02.Text = thisMatch.Stat(2).HomeNum
            lablStatHome03.Text = thisMatch.Stat(3).HomeNum
            lablStatHome04.Text = thisMatch.Stat(4).HomeNum
            lablStatHome05.Text = thisMatch.Stat(5).HomeNum
            lablStatHome06.Text = thisMatch.Stat(6).HomeNum
            lablStatHome07.Text = thisMatch.Stat(7).HomeNum
            lablStatHome08.Text = thisMatch.Stat(8).HomeNum
            lablStatHome09.Text = thisMatch.Stat(9).HomeNum
            lablStatHome10.Text = thisMatch.Stat(10).HomeNum
            lablStatHome11.Text = thisMatch.Stat(11).HomeNum
            lablStatHome12.Text = thisMatch.Stat(12).HomeNum
            lablStatHome13.Text = thisMatch.Stat(13).HomeNum
            lablStatHome14.Text = thisMatch.Stat(14).HomeNum
            lablStatHome15.Text = thisMatch.Stat(15).HomeNum
            lablStatHome16.Text = thisMatch.Stat(16).HomeNum
            lablStatHome17.Text = thisMatch.Stat(17).HomeNum
            lablStatHome18.Text = thisMatch.Stat(18).HomeNum
            lablStatHome19.Text = thisMatch.Stat(19).HomeNum
            lablStatHome20.Text = thisMatch.Stat(20).HomeNum

            lablStatAway01.Text = thisMatch.Stat(1).AwayNum
            lablStatAway02.Text = thisMatch.Stat(2).AwayNum
            lablStatAway03.Text = thisMatch.Stat(3).AwayNum
            lablStatAway04.Text = thisMatch.Stat(4).AwayNum
            lablStatAway05.Text = thisMatch.Stat(5).AwayNum
            lablStatAway06.Text = thisMatch.Stat(6).AwayNum
            lablStatAway07.Text = thisMatch.Stat(7).AwayNum
            lablStatAway08.Text = thisMatch.Stat(8).AwayNum
            lablStatAway09.Text = thisMatch.Stat(9).AwayNum
            lablStatAway10.Text = thisMatch.Stat(10).AwayNum
            lablStatAway11.Text = thisMatch.Stat(11).AwayNum
            lablStatAway12.Text = thisMatch.Stat(12).AwayNum
            lablStatAway13.Text = thisMatch.Stat(13).AwayNum
            lablStatAway14.Text = thisMatch.Stat(14).AwayNum
            lablStatAway15.Text = thisMatch.Stat(15).AwayNum
            lablStatAway16.Text = thisMatch.Stat(16).AwayNum
            lablStatAway17.Text = thisMatch.Stat(17).AwayNum
            lablStatAway18.Text = thisMatch.Stat(18).AwayNum
            lablStatAway19.Text = thisMatch.Stat(19).AwayNum
            lablStatAway20.Text = thisMatch.Stat(20).AwayNum


        End If

    End Sub
    Delegate Sub ShowRTEScoreDataCallback()
    Sub ShowRTEScoreData()
        If lablHomeScoreline.InvokeRequired Then
            Dim d As New ShowRTEScoreDataCallback(AddressOf ShowRTEScoreData)
            Me.Invoke(d, New Object() {})
        Else
            lablHomeScoreline.Text = thisMatch.HomeScoreline
            lablAwayScoreline.Text = thisMatch.AwayScoreline
        End If

    End Sub
    Private Sub DoRead(ByVal ar As IAsyncResult)
        Dim BytesRead As Integer
        Dim strMessage As String
        'Dim dataArray() As String
        Dim inc As Integer
        Static strTemp As String
        Dim strData() As String

        Try
            ' Finish asynchronous read into readBuffer and return number of bytes read.
            BytesRead = client.GetStream.EndRead(ar)
            If BytesRead < 1 Then
                ' If no bytes were read server has closed.  Disable input window.
                Exit Sub
            End If

            strMessage = Encoding.Default.GetString(readBuffer, 0, BytesRead)
            If Microsoft.VisualBasic.Right(strMessage, 2) = vbCrLf Then
                strTemp += Microsoft.VisualBasic.Left(strMessage, Len(strMessage) - 2)    'remove CRLF
                If InStr(strTemp, vbCrLf) > 0 Then
                    strData = strTemp.Split(vbCrLf)
                    For inc = 0 To UBound(strData)
                        ProcessCommands(strData(inc).Replace(vbLf, "").Replace(vbCr, "")) 'tidy, should just be lf left after split
                    Next
                Else
                    ProcessCommands(strTemp)
                End If

                strTemp = ""    'clear for next time
            Else
                strTemp += strMessage   'add for future
            End If

            ' Start a new asynchronous read into readBuffer.
            client.GetStream.BeginRead(readBuffer, 0, READ_BUFFER_SIZE, AddressOf DoRead, Nothing)
        Catch e As Exception
        End Try
    End Sub
    Public Sub SendData(ByVal data As String)
        If bConnectedToServer = True Then
            Try
                Dim writer As New IO.StreamWriter(client.GetStream)
                writer.Write(data & vbCr)
                writer.Flush()
            Catch ex As Exception
            End Try
            'ShowAction(data)
        End If
    End Sub
    Sub Disconnect()
        Try
            client.Close()
            bConnectedToServer = False
        Catch Ex As Exception
        End Try
    End Sub

    Sub Connect()
        Try
            ' The TcpClient is a subclass of Socket, providing higher level 
            ' functionality like streaming.
            '            client = New TcpClient("192.168.0.6", 10540)
            client = New TcpClient(My.Settings.ServerIPAddress, My.Settings.ServerPort)
            client.GetStream.BeginRead(readBuffer, 0, READ_BUFFER_SIZE, AddressOf DoRead, Nothing)

            ' Make sure the window is showing before popping up connection dialog.
            'Me.Show()
            bConnectedToServer = True
        Catch Ex As Exception
            'UnhandledExceptionHandler()
            bConnectedToServer = False
        End Try
        If bConnectedToServer = True Then
            SendData("CONNECT|" & My.Computer.Name & "-StatViewer" & "|Network|")
        End If
    End Sub

    ' When the server disconnects, prevent further chat messages from being sent.
    Private Sub MarkAsDisconnected()
    End Sub
    ' Process the command received from the server, and take appropriate action.
    Private Sub ProcessCommands(ByVal strMessage As String)
        Dim dataArray() As String
        ' Message parts are divided by "|"  Break the string into an array accordingly.
        Try
            dtLastPAData = Now
            dataArray = strMessage.Split(Chr(124))
            Select Case dataArray(0)
                Case "HEARTBEAT"
                    Select Case dataArray(1)
                        Case "FTP"
                            dtLastPAHeartbeat = Now
                            dtLastLocalHeartbeat = Now
                            dtLastRemoteHeartbeat = Now
                        Case "REMOTESPORTSERVER"
                            dtLastRemoteHeartbeat = Now
                            dtLastLocalHeartbeat = Now
                        Case "SPORTSERVER"
                            dtLastLocalHeartbeat = Now
                    End Select
                Case "STATVIEWER"
                    Select Case dataArray(1)
                        Case "LOADPAGE"
                            Dim pageIndex As Integer = Val(dataArray(2))
                            If pageIndex > 0 Then
                                LoadPage(pageIndex)
                            End If
                        Case "DATA"

                        Case "REFRESH"
                            ReloadPage()
                    End Select
                Case "MATCHDATA"
                    Select Case dataArray(1)
                        Case "LIVEMATCHDETAILS"
                            'Reload static data
                            LoadRTEStaticData()
                            ShowRTEStaticData()
                        Case "SCOREUPDATE"
                            If dataArray.GetUpperBound(0) > 5 Then
                                Dim scoreData As String = dataArray(6)
                                If scoreData.Contains("^") Then
                                    Dim split() As String = scoreData.Split("^")
                                    thisMatch.HomeScoreline = split(0)
                                    thisMatch.AwayScoreline = split(1)
                                End If
                                ShowRTEScoreData()
                            End If
                        Case "LOCALMATCHTIME"
                            thisMatch.MatchClock = dataArray(4) '2 = MatchID, 3=period
                            If dataArray.GetUpperBound(0) > 5 Then
                                'old RB doesn't send
                                thisMatch.HomeTimeSinceScore = dataArray(5) '2 = MatchID, 3=period
                                thisMatch.AwayTimeSinceScore = dataArray(6) '2 = MatchID, 3=period
                            End If
                            ShowMatchTime()
                        Case "ALLTEAMSTATS"
                            'MATCHDATA|ALLTEAMSTATS|29793|565^1^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^|578^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^0^|
                            If dataArray.GetUpperBound(0) > 3 Then
                                Dim homeData As String = dataArray(3)
                                If homeData.Contains("^") Then
                                    Dim split() As String = homeData.Split("^")
                                    For incStat As Integer = 1 To split.GetUpperBound(0)
                                        RBTeamStats(incStat).HomeValue = Val(split(incStat))
                                    Next
                                End If
                                Dim awayData As String = dataArray(4)
                                If awayData.Contains("^") Then
                                    Dim split() As String = awayData.Split("^")
                                    For incStat As Integer = 1 To split.GetUpperBound(0)
                                        RBTeamStats(incStat).AwayValue = Val(split(incStat))
                                    Next
                                End If
                                AssignRBStats()
                                ShowRTETeamData()
                            End If
                        Case "POSSESSION"
                            If dataArray.GetUpperBound(0) > 5 Then
                                'old RB doesn't send
                                Dim compositeData As String = dataArray(6)
                                If compositeData.Contains("^") Then
                                    Dim split() As String = compositeData.Split("^")
                                    thisMatch.Period = Val(split(0))
                                    Select Case thisMatch.Period
                                        Case Is < 3
                                            'first half
                                            thisMatch.HomePossession1 = ""
                                            thisMatch.AwayPossession1 = ""
                                            thisMatch.HomePossession2 = ""
                                            thisMatch.AwayPossession2 = ""
                                            thisMatch.HomePossessionTotal = split(1)
                                            thisMatch.AwayPossessionTotal = split(2)
                                            thisMatch.ActionAreaA1 = split(7)
                                            thisMatch.ActionAreaB1 = split(8)
                                            thisMatch.ActionAreaC1 = split(9)
                                            thisMatch.ActionAreaA2 = split(10)
                                            thisMatch.ActionAreaB2 = split(11)
                                            thisMatch.ActionAreaC2 = split(12)

                                            thisMatch.MatchTime1 = ""
                                            thisMatch.TimeInPlay1 = ""
                                            thisMatch.MatchTime2 = ""
                                            thisMatch.TimeInPlay2 = ""
                                            thisMatch.MatchTimeTotal = split(17)
                                            thisMatch.TimeInPlayTotal = split(18)
                                        Case Else
                                            thisMatch.HomePossession1 = split(1)
                                            thisMatch.AwayPossession1 = split(2)
                                            thisMatch.HomePossession2 = split(3)
                                            thisMatch.AwayPossession2 = split(4)
                                            thisMatch.HomePossessionTotal = split(5)
                                            thisMatch.AwayPossessionTotal = split(6)

                                            thisMatch.ActionAreaA1 = split(7)
                                            thisMatch.ActionAreaB1 = split(8)
                                            thisMatch.ActionAreaC1 = split(9)
                                            thisMatch.ActionAreaA2 = split(10)
                                            thisMatch.ActionAreaB2 = split(11)
                                            thisMatch.ActionAreaC2 = split(12)

                                            thisMatch.MatchTime1 = split(13)
                                            thisMatch.TimeInPlay1 = split(14)
                                            thisMatch.MatchTime2 = split(15)
                                            thisMatch.TimeInPlay2 = split(16)
                                            thisMatch.MatchTimeTotal = split(17)
                                            thisMatch.TimeInPlayTotal = split(18)

                                    End Select
                                    ShowPossession()
                                End If
                            End If

                    End Select

                Case Else
                    Console.WriteLine(strMessage)

            End Select

        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub
    Sub AssignRBStats()
        'RBStats are in RB order
        thisMatch.Stat(SVFStatIndex.Wides).HomeNum = RBTeamStats(RBTeamStatIndex.Wides).HomeValue.ToString
        thisMatch.Stat(SVFStatIndex.Wides).AwayNum = RBTeamStats(RBTeamStatIndex.Wides).AwayValue.ToString
        thisMatch.Stat(SVFStatIndex.ScoringChanges).HomeNum = (RBTeamStats(RBTeamStatIndex.ScoreFromPlay).HomeValue + RBTeamStats(RBTeamStatIndex.FreeScored).HomeValue).ToString + "/" + (RBTeamStats(RBTeamStatIndex.ScoringChanceFromPlay).HomeValue + RBTeamStats(RBTeamStatIndex.FreesConcededOppHalf).AwayValue + RBTeamStats(RBTeamStatIndex.FreesConcededOwnHalf).AwayValue).ToString 'other team's conceded
        thisMatch.Stat(SVFStatIndex.ScoringChanges).AwayNum = (RBTeamStats(RBTeamStatIndex.ScoreFromPlay).AwayValue + RBTeamStats(RBTeamStatIndex.FreeScored).AwayValue).ToString + "/" + (RBTeamStats(RBTeamStatIndex.ScoringChanceFromPlay).AwayValue + RBTeamStats(RBTeamStatIndex.FreesConcededOppHalf).HomeValue + RBTeamStats(RBTeamStatIndex.FreesConcededOwnHalf).HomeValue).ToString
        thisMatch.Stat(SVFStatIndex.FreesConverted).HomeNum = (RBTeamStats(RBTeamStatIndex.FreeScored).HomeValue).ToString + "/" + (RBTeamStats(RBTeamStatIndex.FreesConcededOppHalf).AwayValue + RBTeamStats(RBTeamStatIndex.FreesConcededOwnHalf).AwayValue).ToString  'other team's conceded
        thisMatch.Stat(SVFStatIndex.FreesConverted).AwayNum = (RBTeamStats(RBTeamStatIndex.FreeScored).AwayValue).ToString + "/" + (RBTeamStats(RBTeamStatIndex.FreesConcededOppHalf).HomeValue + RBTeamStats(RBTeamStatIndex.FreesConcededOwnHalf).HomeValue).ToString
        thisMatch.Stat(SVFStatIndex.ScoresFromPlay).HomeNum = (RBTeamStats(RBTeamStatIndex.ScoreFromPlay).HomeValue).ToString + "/" + (RBTeamStats(RBTeamStatIndex.ScoringChanceFromPlay).HomeValue).ToString
        thisMatch.Stat(SVFStatIndex.ScoresFromPlay).AwayNum = (RBTeamStats(RBTeamStatIndex.ScoreFromPlay).AwayValue).ToString + "/" + (RBTeamStats(RBTeamStatIndex.ScoringChanceFromPlay).AwayValue).ToString
        thisMatch.Stat(SVFStatIndex.OwnKickoutsWon).HomeNum = (RBTeamStats(RBTeamStatIndex.OwnKickoutsWon).HomeValue).ToString + "/" + (RBTeamStats(RBTeamStatIndex.Kickouts).HomeValue).ToString
        thisMatch.Stat(SVFStatIndex.OwnKickoutsWon).AwayNum = (RBTeamStats(RBTeamStatIndex.OwnKickoutsWon).AwayValue).ToString + "/" + (RBTeamStats(RBTeamStatIndex.Kickouts).AwayValue).ToString
        thisMatch.Stat(SVFStatIndex.KickoutsWonClean).HomeNum = (RBTeamStats(RBTeamStatIndex.KickoutsWonClean).HomeValue).ToString
        thisMatch.Stat(SVFStatIndex.KickoutsWonClean).AwayNum = (RBTeamStats(RBTeamStatIndex.KickoutsWonClean).AwayValue).ToString
        thisMatch.Stat(SVFStatIndex.KickoutsWonBroken).HomeNum = (RBTeamStats(RBTeamStatIndex.KickoutsWonBroken).HomeValue).ToString
        thisMatch.Stat(SVFStatIndex.KickoutsWonBroken).AwayNum = (RBTeamStats(RBTeamStatIndex.KickoutsWonBroken).AwayValue).ToString
        thisMatch.Stat(SVFStatIndex.FreesConcededOwnHalf).HomeNum = (RBTeamStats(RBTeamStatIndex.FreesConcededOwnHalf).HomeValue).ToString
        thisMatch.Stat(SVFStatIndex.FreesConcededOwnHalf).AwayNum = (RBTeamStats(RBTeamStatIndex.FreesConcededOwnHalf).AwayValue).ToString
        thisMatch.Stat(SVFStatIndex.FreesConcededOppHalf).HomeNum = (RBTeamStats(RBTeamStatIndex.FreesConcededOppHalf).HomeValue).ToString
        thisMatch.Stat(SVFStatIndex.FreesConcededOppHalf).AwayNum = (RBTeamStats(RBTeamStatIndex.FreesConcededOppHalf).AwayValue).ToString
        thisMatch.Stat(SVFStatIndex.Blacks).HomeNum = (RBTeamStats(RBTeamStatIndex.BlackCards).HomeValue).ToString
        thisMatch.Stat(SVFStatIndex.Blacks).AwayNum = (RBTeamStats(RBTeamStatIndex.BlackCards).AwayValue).ToString
        thisMatch.Stat(SVFStatIndex.Yellows).HomeNum = (RBTeamStats(RBTeamStatIndex.YellowCards).HomeValue).ToString
        thisMatch.Stat(SVFStatIndex.Yellows).AwayNum = (RBTeamStats(RBTeamStatIndex.YellowCards).AwayValue).ToString
        thisMatch.Stat(SVFStatIndex.Reds).HomeNum = (RBTeamStats(RBTeamStatIndex.RedCards).HomeValue).ToString
        thisMatch.Stat(SVFStatIndex.Reds).AwayNum = (RBTeamStats(RBTeamStatIndex.RedCards).AwayValue).ToString
        thisMatch.Stat(SVFStatIndex.SubsMade).HomeNum = (RBTeamStats(RBTeamStatIndex.SubsUsed).HomeValue).ToString
        thisMatch.Stat(SVFStatIndex.SubsMade).AwayNum = (RBTeamStats(RBTeamStatIndex.SubsUsed).AwayValue).ToString
        thisMatch.Stat(SVFStatIndex.TurnoversWon).HomeNum = (RBTeamStats(RBTeamStatIndex.TurnoversWon).HomeValue).ToString
        thisMatch.Stat(SVFStatIndex.TurnoversWon).AwayNum = (RBTeamStats(RBTeamStatIndex.TurnoversWon).AwayValue).ToString
        thisMatch.Stat(SVFStatIndex.HandPasses).HomeNum = (RBTeamStats(RBTeamStatIndex.HandPasses).HomeValue).ToString
        thisMatch.Stat(SVFStatIndex.HandPasses).AwayNum = (RBTeamStats(RBTeamStatIndex.HandPasses).AwayValue).ToString
        thisMatch.Stat(SVFStatIndex.FootPasses).HomeNum = (RBTeamStats(RBTeamStatIndex.FootPasses).HomeValue).ToString
        thisMatch.Stat(SVFStatIndex.FootPasses).AwayNum = (RBTeamStats(RBTeamStatIndex.FootPasses).AwayValue).ToString
        thisMatch.Stat(SVFStatIndex.FortyFives).HomeNum = (RBTeamStats(RBTeamStatIndex.FortyFive).HomeValue).ToString
        thisMatch.Stat(SVFStatIndex.FortyFives).AwayNum = (RBTeamStats(RBTeamStatIndex.FortyFive).AwayValue).ToString
    End Sub
    Delegate Sub ShowMatchTimeCallback()
    Sub ShowMatchTime()
        If lablMatchClock.InvokeRequired Then
            Dim d As New ShowMatchTimeCallback(AddressOf ShowMatchTime)
            Me.Invoke(d, New Object() {})
        Else
            lablMatchClock.Text = thisMatch.MatchClock
            lablHomeTimeSinceScore.Text = thisMatch.HomeTimeSinceScore
            lablAwayTimeSinceScore.Text = thisMatch.AwayTimeSinceScore
        End If
    End Sub
    Delegate Sub ShowPossessionCallback()
    Sub ShowPossession()
        If lablMatchClock.InvokeRequired Then
            Dim d As New ShowPossessionCallback(AddressOf ShowPossession)
            Me.Invoke(d, New Object() {})
        Else
            lablHomePossession1.Text = thisMatch.HomePossession1
            lablHomePossession2.Text = thisMatch.HomePossession2
            lablHomePossessionTotal.Text = thisMatch.HomePossessionTotal
            lablAwayPossession1.Text = thisMatch.AwayPossession1
            lablAwayPossession2.Text = thisMatch.AwayPossession2
            lablAwayPossessionTotal.Text = thisMatch.AwayPossessionTotal

            lablActionAreaA1.Text = thisMatch.ActionAreaA1
            lablActionAreaB1.Text = thisMatch.ActionAreaB1
            lablActionAreaC1.Text = thisMatch.ActionAreaC1
            lablActionAreaA2.Text = thisMatch.ActionAreaA2
            lablActionAreaB2.Text = thisMatch.ActionAreaB2
            lablActionAreaC2.Text = thisMatch.ActionAreaC2

            lablTimeInPlay1.Text = thisMatch.TimeInPlayText1
            lablTimeInPlay2.Text = thisMatch.TimeInPlayText2
            lablTimeInPlayTotal.Text = thisMatch.TimeInplaytextTotal

            lablTimesLevel.Text = thisMatch.TimesLevel
        End If
    End Sub

    Private Sub timerCheckConnections_Tick(sender As Object, e As EventArgs) Handles timerCheckConnections.Tick
        If DateDiff(DateInterval.Minute, dtLastPAData, Now) > 1 Then
            If bConnectedToServer = True Then
                Disconnect()
            End If
            Connect()
        End If
    End Sub

    Private Sub MaximiseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MaximiseToolStripMenuItem.Click
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.WindowState = FormWindowState.Maximized
        ResizeBrowser(fontSize)
        Me.MenuStrip1.Visible = False
    End Sub

    Private Sub ResetToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ResetToolStripMenuItem.Click
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.Sizable
        Me.WindowState = FormWindowState.Normal
        Me.MenuStrip1.Visible = True
    End Sub

    Private Sub IncreaseFontSizeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles IncreaseFontSizeToolStripMenuItem.Click
        fontSize += 1
        ResizeBrowser(fontSize)
    End Sub

    Private Sub DecreaseFontSizeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DecreaseFontSizeToolStripMenuItem.Click
        fontSize -= 1
        ResizeBrowser(fontSize)
    End Sub
    Private Sub ResizeBrowser(ByVal dispfontsize As Integer)
        Dim zoom As Integer = (dispfontsize / 2 - 3) * 50
        WebBrowser1.ActiveXInstance.ExecWB(OLECMDID_OPTICAL_ZOOM, OLECMDEXECOPT_DONTPROMPTUSER, zoom - 0, Nothing)
        My.Settings.FontSize = dispfontsize
        My.Settings.Save()
    End Sub
    Private Sub ReloadPage()
        Try
            Console.WriteLine("Refreshing")
            WebBrowser1.Refresh()
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
        'If WebBrowser1.IsBusy = False Then
        'End If
    End Sub
    Private Sub LoadPage(pageIndex As Integer)
        Select Case pageIndex
            Case 1
                WebBrowser1.Url = New Uri(My.Settings.URL1)
            Case 2
                WebBrowser1.Url = New Uri(My.Settings.URL2)
            Case 3
                WebBrowser1.Url = New Uri(My.Settings.URL3)
            Case 4
                WebBrowser1.Url = New Uri(My.Settings.URL4)
            Case 5
                WebBrowser1.Url = New Uri(My.Settings.URL5)
            Case 6
                WebBrowser1.Url = New Uri(My.Settings.URL6)
            Case 7
                WebBrowser1.Url = New Uri(My.Settings.URL7)
            Case 8
                WebBrowser1.Url = New Uri(My.Settings.URL8)
            Case 9
                WebBrowser1.Url = New Uri(My.Settings.URL9)
            Case 10
                WebBrowser1.Url = New Uri(My.Settings.URL10)
        End Select
        'will load cached version
        'Threading.Thread.Sleep(2000)
        'WebBrowser1.Refresh()
    End Sub
    Private Sub Page1ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles Page1ToolStripMenuItem.Click
        LoadPage(1)
    End Sub

    Private Sub Page2ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles Page2ToolStripMenuItem.Click
        LoadPage(2)
    End Sub

    Private Sub Page3ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles Page3ToolStripMenuItem.Click
        LoadPage(3)
    End Sub

    Private Sub Page4ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles Page4ToolStripMenuItem.Click
        LoadPage(4)
    End Sub

    Private Sub Page5ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles Page5ToolStripMenuItem.Click
        LoadPage(5)
    End Sub

    Private Sub Page6ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles Page6ToolStripMenuItem.Click
        LoadPage(6)
    End Sub

    Private Sub Page7ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles Page7ToolStripMenuItem.Click
        LoadPage(7)
    End Sub

    Private Sub Page8ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles Page8ToolStripMenuItem.Click
        LoadPage(8)
    End Sub

    Private Sub Page9ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles Page9ToolStripMenuItem.Click
        LoadPage(9)
    End Sub

    Private Sub Page10ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles Page10ToolStripMenuItem.Click
        ReloadPage()
    End Sub

    Private Sub QuitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles QuitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub OnToolStripMenuItem_Click(sender As Object, e As EventArgs)
    End Sub

    Private Sub OffToolStripMenuItem_Click(sender As Object, e As EventArgs)
    End Sub
    Private Sub RepositionBrowser()
        Dim offset As Integer = 0
        Me.WebBrowser1.Left = left
        Me.WebBrowser1.Width = Me.Width - (offset + (left * 2))
        Me.WebBrowser1.Top = top
        Me.WebBrowser1.Height = 4000
        My.Settings.Left = left
        My.Settings.Top = top
        My.Settings.Save()
    End Sub

    Private Sub UpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UpToolStripMenuItem.Click
        top -= 2
        RepositionBrowser()
    End Sub

    Private Sub DownToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DownToolStripMenuItem.Click
        top += 2
        RepositionBrowser()
    End Sub

    Private Sub LeftToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LeftToolStripMenuItem.Click
        left -= 2
        RepositionBrowser()
    End Sub

    Private Sub RightToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RightToolStripMenuItem.Click
        left += 2
        RepositionBrowser()
    End Sub

    Private Sub WebBrowser1_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted
        Console.WriteLine(WebBrowser1.Url)
        'may be from cache if first time - refresh
        If lastPageName <> WebBrowser1.Url.ToString Then
            'new
            Console.WriteLine("Refreshing")
            WebBrowser1.Refresh()
            lastPageName = WebBrowser1.Url.ToString
        End If
    End Sub

    'Private Sub RacingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RacingToolStripMenuItem.Click
    '    displayStyle = 0
    '    SetupDisplay()
    'End Sub

    'Private Sub RTEGAAToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RTEGAAToolStripMenuItem.Click
    '    displayStyle = 1
    '    SetupDisplay()
    'End Sub

    Private Sub Label15_Click(sender As Object, e As EventArgs) Handles lablStatHome20.Click

    End Sub

    Private Sub Label12_Click(sender As Object, e As EventArgs) Handles Label12.Click

    End Sub

    Private Sub lablTimeInPlay_Click(sender As Object, e As EventArgs) Handles lablTimeInPlayTotal.Click

    End Sub
End Class
