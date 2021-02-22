using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calcNet
{
    class ConicalShellDataIn : ShellDataIn
    {
        public ConicalShellDataIn()
            : base(ShellType.Conical)
        {

        }

        public double alfa { get; set; }

        
    }
}
