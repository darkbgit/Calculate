using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace calcNet
{

    

    public enum ShellType
    {
        Cylindrical,
        Elliptical,
        Conical,
        Spherical,
        Torospherical
    }

    class Data_in
    {
        public Data_in(ShellType shellType)
        {
            this.shellType = shellType;
        }

        private string name;
        private string steel;

        internal string Name { get => name; set => name = value; }
        internal string Steel { get => steel; set => steel = value; }

        

        internal ShellType shellType;

        internal EllipticalBottomType ellipticalBottomType;

        internal int temp;

        public double p,
                        E,
                        F,
                        M,
                        Q,
                        sigma_d,
                        D,
                        c1,
                        c2,
                        c3 = 0,
                        fi,
                        fit, // кольцевого сварного шва
                        s,
                        l,
                        l3_1,
                        l3_2,
                        ny = 2.4,
                        elH,
                        elh1,
                        q,
                        f,
                        alfa,
                        R; //Spherical

        internal int FCalcSchema; //1-7

        internal bool isNeedMakeCalcNozzle,
                    isNeedpCalculate,
                    isPressureIn,
                    isNeedFCalculate,
                    isFTensile,
                    isNeedMCalculate,
                    isNeedQCalculate;


        internal int bibliography;

        

        public void SetValue(string name, double value)
        {
            var field = typeof(Data_in).GetField(name);
            field.SetValue(this, value);
        }
    }

    internal struct Data_out
    {
        internal double s_calcr,
                        s_calc,
                        s_calcr1,
                        s_calcr2,
                        s_calcrf,
                        s_calcf,
                        p_d,
                        c,
                        l,
                        b,
                        b_2,
                        b1,
                        b1_2,
                        p_dp,
                        p_de,
                        F_d,
                        F_dp,
                        F_de,
                        F_de1,
                        F_de2,
                        lamda,
                        M_d,
                        M_dp,
                        M_de,
                        Q_d,
                        Q_dp,
                        Q_de,
                        elR,
                        elke,
                        elx,
                        Dk,
                        lpr,
                        F,
                        conditionYstoich;

        internal string err;
        internal bool isConditionUseFormuls,
                        isCriticalError,
                        isError;
    }

    //class DataNozzle_in
    //{
    //    internal NozzleLocation location;

    //    public string name,
    //                    steel1,
    //                    steel2,
    //                    steel3,
    //                    steel4;

    //    internal double sigma_d1,
    //                    sigma_d2,
    //                    sigma_d3,
    //                    sigma_d4;
        
    //    public double   E1,
    //                    E2,
    //                    E3,
    //                    E4;
    //    internal double D,
    //                    d1,
    //                    d2,
    //                    r;


    //    internal double s0,
    //                    s1,
    //                    s2,
    //                    s3;

    //    internal double cs;
    //    internal double cs1;

    //    internal int l;
    //    internal int l1;
    //    internal int l2;
    //    internal int l3;

    //    internal double fi,
    //                    fi1;

    //    internal int delta;
    //    internal int delta1;
    //    internal int delta2;
    //    internal double elx,
    //                    b;
    //    internal NozzleKind nozzleKind;

    //    internal int temp;


    //    internal double ny = 2.4;
    //    internal double t;
    //    internal double gamma;
    //    internal double omega;

    //    internal bool isOval;

    //    public void SetValue(string name, double value)
    //    {
    //        var field = typeof(DataNozzle_in).GetField(name);
    //        field.SetValue(this, value);
    //    }

    //    public void GetValue(string name, ref string value)
    //    {
    //        var field = typeof(DataNozzle_in).GetField(name);
    //        value = field.GetValue(this).ToString();
    //    }
    //}

    class DataNozzle_out
    {
        internal double p_d, p_dp, p_de;
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
        internal double E;
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
        /// <summary>
        /// Get [σ] 
        /// </summary>
        /// <param name="steel">Steel name</param>
        /// <param name="temp">Calculation temperature</param>
        /// <param name="sigma_d">Reference on </param>
        /// <returns>true - Ok, false - Error (Input temperature bigger then the biggest temperature for [σ] in GOST for input steel) </returns>
        internal static bool GetSigma(string steel, double temp, ref double sigma_d, ref List<string> dataInErr)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @"data\data.xml"));
            var root = doc.DocumentElement;
            XmlNode steelTempNodes = root.SelectSingleNode("sigma_list")
                                         .SelectSingleNode("steels")
                                         .SelectSingleNode("//steel[@name='" + steel + "']");

            double sigma, sigmaLittle = 0, sigmaBig = 0;
            int tempLittle = 0, tempBig = 0;

            for (int i = 0; i < steelTempNodes.ChildNodes.Count; i++)
            {
                if ((i == 0 && Convert.ToInt32(steelTempNodes.ChildNodes.Item(i).Attributes["temp"].Value) > temp) ||
                        Convert.ToInt32(steelTempNodes.ChildNodes.Item(i).Attributes["temp"].Value) == temp)
                {
                    sigma_d = Convert.ToDouble(steelTempNodes.ChildNodes.Item(i).Attributes["sigma"].Value,
                                            System.Globalization.CultureInfo.InvariantCulture);
                    return true;
                }
                else if (Convert.ToInt32(steelTempNodes.ChildNodes.Item(i).Attributes["temp"].Value) > temp)
                {
                    tempLittle = Convert.ToInt32(steelTempNodes.ChildNodes.Item(i).Attributes["temp"].Value);
                    sigmaBig = Convert.ToDouble(steelTempNodes.ChildNodes.Item(i).Attributes["sigma"].Value,
                                                System.Globalization.CultureInfo.InvariantCulture);
                    break;
                }
                else if (i == steelTempNodes.ChildNodes.Count - 1)
                {
                    dataInErr.Add($"Температура {temp} °С, больше чем максимальная температура {tempBig} °С для стали {steel} при которой определяется допускаемое напряжение по ГОСТ 34233.1-2017");
                    return false;
                }
                else
                {
                    tempBig = Convert.ToInt32(steelTempNodes.ChildNodes.Item(i).Attributes["temp"].Value);
                    sigmaLittle = double.Parse(steelTempNodes.ChildNodes.Item(i).Attributes["sigma"].Value,
                                                System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            sigma = sigmaBig - ((sigmaBig - sigmaLittle) * (temp - tempLittle) / (tempBig - tempLittle));
            sigma *= 10;
            sigma = Math.Truncate(sigma / 5);
            sigma *= 0.5;
            sigma_d = sigma;
            return true;
        }

        internal static bool GetE(string steel, int temp, ref double E, ref string dataInErr)
        {
            Regex regex = new Regex(@"(.*)(?=\()");
            MatchCollection matches = regex.Matches(steel);

            string steelName;

            if (matches.Count > 0)
            {
                steelName = matches[0].Groups[0].Value;
            }
            else
            {
                steelName = steel;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @"data\data.xml"));
            var root = doc.DocumentElement;
            XmlNode classListNode = root.SelectSingleNode("//n[@name='" + steelName + "']");
            var steelClass = classListNode.ParentNode.Name;
            XmlNode Elist = root.SelectSingleNode("E_list").SelectSingleNode("//class[@name='" + steelClass + "']");

            double ELittle = 0, EBig = 0;
            int tempLittle = 0, tempBig = 0;

            for (int i = 0; i < Elist.ChildNodes.Count; i++)
            {
                if ((i == 0 && Convert.ToInt32(Elist.ChildNodes.Item(i).Attributes["temp"].Value) > temp) ||
                    Convert.ToInt32(Elist.ChildNodes.Item(i).Attributes["temp"].Value) == temp)
                {
                    E = Convert.ToDouble(Elist.ChildNodes.Item(i).Attributes["E"].Value);
                    return true;
                }
                else if (Convert.ToInt32(Elist.ChildNodes.Item(i).Attributes["temp"].Value) > temp)
                {
                    tempLittle = Convert.ToInt32(Elist.ChildNodes.Item(i).Attributes["temp"].Value);
                    EBig = Convert.ToInt32(Elist.ChildNodes.Item(i).Attributes["E"].Value);
                    break;
                }
                else if (i == Elist.ChildNodes.Count - 1)//temperature greater then max in data.xml
                {
                    dataInErr += $"Температура {temp} °С, больше чем максимальная температура {tempBig} °С для стали {steel} при которой определяется модуль  продольной упругости по ГОСТ 34233.1-2017";
                    return false;
                }
                else
                {
                    tempBig = Convert.ToInt32(Elist.ChildNodes.Item(i).Attributes["temp"].Value);
                    ELittle = Convert.ToInt32(Elist.ChildNodes.Item(i).Attributes["E"].Value);
                }
            }


            E = Convert.ToDouble(EBig - ((EBig - ELittle) * (temp - tempLittle) / (tempBig - tempLittle)));

            return true;
        }




        private static void CalculateEllipticalShell(in Data_in d_in, ref Data_out d_out)
        {
            //Data_out d_out = new Data_out { err = "" };
            d_out.c = d_in.c1 + d_in.c2 + d_in.c3;
            
            //Condition use formuls
            {
                const double CONDITION_USE_FORMULS_1_MIN = 0.002,
                            CONDITION_USE_FORMULS_1_MAX = 0.1,
                            CONDITION_USE_FORMULS_2_MIN = 0.2,
                            CONDITION_USE_FORMULS_2_MAX = 0.5;

                if ((((d_in.s - d_out.c) / d_in.D <= CONDITION_USE_FORMULS_1_MAX) &
                    ((d_in.s - d_out.c) / d_in.D >= CONDITION_USE_FORMULS_1_MIN) &
                    (d_in.elH / d_in.D < CONDITION_USE_FORMULS_2_MAX) &
                    (d_in.elH / d_in.D >= CONDITION_USE_FORMULS_2_MIN)) |
                    d_in.s == 0)
                {
                    d_out.isConditionUseFormuls = true;
                }
                else
                {
                    d_out.isConditionUseFormuls = false;
                    d_out.err += "Условие применения формул не выполняется\n";
                }
            }
            d_out.elR = Math.Pow(d_in.D, 2) / (4 * d_in.elH);
            if (d_in.isPressureIn)
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
            else
            {
                d_out.s_calcr2 = 1.2 * d_in.p * d_out.elR / (2 * d_in.sigma_d);

                switch (d_in.ellipticalBottomType)
                {
                    case EllipticalBottomType.Elliptical:
                        d_out.elke = 0.9;
                        break;
                    case EllipticalBottomType.Hemispherical:
                        d_out.elke = 1;
                        break;
                }
                d_out.s_calcr1 = d_out.elke * d_out.elR / 161 * Math.Sqrt(d_in.ny * d_in.p / (0.00001 * d_in.E));
                d_out.s_calcr = Math.Max(d_out.s_calcr1, d_out.s_calcr2);
                d_out.s_calc = d_out.s_calcr + d_out.c;
                if (d_in.s == 0.0)
                {
                    //d_out.elke = 0.9; // # добавить ке для полусферических =1
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
            //return d_out;
        }

        private static void CalculateConicalShell(in Data_in d_in, ref Data_out d_out) // TODO: добавить расчет конуса
        {
            //Data_out d_out = new Data_out { err = "" };
            //return d_out;
        }

        

        internal static DataSaddle_out CalcSaddle(in DataSaddle_in d_in)
        {
            DataSaddle_out d_out = new DataSaddle_out { isConditionUseFormuls = true };

            //d_in.sigma_d = GetSigma(d_in.steel, d_in.temp);
            {
                double sigma_d = 0;
                string dataInErr = "";
                //if (GetSigma(d_in.steel, d_in.temp, ref sigma_d, ref dataInErr))
                //{
                //    d_in.sigma_d = sigma_d;
                //}
                //else
                //{
                //    d_out.err += dataInErr;
                //}
            }

            //E
            {
                double E = 0;
                string dataInErr = "";
                if (GetE(d_in.steel, d_in.temp, ref E, ref dataInErr))
                {
                    d_in.E = E;
                }
                else
                {
                    d_out.err += dataInErr;
                }
            }

            bool isNotError = d_out.err == null;

            if (isNotError)
            {

                d_out.M_d = (0.0000089 * d_in.E) / d_in.ny * Math.Pow(d_in.D, 3) * Math.Pow((100 * (d_in.s - d_in.c)) / d_in.D, 2.5);
                // UNDONE: проверить формулу для расчета [F]
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
                    if (d_out.yslproch1_1 > d_out.yslproch1_2)
                    {
                        d_out.err += "Несущая способность обечайки в сечении между опорами. Условие прочности не выполняется\n";
                    }
                    if (d_in.isPressureIn)
                    {
                        d_out.yslystoich1 = d_out.M12 / d_out.M_d;
                    }
                    else
                    {
                        d_out.yslystoich1 = d_in.p / d_out.p_d + d_out.M12 / d_out.M_d;
                    }
                    if (d_out.yslystoich1 > 1)
                    {
                        d_out.err += "Несущая способность обечайки в сечении между опорами. Условие устойчивости не выполняется\n";
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


                            d_out.K2 = 1.25; // TODO: добавить для условий монтажа
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

                            d_out.yslproch2 = Math.Min(d_out.F_d2, d_out.F_d3);

                            if (d_out.F1 > d_out.yslproch2)
                            {
                                d_out.err += "Несущая способность обечайки, не укрепленной кольцами жесткости в области опорного узла. Условие прочности не выполняется\n";
                            }


                            d_out.Fe = d_out.F1 * (Math.PI / 4) * d_out.K13 * d_out.K15 * Math.Sqrt(d_in.D / (d_in.s - d_in.c));

                            if (d_in.isPressureIn)
                            {
                                d_out.yslystoich2 = d_out.M1 / d_out.M_d + d_out.Fe / d_out.F_d + Math.Pow(d_out.Q1 / d_out.Q_d, 2);
                            }
                            else
                            {
                                d_out.yslystoich2 = d_in.p / d_out.p_d + d_out.M1 / d_out.M_d + d_out.Fe / d_out.F_d + Math.Pow(d_out.Q1 / d_out.Q_d, 2);
                            }
                            if (d_out.yslystoich2 > 1)
                            {
                                d_out.err += "Несущая способность обечайки, не укрепленной кольцами жесткости в области опорного узла. Условие устойчивости не выполняется\n";
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


                            d_out.K2 = 1.25; // TODO: добавить для условий монтажа
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

                            d_out.yslproch2 = Math.Min(d_out.F_d2, d_out.F_d3);

                            if (d_out.F1 > d_out.yslproch2)
                            {
                                d_out.err += "Несущая способность обечайки, не укрепленной кольцами жесткости в области опорного узла. Условие прочности не выполняется\n";
                            }

                            if (d_in.isPressureIn)
                            {
                                d_out.yslystoich2 = d_out.M1 / d_out.M_d + d_out.Fe / d_out.F_d + Math.Pow(d_out.Q1 / d_out.Q_d, 2);
                            }
                            else
                            {
                                d_out.yslystoich2 = d_in.p / d_out.p_d + d_out.M1 / d_out.M_d + d_out.Fe / d_out.F_d + Math.Pow(d_out.Q1 / d_out.Q_d, 2);
                            }
                            if (d_out.yslystoich2 > 1)
                            {
                                d_out.err += "Несущая способность обечайки, не укрепленной кольцами жесткости в области опорного узла. Условие устойчивости не выполняется\n";
                            }

                            break;
                        }
                    case 3:
                        {
                            //TODO: опора с укрепляющим кольцом
                            break;
                        }
                }

                if (d_in.delta1 <= 60 || d_in.delta1 >= 180)
                {
                    d_out.isConditionUseFormuls = false;
                    d_out.err += "delta1 должно быть в пределах 60-180\n";
                }
                if ((d_in.s - d_in.c) / d_in.D > 0.05)
                {
                    d_out.isConditionUseFormuls = false;
                    d_out.err += "Условие применения формул не выполняется\n";
                }
                if (d_in.type == 2)
                {
                    if (d_in.delta2 < d_in.delta1 + 20)
                    {
                        d_out.isConditionUseFormuls = false;
                        d_out.err += "Угол обхвата подкладного листа должен быть delta2>=delta1+20\n";
                    }
                    if (d_in.s2 < d_in.s)
                    {
                        d_out.isConditionUseFormuls = false;
                        d_out.err += "Толщина подкладного листа должна быть s2>=s\n";
                    }
                    d_out.Ak = d_in.b2 * d_in.s2;
                    d_out.Akypf = (d_in.s - d_in.c) * Math.Sqrt(d_in.D * (d_in.s - d_in.c));
                    if (d_out.Ak < d_out.Akypf)
                    {
                        d_out.isConditionUseFormuls = false;
                        d_out.err += "Условие применения формул не выполняется\n";
                    }
                }
            }
            return d_out;
        }

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
