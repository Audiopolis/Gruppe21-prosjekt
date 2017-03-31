Public Class TopBar
    Inherits ContainerControl
    Private logo As New HemoGlobeLogo
    Private buttonList As New List(Of TopBarButton)
    Protected Overridable Sub Test()
        MsgBox("Hei")
    End Sub

    Public Sub New(ParentControl As Control)
        BackColor = Color.FromArgb(110, 120, 127)
        Dock = DockStyle.Top
        Parent = ParentControl
        With logo
            .Parent = Me
            Height = .Bottom + .Top
        End With
    End Sub
    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        For Each C As TopBarButton In buttonList
            With C
                C.Top = Height \ 2 - .Height \ 2 + 1
            End With
        Next
    End Sub
    Public Sub AddButton(Icon As Bitmap, Text As String, Size As Size)
        Dim NB As New TopBarButton(Me, Icon, Text, Size)
        With buttonList
            If .Count > 0 Then
                NB.Left = buttonList.Last().Right + 40
            Else
                NB.Left = logo.Right + 40
            End If
            .Add(NB)
        End With
    End Sub
End Class
Public Class TopBarButton
    Inherits Control
    Private WithEvents TBButtonLabel As New Label
    Private Icon As TBButtonIcon
    Private TextBrush As New SolidBrush(Color.FromArgb(30, 30, 30))
    Private HighlightBrush As New SolidBrush(Color.FromArgb(200, Color.White))
    Private BorderPen As New Pen(Color.FromArgb(155, 155, 155))
    Private ShadowBrush As New SolidBrush(Color.FromArgb(91, 100, 106))
    Private DrawRect, ShadowRect As Rectangle
    Private TextPoint As Point
    Public ReadOnly Property Label As Label
        Get
            Return TBButtonLabel
        End Get
    End Property
    Private Sub SetTextHeight() Handles TBButtonLabel.TextChanged
        TextPoint = New Point(TBButtonLabel.Left, TBButtonLabel.Height \ 2 - TextRenderer.MeasureText(Label.Text, Label.Font).Height \ 2 - 2)
    End Sub
    Protected Friend Sub New(ParentTopBar As TopBar, BMP As Bitmap, LabTxt As String, Size As Size)
        Hide()
        DoubleBuffered = True
        BackColor = Color.FromArgb(247, 247, 247)
        ForeColor = Color.FromArgb(30, 30, 30)
        Me.Size = Size
        Icon = New TBButtonIcon(Me)
        Parent = ParentTopBar
        With TBButtonLabel
            .Hide()
            .Parent = Me
            .Height = Height
            .Left = Icon.Right
            .Width = Width - .Left
            .TextAlign = ContentAlignment.MiddleLeft
        End With
        With Icon
            .BackgroundImage = BMP
        End With
        TBButtonLabel.Text = LabTxt
        Show()
    End Sub
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        With e.Graphics
            .DrawString(TBButtonLabel.Text, Label.Font, HighlightBrush, TextPoint)
            .DrawString(TBButtonLabel.Text, Label.Font, TextBrush, New Point(TextPoint.X - 1, TextPoint.Y + 1))
            .DrawRectangle(BorderPen, DrawRect)
            .FillRectangle(ShadowBrush, ShadowRect)
        End With
    End Sub
    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        SelectButton(True)
    End Sub
    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        SelectButton(False)
    End Sub
    Protected Friend Sub SelectButton(ByVal DoSelect As Boolean)
        If DoSelect Then
            Height += 2
            Top -= 1
        Else
            Height -= 2
            Top += 1
        End If
    End Sub
    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        With TBButtonLabel
            .Width = Width - .Left
        End With
        DrawRect = New Rectangle(Point.Empty, New Size(Width - 1, Height - 4))
        ShadowRect = New Rectangle(New Point(0, Height - 3), New Size(Width, 3))
    End Sub


    Protected Friend Class TBButtonIcon
        Inherits Control
        Public Shadows Property Parent As TopBarButton
            Get
                Return DirectCast(MyBase.Parent, TopBarButton)
            End Get
            Set(value As TopBarButton)
                MyBase.Parent = value
            End Set
        End Property
        Protected Overrides Sub OnMouseEnter(e As EventArgs)
            MyBase.OnMouseEnter(e)
            Parent.SelectButton(True)
        End Sub
        Protected Overrides Sub OnMouseLeave(e As EventArgs)
            MyBase.OnMouseLeave(e)
            Parent.SelectButton(False)
        End Sub
        Public Sub New(ParentButton As TopBarButton)
            Parent = ParentButton
            With Parent
                Size = New Size(.Height - 2, .Height - 5)
            End With
            Location = New Point(1, 1)
            BackgroundImageLayout = ImageLayout.Center
        End Sub
    End Class
End Class

