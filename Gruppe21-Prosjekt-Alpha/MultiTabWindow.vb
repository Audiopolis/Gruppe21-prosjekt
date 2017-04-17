Option Strict On
Option Explicit On
Option Infer Off

Public NotInheritable Class MultiTabWindow
    Inherits Panel
#Region "Fields"
    Private TabList As List(Of Tab)
    Private Shared ZeroPoint As New Point(0, 0)
    Private SelectedIndex As Integer = 0
    Private ScaleXY() As Boolean = {True, False}
#End Region
#Region "Events"
    Public Event TabChanged(Sender As MultiTabWindow)
    Public Event TabCountChanged(Sender As MultiTabWindow)
    Public Event TabResized(Sender As MultiTabWindow, Tab As Tab)
#End Region
#Region "Public properties"
    Public Overloads Property ScaleTabsToSize As Boolean()
        Get
            Return ScaleXY
        End Get
        Set(value As Boolean())
            ScaleXY(0) = value(0)
            ScaleXY(1) = value(1)
        End Set
    End Property
    Public Enum TabSize
        Horizontal
        Vertical
    End Enum
    Public Overloads Property ScaleTabsToSize(ByVal Component As TabSize) As Boolean
        Get
            Select Case Component
                Case TabSize.Horizontal
                    Return ScaleXY(0)
                Case Else
                    Return ScaleXY(1)
            End Select
        End Get
        Set(value As Boolean)
            Select Case Component
                Case TabSize.Horizontal
                    ScaleXY(0) = value
                Case Else
                    ScaleXY(1) = value
            End Select
        End Set
    End Property
    Public ReadOnly Property Tabs As List(Of Tab)
        Get
            Return TabList
        End Get
    End Property
    Public Property Index As Integer
        Get
            Return SelectedIndex
        End Get
        Set(value As Integer)
            If SelectedIndex >= 0 AndAlso SelectedIndex < TabList.Count Then
                TabList(SelectedIndex).Hide()
            End If
            If value >= 0 AndAlso value < TabList.Count Then
                SelectedIndex = value
                With TabList(SelectedIndex)
                    .Show()
                    .BringToFront()
                End With
            Else
                MsgBox("2: " & value)
                SelectedIndex = -1
            End If
        End Set
    End Property
    Private Enum WindowProperty
        Tab
        ScaleTabs
        TabCount
    End Enum
    'Private Sub OnTabResize()
    '    Dim RaiseTabResizedEvent As Boolean
    '    Dim NewTabSize() As Integer
    '    With CurrentTab
    '        NewTabSize = { .Width, .Height}
    '        If NewTabSize(0) <> Width Then
    '            If ScaleXY(0) Then
    '                NewTabSize(0) = Width
    '                RaiseTabResizedEvent = True
    '            ElseIf .Width > Width Then
    '                HScroll = True
    '            Else
    '                HScroll = False
    '            End If
    '        End If
    '        If NewTabSize(1) <> Height Then
    '            If ScaleXY(1) Then
    '                NewTabSize(1) = Height
    '                RaiseTabResizedEvent = True
    '            ElseIf NewTabSize(1) > Height Then
    '                VScroll = True
    '            Else
    '                VScroll = False
    '            End If
    '        End If
    '        .Size = New Size(NewTabSize(0), NewTabSize(1))
    '    End With
    'End Sub
    Public ReadOnly Property CurrentTab As Tab
        Get
            If SelectedIndex >= 0 AndAlso SelectedIndex < TabList.Count Then
                Return TabList(SelectedIndex)
            Else
                Return Nothing
            End If
        End Get
    End Property
#End Region
#Region "Public methods"
    Public Sub New(Parent As Control)
        TabList = New List(Of Tab)
        With Me
            .Hide()
            .DoubleBuffered = True
            .Parent = Parent
            .Location = ZeroPoint
            .ClientSize = Parent.ClientSize
            .Show()
        End With
        AddHandler Parent.Resize, AddressOf OnParentResize
    End Sub
    Public Overloads Sub AddTab(Tab As Tab)
        AddTab(Tab, TabList.Count)
    End Sub
    Public Overloads Sub AddTab(Controls As List(Of Control))
        Dim iLast As Integer = Controls.Count - 1
        Dim NT As New Tab(Me)
        With NT
            For i As Integer = 0 To iLast
                .Controls.Add(Controls(i))
            Next
            .ListIndex = TabList.Count
        End With
        AddTab(NT)
    End Sub
    Public Overloads Sub AddTab(Controls() As Control)
        Dim iLast As Integer = Controls.Count - 1
        Dim NT As New Tab(Me)
        With NT
            For i As Integer = 0 To iLast
                .Controls.Add(Controls(i))
            Next
            .ListIndex = TabList.Count
        End With
        AddTab(NT)
    End Sub
    Public Overloads Sub AddTab(Tab As Tab, AtIndex As Integer)
        With Tab
            .Hide()
            .Location = ZeroPoint
            .Size = ClientSize
            .ListIndex = AtIndex
        End With
        Dim iLast As Integer
        With TabList
            .Insert(AtIndex, Tab)
            iLast = .Count - 1
        End With
        If AtIndex < iLast Then
            For i As Integer = AtIndex + 1 To iLast
                TabList(i).ListIndex = i
            Next
        End If
    End Sub
    Public Overloads Sub RemoveTab(Index As Integer)
        If Index = SelectedIndex Then
            Me.Index -= 1
        End If
        TabList.RemoveAt(Index)
        Dim iLast As Integer = TabList.Count - 1
        If iLast >= Index Then
            For i As Integer = Index To TabList.Count - 1
                TabList(i).ListIndex = i
            Next
        End If
    End Sub
    Public Sub ShowTab(ByVal Index As Integer)
        Me.Index = Index
    End Sub
