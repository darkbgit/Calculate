using CalculateVessels.Core.Interfaces;

namespace CalculateVessels.Output;

public interface IOutputService
{
    void Output(string outputName, OutputType outputType, IEnumerable<ICalculatedElement> elements);
}