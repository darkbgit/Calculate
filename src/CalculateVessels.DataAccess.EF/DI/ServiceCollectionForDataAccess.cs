using CalculateVessels.DataAccess.EF.PhysicalData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CalculateVessels.DataAccess.EF.DI;

public class ServiceCollectionForDataAccess : IServiceCollectionForData
{
    public void RegisterDependencies(IConfiguration configuration, IServiceCollection services)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<CalculateVesselsPhysicalDataContext>(options =>
            options.UseSqlServer(connectionString));
    }
}