using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using CalculateVessels.Core.Word;
using CalculateVessels.Core.Word.Enums;

namespace CalculateVessels.Core.Bibliography
{
    public static class Bibliography
    {
        public static void AddBibliography(List<string> bibliography, string filename)
        {
            using WordprocessingDocument package = WordprocessingDocument.Open(filename, true);

            var mainPart = package.MainDocumentPart;
            var body = mainPart?.Document.Body;

            if (body == null) return;


            //body.AddParagraph("").InsertPageBreakAfterSelf();

            body.AddParagraph("Литература").Heading(HeadingType.Heading1).Alignment(AlignmentType.Center);

            body.AddParagraph("");

            bibliography.Sort();

            var i = 0;
            foreach (var biblio in bibliography)
            {
                body.AddParagraph($"{i++ + 1}. " 
                    + Data.Properties.Resources.ResourceManager.GetString(biblio));
            }

            package.Close();
        }

    }
}
