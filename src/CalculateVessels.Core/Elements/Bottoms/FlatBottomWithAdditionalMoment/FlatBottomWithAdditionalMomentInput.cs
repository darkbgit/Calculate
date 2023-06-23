using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Enums;
using CalculateVessels.Core.Elements.Base;
using CalculateVessels.Core.Elements.Bottoms.Enums;

namespace CalculateVessels.Core.Elements.Bottoms.FlatBottomWithAdditionalMoment;

#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming

public class FlatBottomWithAdditionalMomentInput : InputData, IInputData
{
    //shell
    public double c1 { get; set; }
    public double c2 { get; set; }
    public double c3 { get; set; }
    public double D { get; set; }
    public double s { get; set; }

    //cover
    public string CoverSteel { get; set; } = string.Empty;
    public double D2 { get; set; }
    public double D3 { get; set; }
    public bool IsCoverFlat { get; set; }
    public bool IsCoverWithGroove { get; set; }
    public double s1 { get; set; }
    public double s2 { get; set; }
    public double s3 { get; set; }
    public double s4 { get; set; }

    //stress condition
    public double F { get; set; }
    public double fi { get; set; }
    public bool IsPressureIn { get; set; }
    public double M { get; set; }
    public double p { get; set; }
    public double t { get; set; }
    public double SigmaAllow { get; set; }

    //screw
    public bool IsStud { get; set; }
    public bool IsScrewWithGroove { get; set; }
    public double Lb0 { get; set; }
    public int n { get; set; }
    public string ScrewSteel { get; set; } = string.Empty;
    public int Screwd { get; set; }

    //gasket
    public double bp { get; set; }
    public double Dcp { get; set; }
    public string GasketType { get; set; } = string.Empty;
    public double hp { get; set; }

    //flange
    public double Db { get; set; }
    public double Dn { get; set; }
    public FlangeFaceType FlangeFace { get; set; }
    public string FlangeSteel { get; set; } = string.Empty;
    public double h { get; set; }
    public bool IsFlangeIsolated { get; set; }
    public bool IsFlangeFlat { get; set; }
    public double l { get; set; }
    public double S0 { get; set; }
    public double S1 { get; set; }

    //washer
    public bool IsWasher { get; set; }
    public double hsh { get; set; }
    public string WasherSteel { get; set; } = string.Empty;

    //hole
    public double di { get; set; }
    public double d { get; set; }
    public double E { get; set; }
    public HoleInFlatBottom Hole { get; set; }
}