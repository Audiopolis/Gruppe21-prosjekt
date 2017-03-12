Public Class Form1
    Dim WithEvents Cal As CustomCalendar
    Private Sub Setup_Tjener_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Cal = New CustomCalendar(Me, 14, 14, 80, 80, 14, 14)
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
        Cal.Dispose()
    End Sub
End Class
