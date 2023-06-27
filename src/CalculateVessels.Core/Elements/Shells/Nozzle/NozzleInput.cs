using CalculateVessels.Core.Elements.Shells.Nozzle.Enums;
using CalculateVessels.Core.Interfaces;

namespace CalculateVessels.Core.Elements.Shells.Nozzle;

#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming

public class NozzleInput : IInputData
{
    public NozzleInput(ICalculatedElement shellCalculatedData)
    {
        ShellCalculatedData = shellCalculatedData;
    }

    public string Type => nameof(NozzleInput);
    public string Name { get; set; } = string.Empty;
    public double cs { get; set; }
    public double cs1 { get; set; }
    public double d { get; set; }
    public double d1 { get; set; }
    public double d2 { get; set; }
    public double delta { get; set; }
    public double delta1 { get; set; }
    public double delta2 { get; set; }
    public double E1 { get; set; }
    public double E2 { get; set; }
    public double E3 { get; set; }
    public double E4 { get; set; }
    public double ellx { get; set; }
    public double phi { get; set; }
    public double phi1 { get; set; }
    public double gamma { get; set; }
    public bool IsOval { get; set; }
    public NozzleLocation Location { get; set; }
    public double l { get; set; }
    public double l1 { get; set; }
    public double l2 { get; set; }
    public double l3 { get; set; }
    public NozzleKind NozzleKind { get; set; }
    public double omega { get; set; }
    public double r { get; set; }
    public ICalculatedElement ShellCalculatedData { get; }
    public double SigmaAllow1 { get; set; }
    public double SigmaAllow2 { get; set; }
    public double SigmaAllow3 { get; set; }
    public double SigmaAllow4 { get; set; }
    public double s0 { get; set; }
    public double s1 { get; set; }
    public double s2 { get; set; }
    public double s3 { get; set; }
    public string steel1 { get; set; } = string.Empty;
    public string steel2 { get; set; } = string.Empty;
    public string steel3 { get; set; } = string.Empty;
    public string steel4 { get; set; } = string.Empty;
    //public double t { get; set; }
    public double tTransversely { get; set; }
}