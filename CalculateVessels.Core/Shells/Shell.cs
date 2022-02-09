using System.Collections.Generic;
using System.Linq;

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

        public double c => _c;
        public double s_p => _s_p;
        public double s => _s;
        public double p_de => _p_de;
        public double p_d => _p_d;

        public bool IsCriticalError { get; protected set; }
        public bool IsError { get; protected set; }
        public IEnumerable<string> ErrorList => _errorList;

        public bool IsConditionUseFormulas { get; protected set; }

        protected double _c;
        protected double _s_p;
        protected double _s_p_1;
        protected double _s_p_2;
        protected double _s;
        protected double _p_de;
        protected double _p_dp;
        protected double _p_d;
        protected double _sigmaAllow;
        public double SigmaAllow => _sigmaAllow;

        protected List<string> _errorList = new();

        public IEnumerable<string> Bibliography { get; } = new List<string> 
        {
            Data.Properties.Resources.GOST_34233_1,
            Data.Properties.Resources.GOST_34233_2
        };
    }

}
