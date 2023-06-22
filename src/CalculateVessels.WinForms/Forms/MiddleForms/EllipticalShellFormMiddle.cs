using System;
using System.Collections.Generic;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Elliptical;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Forms.Base;
using CalculateVessels.Helpers;

namespace CalculateVessels.Forms.MiddleForms;

public class EllipticalShellFormMiddle : CalculateFormWithFormFactory<EllipticalShellInput>
{
    protected EllipticalShellFormMiddle(IEnumerable<ICalculateService<EllipticalShellInput>> calculateServices,
        IPhysicalDataService physicalDataService,
        IFormFactory formFactory)
        : base(calculateServices, physicalDataService, formFactory)
    {

    }

    public EllipticalShellFormMiddle()
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