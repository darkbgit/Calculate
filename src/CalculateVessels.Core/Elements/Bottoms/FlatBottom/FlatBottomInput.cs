using CalculateVessels.Core.Elements.Bottoms.Enums;
using CalculateVessels.Core.Interfaces;

namespace CalculateVessels.Core.Elements.Bottoms.FlatBottom;

#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming

public class FlatBottomInput : IInputData
{
    public string Type => nameof(FlatBottomInput);
    public string Name { get; set; } = string.Empty;

    public double a { get; set; }
    public double c1 { get; set; }
    public double c2 { get; set; }
    public double c3 { get; set; }
    public double D { get; set; }
    public double D2 { get; set; }
    public double D3 { get; set; }
    public double Dcp { get; set; }
    public double d { get; set; }
    public double di { get; set; }
    public double E { get; set; }
    public double fi { get; set; }
    public double gamma { get; set; }
    public HoleInFlatBottom Hole { get; set; }
    public double h1 { get; set; }
    public double p { get; set; }
    public double r { get; set; }
    public string Steel { get; set; } = string.Empty;
    public double SigmaAllow { get; set; }
    public double s { get; set; }
    public double s1 { get; set; }
    public double s2 { get; set; }
    public int FlatBottomType { get; set; }
    public double t { get; set; }
}