using CalculateVessels.Core.HeatExchangers.HeatExchangerStationaryTubePlates;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Conical;
using CalculateVessels.Core.Shells.Cylindrical;
using CalculateVessels.Core.Shells.Elliptical;
using CalculateVessels.Core.Shells.Nozzle;
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
            //dynamic typeInfo = Activator.CreateInstance(element.GetType())
            //                   ?? throw new InvalidOperationException();

            //var wordOutputService = _wordOutputElementFactory.Create(typeInfo)
            //                        ?? throw new NullReferenceException();


            dynamic wordOutputService = element switch
            {
                CylindricalShellCalculated => _wordOutputElementFactory.Create<CylindricalShellCalculated>()
                                              ?? throw new NullReferenceException(),
                ConicalShellCalculated => _wordOutputElementFactory.Create<ConicalShellCalculated>()
                                          ?? throw new NullReferenceException(),
                EllipticalShellCalculated => _wordOutputElementFactory.Create<EllipticalShellCalculated>()
                                             ?? throw new NullReferenceException(),
                NozzleCalculated => _wordOutputElementFactory.Create<NozzleCalculated>()
                                    ?? throw new NullReferenceException(),
                HeatExchangerStationaryTubePlatesCalculated => _wordOutputElementFactory
                                                                   .Create<
                                                                       HeatExchangerStationaryTubePlatesCalculated>()
                                                               ?? throw new NullReferenceException(),
                _ => throw new Exception($"Type {element.GetType()} isn't supported."),
            };

            bibliography = bibliography.Union(element.Bibliography).ToList();

            wordOutputService.MakeWord(outputName, element);
        }

        Bibliography.Bibliography.AddBibliography(bibliography, outputName);
    }
}