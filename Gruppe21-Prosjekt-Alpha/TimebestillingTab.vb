Option Strict On
Option Explicit On
Option Infer Off
Imports System.Text
Imports AudiopoLib

Public Class TimebestillingTab
    Inherits Tab
    Private WelcomeLabel As New InfoLabel(True, Direction.Right)
    Private RightForm, TimeForm As New BorderControl(Color.Red)
    Private WithEvents TopBar As New TopBar(Me)
    Private Footer As New Footer(Me)
    Private WithEvents BestillTimeKnapp, AvbestillTimeKnapp As TopBarButton
    Private WithEvents Calendar As CustomCalendar
    Private varSelectedDay As Date
    Private HeaderLab, TimeHeaderLab, TimeLab, AktuellTimeLab, AvbestillInfoLab As New Label
    Private TimeInfo As New PictureBox
    Private Tabell As New Timetabell
    Private WithEvents DBC, DBC_GetRules As New DatabaseClient(Credentials.Server, Credentials.Database, Credentials.UserID, Credentials.Password)
    Private Sub TopBar_Click(Sender As TopBarButton, e As EventArgs) Handles TopBar.ButtonClick
        If Sender.IsLogout Then
            Parent.Index = 4
            Logout()
        Else
            Parent.Index = 5
        End If
    End Sub
    Public Sub SetAppointment()
        Dim iLast As Integer = TimeListe.Count - 1
        Dim Dates(iLast) As Date
        Dim TimeToday As DatabaseTimeElement = Nothing
        For i As Integer = 0 To iLast
            With TimeListe.Time(i).DatoOgTid
                Dates(i) = .Date
                If .Date = Date.Now.Date Then
                    TimeToday = TimeListe.Time(i)
                End If
            End With
        Next
        Calendar.ApplyCustomStyle(Dates, 0)


        ' TODO: Uncomment later
        'If TimeToday IsNot Nothing Then
        '    HentTimer_DBC.SQLQuery = "SELECT * FROM Egenerklæring WHERE time_id = @id;"
        '    HentTimer_DBC.Execute({"@id"}, {CStr(TimeToday.TimeID)})
        'End If
    End Sub
    Private Sub NameSet() Handles TopBar.NameSet
        With WelcomeLabel
            .Text = "Du er logget inn som " & CurrentLogin.FullName
            .PanIn() 'TODO: Add optional method to skip the panning
        End With
    End Sub
    Public Sub New(ParentWindow As MultiTabWindow)
        MyBase.New(ParentWindow)
        Dim PrevStyle As New CustomCalendar.CalendarDayStyle(Color.FromArgb(160, 160, 160), Color.FromArgb(195, 195, 195), Color.FromArgb(160, 160, 160))
        Dim CurrentStyle As New CustomCalendar.CalendarDayStyle(Color.White, Color.FromArgb(162, 25, 51), Color.FromArgb(225, 111, 111))
        Dim NextStyle As New CustomCalendar.CalendarDayStyle(Color.FromArgb(160, 160, 160), Color.FromArgb(195, 195, 195), Color.FromArgb(160, 160, 160))
        Calendar = New CustomCalendar(PrevStyle, CurrentStyle, NextStyle, 0, 0, 80, 80, 5, 5,,,, New String() {"Januar", "Februar", "Mars", "April", "Mai", "Juni", "Juli", "August", "September", "Oktober", "November", "Desember"})
        With Calendar
            .SetDayNames(New String() {"Søndag", "Mandag", "Tirsdag", "Onsdag", "Torsdag", "Fredag", "Lørdag"})
            .Parent = Me
            .Location = New Point(20, (Height + TopBar.Height - Footer.Height) \ 2 - 20)
            .DrawGradient = False
            .AddCustomStyle(0, New CustomCalendar.CalendarDayStyle(Color.White, Color.FromArgb(238, 62, 95), Color.White))
            .Display()
            .ArrowColorDefault = ColorHelper.Multiply(Color.FromArgb(107, 21, 37), 2)
            .ArrowColorHover = Color.FromArgb(107, 21, 37)
            '.Hide()
        End With
        With TimeForm
            .Parent = Me
            .Size = New Size(400, 420)
            .BackColor = Color.FromArgb(245, 245, 245)
            .Location = New Point(Calendar.Right + 20, Calendar.Top + 80)
            .MakeDashed(Color.FromArgb(220, 220, 220), RightForm.BackColor)
            .Hide()
        End With
        With RightForm
            .Parent = Me
            .Size = New Size(400, 420)
            .BackColor = Color.FromArgb(245, 245, 245)
            .Location = New Point(Calendar.Right + 20, Calendar.Top + 80)
            .MakeDashed(Color.FromArgb(220, 220, 220), RightForm.BackColor)
        End With
        With HeaderLab
            .Parent = RightForm
            .AutoSize = False
            .Size = New Size(RightForm.Width - 40, 30)
            .Location = New Point(20, 20)
            .BackColor = RightForm.BackColor
            .ForeColor = Color.FromArgb(60, 60, 60)
            .Text = "Når på dagen passer det best for deg?"
            .Font = New Font(.Font.FontFamily, 10)
        End With
        With TimeHeaderLab
            .Parent = TimeForm
            .AutoSize = False
            .Size = New Size(RightForm.Width - 40, 30)
            .Location = New Point(20, 20)
            .BackColor = RightForm.BackColor
            .ForeColor = Color.FromArgb(60, 60, 60)
            .Text = "Du har en time på den valgte dagen."
            .Font = New Font(.Font.FontFamily, 12)
        End With
        With TimeLab
            .Parent = RightForm
            .AutoSize = False
            .Size = New Size(RightForm.Width - 40, 40)
            .Location = New Point(20, HeaderLab.Bottom + 15)
            .BackColor = RightForm.BackColor
            .ForeColor = Color.FromArgb(60, 60, 60)
            .Text = "PS: Vi kan ikke garantere at vi har kapasitet på det ønskede tidspunktet. Les derfor nøye gjennom innkallingen før du godkjenner timen."
        End With
        With AktuellTimeLab
            .Parent = TimeForm
            .AutoSize = False
            .Size = New Size(RightForm.Width - 40, 40)
            .Location = New Point(20, HeaderLab.Bottom + 15)
            .BackColor = RightForm.BackColor
            .ForeColor = Color.FromArgb(60, 60, 60)
            .Text = "Tidspunkt:"
            .Font = New Font(.Font.FontFamily, 10)
        End With
        With Tabell
            .Parent = RightForm
            .Build()
            .AddSpecialDayRules(Date.Now, New Timetabell.DayStateSeries({0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}))
            .Location = New Point(20, TimeLab.Bottom + 20)
        End With
        AvbestillTimeKnapp = New TopBarButton(TimeForm, My.Resources.AvbrytIcon, "Kanseller denne timen", New Size(136, 36))
        BestillTimeKnapp = New TopBarButton(RightForm, My.Resources.OKIconHvit, "Send timeforespørsel", New Size(136, 36),, AvbestillTimeKnapp.Width)
        With BestillTimeKnapp
            .BackColor = Color.LimeGreen
            .ForeColor = Color.White
            .Location = New Point(20, RightForm.Height - .Height - 20)
        End With
        With AvbestillTimeKnapp
            .Location = New Point(20, TimeForm.Height - .Height - 20)
            .BackColor = Color.FromArgb(162, 25, 51)
            .ForeColor = Color.White
        End With
        With AvbestillInfoLab
            .Parent = TimeForm
            .AutoSize = False
            .Location = New Point(AvbestillTimeKnapp.Right + 10, AvbestillTimeKnapp.Top)
            .Size = New Size(TimeForm.Width - .Left - 10, 32)
            .TextAlign = ContentAlignment.MiddleLeft
            .Text = "Elektronisk kansellering må skje minst 24 timer i forveien."
            .Font = New Font(.Font.FontFamily, 8)
            .ForeColor = Color.FromArgb(70, 70, 70)
        End With
        With TimeInfo
            .Parent = RightForm
            .BackgroundImage = My.Resources.TimeTabellInfo
            .Size = New Size(.BackgroundImage.Width, .BackgroundImage.Height)
            .Left = Tabell.Right + 15
            .Top = Tabell.Top + Tabell.Height \ 2 - .Height \ 2
        End With
        With TopBar
            .AddButton(My.Resources.HjemIcon, "Hjem", New Size(136, 36))
            .AddLogout("Logg ut", New Size(136, 36))
        End With
        With WelcomeLabel
            .ForeColor = Color.White
            .Parent = TopBar
            .Top = TopBar.Height \ 2 - .Height \ 2
            .Text = "Du er logget inn som..."
            .Height = TopBar.LogoutButton.Height - 3
        End With
        DBC.SQLQuery = "INSERT INTO Time (t_dato, t_klokkeslett, b_fodselsnr) VALUES (@dato, @tid, @nr);"
        DBC_GetRules.SQLQuery = "SELECT Serie FROM Ukeregler;"
        DBC_GetRules.Execute()
    End Sub
    Private Sub BestillClick() Handles BestillTimeKnapp.Click
        DBC.Execute({"@dato", "@tid", "@nr"}, {varSelectedDay.ToString("yyyy-MM-dd"), Tabell.SelectedTime.ToString("HH:mm"), CurrentLogin.PersonalNumber})
    End Sub
    Private Sub DBC_GetRules_Finished(sender As Object, e As DatabaseListEventArgs) Handles DBC_GetRules.ListLoaded
        With e
            If Not .ErrorOccurred Then
                Dim TestSeries(6) As Timetabell.DayStateSeries
                With .Data
                    For i As Integer = 0 To 6
                        Dim CharArr() As Char = DirectCast(.Rows(i).Item(0), String).ToCharArray
                        Dim IntArr(24) As Integer
                        For n As Integer = 0 To 24
                            IntArr(n) = CInt(Char.GetNumericValue(CharArr(n)))
                        Next
                        TestSeries(i) = New Timetabell.DayStateSeries(IntArr)
                    Next
                End With
                Tabell.Rules = New Timetabell.WeekRules(TestSeries(0), TestSeries(1), TestSeries(2), TestSeries(3), TestSeries(4), TestSeries(5), TestSeries(6))
            Else
                MsgBox(.ErrorMessage)
            End If
        End With
    End Sub
    Private Sub DBC_Finished(sender As Object, e As DatabaseListEventArgs) Handles DBC.ListLoaded
        If e.ErrorOccurred Then
            MsgBox("Error: " & e.ErrorMessage)
        Else
            MsgBox("Ja")
        End If
    End Sub
    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        If Calendar IsNot Nothing Then
            Dim AlignRect As New Rectangle()
            With AlignRect
                .Width = Calendar.Width + RightForm.Width + 20
                .Height = Calendar.Height
                .X = Width \ 2 - .Width \ 2
                .Y = (Height - .Height + TopBar.Height - Footer.Height) \ 2 - 20
            End With
            With Calendar
                .Location = New Point(AlignRect.Left, AlignRect.Top)
            End With
            With RightForm
                .Location = New Point(AlignRect.Right - .Width, Calendar.Top + 80)
            End With
            With TimeForm
                .Location = New Point(AlignRect.Right - .Width, Calendar.Top + 80)
            End With
            With WelcomeLabel
                .Location = New Point(Width - 430, TopBar.LogoutButton.Top)
                .Size = New Size(300, TopBar.LogoutButton.Height - 3)
            End With
        End If
    End Sub
    Private Sub DBC_Failed() Handles DBC.ExecutionFailed
        MsgBox("Failed")
    End Sub
    Private Sub DayEnter(sender As CustomCalendar.CalendarDay) Handles Calendar.MouseEnter
        With sender
            .BackColor = ColorHelper.Add(.BackColor, 20)
        End With
    End Sub
    Private Sub DayLeave(sender As CustomCalendar.CalendarDay) Handles Calendar.MouseLeave
        With sender
            .BackColor = ColorHelper.Add(.BackColor, -20)
        End With
    End Sub
    Private Sub DayClick(sender As CustomCalendar.CalendarDay) Handles Calendar.Click
        Dim PreviouslySelected As CustomCalendar.CalendarDay = Calendar.Day(varSelectedDay)
        If PreviouslySelected IsNot Nothing Then
            PreviouslySelected.SetColors(PreviouslySelected.LastStyleApplied)
        End If
        With sender
            .BackColor = Color.FromArgb(132, 20, 41)
            varSelectedDay = .Day
        End With
        Tabell.CurrentDate = varSelectedDay
        Dim iLast As Integer = TimeListe.Count - 1
        If iLast >= 0 Then
            Dim MatchFound As Boolean
            Dim MatchAt As Integer
            For i As Integer = 0 To iLast
                If TimeListe.Time(i).DatoOgTid.Date = varSelectedDay.Date Then
                    MatchFound = True
                    MatchAt = i
                    Exit For
                End If
            Next
            If MatchFound Then
                With TimeListe.Time(MatchAt).DatoOgTid
                    AktuellTimeLab.Text = "Dato: " & vbTab & vbTab & .ToString("d/M/yyyy") & vbNewLine & "Klokkeslett: " & .ToString("HH:mm") & vbNewLine & vbNewLine & "Lorem ipsum"
                End With
                RightForm.Hide()
                TimeForm.Show()
            Else
                TimeForm.Hide()
                RightForm.Show()
            End If
        Else
            TimeForm.Hide()
            RightForm.Show()
        End If
    End Sub
