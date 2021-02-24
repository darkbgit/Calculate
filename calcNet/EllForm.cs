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

        EllipticalShellDataIn ellipticalShellDataIn = new EllipticalShellDataIn();

        private void EllForm_Load(object sender, EventArgs e)
        {
            SetSteelList.SetList(steel_cb);
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
            private List<string> dataInErr = new List<string>();

        //t
        //InputClass.GetInput_t(t_tb, ref d_in, ref dataInErr);

        //steel
    
        ellipticalShellDataIn.Steel = steel_cb.Text;

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
                }
            }

            //p
            InputClass.GetInput_p(p_tb, ref d_in, ref dataInErr);

            //fi
            InputClass.GetInput_fi(fi_tb, ref d_in, ref dataInErr);

            //D
            InputClass.GetInput_D(D_tb, ref d_in, ref dataInErr);

            //H

            try
            {
                if (Convert.ToInt32(H_tb.Text) > 0)
                {
                    d_in.elH = Convert.ToInt32(H_tb.Text);
                }
                else
                {
                    dataInErr += "H должно быть больше 0\n";
                }
            }
            catch
            {
                dataInErr += "H неверные данные\n";
            }

            //h1
            try
            {
                if (Convert.ToInt32(h1_tb.Text) > 0)
                {
                    d_in.elh1 = Convert.ToInt32(h1_tb.Text);
                }
                else
                {
                    dataInErr += "h1 должно быть больше 0\n";
                }
            }
            catch
            {
                dataInErr += "h1 неверные данные\n";
            }

            //c1
            InputClass.GetInput_c1(c1_tb, ref d_in, ref dataInErr);

            //c2
            InputClass.GetInput_c2(c2_tb, ref d_in, ref dataInErr);

            //c3
            InputClass.GetInput_c3(c3_tb, ref d_in, ref dataInErr);

            if (ell_rb.Checked)
                d_in.ellipticalBottomType = EllipticalBottomType.Elliptical;
            else
                d_in.ellipticalBottomType = EllipticalBottomType.Hemispherical;
            

            if (dataInErr == "")
            {
                //Data_out d_out = new Data_out();
                //CalcClass.CalculateShell(in d_in, ref d_out);
                //if (d_out.err != null)
                //{
                //    System.Windows.Forms.MessageBox.Show(d_out.err);
                //}
                //c_tb.Text = $"{d_out.c:f2}";
                //scalc_l.Text = $"sp={d_out.s_calc:f3} мм";
                //calc_b.Enabled = true;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(dataInErr);
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
            Data_in d_in = new Data_in(ShellType.Elliptical);

            string dataInErr = "";

            //name
            d_in.Name = name_tb.Text;

            //t
            //InputClass.GetInput_t(t_tb, ref d_in, ref dataInErr);
            
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
                }
            }

            //p
            InputClass.GetInput_p(p_tb, ref d_in, ref dataInErr);

            //fi
            InputClass.GetInput_fi(fi_tb, ref d_in, ref dataInErr);

            //D
            InputClass.GetInput_D(D_tb, ref d_in, ref dataInErr);


            try
            {
                if (Convert.ToInt32(H_tb.Text) > 0)
                {
                    d_in.elH = Convert.ToInt32(H_tb.Text);
                }
                else
                {
                    dataInErr += "H должно быть больше 0\n";
                }
            }
            catch
            {
                dataInErr += "H неверные данные\n";
            }

            try
            {
                if (Convert.ToInt32(h1_tb.Text) > 0)
                {
                    d_in.elh1 = Convert.ToInt32(h1_tb.Text);
                }
                else
                {
                    dataInErr += "h1 должно быть больше 0\n";
                }
            }
            catch
            {
                dataInErr += "h1 неверные данные\n";
            }

            //c1
            InputClass.GetInput_c1(c1_tb, ref d_in, ref dataInErr);

            //c2
            InputClass.GetInput_c2(c2_tb, ref d_in, ref dataInErr);

            //c3
            InputClass.GetInput_c3(c3_tb, ref d_in, ref dataInErr);

            //s
            InputClass.GetInput_s(s_tb, ref d_in, ref dataInErr);

            if (ell_rb.Checked)
            {
                d_in.ellipticalBottomType = EllipticalBottomType.Elliptical;
            }
            else
            {
                d_in.ellipticalBottomType = EllipticalBottomType.Hemispherical;
            }

            isNotError = dataInErr == "";
            if (isNotError)
            {
                //Data_out d_out = new Data_out();
                //CalcClass.Calculate(in d_in, ref d_out);
                //if (!d_out.isCriticalError) // если нет ошибок расчета
                //{
                //    p_d_l.Text = $"[p]={d_out.p_d:f2} МПа";
                //    scalc_l.Text = $"sp={d_out.s_calc:f3} мм";

                //    if (this.Owner is MainForm main)
                //    {
                //        int i;
                //        main.Word_lv.Items.Add($"{d_in.D} мм, {d_in.p} МПа, {d_in.temp} C, {d_in.shellType}");
                //        i = main.Word_lv.Items.Count;

                //        DataWordOut.DataOutArrEl dataArrEl = new DataWordOut.DataOutArrEl
                //        {
                //            Data_In = d_in,
                //            Data_Out = d_out,
                //            id = i,
                //            calculatedElementType = CalculatedElementType.Elliptical
                //        };

                //        DataInOutShell = dataArrEl;

                //        DataWordOut.DataArr.Add(DataInOutShell);
                //    }
                //    else
                //    {
                //        System.Windows.Forms.MessageBox.Show("MainForm Error");
                //    }

                //    if (d_out.isError)
                //    {
                //        System.Windows.Forms.MessageBox.Show(d_out.err);
                //    }
                    
                //    System.Windows.Forms.MessageBox.Show("Calculation complete");
                    
                //    MessageBoxCheckBox mbcb = new MessageBoxCheckBox { Owner = this };
                //    mbcb.ShowDialog();
                //}
                //else
                //{
                //    System.Windows.Forms.MessageBox.Show(d_out.err);
                //}
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(dataInErr);
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
