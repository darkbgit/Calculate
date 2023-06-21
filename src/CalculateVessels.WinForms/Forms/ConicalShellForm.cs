using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Conical;
using CalculateVessels.Core.Shells.Enums;
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

public sealed partial class ConicalShellForm : ConicalShellFormMiddle
{
    public ConicalShellForm(IEnumerable<ICalculateService<ConicalShellInput>> calculateServices,
        IPhysicalDataService physicalDataService,
        IFormFactory formFactory)
        : base(calculateServices, physicalDataService, formFactory)
    {
        InitializeComponent();
    }

    protected override void LoadInputData()
    {
        if (InputData == null) return;

        steel_cb.Text = InputData.Steel;
        phip_tb.Text = InputData.phi.ToString(CultureInfo.CurrentCulture);
        phit_tb.Text = InputData.phi_t.ToString(CultureInfo.CurrentCulture);
        D_tb.Text = InputData.D.ToString(CultureInfo.CurrentCulture);
        D1_tb.Text = InputData.D1.ToString(CultureInfo.CurrentCulture);
        c1_tb.Text = InputData.c1.ToString(CultureInfo.CurrentCulture);
        c2_tb.Text = InputData.c2.ToString(CultureInfo.CurrentCulture);
        c3_tb.Text = InputData.c3.ToString(CultureInfo.CurrentCulture);
        s_tb.Text = InputData.s.ToString(CultureInfo.CurrentCulture);
        L_tb.Text = InputData.L.ToString(CultureInfo.CurrentCulture);

        if (InputData.LoadingConditions.Count() == 1)
        {
            loadingConditionControl.SetLoadingCondition(InputData.LoadingConditions.First());
        }
        else
        {
            loadingConditionsControl.SetLoadingConditions(InputData.LoadingConditions);
        }

        if (InputData.IsConnectionWithLittle)
        {
            littleConnection_cb.Checked = true;
            littleConnectionSimple_1_rb.Checked = true;

            s1Little_tb.Text = InputData.s1Little.ToString(CultureInfo.CurrentCulture);
            steel1Little_cb.Text = InputData.Steel1Little;

            s2Little_tb.Text = InputData.s2Little.ToString(CultureInfo.CurrentCulture);
            steel2Little_cb.Text = InputData.Steel2Little;
        }

        if (InputData.ConnectionType != ConicalConnectionType.WithoutConnection)
        {
            bigConnection_cb.Checked = true;
            switch (InputData.ConnectionType)
            {
                case ConicalConnectionType.Simply:
                    bigConnectionSimple_1_rb.Checked = true;

                    s1Big_tb.Text = InputData.s1Big.ToString(CultureInfo.CurrentCulture);
                    steel1Big_cb.Text = InputData.Steel1Big;

                    s2Big_tb.Text = InputData.s2Big.ToString(CultureInfo.CurrentCulture);
                    steel2Big_cb.Text = InputData.Steel2Big;
                    break;
                case ConicalConnectionType.WithRingPicture25b:
                    bigConnectionWithRing_rb.Checked = true;

                    s1Big_tb.Text = InputData.s1Big.ToString(CultureInfo.CurrentCulture);
                    steel1Big_cb.Text = InputData.Steel1Big;

                    s2Big_tb.Text = InputData.s2Big.ToString(CultureInfo.CurrentCulture);
                    steel2Big_cb.Text = InputData.Steel2Big;

                    AkBig_tb.Text = InputData.Ak.ToString(CultureInfo.CurrentCulture);
                    steelC_cb.Text = InputData.SteelC;
                    phi_k_tb.Text = InputData.phi_k.ToString(CultureInfo.CurrentCulture);
                    break;
                case ConicalConnectionType.WithRingPicture29:
                    bigConnectionWithRingPicture29_rb.Checked = true;

                    s1Big_tb.Text = InputData.s1Big.ToString(CultureInfo.CurrentCulture);
                    steel1Big_cb.Text = InputData.Steel1Big;

                    s2Big_tb.Text = InputData.s2Big.ToString(CultureInfo.CurrentCulture);
                    steel2Big_cb.Text = InputData.Steel2Big;

                    AkBig_tb.Text = InputData.Ak.ToString(CultureInfo.CurrentCulture);
                    steelC_cb.Text = InputData.SteelC;
                    phi_k_tb.Text = InputData.phi_k.ToString(CultureInfo.CurrentCulture);
                    break;
                case ConicalConnectionType.Toroidal:
                    bigConnectionToroidal_rb.Checked = true;

                    sT_tb.Text = InputData.sT.ToString(CultureInfo.CurrentCulture);
                    steelT_cb.Text = InputData.SteelT;

                    r_tb.Text = InputData.r.ToString(CultureInfo.CurrentCulture);
                    break;
            }
        }
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
            IsConnectionWithLittle = littleConnection_cb.Checked,

            //public double ny { get; set; } = 2.4;

            //public double SigmaAllow1Little { get; set; }
            //public double SigmaAllow1Big { get; set; }
            //public double SigmaAllow2Little { get; set; }
            //public double SigmaAllow2Big { get; set; }
            //public double SigmaAllowC { get; set; }
            //public double SigmaAllowT { get; set; }

            c1 = Parameters.GetParam<double>(c1_tb.Text, "c1", dataInErr),
            c2 = Parameters.GetParam<double>(c2_tb.Text, "c2", dataInErr),
            c3 = Parameters.GetParam<double>(c3_tb.Text, "c3", dataInErr),
            D = Parameters.GetParam<double>(D_tb.Text, "D", dataInErr),
            D1 = Parameters.GetParam<double>(D1_tb.Text, "D1", dataInErr),
            L = Parameters.GetParam<double>(L_tb.Text, "L", dataInErr),
            //M = Parameters.GetParam<double>(M_tb.Text, "M", ref dataInErr),
            phi = Parameters.GetParam<double>(phip_tb.Text, "φp", dataInErr),
            phi_t = Parameters.GetParam<double>(phit_tb.Text, "φt", dataInErr),
            s = Parameters.GetParam<double>(s_tb.Text, "s", dataInErr)
        };

