using System;
using System.IO;
using CalculateVessels.Core.Bottoms.Enums;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Word;
using CalculateVessels.Core.Word.Enums;
using DocumentFormat.OpenXml.Packaging;

namespace CalculateVessels.Core.Bottoms.FlatBottom
{
    public class FlatBottomWordProvider : IWordProvider
    {
        public void MakeWord(string filePath, ICalculatedData calculatedData)
        {
            if (calculatedData is not FlatBottomCalculatedData data)
                throw new NullReferenceException();

            var dataIn = (FlatBottomInputData)data.InputData;

            if (string.IsNullOrWhiteSpace(filePath))
            {
                const string DEFAULT_FILE_NAME = "temp.docx";
                filePath = DEFAULT_FILE_NAME;
            }



            using WordprocessingDocument package = WordprocessingDocument.Open(filePath, true);

            var mainPart = package.MainDocumentPart;
            var body = mainPart?.Document.Body;

            if (body == null) return;

            body.AddParagraph($"Расчет на прочность плоской круглой крышки {dataIn.Name}")
                .Heading(HeadingType.Heading1)
                .Alignment(AlignmentType.Center);

            body.AddParagraph("");

            {
                var imagePart = mainPart.AddImagePart(ImagePartType.Gif);

                byte[] bytes = (byte[])Data.Properties.Resources.ResourceManager.GetObject("pldn" + dataIn.Type);

                imagePart.FeedData(new MemoryStream(bytes));

                body.AddParagraph("").AddImage(mainPart.GetIdOfPart(imagePart), bytes);
            }


            body.AddParagraph("Исходные данные").Alignment(AlignmentType.Center);

            //table
            {
                var table = body.AddTable();

                table.AddRow()
                    .AddCell("Марка стали")
                    .AddCell($"{dataIn.Steel}");

                table.AddRow()
                    .AddCell("Коэффициент прочности сварного шва, φ:")
                    .AddCell($"{dataIn.fi}");

                table.AddRow()
                    .AddCell("Прибавка на коррозию, ")
                    .AppendEquation("c_1")
                    .AppendText(":")
                    .AddCell($"{dataIn.c1} мм");

                table.AddRow()
                    .AddCell("Прибавка для компенсации минусового допуска, ")
                    .AppendEquation("c_2")
                    .AppendText(":")
                    .AddCell($"{dataIn.c2} мм");

                if (dataIn.c3 > 0)
                {
                    table.AddRow()
                        .AddCell("Технологическая прибавка, ")
                        .AppendEquation("c_3")
                        .AppendText(":")
                        .AddCell($"{dataIn.c3} мм");
                }

                table.AddRow()
                    .AddCell("Тип конструкции по ГОСТ 34233.2-2017 табл.4:")
                    .AddCell($"{dataIn.Type}");

                switch (dataIn.Type)
                {
                    case 1:
                    case 2:
                        table.AddRow()
                            .AddCell("Внутренний диаметр аппарата, D:")
                            .AddCell($"{dataIn.D} мм");

                        table.AddRow()
                            .AddCell("Толщина стенки аппарата, s:")
                            .AddCell($"{dataIn.s} мм");

                        table.AddRow()
                            .AddCell("Катет сварного шва, a:")
                            .AddCell($"{dataIn.a} мм");

                        break;
                    case 3:
                    case 4:
                    case 5:
                        table.AddRow()
                            .AddCell("Внутренний диаметр аппарата, D:")
                            .AddCell($"{dataIn.D} мм");

                        table.AddRow()
                            .AddCell("Толщина стенки аппарата, s:")
                            .AddCell($"{dataIn.s} мм");

                        break;
                    case 6:
                        goto case 2;
                    case 7:
                    case 8:
                        goto case 5;
                    case 9:
                        table.AddRow()
                            .AddCell("Внутренний диаметр аппарата, D:")
                            .AddCell($"{dataIn.D} мм");

                        table.AddRow()
                            .AddCell("Толщина стенки аппарата, s:")
                            .AddCell($"{dataIn.s} мм");

                        table.AddRow()
                            .AddCell("Радиус отбортовки, r:")
                            .AddCell($"{dataIn.r} мм");

                        table.AddRow()
                            .AddCell("Высота отбортовки, ")
                            .AppendEquation("h_1")
                            .AppendText(":")
                            .AddCell($"{dataIn.h1} мм");

                        break;
                    case 10:
                        table.AddRow()
                            .AddCell("Внутренний диаметр аппарата, D:")
                            .AddCell($"{dataIn.D} мм");

                        table.AddRow()
                            .AddCell("Толщина стенки аппарата, s:")
                            .AddCell($"{dataIn.s} мм");

                        table.AddRow()
                            .AddCell("Радиус выточки, r:")
                            .AddCell($"{dataIn.r} мм");

                        table.AddRow()
                            .AddCell("Высота отбортовки, ")
                            .AppendEquation("γ")
                            .AppendText(":")
                            .AddCell($"{dataIn.gamma} °");

                        table.AddRow()
                            .AddCell("Толщина крышки в зоне проточки, ")
                            .AppendEquation("s_2")
                            .AppendText(":")
                            .AddCell($"{dataIn.s2} мм");

                        break;

                    case 11:
                        table.AddRow()
                            .AddCell("Наименьший диаметр наружной утоненной части плоской крышки, ")
                            .AppendEquation("D_2")
                            .AppendText(":")
                            .AddCell($"{dataIn.D2} мм");

                        table.AddRow()
                            .AddCell("Диаметр болтовой окружности, ")
                            .AppendEquation("D_3")
                            .AppendText(":")
                            .AddCell($"{dataIn.D3} мм");

                        table.AddRow()
                            .AddCell("Толщина крышки в зоне уплотнения, ")
                            .AppendEquation("s_2")
                            .AppendText(":")
                            .AddCell($"{dataIn.s2} мм");

                        break;

                    case 12:
                        table.AddRow()
                            .AddCell("Наименьший диаметр наружной утоненной части плоской крышки, ")
                            .AppendEquation("D_2")
                            .AppendText(":")
                            .AddCell($"{dataIn.D2} мм");

                        table.AddRow()
                            .AddCell("Расчетный диаметр прокладки, ")
                            .AppendEquation("D_c.п")
                            .AppendText(":")
                            .AddCell($"{dataIn.Dcp} мм");

                        table.AddRow()
                            .AddCell("Толщина крышки в зоне уплотнения, ")
                            .AppendEquation("s_2")
                            .AppendText(":")
                            .AddCell($"{dataIn.s2} мм");

                        break;
                    case 13:
                    case 14:
                    case 15:
                        //TODO Make or delete flat bottom type
                        break;
                }

                table.AddRowWithOneCell("Условия нагружения");


                table.AddRow()
                    .AddCell("Расчетная температура, Т:")
                    .AddCell($"{dataIn.t} °С");

                table.AddRow()
                    .AddCell("Расчетное давление, p:")
                    .AddCell($"{dataIn.p} МПа");


                table.AddRowWithOneCell($"Характеристики материалов");

                table.AddRow()
                    .AddCell("Допускаемое напряжение при расчетной температуре, [σ]:")
                    .AddCell($"{data.SigmaAllow} МПа");

                body.InsertTable(table);
            }


            body.AddParagraph("");
            body.AddParagraph("Результаты расчета").Alignment(AlignmentType.Center);
            body.AddParagraph("");

            body.AddParagraph("Толщину плоской круглой крышки аппарата, работающего под внутренним избыточным давлением вычисляют по формуле");
            body.AddParagraph("").AppendEquation("s_1≥s_1p+c");
            body.AddParagraph("где ");
            body.AddParagraph("").AppendEquation("s_1p=K∙K_0∙D_p∙√(p/(φ∙[σ]))");

            body.AddParagraph("Коэффициент К в зависимости от конструкции днищ и крышек определяют по таблице 4 ГОСТ 34233.2-2017.");

            switch (dataIn.Type)
            {
                case 1:
                case 2:
                    body.AddParagraph("")
                        .AppendEquation($"D_p=D={data.Dp} мм, K={data.K}");
                    break;
                case 3:
                    body.AddParagraph("")
                        .AppendEquation($"D_p=D={data.Dp} мм");
                    body.AddParagraph("")
                        .AppendEquation($"при (s-c)/(s_1-c)=({dataIn.s}-{data.c})/({dataIn.s1}-{data.c})={(dataIn.s - data.c) / (dataIn.s1 - data.c):f2}" +
                        ((dataIn.s - data.c) / (dataIn.s1 - data.c) < 0.25 ? "<0.25" : "≥0.25") + $"  K={data.K}");
                    break;
                case 4:
                    body.AddParagraph("")
                        .AppendEquation($"D_p=D={data.Dp} мм");
                    body.AddParagraph("")
                        .AppendEquation($"при (s-c)/(s_1-c)=({dataIn.s}-{data.c})/({dataIn.s1}-{data.c})={(dataIn.s - data.c) / (dataIn.s1 - data.c):f2}" +
                        ((dataIn.s - data.c) / (dataIn.s1 - data.c) < 0.5 ? "<0.5" : "≥0.5") + $"  K={data.K}");
                    break;
                case 5:
                    goto case 3;
                case 6:
                    goto case 2;
                case 7:
                case 8:
                    goto case 4;
                case 9:
                    body.AddParagraph("")
                        .AppendEquation($"D_p=D-2∙r={data.Dp} мм");
                    body.AddParagraph("")
                        .AppendEquation($"K=max[0.41∙(1-0.23∙(s-c)/(s_1-c));0.35]={data.K}");
                    break;
                case 10:
                    goto case 4;
                case 11:
                    body.AddParagraph("")
                        .AppendEquation($"D_p=D_3={data.Dp} мм, K={data.K}");
                    break;
                case 12:
                    body.AddParagraph("")
                        .AppendEquation($"D_p=D_c.п={data.Dp} мм, K={data.K}");
                    break;
            }


            switch (dataIn.Hole)
            {
                case HoleInFlatBottom.WithoutHole:
                    body.AddParagraph("Коэффициент ")
                        .AppendEquation("K_0=1")
                        .AddRun(" - для крышек без отверстий.");
                    break;
                case HoleInFlatBottom.OneHole:
                    body.AddParagraph("Коэффициент ")
                       .AppendEquation("K_0")
                       .AddRun(" - для крышек, имеющих одно отверстие, вычисляют по формул");
                    body.AddParagraph("")
                        .AppendEquation("K_0=√(1+d/D_p+(d/D_p)^2)" +
                        $"=√(1+{dataIn.d}/{data.Dp:f2}+({dataIn.d}/{data.Dp:f2})^2)={data.K0:f2}");
                    break;
                case HoleInFlatBottom.MoreThenOneHole:
                    body.AddParagraph("Коэффициент ")
                       .AppendEquation("K_0")
                       .AddRun(" - для крышек, имеющих несколько отверстий, вычисляют по формул");
                    body.AddParagraph("")
                        .AppendEquation("K_0=√((1-(Σd_i/D_p)^3)/(1-(Σd_i/D_p)))" +
                        $"=√((1-({dataIn.di}/{data.Dp:f2})^3)/(1-({dataIn.di}/{data.Dp:f2})))={data.K0:f2}");
                    break;
            }


            body.AddParagraph("").AppendEquation($"s_1p={data.K:f2}∙{data.K0:f2}∙{data.Dp:f2}∙√({dataIn.p}/({dataIn.fi}∙{data.SigmaAllow}))={data.s1p:f2} мм");

            body.AddParagraph("c - сумма прибавок к расчетной толщине");
            body.AddParagraph("")
                .AppendEquation($"c=c_1+c_2+c_3={dataIn.c1}+{dataIn.c2}+{dataIn.c3}={data.c:f2} мм");

            body.AddParagraph("").AppendEquation($"s_1={data.s1p:f2}+{data.c:f2}={data.s1:f2} мм");

            if (dataIn.s1 > data.s1)
            {
                body.AddParagraph("Принятая толщина ")
                    .AppendEquation($"s_1={dataIn.s1} мм")
                    .Bold();
            }
            else
            {
                body.AddParagraph($"Принятая толщина ")
                    .AppendEquation($"s_1={dataIn.s1} мм")
                    .Bold()
                    .Color(System.Drawing.Color.Red);
            }

            if (dataIn.Type is 10 or 11 or 12)
            {
                body.AddParagraph("").AppendEquation("s_2≥s_2p+c");
                body.AddParagraph("где ");

                switch (dataIn.Type)
                {
                    case 10:
                        body.AddParagraph("")
                            .AppendEquation("s_2p=max{1.1∙(s-c);(s_1-c)/(1+(D_p-2∙r)/(1.2∙(s_1-c))∙sinγ)}");
                        body.AddParagraph("")
                            .AppendEquation($"1.1∙(s-c)=1.1∙({dataIn.s}-{data.c})={data.s2p_1:f2}");
                        body.AddParagraph("")
                            .AppendEquation($"(s_1-c)/(1+(D_p-2∙r)/(1.2∙(s_1-c))∙sinγ)=({dataIn.s1}-{data.c})/(1+({data.Dp}-2∙{dataIn.r})/(1.2∙({dataIn.s1}-{data.c}))∙sin{dataIn.gamma})={data.s2p_2:f2}");
                        break;
                    case 11:
                    case 12:
                        body.AddParagraph("")
                            .AppendEquation("s_2p=max{0.7∙(s_1-c);(s_1-c)∙√(2∙((D_p-D_2)∙D_2)/(D_p^2))}");
                        body.AddParagraph("")
                            .AppendEquation($"0.7∙(s_1-c)=0.7∙({dataIn.s1}-{data.c})={data.s2p_1:f2}");
                        body.AddParagraph("")
                            .AppendEquation($"(s_1-c)∙√(2∙((D_p-D_2)∙D_2)/(D_p^2))=({dataIn.s1}-{data.c})∙√(2∙(({data.Dp}-{dataIn.D2})∙{dataIn.D2})/({data.Dp}^2))={data.s2p_2:f2}");
                        break;
                }

                body.AddParagraph("").AppendEquation($"s_2={data.s2p:f2}+{data.c:f2}={data.s2:f2} мм");

                if (dataIn.s2 > data.s2)
                {
                    body.AddParagraph("Принятая толщина ")
                        .AppendEquation($"s_2={dataIn.s2} мм")
                        .Bold();
                }
                else
                {
                    body.AddParagraph($"Принятая толщина ")
                        .AppendEquation($"s_2={dataIn.s2} мм")
                        .Bold()
                        .Color(System.Drawing.Color.Red);
                }
            }

            body.AddParagraph("Допускаемое давление вычисляют по формуле:");
            body.AddParagraph("")
                .AppendEquation("[p]=((s_1-c)/(K∙K_0∙D_p))^2∙[σ]∙φ"
                                + $"=(({dataIn.s1}-{data.c:f2})/({data.K}∙{data.K0:f2}∙{data.Dp:f2}))^2∙{data.SigmaAllow}∙{dataIn.fi}"
                                + $"={data.p_d:f2} МПа");

            body.AddParagraph("Условия применения расчетных формул ");
            body.AddParagraph("")
                .AppendEquation($"(s_1-c)/D_p=({dataIn.s1}-{data.c:f2})/{data.Dp:f2}={data.ConditionUseFormulas:f2}≤0.11");
            if (data.IsConditionUseFormulas)
            {
                body.AddParagraph("Условие прочности");
                body.AddParagraph("").AppendEquation("[p]≥p");
                body.AddParagraph("")
                    .AppendEquation($"{data.p_d:f2}≥{dataIn.p}");
            }
            else
            {
                body.AddParagraph("Т.к. условие применения формул не выполняется, то условие прочности имеет вид");
                body.AddParagraph("").AppendEquation("K_p∙[p]≥p");
                body.AddParagraph("где ")
                        .AppendEquation("K_p")
                        .AddRun("  - поправочный коэффициент");
                body.AddParagraph("")
                        .AppendEquation("K_p=2.2/(1+√(1+(6∙(s_1-c)/D_p)^2))" +
                        $"=2.2/(1+√(1+(6∙({dataIn.s1}-{data.c:f2})/{data.Dp:f2})^2))={data.Kp:f2}");

                body.AddParagraph("")
                    .AppendEquation($"{data.Kp:f2}∙{data.p_d:f2}={data.Kp * data.p_d:f2}≥{dataIn.p}");
            }
            if (data.p_d * data.Kp > dataIn.p)
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


            if (dataIn.Type is 1 or 2 or 6 or 9 or 10)
            {
                body.AddParagraph("Условия закрепления");

                switch (dataIn.Type)
                {
                    case 1:
                        body.AddParagraph("")
                            .AppendEquation($"a≥1.7∙s={dataIn.a}≥1.7∙{dataIn.s}={1.7 * dataIn.s:f2}");
                        break;
                    case 2:
                    case 6:
                        body.AddParagraph("")
                            .AppendEquation($"a≥0.85∙s={dataIn.a}≥0.85∙{dataIn.s}={0.85 * dataIn.s:f2}");
                        break;
                    case 9:
                        body.AddParagraph("")
                            .AppendEquation("max{s;0.25∙s_1}≤r≤min{s_1;0.1∙D}");
                        body.AddParagraph("")
                            .AppendEquation(
                                $"{Math.Max(dataIn.s, 0.25 * dataIn.s1):f2}≤{dataIn.r}≤{Math.Min(dataIn.s1, 0.1 * dataIn.D):f2}");
                        body.AddParagraph("")
                            .AppendEquation($"h_1={dataIn.h1}≥r={dataIn.r}");
                        break;
                    case 10:
                        body.AddParagraph("")
                            .AppendEquation($"a≥1.7∙s={dataIn.a}≥1.7∙{dataIn.s}={1.7 * dataIn.s:f2}");
                        break;
                }
            }



            if (data.IsConditionFixed)
            {
                body.AddParagraph("Условие закрепления выполняется")
                    .Bold();
            }
            else
            {
                body.AddParagraph("Условие закрепления не выполняется")
                    .Bold()
                    .Color(System.Drawing.Color.Red);
            }

            package.Close();
        }
    }
}