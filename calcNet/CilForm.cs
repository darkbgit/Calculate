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

namespace CalculateVessels
{
    public partial class CilForm : Form
    {
        public CilForm()
        {
            InitializeComponent();
        }

        public IDataIn DataIn => _cylindricalShellDataIn;
        private CylindricalShellDataIn _cylindricalShellDataIn;

        private void Cancel_b_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void Force_rb_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
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
        }

        private void Pressure_rb(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                bool isPressureOut = nar_rb.Checked;
                l_tb.Enabled = isPressureOut;
                //l_tb.ReadOnly = isPressureIn;
                getE_b.Enabled = isPressureOut;
                getL_b.Enabled = isPressureOut;
            }
        }

        private void PreCalc_b_Click(object sender, EventArgs e)
        {
            c_tb.Text = "";
            scalc_l.Text = "";
            calc_b.Enabled = false;

            _cylindricalShellDataIn = new CylindricalShellDataIn();

            var dataInErr = new List<string>();

            //t
            {
                if (double.TryParse(t_tb.Text, System.Globalization.NumberStyles.Integer,
                    System.Globalization.CultureInfo.InvariantCulture, out double t))
                {
                    _cylindricalShellDataIn.t = t;
                }
                else
                {
                    dataInErr.Add("t неверный ввод");
                }
            }

            //steel
            _cylindricalShellDataIn.Steel = steel_cb.Text;

            //
            _cylindricalShellDataIn.IsPressureIn = vn_rb.Checked;

            //[σ]
            if (sigmaHandle_cb.Checked)
            {
                if (double.TryParse(sigma_d_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out  var sigmaAllow))
                {
                    _cylindricalShellDataIn.sigma_d = sigmaAllow;
                }
                else
                {
                    dataInErr.Add("[σ] неверный ввод");
                }
            }
            
            if (!_cylindricalShellDataIn.IsPressureIn)
            {
                //E
                if (EHandle_cb.Checked)
                {
                    if (double.TryParse(sigma_d_tb.Text, NumberStyles.AllowDecimalPoint,
                        CultureInfo.InvariantCulture, out var E))
                    {
                        _cylindricalShellDataIn.E = E;
                    }
                    else
                    {
                        dataInErr.Add("E неверный ввод");
                    }
                }


                //l
                //InputClass.GetInput_l(l_tb, ref d_in, ref dataInErr);
                {
                    if (double.TryParse(l_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                    System.Globalization.CultureInfo.InvariantCulture, out var l))
                    {
                        _cylindricalShellDataIn.l = l;
                    }
                    else
                    {
                        dataInErr.Add("l неверный ввод");
                    }
                }
            }

            //p
            //    InputClass.GetInput_p(p_tb, ref d_in, ref dataInErr);
            {
                if (double.TryParse(p_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.InvariantCulture, out var p))
                {
                    _cylindricalShellDataIn.p = p;
                }
                else
                {
                    dataInErr.Add("p неверный ввод");
                }
            }

            //fi
            //    InputClass.GetInput_fi(fi_tb, ref d_in, ref dataInErr);
            {
                if (double.TryParse(fi_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.InvariantCulture, out var fi))
                {
                    _cylindricalShellDataIn.fi = fi;
                }
                else
                {
                    dataInErr.Add("φ неверный ввод");
                }
            }

            //D
            //    InputClass.GetInput_D(D_tb, ref d_in, ref dataInErr);
            {
                if (double.TryParse(D_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.InvariantCulture, out var D))
                {
                    _cylindricalShellDataIn.D = D;
                }
                else
                {
                    dataInErr.Add("D неверный ввод");
                }
            }

            //c1
            //    InputClass.GetInput_c1(c1_tb, ref d_in, ref dataInErr);
            {
                if (double.TryParse(c1_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.InvariantCulture, out var c1))
                {
                    _cylindricalShellDataIn.c1 = c1;
                }
                else
                {
                    dataInErr.Add("c1 неверный ввод");
                }
            }

            //c2
            //    InputClass.GetInput_c2(c2_tb, ref d_in, ref dataInErr);
            {
                if (c2_tb.Text == "")
                {
                    _cylindricalShellDataIn.c2 = 0;
                }
                else if (double.TryParse(c2_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.InvariantCulture, out var c2))
                {
                    _cylindricalShellDataIn.c2 = c2;
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
                    _cylindricalShellDataIn.c3 = 0;
                }
                else if (double.TryParse(c3_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.InvariantCulture, out var c3))
                {
                    _cylindricalShellDataIn.c3 = c3;
                }
                else
                {
                    dataInErr.Add("c3 неверный ввод");
                }
            }


            var isNotError = !dataInErr.Any() && ((IDataIn)_cylindricalShellDataIn).IsDataGood;

            if (isNotError)
            {
                var cyl = new CylindricalShell(_cylindricalShellDataIn);
                cyl.Calculate();
                if (!cyl.IsCriticalError)
                {
                    c_tb.Text = $"{cyl.c:f2}";
                    scalc_l.Text = $"sp={cyl.s:f3} мм";
                    calc_b.Enabled = true;
                    if (cyl.IsError)
                    {
                        MessageBox.Show(string.Join<string>(Environment.NewLine, cyl.ErrorList));
                    }
                }
                else
                {
                    MessageBox.Show(string.Join<string>(Environment.NewLine, cyl.ErrorList));
                }
            }
            else
            {
                MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(_cylindricalShellDataIn.ErrorList)));
            }
        }

        private void Calc_b_Click(object sender, EventArgs e)
        {
            c_tb.Text = "";
            scalc_l.Text = "";

            //CylindricalShellDataIn cylindricalShellDataIn = new CylindricalShellDataIn();

            List<string> dataInErr = new();

            //name
            _cylindricalShellDataIn.Name = Name_tb.Text;

            //s
            {
                if (double.TryParse(s_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.InvariantCulture, out double s))
                {
                    _cylindricalShellDataIn.s = s;
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
                        _cylindricalShellDataIn.Q = Q;
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
                        _cylindricalShellDataIn.M = M;
                    }
                    else
                    {
                        dataInErr.Add("M неверный ввод");
                    }
                }

                _cylindricalShellDataIn.IsFTensile = forceStretch_rb.Checked;

                //F
                {
                    if (double.TryParse(F_tb.Text, NumberStyles.AllowDecimalPoint,
                        CultureInfo.InvariantCulture, out double F))
                    {
                        _cylindricalShellDataIn.F = F;
                    }
                    else
                    {
                        dataInErr.Add("F неверный ввод");
                    }
                }

                if (!_cylindricalShellDataIn.IsFTensile)
                {
                    var idx = force_gb.Controls.OfType<RadioButton>().FirstOrDefault(rb => rb.Checked)?.Text;
                    if (int.TryParse(idx, NumberStyles.Integer, CultureInfo.InvariantCulture, out int i))
                    {
                        _cylindricalShellDataIn.FCalcSchema = i;

                        switch (i)
                        {
                            case 5:
                                if (double.TryParse(F_tb.Text, NumberStyles.AllowDecimalPoint,
                                    CultureInfo.InvariantCulture, out double q))
                                {
                                    _cylindricalShellDataIn.q = q;
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
                                    _cylindricalShellDataIn.f = f;
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

            bool isNotError = !dataInErr.Any() && DataIn.IsDataGood;

            if (isNotError)
            {
                IElement cylinder = new CylindricalShell(_cylindricalShellDataIn);

                CalculatedElement calculatedElement = new(cylinder);

                calculatedElement.Element.Calculate();

                if (!calculatedElement.Element.IsCriticalError)
                {
                    scalc_l.Text = $"sp={((CylindricalShell)calculatedElement.Element).s:f3} мм";
                    p_d_l.Text =
                        $"pd={((CylindricalShell)calculatedElement.Element).p_d:f2} МПа";

                    if (this.Owner is MainForm main)
                    {
                        main.Word_lv.Items.Add(calculatedElement.Element.ToString());
                        main.ElementsCollection.Add(calculatedElement);

                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("MainForm Error");
                    }

                    if (calculatedElement.Element.IsError)
                    {
                        MessageBox.Show(string.Join<string>(Environment.NewLine, calculatedElement.Element.ErrorList));
                    }

                    MessageBox.Show("Calculation complete");

                    MessageBoxCheckBox mbcb = new(calculatedElement.Element, _cylindricalShellDataIn) { Owner = this };
                    mbcb.ShowDialog();

                }
                else
                {
                    MessageBox.Show(string.Join<string>(Environment.NewLine, calculatedElement.Element.ErrorList));
                }
            }
            else
            {
                MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(_cylindricalShellDataIn.ErrorList)));
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
            List<string> dataInErr = new();
            double E = 0.0;
            if (Physical.TryGetE(steel_cb.Text, Convert.ToInt32(t_tb.Text), ref E, ref dataInErr))
            {
                E_tb.Text = E.ToString();
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
            var rb = sender as RadioButton;
            if (rb.Checked)
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
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                bool isCompress = forceCompress_rb.Checked;
                rb1.Enabled = isCompress;
                rb2.Enabled = isCompress;
                rb3.Enabled = isCompress;
                rb4.Enabled = isCompress;
                rb5.Enabled = isCompress;
                rb6.Enabled = isCompress;
                rb7.Enabled = isCompress;
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

        private void DisabledCalculateBtn(object sender, EventArgs e)
        {
            calc_b.Enabled = false;
        }
    }
}

