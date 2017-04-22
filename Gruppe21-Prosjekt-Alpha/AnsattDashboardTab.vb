Imports AudiopoLib

Public Class AnsattDashboardTab
    Inherits Tab
    Private ViewContainer As New BorderControl(Color.FromArgb(220, 220, 220))
    Private RightContainer As New BorderControl(Color.FromArgb(220, 220, 220))
    Private ViewList As New MultiTabWindow(ViewContainer)
    Private RightViewList As New MultiTabWindow(RightContainer)
    Private T_View As New T_RightView(RightViewList)

    Private Egenerklæringer As New Egenerklæringsliste
    Private Timer As New StaffTimeliste
    Public WithEvents T_NotificationList, E_NotificationList As StaffNotificationContainer
    Private Header As New TopBar(Me)
    Private Footer As New Footer(Me)
    'Dim ScrollList As New Donasjoner(Me)
    Private WelcomeLabel As New InfoLabel(True, Direction.Right)
    Private Messages As New MessageNotification(Header)
    Private varTimerHentet, varErklæringerHentet As Boolean
    Private WithEvents DBC_HentEgenerklæringer, DBC_HentTimer As New DatabaseClient(Credentials.Server, Credentials.Database, Credentials.UserID, Credentials.Password)
    Private Sub NotificationList_Reloaded(Sender As Object, e As EventArgs) Handles T_NotificationList.Reloaded
        With DBC_HentEgenerklæringer
            .SQLQuery = "SELECT A.* FROM Egenerklæring A INNER JOIN Time B ON A.time_id = B.time_id WHERE B.a_id = @aid AND A.svar IS NULL ORDER BY A.time_id DESC LIMIT 20;"
            .Execute({"@aid"}, {CStr(CurrentStaff.ID)})
        End With
        With DBC_HentTimer
            .SQLQuery = "SELECT * FROM Time WHERE (a_id = @aid AND ansatt_godkjent = 1 OR a_id IS NULL) AND fullført = 0 ORDER BY t_dato ASC LIMIT 20;"
            .Execute({"@aid"}, {CStr(CurrentStaff.ID)})
        End With
    End Sub
    Public Sub GetData()
        T_NotificationList.Reload()
    End Sub
    Private Sub DBC_HentTimer_Ferdig(sender As Object, e As DatabaseListEventArgs) Handles DBC_HentTimer.ListLoaded
        If e.ErrorOccurred Then
            T_NotificationList.ShowMessage("Det oppsto en uventet feil." & vbNewLine & "Sjekk tilkoblingen.", NotificationPreset.OffRedAlert)
        Else
            With e.Data
                If .Rows.Count > 0 Then
                    For Each Row As DataRow In .Rows
                        Dim TimeID As Integer = DirectCast(Row.Item(0), Integer)
                        Dim AnsattGodkjent As Boolean = DirectCast(Row.Item(1), Boolean)
                        Dim Dato As Date = DirectCast(Row.Item(2), Date).Date.Add(DirectCast(Row.Item(3), TimeSpan))
                        Dim AnsattID As Object = Row.Item(4)
                        Dim Fødselsnummer As String = DirectCast(Row.Item(5), Int64).ToString
                        Dim Behandlet As Boolean = DirectCast(Row.Item(6), Boolean)
                        Dim NewTime As New StaffTimeliste.StaffTime(TimeID, Dato, AnsattGodkjent, Fødselsnummer, AnsattID)
                        Timer.Add(NewTime)
                        If NewTime.AnsattID <> CurrentStaff.ID Then
                            T_NotificationList.AddNotification("Timeforespørsel fra " & NewTime.Fødselsnummer.ToString, 0, AddressOf Testsub, Color.Blue, NewTime, DatabaseElementType.Time)
                        End If
                    Next
                Else
                    T_NotificationList.Spin(False)
                End If
            End With
            varTimerHentet = True
        End If
    End Sub
    Private Sub Testsub(Sender As StaffNotification, e As StaffNotificationEventArgs)
        T_View.SelectTime(Sender, e)
    End Sub
    Private Sub DBC_HentEgenerklæringer_Finished(sender As Object, e As DatabaseListEventArgs) Handles DBC_HentEgenerklæringer.ListLoaded
        If e.ErrorOccurred Then
            T_NotificationList.ShowMessage("Det oppsto en uventet feil." & vbNewLine & "Sjekk tilkoblingen.", NotificationPreset.OffRedAlert)
        Else
            With e.Data
                If .Rows.Count > 0 Then
                    For Each Row As DataRow In .Rows
                        Dim TimeID As Integer = DirectCast(Row.Item(0), Integer)
                        Dim SvarString As String = DirectCast(Row.Item(1), String)
                        Dim Land As String = DirectCast(Row.Item(2), String)
                        Dim Godkjent As Boolean = DirectCast(Row.Item(3), Boolean)
                        Dim NewEgenerklæring As New Egenerklæringsliste.Egenerklæring(TimeID, SvarString, Land, Godkjent)
                        If Not IsDBNull(Row.Item(4)) Then
                            NewEgenerklæring.AnsattSvar = DirectCast(Row.Item(4), String)
                        End If
                        Egenerklæringer.Add(NewEgenerklæring)
                    Next
                End If
                varErklæringerHentet = True
            End With
        End If
    End Sub
    Public Sub New(ParentWindow As MultiTabWindow)
        MyBase.New(ParentWindow)
        With ViewContainer
            .Size = New Size(502, 502)
            .Left = 20
            .Parent = Me
        End With
        With RightContainer
            .Size = New Size(602, 502)
            .Left = ViewContainer.Right + 20
            .Parent = Me
        End With
        With ViewList
            .Size = New Size(500, 500)
            .Location = New Point(1, 1)
            .BackColor = Color.White
            .Show()
        End With
        With RightViewList
            .Size = New Size(600, 500)
            .Location = New Point(1, 1)
            .BackColor = Color.White
            .Show()
        End With
        T_NotificationList = New StaffNotificationContainer(ViewList, "Timeforespørsler")
        E_NotificationList = New StaffNotificationContainer(ViewList, "Nye egenerklæringer")
        RightViewList.Index = 0
        ViewList.Index = 0
    End Sub
    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        If ViewContainer IsNot Nothing Then
            With ViewContainer
                .Top = (ClientSize.Height - .Height + Header.Bottom - Footer.Height) \ 2
                RightContainer.Top = .Top
            End With
        End If
    End Sub

    Private Class E_RightView
        Inherits Tab
        Private varSelectedNotification As StaffNotification
        Private WithEvents SendSvarKnapp As New FullWidthControl(Me, True, FullWidthControl.SnapType.Bottom)
        Private DonorInfoLabels(8), SkjemaSvar As Label
        Private Header As New FullWidthControl(Me)
        Private RedigerEgenerklæring, AutoSjekk As New BorderControl(Color.FromArgb(0, 100, 235))
        Private AcceptedCheckbox As New CheckBox
        Private SvarTextBox As New TextBox
        Private WithEvents DBC_Oppdater As New DatabaseClient(Credentials.Server, Credentials.Database, Credentials.UserID, Credentials.Password)
        Private WithEvents RedigerSkjemaLab, AutoSjekkLab As New Label
        Public Sub New(ParentWindow As MultiTabWindow)
            MyBase.New(ParentWindow)
            With Header
                .Text = "Valgt egenerklæring"
            End With
            For i As Integer = 0 To DonorInfoLabels.Length - 1
                DonorInfoLabels(i) = New Label
                With DonorInfoLabels(i)
                    .Parent = Me
                    .AutoSize = False
                    .Size = New Size(300, 20)
                    .Location = New Point(20, Header.Bottom + 20 * (i + 1))
                End With
            Next
            SkjemaSvar = New Label
            With SkjemaSvar
                .Parent = Me
                .AutoSize = False
                .Size = New Size(300, 20)
                .Location = New Point(320, Header.Bottom + 20)
            End With
            With RedigerEgenerklæring
                .Parent = Me
                .DrawBorder(FormField.ElementSide.Left) = False
                .DrawBorder(FormField.ElementSide.Top) = False
                .DrawBorder(FormField.ElementSide.Right) = False
                .DrawBorder(FormField.ElementSide.Bottom) = False
                .Size = TextRenderer.MeasureText("Se/rediger skjema...", DefaultFont)
                .Location = New Point(320, Header.Bottom + 80)
            End With
            With AutoSjekk
                .Parent = Me
                .DrawBorder(FormField.ElementSide.Left) = False
                .DrawBorder(FormField.ElementSide.Top) = False
                .DrawBorder(FormField.ElementSide.Right) = False
                .DrawBorder(FormField.ElementSide.Bottom) = False
                .Size = TextRenderer.MeasureText("Utfør automatisk sjekk...", DefaultFont)
                .Location = New Point(320, Header.Bottom + 100)
            End With
            With RedigerSkjemaLab
                .AutoSize = False
                .Parent = RedigerEgenerklæring
                .Size = New Size(RedigerEgenerklæring.Width, RedigerEgenerklæring.Height - 1)
                .ForeColor = Color.FromArgb(0, 100, 235)
                .Text = "Se/rediger skjema..."
            End With
            With AutoSjekkLab
                .AutoSize = False
                .Parent = AutoSjekk
                .Size = New Size(AutoSjekk.Width, AutoSjekk.Height - 1)
                .ForeColor = Color.FromArgb(0, 100, 235)
                .Text = "Utfør automatisk sjekk..."
            End With
            With SendSvarKnapp
                .Size = New Size(130, 40)
                .Text = "Avslå timeforespørsel"
                .BackColorNormal = Color.LimeGreen
                .BackColorSelected = ColorHelper.Multiply(.BackColorNormal, 0.7)
                .BackColorDisabled = Color.FromArgb(200, 200, 200)
                .Enabled = False
                .ForeColor = Color.White
                .Left = 20
            End With
            With AcceptedCheckbox
                .Text = "Denne personen kan gi blod"
                .Parent = Me
                .Left = SendSvarKnapp.Right + 10
                .AutoSize = False
                .Width = 100
                .Height = 40
            End With
            With SvarTextBox
                .Parent = Me
                .Location = New Point(200, 300)
                .Multiline = True
                .WordWrap = True
                .Size = New Size(100, 50)
            End With
        End Sub
        Private Sub DBC_Oppdater_Finished(sender As Object, e As DatabaseListEventArgs) Handles DBC_Oppdater.ListLoaded
            If e.ErrorOccurred Then
                MsgBox("Error")
            Else
                MsgBox("Success")
                varSelectedNotification.Close()
            End If
        End Sub
        Private Sub SendSvar_Click() Handles SendSvarKnapp.Click
            If AcceptedCheckbox.Checked Then
                If SvarTextBox.Text = "" Then SvarTextBox.Text = "Godkjent"
                DBC_Oppdater.SQLQuery = "UPDATE Egenerklæring SET svar = @svar, godkjent = 1 WHERE time_id = @timeid LIMIT 1;"
                With DirectCast(varSelectedNotification.RelatedElement, Egenerklæringsliste.Egenerklæring)
                    DBC_Oppdater.Execute({"@svar", "@timeid"}, {SvarTextBox.Text, .TimeID.ToString})
                End With
                ' TODO: Loading graphics
            Else
                If Not SvarTextBox.Text = "" Then
                    DBC_Oppdater.SQLQuery = "UPDATE Egenerklæring SET svar = @svar WHERE time_id = @timeid LIMIT 1;"
                    With DirectCast(varSelectedNotification.RelatedElement, Egenerklæringsliste.Egenerklæring)
                        DBC_Oppdater.Execute({"@svar", "@timeid"}, {SvarTextBox.Text, .TimeID.ToString})
                    End With
                    ' TODO: Prevent changes while updating
                Else
                    SvarTextBox.Focus()
                    MsgBox("Begrunnelse må gis")
                End If
            End If
        End Sub
        Private Sub RedigerSkjemaLab_MouseEnter() Handles RedigerSkjemaLab.MouseEnter
            RedigerEgenerklæring.DrawBorder(FormField.ElementSide.Bottom) = True
        End Sub
        Private Sub RedigerSkjemaLab_MouseLeave() Handles RedigerSkjemaLab.MouseLeave
            RedigerEgenerklæring.DrawBorder(FormField.ElementSide.Bottom) = False
        End Sub
        Private Sub AutoSjekkLab_MouseEnter() Handles AutoSjekkLab.MouseEnter
            AutoSjekk.DrawBorder(FormField.ElementSide.Bottom) = True
        End Sub
        Private Sub AutoSjekkLab_MouseLeave() Handles AutoSjekkLab.MouseLeave
            AutoSjekk.DrawBorder(FormField.ElementSide.Bottom) = False
        End Sub
        Private Sub RedigerSkjemaLab_Click() Handles RedigerSkjemaLab.Click
            MsgBox("TODO: Vis skjema i under-tab")
        End Sub
        Private Sub AutoSJekkLab_Click() Handles AutoSjekkLab.Click
            MsgBox("TODO: Vis alternativer for auto-sjekk")
        End Sub
        Public Sub SelectErklæring(Sender As StaffNotification, e As StaffNotificationEventArgs)
            If varSelectedNotification IsNot Nothing Then
                varSelectedNotification.BackColor = varSelectedNotification.DefaultColor
                varSelectedNotification.IsSelected = False
            End If
            varSelectedNotification = Sender
            With Sender
                .IsSelected = True
                .BackColor = .PressColor
                If .RelatedDonor IsNot Nothing Then
                    With .RelatedDonor
                        DonorInfoLabels(0).Text = "Fødselsnummer: " & .Fødselsnummer.ToString
                        DonorInfoLabels(1).Text = "Fornavn: " & .Fornavn
                        DonorInfoLabels(2).Text = "Etternavn: " & .Etternavn
                        DonorInfoLabels(3).Text = "Telefon: " & .Telefon(0)
                        DonorInfoLabels(4).Text = "Epostadresse: " & .Epost
                        DonorInfoLabels(5).Text = "Adresse: " & .Adresse
                        Dim PostnummerString As String = .Postnummer.ToString
                        If PostnummerString.Length < 4 Then
                            PostnummerString = "0" & PostnummerString
                        End If
                        DonorInfoLabels(6).Text = "Postnummer: " & PostnummerString
                        Dim KjønnString As String
                        If .Hankjønn Then
                            KjønnString = "Mann"
                        Else
                            KjønnString = "Kvinne"
                        End If
                        DonorInfoLabels(7).Text = "Kjønn: " & KjønnString
                        DonorInfoLabels(8).Text = "Blodtype: " & .Blodtype
                    End With
                    With DirectCast(.RelatedElement, Egenerklæringsliste.Egenerklæring)
                        SkjemaSvar.Text = "Sammendrag: " & .SkjemaString
                    End With
                Else
                    ' TODO: Loading graphics
                    MsgBox("Donor nothing")
                End If
            End With
        End Sub
        Protected Overrides Sub OnResize(e As EventArgs)
            MyBase.OnResize(e)
            Header.Width = Width
            With SendSvarKnapp
                .Top = Height - .Height - 20
                .Left = 20
            End With
            With AcceptedCheckbox
                .Top = SendSvarKnapp.Top
            End With
        End Sub
    End Class



    Private Class T_RightView
        Inherits Tab
        Private varSelectedNotification As StaffNotification
        Private WithEvents GodkjennKnapp, AvslåKnapp As New FullWidthControl(Me, True, FullWidthControl.SnapType.Bottom)
        Private DonorInfoLabels(8), TimeInfoLabels(1) As Label
        Private Header As New FullWidthControl(Me)
        Private EndreDato, EndreKlokkeslett As New BorderControl(Color.FromArgb(0, 100, 235))
        Private WithEvents DBC_Oppdater As New DatabaseClient(Credentials.Server, Credentials.Database, Credentials.UserID, Credentials.Password)

        Private WithEvents EndreDatoLab, EndreKlokkeslettLab As New Label
        Public Sub New(ParentWindow As MultiTabWindow)
            MyBase.New(ParentWindow)
            With Header
                .Text = "Valgt timeforespørsel"
            End With
            For i As Integer = 0 To DonorInfoLabels.Length - 1
                DonorInfoLabels(i) = New Label
                With DonorInfoLabels(i)
                    .Parent = Me
                    .AutoSize = False
                    .Size = New Size(300, 20)
                    .Location = New Point(20, Header.Bottom + 20 * (i + 1))
                End With
            Next
            For i As Integer = 0 To 1
                TimeInfoLabels(i) = New Label
                With TimeInfoLabels(i)
                    .Parent = Me
                    .AutoSize = False
                    .Size = New Size(300, 20)
                    .Location = New Point(320, Header.Bottom + 20 * (i + 1))
                End With
            Next
            With EndreDato
                .Parent = Me
                .DrawBorder(FormField.ElementSide.Left) = False
                .DrawBorder(FormField.ElementSide.Top) = False
                .DrawBorder(FormField.ElementSide.Right) = False
                .DrawBorder(FormField.ElementSide.Bottom) = False
                .Size = TextRenderer.MeasureText("Endre dato...", DefaultFont)
                .Location = New Point(320, Header.Bottom + 80)
            End With
            With EndreKlokkeslett
                .Parent = Me
                .DrawBorder(FormField.ElementSide.Left) = False
                .DrawBorder(FormField.ElementSide.Top) = False
                .DrawBorder(FormField.ElementSide.Right) = False
                .DrawBorder(FormField.ElementSide.Bottom) = False
                .Size = TextRenderer.MeasureText("Endre klokkeslett...", DefaultFont)
                .Location = New Point(320, Header.Bottom + 100)
            End With
            With EndreDatoLab
                .AutoSize = False
                .Parent = EndreDato
                .Size = New Size(EndreDato.Width, EndreDato.Height - 1)
                .ForeColor = Color.FromArgb(0, 100, 235)
                .Text = "Endre dato..."
            End With
            With EndreKlokkeslettLab
                .AutoSize = False
                .Parent = EndreKlokkeslett
                .Size = New Size(EndreKlokkeslett.Width, EndreKlokkeslett.Height - 1)
                .ForeColor = Color.FromArgb(0, 100, 235)
                .Text = "Endre klokkeslett..."
            End With
            With AvslåKnapp
                .Size = New Size(130, 40)
                .Text = "Avslå timeforespørsel"
                .BackColorNormal = Color.FromArgb(162, 25, 51)
                .BackColorSelected = ColorHelper.Multiply(.BackColorNormal, 0.7)
                .ForeColor = Color.White
                .Left = 20
            End With
            With GodkjennKnapp
                .Size = New Size(130, 40)
                .Text = "Send innkalling"
                .ForeColor = Color.White
                .BackColorNormal = Color.LimeGreen
                .BackColorSelected = ColorHelper.Multiply(Color.LimeGreen, 0.7)
                .Left = AvslåKnapp.Width + 40
            End With
        End Sub
        Private Sub Avslå_Click() Handles AvslåKnapp.Click
            If MsgBox("Hvis det er kapasitet på en nærliggende dag, endre dato og tidspunkt i stedet for å avslå timeforespørselen. Blodgiveren vil bli automatisk informert.", MsgBoxStyle.OkCancel, "Forkast timeforespørsel") = MsgBoxResult.Ok Then
                DBC_Oppdater.SQLQuery = "UPDATE Time SET ansatt_godkjent = 0, a_id = @aid WHERE time_id = @timeid LIMIT 1;"
                With DirectCast(varSelectedNotification.RelatedElement, StaffTimeliste.StaffTime)
                    DBC_Oppdater.Execute({"@aid", "@timeid"}, {CurrentStaff.ID.ToString, .TimeID.ToString})
                End With
                ' TODO: Prevent changes while updating
            End If
        End Sub
        Private Sub DBC_Oppdater_Finished(sender As Object, e As DatabaseListEventArgs) Handles DBC_Oppdater.ListLoaded
            If e.ErrorOccurred Then
                MsgBox("Error")
            Else
                MsgBox("Success")
                varSelectedNotification.Close()
            End If
        End Sub
        Private Sub Godkjenn_Click() Handles GodkjennKnapp.Click
            DBC_Oppdater.SQLQuery = "UPDATE Time SET ansatt_godkjent = 1, a_id = @aid, t_dato = @dato, t_klokkeslett = @klokkeslett WHERE time_id = @timeid LIMIT 1;"
            With DirectCast(varSelectedNotification.RelatedElement, StaffTimeliste.StaffTime)
                DBC_Oppdater.Execute({"@aid", "@dato", "@klokkeslett", "@timeid"}, {CurrentStaff.ID.ToString, .DatoOgTid.ToString("yyyy-MM-dd"), .DatoOgTid.ToString("HH:mm"), .TimeID.ToString})
            End With
            ' TODO: Prevent changes while updating
        End Sub
        Private Sub EndreDatoLab_MouseEnter() Handles EndreDatoLab.MouseEnter
            EndreDato.DrawBorder(FormField.ElementSide.Bottom) = True
        End Sub
        Private Sub EndreDatoLab_MouseLeave() Handles EndreDatoLab.MouseLeave
            EndreDato.DrawBorder(FormField.ElementSide.Bottom) = False
        End Sub
        Private Sub EndreKlokkeslettLab_MouseEnter() Handles EndreKlokkeslettLab.MouseEnter
            EndreKlokkeslett.DrawBorder(FormField.ElementSide.Bottom) = True
        End Sub
        Private Sub EndreKlokkeslettLab_MouseLeave() Handles EndreKlokkeslettLab.MouseLeave
            EndreKlokkeslett.DrawBorder(FormField.ElementSide.Bottom) = False
        End Sub
        Private Sub EndreDato_Click() Handles EndreDatoLab.Click
            Try
                Dim NyDato As Date = Date.Parse(InputBox("Skriv inn ny dato (ÅÅÅÅ-MM-DD)", "Endre dato", DirectCast(varSelectedNotification.RelatedElement, StaffTimeliste.StaffTime).DatoOgTid.ToString("yyyy-MM-dd")))
                With DirectCast(varSelectedNotification.RelatedElement, StaffTimeliste.StaffTime)
                    .DatoOgTid = NyDato.Date.Add(.DatoOgTid.TimeOfDay)
                End With
                TimeInfoLabels(0).Text = "Dato: " & NyDato.ToShortDateString
            Catch
                MsgBox("Datoen ble skrevet inn feil")
            End Try
        End Sub
        Private Sub EndreKlokkeslettLab_Click() Handles EndreKlokkeslettLab.Click
            Try
                Dim NyttKlokkeslett As TimeSpan = TimeSpan.Parse(InputBox("Skriv inn nytt klokkeslett (HH:MM)", "Endre klokkeslett", DirectCast(varSelectedNotification.RelatedElement, StaffTimeliste.StaffTime).DatoOgTid.ToString("HH:mm")))
                With DirectCast(varSelectedNotification.RelatedElement, StaffTimeliste.StaffTime)
                    .DatoOgTid = .DatoOgTid.Date.Add(NyttKlokkeslett)
                    TimeInfoLabels(1).Text = "Klokkeslett: " & .DatoOgTid.ToString("HH:mm")
                End With
            Catch
                MsgBox("Datoen ble skrevet inn feil")
            End Try
        End Sub

        Public Sub SelectTime(Sender As StaffNotification, e As StaffNotificationEventArgs)
            If varSelectedNotification IsNot Nothing Then
                varSelectedNotification.BackColor = varSelectedNotification.DefaultColor
                varSelectedNotification.IsSelected = False
            End If
            varSelectedNotification = Sender
            With Sender
                .IsSelected = True
                .BackColor = .PressColor
                If .RelatedDonor IsNot Nothing Then
                    With .RelatedDonor
                        DonorInfoLabels(0).Text = "Fødselsnummer: " & .Fødselsnummer.ToString
                        DonorInfoLabels(1).Text = "Fornavn: " & .Fornavn
                        DonorInfoLabels(2).Text = "Etternavn: " & .Etternavn
                        DonorInfoLabels(3).Text = "Telefon: " & .Telefon(0)
                        DonorInfoLabels(4).Text = "Epostadresse: " & .Epost
                        DonorInfoLabels(5).Text = "Adresse: " & .Adresse
                        Dim PostnummerString As String = .Postnummer.ToString
                        If PostnummerString.Length < 4 Then
                            PostnummerString = "0" & PostnummerString
                        End If
                        DonorInfoLabels(6).Text = "Postnummer: " & PostnummerString
                        Dim KjønnString As String
                        If .Hankjønn Then
                            KjønnString = "Mann"
                        Else
                            KjønnString = "Kvinne"
                        End If
                        DonorInfoLabels(7).Text = "Kjønn: " & KjønnString
                        DonorInfoLabels(8).Text = "Blodtype: " & .Blodtype
                    End With
                    With DirectCast(.RelatedElement, StaffTimeliste.StaffTime)
                        TimeInfoLabels(0).Text = "Dato: " & .DatoOgTid.ToShortDateString
                        TimeInfoLabels(1).Text = "Klokkeslett: " & .DatoOgTid.ToString("HH:mm")
                    End With
                Else
                        ' TODO: Loading graphics
                        MsgBox("Donor nothing")
                End If
            End With
        End Sub
        Protected Overrides Sub OnResize(e As EventArgs)
            MyBase.OnResize(e)
            Header.Width = Width
            With AvslåKnapp
                .Top = Height - .Height - 20
                .Left = 20
            End With
            With GodkjennKnapp
                .Top = Height - .Height - 20
            End With
        End Sub
    End Class
