Option Strict On
Option Explicit On
Option Infer Off
Imports System.ComponentModel
Imports System.IO
Imports System.Security.Cryptography
Imports AudiopoLib

Public Class Main
    Private IsLoaded As Boolean = False
    Protected Friend Windows As MultiTabWindow

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

            FirstTabTest = New FirstTab(Windows)
            SecondTabTest = New SecondTab(Windows)
            ThirdTabTest = New ThirdTab(Windows)
            PersonaliaTest = New Personopplysninger(Windows)
            With Windows
                .BackColor = Color.White
                .Index = 0
            End With
            Width = 800
            Height = 500
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
    Private Sub LoginInvalid(ByVal ErrorOccurred As Boolean)
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
            If Testspørreskjema IsNot Nothing Then
                Testspørreskjema.Hide()
            End If
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
            .AddRadioContext()
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
        PaintMessageHelper.SuspendDrawing(Spørreskjema)
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
    Dim Personalia As New FlatForm(400, 300, 0)
    Dim LayoutTool As New FormLayoutTools(Me)
    Public ReadOnly Property Result() As FlatFormResult
        Get
            Return Personalia.Result
        End Get
    End Property
    Public Sub New(Window As MultiTabWindow)
        MyBase.New(Window)
        With Personalia
            .AddField(FormElementType.Label)
            .Last.Value = "Vennligst fyll ut følgende informasjon. Du vil bli bedt om å bekrefte opplysningene ved ny timeforespørsel, og med signatur ved oppmøte."
            .LastRow.Height = 60
            .NewRowHeight = 50

            .AddField(FormElementType.TextField, 180)
            .Last.Header.Text = "Fødselsnummer (11 siffer)"
            .AddField(FormElementType.TextField, 110)
            .Last.Header.Text = "Fornavn"
            .Last.DrawDashedSepararators(FormField.ElementSide.Left) = True
            .AddField(FormElementType.TextField)
            .Last.Header.Text = "Etternavn"
            .Last.DrawDashedSepararators(FormField.ElementSide.Left) = True
            .AddField(FormElementType.TextField, 290)
            With .Last
                .Header.Text = "Privatadresse"
            End With
            .AddField(FormElementType.TextField)
            With .Last
                .Header.Text = "Postnummer"
                .DrawDashedSepararators(FormField.ElementSide.Left) = True
            End With
            .NewRowHeight = 30
            .AddField(FormElementType.Label)
            With .Last
                .Value = "Fyll ut minst en"
                .SwitchHeader(False)
            End With
            .NewRowHeight = 50
            .AddField(FormElementType.TextField, 133)
            With .Last
                .Header.Text = "Telefon privat"
            End With
            .AddField(FormElementType.TextField, 133)
            With .Last
                .Header.Text = "Telefon mobil"
                .DrawDashedSepararators(FormField.ElementSide.Left) = True
            End With
            .AddField(FormElementType.TextField)
            With .Last
                .Header.Text = "Telefon arbeid"
                .DrawDashedSepararators(FormField.ElementSide.Left) = True
            End With
            .AddField(FormElementType.TextField)
            Dim BG As Color = .Last.BackColor
            With .Last
                .Header.Text = "Epost-adresse"
            End With
            .AddField(FormElementType.Label, 240)
            With .Last
                .DrawDashedSepararators(FormField.ElementSide.Top) = True
                .SwitchHeader(False)
                .BackColor = BG
                .DrawGradient = False
                .Value = "Tillater du at blodbanken sender innkalling, påminnelser og informasjon via epost?"
            End With
            .AddField(FormElementType.Radio, 80)
            With .Last
                .BackColor = BG
                .SwitchHeader(False)
                .DrawGradient = False
                .DrawDashedSepararators(FormField.ElementSide.Left) = True
                .DrawDashedSepararators(FormField.ElementSide.Top) = True
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .BackColor = BG
                .SwitchHeader(False)
                .DrawGradient = False
                .DrawDashedSepararators(FormField.ElementSide.Top) = True
                .SecondaryValue = "Nei"
            End With
            .AddField(FormElementType.Label, 240)
            With .Last
                .DrawDashedSepararators(FormField.ElementSide.Top) = True
                .SwitchHeader(False)
                .BackColor = BG
                .DrawGradient = False
                .Value = "Tillater du at blodbanken sender innkalling, påminnelser og informasjon via SMS?"
            End With
            .AddField(FormElementType.Radio, 80)
            With .Last
                .BackColor = BG
                .SwitchHeader(False)
                .DrawGradient = False
                .DrawDashedSepararators(FormField.ElementSide.Left) = True
                .DrawDashedSepararators(FormField.ElementSide.Top) = True
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .BackColor = BG
                .SwitchHeader(False)
                .DrawGradient = False
                .DrawDashedSepararators(FormField.ElementSide.Top) = True
                .SecondaryValue = "Nei"
            End With
            .Parent = Me
            .Display()
        End With
        LayoutTool.CenterSurface(Personalia, Me)
        Dim NB As New Button
        With NB
            .Size = New Size(100, 100)
            .Location = Point.Empty
            .Parent = Me
            .Show()
            .Enabled = False
            .Hide()
        End With
    End Sub
    Private DBC As New DatabaseClient(Credentials.Server, Credentials.Database, Credentials.UserID, Credentials.Password)
    Public Sub Execute(Sender As Object, e As EventArgs)
        Dim Ret As FlatFormResult = Result()
        Dim RetX() As HeaderValuePair = Ret.GetAllSeries
        Dim ParamList As New List(Of String)
        Dim RadioCounter As Integer = 0
        For i As Integer = 0 To RetX.Count - 1
            If RetX(i).Type = FormElementType.TextField Then
                ParamList.Add(RetX(i).Value.ToString)
            ElseIf RetX(i).Type = FormElementType.Radio Then
                RadioCounter += 1
                If RadioCounter = 1 OrElse RadioCounter = 3 Then
                    If DirectCast(RetX(i).Value, Boolean) = True Then
                        ParamList.Add("1")
                    Else
                        ParamList.Add("0")
                    End If
                End If
            End If
        Next
        DBC.SQLQuery = "INSERT INTO 'g_oops_21'.'BLODGIVER' ('b_fodselsnr', 'b_fornavn', 'b_etternavn', 'b_telefon1', 'b_telefon2', 'b_telefon3', 'b_epost', 'b_adresse', 'b_postnr', 'b_kjonn', 'send_epost', 'send_sms') VALUES (@fodselsnr, @b_fornavn, @b_etternavn, @b_telefon1, @b_telefon2, @b_telefon3, @b_epost, @b_adresse, @b_postnr, @b_kjonn, @send_epost, @send_sms);"
        DBC.Execute({"@fodselsnr", "@b_fornavn", "@b_etternavn", "@b_telefon1", "@b_telefon2", "@b_telefon3", "@b_epost", "@b_adresse", "@b_postnr", "@b_kjonn", "@send_epost", "@send_sms"}, ParamList.ToArray, True)
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        If LayoutTool IsNot Nothing Then
            LayoutTool.CenterSurface(Personalia, Me)
        End If
    End Sub
