using System;
using System.Collections.Generic;
using CalculateVessels.Core.Elements.Shells.Conical;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Forms.Base;
using CalculateVessels.Helpers;
using FluentValidation;

namespace CalculateVessels.Forms.MiddleForms;

public class ConicalShellFormMiddle : CalculateFormWithFormFactory<ConicalShellInput>
{
    protected ConicalShellFormMiddle(IEnumerable<ICalculateService<ConicalShellInput>> calculateServices,
        IPhysicalDataService physicalDataService,
        IValidator<ConicalShellInput> validator,
        IFormFactory formFactory)
        : base(calculateServices, physicalDataService, validator, formFactory)
    {

    }

    public ConicalShellFormMiddle()
        : this(null!, null!, null!, null!)
    {

    }

    protected override string GetServiceName()
    {
        throw new NotImplementedException();
    }

    protected override void LoadInputData(ConicalShellInput inputData)
    {
        throw new NotImplementedException();
    }

    protected override bool TryCollectInputData(out ConicalShellInput inputData)
    {
        throw new NotImplementedException();
    }
}