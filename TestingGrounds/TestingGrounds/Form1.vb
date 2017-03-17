Option Strict On
Option Explicit On
Option Infer Off

Public Class Form1
    Dim WithEvents Cal As CustomCalendar
    Dim Test As FlatForm
    Private Sub Setup_Tjener_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Cal = New CustomCalendar(Me, 14, 14, 80, 80, 14, 14)
        Test = New FlatForm(400, 10)
        With Test
            .Parent = Me
            .Height = 1000
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
        End With
        ' Legg til ExtrudeBottom, Right og Top
        Test.Field(1, 2).Extrude(FieldExtrudeSide.Left, 10)
        With Test.Field(0, 1)
            '.Extrude(FieldExtrudeSide.Bottom, 10)
            .Value = "Dette er et eksempel på både vertikal og horisontal sammensmelting."
        End With
        With Test.Field(1, 2)
            '.SwitchHeader(False)
            .SecondaryValue = "Test"
        End With
        Test.MergeWithAbove(1, 1)
        Test.MergeWithAbove(1, 2, -1)
        With Test.Field(2, 0)
            .Value = "Hei"
            .Header.Text = "Har du noen ekstra kommentarer?"
        End With
        Debug.Print(Test.FieldCount & ", " & Test.RowCount)
        Test.HeightToContent()
        Test.Display()
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
        Dim Result()() As HeaderValuePair = Test.Result
        MsgBox(Result(2)(0).Value.ToString)
        Test.Dispose()
    End Sub
    Private Sub Me_Resize() Handles MyBase.Resize
        If Test IsNot Nothing Then
            With Test
                .Left = CInt((Me.ClientRectangle.Width / 2) - (Test.Width / 2))
                .Top = CInt((Me.ClientRectangle.Height / 2) - (Test.Height / 2))
            End With
        End If
    End Sub
End Class
