Option Strict On
Option Explicit On
Option Infer Off

Public Class Form1
    Dim WithEvents Cal As CustomCalendar
    Dim TestForm1, TestForm2 As FlatForm
    Dim TestQuestionnaire As Questionnaire
    Private Sub Setup_Tjener_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Cal = New CustomCalendar(Me, 14, 14, 80, 80, 14, 14)
        InitializeForms()
    End Sub
    Private Sub InitializeForms()
        TestForm1 = New FlatForm(400, 300, 10)
        With TestForm1
            .Parent = Me
            .AddField(FormElementType.CheckBox, 120)
            .AddField(FormElementType.Label)
            .AddField(FormElementType.CheckBox, 120)
            .AddField(FormElementType.CheckBox, 160)
            .AddField(FormElementType.CheckBox)
            .AddField(FormElementType.TextField)
            .AddField(FormElementType.Radio, 120, True)
            .AddField(FormElementType.Radio)
            .AddRadioContext()
            .AddField(FormElementType.Radio, 120, True)
            .AddField(FormElementType.Radio)
            .Field(1, 2).Extrude(FieldExtrudeSide.Left, 10)
            .Field(0, 1).Value = "Dette er et eksempel på både vertikal og horisontal sammensmelting." & vbNewLine & " "
            .Field(1, 2).SecondaryValue = "Test"
            .MergeWithAbove(1, 1)
            .MergeWithAbove(1, 2, -1)
            With .Field(2, 0)
                .Value = "Hei"
                .Header.Text = "Har du noen ekstra kommentarer?"
            End With
            '.HeightToContent()
        End With


        TestForm2 = New FlatForm(400, 300, 10)
        With TestForm2
            .Parent = Me
            .AddField(FormElementType.CheckBox, 120)
            .AddField(FormElementType.Label)
            .AddField(FormElementType.CheckBox, 120)
            .AddField(FormElementType.CheckBox, 160)
            .AddField(FormElementType.CheckBox)
            .AddField(FormElementType.TextField)
            .AddField(FormElementType.Radio, 120, True)
            .AddField(FormElementType.Radio)
            .AddRadioContext()
            .AddField(FormElementType.Radio, 120, True)
            .AddField(FormElementType.Radio)
            .Field(1, 2).Extrude(FieldExtrudeSide.Left, 10)
            .Field(0, 1).Value = "Dette er et annet eksempel på både vertikal og horisontal sammensmelting." & vbNewLine & " "
            .Field(1, 2).SecondaryValue = "Test"
            .MergeWithAbove(1, 1)
            .MergeWithAbove(1, 2, -1)
            With .Field(2, 0)
                .Value = "Hei"
                .Header.Text = "Har du noen ekstra kommentarer?"
            End With
            '.HeightToContent()
        End With

        TestQuestionnaire = New Questionnaire(Me)
        With TestQuestionnaire
            .Width = 500
            .Height = 400
            .Add(TestForm1)
            .Add(TestForm2)
            .Display()
        End With
    End Sub
    Private Sub CalMouseEnter(Sender As CustomCalendar.CalendarDay) Handles Cal.MouseEnter
        Select Case Sender.Area
            Case CustomCalendar.CalendarArea.CurrentMonth
                Sender.BackColor = ColorHelper.Add(Sender.BackColor, 50)
        End Select
        Debug.Print(Sender.Day.ToShortDateString)
    End Sub
    Private Sub CalMouseLeave(Sender As CustomCalendar.CalendarDay) Handles Cal.MouseLeave
        Select Case Sender.Area
            Case CustomCalendar.CalendarArea.CurrentMonth
                Sender.BackColor = ColorHelper.Add(Sender.BackColor, -50)
        End Select
    End Sub
    Private Sub CalClick(Sender As CustomCalendar.CalendarDay) Handles Cal.Click
        Select Case Sender.Area
            Case CustomCalendar.CalendarArea.CurrentMonth
                Sender.BackColor = Color.Aqua
        End Select
        Dim Shad As New BoxShadow(Sender, Color.FromArgb(0, 50, 60))
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Cal.Dispose()
        TestForm1.Dispose()
    End Sub
    Private Sub Me_Resize() Handles MyBase.Resize
        If TestQuestionnaire IsNot Nothing Then
            With TestQuestionnaire
                .Left = CInt((Me.ClientRectangle.Width / 2) - (.Width / 2))
                .Top = CInt((Me.ClientRectangle.Height / 2) - (.Height / 2))
            End With
        End If
    End Sub
End Class
