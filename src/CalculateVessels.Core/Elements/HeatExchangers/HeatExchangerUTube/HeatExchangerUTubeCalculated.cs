using CalculateVessels.Core.Elements.Base;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Properties;

namespace CalculateVessels.Core.Elements.HeatExchangers.HeatExchangerUTube;

public class HeatExchangerUTubeCalculated : CalculatedElement, ICalculatedElement
{
    public HeatExchangerUTubeCalculated()
    {
        Bibliography = new[]
        {
            Resources.GOST_34233_1,
            Resources.GOST_34233_6,
            Resources.GOST_34233_7
        };
    }

    public override string Type => nameof(HeatExchangerUTubeCalculated);

    public double sigmap { get; set; }
    public double phiE { get; set; }
    public double spp { get; set; }
    public double dE { get; set; }
    public double spp2 { get; set; }
    public double sp { get; set; }
    public double pp { get; set; }
    public double spp_5_5_1 { get; set; }
    public double sp_5_5_1 { get; set; }
    public double sprp1 { get; set; }
    public double sprp2 { get; set; }
    public double sprp { get; set; }
    public double spr { get; set; }

}