'Option Strict On
'Option Explicit On
'Option Infer Off
'Imports System.Drawing
'Imports System.Drawing.Drawing2D
'Imports System.Windows.Forms
'Imports AudiopoLib

'Public Class FormField
'    Inherits Control
'    Private labHeader As Label
'    Private varValue As Object = Nothing
'    Private varSecondaryValue As Object = Nothing
'    Private varSpacingBottom As Integer = 10
'    Private varDrawGradient As Boolean = True
'    Protected Friend varFieldType As FormElementType
'    Protected Friend Event ValueChanged(Sender As FormField, ByVal Value As Object)
'    Protected Friend Event SecondaryValueChanged(Sender As FormField, ByVal Value As Object)
'    Protected Event HeaderVisibleChanged(ByVal Visible As Boolean)
'    'Protected Friend varIsInputType As Boolean = True
'    Private varTopColor As Color
'    Private varBottomColor As Color
'    Protected MeType As FormElementType
'    Protected Friend GradBrush As LinearGradientBrush
'    Public Property DrawGradient As Boolean
'        Get
'            Return varDrawGradient
'        End Get
'        Set(value As Boolean)
'            varDrawGradient = value
'            Invalidate()
'        End Set
'    End Property
'    Protected Friend Property TopColor As Color
'        Get
'            Return varTopColor
'        End Get
'        Set(value As Color)
'            varTopColor = value
'            If GradBrush IsNot Nothing Then
'                GradBrush.Dispose()
'            End If
'            GradBrush = New LinearGradientBrush(New Point(0, 0), New Point(0, Bottom), varTopColor, varBottomColor)
'            Invalidate()
'        End Set
'    End Property
'    Protected Friend Property BottomColor As Color
'        Get
'            Return varBottomColor
'        End Get
'        Set(value As Color)
'            varBottomColor = value
'            If GradBrush IsNot Nothing Then
'                GradBrush.Dispose()
'            End If
'            GradBrush = New LinearGradientBrush(New Point(0, 0), New Point(0, Bottom), varTopColor, varBottomColor)
'            Invalidate()
'        End Set
'    End Property
'    ' TODO: Override in all derived classes for which pressing enter og space should have an effect.
'    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
'        MyBase.OnKeyDown(e)
'        If e.KeyCode = Keys.Space OrElse e.KeyCode = Keys.Enter Then
'            OnUserClick()
'        End If
'    End Sub
'    Protected Overridable Sub OnUserClick()

