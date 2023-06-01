using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Conical;
using CalculateVessels.Core.Shells.Enums;
using CalculateVessels.Data.Enums;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Data.Properties;
using CalculateVessels.Helpers;

namespace CalculateVessels.Forms;

public sealed partial class ConicalShellForm : ConicalShellFormMiddle
{
    public ConicalShellForm(IEnumerable<ICalculateService<ConicalShellInput>> calculateServices,
        IPhysicalDataService physicalDataService)
        : base(calculateServices, physicalDataService)
    {
        InitializeComponent();
    }

    protected override string GetServiceName()
    {
        return Gost_cb.Text;
    }

    private void ConicalShellForm_Load(object sender, EventArgs e)
    {
        LoadSteelsToComboBox(steel_cb, SteelSource.G34233D1);
        LoadSteelsToComboBox(steel1Big_cb, SteelSource.G34233D1);
        LoadSteelsToComboBox(steel2Big_cb, SteelSource.G34233D1);
        LoadSteelsToComboBox(steel1Little_cb, SteelSource.G34233D1);
        LoadSteelsToComboBox(steel2Little_cb, SteelSource.G34233D1);
        LoadSteelsToComboBox(steelC_cb, SteelSource.G34233D1);
        LoadSteelsToComboBox(steelCLittle_cb, SteelSource.G34233D1);
        LoadSteelsToComboBox(steelT_cb, SteelSource.G34233D1);

        LoadCalculateServicesNamesToComboBox(Gost_cb);

        shell_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(Resources.ConeElem)
                                                  ?? throw new InvalidOperationException());