        var loadingConditions = FormHelpers.ParseLoadingConditions(loadingConditionsControl, loadingConditionControl).ToList();

        if (!loadingConditions.Any())
        {
            return false;
        }

        InputData.LoadingConditions = loadingConditions;

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
                    InputData.s1Big = Parameters.GetParam<double>(s_tb.Text, "s1Big", dataInErr);
                    InputData.Steel1Big = steel_cb.Text;
                    InputData.s2Big = Parameters.GetParam<double>(s2Big_tb.Text, "s2Big", dataInErr);
                    InputData.Steel2Big = steel2Big_cb.Text;
                    break;
                case nameof(bigConnectionSimple_1_rb):
                    InputData.ConnectionType = ConicalConnectionType.Simply;
                    InputData.s1Big = Parameters.GetParam<double>(s1Big_tb.Text, "s1Big", dataInErr);
                    InputData.Steel1Big = steel1Big_cb.Text;
                    InputData.s2Big = Parameters.GetParam<double>(s2Big_tb.Text, "s2Big", dataInErr);
                    InputData.Steel2Big = steel2Big_cb.Text;
                    break;
                case nameof(bigConnectionWithRing_rb):
                    InputData.ConnectionType = ConicalConnectionType.WithRingPicture25b;
                    InputData.s1Big = Parameters.GetParam<double>(s1Big_tb.Text, "s1Big", dataInErr);
                    InputData.Steel1Big = steel1Big_cb.Text;
                    InputData.s2Big = Parameters.GetParam<double>(s2Big_tb.Text, "s2Big", dataInErr);
                    InputData.Steel2Big = steel2Big_cb.Text;
                    InputData.Ak = Parameters.GetParam<double>(AkBig_tb.Text, "Ak", dataInErr);
                    InputData.SteelC = steelC_cb.Text;
                    InputData.phi_k = Parameters.GetParam<double>(phi_k_tb.Text, "φk", dataInErr);
                    break;
                case nameof(bigConnectionWithRingPicture29_rb):
                    InputData.ConnectionType = ConicalConnectionType.WithRingPicture29;
                    InputData.s1Big = Parameters.GetParam<double>(s1Big_tb.Text, "s1Big", dataInErr);
                    InputData.Steel1Big = steel1Big_cb.Text;
                    InputData.s2Big = Parameters.GetParam<double>(s2Big_tb.Text, "s2Big", dataInErr);
                    InputData.Steel2Big = steel2Big_cb.Text;
                    InputData.Ak = Parameters.GetParam<double>(AkBig_tb.Text, "Ak", dataInErr);
                    InputData.SteelC = steelC_cb.Text;
                    InputData.phi_k = Parameters.GetParam<double>(phi_k_tb.Text, "φk", dataInErr);
                    break;
                case nameof(bigConnectionToroidal_rb):
                    InputData.ConnectionType = ConicalConnectionType.Toroidal;
                    InputData.sT = Parameters.GetParam<double>(sT_tb.Text, "sT", dataInErr);
                    InputData.SteelT = steelT_cb.Text;
                    InputData.r = Parameters.GetParam<double>(r_tb.Text, "r", dataInErr);
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
                    InputData.s1Little = Parameters.GetParam<double>(s_tb.Text, "s1Little", dataInErr);
                    InputData.Steel1Little = steel_cb.Text;
                    InputData.s2Little = Parameters.GetParam<double>(s2Little_tb.Text, "s2Little", dataInErr);
                    InputData.Steel2Little = steel2Little_cb.Text;
                    break;
                case nameof(littleConnectionSimple_1_rb):
                    InputData.s1Little = Parameters.GetParam<double>(s1Little_tb.Text, "s1Little", dataInErr);
                    InputData.Steel1Little = steel1Little_cb.Text;
                    InputData.s2Little = Parameters.GetParam<double>(s2Little_tb.Text, "s2Little", dataInErr);
                    InputData.Steel2Little = steel2Little_cb.Text;
                    break;
                case nameof(littleConnectionWithRingPicture29_rb):
                    InputData.s1Little = Parameters.GetParam<double>(s1Little_tb.Text, "s1Little", dataInErr);
                    InputData.Steel1Little = steel1Little_cb.Text;
                    InputData.s2Little = Parameters.GetParam<double>(s2Little_tb.Text, "s2Little", dataInErr);
                    InputData.Steel2Little = steel2Little_cb.Text;
                    //InputData.AkLittle = Parameters.GetParam<double>(AkLittle_tb.Text, "AkLittle", ref dataInErr);
                    //InputData.SteelCLittle = steelCLittle_cb.Text;
                    break;
                default:
                    dataInErr.Add("Invalid little connection type.");
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
        sk_l.Text = string.Empty;
        p_d_l.Text = "[p]";
        calculate_btn.Enabled = false;