End Class

Public Class Timetabell
    Inherits Control
    Private Timeliste As New List(Of TimeElement)
    Private varCurrentDate As Date = Date.Now
    Private varRuleSet As WeekRules = Nothing
    Private varSpecialRules As New List(Of SpecialDayRule)
    Private varSelected As TimeElement = Nothing
#Region "Properties"
    Public ReadOnly Property SelectedTime As Date
        Get
            Return varSelected.Tid
        End Get
    End Property
    Public Property CurrentDate As Date
        Get
            Return varCurrentDate
        End Get
        Set(value As Date)
            varCurrentDate = value
            RefreshStates()
        End Set
    End Property
    Public Property Rules As WeekRules
        Get
            Return varRuleSet
        End Get
        Set(RuleSet As WeekRules)
            varRuleSet = RuleSet
            RefreshStates()
        End Set
    End Property
#End Region
#Region "Public methods"
    Public Sub New()
        Height = 200
        Width = 250
    End Sub
    Public Sub AddSpecialDayRules(SpecialDate As Date, SpecialSeries As DayStateSeries)
        varSpecialRules.Add(New SpecialDayRule(SpecialDate, SpecialSeries))
        RefreshStates()
    End Sub
    Public Sub Build()
        Dim Tid As Date = Date.Now.Date
        Tid = Tid.AddHours(7)
        For i As Integer = 0 To 24
            Tid = Tid.AddMinutes(30)
            Dim T As New TimeElement(Tid, Me)
            With T
                If Timeliste.Count > 0 Then
                    Dim LastRight As Integer = Timeliste.Last.Right
                    If LastRight + 50 <= Width Then
                        .Left = Timeliste.Last.Right + 3
                        .Top = Timeliste.Last.Top
                    Else
                        .Left = 0
                        .Top = Timeliste.Last.Top + 33
                    End If
                End If
            End With
            Timeliste.Add(T)
        Next
        Width = Timeliste.Last.Right
        Height = Timeliste.Last.Bottom
    End Sub
    Protected Sub OnTimeElementClick(Sender As TimeElement)
        RefreshStates()
        varSelected = Sender
        MsgBox(varSelected.Tid.ToString("HH:mm"))
        varSelected.BackColor = Color.LimeGreen
        varSelected.ForeColor = Color.White
    End Sub
