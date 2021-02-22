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

        internal ShellDataIn ShellDataIn;

        protected bool isCriticalError;
        protected bool isError;
        protected List<string> err = new List<string>();

        //public ShellType ShellType { get => shellType; }

        internal double c { get => _c; }
        internal double s_calcr { get => _s_calcr; }
        internal double p_de { get => _p_de; }


        protected double _c;
        protected double _s_calcr;
        protected double _p_de;
    }

}
