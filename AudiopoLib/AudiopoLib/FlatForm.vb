Option Strict On
Option Explicit On
Option Infer Off
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

Public Class FormField
    Inherits Control
    Private WithEvents labHeader As Label
    Private varValue As Object = Nothing
    Private varSecondaryValue As Object = Nothing
    Private varSpacingBottom As Integer = 10
    Private varDrawGradient As Boolean = True
    Protected Friend varFieldType As FormElementType
    Protected Friend Event ValueChanged(Sender As FormField, ByVal Value As Object)
    Protected Friend Event SecondaryValueChanged(Sender As FormField, ByVal Value As Object)
    Protected Event HeaderVisibleChanged(ByVal Visible As Boolean)
    Protected Friend varIsInputType As Boolean = True
    Private varTopColor, varBottomColor As Color
    Protected MeType As FormElementType
    Protected Friend GradBrush As LinearGradientBrush
    Private DashedPen As Pen
    Private varDrawDash(3) As Boolean
    Public Enum ElementSide
        Left = 0
        Top = 1
        Right = 2
        Bottom = 3
    End Enum
    Private Sub OnHeaderTextAlignChanged() Handles labHeader.TextAlignChanged
        Select Case labHeader.TextAlign
            Case ContentAlignment.TopLeft
                labHeader.Padding = New Padding(6, 2, 0, 0)
            Case ContentAlignment.TopCenter
                labHeader.Padding = New Padding(0, 2, 0, 0)
            Case ContentAlignment.TopRight
                labHeader.Padding = New Padding(0, 2, 6, 0)
            Case ContentAlignment.MiddleLeft
                labHeader.Padding = New Padding(6, 0, 0, 0)
            Case ContentAlignment.MiddleCenter
                labHeader.Padding = New Padding(0, 0, 0, 0)
            Case ContentAlignment.MiddleRight
                labHeader.Padding = New Padding(0, 0, 6, 0)
            Case ContentAlignment.BottomLeft
                labHeader.Padding = New Padding(6, 0, 0, 2)
            Case ContentAlignment.BottomCenter
                labHeader.Padding = New Padding(0, 0, 0, 2)
            Case ContentAlignment.BottomRight
                labHeader.Padding = New Padding(0, 0, 6, 2)
        End Select
    End Sub
    Public Property DrawDashedSepararators(ByVal Side As ElementSide) As Boolean
        Get
            Return varDrawDash(Side)
        End Get
        Set(value As Boolean)
            varDrawDash(Side) = value
            Invalidate()
        End Set
    End Property
    Public ReadOnly Property IsInputType As Boolean
        Get
            Return varIsInputType
        End Get
    End Property
    Public Property DrawGradient As Boolean
        Get
            Return varDrawGradient
        End Get
        Set(value As Boolean)
            varDrawGradient = value
            Invalidate()
        End Set
    End Property
    Protected Friend Property TopColor As Color
        Get
            Return varTopColor
        End Get
        Set(value As Color)
            varTopColor = value
            If GradBrush IsNot Nothing Then
                GradBrush.Dispose()
            End If
            GradBrush = New LinearGradientBrush(Point.Empty, New Point(0, Bottom), varTopColor, varBottomColor)
            Invalidate()
        End Set
    End Property
    Protected Friend Property BottomColor As Color
        Get
            Return varBottomColor
        End Get
        Set(value As Color)
            varBottomColor = value
            If GradBrush IsNot Nothing Then
                GradBrush.Dispose()
            End If
            GradBrush = New LinearGradientBrush(Point.Empty, New Point(0, Bottom), varTopColor, varBottomColor)
            Invalidate()
        End Set
    End Property
    ' TODO: Override in all derived classes for which pressing enter og space should have an effect.
    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        MyBase.OnKeyDown(e)
        If e.KeyCode = Keys.Space OrElse e.KeyCode = Keys.Enter Then
            OnUserClick()
        End If
    End Sub
    Protected Overridable Sub OnUserClick()

    End Sub
    Protected Overridable Sub OnValueChanged(ByVal Value As Object)
        RaiseEvent ValueChanged(Me, Value)
    End Sub
    Protected Overridable Sub OnSecondaryValueChanged(ByVal Value As Object)
        RaiseEvent SecondaryValueChanged(Me, Value)
    End Sub
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        With e.Graphics
            .SmoothingMode = SmoothingMode.AntiAlias
            '.CompositingQuality = CompositingQuality.HighQuality
            If varDrawGradient Then
                .FillRectangle(GradBrush, ClientRectangle)
            End If
            'If varDrawDash(0) Then

            If varDrawDash(0) Then
                .DrawLine(DashedPen, Point.Empty, New Point(0, Height))
            End If
            If varDrawDash(1) Then
                .DrawLine(DashedPen, Point.Empty, New Point(Width, 0))
            End If
            If varDrawDash(2) Then
                .DrawLine(DashedPen, New Point(Width, 0), New Point(Width, Height))
            End If
            If varDrawDash(3) Then
                .DrawLine(DashedPen, New Point(0, Height), New Point(Width, Height))
            End If

            'End If
            MyBase.OnPaint(e)
        End With
    End Sub
    'Protected Overrides Sub OnGotFocus(e As EventArgs)
    '    MyBase.OnGotFocus(e)
    '    If Not MeType = FormElementType.TextField Then
    '        BackColor = ColorHelper.Multiply(BackColor, 0.4)
    '        DrawGradient = False
    '    End If
    'End Sub
    'Protected Overrides Sub OnLostFocus(e As EventArgs)
    '    MyBase.OnLostFocus(e)
    '    If Not MeType = FormElementType.TextField Then
    '        BackColor = ColorHelper.Multiply(BackColor, 2.5)
    '        DrawGradient = True
    '    End If
    'End Sub
    Public Sub New(ByVal FieldSpacing As Integer, ByVal FieldType As FormElementType)
        DoubleBuffered = True
        Using NH As New HatchBrush(HatchStyle.DottedGrid, ColorHelper.Add(Color.FromArgb(5, 53, 68), 20))
            DashedPen = New Pen(NH)
        End Using
        varFieldType = FieldType
        varSpacingBottom = FieldSpacing
        SetStyle(ControlStyles.Selectable, False)
        labHeader = New Label
        With labHeader
            .TabStop = False
            .Location = Point.Empty
            .Width = Width
            .Height = 17
            .Font = New Font(.Font.FontFamily, 8)
            .BackColor = Color.FromArgb(5, 53, 68)
            .TextAlign = ContentAlignment.MiddleLeft
            .ForeColor = Color.White
            .Padding = New Padding(6, 0, 0, 0)
            .Parent = Me
            AddHandler .Paint, AddressOf PaintLines
        End With
        BackColor = Color.FromArgb(7, 70, 91)
        varTopColor = Color.FromArgb(50, ColorHelper.Multiply(BackColor, 1.5))
        varBottomColor = Color.FromArgb(50, BackColor)
    End Sub
    Private Sub PaintLines(Sender As Object, e As PaintEventArgs)
        Dim LabSize As Size = labHeader.ClientSize
        With e.Graphics
            If varDrawDash(0) Then
                .DrawLine(DashedPen, Point.Empty, New Point(0, LabSize.Height))
            End If
            If varDrawDash(1) Then
                .DrawLine(DashedPen, Point.Empty, New Point(LabSize.Width, 0))
            End If
            If varDrawDash(2) Then
                .DrawLine(DashedPen, New Point(LabSize.Width, 0), New Point(LabSize.Width, LabSize.Height))
            End If
        End With
    End Sub
    Protected Overrides Sub OnSizeChanged(e As EventArgs)
        MyBase.OnSizeChanged(e)
        labHeader.Width = Width
        If GradBrush IsNot Nothing Then
            GradBrush.Dispose()
        End If
        GradBrush = New LinearGradientBrush(Point.Empty, New Point(0, Bottom), TopColor, BottomColor)
    End Sub
    Protected Overrides Sub OnVisibleChanged(e As EventArgs)
        MyBase.OnVisibleChanged(e)
        labHeader.Width = Width
    End Sub
    Public ReadOnly Property Header As Label
        Get
            Return labHeader
        End Get
    End Property
    Public Overridable Property Value As Object
        Get
            Return varValue
        End Get
        Set(value As Object)
            varValue = value
            OnValueChanged(value)
        End Set
    End Property
    Public Overridable Property SecondaryValue As Object
        Get
            Return varSecondaryValue
        End Get
        Set(value As Object)
            varSecondaryValue = value
            OnSecondaryValueChanged(value)
        End Set
    End Property
    Public Overridable Overloads Sub SwitchHeader(ByVal SwitchOn As Boolean)
        With Header
            If SwitchOn Then
                .Height = 17
                .Show()
            Else
                .Height = 0
                .Hide()
            End If
        End With
        RaiseEvent HeaderVisibleChanged(SwitchOn)
    End Sub
    Public Sub Extrude(ByVal Side As FieldExtrudeSide, ByVal Amount As Integer)
        SuspendLayout()
        Select Case Side
            Case FieldExtrudeSide.Left
                Width += Amount
                Left -= Amount
            Case FieldExtrudeSide.Right
                Width += Amount
            Case FieldExtrudeSide.Bottom
                Height += Amount
        End Select
        ResumeLayout()
    End Sub
    Protected Overrides Sub Dispose(disposing As Boolean)
        With labHeader
            .Dispose()
        End With
        If GradBrush IsNot Nothing Then
            GradBrush.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub
