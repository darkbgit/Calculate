using CalculateVessels.Core.Interfaces;
using CalculateVessels.Output.Interfaces;

namespace CalculateVessels.Output;

internal class OutputService : IOutputService
{
    private readonly IWordOutputService _wordOutputService;

    public OutputService(IWordOutputService wordOutputService)
    {
        _wordOutputService = wordOutputService;
    }

    public void Output(string outputName, OutputType outputType, IEnumerable<ICalculatedElement> elements)
    {
        switch (outputType)
        {
            case OutputType.Word:
                _wordOutputService.Output(outputName, elements);
                break;
            case OutputType.Pdf:
                throw new NotImplementedException();
            default:
                throw new NotImplementedException();
        }
    }
}