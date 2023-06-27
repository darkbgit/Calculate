using System;
using System.Collections.Generic;
using CalculateVessels.Core.Elements.Shells.Elliptical;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Forms.Base;
using CalculateVessels.Helpers;
using FluentValidation;

namespace CalculateVessels.Forms.MiddleForms;

public class EllipticalShellFormMiddle : CalculateFormWithFormFactory<EllipticalShellInput>
{
    protected EllipticalShellFormMiddle(IEnumerable<ICalculateService<EllipticalShellInput>> calculateServices,
        IPhysicalDataService physicalDataService,
        IValidator<EllipticalShellInput> validator,
        IFormFactory formFactory)
        : base(calculateServices, physicalDataService, validator, formFactory)
    {

    }

    public EllipticalShellFormMiddle()
        : this(null!, null!, null!, null!)
    {

    }

    protected override string GetServiceName()
    {
        throw new NotImplementedException();
    }

    protected override void LoadInputData(EllipticalShellInput inputData)
    {
        throw new NotImplementedException();
    }

    protected override bool TryCollectInputData(out EllipticalShellInput inputData)
    {
        throw new NotImplementedException();
    }
}