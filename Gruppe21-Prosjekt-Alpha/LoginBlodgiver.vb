Option Strict On
Option Explicit On
Option Infer Off

Imports AudiopoLib

Public Class LoginBlodgiver
    Private LayoutHelper As New FormLayoutTools(Me)
    Private logo As HemoGlobeLogo
    Private LoginBox As loginForm
    Private Sub LoginBlodgiver_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Size = New Size(1280, 720)
        logo = New HemoGlobeLogo
        logo.Parent = Me
        LoginBox = New loginForm
        LoginBox.Parent = Me
        LayoutHelper.CenterOnForm(LoginBox)
    End Sub
    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        If LoginBox IsNot Nothing Then
            LayoutHelper.CenterOnForm(LoginBox)
        End If
    End Sub
End Class

Public Class HemoGlobeLogo
    Inherits Control
    Public Sub New()
        BackgroundImage = My.Resources.Logo_Blodbank
        Dim hoy As Integer = (BackgroundImage.Height \ 2)
        Width = 175
        Height = 54
        Location = New Point(10, 10)
        BackgroundImageLayout = ImageLayout.Zoom
    End Sub
End Class

Public Class loginForm
    Inherits Panel
    Dim WithEvents UserLogin As MySqlUserLogin
    Dim LoadingGraphics As LoadingGraphics(Of Control)
    Dim WithEvents FWButton, BliMedButton As FullWidthControl
    Dim PicLoadingSurface As Control
    Dim loginBrukernavnField As New LoginField
    Dim loginPassordField As New LoginField
    Dim LayoutHelper As New FormLayoutTools(Me)
    Dim IsLoaded As Boolean
    Dim Header As New FullWidthControl(Me)

    Public Sub New()

        With Header
            .BackColor = Color.FromArgb(100, 10, 30)
            .TextAlign = ContentAlignment.MiddleLeft
            .ForeColor = Color.FromArgb(255, 255, 255)
            .Text = "Logg inn"
            .Font = New Font(.Font.FontFamily, 12)
        End With
        'GroupLoggInn = New GroupBox

        ' TEMPORARY; TODO: Switch to secure class

        'NotifManager = New NotificationManager(Me)
        'NotifManager.AssignedLayoutManager = LayoutHelper
        BackColor = Color.White
        With loginBrukernavnField
            .Header.Text = "Brukernavn"
            .Header.Font = New Font(.Font.FontFamily, 11)
            .Parent = Me
            .Width = 280

        End With
        With loginPassordField
            .Header.Text = "Passord"
            .Parent = Me
            .Width = 280
            .InnerTextField.Multiline = False
            .InnerTextField.UseSystemPasswordChar = True
            .Header.Font = New Font(.Font.FontFamily, 11)
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
            .Font = New Font(.Font.FontFamily, 12)
            .TabIndex = 4
            .BackColorNormal = Color.FromArgb(120, 20, 40)
            .BackColorSelected = ColorHelper.Multiply(.BackColorNormal, 0.7)
            .Width = 280
            .Height = 50
        End With
        FWButton = New FullWidthControl(Me, True, FullWidthControl.SnapType.Bottom, -BliMedButton.Height)
        With FWButton
            .Text = "Logg inn"
            .Font = New Font(.Font.FontFamily, 12)
            .TabIndex = 3
            .Width = 280
            .Height = 50
            .BackColorNormal = Color.FromArgb(230, 50, 80)
            .BackColorSelected = ColorHelper.Multiply(.BackColorNormal, 0.7)
        End With

        UserLogin = New MySqlUserLogin("mysql.stud.iie.ntnu.no", "g_oops_21", "g_oops_21", "NWRhPBUk")
        With UserLogin
            .IfValid = AddressOf LoginValid
            .IfInvalid = AddressOf LoginInvalid
        End With
        'With LayoutHelper
        '    .CenterSurface(PicLoadingSurface, Me, 0, 10)
        'End With
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
    'Protected Overrides Sub OnResize(e As EventArgs)
    '    MyBase.OnResize(e)
    '    If LayoutHelper IsNot Nothing Then
    '        With LayoutHelper
    '            .CenterSurface(PicLoadingSurface, Me, 0, 10)
    '        End With
    '    End If
    'End Sub
    Private Sub LoginValid()
        Hide()
        LoadingGraphics.StopSpin()
        For Each C As Control In Controls
            C.Show()
        Next
        FWButton.Enabled = True
        BliMedButton.Enabled = True
        loginBrukernavnField.Text = ""
        loginPassordField.Text = ""
    End Sub
    Private Sub LoginInvalid(ByVal ErrorOccurred As Boolean)
        LoadingGraphics.StopSpin()
        SuspendLayout()
        For Each C As Control In Controls
            'If Not C.GetType = GetType(FullWidthControl) Then
            C.Show()
            'End If
        Next
        'FWButton.Show()
        FWButton.Enabled = True
        BliMedButton.Enabled = True
        ResumeLayout()
        loginBrukernavnField.Text = ""
        loginPassordField.Text = ""
        'LayoutHelper.SlideToHeight(40)
        'If ErrorOccurred Then
        '    NotifManager.Display("Tilkoblingen mislyktes. Vennligst prøv igjen senere, og verifiser at du har internettilgang.", NotificationPreset.RedAlert)
        'Else
        '    NotifManager.Display("Brukernavnet eller passordet er feil", NotificationPreset.RedAlert)
        'End If
    End Sub
    Private Sub FWButton_Click(sender As Object, e As EventArgs) Handles FWButton.Click
        SuspendLayout()
        For Each C As Control In Controls
            C.Hide()
        Next
        FWButton.Enabled = False
        BliMedButton.Enabled = False
        UserLogin.LoginAsync(loginBrukernavnField.Text, loginPassordField.Text, "DonorKonti", "bruker_ID", "passord")
        LoadingGraphics.Spin(30, 10)
        ResumeLayout()
    End Sub
    Private Sub BliMedButton_Click(Sender As Object, e As EventArgs) Handles BliMedButton.Click

    End Sub
End Class