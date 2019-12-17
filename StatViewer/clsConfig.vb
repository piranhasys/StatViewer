Public Class clsConfig
    Private strUserName As String = ""
    Private strServerAddress As String = "LocalHost"
    Private iServerPort As Integer = 10540
    Private strLocalIPAddress As String = "Localhost"
    Private strConfigFilename As String = "Config.ini"
    Public Property FileExists As Boolean = False

    Public Property ConfigFilename() As String
        Get
            Return strConfigFilename
        End Get
        Set(ByVal value As String)
            strConfigFilename = value
        End Set
    End Property

    Public Property LocalIPAddress() As String
        Get
            Return strLocalIPAddress
        End Get
        Set(ByVal value As String)
            strLocalIPAddress = value
        End Set
    End Property

    Property ServerPort() As Integer
        Get
            Return iServerPort
        End Get
        Set(ByVal Value As Integer)
            iServerPort = Value
        End Set
    End Property
    Property UserName() As String
        Get
            Return strUserName
        End Get
        Set(ByVal Value As String)
            strUserName = Value
        End Set
    End Property
    Property ServerAddress() As String
        Get
            Return strServerAddress
        End Get
        Set(ByVal Value As String)
            strServerAddress = Value
        End Set
    End Property

    Sub ReadSetup()
        Dim strTextLine As String, strFilename As String
        Dim inputFile As System.IO.StreamReader
        Dim TempArray() As String
        Try
            strFilename = strConfigFilename
            If Not System.IO.File.Exists(strFilename) Then
                'MessageBox.Show("No Config File: " & strConfigFilename & vbLf & "MatchLog cannot continue")
                Console.WriteLine("No Config.ini")
                FileExists = False
                Exit Sub
            Else
                Console.WriteLine("Config.ini exists")
                FileExists = True
                inputFile = System.IO.File.OpenText(strFilename)
                Do
                    strTextLine = inputFile.ReadLine
                    If strTextLine Is Nothing Then
                        'read empty line, avoid next batch of tests on it
                    Else
                        TempArray = strTextLine.Split("|".ToCharArray)
                        Select Case TempArray(0).ToUpper
                            Case "PCIPADDRESS"
                                strLocalIPAddress = TempArray(1)
                            Case "USERNAME"
                                strUserName = TempArray(1)
                            Case "SERVER"
                                strServerAddress = TempArray(1)
                            Case "SERVERPORT"
                                iServerPort = Convert.ToInt16(TempArray(1))
                        End Select
                    End If
                Loop Until strTextLine Is Nothing
                inputFile.Close()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "Error Reading Config.ini")
        End Try
    End Sub

End Class
