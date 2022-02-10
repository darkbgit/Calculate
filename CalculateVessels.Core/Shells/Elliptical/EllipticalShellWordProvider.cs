using System;
using System.IO;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Enums;
using CalculateVessels.Core.Word;
using CalculateVessels.Core.Word.Enums;
using DocumentFormat.OpenXml.Packaging;

namespace CalculateVessels.Core.Shells.Elliptical
{
    internal class EllipticalShellWordProvider : IWordProvider
    {
        public void MakeWord(string filePath, ICalculatedData calculatedData)
        {
            if (calculatedData is not EllipticalShellCalculatedData data)
                throw new NullReferenceException();

            if (string.IsNullOrWhiteSpace(filePath))
            {
                const string DEFAULT_FILE_NAME = "temp.docx";
                filePath = DEFAULT_FILE_NAME;
            }

            using var package = WordprocessingDocument.Open(filePath, true);

            var mainPart = package.MainDocumentPart;
            var body = mainPart?.Document.Body;

            if (body == null) return;

            // TODO: добавить полусферическое

            //body.AddParagraph("").InsertPageBreakAfterSelf();

            body.AddParagraph($"Расчет на прочность эллиптического днища {data.InputData.Name}, нагруженного " +
                (data.InputData.IsPressureIn ? "внутренним избыточным давлением" : "наружным давлением"))
                .Heading(HeadingType.Heading1)
                .Alignment(AlignmentType.Center);

            body.AddParagraph("");


            var imagePart = mainPart.AddImagePart(ImagePartType.Gif);

            byte[] bytes = Data.Properties.Resources.Ell;

            if (bytes != null)
            {
                imagePart.FeedData(new MemoryStream(bytes));

                body.AddParagraph("").AddImage(mainPart.GetIdOfPart(imagePart), bytes);
            }

            body.AddParagraph("Исходные данные")
                .Alignment(AlignmentType.Center);

            //table
            {
                var table = body.AddTable();

                table.AddRow()
                    .AddCell("Материал днища")
                    .AddCell($"{data.InputData.Steel}");

                table.AddRow()
                    .AddCell("Внутренний диаметр днища, D:")
                    .AddCell($"{data.InputData.D} мм");

                table.AddRow()
                    .AddCell("Высота выпуклой части, H:")
                    .AddCell($"{data.InputData.EllipseH} мм");

                table.AddRow()
                    .AddCell("Длина отбортовки ")
                    .AppendEquation("h_1")
                    .AppendText(":")
                    .AddCell($"{data.InputData.Ellipseh1}");

                table.AddRow()
                    .AddCell("Прибавка на коррозию, ")
                    .AppendEquation("c_1")
                    .AppendText(":")
                    .AddCell($"{data.InputData.c1} мм");

                table.AddRow()
                    .AddCell("Прибавка для компенсации минусового допуска, ")
                    .AppendEquation("c_2")
                    .AppendText(":")
                    .AddCell($"{data.InputData.c2} мм");

                if (data.InputData.c3 > 0)
                {
                    table.AddRow()
                        .AddCell("Технологическая прибавка, ")
                        .AppendEquation("c_3")
                        .AppendText(":")
                        .AddCell($"{data.InputData.c3}");
                }

                table.AddRow()
                    .AddCell("Коэффициент прочности сварного шва, ")
                    .AppendEquation("φ_p")
                    .AppendText(":")
                    .AddCell($"{data.InputData.fi}");

                table.AddRowWithOneCell("Условия нагружения");

                table.AddRow()
                    .AddCell("Расчетная температура, Т:")
                    .AddCell($"{data.InputData.t} °С");

                table.AddRow()
                    .AddCell(data.InputData.IsPressureIn ? "Расчетное внутреннее избыточное давление, p:"
                        : "Расчетное наружное давление, p:")
                    .AddCell($"{data.InputData.p} МПа");

                table.AddRowWithOneCell($"Характеристики материала сталь {data.InputData.Steel}");

                table.AddRow()
                    .AddCell("Допускаемое напряжение при расчетной температуре, [σ]:")
                    .AddCell($"{data.InputData.SigmaAllow} МПа");

                if (!data.InputData.IsPressureIn)
                {
                    table.AddRow()
                        .AddCell("Модуль продольной упругости при расчетной температуре, E:")
                        .AddCell($"{data.E} МПа");
                }

                body.InsertTable(table);
            }

            body.AddParagraph("");
            body.AddParagraph("Результаты расчета").Alignment(AlignmentType.Center);
            body.AddParagraph("");
            body.AddParagraph("Толщину стенки вычисляют по формуле:");
            body.AddParagraph("").AppendEquation("s_1≥s_1p+c");
            body.AddParagraph("где ")
                .AppendEquation("s_1p")
                .AddRun(" - расчетная толщина стенки днища");

            body.AddParagraph("")
                .AppendEquation(data.InputData.IsPressureIn
                ? "s_1p=(p∙R)/(2∙[σ]∙φ-0.5∙p)"
                : "s_1p=max{(K_Э∙R)/(161)∙√((n_y∙p)/(10^-5∙E));(1.2∙p∙R)/(2∙[σ])}");
            body.AddParagraph("где R - радиус кривизны в вершине днища");

            // TODO: добавить расчет R для разных ситуаций

            switch (data.InputData.EllipticalBottomType)
            {
                case EllipticalBottomType.Elliptical when Math.Abs(data.InputData.D - data.EllipseR) < 0.00001:
                    body.AddParagraph($"R=D={data.InputData.D} мм - для эллиптических днищ с H=0.25D");
                    break;
                case EllipticalBottomType.Hemispherical when Math.Abs(0.5 * data.InputData.D - data.EllipseR) < 0.00001:
                    body.AddParagraph("")
                        .AppendEquation($"R=0.5∙D={data.InputData.D} мм")
                        .AddRun(" - для полусферических днищ с H=0.5D");
                    break;
                default:
                    body.AddParagraph("")
                        .AppendEquation($"R=D^2/(4∙H)={data.InputData.D}^2/(4∙{data.InputData.EllipseH})={data.EllipseR:f2} мм");
                    break;
            }

            if (data.InputData.IsPressureIn)
            {
                body.AddParagraph("")
                    .AppendEquation($"s_p=({data.InputData.p}∙{data.EllipseR:f2})/(2∙{data.InputData.SigmaAllow}∙{data.InputData.fi}-0.5{data.InputData.p})={data.s_p:f2} мм");
            }
            else
            {
                body.AddParagraph("Для предварительного расчета ")
                    .AppendEquation($"К_Э={data.EllipseKePrev}")
                    .AddRun(data.InputData.EllipticalBottomType == EllipticalBottomType.Elliptical
                        ? " для эллиптических днищ"
                        : " для полусферических днищ");
                body.AddParagraph("")
                    .AppendEquation($"({data.EllipseKePrev}∙{data.EllipseR:f2})/(161)∙√(({data.InputData.ny}∙{data.InputData.p})/(10^-5∙{data.E}))=" +
                                                    $"{data.s_p_1:f2}");
                body.AddParagraph("").AppendEquation($"(1.2∙{data.InputData.p}∙{data.EllipseR:f2})/(2∙{data.InputData.SigmaAllow})={data.s_p_2:f2}");
                body.AddParagraph("").AppendEquation($"s_1p=max({data.s_p_1:f2};{data.s_p_2:f2})={data.s_p:f2} мм");
            }
            body.AddParagraph("c - сумма прибавок к расчетной толщине");
            body.AddParagraph("")
                .AppendEquation($"c=c_1+c_2+c_3={data.InputData.c1}+{data.InputData.c2}+{data.InputData.c3}={data.c:f2} мм");

            body.AddParagraph("").AppendEquation($"s={data.s_p:f2}+{data.c:f2}={data.s:f2} мм");

            if (data.InputData.s >= data.s)
            {
                body.AddParagraph("Принятая толщина ").Bold().AppendEquation($"s_1={data.InputData.s} мм");
            }
            else
            {
                body.AddParagraph("Принятая толщина ").Bold().Color(System.Drawing.Color.Red)
                    .AppendEquation($"s_1={data.InputData.s} мм");
            }

            if (data.InputData.IsPressureIn)
            {
                body.AddParagraph("Допускаемое внутреннее избыточное давление вычисляют по формуле:");
                body.AddParagraph("")
                    .AppendEquation("[p]=(2∙[σ]∙φ∙(s_1-c))/(R+0.5∙(s-c))" +
                                    $"=(2∙{data.InputData.SigmaAllow}∙{data.InputData.fi}∙({data.InputData.s}-{data.c:f2}))/" +
                                    $"({data.EllipseR:f2}+0.5∙({data.InputData.s}-{data.c:f2}))={data.p_d:f2} МПа");
            }
            else
            {
                body.AddParagraph("Допускаемое наружное давление вычисляют по формуле:");
                body.AddParagraph("")
                    .AppendEquation("[p]=[p]_П/√(1+([p]_П/[p]_E)^2)");
                body.AddParagraph("допускаемое давление из условия прочности вычисляют по формуле:");
                body.AddParagraph("")
                    .AppendEquation("[p]_П=(2∙[σ]∙(s_1-c))/(R+0.5(s_1-c))" +
                                    $"=(2∙{data.InputData.SigmaAllow}∙({data.InputData.s}-{data.c:f2}))/({data.EllipseR}+0.5({data.InputData.s}-{data.c:f2}))={data.p_dp:f2} МПа");
                body.AddParagraph("допускаемое давление из условия устойчивости в пределах упругости вычисляют по формуле:");
                body.AddParagraph("")
                    .AppendEquation("[p]_E=(2.6∙10^-5∙E)/n_y∙[(100∙(s_1-c))/(К_Э∙R)]^2");
                body.AddParagraph("коэффициент ")
                    .AppendEquation("К_Э")
                    .AddRun(" вычисляют по формуле");
                body.AddParagraph("")
                    .AppendEquation("К_Э=(1+(2.4+8∙x)∙x)/(1+(3+10∙x)∙x)");
                body.AddParagraph("")
                    .AppendEquation($"x=10∙(s_1-c)/D∙(D/(2∙H)-(2∙H)/D)=10∙({data.InputData.s - data.c:f2})/{data.InputData.D}∙({data.InputData.D}/(2∙{data.InputData.EllipseH})-(2∙{data.InputData.EllipseH})/{data.InputData.D})={data.Ellipsex:f2}");
                body.AddParagraph("")
                    .AppendEquation($"К_Э=(1+(2.4+8∙{data.Ellipsex})∙{data.Ellipsex})/(1+(3+10∙{data.Ellipsex})∙{data.Ellipsex}={data.EllipseKe:f2}");
                body.AddParagraph("")
                    .AppendEquation($"[p]_E=(2.6∙10^-5∙{data.E})/{data.InputData.ny}∙" +
                                    $"[(100∙({data.InputData.s}-{data.c:f2}))/({data.EllipseKe:f2}∙{data.EllipseR:f2})]^2={data.p_de:f2} МПа");
                body.AddParagraph("")
                    .AppendEquation($"[p]={data.p_dp:f2}/√(1+({data.p_dp:f2}/{data.p_de:f2})^2)={data.p_d:f2} МПа");
            }
            body.AddParagraph("").AppendEquation("[p]≥p");
            body.AddParagraph("").AppendEquation($"{data.p_d:f2}≥{data.InputData.p}");

            if (data.p_d > data.InputData.p)
            {
                body.AddParagraph("Условие прочности выполняется")
                    .Bold();
            }
            else
            {
                body.AddParagraph("Условие прочности не выполняется")
                    .Bold()
                    .Color(System.Drawing.Color.Red);
            }

            body.AddParagraph("Условия применения расчетных формул");

            // elliptical bottom
            body.AddParagraph("")
                .AppendEquation("0.002≤(s_1-c)/(D)" +
                                $"=({data.InputData.s}-{data.c:f2})/({data.InputData.D})={(data.InputData.s - data.c) / data.InputData.D:f3}≤0.1");
            body.AddParagraph("")
                .AppendEquation($"0.2≤H/D={data.InputData.EllipseH}/{data.InputData.D}={data.InputData.EllipseH / data.InputData.D:f3}<0.5");

            if (!data.IsConditionUseFormulas)
            {
                body.AddParagraph("Условия применения расчетных формул не выполняется ")
                    .Bold()
                    .Color(System.Drawing.Color.Red);
            }
            package.Close();
          
        }
    }
}
