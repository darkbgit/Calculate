using System;
using System.Collections.Generic;
using CalculateVessels.Core.Elements.Bottoms.FlatBottom;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Forms.Base;
using FluentValidation;

namespace CalculateVessels.Forms.MiddleForms;

public class FlatBottomFormMiddle : BaseCalculateForm<FlatBottomInput>
{
    protected FlatBottomFormMiddle(IEnumerable<ICalculateService<FlatBottomInput>> calculateServices,
        IPhysicalDataService physicalDataService,
        IValidator<FlatBottomInput> validator)
        : base(calculateServices, physicalDataService, validator)
    {

    }

    public FlatBottomFormMiddle()
        : this(null!, null!, null!)
    {

    }

    protected override string GetServiceName()
    {
        throw new NotImplementedException();
    }

    protected override void LoadInputData(FlatBottomInput inputData)
    {
        throw new NotImplementedException();
    }

    protected override bool TryCollectInputData(out FlatBottomInput inputData)
    {
        throw new NotImplementedException();
    }
}