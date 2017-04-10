Imports System.Drawing.Drawing2D

Public Class BorderControl
    Inherits ContainerControl
    Private varBorderPen, varDashedPenH, varDashedPenV As Pen
    Private varDashed As Boolean
    Public Sub MakeDashed(BackColor As Color)
        If varDashedPenH IsNot Nothing Then
            varDashedPenH.Dispose()
        End If
        If varDashedPenV IsNot Nothing Then
            varDashedPenV.Dispose()
        End If
        Using NH As New HatchBrush(HatchStyle.DashedHorizontal, varBorderPen.Color, BackColor)
            varDashedPenH = New Pen(NH)
        End Using
        Using NV As New HatchBrush(HatchStyle.DashedVertical, varBorderPen.Color, BackColor)
            varDashedPenV = New Pen(NV)
        End Using
        varDashed = True
    End Sub
    Public Sub MakeSolid()
        varDashed = False
    End Sub
    Public Sub New(BorderColor As Color)
        DoubleBuffered = True
        varBorderPen = New Pen(BorderColor)
    End Sub
    Public Property BorderPen As Pen
        Get
            Return varBorderPen
        End Get
        Set(value As Pen)
            varBorderPen.Dispose()
            varBorderPen = value
        End Set
    End Property
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim Height As Integer = ClientSize.Height
        Dim Width As Integer = ClientSize.Width
        With e.Graphics
            If Not varDashed Then
                .DrawLine(varBorderPen, Point.Empty, New Point(0, Height - 1))
                .DrawLine(varBorderPen, Point.Empty, New Point(Width - 1, 0))
                .DrawLine(varBorderPen, New Point(Width - 1, 0), New Point(Width - 1, Height - 1))
                .DrawLine(varBorderPen, New Point(0, Height - 1), New Point(Width - 1, Height - 1))
            Else
                .DrawLine(varDashedPenV, Point.Empty, New Point(0, Height - 1))
                .DrawLine(varDashedPenH, Point.Empty, New Point(Width - 1, 0))
                .DrawLine(varDashedPenV, New Point(Width - 1, 0), New Point(Width - 1, Height - 1))
                .DrawLine(varDashedPenH, New Point(0, Height - 1), New Point(Width - 1, Height - 1))
            End If
        End With
    End Sub
    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            varBorderPen.Dispose()
            If varDashedPenH IsNot Nothing Then
                varDashedPenH.Dispose()
            End If
            If varDashedPenV IsNot Nothing Then
                varDashedPenV.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub
End Class
