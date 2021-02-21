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
    }

}
