Option Strict On
Option Explicit On
Option Infer Off

Imports System.Threading
Imports AudiopoLib

Public Class BlodBeholder
    Inherits Control
    ' Function = 1+0.08x-0.001x^2
    Private Empty As New PictureBox
    Private SC As SynchronizationContext = SynchronizationContext.Current
    Private WithEvents SlideTimer As New Timers.Timer(1000 \ 50)

    Private Initial, Current, Goal As Double

    Private Const XLast As Integer = 40
    Private CurrentX As Integer = 0
    Public Sub New(EmptyBitmap As Bitmap, FullBitmap As Bitmap)
        DoubleBuffered = True
        With Empty
            .Parent = Me
            .BackgroundImage = EmptyBitmap
            .Size = EmptyBitmap.Size
            .Top = 0
        End With
        With SlideTimer
            .AutoReset = False
        End With
        BackgroundImage = FullBitmap
        Size = FullBitmap.Size
    End Sub
    Private Sub SlideTimer_Tick(Sender As Object, e As EventArgs) Handles SlideTimer.Elapsed
        SC.Send(AddressOf Send_Tick, Nothing)
    End Sub
    Protected Overrides Sub OnLocationChanged(e As EventArgs)
        SuspendLayout()
        MyBase.OnLocationChanged(e)
        ResumeLayout(True)
    End Sub
    Private Sub Send_Tick(State As Object)
        ' TEST
        Current = EaseInOut.GetY(Initial, Goal, CurrentX, XLast)
        CurrentX += 1
        Empty.Height = CInt(Height - (Height / 100) * Current)
        If CurrentX < XLast Then
            SlideTimer.Start()
        End If
    End Sub
    Public Sub SetPercentage(Percent As Integer)
        SuspendLayout()
        With Empty
            .Height = CInt(Height - (Height / 100) * Percent)
        End With
        ResumeLayout(True)
    End Sub
    Public Sub SlideToPercentage(Percent As Double)
        Initial = Current
        CurrentX = 0
        Goal = Percent
        SlideTimer.Start()
    End Sub
End Class