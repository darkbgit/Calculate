using CalculateVessels.Core.Elements.Shells.Base;
using CalculateVessels.Core.Elements.Shells.Enums;
using CalculateVessels.Core.Interfaces;

namespace CalculateVessels.Core.Elements.Shells.Cylindrical;

#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming

public class CylindricalShellInput : ShellInputData, IInputData
{
    public CylindricalShellInput()
        : base(ShellType.Cylindrical)
    {

    }

    public string Type => nameof(CylindricalShellInput);

    public bool ConditionForCalcF5341 { get; set; }
    public double F { get; set; }
    public int FCalcSchema { get; set; } //1-7
    public double f { get; set; }
    public double fi_t { get; set; }
    public bool IsFTensile { get; set; }
    public double l { get; set; }
    public double l3 { get; set; }
    public double M { get; set; }
    public double ny { get; set; } = 2.4;
    public double Q { get; set; }
    public double q { get; set; }
}
