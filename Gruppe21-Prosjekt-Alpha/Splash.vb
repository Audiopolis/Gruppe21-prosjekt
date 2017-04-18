
Imports System.Drawing.Drawing2D
Imports System.Timers
Imports System.Threading
Imports AudiopoLib
Imports System.ComponentModel
Public NotInheritable Class Splash
    Dim GB As LinearGradientBrush
    Private SC As SynchronizationContext = SynchronizationContext.Current
    Private WithEvents DelayTimer As New Timers.Timer(1000)
    'TODO: This form can easily be set as the splash screen for the application by going to the "Application" tab
    '  of the Project Designer ("Properties" under the "Project" menu).
    Protected Overrides Sub OnPaintBackground(e As PaintEventArgs)
        MyBase.OnPaintBackground(e)
        e.Graphics.FillRectangle(GB, ClientRectangle)
    End Sub
    Private Sub InitializeStuff(State As Object)
        DelayTimer.Dispose()

        BlodgiverApning = New LoginBlodgiver




        'Testoversikt = New Timeoversikt
        'Testdashbord = New BlodgiverDashboard
        'Testlogginn = New LoggInn_Admin
        'Testspørreskjema = New Skjema

        'blodgiverDashboard2.Show()
        'Testlogginn.Show()
        MainWindow = New Main
        MainWindow.Show()

        Hide()
        'BlodgiverApning.Show()

        GB.Dispose()
        Close()

        'TODO: Dispose Splash on end

        'Dispose()
    End Sub

    Private Sub DelayTimer_Elapsed(Sender As Object, e As ElapsedEventArgs) Handles DelayTimer.Elapsed
        SC.Post(AddressOf InitializeStuff, Nothing)
    End Sub
    Private Sub Splash_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Hide()
        DoubleBuffered = True
        GB = New LinearGradientBrush(ClientRectangle, Color.FromArgb(120, Color.LightYellow), Color.FromArgb(0, Color.LightYellow), LinearGradientMode.Vertical)
        DelayTimer.AutoReset = False
        Version.Text = String.Format(Version.Text, My.Application.Info.Version.Major, My.Application.Info.Version.Minor)
        Copyright.TextAlign = ContentAlignment.MiddleCenter
        Copyright.Text = ("This application is subject to international copyright laws. " & Chr(169) & " 2017 Magnus Bakke, Andreas Ore Larssen, Ahsan Azim, Eskil Uhlving Larsen; AudiopoLib " & Chr(169) & " 2017 Magnus Bakke")
        Show()
        DelayTimer.Start()
    End Sub


End Class