#End Region
#Region "Private methods"
    Private Sub RefreshStates()
        If varCurrentDate.Date.CompareTo(Date.Now.Date) > 0 Then
            Dim Match As SpecialDayRule = varSpecialRules.Find(Function(Rule As SpecialDayRule)
                                                                   With Rule.SpecialDate
                                                                       If .Day = varCurrentDate.Day AndAlso .Month = varCurrentDate.Month AndAlso .Year = varCurrentDate.Year Then
                                                                           Return True
                                                                       Else
                                                                           Return False
                                                                       End If
                                                                   End With
                                                               End Function)
            If Match Is Nothing Then
                If varRuleSet IsNot Nothing Then
                    Dim Series As DayState() = varRuleSet.Rule(varCurrentDate.DayOfWeek).Series
                    For i As Integer = 0 To 24
                        Timeliste(i).SetState(Series(i))
                    Next
                Else
                    For i As Integer = 0 To 24
                        Timeliste(i).SetState(DayState.Enabled)
                    Next
                End If
            Else
                Dim Series As DayState() = Match.Series.Series
                For i As Integer = 0 To 24
                    Timeliste(i).SetState(Series(i))
                Next
            End If
        Else
            For i As Integer = 0 To 24
                Timeliste(i).SetState(DayState.Disabled)
            Next
        End If
    End Sub
