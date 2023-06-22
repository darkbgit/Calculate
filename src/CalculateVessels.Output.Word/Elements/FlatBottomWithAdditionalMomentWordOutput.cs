using CalculateVessels.Core.Bottoms.Enums;
using CalculateVessels.Core.Bottoms.FlatBottomWithAdditionalMoment;
using CalculateVessels.Core.Enums;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Properties;
using CalculateVessels.Output.Word.Core;
using CalculateVessels.Output.Word.Enums;
using CalculateVessels.Output.Word.Interfaces;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace CalculateVessels.Output.Word.Elements;

internal class FlatBottomWithAdditionalMomentWordOutput : IWordOutputElement<FlatBottomWithAdditionalMomentCalculated>
{
    public void MakeWord(string filePath, ICalculatedElement calculatedData)
    {
        if (calculatedData is not FlatBottomWithAdditionalMomentCalculated data)
            throw new NullReferenceException();

        var dataIn = (FlatBottomWithAdditionalMomentInput)data.InputData;

        if (string.IsNullOrWhiteSpace(filePath))
        {
            const string defaultFileName = "temp.docx";
            filePath = defaultFileName;
        }

        using var package = WordprocessingDocument.Open(filePath, true);

        var mainPart = package.MainDocumentPart ?? throw new InvalidOperationException();
        var body = mainPart.Document.Body ?? throw new InvalidOperationException();

        body.AddParagraph($"Расчет на прочность плоской круглой крышки {dataIn.Name} с дополнительным краевым моментом")
            .Heading(HeadingType.Heading1)
            .Alignment(AlignmentType.Center);

        body.AddParagraph();

        {
            var imagePart = mainPart.AddImagePart(ImagePartType.Gif);

            byte[] bytes = dataIn.IsCoverWithGroove
                ? Resources.FlatBottomWithMomentWithGroove
                : Resources.FlatBottomWithMoment;

            imagePart.FeedData(new MemoryStream(bytes));

            body.AddParagraph().AddImage(mainPart.GetIdOfPart(imagePart), bytes);
        }

        {
            var imagePart = mainPart.AddImagePart(ImagePartType.Gif);

            var type = dataIn.IsFlangeFlat ? "f21_" : "fl1_";

            var type1 = dataIn.FlangeFace switch
            {
                FlangeFaceType.Flat => "a",
                FlangeFaceType.MaleFemale => "b",
                FlangeFaceType.TongueGroove => "c",
                FlangeFaceType.Ring => "d",
                _ => throw new InvalidOperationException()
            };

            var bytes = (byte[])(Resources.ResourceManager.GetObject(type + type1)
                    ?? throw new InvalidOperationException());

            imagePart.FeedData(new MemoryStream(bytes));
            body.Elements<Paragraph>().LastOrDefault()!.AddImage(mainPart.GetIdOfPart(imagePart), bytes);
        }

        body.AddParagraph("Исходные данные").Alignment(AlignmentType.Center);

        //table
        {
            var table = body.AddTable();

            table.AddRowWithOneCell("Крышка", gridSpanCount: 2);

            table.AddRow()
                .AddCell("Марка стали")
                .AddCell($"{dataIn.CoverSteel}");

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
                .AddCell(dataIn.IsCoverWithGroove
                    ? "Исполнительная толщина плоской крышки в месте паза для перегородки, "
                    : "Исполнительная толщина крышки, ")
                .AppendEquation("s_1")
                .AppendText(":")
                .AddCell($"{dataIn.s1} мм");

            table.AddRow()
                .AddCell("Исполнительная толщина плоской крышки в зоне уплотнения, ")
                .AppendEquation("s_2")
                .AppendText(":")
                .AddCell($"{dataIn.s2} мм");

            table.AddRow()
                .AddCell("Толщина крышки вне уплотнения, ")
                .AppendEquation("s_3")
                .AppendText(":")
                .AddCell($"{dataIn.s3} мм");

            if (dataIn.IsCoverWithGroove)
            {
                table.AddRow()
                    .AddCell("Ширина паза под перегородку, ")
                    .AppendEquation("s_4")
                    .AppendText(":")
                    .AddCell($"{dataIn.s4} мм");
            }

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
                .AddCell("Отверстия в крышке")
                .AddCell(dataIn.Hole == HoleInFlatBottom.WithoutHole ? "нет" : "есть");

            switch (dataIn.Hole)
            {
                case HoleInFlatBottom.OneHole:
                    table.AddRow()
                        .AddCell("Диаметр отверстия в крышке, d:")
                        .AddCell($"{dataIn.d} мм");
                    break;
                case HoleInFlatBottom.MoreThenOneHole:
                    table.AddRow()
                        .AddCell("Диаметр отверстий в крышке, ")
                        .AppendEquation("d_i")
                        .AppendText(":")
                        .AddCell($"{dataIn.di} мм");
                    break;
            }


            table.AddRowWithOneCell("Фланец");

            table.AddRow()
                .AddCell("Марка стали")
                .AddCell($"{dataIn.FlangeSteel}");

            table.AddRow()
                .AddCell("Тип фланца")
                .AddCell(dataIn.IsFlangeFlat
                    ? "Плоский приварной"
                    : "Приварной встык");

            switch (dataIn.FlangeFace)
            {
                case FlangeFaceType.Flat:
                    table.AddRow()
                        .AddCell("Тип уплотнительной поверхности")
                        .AddCell("Плоская");
                    break;
                case FlangeFaceType.MaleFemale:
                    table.AddRow()
                        .AddCell("Тип уплотнительной поверхности")
                        .AddCell("Выступ - впадина");
                    break;
                case FlangeFaceType.TongueGroove:
                    table.AddRow()
                        .AddCell("Тип уплотнительной поверхности")
                        .AddCell("Шип - паз");
                    break;
                case FlangeFaceType.Ring:
                    table.AddRow()
                        .AddCell("Тип уплотнительной поверхности")
                        .AddCell("Под прокладку овального сечения");
                    break;
            }

            table.AddRow()
                .AddCell("Наружный диаметр фланца, ")
                .AppendEquation("D_н")
                .AppendText(":")
                .AddCell($"{dataIn.Dn} мм");

            table.AddRow()
                .AddCell("Диаметр болтовой окружности, ")
                .AppendEquation("D_б")
                .AppendText(":")
                .AddCell($"{dataIn.Db} мм");

            table.AddRow()
                .AddCell("Внутренний диаметр фланца, D:")
                .AddCell($"{dataIn.D} мм");

            if (dataIn.IsFlangeFlat)
            {
                table.AddRow()
                    .AddCell("Толщина стенки обечайки, s:")
                    .AddCell($"{dataIn.s} мм");
            }
            else
            {
                table.AddRow()
                    .AddCell("Толщина втулки приварного встык фланца в месте приварки к обечайке, ")
                    .AppendEquation("S_0")
                    .AppendText(":")
                    .AddCell($"{dataIn.S0} мм");

                table.AddRow()
                    .AddCell("Толщина втулки приварного встык фланца в месте присоединения к тарелке, ")
                    .AppendEquation("S_1")
                    .AppendText(":")
                    .AddCell($"{dataIn.S1} мм");

                table.AddRow()
                    .AddCell("Длина конической втулки приварного встык фланца, l:")
                    .AddCell($"{dataIn.l} мм");
            }

            table.AddRowWithOneCell("Прокладка");


            table.AddRow()
                .AddCell("Материал")
                .AddCell($"{dataIn.GasketType}");

            table.AddRow()
                .AddCell("Ширина прокладки, ")
                .AppendEquation("b_п")
                .AppendText(":")
                .AddCell($"{dataIn.bp} мм");

            table.AddRow()
                .AddCell("Толщина прокладки, ")
                .AppendEquation("h_п")
                .AppendText(":")
                .AddCell($"{dataIn.hp} мм");

            table.AddRow()
                .AddCell("Средний диаметр прокладки, ")
                .AppendEquation("D_сп")
                .AppendText(":")
                .AddCell($"{dataIn.Dcp} мм");


            table.AddRowWithOneCell("Крепеж");

            table.AddRow()
                .AddCell("Материал")
                .AddCell($"{dataIn.ScrewSteel}");

            table.AddRow()
                .AddCell("Наружный диаметр крепежа, d:")
                .AddCell($"{dataIn.Screwd} мм");

            table.AddRow()
                .AddCell("Количество, n:")
                .AddCell($"{dataIn.n} мм");

            table.AddRow()
                .AddCell("Вид крепежа:")
                .AddCell(dataIn.IsStud ? "Шпилька" : "Болт");

            table.AddRow()
                .AddCell("Проточка:")
                .AddCell(dataIn.IsScrewWithGroove ? "Есть" : "Нет");

            table.AddRow()
                .AddCell("Шайба:")
                .AddCell(dataIn.IsWasher ? "Есть" : "Нет");

            if (dataIn.IsWasher)
            {
                table.AddRow()
                    .AddCell("Материал")
                    .AddCell($"{dataIn.WasherSteel}");

                table.AddRow()
                    .AddCell("Толщина шайбы, ")
                    .AppendEquation("h_ш")
                    .AppendText(":")
                    .AddCell($"{dataIn.hsh} мм");
            }

            table.AddRowWithOneCell("Условия нагружения");


            table.AddRow()
                .AddCell("Расчетная температура, Т:")
                .AddCell($"{dataIn.t} °С");

            table.AddRow()
                .AddCell("Расчетное " +
                         (dataIn.IsPressureIn
                             ? "внутреннее избыточное"
                             : "наружное") + " давление, p:")
                .AddCell($"{dataIn.p} МПа");

            table.AddRowWithOneCell($"Крышка сталь {dataIn.CoverSteel}");

            table.AddRow()
                .AddCell("Допускаемое напряжение при расчетной температуре, [σ]:")
                .AddCell($"{dataIn.SigmaAllow} МПа");

            table.AddRow()
                .AddCell("Модуль продольной упругости при температуре 20°C, ")
                .AppendEquation("E^20")
                .AppendText(":")
                .AddCell($"{data.Ekr:f2} МПа");

            table.AddRow()
                .AddCell("Модуль продольной упругости при расчетной температуре, E:")
                .AddCell($"{data.Ekr:f2} МПа");

            table.AddRow()
                .AddCell("Коэффициент линейного расширения, ")
                .AppendEquation("α")
                .AppendText(":")
                .AddCell($"{data.alpha_kr:f7} ")
                .AppendEquation("°C^-1");

            table.AddRowWithOneCell($"Фланец сталь {dataIn.FlangeSteel}");

            table.AddRow()
                .AddCell("Модуль продольной упругости при температуре 20°C, ")
                .AppendEquation("E^20_ф")
                .AppendText(":")
                .AddCell($"{data.E20:f2} МПа");

            table.AddRow()
                .AddCell("Модуль продольной упругости при расчетной температуре, ")
                .AppendEquation("E_ф")
                .AppendText(":")
                .AddCell($"{data.E:f2} МПа");

            table.AddRow()
                .AddCell("Коэффициент линейного расширения, ")
                .AppendEquation("α_ф")
                .AppendText(":")
                .AddCell($"{data.alpha:f7} ")
                .AppendEquation("°C^-1");

            table.AddRowWithOneCell($"Крепеж сталь {dataIn.ScrewSteel}");

            table.AddRow()
                .AddCell("Допускаемое напряжение при расчетной температуре, ")
                .AppendEquation("[σ]_б")
                .AppendText(":")
                .AddCell($"{data.sigma_dnb:f1} МПа");

            table.AddRow()
                .AddCell("Модуль продольной упругости при температуре 20C°, ")
                .AppendEquation("E^20_б")
                .AppendText(":")
                .AddCell($"{data.Eb20:f2} МПа");

            table.AddRow()
                .AddCell("Модуль продольной упругости при расчетной температуре, ")
                .AppendEquation("E_б")
                .AppendText(":")
                .AddCell($"{data.Eb:f2} МПа");

            table.AddRow()
                .AddCell("Коэффициент линейного расширения,")
                .AppendEquation("α_б")
                .AppendText(":")
                .AddCell($"{data.alpha_b:f7} ")
                .AppendEquation("°C^-1");

            if (dataIn.IsWasher)
            {
                table.AddRowWithOneCell($"Шайба сталь {dataIn.WasherSteel}");

                table.AddRow()
                    .AddCell("Коэффициент линейного расширения,")
                    .AppendEquation("α_ш")
                    .AppendText(":")
                    .AddCell($"{data.alpha_sh1:f7} ")
                    .AppendEquation("°C^-1");
            }

            body.InsertTable(table);
        }

        body.AddParagraph();
        body.AddParagraph("Результаты расчета").Alignment(AlignmentType.Center);
        body.AddParagraph();

        body.AddParagraph("Толщину плоской круглой крышки с дополнительным краевым моментом под действием внутреннего давления вычисляют по формуле");
        body.AddParagraph().AppendEquation("s_1≥s_1p+c");
        body.AddParagraph("где ");
        body.AddParagraph().AppendEquation("s_1p=K_0∙K_6∙D_p∙√(p/(φ∙[σ]))");
        body.AddParagraph("где ");

        body.AddParagraph().AppendEquation($"D_p=D_сп={data.Dcp:f2} мм");

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
                body.AddParagraph()
                    .AppendEquation("K_0=√(1+d/D_p+(d/D_p)^2)" +
                                    $"=√(1+{dataIn.d}/{data.Dp:f2}+({dataIn.d}/{data.Dp:f2})^2)={data.K0:f2}");
                break;
            case HoleInFlatBottom.MoreThenOneHole:
                body.AddParagraph("Коэффициент ")
                    .AppendEquation("K_0")
                    .AddRun(" - для крышек, имеющих несколько отверстий, вычисляют по формул");
                body.AddParagraph()
                    .AppendEquation("K_0=√((1-(Σd_i/D_p)^3)/(1-(Σd_i/D_p)))" +
                                    $"=√((1-({dataIn.di}/{data.Dp:f2})^3)/(1-({dataIn.di}/{data.Dp:f2})))={data.K0:f2}");
                break;
        }

