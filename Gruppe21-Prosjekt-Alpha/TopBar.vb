Public Class TopBar
    Inherits ContainerControl
    Private logo As New HemoGlobeLogo
    Private LogoutButton As TopBarButton
    Private buttonList As New List(Of TopBarButton)
    Private BorderPen As New Pen(Color.FromArgb(72, 78, 83))
    Private HighlightPen As New Pen(Color.FromArgb(247, 247, 247))
    Protected Overridable Sub Test()
        MsgBox("Hei")
    End Sub

    Public Sub New(ParentControl As Control)
        BackColor = Color.FromArgb(110, 120, 127)
        Dock = DockStyle.Top
        Parent = ParentControl
        With logo
            .Parent = Me
            Height = .Height + .Top * 2
        End With
    End Sub
    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        For Each C As TopBarButton In buttonList
            With C
                C.Top = (Height - .Height) \ 2 + 1
            End With
        Next
        If LogoutButton IsNot Nothing Then
            With LogoutButton
                .Left = Width - .Width - .Top
                .Top = (Height - .Height) \ 2 + 1
            End With
        End If
    End Sub
    Public Sub AddButton(Icon As Bitmap, Text As String, Size As Size)
        Dim NB As New TopBarButton(Me, Icon, Text, Size)
        With buttonList
            If .Count > 0 Then
                With .Last
                    NB.Left = .Right + (Height - Size.Height) \ 2
                End With
            Else
                NB.Left = logo.Right + (Height - Size.Height) \ 2
            End If
            .Add(NB)
        End With
    End Sub
    Public Sub AddLogout(Text As String, Size As Size)
        LogoutButton = New TopBarButton(Me, My.Resources.LoggUtIcon, Text, Size, True)
        With LogoutButton
            .Left = Width - .Width - .Top
            .BackColor = Color.FromArgb(162, 25, 51)
            .ForeColor = Color.White
        End With
    End Sub
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        e.Graphics.DrawLine(BorderPen, New Point(0, Height - 1), New Point(Width - 1, Height - 1))
    End Sub
End Class
Public Class TopBarButton
    Inherits Control
    Private WithEvents TBButtonLabel As New Label
    Private Icon As TBButtonIcon
    Private varIsLogout As Boolean = False
    Private TextBrush As New SolidBrush(Color.FromArgb(30, 30, 30))
    Private HighlightBrush As New SolidBrush(Color.FromArgb(200, Color.White))
    Private BorderPen As New Pen(Color.FromArgb(155, 155, 155))
    Private ShadowBrush As New SolidBrush(Color.FromArgb(91, 100, 106))
    Private DrawRect, ShadowRect As Rectangle
    Private TextPoint As Point
    Public ReadOnly Property IsLogout As Boolean
        Get
            Return varIsLogout
        End Get
    End Property
    Public ReadOnly Property Label As Label
        Get
            Return TBButtonLabel
        End Get
    End Property
    Private Sub SetTextHeight() Handles TBButtonLabel.TextChanged
        Dim TextSize As Size = TextRenderer.MeasureText(Label.Text, Label.Font)
        TextPoint = New Point(TBButtonLabel.Left, TBButtonLabel.Height \ 2 - TextSize.Height \ 2 - 3)
        Width = TextSize.Width + Icon.Right + 10
    End Sub
    Protected Friend Sub New(ParentTopBar As TopBar, BMP As Bitmap, LabTxt As String, Size As Size, Optional IsLogout As Boolean = False)
        varIsLogout = IsLogout
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
        If varIsLogout Then
            HighlightBrush.Dispose()
            BorderPen.Color = AudiopoLib.ColorHelper.Multiply(Color.FromArgb(162, 25, 51), 0.4)
            TextBrush.Color = Color.White
        End If
        Show()
    End Sub
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        With e.Graphics
            If Not varIsLogout Then
                .DrawString(TBButtonLabel.Text, Label.Font, HighlightBrush, TextPoint)
            End If
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

