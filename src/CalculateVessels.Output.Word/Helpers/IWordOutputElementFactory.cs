using CalculateVessels.Core.Interfaces;
using CalculateVessels.Output.Word.Interfaces;

namespace CalculateVessels.Output.Word.Helpers;

public interface IWordOutputElementFactory
{
    IWordOutputElement<T>? Create<T>()
     where T : class, ICalculatedElement;

    public IWordOutputElement<T>? Create<T>(T type)
        where T : class, ICalculatedElement;
}