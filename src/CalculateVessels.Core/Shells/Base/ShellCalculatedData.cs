using CalculateVessels.Core.Base;

namespace CalculateVessels.Core.Shells.Base;

#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming

public abstract class ShellCalculatedData : CalculatedElement
{
    public double c { get; set; }
    public double s_p { get; set; }
    public double s { get; set; }
    public double p_de { get; set; }
    public double p_d { get; set; }
    public double SigmaAllow { get; set; }
    public bool IsConditionUseFormulas { get; set; }
}