        body.AddParagraph("Коэффициент ")
            .AppendEquation("K_6")
            .AddRun(" вычисляют по формуле");
        body.AddParagraph()
            .AppendEquation(dataIn.IsCoverWithGroove
                ? "K_6=0.41∙√((1+3∙ψ_1∙(D_3/D_сп-1)+9.6∙D_3/D_сп∙s_4/D_сп)/(D_3/D_сп))"
                : "K_6=0,41∙√((1+3∙ψ_1∙(D_3/D_сп-1))/(D_3/D_сп))");

        body.AddParagraph("Значение ")
            .AppendEquation("ψ_1")
            .AddRun(" вычисляют по формуле");
        body.AddParagraph()
            .AppendEquation("ψ_1=P_б^p/Q_д");
        body.AddParagraph()
            .AppendEquation($"P_б^p={data.Pbp:f2} H")
            .AddRun(" расчетная нагрузка на " +
                    (dataIn.IsStud ? "шпильки" : "болты") +
                    " фланцевых соединений, определяют по ГОСТ 34233.4 для рабочих условий");
        body.AddParagraph()
            .AppendEquation($"Q_д=0.785∙p∙D_сп^2=0.785∙{dataIn.p}∙{data.Dcp:f2}^2={data.Qd:f2} H");

