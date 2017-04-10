﻿Imports AudiopoLib
Public Class TopBar
    Inherits ContainerControl
    Private logo As New HemoGlobeLogo
    Private LogoutButton As TopBarButton
    Private buttonList As New List(Of TopBarButton)
    Private BorderPen As New Pen(Color.FromArgb(72, 78, 83))
    Private HighlightPen As New Pen(Color.FromArgb(247, 247, 247))
    Public Event ButtonClick(Sender As TopBarButton, e As EventArgs)
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
                .Top = (Height - .Height) \ 2 + 1
            End With
        Next
        If LogoutButton IsNot Nothing Then
            With LogoutButton
                .Left = Width - .Width - .Top
                .Top = (Height - .Height) \ 2 + 1
            End With
        End If
    End Sub
    Private Sub OnButtonClick(Sender As Object, e As EventArgs)
        RaiseEvent ButtonClick(DirectCast(Sender, TopBarButton), e)
    End Sub
    Public Sub AddButton(Icon As Bitmap, Text As String, Size As Size)
        Dim NB As New TopBarButton(Me, Icon, Text, Size)
        AddHandler NB.Click, AddressOf OnButtonClick
        With buttonList
            Dim ListCount As Integer = .Count
            NB.Tag = ListCount
            If ListCount > 0 Then
                NB.Left = .Last.Right + (Height - Size.Height) \ 2
            Else
                NB.Left = logo.Right + (Height - Size.Height) \ 2
            End If
            .Add(NB)
        End With
    End Sub
    Public Sub AddLogout(Text As String, Size As Size)
        LogoutButton = New TopBarButton(Me, My.Resources.LoggUtIcon, Text, Size, True)
        With LogoutButton
            .Top = (Height - .Height) \ 2 + 1
            .Left = Width - .Width - .Top
            .BackColor = Color.FromArgb(162, 25, 51)
            .ForeColor = Color.White
        End With
    End Sub
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        e.Graphics.DrawLine(BorderPen, New Point(0, Height - 1), New Point(Width - 1, Height - 1))
    End Sub
    Protected Overrides Sub Dispose(disposing As Boolean)
        For Each B As TopBarButton In buttonList
            RemoveHandler B.Click, AddressOf OnButtonClick
        Next
        MyBase.Dispose(disposing)
    End Sub
