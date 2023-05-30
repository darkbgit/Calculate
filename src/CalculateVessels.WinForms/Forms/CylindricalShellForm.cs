using CalculateVessels.Core.Exceptions;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Cylindrical;
using CalculateVessels.Data.Enums;
using CalculateVessels.Data.Exceptions;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Data.Properties;
using CalculateVessels.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace CalculateVessels.Forms;

public partial class CylindricalShellForm : Form
{
    private readonly IEnumerable<ICalculateService<CylindricalShellInput>> _calculateServices;
    private readonly IPhysicalDataService _physicalDataService;
    private readonly IFormFactory _formFactory;

    private CylindricalShellInput? _inputData;

    public CylindricalShellForm(IEnumerable<ICalculateService<CylindricalShellInput>> calculateServices,
        IFormFactory formFactory, IPhysicalDataService physicalDataService)
    {
        InitializeComponent();
        _calculateServices = calculateServices;
        _formFactory = formFactory;
        _physicalDataService = physicalDataService;
    }

    private ICalculateService<CylindricalShellInput> GetCalculateService()
    {
        return _calculateServices
                       .FirstOrDefault(s => s.Name == Gost_cb.Text)
                            ?? throw new InvalidOperationException("Service wasn't found.");
    }

    private void Cancel_b_Click(object sender, EventArgs e)
    {
        Hide();
    }

