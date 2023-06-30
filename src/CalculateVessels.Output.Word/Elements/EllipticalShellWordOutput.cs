using CalculateVessels.Core.Elements.Shells.Elliptical;
using CalculateVessels.Core.Elements.Shells.Enums;
using CalculateVessels.Core.Enums;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Output.Word.Core;
using CalculateVessels.Output.Word.Enums;
using CalculateVessels.Output.Word.Helpers;
using CalculateVessels.Output.Word.Interfaces;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace CalculateVessels.Output.Word.Elements;

internal class EllipticalShellWordOutput : IWordOutputElement<EllipticalShellCalculated>
{
    public void MakeWord(string filePath, ICalculatedElement calculatedData)
    {
        if (calculatedData is not EllipticalShellCalculated data)
            throw new NullReferenceException();

        var dataIn = (EllipticalShellInput)data.InputData;

        filePath = WordHelpers.CheckFilePath(filePath);

        using var package = WordprocessingDocument.Open(filePath, true);

        var mainPart = package.MainDocumentPart ?? throw new InvalidOperationException();
        var body = mainPart.Document.Body ?? throw new InvalidOperationException();

        // TODO: добавить полусферическое

        //body.AddParagraph("").InsertPageBreakAfterSelf();

        body.AddParagraph($"Расчет на прочность эллиптического днища {dataIn.Name}") //, нагруженного (dataIn.IsPressureIn ? "внутренним избыточным давлением" : "наружным давлением"))
            .Heading(HeadingType.Heading1)
            .Alignment(AlignmentType.Center);

        body.AddParagraph();

        MakeImage(mainPart);

        MakeInputDataTable(body, dataIn, data);

        dataIn.LoadingConditions
            .ToList()
            .ForEach(lc => MakeCalculateResult(body,
                data.Results
                    .First(r => r.LoadingConditionId == lc.Id),
                data.CommonData,
                dataIn));

        MakeCheckConditionsUseFormulas(body, dataIn.D, dataIn.s, dataIn.EllipseH, data.CommonData.c, data.CommonData.IsConditionUseFormulas);

        package.Dispose();
    }

    private static void MakeCheckConditionsUseFormulas(Body body, double D, double s, double ellipseH, double c, bool isConditionUseFormulas)
    {
        body.AddParagraph("Условия применения расчетных формул");

        // elliptical bottom
        body.AddParagraph()
            .AppendEquation("0.002≤(s_1-c)/(D)" +
                            $"=({s}-{c:f2})/({D})={(s - c) / D:f3}≤0.1");
        body.AddParagraph()
            .AppendEquation($"0.2≤H/D={ellipseH}/{D}={ellipseH / D:f3}<0.5");

        if (!isConditionUseFormulas)
        {
            body.AddParagraph("Условия применения расчетных формул не выполняется ")
                .Bold()
                .Color(System.Drawing.Color.Red);
        }
    }

