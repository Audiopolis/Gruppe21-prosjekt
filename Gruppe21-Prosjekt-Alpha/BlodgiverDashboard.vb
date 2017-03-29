'Option Strict On
'Option Explicit On
'Option Infer Off
'Imports System.ComponentModel
'Imports AudiopoLib

'Public Class BlodgiverDashboard
'    Dim WithEvents GM As GridMenu(Of Label)
'    Dim LayoutHelper As FormLayoutTools
'    Private IsLoaded As Boolean = False
'    Public Sub New()
'        ' This call is required by the designer.
'        InitializeComponent()
'        MyBase.OnLoad(New EventArgs)
'        ' Add any initialization after the InitializeComponent() call.
'    End Sub
'    Private Sub BlodgiverDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
'        If Not IsLoaded Then
'            LayoutHelper = New FormLayoutTools(Me)
'            GM = New GridMenu(Of Label)(3, 3, 200, 100, 20)
'            GM.Parent = Me
'            With Me
'                .KeyPreview = True
'            End With
'            Dim iLast As Integer = GM.Count - 1
'            Dim TextArr() As String = {"Timeoversikt", "Forny egenerklæring", "Min profil", "Hjelp", "Test5", "Test6", "Test7", "Test8", "Test9"}
'            For i As Integer = 0 To iLast
'                With GM.Item(i)
'                    .BackColor = Color.FromArgb(0, 80, 110)
'                    .TextAlign = ContentAlignment.MiddleCenter
'                    .ForeColor = Color.White
'                    .Font = New Font(.Font.FontFamily, 12)
'                    .Text = TextArr(i)
'                End With
'            Next
'            LayoutHelper.CenterOnForm(GM)
'            GM.Display()
'            GM.DrawGradient = True
'            IsLoaded = True
'        End If
'    End Sub
'    Protected Overrides Sub OnClosing(e As CancelEventArgs)
'        Dim Result As MsgBoxResult = MsgBox("Vil du logge ut?", MsgBoxStyle.YesNo, "Er du sikker?")
'        If Result = MsgBoxResult.Yes Then
'            If Testspørreskjema IsNot Nothing Then
'                Testspørreskjema.Hide()
'            End If
'            Hide()
'            Testlogginn.Show()
'        End If
'        e.Cancel = True
'        MyBase.OnClosing(e)
'    End Sub
'    Protected Overrides Sub OnResize(e As EventArgs)
'        If LayoutHelper IsNot Nothing Then
'            LayoutHelper.CenterOnForm(GM)
'        End If
'    End Sub
'    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
'        Select Case keyData
'            Case Keys.Up
'                GM.SelectDirection(GridSelectDirection.Up)
'                Return True
'            Case Keys.Right
'                GM.SelectDirection(GridSelectDirection.Right)
'                Return True
'            Case Keys.Down
'                GM.SelectDirection(GridSelectDirection.Down)
'                Return True
'            Case Keys.Left
'                GM.SelectDirection(GridSelectDirection.Left)
'                Return True
'        End Select
'        Return MyBase.ProcessCmdKey(msg, keyData)
'    End Function
'    Private Sub OnGridSelectionChanged(Sender As Label, Selected As Boolean, ItemIndex As Integer) Handles GM.SelectionChanged
'        Select Case Selected
'            Case True
'                Sender.BackColor = ColorHelper.Multiply(Color.FromArgb(0, 80, 110), 1.33)
'            Case Else
'                Sender.BackColor = Color.FromArgb(0, 80, 110)
'        End Select
'    End Sub
'    Private Sub OnGridClick(Sender As Label, ItemIndex As Integer) Handles GM.ItemClicked
'        Select Case ItemIndex
'            Case 0
'                Testoversikt.Show()
'                Hide()
'            Case 1
'                Testspørreskjema.Show()
'                Hide()
'        End Select
'    End Sub
'End Class