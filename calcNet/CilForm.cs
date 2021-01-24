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
    public partial class CilForm : Form
    {
        public CilForm()
        {
            InitializeComponent();
        }

        internal const string TypeElement = "cil";

        private DataWordOut.DataOutArrEl dataArrEl;
        internal DataWordOut.DataOutArrEl DataArrEl { get => dataArrEl; set => dataArrEl = value; }

        public struct DataForm
        {
            internal Data_in Data_In;// { get; set; }
            internal Data_out Data_Out;// { get; set; }
            internal string Typ; // cil, ell, kon, cilyk, konyk, ellyk, saddle, heat
        }
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


        private void GetInput_t(ref Data_in d_in, ref string dataInErr)
        {
            if (int.TryParse(t_tb.Text, out int t))
            {
                const int MIN_TEMPERATURE = 20,
                        MAX_TEMPERATURE = 1000;
                if (t >= MIN_TEMPERATURE && t < MAX_TEMPERATURE)
                {
                    d_in.temp = t;
                }
                else
                {
                    dataInErr += "T должна быть в диапазоне 20 - 1000\n";
                }
            }
            else
            {
                dataInErr += "T неверный ввод\n";
            }
        }

        private void GetInput_sigma_d(ref Data_in d_in, ref string dataInErr)
        {
            double sigma_d = 0;
            if (!sigma_d_tb.ReadOnly)
            {
                if (double.TryParse(sigma_d_tb.Text.Replace(',', '.'), System.Globalization.NumberStyles.None,
                    System.Globalization.CultureInfo.InvariantCulture, out sigma_d))
                {
                    if (sigma_d > 0)
                    {
                        d_in.sigma_d = sigma_d;
                    }
                    else
                    {
                        dataInErr += "[σ] должно быть больше 0\n";
                    }
                }
                else
                {
                    dataInErr += "[σ] неверный ввод\n";
                }
            }
            else
            {
                if (CalcClass.GetSigma(d_in.steel, d_in.temp, ref sigma_d, ref dataInErr))
                {
                    //sigma_d = CalcClass.GetSigma(d_in.steel, d_in.temp);
                    sigma_d_tb.ReadOnly = false;
                    sigma_d_tb.Text = sigma_d.ToString();
                    sigma_d_tb.ReadOnly = true;
                }
            }
        }

        private void GetInput_l(ref Data_in d_in, ref string dataInErr)
        {
            if (double.TryParse(l_tb.Text.Replace(',', '.'), System.Globalization.NumberStyles.None,
                    System.Globalization.CultureInfo.InvariantCulture, out double l))
            {
                if (l > 0)
                {
                    d_in.l = l;
                }
                else
                {
                    dataInErr += "l должно быть больше 0\n";
                }
            }
            else
            {
                dataInErr += "l неверный ввод\n";
            }
        }

        private void PredCalc_b_Click(object sender, EventArgs e)
        {
            Data_in d_in = new Data_in();

            string dataInErr = "";

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

            //t
            GetInput_t(ref d_in, ref dataInErr);

            d_in.steel = steel_cb.Text;

            bool isNotError = dataInErr == "";
            if (isNotError)
            {
                GetInput_sigma_d(ref d_in, ref dataInErr); //[σ]

                d_in.isPressureIn = vn_rb.Checked;

                if (!d_in.isPressureIn)
                {
                    //E
                    {
                        double E;
                        if (!E_tb.ReadOnly)
                        {
                            try
                            {
                                E = Convert.ToDouble(E_tb.Text.Replace(',', '.'),
                                    System.Globalization.CultureInfo.InvariantCulture);
                            }
                            catch (FormatException)
                            {
                                E = 0;
                                dataInErr += "E неверный ввод\n";
                            }
                        }
                        else
                        {
                            E = CalcClass.GetE(d_in.steel, d_in.temp);
                            E_tb.ReadOnly = false;
                            E_tb.Text = E.ToString();
                            E_tb.ReadOnly = true;
                        }
                        if (E > 0)
                        {
                            d_in.E = E;
                        }
                        else
                        {
                            dataInErr += "E должно быть больше 0\n";
                        }
                    }
                    
                    GetInput_l(ref d_in, ref dataInErr);//l
                }

                //p
                {
                    double p;
                    if (double.TryParse(p_tb.Text.Replace(',', '.'), System.Globalization.NumberStyles.AllowDecimalPoint,
                        System.Globalization.CultureInfo.InvariantCulture, out p))
                    {
                        const int NO_PRESSURE = 0,
                                    MAX_PRESSURE = 200;
                        if (p > NO_PRESSURE && p < MAX_PRESSURE)
                        {
                            d_in.isNeedpCalculate = true;
                            d_in.p = p;
                        }
                        else
                        {
                            dataInErr += $"p должно быть в диапазоне {NO_PRESSURE} - {MAX_PRESSURE}\n";
                        }
                    }
                    else
                    {
                        dataInErr += "p неверный ввод\n";
                    }
                }

                //fi
                {
                    double fi;
                    try
                    {
                        fi = Convert.ToDouble(fi_tb.Text.Replace(',', '.'),
                                            System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch (FormatException)
                    {
                        fi = 0;
                        dataInErr += "fi неверный ввод\n";
                    }
                    const int MIN_FI = 0,
                            MAX_FI = 1;
                    if (fi > MIN_FI && fi <= MAX_FI)
                    {
                        d_in.fi = fi;
                    }
                    else
                    {
                        dataInErr += "fi должен быть в диапазоне 0 - 1\n";
                    }
                }

                //D
                {
                    double D;
                    try
                    {
                        D = Convert.ToDouble(D_tb.Text.Replace(',', '.'),
                                            System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch (FormatException)
                    {
                        D = 0;
                        dataInErr += "D неверный ввод\n";
                    }
                    if (D > 0)
                    {
                        d_in.D = D;
                    }
                    else
                    {
                        dataInErr += "D должно быть больше 0\n";
                    }
                }

                //c1
                {
                    double c1;
                    try
                    {
                        c1 = Convert.ToDouble(c1_tb.Text.Replace(',', '.'),
                                            System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch (FormatException)
                    {
                        c1 = -1;
                        dataInErr += "c1 неверный ввод\n";
                    }
                    if (c1 >= 0)
                    {
                        d_in.c1 = c1;
                    }
                    else
                    {
                        dataInErr += "c1 должно быть больше 0\n";
                    }
                }

                //c2
                {
                    double c2;
                    try
                    {
                        c2 = Convert.ToDouble(c2_tb.Text.Replace(',', '.'),
                                            System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch (FormatException)
                    {
                        c2 = -1;
                        dataInErr += "c2 неверный ввод\n";
                    }
                    if (c2 >= 0)
                    {
                        d_in.c2 = c2;
                    }
                    else
                    {
                        dataInErr += "c2 должно быть больше 0\n";
                    }
                }

                //c3
                {
                    if (c3_tb.Text == "")
                    {
                        d_in.c3 = 0;
                    }
                    else
                    {
                        double c3;
                        try
                        {
                            c3 = Convert.ToDouble(c3_tb.Text,
                                                System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch (FormatException)
                        {
                            c3 = -1;
                            dataInErr += "c3 неверный ввод\n";
                        }
                        if (c3 >= 0)
                        {
                            d_in.c3 = c3;
                        }
                        else
                        {
                            dataInErr += "должно быть >= 0\n";
                        }
                    }
                }


                isNotError = dataInErr == "";
                if (isNotError)
                {
                    Data_out d_out = CalcClass.CalculateCilindricalShell(in d_in);
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
            Data_in d_in = new Data_in();
            //Data_out d_out = new Data_out();

            string data_inerr = "";

            //name
            d_in.name = name_tb.Text;

            //t
            {
                int t;
                try
                {
                    t = Convert.ToInt32(t_tb.Text);
                }
                catch (FormatException)
                {
                    t = 0;
                    data_inerr += "T неверный ввод\n";
                }
                const int MIN_TEMPERATURE = 20,
                            MAX_TEMPERATURE = 1000;
                if (t >= MIN_TEMPERATURE && t < MAX_TEMPERATURE)
                {
                    d_in.temp = t;
                }
                else
                {
                    data_inerr += "T должна быть в диапазоне 20 - 1000\n";
                }
            }

            //steel
            d_in.steel = steel_cb.Text;

            bool isNotError = data_inerr == "";
            if (isNotError)
            {
                //[σ]
                {
                    double sigma_d;
                    if (!sigma_d_tb.ReadOnly)
                    {
                        try
                        {
                            sigma_d = Convert.ToDouble(sigma_d_tb.Text.Replace(',', '.'),
                                                        System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch (FormatException)
                        {
                            sigma_d = 0;
                            data_inerr += "[σ] неверный ввод\n";
                        }
                    }
                    else
                    {
                        //sigma_d = CalcClass.GetSigma(d_in.steel, d_in.temp);
                        sigma_d_tb.ReadOnly = false;
                        // sigma_d_tb.Text = sigma_d.ToString();
                        sigma_d_tb.ReadOnly = true;
                    }
                    if (true)// if (sigma_d > 0)
                    {
                        // d_in.sigma_d = sigma_d;
                    }
                    else
                    {
                        data_inerr += "[σ] должно быть больше 0\n";
                    }
                }

                d_in.isPressureIn = vn_rb.Checked;

                if (d_in.isPressureIn)
                {
                    d_in.met = "cilvn";
                }
                else
                {
                    d_in.met = "cilnar";
                    //E
                    {
                        double E;
                        if (!E_tb.ReadOnly)
                        {
                            try
                            {
                                E = Convert.ToDouble(E_tb.Text.Replace(',', '.'),
                                    System.Globalization.CultureInfo.InvariantCulture);
                            }
                            catch (FormatException)
                            {
                                E = 0;
                                data_inerr += "E неверный ввод\n";
                            }
                        }
                        else
                        {
                            E = CalcClass.GetE(d_in.steel, d_in.temp);
                            E_tb.ReadOnly = false;
                            E_tb.Text = E.ToString();
                            E_tb.ReadOnly = true;
                        }
                        if (E > 0)
                        {
                            d_in.E = E;
                        }
                        else
                        {
                            data_inerr += "E должно быть больше 0\n";
                        }
                    }

                    //l
                    {
                        double l;
                        try
                        {
                            l = Convert.ToDouble(l_tb.Text.Replace(',', '.'),
                                System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch (FormatException)
                        {
                            l = 0;
                            data_inerr += "l неверный ввод\n";
                        }
                        if (l > 0)
                        {
                            d_in.l = l;
                        }
                        else
                        {
                            data_inerr += "l должно быть больше 0\n";
                        }
                    }
                }

                //p
                {
                    double p;
                    try
                    {
                        p = Convert.ToDouble(p_tb.Text.Replace(',', '.'),
                                            System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch (FormatException)
                    {
                        p = 0;
                        data_inerr += "p неверный ввод\n";
                    }
                    const int NO_PRESSURE = 0,
                                MAX_PRESSURE = 200;
                    if (p > NO_PRESSURE && p < MAX_PRESSURE)
                    {
                        d_in.isNeedpCalculate = true;
                        d_in.p = p;
                    }
                    else
                    {
                        data_inerr += "p должно быть в диапазоне 0 - 200\n";
                    }
                }

                //fi
                {
                    double fi;
                    try
                    {
                        fi = Convert.ToDouble(fi_tb.Text.Replace(',', '.'),
                                            System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch (FormatException)
                    {
                        fi = 0;
                        data_inerr += "fi неверный ввод\n";
                    }
                    const int MIN_FI = 0,
                            MAX_FI = 1;
                    if (fi > MIN_FI && fi <= MAX_FI)
                    {
                        d_in.fi = fi;
                    }
                    else
                    {
                        data_inerr += "fi должен быть в диапазоне 0 - 1\n";
                    }
                }

                //D
                {
                    double D;
                    try
                    {
                        D = Convert.ToDouble(D_tb.Text.Replace(',', '.'),
                                            System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch (FormatException)
                    {
                        D = 0;
                        data_inerr += "D неверный ввод\n";
                    }
                    if (D > 0)
                    {
                        d_in.D = D;
                    }
                    else
                    {
                        data_inerr += "D должно быть больше 0\n";
                    }
                }

                //c1
                {
                    double c1;
                    try
                    {
                        c1 = Convert.ToDouble(c1_tb.Text.Replace(',', '.'),
                                            System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch (FormatException)
                    {
                        c1 = -1;
                        data_inerr += "c1 неверный ввод\n";
                    }
                    if (c1 >= 0)
                    {
                        d_in.c1 = c1;
                    }
                    else
                    {
                        data_inerr += "c1 должно быть больше 0\n";
                    }
                }

                //c2
                {
                    double c2;
                    try
                    {
                        c2 = Convert.ToDouble(c2_tb.Text.Replace(',', '.'),
                                            System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch (FormatException)
                    {
                        c2 = -1;
                        data_inerr += "c2 неверный ввод\n";
                    }
                    if (c2 >= 0)
                    {
                        d_in.c2 = c2;
                    }
                    else
                    {
                        data_inerr += "c2 должно быть больше 0\n";
                    }
                }

                //c3
                {
                    if (c3_tb.Text == "")
                    {
                        d_in.c3 = 0;
                    }
                    else
                    {
                        double c3;
                        try
                        {
                            c3 = Convert.ToDouble(c3_tb.Text,
                                                System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch (FormatException)
                        {
                            c3 = -1;
                            data_inerr += "c3 неверный ввод\n";
                        }
                        if (c3 >= 0)
                        {
                            d_in.c3 = c3;
                        }
                        else
                        {
                            data_inerr += "должно быть >= 0\n";
                        }
                    }
                }

                //s
                {
                    double s;
                    try
                    {
                        s = Convert.ToDouble(s_tb.Text.Replace(',', '.'),
                                            System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch (FormatException)
                    {
                        s = 0;
                        data_inerr += "s неверный ввод\n";
                    }
                    if (s > 0)
                    {
                        d_in.s = s;
                    }
                    else
                    {
                        data_inerr += "s должно быть больше 0\n";
                    }
                }

                isNotError = data_inerr == "";
                if (isNotError) // если данные введены правильно
                {
                    Data_out d_out = CalcClass.CalculateCilindricalShell(in d_in);
                    if (!d_out.isCriticalError) // если нет ошибок расчета
                    {
                        p_d_l.Text = $"[p]={d_out.p_d:f2} МПа";
                        scalc_l.Text = $"sp={d_out.s_calc:f3} мм";

                        if (this.Owner is MainForm main)
                        {
                            main.Word_lv.Items.Add($"{d_in.D} мм, {d_in.p} МПа, {d_in.temp} C, {d_in.met}");
                            int listItemsCount;
                            listItemsCount = main.Word_lv.Items.Count;
                            //DataWordOut.DataArr[0].  DataArr .DataOutArr[]. .Value = $"{d_in.D} мм, {d_in.p} МПа, {d_in.temp} C, {d_in.met}";
                            dataArrEl.id = listItemsCount;
                            dataArrEl.Data_In = d_in;
                            dataArrEl.Data_Out = d_out;

                            dataArrEl.Typ = TypeElement;


                            //Df.Data_In = d_in;
                            //Df.Data_Out = d_out;
                            //Df.Typ = "cil";

                            DataWordOut.DataArr.Add(DataArrEl);

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
                    System.Windows.Forms.MessageBox.Show(data_inerr);
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
            E_tb.Text = CalcClass.GetE(steel_cb.Text, Convert.ToInt32(t_tb.Text)).ToString();
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
