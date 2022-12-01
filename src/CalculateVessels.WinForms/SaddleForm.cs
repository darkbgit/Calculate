using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CalculateVessels.Core.Exceptions;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Models;
using CalculateVessels.Core.Shells;
using CalculateVessels.Core.Supports.Enums;
using CalculateVessels.Core.Supports.Saddle;
using CalculateVessels.Data.PhysicalData;
using CalculateVessels.Data.Properties;

namespace CalculateVessels
{
    public partial class SaddleForm : Form
    {
        private SaddleInputData _saddleDataIn;

        public SaddleForm()
        {
            InitializeComponent();
        }

        public IInputData DataIn => _saddleDataIn;

        

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

            _saddleDataIn = new SaddleInputData();

            var dataInErr = new List<string>();

            //D
            {
                if (double.TryParse(D_tb.Text, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out var D))
                {
                    _saddleDataIn.D = D;
                }
                else
                {
                    dataInErr.Add("D неверный ввод");
                }
            }

            //s
            {
                if (double.TryParse(s_tb.Text, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out var s))
                {
                    _saddleDataIn.s = s;
                }
                else
                {
                    dataInErr.Add("s неверный ввод");
                }
            }

            //c
            {
                if (double.TryParse(c_tb.Text, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out var c))
                {
                    _saddleDataIn.c = c;
                }
                else
                {
                    dataInErr.Add("c неверный ввод");
                }
            }

            //fi
            {
                if (double.TryParse(fi_tb.Text, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out var fi))
                {
                    _saddleDataIn.fi = fi;
                }
                else
                {
                    dataInErr.Add("φ неверный ввод");
                }
            }

            //steel
            _saddleDataIn.Steel = steel_cb.Text;

            //p
            {
                if (double.TryParse(p_tb.Text, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out var p))
                {
                    _saddleDataIn.p = p;
                }
                else
                {
                    dataInErr.Add("p неверный ввод");
                }
            }

            //
            _saddleDataIn.IsPressureIn = !isNotPressureIn_cb.Checked;

            //t
            {
                if (double.TryParse(t_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var t))
                {
                    _saddleDataIn.t = t;
                }
                else
                {
                    dataInErr.Add("t неверный ввод");
                }
            }

            //N
            {
                if (int.TryParse(N_cb.Text, NumberStyles.Integer,
                    CultureInfo.InvariantCulture, out var N))
                {
                    _saddleDataIn.N = N;
                }
                else
                {
                    dataInErr.Add("N неверный ввод");
                }
            }

            //
            _saddleDataIn.IsAssembly = isAssembly_cb.Checked;

            //G
            {
                if (double.TryParse(G_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var G))
                {
                    _saddleDataIn.G = G;
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
                    _saddleDataIn.Type = SaddleType.SaddleWithoutRingWithoutSheet;
                    break;
                case nameof(sheet_rb):
                    _saddleDataIn.Type = SaddleType.SaddleWithoutRingWithSheet;

                    //s2
                    {
                        if (double.TryParse(s2_tb.Text, NumberStyles.AllowDecimalPoint,
                            CultureInfo.InvariantCulture, out var s2))
                        {
                            _saddleDataIn.s2 = s2;
                        }
                        else
                        {
                            dataInErr.Add("s2 неверный ввод");
                        }
                    }

                    //b2
                    {
                        if (double.TryParse(b2_tb.Text, NumberStyles.AllowDecimalPoint,
                            CultureInfo.InvariantCulture, out var b2))
                        {
                            _saddleDataIn.b2 = b2;
                        }
                        else
                        {
                            dataInErr.Add("b2 неверный ввод");
                        }
                    }

                    //delta2
                    {
                        if (double.TryParse(delta2_tb.Text, NumberStyles.AllowDecimalPoint,
                            CultureInfo.InvariantCulture, out var delta2))
                        {
                            _saddleDataIn.delta2 = delta2;
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
                    _saddleDataIn.Type = SaddleType.SaddleWithRing;
                    break;
                default:
                    dataInErr.Add("Невозможно определить тип укрепления обечайки");
                    break;
            }

            //b
            {
                if (double.TryParse(b_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var b))
                {
                    _saddleDataIn.b = b;
                }
                else
                {
                    dataInErr.Add("b неверный ввод");
                }
            }

            //delta1
            {
                if (double.TryParse(delta1_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var delta1))
                {
                    _saddleDataIn.delta1 = delta1;
                }
                else
                {
                    dataInErr.Add("delta1 неверный ввод");
                }
            }

            //a
            {
                if (double.TryParse(a_tb.Text, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out var a))
                {
                    _saddleDataIn.a = a;
                }
                else
                {
                    dataInErr.Add("a неверный ввод");
                }
            }

            //H
            {
                if (double.TryParse(H_tb.Text, NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out var H))
                {
                    _saddleDataIn.H = H;
                }
                else
                {
                    dataInErr.Add("H неверный ввод");
                }
            }

            //L
            {
                if (double.TryParse(L_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var L))
                {
                    _saddleDataIn.L = L;
                }
                else
                {
                    dataInErr.Add("L неверный ввод");
                }
            }

            //e
            {
                if (double.TryParse(e_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var eValue))
                {
                    _saddleDataIn.e = eValue;
                }
                else
                {
                    dataInErr.Add("e неверный ввод");
                }
            }

            var isError = dataInErr.Any() || !DataIn.IsDataGood;

            if (isError)
            {
                MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(_saddleDataIn.ErrorList)));
            }

            IElement saddle = new Saddle(_saddleDataIn);

            try
            {
                saddle.Calculate();
            }
            catch (CalculateException ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (saddle.IsCalculated)
            {
                if (saddle.CalculatedData.ErrorList.Any())
                {
                    MessageBox.Show(string.Join<string>(Environment.NewLine, saddle.CalculatedData.ErrorList));
                }

                calc_btn.Enabled = true;
                MessageBox.Show(Resources.CalcComplete);
            }
        }

        private void Calc_btn_Click(object sender, EventArgs e)
        {

            List<string> dataInErr = new();

            //name
            _saddleDataIn.Name = name_tb.Text;

            //shell name
            _saddleDataIn.NameShell = nameShell_tb.Text;

            var isError = dataInErr.Any() || !DataIn.IsDataGood;

            if (isError)
            {
                MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(_saddleDataIn.ErrorList)));
            }

            IElement saddle = new Saddle(_saddleDataIn);

            try
            {
                saddle.Calculate();
            }
            catch (CalculateException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            if (Owner is MainForm main)
            {
                main.Word_lv.Items.Add(saddle.ToString());
                main.ElementsCollection.Add(saddle);

                //_form.Hide();
            }
            else
            {
                MessageBox.Show("MainForm Error");
            }

            if (saddle.IsCalculated)
            {
                if (saddle.CalculatedData.ErrorList.Any())
                {
                    MessageBox.Show(string.Join<string>(Environment.NewLine, saddle.CalculatedData.ErrorList));
                }

                calc_btn.Enabled = true;
                MessageBox.Show(Resources.CalcComplete);
            }
        }
    }
}
