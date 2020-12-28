using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace calc
{
    public partial class CilForm : Form
    {
        public CilForm()
        {
            InitializeComponent();
        }

        public struct DataForm
        {
            internal Data_in Data_In;// { get; set; }
            internal Data_out Data_Out;// { get; set; }
            internal string Typ; // cil, ell, kon, cilyk, konyk, ellyk, saddle, heat
        }
        DataForm df = new DataForm();

        private void Cancel_b_Click(object sender, EventArgs e)
        {
            //pictureBox1.Image.Dispose();
            //f_pb.Image.Dispose();
            this.Hide();
        }


        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            // приводим отправителя к элементу типа RadioButton
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                
                //var path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @"pic\PC\PC" + rb.Text + ".gif");
                //MessageBox.Show(path);
                Bitmap img = new Bitmap(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @"pic\PC\PC" + rb.Text + ".gif"));
                f_pb.Image = img;
            }
        }

        private void Pressure_rb(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                if (rb.Name == "vn_rb")
                {
                    l_tb.Enabled = false;
                    l_tb.ReadOnly = true;
                    getE_b.Enabled = false;
                    getL_b.Enabled = false;
                }
                else if (rb.Name == "nar_rb")
                {
                    l_tb.Enabled = true;
                    l_tb.ReadOnly = false;
                    getE_b.Enabled = true;
                    getL_b.Enabled = true;
                }
            }
        }

        private void PredCalc_b_Click(object sender, EventArgs e)
        {
            Data_in d_in = new Data_in();
            //Data_out d_out = new Data_out();

            //CalcClass cc = new CalcClass();

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

                    try
                    {
                        if (Convert.ToDouble(l_tb.Text) > 0)
                        {
                            d_in.l = Convert.ToDouble(l_tb.Text);
                        }
                        else
                        {
                            data_inerr += "l должно быть больше 0\n";
                        }
                    }
                    catch
                    {
                        data_inerr += "l неверные данные\n";
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

            if (data_inerr == "")
            {
                Data_out d_out = CalcClass.CalcCil(d_in);
                if (d_out.err == "")
                {
                    c_tb.Text = Convert.ToString(Math.Round(d_out.c, 2));
                    scalc_l.Text = $"sp={d_out.s_calc:f3} мм";
                    calc_b.Enabled = true;
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

        private void Calc_b_Click(object sender, EventArgs e)
        {
            Data_in d_in = new Data_in();
            //Data_out d_out = new Data_out();

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
            }//temp

            d_in.steel = steel_cb.Text;

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
            }//sigma_d

            if (vn_rb.Checked)
            {
                d_in.dav = 0;
                d_in.met = "obvn";
            }
            else
            {
                d_in.dav = 1;
                d_in.met = "obnar";
                
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
                }//E

                try
                {
                    if (Convert.ToDouble(l_tb.Text) > 0)
                    {
                        d_in.l = Convert.ToDouble(l_tb.Text);
                    }
                    else
                    {
                        data_inerr += "l должно быть больше 0\n";
                    }
                }
                catch
                {
                    data_inerr += "l неверные данные\n";
                }//l
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
            }//p

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
            }//fi

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
            }//D

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
            }//c1

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
            }//c2

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
            }//c3

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
            }//s

            if (data_inerr == "") // если данные введены правильно
            {
                //CalcClass cc = new CalcClass();
                string v = "";
                
                Data_out d_out = CalcClass.CalcCil(d_in);
                if (d_out.err == "") // если нет ошибок расчета
                {
                    p_d_l.Text = $"[p]={d_out.p_d:f2} МПа";
                    scalc_l.Text = $"sp={d_out.s_calc:f3} мм";
                    //self.pbObToNozzle.setEnabled(True)
                    MainForm main = this.Owner as MainForm;
                    if (main != null)
                    {
                        int i;
                        main.Word_lv.Items.Add($"{d_in.D} мм, {d_in.p} МПа, {d_in.temp} C, {d_in.met}");
                        i = main.Word_lv.Items.Count - 1;
                        //DataWordOut.DataArr[0].  DataArr .DataOutArr[]. .Value = $"{d_in.D} мм, {d_in.p} МПа, {d_in.temp} C, {d_in.met}";
                        DataWordOut.DataOutArrEl el = new DataWordOut.DataOutArrEl();
                        el.Data_In = d_in;
                        df.Data_In = d_in;
                        el.Data_Out = d_out;
                        df.Data_Out = d_out;
                        el.id = i+1;
                        el.Typ = "cil";
                        df.Typ = "cil";

                        DataWordOut.DataArr[i] = el;

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
                    MessageBoxCheckBox mbcb = new MessageBoxCheckBox();
                    mbcb.Owner = this.Owner;
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


            //    data_out = cc.calc_ob(data_in)
            //    globalvar.elementdatayk = [data_in, data_out]
            //    data_word.append([data_in, data_out])
            //    i = word_lv.rowCount()
            //    word_lv.insertRow(i)
            //    word_lv.setData(word_lv.index(i), f'{data_in.dia} мм, {data_in.press} МПа, {data_in.temp} C, {data_in.met}')
            //    self.parent().lvCalc.setModel(word_lv)

            //    self.s_calc_lob.setText(f'sp={data_out.s_calc:.3f} мм')
            //    self.press_d_lob.setText(f'[p]={data_out.press_d:.3f} МПа')
                
        }

        private void Set_steellist()
        {
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @"data\data.xml"));
                var root = doc.DocumentElement;
                XmlNodeList steels = root.SelectNodes("sigma_list/steels/steel");
                foreach (XmlNode steel in steels)
                {
                    steel_cb.Items.Add(steel.Attributes["name"].Value);
                }
            }
        }

        private void CilForm_Load(object sender, EventArgs e)
        {
            Set_steellist();
            steel_cb.SelectedIndex = 0;
            Gost_cb.SelectedIndex = 0;
        }

        private void GetE_b_Click(object sender, EventArgs e)
        {
            //CalcClass cc = new CalcClass();
            E_tb.Text = Convert.ToString(CalcClass.GetE(steel_cb.Text, Convert.ToInt32(t_tb.Text)));

        }

        private void GetFi_b_Click(object sender, EventArgs e)
        {
            FiForm ff = new FiForm(); // создаем
            ff.Owner = this;
            ff.ShowDialog(); // показываем
        }

        private void CilForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sender is Button)
            {
                if (this.Owner is MainForm main)
                {
                    if (main != null)
                    {
                        main.cf = null;
                    }
                }
            }
        }
    }
}
