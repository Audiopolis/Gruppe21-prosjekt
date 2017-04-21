Imports AudiopoLib
Imports System.Threading

Public NotInheritable Class UserNotificationContainer
    Inherits BorderControl
    Private SC As SynchronizationContext = SynchronizationContext.Current
    Private WithEvents CloseTimer, TopTimer As New Timers.Timer(16)
    Private NotificationList As New List(Of UserNotification)
    Private WithEvents NotificationCounter As New Label
    Private LoadingSurface As New PictureBox
    Private LG As LoadingGraphics(Of PictureBox)
    Private Header As FullWidthControl
    Private EmptyLabel As New Label
    Private varDisplayEmptyLabel As Boolean
    Private ClosingNotifications As New List(Of UserNotification)

    Private Sub CloseTimer_Tick() Handles CloseTimer.Elapsed
        SC.Post(AddressOf AdjustWidth, Nothing)
    End Sub
    Private Sub TopTimer_Tick() Handles TopTimer.Elapsed
        SC.Post(AddressOf AdjustTop, Nothing)
    End Sub
    Private Sub RemoveAt(Index As Integer)
        With NotificationList
            .RemoveAt(Index)
            NotificationCounter.Text = CStr(.Count)
        End With
    End Sub
    Private Sub AdjustTop(State As Object)
        SuspendLayout()
        Dim iLast As Integer = NotificationList.Count - 1
        Dim ProblemFound As Boolean
        With NotificationList(0)
            If .Top >= Header.Bottom + 30 Then
                ProblemFound = True
                .Top -= 10
            Else
                .Top = Header.Bottom + 20
            End If
        End With
        If iLast > 0 Then
            For i As Integer = 1 To iLast
                With NotificationList(i)
                    If .Top >= NotificationList(i - 1).Bottom + 30 Then
                        .Top -= 10
                        ProblemFound = True
                    Else
                        .Top = NotificationList(i - 1).Bottom + 20
                    End If
                End With
            Next
        End If
        ResumeLayout(True)
        If ProblemFound Then
            TopTimer.Start()
        End If
    End Sub
    Private Sub AdjustWidth(State As Object)
        If ClosingNotifications.Count > 0 Then
            Dim iLast As Integer = ClosingNotifications.Count - 1
            For i As Integer = iLast To 0 Step -1
                Dim N As UserNotification = ClosingNotifications(i)
                With N
                    If .Width > 40 Then
                        .Width -= 40
                    Else
                        .IsClosed = True
                        ClosingNotifications.RemoveAt(i)
                    End If
                End With
            Next
            Dim nLast As Integer = NotificationList.Count - 1
            Dim StartTimer As Boolean
            For n As Integer = nLast To 0 Step -1
                Dim CurrentNotification As UserNotification = NotificationList(n)
                With CurrentNotification
                    If .IsClosed Then
                        RemoveAt(n)
                        .Dispose()
                        StartTimer = True
                    End If
                End With
            Next
            If ClosingNotifications.Count > 0 Then
                CloseTimer.Start()
                End If
            If NotificationList.Count > 0 AndAlso StartTimer Then
                TopTimer.Start()
            End If
        End If
    End Sub
    Public Sub Spin()
        LG.Spin(30, 10)
    End Sub
    Public Property DisplayEmptyLabel As Boolean
        Get
            Return varDisplayEmptyLabel
        End Get
        Set(value As Boolean)
            varDisplayEmptyLabel = value
        End Set
    End Property
    Private Sub CounterChanged() Handles NotificationCounter.TextChanged
        If CInt(NotificationCounter.Text) > 0 Then
            NotificationCounter.BackColor = Color.FromArgb(0, 99, 157)
            EmptyLabel.Hide()
        Else
            NotificationCounter.BackColor = Color.FromArgb(180, 180, 180)
            If varDisplayEmptyLabel Then
                EmptyLabel.Show()
            End If
        End If
    End Sub
    Public Sub New(BorderColor As Color)
        MyBase.New(BorderColor)
        Size = New Size(500, 500)
        Header = New FullWidthControl(Me)
        With Header
            .Width -= 2
            .Location = New Point(1, 1)
            .BackColor = Color.FromArgb(220, 220, 220)
            .ForeColor = Color.FromArgb(60, 60, 60)
            .Text = "Notifikasjoner og gjøremål"
        End With
        With NotificationCounter
            .Parent = Header
            .Size = New Size(Header.Height, Header.Height)
            .BackColor = Color.FromArgb(180, 180, 180)
            .ForeColor = Color.White
            .Font = New Font(.Font.FontFamily, 15)
            .TextAlign = ContentAlignment.MiddleCenter
            .Text = "0"
        End With
        With EmptyLabel
            .Hide()
            .Parent = Me
            .AutoSize = False
            .Size = New Size(100, 40)
            .BackColor = Color.White
            .ForeColor = Color.FromArgb(80, 80, 80)
            .Text = "Ingenting å vise"
            .TextAlign = ContentAlignment.MiddleCenter
            .Location = New Point(Width \ 2 - .Width \ 2, (Height - .Height + Header.Bottom) \ 2)
        End With
        With LoadingSurface
            .Hide()
            .Size = New Size(50, 50)
            .Parent = Me
            .Location = New Point(Width \ 2 - .Width \ 2, (Height - .Height + Header.Bottom) \ 2)
        End With
        CloseTimer.AutoReset = False
        TopTimer.AutoReset = False
        LG = New LoadingGraphics(Of PictureBox)(LoadingSurface)
        With LG
            .Stroke = 3
            .Pen.Color = Color.FromArgb(230, 50, 80)
        End With
        BackColor = Color.White
    End Sub
    Public Sub AddNotification(Text As String, ID As Object, ClickAction As Action(Of Object, UserNotificationEventArgs), Color As Color)
        Dim NewNotification As New UserNotification(Me, Text, ID, ClickAction, Color)
        With NotificationList
            'If .Count > 0 Then
            '    NewNotification.Top = .Last.Bottom + 20
            'Else
            '    NewNotification.Top = Header.Bottom + 20
            'End If
            .Insert(0, NewNotification)
            NotificationList(0).Top = Header.Bottom + 20
            Dim iLast As Integer = .Count - 1
            If iLast > 0 Then
                For i As Integer = 1 To iLast
                    NotificationList(i).Top = NotificationList(i - 1).Bottom + 20
                Next
            End If
            NotificationCounter.Text = CStr(.Count)
        End With
        varDisplayEmptyLabel = True
        LG.StopSpin()
    End Sub
    Protected Friend Sub CloseNotification(Sender As UserNotification)
        With NotificationList
            Dim iLast As Integer = .Count - 1
            Dim MatchFound As Boolean
            For i As Integer = 0 To iLast
                If NotificationList(i) Is Sender Then
                    ClosingNotifications.Add(NotificationList(i))
                    MatchFound = True
                    Exit For
                End If
            Next
            If MatchFound Then
                CloseTimer.Start()
            End If
        End With
    End Sub
    Public Sub RemoveNotification(ID As Object)
        With NotificationList
            Dim iLast As Integer = .Count - 1
            Dim MatchFound As Boolean
            For i As Integer = 0 To iLast
                If NotificationList(i).ID.Equals(ID) Then
                    .RemoveAt(i)
                    MatchFound = True
                    Exit For
                End If
            Next
            If MatchFound AndAlso .Count > 0 Then
                iLast = .Count - 1
                NotificationList(0).Top = Header.Bottom + 20
                If iLast > 0 Then
                    For i As Integer = 1 To iLast
                        NotificationList(i).Top = NotificationList(i - 1).Bottom + 20
                    Next
                End If
            Else
                MsgBox("match not found")
            End If
            NotificationCounter.Text = CStr(.Count)
        End With
    End Sub
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing Then
                LG.Dispose()
                TopTimer.Dispose()
                CloseTimer.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub
