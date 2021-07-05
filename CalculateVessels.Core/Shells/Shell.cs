using System.Collections.Generic;

namespace CalculateVessels.Core.Shells
{
    public abstract class Shell
    {
        protected Shell()//ShellType shellType)
        {
            //this.shellType = shellType;
        }

        //private readonly ShellType shellType;

        //internal ShellDataIn ShellDataIn;

        public double c { get => _c; }
        public double s_calcr { get => _s_calcr; }
        public double s_calc { get => _s_calc; }
        public double p_de { get => _p_de; }
        public double p_d { get => _p_d; }

        public bool IsCriticalError { get; protected set; }
        public bool IsError { get; protected set; }
        public List<string> ErrorList { get; protected set; } = new();

        public bool IsConditionUseFormulas { get; protected set; }

        //public ShellType ShellType { get => shellType; }

        protected double _c;
        protected double _s_calcr;
        protected double _s_calcr1;
        protected double _s_calcr2;
        protected double _s_calc;
        protected double _p_de;
        protected double _p_dp;
        protected double _p_d;
    }

}
