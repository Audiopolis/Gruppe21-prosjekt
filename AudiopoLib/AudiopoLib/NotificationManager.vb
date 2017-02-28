Option Strict On
Option Explicit On
Option Infer Off

Imports System.Windows.Forms
Imports System.Threading

Public Class NotificationManager
    Implements IDisposable

    Private SC As SynchronizationContext = SynchronizationContext.Current
    Private ParentControl As Control
    Private LinkedLayoutTools As FormLayoutTools
    Private IsReady As Boolean
    Private AdjustReservedSpace As Boolean = True
    Private QueueList As List(Of QueuedNotification)
    Public Event NotificationClosed(sender As Notification)
    Public Event Click(sender As Notification)
    Public Event MouseEnter(sender As Notification)
    Public Event MouseLeave(sender As Notification)
    Public Property AssignedLayoutManager() As FormLayoutTools
        Get
            Return LinkedLayoutTools
        End Get
        Set(Instance As FormLayoutTools)
            LinkedLayoutTools = Instance
        End Set
    End Property
    Public Property Parent As Control
        Get
            Return ParentControl
        End Get
        Set(Parent As Control)
            ParentControl = Parent
        End Set
    End Property
    Public Sub New(Parent As Control)
        QueueList = New List(Of QueuedNotification)
        ParentControl = Parent
        IsReady = True
    End Sub
    ''' <summary>
    ''' Display the notification on screen.
    ''' </summary>
    ''' <param name="Message">The message to display in the notification.</param>
    ''' <param name="Duration">The notification's duration in seconds before fading out. 0 means no time limit (must be closed by the user).</param>
    Public Overloads Sub Display(ByVal Message As String, ByVal Appearance As NotificationAppearance, Optional ByVal Duration As Double = 5, Optional ByVal AlignmentX As FloatX = FloatX.FillWidth, Optional ByVal AlignmentY As FloatY = FloatY.Top)
        'AddHandler NewNotification.NotificationFinished, AddressOf DisplayNext
        If IsReady Then
            IsReady = False
            Dim NewNotification As New Notification(Parent, Appearance, Message, Duration, AddressOf onNotificationFinished, AlignmentX, AlignmentY)
            AddHandler NewNotification.Click, AddressOf onClick
            AddHandler NewNotification.MouseEnter, AddressOf onMouseEnter
            AddHandler NewNotification.MouseLeave, AddressOf onMouseLeave
            NewNotification.Display()
            If LinkedLayoutTools IsNot Nothing Then
                LinkedLayoutTools.SlideToHeight(NewNotification.Top + NewNotification.Height)
            End If
        Else
            QueueList.Add(New QueuedNotification(Parent, AddressOf onNotificationFinished, Message, Appearance, Duration, AlignmentX, AlignmentY))
        End If
    End Sub
    Private Sub onClick(sender As Object, e As EventArgs)
        RaiseEvent Click(DirectCast(sender, Notification))
    End Sub
    Private Sub onMouseEnter(sender As Object, e As EventArgs)
        RaiseEvent MouseEnter(DirectCast(sender, Notification))
    End Sub
    Private Sub onMouseLeave(sender As Object, e As EventArgs)
        RaiseEvent MouseLeave(DirectCast(sender, Notification))
    End Sub
    Private Class QueuedNotification
        Protected Friend vParent As Control
        Protected Friend vAppearance As NotificationAppearance
        Protected Friend vMessage As String
        Protected Friend vDuration As Double
        Protected Friend vWhenDone As Action(Of Notification)
        Protected Friend vFloatX As FloatX
        Protected Friend vFloatY As FloatY
        Public Sub New(Parent As Control, WhenDone As Action(Of Notification), ByVal Message As String, ByVal Appearance As NotificationAppearance, Optional ByVal Duration As Double = 5, Optional ByVal AlignmentX As FloatX = FloatX.FillWidth, Optional ByVal AlignmentY As FloatY = FloatY.Top)
            vParent = Parent
            vAppearance = Appearance
            vMessage = Message
            vDuration = Duration
            vWhenDone = WhenDone
            vFloatX = AlignmentX
            vFloatY = AlignmentY
        End Sub
        Public Sub Clear()
            vParent = Nothing
            vAppearance = Nothing
            vWhenDone = Nothing
        End Sub
    End Class
    Private Sub onNotificationFinished(sender As Notification)
        RemoveHandler sender.Click, AddressOf onClick
        RemoveHandler sender.MouseEnter, AddressOf onMouseEnter
        RemoveHandler sender.MouseLeave, AddressOf onMouseLeave
        ' SENDER BLIR DISPOSED, IKKE VIDERESEND SENDER
        Debug.Print("IF COUNT 0")
        If QueueList IsNot Nothing AndAlso QueueList.Count = 0 Then
            If LinkedLayoutTools IsNot Nothing Then
                LinkedLayoutTools.SlideToDefault()
            End If
            IsReady = True
        Else
            Dim Temp As QueuedNotification = QueueList(0)
            Dim NewNotification As New Notification(Temp.vParent, Temp.vAppearance, Temp.vMessage, Temp.vDuration, Temp.vWhenDone, Temp.vFloatX, Temp.vFloatY)
            If LinkedLayoutTools IsNot Nothing Then
                LinkedLayoutTools.SlideToHeight(NewNotification.Top + NewNotification.Height)
            End If
            Temp.Clear()
            QueueList.RemoveAt(0)
            NewNotification.Display()
        End If
        sender.Dispose()
        RaiseEvent NotificationClosed(sender)
    End Sub
#Region "IDisposable Support"
    Private disposedValue As Boolean
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then

            End If
            Debug.Print("SETTING QUEUELIST NOTHING")
            QueueList = Nothing
            ParentControl = Nothing
            LinkedLayoutTools = Nothing
        End If
        disposedValue = True
    End Sub
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
    End Sub
#End Region
End Class