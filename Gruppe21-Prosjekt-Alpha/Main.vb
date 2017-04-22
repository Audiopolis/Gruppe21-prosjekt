Option Strict On
Option Explicit On
Option Infer Off
Imports System.ComponentModel
Imports System.Threading
Imports AudiopoLib

Public Class Main
    Private IsLoaded As Boolean = False
    Public Sub New()
        InitializeComponent()
        MyBase.OnLoad(New EventArgs)
    End Sub
    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        MyBase.OnKeyDown(e)
        If e.KeyCode = Keys.Escape Then
            Windows.Index = 1
        End If
    End Sub
    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not IsLoaded Then
            Hide()
            KeyPreview = True
            Windows = New MultiTabWindow(Me)

            ' ALPHA
            'FirstTabTest = New FirstTab(Windows) ' Index = 0
            'SecondTabTest = New SecondTab(Windows) ' Index = 1
            'ThirdTabTest = New ThirdTab(Windows) ' Index = 2

            ' MÅ GJØRES FERDIG
            PersonaliaTest = New Personopplysninger(Windows) ' Index = 0

            ' BETA
            LoggInnTab = New LoggInnNy(Windows) ' Index = 1
            Dashboard = New DashboardTab(Windows) ' Index = 2
            Egenerklæring = New EgenerklæringTab(Windows) ' Index = 3
            Timebestilling = New TimebestillingTab(Windows) ' Index = 4
            AnsattLoggInn = New AnsattLoggInnTab(Windows) ' Index 5
            OpprettAnsatt = New OpprettAnsattTab(Windows) ' Index 6
            AnsattDashboard = New AnsattDashboardTab(Windows) ' Index = 7
            With Windows
                .BackColor = Color.FromArgb(240, 240, 240)
                .Index = 1
            End With
            IsLoaded = True


            '
            ' TODO: Remove:
            ControlBox = True
            MaximizeBox = True
            FormBorderStyle = FormBorderStyle.SizableToolWindow
            ShowInTaskbar = True
            SizeGripStyle = SizeGripStyle.Show
            TopMost = False
        End If
    End Sub

    Private Sub Main_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        'TODO: Dispose
        End
    End Sub
End Class

