using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Document.NET;

namespace calcNet
{
    class MakeWord
    {
        
        //enum Gosts
        //{
        //    GOST_34233_1 = 1,
        //    GOST_34233_2 = 2,
        //    GOST_34233_3 = 3,
        //    GOST_34233_4 = 4,
        //    GOST_34233_5 = 5,
        //    GOST_34233_6 = 6,
        //    GOST_34233_7 = 7,
        //    GOST_34233_8 = 8,
        //    GOST_34233_9 = 9,
        //    GOST_34233_10 = 10,
        //    GOST_34233_11 = 11
        //}


        internal static void MakeWord_cil(Data_in d_in, Data_out d_out, string Docum = null)
        {
            if (Docum == null)
            {
                Docum = "temp.docx";
            }

            var doc = Xceed.Words.NET.DocX.Load(Docum);



            doc.InsertParagraph().InsertPageBreakAfterSelf();
            doc.InsertParagraph($"Расчет на прочность обечайки {d_in.Name}, нагруженной ").Heading(HeadingType.Heading1).Alignment = Alignment.center;
            if (d_in.isPressureIn)
            {
                doc.Paragraphs.Last().Append("внутренним избыточным давлением");
            }
            else
            {
                doc.Paragraphs.Last().Append("наружным давлением");
            }
            doc.InsertParagraph().Alignment = Alignment.center;
            
            var image = doc.AddImage("pic/ObCil.gif");
            var picture = image.CreatePicture();
            doc.InsertParagraph().AppendPicture(picture);
            doc.InsertParagraph("Исходные данные").Alignment = Alignment.center;

            //table
            {
                var table = doc.AddTable(1, 2);
                table.SetWidths(new float[] { 300, 100 });
                int i = 0;
                //table.InsertRow(i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Материал обечайки");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.Steel}");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Внутренний диаметр обечайки, D:");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.D} мм");

                if (d_in.isPressureIn)
                {
                    table.InsertRow(++i);
                    table.Rows[i].Cells[0].Paragraphs[0].Append("Длина обечайки, l:");
                    table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.l} мм");
                }
                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Прибавка на коррозию, ")
                                                    .AppendEquation("c_1")
                                                    .Append(":");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.c1} мм");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Прибавка для компенсации минусового допуска, ")
                                                    .AppendEquation("c_2")
                                                    .Append(":");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.c2} мм");

                if (d_in.c3 > 0)
                {
                    table.InsertRow(++i);
                    table.Rows[i].Cells[0].Paragraphs[0].Append("Технологическая прибавка, ")
                                                        .AppendEquation("c_3")
                                                        .Append(":");
                    table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.c3} мм");
                }
                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Коэффициент прочности сварного шва, ")
                                                    .AppendEquation("φ_p")
                                                    .Append(":");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.fi} мм");

                doc.InsertParagraph().InsertTableAfterSelf(table);
            }

            doc.InsertParagraph();
            doc.InsertParagraph("Условия нагружения").Alignment = Alignment.center;

            //table
            {
                var table = doc.AddTable(1, 2);
                table.SetWidths(new float[] { 300, 100 });
                int i = 0;
                table.Rows[i].Cells[0].Paragraphs[0].Append("Расчетная температура, Т:");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.temp} °С");

                table.InsertRow(++i);
                if (d_in.isPressureIn)
                {
                    table.Rows[i].Cells[0].Paragraphs[0].Append("Расчетное внутреннее избыточное давление, p:");
                }
                else
                {
                    table.Rows[i].Cells[0].Paragraphs[0].Append("Расчетное наружное давление, p:");
                }
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.p} МПа");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append($"Допускаемое напряжение для материала {d_in.Steel} при расчетной температуре, [σ]:");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.sigma_d} МПа");
                if (!d_in.isPressureIn)
                {
                    table.InsertRow(++i);
                    table.Rows[i].Cells[0].Paragraphs[0].Append("Модуль продольной упругости при расчетной температуре, E:");
                    table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.E} МПа");
                }
                doc.InsertParagraph().InsertTableAfterSelf(table);
            }

            doc.InsertParagraph("");
            doc.InsertParagraph("Результаты расчета").Alignment = Alignment.center;
            doc.InsertParagraph("");
            doc.InsertParagraph("Толщину стенки вычисляют по формуле:");
            doc.InsertParagraph().AppendEquation("s≥s_p+c");
            doc.InsertParagraph("где ").AppendEquation("s_p").Append(" - расчетная толщина стенки обечайки");
            if (d_in.isPressureIn)
            {
                doc.InsertParagraph().AppendEquation("s_p=(p∙D)/(2∙[σ]∙φ_p-p)");
                doc.InsertParagraph().AppendEquation($"s_p=({d_in.p}∙{d_in.D})/(2∙{d_in.sigma_d}∙{d_in.fi}-{d_in.p})=" +
                                                    "{d_out.s_calcr:f2} мм");
            }
            else
            {
                doc.InsertParagraph().AppendEquation("s_p=max{1.06∙(10^-2∙D)/(B)∙(p/(10^-5∙E)∙l/D)^0.4;(1.2∙p∙D)/(2∙[σ]-p)}");
                doc.InsertParagraph("Коэффициент B вычисляют по формуле:");
                doc.InsertParagraph().AppendEquation("B=max{1;0.47∙(p/(10^-5∙E))^0.067∙(l/D)^0.4}");
                doc.InsertParagraph().AppendEquation($"0.47∙({d_in.p}/(10^-5∙{d_in.E}))^0.067∙({d_out.l}/{d_in.D})^0.4={d_out.b_2:f2}");
                doc.InsertParagraph().AppendEquation($"B=max(1;{d_out.b_2:f2})={d_out.b:f2}");
                doc.InsertParagraph().AppendEquation($"1.06∙(10^-2∙{d_in.D})/({d_out.b:f2})∙({d_in.p}/(10^-5∙{d_in.E})∙{d_out.l}/{d_in.D})^0.4={d_out.s_calcr1:f2}");
                doc.InsertParagraph().AppendEquation($"(1.2∙{d_in.p}∙{d_in.D})/(2∙{d_in.sigma_d}-{d_in.p})={d_out.s_calcr2:f2}");
                doc.InsertParagraph().AppendEquation($"s_p=max({d_out.s_calcr1:f2};{d_out.s_calcr2:f2})={d_out.s_calcr:f2} мм");
            }

            doc.InsertParagraph("c - сумма прибавок к расчетной толщине");
            doc.InsertParagraph().AppendEquation("c=c_1+c_2+c_3");
            doc.InsertParagraph().AppendEquation($"c={d_in.c1}+{d_in.c2}+{d_in.c3}={d_out.c:f2} мм");

            doc.InsertParagraph().AppendEquation($"s={d_out.s_calcr:f2}+{d_out.c:f2}={d_out.s_calc:f2} мм");
            if (d_in.s > d_out.s_calc)
            {
                doc.InsertParagraph($"Принятая толщина s={d_in.s} мм").Bold();
            }
            else
            {
                doc.InsertParagraph($"Принятая толщина s={d_in.s} мм").Bold().Color(System.Drawing.Color.Red);
            }
            if (d_in.isPressureIn)
            {
                doc.InsertParagraph("Допускаемое внутреннее избыточное давление вычисляют по формуле:");
                doc.InsertParagraph().AppendEquation("[p]=(2∙[σ]∙φ_p∙(s-c))/(D+s-c)");
                doc.InsertParagraph().AppendEquation($"[p]=(2∙{d_in.sigma_d}∙{d_in.fi}∙({d_in.s}-{d_out.c:f2}))/" +
                                                    "({d_in.D}+{d_in.s}-{d_out.c:f2})={d_out.p_d:f2} МПа");
            }
            else
            {
                doc.InsertParagraph("Допускаемое наружное давление вычисляют по формуле:");
                doc.InsertParagraph().AppendEquation("[p]=[p]_П/√(1+([p]_П/[p]_E)^2)");
                doc.InsertParagraph("допускаемое давление из условия прочности вычисляют по формуле:");
                doc.InsertParagraph().AppendEquation("[p]_П=(2∙[σ]∙(s-c))/(D+s-c)");
                doc.InsertParagraph().AppendEquation($"[p]_П=(2∙{d_in.sigma_d}∙({d_in.s}-{d_out.c:f2}))/" +
                                                    "({d_in.D}+{d_in.s}-{d_out.c:f2})={d_out.p_dp:f2} МПа");
                doc.InsertParagraph("допускаемое давление из условия устойчивости в пределах упругости вычисляют по формуле:");
                doc.InsertParagraph().AppendEquation("[p]_E=(2.08∙10^-5∙E)/(n_y∙B_1)∙D/l∙[(100∙(s-c))/D]^2.5");
                doc.InsertParagraph("коэффициент ").AppendEquation("B_1").Append(" вычисляют по формуле");
                doc.InsertParagraph().AppendEquation("B_1=min{1;9.45∙D/l∙√(D/(100∙(s-c)))}");
                doc.InsertParagraph().AppendEquation($"9.45∙{d_in.D}/{d_out.l}∙√({d_in.D}/(100∙({d_in.s}-{d_out.c:f2})))=" +
                                                    "{d_out.b1_2:f2}");
                doc.InsertParagraph().AppendEquation($"B_1=min(1;{d_out.b1_2:f2})={d_out.b1:f1}");
                doc.InsertParagraph().AppendEquation($"[p]_E=(2.08∙10^-5∙{d_in.E})/({d_in.ny}∙{d_out.b1:f2})∙{d_in.D}/" +
                                                    "{d_out.l}∙[(100∙({d_in.s}-{d_out.c:f2}))/{d_in.D}]^2.5={d_out.p_de:f2} МПа");
                doc.InsertParagraph().AppendEquation($"[p]={d_out.p_dp:f2}/√(1+({d_out.p_dp:f2}/{d_out.p_de:f2})^2)={d_out.p_d:f2} МПа");
            }

            doc.InsertParagraph().AppendEquation("[p]≥p");
            doc.InsertParagraph().AppendEquation($"{d_out.p_d:f2}≥{d_in.p}");
            if (d_out.p_d > d_in.p)
            {
                doc.InsertParagraph("Условие прочности выполняется").Bold();
            }
            else
            {
                doc.InsertParagraph("Условие прочности не выполняется").Bold().Color(System.Drawing.Color.Red);
            }
            if (d_out.isConditionUseFormuls)
            {
                doc.InsertParagraph("Границы применения формул ");
            }
            else
            {
                doc.InsertParagraph("Границы применения формул. Условие не выполняется ").Bold().Color(System.Drawing.Color.Red);
            }
            const int DIAMETR_BIG_LITTLE_BORDER = 200;
            if (d_in.D >= DIAMETR_BIG_LITTLE_BORDER)
            {
                doc.Paragraphs.Last().Append("при D ≥ 200 мм");
                doc.InsertParagraph().AppendEquation("(s-c)/(D)≤0.1");
                doc.InsertParagraph().AppendEquation($"({d_in.s}-{d_out.c:f2})/({d_in.D})={(d_in.s - d_out.c) / d_in.D:f3}≤0.1");
            }
            else
            {
                doc.Paragraphs.Last().Append("при D < 200 мм");
                doc.InsertParagraph().AppendEquation("(s-c)/(D)≤0.3");
                doc.InsertParagraph().AppendEquation($"({d_in.s}-{d_out.c:f2})/({d_in.D})={(d_in.s - d_out.c) / d_in.D:f3}≤0.3");
            }

            doc.SaveAs(Docum);
        }

        internal static void MakeWord_ell(Data_in d_in, Data_out d_out, string Docum = null)
        {
            var doc = Xceed.Words.NET.DocX.Load(Docum);
            // TODO: добавить полусферическое

            doc.InsertParagraph().InsertPageBreakAfterSelf();
            doc.InsertParagraph($"Расчет на прочность эллиптического днища {d_in.Name}, нагруженного ").Heading(HeadingType.Heading1).Alignment = Alignment.center;
            if (d_in.isPressureIn)
            {
                doc.Paragraphs.Last().Append("внутренним избыточным давлением");
            }
            else
            {
                doc.Paragraphs.Last().Append("наружным давлением");
            }
            doc.InsertParagraph("");
            var image = doc.AddImage("pic/El.gif");
            var picture = image.CreatePicture();
            doc.InsertParagraph().AppendPicture(picture);
            doc.InsertParagraph("Исходные данные").Alignment = Alignment.center;

            //table
            {
                var table = doc.AddTable(1, 2);
                table.SetWidths(new float[] { 300, 100 });
                int i = 0;
                //table.InsertRow(i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Материал днища");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.Steel}");
                
                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Внутренний диаметр днища, D:");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.D} мм");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Высота выпуклой части, H:");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.elH} мм");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Длина отбортовки ").AppendEquation("h_1").Append(":");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.elh1}");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Прибавка на коррозию, ").AppendEquation("c_1").Append(":");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.c1} мм");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Прибавка для компенсации минусового допуска, ")
                                                    .AppendEquation("c_2")
                                                    .Append(":");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.c2} мм");

                if (d_in.c3 > 0)
                {
                    table.InsertRow(++i);
                    table.Rows[i].Cells[0].Paragraphs[0].Append("Технологическая прибавка, ").AppendEquation("c_3").Append(":");
                    table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.c3}");
                }
                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Коэффициент прочности сварного шва, ").AppendEquation("φ_p");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.fi}");
                doc.InsertParagraph().InsertTableAfterSelf(table);
            }

            doc.InsertParagraph();
            doc.InsertParagraph("Условия нагружения").Alignment = Alignment.center;

            //table
            {
                var table = doc.AddTable(1, 2);
                table.SetWidths(new float[] { 300, 100 });
                int i = 0;
                table.Rows[i].Cells[0].Paragraphs[0].Append("Расчетная температура, Т:");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.temp} °С");

                table.InsertRow(++i);
                if (d_in.isPressureIn)
                {
                    table.Rows[i].Cells[0].Paragraphs[0].Append("Расчетное внутреннее избыточное давление, p:");
                }
                else
                {
                    table.Rows[i].Cells[0].Paragraphs[0].Append("Расчетное наружное давление, p:");
                }
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.p} МПа");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append($"Допускаемое напряжение для материала {d_in.Steel} " +
                                                            "при расчетной температуре, [σ]:");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.sigma_d} МПа");

                if (!d_in.isPressureIn)
                {
                    table.InsertRow(++i);
                    table.Rows[i].Cells[0].Paragraphs[0].Append("Модуль продольной упругости при расчетной температуре, E:");
                    table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.E} МПа");
                }

                doc.InsertParagraph().InsertTableAfterSelf(table);
            }

            doc.InsertParagraph();
            doc.InsertParagraph("Результаты расчета").Alignment = Alignment.center;
            doc.InsertParagraph();
            doc.InsertParagraph("Толщину стенки вычисляют по формуле:");
            doc.InsertParagraph().AppendEquation("s_1≥s_1p+c");
            doc.InsertParagraph("где ").AppendEquation("s_1p").Append(" - расчетная толщина стенки днища");
            if (d_in.isPressureIn)
            {
                doc.InsertParagraph().AppendEquation("s_1p=(p∙R)/(2∙[σ]∙φ-0.5∙p)");
            }
            else
            {
                doc.InsertParagraph().AppendEquation("s_1p=max{(K_Э∙R)/(161)∙√((n_y∙p)/(10^-5∙E));(1.2∙p∙R)/(2∙[σ])}");
            }
            doc.InsertParagraph("где R - радиус кривизны в вершине днища");
            // добавить расчет R для разных ситуаций
            if (d_in.D == d_out.elR)
            {
                doc.InsertParagraph($"R=D={d_in.D} мм - для эллиптичекских днищ с H=0.25D");
            }
            else
            {
                doc.InsertParagraph().AppendEquation("R=D^2/(4∙H)");
                doc.InsertParagraph().AppendEquation($"R={d_in.D}^2/(4∙{d_in.elH})={d_out.elR} мм");
            }
            if (d_in.isPressureIn)
            {
                doc.InsertParagraph().AppendEquation($"s_p=({d_in.p}∙{d_out.elR})/(2∙{d_in.sigma_d}∙{d_in.fi}-0.5{d_in.p})={d_out.s_calcr:f2} мм");
            }
            else
            {
                doc.InsertParagraph("Для предварительного расчета ").AppendEquation("К_Э=0.9").Append(" для эллиптических днищ");
                doc.InsertParagraph().AppendEquation($"(0.9∙{d_out.elR})/(161)∙√(({d_in.ny}∙{d_in.p})/(10^-5∙{d_in.E}))=" +
                                                    "{d_out.s_calcr1:f2}");
                doc.InsertParagraph().AppendEquation($"(1.2∙{d_in.p}∙{d_out.elR})/(2∙{d_in.sigma_d})={d_out.s_calcr2:f2}");
                doc.InsertParagraph().AppendEquation($"s_1p=max({d_out.s_calcr1:f2};{d_out.s_calcr2:f2})={d_out.s_calcr:f2} мм");
            }
            doc.InsertParagraph("c - сумма прибавок к расчетной толщине");
            doc.InsertParagraph().AppendEquation("c=c_1+c_2+c_3");
            doc.InsertParagraph().AppendEquation($"c={d_in.c1}+{d_in.c2}+{d_in.c3}={d_out.c:f2} мм");

            doc.InsertParagraph().AppendEquation($"s={d_out.s_calcr:f2}+{d_out.c:f2}={d_out.s_calc:f2} мм");

            if (d_in.s >= d_out.s_calc)
            {
                doc.InsertParagraph("Принятая толщина ").Bold().AppendEquation($"s_1={d_in.s} мм");
            }
            else
            {
                doc.InsertParagraph($"Принятая толщина s={d_in.s} мм").Bold().Color(System.Drawing.Color.Red);
            }
            doc.InsertParagraph("Допускаемое внутреннее избыточное давление вычисляют по формуле:");
            doc.InsertParagraph().AppendEquation("[p]=(2∙[σ]∙φ∙(s_1-c))/(R+0.5∙(s-c))");
            doc.InsertParagraph().AppendEquation($"[p]=(2∙{d_in.sigma_d}∙{d_in.fi}∙({d_in.s}-{d_out.c:f2}))/" +
                                                "({d_out.elR}+0.5∙({d_in.s}-{d_out.c:f2}))={d_out.p_d:f2} МПа");
            doc.InsertParagraph().AppendEquation("[p]≥p");
            doc.InsertParagraph().AppendEquation($"{d_out.p_d:f2}≥{d_in.p}");
            if (d_out.p_d > d_in.p)
            {
                doc.InsertParagraph("Условие прочности выполняется").Bold();
            }
            else
            {
                doc.InsertParagraph("Условие прочности не выполняется").Bold().Color(System.Drawing.Color.Red);
            }
            if (d_out.isConditionUseFormuls)
            {
                doc.InsertParagraph("Границы применения формул ");
            }
            else
            {
                doc.InsertParagraph().AppendEquation("Границы применения формул. Условие не выполняется").Bold().Color(System.Drawing.Color.Red);
            }
            //# эллептические днища
            doc.InsertParagraph().AppendEquation("0.002≤(s_1-c)/(D)≤0.1");
            doc.InsertParagraph().AppendEquation($"0.002≤({d_in.s}-{d_out.c:f2})/({d_in.D})={(d_in.s - d_out.c) / d_in.D:f3}≤0.1");
            doc.InsertParagraph().AppendEquation("0.2≤H/D≤0.5");
            doc.InsertParagraph().AppendEquation($"0.2≤{d_in.elH}/{d_in.D}={d_in.elH / d_in.D:f3}<0.5");


            doc.Save();
        }



        internal static void MakeWord_saddle(DataSaddle_in d_in, DataSaddle_out d_out, string Docum = null)
        {
            var doc = Xceed.Words.NET.DocX.Load(Docum);
            doc.InsertParagraph().InsertPageBreakAfterSelf();
            doc.InsertParagraph($"Расчет на прочность обечайки {d_in.nameob} от воздействия опорных нагрузок").Heading(HeadingType.Heading1).Alignment = Alignment.center;
            doc.InsertParagraph();


            Xceed.Document.NET.Image image;
            Xceed.Document.NET.Picture picture;

            if (d_in.type == 1)
            {
                image = doc.AddImage("pic/Saddle/SaddleNothingElem.gif");
                picture = image.CreatePicture();
                doc.InsertParagraph().AppendPicture(picture);
            }
            else if (d_in.type == 2)
            {
                image = doc.AddImage("pic/Saddle/SaddleSheetElem.gif");
                picture = image.CreatePicture();
                doc.InsertParagraph().AppendPicture(picture);
            }

            doc.InsertParagraph("Исходные данные").Alignment = Alignment.center;

            var table = doc.AddTable(1, 2);
            table.SetWidths(new float[] { 300, 100 });
            //int i = 0;
            //table.InsertRow(i);
            table.Rows[0].Cells[0].Paragraphs[0].Append("Внутренний диаметр обечайки, D:");
            table.Rows[0].Cells[1].Paragraphs[0].Append($"{d_in.D} мм");
            int i = 1;
            table.InsertRow(i);
            table.Rows[i].Cells[0].Paragraphs[0].Append("Толщина стенки обечайки, s:");
            table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.s} мм");
            i++;
            table.InsertRow(i);
            table.Rows[i].Cells[0].Paragraphs[0].Append("Прибавка к расчетной толщине, c:");
            table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.c} мм");
            i++;
            table.InsertRow(i);
            table.Rows[i].Cells[0].Paragraphs[0].Append("Длина обечайки, Lob:");
            table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.Lob} мм");
            i++;
            table.InsertRow(i);
            table.Rows[i].Cells[0].Paragraphs[0].Append("Коэффициент прочности сварного шва, φ:");
            table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.fi}");
            i++;
            table.InsertRow(i);
            table.Rows[i].Cells[0].Paragraphs[0].Append("Марка стали");
            table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.steel}");
            i++;
            table.InsertRow(i);
            table.Rows[i].Cells[0].Paragraphs[0].Append("Собственный вес с содержимым, G:");
            table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.G} H");
            i++;
            table.InsertRow(i);
            table.Rows[i].Cells[0].Paragraphs[0].Append("Ширина опоры, b:");
            table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.b} мм");
            i++;
            table.InsertRow(i);
            table.Rows[i].Cells[0].Paragraphs[0].Append("Угол охвата опоры, ").AppendEquation("δ_1");
            table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.delta1} °");
            i++;
            table.InsertRow(i);
            table.Rows[i].Cells[0].Paragraphs[0].Append("Длина свободно выступающей части, e:");
            table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.e} мм");
            i++;
            table.InsertRow(i);
            table.Rows[i].Cells[0].Paragraphs[0].Append("Длина выступающей цилиндрической части сосуда, включая отбортовку днища, a");
            table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.a} мм");
            i++;
            table.InsertRow(i);
            table.Rows[i].Cells[0].Paragraphs[0].Append("Высота опоры, Н");
            table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.H} мм");
            if (d_in.type == 2)
            {
                i++;
                table.InsertRow(i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Толщина подкладного листа, ").AppendEquation("s_2");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.s2} мм");
                i++;
                table.InsertRow(i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Ширина подкладного листа, мм").AppendEquation("b_2");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.b2} мм");
                i++;
                table.InsertRow(i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Угол охвата подкладного листа, ").AppendEquation("δ_2");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.delta2} °");
            }

            doc.InsertParagraph().InsertTableAfterSelf(table);

            doc.InsertParagraph();
            doc.InsertParagraph("Условия нагружения").Alignment = Alignment.center;
            var table1 = doc.AddTable(1, 2);
            table1.SetWidths(new float[] { 300, 100 });

            table1.Rows[0].Cells[0].Paragraphs[0].Append("Расчетная температура, Т:");
            table1.Rows[0].Cells[1].Paragraphs[0].Append($"{d_in.temp} °С");
            i = 1;
            table1.InsertRow(i);
            if (d_in.isPressureIn)
            {
                table1.Rows[i].Cells[0].Paragraphs[0].Append("Расчетное внутреннее избыточное давление, p:");
            }
            else
            {
                table1.Rows[i].Cells[0].Paragraphs[0].Append("Расчетное наружное давление, p:");
            }
            table1.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.p} МПа");
            i++;
            table1.InsertRow(i);
            table1.Rows[i].Cells[0].Paragraphs[0].Append($"Допускаемое напряжение для материала {d_in.steel} при расчетной температуре, [σ]:");
            table1.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.sigma_d} МПа");
            i++;
            if (!d_in.isPressureIn)
            {
                table1.InsertRow(i);
                table1.Rows[i].Cells[0].Paragraphs[0].Append("Модуль продольной упругости при расчетной температуре, E:");
                table1.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.E} МПа");
            }

            doc.InsertParagraph().InsertTableAfterSelf(table1);

            doc.InsertParagraph();
            doc.InsertParagraph("Результаты расчета").Alignment = Alignment.center;
            doc.InsertParagraph();


            doc.InsertParagraph("Расчетные параметры").Alignment = Alignment.center;
            doc.InsertParagraph();

            doc.InsertParagraph("Распределенная весовая нагрузка");
            doc.InsertParagraph().AppendEquation("q=G/(L+4/3∙H)");
            doc.InsertParagraph().AppendEquation($"q={d_in.G}/({d_in.L}+4/3∙{d_in.H})={d_out.q:f2} Н/мм");

            doc.InsertParagraph("Расчетный изгибающий момент, действующий на консольную часть обечайки");
            doc.InsertParagraph().AppendEquation("M_0=q∙D^2/16");
            doc.InsertParagraph().AppendEquation($"M_0={d_out.q:f2}∙{d_in.D}^2/16={d_out.M0:f2} Н∙мм");

            doc.InsertParagraph("Опорное усилие");
            doc.InsertParagraph().AppendEquation("F_1=F_2=G/2");
            doc.InsertParagraph().AppendEquation($"F_1=F_2={d_in.G}/2={d_out.F1:f2} H");

            doc.InsertParagraph("Изгибающий момент над опорами");
            doc.InsertParagraph().AppendEquation("M_1=M_2=(q∙e^2)/2-M_0");
            doc.InsertParagraph().AppendEquation($"M_1=M_2=({d_out.q:f2}∙{d_in.e:f2}^2)/2-{d_out.M0:f2}={d_out.M1:f2} Н∙мм");

            doc.InsertParagraph("Максимальный изгибающий момент между опорами");
            doc.InsertParagraph().AppendEquation("M_12=M_0+F_1∙(L/2-a)-q/2∙(L/2+2/3∙H)^2");
            doc.InsertParagraph().AppendEquation($"M_12={d_out.M0:f2}+{d_out.F1:f2}∙({d_in.L}/2-{d_in.a})-{d_out.q:f2}/2∙({d_in.L}/2+2/3∙{d_in.H})^2={d_out.M12:f2} Н∙мм");

            doc.InsertParagraph("Поперечное усилие в сечении оболочки над опорой");
            doc.InsertParagraph().AppendEquation("Q_1=Q_2=(L-2∙a)/(L+4/3∙H)∙F_1");
            doc.InsertParagraph().AppendEquation($"Q_1=Q_2=({d_in.L}-2∙{d_in.a})/({d_in.L}+4/3∙{d_in.H})∙{d_out.F1:f2}={d_out.Q1:f2} H");

            doc.InsertParagraph("Несущую способность обечайки в сечении между опорами следует проверять при условии");
            doc.InsertParagraph().AppendEquation("max{M_12}>max{M_1}");
            doc.InsertParagraph().AppendEquation($"{d_out.M12:f2} Н∙мм > {d_out.M1:f2} Н∙мм");
            if (d_out.M12 > d_out.M1)
            {
                doc.InsertParagraph("Проверка несущей способности обечайки в сечении между опорами");
                doc.InsertParagraph("Условие прочности");
                doc.InsertParagraph().AppendEquation("(p∙D)/(4∙(s-c))+(4∙M_12∙K_9)/(π∙D^2∙(s-c))≤[σ]∙φ");
                doc.InsertParagraph("где ").AppendEquation("K_9").Append(" - коэффициент, учитывающий частичное заполнение жидкостью");
                doc.InsertParagraph().AppendEquation("K_9=max{[1.6-0.20924∙(x-1)+0.028702∙x∙(x-1)+0.4795∙10^3∙y∙(x-1)-0.2391∙10^-6∙x∙y∙(x-1)-0.29936∙10^-2∙(x-1)∙x^2-0.85692∙10^-6∙(х-1)∙у^2+0.88174∙10^-6∙х^2∙(х-1)∙у-0.75955∙10^-8∙у^2∙(х-1)∙х+0.82748∙10^-4∙(х-1)∙х^3+0.48168∙10^-9∙(х-1)∙у^3];1}");
                doc.InsertParagraph("где ").AppendEquation("y=D/(s-c);x=L/D");
                doc.InsertParagraph().AppendEquation($"y={d_in.D}/({d_in.s}-{d_in.c})={d_out.y:f2}");
                doc.InsertParagraph().AppendEquation($"x={d_in.L}/{d_in.D}={d_out.x:f2}");

                doc.InsertParagraph().AppendEquation($"K_9=max({d_out.K9:f2};1)={d_out.K9:f2}");

                doc.InsertParagraph().AppendEquation($"(p∙D)/(4∙(s-c))+(4∙M_12∙K_9)/(π∙D^2∙(s-c))=({d_in.p}∙{d_in.D})/(4∙({d_in.s}-{d_in.c}))+(4∙{d_out.M12:f2}∙{d_out.K9:f2})/(π∙{d_in.D}^2∙({d_in.s}-{d_in.c}))={d_out.yslproch1_1:f2}");
                doc.InsertParagraph().AppendEquation($"[σ]∙φ={d_in.sigma_d}∙{d_in.fi}={d_out.yslproch1_2:f2}");
                doc.InsertParagraph().AppendEquation($"{d_out.yslproch1_1:f2}≤{d_out.yslproch1_2:f2}");
                if (d_out.yslproch1_1 <= d_out.yslproch1_2)
                {
                    doc.InsertParagraph("Условие прочности выполняется");
                }
                else
                {
                    doc.InsertParagraph("Условие прочности не выполняется").Bold().Color(System.Drawing.Color.Red);
                }
                doc.InsertParagraph("Условие устойчивости");
                doc.InsertParagraph().AppendEquation("|M_12|/[M]≤1");

                doc.InsertParagraph("где [M] - допускаемый изгибающий момент из условия устойчивости");
                doc.InsertParagraph().AppendEquation("[M]=(8.9∙10^-5∙E)/n_y∙D^3∙[(100∙(s-c))/D]^2.5");

                doc.InsertParagraph().AppendEquation($"[M]=(8.9∙10^-5∙{d_in.E})/{d_in.ny}∙{d_in.D}^3∙[(100∙({d_in.s}-{d_in.c}))/{d_in.D}]^2.5={d_out.M_d:f2} Н∙мм");
                doc.InsertParagraph().AppendEquation($"|{d_out.M12:f2}|/{d_out.M_d:f2}={d_out.yslystoich1:f2}≤1");

                if (d_out.yslystoich1 <= 1)
                {
                    doc.InsertParagraph("Условие устойчивости выполняется");
                }
                else
                {
                    doc.InsertParagraph("Условие устойчивости не выполняется").Bold().Color(System.Drawing.Color.Red);
                }
            }
            else
            {
                doc.InsertParagraph("Проверка несущей способности обечайки в сечении между опорами не требуется");
            }

            if (d_in.type == 1)
            {
                doc.InsertParagraph("Проверка несущей способности обечайки, не укрепленной кольцами жесткости в области опорного узла и без подкладного листа в месте опоры");
                doc.InsertParagraph("Вспомогательные параметры и коэффициенты");
                doc.InsertParagraph("Параметр, определяемый расстоянием от середины опоры до днища");
                doc.InsertParagraph().AppendEquation("γ=2.83∙a/D∙√((s-c)/D)");
                doc.InsertParagraph().AppendEquation($"γ={d_out.gamma:f2}");

                doc.InsertParagraph("Параметр, определяемый шириной пояса опоры");
                doc.InsertParagraph().AppendEquation("β_1=0.91∙b/√(D∙(s-c))");
                doc.InsertParagraph().AppendEquation($"β_1={d_out.beta1:f2}");

                doc.InsertParagraph("Коэффициенты, учитывающие влияние ширины пояса опоры");
                doc.InsertParagraph().AppendEquation("K_10=max{(exp(-β_1)∙sin(β_1))/β_1;0.25}");
                doc.InsertParagraph().AppendEquation($"K_10=max({d_out.K10_1:f2};0.25)={d_out.K10:f2}");

                doc.InsertParagraph().AppendEquation("K_11=(1-exp(-β_1)∙cos(β_1))/β_1");
                doc.InsertParagraph().AppendEquation($"K_11={d_out.K11:f2}");

                doc.InsertParagraph("Коэффициенты, учитывающие влияние угла охвата");
                doc.InsertParagraph().AppendEquation("K_12=(1.15-0.1432∙δ_1)/sin(0.5∙δ_1)");
                doc.InsertParagraph().AppendEquation($"K_12={d_out.K12:f2}");

                doc.InsertParagraph().AppendEquation("K_13=(max{1.7-(2.1∙δ_1)/π;0})/sin(0.5∙δ_1)");
                doc.InsertParagraph().AppendEquation($"K_13={d_out.K13:f2}");

                doc.InsertParagraph().AppendEquation("K_14=(1.45-0.43∙δ_1)/sin(0.5∙δ_1)");
                doc.InsertParagraph().AppendEquation($"K_14={d_out.K14:f2}");

                doc.InsertParagraph("Коэффициенты, учитывающие влияние расстояния от середины опоры до днища и угла охвата");
                doc.InsertParagraph().AppendEquation("K_15=min{1.0;(0.8∙√γ+6∙γ)/δ_1}");
                doc.InsertParagraph().AppendEquation($"K_15=min(1.0;{d_out.K15_2:f2})={d_out.K15:f2}");

                doc.InsertParagraph().AppendEquation("K_16=1-0.65/(1+(6∙γ)^2)∙√(π/(3∙δ_1))");
                doc.InsertParagraph().AppendEquation($"K_16={d_out.K16:f2}");

                doc.InsertParagraph("Коэффициенты, учитывающие влияние ширины пояса опоры и угла охвата");
                doc.InsertParagraph().AppendEquation("K_17=1/(1+0.6∙∛(D/(s-c))∙(b/D)∙δ_1)");
                doc.InsertParagraph().AppendEquation($"K_17={d_out.K17:f2}");

                doc.InsertParagraph("Общее мембранное меридиональное напряжение изгиба от весовых нагрузок, действующее в области опорного узла");
                doc.InsertParagraph().AppendEquation("σ ̅_mx=4∙M_i/(π∙D^2∙(s-c))");
                doc.InsertParagraph().AppendEquation($"σ ̅_mx={d_out.sigma_mx:f2}");

                doc.InsertParagraph("Условие прочности");
                doc.InsertParagraph().AppendEquation("F_1≤min{[F]_2;[F]_3}");
                doc.InsertParagraph("где ").AppendEquation("[F]_2").Append(" - допускаемое опорное усилие от нагружения в меридиональном направлении");
                doc.InsertParagraph().AppendEquation("[F]_2=(0.7∙[σ_i]_2∙(s-c)∙√(D∙(s-c)))/(K_10∙K_12)");

                doc.InsertParagraph("\t").AppendEquation("[F]_3").Append(" - допускаемое опорное усилие от нагружения в окружном направлении");
                doc.InsertParagraph().AppendEquation("[F]_3=(0.9∙[σ_i]_3∙(s-c)∙√(D∙(s-c)))/(K_14∙K_16∙K_17)");

                doc.InsertParagraph("где ").AppendEquation("[σ_i]_2, [σ_i]_2").Append(" - предельные напряжения изгиба в меридиональном и окружном направлениях");

                doc.InsertParagraph().AppendEquation("[σ_i]=K_1∙K_2∙[σ]");

                doc.InsertParagraph().AppendEquation("K_1=(1-ϑ_2^2)/((1/3+ϑ_1∙ϑ_2)+√((1/3+ϑ_1∙ϑ_2)^2+(1-ϑ_2^2)∙ϑ_1^2))");

                // TODO: для условий монтажа 
                doc.InsertParagraph().AppendEquation("K_2=1.25").Append(" - для рабочих условий");

                doc.InsertParagraph("для ").AppendEquation("[σ_i]_2");
                doc.InsertParagraph().AppendEquation("ϑ_1=-(0,23∙K_13∙K_15)/(K_12∙K_10)");
                doc.InsertParagraph().AppendEquation($"ϑ_1={d_out.v1_2:f2}");

                doc.InsertParagraph().AppendEquation("ϑ_(2,1)=- ̅σ_mx∙1/(K_2∙[σ])");
                doc.InsertParagraph().AppendEquation($"ϑ_(2,1)={d_out.v21_2:f2}");
                doc.InsertParagraph().AppendEquation("ϑ_(2,2)=[(p∙D)/(4∙(s-c))- ̅σ_mx]∙1/(K_2∙[σ])");
                doc.InsertParagraph().AppendEquation($"ϑ_(2,2)={d_out.v22_2:f2}");

                doc.InsertParagraph("Для ").AppendEquation("ϑ_2").Append("принимают одно из значений ").AppendEquation("ϑ_(2,1)").Append(" или ").AppendEquation("ϑ_(2,2)").Append(", для которого предельное напряжение изгибабудет наименьшим.");
                if (d_out.K1_21 < d_out.K1_22)
                {
                    doc.InsertParagraph().AppendEquation($"ϑ_2=ϑ_(2,1)={d_out.v21_2:f2}");
                }
                else
                {
                    doc.InsertParagraph().AppendEquation($"ϑ_2=ϑ_(2,2)={d_out.v22_2:f2}");
                }

                doc.InsertParagraph().AppendEquation($"K_1={d_out.K1_2:f2}");

                doc.InsertParagraph().AppendEquation($"[σ_i]_2={d_out.K1_2:f2}∙{d_out.K2:f2}∙{d_in.sigma_d}={d_out.sigmai2:f2}");

                doc.InsertParagraph().AppendEquation($"[F]_2=(0.7∙{d_out.sigmai2:f2}∙({d_in.s}-{d_in.c})∙√({d_in.D}∙({d_in.s}-{d_in.c})))/({d_out.K10:f2}∙{d_out.K12:f2})={d_out.F_d2:f2}");


                doc.InsertParagraph("для ").AppendEquation("[σ_i]_3");
                doc.InsertParagraph().AppendEquation("ϑ_1=-(0,53∙K_11)/(K_14∙K_16∙K_17∙sin(0.5∙δ_1))");
                doc.InsertParagraph().AppendEquation($"ϑ_1={d_out.v1_3:f2}");

                doc.InsertParagraph().AppendEquation("ϑ_(2,1)=0");
                doc.InsertParagraph().AppendEquation($"ϑ_(2,1)={d_out.v21_3:f2}");
                doc.InsertParagraph().AppendEquation("ϑ_(2,2)=(p∙D)/(2∙(s-c))∙1/(K_2∙[σ])");
                doc.InsertParagraph().AppendEquation($"ϑ_(2,2)={d_out.v22_3:f2}");

                doc.InsertParagraph("Для ").AppendEquation("ϑ_2").Append("принимают одно из значений ").AppendEquation("ϑ_(2,1)").Append(" или ").AppendEquation("ϑ_(2,2)").Append(", для которого предельное напряжение изгибабудет наименьшим.");
                if (d_out.K1_31 < d_out.K1_32)
                {
                    doc.InsertParagraph().AppendEquation($"ϑ_2=ϑ_(2,1)={d_out.v21_3:f2}");
                }
                else
                {
                    doc.InsertParagraph().AppendEquation($"ϑ_2=ϑ_(2,2)={d_out.v22_3:f2}");
                }

                doc.InsertParagraph().AppendEquation($"K_1={d_out.K1_3:f2}");

                doc.InsertParagraph().AppendEquation($"[σ_i]_3={d_out.K1_3:f2}∙{d_out.K2:f2}∙{d_in.sigma_d}={d_out.sigmai3:f2}");

                doc.InsertParagraph().AppendEquation($"[F]_3=(0.9∙{d_out.sigmai2:f2}∙({d_in.s}-{d_in.c})∙√({d_in.D}∙({d_in.s}-{d_in.c})))/({d_out.K14:f2}∙{d_out.K16:f2}∙{d_out.K17:f2})={d_out.F_d3:f2}");

                doc.InsertParagraph().AppendEquation($"{d_out.F1:f2}≤min({d_out.F_d2:f2};{d_out.F_d3:f2})");
                if (d_out.F1 <= Math.Min(d_out.F_d2, d_out.F_d3))
                {
                    doc.InsertParagraph("Условие прочности выполняется");
                }
                else
                {
                    doc.InsertParagraph("Условие прочности не выполняется").Bold().Color(System.Drawing.Color.Red);
                }

                doc.InsertParagraph("Условие устойчивости");
                // TODO: добавить наружное давление
                doc.InsertParagraph().AppendEquation("|p|/[p]+|M_i|/[M]+|F_e|/[F]+(Q_i/[Q])^2≤1");

                doc.InsertParagraph("где ").AppendEquation("F_e").Append(" - эффективное осевое усилие от местных мембранных напряжений, действующих в области опоры");
                doc.InsertParagraph().AppendEquation("F_e=F_i∙π/4∙K_13∙K_15∙√(D/(s-c))");
                doc.InsertParagraph().AppendEquation($"F_e={d_out.F1:f2}∙π/4∙{d_out.K13:f2}∙{d_out.K15:f2}∙√({d_in.D}/({d_in.s}-{d_in.c}))={d_out.Fe:f2}");
                doc.InsertParagraph().AppendEquation($"{d_out.M1:f2}/{d_out.M_d:f2}+{d_out.Fe:f2}/{d_out.F_d:f2}+({d_out.Q1:f2}/{d_out.Q_d:f2})^2={d_out.yslystoich2:f2}≤1");

                if (d_out.yslystoich2 <= 1)
                {
                    doc.InsertParagraph("Условие устойчивости выполняется");
                }
                else
                {
                    doc.InsertParagraph("Условие устойчивости не выполняется").Bold().Color(System.Drawing.Color.Red);
                }
            }
            else if (d_in.type == 2)
            {
                doc.InsertParagraph("Проверка несущей способности обечайки, не укрепленной кольцами жесткости в области опорного узла с подкладным листом в месте опоры");
                doc.InsertParagraph("Вспомогательные параметры и коэффициенты");

                doc.InsertParagraph().AppendEquation("s_ef=(s-c)∙√(1+(s_2/(s-c))^2)");
                doc.InsertParagraph().AppendEquation($"s_ef={d_out.sef:f2}");

                doc.InsertParagraph("Параметр, определяемый расстоянием от середины опоры до днища");
                doc.InsertParagraph().AppendEquation("γ=2.83∙a/D∙√(s_ef)/D");
                doc.InsertParagraph().AppendEquation($"γ={d_out.gamma:f2}");

                doc.InsertParagraph("Параметр, определяемый шириной пояса опоры");
                doc.InsertParagraph().AppendEquation("β_1=0.91∙b_2/√(D∙(s_ef))");
                doc.InsertParagraph().AppendEquation($"β_1={d_out.beta1:f2}");

                doc.InsertParagraph("Коэффициенты, учитывающие влияние ширины пояса опоры");
                doc.InsertParagraph().AppendEquation("K_10=max{(exp(-β_1)∙sin(β_1))/β_1;0.25}");
                doc.InsertParagraph().AppendEquation($"K_10=max({d_out.K10_1:f2};0.25)={d_out.K10:f2}");

                doc.InsertParagraph().AppendEquation("K_11=(1-exp(-β_1)∙cos(β_1))/β_1");
                doc.InsertParagraph().AppendEquation($"K_11={d_out.K11:f2}");

                doc.InsertParagraph("Коэффициенты, учитывающие влияние угла охвата");
                doc.InsertParagraph().AppendEquation("K_12=(1.15-0.1432∙δ_1)/sin(0.5∙δ_1)");
                doc.InsertParagraph().AppendEquation($"K_12={d_out.K12:f2}");

                doc.InsertParagraph().AppendEquation("K_13=(max{1.7-(2.1∙δ_1)/π;0})/sin(0.5∙δ_1)");
                doc.InsertParagraph().AppendEquation($"K_13={d_out.K13:f2}");

                doc.InsertParagraph().AppendEquation("K_14=(1.45-0.43∙δ_1)/sin(0.5∙δ_1)");
                doc.InsertParagraph().AppendEquation($"K_14={d_out.K14:f2}");

                doc.InsertParagraph("Коэффициенты, учитывающие влияние расстояния от середины опоры до днища и угла охвата");
                doc.InsertParagraph().AppendEquation("K_15=min{1.0;(0.8∙√γ+6∙γ)/δ_1}");
                doc.InsertParagraph().AppendEquation($"K_15=min(1.0;{d_out.K15_2:f2})={d_out.K15:f2}");

                doc.InsertParagraph().AppendEquation("K_16=1-0.65/(1+(6∙γ)^2)∙√(π/(3∙δ_1))");
                doc.InsertParagraph().AppendEquation($"K_16={d_out.K16:f2}");

                doc.InsertParagraph("Коэффициенты, учитывающие влияние ширины пояса опоры и угла охвата");
                doc.InsertParagraph().AppendEquation("K_17=1/(1+0.6∙∛(D/(s_ef))∙(b_2/D)∙δ_1)");
                doc.InsertParagraph().AppendEquation($"K_17={d_out.K17:f2}");

                doc.InsertParagraph("Общее мембранное меридиональное напряжение изгиба от весовых нагрузок, действующее в области опорного узла");
                doc.InsertParagraph().AppendEquation("σ ̅_mx=4∙M_i/(π∙D^2∙(s_ef))");
                doc.InsertParagraph().AppendEquation($"σ ̅_mx={d_out.sigma_mx:f2}");

                doc.InsertParagraph("Условие прочности");
                doc.InsertParagraph().AppendEquation("F_1≤min{[F]_2;[F]_3}");
                doc.InsertParagraph("где ").AppendEquation("[F]_2").Append(" - допускаемое опорное усилие от нагружения в меридиональном направлении");
                doc.InsertParagraph().AppendEquation("[F]_2=(0.7∙[σ_i]_2∙(s_ef)∙√(D∙(s_ef)))/(K_10∙K_12)");

                doc.InsertParagraph("\t").AppendEquation("[F]_3").Append(" - допускаемое опорное усилие от нагружения в окружном направлении");
                doc.InsertParagraph().AppendEquation("[F]_3=(0.9∙[σ_i]_3∙(s_ef)∙√(D∙(s_ef)))/(K_14∙K_16∙K_17)");

                doc.InsertParagraph("где ").AppendEquation("[σ_i]_2, [σ_i]_2").Append(" - предельные напряжения изгиба в меридиональном и окружном направлениях");

                doc.InsertParagraph().AppendEquation("[σ_i]=K_1∙K_2∙[σ]");

                doc.InsertParagraph().AppendEquation("K_1=(1-ϑ_2^2)/((1/3+ϑ_1∙ϑ_2)+√((1/3+ϑ_1∙ϑ_2)^2+(1-ϑ_2^2)∙ϑ_1^2))");

                // TODO: для условий монтажа 
                doc.InsertParagraph().AppendEquation("K_2=1.25").Append(" - для рабочих условий");

                doc.InsertParagraph("для ").AppendEquation("[σ_i]_2");
                doc.InsertParagraph().AppendEquation("ϑ_1=-(0,23∙K_13∙K_15)/(K_12∙K_10)");
                doc.InsertParagraph().AppendEquation($"ϑ_1={d_out.v1_2:f2}");

                doc.InsertParagraph().AppendEquation("ϑ_(2,1)=- ̅σ_mx∙1/(K_2∙[σ])");
                doc.InsertParagraph().AppendEquation($"ϑ_(2,1)={d_out.v21_2:f2}");
                doc.InsertParagraph().AppendEquation("ϑ_(2,2)=[(p∙D)/(4∙(s_ef))- ̅σ_mx]∙1/(K_2∙[σ])");
                doc.InsertParagraph().AppendEquation($"ϑ_(2,2)={d_out.v22_2:f2}");

                doc.InsertParagraph("Для ").AppendEquation("ϑ_2").Append(" принимают одно из значений ").AppendEquation("ϑ_(2,1)").Append(" или ").AppendEquation("ϑ_(2,2)").Append(", для которого предельное напряжение изгибабудет наименьшим.");
                if (d_out.K1_21 < d_out.K1_22)
                {
                    doc.InsertParagraph().AppendEquation($"ϑ_2=ϑ_(2,1)={d_out.v21_2:f2}");
                }
                else
                {
                    doc.InsertParagraph().AppendEquation($"ϑ_2=ϑ_(2,2)={d_out.v22_2:f2}");
                }

                doc.InsertParagraph().AppendEquation($"K_1={d_out.K1_2:f2}");

                doc.InsertParagraph().AppendEquation($"[σ_i]_2={d_out.K1_2:f2}∙{d_out.K2:f2}∙{d_in.sigma_d}={d_out.sigmai2:f2}");

                doc.InsertParagraph().AppendEquation($"[F]_2=(0.7∙{d_out.sigmai2:f2}∙({d_out.sef:f2})∙√({d_in.D}∙({d_out.sef:f2})))/({d_out.K10:f2}∙{d_out.K12:f2})={d_out.F_d2:f2}");


                doc.InsertParagraph("для ").AppendEquation("[σ_i]_3");
                doc.InsertParagraph().AppendEquation("ϑ_1=-(0,53∙K_11)/(K_14∙K_16∙K_17∙sin(0.5∙δ_1))");
                doc.InsertParagraph().AppendEquation($"ϑ_1={d_out.v1_3:f2}");

                doc.InsertParagraph().AppendEquation("ϑ_(2,1)=0");
                doc.InsertParagraph().AppendEquation($"ϑ_(2,1)={d_out.v21_3:f2}");
                doc.InsertParagraph().AppendEquation("ϑ_(2,2)=(p∙D)/(2∙(s_ef))∙1/(K_2∙[σ])");
                doc.InsertParagraph().AppendEquation($"ϑ_(2,2)={d_out.v22_3:f2}");

                doc.InsertParagraph("Для ").AppendEquation("ϑ_2").AppendEquation(" принимают одно из значений ").AppendEquation("ϑ_(2,1)").Append(" или ").AppendEquation("ϑ_(2,2)").Append(", для которого предельное напряжение изгибабудет наименьшим.");
                if (d_out.K1_31 < d_out.K1_32)
                {
                    doc.InsertParagraph().AppendEquation($"ϑ_2=ϑ_(2,1)={d_out.v21_3:f2}");
                }
                else
                {
                    doc.InsertParagraph().AppendEquation($"ϑ_2=ϑ_(2,2)={d_out.v22_3:f2}");
                }

                doc.InsertParagraph().AppendEquation($"K_1={d_out.K1_3:f2}");

                doc.InsertParagraph().AppendEquation($"[σ_i]_3={d_out.K1_3:f2}∙{d_out.K2:f2}∙{d_in.sigma_d}={d_out.sigmai3:f2}");

                doc.InsertParagraph().AppendEquation($"[F]_3=(0.9∙{d_out.sigmai2:f2}∙({d_out.sef:f2})∙√({d_in.D}∙({d_out.sef:f2})))/({d_out.K14:f2}∙{d_out.K16:f2}∙{d_out.K17:f2})={d_out.F_d3:f2}");

                doc.InsertParagraph().AppendEquation($"{d_out.F1:f2}≤min({d_out.F_d2:f2};{d_out.F_d3:f2})");
                if (d_out.F1 <= Math.Min(d_out.F_d2, d_out.F_d3))
                {
                    doc.InsertParagraph("Условие прочности выполняется");
                }
                else
                {
                    doc.InsertParagraph("Условие прочности не выполняется").Bold().Color(System.Drawing.Color.Red);
                }

                doc.InsertParagraph("Условие устойчивости");
                // TODO: добавить наружное давление
                doc.InsertParagraph().AppendEquation("|p|/[p]+|M_i|/[M]+|F_e|/[F]+(Q_i/[Q])^2≤1");

                doc.InsertParagraph("где ").AppendEquation("F_e").Append(" - эффективное осевое усилие от местных мембранных напряжений, действующих в области опоры");
                doc.InsertParagraph().AppendEquation("F_e=F_i∙π/4∙K_13∙K_15∙√(D/(s_ef))");
                doc.InsertParagraph().AppendEquation($"F_e={d_out.F1:f2}∙π/4∙{d_out.K13:f2}∙{d_out.K15:f2}∙√({d_in.D}/({d_out.sef:f2}))={d_out.Fe:f2}");
                doc.InsertParagraph().AppendEquation($"{d_out.M1:f2}/{d_out.M_d:f2}+{d_out.Fe:f2}/{d_out.F_d:f2}+({d_out.Q1:f2}/{d_out.Q_d:f2})^2={d_out.yslystoich2:f2}≤1");

                if (d_out.yslystoich2 <= 1)
                {
                    doc.InsertParagraph("Условие устойчивости выполняется");
                }
                else
                {
                    doc.InsertParagraph("Условие устойчивости не выполняется").Bold().Color(System.Drawing.Color.Red);
                }
            }

            if (d_out.isConditionUseFormuls)
            {
                doc.InsertParagraph("Условия применения формул");
            }
            else
            {
                doc.InsertParagraph("Условия применения формул не выполняются").Bold().Color(System.Drawing.Color.Red);
            }
            doc.InsertParagraph().AppendEquation("60°≤δ_1≤180°");
            doc.InsertParagraph($"60°≤{d_in.delta1}°≤180°");
            doc.InsertParagraph().AppendEquation("(s-c)/D≤0.05");
            doc.InsertParagraph().AppendEquation($"({d_in.s}-{d_in.c})/{d_in.D}={(d_in.s - d_in.c) / d_in.D:f2}≤0.05");
            if (d_in.type == 2)
            {
                doc.InsertParagraph().AppendEquation("s_2≥s");
                doc.InsertParagraph().AppendEquation($"{d_in.s2} мм ≥ {d_in.s} мм");
                doc.InsertParagraph().AppendEquation("δ_2≥δ_1+20°");
                doc.InsertParagraph().AppendEquation($"{d_in.delta2}°≥{d_in.delta1}°+20°={d_in.delta1 + 20}°");
                doc.InsertParagraph().AppendEquation("A_k≥(s-c)√(D∙(s-c))");
                doc.InsertParagraph().AppendEquation($"{d_out.Ak:f2}≥({d_in.s}-{d_in.c})√({d_in.D}∙({d_in.s}-{d_in.c}))={d_out.Akypf:f2}");
            }

            doc.Save();
        }

        internal static void MakeLit(List<int> bibliography, string Docum = null)
        {
            
                

            const string GOST_34233_1 = "ГОСТ 34233.1-2017 Сосуды и аппараты. Нормы и методы расчета на прочность. Общие требования";
            const string GOST_34233_2 = "ГОСТ 34233.2-2017 Сосуды и аппараты. Нормы и методы расчета на прочность. " +
                                        "Расчет цилиндрических и конических обечаек, выпуклых и плоских днищ и крышек";
            const string GOST_34233_3 = "ГОСТ 34233.3-2017 Сосуды и аппараты. Нормы и методы расчета на прочность. " +
                                        "Укрепление отверстий в обечайках и днищах при внутреннем и наружном давлениях. " +
                                        "Расчет на прочность обечаек и днищ при внешних статических нагрузках на штуцер";
            const string GOST_34233_4 = "ГОСТ 34233.4-2017 Сосуды и аппараты. Нормы и методы расчета на прочность. " +
                                        "Расчет на прочность и герметичность фланцевых соединений";
            const string GOST_34233_5 = "ГОСТ 34233.5-2017 Сосуды и аппараты. Нормы и методы расчета на прочность. " +
                                        "Расчет  обечаек и днищ от воздействия опорных нагрузок";
            const string GOST_34233_6 = "ГОСТ 34233.6-2017 Сосуды и аппараты. Нормы и методы расчета на прочность. " +
                                        "Расчет на прочность при малоцикловых нагрузках";
            const string GOST_34233_7 = "ГОСТ 34233.7-2017 Сосуды и аппараты. Нормы и методы расчета на прочность. " +
                                        "Теплообменные аппараты";
            const string GOST_34233_8 = "ГОСТ 34233.8-2017 Сосуды и аппараты. Нормы и методы расчета на прочность. " +
                                        "Сосуды и аппараты с рубашками";
            const string GOST_34233_9 = "ГОСТ 34233.9-2017 Сосуды и аппараты. Нормы и методы расчета на прочность. " +
                                        "Аппараты колонного типа";
            const string GOST_34233_10 = "ГОСТ 34233.10-2017 Сосуды и аппараты. Нормы и методы расчета на прочность. " +
                                        "Сосуды и аппараты работающие с сероводородными средами";
            const string GOST_34233_11 = "ГОСТ 34233.11-2017 Сосуды и аппараты. Нормы и методы расчета на прочность. " +
                                        "Метод расчета на прочность обечаек и днищ с учетом смещения кромок сварных соединений, " +
                                        "угловатости и некруглости обечаек";

            

            if (bibliography != null)
            {
                var doc = Xceed.Words.NET.DocX.Load(Docum);
                doc.InsertParagraph().InsertPageBreakAfterSelf();
                doc.InsertParagraph("Литература")
                   .Heading(HeadingType.Heading1)
                   .Color(System.Drawing.Color.Black).Alignment = Alignment.center;
                doc.InsertParagraph("1. " + GOST_34233_1);

                List<int> bibliographySort = bibliography.Distinct()
                                                         .ToList();
                bibliographySort.Sort();

                string[] Gosts = new string[] { "", GOST_34233_1, GOST_34233_2, GOST_34233_3, GOST_34233_4, GOST_34233_5,
                                            GOST_34233_6, GOST_34233_7, GOST_34233_8, GOST_34233_9, GOST_34233_10,
                                            GOST_34233_11 };

                for (int i = 0; i < bibliographySort.Count; i++)
                {
                    doc.InsertParagraph($"{i + 1}. " + Gosts[bibliographySort[i]]);

                    //switch (bibliographySort[i])
                    //{
                    //    case 1:
                    //        doc.InsertParagraph($"{i + 1}. " + GOST_34233_1);
                    //        break;
                    //    case 2:
                    //        doc.InsertParagraph($"{i + 1}. " + GOST_34233_2);
                    //        break;
                    //    case 3:
                    //        doc.InsertParagraph($"{i + 1}. " + GOST_34233_3);
                    //        break;
                    //    case 4:
                    //        doc.InsertParagraph($"{i + 1}. " + GOST_34233_4);
                    //        break;
                    //    case 5:
                    //        doc.InsertParagraph($"{i + 1}. " + GOST_34233_5);
                    //        break;
                    //    case 6:
                    //        doc.InsertParagraph($"{i + 1}. " + GOST_34233_6);
                    //        break;
                    //    case 7:
                    //        doc.InsertParagraph($"{i + 1}. " + GOST_34233_7);
                    //        break;
                    //    case 8:
                    //        doc.InsertParagraph($"{i + 1}. " + GOST_34233_8);
                    //        break;
                    //    case 9:
                    //        doc.InsertParagraph($"{i + 1}. " + GOST_34233_9);
                    //        break;
                    //    case 10:
                    //        doc.InsertParagraph($"{i + 1}. " + GOST_34233_10);
                    //        break;
                    //    case 11:
                    //        doc.InsertParagraph($"{i + 1}. " + GOST_34233_11);
                    //        break;
                    //}
                }

                doc.Save();
            }
        }

    }
}