End Class


Public NotInheritable Class EncryptedReadWrite
    Implements IDisposable

    Private TripleDes As New TripleDESCryptoServiceProvider
    Private Function TruncateHash(ByVal key As String, ByVal length As Integer) As Byte()
        Dim sha1 As New SHA1CryptoServiceProvider
        Dim keyBytes() As Byte = System.Text.Encoding.Unicode.GetBytes(key)
        'Dim hashList As New List(Of Byte)(sha1.ComputeHash(keyBytes))
        Dim hash() As Byte = sha1.ComputeHash(keyBytes)

        ' Truncate or pad the hash. ?????
        ReDim Preserve hash(length - 1)
        Return hash
    End Function
    Sub New(ByVal key As String)
        TripleDes.Key = TruncateHash(key, TripleDes.KeySize \ 8)
        TripleDes.IV = TruncateHash("", TripleDes.BlockSize \ 8)
    End Sub
    Public Function EncryptData(ByVal plaintext As String) As String
        ' Convert the plaintext string to a byte array.
        Dim plaintextBytes() As Byte = Text.Encoding.Unicode.GetBytes(plaintext)

        ' Create the stream.
        Dim ms As New IO.MemoryStream
        ' Create the encoder to write to the stream.
        Dim encStream As New CryptoStream(ms, TripleDes.CreateEncryptor(), CryptoStreamMode.Write)

        ' Use the crypto stream to write the byte array to the stream.
        encStream.Write(plaintextBytes, 0, plaintextBytes.Length)
        encStream.FlushFinalBlock()

        ' Convert the encrypted stream to a printable string.
        Return Convert.ToBase64String(ms.ToArray)
    End Function
    Public Function DecryptData(ByVal encryptedtext As String) As String
        Try
            ' Convert the encrypted text string to a byte array.
            Dim encryptedBytes() As Byte = Convert.FromBase64String(encryptedtext)

            ' Create the stream.
            Dim ms As New IO.MemoryStream
            ' Create the decoder to write to the stream.
            Dim decStream As New CryptoStream(ms, TripleDes.CreateDecryptor(), CryptoStreamMode.Write)

            ' Use the crypto stream to write the byte array to the stream.
            decStream.Write(encryptedBytes, 0, encryptedBytes.Length)
            Try
                decStream.FlushFinalBlock()
            Catch
                decStream.Dispose()
            End Try
            ' Convert the plaintext stream to a string.
            Return Text.Encoding.Unicode.GetString(ms.ToArray)
            Catch ex As System.Security.Cryptography.CryptographicException
                Return Nothing
        End Try
    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                TripleDes.Dispose()
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        disposedValue = True
    End Sub
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        ' GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class

Public NotInheritable Class CredentialsManager
    Private DefPath As String
    Public Sub New(Optional DefaultPath As String = "Default")
        If Not DefaultPath = "Default" Then
            DefPath = DefaultPath
        Else
            DefPath = Application.StartupPath & "\test\"
        End If

        If (Not System.IO.Directory.Exists(DefPath)) Then
            System.IO.Directory.CreateDirectory(DefPath)
        End If
        If Not File.Exists(DefPath & "\test.txt") Then
            File.Create(DefPath & "\test.txt")
        End If
    End Sub
    Public Sub Encode(ByVal Value As String, ByVal Password As String)
        Dim wrapper As New EncryptedReadWrite(Password)
        Dim cipherText As String = wrapper.EncryptData(Value)
        MsgBox("The cipher text is: " & cipherText)
        My.Computer.FileSystem.WriteAllText(DefPath & "\test.txt", cipherText, False)
        wrapper.Dispose()
    End Sub
    Public Function Decode(ByVal Password As String) As String
        Dim cipherText As String = My.Computer.FileSystem.ReadAllText(DefPath & "\test.txt")
        Dim wrapper As New EncryptedReadWrite(Password)

        ' DecryptData throws if the wrong password is used.
        Try
            Dim plainText As String = wrapper.DecryptData(cipherText)
            Return plainText
        Catch ex As System.Security.Cryptography.CryptographicException
            Return Nothing
        Finally
            wrapper.Dispose()
        End Try
    End Function
End Class
