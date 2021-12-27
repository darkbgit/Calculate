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

namespace CalculateVessels.Core.Supports.BracketVertical
{
    public class BracketVerticalWordProvider : IWordProvider
    {

        public void MakeWord(string filePath, ICalculatedData calculatedData)
        {
            if (calculatedData is not BracketVerticalCalculatedData data)
                throw new NullReferenceException();

            if (string.IsNullOrWhiteSpace(filePath))
            {
                const string DEFAULT_FILE_NAME = "temp.docx";
                filePath = DEFAULT_FILE_NAME;
            }


            using var package = WordprocessingDocument.Open(filePath, true);

            var mainPart = package.MainDocumentPart;
            var body = mainPart?.Document.Body;

            if (body == null)
                throw new ArgumentException();

            body.AddParagraph($"Расчет на прочность обечайки {data.InputData.NameShell} от воздействия опорных нагрузок. Опорные лапы")
                .Heading(HeadingType.Heading1)
                .Alignment(AlignmentType.Center);

            body.AddParagraph("");

            var imagePart = mainPart.AddImagePart(ImagePartType.Gif);

            //TODO make picture

            byte[] bytes = data.InputData.Type switch
            {
                BracketVerticalType.A =>
                    Data.Properties.Resources.SaddleNothingElem,
                BracketVerticalType.B =>
                    Data.Properties.Resources.SaddleSheetElem,
                BracketVerticalType.C => null,
                BracketVerticalType.D => null,
                _ => null
            };

            if (bytes != null)
            {
                imagePart.FeedData(new MemoryStream(bytes));
                body.AddParagraph("").AddImage(mainPart.GetIdOfPart(imagePart), bytes);
            }

            body.AddParagraph("Исходные данные").Alignment(AlignmentType.Center);

            //table
            {
                var table = body.AddTable();

                table.AddRow()
                    .AddCell("Внутренний диаметр обечайки, D:")
                    .AddCell($"{data.InputData.D} мм");

                table.AddRow()
                    .AddCell("Толщина стенки обечайки, s:")
                    .AddCell($"{data.InputData.s} мм");

                table.AddRow()
                    .AddCell("Прибавка к расчетной толщине, c:")
                    .AddCell($"{data.InputData.c} мм");

                table.AddRow()
                    .AddCell("Коэффициент прочности сварного шва, φ:")
                    .AddCell($"{data.InputData.fi}");

                table.AddRow()
                    .AddCell("Материал обечайки")
                    .AddCell($"{data.InputData.Steel}");

                table.AddRow()
                    .AddCell("Количество опор")
                    .AddCell($"{data.InputData.n}");

                table.AddRow()
                    .AddCell("Ширина плиты основания опорной лапы, ")
                    .AppendEquation("b_4")
                    .AppendText(":")
                    .AddCell($"{data.InputData.b4} мм");

                table.AddRow()
                    .AddCell("Высота опорной лапы, ")
                    .AppendEquation("h_1")
                    .AppendText(":")
                    .AddCell($"{data.InputData.h1} мм");

                table.AddRow()
                    .AddCell("Расстояние между средними линиями ребер, g:")
                    .AddCell($"{data.InputData.g} мм");

                table.AddRow()
                    .AddCell("Длина опорной лапы, ")
                    .AppendEquation("l_1")
                    .AppendText(":")
                    .AddCell($"{data.InputData.l1} мм");

                if (data.InputData.e1 != 0)
                {
                    table.AddRow()
                        .AddCell("Расстояние между точкой приложения усилия и " +
                                 (data.InputData.ReinforceingPad ? " подкладным листом" : "обечайкой") + ", ")
                        .AppendEquation("e_1")
                        .AppendText(":")
                        .AddCell($"{data.e1} мм");
                }

                table.AddRow()
                    .AddCell("Подкладной лист")
                    .AddCell(data.InputData.ReinforceingPad ? "да" : "нет");

                if (data.InputData.ReinforceingPad)
                {
                    table.AddRow()
                        .AddCell("Толщина подкладного листа, ")
                        .AppendEquation("s_2")
                        .AppendText(":")
                        .AddCell($"{data.InputData.s2} мм");

                    table.AddRow()
                        .AddCell("Ширина подкладного листа, ")
                        .AppendEquation("b_2")
                        .AppendText(":")
                        .AddCell($"{data.InputData.b2} мм");

                    table.AddRow()
                        .AddCell("Длина подкладного листа, ")
                        .AppendEquation("b_3")
                        .AppendText(":")
                        .AddCell($"{data.InputData.b3} мм");
                }

                table.AddRowWithOneCell("Условия нагружения");

                table.AddRow()
                    .AddCell("Собственный вес с содержимым, G:")
                    .AddCell($"{data.InputData.G} H");

                table.AddRow()
                    .AddCell("Расчетный изгибающий момент, M:")
                    .AddCell($"{data.InputData.M} H∙мм");

                table.AddRow()
                    .AddCell("Расчетная поперечная сила, Q:")
                    .AddCell($"{data.InputData.Q} H");

                table.AddRow()
                    .AddCell("Расчетная температура, Т:")
                    .AddCell($"{data.InputData.t} °С");

                table.AddRow()
                    .AddCell("Расчетное давление, p:")
                    .AddCell($"{data.InputData.p} МПа");

                table.AddRow()
                    .AddCell($"Допускаемое напряжение для материала {data.InputData.Steel} при расчетной температуре, [σ]:")
                    .AddCell($"{data.SigmaAlloy} МПа");

                body.InsertTable(table);
            }

            body.AddParagraph("");
            body.AddParagraph("Результаты расчета").Alignment(AlignmentType.Center);
            body.AddParagraph("");


            body.AddParagraph("Расчет усилий").Alignment(AlignmentType.Center);
            body.AddParagraph("");

            if (data.InputData.e1 == 0)
            {
                body.AddParagraph("Если неизвестно точное значение расстояния между точкой приложения усилия и " +
                                  (data.InputData.ReinforceingPad ? "подкладным листом" : "обечайкой") + ", то ");

                body.AddParagraph("")
                    .AppendEquation("e_1=5/(6∙l_1H)" +
                                    $"=5/(6∙{data.InputData.l1})={data.e1:f2} мм");
            }

            body.AddParagraph("Вертикальное усилие, действующее на опорную лапу, вычисляют по формуле");

            switch (data.InputData.n)
            {
                case 4 when data.InputData.PreciseMontage:
                    body.AddParagraph("При n=4, обеспечивающем равномерное распределение нагрузки между всеми опорными лапами (точный монтаж, установка прокладок, подливка бетона и т. д.)");
                    body.AddParagraph("")
                        .AppendEquation("F_1=G/4+M/(D_p+2∙(e_1+s+s_2))" +
                                        $"={data.InputData.G}/4+{data.InputData.M}/({data.Dp}+2∙({data.e1:f2}+{data.InputData.s}+{data.InputData.s2}))={data.F1:f2} H");
                    break;
                case 2 or 4:
                    body.AddParagraph("При n=2 или n=4");
                    body.AddParagraph("")
                        .AppendEquation("F_1=G/2+M/(D_p+2∙(e_1+s+s_2))" +
                                        $"={data.InputData.G}/2+{data.InputData.M}/({data.Dp}+2∙({data.e1:f2}+{data.InputData.s}+{data.InputData.s2}))={data.F1:f2} H");
                    break;
                case 3:
                    body.AddParagraph("При n=3");
                    body.AddParagraph("")
                        .AppendEquation("F_1=G/3+M/(0.75∙[D_p+2∙(e_1+s+s_2)])" +
                                        $"={data.InputData.G}/3+{data.InputData.M}/([0.75∙{data.Dp}+2∙({data.e1:f2}+{data.InputData.s}+{data.InputData.s2})])={data.F1:f2} H");
                    break;
                default:
                    throw new ArgumentException();
            }

            body.AddParagraph("Горизонтальное усилие, действующее в основании опорной лапы или в основании стойки, в случае приварки к ней опорной лапы вычисляют по формуле");
            body.AddParagraph("")
                .AppendEquation("Q_1=Q/n" + $"={data.InputData.Q}/{data.InputData.n}={data.Q1:f2} H");


            body.AddParagraph("Эквивалентное плечо нагрузки ")
                .AppendEquation("e_1Э");
            body.AddParagraph("")
                .AppendEquation("e_1Э=e_1+Q_1∙h/F_1" + $"={data.InputData.e1}+{data.Q1:f2}∙{data.InputData.h}/{data.F1:f2}={data.e1e:f2} мм");

            if (!data.InputData.ReinforceingPad)
            {
                body.AddParagraph("Несущая способность обечайки в месте приварки опорной лапы без подкладного листа должна удовлетворять условию");
                body.AddParagraph("")
                    .AppendEquation("F_1≤[F]_1=([σ_i]∙h_1∙(s-c)^2)/(K_7∙e_1Э)");
                body.AddParagraph("Коэффициент ")
                    .AppendEquation("K_7")
                    .AddRun(" - вычисляют по формуле");


                switch (data.InputData.Type)
                {
                    case BracketVerticalType.A:
                    case BracketVerticalType.C:
                        body.AddParagraph("")
                            .AppendEquation("K_7=exp[(-5.964-11.395∙x-18.984∙y-2.413∙x^2-7.286∙x∙y-2.042∙y^2+0.1322∙x^3+0.4833∙х^2∙у+0.8469∙x∙y^2+1.428∙y^3)∙10^-2]");
                        break;
                    case BracketVerticalType.B:
                        body.AddParagraph("")
                            .AppendEquation("K_7=min{exp[(-26.791-6.936∙x-36.33∙y-3.503∙x^2-3.357∙x∙y+2.786∙y^2+0.2267∙x^3+0.2831∙x^2∙y+0.3851∙x∙y^2+1.37∙y^3)∙10^-2];exp[(-5.964-11.395∙x-18.984∙y-2.413∙x^2-7.286∙x∙y-2.042∙y^2+0.1322∙x^3+0.4833∙х^2∙у+0.8469∙x∙y^2+1.428∙y^3)∙10^-2];}");
                        break;
                    case BracketVerticalType.D:
                        body.AddParagraph("")
                            .AppendEquation("K_7=exp[(-29.532-45.958∙x-91.759∙z-1.801∙x^2-12.062∙x∙z-18.872∙z^2+0.1551∙x^3+1.617∙x^2∙z+3.736∙x∙z^2+1.425∙z^3)∙10^-2]");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                body.AddParagraph("где ");
                body.AddParagraph("")
                    .AppendEquation("x=ln(D_p/2∙(s-c))" + $"=ln({data.Dp}/2∙({data.InputData.s}-{data.InputData.c}))={data.x:f2}");

                switch (data.InputData.Type)
                {
                    case BracketVerticalType.A:
                    case BracketVerticalType.B:
                    case BracketVerticalType.C:
                        body.AddParagraph("")
                            .AppendEquation("y=ln(h_1/D_p)" + $"=ln({data.InputData.h1}/{data.Dp})={data.y:f2}");
                        break;
                    case BracketVerticalType.D:
                        body.AddParagraph("")
                            .AppendEquation("z=ln(b_4/D_p)" + $"=ln({data.InputData.b4}/{data.Dp})={data.z:f2}");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }


                body.AddParagraph("").AppendEquation($"K_7={data.K7:f2}");

                body.AddParagraph("предельные напряжения изгиба ")
                            .AppendEquation("[σ_i]")
                            .AddRun(" - вычисляют по формуле");

                body.AddParagraph("")
                    .AppendEquation("[σ_i]=K_1∙K_2∙[σ]");

                body.AddParagraph("")
                    .AppendEquation("K_1=(1-ϑ_2^2)/((1/3+ϑ_1∙ϑ_2)+√((1/3+ϑ_1∙ϑ_2)^2+(1-ϑ_2^2)∙ϑ_1^2))");

                body.AddParagraph("")
                    .AppendEquation($"K_2={data.K2}")
                    .AddRun(data.InputData.IsAssembly
                    ? " - для условий испытания и монтажа"
                    : " - для рабочих условий");

                body.AddParagraph("")
                    .AppendEquation("ϑ_1" + $"={data.v1}");

                body.AddParagraph("")
                    .AppendEquation("ϑ_2=σ_m/(K_2∙[σ]∙φ)");

                switch (data.InputData.Type)
                {
                    case BracketVerticalType.A:
                    case BracketVerticalType.B:
                    case BracketVerticalType.C:
                        body.AddParagraph("")
                            .AppendEquation("σ_m=σ_my=(p∙D_p)/(2∙(s-c))" + $"=({data.InputData.p}∙{data.Dp})/(2∙({data.InputData.s}-{data.InputData.c}))={data.sigma_m:f2}");
                        break;
                    case BracketVerticalType.D:
                        body.AddParagraph("")
                            .AppendEquation("σ_m=σ_mx=(p∙D_p)/(4∙(s-c))+1/(π∙D_p∙(s-c))∙(F+(4∙M)/D_p)" +
                                            $"=({data.InputData.p}∙{data.Dp})/(4∙({data.InputData.s}-{data.InputData.c}))+1/(π∙{data.Dp}∙({data.InputData.s}-{data.InputData.c}))∙({data.F1}+(4∙{data.InputData.M})/{data.Dp})={data.sigma_m:f2}");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                body.AddParagraph("")
                    .AppendEquation($"ϑ_2={data.sigma_m:f2}/({data.K2}∙{data.SigmaAlloy}∙{data.InputData.fi})={data.v2:f2}");

                body.AddParagraph("")
                    .AppendEquation($"K_1=(1-{data.v2:f2}^2)/((1/3+{data.v1:f2}∙{data.v2:f2})+√((1/3+{data.v1:f2}∙{data.v2:f2})^2+(1-{data.v2:f2}^2)∙{data.v1:f2}^2))={data.K1:f2}");

                body.AddParagraph("")
                    .AppendEquation($"[σ_i]={data.K1:f2}∙{data.K2}∙{data.SigmaAlloy}={data.sigmaid:f2}");


                body.AddParagraph("")
                    .AppendEquation($"[F]_1=({data.sigmaid:f2}∙{data.InputData.h1}∙({data.InputData.s}-{data.InputData.c})^2)/({data.K7:f2}∙{data.e1e:f2})={data.F1Alloy:f2}");
            }
            else
            {
                body.AddParagraph("Несущая способность обечайки в месте приварки опорной лапы с подкладным листом должна удовлетворять условию");
                body.AddParagraph("")
                    .AppendEquation("F_1≤[F]_1=([σ_i]∙b_3∙(s-c)^2)/(K_8∙(e_1Э+s_2))");
                body.AddParagraph("Коэффициент ")
                    .AppendEquation("K_8")
                    .AddRun(" - вычисляют по формуле");


                body.AddParagraph("")
                    .AppendEquation("K_8=min{exp[(-49.919-39.119∙x-107.01∙y_1-1.693∙x^2-11.92∙x∙y_1-39.276∙y_1^2+0.237∙x^3+1.608∙x^2∙y_1+2.761∙x∙y_1^2-3.854∙y_1^3)∙10^-2]; exp[(-5.964-11.395∙x-18.984∙y-2.413∙x^2-7.286∙x∙y-2.042∙y^2+0.1322∙x^3+0.4833∙х^2∙у+0.8469∙x∙y^2+1.428∙y^3)∙10^-2];}");



                body.AddParagraph("где ");
                body.AddParagraph("")
                    .AppendEquation("x=ln(D_p/2∙(s-c))" + $"=ln({data.Dp}/2∙({data.InputData.s}-{data.InputData.c}))={data.x:f2}");

                body.AddParagraph("")
                    .AppendEquation("y=ln(h_1/D_p)" + $"=ln({data.InputData.h1}/{data.Dp})={data.y:f2}");

                body.AddParagraph("")
                    .AppendEquation("y_1=ln(b_3/D_p)" + $"=ln({data.InputData.b3}/{data.Dp})={data.y1:f2}");

                body.AddParagraph("").AppendEquation($"K_8=min{{{data.K81:f2};{data.K82:f2}}}={data.K8:f2}");

                body.AddParagraph("предельные напряжения изгиба ")
                            .AppendEquation("[σ_i]")
                            .AddRun(" - вычисляют по формуле");

                body.AddParagraph("")
                    .AppendEquation("[σ_i]=K_1∙K_2∙[σ]");

                body.AddParagraph("")
                    .AppendEquation("K_1=(1-ϑ_2^2)/((1/3+ϑ_1∙ϑ_2)+√((1/3+ϑ_1∙ϑ_2)^2+(1-ϑ_2^2)∙ϑ_1^2))");

                body.AddParagraph("")
                    .AppendEquation($"K_2={data.K2}")
                    .AddRun(data.InputData.IsAssembly
                    ? " - для условий испытания и монтажа"
                    : " - для рабочих условий");

                body.AddParagraph("")
                    .AppendEquation("ϑ_1" + $"={data.v1}");

                body.AddParagraph("")
                    .AppendEquation("ϑ_2=σ_m/(K_2∙[σ]∙φ)");


                body.AddParagraph("")
                    .AppendEquation("σ_m=σ_my=(p∙D_p)/(2∙(s-c))" +
                                    $"=({data.InputData.p}∙{data.Dp})/(2∙({data.InputData.s}-{data.InputData.c}))={data.sigma_m:f2}");

                body.AddParagraph("")
                    .AppendEquation($"ϑ_2={data.sigma_m:f2}/({data.K2}∙{data.SigmaAlloy}∙{data.InputData.fi})={data.v2:f2}");

                body.AddParagraph("")
                    .AppendEquation($"K_1=(1-{data.v2:f2}^2)/((1/3+{data.v1:f2}∙{data.v2:f2})+√((1/3+{data.v1:f2}∙{data.v2:f2})^2+(1-{data.v2:f2}^2)∙{data.v1:f2}^2))={data.K1:f2}");

                body.AddParagraph("")
                    .AppendEquation($"[σ_i]={data.K1:f2}∙{data.K2}∙{data.SigmaAlloy}={data.sigmaid:f2}");


                body.AddParagraph("")
                    .AppendEquation($"[F]_1=({data.sigmaid:f2}∙{data.InputData.h1}∙({data.InputData.s}-{data.InputData.c})^2)/({data.K7:f2}∙{data.e1e:f2})={data.F1Alloy:f2}");
            }


            body.AddParagraph("")
                .AppendEquation($"{data.F1:f2}≤min{data.F1Alloy:f2}");

            if (data.F1 <= data.F1Alloy)
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

            body.AddParagraph("Условия применения расчетных формул ");

            if (data.IsConditionUseFormulas)
            {
                body.AddParagraph("Условия применения формул");
            }
            else
            {
                body.AddParagraph("Условия применения формул не выполняются").Bold().Color(System.Drawing.Color.Red);
            }
            body.AddParagraph("")
                .AppendEquation($"(s-c)/D=({data.InputData.s}-{data.InputData.c})/{data.Dp}={data.ConditionUseFormulas1:f3}≤0.05");

            body.AddParagraph("")
                .AppendEquation($"g={data.InputData.g}≥0.2∙h_1=0.2∙{data.InputData.h1}={data.ConditionUseFormulas2:f2}");

            body.AddParagraph("")
                .AppendEquation($"0.04≤h_1/D_p={data.InputData.h1}/{data.Dp}={data.ConditionUseFormulas3:f3}≤0.5");

            body.AddParagraph("")
                .AppendEquation($"0.04≤b_4/D_p={data.InputData.b4}/{data.Dp}={data.ConditionUseFormulas4:f3}≤0.5");

            if (data.InputData.ReinforceingPad)
            {
                body.AddParagraph("")
                    .AppendEquation($"0.04≤b_3/D_p={data.InputData.b3}/{data.Dp}={data.ConditionUseFormulas5:f3}≤0.8");

                body.AddParagraph("")
                    .AppendEquation($"b_2={data.InputData.b2}≥0.6∙b_3=0.6∙{data.InputData.b3}={data.ConditionUseFormulas6:f2}");

                body.AddParagraph("")
                    .AppendEquation($"b_3={data.InputData.b3}≤1.5∙h_1=1.5∙{data.InputData.h1}={data.ConditionUseFormulas7:f2}");

                body.AddParagraph("")
                    .AppendEquation($"s_2={data.InputData.s2}≥s={data.InputData.s}");
            }

            package.Close();
        }
    }
}
