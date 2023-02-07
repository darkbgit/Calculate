using CalculateVessels.Core.Exceptions;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Elliptical;
using CalculateVessels.Core.Shells.Enums;
using CalculateVessels.Data.Enums;
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

public partial class EllipticalShellForm : Form
{
    private EllipticalShellInput? _inputData;
    private readonly IEnumerable<ICalculateService<EllipticalShellInput>> _calculateServices;
    private readonly IPhysicalDataService _physicalDataService;
    private readonly IFormFactory _formFactory;

    public EllipticalShellForm(IEnumerable<ICalculateService<EllipticalShellInput>> calculateServices,
        IFormFactory formFactory,
        IPhysicalDataService physicalDataService)
    {
        InitializeComponent();
        _calculateServices = calculateServices;
        _formFactory = formFactory;
        _physicalDataService = physicalDataService;
    }

    private ICalculateService<EllipticalShellInput> GetCalculateService()
    {
        return _calculateServices
                   .FirstOrDefault(s => s.Name == Gost_cb.Text)
               ?? throw new InvalidOperationException("Service wasn't found.");
    }

    private void EllForm_Load(object sender, EventArgs e)
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
    }

    private void Cancel_b_Click(object sender, EventArgs e)
    {
        this.Hide();
    }

    private void EllForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (sender is EllipticalShellForm && Owner is MainForm { EllipticalForm: { } } main)
        {
            main.EllipticalForm = null;
        }
    }

    private bool CollectDataForPreliminarilyCalculation()
    {
        List<string> dataInErr = new();

        _inputData = new EllipticalShellInput()
        {
            t = Parameters.GetParam<double>(t_tb.Text, "t", ref dataInErr, NumberStyles.Integer),
            Steel = steel_cb.Text,
            IsPressureIn = vn_rb.Checked,
            p = Parameters.GetParam<double>(p_tb.Text, "p", ref dataInErr),
            fi = Parameters.GetParam<double>(fi_tb.Text, "fi", ref dataInErr),
            D = Parameters.GetParam<double>(D_tb.Text, "D", ref dataInErr),
            EllipseH = Parameters.GetParam<double>(H_tb.Text, "H", ref dataInErr),
            Ellipseh1 = Parameters.GetParam<double>(h1_tb.Text, "h1", ref dataInErr),
            c1 = Parameters.GetParam<double>(c1_tb.Text, "c1", ref dataInErr),
            c2 = Parameters.GetParam<double>(c2_tb.Text, "c2", ref dataInErr),
            c3 = Parameters.GetParam<double>(c3_tb.Text, "c3", ref dataInErr),
            EllipticalBottomType =
                ell_rb.Checked ? EllipticalBottomType.Elliptical : EllipticalBottomType.Hemispherical,
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
        List<string> dataInErr = new();

        _inputData = new EllipticalShellInput()
        {
            Name = name_tb.Text,
            s = Parameters.GetParam<double>(s_tb.Text, "s", ref dataInErr),

            t = Parameters.GetParam<double>(t_tb.Text, "t", ref dataInErr, NumberStyles.Integer),
            Steel = steel_cb.Text,
            IsPressureIn = vn_rb.Checked,
            p = Parameters.GetParam<double>(p_tb.Text, "p", ref dataInErr),
            fi = Parameters.GetParam<double>(fi_tb.Text, "fi", ref dataInErr),
            D = Parameters.GetParam<double>(D_tb.Text, "D", ref dataInErr),
            EllipseH = Parameters.GetParam<double>(H_tb.Text, "H", ref dataInErr),
            Ellipseh1 = Parameters.GetParam<double>(h1_tb.Text, "h1", ref dataInErr),
            c1 = Parameters.GetParam<double>(c1_tb.Text, "c1", ref dataInErr),
            c2 = Parameters.GetParam<double>(c2_tb.Text, "c2", ref dataInErr),
            c3 = Parameters.GetParam<double>(c3_tb.Text, "c3", ref dataInErr),
            EllipticalBottomType =
                ell_rb.Checked ? EllipticalBottomType.Elliptical : EllipticalBottomType.Hemispherical,
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
        c_tb.Text = string.Empty;
        scalc_l.Text = string.Empty;
        calc_b.Enabled = false;

        if (!CollectDataForPreliminarilyCalculation()) return;

        ICalculatedElement ellipse;

        try
        {
            ellipse = GetCalculateService().Calculate(_inputData ?? throw new InvalidOperationException());
        }
        catch (CalculateException ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }


        if (ellipse.ErrorList.Any())
        {
            MessageBox.Show(string.Join<string>(Environment.NewLine, ellipse.ErrorList));
        }

        calc_b.Enabled = true;
        scalc_l.Text = $@"sp={((EllipticalShellCalculated)ellipse).s:f3} мм";
        p_d_l.Text =
            $@"pd={((EllipticalShellCalculated)ellipse).p_d:f2} МПа";
        MessageBox.Show(Resources.CalcComplete);
    }

    private void GetGostDim_b_Click(object sender, EventArgs e)
    {
        var ellipticalParametersForm = _formFactory.Create<EllipticalParametersForm>()
                                       ?? throw new InvalidOperationException($"Couldn't create {nameof(EllipticalParametersForm)}.");
        ellipticalParametersForm.Owner = this;
        ellipticalParametersForm.ShowDialog();
    }

    private void GetFi_b_Click(object sender, EventArgs e)
    {
        var fiForm = _formFactory.Create<FiForm>()
                                       ?? throw new InvalidOperationException($"Couldn't create {nameof(FiForm)}.");
        fiForm.Owner = this;
        fiForm.ShowDialog();
    }

    private void Calc_b_Click(object sender, EventArgs e)
    {
        c_tb.Text = "";
        scalc_l.Text = "";

        if (!CollectDataForFinishCalculation()) return;

        ICalculatedElement ellipse;

        try
        {
            ellipse = GetCalculateService().Calculate(_inputData
                                                      ?? throw new InvalidOperationException());
        }
        catch (CalculateException ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }

        if (ellipse.ErrorList.Any())
        {
            MessageBox.Show(string.Join<string>(Environment.NewLine, ellipse.ErrorList));
        }

        scalc_l.Text = $@"sp={((EllipticalShellCalculated)ellipse).s:f3} мм";
        p_d_l.Text =
            $@"pd={((EllipticalShellCalculated)ellipse).p_d:f2} МПа";


        if (isNozzleCalculateCheckBox.Checked)
        {
            if (Owner is not MainForm mainForm) return;

            mainForm.NozzleForm = _formFactory.Create<NozzleForm>()
                                  ?? throw new InvalidOperationException($"Couldn't create {nameof(NozzleForm)}.");
            mainForm.NozzleForm.Owner = mainForm;
            mainForm.NozzleForm.Show(ellipse);
        }

        if (Owner is not MainForm main)
        {
            MessageBox.Show($"{nameof(MainForm)} Error");
            return;

        }

        main.Word_lv.Items.Add(ellipse.ToString());
        main.ElementsCollection.Add(ellipse);

        MessageBox.Show(Resources.CalcComplete);
        Close();
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
}