Option Strict On
Option Explicit On
Option Infer Off
Imports System.ComponentModel
Imports System.Drawing.Drawing2D
Imports System.IO
Imports System.Security.Cryptography
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
            FirstTabTest = New FirstTab(Windows) ' Index = 0
            SecondTabTest = New SecondTab(Windows) ' Index = 1
            ThirdTabTest = New ThirdTab(Windows) ' Index = 2

            ' MÅ GJØRES FERDIG
            PersonaliaTest = New Personopplysninger(Windows) ' Index = 3

            ' BETA
            LoggInnTab = New LoggInnNy(Windows) ' Index = 4
            Dashboard = New DashboardTab(Windows) ' Index = 5
            Egenerklæring = New EgenerklæringTab(Windows) ' Index = 6
            Timebestilling = New TimebestillingTab(Windows) ' Index = 7

            With Windows
                .BackColor = Color.FromArgb(240, 240, 240)
                .Index = 4
            End With
            WindowState = FormWindowState.Maximized
            'Width = 800
            'Height = 500
            IsLoaded = True
        End If
    End Sub

    Private Sub Main_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        'TODO: Dispose
        End
    End Sub
End Class

Public Class FirstTab
    Inherits Tab
    Dim GroupLoggInn As GroupBox
    Dim WithEvents UserLogin As MySqlUserLogin
    Dim LoadingGraphics As LoadingGraphics(Of Control)
    Dim WithEvents FWButton, BliMedButton As FullWidthControl
    Dim PicLoadingSurface As Control
    Dim NotifManager As NotificationManager
    Dim txtBrukernavn, txtPassord As TextBox
    Dim GroupHeader As FullWidthControl
    Dim LabBrukernavn, LabPassord As Label
    Dim LayoutHelper As New FormLayoutTools(Me)
    Dim IsLoaded As Boolean
    Public Sub New(ParentWindow As MultiTabWindow)
        MyBase.New(ParentWindow)
        Hide()
        GroupLoggInn = New GroupBox
        LabBrukernavn = New Label
        LabPassord = New Label
        txtBrukernavn = New TextBox
        txtPassord = New TextBox
        With GroupLoggInn
            .Size = New Size(230, 151)
            .Text = "Logg inn"
            With .Controls
                .Add(LabBrukernavn)
                .Add(LabPassord)
                .Add(txtBrukernavn)
                .Add(txtPassord)
            End With
            .Parent = Me
        End With
        Dim TextBoxSize As New Size(145, 20)
        With txtBrukernavn
            .Size = TextBoxSize
            .Location = New Point(74, 33)
            .TabIndex = 1
            .BringToFront()
        End With
        With txtPassord
            .Size = TextBoxSize
            .UseSystemPasswordChar = True
            .Location = New Point(74, 59)
            .TabIndex = 2
            .BringToFront()
        End With
        With LabBrukernavn
            .AutoSize = False
            .Text = "Bruker-ID"
            .Top = txtBrukernavn.Top
            .Left = 5
            .Width = txtBrukernavn.Left - 15
            .Height = txtBrukernavn.Height
            .TextAlign = ContentAlignment.MiddleRight
        End With
        With LabPassord
            .AutoSize = False
            .Text = "Passord"
            .Top = txtPassord.Top
            .Left = 5
            .Size = LabBrukernavn.Size
            .TextAlign = ContentAlignment.MiddleRight
        End With
        ' TEMPORARY; TODO: Switch to secure class

        NotifManager = New NotificationManager(Me)
        NotifManager.AssignedLayoutManager = LayoutHelper

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
        BliMedButton = New FullWidthControl(GroupLoggInn, True, FullWidthControl.SnapType.Bottom)
        With BliMedButton
            .Text = "Opprett bruker"
            .TabIndex = 4
            .BackColorSelected = ColorHelper.Multiply(Color.LimeGreen, 0.7)
            .BackColorNormal = Color.LimeGreen
        End With
        FWButton = New FullWidthControl(GroupLoggInn, True, FullWidthControl.SnapType.Bottom, -BliMedButton.Height)
        With FWButton
            .Text = "Logg inn"
            .TabIndex = 3
        End With
        GroupHeader = New FullWidthControl(GroupLoggInn, False, FullWidthControl.SnapType.Top)
        With GroupHeader
            .Height = 20
            .BackColor = Color.FromArgb(230, 230, 230)
            .ForeColor = Color.FromArgb(100, 100, 100)
            .TextAlign = ContentAlignment.MiddleLeft
            .Padding = New Padding(5, 0, 0, 0)
            .Text = "Logg inn"
        End With
        With Credentials
            UserLogin = New MySqlUserLogin(.Server, .Database, .UserID, .Password)
        End With
        With UserLogin
            .IfValid = AddressOf LoginValid
            .IfInvalid = AddressOf LoginInvalid
        End With
        With LayoutHelper
            .CenterSurface(GroupLoggInn, Me)
            .CenterSurface(PicLoadingSurface, GroupLoggInn, 0, 10)
        End With
        IsLoaded = True
    End Sub
    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        If LayoutHelper IsNot Nothing Then
            With LayoutHelper
                .CenterSurface(GroupLoggInn, Me)
                .CenterSurface(PicLoadingSurface, GroupLoggInn, 0, 10)
            End With
        End If
    End Sub
    Private Sub LoginValid()
        Hide()
        LoadingGraphics.StopSpin()
        For Each C As Control In GroupLoggInn.Controls
            'If Not .GetType = GetType(FullWidthControl) Then
            C.Show()
            'End If
        Next
        FWButton.Enabled = True
        BliMedButton.Enabled = True
        Parent.Index = 1
        'Testdashbord.Show()
        txtBrukernavn.Clear()
        txtPassord.Clear()
    End Sub
    Private Sub LoginInvalid(ByVal ErrorOccurred As Boolean, ErrorMessage As String)
        LoadingGraphics.StopSpin()
        SuspendLayout()
        For Each C As Control In GroupLoggInn.Controls
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
        LayoutHelper.SlideToHeight(40)
        If ErrorOccurred Then
            NotifManager.Display("Tilkoblingen mislyktes. Vennligst prøv igjen senere, og verifiser at du har internettilgang.", NotificationPreset.RedAlert)
        Else
            NotifManager.Display("Brukernavnet eller passordet er feil", NotificationPreset.RedAlert)
        End If
    End Sub
    Protected Overrides Sub OnClosed(e As TabClosingEventArgs)
        MyBase.OnClosed(e)
        End
    End Sub
    Private Sub FWButton_Click(sender As Object, e As EventArgs) Handles FWButton.Click
        SuspendLayout()
        For Each C As Control In GroupLoggInn.Controls
            C.Hide()
        Next
        GroupHeader.Show()
        FWButton.Enabled = False
        BliMedButton.Enabled = False
        UserLogin.LoginAsync(txtBrukernavn.Text, txtPassord.Text, "DonorKonti", "bruker_ID", "passord")
        RefreshLayout()
        LoadingGraphics.Spin(30, 10)
        ResumeLayout()
    End Sub
    Private Sub BliMedButton_Click(Sender As Object, e As EventArgs) Handles BliMedButton.Click
        Parent.Index = 3
    End Sub
