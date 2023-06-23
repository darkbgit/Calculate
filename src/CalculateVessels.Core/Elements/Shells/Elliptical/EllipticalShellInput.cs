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

    public override string Type => nameof(EllipticalShellInput);

    public double EllipseH
    {
        get => _ellipseH;
        init
        {
            _ellipseH = value;
            if (_ellipseH <= 0)
            {
                ErrorList.Add("H не задано.");
            }
        }
    }

    public double Ellipseh1
    {
        get => _h1;
        init
        {
            _h1 = value;
            if (_h1 <= 0)
            {
                ErrorList.Add("h1 не задано.");
            }
        }
    }

    public double ny { get; set; } = 2.4;
    public EllipticalBottomType EllipticalBottomType { get; init; }
}