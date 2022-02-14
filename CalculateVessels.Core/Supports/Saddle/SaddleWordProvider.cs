using System;
using System.IO;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Supports.Enums;
using CalculateVessels.Core.Word;
using CalculateVessels.Core.Word.Enums;
using DocumentFormat.OpenXml.Packaging;

namespace CalculateVessels.Core.Supports.Saddle
{
    internal class SaddleWordProvider : IWordProvider
    {
        public void MakeWord(string filePath, ICalculatedData calculatedData)
        {
            if (calculatedData is not SaddleCalculatedData data)
                throw new NullReferenceException();

            var dataIn = (SaddleInputData)data.InputData;

            if (string.IsNullOrWhiteSpace(filePath))
            {
                const string DEFAULT_FILE_NAME = "temp.docx";
                filePath = DEFAULT_FILE_NAME;
            }

            using var package = WordprocessingDocument.Open(filePath, true);

            var mainPart = package.MainDocumentPart;
            var body = mainPart?.Document.Body;

            if (body == null) return;

            body.AddParagraph($"Расчет на прочность обечайки {dataIn.NameShell} от воздействия опорных нагрузок. Седловые опоры")
                .Heading(HeadingType.Heading1)
                .Alignment(AlignmentType.Center);

            body.AddParagraph("");

            var imagePart = mainPart.AddImagePart(ImagePartType.Gif);

            byte[] bytes = dataIn.Type switch
            {
                SaddleType.SaddleWithoutRingWithoutSheet =>
                    Data.Properties.Resources.SaddleNothingElem,
                SaddleType.SaddleWithoutRingWithSheet =>
                    Data.Properties.Resources.SaddleSheetElem,
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
                    .AddCell($"{dataIn.D} мм");

                table.AddRow()
                    .AddCell("Толщина стенки обечайки, s:")
                    .AddCell($"{dataIn.s} мм");

                table.AddRow()
                    .AddCell("Прибавка к расчетной толщине, c:")
                    .AddCell($"{dataIn.c} мм");

                table.AddRow()
                    .AddCell("Длина цилиндрической части сосуда, включая длину цилиндрической отбортовки днища, L:")
                    .AddCell($"{dataIn.L} мм");

                table.AddRow()
                    .AddCell("Коэффициент прочности сварного шва, φ:")
                    .AddCell($"{dataIn.fi}");

                table.AddRow()
                    .AddCell("Марка стали")
                    .AddCell($"{dataIn.Steel}");

                table.AddRow()
                    .AddCell("Ширина опоры, b:")
                    .AddCell($"{dataIn.b} мм");

                table.AddRow()
                    .AddCell("Угол охвата опоры, ")
                    .AppendEquation("δ_1")
                    .AppendText(":")
                    .AddCell($"{dataIn.delta1} °");

                table.AddRow()
                    .AddCell("Длина свободно выступающей части сосуда, e:")
                    .AddCell($"{dataIn.e} мм");

                table.AddRow()
                    .AddCell("Длина выступающей цилиндрической части сосуда, включая отбортовку днища, a")
                    .AddCell($"{dataIn.a} мм");

                table.AddRow()
                    .AddCell("Высота опоры, Н")
                    .AddCell($"{dataIn.H} мм");
                if (dataIn.Type == SaddleType.SaddleWithoutRingWithSheet)
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
                        .AddCell("Угол охвата подкладного листа, ")
                        .AppendEquation("δ_2")
                        .AppendText(":")
                        .AddCell($"{dataIn.delta2} °");
                }

                table.AddRowWithOneCell("Условия нагружения");

                table.AddRow()
                    .AddCell("Собственный вес с содержимым, G:")
                    .AddCell($"{dataIn.G} H");

                table.AddRow()
                    .AddCell("Расчетная температура, Т:")
                    .AddCell($"{dataIn.t} °С");

                table.AddRow()
                    .AddCell("Расчетное " +
                             (dataIn.IsPressureIn
                             ? "внутреннее избыточное"
                             : "наружное") + " давление, p:")
                    .AddCell($"{dataIn.p} МПа");

                table.AddRow()
                    .AddCell($"Допускаемое напряжение для материала {dataIn.Steel} при расчетной температуре, [σ]:")
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


            body.AddParagraph("Расчетные параметры").Alignment(AlignmentType.Center);
            body.AddParagraph("");

            body.AddParagraph("Распределенная весовая нагрузка");
            body.AddParagraph("")
                .AppendEquation("q=G/(L+4/3∙H)" +
                    $"={dataIn.G}/({dataIn.L}+4/3∙{dataIn.H})={data.q:f2} Н/мм");

            body.AddParagraph("Расчетный изгибающий момент, действующий на консольную часть обечайки");
            body.AddParagraph("")
                .AppendEquation("M_0=q∙D^2/16" +
                                $"={data.q:f2}∙{dataIn.D}^2/16={data.M0:f2} Н∙мм");

            body.AddParagraph("Опорное усилие");
            body.AddParagraph("")
                .AppendEquation("F_1=F_2=G/2" +
                                $"={dataIn.G}/2={data.F1:f2} H");

            body.AddParagraph("Изгибающий момент над опорами");
            body.AddParagraph("")
                .AppendEquation("M_1=M_2=(q∙e^2)/2-M_0" +
                                $"=({data.q:f2}∙{data.E:f2}^2)/2-{data.M0:f2}={data.M1:f2} Н∙мм");

            body.AddParagraph("Максимальный изгибающий момент между опорами");
            body.AddParagraph("")
                .AppendEquation("M_12=M_0+F_1∙(L/2-a)-q/2∙(L/2+2/3∙H)^2" +
                                $"={data.M0:f2}+{data.F1:f2}∙({dataIn.L}/2-{dataIn.a})-{data.q:f2}/2∙({dataIn.L}/2+2/3∙{dataIn.H})^2={data.M12:f2} Н∙мм");

            body.AddParagraph("Поперечное усилие в сечении оболочки над опорой");
            body.AddParagraph("")
                .AppendEquation("Q_1=Q_2=(L-2∙a)/(L+4/3∙H)∙F_1" +
                                $"=({dataIn.L}-2∙{dataIn.a})/({dataIn.L}+4/3∙{dataIn.H})∙{data.F1:f2}={data.Q1:f2} H");

            body.AddParagraph("Несущую способность обечайки в сечении между опорами следует проверять при условии");
            body.AddParagraph("").AppendEquation("max{M_12}>max{M_1}");
            body.AddParagraph("").AppendEquation($"{data.M12:f2} Н∙мм > {data.M1:f2} Н∙мм");
            if (data.M12 > data.M1)
            {
                body.AddParagraph("Проверка несущей способности обечайки в сечении между опорами");
                body.AddParagraph("Условие прочности");
                body.AddParagraph("").AppendEquation("(p∙D)/(4∙(s-c))+(4∙M_12∙K_9)/(π∙D^2∙(s-c))≤[σ]∙φ");
                body.AddParagraph("где ")
                    .AppendEquation("K_9")
                    .AddRun(" - коэффициент, учитывающий частичное заполнение жидкостью");
                body.AddParagraph("")
                    .AppendEquation("K_9=max{[1.6-0.20924∙(x-1)+0.028702∙x∙(x-1)+0.4795∙10^3∙y∙(x-1)-0.2391∙10^-6∙x∙y∙(x-1)-0.29936∙10^-2∙(x-1)∙x^2-0.85692∙10^-6∙(х-1)∙у^2+0.88174∙10^-6∙х^2∙(х-1)∙у-0.75955∙10^-8∙у^2∙(х-1)∙х+0.82748∙10^-4∙(х-1)∙х^3+0.48168∙10^-9∙(х-1)∙у^3];1}");
                body.AddParagraph("где ").AppendEquation("y=D/(s-c);x=L/D");
                body.AddParagraph("")
                    .AppendEquation($"y={dataIn.D}/({dataIn.s}-{dataIn.c})={data.y:f2}");
                body.AddParagraph("")
                    .AppendEquation($"x={dataIn.L}/{dataIn.D}={data.x:f2}");

                body.AddParagraph("").AppendEquation($"K_9=max({data.K9_1:f2};1)={data.K9:f2}");

                body.AddParagraph("")
                    .AppendEquation($"(p∙D)/(4∙(s-c))+(4∙M_12∙K_9)/(π∙D^2∙(s-c))=({dataIn.p}∙{dataIn.D})/(4∙({dataIn.s}-{dataIn.c}))+(4∙{data.M12:f2}∙{data.K9:f2})/(π∙{dataIn.D}^2∙({dataIn.s}-{dataIn.c}))={data.ConditionStrength1_1:f2}");
                body.AddParagraph("")
                    .AppendEquation($"[σ]∙φ={data.SigmaAllow}∙{dataIn.fi}={data.ConditionStrength1_2:f2}");
                body.AddParagraph("")
                    .AppendEquation($"{data.ConditionStrength1_1:f2}≤{data.ConditionStrength1_2:f2}");
                if (data.ConditionStrength1_1 <= data.ConditionStrength1_2)
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
                body.AddParagraph("Условие устойчивости");
                body.AddParagraph("").AppendEquation("|M_12|/[M]≤1");

                body.AddParagraph("где [M] - допускаемый изгибающий момент из условия устойчивости");
                body.AddParagraph("")
                    .AppendEquation("[M]=(8.9∙10^-5∙E)/n_y∙D^3∙[(100∙(s-c))/D]^2.5" +
                                    $"=(8.9∙10^-5∙{data.E})/{data.ny}∙{dataIn.D}^3∙[(100∙({dataIn.s}-{dataIn.c}))/{dataIn.D}]^2.5={data.M_d:f2} Н∙мм");
                body.AddParagraph("").AppendEquation($"|{data.M12:f2}|/{data.M_d:f2}={data.ConditionStability1:f2}≤1");

                if (data.ConditionStability1 <= 1)
                {
                    body.AddParagraph("Условие устойчивости выполняется")
                        .Bold();
                }
                else
                {
                    body.AddParagraph("Условие устойчивости не выполняется")
                        .Bold()
                        .Color(System.Drawing.Color.Red);
                }
            }
            else
            {
                body.AddParagraph("Проверка несущей способности обечайки в сечении между опорами не требуется");
            }

            body.AddParagraph("");

            switch (dataIn.Type)
            {
                case SaddleType.SaddleWithoutRingWithoutSheet:
                    {
                        body.AddParagraph("Проверка несущей способности обечайки, не укрепленной кольцами жесткости в области опорного узла и без подкладного листа в месте опоры");
                        body.AddParagraph("Вспомогательные параметры и коэффициенты");
                        body.AddParagraph("Параметр, определяемый расстоянием от середины опоры до днища");
                        body.AddParagraph("")
                            .AppendEquation("γ=2.83∙a/D∙√((s-c)/D)" +
                                            $"=2.83∙{dataIn.a}/{dataIn.D}∙√(({dataIn.s}-{dataIn.c})/{dataIn.D})={data.gamma:f2}");

                        body.AddParagraph("Параметр, определяемый шириной пояса опоры");
                        body.AddParagraph("")
                            .AppendEquation("β_1=0.91∙b/√(D∙(s-c))" +
                                            $"=0.91∙{dataIn.b}/√({dataIn.D}∙({dataIn.s}-{dataIn.c}))={data.beta1:f2}");

                        body.AddParagraph("Коэффициенты, учитывающие влияние ширины пояса опоры");
                        body.AddParagraph("")
                            .AppendEquation("K_10=max{(exp(-β_1)∙sin(β_1))/β_1;0.25}" +
                                            $"=max{{(exp(-{data.beta1:f2})∙sin({data.beta1:f2}))/{data.beta1:f2};0.25}}" +
                                            $"=max({data.K10_1:f2};0.25)={data.K10:f2}");

                        body.AddParagraph("")
                            .AppendEquation("K_11=(1-exp(-β_1)∙cos(β_1))/β_1" +
                                            $"=(1-exp(-{data.beta1:f2})∙cos({data.beta1:f2}))/{data.beta1:f2}={data.K11:f2}");

                        body.AddParagraph("Коэффициенты, учитывающие влияние угла охвата");
                        body.AddParagraph("")
                            .AppendEquation("K_12=(1.15-0.1432∙δ_1)/sin(0.5∙δ_1)" +
                                            $"=(1.15-0.1432∙{DegToRad(dataIn.delta1):f2})/sin(0.5∙{DegToRad(dataIn.delta1):f2})={data.K12:f2}");

                        body.AddParagraph("")
                            .AppendEquation("K_13=(max{1.7-(2.1∙δ_1)/π;0})/sin(0.5∙δ_1)" +
                                            $"=(max{{1.7 - (2.1∙{DegToRad(dataIn.delta1):f2})/π;0}})/sin(0.5∙{DegToRad(dataIn.delta1):f2})={data.K13:f2}");

                        body.AddParagraph("")
                            .AppendEquation("K_14=(1.45-0.43∙δ_1)/sin(0.5∙δ_1)" +
                                            $"=(1.45-0.43∙{DegToRad(dataIn.delta1):f2})/sin(0.5∙{DegToRad(dataIn.delta1):f2})={data.K14:f2}");

                        body.AddParagraph("Коэффициенты, учитывающие влияние расстояния от середины опоры до днища и угла охвата");
                        body.AddParagraph("")
                            .AppendEquation("K_15=min{1.0;(0.8∙√γ+6∙γ)/δ_1}" +
                                            $"min{{1.0;(0.8∙√{data.gamma:f2}+6∙{data.gamma:f2})/{DegToRad(dataIn.delta1):f2}}}=min{{1.0;{data.K15_2:f2}}}={data.K15:f2}");

                        body.AddParagraph("")
                            .AppendEquation("K_16=1-0.65/(1+(6∙γ)^2)∙√(π/(3∙δ_1))"
                                            + $"=1-0.65/(1+(6∙{data.gamma:f2})^2)∙√(π/(3∙{DegToRad(dataIn.delta1):f2}))={data.K16:f2}");

                        body.AddParagraph("Коэффициенты, учитывающие влияние ширины пояса опоры и угла охвата");
                        body.AddParagraph("")
                            .AppendEquation("K_17=1/(1+0.6∙∛(D/(s-c))∙(b/D)∙δ_1)" +
                                            $"=1/(1+0.6∙∛({dataIn.D}/({dataIn.s}-{dataIn.c}))∙({dataIn.b}/{dataIn.D})∙{DegToRad(dataIn.delta1):f2})={data.K17:f2}");

                        body.AddParagraph("Общее мембранное меридиональное напряжение изгиба от весовых нагрузок, действующее в области опорного узла");
                        body.AddParagraph("")
                            .AppendEquation("σ_mx=4∙M_i/(π∙D^2∙(s-c))" +
                                            $"=4∙{data.M1:f2}/(π∙{dataIn.D}^2∙({dataIn.s}-{dataIn.c}))={data.sigma_mx:f2}");

                        body.AddParagraph("Условие прочности");
                        body.AddParagraph("").AppendEquation("F_1≤min{[F]_2;[F]_3}");
                        body.AddParagraph("где ")
                            .AppendEquation("[F]_2")
                            .AddRun(" - допускаемое опорное усилие от нагружения в меридиональном направлении");
                        body.AddParagraph("").AppendEquation("[F]_2=(0.7∙[σ_i]_2∙(s-c)∙√(D∙(s-c)))/(K_10∙K_12)");

                        body.AddParagraph("\t")
                            .AppendEquation("[F]_3")
                            .AddRun(" - допускаемое опорное усилие от нагружения в окружном направлении");

                        body.AddParagraph("")
                            .AppendEquation("[F]_3=(0.9∙[σ_i]_3∙(s-c)∙√(D∙(s-c)))/(K_14∙K_16∙K_17)");

                        body.AddParagraph("где ")
                            .AppendEquation("[σ_i]_2, [σ_i]_2")
                            .AddRun(" - предельные напряжения изгиба в меридиональном и окружном направлениях");

                        body.AddParagraph("")
                            .AppendEquation("[σ_i]=K_1∙K_2∙[σ]");

                        body.AddParagraph("")
                            .AppendEquation("K_1=(1-ϑ_2^2)/((1/3+ϑ_1∙ϑ_2)+√((1/3+ϑ_1∙ϑ_2)^2+(1-ϑ_2^2)∙ϑ_1^2))");

                        body.AddParagraph("")
                            .AppendEquation($"K_2={data.K2}")
                            .AddRun(dataIn.IsAssembly
                            ? " - для условий испытания и монтажа"
                            : " - для рабочих условий");

                        body.AddParagraph("для ").AppendEquation("[σ_i]_2");
                        body.AddParagraph("")
                            .AppendEquation("ϑ_1=-(0,23∙K_13∙K_15)/(K_12∙K_10)" +
                                            $"={data.v1_2:f2}");

                        body.AddParagraph("")
                            .AppendEquation("ϑ_(2,1)=- ̅σ_mx∙1/(K_2∙[σ])" +
                                            $"={data.v21_2:f2}");
                        body.AddParagraph("")
                            .AppendEquation("ϑ_(2,2)=[(p∙D)/(4∙(s-c))- ̅σ_mx]∙1/(K_2∙[σ])" +
                                            $"={data.v22_2:f2}");

                        body.AddParagraph("Для ")
                            .AppendEquation("ϑ_2")
                            .AddRun(" принимают одно из значений ")
                            .AppendEquation("ϑ_(2,1)")
                            .AddRun(" или ")
                            .AppendEquation("ϑ_(2,2)")
                            .AddRun(", для которого предельное напряжение изгиба будет наименьшим.");

                        body.AddParagraph("")
                            .AppendEquation(data.K1_2For_v21 < data.K1_2For_v22
                            ? $"ϑ_2=ϑ_(2,1)={data.v21_2:f2}"
                            : $"ϑ_2=ϑ_(2,2)={data.v22_2:f2}");

                        body.AddParagraph("").AppendEquation($"K_1={data.K1_2:f2}");

                        body.AddParagraph("")
                            .AppendEquation($"[σ_i]_2={data.K1_2:f2}∙{data.K2:f2}∙{data.SigmaAllow}={data.sigmai2:f2}");

                        body.AddParagraph("")
                            .AppendEquation($"[F]_2=(0.7∙{data.sigmai2:f2}∙({dataIn.s}-{dataIn.c})∙√({dataIn.D}∙({dataIn.s}-{dataIn.c})))/({data.K10:f2}∙{data.K12:f2})={data.F_d2:f2}");

                        body.AddParagraph("для ").AppendEquation("[σ_i]_3");
                        body.AddParagraph("")
                            .AppendEquation("ϑ_1=-(0,53∙K_11)/(K_14∙K_16∙K_17∙sin(0.5∙δ_1))" +
                                            $"={data.v1_3:f2}");

                        body.AddParagraph("").AppendEquation("ϑ_(2,1)=0");

                        body.AddParagraph("")
                            .AppendEquation("ϑ_(2,2)=(p∙D)/(2∙(s-c))∙1/(K_2∙[σ])" + $"={data.v22_3:f2}");

                        body.AddParagraph("Для ")
                            .AppendEquation("ϑ_2")
                            .AddRun(" принимают одно из значений ")
                            .AppendEquation("ϑ_(2,1)")
                            .AddRun(" или ")
                            .AppendEquation("ϑ_(2,2)")
                            .AddRun(", для которого предельное напряжение изгиба будет наименьшим.");

                        body.AddParagraph("").AppendEquation(data.K1_3For_v21 < data.K1_3For_v22
                            ? $"ϑ_2=ϑ_(2,1)={data.v21_3:f2}"
                            : $"ϑ_2=ϑ_(2,2)={data.v22_3:f2}");

                        body.AddParagraph("").AppendEquation($"K_1={data.K1_3:f2}");

                        body.AddParagraph("")
                            .AppendEquation($"[σ_i]_3={data.K1_3:f2}∙{data.K2:f2}∙{data.SigmaAllow}={data.sigmai3:f2}");

                        body.AddParagraph("")
                            .AppendEquation($"[F]_3=(0.9∙{data.sigmai2:f2}∙({dataIn.s}-{dataIn.c})∙√({dataIn.D}∙({dataIn.s}-{dataIn.c})))/({data.K14:f2}∙{data.K16:f2}∙{data.K17:f2})={data.F_d3:f2}");

                        body.AddParagraph("")
                            .AppendEquation($"{data.F1:f2}≤min{{{data.F_d2:f2};{data.F_d3:f2}}}={data.ConditionStrength2:f2}");

                        if (data.F1 <= Math.Min(data.F_d2, data.F_d3))
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

                        body.AddParagraph("");

                        body.AddParagraph("Условие устойчивости");

                        body.AddParagraph("").AppendEquation("|p|/[p]+|M_i|/[M]+|F_e|/[F]+(Q_i/[Q])^2≤1");

                        body.AddParagraph("где p - расчетное наружное давление (для сосудов, работающих под внутренним избыточным давлением, р=0");

                        body.AddParagraph("где ")
                            .AppendEquation("F_e")
                            .AddRun(" - эффективное осевое усилие от местных мембранных напряжений, действующих в области опоры");

                        body.AddParagraph("")
                            .AppendEquation("F_e=F_i∙π/4∙K_13∙K_15∙√(D/(s-c))" +
                                            $"={data.F1:f2}∙π/4∙{data.K13:f2}∙{data.K15:f2}∙√({dataIn.D}/({dataIn.s}-{dataIn.c}))={data.Fe:f2}");

                        body.AddParagraph("")
                            .AppendEquation((dataIn.IsPressureIn ? "" : $"{dataIn.p}/{data.p_d}") +
                                            $"+{data.M1:f2}/{data.M_d:f2}+{data.Fe:f2}/{data.F_d:f2}+({data.Q1:f2}/{data.Q_d:f2})^2={data.ConditionStability2:f2}≤1");

                        if (data.ConditionStability2 <= 1)
                        {
                            body.AddParagraph("Условие устойчивости выполняется")
                                .Bold();
                        }
                        else
                        {
                            body.AddParagraph("Условие устойчивости не выполняется")
                                .Bold()
                                .Color(System.Drawing.Color.Red);
                        }
                        break;
                    }
                case SaddleType.SaddleWithoutRingWithSheet:
                    {
                        body.AddParagraph("Проверка несущей способности обечайки, не укрепленной кольцами жесткости в области опорного узла с подкладным листом в месте опоры");
                        body.AddParagraph("Вспомогательные параметры и коэффициенты");

                        body.AddParagraph("")
                            .AppendEquation("s_ef=(s-c)∙√(1+(s_2/(s-c))^2)" +
                                            $"=({dataIn.s}-{dataIn.c})∙√(1+({dataIn.s2}/({dataIn.s}-{dataIn.c}))^2)={data.sef:f2}");

                        body.AddParagraph("Параметр, определяемый расстоянием от середины опоры до днища");
                        body.AddParagraph("")
                            .AppendEquation("γ=2.83∙a/D∙√(s_ef/D)" +
                                            $"=2.83∙{dataIn.a}/{dataIn.D}∙√({data.sef:f2}/{dataIn.D})={data.gamma:f2}");

                        body.AddParagraph("Параметр, определяемый шириной пояса опоры");
                        body.AddParagraph("")
                            .AppendEquation("β_1=0.91∙b_2/√(D∙s_ef)" +
                                             $"=0.91∙{dataIn.b}/√({dataIn.D}∙{data.sef:f2})={data.beta1:f2}");

                        body.AddParagraph("Коэффициенты, учитывающие влияние ширины пояса опоры");
                        body.AddParagraph("")
                            .AppendEquation("K_10=max{(exp(-β_1)∙sin(β_1))/β_1;0.25}" +
                                            $"=max{{(exp(-{data.beta1:f2})∙sin({data.beta1:f2}))/{data.beta1:f2};0.25}}" +
                                            $"=max({data.K10_1:f2};0.25)={data.K10:f2}");

                        body.AddParagraph("")
                            .AppendEquation("K_11=(1-exp(-β_1)∙cos(β_1))/β_1" +
                                            $"=(1-exp(-{data.beta1:f2})∙cos({data.beta1:f2}))/{data.beta1:f2}={data.K11:f2}");

                        body.AddParagraph("Коэффициенты, учитывающие влияние угла охвата");
                        body.AddParagraph("")
                            .AppendEquation("K_12=(1.15-0.1432∙δ_2)/sin(0.5∙δ_2)" +
                                            $"=(1.15-0.1432∙{DegToRad(dataIn.delta2):f2})/sin(0.5∙{DegToRad(dataIn.delta2):f2})={data.K12:f2}");

                        body.AddParagraph("")
                            .AppendEquation("K_13=(max{1.7-(2.1∙δ_2)/π;0})/sin(0.5∙δ_2)" +
                                            $"=(max{{1.7 - (2.1∙{DegToRad(dataIn.delta2):f2})/π;0}})/sin(0.5∙{DegToRad(dataIn.delta2):f2})={data.K13:f2}");

                        body.AddParagraph("")
                            .AppendEquation("K_14=(1.45-0.43∙δ_2)/sin(0.5∙δ_2)" +
                                            $"=(1.45-0.43∙{DegToRad(dataIn.delta2):f2})/sin(0.5∙{DegToRad(dataIn.delta2):f2})={data.K14:f2}");

                        body.AddParagraph("Коэффициенты, учитывающие влияние расстояния от середины опоры до днища и угла охвата");
                        body.AddParagraph("")
                            .AppendEquation("K_15=min{1.0;(0.8∙√γ+6∙γ)/δ_2}" +
                                            $"min{{1.0;(0.8∙√{data.gamma:f2}+6∙{data.gamma:f2})/{DegToRad(dataIn.delta2):f2}}}=min{{1.0;{data.K15_2:f2}}}={data.K15:f2}");

                        body.AddParagraph("")
                            .AppendEquation("K_16=1-0.65/(1+(6∙γ)^2)∙√(π/(3∙δ_2))"
                                            + $"=1-0.65/(1+(6∙{data.gamma:f2})^2)∙√(π/(3∙{DegToRad(dataIn.delta2):f2}))={data.K16:f2}");

                        body.AddParagraph("Коэффициенты, учитывающие влияние ширины пояса опоры и угла охвата");
                        body.AddParagraph("")
                            .AppendEquation("K_17=1/(1+0.6∙∛(D/s_ef)∙(b_2/D)∙δ_2)" +
                                            $"=1/(1+0.6∙∛({dataIn.D}/{data.sef:f2})∙({dataIn.b2}/{dataIn.D})∙{DegToRad(dataIn.delta2):f2})={data.K17:f2}");

                        body.AddParagraph("Общее мембранное меридиональное напряжение изгиба от весовых нагрузок, действующее в области опорного узла");
                        body.AddParagraph("")
                            .AppendEquation("σ_mx=4∙M_i/(π∙D^2∙s_ef)" +
                                            $"=4∙{data.M1:f2}/(π∙{dataIn.D}^2∙{data.sef:f2})={data.sigma_mx:f2}");

                        body.AddParagraph("Условие прочности");
                        body.AddParagraph("").AppendEquation("F_1≤min{[F]_2;[F]_3}");
                        body.AddParagraph("где ")
                            .AppendEquation("[F]_2")
                            .AddRun(" - допускаемое опорное усилие от нагружения в меридиональном направлении");
                        body.AddParagraph("").AppendEquation("[F]_2=(0.7∙[σ_i]_2∙s_ef∙√(D∙s_ef))/(K_10∙K_12)");

                        body.AddParagraph("\t")
                            .AppendEquation("[F]_3")
                            .AddRun(" - допускаемое опорное усилие от нагружения в окружном направлении");

                        body.AddParagraph("")
                            .AppendEquation("[F]_3=(0.9∙[σ_i]_3∙s_ef∙√(D∙s_ef))/(K_14∙K_16∙K_17)");

                        body.AddParagraph("где ")
                            .AppendEquation("[σ_i]_2, [σ_i]_2")
                            .AddRun(" - предельные напряжения изгиба в меридиональном и окружном направлениях");

                        body.AddParagraph("")
                            .AppendEquation("[σ_i]=K_1∙K_2∙[σ]");

                        body.AddParagraph("")
                            .AppendEquation("K_1=(1-ϑ_2^2)/((1/3+ϑ_1∙ϑ_2)+√((1/3+ϑ_1∙ϑ_2)^2+(1-ϑ_2^2)∙ϑ_1^2))");

                        body.AddParagraph("")
                            .AppendEquation($"K_2={data.K2}")
                            .AddRun(dataIn.IsAssembly
                            ? " - для условий испытания и монтажа"
                            : " - для рабочих условий");

                        body.AddParagraph("для ").AppendEquation("[σ_i]_2");
                        body.AddParagraph("")
                            .AppendEquation("ϑ_1=-(0,23∙K_13∙K_15)/(K_12∙K_10)" +
                                            $"={data.v1_2:f2}");

                        body.AddParagraph("")
                            .AppendEquation("ϑ_(2,1)=-¯σ_mx∙1/(K_2∙[σ])" +
                                            $"={data.v21_2:f2}");
                        body.AddParagraph("")
                            .AppendEquation("ϑ_(2,2)=[(p∙D)/(4∙s_ef)-¯σ_mx]∙1/(K_2∙[σ])" +
                                            $"={data.v22_2:f2}");

                        body.AddParagraph("Для ")
                            .AppendEquation("ϑ_2")
                            .AddRun(" принимают одно из значений ")
                            .AppendEquation("ϑ_(2,1)")
                            .AddRun(" или ")
                            .AppendEquation("ϑ_(2,2)")
                            .AddRun(", для которого предельное напряжение изгиба будет наименьшим.");

                        body.AddParagraph("")
                            .AppendEquation(data.K1_2For_v21 < data.K1_2For_v22
                            ? $"ϑ_2=ϑ_(2,1)={data.v21_2:f2}"
                            : $"ϑ_2=ϑ_(2,2)={data.v22_2:f2}");

                        body.AddParagraph("").AppendEquation($"K_1={data.K1_2:f2}");

                        body.AddParagraph("")
                            .AppendEquation($"[σ_i]_2={data.K1_2:f2}∙{data.K2:f2}∙{data.SigmaAllow}={data.sigmai2:f2}");

                        body.AddParagraph("")
                            .AppendEquation($"[F]_2=(0.7∙{data.sigmai2:f2}∙{data.sef:f2}∙√({dataIn.D}∙{data.sef:f2}))/({data.K10:f2}∙{data.K12:f2})={data.F_d2:f2}");

                        body.AddParagraph("для ").AppendEquation("[σ_i]_3");
                        body.AddParagraph("")
                            .AppendEquation("ϑ_1=-(0,53∙K_11)/(K_14∙K_16∙K_17∙sin(0.5∙δ_2))" +
                                            $"={data.v1_3:f2}");

                        body.AddParagraph("").AppendEquation("ϑ_(2,1)=0");

                        body.AddParagraph("")
                            .AppendEquation("ϑ_(2,2)=(p∙D)/(2∙s_ef)∙1/(K_2∙[σ])" + $"={data.v22_3:f2}");

                        body.AddParagraph("Для ")
                            .AppendEquation("ϑ_2")
                            .AddRun(" принимают одно из значений ")
                            .AppendEquation("ϑ_(2,1)")
                            .AddRun(" или ")
                            .AppendEquation("ϑ_(2,2)")
                            .AddRun(", для которого предельное напряжение изгиба будет наименьшим.");

                        body.AddParagraph("").AppendEquation(data.K1_3For_v21 < data.K1_3For_v22
                            ? $"ϑ_2=ϑ_(2,1)={data.v21_3:f2}"
                            : $"ϑ_2=ϑ_(2,2)={data.v22_3:f2}");

                        body.AddParagraph("").AppendEquation($"K_1={data.K1_3:f2}");

                        body.AddParagraph("")
                            .AppendEquation($"[σ_i]_3={data.K1_3:f2}∙{data.K2:f2}∙{data.SigmaAllow}={data.sigmai3:f2}");

                        body.AddParagraph("")
                            .AppendEquation($"[F]_3=(0.9∙{data.sigmai2:f2}∙{data.sef:f2}∙√({dataIn.D}∙{data.sef:f2}))/({data.K14:f2}∙{data.K16:f2}∙{data.K17:f2})={data.F_d3:f2}");

                        body.AddParagraph("")
                            .AppendEquation($"{data.F1:f2}≤min{{{data.F_d2:f2};{data.F_d3:f2}}}={data.ConditionStrength2:f2}");

                        if (data.F1 <= Math.Min(data.F_d2, data.F_d3))
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

                        body.AddParagraph("Условие устойчивости");

                        body.AddParagraph("").AppendEquation("|p|/[p]+|M_i|/[M]+|F_e|/[F]+(Q_i/[Q])^2≤1");

                        body.AddParagraph("где p - расчетное наружное давление (для сосудов, работающих под внутренним избыточным давлением, р=0");

                        body.AddParagraph("где ")
                            .AppendEquation("F_e")
                            .AddRun(" - эффективное осевое усилие от местных мембранных напряжений, действующих в области опоры");

                        body.AddParagraph("")
                            .AppendEquation("F_e=F_i∙π/4∙K_13∙K_15∙√(D/s_ef)" +
                                            $"={data.F1:f2}∙π/4∙{data.K13:f2}∙{data.K15:f2}∙√({dataIn.D}/{data.sef:f2})={data.Fe:f2}");

                        body.AddParagraph("")
                            .AppendEquation((dataIn.IsPressureIn ? "" : $"{dataIn.p}/{data.p_d}") +
                                            $"+{data.M1:f2}/{data.M_d:f2}+{data.Fe:f2}/{data.F_d:f2}+({data.Q1:f2}/{data.Q_d:f2})^2={data.ConditionStability2:f2}≤1");

                        if (data.ConditionStability2 <= 1)
                        {
                            body.AddParagraph("Условие устойчивости выполняется")
                                .Bold();
                        }
                        else
                        {
                            body.AddParagraph("Условие устойчивости не выполняется")
                                .Bold()
                                .Color(System.Drawing.Color.Red);
                        }
                        break;
                    }
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
                .AppendEquation($"60°≤δ_1={dataIn.delta1}°≤180°");
            body.AddParagraph("")
                .AppendEquation($"(s-c)/D=({dataIn.s}-{dataIn.c})/{dataIn.D}={(dataIn.s - dataIn.c) / dataIn.D:f2}≤0.05");

            switch (dataIn.Type)
            {
                case SaddleType.SaddleWithoutRingWithSheet:
                    body.AddParagraph("").AppendEquation("s_2≥s");
                    body.AddParagraph("").AppendEquation($"{dataIn.s2} мм ≥ {dataIn.s} мм");
                    body.AddParagraph("").AppendEquation("δ_2≥δ_1+20°");
                    body.AddParagraph("")
                        .AppendEquation(
                            $"{dataIn.delta2}°≥{dataIn.delta1}°+20°={dataIn.delta1 + 20}°");
                    break;
                case SaddleType.SaddleWithRing:
                    body.AddParagraph("").AppendEquation("A_k≥(s-c)√(D∙(s-c))");
                    body.AddParagraph("").AppendEquation($"{dataIn.Ak:f2}≥({dataIn.s}-{dataIn.c})√({dataIn.D}∙({dataIn.s}-{dataIn.c}))={data.Ak:f2}");
                    break;
            }

            package.Close();
        }

        private static double DegToRad(double degree) => degree * Math.PI / 180;
    }
}