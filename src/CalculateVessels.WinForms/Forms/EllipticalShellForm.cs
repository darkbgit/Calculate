using CalculateVessels.Core.Exceptions;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Elliptical;
using CalculateVessels.Core.Shells.Enums;
using CalculateVessels.Data.PhysicalData.Gost34233_1;
using CalculateVessels.Data.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace CalculateVessels.Forms;

public partial class EllipticalShellForm : Form
{
    private EllipticalShellInput _inputData;
    private readonly ICalculateService<EllipticalShellInput> _calculateService;

    public EllipticalShellForm(ICalculateService<EllipticalShellInput> calculateService)
    {
        InitializeComponent();
        _calculateService = calculateService;
    }

    public IInputData InputData => _inputData;

    private void EllForm_Load(object sender, EventArgs e)
    {
        var steels = Gost34233_1.GetSteelsList()?.ToArray();
        if (steels != null)
        {
            steel_cb.Items.AddRange(steels);
            steel_cb.SelectedIndex = 0;
        }
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
        const string WRONG_INPUT = " неверный ввод";
        List<string> dataInErr = new();

        _inputData = new EllipticalShellInput();

        //t
        {
            if (double.TryParse(t_tb.Text, NumberStyles.Integer,
                    CultureInfo.InvariantCulture, out var t))
            {
                _inputData.t = t;
            }
            else
            {
                dataInErr.Add(nameof(t) + WRONG_INPUT);
            }
        }

        //steel
        _inputData.Steel = steel_cb.Text;

        //pressure
        _inputData.IsPressureIn = vn_rb.Checked;


        //[σ]
        if (sigmaHandle_cb.Checked)
        {
            if (double.TryParse(sigma_d_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var sigmaAllow))
            {
                _inputData.SigmaAllow = sigmaAllow;
            }
            else
            {
                dataInErr.Add("[σ]" + WRONG_INPUT);
            }
        }

        if (!_inputData.IsPressureIn)
        {
            //E
            if (EHandle_cb.Checked)
            {
                if (double.TryParse(sigma_d_tb.Text, NumberStyles.AllowDecimalPoint,
                        CultureInfo.InvariantCulture, out var E))
                {
                    _inputData.E = E;
                }
                else
                {
                    dataInErr.Add(nameof(E) + WRONG_INPUT);
                }
            }
        }

        //p
        {
            if (double.TryParse(p_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var p))
            {
                _inputData.p = p;
            }
            else
            {
                dataInErr.Add(nameof(p) + WRONG_INPUT);
            }
        }

        //fi
        {
            if (double.TryParse(fi_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var fi))
            {
                _inputData.fi = fi;
            }
            else
            {
                dataInErr.Add("φ" + WRONG_INPUT);
            }
        }

        //D
        {
            if (double.TryParse(D_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var D))
            {
                _inputData.D = D;
            }
            else
            {
                dataInErr.Add(nameof(D) + WRONG_INPUT);
            }
        }

        //H
        {
            if (double.TryParse(H_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var H))
            {
                _inputData.EllipseH = H;
            }
            else
            {
                dataInErr.Add(nameof(H) + WRONG_INPUT);
            }
        }

        //h1
        {
            if (double.TryParse(h1_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var h1))
            {
                _inputData.Ellipseh1 = h1;
            }
            else
            {
                dataInErr.Add(nameof(h1) + WRONG_INPUT);
            }
        }

        //c1
        {
            if (double.TryParse(c1_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var c1))
            {
                _inputData.c1 = c1;
            }
            else
            {
                dataInErr.Add(nameof(c1) + WRONG_INPUT);
            }
        }

        //c2
        {
            if (string.IsNullOrWhiteSpace(c2_tb.Text))
            {
                _inputData.c2 = 0;
            }
            else if (double.TryParse(c2_tb.Text, NumberStyles.AllowDecimalPoint,
                         CultureInfo.InvariantCulture, out var c2))
            {
                _inputData.c2 = c2;
            }
            else
            {
                dataInErr.Add(nameof(c2) + WRONG_INPUT);
            }
        }

        //c3
        {
            if (string.IsNullOrWhiteSpace(c3_tb.Text))
            {
                _inputData.c3 = 0;
            }
            else if (double.TryParse(c3_tb.Text, NumberStyles.AllowDecimalPoint,
                         CultureInfo.InvariantCulture, out var c3))
            {
                _inputData.c3 = c3;
            }
            else
            {
                dataInErr.Add(nameof(c3) + WRONG_INPUT);
            }
        }

        _inputData.EllipticalBottomType =
            ell_rb.Checked ? EllipticalBottomType.Elliptical : EllipticalBottomType.Hemispherical;


        var isNoError = !dataInErr.Any() && InputData.IsDataGood;

        if (!isNoError)
        {
            MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(_inputData.ErrorList)));
        }

        return isNoError;
    }

    private bool CollectDataForFinishCalculation()
    {
        List<string> dataInErr = new();

        //name
        _inputData.Name = name_tb.Text;

        //s
        {
            if (double.TryParse(s_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var s))
            {
                _inputData.s = s;
            }
            else
            {
                dataInErr.Add(nameof(s) + " неверный ввод");
            }
        }

        var isNoError = !dataInErr.Any() && InputData.IsDataGood;

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
            ellipse = _calculateService.Calculate(_inputData);
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
        try
        {
            GostEllForm gef = new() { Owner = this };
            gef.ShowDialog(); // показываем
        }
        catch (ArgumentNullException)
        {
            MessageBox.Show("Couldn't get ellipse parameters");
        }
    }

    private void GetFi_b_Click(object sender, EventArgs e)
    {
        FiForm ff = new() { Owner = this };
        ff.ShowDialog(); // показываем
    }

    private void Calc_b_Click(object sender, EventArgs e)
    {
        c_tb.Text = "";
        scalc_l.Text = "";

        if (!CollectDataForFinishCalculation()) return;

        ICalculatedElement ellipse;

        try
        {
            ellipse = _calculateService.Calculate(_inputData);
        }
        catch (CalculateException ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }

        if (Owner is MainForm main)
        {
            main.Word_lv.Items.Add(ellipse.ToString());
            main.ElementsCollection.Add(ellipse);

            //_form.Hide();
        }
        else
        {
            MessageBox.Show("MainForm Error");
            return;
        }

        if (ellipse.ErrorList.Any())
        {
            MessageBox.Show(string.Join<string>(Environment.NewLine, ellipse.ErrorList));
        }

        scalc_l.Text = $@"sp={((EllipticalShellCalculated)ellipse).s:f3} мм";
        p_d_l.Text =
            $@"pd={((EllipticalShellCalculated)ellipse).p_d:f2} МПа";

        //MessageBoxCheckBox needNozzleCalculate = new(ellipse) { Owner = this };
        //needNozzleCalculate.ShowDialog();

        MessageBox.Show(Resources.CalcComplete);
    }

    private void Ell_Hemispherical_rb_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is RadioButton { Checked: true } rb)
        {
            pictureBox.Image = rb.Name switch
            {
                "ell_rb" => (Bitmap)new ImageConverter().ConvertFrom(Resources.Ell),
                "hemispherical_rb" => (Bitmap)new ImageConverter().ConvertFrom(Resources.Sfer),
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