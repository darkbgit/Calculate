using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calcNet
{
    class EllipticalShellDataIn : ShellDataIn
    {
        public EllipticalShellDataIn()
            : base(ShellType.Elliptical)
        {

        }

        //private double _ellx;
        private double _ellH;

        //public double ellx { get => _ellx; set => _ellx = value; }
        public double ellH { get => _ellH; }

    }
}
