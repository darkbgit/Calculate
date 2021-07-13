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
        public double s_p { get => _s_p; }
        public double s { get => _s; }
        public double p_de { get => _p_de; }
        public double p_d { get => _p_d; }

        public bool IsCriticalError { get; protected set; }
        public bool IsError { get; protected set; }
        public List<string> ErrorList { get; protected set; } = new();

        public bool IsConditionUseFormulas { get; protected set; }

        protected double _c;
        protected double _s_p;
        protected double _s_p_1;
        protected double _s_p_2;
        protected double _s;
        protected double _p_de;
        protected double _p_dp;
        protected double _p_d;

        public List<string> Bibliograhy { get; } = new() 
        {
            Data.Properties.Resources.GOST_34233_1,
            Data.Properties.Resources.GOST_34233_2
        };
    }

}
