using CalculateVessels.Core.Interfaces;

namespace CalculateVessels.Output.Word.Interfaces;

public interface IWordOutputElement<T>
    where T : class, ICalculatedElement
{
    void MakeWord(string filePath, ICalculatedElement calculatedElement);
}