'    End Sub
'    Protected Overridable Sub OnValueChanged(ByVal Value As Object)
'        RaiseEvent ValueChanged(Me, Value)
'    End Sub
'    Protected Overridable Sub OnSecondaryValueChanged(ByVal Value As Object)
'        RaiseEvent SecondaryValueChanged(Me, Value)
'    End Sub
'    Protected Overrides Sub OnPaint(e As PaintEventArgs)
'        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
'        e.Graphics.CompositingQuality = CompositingQuality.HighQuality
'        If varDrawGradient Then
'            e.Graphics.FillRectangle(GradBrush, ClientRectangle)
'        End If
'        MyBase.OnPaint(e)
'    End Sub
'    Protected Overrides Sub OnGotFocus(e As EventArgs)
'        MyBase.OnGotFocus(e)
'        If Not MeType = FormElementType.TextField Then
'            BackColor = ColorHelper.Multiply(BackColor, 0.4)
'            DrawGradient = False
'        End If
'    End Sub
'    Protected Overrides Sub OnLostFocus(e As EventArgs)
'        MyBase.OnLostFocus(e)
'        If Not MeType = FormElementType.TextField Then
'            BackColor = ColorHelper.Multiply(BackColor, 2.5)
'            DrawGradient = True
'        End If
'    End Sub
'    Public Sub New(ByVal FieldSpacing As Integer, ByVal FieldType As FormElementType)
'        varFieldType = FieldType
'        varSpacingBottom = FieldSpacing
'        SetStyle(ControlStyles.Selectable, True)
'        labHeader = New Label
'        Controls.Add(labHeader)
'        SuspendLayout()
'        With labHeader
'            .Top = 0
'            .Left = 0
'            .Width = Width
'            .Font = New Font(.Font.FontFamily, 8)
'            .Height = 17
'            .BackColor = Color.FromArgb(5, 53, 68)
'            .TextAlign = ContentAlignment.MiddleLeft
'            .ForeColor = Color.White
'            .Padding = New Padding(6, 0, 0, 0)
'        End With
'        BackColor = Color.FromArgb(7, 70, 91)
'        varTopColor = Color.FromArgb(50, ColorHelper.Multiply(BackColor, 1.5))
'        varBottomColor = Color.FromArgb(50, BackColor)
'        ResumeLayout()
'    End Sub
'    Protected Overrides Sub OnSizeChanged(e As EventArgs)
'        MyBase.OnSizeChanged(e)
'        labHeader.Width = Width
'        If GradBrush IsNot Nothing Then
'            GradBrush.Dispose()
'        End If
'        GradBrush = New LinearGradientBrush(New Point(0, 0), New Point(0, Bottom), TopColor, BottomColor)
'    End Sub
'    Protected Overrides Sub OnVisibleChanged(e As EventArgs)
'        MyBase.OnVisibleChanged(e)
'        labHeader.Width = Width
'    End Sub
'    Public ReadOnly Property Header As Label
'        Get
'            Return labHeader
'        End Get
'    End Property
'    Public Overridable Property Value As Object
'        Get
'            Return varValue
'        End Get
'        Set(value As Object)
'            varValue = value
'            OnValueChanged(value)
'        End Set
'    End Property
'    Public Overridable Property SecondaryValue As Object
'        Get
'            Return varSecondaryValue
'        End Get
'        Set(value As Object)
'            varSecondaryValue = value
'            OnSecondaryValueChanged(value)
'        End Set
'    End Property
'    Public Overridable Overloads Sub SwitchHeader(ByVal SwitchOn As Boolean)
'        If SwitchOn Then
'            Header.Height = 17
'            Header.Show()
'        Else
'            Header.Height = 0
'            Header.Hide()
'        End If
'        RaiseEvent HeaderVisibleChanged(SwitchOn)
'    End Sub
'    Public Sub Extrude(ByVal Side As FieldExtrudeSide, ByVal Amount As Integer)
'        SuspendLayout()
'        Select Case Side
'            Case FieldExtrudeSide.Left
'                Width += Amount
'                Left -= Amount
'            Case FieldExtrudeSide.Right
'                Width += Amount
'            Case FieldExtrudeSide.Bottom
'                Height += Amount
'        End Select
'        ResumeLayout()
'    End Sub
'    Protected Overrides Sub Dispose(disposing As Boolean)
'        labHeader.Dispose()
'        GradBrush.Dispose()
'        MyBase.Dispose(disposing)
'    End Sub
'End Class
'Public Enum FieldExtrudeSide
'    Left
'    Right
'    Bottom
'End Enum
'Public Enum FormElementType
'    CheckBox
'    TextField
'    Button
'    Label
'    DropDown
'    Radio
'End Enum
'Public Class FormElement
'    Private E As FormElementType
'    Private W As Integer
'    Private V As Object
'    Public Sub New(ByVal Type As FormElementType, Optional ByVal Width As Integer = -1, Optional ByVal Value As Object = Nothing)
'        E = Type
'        W = Width
'        V = Value
'    End Sub
'    Public Property Width As Integer
'        Get
'            Return W
'        End Get
'        Set(value As Integer)
'            W = value
'        End Set
'    End Property
'    Public Property Type As FormElementType
'        Get
'            Return E
'        End Get
'        Set(value As FormElementType)
'            E = value
'        End Set
'    End Property
'    Public Property Value As Object
'        Get
'            Return V
'        End Get
'        Set(value As Object)
'            V = value
'        End Set
'    End Property
'End Class
'Public Class HeaderValuePair
'    Public Header As String = "Not set"
'    Public Value As Object = False
'    Public Type As FormElementType
'    Public Sub New(ByVal HeaderString As String, ByVal ValueObject As Object, ByVal ElementType As FormElementType)
'        Header = HeaderString
'        Value = ValueObject
'        Type = ElementType
'    End Sub
'End Class
'Public Class FlatFormResult
'    Private ResultArr()() As HeaderValuePair
'    Protected Friend Sub New(Result As HeaderValuePair()())
'        ResultArr = Result
'    End Sub
'    Public Function Header(ByVal Row As Integer, ByVal Field As Integer) As String
'        Return ResultArr(Row)(Field).Header
'    End Function
'    Public Function Value(ByVal Row As Integer, ByVal Field As Integer) As Object
'        Return ResultArr(Row)(Field).Value
'    End Function
'    Public Function FieldType(ByVal Row As Integer, ByVal Field As Integer) As FormElementType
'        Return ResultArr(Row)(Field).Type
'    End Function
'End Class
'Public Class TestFlatForm
'#Region "FlatForm"
'    Inherits Control
'    Private varFieldSpacing As Integer
'    Private RadioContextList As List(Of RadioButtonContext)
'    Private RowList As List(Of FormRow)
'    Public ReadOnly Property Result() As FlatFormResult
'        Get
'            Dim iLast As Integer = RowList.Count - 1
'            Dim RetArr(iLast)() As HeaderValuePair
'            For i As Integer = 0 To iLast
'                Dim ThisRow As FormRow = RowList(i)
'                Dim nLast As Integer = RowList(i).Fields.Count - 1
'                Dim PairArr(nLast) As HeaderValuePair
'                For n As Integer = 0 To nLast
'                    Dim Field As FormField = ThisRow.Fields(n)
'                    PairArr(n) = New HeaderValuePair(Field.Header.Text, Field.Value, Field.varFieldType)
'                Next
'                RetArr(i) = PairArr
'            Next
'            Return New FlatFormResult(RetArr)
'        End Get
'    End Property
'    Public ReadOnly Property FieldCount As Integer
'        Get
'            Dim Sum As Integer = 0
'            If RowList.Count > 0 Then
'                For i As Integer = 0 To RowList.Count - 1
'                    Sum += RowList(i).Controls.Count
'                Next
'            End If
'            Return Sum
'        End Get
'    End Property
'    Public ReadOnly Property RowCount As Integer
'        Get
'            Return RowList.Count
'        End Get
'    End Property
'    Public ReadOnly Property Field(ByVal Row As Integer, ByVal Index As Integer) As FormField
'        Get
'            Return RowList(Row).Fields(Index)
'        End Get
'    End Property
'    Public Sub New(ByVal Width As Integer, ByVal Height As Integer, ByVal FieldSpacing As Integer)
'        RowList = New List(Of FormRow)
'        RadioContextList = New List(Of RadioButtonContext)
'        RadioContextList.Add(New RadioButtonContext)
'        varFieldSpacing = FieldSpacing
'        With Me
'            .Hide()
'            .SuspendLayout()
'            .Width = Width
'            .ResumeLayout()
'        End With
'    End Sub
'    Protected Overrides Sub OnResize(e As EventArgs)
'        MyBase.OnResize(e)
'        If RowList.Count > 0 Then
'            Dim NewHeight As Integer = RowList(RowList.Count - 1).Bottom - varFieldSpacing
'            If Height <> NewHeight Then Height = NewHeight
'        End If
'    End Sub
'    Protected Overrides Sub OnVisibleChanged(e As EventArgs)
'        MyBase.OnVisibleChanged(e)
'        If RowList.Count <> 0 Then
'            Height = RowList(RowList.Count - 1).Bottom - varFieldSpacing
'        End If
'    End Sub
'    Public Sub MergeWithAbove(ByVal Row As Integer, ByVal FieldIndex As Integer, Optional ByVal UpperFieldIndexOffset As Integer = 0, Optional ByVal RepositionLowerChildren As Boolean = False)
'        Dim Upper As FormField = RowList(Row - 1).Fields(FieldIndex + UpperFieldIndexOffset)
'        Dim Lower As FormField = RowList(Row).Fields(FieldIndex)
'        With Upper
'            .Extrude(FieldExtrudeSide.Bottom, varFieldSpacing)
'        End With
'        With Lower
'            If RepositionLowerChildren Then
'                .SwitchHeader(False)
'            Else
'                .Header.Hide()
'            End If
'        End With
'        Dim TotalHeight As Integer = Upper.Height + Lower.Height
'        Dim Ratio As Double = Upper.Height / TotalHeight
'        Upper.TopColor = Color.FromArgb(50, ColorHelper.Multiply(Upper.BackColor, 2))
'        Upper.BottomColor = Color.FromArgb(50, ColorHelper.Mix(Color.FromArgb(0, Upper.BackColor), Upper.TopColor, Ratio))
'        Lower.TopColor = Upper.BottomColor
'        Lower.BottomColor = Color.FromArgb(0, Lower.BackColor)
'    End Sub
'    Public Sub Display()
'        Show()
'    End Sub
'    Public Sub AddRow(Optional ByVal RowHeight As Integer = 40)
'        Dim NewRow As New FormRow(varFieldSpacing)
'        With NewRow
'            .Width = Width
'            .Height = RowHeight + varFieldSpacing
'            .Parent = Me
'            .Left = 0
'            If RowList.Count > 0 Then
'                .Top = RowList(RowList.Count - 1).Bottom
'            Else
'                .Top = 0
'            End If
'        End With
'        RowList.Add(NewRow)
'    End Sub
'    Public Sub AddRadioContext()
'        Dim R As New RadioButtonContext
'        RadioContextList.Add(R)
'    End Sub
'    Public Overloads Sub AddField(Elements() As FormElement)
'        Dim iLast As Integer = Elements.GetLength(0) - 1
'        For i As Integer = 0 To iLast
'            Dim Element As FormElement = Elements(i)
'            AddField(Element.Type, Element.Width, Element.Value)
'        Next
'    End Sub
'    Public Overloads Sub AddField(ByVal FieldType As FormElementType, Optional ByVal FieldWidth As Integer = -1, Optional ByVal Value As Object = Nothing)
'        Dim LastRow As FormRow
'        If RowList.Count > 0 Then
'            LastRow = RowList(RowList.Count - 1)
'            If LastRow.CalculateTotalWidth + FieldWidth > LastRow.Width Then
'                Dim NewRow As New FormRow(varFieldSpacing)
'                With NewRow
'                    .Width = Width
'                    .Height = 57 + varFieldSpacing
'                    .Parent = Me
'                    .Left = 0
'                    .Top = LastRow.Bottom
'                End With
'                RowList.Add(NewRow)
'            End If
'        Else
'            Dim NewRow As New FormRow(varFieldSpacing)
'            With NewRow
'                .Width = Width
'                .Height = 57 + varFieldSpacing
'                .Parent = Me
'                .Left = 0
'                .Top = 0
'            End With
'            RowList.Add(NewRow)
'        End If
'        If RowList.Count = 0 Then
'            Dim NewRow As New FormRow(varFieldSpacing)
'            With NewRow
'                .Parent = Me
'                .Top = 0
'                .Left = 0
'                .Width = Width
'                .Height = 57 + varFieldSpacing
'            End With
'            RowList.Add(NewRow)
'        End If
'        LastRow = RowList(RowList.Count - 1)
'        Dim NewControl As FormField
'        Select Case FieldType
'            Case FormElementType.CheckBox
'                NewControl = New FormCheckBox(varFieldSpacing)
'            Case FormElementType.TextField
'                NewControl = New FormTextField(varFieldSpacing)
'                NewControl.BackColor = ColorHelper.Multiply(NewControl.BackColor, 0.4)
'                NewControl.DrawGradient = False
'            Case FormElementType.Button
'                NewControl = New FormButton(varFieldSpacing)
'            Case FormElementType.DropDown
'                NewControl = New FormDropDown(varFieldSpacing)
'            Case FormElementType.Label
'                NewControl = New FormLabel(varFieldSpacing)
'            Case FormElementType.Radio
'                NewControl = New FormRadioButton(RadioContextList(RadioContextList.Count - 1).Count, (varFieldSpacing))
'                RadioContextList(RadioContextList.Count - 1).Add(DirectCast(NewControl, FormRadioButton))
'            Case Else
'                NewControl = Nothing
'        End Select
'        'With NewControl
'        '    .Height = LastRow.Height - varFieldSpacing
'        'End With
'        If Value IsNot Nothing Then
'            NewControl.Value = Value
'        End If
'        If FieldWidth < 0 Then
'            LastRow.AddField(NewControl, LastRow.CalculateRemainingWidth)
'        Else
'            LastRow.AddField(NewControl, FieldWidth)
'        End If
'    End Sub
'    Protected Overrides Sub Dispose(disposing As Boolean)
'        Dim nLast As Integer = RadioContextList.Count - 1
'        For n As Integer = 0 To nLast
'            RadioContextList(n).Dispose()
'        Next
'        RadioContextList = Nothing
'        Dim iLast As Integer = RowList.Count - 1
'        For i As Integer = 0 To iLast
'            RowList(i).Dispose()
'        Next
'        MyBase.Dispose(disposing)
'    End Sub
'#End Region
'    Private Class FormRow
'        Inherits Control
'        Private varFieldSpacing As Integer
'        Private FieldList As List(Of FormField)
'        Public ReadOnly Property Fields As List(Of FormField)
'            Get
'                Return FieldList
'            End Get
'        End Property
'        Public ReadOnly Property FieldSpacing As Integer
'            Get
'                Return varFieldSpacing
'            End Get
'        End Property
'        'Public Sub Display()
'        '    Dim iLast As Integer = Controls.Count - 1
'        '    For i As Integer = 0 To iLast
'        '        Controls(i).Show()
'        '    Next
'        'End Sub
'        Public Sub New(ByVal FieldSpacing As Integer)
'            FieldList = New List(Of FormField)
'            SetStyle(ControlStyles.Selectable, False)
'            'GradSurface = New Control
'            SuspendLayout()
'            'With GradSurface
'            '.Parent = Me
'            '.Top = 0
'            '.Left = 0
'            '.SendToBack()
'            'End With
'            varFieldSpacing = FieldSpacing
'            With Me
'                .Height = 40
'                .Width = 600
'            End With
'            'GradBrush = New LinearGradientBrush(New Point(0, 0), New Point(Left, Bottom), ColorFirst, ColorSecond)
'            ResumeLayout()
'            Show()
'        End Sub
'        Public Sub AddField(Item As FormField, ByVal FieldWidth As Integer)
'            With Item
'                .Hide()
'                If FieldList.Count > 0 Then
'                    .Left = CalculateTotalWidth()
'                Else
'                    .Left = 0
'                End If
'                .Top = 0
'                .Height = Height - varFieldSpacing
'                .Width = FieldWidth
'                .Show()
'            End With
'            Controls.Add(Item)
'            FieldList.Add(Item)
'        End Sub
'        Public Function CalculateTotalWidth() As Integer
'            Dim Total As Integer = FieldList(FieldList.Count - 1).Right + varFieldSpacing
'            ' Do not count the left and right side
'            Return Total
'        End Function
'        Public Function CalculateRemainingWidth() As Integer
'            Dim Total As Integer
'            If FieldList.Count > 0 Then
'                Total = FieldList(FieldList.Count - 1).Right + varFieldSpacing
'                ' Return difference
'            Else
'                Total = 0
'            End If
'            Return Width - Total
'        End Function
'        Protected Overrides Sub Dispose(disposing As Boolean)
'            Dim iLast As Integer = FieldList.Count - 1
'            For i As Integer = 0 To iLast
'                FieldList(i).Dispose()
'            Next
'            MyBase.Dispose(disposing)
'        End Sub
'    End Class
'    Private Class FormCheckBox
'        Inherits FormField
'        Private WithEvents TextLab As Label
'        Private WithEvents Check As PictureBox
'        Private FocusedBrush As New SolidBrush(Color.Gray)
'        Private varChecked As Boolean
'        Private CheckBrush As New SolidBrush(Color.Black)
'        Private Property Checked As Boolean
'            Get
'                Return varChecked
'            End Get
'            Set(value As Boolean)
'                varChecked = value
'                If Check IsNot Nothing Then
'                    Check.Invalidate()
'                End If
'            End Set
'        End Property
'        Protected Overrides Sub OnUserClick()
'            MyBase.OnUserClick()
'            If varChecked Then
'                Checked = False
'            Else
'                Checked = True
'            End If
'        End Sub
'        Private Sub OnHeaderVisibleChanged() Handles Me.HeaderVisibleChanged
'            With TextLab
'                .SuspendLayout()
'                .Top = Header.Height
'                .Height = Height - .Top
'                .Width = Width
'                .ResumeLayout()
'            End With
'            With Check
'                .Top = TextLab.Top + TextLab.Height \ 2 - .Height \ 2
'            End With
'        End Sub
'        Private Sub OnCheckPaint(Sender As Object, e As PaintEventArgs) Handles Check.Paint
'            If varChecked Then
'                e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
'                e.Graphics.DrawString(ChrW(&H2713), SystemFonts.MessageBoxFont, CheckBrush, New Point(0, 0))
'            End If
'        End Sub
'        Protected Overrides Sub OnValueChanged(Value As Object)
'            MyBase.OnValueChanged(Value)
'            Try
'                varChecked = DirectCast(Value, Boolean)
'            Catch ex As InvalidCastException
'            End Try
'            If Check IsNot Nothing Then
'                Check.Invalidate()
'            End If
'        End Sub
'        Protected Overrides Sub OnSecondaryValueChanged(Value As Object)
'            MyBase.OnSecondaryValueChanged(Value)
'            TextLab.Text = DirectCast(Value, String)
'        End Sub
'        Private Sub OnCheckMouseUp(Sender As Object, e As EventArgs) Handles Check.MouseUp
'            If varChecked Then
'                varChecked = False
'            Else
'                varChecked = True
'            End If
'            Value = varChecked
'            Check.Invalidate()
'            OnValueChanged(varChecked)
'        End Sub
'        Public Sub New(ByVal FieldSpacing As Integer)
'            MyBase.New(FieldSpacing, FormElementType.CheckBox)
'            SetStyle(ControlStyles.Selectable, True)
'            MeType = FormElementType.CheckBox
'            Check = New PictureBox
'            TextLab = New Label
'            With Check
'                .Hide()
'                .Width = 16
'                .Height = 16
'                .BackColor = Color.White
'                .Left = 10
'                .Parent = Me
'                .Show()
'            End With
'            With TextLab
'                .Hide()
'                .Parent = Me
'                '.BackColor = Color.FromArgb(7, 70, 91)
'                .BackColor = Color.Transparent
'                .Padding = New Padding(34, 0, 0, 0)
'                .Left = 0
'                .Top = 14
'                .ForeColor = Color.White
'                .Text = "Meld meg på"
'                .FlatStyle = FlatStyle.Flat
'                .TextAlign = ContentAlignment.MiddleLeft
'                .Show()
'            End With

