using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Nozzle;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Forms.Base;
using System;
using System.Collections.Generic;

namespace CalculateVessels.Forms.MiddleForms;

public class NozzleFormMiddle : BaseCalculateForm<NozzleInput>
{
    protected NozzleFormMiddle(IEnumerable<ICalculateService<NozzleInput>> calculateServices,
        IPhysicalDataService physicalDataService)
        : base(calculateServices, physicalDataService)
    {

    }

    public NozzleFormMiddle()
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