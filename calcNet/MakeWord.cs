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
            var doc = Xceed.Words.NET.DocX.Load("temp.docx");
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
            var doc = Xceed.Words.NET.DocX.Load("temp.docx");
            // добавить полусферическое

            doc.InsertParagraph().InsertPageBreakAfterSelf();
            doc.InsertParagraph($"Расчет на прочность эллиптического днища {d_in.name}, нагруженного ").Alignment = Alignment.center;
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
            table.Rows[i].Cells[0].Paragraphs[0].Append(" 'Коэффициент прочности сварного шва, ").AppendEquation("φ_p");
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


            doc.SaveAs(Docum);
        }
    }
}
