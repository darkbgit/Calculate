using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CalculateVessels.Core.Elements.Shells.Elliptical;
using CalculateVessels.Core.Elements.Shells.Enums;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Enums;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Data.Properties;
using CalculateVessels.Forms.MiddleForms;
using CalculateVessels.Helpers;
using FluentValidation;

namespace CalculateVessels.Forms;

public sealed partial class EllipticalShellForm : EllipticalShellFormMiddle
{
    public EllipticalShellForm(IEnumerable<ICalculateService<EllipticalShellInput>> calculateServices,
        IPhysicalDataService physicalDataService,
        IValidator<EllipticalShellInput> validator,
        IFormFactory formFactory)
        : base(calculateServices, physicalDataService, validator, formFactory)
    {
        InitializeComponent();
    }

    protected override void LoadInputData(EllipticalShellInput inputData)
    {
        name_tb.Text = inputData.Name;
        steel_cb.Text = inputData.Steel;
        fi_tb.Text = inputData.phi.ToString(CultureInfo.CurrentCulture);
        D_tb.Text = inputData.D.ToString(CultureInfo.CurrentCulture);
        c1_tb.Text = inputData.c1.ToString(CultureInfo.CurrentCulture);
        c2_tb.Text = inputData.c2.ToString(CultureInfo.CurrentCulture);
        c3_tb.Text = inputData.c3.ToString(CultureInfo.CurrentCulture);
        s_tb.Text = inputData.s.ToString(CultureInfo.CurrentCulture);
        H_tb.Text = inputData.EllipseH.ToString(CultureInfo.CurrentCulture);
        h1_tb.Text = inputData.Ellipseh1.ToString(CultureInfo.CurrentCulture);

        if (inputData.EllipticalBottomType == EllipticalBottomType.Elliptical)
        {
            ell_rb.Checked = true;
        }
        else
        {
            hemispherical_rb.Checked = true;
        }

        if (inputData.LoadingConditions.Count() == 1)
        {
            loadingConditionControl.SetLoadingCondition(inputData.LoadingConditions.First());
        }
        else
        {
            loadingConditionsControl.SetLoadingConditions(inputData.LoadingConditions);
        }
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
        Hide();
    }

    protected override bool TryCollectInputData(out EllipticalShellInput inputData)
    {
        List<string> dataInErr = new();

        inputData = new EllipticalShellInput
        {
            Name = name_tb.Text,
            s = Parameters.GetParam<double>(s_tb.Text, "s", dataInErr),
            Steel = steel_cb.Text,
            phi = Parameters.GetParam<double>(fi_tb.Text, "φ", dataInErr),
            D = Parameters.GetParam<double>(D_tb.Text, "D", dataInErr),
            EllipseH = Parameters.GetParam<double>(H_tb.Text, "H", dataInErr),
            Ellipseh1 = Parameters.GetParam<double>(h1_tb.Text, "h1", dataInErr),
            c1 = Parameters.GetParam<double>(c1_tb.Text, "c1", dataInErr),
            c2 = Parameters.GetParam<double>(c2_tb.Text, "c2", dataInErr),
            c3 = Parameters.GetParam<double>(c3_tb.Text, "c3", dataInErr),
            EllipticalBottomType =
                ell_rb.Checked ? EllipticalBottomType.Elliptical : EllipticalBottomType.Hemispherical
        };

        var loadingConditions = FormHelpers.ParseLoadingConditions(loadingConditionsControl, loadingConditionControl).ToList();

        if (!loadingConditions.Any())
        {
            return false;
        }

        inputData.LoadingConditions = loadingConditions;

        if (!dataInErr.Any()) return true;

        MessageBox.Show(string.Join(Environment.NewLine, dataInErr));
        return false;
    }

    private void PreCalculate_btn_Click(object sender, EventArgs e)
    {
        scalc_l.Text = string.Empty;
        calculate_btn.Enabled = false;

        if (!TryCalculate(out var ellipse)) return;

        if (ellipse == null) throw new NullReferenceException();

        scalc_l.Text = Gets(ellipse);
        p_d_l.Text = Getp(ellipse);

        calculate_btn.Enabled = true;
        MessageBox.Show(Resources.CalcComplete);
    }

    private void Calculate_btn_Click(object sender, EventArgs e)
    {
        scalc_l.Text = string.Empty;

        if (!TryCalculate(out var ellipse)) return;

        if (ellipse == null) throw new NullReferenceException();

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