End Class

Public Class SecondTab
    Inherits Tab
    Dim WithEvents GM As GridMenu(Of Label)
    Dim LayoutHelper As New FormLayoutTools(Me)
    Public Sub New(Window As MultiTabWindow)
        MyBase.New(Window)
        Hide()
        GM = New GridMenu(Of Label)(3, 3, 200, 100, 20)
        GM.Parent = Me
        Dim TextArr() As String = {"Mine timer", "Forny egenerklæring", "Personopplysninger", "Hjelp", "Kontaktskjema", "Test6", "Test7", "Test8", "Test9"}
        Dim iLast As Integer = GM.Count - 1
        For i As Integer = 0 To iLast
            With GM.Item(i)
                .BackColor = Color.FromArgb(0, 80, 110)
                .TextAlign = ContentAlignment.MiddleCenter
                .ForeColor = Color.White
                .Font = New Font(.Font.FontFamily, 12)
                .Text = TextArr(i)
            End With
        Next
        LayoutHelper.CenterOnForm(GM)
        GM.DrawGradient = True
        GM.Display()
    End Sub
    Protected Overrides Sub OnClosing(e As TabClosingEventArgs)
        Dim Result As MsgBoxResult = MsgBox("Vil du logge ut?", MsgBoxStyle.YesNo, "Er du sikker?")
        If Result = MsgBoxResult.Yes Then
            'If Testspørreskjema IsNot Nothing Then
            '    Testspørreskjema.Hide()
            'End If
            Hide()
            Testlogginn.Show()
        End If
        e.Cancel = True
        MyBase.OnClosing(e)
    End Sub
    Protected Overrides Sub OnResize(e As EventArgs)
        If LayoutHelper IsNot Nothing Then
            LayoutHelper.CenterOnForm(GM)
        End If
    End Sub
    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        Select Case keyData
            Case Keys.Up
                GM.SelectDirection(GridSelectDirection.Up)
                Return True
            Case Keys.Right
                GM.SelectDirection(GridSelectDirection.Right)
                Return True
            Case Keys.Down
                GM.SelectDirection(GridSelectDirection.Down)
                Return True
            Case Keys.Left
                GM.SelectDirection(GridSelectDirection.Left)
                Return True
        End Select
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function
    Private Sub OnGridSelectionChanged(Sender As Label, Selected As Boolean, ItemIndex As Integer) Handles GM.SelectionChanged
        Select Case Selected
            Case True
                Sender.BackColor = ColorHelper.Multiply(Color.FromArgb(0, 80, 110), 1.33)
            Case Else
                Sender.BackColor = Color.FromArgb(0, 80, 110)
        End Select
    End Sub
    Private Sub OnGridClick(Sender As Label, ItemIndex As Integer) Handles GM.ItemClicked
        Select Case ItemIndex
            Case 0
                'Testoversikt.Show()
                'Hide()
            Case 1
                Parent.Index = 2
            Case 2
                Parent.Index = 3
        End Select
    End Sub
