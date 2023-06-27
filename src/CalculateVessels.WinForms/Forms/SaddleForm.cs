using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CalculateVessels.Core.Elements.Base;
using CalculateVessels.Core.Elements.Supports.Saddle;
using CalculateVessels.Core.Enums;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Enums;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Data.Properties;
using CalculateVessels.Forms.MiddleForms;
using CalculateVessels.Helpers;
using FluentValidation;

namespace CalculateVessels.Forms;

//[TypeDescriptionProvider(typeof(AbstractControlDescriptionProvider<BaseCalculateForm<SaddleInput>, Form, SaddleInput>))]
public sealed partial class SaddleForm : SaddleFormMiddle
{
    public SaddleForm(IEnumerable<ICalculateService<SaddleInput>> calculateServices,
        IPhysicalDataService physicalDataService,
        IValidator<SaddleInput> validator)
        : base(calculateServices, physicalDataService, validator)
    {
        InitializeComponent();
    }

    protected override void LoadInputData(SaddleInput inputData)
    {
        loadingConditionSaddleControl.SetLoadingCondition(new LoadingConditionSaddle
        {
            p = inputData.p,
            t = inputData.t,
            PressureType = inputData.PressureType,
            IsAssembly = inputData.IsAssembly,
            N = inputData.N,
            G = inputData.G,
            SigmaAllow = inputData.SigmaAllow,
            EAllow = inputData.E
        });


        steel_cb.Text = inputData.Steel;
        name_tb.Text = inputData.Name;
        D_tb.Text = inputData.D.ToString(CultureInfo.CurrentCulture);
        s_tb.Text = inputData.s.ToString(CultureInfo.CurrentCulture);
        c_tb.Text = inputData.c.ToString(CultureInfo.CurrentCulture);
        fi_tb.Text = inputData.fi.ToString(CultureInfo.CurrentCulture);
        b_tb.Text = inputData.b.ToString(CultureInfo.CurrentCulture);
        delta1_tb.Text = inputData.delta1.ToString(CultureInfo.CurrentCulture);
        a_tb.Text = inputData.a.ToString(CultureInfo.CurrentCulture);
        H_tb.Text = inputData.H.ToString(CultureInfo.CurrentCulture);
        L_tb.Text = inputData.L.ToString(CultureInfo.CurrentCulture);
        e_tb.Text = inputData.e.ToString(CultureInfo.CurrentCulture);



        var reinforcementShell = shellReinforcement_gb.Controls
            .OfType<RadioButton>()
            .FirstOrDefault(rb => rb.Checked)
            ?.Name;

        switch (inputData.SaddleType)
        {
            case SaddleType.SaddleWithoutRingWithSheet:
                s2_tb.Text = inputData.s2.ToString(CultureInfo.CurrentCulture);
                b2_tb.Text = inputData.b2.ToString(CultureInfo.CurrentCulture);
                delta2_tb.Text = inputData.delta2.ToString(CultureInfo.CurrentCulture);
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

    protected override string GetServiceName()
    {
        return Gost_cb.Text;
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

    protected override bool TryCollectInputData(out SaddleInput inputData)
    {
        var dataInErr = new List<string>();

        inputData = new SaddleInput
        {
            Name = name_tb.Text,
            NameShell = nameShell_tb.Text,
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
        };

        var loadingCondition = loadingConditionSaddleControl.GetLoadingCondition();

        if (loadingCondition == null)
        {
            return false;
        }

        inputData.p = loadingCondition.p;
        inputData.t = loadingCondition.t;
        inputData.PressureType = loadingCondition.PressureType;
        inputData.N = loadingCondition.N;
        inputData.G = loadingCondition.G;
        inputData.IsAssembly = loadingCondition.IsAssembly;

        var reinforcementShell = shellReinforcement_gb.Controls
            .OfType<RadioButton>()
            .FirstOrDefault(rb => rb.Checked)
            ?.Name;

        switch (reinforcementShell)
        {
            case nameof(nothing_rb):
                inputData.SaddleType = SaddleType.SaddleWithoutRingWithoutSheet;
                break;
            case nameof(sheet_rb):
                inputData.SaddleType = SaddleType.SaddleWithoutRingWithSheet;

                inputData.s2 = Parameters.GetParam<double>(s2_tb.Text, "s2", dataInErr);
                inputData.b2 = Parameters.GetParam<double>(b2_tb.Text, "b2", dataInErr);
                inputData.delta2 = Parameters.GetParam<double>(delta2_tb.Text, "delta2", dataInErr);

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
                    inputData.SaddleType = name switch
                    {
                        nameof(spacerRibRadioButton_0) => SaddleType.SaddleWithRingInNoRibs,
                        nameof(spacerRibRadioButton_1) => SaddleType.SaddleWithRingIn1Rib,
                        nameof(spacerRibsRadioButton_3) => SaddleType.SaddleWithRingIn3Rib,
                        _ => throw new InvalidOperationException()
                    };
                }
                else
                {
                    inputData.SaddleType = SaddleType.SaddleWithRingOutRib;
                }
                break;
            default:
                dataInErr.Add("Невозможно определить тип укрепления обечайки");
                break;
        }

        if (!dataInErr.Any()) return true;

        MessageBox.Show(string.Join(Environment.NewLine, dataInErr));
        return false;
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
        if (!TryCalculate(out var saddle)) return;

        if (saddle == null) throw new NullReferenceException();

        calc_btn.Enabled = true;
        MessageBox.Show(Resources.CalcComplete);
    }
    private void Calc_btn_Click(object sender, EventArgs e)
    {
        if (!TryCalculate(out var saddle)) return;

        if (saddle == null) throw new NullReferenceException();

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