using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace calcNet
{
    class InputClass
    {
        internal static void TrySetValue(in string name, in double val, ref ShellDataIn d_in, ref bool isNotError)
        {
            try
            {
                d_in.SetValue(name, val);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                isNotError = false;
                MessageBox.Show(ex.Message);
            }
        }
        internal static void GetInput_t(TextBox t_tb, ref ShellDataIn d_in)
        {
            if (double.TryParse(t_tb.Text, System.Globalization.NumberStyles.Integer,
                    System.Globalization.CultureInfo.InvariantCulture, out double t_in))
            {
                try
                {
                    d_in.t = t_in;
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                d_in.IsInError = true;
                MessageBox.Show("T неверный ввод\n");
                //dataInErr += "T неверный ввод\n";
            }
        }

        internal static void GetInput_t(TextBox t_tb, ref DataNozzle_in dN_in, ref string dataInErr)
        {
            if (int.TryParse(t_tb.Text, out int t))
            {
                const int MIN_TEMPERATURE = 20,
                        MAX_TEMPERATURE = 1000;
                if (t >= MIN_TEMPERATURE && t < MAX_TEMPERATURE)
                {
                    dN_in.temp = t;
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

        internal static void GetInput_sigma_d(TextBox sigma_d_tb, ref Data_in d_in, ref string dataInErr)
        {
            double sigma_d = 0;
            if (!sigma_d_tb.ReadOnly)
            {
                if (double.TryParse(sigma_d_tb.Text.Replace(',', '.'), System.Globalization.NumberStyles.AllowDecimalPoint,
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
                //if (CalcClass.GetSigma(d_in.Steel, d_in.temp, ref sigma_d, ref dataInErr))
                //{
                //    d_in.sigma_d = sigma_d;
                //    //sigma_d = CalcClass.GetSigma(d_in.steel, d_in.temp);
                //    sigma_d_tb.ReadOnly = false;
                //    sigma_d_tb.Text = sigma_d.ToString();
                //    sigma_d_tb.ReadOnly = true;
                //}
            }
        }

        internal static void GetInput_E(TextBox E_tb, ref Data_in d_in, ref string dataInErr)
        {
            double E = 0;
            if (!E_tb.ReadOnly)
            {
                if (double.TryParse(E_tb.Text.Replace(',', '.'), System.Globalization.NumberStyles.AllowDecimalPoint,
                    System.Globalization.CultureInfo.InvariantCulture, out E))
                {
                    if (E > 0)
                    {
                        d_in.E = E;
                    }
                    else
                    {
                        dataInErr += "E должно быть больше 0\n";
                    }
                }
                else
                {
                    dataInErr += "E неверный ввод\n";
                }
            }
            else
            {
                if (CalcClass.GetE(d_in.Steel, d_in.temp, ref E, ref dataInErr))
                {
                    d_in.E = E;
                    E_tb.ReadOnly = false;
                    E_tb.Text = E.ToString();
                    E_tb.ReadOnly = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="E_tb"></param>
        /// <param name="dN_in"></param>
        /// <param name="dataInErr"></param>
        /// <param name="i"></param>
        internal static void GetInput_E(TextBox E_tb, ref DataNozzle_in dN_in, ref string dataInErr, int i)
        {
            double E = 0;
            if (!E_tb.ReadOnly)
            {
                if (double.TryParse(E_tb.Text.Replace(',', '.'), System.Globalization.NumberStyles.AllowDecimalPoint,
                    System.Globalization.CultureInfo.InvariantCulture, out E))
                {
                    if (E > 0)
                    {
                        dN_in.SetValue("E" + i.ToString(), E);
                    }
                    else
                    {
                        dataInErr += "E должно быть больше 0\n";
                    }
                }
                else
                {
                    dataInErr += "E неверный ввод\n";
                }
            }
            else
            {
                string steel = "";
                dN_in.GetValue("steel" + i.ToString(), ref steel);
                if (CalcClass.GetE(steel, dN_in.temp, ref E, ref dataInErr))
                {
                    dN_in.SetValue("E" + i.ToString(), E);
                    E_tb.ReadOnly = false;
                    E_tb.Text = E.ToString();
                    E_tb.ReadOnly = true;
                }
            }
        }


        internal static void GetInput_l(TextBox l_tb, ref Data_in d_in, ref string dataInErr)
        {
            if (double.TryParse(l_tb.Text.Replace(',', '.'), System.Globalization.NumberStyles.AllowDecimalPoint,
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

        internal static void GetInput_p(TextBox p_tb, ref Data_in d_in, ref string dataInErr)
        {
            if (double.TryParse(p_tb.Text.Replace(',', '.'), System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.InvariantCulture, out double p))
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

        internal static void GetInput_fi(TextBox fi_tb, ref Data_in d_in, ref string dataInErr)
        {
            if (double.TryParse(fi_tb.Text.Replace(',', '.'), System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.CultureInfo.InvariantCulture, out double fi))

            {
                const int MIN_FI = 0,
               MAX_FI = 1;
                if (fi > MIN_FI && fi <= MAX_FI)
                {
                    d_in.fi = fi;
                }
                else
                {
                    dataInErr += $"fi должен быть в диапазоне {MIN_FI} - {MAX_FI}\n";
                }
            }
            else
            {
                dataInErr += "fi неверный ввод\n";
            }

        }

        internal static void GetInput_D(TextBox D_tb, ref Data_in d_in, ref string dataInErr)
        {
            if (double.TryParse(D_tb.Text.Replace(',', '.'), System.Globalization.NumberStyles.AllowDecimalPoint,
                    System.Globalization.CultureInfo.InvariantCulture, out double D))
            {
                if (D > 0)
                {
                    d_in.D = D;
                }
                else
                {
                    dataInErr += "D должно быть больше 0\n";
                }
            }
            else
            {
                dataInErr += "D неверный ввод\n";
            }
        }

        internal static void GetInput_c1(TextBox c1_tb, ref Data_in d_in, ref string dataInErr)
        {
            if (double.TryParse(c1_tb.Text.Replace(',', '.'), System.Globalization.NumberStyles.AllowDecimalPoint,
                    System.Globalization.CultureInfo.InvariantCulture, out double c1))
            {
                if (c1 >= 0)
                {
                    d_in.c1 = c1;
                }
                else
                {
                    dataInErr += "c1 должно быть больше либо равно 0\n";
                }
            }
            else
            {
                dataInErr += "c1 неверный ввод\n";
            }
        }

        internal static void GetInput_c2(TextBox c2_tb, ref Data_in d_in, ref string dataInErr)
        {
            if (double.TryParse(c2_tb.Text.Replace(',', '.'), System.Globalization.NumberStyles.AllowDecimalPoint,
                    System.Globalization.CultureInfo.InvariantCulture, out double c2))
            {
                if (c2 >= 0)
                {
                    d_in.c2 = c2;
                }
                else
                {
                    dataInErr += "c2 должно быть больше либо равно 0\n";
                }
            }
            else
            {
                dataInErr += "c2 неверный ввод\n";
            }
        }

        internal static void GetInput_c3(TextBox c3_tb, ref Data_in d_in, ref string dataInErr)
        {
            if (c3_tb.Text == "")
            {
                d_in.c3 = 0;
            }
            else
            {
                if (double.TryParse(c3_tb.Text.Replace(',', '.'), System.Globalization.NumberStyles.AllowDecimalPoint,
                    System.Globalization.CultureInfo.InvariantCulture, out double c3))
                {
                    if (c3 >= 0)
                    {
                        d_in.c3 = c3;
                    }
                    else
                    {
                        dataInErr += "c3 должно быть больше либо равно 0\n";
                    }
                }
                else
                {
                    dataInErr += "c3 неверный ввод\n";
                }
            }
        }

        internal static void GetInput_s(TextBox s_tb, ref Data_in d_in, ref string dataInErr)
        {
            if (double.TryParse(s_tb.Text.Replace(',', '.'), System.Globalization.NumberStyles.AllowDecimalPoint,
                    System.Globalization.CultureInfo.InvariantCulture, out double s))
            {
                if (s > 0)
                {
                    d_in.s = s;
                }
                else
                {
                    dataInErr += "s должно быть больше 0\n";
                }
            }
            else
            {
                dataInErr += "s неверный ввод\n";
            }
        }
    }
}
