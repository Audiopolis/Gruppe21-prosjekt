Option Strict On
Option Explicit On
Option Infer Off

Imports System.IO
Imports System.Security.Cryptography
Imports AudiopoLib

Public Class LoggInn_Admin
    Dim LoginHelper As MySqlAdminLogin
    Dim LoadingGraphics As LoadingGraphics(Of PictureBox)
    Dim WithEvents LayoutTool As FormLayoutTools
    Dim WithEvents FWButton As FullWidthControl
    Dim NotifManager As NotificationManager
    Private Sub WhenFinished(ByVal Valid As Boolean)
        MsgBox(Valid.ToString)
    End Sub
    Private Sub LoggInn_Admin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadingGraphics = New LoadingGraphics(Of PictureBox)(PicLoadingSurface)
        LayoutTool = New FormLayoutTools(Me)
        LoginHelper = New MySqlAdminLogin("mysql.stud.iie.ntnu.no", "magnbakk")
        NotifManager = New NotificationManager(Me)
        NotifManager.AssignedLayoutManager = LayoutTool
        With LoadingGraphics
            .Stroke = 2
            .Pen.Color = Color.LimeGreen
        End With
        With LoginHelper
            .WhenFinished = AddressOf WhenFinished
        End With
        With LayoutTool
            .IncludeFormTitle = True
            .CenterOnForm(GroupLoggInn)
            .CenterSurface(PicLoadingSurface, GroupLoggInn)
        End With
        FWButton = New FullWidthControl(GroupLoggInn, True, FullWidthControl.SnapType.Bottom)
        Dim GroupHeader As New FullWidthControl(GroupLoggInn, False, FullWidthControl.SnapType.Top)
        With GroupHeader
            .Height = 20
            .BackColor = Color.FromArgb(230, 230, 230)
            .ForeColor = Color.FromArgb(100, 100, 100)
            .TextAlign = ContentAlignment.MiddleLeft
            .Padding = New Padding(5, 0, 0, 0)
            .Text = "Logg inn"
        End With
        Dim Test1 As New CredentialsManager
        Test1.TestEncoding("Hei lol", "kek")
        Test1.Decode("kek")
    End Sub
    Private Sub FWButton_Click(sender As Object, e As EventArgs) Handles FWButton.Click
        For Each C As Control In GroupLoggInn.Controls
            If Not C.GetType = GetType(FullWidthControl) Then
                C.Hide()
            End If
        Next
        FWButton.Hide()
        FWButton.Enabled = False
        LoginHelper.LoginAsync(txtBrukernavn.Text, txtPassord.Text)
        LoadingGraphics.Spin(30, 10)
        LayoutTool.Refresh()
        txtBrukernavn.Clear()
        txtPassord.Clear()
        txtBrukernavn.Focus()
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
            decStream.FlushFinalBlock()

            ' Convert the plaintext stream to a string.
            Return Text.Encoding.Unicode.GetString(ms.ToArray)
        Catch
            MsgBox("HEI LOL TEST")
            Return "Feil"
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

Public Class CredentialsManager
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
    Public Sub TestEncoding(ByVal Value As String, ByVal Password As String)
        Dim wrapper As New EncryptedReadWrite(Password)
        Dim cipherText As String = wrapper.EncryptData(Value)

        MsgBox("The cipher text is: " & cipherText)
        My.Computer.FileSystem.WriteAllText(DefPath & "\test.txt", cipherText, False)
    End Sub
    Public Sub Decode(ByVal Password As String)
        Dim cipherText As String = My.Computer.FileSystem.ReadAllText(DefPath & "\test.txt")
        Dim wrapper As New EncryptedReadWrite(Password)

        ' DecryptData throws if the wrong password is used.
        Try
            Dim plainText As String = wrapper.DecryptData(cipherText)
            MsgBox("The plain text is: " & plainText)
        Catch ex As System.Security.Cryptography.CryptographicException
            MsgBox("The data could not be decrypted with the password.")
        End Try
    End Sub
End Class