'            Value = False
'        End Sub
'        Protected Overrides Sub OnSizeChanged(e As EventArgs)
'            MyBase.OnSizeChanged(e)
'            With TextLab
'                .SuspendLayout()
'                .Top = Header.Height
'                .Height = Height - .Top
'                .Width = Width
'                .ResumeLayout()
'            End With
'            With Check
'                .Top = TextLab.Top + TextLab.Height \ 2 - .Height \ 2
'            End With
'        End Sub
'        Protected Overrides Sub OnVisibleChanged(e As EventArgs)
'            MyBase.OnVisibleChanged(e)
'            With TextLab
'                .SuspendLayout()
'                .Top = Header.Height
'                .Height = Height - .Top
'                .Width = Width
'                .ResumeLayout()
'            End With
'            With Check
'                .Top = TextLab.Top + TextLab.Height \ 2 - .Height \ 2
'            End With
'        End Sub
'        Protected Overrides Sub Dispose(disposing As Boolean)
'            TextLab.Dispose()
'            Check.Dispose()
'            CheckBrush.Dispose()
'            MyBase.Dispose(disposing)
'        End Sub
'    End Class

'    Private Class FormButton
'        Inherits FormField
'        Private TextLab As New Label
'        Public Sub New(ByVal FieldSpacing As Integer)
'            MyBase.New(FieldSpacing, FormElementType.Button)
'            MeType = FormElementType.Button
'        End Sub
'        Protected Overrides Sub OnSecondaryValueChanged(Value As Object)
'            MyBase.OnSecondaryValueChanged(Value)
'            TextLab.Text = DirectCast(Value, String)
'        End Sub
'        ' TODO: Handle OnHeaderVisibleChanged
'        Protected Overrides Sub Dispose(disposing As Boolean)
'            TextLab.Dispose()
'            MyBase.Dispose(disposing)
'        End Sub
'    End Class

