using System;
using System.Collections.Generic;
using CalculateVessels.Core.Elements.HeatExchangers.HeatExchangerUTube;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Forms.Base;
using FluentValidation;

namespace CalculateVessels.Forms.MiddleForms;

public class HeatExchangerUTubesMiddleForm : BaseCalculateForm<HeatExchangerUTubeInput>
{
    protected HeatExchangerUTubesMiddleForm(IEnumerable<ICalculateService<HeatExchangerUTubeInput>> calculateServices,
        IPhysicalDataService physicalDataService,
        IValidator<HeatExchangerUTubeInput> validator)
        : base(calculateServices, physicalDataService, validator)
    {
    }

    public HeatExchangerUTubesMiddleForm()
        : this(null!, null!, null!)
    {

    }

    protected override string GetServiceName()
    {
        throw new NotImplementedException();
    }

    protected override void LoadInputData(HeatExchangerUTubeInput inputData)
    {
        throw new NotImplementedException();
    }

    protected override bool TryCollectInputData(out HeatExchangerUTubeInput inputData)
    {
        throw new NotImplementedException();
    }
}