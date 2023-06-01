using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Base;
using CalculateVessels.Core.Shells.Enums;

namespace CalculateVessels.Core.Shells.Elliptical;

#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming

public class EllipticalShellInput : ShellInputData, IInputData
{
    public EllipticalShellInput()
        : base(ShellType.Elliptical)
    {
    }

    public double EllipseH { get; set; }
    public double Ellipseh1 { get; set; }
    public double ny { get; set; } = 2.4;
    public double SigmaAllow { get; set; }
    public EllipticalBottomType EllipticalBottomType { get; set; }
}