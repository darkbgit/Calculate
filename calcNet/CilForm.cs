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

        private static void TrySet(in string name, in double val, ref DataShellIn d_in, ref bool isNotError)
        {
            //try
            //{
                d_in.SetValue(name, val);
            //}
            //catch (ArgumentOutOfRangeException ex)
            //{
            //    isNotError = false;
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void PredCalc_b_Click(object sender, EventArgs e)
        {
            c_tb.Text = "";
            scalc_l.Text = "";
            calc_b.Enabled = false;

            DataShellIn d_in = new DataShellIn();
            //Data_in d_in = new Data_in(ShellType.Cylindrical);

            string dataInErr = "";

            d_in.IsPressureIn = vn_rb.Checked;

            //InputClass.GetInput_t(t_tb, ref d_in); 

            bool isNotError = true;
            {
                string[] TextBoxNames =
                {
                    "Name_tb",
                    "Gost_cb",
                    "t_tb",
                    "p_tb",
                    "Steel_cb",
                    "sigma_d_tb",
                    "E_tb",
                    "fi_tb",
                    "D_tb",
                    "l_tb",
                    "c1_tb",
                    "c2_tb",
                    "c3_tb",
                    "s_tb"
                };
                foreach (string tb in TextBoxNames)
                {
                    if (tb == "sigma_d_tb")
                    {
                        double sigma = 0;
                        string str = "";
                        if (CalcClass.GetSigma(d_in.Steel, d_in.t, ref sigma, ref str))
                        {
                            sigma_d_tb.Text = sigma.ToString();
                        }
                        else
                        {
                            isNotError = false;
                            MessageBox.Show(str);
                            break;
                        }
                    }

                    Control control = Controls[tb] ?? Controls["dav_gb"].Controls[tb];
                    //if (control is TextBox || control is ComboBox)

                    string name = control.Name;
                    name = name.Remove(name.Length - 3, 3);


                    var type = typeof(DataShellIn).GetProperty(name)?.PropertyType;

                    double val = 0;
                    if (type != null && type.Equals(typeof(double)))
                    {
                        if (control.Text == "")
                        {
                            //try
                            //{
                            //    d_in.SetValue(name, val);
                            //}
                            ////catch (ArgumentOutOfRangeException ex)
                            ////{
                            ////    isNotError = false;
                            ////    MessageBox.Show(ex.Message);
                            ////    break;
                            ////}
                            
                        }
                        else if (double.TryParse((control as TextBox).Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                                System.Globalization.CultureInfo.InvariantCulture, out val))
                        {
                            //InputClass.TrySetValue(in name, in val, ref d_in, ref isNotError);
                            try
                            {
                                d_in.SetValue(name, val);
                            }
                            catch (ArgumentOutOfRangeException ex)
                            {
                                isNotError = false;
                                MessageBox.Show(ex.Message);
                                break;
                            }
                        }
                        else
                        {
                            isNotError = false;
                            MessageBox.Show($"{name} неверный формат");
                            break;
                        }
                    }
                    else if (type != null && type.Equals(typeof(string)))
                    {
                        d_in.SetValue(name, control.Text);
                    }
                    //MessageBox.Show($"{name} is {val} text {control.Text}");
                }

            }

            {
                //t
                //InputClass.GetInput_t(t_tb, ref d1_in);

                //d_in.Steel = steel_cb.Text;

                //bool isNotError = dataInErr == "";
                //if (!d1_in.IsInError)
                //{
                //    //[σ]
                //    InputClass.GetInput_sigma_d(sigma_d_tb, ref d_in, ref dataInErr);



                //    if (!d_in.isPressureIn)
                //    {
                //        //E
                //        InputClass.GetInput_E(E_tb, ref d_in, ref dataInErr);

                //        //l
                //        InputClass.GetInput_l(l_tb, ref d_in, ref dataInErr);
                //    }

                //    //p
                //    InputClass.GetInput_p(p_tb, ref d_in, ref dataInErr);

                //    //fi
                //    InputClass.GetInput_fi(fi_tb, ref d_in, ref dataInErr);

                //    //D
                //    InputClass.GetInput_D(D_tb, ref d_in, ref dataInErr);

                //    //c1
                //    InputClass.GetInput_c1(c1_tb, ref d_in, ref dataInErr);

                //    //c2
                //    InputClass.GetInput_c2(c2_tb, ref d_in, ref dataInErr);

                //    //c3
                //    InputClass.GetInput_c3(c3_tb, ref d_in, ref dataInErr);
            }

            
            if (isNotError)
            {
                //Data_out d_out = new Data_out();
                Cylinder cyl = new Cylinder(d_in);
                if (cyl.IsError)
                {
                    System.Windows.Forms.MessageBox.Show(cyl.ErrorString);
                }
                c_tb.Text = $"{cyl.c:f2}";
                scalc_l.Text = $"sp={cyl.s_calc:f3} мм";
                calc_b.Enabled = true;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(dataInErr);
            }

            //else
            //{
            //    System.Windows.Forms.MessageBox.Show(dataInErr);
            //}
        }
    
        

        private void Calc_b_Click(object sender, EventArgs e)
        {
            Data_in d_in = new Data_in(ShellType.Cylindrical);

            string dataInErr = "";

            //name
            d_in.Name = Name_tb.Text;

            //t
            //InputClass.GetInput_t(t_tb, ref d_in, ref dataInErr);

            //steel
            d_in.Steel = Steel_cb.Text;

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
            Set_steellist.Set_llist(Steel_cb);
            Gost_cb.SelectedIndex = 0;
        }

        private void GetE_b_Click(object sender, EventArgs e)
        {
            double E = 0;
            string dataInErr = "";
            CalcClass.GetE(Steel_cb.Text, Convert.ToInt32(t_tb.Text), ref E, ref dataInErr);
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

