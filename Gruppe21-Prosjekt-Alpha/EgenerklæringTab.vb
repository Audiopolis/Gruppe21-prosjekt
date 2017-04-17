Imports System.Text
Imports AudiopoLib

Public Class DoubleBufferedPanel
    Inherits Panel
    Public Sub New()
        DoubleBuffered = True
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        UpdateStyles()
    End Sub
End Class

Public Class EgenerklæringTab
    Inherits Tab
    Private Questionnaire As New Questionnaire(, 120, -15)
    Private WithEvents DBC As New DatabaseClient(Credentials.Server, Credentials.Database, Credentials.UserID, Credentials.Password)
    Private LayoutTool As New FormLayoutTools(Me)
    Private TopBar As New TopBar(Me)
    Private Footer As New Footer(Me, Color.FromArgb(54, 68, 78), 40)
    Private Forms() As FlatForm = {New FlatForm(500, 500, 3, FormFieldStylePresets.PlainWhite), New FlatForm(500, 500, 3, FormFieldStylePresets.PlainWhite), New FlatForm(500, 500, 3, FormFieldStylePresets.PlainWhite), New FlatForm(500, 500, 3, FormFieldStylePresets.PlainWhite), New FlatForm(500, 500, 3, FormFieldStylePresets.PlainWhite), New FlatForm(500, 500, 3, FormFieldStylePresets.PlainWhite), New FlatForm(500, 500, 3, FormFieldStylePresets.PlainWhite), New FlatForm(500, 500, 3, FormFieldStylePresets.PlainWhite), New FlatForm(500, 500, 3, FormFieldStylePresets.PlainWhite), New FlatForm(500, 500, 3, FormFieldStylePresets.PlainWhite), New FlatForm(500, 500, 3, FormFieldStylePresets.PlainWhite)}
    Public Sub InitiateForm()
        With Questionnaire
            If CurrentLogin.IsMale Then
                .Add(Forms(6))
            Else
                .Add(Forms(7))
            End If
            .Add(Forms(8))
            .Add(Forms(9))
            .Display(0)
        End With
    End Sub
    Public Sub New(ParentWindow As MultiTabWindow)
        MyBase.New(ParentWindow)
        Dim LabelSize As Integer = 360
        Dim RadioSize As Integer = (500 - LabelSize) \ 2 - 3
#Region "Form 0"
        With Forms(0)
            .AddField(FormElementType.Label)
            With .Last
                .Header.Text = "Vennligst besvar"
                .Height = .Header.Bottom + 1
            End With
            With .LastRow
                .Height = 24
            End With
            .NewRowHeight = 40
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "Har du fått informasjon om blodgiving?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "Føler du deg frisk nå?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "Hvis du har gitt blod tidligere, har du vært frisk i perioden fra forrige bldgivning til nå?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "Veier du 50 kg eller mer?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "Har du åpne sår, eksem eller hudsykdom?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "Har du piercing i slimhinne?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
        End With
#End Region

#Region "Form 1"
        With Forms(1)
            .AddField(FormElementType.Label)
            With .Last
                .Header.Text = "Har du i løpet av de siste fire uker..."
                .Height = .Header.Bottom + 1
            End With
            With .LastRow
                .Height = 24
            End With
            .NewRowHeight = 40
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "brukt medisiner?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "vært syk eller hatt feber?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "hatt løs avføring?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "fått vaksine?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "vært hos tannlege eller tannpleier?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
        End With
#End Region

#Region "Form 2"
        With Forms(2)
            .AddField(FormElementType.Label)
            With .Last
                .Header.Text = "Har du i løpet av de siste seks måneder..."
                .Height = .Header.Bottom + 1
            End With
            With .LastRow
                .Height = 24
            End With
            .NewRowHeight = 50
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "vært til legeundersøkelse eller på sykehus, eller fått behandling for noen sykdom?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "fått behandling med sprøyter?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "hatt kjønnssykdom, eller fått behandling for kjønnssykdom?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "hatt seksuell kontakt med person med HIV, hepatitt B eller hepatitt C, eller en person som har testet positivt for en av disse sykdommene?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "hatt seksuell kontakt med person som bruker eller har brukt dopingmidler eller narkotiske midler som sprøyter?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "hatt seksuell kontakt med prostituerte eller tidligere prostituerte?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "blitt tatovert, fått piercing eller tatt hull i ørene?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "fått akupunktur?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
        End With
#End Region