End Class

Public Class ThirdTab
    Inherits Tab
    Dim Spørreskjema As Questionnaire
    Dim LayoutTool As New FormLayoutTools(Me)
    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        If LayoutTool IsNot Nothing Then
            LayoutTool.CenterSurfaceH(Spørreskjema, Me)
        End If
    End Sub
    Public Sub New(Parent As MultiTabWindow)
        MyBase.New(Parent)
        ByggSkjema()
        LayoutTool.CenterSurfaceH(Spørreskjema, Me)
    End Sub
    Private Sub ByggSkjema()
        Dim TestForm1 As New FlatForm(400, 300, 10)
        With TestForm1
            .SuspendLayout()
            .AddField(FormElementType.CheckBox, 120)
            .AddField(FormElementType.Label)
            .AddField(FormElementType.CheckBox, 120)
            .AddField(FormElementType.CheckBox, 160)
            .AddField(FormElementType.CheckBox)
            .AddField(FormElementType.TextField)
            .AddField(FormElementType.Radio, 120, True)
            .AddField(FormElementType.Radio)
            .AddRadioContext(True)
            .AddField(FormElementType.Radio, 120, True)
            .AddField(FormElementType.Radio)
            .Field(1, 2).Extrude(FieldExtrudeSide.Left, 10)
            .Field(0, 1).Value = "Dette er et eksempel på både vertikal og horisontal sammensmelting." & vbNewLine & " "
            .Field(1, 2).SecondaryValue = "Test"
            .MergeWithAbove(1, 1)
            .MergeWithAbove(1, 2, -1)
            With .Field(2, 0)
                .Value = "Hei"
                .Header.Text = "Har du noen ekstra kommentarer?"
            End With
            .ResumeLayout()
            '.HeightToContent()
        End With
        Dim TestForm2 As New FlatForm(400, 300, 10)
        With TestForm2
            .SuspendLayout()
            .AddField(FormElementType.Label, 260)
            .AddField(FormElementType.CheckBox)
            .AddField(FormElementType.CheckBox, 100)
            .AddField(FormElementType.CheckBox, 150)
            .AddField(FormElementType.Radio)
            .AddField(FormElementType.Label)
            .AddField(FormElementType.Label)
            .AddField(FormElementType.Radio)
            .AddField(FormElementType.Radio, 120)
            .AddField(FormElementType.Radio)
            .Field(1, 1).Extrude(FieldExtrudeSide.Left, 10)
            .Field(0, 0).Value = "Vi kan lage helt vilkårlige skjema med stor stilistisk frihet." & vbNewLine & " "
            With .Field(0, 1)
                With .Header
                    .BackColor = Color.DeepPink
                    .Text = "Eksempel"
                    .ForeColor = ColorHelper.Multiply(.ForeColor, 0.4)
                End With
                .SecondaryValue = "Stilistisk frihet"
                .BackColor = Color.HotPink
                .Value = True
            End With
            .MergeWithAbove(1, 0)
            .MergeWithAbove(1, 1, -1)
            With .Field(2, 0)
                .Value = "Ayy"
                .Header.Text = "Faktisk skjema kommer snart"
            End With
            .ResumeLayout()
            '.HeightToContent()
        End With

        Spørreskjema = New Questionnaire(Me)
        'PaintMessageHelper.SuspendDrawing(Spørreskjema)
        With Spørreskjema
            .Width = 500
            .Height = 500
            .Top = 0
            .Add(TestForm1)
            .Add(TestForm2)
            .Display()
        End With
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
                Parent.Index = 4
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
                .MinLength = 6
                .MaxLength = 50
                AddHandler .ValueChanged, AddressOf PasswordChanged
                AddHandler .ValidChanged, AddressOf PasswordValidChanged
                FieldHeight = .Height - .Header.Bottom
            End With
            With DirectCast(.Last, FlatForm.FormTextField)
                .PlaceHolder = "Velg et passord"
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
        'Dim NB As New Button
        'With NB
        '    .Hide()
        '    .Size = New Size(100, 100)
        '    .Location = Point.Empty
        '    .Parent = Me
        '    .Enabled = False
        'End With

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
            .BackColor = Color.FromArgb(162, 25, 51)
            .ForeColor = Color.White
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
        Beep()
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
        Parent.Index = 4
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

