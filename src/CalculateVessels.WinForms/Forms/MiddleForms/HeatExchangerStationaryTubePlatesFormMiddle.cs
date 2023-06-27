using System;
using System.Collections.Generic;
using CalculateVessels.Core.Elements.HeatExchangers.HeatExchangerStationaryTubePlates;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Forms.Base;
using FluentValidation;

namespace CalculateVessels.Forms.MiddleForms;

public class HeatExchangerStationaryTubePlatesFormMiddle : BaseCalculateForm<HeatExchangerStationaryTubePlatesInput>
{
    protected HeatExchangerStationaryTubePlatesFormMiddle(IEnumerable<ICalculateService<HeatExchangerStationaryTubePlatesInput>> calculateServices,
        IPhysicalDataService physicalDataService,
            IValidator<HeatExchangerStationaryTubePlatesInput> validator)
        : base(calculateServices, physicalDataService, validator)
    {
    }

    public HeatExchangerStationaryTubePlatesFormMiddle()
        : this(null!, null!, null!)
    {

    }

    protected override string GetServiceName()
    {
        throw new NotImplementedException();
    }

    protected override void LoadInputData(HeatExchangerStationaryTubePlatesInput inputData)
    {
        throw new NotImplementedException();
    }

    protected override bool TryCollectInputData(out HeatExchangerStationaryTubePlatesInput inputData)
    {
        throw new NotImplementedException();
    }
}