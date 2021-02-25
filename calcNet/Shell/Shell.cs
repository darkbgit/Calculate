using System.Collections.Generic;

namespace calcNet
{
    public abstract class Shell 
    {
        public Shell()//ShellType shellType)
        {
            //this.shellType = shellType;
        }

        //private readonly ShellType shellType;

        //internal ShellDataIn ShellDataIn;

        internal double c { get => _c; }
        internal double s_calcr { get => _s_calcr; }
        internal double s_calc { get => _s_calc; }
        internal double p_de { get => _p_de; }
        internal double p_d { get => _p_d; }

        public bool IsCriticalError { get => isCriticalError; }
        public bool IsError { get => isError; }
        public List<string> ErrorList { get => err; }


        protected bool isCriticalError;
        protected bool isError;
        protected bool isConditionUseFormuls;

        protected List<string> err = new List<string>();

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