        bigConnection_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(Resources.ConeBigConnectionSimple)
                                          ?? throw new InvalidOperationException());
        littleConnection_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(Resources.ConeLittleConnectionSimple)
                                          ?? throw new InvalidOperationException());
    }

    private void ConicalShellForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (sender is not ConicalShellForm) return;

        if (Owner is not MainForm { ConicalForm: not null } main) return;

        main.ConicalForm = null;
    }

    private void Cancel_btn_Click(object sender, EventArgs e)
    {
        Hide();
    }

    protected override bool CollectDataForPreliminarilyCalculation()
    {
        var dataInErr = new List<string>();

        InputData = new ConicalShellInput
        {
            Steel = steel_cb.Text,
            IsPressureIn = !isNotPressureIn_cb.Checked,
            IsConnectionWithLittle = littleConnection_cb.Checked,

            //public double ny { get; set; } = 2.4;

            //public double SigmaAllow1Little { get; set; }
            //public double SigmaAllow1Big { get; set; }
            //public double SigmaAllow2Little { get; set; }
            //public double SigmaAllow2Big { get; set; }
            //public double SigmaAllowC { get; set; }
            //public double SigmaAllowT { get; set; }

            c1 = Parameters.GetParam<double>(c1_tb.Text, "c1", ref dataInErr),
            c2 = Parameters.GetParam<double>(c2_tb.Text, "c2", ref dataInErr),
            c3 = Parameters.GetParam<double>(c3_tb.Text, "c3", ref dataInErr),
            D = Parameters.GetParam<double>(D_tb.Text, "D", ref dataInErr),
            D1 = Parameters.GetParam<double>(D1_tb.Text, "D1", ref dataInErr),
            L = Parameters.GetParam<double>(L_tb.Text, "L", ref dataInErr),
            //M = Parameters.GetParam<double>(M_tb.Text, "M", ref dataInErr),
            p = Parameters.GetParam<double>(p_tb.Text, "p", ref dataInErr),
            phi = Parameters.GetParam<double>(phip_tb.Text, "φp", ref dataInErr),
            phi_t = Parameters.GetParam<double>(phit_tb.Text, "φt", ref dataInErr),
            s = Parameters.GetParam<double>(s_tb.Text, "s", ref dataInErr),
            SigmaAllow = sigmaHandle_cb.Checked
                ? Parameters.GetParam<double>(sigma_d_tb.Text, "[σ]", ref dataInErr)
                : default,
            E = EHandle_cb.Checked
                ? Parameters.GetParam<double>(E_tb.Text, "E", ref dataInErr)
                : default,
            t = Parameters.GetParam<double>(t_tb.Text, "t", ref dataInErr, NumberStyles.Integer)
        };

        if (bigConnection_cb.Checked)
        {
            var bigConnectionType = bigConnection_gb.Controls
                .OfType<RadioButton>()
                .First(rb => rb.Checked)
                .Name;

            switch (bigConnectionType)
            {
                case nameof(bigConnectionSimple_rb):
                    InputData.ConnectionType = ConicalConnectionType.Simply;
                    InputData.s1Big = Parameters.GetParam<double>(s_tb.Text, "s1Big", ref dataInErr);
                    InputData.Steel1Big = steel_cb.Text;
                    InputData.s2Big = Parameters.GetParam<double>(s2Big_tb.Text, "s2Big", ref dataInErr);
                    InputData.Steel2Big = steel2Big_cb.Text;
                    break;
                case nameof(bigConnectionSimple_1_rb):
                    InputData.ConnectionType = ConicalConnectionType.Simply;
                    InputData.s1Big = Parameters.GetParam<double>(s1Big_tb.Text, "s1Big", ref dataInErr);
                    InputData.Steel1Big = steel1Big_cb.Text;
                    InputData.s2Big = Parameters.GetParam<double>(s2Big_tb.Text, "s2Big", ref dataInErr);
                    InputData.Steel2Big = steel2Big_cb.Text;
                    break;
                case nameof(bigConnectionWithRing_rb):
                    InputData.ConnectionType = ConicalConnectionType.WithRingPicture25b;
                    InputData.s1Big = Parameters.GetParam<double>(s1Big_tb.Text, "s1Big", ref dataInErr);
                    InputData.Steel1Big = steel1Big_cb.Text;
                    InputData.s2Big = Parameters.GetParam<double>(s2Big_tb.Text, "s2Big", ref dataInErr);
                    InputData.Steel2Big = steel2Big_cb.Text;
                    InputData.Ak = Parameters.GetParam<double>(AkBig_tb.Text, "Ak", ref dataInErr);
                    InputData.SteelC = steelC_cb.Text;
                    InputData.phi_k = Parameters.GetParam<double>(phi_k_tb.Text, "φk", ref dataInErr);
                    break;
                case nameof(bigConnectionWithRingPicture29_rb):
                    InputData.ConnectionType = ConicalConnectionType.WithRingPicture29;
                    InputData.s1Big = Parameters.GetParam<double>(s1Big_tb.Text, "s1Big", ref dataInErr);
                    InputData.Steel1Big = steel1Big_cb.Text;
                    InputData.s2Big = Parameters.GetParam<double>(s2Big_tb.Text, "s2Big", ref dataInErr);
                    InputData.Steel2Big = steel2Big_cb.Text;
                    InputData.Ak = Parameters.GetParam<double>(AkBig_tb.Text, "Ak", ref dataInErr);
                    InputData.SteelC = steelC_cb.Text;
                    InputData.phi_k = Parameters.GetParam<double>(phi_k_tb.Text, "φk", ref dataInErr);
                    break;
                case nameof(bigConnectionToroidal_rb):
                    InputData.ConnectionType = ConicalConnectionType.Toroidal;
                    InputData.sT = Parameters.GetParam<double>(sT_tb.Text, "sT", ref dataInErr);
                    InputData.SteelT = steelT_cb.Text;
                    InputData.r = Parameters.GetParam<double>(r_tb.Text, "r", ref dataInErr);
                    break;
                default:
                    dataInErr.Add("Invalid big connection type.");
                    break;
            }
        }
        else
        {
            InputData.ConnectionType = ConicalConnectionType.WithoutConnection;
        }

        if (InputData.IsConnectionWithLittle)
        {
            var littleConnectionType = littleConnection_gb.Controls
                .OfType<RadioButton>()
                .First(rb => rb.Checked)
                .Name;

            switch (littleConnectionType)
            {
                case nameof(littleConnectionSimple_rb):
                    InputData.s1Little = Parameters.GetParam<double>(s_tb.Text, "s1Little", ref dataInErr);
                    InputData.Steel1Little = steel_cb.Text;
                    InputData.s2Little = Parameters.GetParam<double>(s2Little_tb.Text, "s2Little", ref dataInErr);
                    InputData.Steel2Little = steel2Little_cb.Text;
                    break;
                case nameof(littleConnectionSimple_1_rb):
                    InputData.s1Little = Parameters.GetParam<double>(s1Little_tb.Text, "s1Little", ref dataInErr);
                    InputData.Steel1Little = steel1Little_cb.Text;
                    InputData.s2Little = Parameters.GetParam<double>(s2Little_tb.Text, "s2Little", ref dataInErr);
                    InputData.Steel2Little = steel2Little_cb.Text;
                    break;
                case nameof(littleConnectionWithRingPicture29_rb):
                    InputData.s1Little = Parameters.GetParam<double>(s1Little_tb.Text, "s1Little", ref dataInErr);
                    InputData.Steel1Little = steel1Little_cb.Text;
                    InputData.s2Little = Parameters.GetParam<double>(s2Little_tb.Text, "s2Little", ref dataInErr);
                    InputData.Steel2Little = steel2Little_cb.Text;
                    //InputData.AkLittle = Parameters.GetParam<double>(AkLittle_tb.Text, "AkLittle", ref dataInErr);
                    //InputData.SteelCLittle = steelCLittle_cb.Text;
                    break;
                default:
                    dataInErr.Add("Invalid big connection type.");
                    break;
            }
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

        var isNoError = !dataInErr.Any() && InputData.IsDataGood;

        if (!isNoError)
        {
            MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(InputData.ErrorList)));
        }

        return isNoError;
    }

    private void BigConnection_cb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is not CheckBox cb)
            return;

        bigConnection_gb.Enabled = cb.Checked;
    }

    private void BigConnection_rb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is not RadioButton { Checked: true } rb) return;

        DrawConnectionPicture(bigConnection_gb, bigConnection_pb);

        switch (rb.Name)
        {
            case nameof(bigConnectionSimple_rb):
                s1BigPanel.Visible = false;
                s2BigPanel.Visible = true;
                AkBigPanel.Visible = false;
                sTBigPanel.Visible = false;
                rBigPanel.Visible = false;
                phi_kPanel.Visible = false;
                break;
            case nameof(bigConnectionSimple_1_rb):
                s1BigPanel.Visible = true;
                s2BigPanel.Visible = true;
                AkBigPanel.Visible = false;
                sTBigPanel.Visible = false;
                rBigPanel.Visible = false;
                phi_kPanel.Visible = false;
                break;
            case nameof(bigConnectionWithRing_rb):
                s1BigPanel.Visible = true;
                s2BigPanel.Visible = true;
                AkBigPanel.Visible = true;
                sTBigPanel.Visible = false;
                rBigPanel.Visible = false;
                phi_kPanel.Visible = true;
                break;
            case nameof(bigConnectionWithRingPicture29_rb):
                s1BigPanel.Visible = true;
                s2BigPanel.Visible = true;
                AkBigPanel.Visible = true;
                sTBigPanel.Visible = false;
                rBigPanel.Visible = false;
                phi_kPanel.Visible = true;
                break;
            case nameof(bigConnectionToroidal_rb):
                s1BigPanel.Visible = false;
                s2BigPanel.Visible = false;
                AkBigPanel.Visible = false;
                sTBigPanel.Visible = true;
                rBigPanel.Visible = true;
                phi_kPanel.Visible = false;
                break;
            default:
                throw new InvalidOperationException($"Invalid radio button - {rb.Name}.");
        }
    }

    private void LittleConnection_cb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is not CheckBox cb)
            return;

        littleConnection_gb.Enabled = cb.Checked;
    }

    private void LittleConnection_rb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is not RadioButton { Checked: true } rb) return;

        DrawConnectionPicture(littleConnection_gb, littleConnection_pb);

        switch (rb.Name)
        {
            case nameof(littleConnectionSimple_rb):
                s1LittlePanel.Visible = false;
                s2LittlePanel.Visible = true;
                AkLittlePanel.Visible = false;
                break;
            case nameof(littleConnectionSimple_1_rb):
                s1LittlePanel.Visible = true;
                s2LittlePanel.Visible = true;
                AkLittlePanel.Visible = false;
                break;
            case nameof(littleConnectionWithRingPicture29_rb):
                s1LittlePanel.Visible = true;
                s2LittlePanel.Visible = true;
                AkLittlePanel.Visible = true;
                break;
            default:
                throw new InvalidOperationException($"Invalid radio button - {rb.Name}.");
        }
    }


    private void PreCalculate_btn_Click(object sender, EventArgs e)
    {
        p_d_l.Text = "[p]";

        if (!CollectDataForPreliminarilyCalculation()) return;

        var conical = Calculate();

        if (conical == null) return;

        sk_l.Text = $"sk={((ConicalShellCalculated)conical).s:f3} мм";

        calculate_btn.Enabled = true;
        MessageBox.Show(Resources.CalcComplete);
    }

    private void Calculate_btn_Click(object sender, EventArgs e)
    {
        if (!CollectDataForFinishCalculation()) return;

        var conical = Calculate();

        if (conical == null) return;

        SetCalculatedElementToStorage(Owner, conical);

        sk_l.Text = $"sk={((ConicalShellCalculated)conical).s:f3} мм";
        p_d_l.Text = $"[p]={((ConicalShellCalculated)conical).p_d:f3} МПа";

        MessageBox.Show(Resources.CalcComplete);
        Close();
    }

    private static void DrawConnectionPicture(Control container, PictureBox picture)
    {
        const string cone = "Cone";

        var type = container.Controls
            .OfType<RadioButton>()
            .First(rb => rb.Checked)
            .Name[..^3];

        var upperType = char.ToUpper(type[0]) + type[1..];

        var resourceName = cone + upperType;

        picture.Image = (Bitmap)(new ImageConverter().ConvertFrom(
                                                 Resources.ResourceManager.GetObject(resourceName)
                                                 ?? throw new InvalidOperationException())
                                             ?? throw new InvalidOperationException());
    }
}