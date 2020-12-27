using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Document.NET;

namespace calc
{
    class MakeWord
    {
        internal void MakeWord_cil(Data_in d_in, Data_out d_out, string Docum = null)
        {
            if (Docum == null)
            {
                Docum = "temp.docx";
            }
            
            var doc = Xceed.Words.NET.DocX.Load(Docum);

            doc.InsertParagraph().InsertPageBreakAfterSelf();
            doc.InsertParagraph($"Расчет на прочность обечайки {d_in.name}, нагруженной ").Heading(HeadingType.Heading1).Alignment = Alignment.center;
            if (d_in.dav == 0)
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

            var table = doc.AddTable(1, 2);
            table.SetWidths(new float[] { 300, 100 });
            //int i = 0;
            //table.InsertRow(i);
            table.Rows[0].Cells[0].Paragraphs[0].Append("Материал обечайки");
            table.Rows[0].Cells[1].Paragraphs[0].Append($"{d_in.steel}");
            int i = 1;
            table.InsertRow(i);
            table.Rows[i].Cells[0].Paragraphs[0].Append("Внутренний диаметр обечайки, D:");
            table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.D} мм");
            i += 1;
            if (d_in.dav == 1)
            {
                table.InsertRow(i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Длина обечайки, l:");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.l} мм");
                i += 1;
            }
            table.InsertRow(i);
            table.Rows[i].Cells[0].Paragraphs[0].Append("Прибавка на коррозию, ").AppendEquation("c_1").Append(":");
            table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.c1} мм");
            i += 1;
            table.InsertRow(i);
            table.Rows[i].Cells[0].Paragraphs[0].Append("Прибавка для компенсации минусового допуска, ").AppendEquation("c_2").Append(":");
            table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.c2} мм");
            i += 1;
            if (d_in.c3 > 0)
            {
                table.InsertRow(i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Технологическая прибавка, ").AppendEquation("c_3").Append(":");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.c3} мм");
                i += 1;
            }
            table.InsertRow(i);
            table.Rows[i].Cells[0].Paragraphs[0].Append("Коэффициент прочности сварного шва, ").AppendEquation("φ_p").Append(":");
            table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.fi} мм");

            doc.InsertParagraph().InsertTableAfterSelf(table);
            

            doc.InsertParagraph();
            doc.InsertParagraph("Условия нагружения").Alignment = Alignment.center;
            var table1 = doc.AddTable(1, 2);
            table1.SetWidths(new float[] { 300, 100 });

            table1.Rows[0].Cells[0].Paragraphs[0].Append("Расчетная температура, Т:");
            table1.Rows[0].Cells[1].Paragraphs[0].Append($"{d_in.temp} °С");
            i = 1;
            table1.InsertRow(i);
            if (d_in.dav == 0)
            {
                table1.Rows[i].Cells[0].Paragraphs[0].Append("Расчетное внутреннее избыточное давление, p:");
            }
            else
            {
                table1.Rows[i].Cells[0].Paragraphs[0].Append("Расчетное наружное давление, p:");
            }
            table1.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.p} МПа");
            i += 1;
            table1.InsertRow(i);
            table1.Rows[i].Cells[0].Paragraphs[0].Append($"Допускаемое напряжение для материала {d_in.steel} при расчетной температуре, [σ]:");
            table1.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.sigma_d} МПа");
            i += 1;
            if (d_in.dav == 1)
            {
                table1.InsertRow(i);
                table1.Rows[i].Cells[0].Paragraphs[0].Append("Модуль продольной упругости при расчетной температуре, E:");
                table1.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.E} МПа");
            }

            doc.InsertParagraph().InsertTableAfterSelf(table1);

            doc.InsertParagraph("");
            doc.InsertParagraph("Результаты расчета").Alignment = Alignment.center;
            doc.InsertParagraph("");
            doc.InsertParagraph("Толщину стенки вычисляют по формуле:");
            doc.InsertParagraph().AppendEquation("s≥s_p+c");
            doc.InsertParagraph("где ").AppendEquation("s_p").Append(" - расчетная толщина стенки обечайки");
            if (d_in.dav == 0)
            {
                doc.InsertParagraph().AppendEquation("s_p=(p∙D)/(2∙[σ]∙φ_p-p)");
                doc.InsertParagraph().AppendEquation($"s_p=({d_in.p}∙{d_in.D})/(2∙{d_in.sigma_d}∙{d_in.fi}-{d_in.p})={d_out.s_calcr:f2} мм");
            }
            else if (d_in.dav == 1)
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
            if (d_in.dav == 0)
            {
                doc.InsertParagraph("Допускаемое внутреннее избыточное давление вычисляют по формуле:");
                doc.InsertParagraph().AppendEquation("[p]=(2∙[σ]∙φ_p∙(s-c))/(D+s-c)");
                doc.InsertParagraph().AppendEquation($"[p]=(2∙{d_in.sigma_d}∙{d_in.fi}∙({d_in.s}-{d_out.c:f2}))/({d_in.D}+{d_in.s}-{d_out.c:f2})={d_out.p_d:f2} МПа");
            }
            else if (d_in.dav == 1)
            {
                doc.InsertParagraph("Допускаемое наружное давление вычисляют по формуле:");
                doc.InsertParagraph().AppendEquation("[p]=[p]_П/√(1+([p]_П/[p]_E)^2)");
                doc.InsertParagraph("допускаемое давление из условия прочности вычисляют по формуле:");
                doc.InsertParagraph().AppendEquation("[p]_П=(2∙[σ]∙(s-c))/(D+s-c)");
                doc.InsertParagraph().AppendEquation($"[p]_П=(2∙{d_in.sigma_d}∙({d_in.s}-{d_out.c:f2}))/({d_in.D}+{d_in.s}-{d_out.c:f2})={d_out.p_dp:f2} МПа");
                doc.InsertParagraph("допускаемое давление из условия устойчивости в пределах упругости вычисляют по формуле:");
                doc.InsertParagraph().AppendEquation("[p]_E=(2.08∙10^-5∙E)/(n_y∙B_1)∙D/l∙[(100∙(s-c))/D]^2.5");
                doc.InsertParagraph("коэффициент ").AppendEquation("B_1").Append(" вычисляют по формуле");
                doc.InsertParagraph().AppendEquation("B_1=min{1;9.45∙D/l∙√(D/(100∙(s-c)))}");
                doc.InsertParagraph().AppendEquation($"9.45∙{d_in.D}/{d_out.l}∙√({d_in.D}/(100∙({d_in.s}-{d_out.c:f2})))={d_out.b1_2:f2}");
                doc.InsertParagraph().AppendEquation($"B_1=min(1;{d_out.b1_2:f2})={d_out.b1:f1}");
                doc.InsertParagraph().AppendEquation($"[p]_E=(2.08∙10^-5∙{d_in.E})/({d_in.ny}∙{d_out.b1:f2})∙{d_in.D}/{d_out.l}∙[(100∙({d_in.s}-{d_out.c:f2}))/{d_in.D}]^2.5={d_out.p_de:f2} МПа");
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
            if (d_out.ypf)
            {
                doc.InsertParagraph("Границы применения формул ");
            }
            else
            {
                doc.InsertParagraph("Границы применения формул. ловие не выполняется ").Bold().Color(System.Drawing.Color.Red);
            }
            if (d_in.D >= 200)
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

        internal void MakeWord_ell(Data_in d_in, Data_out d_out, string Docum = null)
        {
            var doc = Xceed.Words.NET.DocX.Load(Docum);
            // добавить полусферическое

            doc.InsertParagraph().InsertPageBreakAfterSelf();
            doc.InsertParagraph($"Расчет на прочность эллиптического днища {d_in.name}, нагруженного ").Heading(HeadingType.Heading1).Alignment = Alignment.center;
            if (d_in.dav == 0)
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

            var table = doc.AddTable(1, 2);
            table.SetWidths(new float[] { 300, 100 });
            //int i = 0;
            //table.InsertRow(i);
            table.Rows[0].Cells[0].Paragraphs[0].Append("Материал днища");
            table.Rows[0].Cells[1].Paragraphs[0].Append($"{d_in.steel}");
            int i = 1;
            table.InsertRow(i);
            table.Rows[i].Cells[0].Paragraphs[0].Append("Внутренний диаметр днища, D:");
            table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.D} мм");
            i += 1;
            table.InsertRow(i);
            table.Rows[i].Cells[0].Paragraphs[0].Append("Высота выпуклой части, H:");
            table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.elH} мм");
            i += 1;
            table.InsertRow(i);
            table.Rows[i].Cells[0].Paragraphs[0].Append("Длина отбортовки ").AppendEquation("h_1").Append(":");
            table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.elh1}");
            i += 1;
            table.InsertRow(i);
            table.Rows[i].Cells[0].Paragraphs[0].Append("Прибавка на коррозию, ").AppendEquation("c_1").Append(":");
            table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.c1} мм");
            i += 1;
            table.InsertRow(i);
            table.Rows[i].Cells[0].Paragraphs[0].Append(" Прибавка для компенсации минусового допуска листа, ").AppendEquation("c_2").Append(":");
            table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.c2} мм");
            i += 1;
            if (d_in.c3 > 0)
            {
                table.InsertRow(i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Технологическая прибавка, ").AppendEquation("c_3").Append(":");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.c3}");
                i += 1;
            }
            table.InsertRow(i);
            table.Rows[i].Cells[0].Paragraphs[0].Append("Коэффициент прочности сварного шва, ").AppendEquation("φ_p");
            table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.fi}");
            doc.InsertParagraph().InsertTableAfterSelf(table);

            doc.InsertParagraph();
            doc.InsertParagraph("Условия нагружения").Alignment = Alignment.center;
            var table1 = doc.AddTable(1, 2);
            table1.SetWidths(new float[] { 300, 100 });

            table1.Rows[0].Cells[0].Paragraphs[0].Append("Расчетная температура, Т:");
            table1.Rows[0].Cells[1].Paragraphs[0].Append($"{d_in.temp} °С");
            i = 1;
            table1.InsertRow(i);
            if (d_in.dav == 0)
            {
                table1.Rows[i].Cells[0].Paragraphs[0].Append("Расчетное внутреннее избыточное давление, p:");
            }
            else
            {
                table1.Rows[i].Cells[0].Paragraphs[0].Append("Расчетное наружное давление, p:");
            }
            table1.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.p} МПа");
            i += 1;
            table1.InsertRow(i);
            table1.Rows[i].Cells[0].Paragraphs[0].Append($"Допускаемое напряжение для материала {d_in.steel} при расчетной температуре, [σ]:");
            table1.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.sigma_d} МПа");
            i += 1;
            if (d_in.dav == 1)
            {
                table1.InsertRow(i);
                table1.Rows[i].Cells[0].Paragraphs[0].Append("Модуль продольной упругости при расчетной температуре, E:");
                table1.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.E} МПа");
            }

            doc.InsertParagraph().InsertTableAfterSelf(table1);

            doc.InsertParagraph();
            doc.InsertParagraph("Результаты расчета").Alignment = Alignment.center;
            doc.InsertParagraph();
            doc.InsertParagraph("Толщину стенки вычисляют по формуле:");
            doc.InsertParagraph().AppendEquation("s_1≥s_1p+c");
            doc.InsertParagraph("где ").AppendEquation("s_1p").Append(" - расчетная толщина стенки днища");
            if (d_in.dav == 0)
            {
                doc.InsertParagraph().AppendEquation("s_1p=(p∙R)/(2∙[σ]∙φ-0.5∙p)");
            }
            else if (d_in.dav == 1)
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
                doc.InsertParagraph().AppendEquation("R={d_in.D}^2/(4∙{d_in.elH})={d_out.elR} мм");
            }
            if (d_in.dav == 0)
            {
                doc.InsertParagraph().AppendEquation($"s_p=({d_in.p}∙{d_out.elR})/(2∙{d_in.sigma_d}∙{d_in.fi}-0.5{d_in.p})={d_out.s_calcr:f2} мм");
            }
            else if (d_in.dav == 1)
            {
                doc.InsertParagraph("Для предварительного расчета ").AppendEquation("К_Э=0.9").Append(" для эллиптических днищ");
                doc.InsertParagraph().AppendEquation($"(0.9∙{d_out.elR})/(161)∙√(({d_in.ny}∙{d_in.p})/(10^-5∙{d_in.E}))={d_out.s_calcr1:f2}");
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
            doc.InsertParagraph().AppendEquation($"[p]=(2∙{d_in.sigma_d}∙{d_in.fi}∙({d_in.s}-{d_out.c:f2}))/({d_out.elR}+0.5∙({d_in.s}-{d_out.c:f2}))={d_out.p_d:f2} МПа");
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
            if (d_out.ypf)
            {
                doc.InsertParagraph("Границы применения формул ");
            }
            else
            {
                doc.InsertParagraph().AppendEquation("Границы применения формул. Условие не выполняется").Bold().Color(System.Drawing.Color.Red);
            }
            //# эллептические днища
            doc.InsertParagraph().AppendEquation("0.002≤(s_1-c)/(D)≤0.1");
            doc.InsertParagraph().AppendEquation($"0.002≤({d_in.s}-{d_out.c:f2})/({d_in.D})={(d_in.s-d_out.c)/d_in.D:f3}≤0.1");
            doc.InsertParagraph().AppendEquation("0.2≤H/D≤0.5");
            doc.InsertParagraph().AppendEquation($"0.2≤{d_in.elH}/{d_in.D}={d_in.elH/d_in.D:f3}<0.5");


            doc.Save();
        }

        internal void MakeWord_nozzle(Data_in d_in, Data_out d_out, DataNozzle_in dN_in, DataNozzle_out dN_out, string Docum = null)
        {
            var doc = Xceed.Words.NET.DocX.Load(Docum);
            doc.InsertParagraph().InsertPageBreakAfterSelf();
            doc.InsertParagraph($"Расчет на прочность узла врезки штуцера {dN_in.name} в ").Heading(HeadingType.Heading1).Alignment = Alignment.center;
            switch (d_in.met)
            {
                case "obvn":
                case "obnar":
                    doc.Paragraphs.Last().Append($"обечайку {d_in.name}, нагруженную ");
                    break;
                case "konvn":
                case "konnar":
                    doc.Paragraphs.Last().Append($"коническую обечайку {d_in.name}, нагруженную ");
                    break;
                case "ellvn":
                case "ellnar":
                    doc.Paragraphs.Last().Append($"эллиптическое днище {d_in.name}, нагруженное ");
                    break;
            }
            if (d_in.dav == 0)
            {
                doc.Paragraphs.Last().Append("внутренним избыточным давлением");
            }
            else if (d_in.dav == 1)
            {
                doc.Paragraphs.Last().Append("наружным давлением");
            }
            doc.InsertParagraph();
            doc.InsertParagraph("Исходные данные").Alignment = Alignment.center;

            var table = doc.AddTable(1, 2);
            table.SetWidths(new float[] { 200, 200 });
            
            table.Rows[0].Cells[0].Paragraphs[0].Append("Элемент:");
            table.Rows[0].Cells[1].Paragraphs[0].Append($"Штуцер {dN_in.name}");
            
            table.InsertRow();
            table.Rows[1].Cells[0].Paragraphs[0].Append("Элемент несущий штуцер:");
            table.Rows[1].Cells[1].Paragraphs[0].Append($"{d_in.name} мм");

            table.InsertRow();
            table.Rows[2].Cells[0].Paragraphs[0].Append("Тип элемента, несущего штуцер:");
            switch (d_in.met)
            {
                case "obvn":
                case "obnar":
                    table.Rows[2].Cells[1].Paragraphs[0].Append("Обечайка цилиндрическая");
                    break;
                case "konvn":
                case "konnar":
                    table.Rows[2].Cells[1].Paragraphs[0].Append("Обечайка коническая");
                    break;
                case "ellvn":
                case "ellnar":
                    table.Rows[2].Cells[1].Paragraphs[0].Append("Днище эллиптическое");
                    break;
            }
            table.InsertRow();
            table.Rows[3].Cells[0].Paragraphs[0].Append("Тип штуцера:");
            switch (dN_in.vid)
            {
                case 1:
                    table.Rows[3].Cells[1].Paragraphs[0].Append("Непроходящий без укрепления");
                    break;
                case 2:
                    table.Rows[3].Cells[1].Paragraphs[0].Append("Проходящий без укрепления");
                    break;
                case 3:
                    table.Rows[3].Cells[1].Paragraphs[0].Append("Непроходящий с накладным кольцом");
                    break;
                case 4:
                    table.Rows[3].Cells[1].Paragraphs[0].Append("Проходящий с накладным кольцом");
                    break;
                case 5:
                    table.Rows[3].Cells[1].Paragraphs[0].Append("С накладным кольцом и внутренней частью");
                    break;
                case 6:
                    table.Rows[3].Cells[1].Paragraphs[0].Append("С отбортовкой");
                    break;
                case 7:
                    table.Rows[3].Cells[1].Paragraphs[0].Append("С торовой вставкой");
                    break;
                case 8:
                    table.Rows[3].Cells[1].Paragraphs[0].Append("С вварным кольцом");
                    break;
            }
            doc.InsertParagraph().InsertTableAfterSelf(table);

            var image = doc.AddImage($"pic/Nozzle/Nozzle{dN_in.vid}.gif");
            var picture = image.CreatePicture();
            doc.InsertParagraph().AppendPicture(picture);

            var table1 = doc.AddTable(1, 2);
            table1.SetWidths(new float[] { 300, 100 });

            table1.Rows[0].Cells[0].Paragraphs[0].Append("Материал несущего элемента:");
            table1.Rows[0].Cells[1].Paragraphs[0].Append($"{d_in.steel}");
            int i = 1;
            table1.InsertRow(i);
            table1.Rows[i].Cells[0].Paragraphs[0].Append("Толщина стенки несущего элемента, s:");
            table1.Rows[i].Cells[1].Paragraphs[0].Append($"{d_out.c:f2} мм");
            i += 1;
            table1.InsertRow(i);
            table1.Rows[i].Cells[0].Paragraphs[0].Append("Сумма прибавок к стенке несущего элемента, c:");
            table1.Rows[i].Cells[1].Paragraphs[0].Append($"{d_out.c:f2} мм");
            i += 1;
            table1.InsertRow(i);
            table1.Rows[i].Cells[0].Paragraphs[0].Append("Материал штуцера");
            table1.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.steel1}");
            i += 1;
            table1.InsertRow(i);
            table1.Rows[i].Cells[0].Paragraphs[0].Append("Внутренний диаметр штуцера, d:");
            table1.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.D} мм");
            i += 1;
            table1.InsertRow(i);
            table1.Rows[i].Cells[0].Paragraphs[0].Append("Толщина стенки штуцера, ").AppendEquation("s_1").Append(":");
            table1.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.s1} мм");
            i += 1;
            table1.InsertRow(i);
            table1.Rows[i].Cells[0].Paragraphs[0].Append("Длина наружной части штуцера, ").AppendEquation("s_1").Append(":");
            table1.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.l1} мм");
            i += 1;
            table1.InsertRow(i);
            table1.Rows[i].Cells[0].Paragraphs[0].Append("Сумма прибавок к толщине стенки штуцера, ").AppendEquation("c_s").Append(":");
            table1.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.cs} мм");
            i += 1;
            table1.InsertRow(i);
            table1.Rows[i].Cells[0].Paragraphs[0].Append("Прибавка на коррозию, ").AppendEquation("c_s1").Append(":");
            table1.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.cs1} мм");
            i += 1;
            switch (dN_in.vid)
            {
                case 1:
                    table1.InsertRow(i);
                    table1.Rows[i].Cells[0].Paragraphs[0].Append("Минимальный размер сварного шва, Δ:");
                    table1.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.delta} мм");
                    i += 1;
                    break;
                case 2:
                    table1.InsertRow(i);
                    table1.Rows[i].Cells[0].Paragraphs[0].Append("Длина внутренней части штуцера, ").AppendEquation("l_3").Append(":");
                    table1.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.l3} мм");
                    i += 1;
                    table1.InsertRow(i);
                    table1.Rows[i].Cells[0].Paragraphs[0].Append("Толщина внутренней части штуцера, ").AppendEquation("s_3").Append(":");
                    table1.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.s3} мм");
                    i += 1;
                    table1.InsertRow(i);
                    table1.Rows[i].Cells[0].Paragraphs[0].Append("Минимальный размер сварного шва, Δ:");
                    table1.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.delta} мм");
                    i += 1;
                    break;
                case 3:
                    table1.InsertRow(i);
                    table1.Rows[i].Cells[0].Paragraphs[0].Append("Ширина накладного кольца, ").AppendEquation("l_2").Append(":");
                    table1.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.l2} мм");
                    i += 1;
                    table1.InsertRow(i);
                    table1.Rows[i].Cells[0].Paragraphs[0].Append("Толщина накладного кольца, ").AppendEquation("s_2").Append(":");
                    table1.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.s2} мм");
                    i += 1;
                    table1.InsertRow(i);
                    table1.Rows[i].Cells[0].Paragraphs[0].Append("Минимальный размер сварного шва, Δ:");
                    table1.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.delta} мм");
                    i += 1;
                    break;
                case 4:
                    table1.InsertRow(i);
                    table1.Rows[i].Cells[0].Paragraphs[0].Append("Ширина накладного кольца, ").AppendEquation("l_2").Append(":");
                    table1.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.l2} мм");
                    i += 1;
                    table1.InsertRow(i);
                    table1.Rows[i].Cells[0].Paragraphs[0].Append("Толщина накладного кольца, ").AppendEquation("s_2").Append(":");
                    table1.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.s2} мм");
                    i += 1;
                    table1.InsertRow(i);
                    table1.Rows[i].Cells[0].Paragraphs[0].Append("Длина внутренней части штуцера, ").AppendEquation("l_3").Append(":");
                    table1.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.l3} мм");
                    i += 1;
                    table1.InsertRow(i);
                    table1.Rows[i].Cells[0].Paragraphs[0].Append("Толщина внутренней части штуцера, ").AppendEquation("s_3").Append(":");
                    table1.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.s3} мм");
                    i += 1;
                    table1.InsertRow(i);
                    table1.Rows[i].Cells[0].Paragraphs[0].Append("Минимальный размер сварного шва, Δ:");
                    table1.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.delta} мм");
                    i += 1;
                    break;
                case 5:
                    table1.InsertRow(i);
                    table1.Rows[i].Cells[0].Paragraphs[0].Append("Ширина накладного кольца, ").AppendEquation("l_2").Append(":");
                    table1.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.l2} мм");
                    i += 1;
                    table1.InsertRow(i);
                    table1.Rows[i].Cells[0].Paragraphs[0].Append("Толщина накладного кольца, ").AppendEquation("s_2").Append(":");
                    table1.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.s2} мм");
                    i += 1;
                    table1.InsertRow(i);
                    table1.Rows[i].Cells[0].Paragraphs[0].Append("Длина внутренней части штуцера, ").AppendEquation("l_3").Append(":");
                    table1.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.l3} мм");
                    i += 1;
                    table1.InsertRow(i);
                    table1.Rows[i].Cells[0].Paragraphs[0].Append("Толщина внутренней части штуцера, ").AppendEquation("s_3").Append(":");
                    table1.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.s3} мм");
                    i += 1;
                    table1.InsertRow(i);
                    table1.Rows[i].Cells[0].Paragraphs[0].Append("Минимальный размер сварного шва, Δ:");
                    table1.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.delta} мм");
                    i += 1;
                    break;
                case 6:
                    table1.InsertRow(i);
                    table1.Rows[i].Cells[0].Paragraphs[0].Append("Радиус отбортовки, r:");
                    //table1.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.rotbort} мм");
                    i += 1;
                    table1.InsertRow(i);
                    table1.Rows[i].Cells[0].Paragraphs[0].Append("Минимальный размер сварного шва, Δ:");
                    table1.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.delta} мм");
                    i += 1;
                    break;
                case 7:
                    break;
                case 8:
                    break;
            }
            doc.InsertParagraph().InsertTableAfterSelf(table1);

            doc.InsertParagraph("Коэффициенты прочности сварных швов:");
            doc.InsertParagraph("Продольный шов штуцера ").AppendEquation($"φ_1={dN_in.fi1}");
            doc.InsertParagraph("Шов обечайки в зоне врезки штуцера ").AppendEquation($"φ={dN_in.fi}");

            doc.InsertParagraph();
            doc.InsertParagraph("Условия нагружения").Alignment = Alignment.center;
            var table2 = doc.AddTable(1, 2);
            table1.SetWidths(new float[] { 300, 100 });

            table2.Rows[0].Cells[0].Paragraphs[0].Append("Расчетная температура, Т:");
            table2.Rows[0].Cells[1].Paragraphs[0].Append($"{d_in.temp} °С");
            i = 1;
            table2.InsertRow(i);
            if (d_in.dav == 0)
            {
                table2.Rows[i].Cells[0].Paragraphs[0].Append("Расчетное внутреннее избыточное давление, p:");
            }
            else
            {
                table2.Rows[i].Cells[0].Paragraphs[0].Append("Расчетное наружное давление, p:");
            }
            table2.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.p} МПа");
            i += 1;
            table2.InsertRow(i);
            table2.Rows[i].Cells[0].Paragraphs[0].Append($"Допускаемое напряжение для материала {dN_in.steel1} при расчетной температуре, ").AppendEquation("[σ]_1").Append(":");
            table2.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.sigma_d1} МПа");
            i += 1;
            if (d_in.dav == 1)
            {
                table2.InsertRow(i);
                table2.Rows[i].Cells[0].Paragraphs[0].Append("Модуль продольной упругости при расчетной температуре, ").AppendEquation("E_1").Append(":");
                table2.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.E1} МПа");
                i += 1;
            }
            if (dN_in.steel1 != dN_in.steel2)
            {
                table2.InsertRow(i);
                table2.Rows[i].Cells[0].Paragraphs[0].Append($"Допускаемое напряжение для материала {dN_in.steel2} при расчетной температуре, ").AppendEquation("[σ]_2").Append(":");
                table2.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.sigma_d2} МПа");
                i += 1;
                if (d_in.dav == 1)
                {
                    table2.InsertRow(i);
                    table2.Rows[i].Cells[0].Paragraphs[0].Append("Модуль продольной упругости при расчетной температуре, ").AppendEquation("E_2").Append(":");
                    table2.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.E2} МПа");
                    i += 1;
                }
            }
            if (dN_in.steel1 != dN_in.steel3)
            {
                table2.InsertRow(i);
                table2.Rows[i].Cells[0].Paragraphs[0].Append($"Допускаемое напряжение для материала {dN_in.steel3} при расчетной температуре, ").AppendEquation("[σ]_3").Append(":");
                table2.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.sigma_d3} МПа");
                i += 1;
                if (d_in.dav == 1)
                {
                    table2.InsertRow(i);
                    table2.Rows[i].Cells[0].Paragraphs[0].Append("Модуль продольной упругости при расчетной температуре, ").AppendEquation("E_3").Append(":");
                    table2.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.E3} МПа");
                    i += 1;
                }
            }
            if (dN_in.steel1 != dN_in.steel4)
            {
                table2.InsertRow(i);
                table2.Rows[i].Cells[0].Paragraphs[0].Append($"Допускаемое напряжение для материала {dN_in.steel4} при расчетной температуре, ").AppendEquation("[σ]_4").Append(":");
                table2.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.sigma_d4} МПа");
                i += 1;
                if (d_in.dav == 1)
                {
                    table2.InsertRow(i);
                    table2.Rows[i].Cells[0].Paragraphs[0].Append("Модуль продольной упругости при расчетной температуре, ").AppendEquation("E_4").Append(":");
                    table2.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.E4} МПа");
                    i += 1;
                }
            }
            doc.InsertParagraph().InsertTableAfterSelf(table2);

            doc.InsertParagraph();
            doc.InsertParagraph("Результаты расчета").Alignment = Alignment.center;
            doc.InsertParagraph();


            doc.InsertParagraph("Расчетные параметры").Alignment = Alignment.center;
            doc.InsertParagraph();


            doc.InsertParagraph("Расчетный диаметр укрепляемого элемента ");

            switch (d_in.met)
            {
                case "obvn":
                case "obnar":
                    doc.Paragraphs.Last().Append("(для цилиндрической обечайки)");
                    doc.InsertParagraph().AppendEquation($"D_p=D={d_in.D} мм");
                    break;
                case "konvn":
                case "konnar":
                    doc.Paragraphs.Last().Append("(для конической обечайки, перехода или днища)");
                    doc.InsertParagraph().AppendEquation("D_p=D_k/cos(α)");
                    //
                    break;
                case "ellvn":
                case "ellnar":
                    if (d_in.elH * 100 == d_in.D * 25)
                    {
                        doc.InsertParagraph("(для эллиптического днища при H=0.25D)");
                        doc.InsertParagraph().AppendEquation("D_p=2∙D∙√(1-3∙(x/D)^2)");
                        doc.InsertParagraph().AppendEquation($"D_p=2∙{d_in.D}∙√(1-3∙({dN_in.elx}/{d_in.D})^2)={dN_out.Dp:f2} мм");
                    }
                    else
                    {
                        doc.InsertParagraph("(для эллиптического днища)");
                        doc.InsertParagraph().AppendEquation('D_p=D^2/(2∙H)∙√(1-(D^2-4∙H^2)/D^4∙x^2)');
                        doc.InsertParagraph().AppendEquation($"D_p={d_in.D}^2/(2∙{d_in.elH})∙√(1-({d_in.D}^2-4∙{d_in.elH}^2)/{d_in.D}^4∙{dN_in.elx}^2)={dN_out.Dp} мм");
                    }
                    break;
                case "sfer":
                case "torosfer":
                    doc.Paragraphs.Last().Append("(для сферических и торосферических днищ вне зоны отбортовки)");
                    doc.InsertParagraph().AppendEquation("D_p=2∙R");
                    break;
            }

            switch (dN_in.place)
            {
                case 1:
                    doc.InsertParagraph("Расчетный диаметр отверстия в стенке цилиндрической обечайки, конического перехода или выпуклого днища при наличии штуцера с круглым поперечным сечением, ось которого совпадает с нормалью к поверхности в центре отверстия");
                    doc.InsertParagraph().AppendEquation("d_p=d+2∙c_s");
                    doc.InsertParagraph().AppendEquation($"d_p={dN_in.D}+2∙{dN_in.cs}={dN_out.dp:f2} мм");
                    break;
                case 2:
                    doc.InsertParagraph("Расчетный диаметр отверстия в стенке цилиндрической обечайки или конической обечайки при наличии наклонного штуцера, ось которого лежит в плоскости поперечного сечения укрепляемой обечайки");
                    doc.InsertParagraph().AppendEquation("d_p=max{d;0.5∙t}+2∙c_s");
                    //doc.InsertParagraph().AppendEquation($"d_p={dN_in.D}+2∙{dN_in.cs}={dN_out.dp:f2} мм");
                    break;
                case 3:
                    doc.InsertParagraph("Расчетный диаметр отверстия в стенке эллиптического днища при наличии смещенного штуцера, ось которого параллельна оси днища");
                    doc.InsertParagraph().AppendEquation("d_p=(d+2∙c_s)/√(1-((2∙x)/D_p)^2)");
                    //doc.InsertParagraph().AppendEquation($"d_p={dN_in.D}+2∙{dN_in.cs}={dN_out.dp:f2} мм");
                    break;
                case 4:
                    doc.InsertParagraph("Расчетный диаметр отверстия при наличии наклонного штуцера с круглым поперечным сечением, когда максимальная ось симметрии отверстия некруглой формы составляет угол ω с образующей цилиндрической обечайки или с проекцией образующей конической обечайки на плоскость продольного сечения обечайки");
                    doc.InsertParagraph().AppendEquation("d_p=(d+2∙c_s)(1+tg^2 γ∙cos^2 ω)");
                    //doc.InsertParagraph().AppendEquation($"d_p={dN_in.D}+2∙{dN_in.cs}={dN_out.dp:f2} мм");
                    break;
                case 5:
                    doc.InsertParagraph("Расчетный диаметр отверстия для цилиндрической и конической обечаек, когда ось наклонного штуцера лежит в плоскости продольного сечения обечайки, а также для всех отверстий в сферическом и торосферическом днищах при наличии смещенного штуцера");
                    doc.InsertParagraph().AppendEquation("d_p=(d+2∙c_s)/(cos^2 γ)");
                    //doc.InsertParagraph().AppendEquation($"d_p={dN_in.D}+2∙{dN_in.cs}={dN_out.dp:f2} мм");
                    break;
                case 6:
                    doc.InsertParagraph("Расчетный диаметр овального отверстия для перпендикулярно расположенного к поверхности обечайки штуцера с овальным поперечным сечением");
                    doc.InsertParagraph().AppendEquation("d_p=(d+2∙c_s)[sin^2 ω +((d_1+2∙c_s)(d_1+d_2+4∙c_s))/(2(d_2+2∙c_s)^2)cos^2 ω");
                    //doc.InsertParagraph().AppendEquation($"d_p={dN_in.D}+2∙{dN_in.cs}={dN_out.dp:f2} мм");
                    break;
                case 7:
                    doc.InsertParagraph("Расчетный диаметр отверстия для перпендикулярно расположенного к поверхности обечайки или днища штуцера с круглым поперечным сечением при наличии отбортовки или торообразной вставки");
                    doc.InsertParagraph().AppendEquation("d_p=в+1.5(r-s_p)+2∙c_s");
                    //doc.InsertParagraph().AppendEquation($"d_p={dN_in.D}+2∙{dN_in.cs}={dN_out.dp:f2} мм");
                    break;
            }


            doc.InsertParagraph("Расчетная длина внешней части штуцера")
            doc.InsertParagraph().AppendEquation("l_1p=min{l_1;1.25√((d+2∙c_s)(s_1-c_s))}");
            doc.InsertParagraph().AppendEquation($"1.25√((d+2∙c_s)(s_1-c_s))=1.25√(({dN_in.D}+2∙{dN_in.cs})({dN_in.s1}-{dN_in.cs}))={dN_out.l1p2:f2} мм");
            doc.InsertParagraph().AppendEquation($"l_1p=min({dN_in.l1};{dN_out.l1p2:f2})={dN_out.l1p:f2} мм");

            if dN_in.l3 > 0:
                doc.InsertParagraph("Расчетная длина внутренней части штуцера")
                doc.InsertParagraph().AppendEquation("l_3p=min{l_3;0.5√((d+2∙c_s)(s_3-c_s-c_s1))}");
                doc.InsertParagraph().AppendEquation($"0.5√((d+2∙c_s)(s_3-c_s-c_s1))=0.5√(({dN_in.D}+2∙{dN_in.cs})({dN_in.s3}-{dN_in.cs}-{dN_in.cs1}))={dN_out.l3p2:f2} мм");
                doc.InsertParagraph().AppendEquation($"l_3p=min({dN_in.l3};{dN_out.l3p2:f2})={dN_out.l3p:f2} мм");

            doc.InsertParagraph("Ширина зоны укрепления отверстия в цилиндрической обечайке")
            doc.InsertParagraph().AppendEquation("L_0=√(D_p∙(s-c))");
            doc.InsertParagraph().AppendEquation($"L_0=√({dN_out.Dp}∙({d_in.s_prin}-{d_out.c:f2}))={dN_out.L0:f2}");

            doc.InsertParagraph("Расчетная ширина зоны укрепления отверстия в стенке цилиндрической обечайки")
            if dN_in.vid in [1, 2, 3, 4, 5, 6]:
                doc.InsertParagraph().AppendEquation($"l_p=L_0={dN_out.lp:f2} мм");
            elif dN_in.vid in [7, 8]:
                doc.InsertParagraph().AppendEquation("l_p=min{l;L_0}");
                doc.InsertParagraph().AppendEquation($"l_p=min({dN_in.l};{dN_out.L0:f2})={dN_out.lp:f2} мм");

            if dN_in.l2 > 0:
                doc.InsertParagraph("Расчетная ширина накладного кольца")
                doc.InsertParagraph().AppendEquation("l_2p=min{l_2;√(D_p∙(s_2+s-c))}");
                doc.InsertParagraph().AppendEquation($"√(D_p∙(s_2+s-c))=√({dN_out.Dp}∙({dN_in.s2}+{d_in.s_prin}-{d_out.c:f2}))={dN_out.l2p2:f2} мм");
                doc.InsertParagraph().AppendEquation($"l_2p=min({dN_in.l2};{dN_out.l2p2:f2})={dN_out.l2p:f2} мм");

            doc.InsertParagraph("Учет применения различного материального исполнения")
            if d_in.steel != dN_in.steel1:
                p = doc.InsertParagraph("- для внешней части штуцера")
                p.AppendEquation($"χ_1=min(1;[σ]_1/[σ])=min(1;{dN_in.sigma_d1}/{d_in.sigma_d})={dN_out.psi1}")
            if d_in.steel != dN_in.steel2:
                p = doc.InsertParagraph("- для накладного кольца")
                p.AppendEquation($"χ_2=min(1;[σ]_2/[σ])=min(1;{dN_in.sigma_d2}/{d_in.sigma_d})={dN_out.psi2}")
            if d_in.steel != dN_in.steel3:
                p = doc.InsertParagraph("- для внутренней части штуцера")
                p.AppendEquation($"χ_3=min(1;[σ]_3/[σ])=min(1;{dN_in.sigma_d3}/{d_in.sigma_d})={dN_out.psi3}")
            if d_in.steel != dN_in.steel4:
                p = doc.InsertParagraph("- для торообразной вставки или вварного кольца")
                p.AppendEquation($"χ_4=min(1;[σ]_4/[σ])=min(1;{dN_in.sigma_d4}/{d_in.sigma_d})={dN_out.psi4}")

            doc.InsertParagraph("Расчетный диаметр отверстия, не требующий укрепления в стенке цилиндрической обечайки при отсутствии избыточной толщины стенки сосуда и при наличии штуцера")
            doc.InsertParagraph().AppendEquation("d_0p=0,4√(D_p∙(s-c))");
            doc.InsertParagraph().AppendEquation($"d_0p=0.4√({dN_out.Dp}∙({d_in.s_prin}-{d_out.c:f2}))={dN_out.d0p:f2} мм");


            doc.InsertParagraph("Проверка условия необходимости проведения расчета укрепления отверстий")
            doc.InsertParagraph().AppendEquation("d_p≤d_0");

            p = doc.InsertParagraph("")
            p.AppendEquation("d_0");
            p.AppendEquation(" - наибольший допустимый диаметр одиночного отверстия, не требующего дополнительного укрепления при наличии избыточной толщины стенки сосуда")
            doc.InsertParagraph().AppendEquation("d_0=min{2∙((s-c)/s_pn-0.8)∙√(D_p∙(s-c));d_max+2∙c_s}");

            p = doc.InsertParagraph("где ")
            p.AppendEquation("d_max");
            p.AppendEquation(" - максимальный диаметр отверстия ")

            p = doc.InsertParagraph("")
            p.AppendEquation($"d_max=D={d_in.D} мм");
            p.AppendEquation(" - для отверстий в цилиндрических обечайках")
    
    
    
            if d_in.dav == "vn":
                p = doc.InsertParagraph("")
                p.AppendEquation($"s_pn=s_p={d_out.s_calcr:f2} мм");
                p.AppendEquation(" - в случае внутреннего давления")
            else:
                doc.InsertParagraph().AppendEquation("s_pn=(p_pn∙D_p)/(2∙K_1∙[σ]-p_pn)");
                doc.InsertParagraph().AppendEquation("p_pn=p/√(1-(p/[p]_E)^2)");
                p = doc.InsertParagraph("")
                p.AppendEquation("[p]_E");
                p.AppendEquation(" -  допускаемое наружное давление из условия устойчивости в пределах упругости, определяемое по ГОСТ 34233.2 для обечайки без отверстий")
                doc.InsertParagraph().AppendEquation($"p_pn={d_in.press}/√(1-({d_in.press}/{dN_out.pen:f2})^2)={dN_out.ppn:f2} МПа");
                doc.InsertParagraph().AppendEquation("s_pn=({dN_out.ppn:f2}∙{dN_out.Dp:f2})/(2∙{dN_out.K1}∙{d_in.sigma_d}-{dN_out.ppn:f2})={dN_out.spn:f2} мм");

            doc.InsertParagraph().AppendEquation($"2∙((s-c)/s_pn-0.8)∙√(D_p∙(s-c))=2∙(({d_in.s_prin}-{d_out.c:f2})/{dN_out.spn:f2}-0.8)∙√({dN_out.Dp}∙({d_in.s_prin}-{d_out.c:f2}))={dN_out.d01:f2}");
            doc.InsertParagraph().AppendEquation($"d_max+2∙c_s={dN_out.dmax:f2}+2∙{dN_in.cs}={dN_out.d02:f2}");
            doc.InsertParagraph().AppendEquation($"d_0=min({dN_out.d01:f2};{dN_out.d02:f2})={dN_out.d0:f2} мм");
            if dN_out.dp <= dN_out.d0:
                doc.InsertParagraph().AppendEquation($"{dN_out.dp:f2}≤{dN_out.d0:f2}");
                p = doc.InsertParagraph()
                p.AppendEquation("Условие прочности выполняется").bold = True
                p.AppendEquation(", следовательно дальнейших расчетов укрепления отверстия не требуется")
            else:
                doc.InsertParagraph().AppendEquation($"{dN_out.dp:f2}≤{dN_out.d0:f2}");
                p = doc.InsertParagraph()
                p.AppendEquation("Условие прочности не выполняется").bold = True
                p.AppendEquation(", следовательно необходим дальнейший расчет укрепления отверстия")
                doc.InsertParagraph("В случае укрепления отверстия утолщением стенки сосуда или штуцера, или накладным кольцом, или вварным кольцом, или торообразной вставкой, или отбортовкой должно выполняться условие")
                doc.InsertParagraph().AppendEquation("l_1p∙(s_1-s_1p-c_s)∙χ_1+l_2p∙s_2∙χ_2+l_3p∙(s_3-c_s-c_s1)∙χ_3+l1p∙(s-s_p-c)∙χ_4≥0.5∙(d_p-d_0p)∙s_p");
                doc.InsertParagraph().AppendEquation("l_1p∙(s_1-s_1p-c_s)∙χ_1+l_2p∙s_2∙χ_2+l_3p∙(s_3-c_s-c_s1)∙χ_3+l1p∙(s-s_p-c)∙χ_4=");
                doc.InsertParagraph().AppendEquation($"{dN_out.l1p:f2}∙({dN_in.s1}-{dN_out.s1p:f2}-{dN_in.cs})∙{dN_out.psi1}+{dN_out.l2p:f2}∙{dN_in.s2}∙{dN_out.psi2}+{dN_out.l3p:f2}∙({dN_in.s3}-{dN_in.cs}-{dN_in.cs1})∙{dN_out.psi3:f2}+{dN_out.lp:f2}∙({d_in.s_prin}-{d_out.s_calcr:f2}-{d_out.c:f2})∙{dN_out.psi4}={dN_out.yslyk1:f2}");
                doc.InsertParagraph().AppendEquation($"0.5∙(d_p-d_0p)∙s_p=0.5∙({dN_out.dp:f2}-{dN_out.d0p:f2})∙{d_out.s_calcr:f2}={dN_out.yslyk2:f2}");
                doc.InsertParagraph().AppendEquation($"{dN_out.yslyk1:f2}≥{dN_out.yslyk2:f2}");
                if dN_out.yslyk1 >= dN_out.yslyk2:
                    doc.InsertParagraph().AppendEquation("Условие прочности выполняется").bold = True
                else:
                    doc.InsertParagraph().AppendEquation("Условие прочности не выполняется").font.color.rgb = docx.shared.RGBColor(255,0,0)


            p = doc.InsertParagraph("")
            doc.InsertParagraph("Допускаемое внутреннее избыточное давление элемента сосуда с учетом ослабления стенки отверстием вычисляют по формуле")
            doc.InsertParagraph().AppendEquation("[p]=(2∙K_1∙φ∙[σ]∙(s-c)∙V)/(D_p+(s-c)∙V)");

            p = doc.InsertParagraph("где ")
            p.AppendEquation("K_1=1");
            p.AppendEquation(" - для цилиндрических и конических обечаек")
            doc.InsertParagraph("Коэффициент снижения прочности сосуда, ослабленного одиночным отверстием, вычисляют по формуле")
            doc.InsertParagraph().AppendEquation("V=min{(s_0-c)/(s-c);(χ_4+(l_1p∙(s_1-c_s)∙χ_1+l_2p∙s_2∙χ_2+l_3p∙(s_3-c_s-c_s1)∙χ_3)/(l_p∙(s-c)))/(1+0.5∙(d_p-d_0p)/l_p+K_1∙(d+2∙c_s)/D_p∙(φ/φ_1)∙(l_1p/l_p))}");
        # doc.add_picture($"pic/Nozzle/V.png", width=docx.shared.Inches(6))
            l = doc.paragraphs[-1]
            l.Alignment = Alignment.center;
            if dN_in.vid in [1, 2, 6]:
                p = doc.InsertParagraph("При отсутствии накладного кольца и укреплении отверстия штуцером ")
                p.AppendEquation("s_2=0 , s_0=s , χ_4=1");
            elif dN_in.vid in [3, 4, 5]:
                p = doc.InsertParagraph("При отсутствии вварного кольца или торообразной вставки ")
                p.AppendEquation("s_0=s , χ_4=1");



            doc.InsertParagraph().AppendEquation($"(s_0-c)/(s-c)=({dN_in.s0}-{d_out.c:f2})/({d_in.s_prin}-{d_out.c:f2})={dN_out.V1:f2}");
            doc.InsertParagraph().AppendEquation("(χ_4+(l_1p∙(s_1-c_s)∙χ_1+l_2p∙s_2∙χ_2+l_3p∙(s_3-c_s-c_s1)∙χ_3)/(l_p∙(s-c)))/(1+0.5∙(d_p-d_0p)/l_p+K_1∙(d+2∙c_s)/D_p∙(φ/φ_1)∙(l_1p/l_p))=");
            doc.InsertParagraph().AppendEquation($"({dN_out.psi4}+({dN_out.l1p:f2}∙({dN_in.s1}-{dN_in.cs})∙{dN_out.psi1}+{dN_out.l2p:f2}∙{dN_in.s2}∙{dN_out.psi2}+{dN_out.l3p:f2}∙({dN_in.s3}-{dN_in.cs}-{dN_in.cs1})∙{dN_out.psi3:f2})/({dN_out.lp:f2}∙({d_in.s_prin}-{d_out.c:f2})))/(1+0.5∙({dN_out.dp:f2}-{dN_out.d0p:f2})/{dN_out.lp:f2}+{dN_out.K1}∙({dN_in.D}+2∙{dN_in.cs})/{dN_out.Dp}∙({dN_in.fi}/{dN_in.fi1})∙({dN_out.l1p:f2}/{dN_out.lp:f2}))={dN_out.V2:f2}");

            doc.InsertParagraph().AppendEquation($"V=min({dN_out.V1:f2};{dN_out.V2:f2})={dN_out.V:f2} мм");

            doc.InsertParagraph().AppendEquation($"[p]=(2∙{dN_out.K1}∙{dN_in.fi}∙{d_in.sigma_d}∙({d_in.s_prin}-{d_out.c:f2})∙{dN_out.V:f2})/({dN_out.Dp}+({d_in.s_prin}-{d_out.c:f2})∙{dN_out.V:f2})={dN_out.press_d:f2} МПа");
            doc.InsertParagraph().AppendEquation("[p]≥p");
            doc.InsertParagraph().AppendEquation($"{dN_out.press_d:f2} МПа >= {d_in.press} МПа");
            if dN_out.press_d >= d_in.press:
                doc.InsertParagraph().AppendEquation("Условие прочности выполняется").bold = True
            else:
                doc.InsertParagraph().AppendEquation("Условие прочности не выполняется").font.color.rgb = docx.shared.RGBColor(255,0,0)
    
            doc.InsertParagraph("Границы применения формул")
            doc.InsertParagraph().AppendEquation("(d_p-2∙c_s)/D≤1");
            doc.InsertParagraph().AppendEquation($"({dN_out.dp:f2}-2∙{dN_in.cs})/{d_in.D}={(dN_out.dp-2*dN_in.cs)/d_in.D:f2}≤1");
            doc.InsertParagraph().AppendEquation("(s-c)/D≤0.1");
            doc.InsertParagraph().AppendEquation($"({d_in.s_prin}-{d_out.c:f2})/({d_in.D})={(d_in.s_prin-d_out.c)/d_in.D:.3f}≤0.1");

            doc.save(docum)
        }


    }
}
