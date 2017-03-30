Option Strict On
Option Explicit On
Option Infer Off
Public Enum HorizontalPaddingSide
    Left
    Right
End Enum
Public Class LoginField
    Inherits Control
    Private ContainerControl As Control
    Private TB As TextBox
    Private varTextAlign As TextBoxAlignment
    Private varBorders(3) As Integer
    Private HeaderControl As AudiopoLib.FullWidthControl
    Private varPaddingSides(1) As Integer
    Private IsLoaded As Boolean
    Public Shadows Property Padding(ByVal Side As HorizontalPaddingSide) As Integer
        Get
            Return varPaddingSides(Side)
        End Get
        Set(value As Integer)
            varPaddingSides(Side) = value
            RefreshAll()
        End Set
    End Property
    Public ReadOnly Property InnerTextField As TextBox
        Get
            Return TB
        End Get
    End Property
    Public ReadOnly Property Header As AudiopoLib.FullWidthControl
        Get
            Return HeaderControl
        End Get
    End Property
    Public Property TextAlign As TextBoxAlignment
        Get
            Return varTextAlign
        End Get
        Set(value As TextBoxAlignment)
            varTextAlign = value
        End Set
    End Property
    Public Shadows Sub SuspendLayout()
        MyBase.SuspendLayout()
        IsLoaded = False
    End Sub
    Public Shadows Sub ResumeLayout()
        MyBase.ResumeLayout()
        IsLoaded = True
        RefreshAll()
    End Sub
    Public Sub New()
        TB = New TextBox
        ContainerControl = New Control
        HeaderControl = New AudiopoLib.FullWidthControl(Me)
        With TB
            .BorderStyle = BorderStyle.None
            .Parent = ContainerControl
            .Font = New Font(.Font.FontFamily, 12)
            .Multiline = True
            .WordWrap = False
        End With
        With ContainerControl
            .Parent = Me
        End With
        With HeaderControl
            .Height = 20
            .Show()
            .Text = "Header"
            .TextAlign = ContentAlignment.MiddleLeft
            .BackColor = Color.FromArgb(220, 220, 220)
            .ForeColor = Color.FromArgb(40, 40, 40)
        End With
        Size = New Size(200, 60)
        BackColor = Color.White
        BorderColor = Color.FromArgb(220, 220, 220)
        SetBorders(1, 1, 1, 1)
        Padding(HorizontalPaddingSide.Left) = 5
        Padding(HorizontalPaddingSide.Right) = 5
        IsLoaded = True
        RefreshAll()
    End Sub
    Public Shadows Property Text As String
        Get
            Return TB.Text
        End Get
        Set(value As String)
            MyBase.Text = value
            TB.Text = value
        End Set
    End Property
    Public Shadows Property ForeColor As Color
        Get
            Return TB.ForeColor
        End Get
        Set(value As Color)
            MyBase.ForeColor = value
            TB.ForeColor = value
        End Set
    End Property
    Protected Overrides Sub OnSizeChanged(e As EventArgs)
        MyBase.OnSizeChanged(e)
        RefreshAll()
    End Sub
    Public Shadows Property BackColor As Color
        Get
            Return ContainerControl.BackColor
        End Get
        Set(value As Color)
            ContainerControl.BackColor = value
            TB.BackColor = value
        End Set
    End Property
    Public Shadows Property Font As Font
        Get
            Return TB.Font
        End Get
        Set(value As Font)
            TB.Font = value
            RefreshAll()
        End Set
    End Property
    Public Property BorderColor As Color
        Get
            Return MyBase.BackColor
        End Get
        Set(value As Color)
            MyBase.BackColor = value
        End Set
    End Property
    Private Const Alphabet As String = "abcdefghijklmnopqrstuvwxyzæøåABCDEFGHIJKLMNOPQRSTUVWXYZ"
    Private Sub RefreshAll()
        If IsLoaded Then
            With HeaderControl
                .Width = Width - (varBorders(0) + varBorders(2))
                .Left = varBorders(0)
                .Top = varBorders(1)
            End With
            With ContainerControl
                .Location = New Point(varBorders(0), HeaderControl.Bottom)
                .Size = New Size(Width - (varBorders(0) + varBorders(2)), Height - (varBorders(3) + HeaderControl.Bottom))
            End With
            With TB
                .Height = TextRenderer.MeasureText(Alphabet, .Font).Height + 4
                .Top = ContainerControl.Height \ 2 - .DisplayRectangle.Height \ 2 + 2
                .Width = ContainerControl.Width - (varPaddingSides(0) + varPaddingSides(1))
                .Left = varPaddingSides(0)
            End With
        End If
    End Sub
    Public Sub SetBorders(ByVal Top As Integer, ByVal Left As Integer, ByVal Right As Integer, ByVal Bottom As Integer)
        varBorders = {Left, Top, Right, Bottom}
        RefreshAll()
    End Sub
End Class
Public Enum TextBoxVerticalAlign
    Top
    Middle
    Bottom
End Enum
Public Enum TextBoxHorizontalAlign
    Left
    Center
    Right
End Enum
Public Structure TextBoxAlignment
    Public Horizontal As TextBoxVerticalAlign
    Public Vertical As TextBoxHorizontalAlign
    Public Sub New(ByVal Horizontal As TextBoxHorizontalAlign, ByVal Vertical As TextBoxVerticalAlign)
        With Me
            .Horizontal = Vertical
            .Vertical = Horizontal
        End With
    End Sub
End Structure