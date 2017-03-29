Option Strict On
Option Explicit On
Option Infer Off
Imports System.ComponentModel
Imports AudiopoLib
Public Class Timeoversikt
    Dim WithEvents Cal As TestCalendar
    Dim Info As InfoTable
    Dim LayoutHelper As FormLayoutTools
    Private IsLoaded As Boolean
    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        MyBase.OnLoad(New EventArgs)
        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Protected Overrides Sub OnClosing(e As CancelEventArgs)
        Hide()
        If Testdashbord IsNot Nothing Then
            Testdashbord.Show()
            e.Cancel = True
        End If
        MyBase.OnClosing(e)
    End Sub
    Private Sub Timeoversikt_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not IsLoaded Then
            DoubleBuffered = True
            LayoutHelper = New FormLayoutTools(Me)
            Info = New InfoTable("Oversikt for den valgte dagen", "Her vil vi vise en oversikt over ledige timer, så snart nødvendig SQL er på plass.", 10)
            With Info
                .Parent = Me
            End With
            Cal = New TestCalendar(0, 0, 70, 70, 10, 10,,, New String() {"Mandag", "Tirsdag", "Onsdag", "Torsdag", "Fredag", "Lørdag", "Søndag"}, New String() {"Januar", "Februar", "Mars", "April", "Mai", "Juni", "Juli", "August", "September", "Oktober", "November", "Desember"})
            With Cal
                .Parent = Me
                .Display()
            End With
            LayoutHelper.CenterOnForm(Cal, -135, 20)
            LayoutHelper.CenterOnForm(Info, 305, 20)
            With Me
                .Width = 1000
                .Height = 700
                .WindowState = FormWindowState.Maximized
            End With
            IsLoaded = True
        End If
    End Sub
    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        If LayoutHelper IsNot Nothing Then
            LayoutHelper.CenterOnForm(Cal, -135, 20)
            LayoutHelper.CenterOnForm(Info, 305, 20)
        End If
    End Sub
    Private Sub CalEnter(Sender As TestCalendar.CalendarDay) Handles Cal.MouseEnter
        Sender.BackColor = ColorHelper.Add(Sender.BackColor, 20)
    End Sub
    Private Sub CalLeave(Sender As TestCalendar.CalendarDay) Handles Cal.MouseLeave
        Sender.BackColor = ColorHelper.Add(Sender.BackColor, -20)
    End Sub
End Class