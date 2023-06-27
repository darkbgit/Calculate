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
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CalculateVessels.Core.DI;

public class ServiceCollectionForCore : IServiceCollectionForCore
{
    public void RegisterDependencies(IServiceCollection services)
    {
        services.AddScoped<ICalculateService<CylindricalShellInput>, CylindricalShellCalculateService>();
        services.AddScoped<IValidator<CylindricalShellInput>, CylindricalShellInputValidator>();

        services.AddScoped<ICalculateService<EllipticalShellInput>, EllipticalShellCalculateService>();
        services.AddScoped<IValidator<EllipticalShellInput>, EllipticalShellInputValidator>();

        services.AddScoped<ICalculateService<ConicalShellInput>, ConicalShellCalculateService>();
        services.AddScoped<IValidator<ConicalShellInput>, ConicalShellInputValidator>();

        services.AddScoped<ICalculateService<NozzleInput>, NozzleCalculateService>();
        services.AddScoped<IValidator<NozzleInput>, NozzleInputValidator>();

        services.AddScoped<ICalculateService<FlatBottomInput>, FlatBottomCalculateService>();
        services.AddScoped<IValidator<FlatBottomInput>, FlatBottomInputValidator>();

        services.AddScoped<ICalculateService<FlatBottomWithAdditionalMomentInput>,
                FlatBottomWithAdditionalMomentCalculateService>();
        services.AddScoped<IValidator<FlatBottomWithAdditionalMomentInput>,
            FlatBottomWithAdditionalMomentInputValidator>();

        services.AddScoped<ICalculateService<SaddleInput>, SaddleCalculateService>();
        services.AddScoped<IValidator<SaddleInput>, SaddleInputValidator>();

        services.AddScoped<ICalculateService<BracketVerticalInput>, BracketVerticalCalculateService>();
        services.AddScoped<IValidator<BracketVerticalInput>, BracketVerticalInputValidator>();

        services.AddScoped<ICalculateService<HeatExchangerStationaryTubePlatesInput>,
                HeatExchangerStationaryTubePlatesCalculateService>();
        services.AddScoped<IValidator<HeatExchangerStationaryTubePlatesInput>,
            HeatExchangerStationaryTubePlatesInputValidator>();

        services.AddSingleton<IPersistanceService, PersistanceServer>();
    }
}