'Public Class OppdaterInfo
'    Inherits Tab
'    Private Personalia As New FlatForm(400, 300, 3, FormFieldStylePresets.PlainWhite)
'    Private PasswordForm As New FlatForm(200, 100, 3, FormFieldStylePresets.PlainWhite)
'    Private TopBar As New TopBar(Me)
'    Private FormPanel As New BorderControl(Color.FromArgb(210, 210, 210))
'    Private PicDoktor, PicDoktorPassord As New PictureBox
'    Private FormInfo As New Label
'    Private InfoLab As New InfoLabel
'    Private WithEvents SendKnapp As New TopBarButton(FormPanel, My.Resources.OKIcon, "Meld meg inn", New Size(135, 36))
'    Private AvbrytKnapp As New TopBarButton(FormPanel, My.Resources.AvbrytIcon, "Avbryt", New Size(135, 36), True)

'    Private LayoutTool As New FormLayoutTools(Me)
'    Private Footer As New Footer(Me, Color.FromArgb(54, 68, 78), 40)
'    Private WithEvents DBC As New DatabaseClient(Credentials.Server, Credentials.Database, Credentials.UserID, Credentials.Password)
'    Private FirstHeader As New FullWidthControl(FormPanel)

'    Private NotifManager As New NotificationManager(FirstHeader)

