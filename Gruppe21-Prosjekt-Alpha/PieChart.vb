'Public Class PieChart
'    Inherits Control
'    Private DrawRect As New Rectangle(Point.Empty, New Size(Width - 1, Height - 1))
'    Private BGBrush As New SolidBrush(Color.Red)
'    Private SectorList As New List(Of CircleSector)
'    Public Sub New()

'    End Sub
'    Public Shadows Property Size As Size
'        Get
'            Return MyBase.Size
'        End Get
'        Set(value As Size)
'            MyBase.Size = New Size(value.Width, value.Width)
'        End Set
'    End Property
'    Public Shadows Property Width As Integer
'        Get
'            Return MyBase.Width
'        End Get
'        Set(value As Integer)
'            MyBase.Size = New Size(value, value)
'        End Set
'    End Property
'    Public Shadows Property Height As Integer
'        Get
'            Return MyBase.Height
'        End Get
'        Set(value As Integer)
'            MyBase.Size = New Size(value, value)
'        End Set
'    End Property
'    Protected Overrides Sub OnSizeChanged(e As EventArgs)
'        MyBase.OnSizeChanged(e)
'        DrawRect = New Rectangle(Point.Empty, New Size(Width - 1, Height - 1))
'    End Sub
'    Protected Overrides Sub OnPaint(e As PaintEventArgs)
'        MyBase.OnPaint(e)
'        With e.Graphics
'            .FillEllipse(BGBrush, DrawRect)
'        End With
'    End Sub
'    Private Class CircleSector
'        Inherits Control
'        Public Value As Integer
'        Public Label As String
'        Public SectorBrush As SolidBrush
'        Public Shared Rnd As New Random(Date.Now.Millisecond)
'        Public Sub New(ByVal BackColor As Color)
'            SectorBrush = New SolidBrush(BackColor)
'        End Sub
'        Public Sub New()
'            SectorBrush = New SolidBrush(Color.FromArgb(Rnd.Next(0, 256), Rnd.Next(0, 256), Rnd.Next(0, 256)))
'        End Sub
'        Public Property BackColor As Color
'            Get
'                Return SectorBrush.Color
'            End Get
'            Set(value As Color)
'                SectorBrush.Color = value
'            End Set
'        End Property
'    End Class
'End Class
