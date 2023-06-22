using CalculateVessels.Core.HeatExchangers.HeatExchangerStationaryTubePlates;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Forms.Base;
using System.Collections.Generic;
using System;

namespace CalculateVessels.Forms.MiddleForms;

public class HeatExchangerStationaryTubePlatesFormMiddle : BaseCalculateForm<HeatExchangerStationaryTubePlatesInput>
{
    protected HeatExchangerStationaryTubePlatesFormMiddle(IEnumerable<ICalculateService<HeatExchangerStationaryTubePlatesInput>> calculateServices,
        IPhysicalDataService physicalDataService)
        : base(calculateServices, physicalDataService)
    {
    }

    public HeatExchangerStationaryTubePlatesFormMiddle()
        : this(null, null)
    {

    }

    protected override bool CollectDataForPreliminarilyCalculation()
    {
        throw new NotImplementedException();
    }

    protected override bool CollectDataForFinishCalculation()
    {
        throw new NotImplementedException();
    }

    protected override string GetServiceName()
    {
        throw new NotImplementedException();
    }

    protected override void LoadInputData()
    {
        throw new NotImplementedException();
    }
}