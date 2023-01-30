using Microsoft.Extensions.DependencyInjection;

namespace CalculateVessels.Core.DI;

public interface IServiceCollectionForCore
{
    void RegisterDependencies(IServiceCollection services);
}