Imports AudiopoLib

Module Globals
    Public CurrentLogin As UserInfo
    Public Windows As MultiTabWindow
    Public WithEvents HentTimer_DBC As DatabaseClient
    Public TimeListe As New DatabaseTimeListe

    ' ALPHA

    Public Testoversikt As Timeoversikt
    Public Testdashbord As BlodgiverDashboard
    Public Testlogginn As LoggInn_Admin
    Public Testspørreskjema As Skjema
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

    Public TimerHentet As Boolean

    Public Sub Logout()
        ' TODO: Erase all traces of user data
    End Sub

    Private Sub DBC_Finished(Sender As Object, e As DatabaseListEventArgs) Handles HentTimer_DBC.ListLoaded
        If Not TimerHentet Then
            If Not e.ErrorOccurred Then
                For Each R As DataRow In e.Data.Rows
                    Dim NewDate As Date = DirectCast(R.Item(1), Date)
                    Dim TimeComponent As TimeSpan = DirectCast(R.Item(2), TimeSpan)
                    NewDate = NewDate.Add(TimeComponent)
                    TimeListe.Add(New DatabaseTimeElement(CInt(R.Item(0)), NewDate))
                Next
                DirectCast(Windows.Tab(7), TimebestillingTab).SetAppointment()
                TimerHentet = True
            Else
                ' TODO: Logg bruker ut med feilmelding
                MsgBox("Error: " & e.ErrorMessage)
            End If
        Else
            If Not e.ErrorOccurred Then
                For Each R As DataRow In e.Data.Rows

                Next
            Else
                MsgBox("Error: " & e.ErrorMessage)
            End If
            TimerHentet = False
        End If
    End Sub
    Private Sub DBC_Failed() Handles HentTimer_DBC.ExecutionFailed
        MsgBox("Failed")
        ' TODO: Logg ut bruker med feilmelding
    End Sub
End Module

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

Public Structure UserInfo
    Private Number As String
    Public Sub New(PersonalNumber As String)
        Number = PersonalNumber
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
End Structure
