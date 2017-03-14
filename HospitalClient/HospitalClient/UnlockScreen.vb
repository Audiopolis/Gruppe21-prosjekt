Imports AudiopoLib

Public Class UnlockScreen
    ' Gjør om til (Of Control)
    Dim LoadingGraphics As LoadingGraphics(Of PictureBox)
    Dim WithEvents LayoutTool As FormLayoutTools
    Dim CredManager As CredentialsManager
    Dim WithEvents FWButton As FullWidthControl
    Dim NotifManager As NotificationManager
    Private Sub UnlockScreen_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'CredManager = New CredentialsManager("C:\Users\Magnus\Desktop\Blodbank\")
        'CredManager.Decode("Heilol")
        Dim test1 As New EncryptedReadWrite("Heilol")
        test1.DecryptData(test1.EncryptData("Mittpassord123"))
        MsgBox("Test 1 done")
        Dim Test2 As New CredentialsManager
        Test2.TestEncoding("Hei lol", "kek")
        Test2.Decode("kek")


        LoadingGraphics = New LoadingGraphics(Of PictureBox)(PicLoadingSurface)
        LayoutTool = New FormLayoutTools(Me)
        NotifManager = New NotificationManager(Me)
        NotifManager.AssignedLayoutManager = LayoutTool
        With LoadingGraphics
            .Stroke = 2
            .Pen.Color = Color.LimeGreen
        End With
        With LayoutTool
            .IncludeFormTitle = True
            .CenterOnForm(GroupLoggInn)
            .CenterSurface(PicLoadingSurface, GroupLoggInn)
        End With
        FWButton = New FullWidthControl(GroupLoggInn, True, FullWidthControl.SnapType.Bottom)
        Dim GroupHeader As New FullWidthControl(GroupLoggInn, False, FullWidthControl.SnapType.Top)
        With GroupHeader
            .Height = 20
            .BackColor = Color.FromArgb(230, 230, 230)
            .ForeColor = Color.FromArgb(100, 100, 100)
            .TextAlign = ContentAlignment.MiddleLeft
            .Padding = New Padding(5, 0, 0, 0)
            .Text = "Logg inn"
        End With
    End Sub
    Public Sub IfValidAction()
        Debug.Print("Gyldig")
        FWButton.Enabled = True
        FWButton.Show()
        LoadingGraphics.StopSpin()
        Me.Hide()
        'Form1.Show()
    End Sub
    Public Sub IfInvalidAction()
        FWButton.Enabled = True
        FWButton.Show()
        LoadingGraphics.StopSpin()
        'LayoutTool.ReservedSpaceTop = NotificationDisplayer.Control.Height
        For Each C As Control In GroupLoggInn.Controls
            C.Show()
        Next
        NotifManager.Display("Brukernavnet eller passordet er ugyldig.", NotificationPreset.RedAlert, 5)
    End Sub
End Class
