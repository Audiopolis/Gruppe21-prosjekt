Public Class TopBar
    Inherits ContainerControl
    Private logo As New HemoGlobeLogo
    Private buttonList As New List(Of TopBarButton)

    Public Sub New(ParentControl As Control)
        BackColor = Color.FromArgb(110, 120, 127)
        Dock = DockStyle.Top
        Height = 120
        Parent = ParentControl
        With logo
            .Parent = Me
        End With
    End Sub

    Public Sub AddButton(Icon As Bitmap, Text As String, Size As Size)

        Dim NB As New TopBarButton(Me, Icon, Text, Size)

        With buttonList
            If .Count > 0 Then
                NB.Left = buttonList.Last().Right + 40
                .Add(NB)
            Else
                NB.Left = logo.Right + 40

            End If

        End With

    End Sub

End Class
Public Class TopBarButton
    Inherits Control
    Private TBButtonLabel As New Label
    Private Icon As TBButtonIcon
    Public ReadOnly Property Label As Label
        Get
            Return TBButtonLabel
        End Get
    End Property
    Protected Friend Sub New(ParentTopBar As TopBar, BMP As Bitmap, LabTxt As String, Size As Size)
        Hide()
        BackColor = Color.FromArgb(247, 247, 247)
        ForeColor = Color.FromArgb(30, 30, 30)
        Me.Size = Size
        Icon = New TBButtonIcon(Me)
        Parent = ParentTopBar
        With TBButtonLabel
            .Parent = Me
            .Height = Height
            .Left = Icon.Right
            .Width = Width - .Left
            .TextAlign = ContentAlignment.MiddleLeft
            .Padding = New Padding(10, 0, 0, 0)
        End With
        With Icon
            .BackgroundImage = BMP
        End With
        TBButtonLabel.Text = LabTxt
        Show()
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        With TBButtonLabel
            .Width = Width - .Left
        End With
    End Sub


    Protected Friend Class TBButtonIcon
        Inherits PictureBox
        Public Sub New(ParentButton As TopBarButton)
            Parent = ParentButton
            With Parent
                Size = New Size(.Height, .Height)
                BackgroundImageLayout = ImageLayout.Center
            End With
        End Sub
    End Class
End Class

