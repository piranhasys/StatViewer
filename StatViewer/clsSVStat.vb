Public Class clsSVStat
    Private strName As String = ""
    Private strHomeNum As String = "0"
    Private strAwayNum As String = "0"

    Property Name() As String
        Get
            Return strName
        End Get
        Set(ByVal Value As String)
            strName = Value
        End Set
    End Property
    Property HomeNum() As String
        Get
            Return strHomeNum
        End Get
        Set(ByVal Value As String)
            strHomeNum = Value
        End Set
    End Property

    Property AwayNum() As String
        Get
            Return strAwayNum
        End Get
        Set(ByVal Value As String)
            strAwayNum = Value
        End Set
    End Property
    ReadOnly Property DataString As String
        Get
            Return strName & "^" & strHomeNum & "^" & strAwayNum
        End Get
    End Property
    Sub AssignData(dataString As String)
        If dataString.Contains("^") Then
            Dim split() As String = dataString.Split("^")
            strName = split(0)
            strHomeNum = split(1)
            strAwayNum = split(2)
        End If
    End Sub
End Class