Public Class Personopplysninger
    Inherits Tab
    Private Personalia As New FlatForm(400, 300, 3, FormFieldStylePresets.PlainWhite)
    Private PasswordForm As New FlatForm(270, 100, 3, New FormFieldStyle(Color.FromArgb(245, 245, 245), Color.FromArgb(70, 70, 70), Color.White, Color.FromArgb(80, 80, 80), Color.White, Color.Black, {True, True, True, True}, 20))
    Private WithEvents TopBar As New TopBar(Me)
    Private FormPanel As New BorderControl(Color.FromArgb(210, 210, 210))
    Private PicDoktor, PicDoktorPassord As New PictureBox
    Private PicOpprettKontoInfo As New PictureBox
    Private FormInfo As New Label
    Private InfoLab As New InfoLabel
    Private WithEvents SendKnapp As New TopBarButton(FormPanel, My.Resources.NesteIcon, "Neste steg", New Size(0, 36))
    Private AvbrytKnapp As New TopBarButton(FormPanel, My.Resources.AvbrytIcon, "Avbryt", New Size(0, 36), True)
    Private NeiTakkKnapp As New TopBarButton(FormPanel, My.Resources.NeiTakkIcon, "Nei takk", New Size(0, 36))
    Private PasswordFormVisible As Boolean = False
    Private LayoutTool As New FormLayoutTools(Me)
    Private Footer As New Footer(Me)
    Private WithEvents DBC As New DatabaseClient(Credentials.Server, Credentials.Database, Credentials.UserID, Credentials.Password)
    Private FirstHeader As New FullWidthControl(FormPanel)
    Private FormResult() As String
    Private PasswordResult() As String
    Private NotifManager As New NotificationManager(FirstHeader)
    Private CreateLogin As Boolean = True
    Private PicSuccess As New PictureBox
    Private Sub TopBarButtonClick(Sender As TopBarButton, e As EventArgs) Handles TopBar.ButtonClick
        Select Case CInt(Sender.Tag)
            Case 0
                ResetForm()
                Parent.Index = 1
        End Select
    End Sub
    Private Sub SendClick() Handles SendKnapp.Click
        If Not PasswordFormVisible Then
            If Personalia.Validate Then
                Dim Result() As HeaderValuePair = Personalia.Result
                Dim DataArr(11) As String
                For i As Integer = 0 To 4
                    DataArr(i) = Result(i).Value.ToString
                Next
                If DirectCast(Result(5).Value, Boolean) Then
                    DataArr(5) = "1"
                Else
                    DataArr(5) = "0"
                End If
                For i As Integer = 7 To 10
                    DataArr(i - 1) = Result(i).Value.ToString
                Next
                If DirectCast(Result(11).Value, Boolean) Then
                    DataArr(10) = "1"
                Else
                    DataArr(10) = "0"
                End If
                If DirectCast(Result(12).Value, Boolean) Then
                    DataArr(11) = "1"
                Else
                    DataArr(11) = "0"
                End If
                FormResult = DataArr
                Personalia.Hide()
                PicDoktor.Hide()
                FormInfo.Hide()
                SendKnapp.Hide()
                NeiTakkKnapp.Hide()
                With AvbrytKnapp
                    .Hide()
                    .Left = FormInfo.Left
                End With
                With SendKnapp
                    .IconImage = My.Resources.OKIcon
                    With .Label
                        .Text = "Opprett bruker"
                    End With
                    .Left = Personalia.Right - .Width
                    With .Label
                        .Font = New Font(.Font, FontStyle.Bold)
                        .UseCompatibleTextRendering = True
                        .Text = .Text
                    End With
                End With
                With NeiTakkKnapp
                    .Top = SendKnapp.Top
                    .Left = SendKnapp.Left - .Width - 10
                    .Show()
                End With
                AvbrytKnapp.Show()
                SendKnapp.Show()
                PicOpprettKontoInfo.Show()
                PicDoktorPassord.Show()
                PasswordForm.Field(0, 0).Value = FormResult(0)
                PasswordForm.Show()
                PasswordFormVisible = True
            Else
                NotifManager.Display("Noe gikk galt. Vennligst forsikre deg om at skjemaet er fylt inn riktig.", NotificationPreset.OffRedAlert)
            End If
        ElseIf PasswordForm.Validate Then
            DBC.Execute({"@fodselsnr", "@b_fornavn", "@b_etternavn", "@b_adresse", "@b_postnr", "@b_kjonn", "@b_telefon1", "@b_telefon2", "@b_telefon3", "@b_epost", "@send_epost", "@send_sms"}, FormResult)
        End If
    End Sub
    Private Sub DBC_Finished(Sender As Object, e As DatabaseListEventArgs) Handles DBC.ListLoaded
        If e.ErrorOccurred Then
            NotifManager.Display("Noe gikk galt. Vennligst forsikre deg om at skjemaet er fylt inn riktig.", NotificationPreset.OffRedAlert)
        Else
            If CreateLogin = True Then
                CreateLogin = False
                Dim Result() As HeaderValuePair = PasswordForm.Result
                PasswordResult = {FormResult(0), Result(1).Value.ToString}
                DBC.SQLQuery = "INSERT INTO Brukerkonto (b_fodselsnr, passord) VALUES (@fodselsnr, @passord);"
                DBC.Execute({"@fodselsnr", "@passord"}, {PasswordResult(0), PasswordResult(1)})
            Else
                PasswordForm.Hide()
                PicDoktorPassord.Hide()
                SendKnapp.Hide()
                AvbrytKnapp.Hide()
                NeiTakkKnapp.Hide()
                InfoLab.Hide()
                FormInfo.Hide()
                PicOpprettKontoInfo.Hide()
                PicSuccess.Show()
                NotifManager.Display("Du er nå registrert i vårt system.", NotificationPreset.GreenSuccess)
            End If
        End If
    End Sub
    Private Sub DBC_Failed() Handles DBC.ExecutionFailed
        NotifManager.Display("Noe gikk galt. Vennligst forsikre deg om at skjemaet er fylt ut riktig.", NotificationPreset.OffRedAlert)
    End Sub
    Public Shadows Sub Show()
        FormPanel.Hide()
        MyBase.Show()
    End Sub
    Private Sub Me_VisibleChanged() Handles Me.VisibleChanged
        If Visible Then
            FormPanel.Show()
        End If
    End Sub
    Public Sub New(Window As MultiTabWindow)
        MyBase.New(Window)
        DoubleBuffered = True
        SuspendLayout()
        BackColor = Color.FromArgb(240, 240, 240)
        With FormPanel
            .Hide()
            .Parent = Me
            .Top = TopBar.Bottom + 20
            .Left = 30
            .Width = 817
            .Height = 480
            .BackColor = Color.FromArgb(225, 225, 225)
        End With
        With FirstHeader
            .Width = 817
            .Height = 40
            .Text = "Registrering"
            .BackColor = Color.FromArgb(183, 187, 191)
            .ForeColor = Color.White
        End With
