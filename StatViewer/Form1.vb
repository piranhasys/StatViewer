Imports System.Net.Sockets
Imports System.Text
Imports System.Data.SqlClient
Imports System.Net
Imports System.IO
Imports System.Net.DnsPermissionAttribute
Imports System.Security.Permissions
Imports System.Runtime.InteropServices
Imports System.Collections.Generic
Imports System.Web.Script.Serialization
Imports System.Drawing.Drawing2D

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
    Private displayStyle As Integer = 0 '0=web, 1=RTE GAA, 2=Star6, 3=SkySuperLeague
    'Private locationLeft As Integer = 0
    'Private locationTop As Integer = 0
    Private lastPageName As String = ""
    Private thisMatch As New clsMatch

    Private Sub Form1_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        Select Case displayStyle
            Case DisplayType.Racing
                Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
                Me.WindowState = FormWindowState.Maximized
                Me.MenuStrip1.Visible = False
        End Select

    End Sub

    Private Sub Form1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress

    End Sub
    Private Sub ParseCommandLineArgs()
        Dim inputArgument As String = "/input="
        Dim inputName As String = ""

        For Each s As String In My.Application.CommandLineArgs
            Config.ConfigFilename = s
        Next
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ParseCommandLineArgs()
        For incStat As Integer = 0 To RBTeamStats.GetUpperBound(0)
            RBTeamStats(incStat) = New clsRBTeamStat
        Next
        Config.ReadSetup()
        If Config.FileExists Then
            Select Case Config.UserName
                Case "SKYSUPERLEAGUE"
                    displayStyle = DisplayType.SkySuperLeague
                    My.Settings.ServerPort = Config.ServerPort
                    My.Settings.ServerIPAddress = Config.ServerAddress
                Case "RACING", "IRIS"
                    displayStyle = DisplayType.Racing
                    My.Settings.ServerPort = Config.ServerPort
                    My.Settings.ServerIPAddress = Config.ServerAddress
            End Select
        Else
            'default leagacy
            displayStyle = My.Settings.DisplayType
        End If
        dtLastPAData = Now.AddDays(-1)
        timerCheckConnections.Start()
        fontSize = My.Settings.FontSize
        Me.Left = My.Settings.Left
        Me.Top = My.Settings.Top
        Me.Text = "PIRANHA StatViewer v" + ProductVersion + "   Use F1 for Full Screen, F2 for Status Bar, Ctrl+Q to quit"
        Select Case displayStyle
            Case DisplayType.Web
            Case DisplayType.Racing
            Case Else
                For incStat As Integer = 0 To thisMatch.Stat.GetUpperBound(0)
                    thisMatch.Stat(incStat) = New clsSVStat
                Next
        End Select
        SetupDisplay()
        Select Case displayStyle
            Case DisplayType.Racing
                Connect()
                '                MaximiseToolStripMenuItem.PerformClick()
        End Select
    End Sub
    Sub SetupDisplay()
        Select Case displayStyle
            Case DisplayType.RTE_GAA
                panelStar6.Visible = False
                PanelRTE.Visible = True
                panelSL.Visible = False
                panelRacing.Visible = False
                PanelRTE.BringToFront()
                LoadRTEStaticData()
                ShowRTEStaticData()
                ShowRTETeamData()
            Case DisplayType.Star6
                panelStar6.Visible = True
                PanelRTE.Visible = False
                panelSL.Visible = False
                panelRacing.Visible = False
                panelStar6.BringToFront()
                Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
                Me.StartPosition = FormStartPosition.Manual
                Me.Location = New Point(0, 0)
                Me.WindowState = FormWindowState.Maximized
            Case DisplayType.SkySuperLeague
                panelRacing.Visible = False
                panelSL.Visible = True
                panelSL.BringToFront()
            Case DisplayType.Racing
                panelStar6.Visible = False
                PanelRTE.Visible = False
                panelSL.Visible = False
                panelRacing.Visible = True
            Case Else
                panelStar6.Visible = False
                PanelRTE.Visible = False
                panelSL.Visible = False
                panelRacing.Visible = False
                PanelRTE.SendToBack()
                RepositionBrowser()
        End Select
        'RacingToolStripMenuItem.Checked = (displayStyle = 0)
        'RTEGAAToolStripMenuItem.Checked = (displayStyle = 1)

    End Sub
    Sub LoadRTEStaticData()
        Dim strTextLine As String = ""
        Dim strFilename As String = My.Settings.DataFilename
        Dim inputFile As System.IO.StreamReader
        'Dim TempArray() As String
        Try

            If Not System.IO.File.Exists(strFilename) Then
                Exit Sub
            Else
                inputFile = System.IO.File.OpenText(strFilename)
                thisMatch.HomeTeamName = inputFile.ReadLine.ToUpper
                thisMatch.AwayTeamName = inputFile.ReadLine.ToUpper
                Dim dummy1 As String = inputFile.ReadLine   'score. don't read, only take live
                Dim dummy2 As String = inputFile.ReadLine   'score
                thisMatch.HomeBackColour = Color.FromArgb(Val(inputFile.ReadLine))
                thisMatch.HomeTextColour = Color.FromArgb(Val(inputFile.ReadLine))
                thisMatch.AwayBackColour = Color.FromArgb(Val(inputFile.ReadLine))
                thisMatch.AwayTextColour = Color.FromArgb(Val(inputFile.ReadLine))

                If thisMatch.HomeBackColour = thisMatch.HomeTextColour Then
                    thisMatch.HomeTextColour = Color.White
                    thisMatch.HomeBackColour = Color.Gray
                End If
                If thisMatch.AwayBackColour = thisMatch.AwayTextColour Then
                    thisMatch.AwayTextColour = Color.White
                    thisMatch.AwayBackColour = Color.Gray
                End If
                For incStat As Integer = 1 To 20
                    thisMatch.Stat(incStat).AssignData(inputFile.ReadLine)
                Next


                inputFile.Close()
            End If
        Catch ex As Exception
            ' MessageBox.Show(ex.ToString, "Error reading stat names")
        End Try

    End Sub
    Delegate Sub ShowStar6StaticDataCallback()
    Sub ShowStar6StaticData()
        If lablHomeTeam.InvokeRequired Then
            Dim d As New ShowStar6StaticDataCallback(AddressOf ShowStar6StaticData)
            Me.Invoke(d, New Object() {})
        Else
            lablStar6TeamHome.Text = thisMatch.HomeTeamName
            lablStar6TeamAway.Text = thisMatch.AwayTeamName
        End If
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

            lablHomeScorers.Text = thisMatch.HomeScorers.Replace("^", vbLf).Replace(", ", vbLf)
            lablAwayScorers.Text = thisMatch.AwayScorers.Replace("^", vbLf).Replace(", ", vbLf)
            lablHomePossessionsHeader.Visible = False
            lablHomePossessions.Visible = False
            lablAwayPossessionsHeader.Visible = False
            lablAwayPossessions.Visible = False

        End If

    End Sub
    Delegate Sub ShowSkyTeamDataCallback()
    Sub ShowSkyTeamData()
        If lablStatAway01.InvokeRequired Then
            Dim d As New ShowSkyTeamDataCallback(AddressOf ShowSkyTeamData)
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

            lablHomeScorers.Text = thisMatch.HomeScorers.Replace("^", vbLf).Replace(", ", vbLf)
            lablAwayScorers.Text = thisMatch.AwayScorers.Replace("^", vbLf).Replace(", ", vbLf)
            lablHomePossessionsHeader.Visible = True
            lablHomePossessions.Visible = True
            lablAwayPossessionsHeader.Visible = True
            lablAwayPossessions.Visible = True
            lablHomePossessions.Text = thisMatch.HomePossessions.Replace("^", vbLf)
            lablAwayPossessions.Text = thisMatch.AwayPossessions.Replace("^", vbLf)
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
                Console.WriteLine("Sent: " + data)
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
            SendData("CONNECT|" & My.Computer.Name & "-StatViewer" & "|Network||StatViewer|")
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
            Console.WriteLine(strMessage)
            Select Case dataArray(0)
                Case "CONNECTED"
                    Select Case displayStyle
                        Case DisplayType.SkySuperLeague
                            SendData("STATVIEWER|REQUESTLIVEMATCHDATA|")
                    End Select
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
                        Case "LOADBETTING"
                            'JSON string
                            Dim JSONString As String = dataArray(2)
                            CurrentBettingBoard = New clsBettingBoard 'clear old data
                            Dim json As New JavaScriptSerializer
                            CurrentBettingBoard = json.Deserialize(Of clsBettingBoard)(JSONString)
                            ShowBettingBoard()
                        Case "UPDATEBETTING"
                            'JSON string
                            Dim JSONString As String = dataArray(2)
                            CurrentBettingBoard = New clsBettingBoard 'clear old data
                            Dim json As New JavaScriptSerializer
                            CurrentBettingBoard = json.Deserialize(Of clsBettingBoard)(JSONString)
                            ShowBettingBoard()
                        Case "REFRESH"
                            ReloadPage()
                    End Select
                Case "SPORTCLOCK"
                    'SPORTCLOCK|MATCHTIME|49233|1|38:01|01:59|RUNNING|
                    Select Case Config.UserName
                        Case "SKYSUPERLEAGUE"
                            Dim iMatchID As Integer = Val(dataArray(2))
                            RemoteData.CurrentMatchPeriod = Val(dataArray(3))
                            RemoteData.MatchClockTime = dataArray(5)  'counting up
                            RemoteData.MatchClockRunning = (dataArray(6) = "RUNNING")
                            ShowSuperLeagueData()
                    End Select
                Case "MATCHDATA"
                    Select Case dataArray(1)
                        Case "LIVEMATCHDETAILS"
                            'Reload static data
                            LoadRTEStaticData()
                            ShowRTEStaticData()
                        Case "SCOREUPDATE"
                            If dataArray.GetUpperBound(0) > 5 Then
                                Select Case Config.UserName
                                    Case "SKYSUPERLEAGUE"
                                        Dim scoreData As String = dataArray(6)
                                        If scoreData.Contains("^") Then
                                            Dim split() As String = scoreData.Split("^")
                                            thisMatch.HomeScoreline = split(0)
                                            thisMatch.AwayScoreline = split(1)
                                            ShowSuperLeagueData()
                                        End If
                                    Case Else
                                        Dim scoreData As String = dataArray(6)
                                        If scoreData.Contains("^") Then
                                            Dim split() As String = scoreData.Split("^")
                                            thisMatch.HomeScoreline = split(0)
                                            thisMatch.AwayScoreline = split(1)
                                        End If
                                        ShowRTEScoreData()
                                End Select
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
                            'RTE format
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
                                If dataArray.GetUpperBound(0) > 5 Then
                                    'contains scorers data
                                    thisMatch.HomeScorers = dataArray(5)
                                    thisMatch.AwayScorers = dataArray(6)
                                End If
                                If dataArray.GetUpperBound(0) > 7 Then
                                    'subs used
                                    RBTeamStats(35).HomeValue = Val(dataArray(7))
                                    RBTeamStats(35).AwayValue = Val(dataArray(8))
                                End If
                                If dataArray.GetUpperBound(0) > 9 Then
                                    'marks
                                    RBTeamStats(36).HomeValue = Val(dataArray(9))
                                    RBTeamStats(36).AwayValue = Val(dataArray(10))
                                End If

                                AssignRBStats()
                                ShowRTETeamData()
                            End If
                        Case "MATCHFACTS"
                            'MATCHDATA|MATCHFACTS|DATA|KILDARE^LAOIS^10^POSSESSION^^^KICK PASSES^1^0^HAND PASSES^0^0^KICKOUTS WON^0^0^INSIDE 45^0^0^SCORES / SHOTS^1 / 0^0 / 0^WIDES^0^0^FREES CONCEDED^0^0^YELLOW CARDS^0^0^RED CARDS^0^0^TIME IN PLAY: 00:00 OUT OF 00:00^|DONNELLAN 2-1^O'GRADY 0-3^MURNAGHAN 0-1^|R. KEHOE 1-2^M. TIMMONS 0-4^S. ATTRIDE 1-0^|
                            Select Case displayStyle
                                Case DisplayType.Star6
                                    AssignRBStar6Data(strMessage)
                                    ShowStar6StaticData()
                                Case DisplayType.SkySuperLeague
                                    AssignRBSLData(strMessage)
                                    ShowSuperLeagueData()
                                Case Else
                                    AssignRBSkyData(strMessage)
                                    ShowSkyTeamData()
                            End Select

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
    Delegate Sub ShowBettingBoardCallback()
    Sub ShowBettingBoard()
        If picBoxRaceBGD.InvokeRequired Then
            Dim d As New ShowBettingBoardCallback(AddressOf ShowBettingBoard)
            Me.Invoke(d, New Object() {})
        Else
            DrawAllBettingBoard()
            'lablRaceHeading1.Text = CurrentBettingBoard.Heading
            'lablChanges1.Text = CurrentBettingBoard.Changes1Text
            'lablChanges2.Text = CurrentBettingBoard.Changes2Text
            'lablChanges3.Text = CurrentBettingBoard.Changes3Text
            'lablChanges4.Text = CurrentBettingBoard.Changes4Text
        End If
    End Sub
    Sub AssignRBSkyData(dataString As String)
        'already ordered and labelled
        'MATCHDATA|MATCHFACTS|DATA|KILDARE^LAOIS^10^POSSESSION^^^KICK PASSES^1^0^HAND PASSES^0^0^KICKOUTS WON^0^0^INSIDE 45^0^0^SCORES / SHOTS^1 / 0^0 / 0^WIDES^0^0^FREES CONCEDED^0^0^YELLOW CARDS^0^0^RED CARDS^0^0^TIME IN PLAY: 00:00 OUT OF 00:00^|DONNELLAN 2-1^O'GRADY 0-3^MURNAGHAN 0-1^|R. KEHOE 1-2^M. TIMMONS 0-4^S. ATTRIDE 1-0^|
        If dataString.Contains("^") Then
            Dim baseIndex As Integer = 0
            Dim splitMain As String() = dataString.Split("|")
            If splitMain(3).Contains("^") Then
                Dim split As String() = splitMain(3).Split("^")
                For incstat As Integer = 1 To 11    'WAS 10, added wides
                    baseIndex = (incstat * 3) + 3
                    thisMatch.Stat(incstat).Name = split(baseIndex)
                    thisMatch.Stat(incstat).HomeNum = split(baseIndex + 1)
                    thisMatch.Stat(incstat).AwayNum = split(baseIndex + 2)
                Next
            End If
            If splitMain.GetUpperBound(0) > 4 Then
                'contains scorers data
                thisMatch.HomeScorers = splitMain(4)
                thisMatch.AwayScorers = splitMain(5)
            End If
            If splitMain.GetUpperBound(0) > 6 Then
                'contains scorers data
                thisMatch.HomePossessions = splitMain(6)
                thisMatch.AwayPossessions = splitMain(7)
            End If

        End If

    End Sub
    Sub AssignRBSLData(dataString As String)
        'already ordered and labelled
        'MATCHDATA|MATCHFACTS|DATA|DRAGONS^TIGERS^1^0|POSSESSION^^^PENALTIES^0^0^GL DROPOUTS^0^0^TACKLES^0^0^MISSED TACKLES^5^2^BREAKS^0^0^HANDLING ERRORS^0^1^OFFLOADS^5^2^RUNS FROM DUMMY HALF^1^0^CARRIES^0^0^COMPLETED SETS^0^0^COMPLETION RATE^0%^0%^TOTAL METRES^0^0^AVERAGE METRES^0^0^MINS IN OPP HALF^0^0^|ESCARE  8^DUPORT  8^OLDFIELD  4^POMEROY  2^  ^  ^  ^  ^  ^  ^|Webster  4^Lynch  4^Cook  1^  ^  ^  ^  ^  ^  ^  ^|

        If dataString.Contains("^") Then
            Dim baseIndex As Integer = 0
            Dim splitMain As String() = dataString.Split("|")
            If splitMain(3).Contains("^") Then
                Dim split As String() = splitMain(3).Split("^")
                thisMatch.HomeTeamName = split(0)
                thisMatch.AwayTeamName = split(1)
                thisMatch.HomeScoreline = split(2)
                thisMatch.AwayScoreline = split(3)
            End If
            If splitMain(4).Contains("^") Then
                Dim split As String() = splitMain(4).Split("^")
                For incstat As Integer = 1 To 17
                    baseIndex = (incstat * 3) - 3
                    thisMatch.Stat(incstat).Name = split(baseIndex)
                    thisMatch.Stat(incstat).HomeNum = split(baseIndex + 1)
                    thisMatch.Stat(incstat).AwayNum = split(baseIndex + 2)
                Next
            End If
            If splitMain.GetUpperBound(0) > 4 Then
                'contains scorers data
                thisMatch.HomeScorers = splitMain(5)
                thisMatch.AwayScorers = splitMain(6)
            End If
            If splitMain.GetUpperBound(0) > 11 Then
                'contains Carries,Metres,Tackles data
                thisMatch.HomeCarries = splitMain(7)
                thisMatch.AwayCarries = splitMain(8)
                thisMatch.HomeMetresMade = splitMain(9)
                thisMatch.AwayMetresMade = splitMain(10)
                thisMatch.HomeTackles = splitMain(11)
                thisMatch.AwayTackles = splitMain(12)
            End If

        End If

    End Sub
    Sub AssignRBStar6Data(dataString As String)
        'already ordered and labelled
        'MATCHDATA|MATCHFACTS|DATA|KILDARE^LAOIS^10^POSSESSION^^^KICK PASSES^1^0^HAND PASSES^0^0^KICKOUTS WON^0^0^INSIDE 45^0^0^SCORES / SHOTS^1 / 0^0 / 0^WIDES^0^0^FREES CONCEDED^0^0^YELLOW CARDS^0^0^RED CARDS^0^0^TIME IN PLAY: 00:00 OUT OF 00:00^|DONNELLAN 2-1^O'GRADY 0-3^MURNAGHAN 0-1^|R. KEHOE 1-2^M. TIMMONS 0-4^S. ATTRIDE 1-0^|
        If dataString.Contains("^") Then
            Dim baseIndex As Integer = 0
            Dim splitMain As String() = dataString.Split("|")
            If splitMain(3).Contains("^") Then
                Dim split As String() = splitMain(3).Split("^")
                thisMatch.HomeTeamName = split(0).ToUpper
                thisMatch.AwayTeamName = split(1).ToUpper
            End If
        End If

    End Sub
    Sub AssignRBStats()
        'RBStats are in RB order
        thisMatch.Stat(SVFStatIndex.Wides).HomeNum = RBTeamStats(RBTeamStatIndex.Wides).HomeValue.ToString
        thisMatch.Stat(SVFStatIndex.Wides).AwayNum = RBTeamStats(RBTeamStatIndex.Wides).AwayValue.ToString
        thisMatch.Stat(SVFStatIndex.ScoringChanges).HomeNum = (RBTeamStats(RBTeamStatIndex.ScoreFromPlay).HomeValue + RBTeamStats(RBTeamStatIndex.FreeScored).HomeValue).ToString + "/" + (RBTeamStats(RBTeamStatIndex.ScoringChanceFromPlay).HomeValue + RBTeamStats(RBTeamStatIndex.FreeTaken).HomeValue + RBTeamStats(RBTeamStatIndex.FortyFive).HomeValue).ToString
        thisMatch.Stat(SVFStatIndex.ScoringChanges).AwayNum = (RBTeamStats(RBTeamStatIndex.ScoreFromPlay).AwayValue + RBTeamStats(RBTeamStatIndex.FreeScored).AwayValue).ToString + "/" + (RBTeamStats(RBTeamStatIndex.ScoringChanceFromPlay).AwayValue + RBTeamStats(RBTeamStatIndex.FreeTaken).AwayValue + RBTeamStats(RBTeamStatIndex.FortyFive).AwayValue).ToString
        thisMatch.Stat(SVFStatIndex.FreesConverted).HomeNum = (RBTeamStats(RBTeamStatIndex.FreeScored).HomeValue).ToString + "/" + (RBTeamStats(RBTeamStatIndex.FreeTaken).HomeValue + RBTeamStats(RBTeamStatIndex.FortyFive).HomeValue).ToString
        thisMatch.Stat(SVFStatIndex.FreesConverted).AwayNum = (RBTeamStats(RBTeamStatIndex.FreeScored).AwayValue).ToString + "/" + (RBTeamStats(RBTeamStatIndex.FreeTaken).AwayValue + RBTeamStats(RBTeamStatIndex.FortyFive).AwayValue).ToString
        thisMatch.Stat(SVFStatIndex.ScoresFromPlay).HomeNum = (RBTeamStats(RBTeamStatIndex.ScoreFromPlay).HomeValue).ToString + "/" + thisMatch.HomeScores
        thisMatch.Stat(SVFStatIndex.ScoresFromPlay).AwayNum = (RBTeamStats(RBTeamStatIndex.ScoreFromPlay).AwayValue).ToString + "/" + thisMatch.AwayScores
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
        thisMatch.Stat(SVFStatIndex.Marks).HomeNum = (RBTeamStats(RBTeamStatIndex.Marks).HomeValue).ToString
        thisMatch.Stat(SVFStatIndex.Marks).AwayNum = (RBTeamStats(RBTeamStatIndex.Marks).AwayValue).ToString
    End Sub
    Delegate Sub ShowMatchTimeCallback()
    Sub ShowMatchTime()
        If lablMatchClock.InvokeRequired Then
            Dim d As New ShowMatchTimeCallback(AddressOf ShowMatchTime)
            Me.Invoke(d, New Object() {})
        Else
            lablMatchClock.Text = thisMatch.MatchClock
            lablStar6MatchClock.Text = thisMatch.MatchClock
            lablHomeTimeSinceScore.Text = thisMatch.HomeTimeSinceScore
            lablAwayTimeSinceScore.Text = thisMatch.AwayTimeSinceScore
        End If
    End Sub
    Delegate Sub ShowSuperLeagueDataCallback()
    Sub ShowSuperLeagueData()
        If lablMatchClockSL.InvokeRequired Then
            Dim d As New ShowSuperLeagueDataCallback(AddressOf ShowSuperLeagueData)
            Me.Invoke(d, New Object() {})
        Else
            lablRemoteHomePossessionSL.Text = RemoteData.HomePossession
            lablRemoteAwayPossessionSL.Text = RemoteData.AwayPossession
            Select Case RemoteData.CurrentPossessionTeam
                Case 1
                    lablRemoteHomePossessionSL.BackColor = Color.LightGreen
                    lablRemoteAwayPossessionSL.BackColor = Color.White
                Case 2
                    lablRemoteHomePossessionSL.BackColor = Color.White
                    lablRemoteAwayPossessionSL.BackColor = Color.LightGreen
                Case Else
                    lablRemoteHomePossessionSL.BackColor = Color.White
                    lablRemoteAwayPossessionSL.BackColor = Color.White
            End Select
            lablMatchClockSL.Text = RemoteData.MatchClockTime
            lablMatchClockSL.BackColor = If(RemoteData.MatchClockRunning, Color.LightGreen, Color.LightPink)
            Select Case RemoteData.CurrentMatchPeriod
                Case 1
                    lablPeriodSL.Text = "1st HALF"
                Case 2
                    lablPeriodSL.Text = "2nd HALF"
                Case 3
                    lablPeriodSL.Text = "ET 1st HALF"
                Case 4
                    lablPeriodSL.Text = "ET 2nd HALF"
                Case Else
                    lablPeriodSL.Text = "Pre Match"
            End Select

            'MATCHFACTS data:
            lablHomeNameSL.Text = thisMatch.HomeTeamName
            lablAwayNameSL.Text = thisMatch.AwayTeamName
            lablHomeNameSL2.Text = thisMatch.HomeTeamName
            lablAwayNameSL2.Text = thisMatch.AwayTeamName

            lablHomeScoreSL.Text = thisMatch.HomeScoreline
            lablAwayScoreSL.Text = thisMatch.AwayScoreline

            lablStatNameSL01.Text = thisMatch.Stat(1).Name
            lablStatNameSL02.Text = thisMatch.Stat(2).Name
            lablStatNameSL03.Text = thisMatch.Stat(3).Name
            lablStatNameSL04.Text = thisMatch.Stat(4).Name
            lablStatNameSL05.Text = thisMatch.Stat(5).Name
            lablStatNameSL06.Text = thisMatch.Stat(6).Name
            lablStatNameSL07.Text = thisMatch.Stat(7).Name
            lablStatNameSL08.Text = thisMatch.Stat(8).Name
            lablStatNameSL09.Text = thisMatch.Stat(9).Name
            lablStatNameSL10.Text = thisMatch.Stat(10).Name
            lablStatNameSL11.Text = thisMatch.Stat(11).Name
            lablStatNameSL12.Text = thisMatch.Stat(12).Name
            lablStatNameSL13.Text = thisMatch.Stat(13).Name
            lablStatNameSL14.Text = thisMatch.Stat(14).Name
            lablStatNameSL15.Text = thisMatch.Stat(15).Name
            lablStatNameSL16.Text = thisMatch.Stat(16).Name
            lablStatNameSL17.Text = thisMatch.Stat(17).Name

            lablStatHomeSL01.Text = thisMatch.Stat(1).HomeNum
            lablStatHomeSL02.Text = thisMatch.Stat(2).HomeNum
            lablStatHomeSL03.Text = thisMatch.Stat(3).HomeNum
            lablStatHomeSL04.Text = thisMatch.Stat(4).HomeNum
            lablStatHomeSL05.Text = thisMatch.Stat(5).HomeNum
            lablStatHomeSL06.Text = thisMatch.Stat(6).HomeNum
            lablStatHomeSL07.Text = thisMatch.Stat(7).HomeNum
            lablStatHomeSL08.Text = thisMatch.Stat(8).HomeNum
            lablStatHomeSL09.Text = thisMatch.Stat(9).HomeNum
            lablStatHomeSL10.Text = thisMatch.Stat(10).HomeNum
            lablStatHomeSL11.Text = thisMatch.Stat(11).HomeNum
            lablStatHomeSL12.Text = thisMatch.Stat(12).HomeNum
            lablStatHomeSL13.Text = thisMatch.Stat(13).HomeNum
            lablStatHomeSL14.Text = thisMatch.Stat(14).HomeNum
            lablStatHomeSL15.Text = thisMatch.Stat(15).HomeNum
            lablStatHomeSL16.Text = thisMatch.Stat(16).HomeNum
            lablStatHomeSL17.Text = thisMatch.Stat(17).HomeNum

            lablStatAwaySL01.Text = thisMatch.Stat(1).AwayNum
            lablStatAwaySL02.Text = thisMatch.Stat(2).AwayNum
            lablStatAwaySL03.Text = thisMatch.Stat(3).AwayNum
            lablStatAwaySL04.Text = thisMatch.Stat(4).AwayNum
            lablStatAwaySL05.Text = thisMatch.Stat(5).AwayNum
            lablStatAwaySL06.Text = thisMatch.Stat(6).AwayNum
            lablStatAwaySL07.Text = thisMatch.Stat(7).AwayNum
            lablStatAwaySL08.Text = thisMatch.Stat(8).AwayNum
            lablStatAwaySL09.Text = thisMatch.Stat(9).AwayNum
            lablStatAwaySL10.Text = thisMatch.Stat(10).AwayNum
            lablStatAwaySL11.Text = thisMatch.Stat(11).AwayNum
            lablStatAwaySL12.Text = thisMatch.Stat(12).AwayNum
            lablStatAwaySL13.Text = thisMatch.Stat(13).AwayNum
            lablStatAwaySL14.Text = thisMatch.Stat(14).AwayNum
            lablStatAwaySL15.Text = thisMatch.Stat(15).AwayNum
            lablStatAwaySL16.Text = thisMatch.Stat(16).AwayNum
            lablStatAwaySL17.Text = thisMatch.Stat(17).AwayNum

            lablHomeScorersSL.Text = thisMatch.HomeScorers.Replace("^", vbLf)
            lablAwayScorersSL.Text = thisMatch.AwayScorers.Replace("^", vbLf)
            lablHomeCarriesSL.Text = thisMatch.HomeCarries.Replace("^", vbLf)
            lablAwayCarriesSL.Text = thisMatch.AwayCarries.Replace("^", vbLf)
            lablHomeMetresSL.Text = thisMatch.HomeMetresMade.Replace("^", vbLf)
            lablAwayMetresSL.Text = thisMatch.AwayMetresMade.Replace("^", vbLf)
            lablHomeTacklesSL.Text = thisMatch.HomeTackles.Replace("^", vbLf)
            lablAwayTacklesSL.Text = thisMatch.AwayTackles.Replace("^", vbLf)

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
            lablTimeInPlayTotal.Text = thisMatch.TimeInPlayTextTotal

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
        Me.WebBrowser1.Left = Left
        Me.WebBrowser1.Width = Me.Width - (offset + (Left * 2))
        Me.WebBrowser1.Top = Top
        Me.WebBrowser1.Height = 4000
        My.Settings.Left = Left
        My.Settings.Top = Top
        My.Settings.Save()
    End Sub

    Private Sub UpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UpToolStripMenuItem.Click
        Top -= 2
        RepositionBrowser()
    End Sub

    Private Sub DownToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DownToolStripMenuItem.Click
        Top += 2
        RepositionBrowser()
    End Sub

    Private Sub LeftToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LeftToolStripMenuItem.Click
        Left -= 2
        RepositionBrowser()
    End Sub

    Private Sub RightToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RightToolStripMenuItem.Click
        Left += 2
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

    Private Sub panelStar6_Paint(sender As Object, e As PaintEventArgs) Handles panelStar6.Paint
    End Sub

    Private Sub panelStar6_Click(sender As Object, e As EventArgs) Handles panelStar6.Click
        Me.Close()
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Me.Close()
    End Sub

    Private Sub panelSL_Paint(sender As Object, e As PaintEventArgs) Handles panelSL.Paint

    End Sub
    Sub TestLoadImage(ImageName As String)
        PictureBoxTest.Load(Path.Combine(Config.AdImagesPath, ImageName))
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        TestLoadImage("Emirates Skywards 175.png")
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        TestLoadImage("164621674.jpg")
    End Sub
    Sub TestLoadPage(filename As String)
        Try
            WebBrowser2.Navigate(Path.Combine("file:///", Config.WebPagesPath, filename))
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        TestLoadPage("betting.htm")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        TestLoadPage("iisstart.htm")
    End Sub

    'Private Sub picBoxRaceBGD_Paint(sender As Object, e As PaintEventArgs) Handles picBoxRaceBGD.Paint
    '    Dim myFont As Font = New Font("Arial", 22)
    '    e.Graphics.DrawString(CurrentBettingBoard.Heading, myFont, Brushes.White, New Point(2, 25))
    'End Sub

    Public Sub DrawAllBettingBoard()
        ' Create a Graphics object for the Control.
        Try
            Dim g As Graphics = Me.picBoxRaceBGD.CreateGraphics()
            picBoxRaceBGD.Refresh()
            g.SmoothingMode = SmoothingMode.AntiAlias
            RedrawData(g)
            ' Clean up the Graphics object.
            g.Dispose()
        Catch ex As Exception

        End Try
    End Sub
    Sub RedrawData(ByVal g As Graphics)
        Try
            'Dim thisRect As New RectangleF((thisPlot.CoordX * scaleX) - 12, (thisPlot.CoordY * scaleY) - 12, 24, 24)
            'Dim thisBrush As New SolidBrush(Color.White)
            'g.FillEllipse(thisBrush, thisRect)

            Dim fontName As Font = New Font("Eurostile", 26, FontStyle.Bold)
            Dim fontTime As Font = New Font("Eurostile", 32, FontStyle.Bold)
            Dim fontChanges As Font = New Font("Eurostile", 28, FontStyle.Bold)
            Dim fontBetting As Font = New Font("Eurostile", 22, FontStyle.Bold)

            Dim PenBettingLine As Pen = New Pen(Brushes.DarkGreen, 4)

            Dim StringFormatTime As StringFormat = New StringFormat()
            StringFormatTime.Alignment = StringAlignment.Center
            StringFormatTime.LineAlignment = StringAlignment.Near
            Dim StringFormatName As StringFormat = New StringFormat()
            StringFormatName.Alignment = StringAlignment.Center
            StringFormatName.LineAlignment = StringAlignment.Near

            Dim StringFormatCloth As StringFormat = New StringFormat()
            StringFormatCloth.Alignment = StringAlignment.Far
            StringFormatCloth.LineAlignment = StringAlignment.Center
            Dim StringFormatRunnerName As StringFormat = New StringFormat()
            StringFormatRunnerName.Alignment = StringAlignment.Near
            StringFormatRunnerName.LineAlignment = StringAlignment.Center
            Dim StringFormatOdds As StringFormat = New StringFormat()
            StringFormatOdds.Alignment = StringAlignment.Center
            StringFormatOdds.LineAlignment = StringAlignment.Center

            Dim rectCourseTime As Rectangle = New Rectangle(400, 60, 1120, 100)
            Dim rectRaceName As Rectangle = New Rectangle(400, 120, 1120, 200)
            Dim rectBetting As Rectangle = New Rectangle(100, 300, 1500, 500)
            Dim courseLogo As String = Path.Combine(Config.CourseImagesPath, CurrentBettingBoard.CourseLogo)
            Dim sponsorLogo As String = Path.Combine(Config.SponsorImagesPath, CurrentBettingBoard.SponsorLogo)

            If File.Exists(courseLogo) Then
                Try
                    Dim courseImage As Image = Image.FromFile(courseLogo)
                    'g.DrawImage(courseImage, 100, 100, 300, 100)
                    g.DrawImageUnscaled(courseImage, 80, 40)
                Catch ex As Exception

                End Try
            End If

            'headings:
            g.DrawString(CurrentBettingBoard.CourseTime.ToUpper, fontTime, Brushes.White, rectCourseTime, StringFormatTime)
            g.DrawString(CurrentBettingBoard.RaceName, fontName, Brushes.White, rectRaceName, StringFormatName)


            'bottom bar:
            Dim rectChanges As Rectangle = New Rectangle(240, 822, 1440, 180)
            Dim Penchanges As Pen = New Pen(Brushes.Silver, 4)
            Penchanges.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel

            Dim colorChangesBox As Color = Color.FromArgb(80, Color.Black)
            Dim brushChanges = New SolidBrush(colorChangesBox)

            g.FillRectangle(brushChanges, rectChanges)
            g.DrawRectangle(Penchanges, rectChanges)

            If File.Exists(sponsorLogo) Then
                Try
                    Dim sponsorImage As Image = Image.FromFile(sponsorLogo)
                    g.DrawImage(sponsorImage, 1424, 862, 200, 100)
                    'Dim imageHeight As Integer = sponsorImage.Height
                    'Dim Y As Integer = 900 - (imageHeight / 2)
                    'Console.WriteLine(Y.ToString)
                    'g.DrawImageUnscaled(sponsorImage, 1380, Y)
                Catch ex As Exception

                End Try
            End If
            'CurrentBettingBoard.Changes1Text = "Line One"
            'CurrentBettingBoard.Changes2Text = " "
            'CurrentBettingBoard.Changes3Text = "Line Three"
            'CurrentBettingBoard.Changes4Text = "Line Four which is quite long considering the weather what happens..."

            Dim changesBaseline As Integer = 830
            Dim changesBaselineIncrement As Integer = 40
            Select Case CurrentBettingBoard.ChangesCount
                Case 1
                    changesBaseline = 880
                Case 2
                    changesBaseline = 860
                    changesBaselineIncrement = 50
                Case 3
                    changesBaseline = 840
                    changesBaselineIncrement = 50
                Case Else
                    changesBaseline = 830
            End Select
            If CurrentBettingBoard.Changes1Text.Trim <> "" Then
                g.DrawString(CurrentBettingBoard.Changes1Text, fontChanges, Brushes.White, 280, changesBaseline)
                changesBaseline += changesBaselineIncrement
            End If
            If CurrentBettingBoard.Changes2Text.Trim <> "" Then
                g.DrawString(CurrentBettingBoard.Changes2Text, fontChanges, Brushes.White, 280, changesBaseline)
                changesBaseline += changesBaselineIncrement
            End If
            If CurrentBettingBoard.Changes3Text.Trim <> "" Then
                g.DrawString(CurrentBettingBoard.Changes3Text, fontChanges, Brushes.White, 280, changesBaseline)
                changesBaseline += changesBaselineIncrement
            End If
            If CurrentBettingBoard.Changes4Text.Trim <> "" Then
                g.DrawString(CurrentBettingBoard.Changes4Text, fontChanges, Brushes.White, 280, changesBaseline)
                changesBaseline += changesBaselineIncrement
            End If


            'betting:
            Dim baseline As Integer = 250
            Dim columnCloth As Integer = 220
            Dim columnName As Integer = 320
            Dim column1 As Integer = 750
            Dim column2 As Integer = 900
            Dim column3 As Integer = 1050
            Dim column4 As Integer = 1200
            Dim column5 As Integer = 1350
            Dim columnTote As Integer = 1500

            Dim columnClothA As Integer = 100
            Dim columnNameA As Integer = 200
            Dim column5A As Integer = 560
            Dim columnToteA As Integer = 700
            Dim lineAStart As Integer = 140
            Dim lineAEnd As Integer = 844

            Dim columnClothB As Integer = 1000
            Dim columnNameB As Integer = 1100
            Dim column5B As Integer = 1460
            Dim columnToteB As Integer = 1600
            Dim lineBStart As Integer = 1040
            Dim lineBEnd As Integer = 1744

            Dim ClothWidth As Integer = 80
            Dim RunnerNameWidth As Integer = 440
            Dim bettingOddsWidth As Integer = 150
            Dim lineStart As Integer = 260
            Dim lineEnd As Integer = 1650
            Dim rowheight As Integer = 40
            Dim twinColumn As Boolean = False
            Dim rowSplit As Integer = 0
            Select Case CurrentBettingBoard.BettingRowList.Count
                Case < 10
                    'spacing
                    rowheight = 50
                    baseline = 300
                Case 11 To 13
                    'default
                Case 14 To 16
                    rowheight = 34
                    fontBetting = New Font("Eurostile", 20, FontStyle.Bold)
                Case 17 To 18
                    baseline = 240
                    rowheight = 32
                    fontBetting = New Font("Eurostile", 18, FontStyle.Bold)
                'Case 19 To 20
                '    rowheight = 30
                '    fontBetting = New Font("Eurostile", 18, FontStyle.Bold)
                Case > 18
                    '2 columns
                    fontBetting = New Font("Eurostile", 20, FontStyle.Bold)
                    twinColumn = True
                    rowSplit = (CurrentBettingBoard.BettingRowList.Count / 2) + 1
                Case Else
                    'OK for default
            End Select

            If twinColumn Then
                Dim rowCount As Integer = 0
                Dim baselineOrigional As Integer = baseline
                g.DrawImage(My.Resources.Tote_Win, columnToteA + 50, baseline - 46, 100, 50)
                g.DrawImage(My.Resources.Tote_Win, columnToteB + 50, baseline - 46, 100, 50)
                For Each thisRow As clsBettingBoardRow In CurrentBettingBoard.BettingRowList
                    rowCount += 1
                    If rowCount = rowSplit Then
                        baseline = baselineOrigional  'down column B
                    End If
                    If rowCount < rowSplit Then
                        'first column
                        g.DrawString(thisRow.Cloth, fontBetting, Brushes.White, New Rectangle(columnClothA, baseline, ClothWidth, rowheight), StringFormatCloth)
                        g.DrawString(thisRow.Name.ToUpper, fontBetting, Brushes.White, New Rectangle(columnNameA, baseline, RunnerNameWidth, rowheight), StringFormatRunnerName)
                        g.DrawString(thisRow.Odds, fontBetting, Brushes.LightGreen, New Rectangle(column5A, baseline, bettingOddsWidth, rowheight), StringFormatOdds)
                        g.DrawString(thisRow.ToteWin, fontBetting, Brushes.White, New Rectangle(columnToteA, baseline, bettingOddsWidth, rowheight), StringFormatCloth)
                        baseline += rowheight
                        g.DrawLine(PenBettingLine, lineAStart, baseline - 2, lineAEnd, baseline - 2)    'under row
                    Else
                        'second column
                        g.DrawString(thisRow.Cloth, fontBetting, Brushes.White, New Rectangle(columnClothB, baseline, ClothWidth, rowheight), StringFormatCloth)
                        g.DrawString(thisRow.Name.ToUpper, fontBetting, Brushes.White, New Rectangle(columnNameB, baseline, RunnerNameWidth, rowheight), StringFormatRunnerName)
                        g.DrawString(thisRow.Odds, fontBetting, Brushes.LightGreen, New Rectangle(column5B, baseline, bettingOddsWidth, rowheight), StringFormatOdds)
                        g.DrawString(thisRow.ToteWin, fontBetting, Brushes.White, New Rectangle(columnToteB, baseline, bettingOddsWidth, rowheight), StringFormatCloth)
                        baseline += rowheight
                        g.DrawLine(PenBettingLine, lineBStart, baseline - 2, lineBEnd, baseline - 2)    'under row
                    End If
                Next
            Else
                g.DrawImage(My.Resources.Tote_Win, columnTote + 40, baseline - 60, 120, 60)
                For Each thisRow As clsBettingBoardRow In CurrentBettingBoard.BettingRowList
                    g.DrawString(thisRow.Cloth, fontBetting, Brushes.White, New Rectangle(columnCloth, baseline, ClothWidth, rowheight), StringFormatCloth)
                    g.DrawString(thisRow.Name.ToUpper, fontBetting, Brushes.White, New Rectangle(columnName, baseline, RunnerNameWidth, rowheight), StringFormatRunnerName)
                    g.DrawString(thisRow.Odds1, fontBetting, Brushes.White, New Rectangle(column1, baseline, bettingOddsWidth, rowheight), StringFormatOdds)
                    g.DrawString(thisRow.Odds2, fontBetting, Brushes.White, New Rectangle(column2, baseline, bettingOddsWidth, rowheight), StringFormatOdds)
                    g.DrawString(thisRow.Odds3, fontBetting, Brushes.White, New Rectangle(column3, baseline, bettingOddsWidth, rowheight), StringFormatOdds)
                    g.DrawString(thisRow.Odds4, fontBetting, Brushes.White, New Rectangle(column4, baseline, bettingOddsWidth, rowheight), StringFormatOdds)
                    g.DrawString(thisRow.Odds, fontBetting, Brushes.LightGreen, New Rectangle(column5, baseline, bettingOddsWidth, rowheight), StringFormatOdds)
                    g.DrawString(thisRow.ToteWin, fontBetting, Brushes.White, New Rectangle(columnTote, baseline, bettingOddsWidth, rowheight), StringFormatCloth)
                    baseline += rowheight
                    g.DrawLine(PenBettingLine, lineStart, baseline - 2, lineEnd, baseline - 2)    'under row
                Next
            End If



            'tidy:
            fontBetting.Dispose()
            fontChanges.Dispose()
            fontName.Dispose()
            fontTime.Dispose()

            Penchanges.Dispose()
            PenBettingLine.Dispose()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub MenuStrip1_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles MenuStrip1.ItemClicked

    End Sub
    'Sub DrawEventPlot(ByVal g As Graphics, ByVal thisPlot As clsOptaEvent)
    '    Try
    '        Dim thisRect As New RectangleF((thisPlot.CoordX * scaleX) - 12, (thisPlot.CoordY * scaleY) - 12, 24, 24)
    '        Dim thisBrush As New SolidBrush(Color.Red)
    '        g.FillEllipse(thisBrush, thisRect)
    '    Catch ex As Exception

    '    End Try
    'End Sub
    'Sub DrawFormationPlot(ByVal g As Graphics, ByVal thisPlot As clsOptaPlayerData)
    '    Try
    '        Dim thisRect As New RectangleF((thisPlot.CoordX * scaleX) - 12, (thisPlot.CoordY * scaleY) - 12, 24, 24)
    '        Dim thisBrush As New SolidBrush(Color.Blue)
    '        If thisPlot.TeamIndex = 2 Then
    '            'away
    '            thisBrush.Color = Color.Red
    '        End If
    '        g.FillEllipse(thisBrush, thisRect)

    '        'g.DrawString(thisPlot.ShirtNumber + " (" + thisPlot.CoordX.ToString + ", " + thisPlot.CoordY.ToString + ")", Me.Font, Brushes.White, thisRect.Left + 4, thisRect.Bottom - 16)
    '        g.DrawString(thisPlot.ShirtNumber, Me.Font, Brushes.White, thisRect.Left + 4, thisRect.Bottom - 16)
    '    Catch ex As Exception

    '    End Try
    'End Sub

End Class
