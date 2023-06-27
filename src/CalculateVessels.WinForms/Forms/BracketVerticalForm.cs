using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CalculateVessels.Core.Elements.Supports.BracketVertical;
using CalculateVessels.Core.Enums;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Enums;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Data.Properties;
using CalculateVessels.Forms.MiddleForms;
using CalculateVessels.Helpers;
using FluentValidation;

namespace CalculateVessels.Forms;

public sealed partial class BracketVerticalForm : BracketVerticalFormMiddle
{
    public BracketVerticalForm(IEnumerable<ICalculateService<BracketVerticalInput>> calculateServices,
        IPhysicalDataService physicalDataService,
        IValidator<BracketVerticalInput> validator)
        : base(calculateServices, physicalDataService, validator)
    {
        InitializeComponent();
    }

    protected override void LoadInputData(BracketVerticalInput inputData)
    {
        name_tb.Text = inputData.Name;
        nameShell_tb.Text = inputData.NameShell;
        steel_cb.Text = inputData.Steel;
        isNotPressureIn_cb.Checked = inputData.PressureType == PressureType.Outside;
        isAssembly_cb.Checked = inputData.IsAssembly;
        preciseMontage_cb.Checked = inputData.PreciseMontage;
        reinforcementPad_cb.Checked = inputData.ReinforcingPad;
        b4_tb.Text = inputData.b4.ToString(CultureInfo.CurrentCulture);
        c_tb.Text = inputData.c.ToString(CultureInfo.CurrentCulture);
        D_tb.Text = inputData.D.ToString(CultureInfo.CurrentCulture);
        e1_tb.Text = inputData.e1.ToString(CultureInfo.CurrentCulture);
        g_tb.Text = inputData.g.ToString(CultureInfo.CurrentCulture);
        GCapital_tb.Text = inputData.G.ToString(CultureInfo.CurrentCulture);
        h_tb.Text = inputData.h.ToString(CultureInfo.CurrentCulture);
        h1_tb.Text = inputData.h1.ToString(CultureInfo.CurrentCulture);
        l1_tb.Text = inputData.l1.ToString(CultureInfo.CurrentCulture);
        M_tb.Text = inputData.M.ToString(CultureInfo.CurrentCulture);
        p_tb.Text = inputData.p.ToString(CultureInfo.CurrentCulture);
        fi_tb.Text = inputData.phi.ToString(CultureInfo.CurrentCulture);
        Q_tb.Text = inputData.Q.ToString(CultureInfo.CurrentCulture);
        s_tb.Text = inputData.s.ToString(CultureInfo.CurrentCulture);

        if (inputData.SigmaAllow > 0)
        {
            sigmaHandle_cb.Checked = true;
            sigma_d_tb.Text = inputData.SigmaAllow.ToString(CultureInfo.CurrentCulture);
        }
        t_tb.Text = inputData.t.ToString(CultureInfo.CurrentCulture);
        N_cb.Text = inputData.N.ToString(CultureInfo.CurrentCulture);

        numberOfBracket_gb.Controls
            .OfType<RadioButton>()
            .First(rb => rb.Text == inputData.n.ToString())
            .Checked = true;

        bracketType_gb.Controls
            .OfType<RadioButton>()
            .First(rb => rb.Text == inputData.BracketVerticalType.ToString())
            .Checked = true;
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

    protected override bool TryCollectInputData(out BracketVerticalInput inputData)
    {
        var dataInErr = new List<string>();

        inputData = new BracketVerticalInput()
        {
            Name = name_tb.Text,
            NameShell = nameShell_tb.Text,
            Steel = steel_cb.Text,
            PressureType = isNotPressureIn_cb.Checked ? PressureType.Outside : PressureType.Inside,
            IsAssembly = isAssembly_cb.Checked,
            PreciseMontage = preciseMontage_cb.Checked,
            ReinforcingPad = reinforcementPad_cb.Checked,
            b4 = Parameters.GetParam<double>(b4_tb.Text, "b4", dataInErr),
            c = Parameters.GetParam<double>(c_tb.Text, "c", dataInErr),
            D = Parameters.GetParam<double>(D_tb.Text, "D", dataInErr),
            e1 = Parameters.GetParam<double>(e1_tb.Text, "e1", dataInErr),
            g = Parameters.GetParam<double>(g_tb.Text, "g", dataInErr),
            G = Parameters.GetParam<double>(GCapital_tb.Text, "G", dataInErr),
            h = Parameters.GetParam<double>(h_tb.Text, "h", dataInErr),
            h1 = Parameters.GetParam<double>(h1_tb.Text, "h1", dataInErr),
            l1 = Parameters.GetParam<double>(l1_tb.Text, "l1", dataInErr),
            M = Parameters.GetParam<double>(M_tb.Text, "M", dataInErr),
            p = Parameters.GetParam<double>(p_tb.Text, "p", dataInErr),
            phi = Parameters.GetParam<double>(fi_tb.Text, "φ", dataInErr),
            Q = Parameters.GetParam<double>(Q_tb.Text, "Q", dataInErr),
            s = Parameters.GetParam<double>(s_tb.Text, "s", dataInErr),
            SigmaAllow = sigmaHandle_cb.Checked
                ? Parameters.GetParam<double>(sigma_d_tb.Text, "[σ]", dataInErr)
                : default,
            t = Parameters.GetParam<double>(t_tb.Text, "t", dataInErr, NumberStyles.Integer),
            N = Parameters.GetParam<int>(N_cb.Text, "N", dataInErr, NumberStyles.Integer),
        };

        var number = numberOfBracket_gb.Controls
            .OfType<RadioButton>()
            .First(rb => rb.Checked)
            .Text;

        inputData.n = Parameters.GetParam<int>(number, "n", dataInErr);

        var type = bracketType_gb.Controls
            .OfType<RadioButton>()
            .First(rb => rb.Checked)
            .Text;

        inputData.BracketVerticalType = Parameters.GetParam<BracketVerticalType>(type, "Bracket type", dataInErr);

        if (!dataInErr.Any()) return true;

        MessageBox.Show(string.Join(Environment.NewLine, dataInErr));
        return false;
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
        if (!TryCalculate(out var bracketVertical)) return;

        if (bracketVertical == null) throw new NullReferenceException();

        calculate_btn.Enabled = true;
        MessageBox.Show(Resources.CalcComplete);
    }

    private void Calculate_btn_Click(object sender, EventArgs e)
    {
        if (!TryCalculate(out var bracketVertical)) return;

        if (bracketVertical == null) throw new NullReferenceException();

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