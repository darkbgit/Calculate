using Microsoft.Extensions.DependencyInjection;

namespace CalculateVessels.Data.DI;

public interface IServiceCollectionForData
{
    void RegisterDependencies(IServiceCollection services);
}