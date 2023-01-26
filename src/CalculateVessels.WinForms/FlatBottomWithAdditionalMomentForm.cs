using CalculateVessels.Core.Bottoms.Enums;
using CalculateVessels.Core.Bottoms.FlatBottomWithAdditionalMoment;
using CalculateVessels.Core.Exceptions;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.PhysicalData;
using CalculateVessels.Data.PhysicalData.Gost34233_1;
using CalculateVessels.Data.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace CalculateVessels
{
    public partial class FlatBottomWithAdditionalMomentForm : Form
    {

        private FlatBottomWithAdditionalMomentInputData _inputData;

        public FlatBottomWithAdditionalMomentForm()
        {
            InitializeComponent();
        }

        public IInputData DataIn => _inputData;

        private void Cancel_b_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void FlatBottomWithAdditionalMoment_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (sender is not FlatBottomWithAdditionalMomentForm) return;

            if (Owner is MainForm { flatBottomWithAdditionalMomentForm: { } } main)
            {
                main.flatBottomWithAdditionalMomentForm = null;
            }

        }

        private void FlatCover_rb_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is not RadioButton rb) return;

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
            var steels = Gost34233_1.GetSteelsList()?.ToArray();
            if (steels != null)
            {
                steel_cb.Items.AddRange(steels);
                steel_cb.SelectedIndex = 0;

                flangeSteel_cb.Items.AddRange(steels);
                flangeSteel_cb.SelectedIndex = 0;
            }

            Gost_cb.SelectedIndex = 0;

            cover_pb.Image = (Bitmap)new ImageConverter().ConvertFrom(Data.Properties.Resources.FlatBottomWithMoment);
            flange_pb.Image = (Bitmap)new ImageConverter().ConvertFrom
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
            if (sender is not CheckBox cb) return;

            cover_pb.Image = cb.Checked
                ? (Bitmap)new ImageConverter().ConvertFrom(Data.Properties.Resources.FlatBottomWithMomentWithGroove)
                : (Bitmap)new ImageConverter().ConvertFrom(Data.Properties.Resources.FlatBottomWithMoment);

            s4_1_l.Visible = cb.Checked;
            s4_2_l.Visible = cb.Checked;
            s4_tb.Visible = cb.Checked;
        }

        private void Hole_cb_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is CheckBox cb) hole_gb.Enabled = cb.Checked;
        }

        private void Flange_rb_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is not RadioButton { Checked: true } rb) return;

            var i = rb.Name[0].ToString();
            var type = scirtFlange_cb.Checked ? "fl1_" : "fl2_";
            flange_pb.Image =
                (Bitmap)new ImageConverter()
                    .ConvertFrom(Data.Properties.Resources.ResourceManager.GetObject(type + i));
        }

        private void SkirtFlange_cb_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is not CheckBox { Checked: true } cb) return;

            if (d4_flange_rb.Checked)
            {
                a1_flange_rb.Checked = true;
            }

            var name = flange_gb.Controls
                .OfType<RadioButton>()
                .FirstOrDefault(r => r.Checked)?.Name[0].ToString();

            d4_flange_rb.Enabled = cb.Checked;
            var type = cb.Checked ? "fl1_" : "fl2_";
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
            if (sender is not CheckBox cb) return;

            washerSteel_cb.Visible = cb.Checked;
            washerSteel_l.Visible = cb.Checked;

            hsh_1_l.Visible = cb.Checked;
            hsh_2_l.Visible = cb.Checked;
            hsh_tb.Visible = cb.Checked;
        }

        private void PreCalc_b_Click(object sender, EventArgs e)
        {
            //c_tb.Text = "";
            scalc_l.Text = "";
            calc_b.Enabled = false;

            _inputData = new FlatBottomWithAdditionalMomentInputData();

            var dataInErr = new List<string>();

            //t
            {
                if (double.TryParse(t_tb.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var t))
                {
                    _inputData.t = t;
                }
                else
                {
                    dataInErr.Add("t неверный ввод");
                }
            }

            //steel
            _inputData.CoverSteel = steel_cb.Text;

            //
            _inputData.IsPressureIn = !pressureOut_cb.Checked;

            //s1
            {
                if (double.TryParse(s1_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var s1))
                {
                    _inputData.s1 = s1;
                }
                else
                {
                    dataInErr.Add("s1 неверный ввод");
                }
            }


            //[σ]
            if (sigmaHandle_cb.Checked)
            {
                if (double.TryParse(sigma_d_tb.Text, NumberStyles.AllowDecimalPoint,
                        CultureInfo.InvariantCulture, out var sigmaAllow))
                {
                    _inputData.sigma_d = sigmaAllow;
                }
                else
                {
                    dataInErr.Add("[σ] неверный ввод");
                }
            }

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

            //p
            {
                if (double.TryParse(p_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var p))
                {
                    _inputData.p = p;
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
                    _inputData.M = M;
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
                    _inputData.F = F;
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
                    _inputData.fi = fi;
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
                else if (double.TryParse(c2_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var c2))
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
                else if (double.TryParse(c3_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var c3))
                {
                    _inputData.c3 = c3;
                }
                else
                {
                    dataInErr.Add("c3 неверный ввод");
                }
            }

            _inputData.IsCoverFlat = flatCover_rb.Checked;

            _inputData.IsCoverWithGroove = grooveCover_cb.Checked;

            if (_inputData.IsCoverWithGroove)
            {
                //s4
                {
                    if (double.TryParse(s4_tb.Text, NumberStyles.AllowDecimalPoint,
                        CultureInfo.InvariantCulture, out var s4))
                    {
                        _inputData.s4 = s4;
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
                    _inputData.s2 = s2;
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
                    _inputData.s3 = s3;
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
                    _inputData.D2 = D2;
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
                    _inputData.D3 = D3;
                }
                else
                {
                    dataInErr.Add("D3 неверный ввод");
                }
            }

            if (!hole_cb.Checked)
            {
                _inputData.Hole = HoleInFlatBottom.WithoutHole;
            }
            else
            {
                if (double.TryParse(holed_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var d))
                {
                    if (oneHole_rb.Checked)
                    {
                        _inputData.Hole = HoleInFlatBottom.OneHole;
                        _inputData.d = d;
                    }
                    else
                    {
                        _inputData.Hole = HoleInFlatBottom.MoreThenOneHole;
                        _inputData.di = d;
                    }

                }
                else
                {
                    dataInErr.Add("d неверный ввод");
                }
            }

            if (int.TryParse(flange_gb.Controls
                    .OfType<RadioButton>()
                    .FirstOrDefault(r => r.Checked)?.Name[1].ToString(),
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out var flangeFaceInt))
            {
                _inputData.FlangeFace = (FlangeFaceType)(flangeFaceInt - 1);
            }
            else
            {
                dataInErr.Add("Не возможно определить уплотнительную поверхность фланца");
            }


            _inputData.FlangeSteel = flangeSteel_cb.Text;

            _inputData.IsFlangeFlat = !scirtFlange_cb.Checked;

            _inputData.IsFlangeIsolated = isolatedFlange_cb.Checked;

            //D
            {
                if (double.TryParse(D_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var D))
                {
                    _inputData.D = D;
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
                    _inputData.Dn = Dn;
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
                    _inputData.Db = Db;
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
                    _inputData.h = h;
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
                    _inputData.Lb0 = Lb0;
                }
                else
                {
                    dataInErr.Add("Lb0 неверный ввод");
                }
            }

            if (_inputData.IsFlangeFlat)
            {
                //s
                {
                    if (double.TryParse(s_tb.Text, NumberStyles.AllowDecimalPoint,
                        CultureInfo.InvariantCulture, out var s))
                    {
                        _inputData.s = s;
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
                        _inputData.S0 = S0;
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
                        _inputData.S1 = S1;
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
                        _inputData.l = l;
                    }
                    else
                    {
                        dataInErr.Add("l неверный ввод");
                    }
                }
            }

            _inputData.GasketType = gasketType_cb.Text;

            //hp
            {
                if (double.TryParse(hp_tb.Text, NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out var hp))
                {
                    _inputData.hp = hp;
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
                    _inputData.bp = bp;
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
                    _inputData.Dcp = Dnp;
                }
                else
                {
                    dataInErr.Add("Dnp неверный ввод");
                }
            }

            _inputData.ScrewSteel = screwSteel_cb.Text;

            //d
            {
                if (int.TryParse(screwd_cb.Text, NumberStyles.Integer,
                    CultureInfo.InvariantCulture, out var d))
                {
                    _inputData.Screwd = d;
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
                    _inputData.n = n;
                }
                else
                {
                    dataInErr.Add("n неверный ввод");
                }
            }

            _inputData.IsScrewWithGroove = isScrewWithGroove_cb.Checked;

            _inputData.IsStud = isStud_cb.Checked;

            _inputData.IsWasher = isWasher_cb.Checked;

            if (isWasher_cb.Checked)
            {
                _inputData.WasherSteel = washerSteel_cb.Text;

                //hsh
                {
                    if (double.TryParse(hsh_tb.Text, NumberStyles.AllowDecimalPoint,
                        CultureInfo.InvariantCulture, out var hsh))
                    {
                        _inputData.hsh = hsh;
                    }
                    else
                    {
                        dataInErr.Add("hsh неверный ввод");
                    }
                }
            }


            var isError = dataInErr.Any() || !DataIn.IsDataGood;

            if (isError)
            {
                MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(_inputData.ErrorList)));
            }

            IElement bottom = new FlatBottomWithAdditionalMoment(_inputData);

            try
            {
                bottom.Calculate();
            }
            catch (CalculateException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            if (bottom.IsCalculated)
            {
                if (bottom.CalculatedData.ErrorList.Any())
                {
                    MessageBox.Show(string.Join<string>(Environment.NewLine, bottom.CalculatedData.ErrorList));
                }

                calc_b.Enabled = true;
                scalc_l.Text = $@"sp={((FlatBottomWithAdditionalMomentCalculatedData)bottom.CalculatedData).s1:f3} мм";
                p_d_l.Text =
                    $@"pd={((FlatBottomWithAdditionalMomentCalculatedData)bottom.CalculatedData).p_d:f2} МПа";
                MessageBox.Show(Resources.CalcComplete);
            }
        }

        private void Calc_b_Click(object sender, EventArgs e)
        {
            scalc_l.Text = "";

            List<string> dataInErr = new();

            //name
            _inputData.Name = name_tb.Text;

            var isError = dataInErr.Any() || !DataIn.IsDataGood;

            if (isError)
            {
                MessageBox.Show(string.Join<string>(Environment.NewLine, dataInErr.Union(_inputData.ErrorList)));
            }

            IElement bottom = new FlatBottomWithAdditionalMoment(_inputData);

            try
            {
                bottom.Calculate();
            }
            catch (CalculateException ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (Owner is MainForm main)
            {
                main.Word_lv.Items.Add(bottom.ToString());
                main.ElementsCollection.Add(bottom);

                //_form.Hide();
            }
            else
            {
                MessageBox.Show("MainForm Error");
            }

            if (bottom.IsCalculated)
            {
                if (bottom.CalculatedData.ErrorList.Any())
                {
                    MessageBox.Show(string.Join<string>(Environment.NewLine, bottom.CalculatedData.ErrorList));
                }

                calc_b.Enabled = true;
                scalc_l.Text = $@"sp={((FlatBottomWithAdditionalMomentCalculatedData)bottom.CalculatedData).s1:f3} мм";
                p_d_l.Text =
                    $@"pd={((FlatBottomWithAdditionalMomentCalculatedData)bottom.CalculatedData).p_d:f2} МПа";
                MessageBox.Show(Resources.CalcComplete);
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
}