    private static void MakeCalculateResult(Body body, EllipticalShellCalculatedOneLoading data, EllipticalShellCalculatedCommon cdc, EllipticalShellInput dataIn)
    {
        var loadingCondition = dataIn.LoadingConditions.First(lc => lc.Id == data.LoadingConditionId);

        body.AddParagraph();
        body.AddParagraph($"Результаты расчета #{loadingCondition.Id}")
            .Alignment(AlignmentType.Center);
        body.AddParagraph();
        body.AddParagraph("Толщину стенки вычисляют по формуле:");
        body.AddParagraph()
            .AppendEquation("s_1≥s_1p+c");
        body.AddParagraph("где ")
            .AppendEquation("s_1p")
            .AddRun(" - расчетная толщина стенки днища");

        body.AddParagraph()
            .AppendEquation(loadingCondition.PressureType == PressureType.Inside
                ? "s_1p=(p∙R)/(2∙[σ]∙φ-0.5∙p)"
                : "s_1p=max{(K_Э∙R)/(161)∙√((n_y∙p)/(10^-5∙E));(1.2∙p∙R)/(2∙[σ])}");
        body.AddParagraph("где R - радиус кривизны в вершине днища");

        // TODO: добавить расчет R для разных ситуаций

        switch (dataIn.EllipticalBottomType)
        {
            case EllipticalBottomType.Elliptical when Math.Abs(dataIn.D - cdc.EllipseR) < 0.00001:
                body.AddParagraph($"R=D={dataIn.D} мм - для эллиптических днищ с H=0.25D");
                break;
            case EllipticalBottomType.Hemispherical when Math.Abs(0.5 * dataIn.D - cdc.EllipseR) < 0.00001:
                body.AddParagraph()
                    .AppendEquation($"R=0.5∙D={dataIn.D} мм")
                    .AddRun(" - для полусферических днищ с H=0.5D");
                break;
            default:
                body.AddParagraph()
                    .AppendEquation($"R=D^2/(4∙H)={dataIn.D}^2/(4∙{dataIn.EllipseH})={cdc.EllipseR:f2} мм");
                break;
        }

        if (loadingCondition.PressureType == PressureType.Inside)
        {
            body.AddParagraph()
                .AppendEquation($"s_p=({loadingCondition.p}∙{cdc.EllipseR:f2})/(2∙{data.SigmaAllow}∙{dataIn.phi}-0.5{loadingCondition.p})={data.s_p:f2} мм");
        }
        else
        {
            body.AddParagraph("Для предварительного расчета ")
                .AppendEquation($"К_Э={data.EllipseKePrev}")
                .AddRun(dataIn.EllipticalBottomType == EllipticalBottomType.Elliptical
                    ? " для эллиптических днищ"
                    : " для полусферических днищ");
            body.AddParagraph()
                .AppendEquation($"({data.EllipseKePrev}∙{cdc.EllipseR:f2})/(161)∙√(({dataIn.ny}∙{loadingCondition.p})/(10^-5∙{data.E}))=" +
                                $"{data.s_p_1:f2}");
            body.AddParagraph()
                .AppendEquation($"(1.2∙{loadingCondition.p}∙{cdc.EllipseR:f2})/(2∙{data.SigmaAllow})={data.s_p_2:f2}");
            body.AddParagraph()
                .AppendEquation($"s_1p=max({data.s_p_1:f2};{data.s_p_2:f2})={data.s_p:f2} мм");
        }
        body.AddParagraph("c - сумма прибавок к расчетной толщине");
        body.AddParagraph()
            .AppendEquation($"c=c_1+c_2+c_3={dataIn.c1}+{dataIn.c2}+{dataIn.c3}={cdc.c:f2} мм");

        body.AddParagraph()
            .AppendEquation($"s={data.s_p:f2}+{cdc.c:f2}={data.s:f2} мм");

        WordHelpers.CheckCalculatedThickness("s_1", dataIn.s, data.s, body);

        if (loadingCondition.PressureType == PressureType.Inside)
        {
            body.AddParagraph("Допускаемое внутреннее избыточное давление вычисляют по формуле:");
            body.AddParagraph()
                .AppendEquation("[p]=(2∙[σ]∙φ∙(s_1-c))/(R+0.5∙(s-c))" +
                                $"=(2∙{data.SigmaAllow}∙{dataIn.phi}∙({dataIn.s}-{cdc.c:f2}))/" +
                                $"({cdc.EllipseR:f2}+0.5∙({dataIn.s}-{cdc.c:f2}))={data.p_d:f2} МПа");
        }
        else
        {
            body.AddParagraph("Допускаемое наружное давление вычисляют по формуле:");
            body.AddParagraph()
                .AppendEquation("[p]=[p]_П/√(1+([p]_П/[p]_E)^2)");
            body.AddParagraph("допускаемое давление из условия прочности вычисляют по формуле:");
            body.AddParagraph()
                .AppendEquation("[p]_П=(2∙[σ]∙(s_1-c))/(R+0.5(s_1-c))" +
                                $"=(2∙{data.SigmaAllow}∙({dataIn.s}-{cdc.c:f2}))/({cdc.EllipseR}+0.5({dataIn.s}-{cdc.c:f2}))={data.p_dp:f2} МПа");
            body.AddParagraph("допускаемое давление из условия устойчивости в пределах упругости вычисляют по формуле:");
            body.AddParagraph()
                .AppendEquation("[p]_E=(2.6∙10^-5∙E)/n_y∙[(100∙(s_1-c))/(К_Э∙R)]^2");
            body.AddParagraph("коэффициент ")
                .AppendEquation("К_Э")
                .AddRun(" вычисляют по формуле");
            body.AddParagraph()
                .AppendEquation("К_Э=(1+(2.4+8∙x)∙x)/(1+(3+10∙x)∙x)");
            body.AddParagraph()
                .AppendEquation($"x=10∙(s_1-c)/D∙(D/(2∙H)-(2∙H)/D)=10∙({dataIn.s - cdc.c:f2})/{dataIn.D}∙({dataIn.D}/(2∙{dataIn.EllipseH})-(2∙{dataIn.EllipseH})/{dataIn.D})={data.Ellipsex:f2}");
            body.AddParagraph()
                .AppendEquation($"К_Э=(1+(2.4+8∙{data.Ellipsex:f2})∙{data.Ellipsex:f2})/(1+(3+10∙{data.Ellipsex:f2})∙{data.Ellipsex:f2})={data.EllipseKe:f2}");
            body.AddParagraph()
                .AppendEquation($"[p]_E=(2.6∙10^-5∙{data.E})/{dataIn.ny}∙" +
                                $"[(100∙({dataIn.s}-{cdc.c:f2}))/({data.EllipseKe:f2}∙{cdc.EllipseR:f2})]^2={data.p_de:f2} МПа");
            body.AddParagraph()
                .AppendEquation($"[p]={data.p_dp:f2}/√(1+({data.p_dp:f2}/{data.p_de:f2})^2)={data.p_d:f2} МПа");
        }

        WordHelpers.CheckCalculatedPressure(loadingCondition.p, data.p_d, body);
    }

    private static void MakeInputDataTable(Body body, EllipticalShellInput dataIn, EllipticalShellCalculated data)
    {
        body.AddParagraph("Исходные данные")
            .Alignment(AlignmentType.Center);

        var table = body.AddTable();

        table.AddRow()
            .AddCell("Материал днища")
            .AddCell($"{dataIn.Steel}");

        table.AddRow()
            .AddCell("Внутренний диаметр днища, D:")
            .AddCell($"{dataIn.D} мм");

        table.AddRow()
            .AddCell("Высота выпуклой части, H:")
            .AddCell($"{dataIn.EllipseH} мм");

        table.AddRow()
            .AddCell("Длина отбортовки ")
            .AppendEquation("h_1")
            .AppendText(":")
            .AddCell($"{dataIn.Ellipseh1}");

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
                .AddCell($"{dataIn.c3}");
        }

        table.AddRow()
            .AddCell("Коэффициент прочности сварного шва, ")
            .AppendEquation("φ_p")
            .AppendText(":")
            .AddCell($"{dataIn.phi}");

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

    private static void MakeImage(MainDocumentPart mainPart)
    {
        mainPart.InsertImage(Data.Properties.Resources.Ell, ImagePartType.Gif);
    }
}