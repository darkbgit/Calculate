﻿using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Elliptical;
using CalculateVessels.Core.Shells.Enums;
using CalculateVessels.Output.Word.Core;
using CalculateVessels.Output.Word.Enums;
using CalculateVessels.Output.Word.Interfaces;
using DocumentFormat.OpenXml.Packaging;

namespace CalculateVessels.Output.Word.Elements;

internal class EllipticalShellWordOutput : IWordOutputElement<EllipticalShellCalculated>
{
    public void MakeWord(string filePath, ICalculatedElement calculatedData)
    {
        if (calculatedData is not EllipticalShellCalculated data)
            throw new NullReferenceException();

        var dataIn = (EllipticalShellInput)data.InputData;

        if (string.IsNullOrWhiteSpace(filePath))
        {
            const string DEFAULT_FILE_NAME = "temp.docx";
            filePath = DEFAULT_FILE_NAME;
        }

        using var package = WordprocessingDocument.Open(filePath, true);

        var mainPart = package.MainDocumentPart ?? throw new InvalidOperationException();
        var body = mainPart.Document.Body ?? throw new InvalidOperationException();

        // TODO: добавить полусферическое

        //body.AddParagraph("").InsertPageBreakAfterSelf();

        body.AddParagraph($"Расчет на прочность эллиптического днища {dataIn.Name}, нагруженного " +
                          (dataIn.IsPressureIn ? "внутренним избыточным давлением" : "наружным давлением"))
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

            table.AddRowWithOneCell("Условия нагружения");

            table.AddRow()
                .AddCell("Расчетная температура, Т:")
                .AddCell($"{dataIn.t} °С");

            table.AddRow()
                .AddCell(dataIn.IsPressureIn ? "Расчетное внутреннее избыточное давление, p:"
                    : "Расчетное наружное давление, p:")
                .AddCell($"{dataIn.p} МПа");

            table.AddRowWithOneCell($"Характеристики материала сталь {dataIn.Steel}");

            table.AddRow()
                .AddCell("Допускаемое напряжение при расчетной температуре, [σ]:")
                .AddCell($"{data.SigmaAllow} МПа");

            if (!dataIn.IsPressureIn)
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
            .AppendEquation(dataIn.IsPressureIn
                ? "s_1p=(p∙R)/(2∙[σ]∙φ-0.5∙p)"
                : "s_1p=max{(K_Э∙R)/(161)∙√((n_y∙p)/(10^-5∙E));(1.2∙p∙R)/(2∙[σ])}");
        body.AddParagraph("где R - радиус кривизны в вершине днища");

        // TODO: добавить расчет R для разных ситуаций

        switch (dataIn.EllipticalBottomType)
        {
            case EllipticalBottomType.Elliptical when Math.Abs(dataIn.D - data.EllipseR) < 0.00001:
                body.AddParagraph($"R=D={dataIn.D} мм - для эллиптических днищ с H=0.25D");
                break;
            case EllipticalBottomType.Hemispherical when Math.Abs(0.5 * dataIn.D - data.EllipseR) < 0.00001:
                body.AddParagraph("")
                    .AppendEquation($"R=0.5∙D={dataIn.D} мм")
                    .AddRun(" - для полусферических днищ с H=0.5D");
                break;
            default:
                body.AddParagraph("")
                    .AppendEquation($"R=D^2/(4∙H)={dataIn.D}^2/(4∙{dataIn.EllipseH})={data.EllipseR:f2} мм");
                break;
        }

        if (dataIn.IsPressureIn)
        {
            body.AddParagraph("")
                .AppendEquation($"s_p=({dataIn.p}∙{data.EllipseR:f2})/(2∙{data.SigmaAllow}∙{dataIn.phi}-0.5{dataIn.p})={data.s_p:f2} мм");
        }
        else
        {
            body.AddParagraph("Для предварительного расчета ")
                .AppendEquation($"К_Э={data.EllipseKePrev}")
                .AddRun(dataIn.EllipticalBottomType == EllipticalBottomType.Elliptical
                    ? " для эллиптических днищ"
                    : " для полусферических днищ");
            body.AddParagraph("")
                .AppendEquation($"({data.EllipseKePrev}∙{data.EllipseR:f2})/(161)∙√(({dataIn.ny}∙{dataIn.p})/(10^-5∙{data.E}))=" +
                                $"{data.s_p_1:f2}");
            body.AddParagraph("").AppendEquation($"(1.2∙{dataIn.p}∙{data.EllipseR:f2})/(2∙{dataIn.SigmaAllow})={data.s_p_2:f2}");
            body.AddParagraph("").AppendEquation($"s_1p=max({data.s_p_1:f2};{data.s_p_2:f2})={data.s_p:f2} мм");
        }
        body.AddParagraph("c - сумма прибавок к расчетной толщине");
        body.AddParagraph("")
            .AppendEquation($"c=c_1+c_2+c_3={dataIn.c1}+{dataIn.c2}+{dataIn.c3}={data.c:f2} мм");

        body.AddParagraph("").AppendEquation($"s={data.s_p:f2}+{data.c:f2}={data.s:f2} мм");

        if (dataIn.s >= data.s)
        {
            body.AddParagraph("Принятая толщина ").Bold().AppendEquation($"s_1={dataIn.s} мм");
        }
        else
        {
            body.AddParagraph("Принятая толщина ").Bold().Color(System.Drawing.Color.Red)
                .AppendEquation($"s_1={dataIn.s} мм");
        }

        if (dataIn.IsPressureIn)
        {
            body.AddParagraph("Допускаемое внутреннее избыточное давление вычисляют по формуле:");
            body.AddParagraph("")
                .AppendEquation("[p]=(2∙[σ]∙φ∙(s_1-c))/(R+0.5∙(s-c))" +
                                $"=(2∙{data.SigmaAllow}∙{dataIn.phi}∙({dataIn.s}-{data.c:f2}))/" +
                                $"({data.EllipseR:f2}+0.5∙({dataIn.s}-{data.c:f2}))={data.p_d:f2} МПа");
        }
        else
        {
            body.AddParagraph("Допускаемое наружное давление вычисляют по формуле:");
            body.AddParagraph("")
                .AppendEquation("[p]=[p]_П/√(1+([p]_П/[p]_E)^2)");
            body.AddParagraph("допускаемое давление из условия прочности вычисляют по формуле:");
            body.AddParagraph("")
                .AppendEquation("[p]_П=(2∙[σ]∙(s_1-c))/(R+0.5(s_1-c))" +
                                $"=(2∙{data.SigmaAllow}∙({dataIn.s}-{data.c:f2}))/({data.EllipseR}+0.5({dataIn.s}-{data.c:f2}))={data.p_dp:f2} МПа");
            body.AddParagraph("допускаемое давление из условия устойчивости в пределах упругости вычисляют по формуле:");
            body.AddParagraph("")
                .AppendEquation("[p]_E=(2.6∙10^-5∙E)/n_y∙[(100∙(s_1-c))/(К_Э∙R)]^2");
            body.AddParagraph("коэффициент ")
                .AppendEquation("К_Э")
                .AddRun(" вычисляют по формуле");
            body.AddParagraph("")
                .AppendEquation("К_Э=(1+(2.4+8∙x)∙x)/(1+(3+10∙x)∙x)");
            body.AddParagraph("")
                .AppendEquation($"x=10∙(s_1-c)/D∙(D/(2∙H)-(2∙H)/D)=10∙({dataIn.s - data.c:f2})/{dataIn.D}∙({dataIn.D}/(2∙{dataIn.EllipseH})-(2∙{dataIn.EllipseH})/{dataIn.D})={data.Ellipsex:f2}");
            body.AddParagraph("")
                .AppendEquation($"К_Э=(1+(2.4+8∙{data.Ellipsex:f2})∙{data.Ellipsex:f2})/(1+(3+10∙{data.Ellipsex:f2})∙{data.Ellipsex:f2})={data.EllipseKe:f2}");
            body.AddParagraph("")
                .AppendEquation($"[p]_E=(2.6∙10^-5∙{data.E})/{dataIn.ny}∙" +
                                $"[(100∙({dataIn.s}-{data.c:f2}))/({data.EllipseKe:f2}∙{data.EllipseR:f2})]^2={data.p_de:f2} МПа");
            body.AddParagraph("")
                .AppendEquation($"[p]={data.p_dp:f2}/√(1+({data.p_dp:f2}/{data.p_de:f2})^2)={data.p_d:f2} МПа");
        }
        body.AddParagraph("").AppendEquation("[p]≥p");
        body.AddParagraph("").AppendEquation($"{data.p_d:f2}≥{dataIn.p}");

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

        body.AddParagraph("Условия применения расчетных формул");

        // elliptical bottom
        body.AddParagraph("")
            .AppendEquation("0.002≤(s_1-c)/(D)" +
                            $"=({dataIn.s}-{data.c:f2})/({dataIn.D})={(dataIn.s - data.c) / dataIn.D:f3}≤0.1");
        body.AddParagraph("")
            .AppendEquation($"0.2≤H/D={dataIn.EllipseH}/{dataIn.D}={dataIn.EllipseH / dataIn.D:f3}<0.5");

        if (!data.IsConditionUseFormulas)
        {
            body.AddParagraph("Условия применения расчетных формул не выполняется ")
                .Bold()
                .Color(System.Drawing.Color.Red);
        }
        package.Close();
    }
}