        body.AddParagraph()
            .AppendEquation($"ψ_1={data.Pbp:f2}/{data.Qd:f2}={data.psi1:f2}");

        body.AddParagraph()
            .AppendEquation(dataIn.IsCoverWithGroove
                ? $"K_6=0,41∙√((1+3∙{data.psi1:f2}({dataIn.D3}/{data.Dcp:f2}-1)+9.6∙{dataIn.D3}/{data.Dcp:f2}∙{dataIn.s4}/{data.Dcp:f2})/({dataIn.D3}/{data.Dcp:f2}))={data.K6:f2}"
                : $"K_6=0,41∙√((1+3∙{data.psi1:f2}({dataIn.D3}/{data.Dcp:f2}-1))/({dataIn.D3}/{data.Dcp:f2}))={data.K6:f2}");

        body.AddParagraph().AppendEquation($"s_1p={data.K0:f2}∙{data.K6:f2}∙{data.Dp:f2}∙√({dataIn.p}/({dataIn.fi}∙{dataIn.SigmaAllow}))={data.s1p:f2} мм");

        body.AddParagraph("c - сумма прибавок к расчетной толщине");
        body.AddParagraph()
            .AppendEquation($"c=c_1+c_2+c_3={dataIn.c1}+{dataIn.c2}+{dataIn.c3}={data.c:f2} мм");