'    Private Sub SendClick() Handles SendKnapp.Click
'        Dim Result() As HeaderValuePair = Personalia.Result
'        Dim DataArr(11) As String
'        For i As Integer = 0 To 4
'            DataArr(i) = Result(i).Value.ToString
'        Next
'        If DirectCast(Result(5).Value, Boolean) Then
'            DataArr(5) = "1"
'        Else
'            DataArr(5) = "0"
'        End If
'        For i As Integer = 7 To 10
'            DataArr(i - 1) = Result(i).Value.ToString
'        Next
'        If DirectCast(Result(11).Value, Boolean) Then
'            DataArr(10) = "1"
'        Else
'            DataArr(10) = "0"
'        End If
'        If DirectCast(Result(12).Value, Boolean) Then
'            DataArr(11) = "1"
'        Else
'            DataArr(11) = "0"
'        End If
'        DBC.SQLQuery = "INSERT INTO Blodgiver (b_fodselsnr, b_fornavn, b_etternavn, b_telefon1, b_telefon2, b_telefon3, b_epost, b_adresse, b_postnr, b_kjonn, send_epost, send_sms) VALUES (@fodselsnr, @b_fornavn, @b_etternavn, @b_telefon1, @b_telefon2, @b_telefon3, @b_epost, @b_adresse, @b_postnr, @b_kjonn, @send_epost, @send_sms);"
'        DBC.Execute({"@fodselsnr", "@b_fornavn", "@b_etternavn", "@b_adresse", "@b_postnr", "@b_kjonn", "@b_telefon1", "@b_telefon2", "@b_telefon3", "@b_epost", "@send_epost", "@send_sms"}, DataArr)
'    End Sub
'    Private Sub DBC_Finished(Sender As Object, e As DatabaseListEventArgs) Handles DBC.ListLoaded
'        If e.ErrorOccurred Then
'            NotifManager.Display("Noe gikk galt! Vennligst se over skjemaet og forsikre deg om at alle obligatoriske felt er fylt ut.", NotificationPreset.OffRedAlert)
'        Else
'            Personalia.Hide()
'            PasswordForm.Show()
'            PicDoktor.Hide()
'            PicDoktorPassord.Show()
'            NotifManager.Display("Du er nå registrert i vårt system!", NotificationPreset.GreenSuccess)
'        End If
'    End Sub
'    Private Sub DBC_Failed() Handles DBC.ExecutionFailed
'        NotifManager.Display("Noe gikk galt! Vennligst se over skjemaet og forsikre deg om at alle obligatoriske felt er fylt ut.", NotificationPreset.OffRedAlert)
'    End Sub
'    Public Shadows Sub Show()
'        FormPanel.Hide()
'        MyBase.Show()
'    End Sub
'    Private Sub Me_VisibleChanged() Handles Me.VisibleChanged
'        If Visible Then
'            FormPanel.Show()
'        End If
'    End Sub
'    Public Sub New(Window As MultiTabWindow)
'        MyBase.New(Window)
'        DoubleBuffered = True
'        SuspendLayout()
'        BackColor = Color.FromArgb(240, 240, 240)
'        With FormPanel
'            .Hide()
'            .Parent = Me
'            .Top = TopBar.Bottom + 20
'            .Left = 30
'            .Width = 817
'            .Height = 480
'            .BackColor = Color.FromArgb(225, 225, 225)
'        End With
'        With FirstHeader
'            .Width = 817
'            .Height = 40
'            .Text = "Registrering"
'            .BackColor = Color.FromArgb(183, 187, 191)
'            .ForeColor = Color.White
'        End With
'#Region "Form"
'        With Personalia
'            .NewRowHeight = 50
'            .AddField(FormElementType.TextField, 180)
'            .Last.Header.Text = "Fødselsnummer (11 siffer)"
'            .AddField(FormElementType.TextField, 107)
'            .Last.Header.Text = "Fornavn"
'            .AddField(FormElementType.TextField)
'            .Last.Header.Text = "Etternavn"
'            .AddField(FormElementType.TextField, 290)
'            With .Last
'                .Header.Text = "Privatadresse"
'            End With
'            .AddField(FormElementType.TextField)
'            With .Last
'                .Header.Text = "Postnummer"
'            End With
'            .NewRowHeight = 50
'            .AddField(FormElementType.Radio, 200)
'            With .Last
'                .Value = True
'                .Header.Text = "Kjønn"
'                .SecondaryValue = "Jeg er mann"
'                .DrawBorder(FormField.ElementSide.Right) = False
'            End With
'            .AddField(FormElementType.Radio)
'            With .Last
'                .Value = True
'                .SecondaryValue = "Jeg er kvinne"
'                .DrawBorder(FormField.ElementSide.Left) = False
'                .DrawDashedSepararators(FormField.ElementSide.Left) = True
'                .Extrude(FieldExtrudeSide.Left, 3)
'                .DrawDotsOnHeader = False
'            End With
'            .AddField(FormElementType.TextField, 133)
'            With .Last
'                .Header.Text = "Telefon privat"
'            End With
'            .AddField(FormElementType.TextField, 133)
'            With .Last
'                .Header.Text = "Telefon mobil"
'            End With
'            .AddField(FormElementType.TextField)
'            With .Last
'                .Header.Text = "Telefon arbeid"
'            End With
'            .AddField(FormElementType.TextField)
'            With .Last
'                .Header.Text = "Epost-adresse"
'            End With
'            .AddField(FormElementType.CheckBox)
'            .NewRowHeight = 40
'            With .Last
'                .SecondaryValue = "Jeg ønsker å motta innkalling, påminnelser og informasjon via epost"
'                .DrawBorder(FormField.ElementSide.Bottom) = False
'            End With
'            .AddField(FormElementType.CheckBox)
'            With .Last
'                .SecondaryValue = "Jeg ønsker å motta innkalling, påminnelser og informasjon via SMS"
'                .DrawBorder(FormField.ElementSide.Top) = False
'                .DrawDashedSepararators(FormField.ElementSide.Top) = True
'                .SwitchHeader(False)
'            End With
'            .MergeWithAbove(6, 0, 0, True)
'            .Parent = FormPanel
'            .Display()
'            .Location = New Point(20, 60)
'        End With
'#End Region
'        With PasswordForm
'            .AddField(FormElementType.Label)
'            With .Last
'                .Header.Text = "Bruker-ID"
'                .Value = "Fødselsnummer"
'            End With
'            .AddField(FormElementType.TextField)
'            With .Last
'                .Header.Text = "Velg et passord"
'            End With
'            .Parent = FormPanel
'            .Display()
'        End With
'        Dim NB As New Button
'        With NB
'            .Size = New Size(100, 100)
'            .Location = Point.Empty
'            .Parent = Me
'            .Show()
'            .Enabled = False
'            .Hide()
'        End With
'        With TopBar
'            .AddButton(My.Resources.TimeBestillingIcon, "Bestill ny time", New Size(135, 36))
'            .AddButton(My.Resources.EgenerklaeringIcon, "Registrer egenerklæring", New Size(135, 36))
'            .AddButton(My.Resources.RedigerProfilIcon, "Rediger profil", New Size(135, 36))
'            .AddLogout("Logg ut", New Size(135, 36))
'        End With
'        With SendKnapp
'            .Top = Personalia.Bottom + 10
'            .Left = Personalia.Right - .Width
'        End With
'        With AvbrytKnapp
'            .BackColor = Color.FromArgb(162, 25, 51)
'            .ForeColor = Color.White
'            .Top = SendKnapp.Top
'            .Left = SendKnapp.Left - .Width - 10
'        End With
'        With PicDoktor
'            .BackgroundImage = My.Resources.Doktor2
'            .Size = .BackgroundImage.Size
'            .Parent = FormPanel
'            .Top = Personalia.Bottom - .Height
'            .Left = Personalia.Right + 20
'        End With
'        With PicDoktorPassord
'            .BackgroundImage = My.Resources.DoktorPassord
'            .Size = .BackgroundImage.Size
'            .Parent = FormPanel
'            .Location = PicDoktor.Location
'        End With
'        With PasswordForm
'            .Location = New Point(PicDoktorPassord.Left \ 2 - .Width \ 2)
'            .Top = PicDoktorPassord.Bottom - 210
'        End With
'        With FormInfo
'            .Parent = FormPanel
'            .Top = Personalia.Bottom + 10
'            .Left = Personalia.Left
'            .AutoSize = False
'            .Height = SendKnapp.Height
'            .Width = AvbrytKnapp.Left - .Left
'            .TextAlign = ContentAlignment.MiddleLeft
'            .ForeColor = Color.FromArgb(80, 80, 80)
'            .Text = "* markerer obligatoriske felt"
'        End With
'        With InfoLab
'            .Parent = FormPanel
'            .Top = PicDoktor.Bottom + 10
'            .Left = PicDoktor.Left
'            .Width = PicDoktor.Width
'            .Height = SendKnapp.Height
'            .BackColor = Color.Red
'            .Text = "Trenger du mer informasjon?" & vbNewLine & "Besøk www.GiBlod.no"
'        End With
'        FormPanel.Show()
'        ResumeLayout()
'    End Sub
'    Protected Overrides Sub OnResize(e As EventArgs)
'        SuspendLayout()
'        MyBase.OnResize(e)
'        ' TODO: Remove LayoutTool as it is not used
'        If LayoutTool IsNot Nothing Then
'            With FormPanel
'                .Top = TopBar.Bottom + 20
'                .Left = Width \ 2 - .Width \ 2
'                .Top = TopBar.Bottom + (Height - TopBar.Bottom - Footer.Height) \ 2 - .Height \ 2
'            End With
'        End If
'        ResumeLayout(True)
'    End Sub
'End Class

