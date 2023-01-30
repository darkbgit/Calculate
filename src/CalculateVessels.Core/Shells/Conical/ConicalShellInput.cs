using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Base;
using CalculateVessels.Core.Shells.Enums;

namespace CalculateVessels.Core.Shells.Conical;

#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming

public class ConicalShellInput : ShellInputData, IInputData
{
    public ConicalShellInput()
        : base(ShellType.Conical)
    {

    }

    public double alpha1 { get; set; }
    public ConicalConnectionType ConnectionType { get; set; }
    public double c1 { get; set; }
    public double c2 { get; set; }
    public double c3 { get; set; }
    public double D1 { get; set; }
    public double E { get; set; }
    public double fi_k { get; set; }
    public double fi_t { get; set; }
    public bool IsConnectionWithLittle { get; set; }
    public double ny { get; set; } = 2.4;
    public double r { get; set; }
    public double SigmaAllow { get; set; }
    public string Steel1Up { get; set; } = string.Empty;
    public string Steel1Down { get; set; } = string.Empty;
    public string Steel2Up { get; set; } = string.Empty;
    public string Steel2Down { get; set; } = string.Empty;
    public double s1 { get; set; }
    public double s2 { get; set; }
    public double s1Little { get; set; }
    public double s2Little { get; set; }
    public double sT { get; set; }
    public double sr { get; set; }
}