    private void Force_rb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is not RadioButton { Checked: true } rb) return;

        var fPbImage = Resources.ResourceManager.GetObject("PC" + rb.Text) ?? throw new NullReferenceException();
        f_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(fPbImage) ?? throw new NullReferenceException());
        switch (Convert.ToInt32(rb.Text))
        {
            case 5:
                fq_l.Text = "q";
                fq_mes_l.Text = "";
                fq_panel.Visible = true;
                break;
            case 6:
            case 7:
                fq_l.Text = "f";
                fq_mes_l.Text = "мм";
                fq_panel.Visible = true;
                break;
            default:
                fq_panel.Visible = false;
                break;
        }
    }

    private void Pressure_rb(object sender, EventArgs e)
    {
        if (sender is not RadioButton { Checked: true }) return;

        var isPressureOut = nar_rb.Checked;
        l_tb.Enabled = isPressureOut;
        //l_tb.ReadOnly = isPressureIn;
        getE_b.Enabled = isPressureOut;
        getL_b.Enabled = isPressureOut;
    }

    private bool CollectDataForPreliminarilyCalculation()
    {
        var dataInErr = new List<string>();

        _inputData = new CylindricalShellInput
        {
            t = Parameters.GetParam<double>(t_tb.Text, "t", ref dataInErr, NumberStyles.Integer),
            Steel = steel_cb.Text,
            IsPressureIn = vn_rb.Checked,
            p = Parameters.GetParam<double>(p_tb.Text, "p", ref dataInErr),
            fi = Parameters.GetParam<double>(fi_tb.Text, "fi", ref dataInErr),
            D = Parameters.GetParam<double>(D_tb.Text, "D", ref dataInErr),
            c1 = Parameters.GetParam<double>(c1_tb.Text, "c1", ref dataInErr),
            c2 = Parameters.GetParam<double>(c2_tb.Text, "c2", ref dataInErr),
            c3 = Parameters.GetParam<double>(c3_tb.Text, "c3", ref dataInErr),
            SigmaAllow = sigmaHandle_cb.Checked
                ? Parameters.GetParam<double>(sigma_d_tb.Text, "[σ]", ref dataInErr)
                : default
        };

        if (!_inputData.IsPressureIn)
        {
            if (EHandle_cb.Checked)
            {
                _inputData.E = Parameters.GetParam<double>(E_tb.Text, "E", ref dataInErr);
            }

            _inputData.l = Parameters.GetParam<double>(l_tb.Text, "l", ref dataInErr, NumberStyles.Integer);
        }

        var isNoError = !dataInErr.Any() && _inputData.IsDataGood;

        if (!isNoError)
        {
            MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(_inputData.ErrorList)));
        }

        return isNoError;
    }

    private bool CollectDataForFinishCalculation()
    {
        var dataInErr = new List<string>();

        _inputData = new CylindricalShellInput()
        {
            Name = Name_tb.Text,
            s = Parameters.GetParam<double>(s_tb.Text, "s", ref dataInErr),

            t = Parameters.GetParam<double>(t_tb.Text, "t", ref dataInErr, NumberStyles.Integer),
            Steel = steel_cb.Text,
            IsPressureIn = vn_rb.Checked,
            p = Parameters.GetParam<double>(p_tb.Text, "p", ref dataInErr),
            fi = Parameters.GetParam<double>(fi_tb.Text, "φ", ref dataInErr),
            D = Parameters.GetParam<double>(D_tb.Text, "D", ref dataInErr),
            c1 = Parameters.GetParam<double>(c1_tb.Text, "c1", ref dataInErr),
            c2 = Parameters.GetParam<double>(c2_tb.Text, "c2", ref dataInErr),
            c3 = Parameters.GetParam<double>(c3_tb.Text, "c3", ref dataInErr),
            SigmaAllow = sigmaHandle_cb.Checked
                ? Parameters.GetParam<double>(sigma_d_tb.Text, "[σ]", ref dataInErr)
                : default
        };

        if (!_inputData.IsPressureIn)
        {
            if (EHandle_cb.Checked)
            {
                _inputData.E = Parameters.GetParam<double>(E_tb.Text, "E", ref dataInErr);
            }

            _inputData.l = Parameters.GetParam<double>(l_tb.Text, "l", ref dataInErr, NumberStyles.Integer);
        }

        if (stressHand_rb.Checked)
        {
            _inputData.Q = Parameters.GetParam<double>(Q_tb.Text, "Q", ref dataInErr);
            _inputData.M = Parameters.GetParam<double>(M_tb.Text, "M", ref dataInErr);
            _inputData.IsFTensile = forceStretch_rb.Checked;
            _inputData.F = Parameters.GetParam<double>(F_tb.Text, "[σ]", ref dataInErr);

            if (!_inputData.IsFTensile)
            {
                var idx = force_gb.Controls.OfType<RadioButton>().FirstOrDefault(rb => rb.Checked)?.Text;

                _inputData.FCalcSchema = Parameters.GetParam<int>(idx, "Тип сжимающего усилия", ref dataInErr, NumberStyles.Integer);

                switch (_inputData.FCalcSchema)
                {
                    case 5:
                        _inputData.q = Parameters.GetParam<double>(fq_tb.Text, "q", ref dataInErr);
                        break;
                    case 6:
                    case 7:
                        _inputData.f = Parameters.GetParam<double>(fq_tb.Text, "f", ref dataInErr);
                        break;
                }
            }
        }

        var isNoError = !dataInErr.Any() && _inputData.IsDataGood;

        if (!isNoError)
        {
            MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(_inputData.ErrorList)));
        }

        return isNoError;
    }

    private void PreCalc_b_Click(object sender, EventArgs e)
    {
        scalc_l.Text = string.Empty;
        calc_b.Enabled = false;

        if (!CollectDataForPreliminarilyCalculation()) return;

        ICalculatedElement cylinder;

        try
        {
            cylinder = GetCalculateService().Calculate(_inputData
                ?? throw new InvalidOperationException());
        }
        catch (CalculateException ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }

        if (cylinder.ErrorList.Any())
        {
            MessageBox.Show(string.Join<string>(Environment.NewLine, cylinder.ErrorList));
        }

        calc_b.Enabled = true;
        scalc_l.Text = $@"sp={((CylindricalShellCalculated)cylinder).s:f3} мм";
        p_d_l.Text =
            $@"pd={((CylindricalShellCalculated)cylinder).p_d:f2} МПа";

        MessageBox.Show(Resources.CalcComplete);
    }

    private void Calc_b_Click(object sender, EventArgs e)
    {
        scalc_l.Text = string.Empty;

        if (!CollectDataForFinishCalculation()) return;

        ICalculatedElement cylinder;

        try
        {
            cylinder = GetCalculateService().Calculate(_inputData
                ?? throw new InvalidOperationException());
        }
        catch (CalculateException ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }

        if (cylinder.ErrorList.Any())
        {
            MessageBox.Show(string.Join<string>(Environment.NewLine, cylinder.ErrorList));
        }

        scalc_l.Text = $@"sp={((CylindricalShellCalculated)cylinder).s:f3} мм";
        p_d_l.Text =
            $@"pd={((CylindricalShellCalculated)cylinder).p_d:f2} МПа";

        if (isNozzleCalculateCheckBox.Checked)
        {
            if (Owner is not MainForm mainForm) return;

            mainForm.NozzleForm = _formFactory.Create<NozzleForm>()
                ?? throw new InvalidOperationException($"Couldn't create {nameof(NozzleForm)}.");
            mainForm.NozzleForm.Owner = mainForm;
            mainForm.NozzleForm.Show(cylinder);
        }

        if (Owner is not MainForm main)
        {
            MessageBox.Show($"{nameof(MainForm)} error");
            return;
        }

        main.Word_lv.Items.Add(cylinder.ToString());
        main.ElementsCollection.Add(cylinder);

        MessageBox.Show(Resources.CalcComplete);
        Close();
    }

    private void CilForm_Load(object sender, EventArgs e)
    {
        var steels = _physicalDataService.GetSteels(SteelSource.G34233D1)
            .Select(s => s as object)
            .ToArray();

        steel_cb.Items.AddRange(steels);
        steel_cb.SelectedIndex = 0;

        var serviceNames = _calculateServices
            .Select(s => s.Name as object)
            .ToArray();
        Gost_cb.Items.AddRange(serviceNames);
        Gost_cb.SelectedIndex = 0;

        shell_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(Resources.Cil)
            ?? throw new InvalidOperationException());
        f_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(Resources.PC1)
            ?? throw new InvalidOperationException());
    }

    private void GetE_b_Click(object sender, EventArgs e)
    {
        try
        {
            var E = _physicalDataService.GetE(steel_cb.Text, Convert.ToInt32((string?)t_tb.Text), ESource.G34233D1);
            E_tb.Text = E.ToString("N");
        }
        catch (PhysicalDataException ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void GetFi_b_Click(object sender, EventArgs e)
    {
        var ff = new FiForm { Owner = this };
        ff.ShowDialog();
    }

    private void CilForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (sender is not CylindricalShellForm) return;

        if (Owner is MainForm { CylindricalForm: { } } main)
        {
            main.CylindricalForm = null;
        }
    }

    private void Stress_rb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is not RadioButton { Checked: true }) return;

        var isForceHand = stressHand_rb.Checked;
        force_gb.Enabled = isForceHand;
        M_gb.Enabled = isForceHand;
        Q_gb.Enabled = isForceHand;
    }

    private void Defect_chb_CheckedChanged(object sender, EventArgs e)
    {
        defect_b.Enabled = defect_chb.Checked;
    }

    private void ForceStretchCompress_rb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is not RadioButton { Checked: true }) return;

        var isCompress = forceCompress_rb.Checked;
        rb1.Enabled = isCompress;
        rb2.Enabled = isCompress;
        rb3.Enabled = isCompress;
        rb4.Enabled = isCompress;
        rb5.Enabled = isCompress;
        rb6.Enabled = isCompress;
        rb7.Enabled = isCompress;
    }

    private void SigmaHandle_cb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is CheckBox cb)
        {
            sigma_d_tb.Enabled = cb.Checked;
        }
    }

    private void EHandle_cb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is CheckBox cb)
        {
            E_tb.Enabled = cb.Checked;
        }
    }

    private void DisabledCalculateBtn(object sender, EventArgs e)
    {
        calc_b.Enabled = false;
    }
}