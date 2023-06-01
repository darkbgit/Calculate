using System;
using System.Collections.Generic;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Conical;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Forms.Base;

namespace CalculateVessels.Forms;

public class ConicalShellFormMiddle : BaseCalculateForm<ConicalShellInput>
{
    protected ConicalShellFormMiddle(IEnumerable<ICalculateService<ConicalShellInput>> calculateServices, IPhysicalDataService physicalDataService)
        : base(calculateServices, physicalDataService)
    {

    }

    public ConicalShellFormMiddle()
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