using CalculateVessels.Data.Interfaces;
using CalculateVessels.Data.PhysicalData;
using Microsoft.Extensions.DependencyInjection;

namespace CalculateVessels.Data.DI;

public class ServiceCollectionForData : IServiceCollectionForData
{
    public void RegisterDependencies(IServiceCollection services)
    {
        services.AddSingleton<IPhysicalDataService, PhysicalDataService>();
    }
}