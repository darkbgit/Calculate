using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Conical;
using CalculateVessels.Core.Shells.Enums;
using CalculateVessels.Output.Word.Core;
using CalculateVessels.Output.Word.Enums;
using CalculateVessels.Output.Word.Interfaces;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace CalculateVessels.Output.Word.Elements;

internal class ConicalShellWordOutput : IWordOutputElement<ConicalShellCalculated>
{
    public void MakeWord(string filePath, ICalculatedElement calculatedData)
    {
        if (calculatedData is not ConicalShellCalculated data)
            throw new NullReferenceException();

        var dataIn = (ConicalShellInput)data.InputData;

        if (string.IsNullOrWhiteSpace(filePath))
        {
            const string DEFAULT_FILE_NAME = "temp.docx";
            filePath = DEFAULT_FILE_NAME;
        }

        using var package = WordprocessingDocument.Open(filePath, true);

        var mainPart = package.MainDocumentPart ?? throw new InvalidOperationException();
        var body = mainPart.Document.Body ?? throw new InvalidOperationException();

        body.AddParagraph($"Расчет на прочность конической обечайки {dataIn.Name}, нагруженной " +
                          (dataIn.IsPressureIn ? "внутренним избыточным давлением" : "наружным давлением")).Heading(HeadingType.Heading1);
        body.AddParagraph("");

        var imagePart = mainPart.AddImagePart(ImagePartType.Gif);

        byte[] bytes = Data.Properties.Resources.ConeElemBottom;

        if (bytes != null)
        {
            imagePart.FeedData(new MemoryStream(bytes));
            body.AddParagraph("").AddImage(mainPart.GetIdOfPart(imagePart), bytes);
        }

        body.AddParagraph("Исходные данные").Alignment(AlignmentType.Center);

        //table
        {
            var table = body.AddTable();
            //table.SetWidths(new float[] { 300, 100 });
            //int i = 0;
            table.AddRow()
                .AddCell("Материал обечайки")
                .AddCell($"{dataIn.Steel}");

            table.AddRow()
                .AddCell("Внутренний диаметр обечайки, D:")
                .AddCell($"{dataIn.D} мм");

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
                .AddCell("Коэффициент прочности сварного шва, ")
                .AppendEquation("φ_p")
                .AppendText(":")
                .AddCell($"{dataIn.fi}");

            table.AddRowWithOneCell("Условия нагружения");

            table.AddRow()
                .AddCell("Расчетная температура, Т:")
                .AddCell($"{dataIn.t} °С");

            table.AddRow()
                .AddCell("Расчетное " + (dataIn.IsPressureIn ? "внутреннее избыточное" : "наружное")
                                      + " давление, p:")
                .AddCell($"{dataIn.p} МПа");

            table.AddRow()
                .AddCell($"Допускаемое напряжение для материала {dataIn.Steel} при расчетной температуре, [σ]:")
                .AddCell($"{dataIn.SigmaAllow} МПа");

            if (!dataIn.IsPressureIn)
            {
                table.AddRow()
                    .AddCell("Модуль продольной упругости при расчетной температуре, E:")
                    .AddCell($"{dataIn.E} МПа");
            }

            if (dataIn.ConnectionType != ConicalConnectionType.WithoutConnection)
            {
                table.AddRowWithOneCell("Нижнее соединение");

                var connectionType = dataIn.ConnectionType switch
                {
                    ConicalConnectionType.Simply => "Без тороидального перехода",
                    ConicalConnectionType.WithRingPicture25b => "С укрепляющим кольцом",
                    ConicalConnectionType.WithRingPicture29 => "С укрепляющим кольцом",
                    ConicalConnectionType.Toroidal => "С тороидальным переходом",
                    _ => throw new ArgumentOutOfRangeException()
                };

                table.AddRow()
                    .AddCell("Тип")
                    .AddCell(connectionType);
            }


            body.InsertTable(table);
        }

        body.AddParagraph("");
        body.AddParagraph("Результаты расчета").Alignment(AlignmentType.Center);
        body.AddParagraph("");
        body.AddParagraph("Расчетные параметры").Alignment(AlignmentType.Center);
        body.AddParagraph("");

        body.AddParagraph("Расчетные длины переходных частей. В первом приближении принимаем ")
            .AppendEquation($"s_1={dataIn.s} мм");

        switch (dataIn.ConnectionType)
        {
            case ConicalConnectionType.Simply:
            case ConicalConnectionType.WithRingPicture25b:
                body.Elements<Paragraph>().Last()
                    .AppendEquation($"s_2={dataIn.s2Big} мм");
                body.AddParagraph("- для конических и цилиндрических обечаек");
                body.AddParagraph("")
                    .AppendEquation("a_1p=0.7√(D/cosα_1∙(s_1-c))" +
                                    $"=0.7√({dataIn.D}/cos{dataIn.alpha1}({dataIn.s1Big}-{data.c:f2}))={data.a1p:f2}");
                body.AddParagraph("")
                    .AppendEquation("a_2p=0.7√(D∙(s_2-c))" +
                                    $"=0.7√({dataIn.D}∙({dataIn.s2Big}-{data.c:f2}))={data.a2p:f2}");
                break;
            case ConicalConnectionType.Toroidal:
                body.Elements<Paragraph>().Last()
                    .AppendEquation($"s_T={dataIn.sT} мм");
                body.AddParagraph("- для конических и цилиндрических обечаек");
                body.AddParagraph("")
                    .AppendEquation("a_1p=0.7√(D/cosα_1∙(s_T-c))" +
                                    $"=0.7√({dataIn.D}/cos{dataIn.alpha1}({dataIn.sT}-{data.c:f2}))={data.a1p:f2}");
                body.AddParagraph("")
                    .AppendEquation("a_2p=0.5√(D∙(s_T-c))" +
                                    $"=0.5√({dataIn.D}∙({dataIn.sT}-{data.c:f2}))={data.a2p:f2}");
                break;
        }

        if (dataIn.IsConnectionWithLittle)
        {
            body.AddParagraph("- для конических и цилиндрических обечаек или штуцера")
                .AppendEquation("a_1p=√(D_1/cosα_1∙(s_1-c))" +
                                $"=√({dataIn.D1}/cos{dataIn.alpha1}({dataIn.s1Little}-{data.c:f2}))={data.a1p_l:f2}");
            body.AddParagraph("")
                .AppendEquation("a_2p=1.25√(D_1∙(s_2-c))" +
                                $"=1.25√({dataIn.D1}∙({dataIn.s2Little}-{data.c:f2}))={data.a2p_l:f2}");
        }

        body.AddParagraph("Расчетный диаметр гладкой конической обечайки");
        if (dataIn.ConnectionType != ConicalConnectionType.Toroidal)
        {
            body.Elements<Paragraph>().Last()
                .AddRun(" без тороидального перехода");
            body.AddParagraph("")
                .AppendEquation("D_k=D-1.4∙a_1p∙sinα_1" +
                                $"={dataIn.D}-1.4∙{data.a1p:f2}∙sin{dataIn.alpha1}={data.Dk:f2} мм");
        }
        else
        {
            body.Elements<Paragraph>().Last()
                .AddRun(" с тороидальным переходом");
            body.AddParagraph("")
                .AppendEquation("D_k=D-2[r(1-cosα_1)+0.7∙a_1p∙sinα_1]" +
                                $"={dataIn.D}-2[{dataIn.r}(1-cos{dataIn.alpha1})+0.7∙{data.a1p:f2}∙sin{dataIn.alpha1}]={data.Dk:f2} мм");
        }

        body.AddParagraph("Толщину стенки гладкой конической обечайки вычисляют по формуле:");
        body.AddParagraph("").AppendEquation("s_k≥s_k.p+c");
        body.AddParagraph("где ").AppendEquation("s_k.p").AddRun(" - расчетная толщина стенки конической обечайки");

        if (dataIn.IsPressureIn)
        {
            body.AddParagraph("")
                .AppendEquation("s_k.p=(p∙D_k)/(2∙φ_p∙[σ]-p)(1/cosα_1)" +
                                $"=({dataIn.p}∙{data.Dk:f2})/(2∙{dataIn.fi}∙{dataIn.SigmaAllow}-{dataIn.p})(1/cos{dataIn.alpha1})={data.s_p:f2} мм");
        }
        else
        {
            body.AddParagraph("")
                .AppendEquation("s_k.p=max{1.06∙(10^-2∙D_E)/(B_1)∙(p/(10^-5∙E)∙l_E/D_E)^0.4;(1.2∙p∙D_k)/(2∙φ_p∙[σ]-p)(1/cosα_1)}");
            body.AddParagraph("Эффективные размеры конической обечайки вычисляют по формулам (предварительно принимаем ")
                .AppendEquation($"s_k={dataIn.s} мм")
                .AddRun("):");
            body.AddParagraph("")
                .AppendEquation("l_E=(D-D_1)/(2∙sinα_1)" + $"=({dataIn.D}-{dataIn.D1})/(2∙sin{dataIn.alpha1})={data.lE:f2} мм");
            body.AddParagraph("")
                .AppendEquation("D_E=max{(D+D_1)/(2∙cosα_1);D/cosα_1-0.3∙(D+D_1)∙√((D+D_1)/((s_k-c)∙100)∙tgα_1}");
            body.AddParagraph("")
                .AppendEquation("(D+D_1)/(2∙cosα_1)" + $"=({dataIn.D}+{dataIn.D1})/(2∙cos{dataIn.alpha1})={data.DE_1:f2}");
            body.AddParagraph("")
                .AppendEquation("D/cosα_1-0.3∙(D+D_1)∙√((D+D_1)/((s_k-c)∙100)∙tgα_1" +
                                $"{dataIn.D}/cos{dataIn.alpha1}-0.3∙({dataIn.D}+{dataIn.D1})∙√(({dataIn.D}+{dataIn.D1})/(({dataIn.s}-{data.c:f2})∙100)∙tg{dataIn.alpha1}={data.DE_2:f2}");
            body.AddParagraph("")
                .AppendEquation($"D_E=max{{({data.DE_1:f2};{data.DE_2:f2}}}={data.DE:f2} мм");
            body.AddParagraph("Коэффициент ")
                .AppendEquation("B_1")
                .AddRun(" вычисляют по формуле:");
            body.AddParagraph("")
                .AppendEquation("B_1=min{1.0;9.45∙D_E/l_E∙√(D_E/((s_k-c)∙100))}");
            body.AddParagraph("")
                .AppendEquation("9.45∙D_E/l_E∙√(D_E/((s_k-c)∙100))" +
                                $"9.45∙{data.DE:F2}/{data.lE:f2}∙√({data.DE:f2}/(({dataIn.s}-{data.c:f2})∙100))={data.B1_1:f2}");
            body.AddParagraph("")
                .AppendEquation($"B=max(1;{data.B1_1:f2})={data.B1:f2}");
            body.AddParagraph("")
                .AppendEquation($"1.06∙(10^-2∙{data.DE:f2})/{data.B1:f2}∙({dataIn.p}/(10^-5∙{dataIn.E})∙{data.lE:f2}/{data.DE:f2})^0.4={data.s_p_1:f2}");
            body.AddParagraph("")
                .AppendEquation($"(1.2∙{dataIn.p}∙{data.Dk:f2})/(2∙{dataIn.fi}∙{dataIn.SigmaAllow}-{dataIn.p})(1/cos{dataIn.alpha1})={data.s_p_2:f2}");
            body.AddParagraph("")
                .AppendEquation($"s_p=max{{{data.s_p_1:f2};{data.s_p_2:f2}}}={data.s_p:f2} мм");
        }

        body.AddParagraph("c - сумма прибавок к расчетной толщине");
        body.AddParagraph("")
            .AppendEquation($"c=c_1+c_2+c_3={dataIn.c1}+{dataIn.c2}+{dataIn.c3}={data.c:f2} мм");

        body.AddParagraph("").AppendEquation($"s_k={data.s_p:f2}+{data.c:f2}={data.s:f2} мм");

        if (dataIn.s > data.s)
        {
            body.AddParagraph($"Принятая толщина s_k={dataIn.s} мм").Bold();
        }
        else
        {
            body.AddParagraph($"Принятая толщина s_k={dataIn.s} мм")
                .Bold()
                .Color(System.Drawing.Color.Red);
        }
        if (dataIn.IsPressureIn)
        {
            body.AddParagraph("Допускаемое внутреннее избыточное давление вычисляют по формуле:");
            body.AddParagraph("")
                .AppendEquation("[p]=(2∙[σ]∙φ_p∙(s_k-c))/(D_k/cosα_1+(s_k-c))"
                                + $"=(2∙{dataIn.SigmaAllow}∙{dataIn.fi}∙({dataIn.s}-{data.c:f2}))/({data.Dk:f2}/cos{dataIn.alpha1}+({dataIn.s}-{data.c:f2}))={data.p_d:f2} МПа");
        }
        else
        {
            body.AddParagraph("Допускаемое наружное давление вычисляют по формуле:");
            body.AddParagraph("")
                .AppendEquation("[p]=[p]_П/√(1+([p]_П/[p]_E)^2)");
            body.AddParagraph("допускаемое давление из условия прочности вычисляют по формуле:");
            body.AddParagraph("")
                .AppendEquation("[p]_П=(2∙[σ]∙φ_p∙(s_k-c))/(D_k/cosα_1+(s_k-c))" +
                                $"=(2∙{dataIn.SigmaAllow}∙{dataIn.fi}∙({dataIn.s}-{data.c:f2}))/({data.Dk:f2}/cos{dataIn.alpha1}+({dataIn.s}-{data.c:f2}))={data.p_dp:f2} МПа");
            body.AddParagraph("допускаемое давление из условия устойчивости в пределах упругости вычисляют по формуле:");
            body.AddParagraph("")
                .AppendEquation("[p]_E=(2.08∙10^-5∙E)/(n_y∙B_1)∙D_E/l_E∙[(100∙(s_k-c))/D_E]^2.5" +
                                $"=(2.08∙10^-5∙{dataIn.E})/({dataIn.ny}∙{data.B1:f2})∙{data.DE:f2}/{data.lE:f2}∙[(100∙({dataIn.s}-{data.c:f2}))/{data.DE:f2}]^2.5={data.p_de:f2} МПа");
            body.AddParagraph("")
                .AppendEquation($"[p]={data.p_dp:f2}/√(1+({data.p_dp:f2}/{data.p_de:f2})^2)={data.p_d:f2} МПа");
        }

        body.AddParagraph("").AppendEquation("[p]≥p");
        body.AddParagraph("")
            .AppendEquation($"{data.p_d:f2}≥{dataIn.p}");
        if (data.p_d > dataIn.p)
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

        if (dataIn.ConnectionType != ConicalConnectionType.WithoutConnection)
        {
            switch (dataIn.ConnectionType)
            {
                case ConicalConnectionType.Simply:
                    body.AddParagraph("Соединение обечайки без тороидального перехода");
                    body.AddParagraph("Толщину стенки из условия прочности переходной зоны вычисляют по формулам: ");
                    body.AddParagraph("").AppendEquation("s_2≥s_2p+c");
                    body.AddParagraph("")
                        .AppendEquation("s_2p=(p∙D∙β_1)/(2∙[σ]_2∙φ_p-p)");
                    body.AddParagraph("где ")
                        .AppendEquation("β_1")
                        .AddRun(" - коэффициент формы вычисляют по формуле:");
                    body.AddParagraph("")
                        .AppendEquation("β_1=max{0.5;β}");
                    body.AddParagraph("")
                        .AppendEquation("β=0.4√(D/(s_2-c))∙tgα_1/(1+√((1+χ_1((s_1-c)/(s_2-c))^2)/(2∙cosα_1)χ_1(s_1-c)/(s_2-c)))-0.25");
                    body.AddParagraph("")
                        .AppendEquation("χ_1=[σ]_1/[σ]_2" +
                                        $"={data.SigmaAllow1Big}/{data.SigmaAllow2Big}={data.chi_1Big:f2}");
                    body.AddParagraph("")
                        .AppendEquation($"β=0.4√({dataIn.D}/({dataIn.s2Big}-{data.c:f2}))∙tg{dataIn.alpha1}/(1+√((1+{data.chi_1Big:f2} (({dataIn.s1Big}-{data.c:f2})/({dataIn.s2Big}-{data.c:f2}))^2)/(2∙cos{dataIn.alpha1}){data.chi_1Big:f2}({dataIn.s1Big}-{data.c:f2})/({dataIn.s2Big}-{data.c:f2})))-0.25={data.beta:f2}");
                    body.AddParagraph("")
                        .AppendEquation($"β_1=max{{0.5;{data.beta:f2}}}={data.beta_1:f2}");
                    body.AddParagraph("")
                        .AppendEquation($"s_2p=({dataIn.p}∙{dataIn.D}∙{data.beta_1:f2})/(2∙{dataIn.SigmaAllow}∙{dataIn.fi}-{dataIn.p})={data.s_2pBig:f2} мм");
                    body.AddParagraph("Допускаемое " +
                                      (dataIn.IsPressureIn ? "внутреннее избыточное" : "наружное") +
                                      "давление из условия прочности переходной части вычисляют по формуле");
                    body.AddParagraph("")
                        .AppendEquation("[p]=(2∙[σ]_2∙φ_p∙(s_2-c))/(D∙β_1+(s_2-c))" +
                                        $"=(2∙{data.SigmaAllow2Big}∙{dataIn.fi}∙({dataIn.s2Big}-{data.c:f2}))/({dataIn.D}∙{data.beta_1:f2}+({dataIn.s2Big}-{data.c:f2}))={data.p_dBig:f2} МПа");
                    break;
            }

            body.AddParagraph("").AppendEquation("[p]≥p");
            body.AddParagraph("")
                .AppendEquation($"{data.p_dBig:f2}≥{dataIn.p}");
            if (data.p_dBig > dataIn.p)
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

        }
        //UNDONE: Make word conical shell

        const int DIAMETER_BIG_LITTLE_BORDER = 200;
        body.AddParagraph("Условия применения расчетных формул ")
            .AddRun(dataIn.D >= DIAMETER_BIG_LITTLE_BORDER ?
                "при D ≥ 200 мм" : "при D < 200 мм");


        body.AddParagraph("")
            .AppendEquation(dataIn.D >= DIAMETER_BIG_LITTLE_BORDER ?
                $"(s-c)/(D)=({dataIn.s}-{data.c:f2})/({dataIn.D})={(dataIn.s - data.c) / dataIn.D:f3}≤0.1" :
                $"(s-c)/(D)=({dataIn.s}-{data.c:f2})/({dataIn.D})={(dataIn.s - data.c) / dataIn.D:f3}≤0.3");

        if (!data.IsConditionUseFormulas)
        {
            body.AddParagraph("Условия применения расчетных формул не выполняется ")
                .Bold()
                .Color(System.Drawing.Color.Red);
        }

        package.Close();
    }
}