#Region "Form"
        With Personalia
            .NewRowHeight = 50
            .AddField(FormElementType.TextField, 180)
            With .Last
                .Header.Text = "Fødselsnummer* (11 siffer)"
                .Required = True
                .Numeric = True
                .MinLength = 11
                .MaxLength = 11
            End With
            .AddField(FormElementType.TextField, 107)
            With .Last
                .Header.Text = "Fornavn*"
                .Required = True
                .MaxLength = 30
            End With
            .AddField(FormElementType.TextField)
            With .Last
                .Header.Text = "Etternavn*"
                .Required = True
                .MaxLength = 30
            End With
            .AddField(FormElementType.TextField, 290)
            With .Last
                .Header.Text = "Privatadresse*"
                .Required = True
                .MaxLength = 100
            End With
            .AddField(FormElementType.TextField)
            With .Last
                .Header.Text = "Postnummer*"
                .Required = True
                .Numeric = True
                .MinLength = 4
                .MaxLength = 4
            End With
            .NewRowHeight = 50
            .AddField(FormElementType.Radio, 200)
            With .Last
                .Value = True
                .Header.Text = "Kjønn*"
                .SecondaryValue = "Jeg er mann"
                .DrawBorder(FormField.ElementSide.Right) = False
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SecondaryValue = "Jeg er kvinne"
                .DrawBorder(FormField.ElementSide.Left) = False
                .DrawDashedSepararators(FormField.ElementSide.Left) = True
                .Extrude(FieldExtrudeSide.Left, 3)
                .DrawDotsOnHeader = False
            End With
            .AddField(FormElementType.TextField, 133)
            With .Last
                .Header.Text = "Telefon privat*"
                .Required = True
                .MaxLength = 15
            End With
            .AddField(FormElementType.TextField, 133)
            With .Last
                .Header.Text = "Telefon mobil"
            End With
            .AddField(FormElementType.TextField)
            With .Last
                .Header.Text = "Telefon arbeid"
            End With
            .AddField(FormElementType.TextField)
            With .Last
                .Header.Text = "Epost-adresse*"
                .Required = True
                .MinLength = 5
                .MaxLength = 100
            End With
            .AddField(FormElementType.CheckBox)
            .NewRowHeight = 40
            With .Last
                .Value = True
                .SecondaryValue = "Jeg ønsker å motta innkalling, påminnelser og informasjon via epost"
                .DrawBorder(FormField.ElementSide.Bottom) = False
            End With
            .AddField(FormElementType.CheckBox)
            With .Last
                .Value = True
                .SecondaryValue = "Jeg ønsker å motta innkalling, påminnelser og informasjon via SMS"
                .DrawBorder(FormField.ElementSide.Top) = False
                .DrawDashedSepararators(FormField.ElementSide.Top) = True
                .SwitchHeader(False)
            End With
            .MergeWithAbove(6, 0, 0, True)
            .Parent = FormPanel
            .Display()
            .Location = New Point(20, 60)
        End With
#End Region
#Region "Password Form"
        With PasswordForm
            .AddField(FormElementType.Label)
            With .Last
                .Header.Text = "Bruker-ID"
                .Value = "Fødselsnummer"
            End With
            .AddField(FormElementType.TextField)
            Dim FieldHeight As Integer
            With .Last
                .Header.Text = "Velg et passord"
                .DrawBorder(FormField.ElementSide.Bottom) = False
                .Required = True
                .MinLength = 8
                .MaxLength = 50
                AddHandler .ValueChanged, AddressOf PasswordChanged
                AddHandler .ValidChanged, AddressOf PasswordValidChanged
                FieldHeight = .Height - .Header.Bottom
            End With
            With DirectCast(.Last, FlatForm.FormTextField)
                .PlaceHolder = "Minst 8 tegn"
                .TextField.UseSystemPasswordChar = True
            End With
            .AddField(FormElementType.TextField)
            .MergeWithAbove(2, 0)
            With .Last
                AddHandler .ValidChanged, AddressOf PasswordValidChanged
                .DrawBorder(FormField.ElementSide.Top) = False
                .DrawDashedSepararators(FormField.ElementSide.Top) = True
                .SwitchHeader(False)
                .Height = FieldHeight
                .Required = True
                .RequireSpecificValue("")
            End With
            With DirectCast(.Last, FlatForm.FormTextField)
                .PlaceHolder = "Gjenta passordet"
                .TextField.UseSystemPasswordChar = True
            End With
            .Parent = FormPanel
            .Display()
        End With
