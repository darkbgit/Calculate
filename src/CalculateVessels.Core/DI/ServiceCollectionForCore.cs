using CalculateVessels.Core.Bottoms.FlatBottom;
using CalculateVessels.Core.Bottoms.FlatBottomWithAdditionalMoment;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Conical;
using CalculateVessels.Core.Shells.Cylindrical;
using CalculateVessels.Core.Shells.Elliptical;
using CalculateVessels.Core.Shells.Nozzle;
using CalculateVessels.Core.Supports.BracketVertical;
using CalculateVessels.Core.Supports.Saddle;
using Microsoft.Extensions.DependencyInjection;

namespace CalculateVessels.Core.DI;

public class ServiceCollectionForCore : IServiceCollectionForCore
{
    public void RegisterDependencies(IServiceCollection services)
    {
        services.AddScoped<ICalculateService<CylindricalShellInput>, CylindricalShellCalculateService>();
        services.AddScoped<ICalculateService<EllipticalShellInput>, EllipticalShellCalculateService>();
        services.AddScoped<ICalculateService<ConicalShellInput>, ConicalShellCalculateService>();
        services.AddScoped<ICalculateService<NozzleInput>, NozzleCalculateService>();

        services.AddScoped<ICalculateService<FlatBottomInput>, FlatBottomCalculateService>();
        services
            .AddScoped<ICalculateService<FlatBottomWithAdditionalMomentInput>,
                FlatBottomWithAdditionalMomentCalculateService>();
        services.AddScoped<ICalculateService<SaddleInput>, SaddleCalculateService>();
        services.AddScoped<ICalculateService<BracketVerticalInput>, BracketVerticalCalculateService>();
    }
}