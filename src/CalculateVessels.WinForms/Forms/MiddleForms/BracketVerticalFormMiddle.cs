using CalculateVessels.Core.Elements.Supports.BracketVertical;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Public.Interfaces;
using CalculateVessels.Forms.Base;
using FluentValidation;

namespace CalculateVessels.Forms.MiddleForms;

public class BracketVerticalFormMiddle : BaseCalculateForm<BracketVerticalInput>
{
    protected BracketVerticalFormMiddle(IEnumerable<ICalculateService<BracketVerticalInput>> calculateServices,
        IPhysicalDataService physicalDataService,
        IValidator<BracketVerticalInput> validator)
        : base(calculateServices, physicalDataService, validator)
    {
    }

    public BracketVerticalFormMiddle()
        : this(null!, null!, null!)
    {

    }

    protected override string GetServiceName()
    {
        throw new NotImplementedException();
    }

    protected override void LoadInputData(BracketVerticalInput inputData)
    {
        throw new NotImplementedException();
    }

    protected override bool TryCollectInputData(out BracketVerticalInput inputData)
    {
        throw new NotImplementedException();
    }
}