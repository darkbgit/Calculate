using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Base;
using CalculateVessels.Core.Shells.Elliptical;
using CalculateVessels.Core.Shells.Enums;
using CalculateVessels.Core.Shells.Nozzle.Enums;
using CalculateVessels.Core.Word;
using CalculateVessels.Core.Word.Enums;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace CalculateVessels.Core.Shells.Nozzle
{
    internal class NozzleWordProvider : IWordProvider
    {
        public void MakeWord(string filePath, ICalculatedData calculatedData)
        {
            if (calculatedData is not NozzleCalculatedData data)
                throw new NullReferenceException();

            var nozzleDataIn = (NozzleInputData)calculatedData.InputData;

            var shellDataIn = (ShellInputData)nozzleDataIn.ShellCalculatedData.InputData;

            if (string.IsNullOrWhiteSpace(filePath))
            {
                const string DEFAULT_FILE_NAME = "temp.docx";
                filePath = DEFAULT_FILE_NAME;
            }

            using var package = WordprocessingDocument.Open(filePath, true);

            var mainPart = package.MainDocumentPart;
            var body = mainPart?.Document.Body;

            if (body == null) return;



            body.AddParagraph($"Расчет на прочность узла врезки штуцера {nozzleDataIn.Name} в ")
                .Heading(HeadingType.Heading1)
                .Alignment(AlignmentType.Center);
            switch (shellDataIn.ShellType)
            {
                case ShellType.Cylindrical:
                    body.Elements<Paragraph>().Last()
                        .AddRun($"обечайку {shellDataIn.Name}, нагруженную ");
                    break;
                case ShellType.Conical:
                    body.Elements<Paragraph>().Last()
                        .AddRun($"коническую обечайку {shellDataIn.Name}, нагруженную ");
                    break;
                case ShellType.Elliptical:
                    body.Elements<Paragraph>().Last()
                        .AddRun($"эллиптическое днище {shellDataIn.Name}, нагруженное ");
                    break;
            }

            body.Elements<Paragraph>().Last()
                .AddRun(shellDataIn.IsPressureIn
                    ? "внутренним избыточным давлением"
                    : "наружным давлением");
            body.AddParagraph("");
            body.AddParagraph("Исходные данные").Alignment(AlignmentType.Center);

            //table
            {
                var table = body.AddTable();

                table.AddRow()
                    .AddCell("Элемент:")
                    .AddCell($"Штуцер {nozzleDataIn.Name}");

                table.AddRow()
                    .AddCell("Элемент несущий штуцер:")
                    .AddCell($"{shellDataIn.Name}");

                table.AddRow()
                    .AddCell("Тип элемента, несущего штуцер:");
                switch (shellDataIn.ShellType)
                {
                    case ShellType.Cylindrical:
                        table.Elements<TableRow>().Last().AddCell("Обечайка цилиндрическая");
                        break;
                    case ShellType.Conical:
                        table.Elements<TableRow>().Last().AddCell("Обечайка коническая");
                        break;
                    case ShellType.Elliptical:
                        table.Elements<TableRow>().Last().AddCell("Днище эллиптическое");
                        break;
                }


                table.AddRow()
                    .AddCell("Тип штуцера:");
                switch (nozzleDataIn.NozzleKind)
                {
                    case NozzleKind.ImpassWithoutRing:
                        table.Elements<TableRow>().Last().AddCell("Непроходящий без укрепления");
                        break;
                    case NozzleKind.PassWithoutRing:
                        table.Elements<TableRow>().Last().AddCell("Проходящий без укрепления");
                        break;
                    case NozzleKind.ImpassWithRing:
                        table.Elements<TableRow>().Last().AddCell("Непроходящий с накладным кольцом");
                        break;
                    case NozzleKind.PassWithRing:
                        table.Elements<TableRow>().Last().AddCell("Проходящий с накладным кольцом");
                        break;
                    case NozzleKind.WithRingAndInPart:
                        table.Elements<TableRow>().Last().AddCell("С накладным кольцом и внутренней частью");
                        break;
                    case NozzleKind.WithFlanging:
                        table.Elements<TableRow>().Last().AddCell("С отбортовкой");
                        break;
                    case NozzleKind.WithTorusshapedInsert:
                        table.Elements<TableRow>().Last().AddCell("С торовой вставкой");
                        break;
                    case NozzleKind.WithWealdedRing:
                        table.Elements<TableRow>().Last().AddCell("С вварным кольцом");
                        break;
                }

                body.InsertTable(table);
            }

            var imagePart = mainPart.AddImagePart(ImagePartType.Gif);

            byte[] bytes = (byte[]) Data.Properties.Resources.ResourceManager
                .GetObject($"Nozzle{(int) nozzleDataIn.NozzleKind}");

            if (bytes != null)
            {
                imagePart.FeedData(new MemoryStream(bytes));
                body.AddParagraph("").AddImage(mainPart.GetIdOfPart(imagePart), bytes);
            }

            //table
            {
                var table = body.AddTable();

                //table.

                table.AddRow()
                    .AddCell("Материал несущего элемента:")
                    .AddCell($"{shellDataIn.Steel}");

                table.AddRow()
                    .AddCell("Толщина стенки несущего элемента, s:")
                    .AddCell($"{shellDataIn.s} мм");

                table.AddRow()
                    .AddCell("Сумма прибавок к стенке несущего элемента, c:")
                    .AddCell($"{data.c:f2} мм");

                table.AddRow()
                    .AddCell("Материал штуцера")
                    .AddCell($"{nozzleDataIn.steel1}");

                table.AddRow()
                    .AddCell("Внутренний диаметр штуцера, d:")
                    .AddCell($"{nozzleDataIn.d} мм");

                table.AddRow()
                    .AddCell("Толщина стенки штуцера, ")
                    .AppendEquation("s_1")
                    .AppendText(":")
                    .AddCell($"{nozzleDataIn.s1} мм");

                table.AddRow()
                    .AddCell("Длина наружной части штуцера, ")
                    .AppendEquation("l_1")
                    .AppendText(":")
                    .AddCell($"{nozzleDataIn.l1} мм");

                table.AddRow()
                    .AddCell("Сумма прибавок к толщине стенки штуцера, ")
                    .AppendEquation("c_s")
                    .AppendText(":")
                    .AddCell($"{nozzleDataIn.cs} мм");

                table.AddRow()
                    .AddCell("Прибавка на коррозию, ")
                    .AppendEquation("c_s1")
                    .AppendText(":")
                    .AddCell($"{nozzleDataIn.cs1} мм");

                switch (nozzleDataIn.NozzleKind)
                {
                    case NozzleKind.ImpassWithoutRing:
                    {
                        break;
                    }
                    case NozzleKind.PassWithoutRing:
                    {
                        table.AddRow()
                            .AddCell("Длина внутренней части штуцера, ")
                            .AppendEquation("l_3")
                            .AppendText(":")
                            .AddCell($"{nozzleDataIn.l3} мм");

                        table.AddRow()
                            .AddCell("Толщина внутренней части штуцера, ")
                            .AppendEquation("s_3")
                            .AppendText(":")
                            .AddCell($"{nozzleDataIn.s3} мм");

                        break;
                    }
                    case NozzleKind.ImpassWithRing:
                    {
                        table.AddRow()
                            .AddCell("Ширина накладного кольца, ")
                            .AppendEquation("l_2")
                            .AppendText(":")
                            .AddCell($"{nozzleDataIn.l2} мм");

                        table.AddRow()
                            .AddCell("Толщина накладного кольца, ")
                            .AppendEquation("s_2")
                            .AppendText(":")
                            .AddCell($"{nozzleDataIn.s2} мм");

                        break;
                    }
                    case NozzleKind.PassWithRing:
                    case NozzleKind.WithRingAndInPart:
                    {
                        table.AddRow()
                            .AddCell("Ширина накладного кольца, ")
                            .AppendEquation("l_2")
                            .AppendText(":")
                            .AddCell($"{nozzleDataIn.l2} мм");

                        table.AddRow()
                            .AddCell("Толщина накладного кольца, ")
                            .AppendEquation("s_2")
                            .AppendText(":")
                            .AddCell($"{nozzleDataIn.s2} мм");

                        table.AddRow()
                            .AddCell("Длина внутренней части штуцера, ")
                            .AppendEquation("l_3")
                            .AppendText(":")
                            .AddCell($"{nozzleDataIn.l3} мм");

                        table.AddRow()
                            .AddCell("Толщина внутренней части штуцера, ")
                            .AppendEquation("s_3")
                            .AppendText(":")
                            .AddCell($"{nozzleDataIn.s3} мм");

                        break;
                    }
                    case NozzleKind.WithFlanging:
                    {
                        table.AddRow()
                            .AddCell("Радиус отбортовки, r:")
                            .AddCell($"{nozzleDataIn.r} мм");

                        break;
                    }
                    case NozzleKind.WithTorusshapedInsert: //UNDONE:
                        break;
                    case NozzleKind.WithWealdedRing:
                        break;
                }

                table.AddRow()
                    .AddCell("Минимальный размер сварного шва, Δ:")
                    .AddCell($"{nozzleDataIn.delta} мм");

                table.AddRowWithOneCell("Коэффициенты прочности сварных швов");

                table.AddRow()
                    .AddCell("Продольный шов штуцера ")
                    .AppendEquation("φ_1")
                    .AppendText(":")
                    .AddCell($"{nozzleDataIn.fi1}");

                table.AddRow()
                    .AddCell("Шов обечайки в зоне врезки штуцера ")
                    .AppendEquation("φ")
                    .AppendText(":")
                    .AddCell($"{nozzleDataIn.fi}");

                table.AddRowWithOneCell("Условия нагружения");


                table.AddRow()
                    .AddCell("Расчетная температура, Т:")
                    .AddCell($"{shellDataIn.t} °С");

                table.AddRow()
                    .AddCell(shellDataIn.IsPressureIn
                        ? "Расчетное внутреннее избыточное давление, p:"
                        : "Расчетное наружное давление, p:")
                    .AddCell($"{shellDataIn.p} МПа");

                table.AddRowWithOneCell($"Характеристики материала {nozzleDataIn.steel1}");

                table.AddRow()
                    .AddCell("Допускаемое напряжение при расчетной температуре, ")
                    .AppendEquation("[σ]_1")
                    .AppendText(":")
                    .AddCell($"{nozzleDataIn.SigmaAllow1} МПа");

                if (!shellDataIn.IsPressureIn)
                {
                    table.AddRow()
                        .AddCell("Модуль продольной упругости при расчетной температуре, ")
                        .AppendEquation("E_1")
                        .AppendText(":")
                        .AddCell($"{nozzleDataIn.E1} МПа");
                }

                if (!string.IsNullOrEmpty(nozzleDataIn.steel2) && nozzleDataIn.steel1 != nozzleDataIn.steel2)
                {
                    table.AddRowWithOneCell($"Характеристики материала {nozzleDataIn.steel2}");

                    table.AddRow()
                        .AddCell(
                            "Допускаемое напряжение для при расчетной температуре, ")
                        .AppendEquation("[σ]_2")
                        .AppendText(":")
                        .AddCell($"{nozzleDataIn.SigmaAllow2} МПа");

                    if (!shellDataIn.IsPressureIn)
                    {
                        table.AddRow()
                            .AddCell("Модуль продольной упругости при расчетной температуре, ")
                            .AppendEquation("E_2")
                            .AppendText(":")
                            .AddCell($"{nozzleDataIn.E2} МПа");
                    }
                }

                if (!string.IsNullOrEmpty(nozzleDataIn.steel3) && nozzleDataIn.steel1 != nozzleDataIn.steel3)
                {
                    table.AddRowWithOneCell($"Характеристики материала {nozzleDataIn.steel3}");

                    table.AddRow()
                        .AddCell("Допускаемое напряжение при расчетной температуре, ")
                        .AppendEquation("[σ]_3")
                        .AppendText(":")
                        .AddCell($"{nozzleDataIn.SigmaAllow3} МПа");

                    if (!shellDataIn.IsPressureIn)
                    {
                        table.AddRow()
                            .AddCell("Модуль продольной упругости при расчетной температуре, ")
                            .AppendEquation("E_3")
                            .AppendText(":")
                            .AddCell($"{nozzleDataIn.E3} МПа");
                    }
                }

                if (!string.IsNullOrEmpty(nozzleDataIn.steel4) && nozzleDataIn.steel1 != nozzleDataIn.steel4)
                {
                    table.AddRowWithOneCell($"Характеристики материала {nozzleDataIn.steel4}");

                    table.AddRow()
                        .AddCell("Допускаемое напряжение при расчетной температуре, ")
                        .AppendEquation("[σ]_4")
                        .AppendText(":")
                        .AddCell($"{nozzleDataIn.SigmaAllow4} МПа");

                    if (!shellDataIn.IsPressureIn)
                    {
                        table.AddRow()
                            .AddCell("Модуль продольной упругости при расчетной температуре, ")
                            .AppendEquation("E_4")
                            .AppendText(":")
                            .AddCell($"{nozzleDataIn.E4} МПа");
                    }
                }

                body.InsertTable(table);
            }



            body.AddParagraph("");
            body.AddParagraph("Результаты расчета").Alignment(AlignmentType.Center);
            body.AddParagraph("");


            body.AddParagraph("Расчетные параметры").Alignment(AlignmentType.Center);
            body.AddParagraph("");


            body.AddParagraph("Расчетный диаметр укрепляемого элемента ");

            switch (shellDataIn.ShellType)
            {
                case ShellType.Cylindrical:
                {
                    body.Elements<Paragraph>().Last()
                        .AddRun("(для цилиндрической обечайки)");
                    body.AddParagraph("")
                        .AppendEquation($"D_p=D={shellDataIn.D} мм");
                    break;
                }
                case ShellType.Conical:
                {
                    body.Elements<Paragraph>().Last()
                        .AddRun("(для конической обечайки, перехода или днища)");
                    body.AddParagraph("")
                        .AppendEquation("D_p=D_k/cos(α)");
                    break;
                }
                case ShellType.Elliptical:
                {
                    var ellH = ((EllipticalShellInputData)shellDataIn).EllipseH;

                    if (Math.Abs(ellH * 100 - shellDataIn.D * 25) < 0.00001)
                    {
                        body.Elements<Paragraph>().Last()
                            .AddRun("(для эллиптического днища при H=0.25D)");
                        body.AddParagraph("")
                            .AppendEquation("D_p=2∙D∙√(1-3∙(x/D)^2)" +
                                            $"=2∙{shellDataIn.D}∙√(1-3∙({nozzleDataIn.ellx}/{shellDataIn.D})^2)={data.Dp:f2} мм");
                    }
                    else
                    {
                        body.Elements<Paragraph>().Last()
                            .AddRun("для эллиптического днища");
                        body.AddParagraph("")
                            .AppendEquation("D_p=D^2/(2∙H)∙√(1-(D^2-4∙H^2)/D^4∙x^2)" +
                                            $"={shellDataIn.D}^2/(2∙{ellH})∙√(1-({shellDataIn.D}^2-4∙{ellH}^2)/{shellDataIn.D}^4∙{nozzleDataIn.ellx}^2)={data.Dp:f2} мм");
                    }

                    break;
                }
                case ShellType.Spherical:
                case ShellType.Torospherical:
                {
                    var ellR = ((EllipticalShellCalculatedData)nozzleDataIn.ShellCalculatedData).EllipseR;

                    body.Elements<Paragraph>().Last()
                        .AddRun("(для сферических и торосферических днищ вне зоны отбортовки)");
                    body.AddParagraph("")
                        .AppendEquation("D_p=2∙R" +
                                        $"=2∙{ellR}={data.Dp:f2}");
                    break;
                }
            }

            switch (nozzleDataIn.Location)
            {
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_1:
                {
                    body.AddParagraph(
                        "Расчетный диаметр отверстия в стенке цилиндрической обечайки, конического перехода или выпуклого днища при наличии штуцера с круглым поперечным сечением, ось которого совпадает с нормалью к поверхности в центре отверстия");
                    body.AddParagraph("")
                        .AppendEquation("d_p=d+2∙c_s" +
                                        $"={nozzleDataIn.d}+2∙{nozzleDataIn.cs}={data.dp:f2} мм");
                    break;
                }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_2:
                    body.AddParagraph(
                        "Расчетный диаметр отверстия в стенке цилиндрической обечайки или конической обечайки при наличии наклонного штуцера, ось которого лежит в плоскости поперечного сечения укрепляемой обечайки");
                    body.AddParagraph("")
                        .AppendEquation("d_p=max{d;0.5∙t}+2∙c_s");
                    //TODO Add parameter t
                    //body.AddParagraph().AppendEquation($"d_p={nozzleDataIn.d}+2∙{nozzleDataIn.cs}={data.dp:f2} мм");
                    break;
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_3:
                {
                    body.AddParagraph(
                        "Расчетный диаметр отверстия в стенке эллиптического днища при наличии смещенного штуцера, ось которого параллельна оси днища");
                    body.AddParagraph("")
                        .AppendEquation("d_p=(d+2∙c_s)/√(1-((2∙x)/D_p)^2)");
                    //TODO: Add parameter x 
                    //body.AddParagraph().AppendEquation($"d_p={nozzleDataIn.d}+2∙{nozzleDataIn.cs}={data.dp:f2} мм");
                    break;
                }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_4:
                {
                    body.AddParagraph(
                        "Расчетный диаметр отверстия при наличии наклонного штуцера с круглым поперечным сечением, когда максимальная ось симметрии отверстия некруглой формы составляет угол ω с образующей цилиндрической обечайки или с проекцией образующей конической обечайки на плоскость продольного сечения обечайки");
                    body.AddParagraph("")
                        .AppendEquation("d_p=(d+2∙c_s)(1+tg^2 γ∙cos^2 ω)");
                    //TODO
                    //body.AddParagraph().AppendEquation($"d_p={nozzleDataIn.d}+2∙{nozzleDataIn.cs}={data.dp:f2} мм");
                    break;
                }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_5:
                    body.AddParagraph(
                        "Расчетный диаметр отверстия для цилиндрической и конической обечаек, когда ось наклонного штуцера лежит в плоскости продольного сечения обечайки, а также для всех отверстий в сферическом и торосферическом днищах при наличии смещенного штуцера");
                    body.AddParagraph("")
                        .AppendEquation("d_p=(d+2∙c_s)/(cos^2 γ)");
                    //TODO
                    //body.AddParagraph().AppendEquation($"d_p={nozzleDataIn.d}+2∙{nozzleDataIn.cs}={data.dp:f2} мм");
                    break;
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_6:
                {
                    body.AddParagraph(
                        "Расчетный диаметр овального отверстия для перпендикулярно расположенного к поверхности обечайки штуцера с овальным поперечным сечением");
                    body.AddParagraph("")
                        .AppendEquation("d_p=(d+2∙c_s)[sin^2 ω +((d_1+2∙c_s)(d_1+d_2+4∙c_s))/(2(d_2+2∙c_s)^2)cos^2 ω");
                    //TODO
                    //body.AddParagraph().AppendEquation($"d_p={nozzleDataIn.d}+2∙{nozzleDataIn.cs}={data.dp:f2} мм");
                    break;
                }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_7:
                {
                    body.AddParagraph(
                        "Расчетный диаметр отверстия для перпендикулярно расположенного к поверхности обечайки или днища штуцера с круглым поперечным сечением при наличии отбортовки или торообразной вставки");
                    body.AddParagraph("")
                        .AppendEquation("d_p=в+1.5(r-s_p)+2∙c_s");
                    //TODO
                    //body.AddParagraph().AppendEquation($"d_p={nozzleDataIn.d}+2∙{nozzleDataIn.cs}={data.dp:f2} мм");
                    break;
                }
            }

            body.AddParagraph("Расчетная толщина стенки укрепляемого элемента");
            if (shellDataIn.ShellType == ShellType.Elliptical && shellDataIn.IsPressureIn)
            {
                body.AddParagraph("")
                    .AppendEquation("s_p=(p∙D_p)/(4∙φ∙[σ]-p)" +
                                    $"=({shellDataIn.p}∙{data.Dp:f2})/(4∙{shellDataIn.fi}∙{data.SigmaAllowShell}-{shellDataIn.p})={data.sp:f2} мм");
            }
            else
            {
                body.Elements<Paragraph>().Last().AddRun(" определяется в соответствии с ГОСТ 34233.2");
                body.AddParagraph("").AppendEquation($"s_p={data.sp:f2} мм");
            }

            body.AddParagraph("Расчетная толщина стенки штуцера с круглым поперечным сечением");
            body.AddParagraph("")
                .AppendEquation("s_1p=(p(d+2∙c_s))/(2∙φ_1∙[σ]_1-p)" +
                                $"=({shellDataIn.p}({nozzleDataIn.d}+2∙{nozzleDataIn.cs}))/(2∙{nozzleDataIn.fi1}∙{nozzleDataIn.SigmaAllow1}-{shellDataIn.p})={data.s1p:f2} мм");

            body.AddParagraph("Расчетная длина внешней части штуцера");
            body.AddParagraph("").AppendEquation("l_1p=min{l_1;1.25√((d+2∙c_s)(s_1-c_s))}");
            body.AddParagraph("")
                .AppendEquation(
                    $"1.25√((d+2∙c_s)(s_1-c_s))=1.25√(({nozzleDataIn.d}+2∙{nozzleDataIn.cs})({nozzleDataIn.s1}-{nozzleDataIn.cs}))={data.l1p2:f2} мм");
            body.AddParagraph("").AppendEquation($"l_1p=min({nozzleDataIn.l1};{data.l1p2:f2})={data.l1p:f2} мм");

            if (nozzleDataIn.l3 > 0)
            {
                body.AddParagraph("Расчетная длина внутренней части штуцера");
                body.AddParagraph("").AppendEquation("l_3p=min{l_3;0.5√((d+2∙c_s)(s_3-c_s-c_s1))}");
                body.AddParagraph("")
                    .AppendEquation(
                        $"0.5√((d+2∙c_s)(s_3-c_s-c_s1))=0.5√(({nozzleDataIn.d}+2∙{nozzleDataIn.cs})({nozzleDataIn.s3}-{nozzleDataIn.cs}-{nozzleDataIn.cs1}))={data.l3p2:f2} мм");
                body.AddParagraph("").AppendEquation($"l_3p=min({nozzleDataIn.l3};{data.l3p2:f2})={data.l3p:f2} мм");
            }

            body.AddParagraph("Ширина зоны укрепления отверстия в цилиндрической обечайке");
            body.AddParagraph("")
                .AppendEquation("L_0=√(D_p∙(s-c))" +
                                $"=√({data.Dp}∙({shellDataIn.s}-{data.c:f2}))={data.L0:f2}");

            body.AddParagraph("Расчетная ширина зоны укрепления отверстия в стенке цилиндрической обечайки");

            switch (nozzleDataIn.NozzleKind)
            {
                case NozzleKind.ImpassWithoutRing:
                case NozzleKind.PassWithoutRing:
                case NozzleKind.ImpassWithRing:
                case NozzleKind.PassWithRing:
                case NozzleKind.WithRingAndInPart:
                case NozzleKind.WithFlanging:
                    body.AddParagraph("").AppendEquation($"l_p=L_0={data.lp:f2} мм");
                    break;
                case NozzleKind.WithTorusshapedInsert:
                case NozzleKind.WithWealdedRing:
                    body.AddParagraph("")
                        .AppendEquation($"l_p=min{{l;L_0}}=min{{{nozzleDataIn.l};{data.L0:f2}}}={data.lp:f2} мм");
                    break;
            }

            if (nozzleDataIn.l2 > 0)
            {
                body.AddParagraph("Расчетная ширина накладного кольца");
                body.AddParagraph("").AppendEquation("l_2p=min{l_2;√(D_p∙(s_2+s-c))}");
                body.AddParagraph("")
                    .AppendEquation(
                        $"√(D_p∙(s_2+s-c))=√({data.Dp:f2}∙({nozzleDataIn.s2}+{shellDataIn.s}-{data.c:f2}))={data.l2p2:f2} мм");
                body.AddParagraph("").AppendEquation($"l_2p=min({nozzleDataIn.l2};{data.l2p2:f2})={data.l2p:f2} мм");
            }

            if ((data.psi1 is not (1 or 0)) | (data.psi2 is not (1 or 0)) | (data.psi3 is not (1 or 0)) | (data.psi4 is not (1 or 0)))
            {
                body.AddParagraph("Учет применения различного материального исполнения");
            }

            if (!string.IsNullOrEmpty(nozzleDataIn.steel1) && shellDataIn.Steel != nozzleDataIn.steel1)
            {
                body.AddParagraph("- для внешней части штуцера")
                    .AppendEquation(
                        $"χ_1=min(1;[σ]_1/[σ])=min(1;{nozzleDataIn.SigmaAllow1}/{data.SigmaAllowShell})={data.psi1:f2}");
            }

            if (!string.IsNullOrEmpty(nozzleDataIn.steel2) && shellDataIn.Steel != nozzleDataIn.steel2)
            {
                body.AddParagraph("- для накладного кольца")
                    .AppendEquation(
                        $"χ_2=min(1;[σ]_2/[σ])=min(1;{nozzleDataIn.SigmaAllow2}/{data.SigmaAllowShell})={data.psi2:f2}");
            }

            if (!string.IsNullOrEmpty(nozzleDataIn.steel3) && shellDataIn.Steel != nozzleDataIn.steel3)
            {
                body.AddParagraph("- для внутренней части штуцера")
                    .AppendEquation(
                        $"χ_3=min(1;[σ]_3/[σ])=min(1;{nozzleDataIn.SigmaAllow3}/{data.SigmaAllowShell})={data.psi3:f2}");
            }

            if (!string.IsNullOrEmpty(nozzleDataIn.steel4) && shellDataIn.Steel != nozzleDataIn.steel4)
            {
                body.AddParagraph("- для торообразной вставки или вварного кольца")
                    .AppendEquation(
                        $"χ_4=min(1;[σ]_4/[σ])=min(1;{nozzleDataIn.SigmaAllow4}/{data.SigmaAllowShell})={data.psi4:f2}");
            }

            body.AddParagraph(
                "Расчетный диаметр отверстия, не требующий укрепления в стенке цилиндрической обечайки при отсутствии избыточной толщины стенки сосуда и при наличии штуцера");
            body.AddParagraph("")
                .AppendEquation("d_0p=0,4√(D_p∙(s-c))" +
                                $"=0.4√({data.Dp}∙({shellDataIn.s}-{data.c:f2}))={data.d0p:f2} мм");

            body.AddParagraph("Проверка условия необходимости проведения расчета укрепления отверстий");
            body.AddParagraph("").AppendEquation("d_p≤d_0");

            body.AddParagraph("")
                .AppendEquation("d_0")
                .AddRun(
                    " - наибольший допустимый диаметр одиночного отверстия, не требующего дополнительного укрепления при наличии избыточной толщины стенки сосуда");
            body.AddParagraph("")
                .AppendEquation("d_0=min{2∙((s-c)/s_pn-0.8)∙√(D_p∙(s-c));d_max+2∙c_s} ");
            body.AddParagraph("где - ")
                .AppendEquation("d_max")
                .AddRun(" - максимальный диаметр отверстия ");

            switch (shellDataIn.ShellType)
            {
                case ShellType.Cylindrical:
                {
                    body.AddParagraph("")
                        .AppendEquation($"d_max=D={shellDataIn.D} мм")
                        .AddRun(" - для отверстий в цилиндрических обечайках");
                    break;
                }
                case ShellType.Conical:
                {
                    body.AddParagraph("")
                        .AppendEquation($"d_max=D_K={data.dmax:f2} мм")
                        .AddRun(" - для отверстий в конических обечайках");
                    break;
                }
                case ShellType.Elliptical:
                case ShellType.Spherical:
                case ShellType.Torospherical:
                {
                    body.AddParagraph("")
                        .AppendEquation($"d_max=0.6∙D={data.dmax:f2} мм")
                        .AddRun(" - для отверстий в выпуклых днищах");
                    break;
                }
            }


            if (shellDataIn.IsPressureIn)
            {
                body.AddParagraph("")
                    .AppendEquation($"s_pn=s_p={data.sp:f2} мм")
                    .AddRun(" - в случае внутреннего давления");
            }
            else
            {
                body.AddParagraph("")
                    .AppendEquation("s_pn=(p_pn∙D_p)/(2∙K_1∙[σ]-p_pn)");
                switch (shellDataIn.ShellType)
                {
                    case ShellType.Cylindrical:
                    case ShellType.Conical:
                    {
                        body.AddParagraph("")
                            .AppendEquation($"K_1={data.K1}")
                            .AddRun(" - для цилиндрических и конических обечаек");
                        break;
                    }
                    case ShellType.Elliptical:
                    case ShellType.Spherical:
                    case ShellType.Torospherical:
                    {
                        body.AddParagraph("")
                            .AppendEquation($"K_1={data.K1}")
                            .AddRun(" - для отверстий в выпуклых днищах");
                        break;
                    }
                }

                body.AddParagraph("").AppendEquation("p_pn=p/√(1-(p/[p]_E)^2)");
                body.AddParagraph("")
                    .AppendEquation("[p]_E")
                    .AddRun(
                        " -  допускаемое наружное давление из условия устойчивости в пределах упругости, определяемое по ГОСТ 34233.2 для обечайки без отверстий");
                body.AddParagraph("")
                    .AppendEquation(
                        $"p_pn={shellDataIn.p}/√(1-({shellDataIn.p}/{data.pen:f2})^2)={data.ppn:f2} МПа");
                body.AddParagraph("")
                    .AppendEquation($"s_pn=({data.ppn:f2}∙{data.Dp:f2})/(2∙{data.K1}∙{data.SigmaAllowShell}-{data.ppn:f2})={data.spn:f2} мм");
            }

            body.AddParagraph("")
                .AppendEquation(
                    $"2∙((s-c)/s_pn-0.8)∙√(D_p∙(s-c))=2∙(({shellDataIn.s}-{data.c:f2})/{data.spn:f2}-0.8)∙√({data.Dp:f2}∙({shellDataIn.s}-{data.c:f2}))={data.d01:f2}");
            body.AddParagraph("")
                .AppendEquation($"d_max+2∙c_s={data.dmax:f2}+2∙{nozzleDataIn.cs}={data.d02:f2}");
            body.AddParagraph("")
                .AppendEquation($"d_0=min({data.d01:f2};{data.d02:f2})={data.d0:f2} мм");

            body.AddParagraph("").AppendEquation($"{data.dp:f2}≤{data.d0:f2}");
            if (data.dp <= data.d0)
            {
                body.AddParagraph("Условие прочности выполняется").Bold();
                body.Elements<Paragraph>().Last()
                    .AddRun(", следовательно, дальнейших расчетов укрепления отверстия не требуется");
            }
            else
            {
                body.AddParagraph("Условие прочности не выполняется").Bold();
                body.Elements<Paragraph>().Last()
                    .AddRun(", следовательно необходим дальнейший расчет укрепления отверстия");
                body.AddParagraph(
                    "В случае укрепления отверстия утолщением стенки сосуда или штуцера, или накладным кольцом, или вварным кольцом, или торообразной вставкой, или отбортовкой должно выполняться условие");
                body.AddParagraph("")
                    .AppendEquation(
                        "l_1p∙(s_1-s_1p-c_s)∙χ_1+l_2p∙s_2∙χ_2+l_3p∙(s_3-c_s-c_s1)∙χ_3+l1p∙(s-s_p-c)∙χ_4≥0.5∙(d_p-d_0p)∙s_p");
                body.AddParagraph("")
                    .AppendEquation("l_1p∙(s_1-s_1p-c_s)∙χ_1+l_2p∙s_2∙χ_2+l_3p∙(s_3-c_s-c_s1)∙χ_3+l1p∙(s-s_p-c)∙χ_4=");
                body.AddParagraph("")
                    .AppendEquation(
                        $"{data.l1p:f2}∙({nozzleDataIn.s1}-{data.s1p:f2}-{nozzleDataIn.cs})∙{data.psi1:f2}+{data.l2p:f2}∙{nozzleDataIn.s2}∙{data.psi2:f2}+{data.l3p:f2}∙({nozzleDataIn.s3}-{nozzleDataIn.cs}-{nozzleDataIn.cs1})∙{data.psi3:f2}+{data.lp:f2}∙({shellDataIn.s}-{data.sp:f2}-{data.c:f2})∙{data.psi4:f2}={data.ConditionStrengthening1:f2}");
                body.AddParagraph("")
                    .AppendEquation(
                        $"0.5∙(d_p-d_0p)∙s_p=0.5∙({data.dp:f2}-{data.d0p:f2})∙{data.sp:f2}={data.ConditionStrengthening2:f2}");
                body.AddParagraph("")
                    .AppendEquation($"{data.ConditionStrengthening1:f2}≥{data.ConditionStrengthening2:f2}");
                if (data.ConditionStrengthening1 >= data.ConditionStrengthening2)
                {
                    body.AddParagraph("Условие прочности выполняется").Bold();
                }
                else
                {
                    body.AddParagraph("Условие прочности не выполняется")
                        .Bold()
                        .Color(System.Drawing.Color.Red);
                }
            }

            body.AddParagraph("");

            if (shellDataIn.IsPressureIn)
            {
                body.AddParagraph(
                    "Допускаемое внутреннее избыточное давление элемента сосуда с учетом ослабления стенки отверстием вычисляют по формуле");
                body.AddParagraph("").AppendEquation("[p]=(2∙K_1∙φ∙[σ]∙(s-c)∙V)/(D_p+(s-c)∙V)");
            }
            else
            {
                body.AddParagraph(
                    "Допускаемое наружное давление элемента сосуда с учетом ослабления стенки отверстием вычисляют по формуле");
                body.AddParagraph("").AppendEquation("[p]=[p]_П/√(1+([p]_П/[p]_E)^2)");
                body.AddParagraph("где ")
                    .AppendEquation("[p]_П")
                    .AddRun(
                        " - допускаемое наружное давление в пределах пластичности определяется как допускаемое внутреннее избыточное давление для сосуда или аппарата с отверстием при φ=1");
                body.AddParagraph("").AppendEquation("[p]_П=(2∙K_1∙φ∙[σ]∙(s-c)∙V)/(D_p+(s-c)∙V)");
            }

            switch (shellDataIn.ShellType)
            {
                case ShellType.Cylindrical:
                case ShellType.Conical:
                {
                    body.AddParagraph("")
                        .AppendEquation($"K_1={data.K1}")
                        .AddRun(" - для цилиндрических и конических обечаек");
                    break;
                }
                case ShellType.Elliptical:
                case ShellType.Spherical:
                case ShellType.Torospherical:
                {
                    body.AddParagraph("")
                        .AppendEquation($"K_1={data.K1}")
                        .AddRun(" - для отверстий в выпуклых днищах");
                    break;
                }
            }

            body.AddParagraph(
                "Коэффициент снижения прочности сосуда, ослабленного одиночным отверстием, вычисляют по формуле");
            body.AddParagraph("")
                .AppendEquation(
                    "V=min{(s_0-c)/(s-c);(χ_4+(l_1p∙(s_1-c_s)∙χ_1+l_2p∙s_2∙χ_2+l_3p∙(s_3-c_s-c_s1)∙χ_3)/(l_p∙(s-c)))/(1+0.5∙(d_p-d_0p)/l_p+K_1∙(d+2∙c_s)/D_p∙(φ/φ_1)∙(l_1p/l_p))}");

            switch (nozzleDataIn.NozzleKind)
            {
                case NozzleKind.ImpassWithoutRing:
                case NozzleKind.PassWithoutRing:
                case NozzleKind.WithFlanging:
                {
                    body.AddParagraph("При отсутствии накладного кольца и укреплении отверстия штуцером ")
                        .AppendEquation("s_2=0 , s_0=s , χ_4=1");
                    break;
                }
                case NozzleKind.ImpassWithRing:
                case NozzleKind.PassWithRing:
                case NozzleKind.WithRingAndInPart:
                {
                    body.AddParagraph("При отсутствии вварного кольца или торообразной вставки ")
                        .AppendEquation("s_0=s , χ_4=1");
                    break;
                }
            }

            body.AddParagraph("")
                .AppendEquation(
                    $"(s_0-c)/(s-c)=({nozzleDataIn.s0}-{data.c:f2})/({shellDataIn.s}-{data.c:f2})={data.V1:f2}");
            body.AddParagraph("")
                .AppendEquation(
                    "(χ_4+(l_1p∙(s_1-c_s)∙χ_1+l_2p∙s_2∙χ_2+l_3p∙(s_3-c_s-c_s1)∙χ_3)/(l_p∙(s-c)))/(1+0.5∙(d_p-d_0p)/l_p+K_1∙(d+2∙c_s)/D_p∙(φ/φ_1)∙(l_1p/l_p))=");
            body.AddParagraph("")
                .AppendEquation(
                    $"({data.psi4:f2}+({data.l1p:f2}∙({nozzleDataIn.s1}-{nozzleDataIn.cs})∙{data.psi1:f2}+{data.l2p:f2}∙{nozzleDataIn.s2}∙{data.psi2:f2}+{data.l3p:f2}∙({nozzleDataIn.s3}-{nozzleDataIn.cs}-{nozzleDataIn.cs1})∙{data.psi3:f2})/({data.lp:f2}∙({shellDataIn.s}-{data.c:f2})))/(1+0.5∙({data.dp:f2}-{data.d0p:f2})/{data.lp:f2}+{data.K1}∙({nozzleDataIn.d}+2∙{nozzleDataIn.cs})/{data.Dp:f2}∙({nozzleDataIn.fi}/{nozzleDataIn.fi1})∙({data.l1p:f2}/{data.lp:f2}))={data.V2:f2}");

            body.AddParagraph("").AppendEquation($"V=min({data.V1:f2};{data.V2:f2})={data.V:f2} ");

            if (shellDataIn.IsPressureIn)
            {
                body.AddParagraph("")
                    .AppendEquation(
                        $"[p]=(2∙{data.K1}∙{nozzleDataIn.fi}∙{data.SigmaAllowShell}∙({shellDataIn.s}-{data.c:f2})∙{data.V:f2})/({data.Dp:f2}+({shellDataIn.s}-{data.c:f2})∙{data.V:f2})={data.p_d:f2} МПа");
            }
            else
            {
                body.AddParagraph("")
                    .AppendEquation(
                        $"[p]_p=(2∙{data.K1}∙{nozzleDataIn.fi}∙{data.SigmaAllowShell}∙({shellDataIn.s}-{data.c:f2})∙{data.V:f2})/({data.Dp}+({shellDataIn.s}-{data.c:f2})∙{data.V:f2})={data.p_dp:f2} МПа");
                body.AddParagraph("")
                    .AppendEquation("[p]_E")
                    .AddRun(
                        " - допускаемое наружное давление из условия устойчивости в пределах упругости, определяемое по ГОСТ 34233.2 для соответствующих обечайки и днища без отверстий");
                body.AddParagraph("").AppendEquation($"[p]_E={data.p_de:f2} МПа");
                body.AddParagraph("")
                    .AppendEquation($"[p]={data.p_dp:f2}/√(1+({data.p_dp:f2}/{data.p_de:f2})^2)={data.p_d:f2} МПа");
            }

            body.AddParagraph("").AppendEquation("[p]≥p");
            body.AddParagraph("").AppendEquation($"{data.p_d:f2} МПа >= {shellDataIn.p} МПа");
            if (data.p_d >= shellDataIn.p)
            {
                body.AddParagraph("Условие прочности выполняется").Bold();
            }
            else
            {
                body.AddParagraph("Условие прочности не выполняется")
                    .Bold()
                    .Color(System.Drawing.Color.Red);
            }

            body.AddParagraph("Условия применения расчетных формул");
            switch (shellDataIn.ShellType)
            {
                case ShellType.Cylindrical:
                {
                    body.AddParagraph("")
                        .AppendEquation("(d_p-2∙c_s)/D" +
                                        $"=({data.dp:f2}-2∙{nozzleDataIn.cs})/{shellDataIn.D}={data.ConditionUseFormulas1:f2}≤1");
                    body.AddParagraph("")
                        .AppendEquation("(s-c)/D" +
                                        $"=({shellDataIn.s}-{data.c:f2})/({shellDataIn.D})={data.ConditionUseFormulas2:f2}≤0.1");
                    break;
                }
                case ShellType.Conical:
                {
                    body.AddParagraph("")
                        .AppendEquation("(d_p-2∙c_s)/D_K" +
                                        $"=({data.dp:f2}-2∙{nozzleDataIn.cs})/{data.Dk}={data.ConditionUseFormulas1:f2}≤1");
                    body.AddParagraph("")
                        .AppendEquation("(s-c)/D_K≤0.1/cosα");
                    body.AddParagraph("")
                        .AppendEquation($"({shellDataIn.s}-{data.c:f2})/({data.Dk})={data.ConditionUseFormulas2:f2}");
                    body.AddParagraph("")
                        .AppendEquation($"0.1/cos{data.alfa1}");
                    break;
                }
                case ShellType.Elliptical:
                case ShellType.Spherical:
                case ShellType.Torospherical:
                {
                    body.AddParagraph("")
                        .AppendEquation("(d_p-2∙c_s)/D " +
                                        $"=({data.dp:f2}-2∙{nozzleDataIn.cs})/{shellDataIn.D}={data.ConditionUseFormulas1:f2}≤0.6");
                    body.AddParagraph("")
                        .AppendEquation("(s-c)/D" +
                                        $"=({shellDataIn.s}-{data.c:f2})/({shellDataIn.D})={data.ConditionUseFormulas2:f2}≤0.1");
                    break;
                }
            }

            package.Close();
        }
    }
}
