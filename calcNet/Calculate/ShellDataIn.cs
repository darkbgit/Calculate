﻿using System.Collections.Generic;

namespace calcNet
{
    class ShellDataIn 
    {
        //public ShellDataIn()
        //{

        //}

        private bool isError;

        private List<string> errorList = new List<string>();

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
                    isError = true;
                    errorList.Add("c1 должно быть больше 0");
                }
            }
        }
        public double D
        {
            get => _D;
            set
            {
                if (value > 0)
                {
                    _D = value;
                }
                else
                {
                    isError = true;
                    errorList.Add("D должен быть больше 0");
                }
            }
        }
        public double c2
        {
            get => _c2;
            set
            {
                if (value >= 0)
                {
                    _c2 = value;
                }
                else
                {
                    isError = true;
                    errorList.Add("c1 должно быть больше либо равно 0");
                }
            }
        }
        public double c3 { get => _c3; set => _c3 = value; }
        public double p
        {
            get => _p; 
            set
            {
                if (value > 0)
                {
                    _p = value;
                }
                else
                {
                    isError = true;
                    errorList.Add("p должно быть больше 0");
                }
            }
        }
        public double t
        {
            get => _t;
            set
            {
                const int MIN_TEMPERATURE = 20,
                            MAX_TEMPERATURE = 1000;
                if (value >= MIN_TEMPERATURE && value < MAX_TEMPERATURE)
                {
                    _t = value;
                }
                else
                {
                    isError = true;
                    errorList.Add($"T должна быть в диапазоне {MIN_TEMPERATURE} - {MAX_TEMPERATURE}");
                }
            }
        }
        public double E
        {
            get => _E;
            set
            {
                if (value > 0)
                {
                    _E = value;
                }
                else
                {
                    isError = true;
                    errorList.Add("E должно быть больше 0");
                }
            }
        }
        public double s { get => _s; set => _s = value; }
        public bool IsNeedpCalculate { get => isNeedpCalculate; set => isNeedpCalculate = value; }
        public double sigma_d
        {
            get => _sigma_d;
            set
            {
                {
                    if (value > 0)
                    {
                        _sigma_d = value;
                    }
                    else
                    {
                        isError = true;
                        errorList.Add("[σ] должно быть больше 0");
                    }
                }
            }
        }
        public double fi
        {
            get => _fi;
            set
            {
                const int MIN_FI = 0,
                            MAX_FI = 1;
                if (value > MIN_FI && value <= MAX_FI)
                {
                    _fi = value;
                }
                else
                {
                    isError = true;
                    errorList.Add($"φ должен быть в диапазоне {MIN_FI} - {MAX_FI}");
                }
            }
        }
        public double l 
        {
            get => _l;
            set
            {
                if (value > 0)
                {
                    _l = value;
                }
                else
                {
                    isError = true;
                    errorList.Add("l должно быть больше 0");
                }
            } 
        }
        public double l3_1 { get => _l3_1; set => _l3_1 = value; }
        public double l3_2 { get => _l3_2; set => _l3_2 = value; }
        public double ny { get => _ny; set => _ny = value; }
        public double fit { get => _fit; set => _fit = value; }
        public double F { get => _F; set => _F = value; }
        public double q { get => _q; set => _q = value; }
        public double M { get => _M; set => _M = value; }
        public double Q { get => _Q; set => _Q = value; }
        public bool IsInError { get => isInError; set => isInError = value; }
        public bool IsPressureIn { get => isPressureIn; set => isPressureIn = value; }
        public List<string> ErrorList { get => errorList; }

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
            var field = typeof(ShellDataIn).GetProperty(name);
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
            var field = typeof(ShellDataIn).GetProperty(name);
            field.SetValue(this, value);
        }

        //public void GetType
    }
}
