Public Class clsConfig
    Public Property UserName As String = ""
    Public Property ServerAddress As String = "LocalHost"
    Public Property ServerPort As Integer = 10540
    Public Property ChannelName As String = "1" 'future control of multi-displays
    Public Property ConfigFilename As String = "Config.ini"
    Public Property CourseImagesPath As String = "C:\Statviewer\Images\Courses"
    Public Property SponsorImagesPath As String = "C:\Statviewer\Images\Sponsors"
    Public Property SilkImagesPath As String = "C:\Statviewer\Images\Silks"
    Public Property AdImagesPath As String = "C:\Statviewer\Images\Ads"
    Public Property WebPagesPath As String = "C:\Statviewer\URL"
    Public Property FileExists As Boolean = False


    Sub ReadSetup()
        Dim strTextLine As String, strFilename As String
        Dim inputFile As System.IO.StreamReader
        Dim TempArray() As String
        Try
            strFilename = _ConfigFilename
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
                            Case "USER", "USERNAME"
                                _UserName = TempArray(1)
                            Case "SERVER", "SERVER IP ADDRESS"
                                _ServerAddress = TempArray(1)
                            Case "SERVERPORT"
                                _ServerPort = Convert.ToInt16(TempArray(1))
                            Case "CHANNEL", "CHANNELNAME"
                                _ChannelName = TempArray(1)
                            Case "COURSEIMAGESFOLDER"
                                _CourseImagesPath = TempArray(1)
                            Case "SPONSORIMAGESFOLDER"
                                _SponsorImagesPath = TempArray(1)
                            Case "SILKIMAGESFOLDER"
                                _SilkImagesPath = TempArray(1)
                            Case "ADIMAGESFOLDER"
                                _AdImagesPath = TempArray(1)
                            Case "WEBPAGESFOLDER", "WEBPAGEFOLDER", "WEBPAGESPATH", "WEBPAGEPATH"
                                _WebPagesPath = TempArray(1)
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
