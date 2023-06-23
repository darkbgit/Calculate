using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Forms.Base;
using System.Collections.Generic;
using System;
using CalculateVessels.Helpers;
using CalculateVessels.Core.Elements.Shells.Cylindrical;

namespace CalculateVessels.Forms.MiddleForms;

public class CylindricalShellFormMiddle : CalculateFormWithFormFactory<CylindricalShellInput>
{
    protected CylindricalShellFormMiddle(IEnumerable<ICalculateService<CylindricalShellInput>> calculateServices,
        IPhysicalDataService physicalDataService,
        IFormFactory formFactory)
        : base(calculateServices, physicalDataService, formFactory)
    {

    }

    public CylindricalShellFormMiddle()
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