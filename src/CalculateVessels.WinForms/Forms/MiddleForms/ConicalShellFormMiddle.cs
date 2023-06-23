using System;
using System.Collections.Generic;
using CalculateVessels.Core.Elements.Shells.Conical;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Forms.Base;
using CalculateVessels.Helpers;

namespace CalculateVessels.Forms.MiddleForms;

public class ConicalShellFormMiddle : CalculateFormWithFormFactory<ConicalShellInput>
{
    protected ConicalShellFormMiddle(IEnumerable<ICalculateService<ConicalShellInput>> calculateServices,
        IPhysicalDataService physicalDataService,
        IFormFactory formFactory)
        : base(calculateServices, physicalDataService, formFactory)
    {

    }

    public ConicalShellFormMiddle()
        : this(null, null, null)
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