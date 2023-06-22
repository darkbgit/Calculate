using CalculateVessels.Core.Base;
using CalculateVessels.Core.Enums;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Supports.Saddle;
using CalculateVessels.Data.Enums;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Data.Properties;
using CalculateVessels.Forms.MiddleForms;
using CalculateVessels.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace CalculateVessels.Forms;

//[TypeDescriptionProvider(typeof(AbstractControlDescriptionProvider<BaseCalculateForm<SaddleInput>, Form, SaddleInput>))]
public sealed partial class SaddleForm : SaddleFormMiddle
{
    public SaddleForm(IEnumerable<ICalculateService<SaddleInput>> calculateServices,
        IPhysicalDataService physicalDataService)
        : base(calculateServices, physicalDataService)
    {
        InitializeComponent();
    }


    protected override string GetServiceName()
    {
        return Gost_cb.Text;
    }

    protected override void LoadInputData()
    {
        if (InputData == null) return;

        loadingConditionSaddleControl.SetLoadingCondition(new LoadingConditionSaddle
        {
            p = InputData.p,
            t = InputData.t,
            IsPressureIn = InputData.IsPressureIn,
            IsAssembly = InputData.IsAssembly,
            N = InputData.N,
            G = InputData.G,
            SigmaAllow = InputData.SigmaAllow,
            EAllow = InputData.E
        });


        steel_cb.Text = InputData.Steel;
        name_tb.Text = InputData.Name;
        D_tb.Text = InputData.D.ToString(CultureInfo.CurrentCulture);
        s_tb.Text = InputData.s.ToString(CultureInfo.CurrentCulture);
        c_tb.Text = InputData.c.ToString(CultureInfo.CurrentCulture);
        fi_tb.Text = InputData.fi.ToString(CultureInfo.CurrentCulture);
        b_tb.Text = InputData.b.ToString(CultureInfo.CurrentCulture);
        delta1_tb.Text = InputData.delta1.ToString(CultureInfo.CurrentCulture);
        a_tb.Text = InputData.a.ToString(CultureInfo.CurrentCulture);
        H_tb.Text = InputData.H.ToString(CultureInfo.CurrentCulture);
        L_tb.Text = InputData.L.ToString(CultureInfo.CurrentCulture);
        e_tb.Text = InputData.e.ToString(CultureInfo.CurrentCulture);



        var reinforcementShell = shellReinforcement_gb.Controls
            .OfType<RadioButton>()
            .FirstOrDefault(rb => rb.Checked)
            ?.Name;

        switch (InputData.SaddleType)
        {
            case SaddleType.SaddleWithoutRingWithSheet:
                s2_tb.Text = InputData.s2.ToString(CultureInfo.CurrentCulture);
                b2_tb.Text = InputData.b2.ToString(CultureInfo.CurrentCulture);
                delta2_tb.Text = InputData.delta2.ToString(CultureInfo.CurrentCulture);
                break;
            case SaddleType.SaddleWithRingInNoRibs:
                ring_rb.Checked = true;
                break;
            case SaddleType.SaddleWithRingIn1Rib:
                ring_rb.Checked = true;
                spacerRibRadioButton_1.Checked = true;
                break;
            case SaddleType.SaddleWithRingIn3Rib:
                ring_rb.Checked = true;
                spacerRibsRadioButton_3.Checked = true;
                break;
            case SaddleType.SaddleWithRingOutRib:
                ring_rb.Checked = true;
                out_rb.Checked = true;
                break;
        }
    }

