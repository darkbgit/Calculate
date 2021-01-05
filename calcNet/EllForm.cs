using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace calcNet
{
    public partial class EllForm : Form
    {
        public EllForm()
        {
            InitializeComponent();
        }

        public string TypeEl = "ell";
        
        internal DataWordOut.DataOutArrEl dataArrEl;
        internal DataWordOut.DataOutArrEl DataArrEl { get => dataArrEl; set => dataArrEl = value; }

        private void EllForm_Load(object sender, EventArgs e)
        {
            Set_steellist.Set_llist(steel_cb);
            steel_cb.SelectedIndex = 0;
            Gost_cb.SelectedIndex = 0;
        }

        private void Cancel_b_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void EllForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sender is EllForm)
            {
                if (this.Owner is MainForm main)
                {
                    if (main.ef != null)
                    {
                        main.ef = null;
                    }
                }
            }
        }

        private void PredCalc_b_Click(object sender, EventArgs e)
        {
            Data_in d_in = new Data_in();

            string data_inerr = "";

            try
            {
                if ((Convert.ToInt32(t_tb.Text) >= 20) && (Convert.ToInt32(t_tb.Text) < 1000))
                {
                    d_in.temp = Convert.ToInt32(t_tb.Text);
                }
                else
                {
                    data_inerr += "T должна быть в диапазоне 20 - 1000\n";
                }
            }
            catch (FormatException)
            {
                data_inerr += "T должна быть в диапазоне 20 - 1000\n";
            }

            d_in.steel = steel_cb.Text;

            if (data_inerr == "")
            {
                sigma_d_tb.ReadOnly = false;
                sigma_d_tb.Text = Convert.ToString(CalcClass.GetSigma(d_in.steel, d_in.temp));
                sigma_d_tb.ReadOnly = true;

                try
                {
                    if (Convert.ToDouble(sigma_d_tb.Text) > 0)
                    {
                        d_in.sigma_d = Convert.ToDouble(sigma_d_tb.Text);
                    }
                    else
                    {
                        data_inerr += "[σ] должно быт больше 0\n";
                    }
                }
                catch (FormatException)
                {
                    data_inerr += "[σ] неверные данные\n";
                }

                if (vn_rb.Checked)
                {
                    d_in.dav = 0;
                }
                else
                {
                    d_in.dav = 1;
                    E_tb.ReadOnly = false;
                    E_tb.Text = Convert.ToString(CalcClass.GetE(d_in.steel, d_in.temp));
                    E_tb.ReadOnly = true;

                    try
                    {
                        if (Convert.ToDouble(E_tb.Text) > 0)
                        {
                            d_in.E = Convert.ToInt32(E_tb.Text);
                        }
                        else
                        {
                            data_inerr += "E должно быть больше 0\n";
                        }
                    }
                    catch
                    {
                        data_inerr += "E неверные данные\n";
                    }
                }
            }

            try
            {
                if ((Convert.ToDouble(p_tb.Text) > 0) && (Convert.ToDouble(p_tb.Text) < 1000))
                {
                    d_in.p = Convert.ToDouble(p_tb.Text);
                }
                else
                {
                    data_inerr += "p должно быть в диапазоне 0 - 1000\n";
                }
            }
            catch
            {
                data_inerr += "p должно быть в диапазоне 0 - 1000\n";
            }

            try
            {
                if ((Convert.ToDouble(fi_tb.Text) > 0) && (Convert.ToDouble(fi_tb.Text) <= 1))
                {
                    d_in.fi = Convert.ToDouble(fi_tb.Text);
                }
                else
                {
                    data_inerr += "fi должен быть в диапазоне 0 - 1\n";
                }
            }
            catch
            {
                data_inerr += "fi должен быть в диапазоне 0 - 1\n";
            }

            try
            {
                if (Convert.ToInt32(D_tb.Text) > 0)
                {
                    d_in.D = Convert.ToInt32(D_tb.Text);
                }
                else
                {
                    data_inerr += "D должно быть больше 0\n";
                }
            }
            catch
            {
                data_inerr += "D неверные данные\n";
            }

            try
            {
                if (Convert.ToInt32(H_tb.Text) > 0)
                {
                    d_in.elH = Convert.ToInt32(H_tb.Text);
                }
                else
                {
                    data_inerr += "H должно быть больше 0\n";
                }
            }
            catch
            {
                data_inerr += "H неверные данные\n";
            }

            try
            {
                if (Convert.ToInt32(h1_tb.Text) > 0)
                {
                    d_in.elh1 = Convert.ToInt32(h1_tb.Text);
                }
                else
                {
                    data_inerr += "h1 должно быть больше 0\n";
                }
            }
            catch
            {
                data_inerr += "h1 неверные данные\n";
            }

            try
            {
                if (Convert.ToDouble(c1_tb.Text) >= 0)
                {
                    d_in.c1 = Convert.ToDouble(c1_tb.Text);
                }
                else
                {
                    data_inerr += "c1 неверные данные\n";
                }
            }
            catch
            {
                data_inerr += "c1 неверные данные\n";
            }

            try
            {
                if (Convert.ToDouble(c2_tb.Text) >= 0)
                {
                    d_in.c2 = Convert.ToDouble(c2_tb.Text);
                }
                else
                {
                    data_inerr += "c2 неверные данные\n";
                }
            }
            catch
            {
                data_inerr += "c2 неверные данные\n";
            }

            try
            {
                if (c3_tb.Text == "")
                {
                    d_in.c3 = 0;
                }
                else if (Convert.ToDouble(c3_tb.Text) >= 0)
                {
                    d_in.c3 = Convert.ToDouble(c3_tb.Text);
                }
                else
                {
                    data_inerr += "c3 неверные данные\n";
                }
            }
            catch
            {
                data_inerr += "c3 неверные данные\n";
            }

            if (ell_rb.Checked)
            {
                d_in.ellType = "ell";
            }
            else
            {
                d_in.ellType = "polysfer";
            }

            if (data_inerr == "")
            {
                Data_out d_out = CalcClass.CalcEll(d_in);
                if (d_out.err != "")
                {
                    System.Windows.Forms.MessageBox.Show(d_out.err);
                }
                c_tb.Text = $"{d_out.c:f2}";
                scalc_l.Text = $"sp={d_out.s_calc:f3} мм";
                calc_b.Enabled = true;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(data_inerr);
            }
        }

        private void GetGostDim_b_Click(object sender, EventArgs e)
        {
            GostEllForm gef = new GostEllForm { Owner = this };
            gef.ShowDialog(); // показываем
        }

        private void GetFi_b_Click(object sender, EventArgs e)
        {
            FiForm ff = new FiForm { Owner = this };
            ff.ShowDialog(); // показываем
        }

        private void Calc_b_Click(object sender, EventArgs e)
        {
            Data_in d_in = new Data_in();

            string data_inerr = "";

            d_in.name = name_tb.Text;

            try
            {
                if ((Convert.ToInt32(t_tb.Text) >= 20) && (Convert.ToInt32(t_tb.Text) < 1000))
                {
                    d_in.temp = Convert.ToInt32(t_tb.Text);
                }
                else
                {
                    data_inerr += "T должна быть в диапазоне 20 - 1000\n";
                }
            }
            catch (FormatException)
            {
                data_inerr += "T должна быть в диапазоне 20 - 1000\n";
            }

            d_in.steel = steel_cb.Text;

            if (data_inerr == "")
            {
                sigma_d_tb.ReadOnly = false;
                sigma_d_tb.Text = Convert.ToString(CalcClass.GetSigma(d_in.steel, d_in.temp));
                sigma_d_tb.ReadOnly = true;

                try
                {
                    if (Convert.ToDouble(sigma_d_tb.Text) > 0)
                    {
                        d_in.sigma_d = Convert.ToDouble(sigma_d_tb.Text);
                    }
                    else
                    {
                        data_inerr += "[σ] должно быт больше 0\n";
                    }
                }
                catch (FormatException)
                {
                    data_inerr += "[σ] неверные данные\n";
                }

                if (vn_rb.Checked)
                {
                    d_in.dav = 0;
                    d_in.met = "ellvn";
                }
                else
                {
                    d_in.dav = 1;
                    d_in.met = "ellnar";
                    E_tb.ReadOnly = false;
                    E_tb.Text = Convert.ToString(CalcClass.GetE(d_in.steel, d_in.temp));
                    E_tb.ReadOnly = true;

                    try
                    {
                        if (Convert.ToDouble(E_tb.Text) > 0)
                        {
                            d_in.E = Convert.ToInt32(E_tb.Text);
                        }
                        else
                        {
                            data_inerr += "E должно быть больше 0\n";
                        }
                    }
                    catch
                    {
                        data_inerr += "E неверные данные\n";
                    }
                }
            }

            try
            {
                if ((Convert.ToDouble(p_tb.Text) > 0) && (Convert.ToDouble(p_tb.Text) < 1000))
                {
                    d_in.p = Convert.ToDouble(p_tb.Text);
                }
                else
                {
                    data_inerr += "p должно быть в диапазоне 0 - 1000\n";
                }
            }
            catch
            {
                data_inerr += "p должно быть в диапазоне 0 - 1000\n";
            }

            try
            {
                if ((Convert.ToDouble(fi_tb.Text) > 0) && (Convert.ToDouble(fi_tb.Text) <= 1))
                {
                    d_in.fi = Convert.ToDouble(fi_tb.Text);
                }
                else
                {
                    data_inerr += "fi должен быть в диапазоне 0 - 1\n";
                }
            }
            catch
            {
                data_inerr += "fi должен быть в диапазоне 0 - 1\n";
            }

            try
            {
                if (Convert.ToInt32(D_tb.Text) > 0)
                {
                    d_in.D = Convert.ToInt32(D_tb.Text);
                }
                else
                {
                    data_inerr += "D должно быть больше 0\n";
                }
            }
            catch
            {
                data_inerr += "D неверные данные\n";
            }

            try
            {
                if (Convert.ToInt32(H_tb.Text) > 0)
                {
                    d_in.elH = Convert.ToInt32(H_tb.Text);
                }
                else
                {
                    data_inerr += "H должно быть больше 0\n";
                }
            }
            catch
            {
                data_inerr += "H неверные данные\n";
            }

            try
            {
                if (Convert.ToInt32(h1_tb.Text) > 0)
                {
                    d_in.elh1 = Convert.ToInt32(h1_tb.Text);
                }
                else
                {
                    data_inerr += "h1 должно быть больше 0\n";
                }
            }
            catch
            {
                data_inerr += "h1 неверные данные\n";
            }

            try
            {
                if (Convert.ToDouble(c1_tb.Text) >= 0)
                {
                    d_in.c1 = Convert.ToDouble(c1_tb.Text);
                }
                else
                {
                    data_inerr += "c1 неверные данные\n";
                }
            }
            catch
            {
                data_inerr += "c1 неверные данные\n";
            }

            try
            {
                if (Convert.ToDouble(c2_tb.Text) >= 0)
                {
                    d_in.c2 = Convert.ToDouble(c2_tb.Text);
                }
                else
                {
                    data_inerr += "c2 неверные данные\n";
                }
            }
            catch
            {
                data_inerr += "c2 неверные данные\n";
            }

            try
            {
                if (c3_tb.Text == "")
                {
                    d_in.c3 = 0;
                }
                else if (Convert.ToDouble(c3_tb.Text) >= 0)
                {
                    d_in.c3 = Convert.ToDouble(c3_tb.Text);
                }
                else
                {
                    data_inerr += "c3 неверные данные\n";
                }
            }
            catch
            {
                data_inerr += "c3 неверные данные\n";
            }

            try
            {
                if (Convert.ToDouble(s_tb.Text) >= 0)
                {
                    d_in.s = Convert.ToDouble(s_tb.Text);
                }
                else
                {
                    data_inerr += "s неверные данные\n";
                }
            }
            catch
            {
                data_inerr += "s неверные данные\n";
            }

            if (ell_rb.Checked)
            {
                d_in.ellType = "ell";
            }
            else
            {
                d_in.ellType = "polysfer";
            }


            if (data_inerr == "")
            {
                string v = "";

                Data_out d_out = CalcClass.CalcEll(d_in);
                if (d_out.err == "") // если нет ошибок расчета
                {
                    p_d_l.Text = $"[p]={d_out.p_d:f2} МПа";
                    scalc_l.Text = $"sp={d_out.s_calc:f3} мм";

                    if (this.Owner is MainForm main)
                    {
                        int i;
                        main.Word_lv.Items.Add($"{d_in.D} мм, {d_in.p} МПа, {d_in.temp} C, {d_in.met}");
                        i = main.Word_lv.Items.Count - 1;
                        dataArrEl.Data_In = d_in;
                        dataArrEl.Data_Out = d_out;
                        dataArrEl.id = i + 1;
                        dataArrEl.Typ = "ell";

                        DataWordOut.DataArr.Add(DataArrEl);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("MainForm Error");
                    }
                    if (d_out.ypf == false)
                    {
                        v = "Условия применения формул не выполняется";
                    }
                    if (d_out.p_d < d_in.p)
                    {
                        v += "\n[p] меньше p";
                    }
                    if (v == "")
                    {
                        System.Windows.Forms.MessageBox.Show("Calculation complete");
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show(v);
                    }
                    MessageBoxCheckBox mbcb = new MessageBoxCheckBox { Owner = this };
                    mbcb.ShowDialog();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(d_out.err);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(data_inerr);
            }
        }

        private void Ell_Polysfer_rb_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                if (rb.Name == "ell_rb")
                {
                    pictureBox.Image = calcNet.Properties.Resources.Ell;
                }
                else if (rb.Name == "polysfer_rb")
                {
                    pictureBox.Image = calcNet.Properties.Resources.Sfer;
                }
            }
        }
    }
}
