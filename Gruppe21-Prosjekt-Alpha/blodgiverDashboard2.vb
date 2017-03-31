Imports AudiopoLib


Public Class blodgiverDashboard2
    Dim ScrollList As New Donasjoner(Me)
    Private Sub blodgiverDashboard2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        With ScrollList
            .Parent = Me
            .Location = New Point(10, 10)
            With .List
                .AddItem()
                .AddItem()
                .AddItem()
                .AddItem()
                .AddItem()
                .AddItem()
                .AddItem()
                .AddItem()
                .AddItem()
                .AddItem()
                .AddItem()
                .AddItem()
                .AddItem()
                .AddItem()
                .AddItem()
            End With
            .Size = New Size(300, 300)
        End With

    End Sub
End Class