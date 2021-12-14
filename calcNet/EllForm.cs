using CalculateVessels.Core.Shells;
using CalculateVessels.Core.Shells.DataIn;
using CalculateVessels.Core.Shells.Enums;
using CalculateVessels.Data.PhysicalData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Models;

namespace CalculateVessels
{
    public partial class EllForm : Form
    {
        public EllForm()
        {
            InitializeComponent();
        }

        public IDataIn DataIn => _ellipticalShellDataIn;

        EllipticalShellDataIn _ellipticalShellDataIn = new();

        private void EllForm_Load(object sender, EventArgs e)
        {
            var steels = Physical.Gost34233_1.GetSteelsList()?.ToArray();
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
            if (sender is EllForm && this.Owner is MainForm {ef: { }} main)
            {
                main.ef = null;
            }

        }

        private void PreCalc_b_Click(object sender, EventArgs e)
        {
            const string WRONG_INPUT = " неверный ввод";
            List<string> dataInErr = new();

            //t
            {
                if (double.TryParse(t_tb.Text, System.Globalization.NumberStyles.Integer,
                    System.Globalization.CultureInfo.InvariantCulture, out double t))
                {
                    _ellipticalShellDataIn.t = t;
                }
                else
                {
                    dataInErr.Add(nameof(t) + WRONG_INPUT);
                }
            }

            //steel
            _ellipticalShellDataIn.Steel = steel_cb.Text;

            //pressure
            _ellipticalShellDataIn.IsPressureIn = vn_rb.Checked;


            if (((IDataIn)_ellipticalShellDataIn).IsDataGood)
            {
                //[σ]
                //InputClass.GetInput_sigma_d(sigma_d_tb, ref d_in, ref dataInErr);
                {
                    double sigma_d = 0.0;
                    if (sigma_d_tb.ReadOnly)
                    {
                        if (Physical.Gost34233_1.TryGetSigma(_ellipticalShellDataIn.Steel,
                                                    _ellipticalShellDataIn.t,
                                                    ref sigma_d,
                                                    ref dataInErr))
                        {
                            sigma_d_tb.ReadOnly = false;
                            sigma_d_tb.Text = sigma_d.ToString();
                            sigma_d_tb.ReadOnly = true;
                        }
                    }
                    else
                    {
                        if (!double.TryParse(sigma_d_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                            System.Globalization.CultureInfo.InvariantCulture, out sigma_d))
                        {
                            dataInErr.Add("[σ]"+ WRONG_INPUT);
                        }
                    }
                    _ellipticalShellDataIn.sigma_d = sigma_d;
                }


                if (!_ellipticalShellDataIn.IsPressureIn)
                {
                    //E
                    {
                        //if (double.TryParse(E_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                        //    System.Globalization.CultureInfo.InvariantCulture, out double E))
                        //{
                        //    _ellipticalShellDataIn.E = E;
                        //}
                        //else
                        //{
                        //    dataInErr.Add(nameof(E) + WRONG_INPUT);
                        //}
                    }
                }

                //p
                {
                    if (double.TryParse(p_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                        System.Globalization.CultureInfo.InvariantCulture, out double p))
                    {
                        _ellipticalShellDataIn.p = p;
                    }
                    else
                    {
                        dataInErr.Add(nameof(p) + WRONG_INPUT);
                    }
                }

                //fi
                {
                    if (double.TryParse(fi_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                        System.Globalization.CultureInfo.InvariantCulture, out double fi))
                    {
                        _ellipticalShellDataIn.fi = fi;
                    }
                    else
                    {
                        dataInErr.Add("φ" + WRONG_INPUT);
                    }
                }

                //D
                {
                    if (double.TryParse(D_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                        System.Globalization.CultureInfo.InvariantCulture, out double D))
                    {
                        _ellipticalShellDataIn.D = D;
                    }
                    else
                    {
                        dataInErr.Add(nameof(D) + WRONG_INPUT);
                    }
                }

                //H
                {
                    if (double.TryParse(H_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                        System.Globalization.CultureInfo.InvariantCulture, out double H))
                    {
                        _ellipticalShellDataIn.ellH = H;
                    }
                    else
                    {
                        dataInErr.Add(nameof(H) + WRONG_INPUT);
                    }
                }


                //h1
                {
                    if (double.TryParse(h1_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                        System.Globalization.CultureInfo.InvariantCulture, out double h1))
                    {
                        _ellipticalShellDataIn.ellh1 = h1;
                    }
                    else
                    {
                        dataInErr.Add(nameof(h1) + WRONG_INPUT);
                    }
                }

                //c1
                //    InputClass.GetInput_c1(c1_tb, ref d_in, ref dataInErr);
                {
                    if (double.TryParse(c1_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                        System.Globalization.CultureInfo.InvariantCulture, out double c1))
                    {
                        _ellipticalShellDataIn.c1 = c1;
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
                        _ellipticalShellDataIn.c2 = 0;
                    }
                    else if (double.TryParse(c2_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                        System.Globalization.CultureInfo.InvariantCulture, out double c2))
                    {
                        _ellipticalShellDataIn.c2 = c2;
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
                        _ellipticalShellDataIn.c3 = 0;
                    }
                    else if (double.TryParse(c3_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                        System.Globalization.CultureInfo.InvariantCulture, out double c3))
                    {
                        _ellipticalShellDataIn.c3 = c3;
                    }
                    else
                    {
                        dataInErr.Add("c3 неверный ввод");
                    }
                }

                _ellipticalShellDataIn.EllipticalBottomType = ell_rb.Checked ? EllipticalBottomType.Elliptical : EllipticalBottomType.Hemispherical;


                bool isNotError = dataInErr.Count == 0 && DataIn.IsDataGood;

                if (isNotError)
                {
                    IElement ellipse = new EllipticalShell(_ellipticalShellDataIn);

                    CalculatedElement calculatedElement = new CalculateElement(ellipse, this).Calculate(true);

                    if (calculatedElement != null)
                    {
                        calc_b.Enabled = true;
                        scalc_l.Text = $"sp={((EllipticalShell)calculatedElement.Element).s:f3} мм";
                        p_d_l.Text =
                            $"pd={((EllipticalShell)calculatedElement.Element).p_d:f2} МПа";

                    }
                }
                else
                {
                    MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(_ellipticalShellDataIn.ErrorList)));
                }
            }
            else
            {
                MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr));
            }
        }

        private void GetGostDim_b_Click(object sender, EventArgs e)
        {
            GostEllForm gef = new() { Owner = this };
            gef.ShowDialog(); // показываем
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

            List<string> dataInErr = new ();

            //name
            _ellipticalShellDataIn.Name = name_tb.Text;

            //s
            {
                if (double.TryParse(s_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.InvariantCulture, out double s))
                {
                    _ellipticalShellDataIn.s = s;
                }
                else
                {
                    dataInErr.Add(nameof(s) + " неверный ввод");
                }
            }


            bool isNotError = dataInErr.Count == 0 && DataIn.IsDataGood;

            if (isNotError)
            {

                IElement ellipse = new EllipticalShell(_ellipticalShellDataIn);

                CalculatedElement calculatedElement = new CalculateElement(ellipse, this).Calculate(false);

                if (calculatedElement != null)
                {
                    scalc_l.Text = $"sp={((EllipticalShell)calculatedElement.Element).s:f3} мм";
                    p_d_l.Text =
                        $"pd={((EllipticalShell)calculatedElement.Element).p_d:f2} МПа";

                    MessageBoxCheckBox mbcb = new(calculatedElement.Element, _ellipticalShellDataIn) { Owner = this };
                    mbcb.ShowDialog();
                }

            }
            else
            {
                MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(_ellipticalShellDataIn.ErrorList)));
            }
            
        }

        private void Ell_Polysfer_rb_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                if (rb.Name == "ell_rb")
                {
                    pictureBox.Image = (Bitmap)new ImageConverter()
                                            .ConvertFrom(CalculateVessels.Data.Properties.Resources.Ell);
                }
                else if (rb.Name == "polysfer_rb")
                {
                    pictureBox.Image = (Bitmap)new ImageConverter()
                                            .ConvertFrom(CalculateVessels.Data.Properties.Resources.Sfer);
                }
            }
        }
    }
}
