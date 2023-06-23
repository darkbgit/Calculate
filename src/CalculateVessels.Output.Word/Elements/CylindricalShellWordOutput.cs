using CalculateVessels.Core.Elements.Shells.Cylindrical;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Output.Word.Core;
using CalculateVessels.Output.Word.Enums;
using CalculateVessels.Output.Word.Helpers;
using CalculateVessels.Output.Word.Interfaces;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace CalculateVessels.Output.Word.Elements;

internal class CylindricalShellWordOutput : IWordOutputElement<CylindricalShellCalculated>
{
    public void MakeWord(string filePath, ICalculatedElement calculatedElement)
    {
        if (calculatedElement is not CylindricalShellCalculated data)
            throw new NullReferenceException();

        var dataIn = (CylindricalShellInput)data.InputData;

        filePath = WordHelpers.CheckFilePath(filePath);

        using var package = WordprocessingDocument.Open(filePath, true);

        var mainPart = package.MainDocumentPart ?? throw new InvalidOperationException();
        var body = mainPart.Document.Body ?? throw new InvalidOperationException();

        InsertHeader(body, dataIn);

        InsertImage(mainPart);

        InsertInputDataTable(body, dataIn, data);

        InsertCommonDataCalculated(body, dataIn, data.CommonData);

        InsertLoadingConditionsDataCalculated(body, dataIn, data);

        InsertCheckConditionsUseFormulas(body, dataIn, data.CommonData);

        package.Close();
    }

    private static void InsertLoadingConditionsDataCalculated(Body body, CylindricalShellInput dataIn, CylindricalShellCalculated data)
    {
        dataIn.LoadingConditions
            .ToList()
            .ForEach(lc => MakeCalculateResult(body, data.Results
                    .First(r => r.LoadingCondition.OrdinalNumber == lc.OrdinalNumber),
                data.CommonData,
                dataIn));
    }

    private static void InsertCommonDataCalculated(Body body, CylindricalShellInput dataIn, CylindricalShellCalculatedCommon cdc)
    {
        body.AddParagraph();
        body.AddParagraph("Расчетные параметры").Alignment(AlignmentType.Center);
        body.AddParagraph();

        body.AddParagraph("Сумма прибавок к расчетной толщине");
        body.AddParagraph()
            .AppendEquation($"c=c_1+c_2+c_3={dataIn.c1}+{dataIn.c2}+{dataIn.c3}={cdc.c:f2} мм");
    }

    private static void InsertHeader(Body body, CylindricalShellInput dataIn)
    {
        body.AddParagraph($"Расчет на прочность обечайки {dataIn.Name}") //, нагруженной (dataIn.IsPressureIn ? "внутренним избыточным давлением" : "наружным давлением")).Heading(HeadingType.Heading1);
            .Heading(HeadingType.Heading1)
            .Alignment(AlignmentType.Center);
    }

    private static void InsertImage(MainDocumentPart mainPart)
    {
        mainPart.InsertImage(Data.Properties.Resources.Cil, ImagePartType.Gif);
    }

    private static void InsertInputDataTable(Body body, CylindricalShellInput dataIn, CylindricalShellCalculated data)
    {
        body.AddParagraph("Исходные данные")
            .Alignment(AlignmentType.Center);

        var table = body.AddTable();
        //table.SetWidths(new float[] { 300, 100 });
        table.AddRow()
            .AddCell("Материал обечайки")
            .AddCell($"{dataIn.Steel}");

        table.AddRow()
            .AddCell("Внутренний диаметр обечайки, D:")
            .AddCell($"{dataIn.D} мм");

        if (dataIn.LoadingConditions.Any(lc => !lc.IsPressureIn))
        {
            table.AddRow()
                .AddCell("Длина обечайки, l:")
                .AddCell($"{dataIn.l} мм");
        }

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
            .AddCell($"{dataIn.phi}");

        WordHelpers.AddLoadingConditionsInTableForShells(dataIn.LoadingConditions, table);

        WordHelpers.AddMaterialCharacteristicsInTableForShell(dataIn.Steel,
            data.Results.Select(r => new
            {
                r.LoadingCondition.OrdinalNumber,
                r.SigmaAllow,
                r.E
            }).ToList(),
            dataIn.LoadingConditions, table);

        body.InsertTable(table);
    }

