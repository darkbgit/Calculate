using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Base;
using CalculateVessels.Core.Shells.Enums;

namespace CalculateVessels.Core.Shells.Elliptical
{
    public class EllipticalShellInputData : ShellDataIn, IInputData
    {
        public EllipticalShellInputData()
            : base(ShellType.Elliptical)
        {

        }


        public double EllipseH { get; set; }

        public double Ellipseh1 { get; set; }


        public EllipticalBottomType EllipticalBottomType { get; set; }
    }

}
