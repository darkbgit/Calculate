using CalculateVessels.Output.Word.Core;
using CalculateVessels.Output.Word.Enums;
using DocumentFormat.OpenXml.Packaging;

namespace CalculateVessels.Output.Word.Bibliography;

internal static class Bibliography
{
    public static void AddBibliography(IEnumerable<string> bibliography, string filename)
    {
        using var package = WordprocessingDocument.Open(filename, true);

        var mainPart = package.MainDocumentPart;
        var body = mainPart?.Document.Body;

        if (body == null) return;

        //body.AddParagraph("").InsertPageBreakAfterSelf();

        body.AddParagraph("Литература")
            .Heading(HeadingType.Heading1)
            .Alignment(AlignmentType.Center);

        body.AddParagraph("");

        bibliography = bibliography.OrderBy(b => b);

        var i = 1;
        foreach (var biblio in bibliography)
        {
            body.AddParagraph($"{i++}. {biblio}.");
        }

        package.Close();
    }

}