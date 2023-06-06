using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Interfaces;
using System.Collections.Generic;
using CalculateVessels.Core.Supports.BracketVertical;
using CalculateVessels.Data.Enums;
using System.Drawing;
using System.Windows.Forms;
using System;
using System.Linq;
using CalculateVessels.Data.Properties;
using System.Globalization;
using CalculateVessels.Core.Enums;
using CalculateVessels.Helpers;
using CalculateVessels.Forms.MiddleForms;

namespace CalculateVessels.Forms;

public sealed partial class BracketVerticalForm : BracketVerticalFormMiddle
{
    public BracketVerticalForm(IEnumerable<ICalculateService<BracketVerticalInput>> calculateServices,
        IPhysicalDataService physicalDataService)
        : base(calculateServices, physicalDataService)
    {
        InitializeComponent();
    }

    protected override string GetServiceName()
    {
        return Gost_cb.Text;
    }

    private void BracketVerticalForm_Load(object sender, EventArgs e)
    {
        LoadSteelsToComboBox(steel_cb, SteelSource.G34233D1);
        LoadCalculateServicesNamesToComboBox(Gost_cb);

        bracketVerticalNumber_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(Resources.BracketVertical_2Lugs)
                                                  ?? throw new InvalidOperationException());
        bracketVertical_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(Resources.BracketVertical_A_1)
                                            ?? throw new InvalidOperationException());
    }

    private void BracketVerticalForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (sender is not BracketVerticalForm) return;

        if (Owner is not MainForm { BracketVerticalForm: not null } main) return;

        main.BracketVerticalForm = null;
    }

    private void Cancel_btn_Click(object sender, EventArgs e)
    {
        Hide();
    }

    protected override bool CollectDataForPreliminarilyCalculation()
    {
        var dataInErr = new List<string>();

        InputData = new BracketVerticalInput()
        {
            Steel = steel_cb.Text,
            IsPressureIn = !isNotPressureIn_cb.Checked,
            IsAssembly = isAssembly_cb.Checked,
            PreciseMontage = preciseMontage_cb.Checked,
            ReinforcingPad = reinforcementPad_cb.Checked,
            b4 = Parameters.GetParam<double>(b4_tb.Text, "b4", ref dataInErr),
            c = Parameters.GetParam<double>(c_tb.Text, "c", ref dataInErr),
            D = Parameters.GetParam<double>(D_tb.Text, "D", ref dataInErr),
            e1 = Parameters.GetParam<double>(e1_tb.Text, "e1", ref dataInErr),
            g = Parameters.GetParam<double>(g_tb.Text, "g", ref dataInErr),
            G = Parameters.GetParam<double>(GCapital_tb.Text, "G", ref dataInErr),
            h = Parameters.GetParam<double>(h_tb.Text, "h", ref dataInErr),
            h1 = Parameters.GetParam<double>(h1_tb.Text, "h1", ref dataInErr),
            l1 = Parameters.GetParam<double>(l1_tb.Text, "l1", ref dataInErr),
            M = Parameters.GetParam<double>(M_tb.Text, "M", ref dataInErr),
            p = Parameters.GetParam<double>(p_tb.Text, "p", ref dataInErr),
            phi = Parameters.GetParam<double>(fi_tb.Text, "φ", ref dataInErr),
            Q = Parameters.GetParam<double>(Q_tb.Text, "Q", ref dataInErr),
            s = Parameters.GetParam<double>(s_tb.Text, "s", ref dataInErr),
            SigmaAllow = sigmaHandle_cb.Checked
                ? Parameters.GetParam<double>(sigma_d_tb.Text, "[σ]", ref dataInErr)
                : default,
            t = Parameters.GetParam<double>(t_tb.Text, "t", ref dataInErr, NumberStyles.Integer),
            N = Parameters.GetParam<int>(N_cb.Text, "N", ref dataInErr, NumberStyles.Integer),
        };

        var number = numberOfBracket_gb.Controls
            .OfType<RadioButton>()
            .First(rb => rb.Checked)
            .Text;

        InputData.n = Parameters.GetParam<int>(number, "n", ref dataInErr);

        var type = bracketType_gb.Controls
            .OfType<RadioButton>()
            .First(rb => rb.Checked)
            .Text;

        InputData.Type = Parameters.GetParam<BracketVerticalType>(type, "Bracket type", ref dataInErr);

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

    private void Type_rb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is not RadioButton { Checked: true }) return;

        DrawPicture();
    }

    private void ReinforcementPad_cb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is not CheckBox cb) return;

        DrawPicture();
        pad_gb.Enabled = cb.Checked;
    }

    private void Number_rb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is not RadioButton { Checked: true }) return;

        DrawNumberPicture();
    }


    private void PreCalculate_btn_Click(object sender, EventArgs e)
    {
        if (!CollectDataForPreliminarilyCalculation()) return;

        var bracketVertical = Calculate();

        if (bracketVertical == null) return;

        calculate_btn.Enabled = true;
        MessageBox.Show(Resources.CalcComplete);
    }

    private void Calculate_btn_Click(object sender, EventArgs e)
    {
        if (!CollectDataForFinishCalculation()) return;

        var bracketVertical = Calculate();

        if (bracketVertical == null) return;

        SetCalculatedElementToStorage(Owner, bracketVertical);

        MessageBox.Show(Resources.CalcComplete);
        Close();
    }

    private void DrawPicture()
    {
        const string bracketVertical = "BracketVertical_";

        var type = bracketType_gb.Controls
            .OfType<RadioButton>()
            .First(rb => rb.Checked)
            .Text;

        var resourceName = bracketVertical + type +
                           (reinforcementPad_cb.Checked ? "" : "_1");

        bracketVertical_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(
                                                Resources.ResourceManager.GetObject(resourceName)
                                                ?? throw new InvalidOperationException())
                                            ?? throw new InvalidOperationException());
    }

    private void DrawNumberPicture()
    {
        const string bracketVertical = "BracketVertical_";
        const string lugs = "Lugs";

        var number = numberOfBracket_gb.Controls
            .OfType<RadioButton>()
            .First(rb => rb.Checked)
            .Text;

        var resourceName = bracketVertical + number + lugs;

        bracketVerticalNumber_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(
                                                Resources.ResourceManager.GetObject(resourceName)
                                                ?? throw new InvalidOperationException())
                                            ?? throw new InvalidOperationException());
    }
}