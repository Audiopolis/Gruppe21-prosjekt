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
    Dim txtBrukernavn, txtPassord As TextBox
    Dim GroupHeader As FullWidthControl
    Dim LabBrukernavn, LabPassord As Label
    Dim TextBoxContainer(1) As Control
    Dim LayoutHelper As New FormLayoutTools(Me)
    'Dim LayoutHelper As New FormLayoutTools(Me)
    Dim IsLoaded As Boolean
    Public Sub New()
        'GroupLoggInn = New GroupBox
        TextBoxContainer(0) = New Control
        TextBoxContainer(1) = New Control
        With TextBoxContainer(0)
            .Size = New Size(270, 40)
            .BackColor = Color.FromArgb(230, 230, 230)
            .Parent = Me
        End With
        With TextBoxContainer(1)
            .Size = New Size(270, 40)
            .BackColor = Color.FromArgb(230, 230, 230)
            .Parent = Me
        End With

        LabBrukernavn = New Label
        LabPassord = New Label
        txtBrukernavn = New TextBox
        txtPassord = New TextBox

        BackColor = Color.White
        With Controls
            .Add(LabBrukernavn)
            .Add(LabPassord)
            .Add(txtBrukernavn)
            .Add(txtPassord)
        End With

        With txtBrukernavn
            .Width = 268
            .TabIndex = 1
            .BackColor = Color.White
            .ForeColor = Color.Gray
            .BringToFront()
            .Multiline = True
            .WordWrap = False
            .Height = 38
            .Top = 1
            .BorderStyle = BorderStyle.None
        End With
        With txtPassord
            .BackColor = Color.White
            .ForeColor = Color.Gray
            .Width = 268
            .UseSystemPasswordChar = True
            .TabIndex = 2
            .Multiline = True
            .WordWrap = False
            .Height = 38
            .Top = 1
            .BringToFront()
            .Font = New Font(DefaultFont.FontFamily, 20)
            .BorderStyle = BorderStyle.None
        End With
        With LabBrukernavn
            .AutoSize = False
            .Text = "Fødselsnummer"
            .BackColor = Color.FromArgb(240, 240, 240)
            .Size = txtBrukernavn.Size
            .TextAlign = ContentAlignment.MiddleLeft
        End With
        With LabPassord
            .AutoSize = False
            .Text = "Passord"
            .BackColor = Color.FromArgb(240, 240, 240)
            .Size = txtBrukernavn.Size
            .TextAlign = ContentAlignment.MiddleLeft
        End With
        ' TEMPORARY; TODO: Switch to secure class

        'NotifManager = New NotificationManager(Me)
        'NotifManager.AssignedLayoutManager = LayoutHelper

        PicLoadingSurface = New PictureBox
        With PicLoadingSurface
            .Hide()
            .Size = New Size(32, 32)
            .Parent = Me
        End With
        LoadingGraphics = New LoadingGraphics(Of Control)(PicLoadingSurface)
        With LoadingGraphics
            .Stroke = 2
            .Pen.Color = Color.LimeGreen
        End With
        BliMedButton = New FullWidthControl(Me, True, FullWidthControl.SnapType.Bottom)
        With BliMedButton
            .Text = "Opprett bruker"
            .TabIndex = 4
            .BackColorSelected = ColorHelper.Multiply(Color.LimeGreen, 0.7)
            .BackColorNormal = Color.LimeGreen
        End With
        FWButton = New FullWidthControl(Me, True, FullWidthControl.SnapType.Bottom, -BliMedButton.Height)
        With FWButton
            .Text = "Logg inn"
            .TabIndex = 3
        End With
        GroupHeader = New FullWidthControl(TextBoxContainer(0), False, FullWidthControl.SnapType.Top)
        With GroupHeader
            .Height = 20
            .BackColor = Color.FromArgb(230, 230, 230)
            .ForeColor = Color.FromArgb(100, 100, 100)
            .TextAlign = ContentAlignment.MiddleLeft
            .Padding = New Padding(5, 0, 0, 0)
            .Text = "Logg inn"
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
        If txtPassord IsNot Nothing Then
            With LayoutHelper
                .CenterOnForm(TextBoxContainer(0),, -50)
                .CenterOnForm(TextBoxContainer(1),, 50)
                .CenterSurface(txtBrukernavn, TextBoxContainer(0))
                .CenterSurface(txtPassord, TextBoxContainer(1))
                .CenterSurface(LabBrukernavn, TextBoxContainer(0),, -30)
                .CenterSurface(LabPassord, TextBoxContainer(1),, -30)
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
            'If Not .GetType = GetType(FullWidthControl) Then
            C.Show()
            'End If
        Next
        FWButton.Enabled = True
        BliMedButton.Enabled = True

        'Testdashbord.Show()
        txtBrukernavn.Clear()
        txtPassord.Clear()
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
        txtBrukernavn.Clear()
        txtPassord.Clear()
        txtBrukernavn.Focus()
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
        GroupHeader.Show()
        FWButton.Enabled = False
        BliMedButton.Enabled = False
        UserLogin.LoginAsync(txtBrukernavn.Text, txtPassord.Text, "DonorKonti", "bruker_ID", "passord")
        LoadingGraphics.Spin(30, 10)
        ResumeLayout()
    End Sub
    Private Sub BliMedButton_Click(Sender As Object, e As EventArgs) Handles BliMedButton.Click

    End Sub
End Class