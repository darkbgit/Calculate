using CalculateVessels.Core.Elements.HeatExchangers.Enums;
using CalculateVessels.Core.Interfaces;

namespace CalculateVessels.Core.Elements.HeatExchangers.HeatExchangerUTube;

#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming

public class HeatExchangerUTubeInput : IInputData
{
    public string Type => nameof(HeatExchangerUTubeInput);
    public string Name { get; set; } = string.Empty;
    public string Steelp { get; set; } = string.Empty;
    public double t { get; set; }
    public double Dcp { get; set; }
    public UTubeFixType TubeFixType { get; set; }
    public double d0 { get; set; }
    public double sT { get; set; }
    public double tp { get; set; }
    public bool IsSpecialRequirements { get; set; }
    public double a1 { get; set; }
    public double c { get; set; }
    public double sp { get; set; }
    public double pM { get; set; }
    public double pT { get; set; }
    public double DE { get; set; }
    public double DB { get; set; }
    public double spr { get; set; }
    public double TK { get; set; }
    public double TT { get; set; }
}