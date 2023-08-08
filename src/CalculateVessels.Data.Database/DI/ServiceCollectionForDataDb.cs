using CalculateVessels.Data.Public.Interfaces;
using CalculateVessels.DataAccess.EF.DI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CalculateVessels.Data.Database.DI;

public class ServiceCollectionForDataDb : IServiceCollectionForData
{
    public void RegisterDependencies(IConfiguration configuration, IServiceCollection services)
    {
        var serviceCollectionForData = new ServiceCollectionForDataAccess();
        serviceCollectionForData.RegisterDependencies(configuration, services);

        services.AddScoped<IPhysicalDataService, PhysicalDataService>();
    }
}