End Class
Public Enum FieldExtrudeSide
    Left
    Right
    Bottom
End Enum
Public Enum FormElementType
    CheckBox
    TextField
    Button
    Label
    DropDown
    Radio
End Enum
Public Class FormElement
    Private E As FormElementType
    Private W As Integer
    Private V As Object
    Public Sub New(ByVal Type As FormElementType, Optional ByVal Width As Integer = -1, Optional ByVal Value As Object = Nothing)
        E = Type
        W = Width
        V = Value
    End Sub
    Public Property Width As Integer
        Get
            Return W
        End Get
        Set(value As Integer)
            W = value
        End Set
    End Property
    Public Property Type As FormElementType
        Get
            Return E
        End Get
        Set(value As FormElementType)
            E = value
        End Set
    End Property
    Public Property Value As Object
        Get
            Return V
        End Get
        Set(value As Object)
            V = value
        End Set
    End Property
End Class
Public Class HeaderValuePair
    Public Header As String = "Not set"
    Public Value As Object = False
    Public Type As FormElementType
    Public Sub New(ByVal HeaderString As String, ByVal ValueObject As Object, ByVal ElementType As FormElementType)
        Header = HeaderString
        Value = ValueObject
        Type = ElementType
    End Sub
End Class
Public Class FlatFormResult
    Private ResultArr()() As HeaderValuePair
    Public Function GetAllSeries() As HeaderValuePair()
        Dim Counter As Integer
        For i As Integer = 0 To ResultArr.GetLength(0) - 1
            For n As Integer = 0 To ResultArr.GetLength(1) - 1
                Counter += 1
            Next
        Next
        Dim Ret(Counter) As HeaderValuePair
        Dim CounterX As Integer
        For ix As Integer = 0 To ResultArr.GetLength(0) - 1
            For nx As Integer = 0 To ResultArr.GetLength(1) - 1
                CounterX += 1
                Ret(CounterX) = ResultArr(ix)(nx)
            Next
        Next
        Return Ret
    End Function
    Protected Friend Sub New(Result As HeaderValuePair()())
        ResultArr = Result
    End Sub

    Public Function Header(ByVal Row As Integer, ByVal Field As Integer) As String
        Return ResultArr(Row)(Field).Header
    End Function
    Public Function Value(ByVal Row As Integer, ByVal Field As Integer) As Object
        Return ResultArr(Row)(Field).Value
    End Function
    Public Function FieldType(ByVal Row As Integer, ByVal Field As Integer) As FormElementType
        Return ResultArr(Row)(Field).Type
    End Function
