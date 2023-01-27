using CalculateVessels.Core.Interfaces;
using CalculateVessels.Output.Word.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CalculateVessels.Output.Word.Helpers;

public class WordOutputElementFactory : IWordOutputElementFactory
{
    private readonly IServiceScope _scope;

    public WordOutputElementFactory(IServiceScopeFactory scopeFactory)
    {
        _scope = scopeFactory.CreateScope();
    }

    public IWordOutputElement<T>? Create<T>() 
        where T : class, ICalculatedElement
    {
        return _scope.ServiceProvider.GetService<IWordOutputElement<T>>();
    }

    public IWordOutputElement<T>? Create<T>(T type)
        where T : class, ICalculatedElement
    {
        return _scope.ServiceProvider.GetService<IWordOutputElement<T>>();
    }
}