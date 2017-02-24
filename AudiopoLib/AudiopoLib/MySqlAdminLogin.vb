Option Strict On
Option Explicit On
Option Infer Off

Public Class MySqlAdminLogin
    Private ServerString As String
    Private DatabaseString As String

    Private WhenCompletedAction As Action(Of Boolean) = Nothing

    Private DataToPassValid As Object = Nothing
    Private DataToPassInvalid As Object = Nothing
    Private PassDataValid As Boolean = False
    Private PassDataInvalid As Boolean = False
    Private DBC As DatabaseClient
    Public Event ValidationCompleted(ByVal Valid As Boolean)
    Public Sub New(ByVal Server As String, ByVal Database As String)
        ServerString = Server
        DatabaseString = Database
    End Sub
    Public Sub Check(ByVal UID As String, ByVal Password As String)
        DBC = New DatabaseClient(ServerString, DatabaseString, UID, Password)
        AddHandler DBC.ValidationCompleted, AddressOf ValidationCompelted
    End Sub
    Private Sub ValidationCompelted(ByVal Valid As Boolean)
        RaiseEvent ValidationCompleted(Valid)
        If WhenCompletedAction IsNot Nothing Then
            WhenCompletedAction.Invoke(Valid)
        End If
    End Sub
End Class
