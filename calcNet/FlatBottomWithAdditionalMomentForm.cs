using CalculateVessels.Core.Bottoms.Enums;
using CalculateVessels.Core.Bottoms.FlatBottomWithAdditionalMoment;
using CalculateVessels.Data.PhysicalData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Models;

namespace CalculateVessels
{
    public partial class FlatBottomWithAdditionalMomentForm : Form
    {
        public IDataIn DataIn => _dataIn;
        private FlatBottomWithAdditionalMomentDataIn _dataIn;

        public FlatBottomWithAdditionalMomentForm()
        {
            InitializeComponent();
        }



        private void Cancel_b_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void FlatBottomWithAdditionalMoment_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (sender is not FlatBottomWithAdditionalMomentForm) return;

            if (this.Owner is MainForm { flatBottomWithAdditionalMomentForm: { } } main)
            {
                main.flatBottomWithAdditionalMomentForm = null;
            }
            
        }

        private void FlatCover_rb_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                grooveCover_cb.Enabled = true;
                grooveCover_cb.Checked = false;
                cover_pb.Image = (Bitmap)new ImageConverter().ConvertFrom(Data.Properties.Resources.FlatBottomWithMoment);
            }
            else
            {
                grooveCover_cb.Checked = false;
                grooveCover_cb.Enabled = false;
                //cover_pb.Image = (Bitmap)new ImageConverter().ConvertFrom(Data.Properties.Resources.pldn15);
            }
        }

        private void FlatBottomWithAdditionalMomentForm_Load(object sender, EventArgs e)
        {
            var steels = Physical.Gost34233_1.GetSteelsList()?.ToArray();
            if (steels != null)
            {
                steel_cb.Items.AddRange(steels);
                steel_cb.SelectedIndex = 0;

                flangeSteel_cb.Items.AddRange(steels);
                flangeSteel_cb.SelectedIndex = 0;
            }

            Gost_cb.SelectedIndex = 0;

            cover_pb.Image = (Bitmap)new ImageConverter().ConvertFrom(Data.Properties.Resources.FlatBottomWithMoment);
            flange_pb.Image = (Bitmap) new ImageConverter().ConvertFrom
                (Data.Properties.Resources.fl2_a);

            var gaskets = Physical.Gost34233_4.GetGasketsList()?.ToArray();
            if (gaskets != null)
            {
                gasketType_cb.Items.AddRange(gaskets);
                gasketType_cb.SelectedIndex = 0;
            }

            var ds = Physical.Gost34233_4.GetScrewDs()?.ToArray();
            if (ds != null)
            {
                screwd_cb.Items.AddRange(ds);
                screwd_cb.SelectedIndex = 0;
            }

            var screwSteels = Physical.Gost34233_4.GetSteelsList("screw")?.ToArray();
            if (screwSteels != null)
            {
                screwSteel_cb.Items.AddRange(screwSteels);
                screwSteel_cb.SelectedIndex = 0;
            }

            var washerSteels = Physical.Gost34233_4.GetSteelsList("washer")?.ToArray();
            if (washerSteels != null)
            {
                washerSteel_cb.Items.AddRange(washerSteels);
                washerSteel_cb.SelectedIndex = 0;
            }

        }

        private void GrooveCover_cb_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;

            
            cover_pb.Image = cb.Checked
                ? (Bitmap)new ImageConverter().ConvertFrom(Data.Properties.Resources.FlatBottomWithMomentWithGroove)
                : (Bitmap)new ImageConverter().ConvertFrom(Data.Properties.Resources.FlatBottomWithMoment);
            
            s4_1_l.Visible = cb.Checked;
            s4_2_l.Visible = cb.Checked;
            s4_tb.Visible = cb.Checked;
        }

        private void Hole_cb_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            hole_gb.Enabled = cb.Checked;
        }

        private void Flange_rb_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                string i = rb.Name[0].ToString();
                string type = scirtFlange_cb.Checked ? "fl1_" : "fl2_";
                flange_pb.Image =
                    (Bitmap)new ImageConverter()
                    .ConvertFrom(Data.Properties.Resources.ResourceManager.GetObject(type + i));
            }
        }

        private void ScirtFlange_cb_CheckedChanged(object sender, EventArgs e)
        {
            if (d4_flange_rb.Checked)
            {
                a1_flange_rb.Checked = true;
            }

            var name = flange_gb.Controls
                .OfType<RadioButton>()
                .FirstOrDefault(r => r.Checked == true)?.Name[0].ToString();
            CheckBox cb = sender as CheckBox;

            d4_flange_rb.Enabled = cb.Checked;
            string type = cb.Checked ? "fl1_" : "fl2_";
            flange_pb.Image = (Bitmap)new ImageConverter()
                    .ConvertFrom(Data.Properties.Resources.ResourceManager.GetObject(type + name));

            s_1_l.Visible = !cb.Checked;
            s_2_l.Visible = !cb.Checked;
            s_tb.Visible = !cb.Checked;

            s0_1_l.Visible = cb.Checked;
            s0_2_l.Visible = cb.Checked;
            S0_tb.Visible = cb.Checked;

            s1_1_l.Visible = cb.Checked;
            s1_2_l.Visible = cb.Checked;
            s1scirt_tb.Visible = cb.Checked;

            l_1_l.Visible = cb.Checked;
            l_2_l.Visible = cb.Checked;
            l_tb.Visible = cb.Checked;

            //UNDONE
        }

        private void IsWasher_cb_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;

            washerSteel_cb.Visible = cb.Checked;
            washerSteel_l.Visible = cb.Checked;

            hsh_1_l.Visible = cb.Checked;
            hsh_2_l.Visible = cb.Checked;
            hsh_tb.Visible = cb.Checked;
        }

        private void PredCalc_b_Click(object sender, EventArgs e)
        {
            //c_tb.Text = "";
            scalc_l.Text = "";
            calc_b.Enabled = false;

            _dataIn = new FlatBottomWithAdditionalMomentDataIn();

            var dataInErr = new List<string>();

            //t
            {
                if (double.TryParse(t_tb.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out double t))
                {
                    _dataIn.t = t;
                }
                else
                {
                    dataInErr.Add("t неверный ввод");
                }
            }

            //steel
            _dataIn.CoverSteel = steel_cb.Text;

            //
            _dataIn.IsPressureIn = !pressureOut_cb.Checked;

            //s1
            {
                if (double.TryParse(s1_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var s1))
                {
                    _dataIn.s1 = s1;
                }
                else
                {
                    dataInErr.Add("s1 неверный ввод");
                }
            }


            //[σ]
            {
                double sigma_d = default;
                if (sigma_d_tb.ReadOnly)
                {
                    if (Physical.Gost34233_1.TryGetSigma(_dataIn.CoverSteel,
                        _dataIn.t,
                        ref sigma_d,
                        ref dataInErr, s: _dataIn.s1))
                    {
                        sigma_d_tb.ReadOnly = false;
                        sigma_d_tb.Text = sigma_d.ToString(CultureInfo.CurrentCulture);
                        sigma_d_tb.ReadOnly = true;
                    }
                }
                else
                {
                    if (!double.TryParse(sigma_d_tb.Text, NumberStyles.AllowDecimalPoint,
                        CultureInfo.InvariantCulture, out sigma_d))
                    {
                        dataInErr.Add("[σ] неверный ввод");
                    }
                }

                _dataIn.sigma_d = sigma_d;
            }

            //if (!_dataIn.IsPressureIn)
            //{
            //    //E
            //    {
            //        double E;
            //        if (E_tb.ReadOnly)
            //        {
            //            E = Physical.Gost34233_1.GetE(_dataIn.CoverSteel,
            //                                _dataIn.t,
            //                                ref dataInErr);
            //            E_tb.ReadOnly = false;
            //            E_tb.Text = E.ToString();
            //            E_tb.ReadOnly = true;
            //        }
            //        else
            //        {

            //            if (double.TryParse(E_tb.Text, NumberStyles.AllowDecimalPoint,
            //            CultureInfo.InvariantCulture, out E))
            //            {
            //                _dataIn.E = E;
            //            }
            //            else
            //            {
            //                dataInErr.Add("E неверный ввод");
            //            }
            //        }
            //    }
            //}

            //p
            {
                if (double.TryParse(p_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var p))
                {
                    _dataIn.p = p;
                }
                else
                {
                    dataInErr.Add("p неверный ввод");
                }
            }

            //M
            {
                if (double.TryParse(M_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var M))
                {
                    _dataIn.M = M;
                }
                else
                {
                    dataInErr.Add("M неверный ввод");
                }
            }

            //F
            {
                if (double.TryParse(F_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var F))
                {
                    _dataIn.F = F;
                }
                else
                {
                    dataInErr.Add("F неверный ввод");
                }
            }

            //fi
            {
                if (double.TryParse(fi_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var fi))
                {
                    _dataIn.fi = fi;
                }
                else
                {
                    dataInErr.Add("φ неверный ввод");
                }
            }

            //c1
            {
                if (double.TryParse(c1_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var c1))
                {
                    _dataIn.c1 = c1;
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
                    _dataIn.c2 = 0;
                }
                else if (double.TryParse(c2_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var c2))
                {
                    _dataIn.c2 = c2;
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
                    _dataIn.c3 = 0;
                }
                else if (double.TryParse(c3_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var c3))
                {
                    _dataIn.c3 = c3;
                }
                else
                {
                    dataInErr.Add("c3 неверный ввод");
                }
            }

            _dataIn.IsCoverFlat = flatCover_rb.Checked;

            _dataIn.IsCoverWithGroove = grooveCover_cb.Checked;

            if (_dataIn.IsCoverWithGroove)
            {
                //s4
                {
                    if (double.TryParse(s4_tb.Text, NumberStyles.AllowDecimalPoint,
                        CultureInfo.InvariantCulture, out var s4))
                    {
                        _dataIn.s4 = s4;
                    }
                    else
                    {
                        dataInErr.Add("s4 неверный ввод");
                    }
                }
            }



            //s2
            {
                if (double.TryParse(s2_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var s2))
                {
                    _dataIn.s2 = s2;
                }
                else
                {
                    dataInErr.Add("s2 неверный ввод");
                }
            }

            //s3
            {
                if (double.TryParse(s3_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var s3))
                {
                    _dataIn.s3 = s3;
                }
                else
                {
                    dataInErr.Add("s3 неверный ввод");
                }
            }

            //D2
            {
                if (double.TryParse(D2_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var D2))
                {
                    _dataIn.D2 = D2;
                }
                else
                {
                    dataInErr.Add("D2 неверный ввод");
                }
            }

            //D3
            {
                if (double.TryParse(D3_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var D3))
                {
                    _dataIn.D3 = D3;
                }
                else
                {
                    dataInErr.Add("D3 неверный ввод");
                }
            }

            if (!hole_cb.Checked)
            {
                _dataIn.Hole = Core.Bottoms.Enums.HoleInFlatBottom.WithoutHole;
            }
            else
            {
                if (double.TryParse(holed_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var d))
                {
                    if (oneHole_rb.Checked)
                    {
                        _dataIn.Hole = Core.Bottoms.Enums.HoleInFlatBottom.OneHole;
                        _dataIn.d = d;
                    }
                    else
                    {
                        _dataIn.Hole = Core.Bottoms.Enums.HoleInFlatBottom.MoreThenOneHole;
                        _dataIn.di = d;
                    }

                }
                else
                {
                    dataInErr.Add("d неверный ввод");
                }
            }

            if (int.TryParse(flange_gb.Controls
                    .OfType<RadioButton>()
                    .FirstOrDefault(r => r.Checked == true)?.Name[1].ToString(),
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out var flangeFaceInt))
            {
                _dataIn.FlangeFace = (FlangeFaceType) (flangeFaceInt - 1);
            }
            else
            {
                dataInErr.Add("Не возможно определить уплотнительную поверхность фланца");
            }


            _dataIn.FlangeSteel = flangeSteel_cb.Text;

            _dataIn.IsFlangeFlat = !scirtFlange_cb.Checked;

            _dataIn.IsFlangeIsolated = isolatedFlange_cb.Checked;

            //D
            {
                if (double.TryParse(D_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var D))
                {
                    _dataIn.D = D;
                }
                else
                {
                    dataInErr.Add("D неверный ввод");
                }
            }

            //Dn
            {
                if (double.TryParse(Dn_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var Dn))
                {
                    _dataIn.Dn = Dn;
                }
                else
                {
                    dataInErr.Add("Dn неверный ввод");
                }
            }

            //Db
            {
                if (double.TryParse(Db_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var Db))
                {
                    _dataIn.Db = Db;
                }
                else
                {
                    dataInErr.Add("Db неверный ввод");
                }
            }

            //h
            {
                if (double.TryParse(h_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var h))
                {
                    _dataIn.h = h;
                }
                else
                {
                    dataInErr.Add("h неверный ввод");
                }
            }

            //Lbo
            {
                if (double.TryParse(h_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var Lb0))
                {
                    _dataIn.Lb0 = Lb0;
                }
                else
                {
                    dataInErr.Add("Lb0 неверный ввод");
                }
            }

            if (_dataIn.IsFlangeFlat)
            {
                //s
                {
                    if (double.TryParse(s_tb.Text, NumberStyles.AllowDecimalPoint,
                        CultureInfo.InvariantCulture, out var s))
                    {
                        _dataIn.s = s;
                    }
                    else
                    {
                        dataInErr.Add("s неверный ввод");
                    }
                }
            }
            else
            {
                //S0
                {
                    if (double.TryParse(S0_tb.Text, NumberStyles.AllowDecimalPoint,
                        CultureInfo.InvariantCulture, out var S0))
                    {
                        _dataIn.S0 = S0;
                    }
                    else
                    {
                        dataInErr.Add("S0 неверный ввод");
                    }
                }

                //S1
                {
                    if (double.TryParse(s1scirt_tb.Text, NumberStyles.AllowDecimalPoint,
                        CultureInfo.InvariantCulture, out var S1))
                    {
                        _dataIn.S1 = S1;
                    }
                    else
                    {
                        dataInErr.Add("S1 неверный ввод");
                    }
                }

                //l
                {
                    if (double.TryParse(l_tb.Text, NumberStyles.AllowDecimalPoint,
                        CultureInfo.InvariantCulture, out var l))
                    {
                        _dataIn.l = l;
                    }
                    else
                    {
                        dataInErr.Add("l неверный ввод");
                    }
                }
            }

            _dataIn.GasketType = gasketType_cb.Text;

            //hp
            {
                if (double.TryParse(hp_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var hp))
                {
                    _dataIn.hp = hp;
                }
                else
                {
                    dataInErr.Add("hp неверный ввод");
                }
            }

            //bp
            {
                if (double.TryParse(bp_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var bp))
                {
                    _dataIn.bp = bp;
                }
                else
                {
                    dataInErr.Add("bp неверный ввод");
                }
            }

            //Dcp
            {
                if (double.TryParse(Dcp_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var Dnp))
                {
                    _dataIn.Dcp = Dnp;
                }
                else
                {
                    dataInErr.Add("Dnp неверный ввод");
                }
            }

            _dataIn.ScrewSteel = screwSteel_cb.Text;

            //d
            {
                if (int.TryParse(screwd_cb.Text, NumberStyles.Integer,
                    CultureInfo.InvariantCulture, out var d))
                {
                    _dataIn.Screwd = d;
                }
                else
                {
                    dataInErr.Add("d болта неверный ввод");
                }
            }

            //n
            {
                if (int.TryParse(n_tb.Text, NumberStyles.Integer,
                    CultureInfo.InvariantCulture, out var n))
                {
                    _dataIn.n = n;
                }
                else
                {
                    dataInErr.Add("n неверный ввод");
                }
            }

            _dataIn.IsScrewWithGroove = isScrewWithGroove_cb.Checked;

            _dataIn.IsStud = isStud_cb.Checked;

            _dataIn.IsWasher = isWasher_cb.Checked;

            if (isWasher_cb.Checked)
            {
                _dataIn.WasherSteel = washerSteel_cb.Text;

                //hsh
                {
                    if (double.TryParse(hsh_tb.Text, NumberStyles.AllowDecimalPoint,
                        CultureInfo.InvariantCulture, out var hsh))
                    {
                        _dataIn.hsh = hsh;
                    }
                    else
                    {
                        dataInErr.Add("hsh неверный ввод");
                    }
                }
            }


            var isNotError = dataInErr.Count == 0 && ((IDataIn)_dataIn).IsDataGood;

            if (isNotError)
            {
                IElement bottom = new FlatBottomWithAdditionalMoment(_dataIn);

                CalculatedElement calculatedElement = new(bottom);

                calculatedElement.Element.Calculate();


                if (!calculatedElement.Element.IsCriticalError)
                {
                    //c_tb.Text = $"{bottom.c:f2}";
                    p_d_l.Text = $"p={((FlatBottomWithAdditionalMoment)calculatedElement.Element).PressurePermissible:f3} МПа";
                    calc_b.Enabled = true;
                    if (calculatedElement.Element.IsError)
                    {
                        MessageBox.Show(string.Join<string>(Environment.NewLine, calculatedElement.Element.ErrorList));
                    }
                }
                else
                {
                    MessageBox.Show(string.Join<string>(Environment.NewLine, calculatedElement.Element.ErrorList));
                }
            }
            else
            {
                MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(_dataIn.ErrorList)));
            }


        }

        private void Calc_b_Click(object sender, EventArgs e)
        {
            scalc_l.Text = "";

            List<string> dataInErr = new();

            //name
            _dataIn.Name = name_tb.Text;
            

            bool isNotError = dataInErr.Count == 0 && DataIn.IsDataGood;

            if (isNotError)
            {
                IElement bottom = new FlatBottomWithAdditionalMoment(_dataIn);

                CalculatedElement calculatedElement = new(bottom);

                calculatedElement.Element.Calculate();


                if (!calculatedElement.Element.IsCriticalError)
                {
                    p_d_l.Text = $"pd={((FlatBottomWithAdditionalMoment)calculatedElement.Element).PressurePermissible:f2} МПа";

                    if (this.Owner is MainForm main)
                    {
                        main.Word_lv.Items.Add(calculatedElement.Element.ToString());
                        main.ElementsCollection.Add(calculatedElement);
                        this.Hide();
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("MainForm Error");
                    }

                    if (calculatedElement.Element.IsError)
                    {
                        MessageBox.Show(string.Join<string>(Environment.NewLine, calculatedElement.Element.ErrorList));
                    }

                    MessageBox.Show("Calculation complete");

                }
                else
                {
                    MessageBox.Show(string.Join<string>(Environment.NewLine, calculatedElement.Element.ErrorList));
                }
            }
            else
            {
                MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(_dataIn.ErrorList)));
            }

        }
    }
}
