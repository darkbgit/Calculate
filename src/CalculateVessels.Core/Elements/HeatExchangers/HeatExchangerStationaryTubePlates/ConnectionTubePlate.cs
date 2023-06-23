using CalculateVessels.Core.Elements.HeatExchangers.Enums;
using CalculateVessels.Core.Enums;

namespace CalculateVessels.Core.Elements.HeatExchangers.HeatExchangerStationaryTubePlates;

#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming

public class ConnectionTubePlate
{
    public bool IsNeedCheckHardnessTubePlate { get; set; }//+
    public TubePlateType TubePlateType { get; set; }
    public FixTubeInTubePlateType FixTubeInTubePlate { get; set; }
    public TubeRollingType TubeRolling { get; set; }

    //Tube plate
    public string Steelp { get; set; } = string.Empty;

    public double BP { get; set; }//+
    public double c { get; set; }//+

    public double fiP { get; set; }

    public double pp { get; set; }

    public double s1p { get; set; }//+

    public double sn { get; set; }//+
    public double sp { get; set; }//+
    public bool IsWithGroove { get; set; }//+


    //shell for tube plate
    public double s1 { get; set; } //+


    //flange for tube plate
    public string Steel1 { get; set; } = string.Empty; //+
    public double h1 { get; set; } //+
    public double DH { get; set; }//+

    //shell for chamber
    public string SteelD { get; set; } = string.Empty;
    public double s2 { get; set; } //+

    //flange for chamber
    public string Steel2 { get; set; } = string.Empty; //+
    public double h2 { get; set; } //+
    public bool IsChamberFlangeSkirt { get; set; }//+
    public FlangeFaceType FlangeFace { get; set; }

    //tube
    public double lB { get; set; }//+
    public double delta { get; set; }//+

}