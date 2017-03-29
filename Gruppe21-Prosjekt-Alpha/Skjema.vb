Imports System.ComponentModel
Imports AudiopoLib

Public Class Skjema
    Dim Spørreskjema As Questionnaire
    Dim LayoutTool As FormLayoutTools
    Dim boolLoaded As Boolean = False
    Private Sub Skjema_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not boolLoaded Then
            LayoutTool = New FormLayoutTools(Me)
            ByggSkjema()
            LayoutTool.CenterSurfaceH(Spørreskjema, Me)
            boolLoaded = True
        End If
    End Sub
    Protected Overrides Sub OnClosing(e As CancelEventArgs)
        Hide()
        If Testdashbord IsNot Nothing Then
            Testdashbord.Show()
            e.Cancel = True
        End If
        MyBase.OnClosing(e)
    End Sub
    Protected Overrides Sub OnSizeChanged(e As EventArgs)
        MyBase.OnSizeChanged(e)
        If boolLoaded Then
            LayoutTool.CenterSurfaceH(Spørreskjema, Me)
        End If
    End Sub
    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        MyBase.OnLoad(New EventArgs)
        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Private Sub ByggSkjema()
        Dim TestForm1 As New FlatForm(400, 300, 10)
        With TestForm1
            .SuspendLayout()
            .AddField(FormElementType.CheckBox, 120)
            .AddField(FormElementType.Label)
            .AddField(FormElementType.CheckBox, 120)
            .AddField(FormElementType.CheckBox, 160)
            .AddField(FormElementType.CheckBox)
            .AddField(FormElementType.TextField)
            .AddField(FormElementType.Radio, 120, True)
            .AddField(FormElementType.Radio)
            .AddRadioContext()
            .AddField(FormElementType.Radio, 120, True)
            .AddField(FormElementType.Radio)
            .Field(1, 2).Extrude(FieldExtrudeSide.Left, 10)
            .Field(0, 1).Value = "Dette er et eksempel på både vertikal og horisontal sammensmelting." & vbNewLine & " "
            .Field(1, 2).SecondaryValue = "Test"
            .MergeWithAbove(1, 1)
            .MergeWithAbove(1, 2, -1)
            With .Field(2, 0)
                .Value = "Hei"
                .Header.Text = "Har du noen ekstra kommentarer?"
            End With
            .ResumeLayout()
            '.HeightToContent()
        End With
        Dim TestForm2 As New FlatForm(400, 300, 10)
        With TestForm2
            .SuspendLayout()
            .AddField(FormElementType.Label, 260)
            .AddField(FormElementType.CheckBox)
            .AddField(FormElementType.CheckBox, 100)
            .AddField(FormElementType.CheckBox, 150)
            .AddField(FormElementType.Radio)
            .AddField(FormElementType.Label)
            .AddField(FormElementType.Label)
            .AddField(FormElementType.Radio)
            .AddField(FormElementType.Radio, 120)
            .AddField(FormElementType.Radio)
            .Field(1, 1).Extrude(FieldExtrudeSide.Left, 10)
            .Field(0, 0).Value = "Vi kan lage helt vilkårlige skjema med stor stilistisk frihet." & vbNewLine & " "
            With .Field(0, 1)
                .Header.BackColor = Color.DeepPink
                .Header.Text = "Eksempel"
                .Header.ForeColor = ColorHelper.Multiply(.Header.ForeColor, 0.4)
                .SecondaryValue = "Stilistisk frihet"
                .BackColor = Color.HotPink
                .Value = True
            End With
            .MergeWithAbove(1, 0)
            .MergeWithAbove(1, 1, -1)
            With .Field(2, 0)
                .Value = "Ayy"
                .Header.Text = "Faktisk skjema kommer snart"
            End With
            .ResumeLayout()
            '.HeightToContent()
        End With

        Spørreskjema = New Questionnaire(Me)
        PaintMessageHelper.SuspendDrawing(Spørreskjema)
        With Spørreskjema
            .Width = 500
            .Height = 500
            .Top = 0
            .Add(TestForm1)
            .Add(TestForm2)
            .Display()
        End With
    End Sub
End Class