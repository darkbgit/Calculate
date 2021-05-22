using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateVessels.Core.Shells.DataIn
{
    public class EllipticalShellDataIn : ShellDataIn, IDataIn
    {
        public EllipticalShellDataIn()
            : base(ShellType.Elliptical)
        {

        }

        //private double _ellx;
        private double _ellH;
        private double _ellh1;
        private EllipticalBottomType _ellipticalBottomType;



        //public double ellx { get => _ellx; set => _ellx = value; }
        public double ellH
        {
            get => _ellH;
            set => _ellH = value;
        }

        public double ellh1
        {
            get => _ellh1;
            set => _ellh1 = value;
        }

        public EllipticalBottomType EllipticalBottomType
        {
            get => _ellipticalBottomType;
            set => _ellipticalBottomType = value;
        }

    }
}
