using CalculateVessels.Core.Base;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Enums;

namespace CalculateVessels.Core.Shells.Elliptical;

#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming

public class EllipticalShellInput : InputData, IInputData
{
    public double c1 { get; set; }
    public double c2 { get; set; }
    public double c3 { get; set; }
    public double D { get; set; }
    public double E { get; set; }
    public double fi { get; set; }
    public bool IsPressureIn { get; set; }
    public double EllipseH { get; set; }
    public double Ellipseh1 { get; set; }
    public double ny { get; set; } = 2.4;
    public double p { get; set; }
    public double SigmaAllow { get; set; }
    public string Steel { get; set; } = string.Empty;
    public double s { get; set; }
    public double t { get; set; }

    public EllipticalBottomType EllipticalBottomType { get; set; }
}