'Public NotInheritable Class EncryptedReadWrite
'    Implements IDisposable
'    Private TripleDes As New TripleDESCryptoServiceProvider
'    Private Function TruncateHash(ByVal key As String, ByVal length As Integer) As Byte()
'        Dim sha1 As New SHA1CryptoServiceProvider
'        Dim keyBytes() As Byte = System.Text.Encoding.Unicode.GetBytes(key)
'        Dim hash() As Byte = sha1.ComputeHash(keyBytes)
'        ReDim Preserve hash(length - 1)
'        Return hash
'    End Function
'    Sub New(ByVal key As String)
'        TripleDes.Key = TruncateHash(key, TripleDes.KeySize \ 8)
'        TripleDes.IV = TruncateHash("", TripleDes.BlockSize \ 8)
'    End Sub
'    Public Function EncryptData(ByVal plaintext As String) As String
'        Dim plaintextBytes() As Byte = Text.Encoding.Unicode.GetBytes(plaintext)
'        Dim ms As New IO.MemoryStream
'        Dim encStream As New CryptoStream(ms, TripleDes.CreateEncryptor(), CryptoStreamMode.Write)
'        encStream.Write(plaintextBytes, 0, plaintextBytes.Length)
'        encStream.FlushFinalBlock()
'        Return Convert.ToBase64String(ms.ToArray)
'    End Function
'    Public Function DecryptData(ByVal encryptedtext As String) As String
'        Try
'            Dim encryptedBytes() As Byte = Convert.FromBase64String(encryptedtext)
'            Dim ms As New IO.MemoryStream
'            Dim decStream As New CryptoStream(ms, TripleDes.CreateDecryptor(), CryptoStreamMode.Write)
'            decStream.Write(encryptedBytes, 0, encryptedBytes.Length)
'            Try
'                decStream.FlushFinalBlock()
'            Catch
'                decStream.Dispose()
'            End Try
'            Return Text.Encoding.Unicode.GetString(ms.ToArray)
'        Catch ex As System.Security.Cryptography.CryptographicException
'                Return Nothing
'        End Try
'    End Function
'#Region "IDisposable Support"
'    Private disposedValue As Boolean
'    Protected Sub Dispose(disposing As Boolean)
'        If Not disposedValue Then
'            If disposing Then
'                TripleDes.Dispose()
'            End If
'        End If
'        disposedValue = True
'    End Sub
'    Public Sub Dispose() Implements IDisposable.Dispose
'        Dispose(True)
'    End Sub
'#End Region
'End Class

