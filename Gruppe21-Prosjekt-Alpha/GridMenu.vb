Option Strict On
Option Explicit On
Option Infer Off
Imports System.Drawing.Drawing2D
Imports AudiopoLib

Public Enum GridSelectDirection
    Up
    Down
    Left
    Right
End Enum
Public Class GridMenu(Of T As {Control, New})
    Inherits Control
    Private CList As List(Of T)
    Private RowCount, ColumnCount As Integer
    Private varSelectedIndex As Integer = -1
    Private boolSelectNoneOnMouseLeave As Boolean = True
    Private boolDrawGradient As Boolean = False
    Private ZeroPoint As New Point(0, 0)
    Public Event SelectionChanged(Sender As T, Selected As Boolean, ItemIndex As Integer)
    Public Event ItemClicked(Sender As T, Itemindex As Integer)
    Public Property DrawGradient As Boolean
        Get
            Return boolDrawGradient
        End Get
        Set(value As Boolean)
            boolDrawGradient = value
            Invalidate()
        End Set
    End Property
    Public ReadOnly Property Count As Integer
        Get
            Return CList.Count
        End Get
    End Property
    Public Property SelectedIndex As Integer
        Get
            Return varSelectedIndex
        End Get
        Set(value As Integer)
            If value >= 0 Then
                If value >= CList.Count Then
                    value = CList.Count - 1
                End If
                Dim Item As T = CList(value)
                If value <> varSelectedIndex Then
                    If varSelectedIndex <> -1 Then
                        RaiseEvent SelectionChanged(CList(varSelectedIndex), False, DirectCast(CList(varSelectedIndex).Tag, Integer))
                    End If
                    varSelectedIndex = value
                    RaiseEvent SelectionChanged(Item, True, DirectCast(Item.Tag, Integer))
                End If
            Else
                If varSelectedIndex >= 0 AndAlso varSelectedIndex < CList.Count Then
                    RaiseEvent SelectionChanged(CList(varSelectedIndex), False, varSelectedIndex)
                    varSelectedIndex = -1
                End If
            End If
        End Set
    End Property
    Public Overloads ReadOnly Property Item(ByVal Index As Integer) As T
        Get
            Return CList(Index)
        End Get
    End Property
    Public Overloads ReadOnly Property Item(ByVal Column As Integer, ByVal Row As Integer) As T
        Get
            Return CList(Row * RowCount + Column)
        End Get
    End Property
    Public ReadOnly Property Items As List(Of T)
        Get
            Return CList
        End Get
    End Property
    Public Property ItemText As String()
        Get
            Dim iLast As Integer = CList.Count - 1
            Dim RetArr(iLast) As String
            For i As Integer = 0 To iLast
                RetArr(i) = CList(i).Text
            Next
            Return RetArr
        End Get
        Set(value As String())
            Dim iLast As Integer = value.Length - 1
            For i As Integer = 0 To iLast
                CList(i).Text = value(i)
            Next
        End Set
    End Property
    Public Sub New(ByVal Rows As Integer, ByVal Columns As Integer, Optional ByVal ItemWidth As Integer = 100, Optional ByVal ItemHeight As Integer = 80, Optional ByVal ItemSpacing As Integer = 10)
        Hide()
        With Me
            .Width = Columns * (ItemWidth + ItemSpacing) - ItemSpacing
            .Height = Rows * (ItemHeight + ItemSpacing) - ItemSpacing
        End With
        CList = New List(Of T)
        RowCount = Rows
        ColumnCount = Columns
        For c As Integer = 0 To Columns - 1
            For r As Integer = 0 To Rows - 1
                Dim NC As New T
                With NC
                    .Parent = Me
                    .Left = r * (ItemWidth + ItemSpacing)
                    .Top = c * (ItemHeight + ItemSpacing)
                    .Width = ItemWidth
                    .Height = ItemHeight
                    .BackColor = Color.Red
                    .Tag = c * RowCount + r
                    AddHandler .MouseEnter, AddressOf OnItemMouseEnter
                    AddHandler .GotFocus, AddressOf OnItemMouseEnter
                    AddHandler .MouseLeave, AddressOf OnItemMouseLeave
                    AddHandler .LostFocus, AddressOf OnItemMouseLeave
                    AddHandler .Click, AddressOf OnItemClick
                    AddHandler .Paint, AddressOf OnItemPaint
                End With
                CList.Add(NC)
            Next
        Next
    End Sub
    Private Sub OnItemPaint(Sender As Object, e As PaintEventArgs)
        If boolDrawGradient Then
            Dim SenderItem As T = DirectCast(Sender, T)
            Dim GradientBrush As New LinearGradientBrush(New Point(0, SenderItem.Height), ZeroPoint, Color.FromArgb(0, SenderItem.BackColor), Color.FromArgb(60, ColorHelper.Multiply(SenderItem.BackColor, 3)))
            Dim DrawRect As New Rectangle(ZeroPoint, SenderItem.Size)
            For i As Integer = 0 To CList.Count - 1
                DrawRect = New Rectangle(New Point(CList(i).Left, CList(i).Top), New Size(CList(i).Width, CList(i).Height))
                e.Graphics.FillRectangle(GradientBrush, DrawRect)
            Next
            GradientBrush.Dispose()
        End If
    End Sub
    Protected Overrides Sub OnParentChanged(e As EventArgs)
        If Parent IsNot Nothing Then
            RemoveHandler Parent.KeyDown, AddressOf OnParentKeyDown
        End If
        MyBase.OnParentChanged(e)
        If Parent IsNot Nothing Then
            AddHandler Parent.KeyDown, AddressOf OnParentKeyDown
        End If
    End Sub
    Private Sub OnParentKeyDown(Sender As Object, e As KeyEventArgs)
        Select Case e.KeyCode
            Case Keys.Space, Keys.Enter
                OnItemClick(CList(varSelectedIndex), e)
        End Select
    End Sub
    Public Sub SelectDirection(ByVal Direction As GridSelectDirection)
        Select Case Direction
            Case GridSelectDirection.Down
                If varSelectedIndex + ColumnCount < CList.Count Then
                    SelectedIndex += ColumnCount
                End If
            Case GridSelectDirection.Up
                If varSelectedIndex - ColumnCount >= 0 Then
                    SelectedIndex -= ColumnCount
                End If
            Case GridSelectDirection.Left
                If Not (varSelectedIndex + ColumnCount) Mod ColumnCount = 0 Then
                    SelectedIndex -= 1
                End If
            Case GridSelectDirection.Right
                If Not (varSelectedIndex + 1) Mod ColumnCount = 0 Then
                    SelectedIndex += 1
                ElseIf varSelectedIndex = -1 Then
                    SelectedIndex = 0
                End If
        End Select
    End Sub
    Public Sub Display()
        Show()
    End Sub
    Private Sub OnItemMouseEnter(Sender As Object, e As EventArgs)
        Dim SenderItem As T = DirectCast(Sender, T)
        Dim SenderTag As Integer = DirectCast(SenderItem.Tag, Integer)
        SelectedIndex = SenderTag
    End Sub
    Private Sub OnItemMouseLeave(Sender As Object, e As EventArgs)
        If boolSelectNoneOnMouseLeave Then
            SelectedIndex = -1
        End If
    End Sub
    Private Sub OnItemClick(Sender As Object, e As EventArgs)
        Dim SenderItem As T = DirectCast(Sender, T)
        Dim SenderTag As Integer = DirectCast(SenderItem.Tag, Integer)
        RaiseEvent ItemClicked(SenderItem, SenderTag)
    End Sub

End Class