'    Private Class FormTextField
'        Inherits FormField
'        Private PaddingLeft As Integer = 10
'        Private TextField As TextBox
'        Private Sub Me_BackColorChanged(Sender As Object, e As EventArgs) Handles Me.BackColorChanged
'            If TextField IsNot Nothing Then
'                TextField.BackColor = BackColor
'            End If
'            Debug.Print(Me.BackColor.B & " vs " & TextField.BackColor.B)
'        End Sub

'        Public Sub New(ByVal FieldSpacing As Integer)
'            MyBase.New(FieldSpacing, FormElementType.TextField)
'            MeType = FormElementType.TextField
'            TextField = New TextBox
'            With Me
'                .Hide()
'                '.BackColor = Color.FromArgb(7, 70, 91)
'                .ForeColor = Color.White
'            End With
'            With TextField
'                .Parent = Me
'                .Left = PaddingLeft
'                .BackColor = Color.FromArgb(7, 70, 91)
'                .Width = Width - PaddingLeft
'                .Height = 16
'                .Top = CInt((Height / 2) - (.Height / 2) + (Header.Height / 2))
'                .BackColor = Color.FromArgb(7, 70, 91)
'                .ForeColor = Color.White
'                .BorderStyle = BorderStyle.None
'                .Show()
'            End With
'            Show()
'        End Sub
'        Protected Overrides Sub OnSizeChanged(e As EventArgs)
'            MyBase.OnSizeChanged(e)
'            With TextField
'                .Width = Width - PaddingLeft
'                .Top = CInt((Height / 2) - (.Height / 2) + (Header.Height / 2))
'            End With
'        End Sub
'        Protected Overrides Sub OnVisibleChanged(e As EventArgs)
'            MyBase.OnVisibleChanged(e)
'            With TextField
'                .Width = Width - PaddingLeft
'                .Top = CInt((Height / 2) - (.Height / 2) + (Header.Height / 2))
'            End With
'        End Sub
'        Protected Overrides Sub OnValueChanged(Value As Object)
'            MyBase.OnValueChanged(Value)
'            TextField.Text = DirectCast(Value, String)
'        End Sub
'        Protected Overrides Sub OnSecondaryValueChanged(Value As Object)
'            MyBase.OnSecondaryValueChanged(Value)
'            TextField.Text = DirectCast(Value, String)
'        End Sub
'        Protected Overrides Sub Dispose(disposing As Boolean)
'            TextField.Dispose()
'            MyBase.Dispose(disposing)
'        End Sub
'    End Class

