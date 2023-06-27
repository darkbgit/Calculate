using System;
using System.Collections.Generic;
using CalculateVessels.Core.Elements.Bottoms.FlatBottomWithAdditionalMoment;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Forms.Base;
using FluentValidation;

namespace CalculateVessels.Forms.MiddleForms;

public class FlatBottomWithAdditionalMomentFormMiddle : BaseCalculateForm<FlatBottomWithAdditionalMomentInput>
{
    protected FlatBottomWithAdditionalMomentFormMiddle(IEnumerable<ICalculateService<FlatBottomWithAdditionalMomentInput>> calculateServices,
        IPhysicalDataService physicalDataService,
        IValidator<FlatBottomWithAdditionalMomentInput> validator)
        : base(calculateServices, physicalDataService, validator)
    {

    }

    public FlatBottomWithAdditionalMomentFormMiddle()
        : this(null!, null!, null!)
    {

    }

    protected override string GetServiceName()
    {
        throw new NotImplementedException();
    }

    protected override void LoadInputData(FlatBottomWithAdditionalMomentInput inputData)
    {
        throw new NotImplementedException();
    }

    protected override bool TryCollectInputData(out FlatBottomWithAdditionalMomentInput inputData)
    {
        throw new NotImplementedException();
    }
}