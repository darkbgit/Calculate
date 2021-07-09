using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateVessels.Core.Trash
{
    class Class1
    {
        //public void MakeWord(string filename)
        //{
        //    if (filename == null)
        //    {
        //        const string DEFAULT_FILE_NAME = "temp.docx";
        //        filename = DEFAULT_FILE_NAME;
        //    }

        //    //using (WordprocessingDocument package = WordprocessingDocument.Open(filename, true))
        //    //{

        //    //}
        //    var doc = Xceed.Words.NET.DocX.Load(filename);

        //    doc.InsertParagraph().InsertPageBreakAfterSelf();
        //    doc.InsertParagraph($"Расчет на прочность обечайки {Csdi.Name}, нагруженной " +
        //        (Csdi.IsPressureIn ? "внутренним избыточным давлением" : "наружным давлением"))
        //        .Heading(HeadingType.Heading1)
        //        .Alignment = Alignment.center;
        //    //if (Csdi.IsPressureIn)
        //    //{
        //    //    doc.Paragraphs.Last().Append("внутренним избыточным давлением");
        //    //}
        //    //else
        //    //{
        //    //    doc.Paragraphs.Last().Append("наружным давлением");
        //    //}
        //    doc.InsertParagraph().Alignment = Alignment.center;

        //    var image = doc.AddImage(FILENAME_CYLINDER_GIF);
        //    var picture = image.CreatePicture();
        //    doc.InsertParagraph().AppendPicture(picture);
        //    doc.InsertParagraph("Исходные данные").Alignment = Alignment.center;

        //    //table
        //    {
        //        var table = doc.AddTable(1, 2);
        //        table.SetWidths(new float[] { 300, 100 });
        //        int i = 0;
        //        //table.InsertRow(i);
        //        table.Rows[i].Cells[0].Paragraphs[0].Append("Материал обечайки");
        //        table.Rows[i].Cells[1].Paragraphs[0].Append($"{Csdi.Steel}");

        //        table.InsertRow(++i);
        //        table.Rows[i].Cells[0].Paragraphs[0].Append("Внутренний диаметр обечайки, D:");
        //        table.Rows[i].Cells[1].Paragraphs[0].Append($"{Csdi.D} мм");

        //        if (!Csdi.IsPressureIn)
        //        {
        //            table.InsertRow(++i);
        //            table.Rows[i].Cells[0].Paragraphs[0].Append("Длина обечайки, l:");
        //            table.Rows[i].Cells[1].Paragraphs[0].Append($"{Csdi.l} мм");
        //        }
        //        table.InsertRow(++i);
        //        table.Rows[i].Cells[0].Paragraphs[0].Append("Прибавка на коррозию, ")
        //                                            .AppendEquation("c_1")
        //                                            .Append(":");
        //        table.Rows[i].Cells[1].Paragraphs[0].Append($"{Csdi.c1} мм");

        //        table.InsertRow(++i);
        //        table.Rows[i].Cells[0].Paragraphs[0].Append("Прибавка для компенсации минусового допуска, ")
        //                                            .AppendEquation("c_2")
        //                                            .Append(":");
        //        table.Rows[i].Cells[1].Paragraphs[0].Append($"{Csdi.c2} мм");

        //        if (Csdi.c3 > 0)
        //        {
        //            table.InsertRow(++i);
        //            table.Rows[i].Cells[0].Paragraphs[0].Append("Технологическая прибавка, ")
        //                                                .AppendEquation("c_3")
        //                                                .Append(":");
        //            table.Rows[i].Cells[1].Paragraphs[0].Append($"{Csdi.c3} мм");
        //        }
        //        table.InsertRow(++i);
        //        table.Rows[i].Cells[0].Paragraphs[0].Append("Коэффициент прочности сварного шва, ")
        //                                            .AppendEquation("φ_p")
        //                                            .Append(":");
        //        table.Rows[i].Cells[1].Paragraphs[0].Append($"{Csdi.fi}");

        //        doc.InsertParagraph().InsertTableBeforeSelf(table);
        //    }

        //    doc.InsertParagraph();
        //    doc.InsertParagraph("Условия нагружения").Alignment = Alignment.center;

        //    //table
        //    {
        //        var table = doc.AddTable(1, 2);
        //        table.SetWidths(new float[] { 300, 100 });
        //        int i = 0;
        //        table.Rows[i].Cells[0].Paragraphs[0].Append("Расчетная температура, Т:");
        //        table.Rows[i].Cells[1].Paragraphs[0].Append($"{Csdi.t} °С");

        //        table.InsertRow(++i);
        //        table.Rows[i].Cells[0].Paragraphs[0].Append("Расчетное "
        //            + (Csdi.IsPressureIn ? "внутреннее избыточное" : "наружное")
        //            + " давление, p:");
        //        table.Rows[i].Cells[1].Paragraphs[0].Append($"{Csdi.p} МПа");

        //        table.InsertRow(++i);
        //        table.Rows[i].Cells[0].Paragraphs[0].Append($"Допускаемое напряжение для материала {Csdi.Steel} при расчетной температуре, [σ]:");
        //        table.Rows[i].Cells[1].Paragraphs[0].Append($"{Csdi.sigma_d} МПа");
        //        if (!Csdi.IsPressureIn)
        //        {
        //            table.InsertRow(++i);
        //            table.Rows[i].Cells[0].Paragraphs[0].Append("Модуль продольной упругости при расчетной температуре, E:");
        //            table.Rows[i].Cells[1].Paragraphs[0].Append($"{Csdi.E} МПа");
        //        }
        //        doc.InsertParagraph().InsertTableBeforeSelf(table);
        //    }

        //    doc.InsertParagraph("");
        //    doc.InsertParagraph("Результаты расчета").Alignment = Alignment.center;
        //    doc.InsertParagraph("");
        //    doc.InsertParagraph("Толщину стенки вычисляют по формуле:");
        //    doc.InsertParagraph().AppendEquation("s≥s_p+c");
        //    doc.InsertParagraph("где ").AppendEquation("s_p").Append(" - расчетная толщина стенки обечайки");
        //    if (Csdi.IsPressureIn)
        //    {
        //        doc.InsertParagraph()
        //            .AppendEquation($"s_p=(p∙D)/(2∙[σ]∙φ_p-p)" +
        //            $"=({Csdi.p}∙{Csdi.D})/(2∙{Csdi.sigma_d}∙{Csdi.fi}-{Csdi.p})=" +
        //                                             $"{_s_calcr:f2} мм");
        //        //doc.InsertParagraph().AppendEquation($"s_p=({Csdi.p}∙{Csdi.D})/(2∙{Csdi.sigma_d}∙{Csdi.fi}-{Csdi.p})=" +
        //        //                                  $"{_s_calcr:f2} мм");
        //    }
        //    else
        //    {
        //        doc.InsertParagraph().AppendEquation("s_p=max{1.06∙(10^-2∙D)/(B)∙(p/(10^-5∙E)∙l/D)^0.4;(1.2∙p∙D)/(2∙[σ]-p)}");
        //        doc.InsertParagraph("Коэффициент B вычисляют по формуле:");
        //        doc.InsertParagraph().AppendEquation("B=max{1;0.47∙(p/(10^-5∙E))^0.067∙(l/D)^0.4}");
        //        doc.InsertParagraph().AppendEquation($"0.47∙({Csdi.p}/(10^-5∙{Csdi.E}))^0.067∙({_l}/{Csdi.D})^0.4={_b_2:f2}");
        //        doc.InsertParagraph().AppendEquation($"B=max(1;{_b_2:f2})={_b:f2}");
        //        doc.InsertParagraph().AppendEquation($"1.06∙(10^-2∙{Csdi.D})/({_b:f2})∙({Csdi.p}/(10^-5∙{Csdi.E})∙{_l}/{Csdi.D})^0.4={_s_calcr1:f2}");
        //        doc.InsertParagraph().AppendEquation($"(1.2∙{Csdi.p}∙{Csdi.D})/(2∙{Csdi.sigma_d}-{Csdi.p})={_s_calcr2:f2}");
        //        doc.InsertParagraph().AppendEquation($"s_p=max({_s_calcr1:f2};{_s_calcr2:f2})={_s_calcr:f2} мм");
        //    }

        //    doc.InsertParagraph("c - сумма прибавок к расчетной толщине");
        //    doc.InsertParagraph().AppendEquation("c=c_1+c_2+c_3"
        //        + $"={Csdi.c1}+{Csdi.c2}+{Csdi.c3}={_c:f2} мм");
        //    //doc.InsertParagraph().AppendEquation($"c={Csdi.c1}+{Csdi.c2}+{Csdi.c3}={_c:f2} мм");

        //    doc.InsertParagraph().AppendEquation($"s={_s_calcr:f2}+{_c:f2}={_s_calc:f2} мм");
        //    if (Csdi.s > _s_calc)
        //    {
        //        doc.InsertParagraph($"Принятая толщина s={Csdi.s} мм").Bold();
        //    }
        //    else
        //    {
        //        doc.InsertParagraph($"Принятая толщина s={Csdi.s} мм").Bold().Color(System.Drawing.Color.Red);
        //    }
        //    if (Csdi.IsPressureIn)
        //    {
        //        doc.InsertParagraph("Допускаемое внутреннее избыточное давление вычисляют по формуле:");
        //        doc.InsertParagraph().AppendEquation("[p]=(2∙[σ]∙φ_p∙(s-c))/(D+s-c)"
        //            + $"=(2∙{Csdi.sigma_d}∙{Csdi.fi}∙({Csdi.s}-{_c:f2}))/"
        //            + $"({Csdi.D}+{Csdi.s}-{_c:f2})={_p_d:f2} МПа");
        //        //doc.InsertParagraph().AppendEquation($"[p]=(2∙{Csdi.sigma_d}∙{Csdi.fi}∙({Csdi.s}-{_c:f2}))/" +
        //        //                                    $"({Csdi.D}+{Csdi.s}-{_c:f2})={_p_d:f2} МПа");
        //    }
        //    else
        //    {
        //        doc.InsertParagraph("Допускаемое наружное давление вычисляют по формуле:");
        //        doc.InsertParagraph().AppendEquation("[p]=[p]_П/√(1+([p]_П/[p]_E)^2)");
        //        doc.InsertParagraph("допускаемое давление из условия прочности вычисляют по формуле:");
        //        doc.InsertParagraph().AppendEquation("[p]_П=(2∙[σ]∙(s-c))/(D+s-c)");
        //        doc.InsertParagraph().AppendEquation($"[p]_П=(2∙{Csdi.sigma_d}∙({Csdi.s}-{_c:f2}))/" +
        //                                            $"({Csdi.D}+{Csdi.s}-{_c:f2})={_p_dp:f2} МПа");
        //        doc.InsertParagraph("допускаемое давление из условия устойчивости в пределах упругости вычисляют по формуле:");
        //        doc.InsertParagraph().AppendEquation("[p]_E=(2.08∙10^-5∙E)/(n_y∙B_1)∙D/l∙[(100∙(s-c))/D]^2.5");
        //        doc.InsertParagraph("коэффициент ").AppendEquation("B_1").Append(" вычисляют по формуле");
        //        doc.InsertParagraph().AppendEquation("B_1=min{1;9.45∙D/l∙√(D/(100∙(s-c)))}");
        //        doc.InsertParagraph().AppendEquation($"9.45∙{Csdi.D}/{_l}∙√({Csdi.D}/(100∙({Csdi.s}-{_c:f2})))=" +
        //                                            $"{_B1_2:f2}");
        //        doc.InsertParagraph().AppendEquation($"B_1=min(1;{_B1_2:f2})={_B1:f1}");
        //        doc.InsertParagraph().AppendEquation($"[p]_E=(2.08∙10^-5∙{Csdi.E})/({Csdi.ny}∙{_B1:f2})∙{Csdi.D}/" +
        //                                            $"{_l}∙[(100∙({Csdi.s}-{_c:f2}))/{Csdi.D}]^2.5={_p_de:f2} МПа");
        //        doc.InsertParagraph().AppendEquation($"[p]={_p_dp:f2}/√(1+({_p_dp:f2}/{_p_de:f2})^2)={_p_d:f2} МПа");
        //    }

        //    doc.InsertParagraph().AppendEquation("[p]≥p");
        //    doc.InsertParagraph().AppendEquation($"{_p_d:f2}≥{Csdi.p}");
        //    if (_p_d > Csdi.p)
        //    {
        //        doc.InsertParagraph("Условие прочности выполняется").Bold();
        //    }
        //    else
        //    {
        //        doc.InsertParagraph("Условие прочности не выполняется").Bold().Color(System.Drawing.Color.Red);
        //    }
        //    if (isConditionUseFormuls)
        //    {
        //        doc.InsertParagraph("Границы применения формул ");
        //    }
        //    else
        //    {
        //        doc.InsertParagraph("Границы применения формул. Условие не выполняется ").Bold().Color(System.Drawing.Color.Red);
        //    }
        //    const int DIAMETR_BIG_LITTLE_BORDER = 200;
        //    if (Csdi.D >= DIAMETR_BIG_LITTLE_BORDER)
        //    {
        //        doc.Paragraphs.Last().Append("при D ≥ 200 мм");
        //        doc.InsertParagraph().AppendEquation("(s-c)/(D)"
        //            + $"=({ Csdi.s}-{ _c: f2})/({ Csdi.D})={(Csdi.s - _c) / Csdi.D:f3}≤0.1");
        //        //doc.InsertParagraph().AppendEquation($"({Csdi.s}-{_c:f2})/({Csdi.D})={(Csdi.s - _c) / Csdi.D:f3}≤0.1");
        //    }
        //    else
        //    {
        //        doc.Paragraphs.Last().Append("при D < 200 мм");
        //        doc.InsertParagraph().AppendEquation("(s-c)/(D)"
        //            + $"({Csdi.s}-{_c:f2})/({Csdi.D})={(Csdi.s - _c) / Csdi.D:f3}≤0.3");
        //        //    doc.InsertParagraph().AppendEquation($"({Csdi.s}-{_c:f2})/({Csdi.D})={(Csdi.s - _c) / Csdi.D:f3}≤0.3");
        //    }

        //    doc.SaveAs(filename);
        //}



        /// <summary>
        /// Get [σ] 
        /// </summary>
        /// <param name="steel">Steel name</param>
        /// <param name="temp">Calculation temperature</param>
        /// <param name="sigma_d">Reference on </param>
        /// <returns>true - Ok, false - Error (Input temperature bigger then the biggest temperature for [σ] in GOST for input steel) </returns>
        //internal static bool GetSigma(string steel, double temp, ref double sigma_d, ref List<string> dataInErr)
        //{
        //    XmlDocument doc = new XmlDocument();
        //    doc.Load(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @"data\data.xml"));
        //    var root = doc.DocumentElement;
        //    XmlNode steelTempNodes = root.SelectSingleNode("sigma_list")
        //                                 .SelectSingleNode("steels")
        //                                 .SelectSingleNode("//steel[@name='" + steel + "']");

        //    double sigma, sigmaLittle = 0, sigmaBig = 0;
        //    int tempLittle = 0, tempBig = 0;

        //    for (int i = 0; i < steelTempNodes.ChildNodes.Count; i++)
        //    {
        //        if ((i == 0 && Convert.ToInt32(steelTempNodes.ChildNodes.Item(i).Attributes["temp"].Value) > temp) ||
        //                Convert.ToInt32(steelTempNodes.ChildNodes.Item(i).Attributes["temp"].Value) == temp)
        //        {
        //            sigma_d = Convert.ToDouble(steelTempNodes.ChildNodes.Item(i).Attributes["sigma"].Value,
        //                                    System.Globalization.CultureInfo.InvariantCulture);
        //            return true;
        //        }
        //        else if (Convert.ToInt32(steelTempNodes.ChildNodes.Item(i).Attributes["temp"].Value) > temp)
        //        {
        //            tempLittle = Convert.ToInt32(steelTempNodes.ChildNodes.Item(i).Attributes["temp"].Value);
        //            sigmaBig = Convert.ToDouble(steelTempNodes.ChildNodes.Item(i).Attributes["sigma"].Value,
        //                                        System.Globalization.CultureInfo.InvariantCulture);
        //            break;
        //        }
        //        else if (i == steelTempNodes.ChildNodes.Count - 1)
        //        {
        //            dataInErr.Add($"Температура {temp} °С, больше чем максимальная температура {tempBig} °С " +
        //                          $"для стали {steel} при которой определяется допускаемое напряжение по ГОСТ 34233.1-2017");
        //            return false;
        //        }
        //        else
        //        {
        //            tempBig = Convert.ToInt32(steelTempNodes.ChildNodes.Item(i).Attributes["temp"].Value);
        //            sigmaLittle = double.Parse(steelTempNodes.ChildNodes.Item(i).Attributes["sigma"].Value,
        //                                        System.Globalization.CultureInfo.InvariantCulture);
        //        }
        //    }
        //    sigma = sigmaBig - ((sigmaBig - sigmaLittle) * (temp - tempLittle) / (tempBig - tempLittle));
        //    sigma *= 10;
        //    sigma = Math.Truncate(sigma / 5);
        //    sigma *= 0.5;
        //    sigma_d = sigma;
        //    return true;
        //}

        //internal static bool GetE(string steel, double temp, ref double E, ref List<string> dataInErr)
        //{
        //    Regex regex = new Regex(@"(.*)(?=\()");
        //    MatchCollection matches = regex.Matches(steel);

        //    string steelName;

        //    if (matches.Count > 0)
        //    {
        //        steelName = matches[0].Groups[0].Value;
        //    }
        //    else
        //    {
        //        steelName = steel;
        //    }

        //    XmlDocument doc = new XmlDocument();
        //    doc.Load(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @"data\data.xml"));
        //    var root = doc.DocumentElement;
        //    XmlNode classListNode = root.SelectSingleNode("//n[@name='" + steelName + "']");
        //    var steelClass = classListNode.ParentNode.Name;
        //    XmlNode Elist = root.SelectSingleNode("E_list").SelectSingleNode("//class[@name='" + steelClass + "']");

        //    double ELittle = 0, EBig = 0;
        //    double tempLittle = 0, tempBig = 0;

        //    for (int i = 0; i < Elist.ChildNodes.Count; i++)
        //    {
        //        if ((i == 0 && Convert.ToInt32(Elist.ChildNodes.Item(i).Attributes["temp"].Value) > temp) ||
        //            Convert.ToInt32(Elist.ChildNodes.Item(i).Attributes["temp"].Value) == temp)
        //        {
        //            E = Convert.ToDouble(Elist.ChildNodes.Item(i).Attributes["E"].Value);
        //            return true;
        //        }
        //        if (Convert.ToInt32(Elist.ChildNodes.Item(i).Attributes["temp"].Value) > temp)
        //        {
        //            tempLittle = Convert.ToInt32(Elist.ChildNodes.Item(i).Attributes["temp"].Value);
        //            EBig = Convert.ToInt32(Elist.ChildNodes.Item(i).Attributes["E"].Value);
        //            break;
        //        }
        //        if (i == Elist.ChildNodes.Count - 1)//temperature greater then max in data.xml
        //        {
        //            dataInErr.Add($"Температура {temp} °С, больше чем максимальная температура {tempBig} °С " +
        //                          $"для стали {steel} при которой определяется модуль  продольной упругости по ГОСТ 34233.1-2017");
        //            return false;
        //        }

        //        tempBig = Convert.ToInt32(Elist.ChildNodes.Item(i).Attributes["temp"].Value);
        //        ELittle = Convert.ToInt32(Elist.ChildNodes.Item(i).Attributes["E"].Value);


        //    }


        //    E = Convert.ToDouble(EBig - ((EBig - ELittle) * (temp - tempLittle) / (tempBig - tempLittle)));

        //    return true;
        //}

    }
}
