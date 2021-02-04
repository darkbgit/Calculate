using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calcNet
{
    interface IElement
    {
        void ChekInputData();
    }

    interface ICheckedElement
    {
        void Calculate();
    }

    interface ICalculatedElement
    {
        void MakeWord();
    }


    class DataShellIn
    {
        private string name;
        private string steel;
        private ShellType shellType;


        public string Name { get => name; set => name = value; }
        public string Steel { get => steel; set => steel = value; }
        public ShellType ShellType { get => shellType; set => shellType = value; }
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
        public double t
        {
            get => _t;
            set
            {
                const int MIN_TEMPERATURE = 20,
                            MAX_TEMPERATURE = 1000;
                if (value >= MIN_TEMPERATURE && t < MAX_TEMPERATURE)
                {
                    _t = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("t", value,
                        $"T должна быть в диапазоне {MIN_TEMPERATURE} - {MAX_TEMPERATURE}");
                }
            }
        }
        public double E { get => _E; set => _E = value; }
        public double s { get => _s; set => _s = value; }
        public bool IsNeedpCalculate { get => isNeedpCalculate; set => isNeedpCalculate = value; }
        public double sigma_d { get => _sigma_d; set => _sigma_d = value; }
        public double fi { get => _fi; set => _fi = value; }
        public double L { get => _l; set => _l = value; }
        public double l3_1 { get => _l3_1; set => _l3_1 = value; }
        public double l3_2 { get => _l3_2; set => _l3_2 = value; }
        public double ny { get => _ny; set => _ny = value; }
        public double fit { get => _fit; set => _fit = value; }
        public double F { get => _F; set => _F = value; }
        public double q { get => _q; set => _q = value; }
        public double M { get => _M; set => _M = value; }
        public double Q { get => _Q; set => _Q = value; }
        public bool IsInError { get => isInError; set => isInError = value; }
        internal bool IsPressureIn { get => isPressureIn; set => isPressureIn = value; }

        internal EllipticalBottomType ellipticalBottomType;

        

        public double                                                                                                                         elH_f,
                        elh1_f,
                        f_f,
                        alfa_f,
                        R_f; //Spherical

        internal int FCalcSchema; //1-7

        internal bool             isNeedMakeCalcNozzle,
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
        private double _ny;
        private double _fit;
        private double _F;
        private double _q;
        private double _M;
        private double _Q;
        private bool isInError;
        private bool isPressureIn;

        public void SetValue(string name, double value)
        {
            var field = typeof(DataShellIn).GetProperty(name);
            try
            {
                field.SetValue(this, value);
            }
            catch (System.Reflection.TargetInvocationException ex)
            {
                throw ex.InnerException; // System.Windows.Forms.MessageBox.Show(ex.InnerException.Message);
            }
            //catch (ArgumentOutOfRangeException)
            //{
            //    ;
            //}
        }

        public void SetValue(string name, string value)
        {
            var field = typeof(DataShellIn).GetProperty(name);
            field.SetValue(this, value);
        }

        //public void GetType


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

            if (d_in.p > 0)
            {
                if (d_in.IsPressureIn)
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
                    _l = d_in.L + d_in.l3_1 + d_in.l3_2;
                    _b_2 = 0.47 * Math.Pow(d_in.p / (0.00001 * d_in.E), 0.067) * Math.Pow(_l / d_in.D, 0.4);
                    _b = Math.Max(1.0, _b_2);
                    _s_calcr1 = 1.06 * (0.01 * d_in.D / _b) * Math.Pow(d_in.p / (0.00001 * d_in.E) * (_l / d_in.D), 0.4);
                    _s_calcr2 = 1.2 * d_in.p * d_in.D / (2 * d_in.sigma_d - d_in.p);
                    _s_calcr = Math.Max(_s_calcr1, _s_calcr2);
                    _s_calc = _s_calcr + _c;
                    if (d_in.s == 0.0)
                    {
                        _p_dp = 2 * d_in.sigma_d * (_s_calc - _c) / (d_in.D + _s_calc - _c);
                        _b1_2 = 9.45 * (d_in.D / _l) * Math.Sqrt(d_in.D / (100 * (_s_calc - _c)));
                        _b1 = Math.Min(1.0, _b1_2);
                        _p_de = 2.08 * 0.00001 * d_in.E / d_in.ny * _b1 * (d_in.D / _l) * Math.Pow(100 * (_s_calc - _c) / d_in.D, 2.5);
                    }
                    else if (d_in.s >= _s_calc)
                    {
                        _p_dp = 2 * d_in.sigma_d * (d_in.s - _c) / (d_in.D + d_in.s - _c);
                        _b1_2 = 9.45 * (d_in.D / _l) * Math.Sqrt(d_in.D / (100 * (d_in.s - _c)));
                        _b1 = Math.Min(1.0, _b1_2);
                        _p_de = 2.08 * 0.00001 * d_in.E / d_in.ny * _b1 * (d_in.D / _l) * Math.Pow(100 * (d_in.s - _c) / d_in.D, 2.5);
                    }
                    else
                    {
                        throw new Exception("Принятая толщина меньше расчетной\nрасчет не выполнен");
                    }
                    _p_d = _p_dp / Math.Sqrt(1 + Math.Pow(_p_dp / _p_de, 2));
                }
                if (_p_d < d_in.p && d_in.s != 0)
                {
                    isError = true;
                    err += "[p] меньше p";
                }
            }

            if (d_in.F > 0)
            {
                _s_calcrf = d_in.F / (Math.PI * d_in.D * d_in.sigma_d * d_in.fit);
                _s_calcf = _s_calcrf + _c;
                if (d_in.isFTensile)
                {
                    _F_d = Math.PI * (d_in.D + d_in.s - _c) * (d_in.s - _c) * d_in.sigma_d * d_in.fit;
                }
                else
                {
                    _F_dp = Math.PI * (d_in.D + d_in.s - _c) * (d_in.s - _c) * d_in.sigma_d;
                    _F_de1 = 0.000031 * d_in.E / d_in.ny * Math.Pow(d_in.D, 2) * Math.Pow(100 * (d_in.s - _c) / d_in.D, 2.5);

                    const int L_MORE_THEN_D = 10;
                    bool isLMoreThenD = d_in.L / d_in.D > L_MORE_THEN_D;

                    if (isLMoreThenD)
                    {
                        switch (d_in.FCalcSchema)
                        {
                            case 1:
                                _lpr = d_in.L;
                                break;
                            case 2:
                                _lpr = 2 * d_in.L;
                                break;
                            case 3:
                                _lpr = 0.7 * d_in.L;
                                break;
                            case 4:
                                _lpr = 0.5 * d_in.L;
                                break;
                            case 5:
                                _F = d_in.q * d_in.L;
                                _lpr = 1.12 * d_in.L;
                                break;
                            case 6:
                                double fDivl6 = d_in.F / d_in.L;
                                fDivl6 *= 10;
                                fDivl6 = Math.Round(fDivl6 / 2);
                                fDivl6 *= 0.2;
                                switch (fDivl6)
                                {
                                    case 0:
                                        _lpr = 2 * d_in.L;
                                        break;
                                    case 0.2:
                                        _lpr = 1.73 * d_in.L;
                                        break;
                                    case 0.4:
                                        _lpr = 1.47 * d_in.L;
                                        break;
                                    case 0.6:
                                        _lpr = 1.23 * d_in.L;
                                        break;
                                    case 0.8:
                                        _lpr = 1.06 * d_in.L;
                                        break;
                                    case 1:
                                        _lpr = d_in.L;
                                        break;
                                }
                                break;
                            case 7:
                                double fDivl7 = d_in.F / d_in.L;
                                fDivl7 *= 10;
                                fDivl7 = Math.Round(fDivl7 / 2);
                                fDivl7 *= 0.2;
                                switch (fDivl7)
                                {
                                    case 0:
                                        _lpr = 2 * d_in.L;
                                        break;
                                    case 0.2:
                                        _lpr = 1.7 * d_in.L;
                                        break;
                                    case 0.4:
                                        _lpr = 1.4 * d_in.L;
                                        break;
                                    case 0.6:
                                        _lpr = 1.11 * d_in.L;
                                        break;
                                    case 0.8:
                                        _lpr = 0.85 * d_in.L;
                                        break;
                                    case 1:
                                        _lpr = 0.7 * d_in.L;
                                        break;
                                }
                                break;

                        }
                        lamda = 2.83 * _lpr / (d_in.D + d_in.s - _c);
                        _F_de2 = Math.PI * (d_in.D + d_in.s - _c) * (d_in.s - _c) * d_in.E / d_in.ny *
                                        Math.Pow(Math.PI / lamda, 2);
                        _F_de = Math.Min(_F_de1, _F_de2);
                    }
                    else
                    {
                        _F_de = _F_de1;
                    }

                    _F_d = _F_dp / Math.Sqrt(1 + Math.Pow(_F_dp / _F_de, 2));
                }
            }

            if (d_in.M > 0)
            {
                _M_dp = Math.PI / 4 * d_in.D * (d_in.D + d_in.s - _c) * (d_in.s - _c) * d_in.sigma_d;
                _M_de = 0.000089 * d_in.E / d_in.ny * Math.Pow(d_in.D, 3) * Math.Pow(100 * (d_in.s - _c) / d_in.D, 2.5);
                _M_d = _M_dp / Math.Sqrt(1 + Math.Pow(_M_dp / _M_de, 2));
            }

            if (d_in.Q > 0)
            {
                _Q_dp = 0.25 * d_in.sigma_d * Math.PI * d_in.D * (d_in.s - _c);
                _Q_de = 2.4 * d_in.E * Math.Pow(d_in.s - _c, 2) / d_in.ny *
                    (0.18 + 3.3 * d_in.D * (d_in.s - _c) / Math.Pow(d_in.L, 2));
                _Q_d = _Q_dp / Math.Sqrt(1 + Math.Pow(_Q_dp / _Q_de, 2));
            }

            if ((d_in.IsNeedpCalculate || d_in.isNeedFCalculate) &&
                (d_in.isNeedMCalculate || d_in.isNeedQCalculate))
            {
                conditionYstoich = d_in.p / _p_d + d_in.F / _F_d + d_in.M / _M_d +
                                        Math.Pow(d_in.Q / _Q_d, 2);
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
        public double F { get => _dataCylinderIn.F; }

        //internal EllipticalBottomType EllipticalBottomType { get => ellipticalBottomType; set => ellipticalBottomType = value; }
        
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
        internal double F_d { get => _F_d; }
        internal double F_dp { get => _F_dp; }
        internal double F_de { get => _F_de;  }
        internal double F_de1 { get => _F_de1;  }
        internal double F_de2 { get => _F_de2;  }
        internal double Lamda { get => lamda;  }
        internal double M_d { get => _M_d; }
        internal double M_dp { get => _M_dp; }
        internal double M_de { get => _M_de;  }
        internal double Q_d { get => _Q_d;  }
        internal double Q_dp { get => _Q_dp;  }
        internal double Q_de { get => _Q_de;  }
        internal double ElR { get => _elR;  }
        internal double Elke { get => _elke;  }
        internal double Elx { get => _elx;  }
        internal double Dk { get => _Dk; }
        internal double Lpr { get => _lpr; }
        internal double F1 { get => _F1;  }
        internal double ConditionYstoich { get => conditionYstoich;  }
        internal double L { get => _l; }
        

        private EllipticalBottomType ellipticalBottomType;



        public double p,
                        E,
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


        public void SetValue(string name, double value)
        {
            var field = typeof(Data_in).GetField(name);
            field.SetValue(this, value);
        }






        private double _l;

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
        private double _F_d;
        private double _F_dp;
        private double _F_de;
        private double _F_de1;
        private double _F_de2;
        private double lamda;
        private double _M_d;
        private double _M_dp;
        private double _M_de;
        private double _Q_d;
        private double _Q_dp;
        private double _Q_de;
        private double _elR;
        private double _elke;
        private double _elx;
        private double _Dk;
        private double _lpr;
        private double _F1;
        private double conditionYstoich;
        private double l_in;
        private double _F;
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
