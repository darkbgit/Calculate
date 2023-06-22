using CalculateVessels.Core.Interfaces;

namespace CalculateVessels.Output.Interfaces;

public interface IBaseOutputService
{
    void Output(string outputName, IEnumerable<ICalculatedElement> elements);
}