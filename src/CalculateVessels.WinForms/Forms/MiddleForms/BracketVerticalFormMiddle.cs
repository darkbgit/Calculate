using System;
using System.Collections.Generic;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Supports.BracketVertical;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Forms.Base;

namespace CalculateVessels.Forms.MiddleForms;

public class BracketVerticalFormMiddle : BaseCalculateForm<BracketVerticalInput>
{
    protected BracketVerticalFormMiddle(IEnumerable<ICalculateService<BracketVerticalInput>> calculateServices, IPhysicalDataService physicalDataService)
        : base(calculateServices, physicalDataService)
    {
    }

    public BracketVerticalFormMiddle()
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