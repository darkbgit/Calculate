using System;
using System.Collections.Generic;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Supports.Saddle;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Forms.Base;

namespace CalculateVessels.Forms.MiddleForms;

public class SaddleFormMiddle : BaseCalculateForm<SaddleInput>
{
    protected SaddleFormMiddle(IEnumerable<ICalculateService<SaddleInput>> calculateServices,
        IPhysicalDataService physicalDataService)
        : base(calculateServices, physicalDataService)
    {
    }

    public SaddleFormMiddle()
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
}