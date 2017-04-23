Imports System.Text.RegularExpressions
Imports AudiopoLib

Module Globals
    Public CurrentLogin As UserInfo
    Public CurrentStaff As StaffInfo

    Public Windows As MultiTabWindow

    Public TimeListe As New StaffTimeliste

    ' Regex
    Public RegExEmail As New Regex("([\w-+]+(?:\.[\w-+]+)*@(?:[\w-]+\.)+[a-zA-Z]{2,7})")


    ' ALPHA

    'Public Testlogginn As LoggInn_Admin

    'Public Testspørreskjema As Skjema
    'Public BlodgiverApning As LoginBlodgiver

    Public PersonaliaTest As Personopplysninger

    ' BETA
    Public MainWindow As Main
    Public LoggInnTab As LoggInnNy
    Public Dashboard As DashboardTab
    Public Egenerklæring As EgenerklæringTab
    Public Timebestilling As TimebestillingTab
    Public AnsattLoggInn As AnsattLoggInnTab
    Public OpprettAnsatt As OpprettAnsattTab
    Public Credentials As DatabaseCredentials
    Public AnsattDashboard As AnsattDashboardTab
    Public TimerHentet As Boolean


    Public Sub Logout(Optional ByVal StaffLogout As Boolean = False)
        TimeListe.Clear()
        TimerHentet = False
        Windows.ResetAll()
        If Not StaffLogout Then
            Windows.Index = 1
        Else
            Windows.Index = 5
        End If
        ' TODO: Erase all traces of user data
    End Sub


    Public Sub CloseNotification(Sender As UserNotification, e As UserNotificationEventArgs)
        Sender.close
    End Sub
    Public Sub FormAnswered(Sender As UserNotification, e As UserNotificationEventArgs)
        MsgBox("Godkjent: " & CurrentLogin.FormInfo.Godkjent.ToString & ", begrunnelse: " & CurrentLogin.FormInfo.AnsattSvar)
    End Sub
    Public Sub FormNotSent(Sender As UserNotification, e As UserNotificationEventArgs)
        Egenerklæring.SelectTime(Sender, e)
    End Sub
End Module

'Public Class DatabaseTimeListe
'    Private varTimer As New List(Of StaffTimeliste.StaffTime)
'    Public Sub New()

'    End Sub
'    Public ReadOnly Property Count As Integer
'        Get
'            Return varTimer.Count
'        End Get
'    End Property
'    Public Sub Clear()
'        varTimer.Clear()
'    End Sub
'    Public Sub Add(ByRef Element As DatabaseTimeElement)
'        varTimer.Add(Element)
'    End Sub
'    Public ReadOnly Property Time(ByVal Index As Integer) As DatabaseTimeElement
'        Get
'            Return varTimer(Index)
'        End Get
'    End Property
'    Public ReadOnly Property TimeListe As List(Of StaffTimeliste.StaffTime)
'        Get
'            Return varTimer
'        End Get
'    End Property
'End Class

'Public Class DatabaseTimeElement
'    Private varTimeID As Integer
'    Private varDatoOgTid As Date
'    Public ReadOnly Property TimeID As Integer
'        Get
'            Return varTimeID
'        End Get
'    End Property
'    Public ReadOnly Property DatoOgTid As Date
'        Get
'            Return varDatoOgTid
'        End Get
'    End Property
'    Public Sub New(ByVal TimeID As Integer, ByVal DatoOgTid As Date)
'        varTimeID = TimeID
'        varDatoOgTid = DatoOgTid
'    End Sub
'End Class

Public Class StaffInfo
    Private varID As Integer
    Private varUsername, varFirstName, varLastName As String
    Public Property ID As Integer
        Get
            Return varID
        End Get
        Set(value As Integer)
            varID = value
        End Set
    End Property
    Public Property Username As String
        Get
            Return varUsername
        End Get
        Set(value As String)
            varUsername = value
        End Set
    End Property
    Public Property FirstName As String
        Get
            Return varFirstName
        End Get
        Set(value As String)
            varFirstName = value
        End Set
    End Property
    Public Property LastName As String
        Get
            Return varLastName
        End Get
        Set(value As String)
            varLastName = value
        End Set
    End Property
    Public ReadOnly Property FullName As String
        Get
            Return varFirstName & " " & varLastName
        End Get
    End Property
    Public Sub New(Username As String)
        varUsername = Username
        ID = -1
        varFirstName = "Undefined"
        varLastName = "Undefined"
    End Sub
    Public Sub EraseInfo()
        varID = Nothing
        varUsername = Nothing
        varFirstName = Nothing
        varLastName = Nothing
    End Sub
End Class

Public Class UserInfo
    Private Number, varFirstName, varLastName As String
    Private varFormInfo As Egenerklæringsliste.Egenerklæring
    Public Property FormInfo As Egenerklæringsliste.Egenerklæring
        Get
            Return varFormInfo
        End Get
        Set(value As Egenerklæringsliste.Egenerklæring)
            varFormInfo = value
        End Set
    End Property
    Public Sub EraseInfo()
        Number = Nothing
        varFirstName = Nothing
        varLastName = Nothing
    End Sub
    Public ReadOnly Property FullName As String
        Get
            Return varFirstName & " " & varLastName
        End Get
    End Property
    Public Property FirstName As String
        Get
            Return varFirstName
        End Get
        Set(value As String)
            varFirstName = value
        End Set
    End Property
    Public Property LastName As String
        Get
            Return varLastName
        End Get
        Set(value As String)
            varLastName = value
        End Set
    End Property
    Public Sub New(PersonalNumber As String)
        Number = PersonalNumber
        varFirstName = "Undefined"
        varLastName = "Undefined"
    End Sub
    Public Property PersonalNumber As String
        Get
            Return Number
        End Get
        Set(value As String)
            Number = value
        End Set
    End Property
    Public Function IsMale() As Boolean
        Dim Chars() As Char = Number.ToCharArray
        Return (Convert.ToInt32(Chars(8)) Mod 2 = 1)
    End Function
End Class