#End Region
        With TopBar
            .AddButton(My.Resources.HjemIcon, "Hjem", New Size(135, 36))
            'AddHandler .Click, AddressOf 
        End With
        With SendKnapp
            .Top = Personalia.Bottom + 10
            .Left = Personalia.Right - .Width
        End With
        With NeiTakkKnapp
            .Hide()
        End With
        With AvbrytKnapp
            .Top = SendKnapp.Top
            .Left = SendKnapp.Left - .Width - 10
            AddHandler .Click, AddressOf AvbrytKnapp_Klikk
        End With
        With PicDoktor
            .BackgroundImage = My.Resources.Doktor2
            .Size = .BackgroundImage.Size
            .Parent = FormPanel
            .Top = Personalia.Bottom - .Height
            .Left = Personalia.Right + 20
        End With
        With PicDoktorPassord
            .BackgroundImage = My.Resources.DoktorPassord
            .Size = .BackgroundImage.Size
            .Parent = FormPanel
            .Location = PicDoktor.Location
        End With
        With PasswordForm
            .Left = PicDoktorPassord.Left \ 2 - .Width \ 2
            .Top = PicDoktorPassord.Bottom - 210 \ 2 - .Height \ 2
        End With
        With FormInfo
            .Parent = FormPanel
            .Top = Personalia.Bottom + 10
            .Left = Personalia.Left
            .AutoSize = False
            .Height = SendKnapp.Height
            .Width = AvbrytKnapp.Left - .Left
            .TextAlign = ContentAlignment.MiddleLeft
            .ForeColor = Color.FromArgb(80, 80, 80)
            .Text = "* markerer obligatoriske felt"
        End With
        With InfoLab
            .Parent = FormPanel
            .Top = PicDoktor.Bottom + 10
            .Left = PicDoktor.Left
            .Width = PicDoktor.Width
            .Height = SendKnapp.Height
            .Text = "Ved å registrere deg, samtykker du i at denne informasjonen blir lagret i våre systemer. Du kan når som helst slette disse opplysningene."
            ' TODO: Fix
            .PanIn()
        End With
        With PicOpprettKontoInfo
            .Parent = FormPanel
            .Top = TopBar.Bottom
            .Left = 20
            .BackgroundImage = My.Resources.OpprettKontoInfo
            .BackgroundImageLayout = ImageLayout.Center
            .Height = .BackgroundImage.Height
            .Width = .BackgroundImage.Width
            .Hide()
            '.MakeDashed(Color.Red)
        End With
        With PicSuccess
            .Hide()
            .Parent = FormPanel
            .Size = New Size(64, 64)
            .BackColor = Color.LimeGreen
            .Location = New Point(FormPanel.Width \ 2 - .Width \ 2, FormPanel.Height \ 2 - .Height \ 2)
        End With
        DBC.SQLQuery = "INSERT INTO Blodgiver (b_fodselsnr, b_fornavn, b_etternavn, b_telefon1, b_telefon2, b_telefon3, b_epost, b_adresse, b_postnr, b_kjonn, send_epost, send_sms) VALUES (@fodselsnr, @b_fornavn, @b_etternavn, @b_telefon1, @b_telefon2, @b_telefon3, @b_epost, @b_adresse, @b_postnr, @b_kjonn, @send_epost, @send_sms);"
        FormPanel.Show()
        ResumeLayout()
    End Sub
    Private Sub PanInTest() Handles Me.DoubleClick
        InfoLab.PanIn()
    End Sub
    Private Sub PasswordChanged(Sender As FormField, Value As Object)
        PasswordForm.Field(2, 0).RequireSpecificValue(PasswordForm.Field(1, 0).Value.ToString)
    End Sub
    Private Sub PasswordValidChanged(Sender As FormField)
        With PasswordForm
            .Field(1, 0).IsValid = Sender.IsValid
            .Field(2, 0).IsValid = Sender.IsValid
        End With
    End Sub
    Private Sub AvbrytKnapp_Klikk(Sender As Object, e As EventArgs)
        ResetForm()
        Parent.Index = 1
    End Sub
    Private Sub ResetForm()
        FormPanel.Hide()
        DBC.SQLQuery = "INSERT INTO Blodgiver (b_fodselsnr, b_fornavn, b_etternavn, b_telefon1, b_telefon2, b_telefon3, b_epost, b_adresse, b_postnr, b_kjonn, send_epost, send_sms) VALUES (@fodselsnr, @b_fornavn, @b_etternavn, @b_telefon1, @b_telefon2, @b_telefon3, @b_epost, @b_adresse, @b_postnr, @b_kjonn, @send_epost, @send_sms);"
        With PasswordForm
            .ClearAll(True)
            .Hide()
        End With
        With SendKnapp
            .BackColor = NeiTakkKnapp.BackColor
            .Label.Font = New Font(.Font, FontStyle.Regular)
            .Text = "Videre"
            .Top = Personalia.Bottom + 10
            .Left = Personalia.Right - .Width
        End With
        With NeiTakkKnapp
            .Hide()
        End With
        With AvbrytKnapp
            .Top = SendKnapp.Top
            .Left = SendKnapp.Left - .Width - 10
        End With
        PicDoktor.Show()
        PicDoktorPassord.Hide()
        PasswordForm.Hide()
        FormInfo.Show()
        PicOpprettKontoInfo.Hide()
        PicSuccess.Hide()
        FormResult = Nothing
        With Personalia
            .ClearAll()
            .Show()
            .Field(2, 0).Value = True
            .Field(5, 0).Value = True
            .Field(6, 0).Value = True
        End With
        PasswordFormVisible = False
    End Sub
    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            RemoveHandler PasswordForm.Field(1, 0).ValueChanged, AddressOf PasswordChanged
            RemoveHandler PasswordForm.Field(1, 0).ValidChanged, AddressOf PasswordValidChanged
            RemoveHandler PasswordForm.Field(2, 0).ValidChanged, AddressOf PasswordValidChanged
            LayoutTool.Dispose()
            NotifManager.Dispose()

        End If
        MyBase.Dispose(disposing)
    End Sub
    Protected Overrides Sub OnResize(e As EventArgs)
        SuspendLayout()
        MyBase.OnResize(e)
        ' TODO: Remove LayoutTool
        If LayoutTool IsNot Nothing Then
            With FormPanel
                .Top = TopBar.Bottom + 20
                .Left = Width \ 2 - .Width \ 2
                .Top = TopBar.Bottom + (Height - TopBar.Bottom - Footer.Height) \ 2 - .Height \ 2
            End With
        End If
        ResumeLayout(True)
    End Sub
