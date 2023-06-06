using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Base;
using CalculateVessels.Core.Shells.Cylindrical;
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

public partial class CylindricalShellForm : CylindricalShellFormMiddle
{
    public CylindricalShellForm(IEnumerable<ICalculateService<CylindricalShellInput>> calculateServices,
        IPhysicalDataService physicalDataService,
        IFormFactory formFactory)
        : base(calculateServices, physicalDataService, formFactory)
    {
        InitializeComponent();
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

    protected override bool CollectDataForPreliminarilyCalculation()
    {

        var dataInErr = new List<string>();

        InputData = new CylindricalShellInput
        {
            Steel = steel_cb.Text,
            phi = Parameters.GetParam<double>(fi_tb.Text, "phi", ref dataInErr),
            D = Parameters.GetParam<double>(D_tb.Text, "D", ref dataInErr),
            c1 = Parameters.GetParam<double>(c1_tb.Text, "c1", ref dataInErr),
            c2 = Parameters.GetParam<double>(c2_tb.Text, "c2", ref dataInErr),
            c3 = Parameters.GetParam<double>(c3_tb.Text, "c3", ref dataInErr),
        };

        var loadingConditions = FormHelpers.ParseLoadingConditions(loadingConditionsControl, loadingConditionGroupBox).ToList();

        if (!loadingConditions.Any())
        {
            return false;
        }

        InputData.LoadingConditions = loadingConditions;

        if (InputData.LoadingConditions.Any(lc => !lc.IsPressureIn))
        {
            InputData.l = Parameters.GetParam<double>(l_tb.Text, "l", ref dataInErr, NumberStyles.Integer);
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
        var dataInErr = new List<string>();

        InputData = new CylindricalShellInput
        {
            Name = Name_tb.Text,
            s = Parameters.GetParam<double>(s_tb.Text, "s", ref dataInErr),
            Steel = steel_cb.Text,
            phi = Parameters.GetParam<double>(fi_tb.Text, "φ", ref dataInErr),
            D = Parameters.GetParam<double>(D_tb.Text, "D", ref dataInErr),
            c1 = Parameters.GetParam<double>(c1_tb.Text, "c1", ref dataInErr),
            c2 = Parameters.GetParam<double>(c2_tb.Text, "c2", ref dataInErr),
            c3 = Parameters.GetParam<double>(c3_tb.Text, "c3", ref dataInErr),

        };

        if (!loadingConditionsControl.Any())
        {
            var loadingCondition = loadingConditionGroupBox.GetLoadingCondition();

            if (loadingCondition == null)
            {
                return false;
            }

            InputData.LoadingConditions = new List<LoadingCondition>
            {
                loadingCondition
            };
        }
        else
        {
            var loadingConditions = loadingConditionsControl
                .GetLoadingConditions()
                .ToList();

            if (!loadingConditions.Any())
            {
                return false;
            }

            InputData.LoadingConditions = loadingConditions;
        }

        if (InputData.LoadingConditions.Any(lc => !lc.IsPressureIn))
        {
            InputData.l = Parameters.GetParam<double>(l_tb.Text, "l", ref dataInErr, NumberStyles.Integer);
        }

        if (stressHand_rb.Checked)
        {
            InputData.Q = Parameters.GetParam<double>(Q_tb.Text, "Q", ref dataInErr);
            InputData.M = Parameters.GetParam<double>(M_tb.Text, "M", ref dataInErr);
            InputData.IsFTensile = forceStretch_rb.Checked;
            InputData.F = Parameters.GetParam<double>(F_tb.Text, "[σ]", ref dataInErr);

            if (!InputData.IsFTensile)
            {
                var idx = force_gb.Controls.OfType<RadioButton>().FirstOrDefault(rb => rb.Checked)?.Text;

                InputData.FCalcSchema = Parameters.GetParam<int>(idx, "Тип сжимающего усилия", ref dataInErr, NumberStyles.Integer);

                switch (InputData.FCalcSchema)
                {
                    case 5:
                        InputData.q = Parameters.GetParam<double>(fq_tb.Text, "q", ref dataInErr);
                        break;
                    case 6:
                    case 7:
                        InputData.f = Parameters.GetParam<double>(fq_tb.Text, "f", ref dataInErr);
                        break;
                }
            }
        }

        var isNoError = !dataInErr.Any() && InputData.IsDataGood;

        if (!isNoError)
        {
            MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(InputData.ErrorList)));
        }

        return isNoError;
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

    private void OutsidePressureChecked_cb(object sender, EventArgs e)
    {
        if (sender is CheckBox cb)
        {
            FormHelpers.EnabledIfCheck(l_tb, cb.Checked);
        }
    }

    private void PreCalculate_btn_Click(object sender, EventArgs e)
    {
        scalc_l.Text = string.Empty;
        calculate_btn.Enabled = false;

        if (!CollectDataForPreliminarilyCalculation()) return;

        var cylinder = Calculate();

        if (cylinder == null) return;

        scalc_l.Text = Gets(cylinder);
        p_d_l.Text = Getp(cylinder);

        calculate_btn.Enabled = true;
        MessageBox.Show(Resources.CalcComplete);
    }

    private void Calculate_btn_Click(object sender, EventArgs e)
    {
        scalc_l.Text = string.Empty;

        if (!CollectDataForFinishCalculation()) return;

        var cylinder = Calculate();

        if (cylinder == null) return;

        scalc_l.Text = Gets(cylinder);
        p_d_l.Text = Getp(cylinder);

        if (isNozzleCalculateCheckBox.Checked)
        {
            if (Owner is not MainForm mainForm) return;

            mainForm.NozzleForm = FormFactory.Create<NozzleForm>()
                ?? throw new InvalidOperationException($"Couldn't create {nameof(NozzleForm)}.");
            mainForm.NozzleForm.Owner = mainForm;
            mainForm.NozzleForm.Show(cylinder);
        }

        if (Owner is not MainForm main)
        {
            MessageBox.Show($"{nameof(MainForm)} error");
            return;
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