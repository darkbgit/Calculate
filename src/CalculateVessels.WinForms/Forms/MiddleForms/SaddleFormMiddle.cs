using System;
using System.Collections.Generic;
using CalculateVessels.Core.Elements.Supports.Saddle;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Forms.Base;
using FluentValidation;

namespace CalculateVessels.Forms.MiddleForms;

public class SaddleFormMiddle : BaseCalculateForm<SaddleInput>
{
    protected SaddleFormMiddle(IEnumerable<ICalculateService<SaddleInput>> calculateServices,
        IPhysicalDataService physicalDataService,
        IValidator<SaddleInput> validator)
        : base(calculateServices, physicalDataService, validator)
    {
    }

    public SaddleFormMiddle()
        : this(null!, null!, null!)
    {

    }

    protected override string GetServiceName()
    {
        throw new NotImplementedException();
    }

    protected override void LoadInputData(SaddleInput inputData)
    {
        throw new NotImplementedException();
    }

    protected override bool TryCollectInputData(out SaddleInput inputData)
    {
        throw new NotImplementedException();
    }
}