End Class

Public Class LoggInnNy
    Inherits Tab
    Private WithEvents Gear As New GearIcon
    Private LoginForm As New FlatForm(243, 100, 3, New FormFieldStyle(Color.FromArgb(245, 245, 245), Color.FromArgb(70, 70, 70), Color.White, Color.FromArgb(80, 80, 80), Color.White, Color.Black, {True, True, True, True}, 20))
    Private WithEvents TopBar As New TopBar(Me)
    Private FormPanel As New BorderControl(Color.FromArgb(210, 210, 210))
    Private PicSideInfo, PicInfoAbove, RightSide As New PictureBox
    Private FormInfo As New Label
    Private InfoLab As New InfoLabel
    Private WithEvents LoggInnKnapp As New TopBarButton(FormPanel, My.Resources.NesteIcon, "Logg inn", New Size(0, 36))
    Private OpprettBrukerKnapp As New TopBarButton(FormPanel, My.Resources.RedigerProfilIcon, "Opprett bruker", New Size(0, 36))
    Private LayoutTool As New FormLayoutTools(Me)
    Private Footer As New Footer(Me)
    'Private WithEvents DBC As New DatabaseClient(Credentials.Server, Credentials.Database, Credentials.UserID, Credentials.Password)
    Private WithEvents UserLogin As New MySqlUserLogin(Credentials.Server, Credentials.Database, Credentials.UserID, Credentials.Password)
    Private FormHeader As New FullWidthControl(FormPanel)
    Private WithEvents NotifManager As New NotificationManager(FormHeader)
    Private LeftSide As New BorderControl(Color.FromArgb(210, 210, 210))
    Private PersonalNumber As String
    Private Sub NotificationOpened() Handles NotifManager.NotificationOpened
        Gear.Hide()
    End Sub
    Private Sub NotificationClosed() Handles NotifManager.NotificationClosed
        If NotifManager.IsReady Then
            Gear.Show()
        End If
    End Sub
    Private Sub Gear_Click() Handles Gear.Click
        ' TODO: Vis logintab
        Parent.Index = 5
    End Sub
    Private Sub LoginValid()
        CurrentLogin = New UserInfo(PersonalNumber)
        Dashboard.Initiate()
        Parent.Index = 2
        Egenerklæring.InitiateForm()
        LoginForm.ClearAll()
        If HentTimer_DBC IsNot Nothing Then
            HentTimer_DBC.Dispose()
        End If
        HentTimer_DBC = New DatabaseClient(Credentials.Server, Credentials.Database, Credentials.UserID, Credentials.Password)
        With HentTimer_DBC
            .SQLQuery = "SELECT time_id, t_dato, t_klokkeslett FROM Time WHERE b_fodselsnr = @nr AND NOT (a_id IS NOT NULL AND ansatt_godkjent = 0);"
            .Execute({"@nr"}, {PersonalNumber})
        End With
        If HentEgenerklæring_DBC IsNot Nothing Then
            HentEgenerklæring_DBC.Dispose()
        End If
        HentEgenerklæring_DBC = New DatabaseClient(Credentials.Server, Credentials.Database, Credentials.UserID, Credentials.Password)
    End Sub
    Private Sub LoginInvalid(ErrorOccurred As Boolean, ErrorMessage As String)
        If ErrorOccurred Then
            NotifManager.Display("Noe gikk galt. Vennligst kontakt betjeningen.", NotificationPreset.OffRedAlert)
        Else
            NotifManager.Display("Brukernavnet eller passordet er feil.", NotificationPreset.OffRedAlert)
        End If
    End Sub
    Public Shadows Sub Show()
        FormPanel.Hide()
        MyBase.Show()
    End Sub
    Private Sub Me_VisibleChanged() Handles Me.VisibleChanged
        If Visible Then
            FormPanel.Show()
        End If
    End Sub
    Protected Overrides Sub OnDoubleClick(e As EventArgs)
        MyBase.OnDoubleClick(e)
    End Sub
    Public Sub New(Window As MultiTabWindow)
        MyBase.New(Window)
        With UserLogin
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
            .Width = 817
            .Height = 480
            .BackColor = Color.FromArgb(225, 225, 225)
        End With
        With FormHeader
            .Width = 817
            .Height = 40
            .Text = "Logg inn"
            .BackColor = Color.FromArgb(183, 187, 191)
            .ForeColor = Color.White
        End With
        With LeftSide
            .Parent = FormPanel
            .Size = New Size(FormPanel.Width \ 2, FormPanel.Height - FormHeader.Bottom - 1)
            .Top = FormHeader.Bottom
            .Left = 1
            .DrawBorder(FormField.ElementSide.Bottom) = False
            .DrawBorder(FormField.ElementSide.Top) = False
            .DrawBorder(FormField.ElementSide.Left) = False
        End With
        With RightSide
            .Parent = FormPanel
            .Location = New Point(LeftSide.Right, LeftSide.Top)
            .Size = New Size(FormPanel.Width - .Left - 1, FormPanel.Height - .Top - 1)
            .BackgroundImage = My.Resources.PicLoggInn
            .BackgroundImageLayout = ImageLayout.Center
        End With
