Imports System.IO
Imports System.Windows.Forms

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