using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CalculateVessels.Data.Database.DI;

public interface IServiceCollectionForData
{
    void RegisterDependencies(IConfiguration configuration, IServiceCollection services);
}