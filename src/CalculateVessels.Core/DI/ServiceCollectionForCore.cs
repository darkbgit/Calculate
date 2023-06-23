using CalculateVessels.Core.Elements.Bottoms.FlatBottom;
using CalculateVessels.Core.Elements.Bottoms.FlatBottomWithAdditionalMoment;
using CalculateVessels.Core.Elements.HeatExchangers.HeatExchangerStationaryTubePlates;
using CalculateVessels.Core.Elements.Shells.Conical;
using CalculateVessels.Core.Elements.Shells.Cylindrical;
using CalculateVessels.Core.Elements.Shells.Elliptical;
using CalculateVessels.Core.Elements.Shells.Nozzle;
using CalculateVessels.Core.Elements.Supports.BracketVertical;
using CalculateVessels.Core.Elements.Supports.Saddle;
using CalculateVessels.Core.Interfaces;
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
        services
            .AddScoped<ICalculateService<HeatExchangerStationaryTubePlatesInput>,
                HeatExchangerStationaryTubePlatesCalculateService>();

        services.AddSingleton<IPersistanceService, PersistanceServer>();
    }
}