'Public NotInheritable Class CredentialsManager
'    Private DefPath As String
'    Public Sub New(Optional DefaultPath As String = "Default")
'        If Not DefaultPath = "Default" Then
'            DefPath = DefaultPath
'        Else
'            DefPath = Application.StartupPath & "\test\"
'        End If

'        If (Not System.IO.Directory.Exists(DefPath)) Then
'            System.IO.Directory.CreateDirectory(DefPath)
'        End If
'        If Not File.Exists(DefPath & "\test.txt") Then
'            File.Create(DefPath & "\test.txt")
'        End If
'    End Sub
'    Public Sub Encode(ByVal Value As String, ByVal Password As String)
'        Dim wrapper As New EncryptedReadWrite(Password)
'        Dim cipherText As String = wrapper.EncryptData(Value)
'        MsgBox("The cipher text is: " & cipherText)
'        My.Computer.FileSystem.WriteAllText(DefPath & "\test.txt", cipherText, False)
'        wrapper.Dispose()
'    End Sub
'    Public Function Decode(ByVal Password As String) As String
'        Dim cipherText As String = My.Computer.FileSystem.ReadAllText(DefPath & "\test.txt")
'        Dim wrapper As New EncryptedReadWrite(Password)

'        ' DecryptData throws if the wrong password is used.
'        Try
'            Dim plainText As String = wrapper.DecryptData(cipherText)
'            Return plainText
'        Catch ex As System.Security.Cryptography.CryptographicException
'            Return Nothing
'        Finally
'            wrapper.Dispose()
'        End Try
'    End Function
'End Class