End Class
Public Class FlatForm
#Region "FlatForm"
    Inherits Control
    Private varFieldSpacing As Integer
    Private RadioContextList As List(Of RadioButtonContext)
    Private RowList As List(Of FormRow)
    Private varRowHeight As Integer = 57
    Public ReadOnly Property Last As FormField
        Get
            Dim Ret As FormField
            With RowList
                With .Item(.Count - 1).Fields
                    If .Count > 0 Then
                        Ret = .Item(.Count - 1)
                    Else
                        Return Nothing
                    End If
                End With
            End With
            Return Ret
        End Get
    End Property
    Public Property NewRowHeight As Integer
        Get
            Return varRowHeight
        End Get
        Set(value As Integer)
            varRowHeight = value
        End Set
    End Property
    Public ReadOnly Property Row(ByVal Index As Integer) As FormRow
        Get
            Return RowList(Index)
        End Get
    End Property
    Public ReadOnly Property LastRow As FormRow
        Get
            With RowList
                Return .Item(.Count - 1)
            End With
        End Get
    End Property
    Public ReadOnly Property Rows As List(Of FormRow)
        Get
            Return RowList
        End Get
    End Property
    Public ReadOnly Property Result() As FlatFormResult
        Get
            Dim iLast As Integer = RowList.Count - 1
            Dim RetArr(iLast)() As HeaderValuePair
            For i As Integer = 0 To iLast
                Dim ThisRow As FormRow = RowList(i)
                Dim nLast As Integer = RowList(i).Fields.Count - 1
                Dim PairArr(nLast) As HeaderValuePair
                For n As Integer = 0 To nLast
                    Dim Field As FormField = ThisRow.Fields(n)
                    PairArr(n) = New HeaderValuePair(Field.Header.Text, Field.Value, Field.varFieldType)
                Next
                RetArr(i) = PairArr
            Next
            Return New FlatFormResult(RetArr)
        End Get
    End Property
    Public ReadOnly Property FieldCount As Integer
        Get
            Dim Sum As Integer = 0
            If RowList.Count > 0 Then
                For i As Integer = 0 To RowList.Count - 1
                    Sum += RowList(i).Controls.Count
                Next
            End If
            Return Sum
        End Get
    End Property
    Public ReadOnly Property RowCount As Integer
        Get
            Return RowList.Count
        End Get
    End Property
    Public ReadOnly Property Field(ByVal Row As Integer, ByVal Index As Integer) As FormField
        Get
            Return RowList(Row).Fields(Index)
        End Get
    End Property
    Public Sub New(ByVal Width As Integer, ByVal Height As Integer, ByVal FieldSpacing As Integer)
        RowList = New List(Of FormRow)
        RadioContextList = New List(Of RadioButtonContext)
        RadioContextList.Add(New RadioButtonContext)
        varFieldSpacing = FieldSpacing
        With Me
            .SuspendLayout()
            .Width = Width
            .Height = 300
            .ResumeLayout()
        End With
    End Sub
    'Public Sub HeightToContent()
    '    Height = RowList(RowList.Count - 1).Bottom
    'End Sub
    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        If RowList.Count > 0 Then
            Dim Testheight As Integer = RowList(RowList.Count - 1).Bottom - varFieldSpacing
            If Height <> Testheight Then
                Height = Testheight
            End If
        End If
    End Sub
    Protected Overrides Sub OnVisibleChanged(e As EventArgs)
        MyBase.OnVisibleChanged(e)
        If RowList.Count > 0 Then
            Dim Testheight As Integer = RowList(RowList.Count - 1).Bottom - varFieldSpacing
            If Height <> Testheight Then
                Height = Testheight
            End If
        End If
    End Sub
    Public Sub MergeWithAbove(ByVal Row As Integer, ByVal FieldIndex As Integer, Optional ByVal UpperFieldIndexOffset As Integer = 0, Optional ByVal RepositionLowerChildren As Boolean = False)
        Dim Upper, Lower As FormField
        With RowList
            Upper = .Item(Row - 1).Fields(FieldIndex + UpperFieldIndexOffset)
            Lower = .Item(Row).Fields(FieldIndex)
        End With
        With Lower
            If RepositionLowerChildren Then
                .SwitchHeader(False)
            Else
                .Header.Hide()
            End If
        End With
        With Upper
            .Extrude(FieldExtrudeSide.Bottom, varFieldSpacing)
            Dim TotalHeight As Integer = .Height + Lower.Height
            Dim Ratio As Double = .Height / TotalHeight
            .TopColor = Color.FromArgb(50, ColorHelper.Multiply(.BackColor, 2))
            .BottomColor = Color.FromArgb(50, ColorHelper.Mix(Color.FromArgb(0, .BackColor), .TopColor, Ratio))
        End With
        With Lower
            .TopColor = Upper.BottomColor
            .BottomColor = Color.FromArgb(0, .BackColor)
        End With
    End Sub
    Public Sub Display()
        With RowList
            Dim counter As Integer = 0
            Dim iLast As Integer = .Count - 1
            For i As Integer = 0 To iLast
                For Each F As FormField In .Item(i).Fields
                    If Not F.varFieldType = FormElementType.Label Then
                        F.TabIndex = counter
                        counter += 1
                    End If
                Next
                .Item(i).Display()
            Next
        End With
    End Sub
    Public Sub AddRow(Optional ByVal RowHeight As Integer = 40)
        Dim NewRow As New FormRow(varFieldSpacing)
        With NewRow
            .Hide()
            .Width = Width
            .Height = RowHeight + varFieldSpacing
            .Parent = Me
            .Left = 0
            If RowList.Count > 0 Then
                .Top = RowList(RowList.Count - 1).Bottom
            Else
                .Top = 0
            End If
        End With
        RowList.Add(NewRow)
    End Sub
    Public Sub SetTabIndex()
        Dim i As Integer = 0
        For Each R As FormRow In RowList
            For Each F As FormField In R.Fields
                F.TabIndex = i
                i += 1
            Next
        Next
    End Sub
    Public Sub AddRadioContext()
        Dim R As New RadioButtonContext
        RadioContextList.Add(R)
    End Sub
    Public Overloads Sub AddField(Elements() As FormElement)
        With Elements
            Dim iLast As Integer = .Length - 1
            For i As Integer = 0 To iLast
                With .ElementAt(i)
                    AddField(.Type, .Width, .Value)
                End With
            Next
        End With
    End Sub
    Public Overloads Sub AddField(ByVal FieldType As FormElementType, Optional ByVal FieldWidth As Integer = -1, Optional ByVal Value As Object = Nothing)
        Dim LastRow As FormRow
        With RowList
            If .Count > 0 Then
                LastRow = .Item(.Count - 1)
                If LastRow.CalculateTotalWidth + Math.Abs(FieldWidth) > LastRow.Width Then
                    Dim NewRow As New FormRow(varFieldSpacing)
                    With NewRow
                        .Hide()
                        .Width = Width
                        .Height = varRowHeight + varFieldSpacing
                        .Parent = Me
                        .Left = 0
                        .Top = LastRow.Bottom
                    End With
                    .Add(NewRow)
                End If
            Else
                Dim NewRow As New FormRow(varFieldSpacing)
                With NewRow
                    .Hide()
                    .Width = Width
                    .Height = varRowHeight + varFieldSpacing
                    .Parent = Me
                    .Left = 0
                    .Top = 0
                End With
                .Add(NewRow)
            End If
            'If .Count = 0 Then
            '    Dim NewRow As New FormRow(varFieldSpacing)
            '    With NewRow
            '        .Hide()
            '        .Parent = Me
            '        .Top = 0
            '        .Left = 0
            '        .Width = Width
            '        .Height = varRowHeight + varFieldSpacing
            '        '.BackColor = Color.FromArgb(7, 70, 91)
            '        .BackColor = Color.Green
            '    End With
            '    .Add(NewRow)
            'End If
            LastRow = .Item(.Count - 1)
            Dim NewControl As FormField
            Select Case FieldType
                Case FormElementType.CheckBox
                    NewControl = New FormCheckBox(varFieldSpacing)
                Case FormElementType.TextField
                    NewControl = New FormTextField(varFieldSpacing)
                    With NewControl
                        .BackColor = ColorHelper.Multiply(.BackColor, 0.4)
                        .DrawGradient = False
                    End With
                Case FormElementType.Button
                    NewControl = New FormButton(varFieldSpacing)
                Case FormElementType.DropDown
                    NewControl = New FormDropDown(varFieldSpacing)
                Case FormElementType.Label
                    NewControl = New FormLabel(varFieldSpacing)
                Case FormElementType.Radio
                    With RadioContextList
                        If .Count = 0 Then .Add(New RadioButtonContext)
                        With .Item(.Count - 1)
                            NewControl = New FormRadioButton(.Count, (varFieldSpacing))
                            .Add(DirectCast(NewControl, FormRadioButton))
                        End With
                    End With
                Case Else
                    NewControl = Nothing
            End Select
            'With NewControl
            '    .Height = LastRow.Height - varFieldSpacing
            'End With
            If Value IsNot Nothing Then
                NewControl.Value = Value
            End If
            With LastRow
                If FieldWidth < 0 Then
                    .AddField(NewControl, .CalculateRemainingWidth)
                Else
                    .AddField(NewControl, FieldWidth)
                End If
            End With
        End With
    End Sub
    Protected Overrides Sub Dispose(disposing As Boolean)
        With RadioContextList
            Dim nLast As Integer = .Count - 1
            For n As Integer = 0 To nLast
                .Item(n).Dispose()
            Next
        End With
        RadioContextList = Nothing
        With RowList
            Dim iLast As Integer = .Count - 1
            For i As Integer = 0 To iLast
                .Item(i).Dispose()
            Next
        End With
        RowList = Nothing
        MyBase.Dispose(disposing)
    End Sub