End Class


Public Class Egenerklæringsliste
    Private Erklæringsliste As New List(Of Egenerklæring)
    Public Sub New()

    End Sub
    Public Sub Add(ByRef Egenerklæring As Egenerklæring)
        Erklæringsliste.Add(Egenerklæring)
    End Sub
    Public ReadOnly Property TimeAtIndex(ByVal Index As Integer) As Egenerklæring
        Get
            Return Erklæringsliste(Index)
        End Get
    End Property
    Public Function GetAllElementsWhere(ByVal Egenskap As EgenerklæringEgenskap, ByVal Verdi As Object, Optional ByVal Condition As ComparisonOperator = ComparisonOperator.EqualTo, Optional ByVal ReturnIfConditionIs As Boolean = True) As List(Of Egenerklæring)
        Dim Ret As List(Of Egenerklæring)
        Try
            Ret = Erklæringsliste.FindAll(Function(Erklæring As Egenerklæring) As Boolean
                                              Select Case Egenskap
                                                  Case EgenerklæringEgenskap.TimeID
                                                      Return ((Erklæring.TimeID = DirectCast(Verdi, Integer)) = ReturnIfConditionIs)
                                                  Case EgenerklæringEgenskap.Godkjent
                                                      Return ((Erklæring.Godkjent = DirectCast(Verdi, Boolean)) = ReturnIfConditionIs)
                                                  Case EgenerklæringEgenskap.Land
                                                      Return ((Erklæring.Land = DirectCast(Verdi, String)) = ReturnIfConditionIs)
                                                  Case EgenerklæringEgenskap.Skjema
                                                      Select Case Condition
                                                          Case ComparisonOperator.EqualTo
                                                              Return ((Erklæring.SkjemaString = DirectCast(Verdi, String)) = ReturnIfConditionIs)
                                                          Case ComparisonOperator.IsLike
                                                              Return (Erklæring.SkjemaIsLike(DirectCast(Verdi, String)) = ReturnIfConditionIs)
                                                          Case Else
                                                              Return False
                                                      End Select
                                                  Case Else
                                                      Return (ReferenceEquals(Erklæring, Verdi) = ReturnIfConditionIs)
                                              End Select
                                          End Function)
        Catch ex As Exception
            MsgBox(ex.Message)
            Ret = Nothing
        End Try
        Return Ret
    End Function
    Public Function GetElementWhere(ByVal Egenskap As EgenerklæringEgenskap, ByVal Verdi As Object, Optional ByVal Condition As ComparisonOperator = ComparisonOperator.EqualTo, Optional ByVal ReturnIfConditionIs As Boolean = True) As Egenerklæring
        Dim Ret As Egenerklæring
        Try
            Ret = Erklæringsliste.Find(Function(Erklæring As Egenerklæring) As Boolean
                                           Select Case Egenskap
                                               Case EgenerklæringEgenskap.TimeID
                                                   Return ((Erklæring.TimeID = DirectCast(Verdi, Integer)) = ReturnIfConditionIs)
                                               Case EgenerklæringEgenskap.Godkjent
                                                   Return ((Erklæring.Godkjent = DirectCast(Verdi, Boolean)) = ReturnIfConditionIs)
                                               Case EgenerklæringEgenskap.Land
                                                   Return ((Erklæring.Land = DirectCast(Verdi, String)) = ReturnIfConditionIs)
                                               Case EgenerklæringEgenskap.Skjema
                                                   Select Case Condition
                                                       Case ComparisonOperator.EqualTo
                                                           Return ((Erklæring.SkjemaString = DirectCast(Verdi, String)) = ReturnIfConditionIs)
                                                       Case ComparisonOperator.IsLike
                                                           Return (Erklæring.SkjemaIsLike(DirectCast(Verdi, String)) = ReturnIfConditionIs)
                                                       Case Else
                                                           Return False
                                                   End Select
                                               Case Else
                                                   Return (ReferenceEquals(Erklæring, Verdi) = ReturnIfConditionIs)
                                           End Select
                                       End Function)
        Catch ex As Exception
            MsgBox(ex.Message)
            Ret = Nothing
        End Try
        Return Ret
    End Function
    Public Enum EgenerklæringEgenskap As Integer
        TimeID = 0
        Godkjent = 1
        Land = 2
        Skjema = 3
        AnsattSvar = 4
    End Enum
    Public Enum ComparisonOperator As Integer
        EqualTo = 0
        GreaterThan = 1
        LessThan = 2
        BooleanTrue = 3
        ReferencesEqual = 4
        IsLike = 5
    End Enum

    Public Class Egenerklæring
        Private varTimeID As Integer
        Private varLand, varSkjemaString, varAnsattSvar As String
        Private varGodkjent As Boolean

        ''' <summary>
        ''' X = Skip check
        ''' All other chars = Must match
        ''' </summary>
        ''' <param name="Pattern">A string consisting of 25 characters. Case sensitive.</param>
        ''' <returns>A boolean value indicating whether or not the string matches the pattern.</returns>
        Public Function SkjemaIsLike(Pattern As String) As Boolean
            Dim SkipChar As Char = "X".ToCharArray()(0)
            Dim SvarArr() As Char = varSkjemaString.ToCharArray()
            Dim PatternArr() As Char = Pattern.ToCharArray
            Dim IsCompatible As Boolean = True
            For i As Integer = 0 To 24
                If Not PatternArr(i) = SkipChar Then
                    If Not PatternArr(i) = SvarArr(i) Then
                        IsCompatible = False
                        Exit For
                    End If
                End If
            Next
            Return IsCompatible
        End Function
        Public Property AnsattSvar As String
            Get
                Return varAnsattSvar
            End Get
            Set(value As String)
                varAnsattSvar = value
            End Set
        End Property
        Public Property TimeID As Integer
            Get
                Return varTimeID
            End Get
            Set(value As Integer)
                varTimeID = value
            End Set
        End Property
        Public Property Land As String
            Get
                Return varLand
            End Get
            Set(value As String)
                varLand = Land
            End Set
        End Property
        Public Property SkjemaString As String
            Get
                Return varSkjemaString
            End Get
            Set(value As String)
                varSkjemaString = value
            End Set
        End Property
        Public Property Godkjent As Boolean
            Get
                Return varGodkjent
            End Get
            Set(value As Boolean)
                varGodkjent = value
            End Set
        End Property
        Public Sub New(TimeID As Integer, SkjemaString As String, Land As String, Godkjent As Boolean)
            varTimeID = TimeID
            varSkjemaString = SkjemaString
            varLand = Land
            varGodkjent = Godkjent
        End Sub
    End Class
