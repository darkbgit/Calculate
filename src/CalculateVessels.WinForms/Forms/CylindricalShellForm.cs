using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CalculateVessels.Core.Elements.Shells.Cylindrical;
using CalculateVessels.Core.Enums;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Enums;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Data.Properties;
using CalculateVessels.Forms.MiddleForms;
using CalculateVessels.Helpers;
using FluentValidation;

namespace CalculateVessels.Forms;

public partial class CylindricalShellForm : CylindricalShellFormMiddle
{
    public CylindricalShellForm(IEnumerable<ICalculateService<CylindricalShellInput>> calculateServices,
        IPhysicalDataService physicalDataService,
        IValidator<CylindricalShellInput> validator,
        IFormFactory formFactory)
        : base(calculateServices, physicalDataService, validator, formFactory)
    {
        InitializeComponent();
    }


    protected override void LoadInputData(CylindricalShellInput inputData)
    {
        Name_tb.Text = inputData.Name;
        steel_cb.Text = inputData.Steel;
        fi_tb.Text = inputData.phi.ToString(CultureInfo.CurrentCulture);
        D_tb.Text = inputData.D.ToString(CultureInfo.CurrentCulture);
        c1_tb.Text = inputData.c1.ToString(CultureInfo.CurrentCulture);
        c2_tb.Text = inputData.c2.ToString(CultureInfo.CurrentCulture);
        c3_tb.Text = inputData.c3.ToString(CultureInfo.CurrentCulture);
        s_tb.Text = inputData.s.ToString(CultureInfo.CurrentCulture);

        if (inputData.LoadingConditions.Count() == 1)
        {
            loadingConditionControl.SetLoadingCondition(inputData.LoadingConditions.First());
        }
        else
        {
            loadingConditionsControl.SetLoadingConditions(inputData.LoadingConditions);
        }

        if (inputData.LoadingConditions.Any(lc => lc.PressureType == PressureType.Outside))
        {
            l_tb.Text = inputData.l.ToString(CultureInfo.CurrentCulture);
        }

        if (!(inputData.F > 0) && !(inputData.Q > 0) && !(inputData.M > 0)) return;

        stressHand_rb.Checked = true;

        Q_tb.Text = inputData.Q.ToString(CultureInfo.CurrentCulture);
        M_tb.Text = inputData.M.ToString(CultureInfo.CurrentCulture);
        F_tb.Text = inputData.F.ToString(CultureInfo.CurrentCulture);

        forceStretch_rb.Checked = inputData.IsFTensile;

        if (inputData.IsFTensile) return;

        force_gb.Controls
            .OfType<RadioButton>()
            .First(rb => rb.Text == inputData.FCalcSchema.ToString())
            .Checked = true;

        switch (inputData.FCalcSchema)
        {
            case 5:
                fq_tb.Text = inputData.q.ToString(CultureInfo.CurrentCulture);
                break;
            case 6:
            case 7:
                fq_tb.Text = inputData.f.ToString(CultureInfo.CurrentCulture);
                break;
        }
    }

    protected override string GetServiceName()
    {
        return Gost_cb.Text;
    }