        if (!CollectDataForPreliminarilyCalculation()) return;

        var conical = Calculate();

        if (conical == null) return;

        sk_l.Text = Gets(conical);

        calculate_btn.Enabled = true;
        MessageBox.Show(Resources.CalcComplete);
    }

    private void Calculate_btn_Click(object sender, EventArgs e)
    {
        if (!CollectDataForFinishCalculation()) return;

        var conical = Calculate();

        if (conical == null) return;

        if (isNozzleCalculateCheckBox.Checked)
        {
            if (Owner is not MainForm mainForm) return;

            mainForm.NozzleForm = FormFactory.Create<NozzleForm>()
                                  ?? throw new InvalidOperationException($"Couldn't create {nameof(NozzleForm)}.");
            mainForm.NozzleForm.Owner = mainForm;
            mainForm.NozzleForm.Show(conical);
        }

        if (Owner is not MainForm main)
        {
            MessageBox.Show($"{nameof(MainForm)} error");
            return;
        }

        SetCalculatedElementToStorage(Owner, conical);

        sk_l.Text = Gets(conical);
        p_d_l.Text = Getp(conical);

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

    private void DisabledCalculateBtn(object sender, EventArgs e)
    {
        DisableCalculateButton();
    }

    private void DisableCalculateButton()
    {
        calculate_btn.Enabled = false;
    }

    private static string Gets(ICalculatedElement element) => string
        .Join(", ", ((ConicalShellCalculated)element).Results
            .Select((r, j) => $"s{j + 1}={r.s:f3} мм")
            .ToList());

    private static string Getp(ICalculatedElement element) => string
        .Join(", ", ((ConicalShellCalculated)element).Results
            .Select((r, j) => $"p{j + 1}={r.p_d:f3} МПа")
            .ToList());
}