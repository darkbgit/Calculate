﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CalculateVessels.Core.Shells;
using CalculateVessels.Core.Shells.DataIn;
using CalculateVessels.Data.PhysicalData;

namespace CalculateVessels
{
    public partial class CilForm : Form
    {
        public CilForm()
        {
            InitializeComponent();
        }

        private CylindricalShellDataIn cylindricalShellDataIn;

        private void Cancel_b_Click(object sender, EventArgs e)
        {
            //pictureBox1.Image.Dispose();
            //f_pb.Image.Dispose();
            this.Hide();
        }

        private void Force_rb_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                f_pb.Image = (Bitmap)new ImageConverter().ConvertFrom(CalculateVessels.Data.Properties.Resources.ResourceManager.GetObject("PC" + rb.Text));
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
                f_pb.Visible = isPressureOut;
            }
        }

        private void PredCalc_b_Click(object sender, EventArgs e)
        {
            c_tb.Text = "";
            scalc_l.Text = "";
            calc_b.Enabled = false;


            cylindricalShellDataIn = new CylindricalShellDataIn();


            var dataInErr = new List<string>();

            //t
            {
                if (double.TryParse(t_tb.Text, System.Globalization.NumberStyles.Integer,
                    System.Globalization.CultureInfo.InvariantCulture, out double t))
                {
                    cylindricalShellDataIn.t = t;
                }
                else
                {
                    dataInErr.Add("t неверный ввод");
                }
            }

            //steel
            cylindricalShellDataIn.Steel = steel_cb.Text;

            //
            cylindricalShellDataIn.IsPressureIn = vn_rb.Checked;


            if (cylindricalShellDataIn.IsDataGood)
            {
                //[σ]
                //InputClass.GetInput_sigma_d(sigma_d_tb, ref d_in, ref dataInErr);
                {
                    double sigma_d;
                    if (sigma_d_tb.ReadOnly)
                    {
                        sigma_d = Physical.GetSigma(cylindricalShellDataIn.Steel,
                                                    cylindricalShellDataIn.t,
                                                    ref dataInErr);
                        sigma_d_tb.ReadOnly = false;
                        sigma_d_tb.Text = sigma_d.ToString(CultureInfo.CurrentCulture);
                        sigma_d_tb.ReadOnly = true;
                    }
                    else
                    {
                        if (!double.TryParse(sigma_d_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                        System.Globalization.CultureInfo.InvariantCulture, out sigma_d))
                        {
                            dataInErr.Add("[σ] неверный ввод");
                        }
                    }
                    cylindricalShellDataIn.sigma_d = sigma_d;
                }


                if (!cylindricalShellDataIn.IsPressureIn)
                {
                    //E
                    //InputClass.GetInput_E(E_tb, ref d_in, ref dataInErr);
                    {
                        double E;
                        if (E_tb.ReadOnly)
                        {
                            E = Physical.GetE(cylindricalShellDataIn.Steel,
                                                cylindricalShellDataIn.t,
                                                ref dataInErr);
                            E_tb.ReadOnly = false;
                            E_tb.Text = E.ToString();
                            E_tb.ReadOnly = true;
                        }
                        else
                        {

                            if (double.TryParse(E_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                            System.Globalization.CultureInfo.InvariantCulture, out E))
                            {
                                cylindricalShellDataIn.E = E;
                            }
                            else
                            {
                                dataInErr.Add("E неверный ввод");
                            }
                        }
                    }

                    //l
                    //InputClass.GetInput_l(l_tb, ref d_in, ref dataInErr);
                    {
                        if (double.TryParse(l_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                        System.Globalization.CultureInfo.InvariantCulture, out var l))
                        {
                            cylindricalShellDataIn.l = l;
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
                        cylindricalShellDataIn.p = p;
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
                        cylindricalShellDataIn.fi = fi;
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
                        cylindricalShellDataIn.D = D;
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
                        cylindricalShellDataIn.c1 = c1;
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
                        cylindricalShellDataIn.c2 = 0;
                    }
                    else if (double.TryParse(c2_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                    System.Globalization.CultureInfo.InvariantCulture, out var c2))
                    {
                        cylindricalShellDataIn.c2 = c2;
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
                        cylindricalShellDataIn.c3 = 0;
                    }
                    else if (double.TryParse(c3_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                    System.Globalization.CultureInfo.InvariantCulture, out var c3))
                    {
                        cylindricalShellDataIn.c3 = c3;
                    }
                    else
                    {
                        dataInErr.Add("c3 неверный ввод");
                    }
                }


                {
                    //   bool isNotError = true;
                    //{
                    //    string[] TextBoxNames =
                    //    {
                    //        "Name_tb",
                    //        "Gost_cb",
                    //        "t_tb",
                    //        "p_tb",
                    //        "Steel_cb",
                    //        "sigma_d_tb",
                    //        "E_tb",
                    //        "fi_tb",
                    //        "D_tb",
                    //        "l_tb",
                    //        "c1_tb",
                    //        "c2_tb",
                    //        "c3_tb",
                    //        "s_tb"
                    //    };
                    //    foreach (string tb in TextBoxNames)
                    //    {
                    //        if (tb == "sigma_d_tb")
                    //        {
                    //            double sigma = 0;
                    //            string str = "";
                    //            if (CalcClass.GetSigma(d_in.Steel, d_in.t, ref sigma, ref str))
                    //            {
                    //                sigma_d_tb.Text = sigma.ToString();
                    //            }
                    //            else
                    //            {
                    //                isNotError = false;
                    //                MessageBox.Show(str);
                    //                break;
                    //            }
                    //        }

                    //        Control control = Controls[tb] ?? Controls["dav_gb"].Controls[tb];
                    //        //if (control is TextBox || control is ComboBox)

                    //        string name = control.Name;
                    //        name = name.Remove(name.Length - 3, 3);


                    //        var type = typeof(ShellDataIn).GetProperty(name)?.PropertyType;

                    //        double val = 0;
                    //        if (type != null && type.Equals(typeof(double)))
                    //        {
                    //            if (control.Text == "")
                    //            {
                    //                //try
                    //                //{
                    //                //    d_in.SetValue(name, val);
                    //                //}
                    //                ////catch (ArgumentOutOfRangeException ex)
                    //                ////{
                    //                ////    isNotError = false;
                    //                ////    MessageBox.Show(ex.Message);
                    //                ////    break;
                    //                ////}

                    //            }
                    //            else if (double.TryParse((control as TextBox).Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                    //                    System.Globalization.CultureInfo.InvariantCulture, out val))
                    //            {
                    //                //InputClass.TrySetValue(in name, in val, ref d_in, ref isNotError);
                    //                try
                    //                {
                    //                    d_in.SetValue(name, val);
                    //                }
                    //                catch (ArgumentOutOfRangeException ex)
                    //                {
                    //                    isNotError = false;
                    //                    MessageBox.Show(ex.Message);
                    //                    break;
                    //                }
                    //            }
                    //            else
                    //            {
                    //                isNotError = false;
                    //                MessageBox.Show($"{name} неверный формат");
                    //                break;
                    //            }
                    //        }
                    //        else if (type != null && type.Equals(typeof(string)))
                    //        {
                    //            d_in.SetValue(name, control.Text);
                    //        }
                    //        //MessageBox.Show($"{name} is {val} text {control.Text}");
                    //    }

                    //}
                }


                var isNotError = dataInErr.Count == 0 && cylindricalShellDataIn.IsDataGood;

                if (isNotError)
                {
                    var cyl = new CylindricalShell(cylindricalShellDataIn);
                    cyl.Calculate();
                    if (!cyl.IsCriticalError)
                    {
                        c_tb.Text = $"{cyl.c:f2}";
                        scalc_l.Text = $"sp={cyl.s_calc:f3} мм";
                        calc_b.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show(string.Join<string>(Environment.NewLine, cyl.ErrorList));
                    }
                }
                else
                {
                    MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(cylindricalShellDataIn.ErrorList)));
                }
            }
            else
            {
                MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr));
            }
        }

        private void Calc_b_Click(object sender, EventArgs e)
        {
            c_tb.Text = "";
            scalc_l.Text = "";

            //CylindricalShellDataIn cylindricalShellDataIn = new CylindricalShellDataIn();

            List<string> dataInErr = new List<string>();

            //name
            cylindricalShellDataIn.Name = Name_tb.Text;

            //s
            {
                if (double.TryParse(s_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.InvariantCulture, out double s))
                {
                    cylindricalShellDataIn.s = s;
                }
                else
                {
                    dataInErr.Add("s неверный ввод");
                }
            }

            bool isNotError = dataInErr.Count == 0 && cylindricalShellDataIn.IsDataGood;

            if (isNotError)
            {
                CylindricalShell cylindricalShell = new(cylindricalShellDataIn);
                cylindricalShell.Calculate();
                if (!cylindricalShell.IsCriticalError)
                {
                    scalc_l.Text = $"sp={cylindricalShell.s_calc:f3} мм";
                    p_d_l.Text = $"pd={cylindricalShell.p_d:f3} МПа";

                    if (this.Owner is MainForm main)
                    {
                        main.Word_lv.Items.Add(cylindricalShell.ToString());
                        Elements.ElementsList.Add(cylindricalShell);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("MainForm Error");
                    }

                    if (cylindricalShell.IsError)
                    {
                        MessageBox.Show(string.Join<string>(Environment.NewLine, cylindricalShell.ErrorList));
                    }

                    System.Windows.Forms.MessageBox.Show("Calculation complete");

                    MessageBoxCheckBox mbcb = new MessageBoxCheckBox(cylindricalShell, cylindricalShellDataIn) { Owner = this };
                    mbcb.ShowDialog();
                }
                else
                {
                    MessageBox.Show(string.Join<string>(Environment.NewLine, cylindricalShell.ErrorList));
                }
            }
            else
            {
                MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(cylindricalShellDataIn.ErrorList)));
            }


        }

        private void CilForm_Load(object sender, EventArgs e)
        {
            var steels = Physical.GetSteelsList()?.ToArray();
            if (steels != null)
            {
                steel_cb.Items.AddRange(steels);
                steel_cb.SelectedIndex = 0;
            }
            Gost_cb.SelectedIndex = 0;
        }

        private void GetE_b_Click(object sender, EventArgs e)
        {
            List<string> dataInErr = new List<string>();
            var E = Physical.GetE(steel_cb.Text, Convert.ToInt32(t_tb.Text), ref dataInErr);
            E_tb.Text = E.ToString();
        }

        private void GetFi_b_Click(object sender, EventArgs e)
        {
            FiForm ff = new FiForm { Owner = this };
            ff.ShowDialog(); // показываем
        }

        private void CilForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sender is not CilForm) return;

            if (this.Owner is MainForm {cf: { }} main)
            {
                main.cf = null;
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

    }
}

