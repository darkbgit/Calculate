using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Cylindrical;
using CalculateVessels.Core.Shells.Elliptical;
using CalculateVessels.Core.Shells.Nozzle;
using Microsoft.Extensions.DependencyInjection;

namespace CalculateVessels.Core.DI;

public class ServiceCollectionForCore : IServiceCollectionForCore
{
    public void RegisterDependencies(IServiceCollection services)
    {
        services.AddScoped<ICalculateService<CylindricalShellInput>, CylindricalShellCalculateService>();
        services.AddScoped<ICalculateService<EllipticalShellInput>, EllipticalShellCalculateService>();

        services.AddScoped<ICalculateService<NozzleInput>, NozzleCalculateService>();

    }
}