'    Private Class FormLabel
'        Inherits FormField
'        Private WithEvents TextLab As Label
'        Public Sub New(ByVal FieldSpacing As Integer)
'            MyBase.New(FieldSpacing, FormElementType.Label)
'            MeType = FormElementType.Label
'            'varIsInputType = False
'            TextLab = New Label
'            Hide()
'            With TextLab
'                .Parent = Me
'                '.BackColor = Color.FromArgb(7, 70, 91)
'                .BackColor = Color.Transparent
'                .ForeColor = Color.White
'                .AutoSize = False
'                .TextAlign = ContentAlignment.MiddleLeft
'                .Padding = New Padding(6, 0, 0, 0)
'                .Left = 0
'                .Top = Header.Height
'            End With
'            Value = "Change the default text using the Value property."
'            Show()
'        End Sub
'        Protected Overrides Sub OnValueChanged(Value As Object)
'            MyBase.OnValueChanged(Value)
'            TextLab.Text = DirectCast(Value, String)
'        End Sub
'        Protected Overrides Sub OnSecondaryValueChanged(Value As Object)
'            MyBase.OnSecondaryValueChanged(Value)
'            TextLab.Text = DirectCast(Value, String)
'        End Sub
'        Protected Overrides Sub OnSizeChanged(e As EventArgs)
'            MyBase.OnSizeChanged(e)
'            With TextLab
'                .SuspendLayout()
'                .Width = Width
'                .Height = Height - Header.Height
'                .Top = Header.Height
'                .ResumeLayout()
'            End With
'        End Sub
'        Protected Overrides Sub OnVisibleChanged(e As EventArgs)
'            MyBase.OnVisibleChanged(e)
'            With TextLab
'                .SuspendLayout()
'                .Width = Width
'                .Height = Height - Header.Height
'                .Top = Header.Height
'                .ResumeLayout()
'            End With
'        End Sub
'        Private Sub OnHeaderVisibleChanged() Handles Me.HeaderVisibleChanged
'            With TextLab
'                .SuspendLayout()
'                .Width = Width
'                .Height = Height - Header.Height
'                .Top = Header.Height
'                .ResumeLayout()
'            End With
'        End Sub
'        Protected Overrides Sub Dispose(disposing As Boolean)
'            TextLab.Dispose()
'            MyBase.Dispose(disposing)
'        End Sub
'    End Class

'    Private Class FormDropDown
'        Inherits FormField
'        Public Sub New(ByVal FieldSpacing As Integer)
'            MyBase.New(FieldSpacing, FormElementType.DropDown)
'            MeType = FormElementType.DropDown
'        End Sub
'        Protected Overrides Sub Dispose(disposing As Boolean)
'            MyBase.Dispose(disposing)
'        End Sub
'    End Class