#End Region
    Protected Class TimeElement
        Inherits Control
        Private varTid As Date
        Private varState As DayState
        Private WithEvents TidLabel As New Label
        Public Shadows Property ForeColor As Color
            Get
                Return TidLabel.ForeColor
            End Get
            Set(value As Color)
                TidLabel.ForeColor = value
            End Set
        End Property
        Public Shadows Property BackColor As Color
            Get
                Return TidLabel.BackColor
            End Get
            Set(value As Color)
                TidLabel.BackColor = value
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
        Protected Overrides Sub OnMouseEnter(e As EventArgs)
            If varState = DayState.Enabled Then
                MyBase.OnMouseEnter(e)
                With TidLabel
                    .BackColor = ColorHelper.Add(.BackColor, -15)
                End With
            End If
        End Sub
        Protected Overrides Sub OnMouseLeave(e As EventArgs)
            If varState = DayState.Enabled Then
                MyBase.OnMouseLeave(e)
                With TidLabel
                    .BackColor = ColorHelper.Add(.BackColor, 15)
                End With
            End If
        End Sub
        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            If varState = DayState.Enabled Then
                MyBase.OnMouseDown(e)
                With TidLabel
                    .BackColor = ColorHelper.Add(.BackColor, -20)
                End With
            End If
        End Sub
        Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
            If varState = DayState.Enabled Then
                MyBase.OnMouseUp(e)
                With TidLabel
                    .BackColor = ColorHelper.Add(.BackColor, 20)
                End With
                Parent.OnTimeElementClick(Me)
            End If
        End Sub
        Private Sub LabelMouseDown(sender As Object, e As MouseEventArgs) Handles TidLabel.MouseDown
            OnMouseDown(e)
        End Sub
        Private Sub LabelMouseUp(sender As Object, e As MouseEventArgs) Handles TidLabel.MouseUp
            OnMouseUp(e)
        End Sub
        Private Sub LabelEnter(sender As Object, e As EventArgs) Handles TidLabel.MouseEnter
            OnMouseEnter(e)
        End Sub
        Private Sub LabelLeave(sender As Object, e As EventArgs) Handles TidLabel.MouseLeave
            OnMouseLeave(e)
        End Sub
        Public Sub SetState(ByVal State As DayState)
            varState = State
            With TidLabel
                Select Case State
                    Case DayState.Disabled
                        .BackColor = Color.FromArgb(230, 230, 230)
                        .ForeColor = Color.FromArgb(210, 210, 210)
                    Case DayState.Enabled
                        .BackColor = Color.White
                        .ForeColor = Color.FromArgb(60, 60, 60)
                End Select
            End With
        End Sub
        Public Shadows Property Parent As Timetabell
            Get
                Return DirectCast(MyBase.Parent, Timetabell)
            End Get
            Set(value As Timetabell)
                MyBase.Parent = value
            End Set
        End Property
        Public Property Tid As Date
            Get
                Return varTid
            End Get
            Set(value As Date)
                varTid = value
                TidLabel.Text = value.ToString("HH:mm")
            End Set
        End Property
        Protected Friend Sub New(ByVal Tid As Date, ParentTabell As Timetabell)
            Parent = ParentTabell
            Size = New Size(47, 30)
            MyBase.BackColor = Color.FromArgb(220, 220, 220)
            With TidLabel
                .Parent = Me
                .Location = New Point(1, 1)
                .Size = New Size(Width - 2, Height - 2)
                .TextAlign = ContentAlignment.MiddleCenter
                .Font = New Font(.Font.FontFamily, 7)
            End With
            Me.Tid = Tid
        End Sub
    End Class
    Public Enum DayState As Integer
        Disabled = 0
        Enabled = 1
        'Occupied = 2
    End Enum
    Public Class DayStateSeries
        Private varSeries(24) As DayState
        Public Property Series As DayState()
            Get
                Return varSeries
            End Get
            Set(value As DayState())
                For i As Integer = 0 To 24
                    varSeries(i) = value(i)
                Next
            End Set
        End Property
        Public Sub New(TwentyFiveStates() As DayState)
            varSeries = TwentyFiveStates
        End Sub
        Public Sub New(TwentyFiveStates() As Integer)
            For i As Integer = 0 To 24
                varSeries(i) = CType(TwentyFiveStates(i), DayState)
            Next
        End Sub
        ''' <summary>
        ''' Sunday: All disabled
        ''' </summary>
        Public Sub New()
            For i As Integer = 0 To 24
                varSeries(i) = 0
            Next
        End Sub
        Public Overrides Function ToString() As String
            Dim SB As New StringBuilder
            For i As Integer = 0 To 24
                SB.Append(varSeries(i))
            Next
            Return SB.ToString
        End Function
    End Class
    Public Class WeekRules
        Private AllSeries(6) As DayStateSeries
        Public Sub New(SundayRules As DayStateSeries, MondayRules As DayStateSeries, TuesdayRules As DayStateSeries, WednesdayRules As DayStateSeries, ThursdayRules As DayStateSeries, FridayRules As DayStateSeries, SatursdayRules As DayStateSeries)
            AllSeries(0) = SundayRules
            AllSeries(1) = MondayRules
            AllSeries(2) = TuesdayRules
            AllSeries(3) = WednesdayRules
            AllSeries(4) = ThursdayRules
            AllSeries(5) = FridayRules
            AllSeries(6) = SatursdayRules
        End Sub
        Public Property Rule(ByVal Day As DayOfWeek) As DayStateSeries
            Get
                Return AllSeries(Day)
            End Get
            Set(value As DayStateSeries)
                AllSeries(Day) = value
            End Set
        End Property
    End Class
    Private Class SpecialDayRule
        Private varDate As Date
        Private varSeries As DayStateSeries
        Public ReadOnly Property SpecialDate As Date
            Get
                Return varDate
            End Get
        End Property
        Public ReadOnly Property Series As DayStateSeries
            Get
                Return varSeries
            End Get
        End Property
        Public Sub New(ByVal SpecialDate As Date, SpecialSeries As DayStateSeries)
            varDate = SpecialDate
            varSeries = SpecialSeries
        End Sub
    End Class
End Class