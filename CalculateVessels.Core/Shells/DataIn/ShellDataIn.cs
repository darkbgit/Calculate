using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Enums;
using System.Collections.Generic;
using System.Linq;

namespace CalculateVessels.Core.Shells.DataIn
{
    public abstract class ShellDataIn : IDataIn
    {
        public ShellDataIn(ShellType shellType)
        {
            this.shellType = shellType;
        }

        public bool CheckData() => ErrorList.Any();
        




        public ShellType Type { get; }

        protected bool isError;

        private bool isDataGood;



        protected List<string> errorList = new List<string>();

        private string _name;
        private string steel;
        private ShellType shellType;


        public bool IsDataGood { get => isDataGood; }

        public string Name { get => _name; set => _name = value; }
        public string Steel { get => steel; set => steel = value; }
        public ShellType ShellType { get => shellType; }
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

        public double ny { get => _ny; set => _ny = value; }
        public double fit { get => _fit; set => _fit = value; }
        public double F { get => _F; set => _F = value; }
        public double q { get => _q; set => _q = value; }
        public double M { get => _M; set => _M = value; }
        public double Q { get => _Q; set => _Q = value; }

        public bool IsPressureIn { get => isPressureIn; set => isPressureIn = value; }
        public List<string> ErrorList { get => errorList; }




        internal int FCalcSchema; //1-7

        internal bool isNeedMakeCalcNozzle,
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

        private double _sigma_d;
        private double _fi;

        private double _ny = 2.4;
        private double _fit;
        private double _F;
        private double _q;
        private double _M;
        private double _Q;

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
