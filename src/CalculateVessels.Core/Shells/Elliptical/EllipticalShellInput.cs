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

    private readonly double _ellipseH;
    private readonly double _h1;

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