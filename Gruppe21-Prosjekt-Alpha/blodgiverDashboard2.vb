'Imports AudiopoLib


'Public Class blodgiverDashboard2
'    Dim Header As New TopBar(Me)
'    Dim ScrollList As New Donasjoner(Me)
'    Dim Beholder As New BlodBeholder(My.Resources.Tom_beholder, My.Resources.Full_beholder)
'    Dim WithEvents ClickTest As New FullWidthControl(Me, True)
'    Dim current As Integer
'    Dim increment As Boolean = True
'    Private Sub SubClickTest() Handles ClickTest.Click
'        If increment Then
'            current += 30
'        Else
'            current -= 30
'        End If
'        Beholder.SlideToPercentage(current)
'        If current >= 90 Then
'            increment = False
'        ElseIf current <= 0 Then
'            increment = True
'        End If
'    End Sub
'    Private Sub blodgiverDashboard2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
'        With Header
'            .AddButton(My.Resources.TimeBestillingIcon, "Bestill ny time", New Size(135, 36))
'            .AddButton(My.Resources.EgenerklaeringIcon, "Registrer egenerklæring", New Size(135, 36))
'            .AddButton(My.Resources.RedigerProfilIcon, "Rediger profil", New Size(135, 36))
'            .AddLogout("Logg ut", New Size(135, 36))
'        End With
'        With ClickTest
'            .Top = Header.Bottom
'            .Width = 2000
'        End With
'        With ScrollList
'            .Parent = Me
'            .Location = New Point(10, 10)
'            With .List
'                .AddItem()
'                .AddItem()
'                .AddItem()
'                .AddItem()
'                .AddItem()
'                .AddItem()
'                .AddItem()
'                .AddItem()
'                .AddItem()
'                .AddItem()
'                .AddItem()
'                .AddItem()
'                .AddItem()
'                .AddItem()
'                .AddItem()
'            End With
'            .Size = New Size(300, 300)
'        End With
'        With Beholder
'            .Parent = Me
'            .Location = New Point(ClientSize.Width - .Width - 20, Header.Bottom + 20)
'        End With
'    End Sub
'    Protected Overrides Sub OnResize(e As EventArgs)
'        SuspendLayout()
'        MyBase.OnResize(e)
'        With Beholder
'            .Location = New Point(ClientSize.Width - .Width - 20, Header.Bottom + 20)
'        End With
'        ResumeLayout(True)
'    End Sub
'End Class