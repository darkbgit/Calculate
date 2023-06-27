using System;
using System.Collections.Generic;
using CalculateVessels.Core.Elements.Shells.Cylindrical;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Forms.Base;
using CalculateVessels.Helpers;
using FluentValidation;

namespace CalculateVessels.Forms.MiddleForms;

public class CylindricalShellFormMiddle : CalculateFormWithFormFactory<CylindricalShellInput>
{
    protected CylindricalShellFormMiddle(IEnumerable<ICalculateService<CylindricalShellInput>> calculateServices,
        IPhysicalDataService physicalDataService,
        IValidator<CylindricalShellInput> validator,
        IFormFactory formFactory)
        : base(calculateServices, physicalDataService, validator, formFactory)
    {

    }

    public CylindricalShellFormMiddle()
        : this(null!, null!, null!, null!)
    {

    }

    protected override string GetServiceName()
    {
        throw new NotImplementedException();
    }

    protected override void LoadInputData(CylindricalShellInput inputData)
    {
        throw new NotImplementedException();
    }

    protected override bool TryCollectInputData(out CylindricalShellInput inputData)
    {
        throw new NotImplementedException();
    }
}