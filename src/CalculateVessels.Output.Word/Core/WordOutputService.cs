using CalculateVessels.Core.Interfaces;
using CalculateVessels.Output.Interfaces;
using CalculateVessels.Output.Word.Helpers;

namespace CalculateVessels.Output.Word.Core;

internal class WordOutputService : IWordOutputService
{
    private readonly IWordOutputElementFactory _wordOutputElementFactory;

    public WordOutputService(IWordOutputElementFactory wordOutputElementFactory)
    {
        _wordOutputElementFactory = wordOutputElementFactory;
    }

    public void Output(string outputName, IEnumerable<ICalculatedElement> elements)
    {
        var calculatedElements = elements as ICalculatedElement[] ?? elements.ToArray();

        if (!calculatedElements.Any())
        {
            throw new NotImplementedException();
        }

        var bibliography = new List<string>();

        foreach (var element in calculatedElements)
        {
            dynamic typeInfo = Activator.CreateInstance(element.GetType())
                               ?? throw new InvalidOperationException();

            var wordOutputService = _wordOutputElementFactory.Create(typeInfo)
                                    ?? throw new NullReferenceException();

            bibliography = bibliography.Union(element.Bibliography).ToList();

            wordOutputService.MakeWord(outputName, element);
        }

        Bibliography.Bibliography.AddBibliography(bibliography, outputName);
    }
}