    protected override bool CollectDataForPreliminarilyCalculation()
    {
        var dataInErr = new List<string>();

        var loadingCondition = loadingConditionSaddleControl.GetLoadingCondition();

        if (loadingCondition == null)
        {
            return false;
        }

        InputData = new SaddleInput
        {
            Steel = steel_cb.Text,
            D = Parameters.GetParam<double>(D_tb.Text, "D", dataInErr),
            s = Parameters.GetParam<double>(s_tb.Text, "s", dataInErr),
            c = Parameters.GetParam<double>(c_tb.Text, "c", dataInErr),
            fi = Parameters.GetParam<double>(fi_tb.Text, "φ", dataInErr),
            b = Parameters.GetParam<double>(b_tb.Text, "b", dataInErr),
            delta1 = Parameters.GetParam<double>(delta1_tb.Text, "delta1", dataInErr),
            a = Parameters.GetParam<double>(a_tb.Text, "a", dataInErr),
            H = Parameters.GetParam<double>(H_tb.Text, "H", dataInErr),
            L = Parameters.GetParam<double>(L_tb.Text, "L", dataInErr),
            e = Parameters.GetParam<double>(e_tb.Text, "e", dataInErr),

            p = loadingCondition.p,
            t = loadingCondition.t,
            IsPressureIn = loadingCondition.IsPressureIn,
            N = loadingCondition.N,
            G = loadingCondition.G,
            IsAssembly = loadingCondition.IsAssembly
        };


        var reinforcementShell = shellReinforcement_gb.Controls
            .OfType<RadioButton>()
            .FirstOrDefault(rb => rb.Checked)
            ?.Name;

        switch (reinforcementShell)
        {
            case nameof(nothing_rb):
                InputData.SaddleType = SaddleType.SaddleWithoutRingWithoutSheet;
                break;
            case nameof(sheet_rb):
                InputData.SaddleType = SaddleType.SaddleWithoutRingWithSheet;

                InputData.s2 = Parameters.GetParam<double>(s2_tb.Text, "s2", dataInErr);
                InputData.b2 = Parameters.GetParam<double>(b2_tb.Text, "b2", dataInErr);
                InputData.delta2 = Parameters.GetParam<double>(delta2_tb.Text, "delta2", dataInErr);

                ////f
                //{
                //    if (double.TryParse(f_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                //        System.Globalization.CultureInfo.InvariantCulture, out double f))
                //    {
                //        saddleDataIn.f = f;
                //    }
                //    else
                //    {
                //        dataInErr.Add("s2 неверный ввод");
                //    }
                //}
                break;
            case nameof(ring_rb):
                if (in_rb.Checked)
                {

                    var name = spacerRibsGroupBox
                        .Controls
                        .OfType<RadioButton>()
                        .First(rb => rb.Checked)
                        .Name;
                    InputData.SaddleType = name switch
                    {
                        nameof(spacerRibRadioButton_0) => SaddleType.SaddleWithRingInNoRibs,
                        nameof(spacerRibRadioButton_1) => SaddleType.SaddleWithRingIn1Rib,
                        nameof(spacerRibsRadioButton_3) => SaddleType.SaddleWithRingIn3Rib,
                        _ => throw new InvalidOperationException()
                    };
                }
                else
                {
                    InputData.SaddleType = SaddleType.SaddleWithRingOutRib;
                }
                break;
            default:
                dataInErr.Add("Невозможно определить тип укрепления обечайки");
                break;
        }

        var isNoError = !dataInErr.Any() && InputData.IsDataGood;

        if (!isNoError)
        {
            MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(InputData.ErrorList)));
        }

        return isNoError;
    }

    protected override bool CollectDataForFinishCalculation()
    {
        if (InputData == null) throw new InvalidOperationException();

        var dataInErr = new List<string>();

        InputData.Name = name_tb.Text;
        InputData.NameShell = nameShell_tb.Text;

        var isNoError = !dataInErr.Any() && InputData.IsDataGood;

        if (!isNoError)
        {
            MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(InputData.ErrorList)));
        }

        return isNoError;
    }

    private void SaddleForm_Load(object sender, EventArgs e)
    {
        LoadSteelsToComboBox(steel_cb, SteelSource.G34233D1);
        LoadCalculateServicesNamesToComboBox(Gost_cb);

        saddle_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(Resources.SaddleNothingElem)
                                   ?? throw new InvalidOperationException());
    }

    private void SaddleForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (sender is not SaddleForm) return;

        if (Owner is not MainForm { SaddleForm: not null } main) return;

        main.SaddleForm = null;
    }

    private void Cancel_btn_Click(object sender, EventArgs e)
    {
        Hide();
    }

    private void ShellReinforcement_rb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is not RadioButton { Checked: true } rb) return;

        var name = rb.Name.First().ToString().ToUpper() + rb.Name[1..^3];

        switch (name)
        {
            case "Nothing":
                ringLocation_gb.Enabled = false;
                spacerRibsGroupBox.Enabled = false;
                pad_gb.Visible = false;
                break;
            case "Sheet":
                ringLocation_gb.Enabled = false;
                spacerRibsGroupBox.Enabled = false;
                pad_gb.Visible = true;
                break;
            case "Ring":
                name += in_rb.Checked ? "In" + GetNumberOfSpacerRibs() : "Out";
                ringLocation_gb.Enabled = true;
                spacerRibsGroupBox.Enabled = true;
                pad_gb.Visible = false;
                break;
        }

        saddle_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(
                                        Resources.ResourceManager.GetObject("Saddle" + name + "Elem")
                                        ?? throw new InvalidOperationException())
                                    ?? throw new InvalidOperationException());
    }

    private void InOut_rb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is not RadioButton { Checked: true } rb) return;

        spacerRibsGroupBox.Enabled = rb.Name == nameof(in_rb);

        saddle_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(
                                        Resources.ResourceManager.GetObject("SaddleRing" +
                                                                            (in_rb.Checked
                                                                                ? "In" + GetNumberOfSpacerRibs()
                                                                                : "Out") + "Elem")
                                        ?? throw new InvalidOperationException())
                                    ?? throw new InvalidOperationException());
    }


    private void PreCalc_btn_Click(object sender, EventArgs e)
    {
        if (!CollectDataForPreliminarilyCalculation()) return;

        var saddle = Calculate();

        if (saddle == null) return;

        calc_btn.Enabled = true;
        MessageBox.Show(Resources.CalcComplete);
    }

    private void Calc_btn_Click(object sender, EventArgs e)
    {
        if (!CollectDataForFinishCalculation()) return;

        var saddle = Calculate();

        if (saddle == null) return;

        SetCalculatedElementToStorage(Owner, saddle);

        MessageBox.Show(Resources.CalcComplete);
        Close();
    }

    private void SpacerRibRadioButton_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is not RadioButton { Checked: true } rb) return;

        saddle_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(
                                       Resources.ResourceManager.GetObject("SaddleRingIn" + GetNumberOfSpacerRibs() + "Elem")
                                       ?? throw new InvalidOperationException())
                                   ?? throw new InvalidOperationException());
    }

    private string GetNumberOfSpacerRibs()
    {
        var numberOfSpacerRibs = spacerRibsGroupBox
                                     .Controls
                                     .OfType<RadioButton>()
                                     .FirstOrDefault(b => b.Checked)
                                     ?.Name
                                     .LastOrDefault()
                                     .ToString() ??
                                 throw new InvalidOperationException();

        return numberOfSpacerRibs;
    }
}