#Region "LoginForm"
        With LoginForm
            .AddField(FormElementType.TextField)
            With .Last
                .Header.Text = "Fødselsnummer (11 siffer)"
                .Required = True
                .Numeric = True
                .MinLength = 11
                .MaxLength = 11
            End With
            With DirectCast(.Last, FlatForm.FormTextField)
                .PlaceHolder = "11 siffer"
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
            .Parent = LeftSide
            .Location = New Point(LeftSide.Width \ 2 - .Width \ 2, LeftSide.Height \ 2 - .Height \ 2)
            .Display()
        End With
#End Region
        With TopBar
            'AddHandler .Click, AddressOf 
        End With
        With OpprettBrukerKnapp
            .Parent = LeftSide
            .Left = LoginForm.Left
            .Top = LoginForm.Bottom + 10
            AddHandler .Click, AddressOf OpprettBruker_Click
        End With
        With LoggInnKnapp
            .Parent = LeftSide
            .Left = OpprettBrukerKnapp.Right + 10
            .Top = LoginForm.Bottom + 10
            AddHandler .Click, AddressOf LoggInn_Click
        End With
        With FormInfo
            .Parent = FormPanel
            .AutoSize = False
            .Height = LoggInnKnapp.Height
            .TextAlign = ContentAlignment.MiddleLeft
            .ForeColor = Color.FromArgb(80, 80, 80)
            .Text = "* markerer obligatoriske felt"
        End With
        With InfoLab
            .Parent = FormPanel
            .Height = LoggInnKnapp.Height
            .Width = PicSideInfo.Width
            .Text = "Ved å registrere deg, samtykker du i at denne informasjonen blir lagret i våre systemer. Du kan når som helst slette disse opplysningene."
        End With
        With PicInfoAbove
            .Parent = FormPanel
            .Top = TopBar.Bottom
            .Left = 20
            .BackgroundImage = My.Resources.OpprettKontoInfo
            .BackgroundImageLayout = ImageLayout.Center
            .Height = .BackgroundImage.Height
            .Width = .BackgroundImage.Width
            .Hide()
            '.MakeDashed(Color.Red)
        End With
        With Gear
            'a
            .Parent = FormHeader
            .Location = New Point(FormHeader.Width - .Width - 5, FormHeader.Height \ 2 - .Height \ 2)
        End With
        FormPanel.Show()
        ResumeLayout()
    End Sub
    Private Sub OpprettBruker_Click(sender As Object, e As EventArgs)
        Parent.Index = 0
    End Sub
    Private Sub LoggInn_Click(sender As Object, e As EventArgs)
        PersonalNumber = LoginForm.Field(0, 0).Value.ToString
        UserLogin.LoginAsync(LoginForm.Field(0, 0).Value.ToString, LoginForm.Field(1, 0).Value.ToString, "Brukerkonto", "b_fodselsnr", "passord")
    End Sub
    Private Sub PasswordChanged(Sender As FormField, Value As Object)
        LoginForm.Field(2, 0).RequireSpecificValue(LoginForm.Field(1, 0).Value.ToString)
    End Sub
    Private Sub PasswordValidChanged(Sender As FormField)
        With LoginForm
            .Field(1, 0).IsValid = Sender.IsValid
            .Field(2, 0).IsValid = Sender.IsValid
        End With
    End Sub
    Private Sub ResetForm()
        FormPanel.Hide()
        With LoginForm
            .ClearAll()
        End With
    End Sub
    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            NotifManager.Dispose()
            LayoutTool.Dispose()
            RemoveHandler OpprettBrukerKnapp.Click, AddressOf OpprettBruker_Click
            RemoveHandler LoggInnKnapp.Click, AddressOf LoggInn_Click
        End If
        MyBase.Dispose(disposing)
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
End Class

