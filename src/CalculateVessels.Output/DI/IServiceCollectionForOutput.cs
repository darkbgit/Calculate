using Microsoft.Extensions.DependencyInjection;

namespace CalculateVessels.Output.DI;

public interface IServiceCollectionForOutput
{
    void RegisterDependencies(IServiceCollection services);
}