using CalculateVessels.Core.Base;
using CalculateVessels.Core.Shells.Enums;

namespace CalculateVessels.Core.Shells.Base;

#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming

public abstract class ShellInputData : InputData
{
    protected ShellInputData(ShellType shellType)
    {
        ShellType = shellType;
    }

    public double D { get; set; }
    public double fi { get; set; }
    public bool IsPressureIn { get; set; }
    public double p { get; set; }
    public double t { get; set; }
    public ShellType ShellType { get; }
    public string Steel { get; set; } = string.Empty;
    public double s { get; set; }


    //public double c1 { get; set; }
    //public double c2 { get; set; }
    //public double c3 { get; set; }
    //public double E { get; set; }
    //public double SigmaAllow { get; set; }
    //public double ny { get; set; } = 2.4;
    //public double F { get; set; }
    //public double q { get; set; }
    //public double M { get; set; }
    //public double Q { get; set; }
}