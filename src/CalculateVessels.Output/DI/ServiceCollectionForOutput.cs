using CalculateVessels.Core.Shells.Cylindrical;
using CalculateVessels.Output.Word.DI;
using Microsoft.Extensions.DependencyInjection;

namespace CalculateVessels.Output.DI;

public class ServiceCollectionForOutput : IServiceCollectionForOutput
{
    public void RegisterDependencies(IServiceCollection services)
    {
        services.AddSingleton<IOutputService, OutputService>();

        var serviceCollectionForWordOutput = new ServiceCollectionForWordOutput();
        serviceCollectionForWordOutput.RegisterDependencies(services);
    }
}