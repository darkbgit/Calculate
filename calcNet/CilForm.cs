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

namespace calcNet
{
    public partial class CilForm : FormShell
    {
        public CilForm()
        {
            InitializeComponent();
        }

        //private DataWordOut.DataOutArrEl dataArrEl;
        //internal DataWordOut.DataOutArrEl DataInOutShell { get => dataArrEl; set => dataArrEl = value; }

        //public struct DataForm
        //{
        //    internal Data_in Data_In;// { get; set; }
        //    internal Data_out Data_Out;// { get; set; }
        //    internal string Typ; // cil, ell, kon, cilyk, konyk, ellyk, saddle, heat
        //}
        //public static DataForm Df;



        //public static DataForm Df { get => df; set => df = value; }

        private void Cancel_b_Click(object sender, EventArgs e)
        {
            //pictureBox1.Image.Dispose();
            //f_pb.Image.Dispose();
            this.Hide();
        }


        private void Force_rb_CheckedChanged(object sender, EventArgs e)
        {
            // приводим отправителя к элементу типа RadioButton
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                f_pb.Image = (Bitmap)calcNet.Properties.Resources.ResourceManager.GetObject("PC" + rb.Text);
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
            Data_in d_in = new Data_in(ShellType.Cylindrical);

            string dataInErr = "";

            {
                /*
                foreach (Control control in Controls)
                {
                    if (control is TextBox)
                    {
                        string name;
                        double value;
                        name = (control as TextBox).Name;
                        name = name.Remove(name.Length - 3, 3);
                        try
                        {
                            value = Convert.ToDouble((control as TextBox).Text.Replace(',', '.'),
                                                            System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch (FormatException)
                        {
                            value = 0;
                            data_inerr += $"{name} неверный ввод\n";
                        }
                        if (value > 0)
                        {
                            d_in.SetValue(name, value);
                        }
                        else
                        {
                            data_inerr += $"{name} должно быть больше 0";
                        }
                    }
                }
                */
            }

            //t
            InputClass.GetInput_t(t_tb, ref d_in, ref dataInErr);

            d_in.Steel = steel_cb.Text;

            bool isNotError = dataInErr == "";
            if (isNotError)
            {
                //[σ]
                InputClass.GetInput_sigma_d(sigma_d_tb, ref d_in, ref dataInErr); 

                d_in.isPressureIn = vn_rb.Checked;

                if (!d_in.isPressureIn)
                {
                    //E
                    InputClass.GetInput_E(E_tb, ref d_in, ref dataInErr);

                    //l
                    InputClass.GetInput_l(l_tb, ref d_in, ref dataInErr);
                }

                //p
                InputClass.GetInput_p(p_tb, ref d_in, ref dataInErr);

                //fi
                InputClass.GetInput_fi(fi_tb, ref d_in, ref dataInErr);

                //D
                InputClass.GetInput_D(D_tb, ref d_in, ref dataInErr);

                //c1
                InputClass.GetInput_c1(c1_tb, ref d_in, ref dataInErr);

                //c2
                InputClass.GetInput_c2(c2_tb, ref d_in, ref dataInErr);

                //c3
                InputClass.GetInput_c3(c3_tb, ref d_in, ref dataInErr);
                
                isNotError = dataInErr == "";
                if (isNotError)
                {
                    Data_out d_out = new Data_out();
                    CalcClass.CalculateShell(in d_in, ref d_out);
                    if (d_out.err != null)
                    {
                        System.Windows.Forms.MessageBox.Show(d_out.err);
                    }
                    c_tb.Text = $"{d_out.c:f2}";
                    scalc_l.Text = $"sp={d_out.s_calc:f3} мм";
                    calc_b.Enabled = true;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(dataInErr);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(dataInErr);
            }
        }

        private void Calc_b_Click(object sender, EventArgs e)
        {
            Data_in d_in = new Data_in(ShellType.Cylindrical);
            
            string dataInErr = "";

            //name
            d_in.Name = name_tb.Text;

            //t
            InputClass.GetInput_t(t_tb, ref d_in, ref dataInErr);

            //steel
            d_in.Steel = steel_cb.Text;

            bool isNotError = dataInErr == "";
            if (isNotError)
            {
                //[σ]
                InputClass.GetInput_sigma_d(sigma_d_tb, ref d_in, ref dataInErr);

                d_in.isPressureIn = vn_rb.Checked;

                if (!d_in.isPressureIn)
                {
                    //E
                    InputClass.GetInput_E(E_tb, ref d_in, ref dataInErr);

                    //l
                    InputClass.GetInput_l(l_tb, ref d_in, ref dataInErr);
                }

                //p
                InputClass.GetInput_p(p_tb, ref d_in, ref dataInErr);

                //fi
                InputClass.GetInput_fi(fi_tb, ref d_in, ref dataInErr);

                //D
                InputClass.GetInput_D(D_tb, ref d_in, ref dataInErr);

                //c1
                InputClass.GetInput_c1(c1_tb, ref d_in, ref dataInErr);

                //c2
                InputClass.GetInput_c2(c2_tb, ref d_in, ref dataInErr);

                //c3
                InputClass.GetInput_c3(c3_tb, ref d_in, ref dataInErr);

                //s
                InputClass.GetInput_s(s_tb, ref d_in, ref dataInErr);

                isNotError = dataInErr == "";
                if (isNotError) // если данные введены правильно
                {
                    Data_out d_out = new Data_out();
                    CalcClass.CalculateShell(in d_in, ref d_out);
                    if (!d_out.isCriticalError) // если нет ошибок расчета
                    {
                        p_d_l.Text = $"[p]={d_out.p_d:f2} МПа";
                        scalc_l.Text = $"sp={d_out.s_calc:f3} мм";

                        if (this.Owner is MainForm main)
                        {
                            main.Word_lv.Items.Add($"{d_in.D} мм, {d_in.p} МПа, {d_in.temp} C, {d_in.shellType}");
                            int listItemsCount;
                            listItemsCount = main.Word_lv.Items.Count;

                            DataWordOut.DataOutArrEl dataArrEl = new DataWordOut.DataOutArrEl
                            {
                                id = listItemsCount,
                                Data_In = d_in,
                                Data_Out = d_out,
                                calculatedElementType = CalculatedElementType.Cylindrical
                            };

                            DataInOutShell = dataArrEl;


                            DataWordOut.DataArr.Add(DataInOutShell);
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("MainForm Error");
                        }

                        if (d_out.isError)
                        {
                            System.Windows.Forms.MessageBox.Show(d_out.err);
                        }
                        
                        System.Windows.Forms.MessageBox.Show("Calculation complete");

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
                    System.Windows.Forms.MessageBox.Show(dataInErr);
                }
            }
        }

        

        private void CilForm_Load(object sender, EventArgs e)
        {
            Set_steellist.Set_llist(steel_cb);
            Gost_cb.SelectedIndex = 0;
        }

        private void GetE_b_Click(object sender, EventArgs e)
        {
            double E = 0;
            string dataInErr = "";
            CalcClass.GetE(steel_cb.Text, Convert.ToInt32(t_tb.Text), ref E, ref dataInErr);
            E_tb.Text = E.ToString();
        }

        private void GetFi_b_Click(object sender, EventArgs e)
        {
            FiForm ff = new FiForm { Owner = this };
            ff.ShowDialog(); // показываем
        }

        private void CilForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sender is CilForm)
            {
                if (this.Owner is MainForm main)
                {
                    if (main.cf != null)
                    {
                        main.cf = null;
                    }
                }
            }
        }

        private void Stress_rb_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
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
