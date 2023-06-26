using CalculateVessels.Core.Elements.Base;
using CalculateVessels.Core.Elements.HeatExchangers.Enums;
using CalculateVessels.Core.Helpers;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Interfaces;
using System;

namespace CalculateVessels.Core.Elements.HeatExchangers.HeatExchangerUTube;

public class HeatExchangerUTubeCalculateService : CalculateService, ICalculateService<HeatExchangerUTubeInput>
{
    public HeatExchangerUTubeCalculateService(IPhysicalDataService physicsDataService)
    : base(physicsDataService)
    {
        Name = "Gost 34233.7-2017";
    }

    public ICalculatedElement Calculate(HeatExchangerUTubeInput dataIn)
    {
        var data = new HeatExchangerUTubeCalculated
        {
            InputData = dataIn,
            sigmap = PhysicalHelper.GetSigma(dataIn.Steelp, (dataIn.TK > dataIn.TT ? dataIn.TK : dataIn.TT), PhysicalData),
            dE = dataIn.TubeFixType switch
            {
                UTubeFixType.FullyFixed => dataIn.d0 - 2 * dataIn.sT,
                UTubeFixType.PartlyFixed => dataIn.d0 - dataIn.sT,
                UTubeFixType.NonFerrousTubes => dataIn.d0,
                UTubeFixType.AirCooledVessels => throw new NotImplementedException(),
                _ => throw new ArgumentOutOfRangeException(),
            }
        };

        data.phiE = 1 - data.dE / dataIn.tp;

        data.pp = Math.Max(dataIn.pM, Math.Max(dataIn.pT, Math.Abs(dataIn.pM - dataIn.pT)));


        if (dataIn.IsSpecialRequirements)
        {
            data.spp2 = Math.Sqrt((2 * dataIn.a1 + 1.5 * dataIn.Dcp * (dataIn.Dcp - 2 * dataIn.a1)) /
                                  (dataIn.Dcp - 2 * dataIn.a1 * (1 - data.phiE))) * data.phiE +
                        data.pp / (data.phiE * data.sigmap);

            data.spp = 0.82 * dataIn.a1 * Math.Sqrt(data.pp / (data.phiE * data.sigmap)) * Math.Max(1, data.spp2);
        }
        else
        {
            data.spp = dataIn.Dcp / 3.4 * Math.Sqrt(data.pp / (data.phiE * data.sigmap));
        }

        data.sp = data.spp + dataIn.c;

        if (data.sp > dataIn.sp)
        {
            data.ErrorList.Add($"Толщина трубной решетки {dataIn.sp} мм меньше расчетной {data.sp} мм.");
        }

        (data.spp_5_5_1, data.sp_5_5_1) = HeatExchangerAdditionalRequirementsCalculate
            .CheckThicknessAccordingTo5_5_1(dataIn.DE, data.pp, data.sigmap, dataIn.c, dataIn.sp, data.ErrorList);

        (data.sprp1, data.sprp2, data.sprp, data.spr) = HeatExchangerAdditionalRequirementsCalculate
            .CheckThicknessAccordingTo5_5_2(data.pp, dataIn.Dcp, dataIn.DB, data.sigmap, dataIn.c, dataIn.spr,
                data.ErrorList);



        return data;
    }
}