#Region "Form 3"
        With Forms(3)
            .AddField(FormElementType.Label)
            With .Last
                .Header.Text = "Har du i løpet av de siste seks måneder..."
                .Height = .Header.Bottom + 1
            End With
            With .LastRow
                .Height = 24
            End With
            .NewRowHeight = 40
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "stukket eller skåret deg på gjenstander som var forurenset med blod eller kroppsvæsker?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "bodd i samme husstand som en person som har hepatitt B?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "fått blodsøl på slimhinner eller skadet hud?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "blitt bitt av flått?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "hatt seksualpartner som har bodd mer enn ett år sammenhengende utenfor Vest-Europa?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "hatt seksualpartner som har vært i Afrika?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "hatt seksuell kontakt med en person som har fått blod eller blodprodukter utenfor Norden?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "hatt ny seksualpartner?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "vært utenfor Vest-Europa?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
        End With
#End Region

#Region "Form 4"
        With Forms(4)
            .AddField(FormElementType.Label)
            With .Last
                .Header.Text = "Har du i løpet av de siste to år..."
                .Height = .Header.Bottom + 1
            End With
            With .LastRow
                .Height = 24
            End With
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "hatt sjeldne eller alvorlige infeksjonssykdommer?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label)
            With .Last
                .Header.Text = "Har du på noe tidspunkt i livet..."
                .Height = .Header.Bottom + 1
            End With
            With .LastRow
                .Height = 24
            End With
            .NewRowHeight -= 2
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "hatt, hjerte-, lever-, eller lungesykdom?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "hatt kreft?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "hatt blødningstendens?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "hatt allergi mot mat eller medisiner?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "hatt malaria?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "hatt tropesykdommer?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
        End With
#End Region

#Region "Form 5"
        With Forms(5)
            .AddField(FormElementType.Label)
            With .Last
                .Header.Text = "Har du på noe tidspunkt i livet..."
                .Height = .Header.Bottom + 1
            End With
            With .LastRow
                .Height = 24
            End With
            .NewRowHeight = 50
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "hatt hepatitt (gulsott), HIV-infeksjon eller AIDS?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "fått blodoverføring?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "fått veksthormon?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "fått hornhinnetransplantasjon?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "hatt syfilis?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "hatt alvorlig sykdom som ikke er nevnt her?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "brukt dopingmidler eller narkotiske midler som sprøyter?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "mottat penger eller narkotika som gjenytelse for sex?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
        End With
#End Region

#Region "Form 6"
        With Forms(6)
            'If CurrentLogin.IsMale Then
            .AddField(FormElementType.Label)
            With .Last
                .Header.Text = "Vennligst besvar"
                .Height = .Header.Bottom + 1
            End With
            With .LastRow
                .Height = 24
            End With
            .AddField(FormElementType.Label)
            With .Last
                .SwitchHeader(False)
                .Value = "Du er registrert som mann i våre systemer, og får derfor disse spørsmålene rettet mot menn. Hvis dette er en feil, vennligst oppdater personopplysningene dine."
            End With
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "Har eller har du hatt seksull kontakt med andre menn?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
        End With
#End Region

#Region "Form 7"
        With Forms(7)
            'Else
            .AddField(FormElementType.Label)
            With .Last
                .Header.Text = "Vennligst besvar"
                .Height = .Header.Bottom + 1
            End With
            With .LastRow
                .Height = 24
            End With
            .AddField(FormElementType.Label)
            With .Last
                .SwitchHeader(False)
                .Value = "Du er registrert som kvinne i våre systemer, og får derfor disse spørsmålene rettet mot menn. Hvis dette er en feil, vennligst oppdater personopplysningene dine."
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "Er du gravid?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "Har du vært gravid i løpet av de siste tolv måneder, eller ammer du nå?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "Hvis du har gitt blod tidligere, har du vært gravid siden forrige blodgivning?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "Har du i løpet av de siste seks måneder hatt seksuell kontakt med en mann som du vet har hatt seksuell kontakt med andre menn?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            'End If
        End With
#End Region

#Region "Form 8"
        With Forms(8)
            .AddField(FormElementType.Label)
            With .Last
                .Header.Text = "Besvar også (annet)"
                .Height = .Header.Bottom + 1
            End With
            With .LastRow
                .Height = 24
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "Har du brukt narkotika en eller flere ganger de siste 12 måneder?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "Har du eller noen i familien hatt Creutzfeldt-Jakob sykdom eller variant CJD?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "Har du oppholdt deg i Storbritannia i mer enn ett år til sammen i perioden mellom 1980 og 1996?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "Har du i løpet av de siste tre år vært i områder der malaria forekommer?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "Har du oppholdt deg sammenhengende i minst seksmåneder i områder der malaria forekommer?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "Har du oppholdt deg i Afrika i mer enn fem år til sammen?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "Er du eller din mor født i Amerika sør for USA?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
        End With
