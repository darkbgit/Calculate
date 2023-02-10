using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Supports.Saddle;
using CalculateVessels.Data.Enums;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Data.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CalculateVessels.Forms.Base;
using CalculateVessels.Core.Enums;
using CalculateVessels.Helpers;

namespace CalculateVessels.Forms;

public sealed partial class SaddleForm : BaseCalculateForm<SaddleInput>
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

    protected override bool CollectDataForPreliminarilyCalculation()
    {
        var dataInErr = new List<string>();

        InputData = new SaddleInput
        {
            Steel = steel_cb.Text,
            IsPressureIn = !isNotPressureIn_cb.Checked,
            IsAssembly = isAssembly_cb.Checked,
            D = Parameters.GetParam<double>(D_tb.Text, "D", ref dataInErr),
            s = Parameters.GetParam<double>(s_tb.Text, "s", ref dataInErr),
            c = Parameters.GetParam<double>(c_tb.Text, "c", ref dataInErr),
            p = Parameters.GetParam<double>(p_tb.Text, "p", ref dataInErr),
            fi = Parameters.GetParam<double>(fi_tb.Text, "φ", ref dataInErr),
            t = Parameters.GetParam<double>(t_tb.Text, "t", ref dataInErr, NumberStyles.Integer),
            N = Parameters.GetParam<int>(N_cb.Text, "N", ref dataInErr, NumberStyles.Integer),
            G = Parameters.GetParam<double>(G_tb.Text, "G", ref dataInErr),
            b = Parameters.GetParam<double>(b_tb.Text, "b", ref dataInErr),
            delta1 = Parameters.GetParam<double>(delta1_tb.Text, "delta1", ref dataInErr),
            a = Parameters.GetParam<double>(a_tb.Text, "a", ref dataInErr),
            H = Parameters.GetParam<double>(H_tb.Text, "H", ref dataInErr),
            L = Parameters.GetParam<double>(L_tb.Text, "L", ref dataInErr),
            e = Parameters.GetParam<double>(e_tb.Text, "e", ref dataInErr),
        };


        var reinforcementShell = shellReinforcement_gb.Controls
            .OfType<RadioButton>()
            .FirstOrDefault(rb => rb.Checked)
            ?.Name;

        switch (reinforcementShell)
        {
            case nameof(nothing_rb):
                InputData.Type = SaddleType.SaddleWithoutRingWithoutSheet;
                break;
            case nameof(sheet_rb):
                InputData.Type = SaddleType.SaddleWithoutRingWithSheet;

                InputData.s2 = Parameters.GetParam<double>(s2_tb.Text, "s2", ref dataInErr);
                InputData.b2 = Parameters.GetParam<double>(b2_tb.Text, "b2", ref dataInErr);
                InputData.delta2 = Parameters.GetParam<double>(delta2_tb.Text, "delta2", ref dataInErr);

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
                InputData.Type = SaddleType.SaddleWithRing;
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

        if (Owner is not MainForm { SaddleForm: { } } main) return;

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
                ringLocation_gb.Visible = false;
                pad_gb.Visible = false;
                break;
            case "Sheet":
                ringLocation_gb.Visible = false;
                pad_gb.Visible = true;
                break;
            case "Ring":
                name += in_rb.Checked ? "In" : "Out";
                ringLocation_gb.Visible = true;
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
        if (sender is not RadioButton { Checked: true }) return;

        saddle_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(
                                        Resources.ResourceManager.GetObject("SaddleRing" +
                                                                            (in_rb.Checked ? "In" : "Out") + "Elem")
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
}