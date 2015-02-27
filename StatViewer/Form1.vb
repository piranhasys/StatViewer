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
    Private left As Integer = 0
    Private top As Integer = 0
    Private lastPageName As String = ""

    Private Sub Form1_Activated(sender As Object, e As EventArgs) Handles Me.Activated
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dtLastPAData = Now.AddDays(-1)
        timerCheckConnections.Start()
        fontSize = My.Settings.FontSize
        left = My.Settings.Left
        top = My.Settings.Top
        ' WebBrowser1.ScriptErrorsSuppressed = True
        RepositionBrowser()
        '   ResizeBrowser(fontSize)
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
                        Case "REFRESH"
                            ReloadPage()
                    End Select
            End Select

        Catch ex As Exception
        End Try
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
        If WebBrowser1.IsBusy = False Then
            WebBrowser1.Refresh()
        End If
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
End Class
