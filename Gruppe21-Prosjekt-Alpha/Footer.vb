Public Class Footer
    Inherits Control
    Private varParentControl As Control
    Public Shadows Property Parent As Control
        Get
            Return MyBase.Parent
        End Get
        Set(value As Control)
            If varParentControl IsNot Nothing Then
                RemoveHandler varParentControl.Resize, AddressOf OnParentResize
            End If
            varParentControl = value
            AddHandler varParentControl.Resize, AddressOf OnParentResize
            MyBase.Parent = value
        End Set
    End Property
    Public Sub New(ParentControl As Control, BackgroundColor As Color, Height As Integer)
        DoubleBuffered = True
        Parent = ParentControl
        BackColor = BackgroundColor
        Me.Height = Height
    End Sub
    Private Sub OnParentResize(Sender As Object, e As EventArgs)
        With varParentControl
            Top = .Height - Height
            Width = .Width
        End With
    End Sub
End Class
