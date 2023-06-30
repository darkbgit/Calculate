using CalculateVessels.Core.Enums;

namespace CalculateVessels.Core.Elements.Base;

#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming

public class LoadingCondition
{
    public int Id { get; set; }
    public PressureType PressureType { get; set; }
    public double EAllow { get; set; }
    public double p { get; set; }
    public double SigmaAllow { get; set; }
    public double t { get; set; }
}