#End Region

#Region "Form 9"
        With Forms(9)
            .AddField(FormElementType.Label)
            With .Last
                .Header.Text = "Vennligst besvar (annet)"
                .Height = .Header.Bottom + 1
            End With
            With .LastRow
                .Height = 24
            End With
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "Godtar du at anonymiserte prøver av ditt blod kan brukes til forskning? Du er like velkommen som blodgiver enten du svarer ja eller nei. Blodbanken kan gi informasjon om aktuelle forskningsprosjekter."
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "Har du deltatt i medikamentforsøk de siste 12 måneder?"
            End With
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With

            .AddField(FormElementType.Label, LabelSize)
            With .Last
                .SwitchHeader(False)
                .Value = "Jeg samtykker i at mitt plasma føres ut av Norge for legemiddelproduksjon."
            End With
            .AddRadioContext(True)
            .AddField(FormElementType.Radio, RadioSize)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Ja"
            End With
            .AddField(FormElementType.Radio)
            With .Last
                .SwitchHeader(False)
                .SecondaryValue = "Nei"
            End With
            .AddField(FormElementType.TextField)
            With .Last
                .Header.Text = "Jeg er født i..."
            End With
        End With
#End Region
        With Questionnaire
            .Parent = Me
            .Add(Forms(0))
            .Add(Forms(1))
            .Add(Forms(2))
            .Add(Forms(3))
            .Add(Forms(4))
            .Add(Forms(5))
            Dim FinishedButton As New TopBarButton(Me, My.Resources.OKIconHvit, "Send inn", New Size(135, 36), False, 100)
            FinishedButton.BackColor = Color.LimeGreen
            FinishedButton.ForeColor = Color.White
            .AddCustomButtons(New TopBarButton(Me, My.Resources.ForrigeIcon, "Forrige", New Size(135, 36), False, 100), New TopBarButton(Me, My.Resources.NesteIcon, "Neste", New Size(135, 36), False, 100), FinishedButton)
            .Size = New Size(540, 550)
            .BackColor = Color.FromArgb(220, 220, 220)
            AddHandler .FinishedClicked, AddressOf OnFinishedClicked
        End With
        With TopBar
            .AddButton(My.Resources.HjemIcon, "Hjem", New Size(135, 36))
            .AddLogout("Logg ut", New Size(135, 36))
        End With
        With DBC
            .SQLQuery = "INSERT INTO Egenerklæring (time_id, svar_array, land) VALUES (@timeid, @series, @country);"
        End With
    End Sub
    Private Sub OnFinishedClicked()
        Dim Series As String = GetRadioString()
        If Series IsNot Nothing Then
            MsgBox("Executing")
            With DBC
                .Execute({"@timeid", "@series", "@country"}, {"1", Series, DirectCast(Questionnaire.Form(8).Last.Value, String)})
            End With
        Else
            MsgBox("Feil")
        End If
    End Sub
    Private Function GetRadioString() As String
        Dim SB As New StringBuilder
        Dim AllFilledOut As Boolean = True
        For Each F As FlatForm In Questionnaire.Forms
            SB.Append(F.GetRadioSeries)
        Next
        Dim FullString As String = SB.ToString
        Dim CharArr() As Char = FullString.ToCharArray
        For Each C As Char In CharArr
            If C.ToString = "X" Then
                AllFilledOut = False
                Exit For
            End If
        Next
        If Not AllFilledOut Then
            Return Nothing
        Else
            For Each C As Char In CharArr
                If C.ToString = "0" Then
                    C = "1".ToCharArray()(0)
                Else
                    C = "0".ToCharArray()(0)
                End If
            Next
            Return New String(CharArr)
        End If
    End Function
    Protected Overrides Sub OnResize(e As EventArgs)
        SuspendLayout()
        MyBase.OnResize(e)
        If LayoutTool IsNot Nothing Then
            ' TODO: Remove layouttool
            With Questionnaire
                .Location = New Point(20, (Height + TopBar.Bottom - Footer.Height) \ 2 - .Height \ 2)
            End With
        End If
        ResumeLayout(True)
    End Sub
End Class