        body.AddParagraph().AppendEquation($"s_1={data.s1p:f2}+{data.c:f2}={data.s1:f2} мм");

        if (dataIn.s1 > data.s1)
        {
            body.AddParagraph("Принятая толщина ")
                .AppendEquation($"s_1={dataIn.s1} мм")
                .Bold();
        }
        else
        {
            body.AddParagraph("Принятая толщина ")
                .AppendEquation($"s_1={dataIn.s1} мм")
                .Bold()
                .Color(System.Drawing.Color.Red);
        }

        body.AddParagraph("Толщину плоской круглой крышки с дополнительным краевым моментом в месте уплотнения вычисляют по формуле");
        body.AddParagraph().AppendEquation("s_2≥s_2p+c");
        body.AddParagraph("где ");
        body.AddParagraph().AppendEquation("s_2p=max{K_7∙√(Φ);0.6/D_сп∙Φ}");
        body.AddParagraph("где ");

        body.AddParagraph().AppendEquation("Φ=max{P_б^p/[σ]^p;P_б^м/[σ]^м}" +
                                             $"=max{{{data.Pbp:f2}/{dataIn.SigmaAllow};{data.Pbm:f2}/{data.sigma_d_krm:f1}}}" +
                                             $"=max{{{data.Phi_1:f2};{data.Phi_2:f2}}}={data.Phi:f2}");

