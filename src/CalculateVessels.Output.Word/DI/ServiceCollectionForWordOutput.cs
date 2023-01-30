using CalculateVessels.Core.Shells.Cylindrical;
using CalculateVessels.Core.Shells.Elliptical;
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
    }
}