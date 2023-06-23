using CalculateVessels.Data.Interfaces;

namespace CalculateVessels.Core.Elements.Base;

public abstract class CalculateService
{
    protected readonly IPhysicalDataService PhysicalData;

    protected CalculateService(IPhysicalDataService physicalData)
    {
        PhysicalData = physicalData;
    }
}