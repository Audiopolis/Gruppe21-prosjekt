Option Strict On
Option Explicit On
Option Infer Off

Imports AudiopoLib

Public Class LoginBlodgiver
    Private LayoutHelper As New FormLayoutTools(Me)
    Private NotifManager As New NotificationManager(Me)
    Private logo As HemoGlobeLogo
    Private WithEvents LoginBox As LoginForm
    Private Sub LoginBlodgiver_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Size = New Size(1280, 720)
        logo = New HemoGlobeLogo
        logo.Parent = Me
        LoginBox = New LoginForm
        LoginBox.Parent = Me
        LayoutHelper.CenterOnForm(LoginBox)
    End Sub
    Protected Overrides Sub OnClosed(e As EventArgs)
        MyBase.OnClosed(e)
        End
    End Sub
    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        If LoginBox IsNot Nothing Then
            LayoutHelper.CenterOnForm(LoginBox)
        End If
    End Sub
    Private Sub LoginFinished(Sender As Object, Success As Boolean) Handles LoginBox.CheckFinished
        If Success Then
            MsgBox("Success")
        Else
            NotifManager.Display("Fødselsnummeret eller passordet er feil.", NotificationPreset.RedAlert)
        End If
    End Sub
End Class

Public Class HemoGlobeLogo
    Inherits Control
    Public Sub New()
        BackgroundImage = My.Resources.NyLogo
        Size = BackgroundImage.Size
        Location = New Point(0, 10)
        BackgroundImageLayout = ImageLayout.Zoom
    End Sub
End Class

Public Class LoginForm
    Inherits Panel
    Dim WithEvents UserLogin As MySqlUserLogin
    Dim LoadingGraphics As LoadingGraphics(Of Control)
    Dim WithEvents FWButton, BliMedButton As FullWidthControl
    Dim PicLoadingSurface As Control
    Dim loginBrukernavnField, loginPassordField As New LoginField
    Dim LayoutHelper As New FormLayoutTools(Me)
    Dim IsLoaded As Boolean
    Dim Header As New FullWidthControl(Me)
    Public Event CheckFinished(Sender As Object, Success As Boolean)
    Public Event BliMedClicked(Sender As Object, e As EventArgs)
    Public Sub New()
        With Header
            .BackColor = Color.FromArgb(100, 10, 30)
            .TextAlign = ContentAlignment.MiddleLeft
            .ForeColor = Color.FromArgb(255, 255, 255)
            .Text = "Logg inn"
            .Padding = New Padding(10, 0, 0, 0)
            .Font = New Font(.Font.FontFamily, 10)
        End With
        BackColor = Color.White
        With loginBrukernavnField
            .Header.Text = "Brukernavn"
            .Header.Font = New Font(.Font.FontFamily, 10)
            .Parent = Me
            .Width = 280
        End With
        With loginPassordField
            .Header.Text = "Passord"
            .Parent = Me
            .Width = 280
            .InnerTextField.Multiline = False
            .InnerTextField.UseSystemPasswordChar = True
            .Header.Font = New Font(.Font.FontFamily, 10)
        End With
        PicLoadingSurface = New PictureBox
        With PicLoadingSurface
            .Hide()
            .Size = New Size(100, 100)
            .Parent = Me
        End With
        LoadingGraphics = New LoadingGraphics(Of Control)(PicLoadingSurface)
        With LoadingGraphics
            .Stroke = 6
            .Pen.Color = Color.FromArgb(230, 50, 80)
        End With
        BliMedButton = New FullWidthControl(Me, True, FullWidthControl.SnapType.Bottom)
        With BliMedButton
            .Text = "Opprett bruker"
            .Font = New Font(.Font.FontFamily, 10)
            .TabIndex = 4
            .BackColorNormal = Color.FromArgb(120, 20, 40)
            .BackColorSelected = ColorHelper.Multiply(.BackColorNormal, 0.7)
            .Size = New Size(280, 50)
        End With
        FWButton = New FullWidthControl(Me, True, FullWidthControl.SnapType.Bottom, -BliMedButton.Height)
        With FWButton
            .Text = "Logg inn"
            .Font = New Font(.Font.FontFamily, 10)
            .TabIndex = 3
            .Size = New Size(280, 50)
            .BackColorNormal = Color.FromArgb(230, 50, 80)
            .BackColorSelected = ColorHelper.Multiply(.BackColorNormal, 0.7)
        End With
        With Credentials
            UserLogin = New MySqlUserLogin(.Server, .Database, .UserID, .Password)
        End With
        With UserLogin
            .IfValid = AddressOf LoginValid
            .IfInvalid = AddressOf LoginInvalid
        End With
        Size = New Size(400, 500)
        Show()
        IsLoaded = True
    End Sub
    Protected Overrides Sub OnResize(eventargs As EventArgs)
        MyBase.OnResize(eventargs)
        If loginPassordField IsNot Nothing Then
            With LayoutHelper
                .CenterOnForm(loginBrukernavnField,, -105)
                .CenterOnForm(loginPassordField,, -25)
                .CenterOnForm(FWButton,, 115)
                .CenterOnForm(BliMedButton,, 175)
                .CenterOnForm(PicLoadingSurface)
            End With
            With Header
                .Width = ClientSize.Width
            End With
        End If
    End Sub
    Private Sub LoginValid()
        Hide()
        LoadingGraphics.StopSpin()
        PicLoadingSurface.SendToBack()
        For Each C As Control In Controls
            C.Show()
        Next
        FWButton.Enabled = True
        BliMedButton.Enabled = True
        loginBrukernavnField.Text = ""
        loginPassordField.Text = ""
        RaiseEvent CheckFinished(Me, True)
    End Sub
    Private Sub LoginInvalid(ByVal ErrorOccurred As Boolean, ErrorMessage As String)
        SuspendLayout()
        LoadingGraphics.StopSpin()
        PicLoadingSurface.SendToBack()
        For Each C As Control In Controls
            C.Show()
        Next
        FWButton.Enabled = True
        BliMedButton.Enabled = True
        loginBrukernavnField.Text = ""
        loginPassordField.Text = ""
        ResumeLayout(True)
        RaiseEvent CheckFinished(Me, False)
    End Sub
    Private Sub FWButton_Click(sender As Object, e As EventArgs) Handles FWButton.Click
        SuspendLayout()
        For Each C As Control In Controls
            C.Hide()
        Next
        FWButton.Enabled = False
        BliMedButton.Enabled = False
        UserLogin.LoginAsync(loginBrukernavnField.Text, loginPassordField.Text, "Brukerkonto", "b_fodselsnr", "passord")
        LoadingGraphics.Spin(30, 10)
        ResumeLayout()
    End Sub
    Private Sub BliMedButton_Click(Sender As Object, e As EventArgs) Handles BliMedButton.Click
        RaiseEvent BliMedClicked(BliMedButton, EventArgs.Empty)
    End Sub
    Protected Overrides Sub Dispose(disposing As Boolean)
        'UserLogin.Dispose
        If disposing Then
            LoadingGraphics.Dispose()
            FWButton.Dispose()
            BliMedButton.Dispose()
            PicLoadingSurface.Dispose()
            loginBrukernavnField.Dispose()
            loginPassordField.Dispose()
            LayoutHelper.Dispose()
            Header.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub
End Class