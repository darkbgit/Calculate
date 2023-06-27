using System;
using System.Collections.Generic;
using CalculateVessels.Core.Elements.Shells.Nozzle;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Forms.Base;
using FluentValidation;

namespace CalculateVessels.Forms.MiddleForms;

public class NozzleFormMiddle : BaseCalculateForm<NozzleInput>
{
    protected NozzleFormMiddle(IEnumerable<ICalculateService<NozzleInput>> calculateServices,
        IPhysicalDataService physicalDataService,
        IValidator<NozzleInput> validator)
        : base(calculateServices, physicalDataService, validator)
    {

    }

    public NozzleFormMiddle()
        : this(null!, null!, null!)
    {

    }

    protected override string GetServiceName()
    {
        throw new NotImplementedException();
    }

    protected override void LoadInputData(NozzleInput inputData)
    {
        throw new NotImplementedException();
    }

    protected override bool TryCollectInputData(out NozzleInput inputData)
    {
        throw new NotImplementedException();
    }
}