'    Private Class FormRadioButton
'        Inherits FormField
'        Private WithEvents CheckSurface As Control
'        Private WithEvents TextLab As Label
'        Private varChecked As Boolean
'        Private CheckBrush As New SolidBrush(Color.White)
'        Private DotBrush As New SolidBrush(Color.Black)
'        Private HoverBrush As New SolidBrush(Color.LightGray)
'        Private Hovering As Boolean
'        Private varIndex As Integer
'        Private ReadOnly Property Checked As Boolean
'            Get
'                Return varChecked
'            End Get
'        End Property
'        Private Sub Check()
'            varChecked = True
'            CheckSurface.Invalidate()
'            OnValueChanged(varChecked)
'        End Sub
'        Public ReadOnly Property RadioIndex As Integer
'            Get
'                Return varIndex
'            End Get
'        End Property
'        Protected Overrides Sub OnUserClick()
'            MyBase.OnUserClick()
'            If Not varChecked Then
'                Check()
'            End If
'        End Sub
'        Public Sub New(ByVal RadioContextIndex As Integer, ByVal FieldSpacing As Integer)
'            MyBase.New(FieldSpacing, FormElementType.Radio)
'            MeType = FormElementType.Radio
'            CheckSurface = New PictureBox
'            TextLab = New Label
'            varIndex = RadioContextIndex
'            With CheckSurface
'                .Hide()
'                .Parent = Me
'                .Width = 16
'                .Height = 16
'                '.BackColor = Color.FromArgb(7, 70, 91)
'                .BackColor = Color.Transparent
'                .Left = 10
'                .Show()
'            End With
'            With TextLab
'                .Hide()
'                .Parent = Me
'                '.BackColor = Color.FromArgb(7, 70, 91)
'                .BackColor = Color.Transparent
'                .Padding = New Padding(34, 0, 0, 0)
'                .Left = 0
'                .Top = 14
'                .ForeColor = Color.White
'                .Text = "Velg meg"
'                .FlatStyle = FlatStyle.Flat
'                .TextAlign = ContentAlignment.MiddleLeft
'                .SendToBack()
'                .Show()
'            End With
'            Show()
'        End Sub
'        Private Sub CheckSurface_Paint(Sender As Object, e As PaintEventArgs) Handles CheckSurface.Paint
'            Dim DotRect As New Rectangle(0, 0, CheckSurface.Width - 1, CheckSurface.Height - 1)
'            e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
'            e.Graphics.FillEllipse(CheckBrush, DotRect)

'            If varChecked Then
'                DotRect.Inflate(-4, -4)
'                e.Graphics.FillEllipse(DotBrush, DotRect)
'            ElseIf Hovering Then
'                DotRect.Inflate(-4, -4)
'                e.Graphics.FillEllipse(HoverBrush, DotRect)
'            End If
'        End Sub
'        Private Sub CheckSurface_MouseEnter(Sender As Object, e As EventArgs) Handles CheckSurface.MouseEnter
'            Hovering = True
'            CheckSurface.Invalidate()
'        End Sub
'        Private Sub CheckSurface_MouseLeave(Sender As Object, e As EventArgs) Handles CheckSurface.MouseLeave
'            Hovering = False
'            CheckSurface.Invalidate()
'        End Sub
'        Private Sub CheckSurface_Click(Sender As Object, e As EventArgs) Handles CheckSurface.Click
'            If Hovering Then
'                varChecked = True
'            End If
'            CheckSurface.Invalidate()
'            OnValueChanged(varChecked)
'        End Sub
'        Protected Overrides Sub OnSizeChanged(e As EventArgs)
'            MyBase.OnSizeChanged(e)
'            With TextLab
'                .SuspendLayout()
'                .Height = Height - .Top
'                .Width = Width
'                .ResumeLayout()
'                .PerformLayout()
'            End With
'            With CheckSurface
'                .Top = (Height - Header.Height) \ 2 - (.Top \ 2) + Header.Height - 1
'            End With
'        End Sub
'        Protected Overrides Sub OnVisibleChanged(e As EventArgs)
'            MyBase.OnVisibleChanged(e)
'            With TextLab
'                .SuspendLayout()
'                .Height = Height - .Top
'                .Width = Width
'                .ResumeLayout()
'                .PerformLayout()
'            End With
'            'With CheckSurface
'            '    .Top = CInt((Height / 2) - (.Top / 2)) + TextLab.Top - 2
'            'End With
'            With CheckSurface
'                .Top = (Height - Header.Height) \ 2 - (.Height \ 2) + Header.Height - 1
'            End With
'        End Sub
'        Protected Overrides Sub OnValueChanged(Value As Object)
'            MyBase.OnValueChanged(Value)
'            varChecked = DirectCast(Value, Boolean)
'            CheckSurface.Invalidate()
'        End Sub
'        Protected Overrides Sub OnSecondaryValueChanged(Value As Object)
'            MyBase.OnSecondaryValueChanged(Value)
'            TextLab.Text = DirectCast(Value, String)
'        End Sub
'        Protected Overrides Sub Dispose(disposing As Boolean)
'            TextLab.Dispose()
'            CheckSurface.Dispose()
'            CheckBrush.Dispose()
'            DotBrush.Dispose()
'            HoverBrush.Dispose()
'            MyBase.Dispose(disposing)
'        End Sub
'    End Class

'    Private Class RadioButtonContext
'        Implements IDisposable

'        Private ControlList As List(Of FormRadioButton)
'        Public ReadOnly Property Count As Integer
'            Get
'                Return ControlList.Count
'            End Get
'        End Property
'        Public Sub New()
'            ControlList = New List(Of FormRadioButton)
'        End Sub
'        Public Sub Add(R As FormRadioButton)
'            ControlList.Add(R)
'            Dim Test As FormRadioButton = ControlList(ControlList.Count - 1)
'            AddHandler Test.ValueChanged, AddressOf OnValueChanged
'        End Sub
'        Private Sub OnValueChanged(Sender As FormField, Value As Object)
'            Dim SenderX As FormRadioButton = DirectCast(Sender, FormRadioButton)
'            If DirectCast(Value, Boolean) Then
'                Dim iLast As Integer = ControlList.Count - 1
'                For i As Integer = 0 To iLast
'                    If ControlList(i).RadioIndex <> SenderX.RadioIndex Then
'                        ControlList(i).Value = False
'                    End If
'                Next
'            End If
'        End Sub

'#Region "IDisposable Support"
'        Private disposedValue As Boolean ' To detect redundant calls

'        ' IDisposable
'        Protected Overridable Sub Dispose(disposing As Boolean)
'            If Not disposedValue Then
'                If disposing Then
'                    ' TODO: dispose managed state (managed objects).
'                End If
'                Dim iLast As Integer = ControlList.Count - 1
'                For i As Integer = 0 To iLast
'                    RemoveHandler ControlList(i).ValueChanged, AddressOf OnValueChanged
'                Next
'                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
'                ' TODO: set large fields to null.
'            End If
'            disposedValue = True
'        End Sub

'        ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
'        'Protected Overrides Sub Finalize()
'        '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
'        '    Dispose(False)
'        '    MyBase.Finalize()
'        'End Sub

