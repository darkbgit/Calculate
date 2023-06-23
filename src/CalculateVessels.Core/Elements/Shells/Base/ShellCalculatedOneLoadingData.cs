using CalculateVessels.Core.Elements.Base;

namespace CalculateVessels.Core.Elements.Shells.Base;

#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming

public abstract class ShellCalculatedOneLoadingData
{
    public required LoadingCondition LoadingCondition { get; init; }
    public double s_p { get; set; }
    public double s { get; set; }
    public double p_de { get; set; }
    public double p_d { get; set; }
    public double SigmaAllow { get; set; }
}