End Class
Public Class StaffTimeliste
    Private Timeliste As New List(Of StaffTime)
    Public Sub New()

    End Sub
    Public ReadOnly Property Timer As List(Of StaffTime)
        Get
            Return Timeliste
        End Get
    End Property
    Public ReadOnly Property TimeAtIndex(ByVal Index As Integer) As StaffTime
        Get
            Return Timeliste(Index)
        End Get
    End Property
    Public Function GetAllElementsWhere(ByVal Egenskap As TimeEgenskap, ByVal Verdi As Object, Optional ByVal Condition As ComparisonOperator = ComparisonOperator.EqualTo, Optional ByVal ReturnIfConditionIs As Boolean = True) As List(Of StaffTime)
        Dim Ret As List(Of StaffTime)
        Try
            Ret = Timeliste.FindAll(Function(Time As StaffTime) As Boolean
                                        Select Case Egenskap
                                            Case TimeEgenskap.TimeID
                                                Select Case Condition
                                                    Case ComparisonOperator.EqualTo
                                                        Return ((Time.TimeID = DirectCast(Verdi, Integer)) = ReturnIfConditionIs)
                                                    Case Else
                                                        Return False
                                                End Select
                                            Case TimeEgenskap.Dato
                                                Select Case Condition
                                                    Case ComparisonOperator.EqualTo
                                                        Return ((Time.DatoOgTid.Date = DirectCast(Verdi, Date).Date) = ReturnIfConditionIs)
                                                    Case ComparisonOperator.LessThan
                                                        Return ((Time.DatoOgTid.Date.CompareTo(DirectCast(Verdi, Date).Date) < 0) = ReturnIfConditionIs)
                                                    Case ComparisonOperator.GreaterThan
                                                        Return ((Time.DatoOgTid.Date.CompareTo(DirectCast(Verdi, Date).Date) > 0) = ReturnIfConditionIs)
                                                    Case Else
                                                        Return False
                                                End Select
                                            Case TimeEgenskap.Tid
                                                Select Case Condition
                                                    Case ComparisonOperator.EqualTo
                                                        Return ((Time.DatoOgTid.TimeOfDay = DirectCast(Verdi, TimeSpan)) = ReturnIfConditionIs)
                                                    Case ComparisonOperator.GreaterThan
                                                        Return ((Time.DatoOgTid.TimeOfDay.CompareTo(DirectCast(Verdi, TimeSpan)) > 0) = ReturnIfConditionIs)
                                                    Case ComparisonOperator.LessThan
                                                        Return ((Time.DatoOgTid.TimeOfDay.CompareTo(DirectCast(Verdi, TimeSpan)) < 0) = ReturnIfConditionIs)
                                                    Case Else
                                                        Return False
                                                End Select
                                            Case TimeEgenskap.DatoOgTid
                                                Select Case Condition
                                                    Case ComparisonOperator.EqualTo
                                                        Return ((Time.DatoOgTid = DirectCast(Verdi, Date)) = ReturnIfConditionIs)
                                                    Case ComparisonOperator.LessThan
                                                        Return ((Time.DatoOgTid.CompareTo(DirectCast(Verdi, Date)) < 0) = ReturnIfConditionIs)
                                                    Case ComparisonOperator.GreaterThan
                                                        Return ((Time.DatoOgTid.CompareTo(DirectCast(Verdi, Date)) > 0) = ReturnIfConditionIs)
                                                    Case Else
                                                        Return False
                                                End Select
                                            Case TimeEgenskap.Fødselsnummer
                                                Return ((Time.Fødselsnummer = DirectCast(Verdi, String)) = ReturnIfConditionIs)
                                            Case TimeEgenskap.Godkjent
                                                Return ((Time.Godkjent = DirectCast(Verdi, Boolean)) = ReturnIfConditionIs)
                                            Case TimeEgenskap.AnsattID
                                                If IsDBNull(Verdi) Then
                                                    Return ((Time.AnsattID < 0) = ReturnIfConditionIs)
                                                Else
                                                    Return ((Time.AnsattID = DirectCast(Verdi, Integer)) = ReturnIfConditionIs)
                                                End If
                                            Case Else
                                                Return (ReferenceEquals(Time, Verdi) = ReturnIfConditionIs)
                                        End Select
                                    End Function)
        Catch ex As Exception
            MsgBox(ex.Message)
            Ret = Nothing
        End Try
        Return Ret
    End Function
    Public Function GetElementWhere(ByVal Egenskap As TimeEgenskap, ByVal Verdi As Object, Optional ByVal Condition As ComparisonOperator = ComparisonOperator.EqualTo, Optional ByVal ReturnIfConditionIs As Boolean = True) As StaffTime
        Dim Ret As StaffTime
        Try
            Ret = Timeliste.Find(Function(Time As StaffTime) As Boolean
                                     Select Case Egenskap
                                         Case TimeEgenskap.TimeID
                                             Select Case Condition
                                                 Case ComparisonOperator.EqualTo
                                                     Return ((Time.TimeID = DirectCast(Verdi, Integer)) = ReturnIfConditionIs)
                                                 Case Else
                                                     Return False
                                             End Select
                                         Case TimeEgenskap.Dato
                                             Select Case Condition
                                                 Case ComparisonOperator.EqualTo
                                                     Return ((Time.DatoOgTid.Date = DirectCast(Verdi, Date).Date) = ReturnIfConditionIs)
                                                 Case ComparisonOperator.LessThan
                                                     Return ((Time.DatoOgTid.Date.CompareTo(DirectCast(Verdi, Date).Date) < 0) = ReturnIfConditionIs)
                                                 Case ComparisonOperator.GreaterThan
                                                     Return ((Time.DatoOgTid.Date.CompareTo(DirectCast(Verdi, Date).Date) > 0) = ReturnIfConditionIs)
                                                 Case Else
                                                     Return False
                                             End Select
                                         Case TimeEgenskap.Tid
                                             Select Case Condition
                                                 Case ComparisonOperator.EqualTo
                                                     Return ((Time.DatoOgTid.TimeOfDay = DirectCast(Verdi, TimeSpan)) = ReturnIfConditionIs)
                                                 Case ComparisonOperator.GreaterThan
                                                     Return ((Time.DatoOgTid.TimeOfDay.CompareTo(DirectCast(Verdi, TimeSpan)) > 0) = ReturnIfConditionIs)
                                                 Case ComparisonOperator.LessThan
                                                     Return ((Time.DatoOgTid.TimeOfDay.CompareTo(DirectCast(Verdi, TimeSpan)) < 0) = ReturnIfConditionIs)
                                                 Case Else
                                                     Return False
                                             End Select
                                         Case TimeEgenskap.DatoOgTid
                                             Select Case Condition
                                                 Case ComparisonOperator.EqualTo
                                                     Return ((Time.DatoOgTid = DirectCast(Verdi, Date)) = ReturnIfConditionIs)
                                                 Case ComparisonOperator.LessThan
                                                     Return ((Time.DatoOgTid.CompareTo(DirectCast(Verdi, Date)) < 0) = ReturnIfConditionIs)
                                                 Case ComparisonOperator.GreaterThan
                                                     Return ((Time.DatoOgTid.CompareTo(DirectCast(Verdi, Date)) > 0) = ReturnIfConditionIs)
                                                 Case Else
                                                     Return False
                                             End Select
                                         Case TimeEgenskap.Fødselsnummer
                                             Return ((Time.Fødselsnummer = DirectCast(Verdi, String)) = ReturnIfConditionIs)
                                         Case TimeEgenskap.Godkjent
                                             Return ((Time.Godkjent = DirectCast(Verdi, Boolean)) = ReturnIfConditionIs)
                                         Case TimeEgenskap.AnsattID
                                             If IsDBNull(Verdi) Then
                                                 Return ((Time.AnsattID < 0) = ReturnIfConditionIs)
                                             Else
                                                 Return ((Time.AnsattID = DirectCast(Verdi, Integer)) = ReturnIfConditionIs)
                                             End If
                                         Case Else
                                             Return (ReferenceEquals(Time, Verdi) = ReturnIfConditionIs)
                                     End Select
                                 End Function)
        Catch ex As Exception
            MsgBox(ex.Message)
            Ret = Nothing
        End Try
        Return Ret
    End Function
    Public Enum TimeEgenskap As Integer
        TimeID = 0
        Dato = 1
        Tid = 2
        DatoOgTid = 3
        Fødselsnummer = 4
        Godkjent = 5
        AnsattID = 6
        Reference = 7
    End Enum
    Public Enum ComparisonOperator As Integer
        EqualTo = 0
        GreaterThan = 1
        LessThan = 2
        BooleanTrue = 3
        ReferencesEqual = 4
    End Enum
    Public Sub Add(ByRef Time As StaffTime)
        Timeliste.Add(Time)
    End Sub
    Public Class StaffTime
        Private varTimeID As Integer
        Private varDatoOgTid As Date
        Private varFødselsnummer As String
        Private varGodkjent As Boolean
        Private varAnsattID As Integer = -1
        Public Property AnsattID As Integer
            Get
                Return varAnsattID
            End Get
            Set(value As Integer)
                varAnsattID = value
            End Set
        End Property
        Public Property TimeID As Integer
            Get
                Return varTimeID
            End Get
            Set(value As Integer)
                varTimeID = value
            End Set
        End Property
        Public Property DatoOgTid As Date
            Get
                Return varDatoOgTid
            End Get
            Set(value As Date)
                varDatoOgTid = value
            End Set
        End Property
        Public Property Godkjent As Boolean
            Get
                Return varGodkjent
            End Get
            Set(value As Boolean)
                varGodkjent = value
            End Set
        End Property
        Public Property Fødselsnummer As String
            Get
                Return varFødselsnummer
            End Get
            Set(value As String)
                varFødselsnummer = value
            End Set
        End Property
        Public Sub New(TimeID As Integer, DatoOgTid As Date, Godkjent As Boolean, Fødselsnummer As String, AnsattID As Object)
            varTimeID = TimeID
            varDatoOgTid = DatoOgTid
            varGodkjent = Godkjent
            varFødselsnummer = Fødselsnummer
            If IsDBNull(AnsattID) Then
                varAnsattID = -1
            Else
                varAnsattID = DirectCast(AnsattID, Integer)
            End If
        End Sub
    End Class
End Class