using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Models;
using CalculateVessels.Core.Shells;
using CalculateVessels.Core.Shells.DataIn;
using CalculateVessels.Data.PhysicalData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CalculateVessels.Core.Exceptions;
using CalculateVessels.Core.Shells.CylindricalShell;
using CalculateVessels.Data.Properties;

namespace CalculateVessels
{
    public partial class CilForm : Form
    {
        private CylindricalShellInputData _inputData;

        public CilForm()
        {
            InitializeComponent();
        }

        public IInputData InputData => _inputData;
        

        private void Cancel_b_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void Force_rb_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is not RadioButton {Checked: true} rb) return;

            f_pb.Image = (Bitmap)new ImageConverter().ConvertFrom(Data.Properties.Resources.ResourceManager.GetObject("PC" + rb.Text));
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
            if (sender is not RadioButton {Checked: true}) return;

            bool isPressureOut = nar_rb.Checked;
            l_tb.Enabled = isPressureOut;
            //l_tb.ReadOnly = isPressureIn;
            getE_b.Enabled = isPressureOut;
            getL_b.Enabled = isPressureOut;
        }

        private void PreCalc_b_Click(object sender, EventArgs e)
        {
            c_tb.Text = "";
            scalc_l.Text = "";
            calc_b.Enabled = false;

            _inputData = new CylindricalShellInputData();

            var dataInErr = new List<string>();

            //t
            {
                if (double.TryParse(t_tb.Text, System.Globalization.NumberStyles.Integer,
                    System.Globalization.CultureInfo.InvariantCulture, out double t))
                {
                    _inputData.t = t;
                }
                else
                {
                    dataInErr.Add("t неверный ввод");
                }
            }

            //steel
            _inputData.Steel = steel_cb.Text;

            //
            _inputData.IsPressureIn = vn_rb.Checked;

            //[σ]
            if (sigmaHandle_cb.Checked)
            {
                if (double.TryParse(sigma_d_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out  var sigmaAllow))
                {
                    _inputData.SigmaAllow = sigmaAllow;
                }
                else
                {
                    dataInErr.Add("[σ] неверный ввод");
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
                        dataInErr.Add("E неверный ввод");
                    }
                }


                //l
                {
                    if (double.TryParse(l_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                    System.Globalization.CultureInfo.InvariantCulture, out var l))
                    {
                        _inputData.l = l;
                    }
                    else
                    {
                        dataInErr.Add("l неверный ввод");
                    }
                }
            }

            //p
            {
                if (double.TryParse(p_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.InvariantCulture, out var p))
                {
                    _inputData.p = p;
                }
                else
                {
                    dataInErr.Add("p неверный ввод");
                }
            }

            //fi
            {
                if (double.TryParse(fi_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.InvariantCulture, out var fi))
                {
                    _inputData.fi = fi;
                }
                else
                {
                    dataInErr.Add("φ неверный ввод");
                }
            }

            //D
            {
                if (double.TryParse(D_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.InvariantCulture, out var D))
                {
                    _inputData.D = D;
                }
                else
                {
                    dataInErr.Add("D неверный ввод");
                }
            }

            //c1
            {
                if (double.TryParse(c1_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.InvariantCulture, out var c1))
                {
                    _inputData.c1 = c1;
                }
                else
                {
                    dataInErr.Add("c1 неверный ввод");
                }
            }

            //c2
            {
                if (c2_tb.Text == "")
                {
                    _inputData.c2 = 0;
                }
                else if (double.TryParse(c2_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.InvariantCulture, out var c2))
                {
                    _inputData.c2 = c2;
                }
                else
                {
                    dataInErr.Add("c2 неверный ввод");
                }
            }

            //c3
            {
                if (c3_tb.Text == "")
                {
                    _inputData.c3 = 0;
                }
                else if (double.TryParse(c3_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.InvariantCulture, out var c3))
                {
                    _inputData.c3 = c3;
                }
                else
                {
                    dataInErr.Add("c3 неверный ввод");
                }
            }


            var isNotError = !dataInErr.Any() && ((IInputData)_inputData).IsDataGood;

            if (isNotError)
            {
                IElement cylinder = new CylindricalShell(_inputData);

                try
                {
                    cylinder.Calculate();
                }
                catch (CalculateException ex)
                {
                    MessageBox.Show(ex.Message);
                }

                if (cylinder.IsCalculated)
                {
                    if (cylinder.CalculatedData.ErrorList.Any())
                    {
                        MessageBox.Show(string.Join<string>(Environment.NewLine, cylinder.CalculatedData.ErrorList));
                    }

                    calc_b.Enabled = true;
                    scalc_l.Text = $@"sp={((CylindricalShellCalculatedData)cylinder.CalculatedData).s:f3} мм";
                    p_d_l.Text =
                        $@"pd={((CylindricalShellCalculatedData)cylinder.CalculatedData).p_d:f2} МПа";
                    MessageBox.Show(Resources.CalcComplete);
                }
            }
            else
            {
                MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(_inputData.ErrorList)));
            }
        }

        private void Calc_b_Click(object sender, EventArgs e)
        {
            c_tb.Text = "";
            scalc_l.Text = "";

            //CylindricalShellDataIn cylindricalShellDataIn = new CylindricalShellDataIn();

            List<string> dataInErr = new();

            //name
            _inputData.Name = Name_tb.Text;

            //s
            {
                if (double.TryParse(s_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.InvariantCulture, out double s))
                {
                    _inputData.s = s;
                }
                else
                {
                    dataInErr.Add("s неверный ввод");
                }
            }

            if (stressHand_rb.Checked)
            {
                //Q
                {
                    if (double.TryParse(Q_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.InvariantCulture, out double Q))
                    {
                        _inputData.Q = Q;
                    }
                    else
                    {
                        dataInErr.Add("Q неверный ввод");
                    }
                }

                //M
                {
                    if (double.TryParse(M_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.InvariantCulture, out double M))
                    {
                        _inputData.M = M;
                    }
                    else
                    {
                        dataInErr.Add("M неверный ввод");
                    }
                }

                _inputData.IsFTensile = forceStretch_rb.Checked;

                //F
                {
                    if (double.TryParse(F_tb.Text, NumberStyles.AllowDecimalPoint,
                        CultureInfo.InvariantCulture, out double F))
                    {
                        _inputData.F = F;
                    }
                    else
                    {
                        dataInErr.Add("F неверный ввод");
                    }
                }

                if (!_inputData.IsFTensile)
                {
                    var idx = force_gb.Controls.OfType<RadioButton>().FirstOrDefault(rb => rb.Checked)?.Text;
                    if (int.TryParse(idx, NumberStyles.Integer, CultureInfo.InvariantCulture, out int i))
                    {
                        _inputData.FCalcSchema = i;

                        switch (i)
                        {
                            case 5:
                                if (double.TryParse(F_tb.Text, NumberStyles.AllowDecimalPoint,
                                    CultureInfo.InvariantCulture, out double q))
                                {
                                    _inputData.q = q;
                                }
                                else
                                {
                                    dataInErr.Add("q неверный ввод");
                                }
                                break;
                            case 6:
                            case 7:
                                if (double.TryParse(F_tb.Text, NumberStyles.AllowDecimalPoint,
                                    CultureInfo.InvariantCulture, out double f))
                                {
                                    _inputData.f = f;
                                }
                                else
                                {
                                    dataInErr.Add("f неверный ввод");
                                }
                                break;
                        }
                    }
                    else
                    {
                        dataInErr.Add("Не возможно определить тип сжимающего усилия");
                    }
                }
            }

            bool isNotError = !dataInErr.Any() && InputData.IsDataGood;

            if (isNotError)
            {
                IElement cylinder = new CylindricalShell(_inputData);

                try
                {
                    cylinder.Calculate();
                }
                catch (CalculateException ex)
                {
                    MessageBox.Show(ex.Message);
                }

                if (Owner is MainForm main)
                {
                    main.Word_lv.Items.Add(cylinder.ToString());
                    main.ElementsCollection.Add(cylinder);

                    //_form.Hide();
                }
                else
                {
                    MessageBox.Show("MainForm Error");
                }

                if (cylinder.IsCalculated)
                {
                    if (cylinder.CalculatedData.ErrorList.Any())
                    {
                        MessageBox.Show(string.Join<string>(Environment.NewLine, cylinder.CalculatedData.ErrorList));
                    }

                    scalc_l.Text = $@"sp={((CylindricalShellCalculatedData)cylinder.CalculatedData).s:f3} мм";
                    p_d_l.Text =
                        $@"pd={((CylindricalShellCalculatedData)cylinder.CalculatedData).p_d:f2} МПа";

                    MessageBoxCheckBox needNozzleCalculate = new(cylinder) { Owner = this };
                    needNozzleCalculate.ShowDialog();
                    
                    MessageBox.Show(Resources.CalcComplete);
                }
            }
            else
            {
                MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(_inputData.ErrorList)));
            }
        }

        private void CilForm_Load(object sender, EventArgs e)
        {
            var steels = Physical.Gost34233_1.GetSteelsList()?.ToArray();
            if (steels != null)
            {
                steel_cb.Items.AddRange(steels);
                steel_cb.SelectedIndex = 0;
            }
            Gost_cb.SelectedIndex = 0;
            shell_pb.Image = (Bitmap)new ImageConverter().ConvertFrom(Data.Properties.Resources.Cil);
            f_pb.Image = (Bitmap)new ImageConverter().ConvertFrom(Data.Properties.Resources.PC1);
        }

        private void GetE_b_Click(object sender, EventArgs e)
        {
            try
            {
                double E = Physical.GetE(steel_cb.Text, Convert.ToInt32(t_tb.Text));
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
            ff.ShowDialog(); // показываем
        }

        private void CilForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sender is not CilForm) return;

            if (Owner is MainForm { cylindricalForm: { } } main)
            {
                main.cylindricalForm = null;
            }
        }

        private void Stress_rb_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is RadioButton {Checked: true})
            {
                bool isForceHand = stressHand_rb.Checked;
                force_gb.Enabled = isForceHand;
                M_gb.Enabled = isForceHand;
                Q_gb.Enabled = isForceHand;
            }
        }

        private void Defect_chb_CheckedChanged(object sender, EventArgs e)
        {
            defect_b.Enabled = defect_chb.Checked;
        }

        private void ForceStretchCompress_rb_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is not RadioButton {Checked: true}) return;

            bool isCompress = forceCompress_rb.Checked;
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
}