        body.AddParagraph("Коэффициент ")
            .AppendEquation("K_7")
            .AddRun(" вычисляют по формуле");

        body.AddParagraph()
            .AppendEquation($"K_7=0.8∙√(D_3/D_сп-1)=0.8∙√({dataIn.D3}/{data.Dcp:f2}-1)={data.K7_s2:f2}");

        body.AddParagraph().AppendEquation($"K_7∙√(Φ)={data.K7_s2:f2}∙√({data.Phi:f2})={data.s2p_1:f2}");
        body.AddParagraph().AppendEquation($"0.6/D_сп∙Φ=0.6/{data.Dcp:f2}∙{data.Phi:f2}={data.s2p_2:f2}");

        body.AddParagraph()
            .AppendEquation($"s_2p=max{{{data.s2p_1:f2};{data.s2p_2:f2}}}={data.s2p:f2} мм");


        body.AddParagraph().AppendEquation($"s_2={data.s2p:f2}+{data.c:f2}={data.s2:f2} мм");

        if (dataIn.s2 > data.s2)
        {
            body.AddParagraph("Принятая толщина ")
                .AppendEquation($"s_2={dataIn.s2} мм")
                .Bold();
        }
        else
        {
            body.AddParagraph("Принятая толщина ")
                .AppendEquation($"s_2={dataIn.s2} мм")
                .Bold()
                .Color(System.Drawing.Color.Red);
        }

