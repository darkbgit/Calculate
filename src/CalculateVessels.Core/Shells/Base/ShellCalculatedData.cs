using System.Collections.Generic;

namespace CalculateVessels.Core.Shells.Base
{
    public abstract class ShellCalculatedData
    {
        protected ShellCalculatedData()//ShellType shellType)
        {
            //this.shellType = shellType;
        }

        //private readonly ShellType shellType;

        //internal ShellDataIn ShellDataIn;

        public double c { get; set; }
        public double s_p { get; set; }
        public double s { get; set; }
        public double p_de { get; set; }
        public double p_d { get; set; }
        public double SigmaAllow { get; set; }


        public ICollection<string> ErrorList => _errorList;

        public bool IsConditionUseFormulas { get;  set; }

        protected double _c;
        protected double _s_p;
        protected double _s_p_1;
        protected double _s_p_2;
        protected double _s;
        protected double _p_de;
        protected double _p_dp;
        protected double _p_d;
        protected double _sigmaAllow;


        
    

        protected List<string> _errorList = new();

    }

}
