Imports System.Text.RegularExpressions
Imports AudiopoLib

Module Globals
    Public CurrentLogin As UserInfo
    Public Windows As MultiTabWindow
    Public WithEvents HentTimer_DBC As DatabaseClient
    Public WithEvents HentEgenerklæring_DBC As DatabaseClient
    Public TimeListe As New DatabaseTimeListe
    Public EventManager As New BloodBankEventManager
    ' Regex
    Public RegExEmail As New Regex("([\w-+]+(?:\.[\w-+]+)*@(?:[\w-]+\.)+[a-zA-Z]{2,7})")


    ' ALPHA

    Public Testoversikt As Timeoversikt
    Public Testdashbord As BlodgiverDashboard
    Public Testlogginn As LoggInn_Admin
    'Public Testspørreskjema As Skjema
    Public BlodgiverApning As LoginBlodgiver
    Public FirstTabTest As FirstTab
    Public SecondTabTest As SecondTab
    Public ThirdTabTest As ThirdTab
    Public PersonaliaTest As Personopplysninger

    ' BETA
    Public MainWindow As Main
    Public Credentials As DatabaseCredentials
    Public LoggInnTab As LoggInnNy
    Public Dashboard As DashboardTab
    Public Egenerklæring As EgenerklæringTab
    Public Timebestilling As TimebestillingTab
    Public AnsattLoggInn As AnsattLoggInnTab
    Public OpprettAnsatt As OpprettAnsattTab

    Public TimerHentet As Boolean

    Public Sub Logout()
        ' TODO: Erase all traces of user data
    End Sub

    Private Sub DBC_Finished(Sender As Object, e As DatabaseListEventArgs) Handles HentTimer_DBC.ListLoaded
        If Not e.ErrorOccurred Then
            For Each R As DataRow In e.Data.Rows
                Dim NewDate As Date = DirectCast(R.Item(1), Date)
                Dim TimeComponent As TimeSpan = DirectCast(R.Item(2), TimeSpan)
                NewDate = NewDate.Add(TimeComponent)
                TimeListe.Add(New DatabaseTimeElement(CInt(R.Item(0)), NewDate))
            Next
            DirectCast(Windows.Tab(7), TimebestillingTab).SetAppointment()
            Dim AppointmentTodayID As Integer = -1
            For Each T As DatabaseTimeElement In TimeListe.TimeListe
                If T.DatoOgTid.Date = Date.Now.Date Then
                    AppointmentTodayID = T.TimeID
                End If
            Next
            If AppointmentTodayID >= 0 Then
                With HentEgenerklæring_DBC
                    .SQLQuery = "SELECT godkjent, svar FROM Egenerklæring WHERE time_id = @id;"
                    .Execute({"@id"}, {CStr(AppointmentTodayID)})
                    Dashboard.NotificationList.Spin()
                End With
            Else
                ' TODO: Recommend new appointment 4 months after last one
                Dashboard.NotificationList.DisplayEmptyLabel = True
            End If
        Else
            ' TODO: Logg bruker ut med feilmelding
            MsgBox("Error: " & e.ErrorMessage)
        End If
    End Sub
    Private Sub DBC_Failed() Handles HentTimer_DBC.ExecutionFailed
        MsgBox("Failed")
        ' TODO: Logg ut bruker med feilmelding
    End Sub
    Private Sub Egenerklæring_Hentet(Sender As Object, e As DatabaseListEventArgs) Handles HentEgenerklæring_DBC.ListLoaded
        With e.Data.Rows
            If .Count > 0 Then
                CurrentLogin.FormSent = True
                With .Item(0)
                    CurrentLogin.FormAccepted = DirectCast(.Item(0), Boolean)
                    If Not IsDBNull(.Item(1)) Then
                        CurrentLogin.FormAnswer = DirectCast(.Item(1), String)
                        Dashboard.NotificationList.AddNotification("Du har fått svar på din egenerklæring. Klikk her for mer informasjon.", 0, AddressOf NotificationClick, Color.LimeGreen)
                    Else
                        CurrentLogin.FormAnswer = Nothing
                        Dashboard.NotificationList.AddNotification("Vi behandler din egenerklæring.", 1, AddressOf NotificationClick, Color.FromArgb(0, 99, 157))
                        Dashboard.NotificationList.AddNotification("Vi behandler din egenerklæring.", 3, AddressOf NotificationClick, Color.FromArgb(0, 99, 157))
                        Dashboard.NotificationList.AddNotification("Vi behandler din egenerklæring.", 4, AddressOf NotificationClick, Color.FromArgb(0, 99, 157))
                    End If
                End With
            Else
                Dashboard.NotificationList.AddNotification("Du har ikke sendt egenerklæring for dagens time. Klikk her for å gå til skjemaet.", 2, AddressOf NotificationClick, Color.Red)
                CurrentLogin.FormSent = False
            End If
        End With
    End Sub
    Public Sub NotificationClick(Sender As Object, e As UserNotificationEventArgs)
        Select Case DirectCast(e.ID, Integer)
            Case 0
                MsgBox("Godkjent: " & CurrentLogin.FormAccepted.ToString & ", begrunnelse: " & CurrentLogin.FormAnswer)
            Case 1
                Beep()
            Case 2
                Windows.Index = 6
        End Select
    End Sub
End Module

Public Class BloodBankEventManager
    Public Event LoggedIn()
    Public Event LoggedOut()

    Public Sub New()

    End Sub
End Class

Public Class DatabaseTimeListe
    Private varTimer As New List(Of DatabaseTimeElement)
    Public Sub New()

    End Sub
    Public ReadOnly Property Count As Integer
        Get
            Return varTimer.Count
        End Get
    End Property
    Public Sub Clear()
        varTimer.Clear()
    End Sub
    Public Sub Add(ByRef Element As DatabaseTimeElement)
        varTimer.Add(Element)
    End Sub
    Public ReadOnly Property Time(ByVal Index As Integer) As DatabaseTimeElement
        Get
            Return varTimer(Index)
        End Get
    End Property
    Public ReadOnly Property TimeListe As List(Of DatabaseTimeElement)
        Get
            Return varTimer
        End Get
    End Property
End Class

Public Class DatabaseTimeElement
    Private varTimeID As Integer
    Private varDatoOgTid As Date
    Public ReadOnly Property TimeID As Integer
        Get
            Return varTimeID
        End Get
    End Property
    Public ReadOnly Property DatoOgTid As Date
        Get
            Return varDatoOgTid
        End Get
    End Property
    Public Sub New(ByVal TimeID As Integer, ByVal DatoOgTid As Date)
        varTimeID = TimeID
        varDatoOgTid = DatoOgTid
    End Sub
End Class

Public Class UserInfo
    Private Number, varFirstName, varLastName, varFormAnswer As String
    Private varFormSent, varFormAccepted As Boolean
    Public Property FormSent As Boolean
        Get
            Return varFormSent
        End Get
        Set(value As Boolean)
            varFormSent = value
        End Set
    End Property
    Public Property FormAccepted As Boolean
        Get
            Return varFormAccepted
        End Get
        Set(value As Boolean)
            varFormAccepted = value
        End Set
    End Property
    Public Property FormAnswer As String
        Get
            Return varFormAnswer
        End Get
        Set(value As String)
            varFormAnswer = value
        End Set
    End Property
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
