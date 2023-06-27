using CalculateVessels.Core.Elements.Shells.Base;
using CalculateVessels.Core.Elements.Shells.Enums;
using CalculateVessels.Core.Interfaces;

namespace CalculateVessels.Core.Elements.Shells.Elliptical;

#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming

public class EllipticalShellInput : ShellInputData, IInputData
{
    private readonly double _ellipseH;
    private readonly double _h1;

    public EllipticalShellInput()
        : base(ShellType.Elliptical)
    {
    }

    public string Type => nameof(EllipticalShellInput);

    public double EllipseH { get; init; }

    public double Ellipseh1 { get; init; }

    public double ny { get; set; } = 2.4;
    public EllipticalBottomType EllipticalBottomType { get; init; }
}