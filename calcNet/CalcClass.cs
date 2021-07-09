using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;

namespace CalculateVessels
{
 
    class DataSaddle_in
    {
        internal double G;
        internal double L;
        internal double Lob;
        internal double H;
        internal double e;
        internal double a;
        internal double p;
        internal double D;
        internal double s;
        internal double s2;
        internal double c;
        internal double fi;
        internal double sigma_d;
        internal double E;
        internal double ny = 2.4;
        internal int type;
        internal double b;
        internal double b2;
        internal double delta1;
        internal double delta2;
        internal string name;
        internal string nameob;
        internal double temp;
        internal double l0;
        internal string steel;

        internal bool isPressureIn;
    }

    class DataSaddle_out
    {
        internal double q,
                        M0,
                        p_d,
                        F1,
                        F2,
                        F_d,
                        M1,
                        M2,
                        M12,
                        M_d,
                        Q1,
                        Q2,
                        Q_d,
                        B1,
                        B1_2,
                        yslproch1_1,
                        yslproch1_2,
                        yslproch2,
                        yslystoich1,
                        yslystoich2,
                        K9,
                        K9_1,
                        y,
                        x,
                        gamma,
                        beta1,
                        K10,
                        K10_1,
                        K11,
                        K12,
                        K13,
                        K14,
                        K15,
                        K15_2,
                        K16,
                        K17,
                        sigma_mx,
                        F_d2,
                        F_d3,
                        v1_2,
                        v1_3,
                        v21_2,
                        v21_3 = 0,
                        v22_2,
                        v22_3,
                        K2,
                        K1_2,
                        K1_21,
                        K1_22,
                        K1_3,
                        K1_31,
                        K1_32,
                        sigmai2,
                        sigmai2_1,
                        sigmai2_2,
                        sigmai3,
                        sigmai3_1,
                        sigmai3_2,
                        Fe,
                        sef,
                        Ak,
                        Akypf;
        internal string err = "";
        internal bool isConditionUseFormuls;
    }

    struct DataPldn_in
    {
        internal string name,
                        steel;
        internal double D,
            D2,
            D3,
            Dsp,
            s,
            s1,
            s2,
            s3,
            s4,
            a,
            r,
            h1,
            gamma,
            c1,
            c2,
            c3,
            d,
            di,
            sigma_d,
            p,
            fi;
        internal int type; // 1 - 15
        internal int otv; // 0 - 0 , 1 - 1, 2 - >1
        internal bool isPressureIn;
    }

    struct DataPldn_out
    {
        internal string err;
        internal double c,
            Dp,
            ypfzn,
            K,
            K_1,
            K0,
            s1_calcr,
            s1_calc,
            psi1,
            Qd,
            Pbp,
            K6;
        internal bool ypf;
    }

    class DataHeat_in
    {

    }

    class DataHeat_out
    {

    }

    class CalcClass
    {
        
   




        private static double DegToRad(double degree) => degree * Math.PI / 180;
        

        

        