    private static void MakeCalculateResult(Body body, CylindricalShellCalculatedOneLoading data, CylindricalShellCalculatedCommon cdc, CylindricalShellInput dataIn)
    {
        var loadingCondition = data.LoadingCondition;

        body.AddParagraph();
        body.AddParagraph($"Результаты расчета (для условий нагружения #{loadingCondition.OrdinalNumber})").Alignment(AlignmentType.Center);
        body.AddParagraph();
        body.AddParagraph("Толщину стенки вычисляют по формуле:");
        body.AddParagraph().AppendEquation("s≥s_p+c");
        body.AddParagraph("где ").AppendEquation("s_p").AddRun(" - расчетная толщина стенки обечайки");

        if (loadingCondition.IsPressureIn)
        {
            body.AddParagraph()
                .AppendEquation("s_p=(p∙D)/(2∙[σ]∙φ_p-p)" +
                                $"=({loadingCondition.p}∙{dataIn.D})/(2∙{data.SigmaAllow}∙{dataIn.phi}-{loadingCondition.p})=" +
                                $"{data.s_p:f2} мм");
        }
        else
        {
            body.AddParagraph()
                .AppendEquation("s_p=max{1.06∙(10^-2∙D)/(B)∙(p/(10^-5∙E)∙l/D)^0.4;(1.2∙p∙D)/(2∙[σ]-p)}");
            body.AddParagraph("Коэффициент B вычисляют по формуле:");
            body.AddParagraph()
                .AppendEquation("B=max{1;0.47∙(p/(10^-5∙E))^0.067∙(l/D)^0.4}");
            body.AddParagraph()
                .AppendEquation($"0.47∙({loadingCondition.p}/(10^-5∙{data.E}))^0.067∙({data.l}/{dataIn.D})^0.4={data.b_2:f2}");
            body.AddParagraph()
                .AppendEquation($"B=max(1;{data.b_2:f2})={data.b:f2}");
            body.AddParagraph()
                .AppendEquation($"1.06∙(10^-2∙{dataIn.D})/({data.b:f2})∙({loadingCondition.p}/(10^-5∙{data.E})∙{data.l}/{dataIn.D})^0.4={data.s_p_1:f2}");
            body.AddParagraph()
                .AppendEquation($"(1.2∙{loadingCondition.p}∙{dataIn.D})/(2∙{data.SigmaAllow}-{loadingCondition.p})={data.s_p_2:f2}");
            body.AddParagraph()
                .AppendEquation($"s_p=max({data.s_p_1:f2};{data.s_p_2:f2})={data.s_p:f2} мм");
        }

        //body.AddParagraph("c - сумма прибавок к расчетной толщине");
        //body.AddParagraph()
        //    .AppendEquation($"c=c_1+c_2+c_3={dataIn.c1}+{dataIn.c2}+{dataIn.c3}={cdc.c:f2} мм");

        body.AddParagraph()
            .AppendEquation($"s={data.s_p:f2}+{cdc.c:f2}={data.s:f2} мм");

        WordHelpers.CheckCalculatedThickness("s", dataIn.s, data.s, body);

        if (loadingCondition.IsPressureIn)
        {
            body.AddParagraph("Допускаемое внутреннее избыточное давление вычисляют по формуле:");
            body.AddParagraph()
                .AppendEquation("[p]=(2∙[σ]∙φ_p∙(s-c))/(D+s-c)"
                                + $"=(2∙{data.SigmaAllow}∙{dataIn.phi}∙({dataIn.s}-{cdc.c:f2}))/"
                                + $"({dataIn.D}+{dataIn.s}-{cdc.c:f2})={data.p_d:f2} МПа");
        }
        else
        {
            body.AddParagraph("Допускаемое наружное давление вычисляют по формуле:");
            body.AddParagraph()
                .AppendEquation("[p]=[p]_П/√(1+([p]_П/[p]_E)^2)");
            body.AddParagraph("допускаемое давление из условия прочности вычисляют по формуле:");
            body.AddParagraph()
                .AppendEquation("[p]_П=(2∙[σ]∙(s-c))/(D+s-c)" +
                                                 $"=(2∙{data.SigmaAllow}∙({dataIn.s}-{cdc.c:f2}))/({dataIn.D}+{dataIn.s}-{cdc.c:f2})={data.p_dp:f2} МПа");
            body.AddParagraph("допускаемое давление из условия устойчивости в пределах упругости вычисляют по формуле:");
            body.AddParagraph()
                .AppendEquation("[p]_E=(2.08∙10^-5∙E)/(n_y∙B_1)∙D/l∙[(100∙(s-c))/D]^2.5");
            body.AddParagraph("коэффициент ")
                .AppendEquation("B_1")
                .AddRun(" вычисляют по формуле");
            body.AddParagraph()
                .AppendEquation("B_1=min{1;9.45∙D/l∙√(D/(100∙(s-c)))}");
            body.AddParagraph()
                .AppendEquation($"9.45∙{dataIn.D}/{data.l}∙√({dataIn.D}/(100∙({dataIn.s}-{cdc.c:f2})))={data.B1_2:f2}");
            body.AddParagraph()
                .AppendEquation($"B_1=min(1;{data.B1_2:f2})={data.B1:f1}");
            body.AddParagraph()
                .AppendEquation($"[p]_E=(2.08∙10^-5∙{data.E})/({dataIn.ny}∙{data.B1:f2})∙{dataIn.D}/" +
                                $"{data.l}∙[(100∙({dataIn.s}-{cdc.c:f2}))/{dataIn.D}]^2.5={data.p_de:f2} МПа");
            body.AddParagraph()
                .AppendEquation($"[p]={data.p_dp:f2}/√(1+({data.p_dp:f2}/{data.p_de:f2})^2)={data.p_d:f2} МПа");
        }

        WordHelpers.CheckCalculatedPressure(loadingCondition.p, data.p_d, body);
    }

    private static void InsertCheckConditionsUseFormulas(Body body, CylindricalShellInput dataIn, CylindricalShellCalculatedCommon cdc)
    {
        const int diameterBigLittleBorder = 200;
        body.AddParagraph("Условия применения расчетных формул ")
            .AddRun(dataIn.D >= diameterBigLittleBorder ?
                "при D ≥ 200 мм" : "при D < 200 мм");

        body.AddParagraph()
            .AppendEquation(dataIn.D >= diameterBigLittleBorder ?
                $"(s-c)/(D)=({dataIn.s}-{cdc.c:f2})/({dataIn.D})={(dataIn.s - cdc.c) / dataIn.D:f3}≤0.1" :
                $"(s-c)/(D)=({dataIn.s}-{cdc.c:f2})/({dataIn.D})={(dataIn.s - cdc.c) / dataIn.D:f3}≤0.3");

        if (!cdc.IsConditionUseFormulas)
        {
            body.AddParagraph("Условия применения расчетных формул не выполняется ")
                .Bold()
                .Color(System.Drawing.Color.Red);
        }
    }
}
