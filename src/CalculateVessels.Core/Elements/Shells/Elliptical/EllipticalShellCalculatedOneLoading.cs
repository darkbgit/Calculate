using CalculateVessels.Core.Elements.Shells.Base;
using System.Collections.Generic;

namespace CalculateVessels.Core.Elements.Shells.Elliptical;

#pragma warning disable IDE1006
// ReSharper disable InconsistentNaming

public class EllipticalShellCalculatedOneLoading : ShellCalculatedOneLoadingData
{
    public double b { get; set; }
    public double b_2 { get; set; }
    public double B1 { get; set; }
    public double B1_2 { get; set; }
    public double ConditionStability { get; set; }
    public double l { get; set; }
    //public double SigmaAllow { get; set; }
    //public double s { get; set; }
    //public double s_p { get; set; }
    public double s_p_1 { get; set; }
    public double s_p_2 { get; set; }
    //public double p_d { get; set; }
    public double p_dp { get; set; }
    //public double p_de { get; set; }
    public double E { get; set; }
    public double EllipseKePrev { get; set; }
    public double Ellipsex { get; set; }
    public double EllipseKe { get; set; }
    public ICollection<string> ErrorList { get; set; } = new List<string>();
}