End Class
Public Class TopBarButton
    Inherits Control
    Private WithEvents TBButtonLabel As New Label
    Private WithEvents varIcon As TopBarButtonIcon
    Private varIsLogout, varDrawTextHighlight As Boolean
    Private TextBrush As New SolidBrush(Color.FromArgb(30, 30, 30))
    Protected Friend HighlightBrush As New SolidBrush(Color.FromArgb(200, Color.White))
    Private BorderPen As New Pen(Color.FromArgb(155, 155, 155))
    Private ShadowBrush As New SolidBrush(Color.FromArgb(91, 100, 106))
    'Private LinePen As New Pen(Color.FromArgb(220, 220, 220))
    Private DrawRect, ShadowRect As Rectangle
    Private TextPoint As Point
    Public Property IconImage As Image
        Get
            Return varIcon.BackgroundImage
        End Get
        Set(value As Image)
            varIcon.BackgroundImage = value
        End Set
    End Property
    Private Sub Icon_Click(Sender As Object, e As EventArgs) Handles varIcon.Click
        OnClick(e)
    End Sub
    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            TBButtonLabel.Dispose()
            varIcon.Dispose()
            TextBrush.Dispose()
            HighlightBrush.Dispose()
            BorderPen.Dispose()
            ShadowBrush.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub
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
        Width = TextSize.Width + varIcon.Right + 20
        Invalidate()
    End Sub
    Protected Friend Sub New(ParentTopBar As TopBar, BMP As Bitmap, LabTxt As String, Size As Size, Optional IsLogout As Boolean = False)
        varIsLogout = IsLogout
        Hide()
        ResizeRedraw = True
        DoubleBuffered = True
        BackColor = Color.FromArgb(235, 235, 235)
        'BackColor = Color.FromArgb(247, 247, 247)
        'ForeColor = Color.FromArgb(30, 30, 30)
        Me.Size = Size
        varIcon = New TopBarButtonIcon(Me, BMP)
        Parent = ParentTopBar
        With TBButtonLabel
            .Hide()
            .Parent = Me
            .Height = Height
            .Left = varIcon.Right + 2
            .Width = Width - .Left
            .Text = LabTxt
        End With
        If varIsLogout Then
            'HighlightBrush.Dispose()
            BorderPen.Color = ColorHelper.Multiply(Color.FromArgb(162, 25, 51), 0.7)
            TextBrush.Color = Color.White
        End If
        Show()
    End Sub
    Public Shadows Property ForeColor As Color
        Get
            Return MyBase.ForeColor
        End Get
        Set(value As Color)
            TextBrush.Color = value
            MyBase.ForeColor = value
        End Set
    End Property
    Protected Friend Sub New(ParentControl As Control, BMP As Bitmap, LabTxt As String, Size As Size, Optional IsLogout As Boolean = False)
        varIsLogout = IsLogout
        Hide()
        DoubleBuffered = True
        BackColor = Color.FromArgb(235, 235, 235)
        'BackColor = Color.FromArgb(247, 247, 247)
        'ForeColor = Color.FromArgb(30, 30, 30)
        Me.Size = Size
        varIcon = New TopBarButtonIcon(Me, BMP)
        Parent = ParentControl
        With TBButtonLabel
            .Hide()
            .Parent = Me
            .Height = Height
            .Left = varIcon.Right + 2
            .Width = Width - .Left
            .Text = LabTxt
        End With
        If varIsLogout Then
            'HighlightBrush.Dispose()
            BorderPen.Color = ColorHelper.Multiply(Color.FromArgb(162, 25, 51), 0.7)
            TextBrush.Color = Color.White
        End If
        Show()
    End Sub
    Protected Overrides Sub OnBackColorChanged(e As EventArgs)
        MyBase.OnBackColorChanged(e)
        If varIsLogout Then
            HighlightBrush.Color = ColorHelper.Multiply(BackColor, 1.1)
        End If
        BorderPen.Color = ColorHelper.Multiply(BackColor, 0.5)
    End Sub
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        With e.Graphics
            '.FillRectangle(HighlightBrush, New Rectangle(Point.Empty, New Size(Width, Height \ 2)))
            If varDrawTextHighlight Then
                '.DrawLine(LinePen, New Point(Icon.Right, 0), New Point(Icon.Right, Icon.Bottom))
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
            'Height += 2
            'Top -= 1
            'Icon.BackgroundImage.Dispose()
            'Icon.BackColor = Color.FromArgb(247, 247, 247)
        Else
            'Height -= 2
            'Top += 1
            'Icon.BackgroundImage.Dispose()
            'Icon.BackColor = Color.FromArgb(56, 56, 57)
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


    Public Class TopBarButtonIcon
        Inherits Control
        Public Shadows Property Parent As TopBarButton
            Get
                Return DirectCast(MyBase.Parent, TopBarButton)
            End Get
            Set(value As TopBarButton)
                MyBase.Parent = value
            End Set
        End Property
        Protected Overrides Sub OnPaintBackground(pevent As PaintEventArgs)
            With pevent.Graphics
                '.FillRectangle(Parent.HighlightBrush, New Rectangle(Point.Empty, New Size(Width, (Height \ 2) + 2)))
            End With
            MyBase.OnPaintBackground(pevent)
        End Sub
        Protected Overrides Sub OnMouseEnter(e As EventArgs)
            MyBase.OnMouseEnter(e)
            Parent.SelectButton(True)
        End Sub
        Protected Overrides Sub OnMouseLeave(e As EventArgs)
            MyBase.OnMouseLeave(e)
            Parent.SelectButton(False)
        End Sub
        Public Sub New(ParentButton As TopBarButton, BMP As Bitmap)
            Parent = ParentButton
            With Parent
                Size = New Size(.Height - 2, .Height - 5)
            End With
            Location = New Point(1, 1)
            BackgroundImageLayout = ImageLayout.Center
            'BackgroundImage = BMP
            BackgroundImage = BMP

            'BackColor = Color.FromArgb(235, 235, 235)
        End Sub
    End Class
End Class

