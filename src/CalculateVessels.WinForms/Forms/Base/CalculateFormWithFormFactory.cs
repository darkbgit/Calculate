using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Helpers;
using System.Collections.Generic;

namespace CalculateVessels.Forms.Base;

public abstract class CalculateFormWithFormFactory<T> : BaseCalculateForm<T>
    where T : class, IInputData
{
    protected CalculateFormWithFormFactory(IEnumerable<ICalculateService<T>> calculateServices,
        IPhysicalDataService physicalDataService,
        IFormFactory formFactory)
    : base(calculateServices, physicalDataService)
    {
        FormFactory = formFactory;
    }

    protected IFormFactory FormFactory { get; }
}