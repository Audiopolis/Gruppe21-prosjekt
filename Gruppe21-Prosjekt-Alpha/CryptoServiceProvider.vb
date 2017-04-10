﻿Imports System.Security.Cryptography
Public NotInheritable Class CryptoServiceProvider
    Private TripleDes As New TripleDESCryptoServiceProvider
    Sub New(ByVal key As String)
        TripleDes.Key = TruncateHash(key, TripleDes.KeySize \ 8)
        TripleDes.IV = TruncateHash("", TripleDes.BlockSize \ 8)
    End Sub
    Private Function TruncateHash(ByVal key As String, ByVal length As Integer) As Byte()
        Dim sha1 As New SHA1CryptoServiceProvider
        Dim hash() As Byte
        Try
            Dim keyBytes() As Byte = System.Text.Encoding.Unicode.GetBytes(key)
            hash = sha1.ComputeHash(keyBytes)
            ReDim Preserve hash(length - 1)
        Catch
            hash = Nothing
        Finally
            sha1.Dispose()
        End Try
        Return hash
    End Function
    Public Function EncryptData(ByVal plaintext As String) As String
        Dim plaintextBytes() As Byte = System.Text.Encoding.Unicode.GetBytes(plaintext)
        Dim ms As New System.IO.MemoryStream
        Dim encStream As New CryptoStream(ms, TripleDes.CreateEncryptor(), System.Security.Cryptography.CryptoStreamMode.Write)
        Dim Ret As String = Nothing
        Try
            encStream.Write(plaintextBytes, 0, plaintextBytes.Length)
            encStream.FlushFinalBlock()
            Ret = Convert.ToBase64String(ms.ToArray)
        Catch
            Ret = Nothing
            plaintextBytes = Nothing
            ms.Dispose()
            encStream.Dispose()
        End Try
        Return Ret
    End Function
    Public Function DecryptData(ByVal encryptedtext As String) As String
        Dim encryptedBytes() As Byte = Convert.FromBase64String(encryptedtext)
        Dim ms As New System.IO.MemoryStream
        Dim decStream As New CryptoStream(ms, TripleDes.CreateDecryptor(), System.Security.Cryptography.CryptoStreamMode.Write)
        Dim Ret As String = Nothing
        Try
            decStream.Write(encryptedBytes, 0, encryptedBytes.Length)
            ' IF ERROR: RUN PROGRAM WITHOUT DEBUGGER BY PRESSING CTRL + F5
            decStream.FlushFinalBlock()
            Ret = System.Text.Encoding.Unicode.GetString(ms.ToArray)
        Catch
            Ret = Nothing
        Finally
            encryptedBytes = Nothing
            ms.Dispose()
            decStream.Dispose()
        End Try
        Return Ret
    End Function
End Class