#End Region
    Public Class FormRow
        Inherits Control
        Private varFieldSpacing As Integer
        Private FieldList As List(Of FormField)
        Public ReadOnly Property Fields As List(Of FormField)
            Get
                Return FieldList
            End Get
        End Property
        Protected Friend ReadOnly Property FieldSpacing As Integer
            Get
                Return varFieldSpacing
            End Get
        End Property
        Public Sub RemoveGaps()
            SuspendLayout()
            With FieldList
                Dim Part As Integer = Width \ .Count
                For i As Integer = 0 To .Count - 1
                    With .Item(i)
                        .Width = Part
                        .Left = Part * i
                    End With
                Next
                With .Item(.Count - 1)
                    .Width = Width - .Left
                End With
            End With
            ResumeLayout()
        End Sub
        Protected Friend Sub Display()
            With Controls
                Dim iLast As Integer = .Count - 1
                For i As Integer = 0 To iLast
                    .Item(i).Show()
                Next
            End With
            Show()
        End Sub
        Protected Overrides Sub OnResize(e As EventArgs)
            MyBase.OnResize(e)
            With FieldList
                Dim iLast As Integer = .Count - 1
                If iLast >= 0 Then
                    For i As Integer = 0 To iLast
                        .Item(i).Height = Height - varFieldSpacing
                    Next
                End If
            End With
        End Sub
        Protected Friend Sub New(ByVal FieldSpacing As Integer)
            FieldList = New List(Of FormField)
            DoubleBuffered = True
            SetStyle(ControlStyles.Selectable, False)
            TabStop = False
            'GradSurface = New Control
            'With GradSurface
            '.Parent = Me
            '.Top = 0
            '.Left = 0
            '.SendToBack()
            'End With
            varFieldSpacing = FieldSpacing
            'GradBrush = New LinearGradientBrush(New Point(0, 0), New Point(Left, Bottom), ColorFirst, ColorSecond)
        End Sub
        Protected Friend Sub AddField(Item As FormField, ByVal FieldWidth As Integer)
            With FieldList
                If .Count > 0 Then
                    Item.Left = .Item(.Count - 1).Right + varFieldSpacing
                    'Else
                    '    Item.Left = 0
                End If
                With Item
                    '.Top = 0
                    .Height = Height - varFieldSpacing
                    .Width = FieldWidth
                End With
                Controls.Add(Item)
                .Add(Item)
            End With
        End Sub
        Protected Friend Function CalculateTotalWidth() As Integer
            Dim OtherTotal As Integer
            With FieldList
                If .Count > 0 Then
                    OtherTotal = .Item(.Count - 1).Right + varFieldSpacing
                End If
            End With
            Return OtherTotal
        End Function
        Protected Friend Function CalculateRemainingWidth() As Integer
            ' Calculate total width
            Dim OtherTotal As Integer
            With FieldList
                If .Count > 0 Then
                    OtherTotal = .Item(.Count - 1).Right + varFieldSpacing
                End If
            End With
            ' Do not count the left and right side
            'OtherTotal -= varFieldSpacing * 2
            ' Return difference
            ' TODO: No loop
            Return Width - OtherTotal
        End Function
        Protected Overrides Sub Dispose(disposing As Boolean)
            With FieldList
                Dim iLast As Integer = .Count - 1
                For i As Integer = 0 To iLast
                    .Item(i).Dispose()
                Next
            End With
            MyBase.Dispose(disposing)
        End Sub
    End Class
    Private Class FormCheckBox
        Inherits FormField
        Private WithEvents TextLab As Label
        Private WithEvents Check As PictureBox
        Private FocusedBrush As New SolidBrush(Color.Gray)
        Private varChecked As Boolean
        Private CheckBrush As New SolidBrush(Color.Black)
        Private Property Checked As Boolean
            Get
                Return varChecked
            End Get
            Set(value As Boolean)
                varChecked = value
                If Check IsNot Nothing Then
                    Check.Invalidate()
                End If
            End Set
        End Property
        Protected Overrides Sub OnUserClick()
            MyBase.OnUserClick()
            If varChecked Then
                Checked = False
            Else
                Checked = True
            End If
        End Sub
        Private Sub OnHeaderVisibleChanged() Handles Me.HeaderVisibleChanged
            With TextLab
                .SuspendLayout()
                .Top = Header.Height
                .Height = Height - .Top
                .Width = Width
                .ResumeLayout()
            End With
            With Check
                .Top = CInt(TextLab.Top + (TextLab.Height / 2) - .Height / 2)
            End With
        End Sub
        Private Sub OnCheckPaint(Sender As Object, e As PaintEventArgs) Handles Check.Paint
            If varChecked Then
                With e.Graphics
                    .SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
                    .DrawString(ChrW(&H2713), SystemFonts.MessageBoxFont, CheckBrush, Point.Empty)
                End With
            End If
        End Sub
        Protected Overrides Sub OnValueChanged(Value As Object)
            MyBase.OnValueChanged(Value)
            Try
                varChecked = DirectCast(Value, Boolean)
            Catch
                Throw New InvalidCastException("This FormField instance expects the value object to be a boolean value (checked or not checked).")
            End Try
            If Check IsNot Nothing Then
                Check.Invalidate()
            End If
        End Sub
        Protected Overrides Sub OnSecondaryValueChanged(Value As Object)
            MyBase.OnSecondaryValueChanged(Value)
            TextLab.Text = DirectCast(Value, String)
        End Sub
        Private Sub OnCheckMouseUp(Sender As Object, e As EventArgs) Handles Check.MouseUp
            If varChecked Then
                varChecked = False
            Else
                varChecked = True
            End If
            Value = varChecked
            Check.Invalidate()
            OnValueChanged(varChecked)
        End Sub
        Public Sub New(ByVal FieldSpacing As Integer)
            MyBase.New(FieldSpacing, FormElementType.CheckBox)
            SetStyle(ControlStyles.Selectable, True)
            MeType = FormElementType.CheckBox
            Check = New PictureBox
            TextLab = New Label
            Hide()
            With Check
                .Hide()
                .Parent = Me
                .Width = 16
                .Height = 16
                .BackColor = Color.White
                .Left = 10
                .Show()
            End With
            With TextLab
                .Hide()
                .Parent = Me
                '.BackColor = Color.FromArgb(7, 70, 91)
                .BackColor = Color.Transparent
                .Padding = New Padding(34, 0, 0, 0)
                .Left = 0
                .Top = 14
                .ForeColor = Color.White
                .Text = "Meld meg på"
                .FlatStyle = FlatStyle.Flat
                .TextAlign = ContentAlignment.MiddleLeft
                .Show()
            End With
            Value = False
            Show()
        End Sub
        Protected Overrides Sub OnSizeChanged(e As EventArgs)
            MyBase.OnSizeChanged(e)
            With TextLab
                .SuspendLayout()
                .Top = Header.Height
                .Height = Height - .Top
                .Width = Width
                .ResumeLayout()
            End With
            With Check
                .Top = CInt(TextLab.Top + (TextLab.Height / 2) - .Height / 2)
            End With
        End Sub
        Protected Overrides Sub OnVisibleChanged(e As EventArgs)
            MyBase.OnVisibleChanged(e)
            With TextLab
                .SuspendLayout()
                .Top = Header.Height
                .Height = Height - .Top
                .Width = Width
                .ResumeLayout()
            End With
            With Check
                .Top = CInt(TextLab.Top + (TextLab.Height / 2) - .Height / 2)
            End With
        End Sub
        Protected Overrides Sub Dispose(disposing As Boolean)
            TextLab.Dispose()
            Check.Dispose()
            CheckBrush.Dispose()
            FocusedBrush.Dispose()
            MyBase.Dispose(disposing)
        End Sub
    End Class

    Private Class FormButton
        Inherits FormField
        Private TextLab As New Label
        Public Sub New(ByVal FieldSpacing As Integer)
            MyBase.New(FieldSpacing, FormElementType.Button)
            MeType = FormElementType.Button
        End Sub
        Protected Overrides Sub OnSecondaryValueChanged(Value As Object)
            MyBase.OnSecondaryValueChanged(Value)
            TextLab.Text = DirectCast(Value, String)
        End Sub
        ' TODO: Handle OnHeaderVisibleChanged
        Protected Overrides Sub Dispose(disposing As Boolean)
            TextLab.Dispose()
            MyBase.Dispose(disposing)
        End Sub
    End Class

    Private Class FormTextField
        Inherits FormField
        Private PaddingLeft As Integer = 10
        Private TextField As TextBox
        Private Sub Me_BackColorChanged(Sender As Object, e As EventArgs) Handles Me.BackColorChanged
            If TextField IsNot Nothing Then
                TextField.BackColor = BackColor
            End If
        End Sub

        Public Sub New(ByVal FieldSpacing As Integer)
            MyBase.New(FieldSpacing, FormElementType.TextField)
            MeType = FormElementType.TextField
            TextField = New TextBox
            With Me
                .Hide()
                '.BackColor = Color.FromArgb(7, 70, 91)
                .ForeColor = Color.White
            End With
            With TextField
                .Parent = Me
                .Left = PaddingLeft
                .BackColor = Color.FromArgb(7, 70, 91)
                .Width = Width - PaddingLeft
                .Height = 16
                .Top = CInt((Height / 2) - (.Height / 2) + (Header.Height / 2))
                .ForeColor = Color.White
                .BorderStyle = BorderStyle.None
                .Show()
            End With
            Show()
        End Sub
        Protected Overrides Sub OnSizeChanged(e As EventArgs)
            MyBase.OnSizeChanged(e)
            With TextField
                .Width = Width - PaddingLeft
                .Top = CInt((Height / 2) - (.Height / 2) + (Header.Height / 2))
            End With
        End Sub
        Protected Overrides Sub OnVisibleChanged(e As EventArgs)
            MyBase.OnVisibleChanged(e)
            With TextField
                .Width = Width - PaddingLeft
                .Top = CInt((Height / 2) - (.Height / 2) + (Header.Height / 2))
            End With
        End Sub
        Protected Overrides Sub OnValueChanged(Value As Object)
            MyBase.OnValueChanged(Value)
            TextField.Text = DirectCast(Value, String)
        End Sub
        Protected Overrides Sub OnSecondaryValueChanged(Value As Object)
            MyBase.OnSecondaryValueChanged(Value)
            TextField.Text = DirectCast(Value, String)
        End Sub
        Protected Overrides Sub Dispose(disposing As Boolean)
            TextField.Dispose()
            MyBase.Dispose(disposing)
        End Sub
    End Class

    Private Class FormLabel
        Inherits FormField
        Private WithEvents TextLab As Label
        Private Sub OnTextLabAlignChanged() Handles TextLab.TextAlignChanged
            Select Case TextLab.TextAlign
                Case ContentAlignment.TopLeft
                    TextLab.Padding = New Padding(6, 3, 0, 0)
                Case ContentAlignment.TopCenter
                    TextLab.Padding = New Padding(0, 3, 0, 0)
                Case ContentAlignment.TopRight
                    TextLab.Padding = New Padding(0, 3, 6, 0)
                Case ContentAlignment.MiddleLeft
                    TextLab.Padding = New Padding(6, 0, 0, 0)
                Case ContentAlignment.MiddleCenter
                    TextLab.Padding = New Padding(0, 0, 0, 0)
                Case ContentAlignment.MiddleRight
                    TextLab.Padding = New Padding(0, 0, 6, 0)
                Case ContentAlignment.BottomLeft
                    TextLab.Padding = New Padding(6, 0, 0, 3)
                Case ContentAlignment.BottomCenter
                    TextLab.Padding = New Padding(0, 0, 0, 3)
                Case ContentAlignment.BottomRight
                    TextLab.Padding = New Padding(0, 0, 6, 3)
            End Select
        End Sub
        Public Sub New(ByVal FieldSpacing As Integer)
            MyBase.New(FieldSpacing, FormElementType.Label)
            MeType = FormElementType.Label
            varIsInputType = False
            SetStyle(ControlStyles.Selectable, False)
            TextLab = New Label
            Hide()
            With TextLab
                .TabStop = False
                .Parent = Me
                '.BackColor = Color.FromArgb(7, 70, 91)
                .BackColor = Color.Transparent
                .ForeColor = Color.White
                .AutoSize = False
                .TextAlign = ContentAlignment.MiddleLeft
                .Padding = New Padding(6, 0, 0, 0)
                .Left = 0
                .Top = Header.Height
            End With
            Value = "Change the default text using the Value property."

        End Sub
        Protected Overrides Sub OnValueChanged(Value As Object)
            MyBase.OnValueChanged(Value)
            TextLab.Text = DirectCast(Value, String)
        End Sub
        Protected Overrides Sub OnSecondaryValueChanged(Value As Object)
            MyBase.OnSecondaryValueChanged(Value)
            TextLab.Text = DirectCast(Value, String)
        End Sub
        Protected Overrides Sub OnSizeChanged(e As EventArgs)
            MyBase.OnSizeChanged(e)
            With TextLab
                .SuspendLayout()
                .Width = Width
                .Height = Height - Header.Height
                .Top = Header.Height
                .ResumeLayout()
                .PerformLayout()
            End With
        End Sub
        Protected Overrides Sub OnVisibleChanged(e As EventArgs)
            MyBase.OnVisibleChanged(e)
            With TextLab
                .SuspendLayout()
                .Width = Width
                .Height = Height - Header.Height
                .Top = Header.Height
                .ResumeLayout()
            End With
        End Sub
        Private Sub OnHeaderVisibleChanged() Handles Me.HeaderVisibleChanged
            With TextLab
                .SuspendLayout()
                .Width = Width
                .Height = Height - Header.Height
                .Top = Header.Height
                .ResumeLayout()
            End With
        End Sub
        Protected Overrides Sub Dispose(disposing As Boolean)
            TextLab.Dispose()
            MyBase.Dispose(disposing)
        End Sub
    End Class

    Private Class FormDropDown
        Inherits FormField
        Public Sub New(ByVal FieldSpacing As Integer)
            MyBase.New(FieldSpacing, FormElementType.DropDown)
            MeType = FormElementType.DropDown
        End Sub
        Protected Overrides Sub Dispose(disposing As Boolean)
            MyBase.Dispose(disposing)
        End Sub
    End Class

    Private Class FormRadioButton
        Inherits FormField
        Private WithEvents CheckSurface As PictureBox
        Private WithEvents TextLab As Label
        Private varChecked As Boolean
        Private CheckBrush As New SolidBrush(Color.White)
        Private DotBrush As New SolidBrush(Color.Black)
        Private HoverBrush As New SolidBrush(Color.LightGray)
        Private Hovering As Boolean
        Private varIndex As Integer
        Private ReadOnly Property Checked As Boolean
            Get
                Return varChecked
            End Get
        End Property
        Private Sub Check()
            varChecked = True
            CheckSurface.Refresh()
            OnValueChanged(varChecked)
        End Sub
        Public ReadOnly Property RadioIndex As Integer
            Get
                Return varIndex
            End Get
        End Property
        Protected Overrides Sub OnUserClick()
            MyBase.OnUserClick()
            If Not varChecked Then
                Check()
            End If
        End Sub
        Public Sub New(ByVal RadioContextIndex As Integer, ByVal FieldSpacing As Integer)
            MyBase.New(FieldSpacing, FormElementType.Radio)
            SuspendLayout()
            MeType = FormElementType.Radio
            CheckSurface = New PictureBox
            TextLab = New Label
            Hide()
            varIndex = RadioContextIndex
            With CheckSurface
                .Parent = Me
                .Size = New Size(16, 16)
                '.BackColor = Color.FromArgb(7, 70, 91)
                .BackColor = Color.Transparent
                .Left = 10
            End With
            With TextLab
                .Hide()
                .AutoSize = False
                .Parent = Me
                '.BackColor = Color.FromArgb(7, 70, 91)
                .BackColor = Color.Transparent
                .Padding = New Padding(34, 0, 0, 0)
                .Location = New Point(0, Header.Height)
                .Height = Height - Header.Height
                .ForeColor = Color.White
                .Text = "Meld meg på"
                .FlatStyle = FlatStyle.Flat
                .TextAlign = ContentAlignment.MiddleLeft
                .SendToBack()
                .Show()
            End With
            Show()
            ResumeLayout()
        End Sub
        Private Sub CheckSurface_Paint(Sender As Object, e As PaintEventArgs) Handles CheckSurface.Paint
            Dim DotRect As Rectangle
            With CheckSurface
                DotRect = New Rectangle(0, 0, .Width - 1, .Height - 1)
            End With
            With e.Graphics
                .SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
                .FillEllipse(CheckBrush, DotRect)
                If varChecked Then
                    DotRect.Inflate(-4, -4)
                    .FillEllipse(DotBrush, DotRect)
                ElseIf Hovering Then
                    DotRect.Inflate(-4, -4)
                    .FillEllipse(HoverBrush, DotRect)
                End If
            End With
        End Sub
        Private Sub CheckSurface_MouseEnter(Sender As Object, e As EventArgs) Handles CheckSurface.MouseEnter
            Hovering = True
            CheckSurface.Refresh()
        End Sub
        Private Sub CheckSurface_MouseLeave(Sender As Object, e As EventArgs) Handles CheckSurface.MouseLeave
            Hovering = False
            CheckSurface.Refresh()
        End Sub
        Private Sub CheckSurface_Click(Sender As Object, e As EventArgs) Handles CheckSurface.Click
            If Hovering Then
                varChecked = True
            End If
            CheckSurface.Refresh()
            OnValueChanged(varChecked)
        End Sub
        Protected Overrides Sub OnSizeChanged(e As EventArgs)
            MyBase.OnSizeChanged(e)
            SuspendLayout()
            With TextLab
                .Width = Width
                .Height = Height - Header.Height
                .Top = Header.Height
            End With
            With Header
                CheckSurface.Top = .Height + (Height - .Height) \ 2 - 8
                CheckSurface.Left = 10
            End With
            ResumeLayout()
        End Sub
        Protected Overrides Sub OnVisibleChanged(e As EventArgs)
            MyBase.OnVisibleChanged(e)
            SuspendLayout()
            With TextLab
                .Width = Width
                .Height = Height - Header.Height
                .Top = Header.Height
            End With
            'With CheckSurface
            '    .Top = CInt((Height / 2) - (.Top / 2)) + TextLab.Top - 2
            'End With
            With Header
                CheckSurface.Top = .Height + (Height - .Height) \ 2 - 8
                CheckSurface.Left = 10
            End With
            ResumeLayout()
        End Sub
        Protected Overrides Sub OnValueChanged(Value As Object)
            MyBase.OnValueChanged(Value)
            varChecked = DirectCast(Value, Boolean)
            CheckSurface.Refresh()
        End Sub
        Protected Overrides Sub OnSecondaryValueChanged(Value As Object)
            MyBase.OnSecondaryValueChanged(Value)
            TextLab.Text = DirectCast(Value, String)
        End Sub
        Protected Overrides Sub Dispose(disposing As Boolean)
            TextLab.Dispose()
            CheckSurface.Dispose()
            CheckBrush.Dispose()
            DotBrush.Dispose()
            HoverBrush.Dispose()
            MyBase.Dispose(disposing)
        End Sub
    End Class

    Private Class RadioButtonContext
        Implements IDisposable

        Private ControlList As List(Of FormRadioButton)
        Public ReadOnly Property Count As Integer
            Get
                Return ControlList.Count
            End Get
        End Property
        Public Sub New()
            ControlList = New List(Of FormRadioButton)(4)
        End Sub
        Public Sub Add(R As FormRadioButton)
            ControlList.Add(R)
            AddHandler R.ValueChanged, AddressOf OnValueChanged
        End Sub
        Private Sub OnValueChanged(Sender As FormField, Value As Object)
            Dim SenderX As FormRadioButton = DirectCast(Sender, FormRadioButton)
            If DirectCast(Value, Boolean) Then
                With ControlList
                    Dim iLast As Integer = .Count - 1
                    For i As Integer = 0 To iLast
                        With .Item(i)
                            If .RadioIndex <> SenderX.RadioIndex Then
                                .Value = False
                            End If
                        End With
                    Next
                End With
            End If
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                End If
                With ControlList
                    Dim iLast As Integer = .Count - 1
                    For i As Integer = 0 To iLast
                        RemoveHandler .Item(i).ValueChanged, AddressOf OnValueChanged
                    Next
                End With
                ControlList.Clear()
                ControlList.TrimExcess()
                ControlList = Nothing
                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            ' TODO: uncomment the following line if Finalize() is overridden above.
            ' GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class
End Class