Public Class LoggInnNy
    Inherits Tab

    Private LoginForm As New FlatForm(243, 100, 3, New FormFieldStyle(Color.FromArgb(245, 245, 245), Color.FromArgb(70, 70, 70), Color.White, Color.FromArgb(80, 80, 80), Color.White, Color.Black, {True, True, True, True}, 20))
    Private WithEvents TopBar As New TopBar(Me)
    Private FormPanel As New BorderControl(Color.FromArgb(210, 210, 210))
    Private PicSideInfo, PicInfoAbove, RightSide, Gear As New PictureBox
    Private FormInfo As New Label
    Private InfoLab As New InfoLabel
    Private WithEvents LoggInnKnapp As New TopBarButton(FormPanel, My.Resources.NesteIcon, "Logg inn", New Size(0, 36))
    Private OpprettBrukerKnapp As New TopBarButton(FormPanel, My.Resources.RedigerProfilIcon, "Opprett bruker", New Size(0, 36))
    Private LayoutTool As New FormLayoutTools(Me)
    Private Footer As New Footer(Me)
    'Private WithEvents DBC As New DatabaseClient(Credentials.Server, Credentials.Database, Credentials.UserID, Credentials.Password)
    Private WithEvents UserLogin As New MySqlUserLogin(Credentials.Server, Credentials.Database, Credentials.UserID, Credentials.Password)
    Private FormHeader As New FullWidthControl(FormPanel)
    Private NotifManager As New NotificationManager(FormHeader)
    Private LeftSide As New BorderControl(Color.FromArgb(210, 210, 210))
    Private PersonalNumber As String
    Private Sub LoginValid()
        CurrentLogin = New UserInfo(PersonalNumber)
        Dashboard.Initiate()
        Parent.Index = 5
        Egenerklæring.InitiateForm()
        LoginForm.ClearAll()
        If HentTimer_DBC IsNot Nothing Then
            HentTimer_DBC.Dispose()
        End If
        HentTimer_DBC = New DatabaseClient(Credentials.Server, Credentials.Database, Credentials.UserID, Credentials.Password)
        With HentTimer_DBC
            .SQLQuery = "SELECT time_id, t_dato, t_klokkeslett FROM Time WHERE b_fodselsnr = @nr;"
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
                .MinLength = 6
                .MaxLength = 50
            End With
            With DirectCast(.Last, FlatForm.FormTextField)
                .PlaceHolder = "Fødselsnummer"
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
            .BackgroundImage = My.Resources.SettingsIcon
            .Size = .BackgroundImage.Size
            .Parent = FormHeader
            .Location = New Point(FormHeader.Width - .Width - 5, FormHeader.Height \ 2 - .Height \ 2)
        End With
        FormPanel.Show()
        ResumeLayout()
    End Sub
    Private Sub OpprettBruker_Click(sender As Object, e As EventArgs)
        Parent.Index = 3
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

Public Class LoggInn
    Inherits Tab
    Private LayoutHelper As New FormLayoutTools(Me)
    Private NotifManager As New NotificationManager(Me)
    Private logo As HemoGlobeLogo
    Private WithEvents LoginBox As LoginForm
    Public Sub New(ParentWindow As MultiTabWindow)
        MyBase.New(ParentWindow)
        BackColor = Color.FromArgb(240, 238, 235)
        Size = New Size(1280, 720)
        logo = New HemoGlobeLogo
        logo.Parent = Me
        LoginBox = New LoginForm
        LoginBox.Parent = Me
        LayoutHelper.CenterOnForm(LoginBox)
    End Sub
    'Protected Overrides Sub OnClosed(e As EventArgs)
    '    MyBase.OnClosed(e)
    '    End
    'End Sub
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
    Private Sub BliMedClicked(Sender As Object, e As EventArgs) Handles LoginBox.BliMedClicked
        Parent.Index = 3
    End Sub
End Class

Public Class DashboardTab
    Inherits Tab
    ' TODO: Lag tannhjulklasse
    Public NotificationList As New UserNotificationContainer(Color.FromArgb(210, 210, 210))
    Private Header As New TopBar(Me)
    'Dim ScrollList As New Donasjoner(Me)
    Private WithEvents Beholder As New BlodBeholder(My.Resources.Tom_beholder, My.Resources.Full_beholder)
    Private WelcomeLabel As New InfoLabel(True, Direction.Right)
    Private current As Integer
    Private OrganDonorInfo As New PictureBox
    Private increment As Boolean = True
    Private IsLoaded As Boolean
    Private WithEvents DBC As New DatabaseClient(Credentials.Server, Credentials.Database, Credentials.UserID, Credentials.Password)
    Private Sub TopBarButtonClick(sender As Object, e As EventArgs)
        Dim SenderButton As TopBarButton = DirectCast(sender, TopBarButton)
        If SenderButton.IsLogout Then
            Parent.Index = 4
            Logout()
        Else
            Select Case CInt(SenderButton.Tag)
                Case 0
                    Parent.Index = 7
                Case 1
                    Parent.Index = 6
                Case 2
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
        End If
        ResumeLayout(True)
    End Sub
    Protected Overrides Sub Dispose(disposing As Boolean)
        RemoveHandler Header.ButtonClick, AddressOf TopBarButtonClick
        MyBase.Dispose(disposing)
    End Sub
End Class