'        ' This code added by Visual Basic to correctly implement the disposable pattern.
'        Public Sub Dispose() Implements IDisposable.Dispose
'            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
'            Dispose(True)
'            ' TODO: uncomment the following line if Finalize() is overridden above.
'            ' GC.SuppressFinalize(Me)
'        End Sub
'#End Region
'    End Class
'End Class

'Public Class FormNavigationButton
'    Inherits Control
'    Private ButtonType As FormNavigationButtonType
'    Private DrawRect, UpperRect As Rectangle
'    Private LowerBrush As New SolidBrush(Color.FromArgb(0, 50, 60))
'    Private UpperBrush As New SolidBrush(ColorHelper.Add(Color.FromArgb(0, 50, 60), 20))
'    Private ArrowBrush As New SolidBrush(ColorHelper.Add(Color.FromArgb(0, 50, 60), 100))
'    Private ArrowFont As New Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold)
'    Private ArrowPoints() As Point
'    Public Sub New(ByVal NavButtonType As FormNavigationButtonType)
'        ButtonType = NavButtonType
'        DrawRect = New Rectangle(New Point(0, 0), New Size(0, 0))
'        UpperRect = New Rectangle(New Point(0, 0), New Size(0, 0))
'    End Sub
'    Private Sub RefreshGDI() Handles Me.Resize, Me.VisibleChanged
'        With DrawRect
'            .Width = Width - 1
'            .Height = Height - 1
'        End With
'        Select Case ButtonType
'            Case FormNavigationButtonType.PreviousButton
'                Dim FirstPoint As New Point(DrawRect.Width \ 2 - 5, DrawRect.Height \ 2)
'                Dim SecondPoint As New Point(DrawRect.Width \ 2 + 2, DrawRect.Height \ 2 - 7)
'                Dim ThirdPoint As New Point(DrawRect.Width \ 2 + 2, DrawRect.Height \ 2 + 7)
'                ArrowPoints = {FirstPoint, SecondPoint, ThirdPoint}
'            Case Else
'                Dim FirstPoint As New Point(DrawRect.Width \ 2 + 5, DrawRect.Height \ 2)
'                Dim SecondPoint As New Point(DrawRect.Width \ 2 - 2, DrawRect.Height \ 2 - 7)
'                Dim ThirdPoint As New Point(DrawRect.Width \ 2 - 2, DrawRect.Height \ 2 + 7)
'                ArrowPoints = {FirstPoint, SecondPoint, ThirdPoint}
'        End Select
'        With UpperRect
'            .Width = Width - 1
'            .Height = Height \ 2
'        End With
'    End Sub
'    Public Enum FormNavigationButtonType
'        NextButton
'        PreviousButton
'    End Enum
'    Protected Overrides Sub OnPaint(e As PaintEventArgs)
'        MyBase.OnPaint(e)
'        With e.Graphics
'            .FillRectangle(LowerBrush, DrawRect)
'            .FillRectangle(UpperBrush, UpperRect)
'            .FillPolygon(ArrowBrush, ArrowPoints)
'        End With
'    End Sub
'End Class

'Public Class QuestionnaireResult
'    Private ResultsArr() As FlatFormResult
'    Protected Friend Sub New(ByVal Results() As FlatFormResult)
'        ResultsArr = Results
'    End Sub
'    Public Function Value(ByVal Form As Integer, ByVal Row As Integer, ByVal Field As Integer) As Object
'        Return ResultsArr(Form).Value(Row, Field)
'    End Function
'    Public Function Header(ByVal Form As Integer, ByVal Row As Integer, ByVal Field As Integer) As String
'        Return ResultsArr(Form).Header(Row, Field)
'    End Function
'    Public Function FieldType(ByVal Form As Integer, ByVal Row As Integer, ByVal Field As Integer) As FormElementType
'        Return ResultsArr(Form).FieldType(Row, Field)
'    End Function
'End Class

'Public Class Questionnaire
'    Inherits Control
'    Private Class FormNavigation
'        Inherits Label
'        Private NextButton, PrevButton As FormNavigationButton
'        Private BtnSpacing As Integer
'        Public Event NextClicked()
'        Public Event PreviousClicked()
'        Protected Overrides Sub OnResize(e As EventArgs)
'            MyBase.OnResize(e)
'            'If PrevButton IsNot Nothing Then
'            SuspendLayout()
'            With PrevButton
'                .Left = (Width - .Width - BtnSpacing) \ 2
'                .Top = 0
'            End With
'            With NextButton
'                .Left = (Width - .Width + BtnSpacing) \ 2
'                .Top = 0
'            End With
'            ResumeLayout()
'            'End If
'        End Sub
'        Protected Overrides Sub OnVisibleChanged(e As EventArgs)
'            MyBase.OnVisibleChanged(e)
'            'If PrevButton IsNot Nothing Then
'            SuspendLayout()
'            With PrevButton
'                .Left = (Width - .Width - BtnSpacing) \ 2
'            End With
'            With NextButton
'                .Left = (Width - .Width + BtnSpacing) \ 2
'            End With
'            ResumeLayout()
'            'End If
'        End Sub

