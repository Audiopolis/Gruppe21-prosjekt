Imports AudiopoLib


Public Class blodgiverDashboard2
    Dim Header As New TopBar(Me)
    Dim ScrollList As New Donasjoner(Me)
    Private Sub blodgiverDashboard2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Header.AddButton(My.Resources.TimeBestillingIcon, "Hello", New Size(135, 35))

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