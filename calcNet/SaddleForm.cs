using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Supports.Enums;
using CalculateVessels.Core.Supports.Saddle;
using CalculateVessels.Data.PhysicalData;
using CalculateVessels.Data.Properties;

namespace CalculateVessels
{
    public partial class SaddleForm : Form
    {
        public SaddleForm()
        {
            InitializeComponent();
        }

        public IDataIn DataIn => saddleDataIn;

        private SaddleDataIn saddleDataIn;

        private void SaddleForm_Load(object sender, EventArgs e)
        {
            var steels = Physical.Gost34233_1.GetSteelsList()?.ToArray<object>();
            if (steels != null)
            {
                steel_cb.Items.AddRange(steels);
                steel_cb.SelectedIndex = 0;
            }
            Gost_cb.SelectedIndex = 0;

            saddle_pb.Image = (Bitmap)new ImageConverter()
                .ConvertFrom(Data.Properties.Resources.SaddleNothingElem);
        }

        private void ShellReinforcement_rb_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is not RadioButton { Checked: true } rb) return;

            var name = rb.Name.First().ToString().ToUpper() + rb.Name[1..^3];

            switch (name)
            {
                case "Nothing":
                    ringLocation_gb.Visible = false;
                    pad_gb.Visible = false;
                    break;
                case "Sheet":
                    ringLocation_gb.Visible = false;
                    pad_gb.Visible = true;
                    break;
                case "Ring":
                    name += in_rb.Checked ? "In" : "Out";
                    ringLocation_gb.Visible = true;
                    pad_gb.Visible = false;
                    break;
            }


