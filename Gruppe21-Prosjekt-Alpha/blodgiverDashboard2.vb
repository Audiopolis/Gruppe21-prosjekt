Imports AudiopoLib


Public Class blodgiverDashboard2
    Dim Header As New TopBar(Me)
    Dim ScrollList As New Donasjoner(Me)
    Private Sub blodgiverDashboard2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        With Header
            .AddButton(My.Resources.TimeBestillingIcon, "Bestill ny time", New Size(135, 36))
            .AddButton(My.Resources.EgenerklaeringIcon, "Registrer egenerklæring", New Size(135, 36))
            .AddButton(My.Resources.RedigerProfilIcon, "Rediger profil", New Size(135, 36))
            .AddLogout("Logg ut", New Size(135, 36))
        End With

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