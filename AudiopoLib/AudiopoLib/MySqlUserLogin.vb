Option Strict On
Option Explicit On
Option Infer Off

Public Class MySqlUserLogin
    Private WithEvents DBC As DatabaseClient
    Private IfValidAction As Action
    Private InIfValidAction As Action(Of Object)
    Private IfInvalidAction As Action
    Private InIfInvalidAction As Action(Of Object)
    Private DataToPassValid As Object = Nothing
    Private DataToPassInvalid As Object = Nothing
    Private PassDataValid As Boolean = False
    Private PassDataInvalid As Boolean = False
    Private ReportMultipleMatches As Boolean = False
    Private ExecuteIfMultipleMatches As Boolean = True
    Public Event MultipleFound()
    Public Property ReportMultiple As Boolean
        Get
            Return ReportMultipleMatches
        End Get
        Set(value As Boolean)
            ReportMultipleMatches = value
        End Set
    End Property
    Public Property ExecuteIfMultiple As Boolean
        Get
            Return ExecuteIfMultipleMatches
        End Get
        Set(value As Boolean)
            ExecuteIfMultipleMatches = value
        End Set
    End Property
    Public Sub New(ByVal Server As String, ByVal Database As String, ByVal UserID As String, ByVal Password As String)
        DBC = New DatabaseClient(Server, Database, UserID, Password)
    End Sub
    Public Overloads Sub LoginAsync(ByVal Username As String, ByVal Password As String, ByVal TableName As String, ByVal UidColumnName As String, ByVal PwdColumnName As String)
        ' Parameteriser UidColumnName og TableName også
        DBC.SQLQuery = "SELECT " & UidColumnName & " FROM " & TableName & " WHERE " & UidColumnName & "=@username AND " & PwdColumnName & "=@password"
        DBC.Execute({"@username", "@password"}, {Username, Password}, True)
    End Sub
    Public Overloads Sub LoginAsync(ByVal Username As String, ByVal Password As String, ByVal TableName As String, ByVal UidColumnName As String, ByVal PwdColumnName As String, ByVal Limit As Integer)
        ' Parameteriser UidColumnName og TableName også
        DBC.SQLQuery = "SELECT " & UidColumnName & " FROM " & TableName & " WHERE " & UidColumnName & "=@username AND " & PwdColumnName & "=@password LIMIT " & CStr(Limit)
        DBC.Execute({"@username", "@password"}, {Username, Password}, True)
    End Sub
    Public Overloads Sub LoginAsync(ByVal Username As String, ByVal Password As String, ByVal Query As String, ByVal Parameters() As String, ByVal Values() As String)
        DBC.SQLQuery = Query
        DBC.Execute(Parameters, Values, True)
    End Sub
    Private Sub Fin(ByVal Exists As Boolean, ByVal RowCount As Integer, Tag As Integer) Handles DBC.ExistsCheckCompleted
        If Exists Then
            If PassDataValid Then
                If ReportMultiple = True AndAlso RowCount > 1 Then
                    RaiseEvent MultipleFound()
                    If ExecuteIfMultiple = True Then
                        InIfValidAction.Invoke(DataToPassValid)
                    End If
                Else
                    If InIfValidAction IsNot Nothing Then
                        InIfValidAction.Invoke(DataToPassValid)
                    End If
                End If
            Else
                If IfValidAction IsNot Nothing Then
                    IfValidAction.Invoke
                End If
            End If
        Else
            If PassDataInvalid Then
                If InIfInvalidAction IsNot Nothing Then
                    InIfInvalidAction.Invoke(DataToPassInvalid)
                End If
            Else
                If IfInvalidAction IsNot Nothing Then
                    IfInvalidAction.Invoke
                End If
            End If
        End If
    End Sub
    Public Overloads WriteOnly Property IfValid As Action
        Set(Action As Action)
            IfValidAction = Action
            PassDataValid = False
            DataToPassValid = Nothing
        End Set
    End Property
    Public Overloads WriteOnly Property IfValid(ByVal ObjectToPass As Object) As Action(Of Object)
        Set(Action As Action(Of Object))
            InIfValidAction = Action
            DataToPassValid = ObjectToPass
            PassDataValid = True
        End Set
    End Property
    Public Overloads WriteOnly Property IfInvalid As Action
        Set(Action As Action)
            IfInvalidAction = Action
            PassDataInvalid = False
            DataToPassInvalid = Nothing
        End Set
    End Property
    Public Overloads WriteOnly Property IfInvalid(ByVal ObjectToPass As Object) As Action(Of Object)
        Set(Action As Action(Of Object))
            InIfInvalidAction = Action
            DataToPassInvalid = ObjectToPass
            PassDataInvalid = True
        End Set
    End Property
End Class