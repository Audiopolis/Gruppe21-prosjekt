Module Globals
    Public CurrentLogin As UserInfo

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

    Public Sub Logout()
        ' TODO: Erase all traces of user data
    End Sub

End Module

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
