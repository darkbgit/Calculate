using Microsoft.Extensions.DependencyInjection;

namespace CalculateVessels.Output.Word.DI;

public interface IServiceCollectionForWordOutput
{
    void RegisterDependencies(IServiceCollection services);
}