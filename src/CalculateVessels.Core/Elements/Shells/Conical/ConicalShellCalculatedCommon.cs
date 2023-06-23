using CalculateVessels.Core.Elements.Shells.Base;

namespace CalculateVessels.Core.Elements.Shells.Conical;


#pragma warning disable IDE1006
// ReSharper disable InconsistentNaming

public class ConicalShellCalculatedCommon : ShellCalculatedCommonData
{
    //public override string Type => nameof(ConicalShellCalculatedCommon);

    /// <summary>
    /// in radians
    /// </summary>
    public double alpha1 { get; set; }

    public double a1p { get; set; }
    public double a2p { get; set; }
    public double a1p_l { get; set; }
    public double a2p_l { get; set; }
    //public double c { get; set; }
    public double Dk { get; set; }
}