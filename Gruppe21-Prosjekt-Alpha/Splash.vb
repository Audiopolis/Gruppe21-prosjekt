
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
        MainWindow = New Main

        BlodgiverApning = New LoginBlodgiver
        BlodgiverApning.Show()

        'Testoversikt = New Timeoversikt
        'Testdashbord = New BlodgiverDashboard
        'Testlogginn = New LoggInn_Admin
        'Testspørreskjema = New Skjema

        MainWindow.Show()

        'Testlogginn.Show()

        Hide()
        GB.Dispose()
        Close()
        Dispose()
        'Testlogginn.Show()
    End Sub

    Private Sub DelayTimer_Elapsed(Sender As Object, e As ElapsedEventArgs) Handles DelayTimer.Elapsed
        SC.Post(AddressOf InitializeStuff, Nothing)
    End Sub
    Private Sub Splash_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GB = New LinearGradientBrush(ClientRectangle, Color.FromArgb(120, Color.LightYellow), Color.FromArgb(0, Color.LightYellow), LinearGradientMode.Vertical)
        DelayTimer.AutoReset = False
        'Set up the dialog text at runtime according to the application's assembly information.  
        Timeoversikt.Show()
        'TODO: Customize the application's assembly information in the "Application" pane of the project 
        '  properties dialog (under the "Project" menu).

        'Application title

        'Format the version information using the text set into the Version control at design time as the
        '  formatting string.  This allows for effective localization if desired.
        '  Build and revision information could be included by using the following code and changing the 
        '  Version control's designtime text to "Version {0}.{1:00}.{2}.{3}" or something similar.  See
        '  String.Format() in Help for more information.
        '
        '    Version.Text = System.String.Format(Version.Text, My.Application.Info.Version.Major, My.Application.Info.Version.Minor, My.Application.Info.Version.Build, My.Application.Info.Version.Revision)

        Version.Text = String.Format(Version.Text, My.Application.Info.Version.Major, My.Application.Info.Version.Minor)
        Copyright.TextAlign = ContentAlignment.MiddleCenter
        Copyright.Text = ("This application is subject to international copyright laws. " & Chr(169) & " 2017 Magnus Bakke, Andreas Ore Larssen, Ahsan Azim, Eskil Uhlving Larsen; AudiopoLib " & Chr(169) & " 2017 Magnus Bakke")
        DelayTimer.Start()
    End Sub


End Class