#End Region
#Region "Private methods"
    Private Sub OnParentResize(Sender As Object, e As EventArgs)
        With Me
            .SuspendLayout()
            .ClientSize = Parent.ClientSize
            If .CurrentTab IsNot Nothing Then
                .CurrentTab.RefreshLayout()
            End If
            .ResumeLayout()
        End With
    End Sub
    Protected Overrides Sub OnParentChanged(e As EventArgs)
        Try
            MyBase.OnParentChanged(e)
            With Me
                .Location = ZeroPoint
                .ClientSize = .Parent.ClientSize
            End With
            If CurrentTab IsNot Nothing Then
                CurrentTab.RefreshLayout()
            End If
        Catch
            MsgBox("Feil")
        End Try
    End Sub
#End Region
End Class

Public Class Tab
    Inherits Control
    Private Shared ZeroPoint As New Point(0, 0)
    Private varScaleToParent As Boolean = True
    Protected Friend ListIndex As Integer = -1
    Public Event LayoutRefreshed(Sender As Tab)

    Public Property ScaleToWindow As Boolean
        Get
            Return varScaleToParent
        End Get
        Set(value As Boolean)
            varScaleToParent = value
        End Set
    End Property
    Public Overridable Sub RefreshLayout()
        If varScaleToParent Then
            If Parent IsNot Nothing Then
                ClientSize = Parent.ClientSize
            Else
                MsgBox("No Parent")
            End If
        End If
        RaiseEvent LayoutRefreshed(Me)
    End Sub
    Protected Friend Overridable Shadows Sub Show()
        RefreshLayout()
        MyBase.Show()
    End Sub
    Public Shadows Property Parent As MultiTabWindow
        Get
            Return DirectCast(MyBase.Parent, MultiTabWindow)
        End Get
        Set(value As MultiTabWindow)
            If Parent IsNot Nothing AndAlso ListIndex >= 0 Then
                Parent.RemoveTab(ListIndex)
            End If
            MyBase.Parent = value
            If Parent IsNot Nothing Then
                Parent.AddTab(Me)
            End If
        End Set
    End Property
    Public Sub New(Parent As MultiTabWindow)
        Hide()
        DoubleBuffered = True
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        UpdateStyles()
        Location = ZeroPoint
        Me.Parent = Parent
    End Sub
    Public Sub Close(Optional Dispose As Boolean = True)
        OnClosing(New TabClosingEventArgs)
    End Sub
    Protected Overridable Sub OnClosing(e As TabClosingEventArgs)
        If Not e.Cancel Then
            OnClosed(e)
        End If
    End Sub
    Protected Overridable Sub OnClosed(e As TabClosingEventArgs)
        If e.Dispose Then
            Dispose()
        End If
    End Sub
    Protected Overrides Sub OnParentChanged(e As EventArgs)
        MyBase.OnParentChanged(e)
        ClientSize = Parent.ClientSize
    End Sub
    'Protected Class LayoutEventArgs
    '    Inherits EventArgs
    '    Public LayoutHelper As FormLayoutTools = Nothing
    '    Public Sub New(ByRef FormLayoutTool As FormLayoutTools)
    '        LayoutHelper = FormLayoutTool
    '    End Sub
    'End Class
    Protected Overrides Sub Dispose(disposing As Boolean)
        MyBase.Dispose(disposing)
    End Sub
End Class
Public Class TabClosingEventArgs
    Inherits EventArgs
    Private DoCancel As Boolean
    Private DisposeOnClose As Boolean
    Public Property Cancel As Boolean
        Get
            Return DoCancel
        End Get
        Set(value As Boolean)
            DoCancel = value
        End Set
    End Property
    Public Property Dispose As Boolean
        Get
            Return DisposeOnClose
        End Get
        Set(value As Boolean)
            DisposeOnClose = value
        End Set
    End Property
    Public Sub New(Optional ByVal Cancel As Boolean = False, Optional ByVal Dispose As Boolean = True)
        DoCancel = Cancel
        DisposeOnClose = Dispose
    End Sub
End Class