using CalculateVessels.Core.Enums;
using CalculateVessels.Core.Interfaces;

namespace CalculateVessels.Core.Elements.Supports.BracketVertical;

#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming

public class BracketVerticalInput : IInputData
{
    public string Type => nameof(BracketVerticalInput);
    public string Name { get; set; } = string.Empty;
    public bool IsAssembly { get; set; }
    public PressureType PressureType { get; set; }
    public bool PreciseMontage { get; set; }
    public bool ReinforcingPad { get; set; }
    public BracketVerticalType BracketVerticalType { get; set; }
    public double b2 { get; set; }
    public double b3 { get; set; }
    public double b4 { get; set; }
    public double c { get; set; }
    public double D { get; set; }
    public double e1 { get; set; }
    public double g { get; set; }
    public double G { get; set; }
    public double h { get; set; }
    public double h1 { get; set; }
    public double l1 { get; set; }
    public double M { get; set; }
    public double p { get; set; }
    public double phi { get; set; }
    public double Q { get; set; }
    public double s { get; set; }
    public double s2 { get; set; }
    public double SigmaAllow { get; set; }
    public double t { get; set; }
    public int n { get; set; }
    public int N { get; set; }
    public string NameShell { get; set; } = string.Empty;
    public string Steel { get; set; } = string.Empty;
}