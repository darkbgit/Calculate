using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Elliptical;
using CalculateVessels.Core.Shells.Enums;
using CalculateVessels.Data.Enums;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Data.Properties;
using CalculateVessels.Forms.MiddleForms;
using CalculateVessels.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CalculateVessels.Forms;

public sealed partial class EllipticalShellForm : EllipticalShellFormMiddle
{
    public EllipticalShellForm(IEnumerable<ICalculateService<EllipticalShellInput>> calculateServices,
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

    private void EllipticalShellForm_Load(object sender, EventArgs e)
    {
        LoadSteelsToComboBox(steel_cb, SteelSource.G34233D1);

        LoadCalculateServicesNamesToComboBox(Gost_cb);
    }

    private void EllipticalShellForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (sender is not EllipticalShellForm) return;

        if (Owner is not MainForm { EllipticalForm: not null } main) return;

        main.EllipticalForm = null;
    }

    private void Cancel_btn_Click(object sender, EventArgs e)
    {
        this.Hide();
    }

    protected override bool CollectDataForPreliminarilyCalculation()
    {
        List<string> dataInErr = new();

        InputData = new EllipticalShellInput
        {
            Steel = steel_cb.Text,
            phi = Parameters.GetParam<double>(fi_tb.Text, "φ", ref dataInErr),
            D = Parameters.GetParam<double>(D_tb.Text, "D", ref dataInErr),
            EllipseH = Parameters.GetParam<double>(H_tb.Text, "H", ref dataInErr),
            Ellipseh1 = Parameters.GetParam<double>(h1_tb.Text, "h1", ref dataInErr),
            c1 = Parameters.GetParam<double>(c1_tb.Text, "c1", ref dataInErr),
            c2 = Parameters.GetParam<double>(c2_tb.Text, "c2", ref dataInErr),
            c3 = Parameters.GetParam<double>(c3_tb.Text, "c3", ref dataInErr),
            EllipticalBottomType =
                ell_rb.Checked ? EllipticalBottomType.Elliptical : EllipticalBottomType.Hemispherical
        };

        var loadingConditions = FormHelpers.ParseLoadingConditions(loadingConditionsControl, loadingConditionGroupBox).ToList();

        if (!loadingConditions.Any())
        {
            return false;
        }

        InputData.LoadingConditions = loadingConditions;

        var isNoError = !dataInErr.Any() && InputData.IsDataGood;

        if (!isNoError)
        {
            MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(InputData.ErrorList)));
        }

        return isNoError;
    }

    protected override bool CollectDataForFinishCalculation()
    {
        List<string> dataInErr = new();

        InputData = new EllipticalShellInput
        {
            Name = name_tb.Text,
            s = Parameters.GetParam<double>(s_tb.Text, "s", ref dataInErr),
            Steel = steel_cb.Text,
            phi = Parameters.GetParam<double>(fi_tb.Text, "phi", ref dataInErr),
            D = Parameters.GetParam<double>(D_tb.Text, "D", ref dataInErr),
            EllipseH = Parameters.GetParam<double>(H_tb.Text, "H", ref dataInErr),
            Ellipseh1 = Parameters.GetParam<double>(h1_tb.Text, "h1", ref dataInErr),
            c1 = Parameters.GetParam<double>(c1_tb.Text, "c1", ref dataInErr),
            c2 = Parameters.GetParam<double>(c2_tb.Text, "c2", ref dataInErr),
            c3 = Parameters.GetParam<double>(c3_tb.Text, "c3", ref dataInErr),
            EllipticalBottomType =
                ell_rb.Checked ? EllipticalBottomType.Elliptical : EllipticalBottomType.Hemispherical
        };

        var loadingConditions = FormHelpers.ParseLoadingConditions(loadingConditionsControl, loadingConditionGroupBox).ToList();

        if (!loadingConditions.Any())
        {
            return false;
        }

        InputData.LoadingConditions = loadingConditions;

        var isNoError = !dataInErr.Any() && InputData.IsDataGood;

        if (!isNoError)
        {
            MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(InputData.ErrorList)));
        }

        return isNoError;
    }

    private void PreCalculate_btn_Click(object sender, EventArgs e)
    {
        scalc_l.Text = string.Empty;
        calculate_btn.Enabled = false;

        if (!CollectDataForPreliminarilyCalculation()) return;

        var ellipse = Calculate();

        if (ellipse == null) return;

        scalc_l.Text = Gets(ellipse);
        p_d_l.Text = Getp(ellipse);

        calculate_btn.Enabled = true;
        MessageBox.Show(Resources.CalcComplete);
    }

    private void Calculate_btn_Click(object sender, EventArgs e)
    {
        scalc_l.Text = string.Empty;

        if (!CollectDataForFinishCalculation()) return;

        var ellipse = Calculate();

        if (ellipse == null) return;

        scalc_l.Text = Gets(ellipse);
        p_d_l.Text = Getp(ellipse);

        if (isNozzleCalculateCheckBox.Checked)
        {
            if (Owner is not MainForm mainForm) return;

            mainForm.NozzleForm = FormFactory.Create<NozzleForm>()
                                  ?? throw new InvalidOperationException($"Couldn't create {nameof(NozzleForm)}.");
            mainForm.NozzleForm.Owner = mainForm;
            mainForm.NozzleForm.Show(ellipse);
        }

        if (Owner is not MainForm main)
        {
            MessageBox.Show($"{nameof(MainForm)} Error");
            return;
        }

        SetCalculatedElementToStorage(Owner, ellipse);

        MessageBox.Show(Resources.CalcComplete);
        Close();
    }

    private void GetGostDim_btn_Click(object sender, EventArgs e)
    {
        var ellipticalParametersForm = FormFactory.Create<EllipticalParametersForm>()
                                       ?? throw new InvalidOperationException($"Couldn't create {nameof(EllipticalParametersForm)}.");
        ellipticalParametersForm.Owner = this;
        ellipticalParametersForm.ShowDialog();
    }

    private void GetPhi_btn_Click(object sender, EventArgs e)
    {
        var phiForm = FormFactory.Create<FiForm>()
                                       ?? throw new InvalidOperationException($"Couldn't create {nameof(FiForm)}.");
        phiForm.Owner = this;
        phiForm.ShowDialog();
    }

    private void Ell_Hemispherical_rb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is RadioButton { Checked: true } rb)
        {
            pictureBox.Image = rb.Name switch
            {
                "ell_rb" => (Bitmap)(new ImageConverter().ConvertFrom(Resources.Ell)
                                     ?? throw new InvalidOperationException()),
                "hemispherical_rb" => (Bitmap)(new ImageConverter().ConvertFrom(Resources.Sfer)
                                               ?? throw new InvalidOperationException()),
                _ => pictureBox.Image
            };
        }
    }

    private void Defect_chb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is CheckBox cb)
        {
            FormHelpers.EnabledIfCheck(defect_btn, cb.Checked);
        }
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
        .Join(", ", ((EllipticalShellCalculated)element).Results
            .Select((r, j) => $"s{j + 1}={r.s:f3} мм")
            .ToList());

    private static string Getp(ICalculatedElement element) => string
        .Join(", ", ((EllipticalShellCalculated)element).Results
            .Select((r, j) => $"p{j + 1}={r.p_d:f3} МПа")
            .ToList());
}