    private void CylindricalShellForm_Load(object sender, EventArgs e)
    {
        LoadSteelsToComboBox(steel_cb, SteelSource.G34233D1);

        LoadCalculateServicesNamesToComboBox(Gost_cb);

        shell_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(Resources.Cil)
                                  ?? throw new InvalidOperationException());
        f_pb.Image = (Bitmap)(new ImageConverter().ConvertFrom(Resources.PC1)
                              ?? throw new InvalidOperationException());
    }

    private void CylindricalShellForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (sender is not CylindricalShellForm) return;

        if (Owner is not MainForm { CylindricalForm: not null } main) return;

        main.CylindricalForm = null;
    }

    private void Cancel_btn_Click(object sender, EventArgs e)
    {
        Hide();
    }

    protected override bool TryCollectInputData(out CylindricalShellInput inputData)
    {
        var dataInErr = new List<string>();

        inputData = new CylindricalShellInput
        {
            Name = Name_tb.Text,
            s = Parameters.GetParam<double>(s_tb.Text, "s", dataInErr),
            Steel = steel_cb.Text,
            phi = Parameters.GetParam<double>(fi_tb.Text, "φ", dataInErr),
            D = Parameters.GetParam<double>(D_tb.Text, "D", dataInErr),
            c1 = Parameters.GetParam<double>(c1_tb.Text, "c1", dataInErr),
            c2 = Parameters.GetParam<double>(c2_tb.Text, "c2", dataInErr),
            c3 = Parameters.GetParam<double>(c3_tb.Text, "c3", dataInErr),

        };

        var loadingConditions = FormHelpers.ParseLoadingConditions(loadingConditionsControl, loadingConditionControl).ToList();

        if (!loadingConditions.Any())
        {
            return false;
        }

        inputData.LoadingConditions = loadingConditions;

        if (inputData.LoadingConditions.Any(lc => lc.PressureType == PressureType.Outside))
        {
            inputData.l = Parameters.GetParam<double>(l_tb.Text, "l", dataInErr, NumberStyles.Integer);
        }

        if (stressHand_rb.Checked)
        {
            inputData.Q = Parameters.GetParam<double>(Q_tb.Text, "Q", dataInErr);
            inputData.M = Parameters.GetParam<double>(M_tb.Text, "M", dataInErr);
            inputData.IsFTensile = forceStretch_rb.Checked;
            inputData.F = Parameters.GetParam<double>(F_tb.Text, "[σ]", dataInErr);

            if (!inputData.IsFTensile)
            {
                var idx = force_gb.Controls.OfType<RadioButton>().FirstOrDefault(rb => rb.Checked)?.Text;

                inputData.FCalcSchema = Parameters.GetParam<int>(idx, "Тип сжимающего усилия", dataInErr, NumberStyles.Integer);

                switch (inputData.FCalcSchema)
                {
                    case 5:
                        inputData.q = Parameters.GetParam<double>(fq_tb.Text, "q", dataInErr);
                        break;
                    case 6:
                    case 7:
                        inputData.f = Parameters.GetParam<double>(fq_tb.Text, "f", dataInErr);
                        break;
                }
            }
        }

        if (!dataInErr.Any()) return true;

        MessageBox.Show(string.Join(Environment.NewLine, dataInErr));
        return false;
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

    private void PreCalculate_btn_Click(object sender, EventArgs e)
    {
        scalc_l.Text = string.Empty;
        calculate_btn.Enabled = false;

        if (!TryCalculate(out var cylinder)) return;

        if (cylinder == null) throw new NullReferenceException();

        scalc_l.Text = Gets(cylinder);
        p_d_l.Text = Getp(cylinder);

        calculate_btn.Enabled = true;
        MessageBox.Show(Resources.CalcComplete);
    }

    private void Calculate_btn_Click(object sender, EventArgs e)
    {
        scalc_l.Text = string.Empty;

        if (!TryCalculate(out var cylinder)) return;

        if (cylinder == null) throw new NullReferenceException();

        scalc_l.Text = Gets(cylinder);
        p_d_l.Text = Getp(cylinder);

        if (Owner is not MainForm mainForm)
        {
            MessageBox.Show($"{nameof(MainForm)} error");
            return;
        }

        if (isNozzleCalculateCheckBox.Checked)
        {
            mainForm.NozzleForm = FormFactory.Create<NozzleForm>()
                                  ?? throw new InvalidOperationException($"Couldn't create {nameof(NozzleForm)}.");
            mainForm.NozzleForm.Owner = mainForm;
            mainForm.NozzleForm.Show(cylinder);
        }

        SetCalculatedElementToStorage(Owner, cylinder);

        MessageBox.Show(Resources.CalcComplete);
        Close();
    }

    private void GetPhi_btn_Click(object sender, EventArgs e)
    {
        var phiForm = FormFactory.Create<FiForm>()
                      ?? throw new InvalidOperationException($"Couldn't create {nameof(FiForm)}.");
        phiForm.Owner = this;
        phiForm.ShowDialog();
    }

    private void Stress_rb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is not RadioButton { Checked: true }) return;

        var controls = new List<Control>
        {
            force_gb, M_gb, Q_gb
        };

        FormHelpers.EnabledIfCheck(controls, stressHand_rb.Checked);
    }

    private void Defect_chb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is CheckBox cb)
        {
            FormHelpers.EnabledIfCheck(defect_btn, cb.Checked);
        }
    }

    private void ForceStretchCompress_rb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is not RadioButton { Checked: true }) return;

        var rbs = force_gb.Controls
            .OfType<RadioButton>()
            .ToList();

        if (!rbs.Any()) return;

        FormHelpers.EnabledIfCheck(rbs, forceCompress_rb.Checked);
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
    .Join(", ", ((CylindricalShellCalculated)element).Results
        .Select((r, j) => $"s{j + 1}={r.s:f3} мм")
    .ToList());

    private static string Getp(ICalculatedElement element) => string
        .Join(", ", ((CylindricalShellCalculated)element).Results
            .Select((r, j) => $"p{j + 1}={r.p_d:f3} МПа")
            .ToList());
}