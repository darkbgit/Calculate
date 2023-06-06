using System.Collections.Generic;

namespace CalculateVessels.Core.Shells.Nozzle;

#pragma warning disable IDE1006
// ReSharper disable InconsistentNaming

public class NozzleCalculatedCommon
{
    public double alpha1 { get; set; }
    public double b { get; set; }
    //private double _B1n;
    public double c { get; set; }
    //public double ConditionStrengthening1 { get; set; }
    //public double ConditionStrengthening2 { get; set; }
    public double ConditionUseFormulas1 { get; set; }
    public double ConditionUseFormulas2 { get; set; }
    public double ConditionUseFormulas2_2 { get; set; }
    //public double d0 { get; set; }
    //public double d01 { get; set; }
    //public double d02 { get; set; }
    public double d0p { get; set; }
    public double Dk { get; set; }
    public double dmax { get; set; }
    public double dp { get; set; }
    public double Dp { get; set; }
    //public double E1 { get; set; }
    //public double E2 { get; set; }
    //public double E3 { get; set; }
    //public double E4 { get; set; }
    public double EllipseH { get; set; }
    public bool IsConditionUseFormulas { get; set; }
    public int K1 { get; set; }
    public double L0 { get; set; }
    public double l1p { get; set; }
    public double l1p2 { get; set; }
    public double l2p { get; set; }
    public double l2p2 { get; set; }
    public double l3p { get; set; }
    public double l3p2 { get; set; }
    public double lp { get; set; }
    //public double p_d { get; set; }
    //public double p_de { get; set; }
    //public double p_deShell { get; set; }
    //public double p_dp { get; set; }
    //public double pen { get; set; }
    //public double ppn { get; set; }
    //public double psi1 { get; set; }
    //public double psi2 { get; set; }
    //public double psi3 { get; set; }
    //public double psi4 { get; set; }
    //public double SigmaAllowShell { get; set; }
    //public double SigmaAllow1 { get; set; }
    //public double SigmaAllow2 { get; set; }
    //public double SigmaAllow3 { get; set; }
    //public double SigmaAllow4 { get; set; }
    //public double s1p { get; set; }
    //public double sp { get; set; }
    //public double spn { get; set; }
    //public double V { get; set; }
    //public double V1 { get; set; }
    //public double V2 { get; set; }
    public ICollection<string> ErrorList { get; set; } = new List<string>();
}