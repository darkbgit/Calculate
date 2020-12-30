using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace calcNet
{
    class Data_in
    {
        internal string name;
        internal string steel;
        internal double p;
        internal int temp;
        internal double sigma_d;
        internal int D;
        internal double c1;
        internal double c2;
        internal double c3 = 0;
        internal double fi;
        internal double s;
        internal int dav; //0 - vn, 1 - nar
        internal double l;
        internal double l3_1;
        internal double l3_2;
        internal int E;
        internal double ny = 2.4;
        internal string met; // "cilvn", "cilnar", "konvn", "konnar", "ellvn", "ellnar"
        internal int elH;
        internal int elh1;
        internal int alfa;
        internal bool yk;
    }

    class Data_out
    {
        internal double s_calcr;
        internal double s_calc;
        internal double s_calcr1;
        internal double s_calcr2;
        internal double p_d;
        internal double c;
        internal double l;
        internal double b;
        internal double b_2;
        internal double b1;
        internal double b1_2;
        internal double p_dp;
        internal double p_de;
        internal double elR;
        internal double elke;
        internal double elx;
        internal string err = "";
        internal bool ypf;
        internal double Dk;
    }

    class DataNozzle_in
    {
        internal int place; //1(cil, kon, ell)-Ось штуцера совпадает с нормалью к поверхности в центре отверстия
                            //2(cil, kon) - наклонный штуцер ось которого лежит в плоскости поперечного сечения
                            //3(ell) - смещенный штуцер ось которого паралелльна оси днища
                            //4(cil, kon) - наклонный штуцер максимальная ось симметрии отверстия некруглой формы составляет угол с образующей обечайки на плоскость продольного сечения обечайки
                            //5(cil, kon, sfer, torosfer) - наклонный штуцер ось которого лежит в плоскости продольного сечения
                            //6(oval) - овальное отверстие штуцер перпендикулярно расположен к поверхности обечайки
                            //7(otbort, torob) - перпендикулярно расположенного к поверхности обечайки или днища штуцера с круглым поперечным сечением при наличии отбортовки или торообразной вставки
        internal string name;
        internal string steel1;
        internal string steel2;
        internal string steel3;
        internal string steel4;
        internal double sigma_d1;
        internal double sigma_d2;
        internal double sigma_d3;
        internal double sigma_d4;
        internal int E1;
        internal int E2;
        internal int E3;
        internal int E4;
        internal int D;
        internal double s0;
        internal double s1;
        internal double s2;
        internal double s3;
        internal double cs;
        internal double cs1;
        internal int l;
        internal int l1;
        internal int l2;
        internal int l3;
        internal double fi;
        internal double fi1;
        internal int delta;
        internal int delta1;
        internal int delta2;
        internal int elx;
        internal double b;
        internal int vid; //1 - 8
        //internal string dav; //0 - vn, 1-nar
        internal string met;
        internal double ny = 2.4;
        internal double t;
        internal double gamma;
        internal double omega;
    }

    class DataNozzle_out
    {
        internal double p_d;
        internal double p_dp;
        internal double p_de;
        internal double Dp;
        internal double dp;
        internal double d0p;
        internal double s1p;
        internal double d0;
        internal double d01;
        internal double d02;
        internal double dmax;
        internal double sp;
        internal double spn;
        internal double ppn;
        internal int K1;
        internal double lp;
        internal double l1p;
        internal double l1p2;
        internal double l2p;
        internal double l2p2;
        internal double l3p;
        internal double l3p2;
        internal double L0;
        internal double psi1;
        internal double psi2;
        internal double psi3;
        internal double psi4 = 1;
        internal double b;
        internal double V;
        internal double V1;
        internal double V2;
        internal double yslyk1;
        internal double yslyk2;
        internal double B1n;
        internal double pen;
        internal string err;
        internal bool ypf;
        internal double ypf1;
        internal double ypf2;
    }

    class DataSaddle_in
    {
        internal int G;
        internal int L;
        internal int Lob;
        internal int H;
        internal double e;
        internal double a;
        internal double p;
        internal double D;
        internal double s;
        internal double s2;
        internal double c;
        internal double fi;
        internal double sigma_d;
        internal int E;
        internal double ny = 2.4;
        internal int type;
        internal double b;
        internal double b2;
        internal int delta1;
        internal int delta2;
        internal string name;
        internal string nameob;
        internal int temp;
        internal int l0;
        internal string steel;
        internal int dav; //0 - vn, 1 - nar
    }

    class DataSaddle_out
    {
        internal double q;
        internal double M0;
        internal double p_d;
        internal double F1;
        internal double F2;
        internal double F_d;
        internal double M1;
        internal double M2;
        internal double M12;
        internal double M_d;
        internal double Q1;
        internal double Q2;
        internal double Q_d;
        internal double B1;
        internal double B1_2;
        internal double yslproch1_1;
        internal double yslproch1_2;
        internal double yslproch2;
        internal double yslystoich1;
        internal double yslystoich2;
        internal double K9;
        internal double K9_1;
        internal double y;
        internal double x;
        internal double gamma;
        internal double beta1;
        internal double K10;
        internal double K10_1;
        internal double K11;
        internal double K12;
        internal double K13;
        internal double K14;
        internal double K15;
        internal double K15_2;
        internal double K16;
        internal double K17;
        internal double sigma_mx;
        internal double F_d2;
        internal double F_d3;
        internal double v1_2;
        internal double v1_3;
        internal double v21_2;
        internal double v21_3 = 0;
        internal double v22_2;
        internal double v22_3;
        internal double K2;
        internal double K1_2;
        internal double K1_21;
        internal double K1_22;
        internal double K1_3;
        internal double K1_31;
        internal double K1_32;
        internal double sigmai2;
        internal double sigmai2_1;
        internal double sigmai2_2;
        internal double sigmai3;
        internal double sigmai3_1;
        internal double sigmai3_2;
        internal double Fe;
        internal double sef;
    }

    class DataHeat_in
    {

    }

    class DataHeat_out
    {

    }

    class CalcClass
    {
        internal static double GetSigma (string steel, int temp)
        {
            double sigma;
            double sigma_l = 0;
            double sigma_b = 0;
            int temp_l = 0;
            int temp_b = 0;
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @"data\data.xml"));
            var root = doc.DocumentElement;
            XmlNode steel1 = root.SelectSingleNode("sigma_list").SelectSingleNode("steels").SelectSingleNode("//steel[@name='" + steel +"']");
            foreach (XmlNode temps in steel1.ChildNodes)
            {
                if (Convert.ToInt32(temps.Attributes["temp"].Value) == temp)
                {
                    sigma = Convert.ToDouble(temps.Attributes["sigma"].Value);
                    return sigma;
                }
                else if (Convert.ToInt32(temps.Attributes["temp"].Value) > temp)
                {
                    temp_l = Convert.ToInt32(temps.Attributes["temp"].Value);
                    sigma_b = Convert.ToDouble(temps.Attributes["sigma"].Value);
                    break;
                }
                else
                {
                    temp_b = Convert.ToInt32(temps.Attributes["temp"].Value);
                    sigma_l = Convert.ToDouble(temps.Attributes["sigma"].Value);
                }
            }
            sigma = sigma_b - ((sigma_b - sigma_l) * (temp - temp_l) / (temp_b - temp_l));
            sigma *= 10;
            sigma = Math.Truncate(sigma / 5);
            sigma *= 0.5;
            
            return sigma;
        }

        internal static int GetE(string steel, int temp)
        {
            int E;
            int E_l = 0;
            int E_b = 0;
            int temp_l = 0;
            int temp_b = 0;
            string steelf;

           
            Regex regex = new Regex(@"(.*)(?=\()");
            MatchCollection matches = regex.Matches(steel);
            if (matches.Count > 0)
            {
                steelf = matches[0].Groups[0].Value; 
            }
            else
            {
                steelf = steel;
            }
            
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @"data\data.xml"));
            var root = doc.DocumentElement;
            XmlNode cl = root.SelectSingleNode("//n[@name='" + steelf +"']");
            var clsteel = cl.ParentNode.Name;
            XmlNode Elist = root.SelectSingleNode("E_list").SelectSingleNode("//class[@name='" + clsteel + "']");

            foreach (XmlNode temps in Elist.ChildNodes)
            {
                if (Convert.ToInt32(temps.Attributes["temp"].Value) == temp)
                {
                    E = Convert.ToInt32(temps.Attributes["E"].Value);
                    return E;
                }
                else if (Convert.ToInt32(temps.Attributes["temp"].Value) > temp)
                {
                    temp_l = Convert.ToInt32(temps.Attributes["temp"].Value);
                    E_b = Convert.ToInt32(temps.Attributes["E"].Value);
                    break;
                }
                else
                {
                    temp_b = Convert.ToInt32(temps.Attributes["temp"].Value);
                    E_l = Convert.ToInt32(temps.Attributes["E"].Value);
                }
            }
            E = Convert.ToInt32(E_b - ((E_b - E_l) * (temp - temp_l) / (temp_b - temp_l)));
            //sigma *= 10;
            //sigma = Math.Truncate(sigma / 5);
            //sigma *= 0.5;

            return E;
        }

        internal static Data_out CalcCil(Data_in d_in)
        {
            Data_out d_out = new Data_out { err = "" };

            d_out.c = d_in.c1 + d_in.c2 + d_in.c3;
            if ((d_in.D < 200) && ((d_in.s - d_out.c) / d_in.D <= 0.3))
            {
                d_out.ypf = true;
            }
            else if ((d_in.D >= 200) && ((d_in.s - d_out.c) / d_in.D <= 0.3))
            {
                d_out.ypf = true;
            }
            else
            {
                d_out.ypf = false;
                d_out.err += "Условие применения формул не выполняется\n";
            }

            if (d_in.dav == 0)
            {
                d_out.s_calcr = d_in.p * d_in.D / ((2 * d_in.sigma_d * d_in.fi) - d_in.p);
                d_out.s_calc = d_out.s_calcr + d_out.c;
                if (d_in.s == 0.0)
                {
                    d_out.p_d = 2 * d_in.sigma_d * d_in.fi * (d_out.s_calc - d_out.c) / (d_in.D + d_out.s_calc - d_out.c);
                }
                else if (d_in.s >= d_out.s_calc)
                {
                    d_out.p_d = 2 * d_in.sigma_d * d_in.fi * (d_in.s - d_out.c) / (d_in.D + d_in.s - d_out.c);
                }
                else
                {
                    d_out.err += "Принятая толщина меньше расчетной\nрасчет не выполнен";
                }
            }
            else if (d_in.dav == 1)
            {
                d_out.l = d_in.l + d_in.l3_1 + d_in.l3_2;
                d_out.b_2 = 0.47 * Math.Pow(d_in.p / (0.00001 * d_in.E), 0.067) * Math.Pow(d_out.l / d_in.D, 0.4);
                d_out.b = Math.Max(1.0, d_out.b_2);
                d_out.s_calcr1 = 1.06 * (0.01 * d_in.D / d_out.b) * Math.Pow(d_in.p / (0.00001 * d_in.E) * (d_out.l / d_in.D), 0.4);
                d_out.s_calcr2 = 1.2 * d_in.p * d_in.D / (2 * d_in.sigma_d - d_in.p);
                d_out.s_calcr = Math.Max(d_out.s_calcr1, d_out.s_calcr2);
                d_out.s_calc = d_out.s_calcr + d_out.c;
                if (d_in.s == 0.0)
                {
                    d_out.p_dp = 2 * d_in.sigma_d * (d_out.s_calc - d_out.c) / (d_in.D + d_out.s_calc - d_out.c);
                    d_out.b1_2 = 9.45 * (d_in.D / d_out.l) * Math.Sqrt(d_in.D / (100 * (d_out.s_calc - d_out.c)));
                    d_out.b1 = Math.Min(1.0, d_out.b1_2);
                    d_out.p_de = ((2.08 * 0.00001 * d_in.E) / d_in.ny * d_out.b1) * (d_in.D / d_out.l) * Math.Pow(100 * (d_out.s_calc - d_out.c) / d_in.D, 2.5);
                }
                else if (d_in.s >= d_out.s_calc)
                {
                    d_out.p_dp = 2 * d_in.sigma_d * (d_in.s - d_out.c) / (d_in.D + d_in.s - d_out.c);
                    d_out.b1_2 = 9.45 * (d_in.D / d_out.l) * Math.Sqrt(d_in.D / (100 * (d_in.s - d_out.c)));
                    d_out.b1 = Math.Min(1.0, d_out.b1_2);
                    d_out.p_de = 2.08 * 0.00001 * d_in.E / d_in.ny * d_out.b1 * (d_in.D / d_out.l) * Math.Pow(100 * (d_in.s - d_out.c) / d_in.D, 2.5);
                }
                else
                {
                    d_out.err += "Принятая толщина меньше расчетной\nрасчет не выполнен";
                }
                d_out.p_d = d_out.p_dp / Math.Sqrt(1 + Math.Pow(d_out.p_dp / d_out.p_de, 2));
            }
            else
            {
                d_out.err += "Неверный тип давления\nрасчет не выполнен";
            }
            return d_out;
        }

        internal static Data_out CalcEll(Data_in d_in)
        {
            Data_out d_out = new Data_out { err = "" };
            d_out.c = d_in.c1 + d_in.c2 + d_in.c3;

            if ((((d_in.s - d_out.c) / d_in.D <= 0.1) & ((d_in.s - d_out.c) / d_in.D >= 0.002) & (d_in.elH / d_in.D < 0.5) & (d_in.elH / d_in.D >= 0.2)) | d_in.s == 0)
            {
                d_out.ypf = true;
            }
            else
            {
                d_out.ypf = false;
                d_out.err += "Условие применения формул не выполняется\n";
            }
            d_out.elR = Math.Pow(d_in.D, 2) / (4 * d_in.elH);
            if (d_in.dav == 0)
            {
                d_out.s_calcr = d_in.p * d_out.elR / ((2 * d_in.sigma_d * d_in.fi) - 0.5 * d_in.p);
                d_out.s_calc = d_out.s_calcr + d_out.c;

                if (d_in.s == 0.0)
                {
                    d_out.p_d = 2 * d_in.sigma_d * d_in.fi * (d_out.s_calc - d_out.c) / (d_out.elR + 0.5 * (d_out.s_calc - d_out.c));
                }
                else if (d_in.s >= d_out.s_calc)
                {
                    d_out.p_d = 2 * d_in.sigma_d * d_in.fi * (d_in.s - d_out.c) / (d_out.elR + 0.5 * (d_out.s_calc - d_out.c));
                }
                else
                {
                    d_out.err += "Принятая толщина меньше расчетной\n";
                }
            }
            else if (d_in.dav == 1)
            {
                d_out.s_calcr2 = 1.2 * d_in.p * d_out.elR / (2 * d_in.sigma_d);
                d_out.elke = 0.9; // добавить ке для полусферических =1
                d_out.s_calcr1 = (d_out.elke * d_out.elR) / 161 * Math.Sqrt(d_in.ny * d_in.p / (0.00001 * d_in.E));
                d_out.s_calcr = Math.Max(d_out.s_calcr1, d_out.s_calcr2);
                d_out.s_calc = d_out.s_calcr + d_out.c;
                if (d_in.s == 0.0)
                {
                    d_out.elke = 0.9; // # добавить ке для полусферических =1
                    d_out.s_calcr1 = (d_out.elke * d_out.elR) / 161 * Math.Sqrt((d_in.ny * d_in.p) / (0.00001 * d_in.E));
                    d_out.s_calcr = Math.Max(d_out.s_calcr1, d_out.s_calcr2);
                    //#d_out.p_dp = 2*d_in.sigma_d*(d_out.s_calc-d_out.c)/(d_out.elR + 0.5 * (d_out.s_calc-d_out.c))
                    //#d_out.elx = 10 * ((d_in.s-d_out.c)/d_in.D)*(d_in.D/(2*d_in.elH)-(2*d_in.elH)/d_in.D)
                    //d_out.elke = (1 + (2.4 + 8 * d_out.elx)*d_out.elx)/(1+(3.0+10*d_out.elx)*d_out.elx)
                    //#d_out.p_de = (2.6*0.00001*d_in.E)/d_in.ny*Math.Pow(100*(d_out.s-d_out.c)/(d_out.elke*d_out.elR,2))
                    //#d_out.p_d = d_out.p_dp/Math.Sqrt(1+Math.Pow(d_out.p_dp/d_out.p_de,2))
                }
                else if (d_in.s >= d_out.s_calc)
                {
                    d_out.p_dp = 2 * d_in.sigma_d * (d_in.s - d_out.c) / (d_out.elR + 0.5 * (d_in.s - d_out.c));
                    d_out.elx = 10 * ((d_in.s - d_out.c) / d_in.D) * (d_in.D / (2 * d_in.elH) - (2 * d_in.elH) / d_in.D);
                    d_out.elke = (1 + (2.4 + 8 * d_out.elx) * d_out.elx) / (1 + (3.0 + 10 * d_out.elx) * d_out.elx);
                    d_out.p_de = (2.6 * 0.00001 * d_in.E) / d_in.ny * Math.Pow(100 * (d_in.s - d_out.c) / (d_out.elke * d_out.elR), 2);
                    d_out.p_d = d_out.p_dp / Math.Sqrt(1 + Math.Pow(d_out.p_dp / d_out.p_de, 2));
                }
                else
                {
                    d_out.err += "Принятая толщина меньше расчетной\n";
                }
            }
            return d_out;
        }

        internal static DataNozzle_out CalcNozzle(Data_in d_in, Data_out d_out, DataNozzle_in dN_in)
        {
            DataNozzle_out dN_out = new DataNozzle_out { err = "" };

            // расчет Dp, dp
            switch (d_in.met)
            {
                case "cilvn":
                case "cilnar":
                    {
                        dN_out.Dp = d_in.D;
                        break;
                    }
                case "konvn":
                case "konnar":
                    {
                        dN_out.Dp = d_out.Dk/Math.Cos(Math.PI*100*d_in.alfa);
                        break;
                    }
                case "ellvn":
                case "ellnar":
                    {
                        if (d_in.elH * 100 == d_in.D * 25)
                        {
                            dN_out.Dp = d_in.D * 2 * Math.Sqrt(1 - 3 * Math.Pow(dN_in.elx / d_in.D, 2));
                        }
                        else
                        {
                            dN_out.Dp = Math.Pow(d_in.D, 2) / (d_in.elH * 2) * Math.Sqrt(1 - (4 * (Math.Pow(d_in.D, 2) - 4 * Math.Pow(d_in.elH, 2)) * Math.Pow(dN_in.elx, 2)) / Math.Pow(d_in.D, 4));
                        }
                        break;
                    }
                case "sfer":
                case "torosfer":
                    {
                        //dN_out.Dp = 2 * d_in.R;
                        break;
                    }
                default:
                    {
                        dN_out.err += "Ошибка вида укрепляемого элемента\n";
                        break;
                    }
            }

            if (d_in.met == "ellvn")
            {
                dN_out.sp = d_in.p * dN_out.Dp / (4 * d_in.fi * d_in.sigma_d - d_in.p);
            }
            else
            {
                dN_out.sp = d_out.s_calcr;
            }

            dN_out.s1p = d_in.p * (dN_in.D + 2 * dN_in.cs) / (2 * dN_in.fi * dN_in.sigma_d1 - d_in.p);

            switch (dN_in.place)
            {
                case 1:
                    {
                        dN_out.dp = dN_in.D + 2 * dN_in.cs;
                        break;
                    }
                case 2:
                    {
                        dN_out.dp = Math.Max(dN_in.D, 0.5 * dN_in.t);
                        break;
                    }
                case 3:
                    {
                        dN_out.dp = (dN_in.D + 2 * dN_in.cs) / Math.Sqrt(1 + Math.Pow(2 * dN_in.elx / dN_out.Dp, 2));
                        break;
                    }
                case 4:
                    {
                        dN_out.dp = (dN_in.D + 2 * dN_in.cs) * (1 + Math.Pow(Math.Tan(Math.PI * 180 * dN_in.gamma), 2) * Math.Pow(Math.Cos(Math.PI * 180 * dN_in.omega), 2));
                        break;
                    }
                case 5:
                    {
                        dN_out.dp = (dN_in.D + 2 * dN_in.cs) / Math.Pow(Math.Cos(Math.PI * 180 * dN_in.gamma), 2);
                        break;
                    }
                case 6:
                    {
                        //dN_out.dp = (dN_in.D + 2 * dN_in.cs) * (Math.Pow(Math.Sin(Math.PI * 180 * dN_in.omega), 2) + (dN_in.d1 + 2 * dN_in.cs) * (dN_in.d1 + dN_in.d2 + 4 * dN_in.cs) / (2 * Math.Pow(dN_in.d2 + 2 * dN_in.cs, 2)) * Math.Pow(Math.Cos(Math.PI * 180 * dN_in.omega), 2));
                        break;
                    }
                case 7:
                    {
                        //dN_out.dp = dN_in.D + 1.5*(dN_in.r-dN_out.sp) +2 * dN_in.cs;
                        break;
                    }
                default:
                    {
                        dN_out.err += "Ошибка места расположения штуцера\n";
                        break;
                    }
            }

            // l1p, l3p, l2p
            dN_out.l1p2 = 1.25 * Math.Sqrt((dN_in.D + 2 * dN_in.cs) * (dN_in.s1 - dN_in.cs));
            dN_out.l1p = Math.Min(dN_in.l1, dN_out.l1p2);
            if (dN_in.s3 == 0)
            {
                dN_out.l3p = 0;
            }
            else
            {
                dN_out.l3p2 = 0.5 * Math.Sqrt((dN_in.D + 2 * dN_in.cs) * (dN_in.s3 - dN_in.cs - dN_in.cs1));
                dN_out.l3p = Math.Min(dN_in.l3, dN_out.l3p2);
            }

            dN_out.L0 = Math.Sqrt(dN_out.Dp * (d_in.s - d_out.c));

            switch (dN_in.vid)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                    dN_out.lp = dN_out.L0;
                    break;
                case 7:
                case 8:
                    dN_out.lp = Math.Min(dN_in.l, dN_out.L0);
                    break;
            }

            dN_out.l2p2 = Math.Sqrt(dN_out.Dp * (dN_in.s2 + d_in.s - d_out.c));
            dN_out.l2p = Math.Min(dN_in.l2, dN_out.l2p2);

            if ((new[] {1, 2, 3, 4, 5, 6 } ).Contains(dN_in.vid))
            {
                dN_in.s0 = d_in.s;
                dN_in.steel4 = dN_in.steel1;
            }

            dN_in.sigma_d2 = GetSigma(dN_in.steel2, d_in.temp);
            dN_in.sigma_d3 = GetSigma(dN_in.steel3, d_in.temp);
            dN_in.sigma_d4 = GetSigma(dN_in.steel4, d_in.temp);

            dN_out.psi1 = Math.Min(1, dN_in.sigma_d1 / d_in.sigma_d);
            dN_out.psi2 = Math.Min(1, dN_in.sigma_d2 / d_in.sigma_d);
            dN_out.psi3 = Math.Min(1, dN_in.sigma_d3 / d_in.sigma_d);
            dN_out.psi4 = Math.Min(1, dN_in.sigma_d4 / d_in.sigma_d);

            dN_out.d0p = 0.4 * Math.Sqrt(dN_out.Dp * (d_in.s - d_out.c));

            dN_out.b = Math.Sqrt(dN_out.Dp * (d_in.s - d_out.c)) + Math.Sqrt(dN_out.Dp * (d_in.s - d_out.c));

            switch (d_in.met)
            {
                case "cilvn":
                case "cilnar":
                    {
                        dN_out.dmax = d_in.D;
                        break;
                    }
                case "konvn":
                case "konnar":
                    {
                        dN_out.dmax = d_out.Dk;
                        break;
                    }
                case "ellvn":
                case "ellnar":
                case "sfer":
                case "torosfer":
                    {
                        dN_out.dmax = 0.6 * d_in.D;
                        break;
                    }
            }

            switch (d_in.met)
            {
                case "cilvn":
                case "cilnar":
                case "konvn":
                case "konnar":
                    {
                        dN_out.K1 = 1;
                        break;
                    }
                case "ellvn":
                case "ellnar":
                case "sfer":
                case "torosfer":
                    {
                        dN_out.K1 = 2;
                        break;
                    }
            }

            if (d_in.dav == 0)
            {
                dN_out.spn = dN_out.sp;
            }
            else if (d_in.dav == 1)
            {
                
                //dN_out.B1n = Math.Min(1, 9.45 * (d_in.D / d_out.l) * Math.Sqrt(d_in.D / (100 * (d_in.s - d_out.c))));
                //dN_out.pen = 2.08 * 0.00001 * d_in.E / (dN_in.ny * dN_out.B1n) * (d_in.D / d_out.l) * Math.Pow(100 * (d_in.s - d_out.c) / d_in.D, 2.5);
                dN_out.pen = d_out.p_de;
                dN_out.ppn = d_in.p / Math.Sqrt(1 - Math.Pow(d_in.p / dN_out.pen, 2));
                dN_out.spn = dN_out.ppn * dN_out.Dp / (2 * dN_out.K1 * d_in.sigma_d - dN_out.ppn);
            }


            dN_out.d01 = 2 * ((d_in.s - d_out.c) / dN_out.spn - 0.8) * Math.Sqrt(dN_out.Dp * (d_in.s - d_out.c));
            dN_out.d02 = dN_out.dmax + 2 * dN_in.cs;
            dN_out.d0 = Math.Min(dN_out.d01, dN_out.d02);

            if (dN_out.dp > dN_out.d0)
            {
                dN_out.yslyk1 = dN_out.l1p * (dN_in.s1 - dN_out.s1p - dN_in.cs) * dN_out.psi1 + dN_out.l2p * dN_in.s2 * dN_out.psi2 + dN_out.l3p * (dN_in.s3 - dN_in.cs - dN_in.cs1) * dN_out.psi3 + dN_out.lp * (d_in.s - d_out.s_calcr - d_out.c) * dN_out.psi4;
                dN_out.yslyk2 = 0.5 * (dN_out.dp - dN_out.d0p) * d_out.s_calcr;
                if (dN_out.yslyk1 < dN_out.yslyk2)
                {
                    dN_out.err += "Условие укрепления одиночного отверстия не выполняется";
                }
            }



            dN_out.V1 = (dN_in.s0 - d_out.c) / (d_in.s - d_out.c);
            dN_out.V2 = (dN_out.psi4 + (dN_out.l1p * (dN_in.s1 - dN_in.cs) * dN_out.psi1 + dN_out.l2p * dN_in.s2 * dN_out.psi2 + dN_out.l3p * (dN_in.s3 - dN_in.cs - dN_in.cs1) * dN_out.psi3) / dN_out.lp * (d_in.s - d_out.c)) / (1 + 0.5 * (dN_out.dp - dN_out.d0p) / dN_out.lp + dN_out.K1 * (dN_in.D + 2 * dN_in.cs) / dN_out.Dp * (dN_in.fi / dN_in.fi1) * (dN_out.l1p / dN_out.lp));
            dN_out.V = Math.Min(dN_out.V1, dN_out.V2);

            if (d_in.dav == 0)
            {
                dN_out.p_d = 2 * dN_out.K1 * dN_in.fi * d_in.sigma_d * (d_in.s - d_out.c) * dN_out.V / (dN_out.Dp + (d_in.s - d_out.c) * dN_out.V);
            }
            else if (d_in.dav == 1)
            {
                dN_out.p_dp = 2 * dN_out.K1 * dN_in.fi * d_in.sigma_d * (d_in.s - d_out.c) * dN_out.V / (dN_out.Dp + (d_in.s - d_out.c) * dN_out.V);
                dN_out.p_de = d_out.p_de; 
                dN_out.p_d = dN_out.p_dp / Math.Sqrt(1 + Math.Pow(dN_out.p_dp / dN_out.p_de, 2));
            }
            if (dN_out.p_d < d_in.p)
            {
                dN_out.err += "Допускаемое давление меньше расчетного\n";
            }

            switch (d_in.met)
            {
                case "cilvn":
                case "cilnar":
                    {
                        dN_out.ypf1 = (dN_out.dp - 2 * dN_in.cs) / d_in.D;
                        dN_out.ypf2 = (d_in.s - d_out.c) / d_in.D;
                        if (dN_out.ypf1 <= 1 & dN_out.ypf2 <= 0.1)
                        {
                            dN_out.ypf = true;
                        }
                        else
                        {
                            dN_out.ypf = false;
                        }
                        break;
                    }
                case "konvn":
                case "konnar":
                    {
                        dN_out.ypf1 = (dN_out.dp - 2 * dN_in.cs) / d_out.Dk;
                        dN_out.ypf2 = (d_in.s - d_out.c) / d_out.Dk;
                        if (dN_out.ypf1 <= 1 & dN_out.ypf2 <= 0.1/Math.Cos(Math.PI*180*d_in.alfa))
                        {
                            dN_out.ypf = true;
                        }
                        else
                        {
                            dN_out.ypf = false;
                        }
                        break;
                    }
                case "ellvn":
                case "ellnar":
                case "sfer":
                case "torosfer":
                    {
                        dN_out.ypf1 = (dN_out.dp - 2 * dN_in.cs) / d_in.D;
                        dN_out.ypf2 = (d_in.s - d_out.c) / d_in.D;
                        if (dN_out.ypf1 <= 0.6 & dN_out.ypf2 <= 0.1)
                        {
                            dN_out.ypf = true;
                        }
                        else
                        {
                            dN_out.ypf = false;
                        }
                        break;
                    }
            }
            if (dN_out.ypf == false)
            {
                dN_out.err += "Условие применения формул не выполняется";
            }

            return (dN_out);
        }

        internal static DataSaddle_out CalcSaddle(DataSaddle_in d_in)
        {
            d_in.sigma_d = GetSigma(d_in.steel, d_in.temp);
            d_in.E = GetE(d_in.steel, d_in.temp);
            DataSaddle_out d_out = new DataSaddle_out();

            d_out.M_d = (0.0000089 * d_in.E) / d_in.ny * Math.Pow(d_in.D, 3) * Math.Pow((100 * (d_in.s - d_in.c)) / d_in.D, 2.5);
            //проверить формулу для расчета [F]
            d_out.F_d = (0.0000031 * d_in.E) / d_in.ny * Math.Pow(d_in.D, 2) * Math.Pow((100 * (d_in.s - d_in.c)) / d_in.D, 2.5);
            d_out.Q_d = (2.4 * d_in.E * Math.Pow(d_in.s - d_in.c, 2)) / d_in.ny * (0.18 + 3.3 * d_in.D * (d_in.s - d_in.c)) / Math.Pow(d_in.L, 2);
            d_out.B1_2 = 9.45 * (d_in.D / d_in.L) * Math.Sqrt(d_in.D / (100 * (d_in.s - d_in.c)));
            d_out.B1 = Math.Min(1, d_out.B1_2);
            d_out.p_d = (0.00000208 * d_in.E) / (d_in.ny * d_out.B1) * (d_in.D / d_in.L) * Math.Pow(100 * (d_in.s - d_in.c) / d_in.D, 2.5);


            d_out.q = d_in.G / (d_in.L + (4 / 3) * d_in.H);
            d_out.M0 = d_out.q * (Math.Pow(d_in.D, 2) / 16);
            d_out.F1 = d_in.G / 2;
            d_out.F2 = d_out.F1;
            d_out.M1 = d_out.q * Math.Pow(d_in.e, 2) / 2 - d_out.M0;
            d_out.M2 = d_out.M1;
            d_out.M12 = d_out.M0 + d_out.F1 * (d_in.L / 2 - d_in.a) - (d_out.q / 2) * Math.Pow(d_in.L / 2 + (2 / 3) * d_in.H, 2);
            d_out.Q1 = (d_in.L - 2 * d_in.a) * d_out.F1 / (d_in.L + (4 / 3) * d_in.H);
            d_out.Q2 = d_out.Q1;
            if (d_out.M12 > d_out.M1)
            {
                d_out.y = d_in.D / (d_in.s - d_in.c);
                d_out.x = d_in.L / d_in.D;
                d_out.K9_1 = 1.6 - 0.20924 * (d_out.x - 1) + 0.028702 * d_out.x * (d_out.x - 1) + 0.0004795 * d_out.y * (d_out.x - 1) - 0.0000002391 * d_out.x * d_out.y * (d_out.x - 1) - 0.0029936 * (d_out.x - 1) * Math.Pow(d_out.x, 2) - 0.00000085692 * (d_out.x - 1) * Math.Pow(d_out.y, 2) + 0.00000088174 * Math.Pow(d_out.x, 2) * (d_out.x - 1) * d_out.y - 0.0000000075955 * Math.Pow(d_out.y, 2) * (d_out.x - 1) * d_out.x + 0.000082748 * (d_out.x - 1) * Math.Pow(d_out.x, 3) + 0.00000000048168 * (d_out.x - 1) * Math.Pow(d_out.y, 3);
                d_out.K9 = Math.Max(d_out.K9_1, 1);
                d_out.yslproch1_1 = d_in.p * d_in.D / (4 * (d_in.s - d_in.c)) + 4 * d_out.M12 * d_out.K9 / (Math.PI * Math.Pow(d_in.D, 2) * (d_in.s - d_in.c));
                d_out.yslproch1_2 = d_in.sigma_d * d_in.fi;
                if (d_in.dav == 0)
                {
                    d_out.yslystoich1 = d_out.M12 / d_out.M_d;
                }
                else if (d_in.dav == 1)
                {
                    d_out.yslystoich1 = d_in.p / d_out.p_d + d_out.M12 / d_out.M_d;
                }
            }
            switch (d_in.type)
            {
                case 1:
                    {
                        d_out.gamma = 2.83 * (d_in.a / d_in.D) * Math.Sqrt((d_in.s - d_in.c) / d_in.D);
                        d_out.beta1 = 0.91 * d_in.b / Math.Sqrt(d_in.D * (d_in.s - d_in.c));
                        d_out.K10_1 = Math.Exp(-d_out.beta1) * Math.Sin(d_out.beta1) / d_out.beta1;
                        d_out.K10 = Math.Max(d_out.K10_1, 0.25);
                        d_out.K11 = (1 - Math.Exp(-d_out.beta1) * Math.Cos(d_out.beta1)) / d_out.beta1;
                        d_out.K12 = (1.15 - 0.1432 * Math.PI * 180 * d_in.delta1) / Math.Sin(0.5 * Math.PI * 180 * d_in.delta1);
                        d_out.K13 = Math.Max(1.7 - 2.1 * Math.PI * 180 * d_in.delta1 / Math.PI, 0) / Math.Sin(0.5 * Math.PI * 180 * d_in.delta1);
                        d_out.K14 = (1.45 - 0.43 * Math.PI * 180 * d_in.delta1) / Math.Sin(0.5 * Math.PI * 180 * d_in.delta1);
                        d_out.K15_2 = (0.8 * Math.Sqrt(d_out.gamma) + 6 * d_out.gamma) / (Math.PI * 180 * d_in.delta1);
                        d_out.K15 = Math.Min(1, d_out.K15_2);
                        d_out.K16 = 1 - 0.65 / (1 + Math.Pow(6 * d_out.gamma, 2)) * Math.Sqrt(Math.PI / (3 * Math.PI * 180 * d_in.delta1));
                        d_out.K17 = 1 / (1 + 0.6 * Math.Pow(d_in.D / (d_in.s - d_in.c), 1 / 3) * (d_in.b / d_in.D) * Math.PI * 180 * d_in.delta1);
                        d_out.sigma_mx = 4 * d_out.M1 / (Math.PI * Math.Pow(d_in.D, 2) * (d_in.s - d_in.c));

                        d_out.v1_2 = -0.23 * d_out.K13 * d_out.K15 / (d_out.K12 * d_out.K10);
                        d_out.v1_3 = -0.53 * d_out.K11 / (d_out.K14 * d_out.K16 * d_out.K17 * Math.Sin(0.5 * Math.PI * 180 * d_in.delta1));


                        d_out.K2 = 1.25; //# добавить для условий монтажа
                        d_out.v21_2 = -d_out.sigma_mx / (d_out.K2 * d_in.sigma_d);
                        d_out.v21_3 = 0;
                        d_out.v22_2 = (d_in.p * d_in.D / (4 * (d_in.s - d_in.c)) - d_out.sigma_mx) / (d_out.K2 * d_in.sigma_d);
                        d_out.v22_3 = (d_in.p * d_in.D / (2 * (d_in.s - d_in.c))) / (d_out.K2 * d_in.sigma_d);

                        d_out.K1_21 = (1 - Math.Pow(d_out.v21_2, 2)) / ((1 / 3 + d_out.v1_2 * d_out.v21_2) + Math.Sqrt(Math.Pow(1 / 3 + d_out.v1_2 * d_out.v21_2, 2) + (1 - Math.Pow(d_out.v21_2, 2)) * Math.Pow(d_out.v1_2, 2)));
                        d_out.K1_22 = (1 - Math.Pow(d_out.v22_2, 2)) / ((1 / 3 + d_out.v1_2 * d_out.v22_2) + Math.Sqrt(Math.Pow(1 / 3 + d_out.v1_2 * d_out.v22_2, 2) + (1 - Math.Pow(d_out.v22_2, 2)) * Math.Pow(d_out.v1_2, 2)));
                        d_out.K1_2 = Math.Min(d_out.K1_21, d_out.K1_22);

                        d_out.K1_31 = (1 - Math.Pow(d_out.v21_3, 2)) / ((1 / 3 + d_out.v1_3 * d_out.v21_3) + Math.Sqrt(Math.Pow(1 / 3 + d_out.v1_3 * d_out.v21_3, 2) + (1 - Math.Pow(d_out.v21_3, 2)) * Math.Pow(d_out.v1_3, 2)));
                        d_out.K1_32 = (1 - Math.Pow(d_out.v22_3, 2)) / ((1 / 3 + d_out.v1_3 * d_out.v22_3) + Math.Sqrt(Math.Pow(1 / 3 + d_out.v1_3 * d_out.v22_3, 2) + (1 - Math.Pow(d_out.v22_3, 2)) * Math.Pow(d_out.v1_3, 2)));
                        d_out.K1_3 = Math.Min(d_out.K1_31, d_out.K1_32);

                        d_out.sigmai2_1 = d_out.K1_21 * d_out.K2 * d_in.sigma_d;
                        d_out.sigmai2_2 = d_out.K1_22 * d_out.K2 * d_in.sigma_d;
                        d_out.sigmai2 = Math.Min(d_out.sigmai2_1, d_out.sigmai2_2);

                        d_out.sigmai3_1 = d_out.K1_31 * d_out.K2 * d_in.sigma_d;
                        d_out.sigmai3_2 = d_out.K1_32 * d_out.K2 * d_in.sigma_d;
                        d_out.sigmai3 = Math.Min(d_out.sigmai3_1, d_out.sigmai3_2);

                        d_out.F_d2 = 0.7 * d_out.sigmai2 * (d_in.s - d_in.c) * Math.Sqrt(d_in.D * (d_in.s - d_in.c)) / (d_out.K10 * d_out.K12);
                        d_out.F_d3 = 0.9 * d_out.sigmai3 * (d_in.s - d_in.c) * Math.Sqrt(d_in.D * (d_in.s - d_in.c)) / (d_out.K14 * d_out.K16 * d_out.K17);

                        d_out.Fe = d_out.F1 * (Math.PI / 4) * d_out.K13 * d_out.K15 * Math.Sqrt(d_in.D / (d_in.s - d_in.c));

                        if (d_in.dav == 0)
                        {
                            d_out.yslystoich2 = d_out.M1 / d_out.M_d + d_out.Fe / d_out.F_d + Math.Pow(d_out.Q1 / d_out.Q_d, 2);
                        }
                        else if (d_in.dav == 1)
                        {
                            d_out.yslystoich2 = d_in.p / d_out.p_d + d_out.M1 / d_out.M_d + d_out.Fe / d_out.F_d + Math.Pow(d_out.Q1 / d_out.Q_d, 2);
                        }
                        break;
                    }
                case 2:
                    {
                        d_out.sef = (d_in.s - d_in.c) * Math.Sqrt(1 + Math.Pow(d_in.s2 / (d_in.s - d_in.c), 2));
                        d_out.gamma = 2.83 * (d_in.a / d_in.D) * Math.Sqrt(d_out.sef / d_in.D);
                        d_out.beta1 = 0.91 * d_in.b2 / Math.Sqrt(d_in.D * d_out.sef);
                        d_out.K10_1 = Math.Exp(-d_out.beta1) * Math.Sin(d_out.beta1) / d_out.beta1;
                        d_out.K10 = Math.Max(d_out.K10_1, 0.25);
                        d_out.K11 = (1 - Math.Exp(-d_out.beta1) * Math.Cos(d_out.beta1)) / d_out.beta1;
                        d_out.K12 = (1.15 - 0.1432 * Math.PI * 180 * d_in.delta2) / Math.Sin(0.5 * Math.PI * 180 * d_in.delta2);
                        d_out.K13 = Math.Max(1.7 - 2.1 * Math.PI * 180 * d_in.delta2 / Math.PI, 0) / Math.Sin(0.5 * Math.PI * 180 * d_in.delta2);
                        d_out.K14 = (1.45 - 0.43 * Math.PI * 180 * d_in.delta2) / Math.Sin(0.5 * Math.PI * 180 * d_in.delta2);
                        d_out.K15_2 = (0.8 * Math.Sqrt(d_out.gamma) + 6 * d_out.gamma) / Math.PI * 180 * (d_in.delta2);
                        d_out.K15 = Math.Min(1, d_out.K15_2);
                        d_out.K16 = 1 - 0.65 / (1 + Math.Pow(6 * d_out.gamma, 2)) * Math.Sqrt(Math.PI / (3 * Math.PI * 180 * d_in.delta2));
                        d_out.K17 = 1 / (1 + 0.6 * Math.Pow(d_in.D / (d_out.sef), 1 / 3) * (d_in.b2 / d_in.D) * Math.PI * 180 * d_in.delta2);
                        d_out.sigma_mx = 4 * d_out.M1 / (Math.PI * Math.Pow(d_in.D, 2) * d_out.sef);

                        d_out.v1_2 = -0.23 * d_out.K13 * d_out.K15 / (d_out.K12 * d_out.K10);
                        d_out.v1_3 = -0.53 * d_out.K11 / (d_out.K14 * d_out.K16 * d_out.K17 * Math.Sin(0.5 * Math.PI * 180 * d_in.delta2));


                        d_out.K2 = 1.25; //# добавить для условий монтажа
                        d_out.v21_2 = -d_out.sigma_mx / (d_out.K2 * d_in.sigma_d);
                        d_out.v21_3 = 0;
                        d_out.v22_2 = (d_in.p * d_in.D / (4 * d_out.sef) - d_out.sigma_mx) / (d_out.K2 * d_in.sigma_d);
                        d_out.v22_3 = (d_in.p * d_in.D / (2 * d_out.sef)) / (d_out.K2 * d_in.sigma_d);

                        d_out.K1_21 = (1 - Math.Pow(d_out.v21_2, 2)) / ((1 / 3 + d_out.v1_2 * d_out.v21_2) + Math.Sqrt(Math.Pow(1 / 3 + d_out.v1_2 * d_out.v21_2, 2) + (1 - Math.Pow(d_out.v21_2, 2)) * Math.Pow(d_out.v1_2, 2)));
                        d_out.K1_22 = (1 - Math.Pow(d_out.v22_2, 2)) / ((1 / 3 + d_out.v1_2 * d_out.v22_2) + Math.Sqrt(Math.Pow(1 / 3 + d_out.v1_2 * d_out.v22_2, 2) + (1 - Math.Pow(d_out.v22_2, 2)) * Math.Pow(d_out.v1_2, 2)));

                        d_out.K1_31 = (1 - Math.Pow(d_out.v21_3, 2)) / ((1 / 3 + d_out.v1_3 * d_out.v21_3) + Math.Sqrt(Math.Pow(1 / 3 + d_out.v1_3 * d_out.v21_3, 2) + (1 - Math.Pow(d_out.v21_3, 2)) * Math.Pow(d_out.v1_3, 2)));
                        d_out.K1_32 = (1 - Math.Pow(d_out.v22_3, 2)) / ((1 / 3 + d_out.v1_3 * d_out.v22_3) + Math.Sqrt(Math.Pow(1 / 3 + d_out.v1_3 * d_out.v22_3, 2) + (1 - Math.Pow(d_out.v22_3, 2)) * Math.Pow(d_out.v1_3, 2)));

                        d_out.sigmai2_1 = d_out.K1_21 * d_out.K2 * d_in.sigma_d;
                        d_out.sigmai2_2 = d_out.K1_22 * d_out.K2 * d_in.sigma_d;
                        d_out.sigmai2 = Math.Min(d_out.sigmai2_1, d_out.sigmai2_2);

                        d_out.sigmai3_1 = d_out.K1_31 * d_out.K2 * d_in.sigma_d;
                        d_out.sigmai3_2 = d_out.K1_32 * d_out.K2 * d_in.sigma_d;
                        d_out.sigmai3 = Math.Min(d_out.sigmai3_1, d_out.sigmai3_2);

                        d_out.Fe = d_out.F1 * (Math.PI / 4) * d_out.K13 * d_out.K15 * Math.Sqrt(d_in.D / (d_in.s - d_in.c));

                        d_out.F_d2 = 0.7 * d_out.sigmai2 * d_out.sef * Math.Sqrt(d_in.D * d_out.sef) / (d_out.K10 * d_out.K12);
                        d_out.F_d3 = 0.9 * d_out.sigmai3 * d_out.sef * Math.Sqrt(d_in.D * d_out.sef) / (d_out.K14 * d_out.K16 * d_out.K17);

                        if (d_in.dav == 0)
                        {
                            d_out.yslystoich2 = d_out.M1 / d_out.M_d + d_out.Fe / d_out.F_d + Math.Pow(d_out.Q1 / d_out.Q_d, 2);
                        }
                        else if (d_in.dav == 1)
                        {
                            d_out.yslystoich2 = d_in.p / d_out.p_d + d_out.M1 / d_out.M_d + d_out.Fe / d_out.F_d + Math.Pow(d_out.Q1 / d_out.Q_d, 2);
                        }
                        break;
                    }
            }
            return d_out;
        }
    }
}