        internal static DataPldn_out CalcPldn(DataPldn_in d_in)
        {
            DataPldn_out d_out = new DataPldn_out { err = "" };

            d_out.c = d_in.c1 + d_in.c2 + d_in.c3;

            switch (d_in.type)
            {
                case 1:
                    d_out.K = 0.53;
                    d_out.Dp = d_in.D;
                    if (d_in.a < 1.7 * d_in.s)
                    {
                        d_out.err += "Условие закрепления не выполняется a>=1.7s\n";
                    }
                    break;
                case 2:
                    d_out.K = 0.50;
                    d_out.Dp = d_in.D;
                    if (d_in.a < 0.85 * d_in.s)
                    {
                        d_out.err += "Условие закрепления не выполняется a>=0.85s\n";
                    }
                    break;
                case 3:
                    d_out.Dp = d_in.D;
                    if ((d_in.s - d_out.c) / (d_in.s1 - d_out.c) < 0.25)
                    {
                        d_out.K = 0.45;
                    }
                    else
                    {
                        d_out.K = 0.41;
                    }
                    break;
                case 4:
                    d_out.Dp = d_in.D;
                    if ((d_in.s - d_out.c) / (d_in.s1 - d_out.c) < 0.5)
                    {
                        d_out.K = 0.41;
                    }
                    else
                    {
                        d_out.K = 0.38;
                    }
                    break;
                case 5:
                    goto case 3;
                case 6:
                    d_out.K = 0.50;
                    d_out.Dp = d_in.D;
                    if (d_in.a < 0.85 * d_in.s)
                    {
                        d_out.err += "Условие закрепления не выполняется a>=0.85s\n";
                    }
                    break;
                case 7:
                case 8:
                    goto case 4;
                case 9:
                    d_out.Dp = d_in.D - 2 * d_in.r;
                    if (d_in.h1 < d_in.r || d_in.r < Math.Min(d_in.s, 025 * d_in.s1) || d_in.r > Math.Min(d_in.s1, 0.1 * d_in.D))
                    {
                        d_out.err += "Условие закрепления не выполняется\n";
                    }
                    d_out.K_1 = 0.41 * (1 - 0.23 * ((d_in.s - d_out.c) / (d_in.s1 - d_out.c)));
                    d_out.K = Math.Max(d_out.K_1, 0.35);
                    break;
                case 10:
                    if (d_in.gamma < 30 || d_in.gamma > 90 || d_in.r < 0.25 * d_in.s1 || d_in.r > (d_in.s1 - d_in.s2))
                    {
                        d_out.err += "Условие закрепления не выполняется\n";
                    }
                    goto case 4;
                case 11:
                    goto case 4;
                case 12:
                    d_out.K = 0.4;
                    d_out.Dp = d_in.D3;
                    break;
                case 13:
                    d_out.K = 0.41;
                    d_out.Dp = d_in.Dsp;
                    break;
                case 14:
                case 15:
                    d_out.Dp = d_in.Dsp;
                    //d_out.Pbp = 
                    d_out.Qd = 0.785 * d_in.p * Math.Pow(d_in.Dsp, 2);
                    d_out.psi1 = d_out.Pbp / d_out.Qd;
                    d_out.K6 = 0.41 * Math.Sqrt((1 + 3 * d_out.psi1 * (d_in.D3 / d_in.Dsp - 1)) / (d_in.D3 / d_in.Dsp));
                    break;
            }
            // UNDONE: доделать расчет плоского днища
            switch (d_in.otv)
            {
                case 0:
                    d_out.K0 = 1;
                    break;

                case 1:
                    d_out.K0 = Math.Sqrt(1 + d_in.d / d_out.Dp + Math.Pow(d_in.d / d_out.Dp, 2));
                    break;

                case 2:
                    if (d_in.di > 0.7 * d_out.Dp)
                    {
                        d_out.err += "Слишком много отверстий\n";
                    }
                    d_out.K0 = Math.Sqrt((1 - Math.Pow(d_in.di / d_out.Dp, 3)) / (1 - d_in.di / d_out.Dp));
                    break;
            }

            d_out.s1_calcr = d_out.K * d_out.K0 * d_out.Dp * Math.Sqrt(d_in.p / (d_in.fi * d_in.sigma_d));
            d_out.s1_calc = d_out.s1_calcr + d_out.c;



            if (d_in.s1 != 0 && d_in.s1 >= d_out.s1_calc)
            {
                d_out.ypfzn = (d_in.s1 - d_out.c) / d_out.Dp;
                if (d_out.ypfzn <= 0.11)
                {
                    d_out.ypf = true;

                }
            }
            else if (d_in.s1 != 0 && d_in.s1 < d_out.s1_calc)
            {
                d_out.err += "Принятая толщина меньше расчетной\n";
            }


            return d_out;
        }

    }
}
