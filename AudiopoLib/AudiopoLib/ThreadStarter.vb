Option Strict On
Option Explicit On
Option Infer Off

Imports System.Threading

Public Class ThreadStarter
    Implements IDisposable
#Region "Fields"
    Private SC As SynchronizationContext = SynchronizationContext.Current
    Private MethodToRun As Action
    Private MethodToRunIn As Action(Of Object)
    Private FuncToRunOut As Func(Of Object)
    Private FuncToRunInOut As Func(Of Object, Object)
    Private GenericProcedure As [Delegate]
    Private MethodType As Integer
    Private StartTime As Date
    Private EndTime As Date
    Private Running As Boolean
    Private ThreadID As Object = Nothing
    Private DisposeWhenFinished As Boolean
    Private IsBG As Boolean = True
    Private SubWhenFinished As Action(Of Object, ThreadStarterEventArgs)
#End Region
#Region "Events"
    Public Event WorkCompleted(Result As Object, e As ThreadStarterEventArgs)
#End Region
    Public WriteOnly Property WhenFinished As Action(Of Object, ThreadStarterEventArgs)
        Set(Action As Action(Of Object, ThreadStarterEventArgs))
            SubWhenFinished = Action
        End Set
    End Property
    Public Property IsBackground As Boolean
        Get
            Return IsBG
        End Get
        Set(value As Boolean)
            IsBG = value
        End Set
    End Property
    Public ReadOnly Property IsRunning As Boolean
        Get
            Return Running
        End Get
    End Property
    Public Property ID As Object
        Get
            Return ThreadID
        End Get
        Set(value As Object)
            ThreadID = value
        End Set
    End Property

    Public Sub New(Method As Action, Optional ID As Object = 0)
        MethodToRun = Method
        MethodType = 1
        ThreadID = ID
    End Sub
    Public Sub New(Method As Action(Of Object), Optional ID As Object = 0)
        MethodToRunIn = Method
        MethodType = 2
        ThreadID = ID
    End Sub
    Public Sub New(Func As Func(Of Object), Optional ID As Object = 0)
        FuncToRunOut = Func
        MethodType = 3
        ThreadID = ID
    End Sub
    Public Sub New(Func As Func(Of Object, Object), Optional ID As Object = 0)
        FuncToRunInOut = Func
        MethodType = 4
        ThreadID = ID
    End Sub
    Public Sub New(Procedure As [Delegate], Optional ID As Object = 0)
        GenericProcedure = Procedure
        MethodType = 5
        ThreadID = ID
    End Sub
    Public Function RunTime() As TimeSpan
        If Running = True Then
            Return Date.Now.Subtract(StartTime)
        Else
            Return EndTime.Subtract(StartTime)
        End If
    End Function
    Private Sub RaiseWorkCompleted(Result As Object)
        EndTime = Date.Now
        Running = False
        If Not SubWhenFinished Is Nothing Then
            SubWhenFinished.Invoke(Result, New ThreadStarterEventArgs(ThreadID, StartTime, EndTime))
        End If
        RaiseEvent WorkCompleted(Result, New ThreadStarterEventArgs(ThreadID, StartTime, EndTime))
    End Sub
    Public Overloads Sub Start(Optional Parameters As Object = Nothing)
        If Running = False Then
            StartTime = Date.Now
            Running = True
            If IsBG = True Then
                Select Case MethodType
                    Case 0
                        Running = False
                        Throw New Exception("Cannot start this thread before it has been assigned a method to run (use the New constructor).")
                    Case 1
                        Dim NoParamMethodTask As Task = Task.Factory.StartNew(AddressOf ExecuteNoParams)
                    Case 2
                        Dim ParamMethodTask As Task = Task.Factory.StartNew(AddressOf Execute, Parameters)
                    Case 3
                        Dim NoParamFuncThread As Task(Of Object) = Task.Factory.StartNew(AddressOf ExecuteFunction)
                    Case 4
                        Dim ParamFuncThread As Task(Of Object) = Task.Factory.StartNew(AddressOf ExecuteFunction, Parameters)
                End Select
            Else
                Select Case MethodType
                    Case 0
                        Running = False
                        Throw New Exception("Cannot start this thread before it has been assigned a method to run (use the New constructor).")
                    Case 1
                        Dim NoParamMethodTask As New Thread(AddressOf ExecuteNoParams)
                        NoParamMethodTask.IsBackground = False
                        NoParamMethodTask.Start()
                    Case 2
                        Dim ParamMethodTask As New Thread(AddressOf Execute)
                        ParamMethodTask.IsBackground = False
                        ParamMethodTask.Start(Parameters)
                    Case 3
                        Dim test As ThreadStart = AddressOf ExecuteFunction
                        Dim NoParamFuncThread As New Thread(test)
                        NoParamFuncThread.IsBackground = False
                        NoParamFuncThread.Start()
                    Case 4
                        Dim test As ParameterizedThreadStart = AddressOf ExecuteFunction
                        Dim ParamFuncThread As New Thread(test)
                        ParamFuncThread.IsBackground = False
                        ParamFuncThread.Start(Parameters)
                End Select
            End If
        End If
    End Sub
    'Public Overloads Sub Start(ByVal Parameters() As Object)
    '    If Running = False Then
    '        StartTime = Date.Now
    '        Running = True
    '        Dim DynamicInvokeThread As Task = Task.Factory.StartNew(AddressOf DynamicInvokeMethod, Parameters)
    '    End If
    'End Sub
    Private Sub ExecuteNoParams()
        MethodToRun.Invoke
        SC.Post(AddressOf RaiseWorkCompleted, Nothing)
    End Sub
    Private Sub Execute(Parameters As Object)
        MethodToRunIn.Invoke(Parameters)
        SC.Post(AddressOf RaiseWorkCompleted, Nothing)
    End Sub
    Private Overloads Function ExecuteFunction(P As Object) As Object
        Dim Ret As Object = FuncToRunInOut.Invoke(P)
        SC.Post(AddressOf RaiseWorkCompleted, Ret)
        Return Ret
    End Function
    Private Overloads Function ExecuteFunction() As Object
        Dim Ret As Object = FuncToRunOut.Invoke
        SC.Post(AddressOf RaiseWorkCompleted, Ret)
        Return Ret
    End Function
    'Private Sub DynamicInvokeMethod(ByVal Parameters As Object)
    '    Try
    '        Dim args() As Object = DirectCast(Parameters, Object())
    '        If Not GenericProcedure.Method.ReturnType Is Nothing Then
    '            Dim Ret As Object = GenericProcedure.DynamicInvoke(Parameters)
    '            SC.Post(AddressOf RaiseWorkCompleted, Ret)
    '        Else
    '            GenericProcedure.DynamicInvoke(Parameters)
    '            SC.Post(AddressOf RaiseWorkCompleted, Nothing)
    '        End If
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Sub
#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If
            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
            MethodToRun = Nothing
            MethodToRunIn = Nothing
            FuncToRunOut = Nothing
            FuncToRunInOut = Nothing
            SubWhenFinished = Nothing
            GenericProcedure = Nothing

            ThreadID = Nothing
            StartTime = Nothing
            EndTime = Nothing
            MethodType = 0
            Running = False
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

Public Class ThreadStarterEventArgs
    Inherits EventArgs
    Private STime As Date
    Private ETime As Date
    Private ThreadID As Object
    Public ReadOnly Property ID As Object
        Get
            Return ThreadID
        End Get
    End Property
    Public ReadOnly Property StartTime As Date
        Get
            Return STime
        End Get
    End Property
    Public ReadOnly Property EndTime As Date
        Get
            Return ETime
        End Get
    End Property
    Public Function TimeToFinish() As TimeSpan
        Return ETime.Subtract(STime)
    End Function
    Public Sub New(ByVal ID As Object, StartTime As Date, EndTime As Date)
        ThreadID = ID
        STime = StartTime
        ETime = EndTime
    End Sub
End Class
