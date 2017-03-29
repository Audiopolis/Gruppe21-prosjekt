Option Strict On
Option Explicit On
Option Infer Off

Public Class InfoTable
    Inherits Control
    Private LabHeader, LabDescription As Label
    Private varPadding As Integer
    Public Sub New(HeaderText As String, DescriptionText As String, Padding As Integer)
        With Me
            .Hide()
            .Width = 300
            .Height = 400
            .BackColor = Color.White
        End With
        LabHeader = New Label
        LabDescription = New Label
        varPadding = Padding
        With LabHeader
            .Parent = Me
            .Left = Padding
            .Top = Padding
            .Height = 30
            .TextAlign = ContentAlignment.TopLeft
            .Font = New Font(.Font.FontFamily, 12)
            .Width = 300 - Padding * 2
            .Text = HeaderText
        End With
        With LabDescription
            .Parent = Me
            .Left = Padding
            .Top = LabHeader.Height + 10
            .Height = 200
            .TextAlign = ContentAlignment.TopLeft
            .Width = 300 - Padding * 2
            .Text = DescriptionText
        End With
        Show()
    End Sub
End Class