End Class
Public Class UserNotification
    Inherits Control
    Private WithEvents Textlab As New Label
    Private varDefaultColor, varHoverColor, varPressColor As Color
    '   Private varColor As Color = Color.Green
    Private varID As Object
    Private varIsClosed As Boolean
    Private varClickAction As Action(Of Object, UserNotificationEventArgs)
    Private Sub TextLab_MouseDown(Sender As Object, e As MouseEventArgs) Handles Textlab.MouseDown
        OnMouseDown(e)
    End Sub
    Private Sub TextLab_MouseUp(Sender As Object, e As MouseEventArgs) Handles Textlab.MouseUp
        OnMouseUp(e)
    End Sub
    Public Property IsClosed As Boolean
        Get
            Return varIsClosed
        End Get
        Set(value As Boolean)
            varIsClosed = value
            If value Then
                Hide()
            End If
        End Set
    End Property
    Public Shadows Property Parent As UserNotificationContainer
        Get
            Return DirectCast(MyBase.Parent, UserNotificationContainer)
        End Get
        Set(value As UserNotificationContainer)
            MyBase.Parent = value
        End Set
    End Property
    Public Property ID As Object
        Get
            Return varID
        End Get
        Set(value As Object)
            varID = value
        End Set
    End Property
    Public Sub New(ParentContainer As UserNotificationContainer, Message As String, ID As Object, ClickAction As Action(Of Object, UserNotificationEventArgs), Color As Color)
        Hide()
        DoubleBuffered = True
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        UpdateStyles()
        MyBase.Parent = ParentContainer
        varDefaultColor = Color
        varHoverColor = ColorHelper.Multiply(varDefaultColor, 0.9)
        varPressColor = ColorHelper.Multiply(varDefaultColor, 0.8)
        varClickAction = ClickAction
        varID = ID
        Size = New Size(Parent.Width - 40, 60)
        With Textlab
            .Parent = Me
            .ForeColor = Color.White
            .Font = New Font(Font.FontFamily, 10)
            .TextAlign = ContentAlignment.MiddleCenter
            .AutoSize = True
            .MaximumSize = New Size(Width - 20, Height)
            .Text = Message
            .BackColor = Color.Transparent
        End With
        Left = 20
        BackColor = varDefaultColor
        Show()
    End Sub
    Private Sub TextLab_TextChanged() Handles Textlab.SizeChanged, Textlab.TextChanged
        With Textlab
            .Location = New Point(Width \ 2 - .Width \ 2, Height \ 2 - .Height \ 2)
        End With
    End Sub
    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        BackColor = varDefaultColor
        If varClickAction IsNot Nothing Then
            varClickAction.Invoke(Me, New UserNotificationEventArgs(varID))
        End If
        Parent.CloseNotification(Me)
    End Sub
    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        BackColor = varPressColor
    End Sub
    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        BackColor = varHoverColor
    End Sub
    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        If Not Textlab.ClientRectangle.Contains(Textlab.PointToClient(MousePosition)) Then
            BackColor = varDefaultColor
        End If
    End Sub