'        Public Sub New(ParentQuestionnaire As Questionnaire, ByVal Height As Integer, ByVal ButtonSpacing As Integer)
'            SuspendLayout()
'            BtnSpacing = ButtonSpacing
'            PrevButton = New FormNavigationButton(FormNavigationButton.FormNavigationButtonType.PreviousButton)
'            NextButton = New FormNavigationButton(FormNavigationButton.FormNavigationButtonType.NextButton)
'            AddHandler PrevButton.Click, AddressOf OnPreviousClicked
'            AddHandler NextButton.Click, AddressOf OnNextClicked
'            With Me
'                .Hide()
'                .Parent = ParentQuestionnaire
'                .Left = 0
'                .Top = 0
'                .Height = Height 'endre til = Height
'                .Width = ParentQuestionnaire.Width
'                .BringToFront()
'            End With
'            With PrevButton
'                .Parent = Me
'                .Width = 30
'                .Height = 30
'                .Top = 0
'            End With
'            With NextButton
'                .Parent = Me
'                .Width = 30
'                .Height = 30
'                .Top = 0
'            End With
'            ResumeLayout()
'            With Me
'                .Show()
'            End With
'        End Sub
'        Private Sub OnNextClicked(sender As Object, e As EventArgs)
'            RaiseEvent NextClicked()
'        End Sub
'        Private Sub OnPreviousClicked(sender As Object, e As EventArgs)
'            RaiseEvent PreviousClicked()
'        End Sub
'    End Class
'    Private FormList As List(Of TestFlatForm)
'    Private FormNav As FormNavigation
'    Private varFormIndex As Integer = -1
'    Private PanTimer As Timers.Timer
'    Public Function Result() As QuestionnaireResult
'        Dim iLast As Integer = FormList.Count - 1
'        Dim ResultArr(iLast) As FlatFormResult
'        For i As Integer = 0 To FormList.Count - 1
'            ResultArr(i) = FormList(i).Result
'        Next
'        Return New QuestionnaireResult(ResultArr)
'    End Function
'    Public Overloads ReadOnly Property Forms(ByVal Index As Integer) As TestFlatForm
'        Get
'            Return FormList(Index)
'        End Get
'    End Property
'    Public Overloads ReadOnly Property Forms As List(Of TestFlatForm)
'        Get
'            Return FormList
'        End Get
'    End Property
'    Public Property FormIndex As Integer
'        Get
'            Return varFormIndex
'        End Get
'        Set(value As Integer)
'            ' Change to .FadeOut/.SlideLeftOut
'            If varFormIndex >= 0 Then
'                FormList(varFormIndex).Hide()
'            End If
'            varFormIndex = value
'            With FormList(varFormIndex)
'                .Display()
'                Height = .Height + FormNav.Height + 10
'            End With
'        End Set
'    End Property
'    Public Sub New(Optional ByVal NavigationHeight As Integer = 40, Optional ByVal NextPreviousButtonSpacing As Integer = 100)
'        Initialize(NavigationHeight, NextPreviousButtonSpacing)
'    End Sub
'    Public Sub New(Parent As Control, Optional ByVal NavigationHeight As Integer = 40, Optional ByVal NextPreviousButtonSpacing As Integer = 100)
'        Me.Parent = Parent
'        Initialize(NavigationHeight, NextPreviousButtonSpacing)
'    End Sub
'    Public Sub New(ByVal Rectangle As Rectangle, Optional ByVal NavigationHeight As Integer = 40, Optional ByVal NextPreviousButtonSpacing As Integer = 100)
'        ' Study before deciding to correct:
'        PanTimer.AutoReset = False
'        With Rectangle
'            Left = .Left
'            Top = .Top
'            Width = .Width
'            Height = .Height
'        End With
'        Initialize(NavigationHeight, NextPreviousButtonSpacing)
'    End Sub
'    Public Sub New(ByVal Width As Integer, ByVal Height As Integer, Optional ByVal NavigationHeight As Integer = 40, Optional ByVal NextPreviousButtonSpacing As Integer = 100)
'        With Me
'            .Width = Width
'            .Height = Height
'        End With
'        Initialize(NavigationHeight, NextPreviousButtonSpacing)
'    End Sub
'    Public Sub Display(Optional ByVal StartFormIndex As Integer = 0)
'        FormIndex = StartFormIndex
'        If FormNav IsNot Nothing Then
'            With FormNav
'                .SuspendLayout()
'                .Top = Height - .Height
'                .Width = Width
'                .ResumeLayout()
'            End With
'        End If
'        Show()
'    End Sub
'    Private Sub Initialize(ByVal NavHeight As Integer, ByVal NavBtnSpacing As Integer)
'        FormList = New List(Of TestFlatForm)
'        Hide()
'        PanTimer = New Timers.Timer(1000 / 60)
'        FormNav = New FormNavigation(Me, NavHeight, NavBtnSpacing)
'        AddHandler FormNav.NextClicked, AddressOf OnNextClicked
'        AddHandler FormNav.PreviousClicked, AddressOf OnPreviousClicked
'    End Sub
'    Private Sub OnNextClicked()
'        ShowNext()
'    End Sub
'    Private Sub OnPreviousClicked()
'        ShowPrevious()
'    End Sub
'    Public Sub Add(Form As TestFlatForm)
'        With Form
'            .BackColor = Color.Green
'            .Parent = Me
'            .Left = CInt((Width / 2) - (.Width / 2))
'            .Top = 0
'        End With
'        FormList.Add(Form)
'        If varFormIndex = -1 Then
'            varFormIndex = 0
'        End If
'    End Sub
'    Protected Overrides Sub OnResize(e As EventArgs)
'        MyBase.OnResize(e)
'        If FormNav IsNot Nothing Then
'            With FormNav
'                .SuspendLayout()
'                .Top = Height - .Height
'                .Width = Width
'                .ResumeLayout()
'                Beep()
'            End With
'        End If
'        ' Change to if list isnot nothing (if buggy)
'        If varFormIndex >= 0 Then
'            With FormList(varFormIndex)
'                .SuspendLayout()
'                .Left = Width \ 2 - .Width \ 2
'                .ResumeLayout()
'            End With
'        End If
'    End Sub
'    Protected Overrides Sub OnVisibleChanged(e As EventArgs)
'        MyBase.OnVisibleChanged(e)
'        If FormNav IsNot Nothing Then
'            With FormNav
'                .SuspendLayout()
'                .Top = Height - .Height
'                .Width = Width
'                .ResumeLayout()
'            End With
'        End If
'        ' Change to if list isnot nothing (if buggy)
'        If varFormIndex >= 0 Then
'            With FormList(varFormIndex)
'                .SuspendLayout()
'                .Left = Width \ 2 - .Width \ 2
'                .ResumeLayout()
'            End With
'        End If
'    End Sub
'    Public Sub NewForm(ByVal Width As Integer, ByVal Height As Integer, ByVal FieldSpacing As Integer)
'        Dim NF As New TestFlatForm(Width, Height, FieldSpacing)
'        FormList.Add(NF)
'    End Sub
'    Public Sub ShowNext()
'        If varFormIndex < FormList.Count - 1 Then
'            FormIndex += 1
'        End If
'    End Sub
'    Public Sub ShowPrevious()
'        If varFormIndex > 0 Then
'            FormIndex -= 1
'        End If
'    End Sub
'End Class