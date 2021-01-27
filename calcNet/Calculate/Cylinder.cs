using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calcNet.Calculate
{
    class DataShellIn
    {
        private string name;
        private string steel;
        private ShellType shellType;


        internal string Name { get => name; set => name = value; }
        internal string Steel { get => steel; set => steel = value; }
        internal ShellType ShellType { get => shellType; set => shellType = value; }
        public double c1
        {
            get => _c1; 
            set
            {
                if (value > 0)
                {
                    _c1 = value;
                }
                else
                {
                    throw new Exception($"{_c1} должно быть больше 0");
                }
            }
        }
        public double D { get => _D; set => _D = value; }
        public double c2 { get => _c2; set => _c2 = value; }
        public double c3 { get => _c3; set => _c3 = value; }
        public double p { get => _p; set => _p = value; }
        internal double t { get => _t; set => _t = value; }
        public double E { get => _E; set => _E = value; }
        public double s { get => _s; set => _s = value; }
        internal bool IsNeedpCalculate { get => isNeedpCalculate; set => isNeedpCalculate = value; }
        public double sigma_d { get => _sigma_d; set => _sigma_d = value; }
        public double fi { get => _fi; set => _fi = value; }
        public double L { get => _l; set => _l = value; }
        public double l3_1 { get => _l3_1; set => _l3_1 = value; }
        public double l3_2 { get => _l3_2; set => _l3_2 = value; }
        public double ny { get => ny_f; set => ny_f = value; }

        internal EllipticalBottomType ellipticalBottomType;

        

        public double                                                 F_f,
                        M_f,
                        Q_f,
                        fit_f, // кольцевого сварного шва
                        elH_f,
                        elh1_f,
                        q_f,
                        f_f,
                        alfa_f,
                        R_f; //Spherical

        internal int FCalcSchema; //1-7

        internal bool isNeedMakeCalcNozzle,
                    isPressureIn,
                    isNeedFCalculate,
                    isFTensile,
                    isNeedMCalculate,
                    isNeedQCalculate;


        internal int bibliography;
        private double _c1;
        private double _D;
        private double _c2;
        private double _c3;
        private double _p;
        private double _E;
        private double _t;
        private double _s;
        private bool isNeedpCalculate;
        private double _sigma_d;
        private double _fi;
        private double _l;
        private double _l3_1;
        private double _l3_2;
        private double ny_f;

        public void SetValue(string name, double value)
        {
            var field = typeof(Data_in).GetField(name);
            field.SetValue(this, value);
        }
    }

    class Shell
    {
        private string name;
        private string steel;

        internal string Name { get => name; set => name = value; }
        internal string Steel { get => steel; set => steel = value; }
        internal ShellType ShellType { get => shellType; set => shellType = value; }

        public Shell(ShellType shellType)
        {
            ShellType = shellType;
        }

        internal EllipticalBottomType EllipticalBottomType { get => ellipticalBottomType; set => ellipticalBottomType = value; }
        public double T { get => t; set => t = value; }

        private ShellType shellType;

        private EllipticalBottomType ellipticalBottomType;



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
        private double t;

        public void SetValue(string name, double value)
        {
            var field = typeof(Data_in).GetField(name);
            field.SetValue(this, value);
        }
    }

    class Cylinder 
    {
        public Cylinder(DataShellIn dataShellIn)
        {
            if (dataShellIn.ShellType == ShellType.Cylindrical)
            {
                this._dataCylinderIn = dataShellIn;
                CalculateCylindricalShell(_dataCylinderIn);
            }
        }

        private void CalculateCylindricalShell(DataShellIn d_in)
        {

            _c = d_in.c1 + d_in.c2 + d_in.c3;

            // Condition use formuls
            {
                const int DIAMETR_BIG_LITTLE_BORDER = 200;
                bool isDiametrBig = DIAMETR_BIG_LITTLE_BORDER < d_in.D;

                //bool isConditionUseFormuls;

                if (isDiametrBig)
                {
                    const double CONDITION_USE_FORMULS_BIG_DIAMETR = 0.1;
                    isConditionUseFormuls = ((d_in.s - _c) / d_in.D) <= CONDITION_USE_FORMULS_BIG_DIAMETR;
                }
                else
                {
                    const double CONDITION_USE_FORMULS_LITTLE_DIAMETR = 0.3;
                    isConditionUseFormuls = ((d_in.s - _c) / d_in.D) <= CONDITION_USE_FORMULS_LITTLE_DIAMETR;
                }

                if (!isConditionUseFormuls)
                {
                    isError = true;
                    err += "Условие применения формул не выполняется\n";
                }

            }

            if (d_in.IsNeedpCalculate)
            {
                if (d_in.isPressureIn)
                {

                    _s_calcr = d_in.p * d_in.D / (2 * d_in.sigma_d * d_in.fi - d_in.p);
                    _s_calc = _s_calcr + _c;
                    if (d_in.s == 0.0)
                    {
                        _p_d = 2 * d_in.sigma_d * d_in.fi * (_s_calc - _c) / (d_in.D + _s_calc - _c);
                    }
                    else if (d_in.s >= _s_calc)
                    {
                        _p_d = 2 * d_in.sigma_d * d_in.fi * (d_in.s - _c) / (d_in.D + d_in.s - _c);
                    }
                    else
                    {
                        throw new Exception("");
                    }
                }
                else
                {
                    l = d_in.L + d_in.l3_1 + d_in.l3_2;
                    _b_2 = 0.47 * Math.Pow(d_in.p / (0.00001 * d_in.E), 0.067) * Math.Pow(l / d_in.D, 0.4);
                    _b = Math.Max(1.0, _b_2);
                    _s_calcr1 = 1.06 * (0.01 * d_in.D / _b) * Math.Pow(d_in.p / (0.00001 * d_in.E) * (l / d_in.D), 0.4);
                    _s_calcr2 = 1.2 * d_in.p * d_in.D / (2 * d_in.sigma_d - d_in.p);
                    _s_calcr = Math.Max(_s_calcr1, _s_calcr2);
                    _s_calc = _s_calcr + _c;
                    if (d_in.s == 0.0)
                    {
                        _p_dp = 2 * d_in.sigma_d * (_s_calc - _c) / (d_in.D + _s_calc - _c);
                        _b1_2 = 9.45 * (d_in.D / l) * Math.Sqrt(d_in.D / (100 * (_s_calc - _c)));
                        _b1 = Math.Min(1.0, _b1_2);
                        _p_de = 2.08 * 0.00001 * d_in.E / d_in.ny * _b1 * (d_in.D / l) * Math.Pow(100 * (_s_calc - _c) / d_in.D, 2.5);
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
                        d_out.isCriticalError = true;
                        d_out.err += "Принятая толщина меньше расчетной\nрасчет не выполнен";
                    }
                    d_out.p_d = d_out.p_dp / Math.Sqrt(1 + Math.Pow(d_out.p_dp / d_out.p_de, 2));
                }
                if (d_out.p_d < d_in.P && d_in.s != 0)
                {
                    d_out.isError = true;
                    d_out.err += "[p] меньше p";
                }
            }

            if (d_in.isNeedFCalculate)
            {
                d_out.s_calcrf = d_in.F / (Math.PI * d_in.D * d_in.sigma_d * d_in.fit);
                d_out.s_calcf = d_out.s_calcrf + d_out.c;
                if (d_in.isFTensile)
                {
                    d_out.F_d = Math.PI * (d_in.D + d_in.s - d_out.c) * (d_in.s - d_out.c) * d_in.sigma_d * d_in.fit;
                }
                else
                {
                    d_out.F_dp = Math.PI * (d_in.D + d_in.s - d_out.c) * (d_in.s - d_out.c) * d_in.sigma_d;
                    d_out.F_de1 = 0.000031 * d_in.E / d_in.ny * Math.Pow(d_in.D, 2) * Math.Pow(100 * (d_in.s - d_out.c) / d_in.D, 2.5);

                    const int L_MORE_THEN_D = 10;
                    bool isLMoreThenD = d_in.l / d_in.D > L_MORE_THEN_D;

                    if (isLMoreThenD)
                    {
                        switch (d_in.FCalcSchema)
                        {
                            case 1:
                                d_out.lpr = d_in.l;
                                break;
                            case 2:
                                d_out.lpr = 2 * d_in.l;
                                break;
                            case 3:
                                d_out.lpr = 0.7 * d_in.l;
                                break;
                            case 4:
                                d_out.lpr = 0.5 * d_in.l;
                                break;
                            case 5:
                                d_out.F = d_in.q * d_in.l;
                                d_out.lpr = 1.12 * d_in.l;
                                break;
                            case 6:
                                double fDivl6 = d_in.f / d_in.l;
                                fDivl6 *= 10;
                                fDivl6 = Math.Round(fDivl6 / 2);
                                fDivl6 *= 0.2;
                                switch (fDivl6)
                                {
                                    case 0:
                                        d_out.lpr = 2 * d_in.l;
                                        break;
                                    case 0.2:
                                        d_out.lpr = 1.73 * d_in.l;
                                        break;
                                    case 0.4:
                                        d_out.lpr = 1.47 * d_in.l;
                                        break;
                                    case 0.6:
                                        d_out.lpr = 1.23 * d_in.l;
                                        break;
                                    case 0.8:
                                        d_out.lpr = 1.06 * d_in.l;
                                        break;
                                    case 1:
                                        d_out.lpr = d_in.l;
                                        break;
                                }
                                break;
                            case 7:
                                double fDivl7 = d_in.f / d_in.l;
                                fDivl7 *= 10;
                                fDivl7 = Math.Round(fDivl7 / 2);
                                fDivl7 *= 0.2;
                                switch (fDivl7)
                                {
                                    case 0:
                                        d_out.lpr = 2 * d_in.l;
                                        break;
                                    case 0.2:
                                        d_out.lpr = 1.7 * d_in.l;
                                        break;
                                    case 0.4:
                                        d_out.lpr = 1.4 * d_in.l;
                                        break;
                                    case 0.6:
                                        d_out.lpr = 1.11 * d_in.l;
                                        break;
                                    case 0.8:
                                        d_out.lpr = 0.85 * d_in.l;
                                        break;
                                    case 1:
                                        d_out.lpr = 0.7 * d_in.l;
                                        break;
                                }
                                break;

                        }
                        d_out.lamda = 2.83 * d_out.lpr / (d_in.D + d_in.s - d_out.c);
                        d_out.F_de2 = Math.PI * (d_in.D + d_in.s - d_out.c) * (d_in.s - d_out.c) * d_in.E / d_in.ny *
                                        Math.Pow(Math.PI / d_out.lamda, 2);
                        d_out.F_de = Math.Min(d_out.F_de1, d_out.F_de2);
                    }
                    else
                    {
                        d_out.F_de = d_out.F_de1;
                    }

                    d_out.F_d = d_out.F_dp / Math.Sqrt(1 + Math.Pow(d_out.F_dp / d_out.F_de, 2));
                }
            }

            if (d_in.isNeedMCalculate)
            {
                d_out.M_dp = Math.PI / 4 * d_in.D * (d_in.D + d_in.s - d_out.c) * (d_in.s - d_out.c) * d_in.sigma_d;
                d_out.M_de = 0.000089 * d_in.E / d_in.ny * Math.Pow(d_in.D, 3) * Math.Pow(100 * (d_in.s - d_out.c) / d_in.D, 2.5);
                d_out.M_d = d_out.M_dp / Math.Sqrt(1 + Math.Pow(d_out.M_dp / d_out.M_de, 2));
            }

            if (d_in.isNeedQCalculate)
            {
                d_out.Q_dp = 0.25 * d_in.sigma_d * Math.PI * d_in.D * (d_in.s - d_out.c);
                d_out.Q_de = 2.4 * d_in.E * Math.Pow(d_in.s - d_out.c, 2) / d_in.ny *
                    (0.18 + 3.3 * d_in.D * (d_in.s - d_out.c) / Math.Pow(d_in.l, 2));
                d_out.Q_d = d_out.Q_dp / Math.Sqrt(1 + Math.Pow(d_out.Q_dp / d_out.Q_de, 2));
            }

            if ((d_in.IsNeedpCalculate || d_in.isNeedFCalculate) &&
                (d_in.isNeedMCalculate || d_in.isNeedQCalculate))
            {
                d_out.conditionYstoich = d_in.P / d_out.p_d + d_in.F / d_out.F_d + d_in.M / d_out.M_d +
                                        Math.Pow(d_in.Q / d_out.Q_d, 2);
            }
            // TODO: Проверка F для FCalcSchema == 6 F расчитывается и записывается в d_out, а не берется из d_in
            //return d_out;
        }

        readonly DataShellIn _dataCylinderIn;

        internal DataShellIn DataCylinderIn => _dataCylinderIn;

        internal string Name { get => _dataCylinderIn.Name; }
        internal string Steel { get => _dataCylinderIn.Steel; }
        internal ShellType ShellType { get => _dataCylinderIn.ShellType; }
        public double t { get => _dataCylinderIn.t; }
        public double L_in { get => _dataCylinderIn.L; }
        public double ny { get => _dataCylinderIn.ny; }

        internal EllipticalBottomType EllipticalBottomType { get => ellipticalBottomType; set => ellipticalBottomType = value; }
        
        internal double c { get => _c; }
        internal bool IsConditionUseFormuls { get => isConditionUseFormuls; }
        internal bool IsError { get => isError; }
        internal string ErrorString { get => err; }
        internal double s_calcr { get => _s_calcr; }
        internal double s_calc { get => _s_calc; }
        internal double s_calcr1 { get => _s_calcr1;  }
        internal double s_calcr2 { get => _s_calcr2;  }
        internal double s_calcrf { get => _s_calcrf;  }
        internal double s_calcf { get => _s_calcf;  }
        internal double p_d { get => _p_d;  }
        internal double b { get => _b;  }
        internal double b_2 { get => _b_2;  }
        internal double b1 { get => _b1;  }
        internal double b1_2 { get => _b1_2; }
        internal double p_dp { get => _p_dp;  }
        internal double p_de { get => _p_de;  }
        internal double F_d { get => f_d; }
        internal double F_dp { get => f_dp; }
        internal double F_de { get => f_de;  }
        internal double F_de1 { get => f_de1;  }
        internal double F_de2 { get => f_de2;  }
        internal double Lamda { get => lamda;  }
        internal double M_d { get => m_d; }
        internal double M_dp { get => m_dp; }
        internal double M_de { get => m_de;  }
        internal double Q_d { get => q_d;  }
        internal double Q_dp { get => q_dp;  }
        internal double Q_de { get => q_de;  }
        internal double ElR { get => elR;  }
        internal double Elke { get => elke;  }
        internal double Elx { get => elx;  }
        internal double Dk { get => dk; }
        internal double Lpr { get => lpr; set => lpr = value; }
        internal double F1 { get => f1;  }
        internal double ConditionYstoich { get => conditionYstoich;  }
        internal double L { get => l; }

        private EllipticalBottomType ellipticalBottomType;



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
                        l3_1,
                        l3_2,

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
        private double t;

        public void SetValue(string name, double value)
        {
            var field = typeof(Data_in).GetField(name);
            field.SetValue(this, value);
        }






        private double l;

        private string err;
        internal bool _isCriticalError;
        private double _c;
        private bool isConditionUseFormuls;
        private bool isError;
        private double _s_calcr;
        private double _s_calc;
        private double _s_calcr1;
        private double _s_calcr2;
        private double _s_calcrf;
        private double _s_calcf;
        private double _p_d;
        private double _b;
        private double _b_2;
        private double _b1;
        private double _b1_2;
        private double _p_dp;
        private double _p_de;
        private double f_d;
        private double f_dp;
        private double f_de;
        private double f_de1;
        private double f_de2;
        private double lamda;
        private double m_d;
        private double m_dp;
        private double m_de;
        private double q_d;
        private double q_dp;
        private double q_de;
        private double elR;
        private double elke;
        private double elx;
        private double dk;
        private double lpr;
        private double f1;
        private double conditionYstoich;
        private double l_in;
    }

    class p
    {
        void pp()
        {
            //Cylinder cyl = new Cylinder();
            //cyl.
        }
    }
}
