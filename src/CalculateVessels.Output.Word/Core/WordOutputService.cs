using CalculateVessels.Core.Elements.Bottoms.FlatBottom;
using CalculateVessels.Core.Elements.HeatExchangers.HeatExchangerStationaryTubePlates;
using CalculateVessels.Core.Elements.Shells.Conical;
using CalculateVessels.Core.Elements.Shells.Cylindrical;
using CalculateVessels.Core.Elements.Shells.Elliptical;
using CalculateVessels.Core.Elements.Shells.Nozzle;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Output.Interfaces;
using CalculateVessels.Output.Word.Interfaces;

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


            dynamic? wordOutputService = element switch
            {
                CylindricalShellCalculated => _wordOutputElementFactory.Create<CylindricalShellCalculated>(),
                ConicalShellCalculated => _wordOutputElementFactory.Create<ConicalShellCalculated>(),
                EllipticalShellCalculated => _wordOutputElementFactory.Create<EllipticalShellCalculated>(),
                NozzleCalculated => _wordOutputElementFactory.Create<NozzleCalculated>(),
                HeatExchangerStationaryTubePlatesCalculated => _wordOutputElementFactory
                                                                   .Create<HeatExchangerStationaryTubePlatesCalculated>(),
                FlatBottomCalculated => _wordOutputElementFactory.Create<FlatBottomCalculated>(),
                _ => throw new Exception($"Type {element.GetType()} isn't supported."),
            };

            if (wordOutputService == null)
            {
                throw new NullReferenceException();
            }

            bibliography = bibliography.Union(element.Bibliography).ToList();

            wordOutputService.MakeWord(outputName, element);
        }

        Bibliography.Bibliography.AddBibliography(bibliography, outputName);
    }
}