        body.AddParagraph("Толщину плоской круглой крышки с дополнительным краевым моментом вне зоны уплотнения вычисляют по формуле");
        body.AddParagraph().AppendEquation("s_3≥s_3p+c");
        body.AddParagraph("где ");
        body.AddParagraph().AppendEquation("s_3p=max{K_7∙√(Φ);0.6/D_2∙Φ}");

        body.AddParagraph("Коэффициент ")
            .AppendEquation("K_7")
            .AddRun(" вычисляют по формуле");

        body.AddParagraph()
            .AppendEquation($"K_7=0.8∙√(D_3/D_2-1)=0.8∙√({dataIn.D3}/{dataIn.D2}-1)={data.K7_s3:f2}");

        body.AddParagraph().AppendEquation($"K_7∙√(Φ)={data.K7_s3:f2}∙√({data.Phi:f2})={data.s3p_1:f2}");
        body.AddParagraph().AppendEquation($"0.6/D_2∙Φ=0.6/{dataIn.D2}∙{data.Phi:f2}={data.s3p_2:f2}");

        body.AddParagraph()
            .AppendEquation($"s_2p=max{{{data.s3p_1:f2};{data.s3p_2:f2}}}={data.s3p:f2} мм");


        body.AddParagraph().AppendEquation($"s_2={data.s3p:f2}+{data.c:f2}={data.s3:f2} мм");

        if (dataIn.s3 > data.s3)
        {
            body.AddParagraph("Принятая толщина ")
                .AppendEquation($"s_3={dataIn.s3} мм")
                .Bold();
        }
        else
        {
            body.AddParagraph("Принятая толщина ")
                .AppendEquation($"s_3={dataIn.s3} мм")
                .Bold()
                .Color(System.Drawing.Color.Red);
        }

        body.AddParagraph("Допускаемое давление вычисляют по формуле:");
        body.AddParagraph()
            .AppendEquation("[p]=((s_1-c)/(K_0∙K_6∙D_p))^2∙[σ]∙φ"
                            + $"=(({dataIn.s1}-{data.c:f2})/({data.K0:f2}∙{data.K6:f2}∙{data.Dp:f2}))^2∙{dataIn.SigmaAllow}∙{dataIn.fi}"
                            + $"={data.p_d:f2} МПа");

        body.AddParagraph("Условия применения расчетных формул ");
        body.AddParagraph()
            .AppendEquation($"(s_1-c)/D_p=({dataIn.s1}-{data.c:f2})/{data.Dp:f2}={data.ConditionUseFormulas}≤0.11");
        if (data.IsConditionUseFormulas)
        {
            body.AddParagraph().AppendEquation("[p]≥p");
            body.AddParagraph()
                .AppendEquation($"{data.p_d:f2}≥{dataIn.p}");
        }
        else
        {
            body.AddParagraph("Т.к. условие применения формул не выполняется, то условие прочности имеет вид");
            body.AddParagraph().AppendEquation("K_p∙[p]≥p");
            body.AddParagraph("где ")
                .AppendEquation("K_p")
                .AddRun("  - поправочный коэффициент");
            body.AddParagraph()
                .AppendEquation("K_p=2.2/(1+√(1+(6∙(s_1-c)/D_p)^2))" +
                                $"=2.2/(1+√(1+(6∙({dataIn.s1}-{data.c:f2})/{data.Dp:f2})^2))={data.Kp:f2}");

            body.AddParagraph()
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

        package.Close();
    }
}