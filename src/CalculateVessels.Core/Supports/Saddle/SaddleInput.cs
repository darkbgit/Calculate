using CalculateVessels.Core.Base;
using CalculateVessels.Core.Enums;
using CalculateVessels.Core.Interfaces;

namespace CalculateVessels.Core.Supports.Saddle;

#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming

public class SaddleInput : InputData, IInputData
{
    public override string Type => nameof(SaddleInput);
    public bool IsAssembly { get; set; }
    public bool IsPressureIn { get; set; }
    public double a { get; set; }
    public double Ak { get; set; }
    public double b { get; set; }
    public double b2 { get; set; }
    public double c { get; set; }
    public double D { get; set; }
    public double delta1 { get; set; }
    public double delta2 { get; set; }
    public double E { get; set; }
    public double e { get; set; }
    public double fi { get; set; }
    public double G { get; set; }
    public double H { get; set; }
    public double L { get; set; }
    public double p { get; set; }
    public double s { get; set; }
    public double s2 { get; set; }
    public double SigmaAllow { get; set; }
    public double t { get; set; }
    public int N { get; set; }
    public SaddleType SaddleType { get; set; }
    public string NameShell { get; set; } = string.Empty;
    public string Steel { get; set; } = string.Empty;
}