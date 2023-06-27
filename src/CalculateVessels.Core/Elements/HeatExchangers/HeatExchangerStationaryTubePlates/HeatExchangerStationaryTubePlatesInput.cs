using CalculateVessels.Core.Elements.HeatExchangers.Enums;
using CalculateVessels.Core.Interfaces;

#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming

namespace CalculateVessels.Core.Elements.HeatExchangers.HeatExchangerStationaryTubePlates;

public class HeatExchangerStationaryTubePlatesInput : IInputData
{
    public string Type => nameof(HeatExchangerStationaryTubePlatesInput);

    public string Name { get; set; } = string.Empty;

    public bool IsOneGo { get; set; }//+
    public bool IsWorkCondition { get; set; }//+
    public double t0 { get; set; } //+
    public int N { get; set; } //+

    //public double a { get; set; }
    public double a1 { get; set; }//+

    //tube
    public bool IsNeedCheckHardnessTube { get; set; }//+
    public double dT { get; set; }//+
    public double l { get; set; } //+ UNDONE: l - half long tube
    public double pT { get; set; } //+
    public double sT { get; set; }//+
    public double tT { get; set; } //+
    public double TT { get; set; } //+
    public int i { get; set; }//+
    public string SteelT { get; set; } = string.Empty;//+

    //shell
    public double cK { get; set; } //+
    public double D { get; set; } //+
    public double pM { get; set; } //+
    public double sK { get; set; } //+
    public double tK { get; set; } //+
    public double TK { get; set; } //+
    public string SteelK { get; set; } = string.Empty;//+

    //compensator
    public bool IsNeedCompensatorCalculate { get; set; }
    public CompensatorType CompensatorType { get; set; }//+
    public double beta0 { get; set; }
    public double deltakom { get; set; }
    public double Dkom { get; set; }
    public double dkom { get; set; }
    public double Kkom { get; set; }
    public double rkom { get; set; }
    public int nkom { get; set; }
    public string Steelkom { get; set; } = string.Empty;

    //extender
    public double D1 { get; set; }
    public double deltap { get; set; }
    public double Lpac { get; set; }

    //tube plate
    public bool IsDifferentTubePlate { get; set; }//+

    public double d0 { get; set; }//+
    public double DE { get; set; }//+

    public double tp { get; set; }//+
    public double tP { get; set; }//+ distance over hole both side from hole

    public ConnectionTubePlate FirstTubePlate { get; set; } = new();
    public ConnectionTubePlate SecondTubePlate { get; set; } = new();

    //partitions
    public bool IsWithPartitions { get; set; } //+
    public double l1R { get; set; } //+
    public double l2R { get; set; } //+

    public int Bper { get; set; }
    public int Lper { get; set; }
    public double cper { get; set; }
    public double sper { get; set; }
}