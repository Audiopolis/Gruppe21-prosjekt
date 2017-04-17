Option Strict On
Option Explicit On
Option Infer Off
Imports System.Drawing.Text
Imports System.Threading
Imports AudiopoLib

Public Class BlodBeholder
    Inherits Control
    Private WithEvents Empty As New PictureBox
    Private EmptyBrush As New SolidBrush(Color.FromArgb(15, 79, 117))
    Private FullBrush As New SolidBrush(Color.White)
    Private LargeFont As New Font(Font.FontFamily, 120, FontStyle.Bold)
    Private SmallFont As New Font(Font.FontFamily, 12)
    Private LargeText As String = "4x"
    Private TextSizes(2) As Size
    Private LargeTextSize As Size = Size.Empty
    Private SmallText() As String = {"Du har donert X ganger", "så mye blod som en gjennomsnittlig", "person har i kroppen."}
    Private SC As SynchronizationContext = SynchronizationContext.Current
    Private WithEvents SlideTimer As New Timers.Timer(1000 \ 50)
    Private Initial, Current, Goal As Double
    Private Const MengdeBlodIMenneskeILiter As Double = 4.5
    Private Const XLast As Integer = 40
    Private CurrentX As Integer = 0
    Public Sub New(EmptyBitmap As Bitmap, FullBitmap As Bitmap)
        DoubleBuffered = True
        With Empty
            .Parent = Me
            .BackgroundImage = EmptyBitmap
            .Size = .BackgroundImage.Size
            .Top = 0
        End With
        With SlideTimer
            .AutoReset = False
        End With
        BackgroundImage = FullBitmap
        Size = BackgroundImage.Size
        LargeTextSize = TextRenderer.MeasureText(LargeText, LargeFont)
        TextSizes = {TextRenderer.MeasureText(SmallText(0), SmallFont), TextRenderer.MeasureText(SmallText(1), SmallFont), TextRenderer.MeasureText(SmallText(2), SmallFont)}
    End Sub
    Private Sub Empty_MouseEnter(sender As Object, e As EventArgs) Handles Empty.MouseEnter
        OnMouseEnter(e)
    End Sub
    Private Sub Empty_MouseLeave(sender As Object, e As EventArgs) Handles Empty.MouseLeave
        OnMouseLeave(e)
    End Sub
    Private Sub Empty_MouseUp(sender As Object, e As EventArgs) Handles Empty.MouseUp
        OnClick(e)
    End Sub
    Private Sub Empty_DoubleClick(sender As Object, e As EventArgs) Handles Empty.DoubleClick
        OnDoubleClick(e)
    End Sub
    Public WriteOnly Property TotalBloodInLiters As Double
        Set(value As Double)
            Dim RoundedValue As String = Math.Round(value / MengdeBlodIMenneskeILiter, 1).ToString
            LargeText = RoundedValue & "x"
            LargeTextSize = TextRenderer.MeasureText(LargeText, LargeFont)
            SmallText = {"Du har donert " & RoundedValue & " ganger", "så mye blod som en gjennomsnittlig", "person har i kroppen."}
            TextSizes = {TextRenderer.MeasureText(SmallText(0), SmallFont), TextRenderer.MeasureText(SmallText(1), SmallFont), TextRenderer.MeasureText(SmallText(2), SmallFont)}
        End Set
    End Property
    Private Sub EmptyPaint(sender As Object, e As PaintEventArgs) Handles Empty.Paint
        With e.Graphics
            .TextRenderingHint = TextRenderingHint.AntiAlias
            ' TODO: Make more efficient
            .DrawString(LargeText, LargeFont, EmptyBrush, New Point(Width \ 2 - LargeTextSize.Width \ 2, Height \ 2 - LargeTextSize.Height \ 2 - 60))
            .DrawString(SmallText(0), SmallFont, EmptyBrush, New Point(Width \ 2 - TextSizes(0).Width \ 2, Height \ 2 - TextSizes(0).Height \ 2 + 50))
            .DrawString(SmallText(1), SmallFont, EmptyBrush, New Point(Width \ 2 - TextSizes(1).Width \ 2, Height \ 2 - TextSizes(1).Height \ 2 + 70))
            .DrawString(SmallText(2), SmallFont, EmptyBrush, New Point(Width \ 2 - TextSizes(2).Width \ 2, Height \ 2 - TextSizes(2).Height \ 2 + 90))
        End With
    End Sub
    Protected Overrides Sub OnPaintBackground(pevent As PaintEventArgs)
        MyBase.OnPaintBackground(pevent)
        With pevent.Graphics
            .TextRenderingHint = TextRenderingHint.AntiAlias
            ' TODO: Make more efficient
            .DrawString(LargeText, LargeFont, FullBrush, New Point(Width \ 2 - LargeTextSize.Width \ 2, Height \ 2 - LargeTextSize.Height \ 2 - 60))
            .DrawString(SmallText(0), SmallFont, FullBrush, New Point(Width \ 2 - TextSizes(0).Width \ 2, Height \ 2 - TextSizes(0).Height \ 2 + 50))
            .DrawString(SmallText(1), SmallFont, FullBrush, New Point(Width \ 2 - TextSizes(1).Width \ 2, Height \ 2 - TextSizes(1).Height \ 2 + 70))
            .DrawString(SmallText(2), SmallFont, FullBrush, New Point(Width \ 2 - TextSizes(2).Width \ 2, Height \ 2 - TextSizes(2).Height \ 2 + 90))
        End With
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