End Class

Public Class UserNotificationEventArgs
    Inherits EventArgs
    Private varID As Object
    Public ReadOnly Property ID As Object
        Get
            Return varID
        End Get
    End Property
    Public Sub New(ID As Object)
        varID = ID
    End Sub
End Class

Public Class MessageNotification
    Inherits Control
    Private SC As SynchronizationContext = SynchronizationContext.Current
    Private WithEvents JumpTimer As New Timers.Timer(16)
    Private varMessageCount As Integer = -1
    Private varLabelIn As Boolean

    Private Const NumberOfFramesIn As Integer = 20
    Private Const NumberOfFramesJump As Integer = 13
    Private CurrentFrame As Integer
    Private LoadingSurface As New PictureBox
    Private LG As LoadingGraphics(Of PictureBox)
    Private WithEvents ActualLabel As New PictureBox

    Public Sub Start()
        JumpTimer.Start()
    End Sub
    ' Function in: 4(x-0.75x^2)
    ' Function jump: 1+x-x^2
    Private Sub JumpTimer_Tick() Handles JumpTimer.Elapsed
        SC.Post(AddressOf AdjustHeight, Nothing)
    End Sub
    Private Sub AdjustHeight(State As Object)
        If Not varLabelIn Then
            CurrentFrame += 1
            Dim NewCurrentFrame As Double = CurrentFrame / NumberOfFramesIn
            Dim Multiplier As Double = 4 * (NewCurrentFrame - 0.75 * NewCurrentFrame * NewCurrentFrame)
            With ActualLabel
                .Top = Height - CInt(Multiplier * .Height)
            End With
            If CurrentFrame = NumberOfFramesIn Then
                CurrentFrame = 0
                varLabelIn = True

                LG.Spin(30, 10)
                LoadingSurface.SendToBack()
                Height += 10
                Top -= 5

                JumpTimer.Interval = 10000
            End If
        Else
            CurrentFrame += 1
            Dim NewCurrentFrame As Double = CurrentFrame / NumberOfFramesJump
            Dim Multiplier As Double = 1 + 0.8 * (NewCurrentFrame - NewCurrentFrame * NewCurrentFrame)
            With ActualLabel
                .Top = Height - CInt(Multiplier * .Height)
            End With
            If CurrentFrame = NumberOfFramesJump Then
                CurrentFrame = 0
                JumpTimer.Interval = 10000
            ElseIf CurrentFrame = 1 Then
                JumpTimer.Interval = 16
            End If
        End If
        JumpTimer.Start()
    End Sub
    Public Property MessageCount As Integer
        Get
            Return varMessageCount
        End Get
        Set(value As Integer)
            varMessageCount = value
            ActualLabel.Text = CStr(value)
        End Set
    End Property
    Public Sub New(ParentControl As Control)
        Parent = ParentControl
        DoubleBuffered = True
        Dim Bmp As Bitmap = My.Resources.Meldingsboks
        Size = New Size(Bmp.Width + 20, 40)
        Top = (Parent.Height - Height) \ 2
        With ActualLabel
            .AutoSize = False
            .BackgroundImage = Bmp
            .BackgroundImageLayout = ImageLayout.None
            .Size = Bmp.Size
            .Left = (Width - .Width) \ 2
            '.TextAlign = ContentAlignment.MiddleCenter
            .Text = " 1"
            '.Padding = Padding.Empty
            .Parent = Me
            .Top = Height
        End With
        With JumpTimer
            .AutoReset = False
        End With
        With LoadingSurface
            .Hide()
            .Parent = ActualLabel
            .Size = New Size(ActualLabel.Width, ActualLabel.Width \ 4)
            .Top = Height - .Height
            .BackColor = Color.Transparent
        End With
        LG = New LoadingGraphics(Of PictureBox)(LoadingSurface)
        LG.Stroke = 3
        LoadingSurface.SendToBack()
    End Sub
    Private Sub OnLabelResize() Handles ActualLabel.Resize
        LoadingSurface.Top = ActualLabel.Height - LoadingSurface.Height
    End Sub
End Class