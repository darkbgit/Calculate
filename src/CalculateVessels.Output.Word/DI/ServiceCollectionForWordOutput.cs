using CalculateVessels.Core.Bottoms.FlatBottom;
using CalculateVessels.Core.Bottoms.FlatBottomWithAdditionalMoment;
using CalculateVessels.Core.Shells.Conical;
using CalculateVessels.Core.Shells.Cylindrical;
using CalculateVessels.Core.Shells.Elliptical;
using CalculateVessels.Core.Shells.Nozzle;
using CalculateVessels.Core.Supports.Saddle;
using CalculateVessels.Output.Interfaces;
using CalculateVessels.Output.Word.Core;
using CalculateVessels.Output.Word.Elements;
using CalculateVessels.Output.Word.Helpers;
using CalculateVessels.Output.Word.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CalculateVessels.Output.Word.DI;

public class ServiceCollectionForWordOutput : IServiceCollectionForWordOutput
{
    public void RegisterDependencies(IServiceCollection services)
    {
        services.AddScoped<IWordOutputService, WordOutputService>();

        services.AddScoped<IWordOutputElementFactory, WordOutputElementFactory>();

        services.AddScoped<IWordOutputElement<CylindricalShellCalculated>, CylindricalShellWordOutput>();
        services.AddScoped<IWordOutputElement<EllipticalShellCalculated>, EllipticalShellWordOutput>();
        services.AddScoped<IWordOutputElement<ConicalShellCalculated>, ConicalShellWordOutput>();
        services.AddScoped<IWordOutputElement<NozzleCalculated>, NozzleWordOutput>();

        services.AddScoped<IWordOutputElement<FlatBottomCalculated>, FlatBottomWordOutput>();
        services
            .AddScoped<IWordOutputElement<FlatBottomWithAdditionalMomentCalculated>,
                FlatBottomWithAdditionalMomentWordOutput>();
        services.AddScoped<IWordOutputElement<SaddleCalculated>, SaddleWordOutput>();
    }
}