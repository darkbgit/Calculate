using System.Collections.Generic;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Enums;

namespace CalculateVessels.Core.Shells.Base
{
    public abstract class ShellDataIn : IDataIn
    {
        protected ShellDataIn(ShellType shellType)
        {
            this.ShellType = shellType;
        }

        public IEnumerable<string> ErrorList => _errorList;

        protected List<string> _errorList = new();

        public string Name { get; set; }
        public string Steel { get; set; }

        public ShellType ShellType { get; }

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
                    _errorList.Add("c1 должно быть больше 0");
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
                    _errorList.Add("D должен быть больше 0");
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
                    _errorList.Add("c1 должно быть больше либо равно 0");
                }
            }
        }
        public double c3 { get; set; }

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
                    _errorList.Add("p должно быть больше 0");
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
                    _errorList.Add($"T должна быть в диапазоне {MIN_TEMPERATURE} - {MAX_TEMPERATURE}");
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
                    _errorList.Add("E должно быть больше 0");
                }
            }
        }
        public double s { get; set; }

        public double SigmaAllow
        {
            get => _sigmaAllow;
            set
            {
                {
                    if (value > 0)
                    {
                        _sigmaAllow = value;
                    }
                    else
                    {
                        _errorList.Add("[σ] должно быть больше 0");
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
                if (value is > MIN_FI and <= MAX_FI)
                {
                    _fi = value;
                }
                else
                {
                    _errorList.Add($"φ должен быть в диапазоне {MIN_FI} - {MAX_FI}");
                }
            }
        }

        public double ny { get; set; } = 2.4;

        //public double fit { get => _fit; set => _fit = value; }
        public double F { get; set; }

        public double q { get; set; }

        public double M { get; set; }

        public double Q { get; set; }

        public bool IsPressureIn { get; set; }


        private double _c1;
        private double _D;
        private double _c2;
        private double _p;
        private double _E;
        private double _t;

        private double _sigmaAllow;
        private double _fi;


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
    }
}