            saddle_pb.Image = (Bitmap)new ImageConverter()
                .ConvertFrom(Data.Properties.Resources.ResourceManager
                    .GetObject("Saddle" + name + "Elem"));
        }

        private void InOut_rb_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is not RadioButton { Checked: true } rb) return;

            saddle_pb.Image = (Bitmap)new ImageConverter()
                .ConvertFrom(Data.Properties.Resources.ResourceManager
                    .GetObject("SaddleRing" + (in_rb.Checked ? "In" : "Out") + "Elem"));
        }

        private void Cancel_btn_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void SaddleForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sender is not SaddleForm) return;

            if (Owner is MainForm { saddleForm: { } } main)
            {
                main.saddleForm = null;
            }
        }

        private void PreCalc_btn_Click(object sender, EventArgs e)
        {

            saddleDataIn = new SaddleDataIn();

            var dataInErr = new List<string>();

            //D
            {
                if (double.TryParse(D_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.InvariantCulture, out var D))
                {
                    saddleDataIn.D = D;
                }
                else
                {
                    dataInErr.Add("D неверный ввод");
                }
            }

            //s
            {
                if (double.TryParse(s_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.InvariantCulture, out var s))
                {
                    saddleDataIn.s = s;
                }
                else
                {
                    dataInErr.Add("s неверный ввод");
                }
            }

            //c
            {
                if (double.TryParse(c_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.InvariantCulture, out var c))
                {
                    saddleDataIn.c = c;
                }
                else
                {
                    dataInErr.Add("c неверный ввод");
                }
            }

            //fi
            {
                if (double.TryParse(fi_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.InvariantCulture, out var fi))
                {
                    saddleDataIn.fi = fi;
                }
                else
                {
                    dataInErr.Add("φ неверный ввод");
                }
            }

            //steel
            saddleDataIn.Steel = steel_cb.Text;

            //p
            {
                if (double.TryParse(p_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.InvariantCulture, out var p))
                {
                    saddleDataIn.p = p;
                }
                else
                {
                    dataInErr.Add("p неверный ввод");
                }
            }

            //
            saddleDataIn.IsPressureIn = !isNotPressureIn_cb.Checked;

            //t
            {
                if (double.TryParse(t_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                    System.Globalization.CultureInfo.InvariantCulture, out double t))
                {
                    saddleDataIn.t = t;
                }
                else
                {
                    dataInErr.Add("t неверный ввод");
                }
            }

            //N
            {
                if (int.TryParse(N_cb.Text, System.Globalization.NumberStyles.Integer,
                    System.Globalization.CultureInfo.InvariantCulture, out int N))
                {
                    saddleDataIn.N = N;
                }
                else
                {
                    dataInErr.Add("N неверный ввод");
                }
            }

            //
            saddleDataIn.IsAssembly = isAssembly_cb.Checked;

            //G
            {
                if (double.TryParse(G_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                    System.Globalization.CultureInfo.InvariantCulture, out double G))
                {
                    saddleDataIn.G = G;
                }
                else
                {
                    dataInErr.Add("G неверный ввод");
                }
            }

            var reinforcementShell = shellReinforcement_gb.Controls
                .OfType<RadioButton>()
                .FirstOrDefault(rb => rb.Checked)
                ?.Name;

            switch (reinforcementShell)
            {
                case nameof(nothing_rb):
                    saddleDataIn.Type = SaddleType.SaddleWithoutRingWithoutSheet;
                    break;
                case nameof(sheet_rb):
                    saddleDataIn.Type = SaddleType.SaddleWithoutRingWithSheet;

                    //s2
                    {
                        if (double.TryParse(s2_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                            System.Globalization.CultureInfo.InvariantCulture, out double s2))
                        {
                            saddleDataIn.s2 = s2;
                        }
                        else
                        {
                            dataInErr.Add("s2 неверный ввод");
                        }
                    }

                    //b2
                    {
                        if (double.TryParse(b2_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                            System.Globalization.CultureInfo.InvariantCulture, out double b2))
                        {
                            saddleDataIn.b2 = b2;
                        }
                        else
                        {
                            dataInErr.Add("b2 неверный ввод");
                        }
                    }

                    //delta2
                    {
                        if (double.TryParse(delta2_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                            System.Globalization.CultureInfo.InvariantCulture, out double delta2))
                        {
                            saddleDataIn.delta2 = delta2;
                        }
                        else
                        {
                            dataInErr.Add("delta2 неверный ввод");
                        }
                    }

                    ////f
                    //{
                    //    if (double.TryParse(f_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                    //        System.Globalization.CultureInfo.InvariantCulture, out double f))
                    //    {
                    //        saddleDataIn.f = f;
                    //    }
                    //    else
                    //    {
                    //        dataInErr.Add("s2 неверный ввод");
                    //    }
                    //}
                    break;
                case nameof(ring_rb):
                    saddleDataIn.Type = SaddleType.SaddleWithRing;
                    break;
                default:
                    dataInErr.Add("Невозможно определить тип укрепления обечайки");
                    break;
            }

            //b
            {
                if (double.TryParse(b_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                    System.Globalization.CultureInfo.InvariantCulture, out double b))
                {
                    saddleDataIn.b = b;
                }
                else
                {
                    dataInErr.Add("b неверный ввод");
                }
            }

            //delta1
            {
                if (double.TryParse(delta1_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                    System.Globalization.CultureInfo.InvariantCulture, out double delta1))
                {
                    saddleDataIn.delta1 = delta1;
                }
                else
                {
                    dataInErr.Add("delta1 неверный ввод");
                }
            }

            //a
            {
                if (double.TryParse(a_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.InvariantCulture, out double a))
                {
                    saddleDataIn.a = a;
                }
                else
                {
                    dataInErr.Add("a неверный ввод");
                }
            }

            //H
            {
                if (double.TryParse(H_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.InvariantCulture, out double H))
                {
                    saddleDataIn.H = H;
                }
                else
                {
                    dataInErr.Add("H неверный ввод");
                }
            }

            //L
            {
                if (double.TryParse(L_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                    System.Globalization.CultureInfo.InvariantCulture, out double L))
                {
                    saddleDataIn.L = L;
                }
                else
                {
                    dataInErr.Add("L неверный ввод");
                }
            }

            //e
            {
                if (double.TryParse(e_tb.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                    System.Globalization.CultureInfo.InvariantCulture, out double eValue))
                {
                    saddleDataIn.e = eValue;
                }
                else
                {
                    dataInErr.Add("e неверный ввод");
                }
            }

            var isNotError = dataInErr.Count == 0 && DataIn.IsDataGood;

            if (isNotError)
            {
                var saddle = new Saddle(saddleDataIn);
                saddle.Calculate();
                if (!saddle.IsCriticalError)
                {
                    calc_btn.Enabled = true;
                    if (saddle.ErrorList.Any())
                    {
                        MessageBox.Show(string.Join<string>(Environment.NewLine, saddle.ErrorList));
                    }
                }
                else
                {
                    MessageBox.Show(string.Join<string>(Environment.NewLine, saddle.ErrorList));
                }
            }
            else
            {
                MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(saddleDataIn.ErrorList)));
            }

        }

        private void Calc_btn_Click(object sender, EventArgs e)
        {

            List<string> dataInErr = new();

            //name
            saddleDataIn.Name = name_tb.Text;

            //shell name
            saddleDataIn.NameShell = nameShell_tb.Text;

            if (DataIn.IsDataGood)
            {
                Saddle saddle = new(saddleDataIn);
                saddle.Calculate();
                if (!saddle.IsCriticalError)
                {
                    if (this.Owner is MainForm main)
                    {
                        main.Word_lv.Items.Add(saddle.ToString());
                        Elements.ElementsList.Add(saddle);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("MainForm Error");
                    }

                    if (saddle.IsError)
                    {
                        MessageBox.Show(string.Join<string>(Environment.NewLine, saddle.ErrorList));
                    }

                    Hide();
                    MessageBox.Show(Resources.CalcComplete);
                }
                else
                {
                    MessageBox.Show(string.Join<string>(Environment.NewLine, saddle.ErrorList));
                }
            }
            else
            {
                MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(saddleDataIn.ErrorList)));
            }
        }
    }
}
