using CalculateVessels.Core.Interfaces;

namespace CalculateVessels.Output.Word.Interfaces;

public interface IWordOutputElementFactory
{
    IWordOutputElement<T>? Create<T>()
     where T : class, ICalculatedElement;

    public IWordOutputElement<T>? Create<T>(T type)
        where T : class, ICalculatedElement;
}