using CalculateVessels.Core.Elements.Shells.Conical;
using CalculateVessels.Core.Elements.Shells.Enums;
using CalculateVessels.Core.Enums;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Properties;
using CalculateVessels.Output.Word.Core;
using CalculateVessels.Output.Word.Enums;
using CalculateVessels.Output.Word.Helpers;
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

        filePath = WordHelpers.CheckFilePath(filePath);

        using var package = WordprocessingDocument.Open(filePath, true);

        var mainPart = package.MainDocumentPart ?? throw new InvalidOperationException();
        var body = mainPart.Document.Body ?? throw new InvalidOperationException();

        InsertHeader(body, dataIn);

        InsertImage(mainPart, dataIn);

        InsertInitialDataTable(body, dataIn, data);

        InsertCommonDataCalculated(body, dataIn, data.CommonData);

        InsertLoadingConditionsDataCalculated(body, dataIn, data);

        package.Dispose();
    }

    private static void InsertLoadingConditionsDataCalculated(Body body, ConicalShellInput dataIn, ConicalShellCalculated data)
    {
        var moreThanOneLoadingCondition = dataIn.LoadingConditions.Count() > 1;

        dataIn.LoadingConditions
            .ToList()
            .ForEach(lc => InsertOneLoadingConditionCalculated(body, data.Results
                    .First(r => r.LoadingConditionId == lc.Id),
                data.CommonData,
                dataIn,
                moreThanOneLoadingCondition));
    }

    private static void InsertHeader(Body body, ConicalShellInput dataIn)
    {
        body.AddParagraph($"Расчет на прочность конической обечайки {dataIn.Name}")
            .Heading(HeadingType.Heading1)
            .Alignment(AlignmentType.Center);
    }

    private static void InsertCommonDataCalculated(Body body, ConicalShellInput dataIn, ConicalShellCalculatedCommon data)
    {
        body.AddParagraph();
        body.AddParagraph("Расчетные параметры").Alignment(AlignmentType.Center);
        body.AddParagraph();

        if (dataIn.ConnectionType != ConicalConnectionType.WithoutConnection || dataIn.IsConnectionWithLittle)
        {
            body.AddParagraph("Расчетные длины переходных частей. В первом приближении принимаем ")
                .AppendEquation($"s_1={dataIn.s} мм");
        }

        if (dataIn.ConnectionType != ConicalConnectionType.WithoutConnection)
        {
            body.AddParagraph("Для узла соединения в месте большего диаметра");

            switch (dataIn.ConnectionType)
            {
                case ConicalConnectionType.Simply:
                case ConicalConnectionType.WithRingPicture25b:
                    body.Elements<Paragraph>().Last()
                        .AppendEquation($" s_2={dataIn.s2Big} мм");
                    body.AddParagraph("- для конических и цилиндрических обечаек");
                    body.AddParagraph()
                        .AppendEquation("a_1p=0.7√(D/cosα_1∙(s_1-c))" +
                                        $"=0.7√({dataIn.D}/cos{MathHelper.RadiansToDegree(data.alpha1):f0}∙({dataIn.s1Big}-{data.c:f2}))={data.a1p:f2}");
                    body.AddParagraph()
                        .AppendEquation("a_2p=0.7√(D∙(s_2-c))" +
                                        $"=0.7√({dataIn.D}∙({dataIn.s2Big}-{data.c:f2}))={data.a2p:f2}");
                    break;
                case ConicalConnectionType.Toroidal:
                    body.Elements<Paragraph>().Last()
                        .AppendEquation($"s_T={dataIn.sT} мм");
                    body.AddParagraph("- для конических и цилиндрических обечаек");
                    body.AddParagraph()
                        .AppendEquation("a_1p=0.7√(D/cosα_1∙(s_T-c))" +
                                        $"=0.7√({dataIn.D}/cos{MathHelper.RadiansToDegree(data.alpha1):f0}∙({dataIn.sT}-{data.c:f2}))={data.a1p:f2}");
                    body.AddParagraph()
                        .AppendEquation("a_2p=0.5√(D∙(s_T-c))" +
                                        $"=0.5√({dataIn.D}∙({dataIn.sT}-{data.c:f2}))={data.a2p:f2}");
                    break;
            }
        }

        if (dataIn.IsConnectionWithLittle)
        {
            body.AddParagraph("Для узла соединения в месте меньшего диаметра");
            body.AddParagraph("- для конических и цилиндрических обечаек или штуцера");

            body.AddParagraph()
            .AppendEquation("a_1p=√(D_1/cosα_1∙(s_1-c))" +
                            $"=√({dataIn.D1}/cos{MathHelper.RadiansToDegree(data.alpha1):f0}∙({dataIn.s1Little}-{data.c:f2}))={data.a1p_l:f2}");
            body.AddParagraph()
                .AppendEquation("a_2p=1.25√(D_1∙(s_2-c))" +
                                $"=1.25√({dataIn.D1}∙({dataIn.s2Little}-{data.c:f2}))={data.a2p_l:f2}");
        }

        body.AddParagraph("Расчетный диаметр гладкой конической обечайки");
        if (dataIn.ConnectionType != ConicalConnectionType.Toroidal)
        {
            body.Elements<Paragraph>().Last()
                .AddRun(" без тороидального перехода");
            body.AddParagraph()
                .AppendEquation("D_k=D-1.4∙a_1p∙sinα_1" +
                                $"={dataIn.D}-1.4∙{data.a1p:f2}∙sin{MathHelper.RadiansToDegree(data.alpha1):f0}={data.Dk:f2} мм");
        }
        else
        {
            body.Elements<Paragraph>().Last()
                .AddRun(" с тороидальным переходом");
            body.AddParagraph()
                .AppendEquation("D_k=D-2[r(1-cosα_1)+0.7∙a_1p∙sinα_1]" +
                                $"={dataIn.D}-2[{dataIn.r}(1-cos{MathHelper.RadiansToDegree(data.alpha1):f0})+0.7∙{data.a1p:f2}∙sin{MathHelper.RadiansToDegree(data.alpha1)}]={data.Dk:f2} мм");
        }

        body.AddParagraph("Сумма прибавок к расчетной толщине");
        body.AddParagraph()
            .AppendEquation($"c=c_1+c_2+c_3={dataIn.c1}+{dataIn.c2}+{dataIn.c3}={data.c:f2} мм");
    }

    private static void InsertImage(MainDocumentPart mainPart, ConicalShellInput dataIn)
    {
        mainPart.InsertImage(Resources.ConeElem, ImagePartType.Gif);

        if (dataIn.ConnectionType != ConicalConnectionType.WithoutConnection)
        {
            byte[] image = dataIn.ConnectionType switch
            {
                ConicalConnectionType.Simply => Resources.ConeBigConnectionSimple_1,
                ConicalConnectionType.WithRingPicture25b => Resources.ConeBigConnectionWithRing,
                ConicalConnectionType.WithRingPicture29 => Resources.ConeBigConnectionWithRingPicture29,
                ConicalConnectionType.Toroidal => Resources.ConeBigConnectionToroidal,
                _ => throw new InvalidOperationException()
            };

            mainPart.InsertImage(image, ImagePartType.Gif);
        }

        if (dataIn.IsConnectionWithLittle)
        {
            mainPart.InsertImage(Resources.ConeLittleConnectionSimple_1, ImagePartType.Gif);
        }
    }

    private static void InsertInitialDataTable(Body body, ConicalShellInput dataIn, ConicalShellCalculated data)
    {
        body.AddParagraph("Исходные данные").Alignment(AlignmentType.Center);

        var table = body.AddTable();
        //table.SetWidths(new float[] { 300, 100 });
        //int i = 0;
        table.AddRow()
            .AddCell("Материал обечайки")
            .AddCell($"{dataIn.Steel}");

        table.AddRow()
            .AddCell("Больший диаметр конуса, D:")
            .AddCell($"{dataIn.D} мм");

        table.AddRow()
            .AddCell("Меньший диаметр конуса, ")
            .AppendEquation("D_1")
            .AppendText(":")
            .AddCell($"{dataIn.D1} мм");

        table.AddRow()
            .AddCell("Длина конуса, L:")
            .AddCell($"{dataIn.L} мм");

        table.AddRow()
            .AddCell("Половина угла раствора при вершине конической обечайки, ")
            .AppendEquation("α_1")
            .AppendText(":")
            .AddCell($"{MathHelper.RadiansToDegree(data.CommonData.alpha1):f0} град");

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
            .AddCell("Коэффициент прочности продольного сварного шва, ")
            .AppendEquation("φ_p")
            .AppendText(":")
            .AddCell($"{dataIn.phi}");

        table.AddRow()
            .AddCell("Коэффициент прочности кольцевого сварного шва, ")
            .AppendEquation("φ_t")
            .AppendText(":")
            .AddCell($"{dataIn.phi_t}");

        if (dataIn.ConnectionType != ConicalConnectionType.WithoutConnection)
        {
            table.AddRowWithOneCell("Узел соединения большего диаметра");

            switch (dataIn.ConnectionType)
            {
                case ConicalConnectionType.Simply:
                    table.AddRow()
                        .AddCell("Тип")
                        .AddCell("Без тороидального перехода");
                    table.AddRow()
                        .AddCell("Исполнительная толщина переходной зоны конической обечайки, ")
                        .AppendEquation("s_1")
                        .AppendText(":")
                        .AddCell($"{dataIn.s1Big} мм");
                    table.AddRow()
                        .AddCell("Материал переходной зоны конической обечайки")
                        .AddCell($"{dataIn.Steel1Big}");
                    table.AddRow()
                        .AddCell("Исполнительная толщина переходной зоны цилиндрической обечайки, ")
                        .AppendEquation("s_2")
                        .AppendText(":")
                        .AddCell($"{dataIn.s2Big} мм");
                    table.AddRow()
                        .AddCell("Материал переходной зоны цилиндрической обечайки")
                        .AddCell($"{dataIn.Steel2Big}");
                    break;
                case ConicalConnectionType.WithRingPicture25b:
                case ConicalConnectionType.WithRingPicture29:
                    table.AddRow()
                        .AddCell("Тип")
                        .AddCell("С укрепляющим кольцом");
                    table.AddRow()
                        .AddCell("Исполнительная толщина переходной зоны конической обечайки, ")
                        .AppendEquation("s_1")
                        .AppendText(":")
                        .AddCell($"{dataIn.s1Big} мм");
                    table.AddRow()
                        .AddCell("Материал переходной зоны конической обечайки")
                        .AddCell($"{dataIn.Steel1Big}");
                    table.AddRow()
                        .AddCell("Исполнительная толщина переходной зоны цилиндрической обечайки, ")
                        .AppendEquation("s_2")
                        .AppendText(":")
                        .AddCell($"{dataIn.s2Big} мм");
                    table.AddRow()
                        .AddCell("Материал переходной зоны цилиндрической обечайки")
                        .AddCell($"{dataIn.Steel2Big}");
                    table.AddRow()
                        .AddCell("Площадь поперечного сечения кольца жесткости, ")
                        .AppendEquation("A_k")
                        .AppendText(":")
                        .AddCell($"{dataIn.Ak} ")
                        .AppendEquation("мм^2");
                    table.AddRow()
                        .AddCell("Материал кольца жесткости")
                        .AddCell($"{dataIn.SteelC}");
                    table.AddRow()
                        .AddCell("Коэффициент прочности сварных швов кольца жесткости, ")
                        .AppendEquation("φ_k")
                        .AppendText(":")
                        .AddCell($"{dataIn.phi_k}");
                    break;
                case ConicalConnectionType.Toroidal:
                    table.AddRow()
                        .AddCell("Тип")
                        .AddCell("С тороидальным переходом");
                    table.AddRow()
                        .AddCell("Исполнительная толщина тороидального перехода, ")
                        .AppendEquation("s_T")
                        .AppendText(":")
                        .AddCell($"{dataIn.sT} мм");
                    table.AddRow()
                        .AddCell("Материал тороидального перехода")
                        .AddCell($"{dataIn.SteelT}");
                    table.AddRow()
                        .AddCell("Радиус тороидального перехода, r:")
                        .AddCell($"{dataIn.r} мм");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        if (dataIn.IsConnectionWithLittle)
        {
            table.AddRowWithOneCell("Узел соединения меньшего диаметра");

            table.AddRow()
                .AddCell("Исполнительная толщина переходной зоны конической обечайки, ")
                .AppendEquation("s_1")
                .AppendText(":")
                .AddCell($"{dataIn.s1Little} мм");
            table.AddRow()
                .AddCell("Материал переходной зоны конической обечайки")
                .AddCell($"{dataIn.Steel1Little}");
            table.AddRow()
                .AddCell("Исполнительная толщина переходной зоны цилиндрической обечайки, ")
                .AppendEquation("s_2")
                .AppendText(":")
                .AddCell($"{dataIn.s2Little} мм");
            table.AddRow()
                .AddCell("Материал переходной зоны цилиндрической обечайки")
                .AddCell($"{dataIn.Steel2Little}");
        }

        WordHelpers.AddLoadingConditionsInTableForShells(dataIn.LoadingConditions, table);

        WordHelpers.AddMaterialCharacteristicsInTableForShell(dataIn.Steel,
            data.Results.Select(r => new
            {
                r.LoadingConditionId,
                r.SigmaAllow,
                r.E
            }).ToList(),
            dataIn.LoadingConditions, table);

        body.InsertTable(table);
    }

    private static void InsertOneLoadingConditionCalculated(Body body, ConicalShellCalculatedOneLoading data, ConicalShellCalculatedCommon cdc, ConicalShellInput dataIn, bool withNumber = true)
    {
        var loadingCondition = dataIn.LoadingConditions.First(lc => lc.Id == data.LoadingConditionId);

        body.AddParagraph();
        body.AddParagraph("Результаты расчета" + (withNumber ? $" для условий нагружения #{loadingCondition.Id})" : ""))
            .Alignment(AlignmentType.Center);


        body.AddParagraph("Толщину стенки гладкой конической обечайки вычисляют по формуле:");
        body.AddParagraph()
            .AppendEquation("s_k≥s_(k.p)+c");
        body.AddParagraph("где ").AppendEquation("s_(k.p)").AddRun(" - расчетная толщина стенки конической обечайки");

        if (loadingCondition.PressureType == PressureType.Inside)
        {
            body.AddParagraph()
                .AppendEquation("s_(k.p)=(p∙D_k)/(2∙φ_p∙[σ]-p)(1/cosα_1)" +
                                $"=({loadingCondition.p}∙{cdc.Dk:f2})/(2∙{dataIn.phi}∙{data.SigmaAllow}-{loadingCondition.p})(1/cos{MathHelper.RadiansToDegree(cdc.alpha1):f0})={data.s_p:f2} мм");
        }
        else
        {
            body.AddParagraph()
                .AppendEquation("s_(k.p)=max{1.06∙(10^-2∙D_E)/(B_1)∙(p/(10^-5∙E)∙l_E/D_E)^0.4;(1.2∙p∙D_k)/(2∙φ_p∙[σ]-p)(1/cosα_1)}");
            body.AddParagraph("Эффективные размеры конической обечайки вычисляют по формулам (предварительно принимаем ")
                .AppendEquation($"s_k={dataIn.s} мм")
                .AddRun("):");
            body.AddParagraph()
                .AppendEquation("l_E=(D-D_1)/(2∙sinα_1)" + $"=({dataIn.D}-{dataIn.D1})/(2∙sin{MathHelper.RadiansToDegree(cdc.alpha1):f0})={data.lE:f2} мм");
            body.AddParagraph()
                .AppendEquation("D_E=max{(D+D_1)/(2∙cosα_1);D/cosα_1-0.3∙(D+D_1)∙√((D+D_1)/((s_k-c)∙100))∙tgα_1}");
            body.AddParagraph()
                .AppendEquation("(D+D_1)/(2∙cosα_1)" + $"=({dataIn.D}+{dataIn.D1})/(2∙cos{MathHelper.RadiansToDegree(cdc.alpha1):f0})={data.DE_1:f2}");
            body.AddParagraph()
                .AppendEquation("D/cosα_1-0.3∙(D+D_1)∙√((D+D_1)/((s_k-c)∙100))∙tgα_1" +
                                $"={dataIn.D}/cos{MathHelper.RadiansToDegree(cdc.alpha1):f0}-0.3∙({dataIn.D}+{dataIn.D1})∙√(({dataIn.D}+{dataIn.D1})/(({dataIn.s}-{cdc.c:f2})∙100))∙tg{MathHelper.RadiansToDegree(cdc.alpha1):f0}={data.DE_2:f2}");
            body.AddParagraph()
                .AppendEquation($"D_E=max{{{data.DE_1:f2};{data.DE_2:f2}}}={data.DE:f2} мм");
            body.AddParagraph("Коэффициент ")
                .AppendEquation("B_1")
                .AddRun(" вычисляют по формуле:");
            body.AddParagraph()
                .AppendEquation("B_1=min{1.0;9.45∙D_E/l_E∙√(D_E/((s_k-c)∙100))}");
            body.AddParagraph()
                .AppendEquation("9.45∙D_E/l_E∙√(D_E/((s_k-c)∙100))" +
                                $"9.45∙{data.DE:F2}/{data.lE:f2}∙√({data.DE:f2}/(({dataIn.s}-{cdc.c:f2})∙100))={data.B1_1:f2}");
            body.AddParagraph()
                .AppendEquation($"B_1=min{{1;{data.B1_1:f2}}}={data.B1:f2}");
            body.AddParagraph()
                .AppendEquation($"1.06∙(10^-2∙{data.DE:f2})/{data.B1:f2}∙({loadingCondition.p}/(10^-5∙{loadingCondition.EAllow})∙{data.lE:f2}/{data.DE:f2})^0.4={data.s_p_1:f2}");
            body.AddParagraph()
                .AppendEquation($"(1.2∙{loadingCondition.p}∙{cdc.Dk:f2})/(2∙{dataIn.phi}∙{data.SigmaAllow}-{loadingCondition.p})(1/cos{MathHelper.RadiansToDegree(cdc.alpha1):f0})={data.s_p_2:f2}");
            body.AddParagraph()
                .AppendEquation($"s_p=max{{{data.s_p_1:f2};{data.s_p_2:f2}}}={data.s_p:f2} мм");
        }

        body.AddParagraph()
            .AppendEquation($"s_k={data.s_p:f2}+{cdc.c:f2}={data.s:f2} мм");

        WordHelpers.CheckCalculatedThickness("s_k", dataIn.s, data.s_p, body);

        if (loadingCondition.PressureType == PressureType.Inside)
        {
            body.AddParagraph("Допускаемое внутреннее избыточное давление вычисляют по формуле:");
            body.AddParagraph()
                .AppendEquation("[p]=(2∙[σ]∙φ_p∙(s_k-c))/(D_k/cosα_1+(s_k-c))"
                                + $"=(2∙{data.SigmaAllow}∙{dataIn.phi}∙({dataIn.s}-{cdc.c:f2}))/({cdc.Dk:f2}/cos{MathHelper.RadiansToDegree(cdc.alpha1):f0}+({dataIn.s}-{cdc.c:f2}))={data.p_d:f2} МПа");
        }
        else
        {
            body.AddParagraph("Допускаемое наружное давление вычисляют по формуле:");
            body.AddParagraph()
                .AppendEquation("[p]=[p]_П/√(1+([p]_П/[p]_E)^2)");
            body.AddParagraph("допускаемое давление из условия прочности вычисляют по формуле:");
            body.AddParagraph()
                .AppendEquation("[p]_П=(2∙[σ]∙φ_p∙(s_k-c))/(D_k/cosα_1+(s_k-c))" +
                                $"=(2∙{data.SigmaAllow}∙{dataIn.phi}∙({dataIn.s}-{cdc.c:f2}))/({cdc.Dk:f2}/cos{MathHelper.RadiansToDegree(cdc.alpha1):f0}+({dataIn.s}-{cdc.c:f2}))={data.p_dp:f2} МПа");
            body.AddParagraph("допускаемое давление из условия устойчивости в пределах упругости вычисляют по формуле:");
            body.AddParagraph()
                .AppendEquation("[p]_E=(2.08∙10^-5∙E)/(n_y∙B_1)∙D_E/l_E∙[(100∙(s_k-c))/D_E]^2.5" +
                                $"=(2.08∙10^-5∙{loadingCondition.EAllow})/({dataIn.ny}∙{data.B1:f2})∙{data.DE:f2}/{data.lE:f2}∙[(100∙({dataIn.s}-{cdc.c:f2}))/{data.DE:f2}]^2.5={data.p_de:f2} МПа");
            body.AddParagraph()
                .AppendEquation($"[p]={data.p_dp:f2}/√(1+({data.p_dp:f2}/{data.p_de:f2})^2)={data.p_d:f2} МПа");
        }

        WordHelpers.CheckCalculatedPressure(loadingCondition.p, data.p_d, body);

        body.AddParagraph("Условия применения расчетных формул ");

        if (loadingCondition.PressureType == PressureType.Outside)
        {
            body.AddParagraph()
                .AppendEquation($"α_1={MathHelper.RadiansToDegree(cdc.alpha1):f0}≤70");
        }

        body.AddParagraph()
            .AppendEquation(
                $"0.001≤(s∙cosα_1)/D=({dataIn.s}∙cos{MathHelper.RadiansToDegree(cdc.alpha1):f0})/{dataIn.D}={data.ConditionUseFormulas:f3}≤0.05");

        if (!data.IsConditionUseFormulas)
        {
            body.AddParagraph("Условия применения расчетных формул не выполняется ")
                .Bold()
                .Color(System.Drawing.Color.Red);
        }

        if (dataIn.ConnectionType != ConicalConnectionType.WithoutConnection)
        {
            body.AddParagraph();
            body.AddParagraph("Расчет узла соединения цилиндрической обечайки большего диаметра с конической обечайкой")
                .Alignment(AlignmentType.Center);

            switch (dataIn.ConnectionType)
            {
                case ConicalConnectionType.Simply:
                    body.AddParagraph("Соединение обечайки без тороидального перехода")
                        .Alignment(AlignmentType.Center);
                    body.AddParagraph("Толщину стенки из условия прочности переходной зоны вычисляют по формулам: ");
                    body.AddParagraph()
                        .AppendEquation("s_2≥s_2p+c");
                    body.AddParagraph()
                        .AppendEquation("s_2p=(p∙D∙β_1)/(2∙[σ]_2∙φ_p-p)");
                    body.AddParagraph("где ")
                        .AppendEquation("β_1")
                        .AddRun(" - коэффициент формы вычисляют по формуле:");
                    body.AddParagraph()
                        .AppendEquation("β_1=max{0.5;β}");
                    body.AddParagraph()
                        .AppendEquation(
                            "β=0.4√(D/(s_2-c))∙tgα_1/(1+√((1+χ_1∙((s_1-c)/(s_2-c))^2)/(2∙cosα_1)χ_1∙(s_1-c)/(s_2-c)))-0.25");
                    body.AddParagraph()
                        .AppendEquation("χ_1=[σ]_1/[σ]_2" +
                                        $"={data.SigmaAllow1Big}/{data.SigmaAllow2Big}={data.chi_1Big:f2}");
                    body.AddParagraph()
                        .AppendEquation(
                            $"β=0.4√({dataIn.D}/({dataIn.s2Big}-{cdc.c:f2}))∙tg{MathHelper.RadiansToDegree(cdc.alpha1):f0}/(1+√((1+{data.chi_1Big:f2} (({dataIn.s1Big}-{cdc.c:f2})/({dataIn.s2Big}-{cdc.c:f2}))^2)/(2∙cos{MathHelper.RadiansToDegree(cdc.alpha1):f0}){data.chi_1Big:f2}({dataIn.s1Big}-{cdc.c:f2})/({dataIn.s2Big}-{cdc.c:f2})))-0.25={data.beta:f2}");
                    body.AddParagraph()
                        .AppendEquation($"β_1=max{{0.5;{data.beta:f2}}}={data.beta_1:f2}");
                    body.AddParagraph()
                        .AppendEquation(
                            $"s_2p=({loadingCondition.p}∙{dataIn.D}∙{data.beta_1:f2})/(2∙{data.SigmaAllow}∙{dataIn.phi}-{loadingCondition.p})={data.s_2pBig:f2} мм");

                    body.AddParagraph()
                        .AppendEquation($"s_2={data.s_2pBig:f2}+{cdc.c:f2}={data.s_2Big:f2} мм");

                    WordHelpers.CheckCalculatedThickness("s_2", dataIn.s2Big, data.s_2Big, body);

                    body.AddParagraph()
                        .AppendEquation("s_1=(s_1-c)/(s_2-c)s_2p+c=" +
                                    $"({dataIn.s1Big}-{cdc.c})/({dataIn.s2Big}-{cdc.c}){data.s_2pBig:f2}+{cdc.c:f2}={data.s_1Big:f2} мм");

                    WordHelpers.CheckCalculatedThickness("s_1", dataIn.s1Big, data.s_1Big, body);

                    body.AddParagraph("Допускаемое " +
                                      (loadingCondition.PressureType == PressureType.Inside ? "внутреннее избыточное" : "наружное") +
                                      "давление из условия прочности переходной части вычисляют по формуле");
                    body.AddParagraph()
                        .AppendEquation("[p]=(2∙[σ]_2∙φ_p∙(s_2-c))/(D∙β_1+(s_2-c))" +
                                        $"=(2∙{data.SigmaAllow2Big}∙{dataIn.phi}∙({dataIn.s2Big}-{cdc.c:f2}))/({dataIn.D}∙{data.beta_1:f2}+({dataIn.s2Big}-{cdc.c:f2}))={data.p_dBig:f2} МПа");
                    break;
                case ConicalConnectionType.WithRingPicture25b:
                    body.AddParagraph("Соединение обечайки с укрепляющим кольцом")
                        .Alignment(AlignmentType.Center);
                    body.AddParagraph("Площадь поперечного сечения укрепляющего кольца вычисляют по формуле");
                    body.AddParagraph()
                        .AppendEquation("A_K=(p∙D^2∙tgα_1)/(8∙[σ]_K∙φ_K)(1-(β_a+0.25)/(β+0.25))");
                    body.AddParagraph("где ");
                    body.AddParagraph()
                        .AppendEquation("β_a=((2∙[σ]_2∙φ_p)/p-1)(s_2-c)/D" +
                                        $"((2∙{data.SigmaAllow2Big}∙{dataIn.phi})/{loadingCondition.p}-1)∙({dataIn.s2Big}-{cdc.c})/{dataIn.D}={data.beta_a:f2}");
                    body.AddParagraph()
                        .AppendEquation(
                            "β=0.4√(D/(s_2-c))∙tgα_1/(1+√((1+χ_1∙((s_1-c)/(s_2-c))^2)/(2∙cosα_1)χ_1∙(s_1-c)/(s_2-c)))-0.25");
                    body.AddParagraph()
                        .AppendEquation("χ_1=[σ]_1/[σ]_2" +
                                        $"={data.SigmaAllow1Big}/{data.SigmaAllow2Big}={data.chi_1Big:f2}");
                    body.AddParagraph()
                        .AppendEquation(
                            $"β=0.4√({dataIn.D}/({dataIn.s2Big}-{cdc.c:f2}))∙tg{MathHelper.RadiansToDegree(cdc.alpha1):f0}/(1+√((1+{data.chi_1Big:f2} (({dataIn.s1Big}-{cdc.c:f2})/({dataIn.s2Big}-{cdc.c:f2}))^2)/(2∙cos{MathHelper.RadiansToDegree(cdc.alpha1)}){data.chi_1Big:f2}({dataIn.s1Big}-{cdc.c:f2})/({dataIn.s2Big}-{cdc.c:f2})))-0.25={data.beta:f2}");

                    body.AddParagraph()
                        .AppendEquation($"A_K=({loadingCondition.p}∙{dataIn.D}^2∙tg{MathHelper.RadiansToDegree(cdc.alpha1):f0})/(8∙{data.SigmaAllowC}∙{dataIn.phi_k})∙(1-({data.beta_a:f2}+0.25)/({data.beta:f2}+0.25))={data.Ak:f2} мм^2");

                    ReinforcementRingSquareCheck(dataIn, data, body);

                    body.AddParagraph("Допускаемое " +
                                      (loadingCondition.PressureType == PressureType.Inside ? "внутреннее избыточное" : "наружное") +
                                      "давление из условия прочности переходной части вычисляют по формуле");
                    body.AddParagraph()
                        .AppendEquation("[p]=(2∙[σ]_2∙φ_p∙(s_2-c))/(D∙β_2+(s_2-c))");
                    body.AddParagraph("Общий коэффициент формы для переходной части равен:");
                    body.AddParagraph()
                        .AppendEquation("β_2=max{0.5;∙β_0}");
                    body.AddParagraph()
                        .AppendEquation(
                            "β_0=(0.4√(D/(s_2-c))∙tgα_1-B_3[1+√((1+χ_1∙((s_1-c)/(s_2-c))^2)/(2∙cosα_1)χ_1∙(s_1-c)/(s_2-c))])/(B_2+[1+√((1+χ_1∙((s_1-c)/(s_2-c))^2)/(2∙cosα_1)χ_1∙(s_1-c)/(s_2-c)))");
                    body.AddParagraph()
                        .AppendEquation("B_2=(1.6∙A_K)/((s_2-c)√(D∙(s_2-c)))∙([σ]_K∙φ_K)/([σ]_2∙φ_T)" +
                                        $"(1.6∙{dataIn.Ak})/(({dataIn.s2Big} - {cdc.c})∙√({dataIn.D}∙({dataIn.s2Big}-{cdc.c})))∙({data.SigmaAllowC}∙{dataIn.phi_k})/({data.SigmaAllow2Big}∙{dataIn.phi_t})={data.B2:f2}");
                    body.AddParagraph()
                        .AppendEquation("B_3=0.25");

                    body.AddParagraph()
                        .AppendEquation(
                            $"β_0=(0.4√({dataIn.D}/({dataIn.s2Big}-{cdc.c}))∙tg{MathHelper.RadiansToDegree(cdc.alpha1):f0}-{data.B3}[1+√((1+{data.chi_1Big}(({dataIn.s1Big}-{cdc.c})/({dataIn.s2Big}-{cdc.c}))^2)/(2∙cos{MathHelper.RadiansToDegree(cdc.alpha1):f0}){data.chi_1Big}({dataIn.s1Big}-{cdc.c})/({dataIn.s2Big}-{cdc.c}))])/" +
                            $"({data.B2:f2}+[1+√((1+{data.chi_1Big}(({dataIn.s1Big}-{cdc.c})/({dataIn.s2Big}-{cdc.c}))^2)/(2∙cos{MathHelper.RadiansToDegree(cdc.alpha1):f0}){data.chi_1Big}({dataIn.s1Big}-{cdc.c})/({dataIn.s2Big}-{cdc.c}))])={data.beta_0:f2}");

                    body.AddParagraph()
                        .AppendEquation($"β_2=max{{0.5;{data.beta_0:f2}}}={data.beta_2:f2}");

                    body.AddParagraph()
                        .AppendEquation(
                            $"[p]=(2∙{data.SigmaAllow2Big}∙{dataIn.phi}∙({dataIn.s2Big}-{cdc.c}))/({dataIn.D}∙{data.beta_2:f2}+({dataIn.s2Big}-{cdc.c}))={data.p_dBig:f2} МПа");
                    break;
                case ConicalConnectionType.WithRingPicture29:
                    body.AddParagraph("Соединение обечайки с укрепляющим кольцом")
                        .Alignment(AlignmentType.Center);
                    body.AddParagraph("Площадь поперечного сечения укрепляющего кольца вычисляют по формуле");
                    body.AddParagraph()
                        .AppendEquation("A_K=(p∙D^2∙tgα_1)/(8∙[σ]_K∙φ_K)=" +
                                        $"({loadingCondition.p}∙{dataIn.D}^2∙tg{MathHelper.RadiansToDegree(cdc.alpha1):f0})/(8∙{data.SigmaAllowC}∙{dataIn.phi_k})={data.Ak:f2} мм^2");

                    ReinforcementRingSquareCheck(dataIn, data, body);

                    body.AddParagraph("Допускаемое " +
                                      (loadingCondition.PressureType == PressureType.Inside ? "внутреннее избыточное" : "наружное") +
                                      "давление из условия прочности переходной части вычисляют по формуле");
                    body.AddParagraph()
                        .AppendEquation("[p]=(8∙[σ]_K∙φ_K)/(D^2∙tgα_1)" +
                                        $"(8∙{data.SigmaAllowC}∙{dataIn.phi_k})/({dataIn.D}^2∙tg{MathHelper.RadiansToDegree(cdc.alpha1):f0})={data.p_dBig:f2} МПа");
                    break;
                case ConicalConnectionType.Toroidal:
                    body.AddParagraph("Соединение обечаек с тороидальным переходом")
                        .Alignment(AlignmentType.Center);
                    body.AddParagraph("Толщину стенки переходной части вычисляют по формуле");

                    body.AddParagraph()
                        .AppendEquation("s_T≥s_(T∙p)+c");
                    body.AddParagraph()
                        .AppendEquation("s_(T∙p)=(p∙D∙β_3)/(2∙φ_p∙[σ]_2-p)");
                    body.AddParagraph("где ")
                        .AppendEquation("β_3")
                        .AddRun(" - коэффициент формы вычисляют по формуле:");
                    body.AddParagraph()
                        .AppendEquation("β_3=max{0.5;β∙β_T}");
                    body.AddParagraph()
                        .AppendEquation(
                            "β=0.4√(D/(s_2-c))∙tgα_1/(1+√((1+χ_1∙((s_1-c)/(s_2-c))^2)/(2∙cosα_1)χ_1∙(s_1-c)/(s_2-c)))-0.25");
                    body.AddParagraph()
                        .AppendEquation("χ_1=[σ]_1/[σ]_2" +
                                        $"={data.SigmaAllow1Big}/{data.SigmaAllow2Big}={data.chi_1Big:f2}");
                    body.AddParagraph()
                        .AppendEquation(
                            $"β=0.4√({dataIn.D}/({dataIn.s2Big}-{cdc.c:f2}))∙tg{MathHelper.RadiansToDegree(cdc.alpha1):f0}/(1+√((1+{data.chi_1Big:f2} (({dataIn.s1Big}-{cdc.c:f2})/({dataIn.s2Big}-{cdc.c:f2}))^2)/(2∙cos{MathHelper.RadiansToDegree(cdc.alpha1)}){data.chi_1Big:f2}({dataIn.s1Big}-{cdc.c:f2})/({dataIn.s2Big}-{cdc.c:f2})))-0.25={data.beta:f2}");
                    body.AddParagraph()
                        .AppendEquation(
                            "β_T=1/(1+(0.028∙α_1∙r/D∙√(D/(s_T-c)))/(1/√cosα_1+1" +
                            $"=1/(1+(0.028∙{MathHelper.RadiansToDegree(cdc.alpha1):f0}∙{dataIn.r}/{dataIn.D}∙√({dataIn.D}/({dataIn.sT}-{cdc.c})))/(1/√cos{MathHelper.RadiansToDegree(cdc.alpha1):f0}+1={data.beta_t:f2}");
                    body.AddParagraph()
                        .AppendEquation($"β_3=max{{0.5;{data.beta:f2}∙{data.beta_t:f2}}}=max{{0.5;{data.beta_3_2:f2}}}={data.beta_3:f2}");
                    body.AddParagraph()
                        .AppendEquation(
                            $"s_(T∙p)=({loadingCondition.p}∙{dataIn.D}∙{data.beta_3:f2})/(2∙{dataIn.phi}∙{data.SigmaAllowT}-{loadingCondition.p})={data.s_tp:f2} мм");

                    body.AddParagraph().AppendEquation($"s_T={data.s_tp:f2}+{cdc.c:f2}={data.s_t:f2} мм");

                    WordHelpers.CheckCalculatedThickness("s_T", dataIn.sT, data.s_t, body);

                    body.AddParagraph("Допускаемое " +
                                      (loadingCondition.PressureType == PressureType.Inside ? "внутреннее избыточное" : "наружное") +
                                      "давление из условия прочности переходной части вычисляют по формуле");
                    body.AddParagraph()
                        .AppendEquation("[p]=(2∙[σ]∙φ_p∙(s_T-c))/(D∙β_3+(s_T-c))" +
                                        $"=(2∙{data.SigmaAllowT}∙{dataIn.phi}∙({dataIn.sT}-{cdc.c:f2}))/({dataIn.D}∙{data.beta_3:f2}+({dataIn.sT}-{cdc.c:f2}))={data.p_dBig:f2} МПа");

                    break;
                default:
                    throw new ArgumentOutOfRangeException();

            }

            WordHelpers.CheckCalculatedPressure(loadingCondition.p, data.p_dBig, body);

            body.AddParagraph("Условия применения расчетных формул");

            body.AddParagraph()
                .AppendEquation($"α_1={MathHelper.RadiansToDegree(cdc.alpha1):f0}≤70");

            switch (dataIn.ConnectionType)
            {
                case ConicalConnectionType.Simply:
                case ConicalConnectionType.WithRingPicture25b:
                    body.AddParagraph()
                        .AppendEquation($"(s_1-c)=({dataIn.s1Big}-{cdc.c})={dataIn.s1Big - cdc.c}≥(s_2-c)=({dataIn.s2Big}-{cdc.c})={dataIn.s2Big - cdc.c}");
                    break;
                case ConicalConnectionType.Toroidal:
                    body.AddParagraph()
                        .AppendEquation($"0≤r/D={dataIn.r}/{dataIn.D}={data.ConditionUseFormulasToroidal:f2}<0.3");
                    break;
            }
            if (!data.IsConditionUseFormulasBigConnection)
            {
                body.AddParagraph("Условия применения расчетных формул не выполняется")
                    .Bold()
                    .Color(System.Drawing.Color.Red);
            }
        }

        //little connection
        if (dataIn.IsConnectionWithLittle)
        {
            body.AddParagraph();
            body.AddParagraph("Расчет узла соединение штуцера или цилиндрической обечайки меньшего диаметра с конической обечайкой")
                .Alignment(AlignmentType.Center);
            body.AddParagraph("Толщину стенки вычисляют по формуле: ");
            body.AddParagraph()
                .AppendEquation("s_2≥s_2p+c");
            body.AddParagraph()
                .AppendEquation("s_2p=(p∙D_1∙β_4)/(2∙φ_p∙[σ]_2-p)");
            body.AddParagraph("где ")
                .AppendEquation("β_4")
                .AddRun(" - коэффициент формы вычисляют по формуле:");
            body.AddParagraph()
                .AppendEquation("β_4=max{1;β_H}");

            body.AddParagraph()
                .AppendEquation("χ_1=[σ]_1/[σ]_2" +
                                $"={data.SigmaAllow1Little}/{data.SigmaAllow2Little}={data.chi_1Little:f2}");

            if (data.ConditionForBetaH >= 1)
            {
                body.AddParagraph()
                    .AppendEquation("При χ_1∙((s_1-c)/(s_2-c))^2" +
                    $"={data.chi_1Little:f2}(({dataIn.s1Little}-{cdc.c:f2})/({dataIn.s2Little}-{cdc.c:f2}))^2={data.ConditionForBetaH:f2}≥1");

                body.AddParagraph()
                    .AppendEquation("β_H=β+0.75");

                body.AddParagraph()
                    .AppendEquation(
                        "β=0.4√(D_1/(s_2-c))∙tgα_1/(1+√((1+χ_1∙((s_1-c)/(s_2-c))^2)/(2∙cosα_1)χ_1∙(s_1-c)/(s_2-c)))-0.25");

                body.AddParagraph()
                    .AppendEquation(
                        $"β=0.4√({dataIn.D1}/({dataIn.s2Little}-{cdc.c:f2}))∙tg{MathHelper.RadiansToDegree(cdc.alpha1):f0}/(1+√((1+{data.chi_1Little:f2} (({dataIn.s1Little}-{cdc.c:f2})/({dataIn.s2Little}-{cdc.c:f2}))^2)/(2∙cos{MathHelper.RadiansToDegree(cdc.alpha1):f0}){data.chi_1Little:f2}({dataIn.s1Little}-{cdc.c:f2})/({dataIn.s2Little}-{cdc.c:f2})))-0.25={data.betaLittle:f2}");

                body.AddParagraph()
                    .AppendEquation($"β_H={data.betaLittle:f2}+0.75={data.beta_H:f2}");
            }
            else
            {
                body.AddParagraph()
                    .AppendEquation("При χ_1∙((s_1-c)/(s_2-c))^2" +
                                    $"={data.chi_1Little:f2}(({dataIn.s1Little}-{cdc.c:f2})/({dataIn.s2Little}-{cdc.c:f2}))^2={data.ConditionForBetaH:f2}<1");

                body.AddParagraph()
                    .AppendEquation(
                        "β_H=0.4√(D_1/(s_2-c))∙tgα_1/(χ_1∙(s_1-c)/(s_2-c)∙√((s_1-c)/((s_2-c)∙cosα_1))+√((1+χ_1∙((s_1-c)/(s_2-c))^2)/2)+0.25");

                body.AddParagraph()
                    .AppendEquation(
                        $"β_H=0.4√({dataIn.D1}/({dataIn.s2Little}-{cdc.c:f2}))∙tg{MathHelper.RadiansToDegree(cdc.alpha1):f0}/({data.chi_1Little}({dataIn.s1Little}-{cdc.c})/({dataIn.s2Little}-{cdc.c})∙√(({dataIn.s1Little}-{cdc.c})/(({dataIn.s2Little}-{cdc.c})∙cos{MathHelper.RadiansToDegree(cdc.alpha1):f0}))+√((1+{data.chi_1Little:f2}(({dataIn.s1Little}-{cdc.c:f2})/({dataIn.s2Little}-{cdc.c:f2}))^2)/2)+0.5={data.beta_H:f2}");

                body.AddParagraph()
                    .AppendEquation($"β_H={data.betaLittle:f2}+0.75={data.beta_H:f2}");
            }

            body.AddParagraph()
                  .AppendEquation($"β_4=max{{1;{data.beta_H:f2}}}={data.beta_4:f2}");
            body.AddParagraph()
                .AppendEquation(
                    $"s_2p=({loadingCondition.p}∙{dataIn.D1}∙{data.beta_4:f2})/(2∙{dataIn.phi}∙{data.SigmaAllow2Little}-{loadingCondition.p})={data.s_2pLittle:f2} мм");

            body.AddParagraph().AppendEquation($"s_2={data.s_2pLittle:f2}+{cdc.c:f2}={data.s_2Little:f2} мм");

            WordHelpers.CheckCalculatedThickness("s_2", dataIn.s2Little, data.s_2Little, body);

            body.AddParagraph()
                .AppendEquation("s_1=(s_1-c)/(s_2-c)s_2p+c=" +
                                $"({dataIn.s1Little}-{cdc.c})/({dataIn.s2Little}-{cdc.c}){data.s_2pLittle:f2}+{cdc.c:f2}={data.s_1Little:f2} мм");

            WordHelpers.CheckCalculatedThickness("s_1", dataIn.s1Little, data.s_1Little, body);

            body.AddParagraph("Допускаемое " +
                              (loadingCondition.PressureType == PressureType.Inside ? "внутреннее избыточное" : "наружное") +
                              "давление из условия прочности переходной части вычисляют по формуле");
            body.AddParagraph()
                .AppendEquation("[p]=(2∙[σ]_2∙φ_p∙(s_2-c))/(D_1∙β_4+(s_2-c))" +
                                $"=(2∙{data.SigmaAllow2Little}∙{dataIn.phi}∙({dataIn.s2Little}-{cdc.c:f2}))/({dataIn.D1}∙{data.beta_4:f2}+({dataIn.s2Little}-{cdc.c:f2}))={data.p_dLittle:f2} МПа");

            WordHelpers.CheckCalculatedPressure(loadingCondition.p, data.p_dLittle, body);


            body.AddParagraph("Условия применения расчетных формул");

            body.AddParagraph()
                .AppendEquation($"α_1={MathHelper.RadiansToDegree(cdc.alpha1):f0}≤70");

            if (!data.IsConditionUseFormulasLittleConnection)
            {
                body.AddParagraph("Условия применения расчетных формул не выполняется")
                    .Bold()
                    .Color(System.Drawing.Color.Red);
            }
        }
    }

    private static void ReinforcementRingSquareCheck(ConicalShellInput dataIn, ConicalShellCalculatedOneLoading data, Body body)
    {
        if (data.Ak <= 0)
        {
            body.AddParagraph("При ")
                .AppendEquation("A_K≤0")
                .AddRun(" укрепление кольцом жесткости не требуется.");
        }
        else
        {
            body.AddParagraph()
                .AppendEquation($"{dataIn.Ak} мм^2≥{data.Ak} мм^2");

            if (dataIn.Ak > data.Ak)
            {
                body.AddParagraph("Площадь укрепляющего кольца достаточна.")
                    .Bold();
            }
            else
            {
                body.AddParagraph("Площадь укрепляющего кольца недостаточна.")
                    .Bold()
                    .Color(System.Drawing.Color.Red);
            }
        }
    }
}