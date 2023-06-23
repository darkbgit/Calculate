using CalculateVessels.Core.Elements.Supports.BracketVertical;
using CalculateVessels.Core.Enums;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Properties;
using CalculateVessels.Output.Word.Core;
using CalculateVessels.Output.Word.Enums;
using CalculateVessels.Output.Word.Interfaces;
using DocumentFormat.OpenXml.Packaging;

namespace CalculateVessels.Output.Word.Elements;

internal class BracketVerticalWordOutput : IWordOutputElement<BracketVerticalCalculated>
{
    public void MakeWord(string filePath, ICalculatedElement calculatedData)
    {
        if (calculatedData is not BracketVerticalCalculated data)
            throw new InvalidOperationException();

        var dataIn = (BracketVerticalInput)data.InputData;

        if (string.IsNullOrWhiteSpace(filePath))
        {
            const string defaultFileName = "temp.docx";
            filePath = defaultFileName;
        }

        using var package = WordprocessingDocument.Open(filePath, true);

        var mainPart = package.MainDocumentPart ?? throw new InvalidOperationException();
        var body = mainPart.Document.Body ?? throw new InvalidOperationException();

        body.AddParagraph($"Расчет на прочность обечайки {dataIn.NameShell} от воздействия опорных нагрузок. Опорные лапы")
            .Heading(HeadingType.Heading1)
            .Alignment(AlignmentType.Center);

        body.AddParagraph();

        var imagePart = mainPart.AddImagePart(ImagePartType.Gif);

        //TODO make picture

        const string bracketVertical = "BracketVertical_";

        var resourceName = bracketVertical + dataIn.Type.ToString() +
                           (dataIn.ReinforcingPad ? "" : "_1");

        var bytes = (byte[])(Resources.ResourceManager.GetObject(resourceName)
                             ?? throw new InvalidOperationException());

        imagePart.FeedData(new MemoryStream(bytes));
        body.AddParagraph().AddImage(mainPart.GetIdOfPart(imagePart), bytes);

        body.AddParagraph("Исходные данные").Alignment(AlignmentType.Center);

        //table
        {
            var table = body.AddTable();

            table.AddRow()
                .AddCell("Внутренний диаметр обечайки, D:")
                .AddCell($"{dataIn.D} мм");

            table.AddRow()
                .AddCell("Толщина стенки обечайки, s:")
                .AddCell($"{dataIn.s} мм");

            table.AddRow()
                .AddCell("Прибавка к расчетной толщине, c:")
                .AddCell($"{dataIn.c} мм");

            table.AddRow()
                .AddCell("Коэффициент прочности сварного шва, φ:")
                .AddCell($"{dataIn.phi}");

            table.AddRow()
                .AddCell("Материал обечайки")
                .AddCell($"{dataIn.Steel}");

            table.AddRow()
                .AddCell("Количество опор")
                .AddCell($"{dataIn.n}");

            table.AddRow()
                .AddCell("Ширина плиты основания опорной лапы, ")
                .AppendEquation("b_4")
                .AppendText(":")
                .AddCell($"{dataIn.b4} мм");

            table.AddRow()
                .AddCell("Высота опорной лапы, ")
                .AppendEquation("h_1")
                .AppendText(":")
                .AddCell($"{dataIn.h1} мм");

            table.AddRow()
                .AddCell("Расстояние между средними линиями ребер, g:")
                .AddCell($"{dataIn.g} мм");

            table.AddRow()
                .AddCell("Длина опорной лапы, ")
                .AppendEquation("l_1")
                .AppendText(":")
                .AddCell($"{dataIn.l1} мм");

            if (dataIn.e1 != 0)
            {
                table.AddRow()
                    .AddCell("Расстояние между точкой приложения усилия и " +
                             (dataIn.ReinforcingPad ? " подкладным листом" : "обечайкой") + ", ")
                    .AppendEquation("e_1")
                    .AppendText(":")
                    .AddCell($"{data.e1} мм");
            }

            if (dataIn.h != 0)
            {
                table.AddRow()
                    .AddCell("Расстояние от центра тяжести опорной лапы, приваренной к стойке, до верхнего обреза фундамента, h:")
                    .AddCell($"{dataIn.h} мм"); ;
            }

            table.AddRow()
                .AddCell("Подкладной лист")
                .AddCell(dataIn.ReinforcingPad ? "да" : "нет");

            if (dataIn.ReinforcingPad)
            {
                table.AddRow()
                    .AddCell("Толщина подкладного листа, ")
                    .AppendEquation("s_2")
                    .AppendText(":")
                    .AddCell($"{dataIn.s2} мм");

                table.AddRow()
                    .AddCell("Ширина подкладного листа, ")
                    .AppendEquation("b_2")
                    .AppendText(":")
                    .AddCell($"{dataIn.b2} мм");

                table.AddRow()
                    .AddCell("Длина подкладного листа, ")
                    .AppendEquation("b_3")
                    .AppendText(":")
                    .AddCell($"{dataIn.b3} мм");
            }

            table.AddRowWithOneCell("Условия нагружения");

            table.AddRow()
                .AddCell("Собственный вес с содержимым, G:")
                .AddCell($"{dataIn.G} H");

            table.AddRow()
                .AddCell("Расчетный изгибающий момент, M:")
                .AddCell($"{dataIn.M} H∙мм");

            table.AddRow()
                .AddCell("Расчетная поперечная сила, Q:")
                .AddCell($"{dataIn.Q} H");

            table.AddRow()
                .AddCell("Расчетная температура, Т:")
                .AddCell($"{dataIn.t} °С");

            table.AddRow()
                .AddCell("Расчетное давление, p:")
                .AddCell($"{dataIn.p} МПа");

            table.AddRow()
                .AddCell($"Допускаемое напряжение для материала {dataIn.Steel} при расчетной температуре, [σ]:")
                .AddCell($"{data.SigmaAllow} МПа");

            body.InsertTable(table);
        }

        body.AddParagraph();
        body.AddParagraph("Результаты расчета").Alignment(AlignmentType.Center);
        body.AddParagraph();


        body.AddParagraph("Расчет усилий").Alignment(AlignmentType.Center);
        body.AddParagraph();

        if (dataIn.e1 == 0)
        {
            body.AddParagraph("Если неизвестно точное значение расстояния между точкой приложения усилия и " +
                              (dataIn.ReinforcingPad ? "подкладным листом" : "обечайкой") + ", то ");

            body.AddParagraph()
                .AppendEquation("e_1=5/6∙l_1" +
                                $"=5/6∙{dataIn.l1}={data.e1:f2} мм");
        }

        body.AddParagraph("Вертикальное усилие, действующее на опорную лапу, вычисляют по формуле");

        switch (dataIn.n)
        {
            case 4 when dataIn.PreciseMontage:
                body.AddParagraph("При n=4, обеспечивающем равномерное распределение нагрузки между всеми опорными лапами (точный монтаж, установка прокладок, подливка бетона и т. д.)");
                body.AddParagraph()
                    .AppendEquation("F_1=G/4+M/(D_p+2∙(e_1+s+s_2))" +
                                    $"={dataIn.G}/4+{dataIn.M}/({data.Dp}+2∙({data.e1:f2}+{dataIn.s}+{dataIn.s2}))={data.F1:f2} H");
                break;
            case 2 or 4:
                body.AddParagraph("При n=2 или n=4");
                body.AddParagraph()
                    .AppendEquation("F_1=G/2+M/(D_p+2∙(e_1+s+s_2))" +
                                    $"={dataIn.G}/2+{dataIn.M}/({data.Dp}+2∙({data.e1:f2}+{dataIn.s}+{dataIn.s2}))={data.F1:f2} H");
                break;
            case 3:
                body.AddParagraph("При n=3");
                body.AddParagraph()
                    .AppendEquation("F_1=G/3+M/(0.75∙[D_p+2∙(e_1+s+s_2)])" +
                                    $"={dataIn.G}/3+{dataIn.M}/([0.75∙{data.Dp}+2∙({data.e1:f2}+{dataIn.s}+{dataIn.s2})])={data.F1:f2} H");
                break;
            default:
                throw new ArgumentException();
        }

        body.AddParagraph("Горизонтальное усилие, действующее в основании опорной лапы или в основании стойки, в случае приварки к ней опорной лапы вычисляют по формуле");
        body.AddParagraph()
            .AppendEquation("Q_1=Q/n" + $"={dataIn.Q}/{dataIn.n}={data.Q1:f2} H");


        body.AddParagraph("Эквивалентное плечо нагрузки ")
            .AppendEquation("e_1Э");
        body.AddParagraph()
            .AppendEquation("e_1Э=e_1+Q_1∙h/F_1" + $"={dataIn.e1}+{data.Q1:f2}∙{dataIn.h}/{data.F1:f2}={data.e1e:f2} мм");

        if (!dataIn.ReinforcingPad)
        {
            body.AddParagraph("Несущая способность обечайки в месте приварки опорной лапы без подкладного листа должна удовлетворять условию");
            body.AddParagraph()
                .AppendEquation("F_1≤[F]_1=([σ_i]∙h_1∙(s-c)^2)/(K_7∙e_1Э)");
            body.AddParagraph("Коэффициент ")
                .AppendEquation("K_7")
                .AddRun(" - вычисляют по формуле");


            switch (dataIn.Type)
            {
                case BracketVerticalType.A:
                case BracketVerticalType.C:
                    body.AddParagraph()
                        .AppendEquation("K_7=exp[(-5.964-11.395∙x-18.984∙y-2.413∙x^2-7.286∙x∙y-2.042∙y^2+0.1322∙x^3+0.4833∙х^2∙у+0.8469∙x∙y^2+1.428∙y^3)∙10^-2]");
                    break;
                case BracketVerticalType.B:
                    body.AddParagraph()
                        .AppendEquation("K_7=min{exp[(-26.791-6.936∙x-36.33∙y-3.503∙x^2-3.357∙x∙y+2.786∙y^2+0.2267∙x^3+0.2831∙x^2∙y+0.3851∙x∙y^2+1.37∙y^3)∙10^-2];exp[(-5.964-11.395∙x-18.984∙y-2.413∙x^2-7.286∙x∙y-2.042∙y^2+0.1322∙x^3+0.4833∙х^2∙у+0.8469∙x∙y^2+1.428∙y^3)∙10^-2];}");
                    break;
                case BracketVerticalType.D:
                    body.AddParagraph()
                        .AppendEquation("K_7=exp[(-29.532-45.958∙x-91.759∙z-1.801∙x^2-12.062∙x∙z-18.872∙z^2+0.1551∙x^3+1.617∙x^2∙z+3.736∙x∙z^2+1.425∙z^3)∙10^-2]");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            body.AddParagraph("где ");
            body.AddParagraph()
                .AppendEquation("x=ln(D_p/2∙(s-c))" + $"=ln({data.Dp}/2∙({dataIn.s}-{dataIn.c}))={data.x:f2}");

            switch (dataIn.Type)
            {
                case BracketVerticalType.A:
                case BracketVerticalType.B:
                case BracketVerticalType.C:
                    body.AddParagraph()
                        .AppendEquation("y=ln(h_1/D_p)" + $"=ln({dataIn.h1}/{data.Dp})={data.y:f2}");
                    break;
                case BracketVerticalType.D:
                    body.AddParagraph()
                        .AppendEquation("z=ln(b_4/D_p)" + $"=ln({dataIn.b4}/{data.Dp})={data.z:f2}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            body.AddParagraph().AppendEquation($"K_7={data.K7:f2}");

            body.AddParagraph("предельные напряжения изгиба ")
                .AppendEquation("[σ_i]")
                .AddRun(" - вычисляют по формуле");

            body.AddParagraph()
                .AppendEquation("[σ_i]=K_1∙K_2∙[σ]");

            body.AddParagraph()
                .AppendEquation("K_1=(1-ϑ_2^2)/((1/3+ϑ_1∙ϑ_2)+√((1/3+ϑ_1∙ϑ_2)^2+(1-ϑ_2^2)∙ϑ_1^2))");

            body.AddParagraph()
                .AppendEquation($"K_2={data.K2}")
                .AddRun(dataIn.IsAssembly
                    ? " - для условий испытания и монтажа"
                    : " - для рабочих условий");

            body.AddParagraph()
                .AppendEquation("ϑ_1" + $"={data.v1}");

            body.AddParagraph()
                .AppendEquation("ϑ_2=σ_m/(K_2∙[σ]∙φ)");

            switch (dataIn.Type)
            {
                case BracketVerticalType.A:
                case BracketVerticalType.B:
                case BracketVerticalType.C:
                    body.AddParagraph()
                        .AppendEquation("σ_m=σ_my=(p∙D_p)/(2∙(s-c))" + $"=({dataIn.p}∙{data.Dp})/(2∙({dataIn.s}-{dataIn.c}))={data.sigma_m:f2}");
                    break;
                case BracketVerticalType.D:
                    body.AddParagraph()
                        .AppendEquation("σ_m=σ_mx=(p∙D_p)/(4∙(s-c))+1/(π∙D_p∙(s-c))∙(F+(4∙M)/D_p)" +
                                        $"=({dataIn.p}∙{data.Dp})/(4∙({dataIn.s}-{dataIn.c}))+1/(π∙{data.Dp}∙({dataIn.s}-{dataIn.c}))∙({data.F1}+(4∙{dataIn.M})/{data.Dp})={data.sigma_m:f2}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            body.AddParagraph()
                .AppendEquation($"ϑ_2={data.sigma_m:f2}/({data.K2}∙{data.SigmaAllow}∙{dataIn.phi})={data.v2:f2}");

            body.AddParagraph()
                .AppendEquation($"K_1=(1-{data.v2:f2}^2)/((1/3+{data.v1:f2}∙{data.v2:f2})+√((1/3+{data.v1:f2}∙{data.v2:f2})^2+(1-{data.v2:f2}^2)∙{data.v1:f2}^2))={data.K1:f2}");

            body.AddParagraph()
                .AppendEquation($"[σ_i]={data.K1:f2}∙{data.K2}∙{data.SigmaAllow}={data.sigmaid:f2}");


            body.AddParagraph()
                .AppendEquation($"[F]_1=({data.sigmaid:f2}∙{dataIn.h1}∙({dataIn.s}-{dataIn.c})^2)/({data.K7:f2}∙{data.e1e:f2})={data.F1Allow:f2}");
        }
        else
        {
            body.AddParagraph("Несущая способность обечайки в месте приварки опорной лапы с подкладным листом должна удовлетворять условию");
            body.AddParagraph()
                .AppendEquation("F_1≤[F]_1=([σ_i]∙b_3∙(s-c)^2)/(K_8∙(e_1Э+s_2))");
            body.AddParagraph("Коэффициент ")
                .AppendEquation("K_8")
                .AddRun(" - вычисляют по формуле");


            body.AddParagraph()
                .AppendEquation("K_8=min{exp[(-49.919-39.119∙x-107.01∙y_1-1.693∙x^2-11.92∙x∙y_1-39.276∙y_1^2+0.237∙x^3+1.608∙x^2∙y_1+2.761∙x∙y_1^2-3.854∙y_1^3)∙10^-2]; exp[(-5.964-11.395∙x-18.984∙y-2.413∙x^2-7.286∙x∙y-2.042∙y^2+0.1322∙x^3+0.4833∙х^2∙у+0.8469∙x∙y^2+1.428∙y^3)∙10^-2];}");



            body.AddParagraph("где ");
            body.AddParagraph()
                .AppendEquation("x=ln(D_p/2∙(s-c))" + $"=ln({data.Dp}/2∙({dataIn.s}-{dataIn.c}))={data.x:f2}");

            body.AddParagraph()
                .AppendEquation("y=ln(h_1/D_p)" + $"=ln({dataIn.h1}/{data.Dp})={data.y:f2}");

            body.AddParagraph()
                .AppendEquation("y_1=ln(b_3/D_p)" + $"=ln({dataIn.b3}/{data.Dp})={data.y1:f2}");

            body.AddParagraph().AppendEquation($"K_8=min{{{data.K81:f2};{data.K82:f2}}}={data.K8:f2}");

            body.AddParagraph("предельные напряжения изгиба ")
                .AppendEquation("[σ_i]")
                .AddRun(" - вычисляют по формуле");

            body.AddParagraph()
                .AppendEquation("[σ_i]=K_1∙K_2∙[σ]");

            body.AddParagraph()
                .AppendEquation("K_1=(1-ϑ_2^2)/((1/3+ϑ_1∙ϑ_2)+√((1/3+ϑ_1∙ϑ_2)^2+(1-ϑ_2^2)∙ϑ_1^2))");

            body.AddParagraph()
                .AppendEquation($"K_2={data.K2}")
                .AddRun(dataIn.IsAssembly
                    ? " - для условий испытания и монтажа"
                    : " - для рабочих условий");

            body.AddParagraph()
                .AppendEquation("ϑ_1" + $"={data.v1}");

            body.AddParagraph()
                .AppendEquation("ϑ_2=σ_m/(K_2∙[σ]∙φ)");


            body.AddParagraph()
                .AppendEquation("σ_m=σ_my=(p∙D_p)/(2∙(s-c))" +
                                $"=({dataIn.p}∙{data.Dp})/(2∙({dataIn.s}-{dataIn.c}))={data.sigma_m:f2}");

            body.AddParagraph()
                .AppendEquation($"ϑ_2={data.sigma_m:f2}/({data.K2}∙{data.SigmaAllow}∙{dataIn.phi})={data.v2:f2}");

            body.AddParagraph()
                .AppendEquation($"K_1=(1-{data.v2:f2}^2)/((1/3+{data.v1:f2}∙{data.v2:f2})+√((1/3+{data.v1:f2}∙{data.v2:f2})^2+(1-{data.v2:f2}^2)∙{data.v1:f2}^2))={data.K1:f2}");

            body.AddParagraph()
                .AppendEquation($"[σ_i]={data.K1:f2}∙{data.K2}∙{data.SigmaAllow}={data.sigmaid:f2}");


            body.AddParagraph()
                .AppendEquation($"[F]_1=({data.sigmaid:f2}∙{dataIn.h1}∙({dataIn.s}-{dataIn.c})^2)/({data.K7:f2}∙{data.e1e:f2})={data.F1Allow:f2}");
        }


        body.AddParagraph()
            .AppendEquation($"{data.F1:f2}≤{data.F1Allow:f2}");

        if (data.F1 <= data.F1Allow)
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
        body.AddParagraph()
            .AppendEquation($"(s-c)/D=({dataIn.s}-{dataIn.c})/{data.Dp}={data.ConditionUseFormulas1:f3}≤0.05");

        body.AddParagraph()
            .AppendEquation($"g={dataIn.g}≥0.2∙h_1=0.2∙{dataIn.h1}={data.ConditionUseFormulas2:f2}");

        body.AddParagraph()
            .AppendEquation($"0.04≤h_1/D_p={dataIn.h1}/{data.Dp}={data.ConditionUseFormulas3:f3}≤0.5");

        body.AddParagraph()
            .AppendEquation($"0.04≤b_4/D_p={dataIn.b4}/{data.Dp}={data.ConditionUseFormulas4:f3}≤0.5");

        if (dataIn.ReinforcingPad)
        {
            body.AddParagraph()
                .AppendEquation($"0.04≤b_3/D_p={dataIn.b3}/{data.Dp}={data.ConditionUseFormulas5:f3}≤0.8");

            body.AddParagraph()
                .AppendEquation($"b_2={dataIn.b2}≥0.6∙b_3=0.6∙{dataIn.b3}={data.ConditionUseFormulas6:f2}");

            body.AddParagraph()
                .AppendEquation($"b_3={dataIn.b3}≤1.5∙h_1=1.5∙{dataIn.h1}={data.ConditionUseFormulas7:f2}");

            body.AddParagraph()
                .AppendEquation($"s_2={dataIn.s2}≥s={dataIn.s}");
        }

        package.Close();
    }
}