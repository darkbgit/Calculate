using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CalculateVessels.DataAccess.EF.DI;

public interface IServiceCollectionForData
{
    void RegisterDependencies(IConfiguration configuration, IServiceCollection services);
}