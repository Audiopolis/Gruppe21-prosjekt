﻿Imports AudiopoLib
Public Class AnsattLoggInnTab
    Inherits Tab
    Private WithEvents BackButton As New PictureBox
    Private BackLabel As New Label
    Private LoginForm As New FlatForm(243, 100, 3, New FormFieldStyle(Color.FromArgb(245, 245, 245), Color.FromArgb(70, 70, 70), Color.White, Color.FromArgb(80, 80, 80), Color.White, Color.Black, {True, True, True, True}, 20))
    Private WithEvents TopBar As New TopBar(Me)
    Private FormPanel As New BorderControl(Color.FromArgb(210, 210, 210))
    Private WithEvents LoggInnKnapp As New TopBarButton(FormPanel, My.Resources.NesteIcon, "Logg inn", New Size(0, 36))
    Private WithEvents OpprettBrukerKnapp As New TopBarButton(FormPanel, My.Resources.RedigerProfilIcon, "Opprett bruker", New Size(0, 36))
    Private LayoutTool As New FormLayoutTools(Me)
    Private Footer As New Footer(Me)
    Private WithEvents AnsattLogin As New MySqlUserLogin(Credentials.Server, Credentials.Database, Credentials.UserID, Credentials.Password)
    Private FormHeader As New FullWidthControl(FormPanel)
    Private WithEvents NotifManager As New NotificationManager(FormHeader)
    Private UsernameString As String
    Public Sub New(ParentWindow As MultiTabWindow)
        MyBase.New(ParentWindow)
        With AnsattLogin
            .IfInvalid = AddressOf LoginInvalid
            .IfValid = AddressOf LoginValid
        End With
        DoubleBuffered = True
        SuspendLayout()
        BackColor = Color.FromArgb(240, 240, 240)
        With FormPanel
            .Hide()
            .Parent = Me
            .Top = TopBar.Bottom + 20
            .Left = 30
            .Width = 408
            .Height = 480
            .BackColor = Color.FromArgb(225, 225, 225)
        End With
        With FormHeader
            .Padding = New Padding(10, 0, 0, 0)
            .Width = 408
            .Height = 40
            .Text = "Logg inn som ansatt"
            .TextAlign = ContentAlignment.MiddleLeft
            .BackColor = Color.FromArgb(183, 187, 191)
            .ForeColor = Color.White
        End With
        With BackButton
            .Size = New Size(38, 38)
            .Parent = FormHeader
            .Location = New Point(FormHeader.Width - .Width, 1)
            .BackgroundImage = My.Resources.ForrigeIcon
            .BackgroundImageLayout = ImageLayout.Center
            .SendToBack()
        End With
        With BackLabel
            .AutoSize = False
            .Size = New Size(200, 38)
            .Parent = FormHeader
            .Location = New Point(BackButton.Left - .Width, 1)
            .TextAlign = ContentAlignment.MiddleRight
            .ForeColor = ColorHelper.Multiply(FormHeader.BackColor, 0.2)
            .Text = "Tilbake til innlogging for blodgivere"
            .SendToBack()
        End With
#Region "LoginForm"
        With LoginForm
            .AddField(FormElementType.TextField)
            With .Last
                .Header.Text = "Brukernavn (ansatt)"
                .Required = True
                .MinLength = 6
                .MaxLength = 50
            End With
            With DirectCast(.Last, FlatForm.FormTextField)
                .PlaceHolder = "Brukernavn"
            End With
            .AddField(FormElementType.TextField)
            With .Last
                .Header.Text = "Passord"
                .Required = True
                .MinLength = 6
                .MaxLength = 50
            End With
            With DirectCast(.Last, FlatForm.FormTextField)
                .PlaceHolder = "Passord"
                .TextField.UseSystemPasswordChar = True
            End With
            .Parent = FormPanel
            .Location = New Point(FormPanel.Width \ 2 - .Width \ 2, FormPanel.Height \ 2 - .Height \ 2)
            .Display()
        End With
#End Region
        With TopBar
            'AddHandler .Click, AddressOf 
        End With
        With OpprettBrukerKnapp
            .Parent = FormPanel
            .Left = LoginForm.Left
            .Top = LoginForm.Bottom + 10
            AddHandler .Click, AddressOf Opprett_Click
        End With
        With LoggInnKnapp
            .Parent = FormPanel
            .Left = OpprettBrukerKnapp.Right + 10
            .Top = LoginForm.Bottom + 10
            AddHandler .Click, AddressOf LoggInn_Click
        End With
        FormPanel.Show()
        ResumeLayout()
    End Sub
    Private Sub LoggInn_Click(sender As Object, e As EventArgs)
        UsernameString = LoginForm.Field(0, 0).Value.ToString
        AnsattLogin.LoginAsync(LoginForm.Field(0, 0).Value.ToString, LoginForm.Field(1, 0).Value.ToString, "Ansatt", "a_brukernavn", "a_passord")
    End Sub
    Private Sub Opprett_Click(sender As Object, e As EventArgs)
        Parent.Index = 9
    End Sub
    Private Sub NotificationOpened() Handles NotifManager.NotificationOpened
        BackButton.Hide()
        BackLabel.Hide()
    End Sub
    Private Sub NotificationClosed() Handles NotifManager.NotificationClosed
        If NotifManager.IsReady Then
            BackButton.Show()
            BackLabel.Show()
        End If
    End Sub
    Protected Overrides Sub OnResize(e As EventArgs)
        SuspendLayout()
        MyBase.OnResize(e)
        If LayoutTool IsNot Nothing Then
            With FormPanel
                .Left = Width \ 2 - .Width \ 2
                .Top = TopBar.Bottom + (Height - TopBar.Bottom - Footer.Height) \ 2 - .Height \ 2
            End With
        End If
        ResumeLayout(True)
    End Sub
    Private Sub Back_Enter() Handles BackButton.MouseEnter
        BackButton.BackColor = ColorHelper.Multiply(FormHeader.BackColor, 0.7)
        BackButton.BackgroundImage = My.Resources.OKIconHvit
    End Sub
    Private Sub Back_MouseLeave() Handles BackButton.MouseLeave
        BackButton.BackColor = FormHeader.BackColor
        BackButton.BackgroundImage = My.Resources.OKIcon
    End Sub
    Private Sub Back_Click() Handles BackButton.Click
        UsernameString = Nothing
        Parent.Index = 4
    End Sub
    Private Sub LoginValid()
        CurrentLogin = New UserInfo(UsernameString)
        MsgBox("Ja")
        LoginForm.ClearAll()
    End Sub
    Private Sub LoginInvalid(ErrorOccurred As Boolean, ErrorMessage As String)
        If ErrorOccurred Then
            NotifManager.Display("Noe gikk galt. Kontakt administrator.", NotificationPreset.OffRedAlert)
        Else
            NotifManager.Display("Brukernavnet eller passordet er feil.", NotificationPreset.OffRedAlert)
        End If
    End Sub

End Class
