using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Word;
using CalculateVessels.Core.Word.Enums;
using DocumentFormat.OpenXml.Packaging;

namespace CalculateVessels.Core.Shells.CylindricalShell
{
    internal class CylindricalShellWordProvider : IWordProvider
    {
        public void MakeWord(string filePath, ICalculatedData calculatedData)
        {
            if (calculatedData is not CylindricalShellCalculatedData data)
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

            body.AddParagraph($"Расчет на прочность обечайки {data.InputData.Name}, нагруженной " +
                              (data.InputData.IsPressureIn ? "внутренним избыточным давлением" : "наружным давлением")).Heading(HeadingType.Heading1);
            body.AddParagraph("");

            var imagePart = mainPart.AddImagePart(ImagePartType.Gif);

            byte[] bytes = Data.Properties.Resources.Cil;


            imagePart.FeedData(new MemoryStream(bytes));

            body.AddParagraph("").AddImage(mainPart.GetIdOfPart(imagePart), bytes);

            body.AddParagraph("Исходные данные").Alignment(AlignmentType.Center);

            //table
            {
                var table = body.AddTable();
                //table.SetWidths(new float[] { 300, 100 });
                //int i = 0;
                table.AddRow()
                    .AddCell("Материал обечайки")
                    .AddCell($"{data.InputData.Steel}");

                table.AddRow()
                    .AddCell("Внутренний диаметр обечайки, D:")
                    .AddCell($"{data.InputData.D} мм");

                if (!data.InputData.IsPressureIn)
                {
                    table.AddRow()
                        .AddCell("Длина обечайки, l:")
                        .AddCell($"{data.InputData.l} мм");
                }

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
                        .AddCell($"{data.InputData.c3} мм");
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
                    .AddCell("Расчетное " + (data.InputData.IsPressureIn ? "внутреннее избыточное" : "наружное")
                                          + " давление, p:")
                    .AddCell($"{data.InputData.p} МПа");

                table.AddRowWithOneCell($"Характеристики материала {data.InputData.Steel}");

                table.AddRow()
                    .AddCell("Допускаемое напряжение при расчетной температуре, [σ]:")
                    .AddCell($"{data.SigmaAllow} МПа");

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
            body.AddParagraph("").AppendEquation("s≥s_p+c");
            body.AddParagraph("где ").AppendEquation("s_p").AddRun(" - расчетная толщина стенки обечайки");

            if (data.InputData.IsPressureIn)
            {
                body.AddParagraph("")
                    .AppendEquation("s_p=(p∙D)/(2∙[σ]∙φ_p-p)" +
                                    $"=({data.InputData.p}∙{data.InputData.D})/(2∙{data.SigmaAllow}∙{data.InputData.fi}-{data.InputData.p})=" +
                                    $"{data.s_p:f2} мм");
            }
            else
            {
                body.AddParagraph("")
                    .AppendEquation("s_p=max{1.06∙(10^-2∙D)/(B)∙(p/(10^-5∙E)∙l/D)^0.4;(1.2∙p∙D)/(2∙[σ]-p)}");
                body.AddParagraph("Коэффициент B вычисляют по формуле:");
                body.AddParagraph("")
                    .AppendEquation("B=max{1;0.47∙(p/(10^-5∙E))^0.067∙(l/D)^0.4}");
                body.AddParagraph("")
                    .AppendEquation($"0.47∙({data.InputData.p}/(10^-5∙{data.E}))^0.067∙({data.l}/{data.InputData.D})^0.4={data.b_2:f2}");
                body.AddParagraph("")
                    .AppendEquation($"B=max(1;{data.b_2:f2})={data.b:f2}");
                body.AddParagraph("")
                    .AppendEquation($"1.06∙(10^-2∙{data.InputData.D})/({data.b:f2})∙({data.InputData.p}/(10^-5∙{data.E})∙{data.l}/{data.InputData.D})^0.4={data.s_p_1:f2}");
                body.AddParagraph("")
                    .AppendEquation($"(1.2∙{data.InputData.p}∙{data.InputData.D})/(2∙{data.SigmaAllow}-{data.InputData.p})={data.s_p_2:f2}");
                body.AddParagraph("")
                    .AppendEquation($"s_p=max({data.s_p_1:f2};{data.s_p_2:f2})={data.s_p:f2} мм");
            }

            body.AddParagraph("c - сумма прибавок к расчетной толщине");
            body.AddParagraph("")
                .AppendEquation($"c=c_1+c_2+c_3={data.InputData.c1}+{data.InputData.c2}+{data.InputData.c3}={data.c:f2} мм");

            body.AddParagraph("").AppendEquation($"s={data.s_p:f2}+{data.c:f2}={data.s:f2} мм");

            if (data.InputData.s > data.s)
            {
                body.AddParagraph($"Принятая толщина s={data.InputData.s} мм").Bold();
            }
            else
            {
                body.AddParagraph($"Принятая толщина s={data.InputData.s} мм")
                    .Bold()
                    .Color(System.Drawing.Color.Red);
            }
            if (data.InputData.IsPressureIn)
            {
                body.AddParagraph("Допускаемое внутреннее избыточное давление вычисляют по формуле:");
                body.AddParagraph("")
                    .AppendEquation("[p]=(2∙[σ]∙φ_p∙(s-c))/(D+s-c)"
                                    + $"=(2∙{data.SigmaAllow}∙{data.InputData.fi}∙({data.InputData.s}-{data.c:f2}))/"
                                    + $"({data.InputData.D}+{data.InputData.s}-{data.c:f2})={data.p_d:f2} МПа");
            }
            else
            {
                body.AddParagraph("Допускаемое наружное давление вычисляют по формуле:");
                body.AddParagraph("")
                    .AppendEquation("[p]=[p]_П/√(1+([p]_П/[p]_E)^2)");
                body.AddParagraph("допускаемое давление из условия прочности вычисляют по формуле:");
                body.AddParagraph("").AppendEquation("[p]_П=(2∙[σ]∙(s-c))/(D+s-c)" +
                                                     $"=(2∙{data.SigmaAllow}∙({data.InputData.s}-{data.c:f2}))/({data.InputData.D}+{data.InputData.s}-{data.c:f2})={data.p_dp:f2} МПа");
                body.AddParagraph("допускаемое давление из условия устойчивости в пределах упругости вычисляют по формуле:");
                body.AddParagraph("")
                    .AppendEquation("[p]_E=(2.08∙10^-5∙E)/(n_y∙B_1)∙D/l∙[(100∙(s-c))/D]^2.5");
                body.AddParagraph("коэффициент ")
                    .AppendEquation("B_1")
                    .AddRun(" вычисляют по формуле");
                body.AddParagraph("")
                    .AppendEquation("B_1=min{1;9.45∙D/l∙√(D/(100∙(s-c)))}");
                body.AddParagraph("")
                    .AppendEquation($"9.45∙{data.InputData.D}/{data.l}∙√({data.InputData.D}/(100∙({data.InputData.s}-{data.c:f2})))={data.B1_2:f2}");
                body.AddParagraph("")
                    .AppendEquation($"B_1=min(1;{data.B1_2:f2})={data.B1:f1}");
                body.AddParagraph("")
                    .AppendEquation($"[p]_E=(2.08∙10^-5∙{data.E})/({data.InputData.ny}∙{data.B1:f2})∙{data.InputData.D}/" +
                                    $"{data.l}∙[(100∙({data.InputData.s}-{data.c:f2}))/{data.InputData.D}]^2.5={data.p_de:f2} МПа");
                body.AddParagraph("")
                    .AppendEquation($"[p]={data.p_dp:f2}/√(1+({data.p_dp:f2}/{data.p_de:f2})^2)={data.p_d:f2} МПа");
            }

            body.AddParagraph("").AppendEquation("[p]≥p");
            body.AddParagraph("")
                .AppendEquation($"{data.p_d:f2}≥{data.InputData.p}");
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

            const int DIAMETER_BIG_LITTLE_BORDER = 200;
            body.AddParagraph("Условия применения расчетных формул ")
                .AddRun(data.InputData.D >= DIAMETER_BIG_LITTLE_BORDER ?
                    "при D ≥ 200 мм" : "при D < 200 мм");


            body.AddParagraph("")
                .AppendEquation(data.InputData.D >= DIAMETER_BIG_LITTLE_BORDER ?
                    $"(s-c)/(D)=({data.InputData.s}-{data.c:f2})/({data.InputData.D})={(data.InputData.s - data.c) / data.InputData.D:f3}≤0.1" :
                    $"(s-c)/(D)=({data.InputData.s}-{data.c:f2})/({data.InputData.D})={(data.InputData.s - data.c) / data.InputData.D:f3}≤0.3");

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
