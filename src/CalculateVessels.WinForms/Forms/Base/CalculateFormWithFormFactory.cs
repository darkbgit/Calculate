using System.Collections.Generic;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Helpers;
using FluentValidation;

namespace CalculateVessels.Forms.Base;

public abstract class CalculateFormWithFormFactory<T> : BaseCalculateForm<T>
    where T : class, IInputData
{
    protected CalculateFormWithFormFactory(IEnumerable<ICalculateService<T>> calculateServices,
        IPhysicalDataService physicalDataService,
        IValidator<T> validator,
        IFormFactory formFactory)
    : base(calculateServices, physicalDataService, validator)
    {
        FormFactory = formFactory;
    }

    protected IFormFactory FormFactory { get; }
}