Public Class GearIcon
    Inherits PictureBox
    Private ImageSize As New Size(34, 34)
    Private SC As SynchronizationContext = SynchronizationContext.Current
    Private GearIcon As Bitmap = My.Resources.SettingsIcon
    Private varCurrentDegree As Integer = 0 ' TODO: Try Double
    Private varIsHovering As Boolean
    Private varIncrement As Integer = 5
    Private WithEvents SpinTimer As New Timers.Timer(1000 \ 30)
    Public Sub New()
        DoubleBuffered = True
        SpinTimer.AutoReset = False
        BackgroundImage = GearIcon
        BackgroundImageLayout = ImageLayout.Center
        Cursor = Cursors.Hand
        Size = New Size(34, 34)
    End Sub
    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        varIsHovering = True
        varIncrement = 5
        SpinTimer.Start()
    End Sub
    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        varIsHovering = False
    End Sub
    Private Sub SpinTimer_Tick() Handles SpinTimer.Elapsed
        Dim returnBitmap As New Bitmap(ImageSize.Width, ImageSize.Height)
        Using g As Graphics = Graphics.FromImage(returnBitmap)
            With g
                .CompositingQuality = Drawing2D.CompositingQuality.HighQuality
                Dim OffsetSingle As Single = CSng(16.5)
                .TranslateTransform(OffsetSingle, OffsetSingle)
                .RotateTransform(varCurrentDegree)
                .TranslateTransform(-OffsetSingle, -OffsetSingle)
                .DrawImage(GearIcon, New Rectangle(Point.Empty, ImageSize))
            End With
        End Using
        SC.Post(AddressOf AdjustRotation, returnBitmap)
    End Sub
    Private Sub AdjustRotation(State As Object)
        Dim NewImage As Bitmap = DirectCast(State, Bitmap)
        varCurrentDegree += varIncrement
        If varCurrentDegree >= 360 Then varCurrentDegree = 0
        If varIsHovering Then
            BackgroundImage = NewImage
            SpinTimer.Start()
        ElseIf varCurrentDegree > 0 Then
            BackgroundImage = NewImage
            varIncrement += 2
            SpinTimer.Start()
        Else
            NewImage.Dispose()
            varIncrement = 5
            varCurrentDegree = 0
            BackgroundImage = GearIcon
        End If
    End Sub
    ' TODO: Add class for this in AudiopoLib
    Private Function RotateImage() As Bitmap
        Dim returnBitmap As New Bitmap(ImageSize.Width, ImageSize.Height)
        Using g As Graphics = Graphics.FromImage(returnBitmap)
            With g
                .CompositingQuality = Drawing2D.CompositingQuality.HighQuality
                .TranslateTransform(CSng(16.5), CSng(16.5))
                .RotateTransform(varCurrentDegree)
                .TranslateTransform(CSng(-16.5), CSng(-16.5))
                .DrawImage(GearIcon, New Rectangle(Point.Empty, ImageSize))
            End With
        End Using
        Return returnBitmap
    End Function
End Class

