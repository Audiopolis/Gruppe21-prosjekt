Public Class InfoLabel
    Inherits Control
    Private BarBrush As New SolidBrush(Color.FromArgb(60, 60, 60))
    Private BarRect As Rectangle
    Private TextLab As New Label
    Public ReadOnly Property Label As Label
        Get
            Return TextLab
        End Get
    End Property
    Public Shadows Property Text As String
        Get
            Return TextLab.Text
        End Get
        Set(value As String)
            TextLab.Text = value
        End Set
    End Property
    Public Sub New()
        With TextLab
            .Parent = Me
            .Location = New Point(8, 0)
            .AutoSize = False
            .Size = New Size(Width - 8, Height)
            .ForeColor = Color.FromArgb(60, 60, 60)
            .TextAlign = ContentAlignment.MiddleLeft
        End With
    End Sub
    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        BarRect = New Rectangle(Point.Empty, New Size(4, Height))
        TextLab.Size = New Size(Width - 8, Height)
    End Sub
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        With e.Graphics
            .FillRectangle(BarBrush, BarRect)
        End With
    End Sub
End Class
