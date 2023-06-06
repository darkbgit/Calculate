namespace CalculateVessels.Core.Shells.Base;

#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming

public class LoadingCondition
{
    public bool IsPressureIn { get; set; }
    public double EAllow { get; set; }
    public double p { get; set; }
    public double SigmaAllow { get; set; }
    public double t { get; set; }
    public int OrdinalNumber { get; set; }
}