Public Class DashboardTab
    Inherits Tab
    Public NotificationList As New UserNotificationContainer(Color.FromArgb(210, 210, 210))
    Private Header As New TopBar(Me)
    'Dim ScrollList As New Donasjoner(Me)
    Private WithEvents Beholder As New BlodBeholder(My.Resources.Tom_beholder, My.Resources.Full_beholder)
    Private WelcomeLabel As New InfoLabel(True, Direction.Right)

    ' TODO: Remove
    Private current As Integer
    Private increment As Boolean = True

    Private OrganDonorInfo As New PictureBox
    Private IsLoaded As Boolean
    Private Messages As New MessageNotification(Header)
    Private WithEvents DBC As New DatabaseClient(Credentials.Server, Credentials.Database, Credentials.UserID, Credentials.Password)
    Private Sub TopBarButtonClick(sender As Object, e As EventArgs)
        Dim SenderButton As TopBarButton = DirectCast(sender, TopBarButton)
        If SenderButton.IsLogout Then
            Parent.Index = 1
            Logout()
        Else
            Select Case CInt(SenderButton.Tag)
                Case 0
                    Parent.Index = 4
                Case 1
                    Parent.Index = 3
                Case 2
                    Messages.Start()
            End Select
        End If
    End Sub

    Public Sub New(ParentWindow As MultiTabWindow)
        MyBase.New(ParentWindow)
        If Not IsLoaded Then
            With Header
                .AddButton(My.Resources.TimeBestillingIcon, "Bestill ny time", New Size(135, 36))
                .AddButton(My.Resources.EgenerklaeringIcon, "Registrer egenerklæring", New Size(135, 36))
                .AddButton(My.Resources.RedigerProfilIcon, "Rediger profil", New Size(135, 36))
                .AddLogout("Logg ut", New Size(135, 36))
                AddHandler .ButtonClick, AddressOf TopBarButtonClick
            End With
            With Beholder
                .Parent = Me
                .Location = New Point(ClientSize.Width - .Width - 20, Header.Bottom + 20)
            End With
            With WelcomeLabel
                .ForeColor = Color.White
                .Parent = Header
                .Top = Header.Height \ 2 - .Height \ 2
                .Text = "Du er logget inn som..."
                .Height = Header.LogoutButton.Height - 3
            End With
            With NotificationList
                .Parent = Me
                .Location = New Point(20, Header.Bottom + 20)
            End With
            With OrganDonorInfo
                .BackgroundImage = My.Resources.infoTekstDashboard
                .Size = .BackgroundImage.Size
                .Parent = Me
            End With
            With Messages
                .Show()
                .Parent = Header
                .Left = ClientSize.Width - 500
                .Top = Header.Height \ 2 - .Height \ 2
                .BringToFront()
            End With
            IsLoaded = True
        End If
    End Sub
    Public Sub Initiate()
        With DBC
            .SQLQuery = "SELECT b_fornavn, b_etternavn FROM Blodgiver WHERE b_fodselsnr = @nr;"
            .Execute({"@nr"}, {CurrentLogin.PersonalNumber})
        End With
    End Sub
    Private Sub DBC_Finished(Sender As Object, e As DatabaseListEventArgs) Handles DBC.ListLoaded
        With e.Data
            CurrentLogin.FirstName = .Rows(0).Item(0).ToString
            CurrentLogin.LastName = .Rows(0).Item(1).ToString
            Header.RaiseNameSetEvent()
            WelcomeLabel.Text = "Du er logget inn som " & CurrentLogin.FirstName & " " & CurrentLogin.LastName
            WelcomeLabel.PanIn()
        End With
    End Sub
    'Public Shadows Sub Show()
    '    MyBase.Show()
    'End Sub
    Private Sub SubClickTest() Handles Beholder.Click
        If increment Then
            current += 30
        Else
            current -= 30
        End If
        Beholder.SlideToPercentage(current)
        If current >= 90 Then
            increment = False
        ElseIf current <= 0 Then
            increment = True
        End If
    End Sub
    Protected Overrides Sub OnResize(e As EventArgs)
        SuspendLayout()
        MyBase.OnResize(e)
        If IsLoaded Then
            With WelcomeLabel
                .Location = New Point(Width - 430, Header.LogoutButton.Top)
                .Size = New Size(300, Header.LogoutButton.Height - 3)
            End With
            With NotificationList
                .Top = (ClientSize.Height - .Height + Header.Bottom) \ 2
            End With
            With OrganDonorInfo
                .Location = New Point(ClientSize.Width - .Width - 20, NotificationList.Top)
            End With
            With Beholder
                .Location = New Point((NotificationList.Right + OrganDonorInfo.Left - .Width) \ 2, (ClientSize.Height - .Height + Header.Bottom) \ 2)
            End With
            With Messages
                .Left = ClientSize.Width - 500
                .Top = Header.Height \ 2 - .Height \ 2
            End With
        End If
        ResumeLayout(True)
    End Sub
    Protected Overrides Sub Dispose(disposing As Boolean)
        RemoveHandler Header.ButtonClick, AddressOf TopBarButtonClick
        MyBase.Dispose(disposing)
    End Sub
End Class