using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.DataIn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Document.NET;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;




namespace CalculateVessels.Core.Shells
{
    public class CylindricalShell : Shell, IElement
    {
        public CylindricalShell(CylindricalShellDataIn cylindricalShellDataIn)
        //: base(ShellType.Cylindrical)
        {
            _csdi = cylindricalShellDataIn;
        }

        private readonly CylindricalShellDataIn _csdi;

        private const string FILENAME_CYLINDR_GIF = "pic/ObCil.gif";

        public void MakeWord(string filename)
        {
            if (filename == null)
            {
                const string DEFAULT_FILE_NAME = "temp.docx";
                filename = DEFAULT_FILE_NAME;
            }

            //using (WordprocessingDocument package = WordprocessingDocument.Open(filename, true))
            //{

            //}
            var doc = Xceed.Words.NET.DocX.Load(filename);

            doc.InsertParagraph().InsertPageBreakAfterSelf();
            doc.InsertParagraph($"Расчет на прочность обечайки {Csdi.Name}, нагруженной " +
                (Csdi.IsPressureIn ? "внутренним избыточным давлением" : "наружным давлением"))
                .Heading(HeadingType.Heading1)
                .Alignment = Alignment.center;
            //if (Csdi.IsPressureIn)
            //{
            //    doc.Paragraphs.Last().Append("внутренним избыточным давлением");
            //}
            //else
            //{
            //    doc.Paragraphs.Last().Append("наружным давлением");
            //}
            doc.InsertParagraph().Alignment = Alignment.center;

            var image = doc.AddImage(FILENAME_CYLINDR_GIF);
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
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{Csdi.Steel}");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Внутренний диаметр обечайки, D:");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{Csdi.D} мм");

                if (!Csdi.IsPressureIn)
                {
                    table.InsertRow(++i);
                    table.Rows[i].Cells[0].Paragraphs[0].Append("Длина обечайки, l:");
                    table.Rows[i].Cells[1].Paragraphs[0].Append($"{Csdi.l} мм");
                }
                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Прибавка на коррозию, ")
                                                    .AppendEquation("c_1")
                                                    .Append(":");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{Csdi.c1} мм");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Прибавка для компенсации минусового допуска, ")
                                                    .AppendEquation("c_2")
                                                    .Append(":");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{Csdi.c2} мм");

                if (Csdi.c3 > 0)
                {
                    table.InsertRow(++i);
                    table.Rows[i].Cells[0].Paragraphs[0].Append("Технологическая прибавка, ")
                                                        .AppendEquation("c_3")
                                                        .Append(":");
                    table.Rows[i].Cells[1].Paragraphs[0].Append($"{Csdi.c3} мм");
                }
                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Коэффициент прочности сварного шва, ")
                                                    .AppendEquation("φ_p")
                                                    .Append(":");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{Csdi.fi}");

                doc.InsertParagraph().InsertTableBeforeSelf(table);
            }

            doc.InsertParagraph();
            doc.InsertParagraph("Условия нагружения").Alignment = Alignment.center;

            //table
            {
                var table = doc.AddTable(1, 2);
                table.SetWidths(new float[] { 300, 100 });
                int i = 0;
                table.Rows[i].Cells[0].Paragraphs[0].Append("Расчетная температура, Т:");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{Csdi.t} °С");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Расчетное "
                    + (Csdi.IsPressureIn ? "внутреннее избыточное" : "наружное")
                    + " давление, p:");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{Csdi.p} МПа");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append($"Допускаемое напряжение для материала {Csdi.Steel} при расчетной температуре, [σ]:");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{Csdi.sigma_d} МПа");
                if (!Csdi.IsPressureIn)
                {
                    table.InsertRow(++i);
                    table.Rows[i].Cells[0].Paragraphs[0].Append("Модуль продольной упругости при расчетной температуре, E:");
                    table.Rows[i].Cells[1].Paragraphs[0].Append($"{Csdi.E} МПа");
                }
                doc.InsertParagraph().InsertTableBeforeSelf(table);
            }

            doc.InsertParagraph("");
            doc.InsertParagraph("Результаты расчета").Alignment = Alignment.center;
            doc.InsertParagraph("");
            doc.InsertParagraph("Толщину стенки вычисляют по формуле:");
            doc.InsertParagraph().AppendEquation("s≥s_p+c");
            doc.InsertParagraph("где ").AppendEquation("s_p").Append(" - расчетная толщина стенки обечайки");
            if (Csdi.IsPressureIn)
            {
                doc.InsertParagraph()
                    .AppendEquation($"s_p=(p∙D)/(2∙[σ]∙φ_p-p)" +
                    $"=({Csdi.p}∙{Csdi.D})/(2∙{Csdi.sigma_d}∙{Csdi.fi}-{Csdi.p})=" +
                                                     $"{_s_calcr:f2} мм");
                //doc.InsertParagraph().AppendEquation($"s_p=({Csdi.p}∙{Csdi.D})/(2∙{Csdi.sigma_d}∙{Csdi.fi}-{Csdi.p})=" +
                //                                  $"{_s_calcr:f2} мм");
            }
            else
            {
                doc.InsertParagraph().AppendEquation("s_p=max{1.06∙(10^-2∙D)/(B)∙(p/(10^-5∙E)∙l/D)^0.4;(1.2∙p∙D)/(2∙[σ]-p)}");
                doc.InsertParagraph("Коэффициент B вычисляют по формуле:");
                doc.InsertParagraph().AppendEquation("B=max{1;0.47∙(p/(10^-5∙E))^0.067∙(l/D)^0.4}");
                doc.InsertParagraph().AppendEquation($"0.47∙({Csdi.p}/(10^-5∙{Csdi.E}))^0.067∙({_l}/{Csdi.D})^0.4={_b_2:f2}");
                doc.InsertParagraph().AppendEquation($"B=max(1;{_b_2:f2})={_b:f2}");
                doc.InsertParagraph().AppendEquation($"1.06∙(10^-2∙{Csdi.D})/({_b:f2})∙({Csdi.p}/(10^-5∙{Csdi.E})∙{_l}/{Csdi.D})^0.4={_s_calcr1:f2}");
                doc.InsertParagraph().AppendEquation($"(1.2∙{Csdi.p}∙{Csdi.D})/(2∙{Csdi.sigma_d}-{Csdi.p})={_s_calcr2:f2}");
                doc.InsertParagraph().AppendEquation($"s_p=max({_s_calcr1:f2};{_s_calcr2:f2})={_s_calcr:f2} мм");
            }

            doc.InsertParagraph("c - сумма прибавок к расчетной толщине");
            doc.InsertParagraph().AppendEquation("c=c_1+c_2+c_3"
                + $"={Csdi.c1}+{Csdi.c2}+{Csdi.c3}={_c:f2} мм");
            //doc.InsertParagraph().AppendEquation($"c={Csdi.c1}+{Csdi.c2}+{Csdi.c3}={_c:f2} мм");

            doc.InsertParagraph().AppendEquation($"s={_s_calcr:f2}+{_c:f2}={_s_calc:f2} мм");
            if (Csdi.s > _s_calc)
            {
                doc.InsertParagraph($"Принятая толщина s={Csdi.s} мм").Bold();
            }
            else
            {
                doc.InsertParagraph($"Принятая толщина s={Csdi.s} мм").Bold().Color(System.Drawing.Color.Red);
            }
            if (Csdi.IsPressureIn)
            {
                doc.InsertParagraph("Допускаемое внутреннее избыточное давление вычисляют по формуле:");
                doc.InsertParagraph().AppendEquation("[p]=(2∙[σ]∙φ_p∙(s-c))/(D+s-c)"
                    + $"=(2∙{Csdi.sigma_d}∙{Csdi.fi}∙({Csdi.s}-{_c:f2}))/"
                    + $"({Csdi.D}+{Csdi.s}-{_c:f2})={_p_d:f2} МПа");
                //doc.InsertParagraph().AppendEquation($"[p]=(2∙{Csdi.sigma_d}∙{Csdi.fi}∙({Csdi.s}-{_c:f2}))/" +
                //                                    $"({Csdi.D}+{Csdi.s}-{_c:f2})={_p_d:f2} МПа");
            }
            else
            {
                doc.InsertParagraph("Допускаемое наружное давление вычисляют по формуле:");
                doc.InsertParagraph().AppendEquation("[p]=[p]_П/√(1+([p]_П/[p]_E)^2)");
                doc.InsertParagraph("допускаемое давление из условия прочности вычисляют по формуле:");
                doc.InsertParagraph().AppendEquation("[p]_П=(2∙[σ]∙(s-c))/(D+s-c)");
                doc.InsertParagraph().AppendEquation($"[p]_П=(2∙{Csdi.sigma_d}∙({Csdi.s}-{_c:f2}))/" +
                                                    $"({Csdi.D}+{Csdi.s}-{_c:f2})={_p_dp:f2} МПа");
                doc.InsertParagraph("допускаемое давление из условия устойчивости в пределах упругости вычисляют по формуле:");
                doc.InsertParagraph().AppendEquation("[p]_E=(2.08∙10^-5∙E)/(n_y∙B_1)∙D/l∙[(100∙(s-c))/D]^2.5");
                doc.InsertParagraph("коэффициент ").AppendEquation("B_1").Append(" вычисляют по формуле");
                doc.InsertParagraph().AppendEquation("B_1=min{1;9.45∙D/l∙√(D/(100∙(s-c)))}");
                doc.InsertParagraph().AppendEquation($"9.45∙{Csdi.D}/{_l}∙√({Csdi.D}/(100∙({Csdi.s}-{_c:f2})))=" +
                                                    $"{_B1_2:f2}");
                doc.InsertParagraph().AppendEquation($"B_1=min(1;{_B1_2:f2})={_B1:f1}");
                doc.InsertParagraph().AppendEquation($"[p]_E=(2.08∙10^-5∙{Csdi.E})/({Csdi.ny}∙{_B1:f2})∙{Csdi.D}/" +
                                                    $"{_l}∙[(100∙({Csdi.s}-{_c:f2}))/{Csdi.D}]^2.5={_p_de:f2} МПа");
                doc.InsertParagraph().AppendEquation($"[p]={_p_dp:f2}/√(1+({_p_dp:f2}/{_p_de:f2})^2)={_p_d:f2} МПа");
            }

            doc.InsertParagraph().AppendEquation("[p]≥p");
            doc.InsertParagraph().AppendEquation($"{_p_d:f2}≥{Csdi.p}");
            if (_p_d > Csdi.p)
            {
                doc.InsertParagraph("Условие прочности выполняется").Bold();
            }
            else
            {
                doc.InsertParagraph("Условие прочности не выполняется").Bold().Color(System.Drawing.Color.Red);
            }
            if (isConditionUseFormuls)
            {
                doc.InsertParagraph("Границы применения формул ");
            }
            else
            {
                doc.InsertParagraph("Границы применения формул. Условие не выполняется ").Bold().Color(System.Drawing.Color.Red);
            }
            const int DIAMETR_BIG_LITTLE_BORDER = 200;
            if (Csdi.D >= DIAMETR_BIG_LITTLE_BORDER)
            {
                doc.Paragraphs.Last().Append("при D ≥ 200 мм");
                doc.InsertParagraph().AppendEquation("(s-c)/(D)"
                    + $"=({ Csdi.s}-{ _c: f2})/({ Csdi.D})={(Csdi.s - _c) / Csdi.D:f3}≤0.1");
                //doc.InsertParagraph().AppendEquation($"({Csdi.s}-{_c:f2})/({Csdi.D})={(Csdi.s - _c) / Csdi.D:f3}≤0.1");
            }
            else
            {
                doc.Paragraphs.Last().Append("при D < 200 мм");
                doc.InsertParagraph().AppendEquation("(s-c)/(D)"
                    + $"({Csdi.s}-{_c:f2})/({Csdi.D})={(Csdi.s - _c) / Csdi.D:f3}≤0.3");
                //    doc.InsertParagraph().AppendEquation($"({Csdi.s}-{_c:f2})/({Csdi.D})={(Csdi.s - _c) / Csdi.D:f3}≤0.3");
            }

            doc.SaveAs(filename);
        }


        public void Calculate()
        {
            _c = _csdi.c1 + _csdi.c2 + _csdi.c3;

            // Condition use formuls
            {
                const int DIAMETR_BIG_LITTLE_BORDER = 200;
                bool isDiametrBig = DIAMETR_BIG_LITTLE_BORDER < _csdi.D;

                //bool isConditionUseFormuls;

                if (isDiametrBig)
                {
                    const double CONDITION_USE_FORMULS_BIG_DIAMETR = 0.1;
                    isConditionUseFormuls = (_csdi.s - _c) / _csdi.D <= CONDITION_USE_FORMULS_BIG_DIAMETR;
                }
                else
                {
                    const double CONDITION_USE_FORMULS_LITTLE_DIAMETR = 0.3;
                    isConditionUseFormuls = (_csdi.s - _c) / _csdi.D <= CONDITION_USE_FORMULS_LITTLE_DIAMETR;
                }

                if (!isConditionUseFormuls)
                {
                    isError = true;
                    err.Add("Условие применения формул не выполняется");
                }
            }

            if (_csdi.p > 0)
            {
                if (_csdi.IsPressureIn)
                {

                    _s_calcr = _csdi.p * _csdi.D / (2 * _csdi.sigma_d * _csdi.fi - _csdi.p);
                    _s_calc = _s_calcr + _c;
                    if (_csdi.s == 0.0)
                    {
                        _p_d = 2 * _csdi.sigma_d * _csdi.fi * (_s_calc - _c) / (_csdi.D + _s_calc - _c);
                    }
                    else if (_csdi.s >= _s_calc)
                    {
                        _p_d = 2 * _csdi.sigma_d * _csdi.fi * (_csdi.s - _c) / (_csdi.D + _csdi.s - _c);
                    }
                    else
                    {
                        isCriticalError = true;
                        err.Add("Принятая толщина меньше расчетной");
                    }
                }
                else
                {
                    _l = _csdi.l + _csdi.l3_1 + _csdi.l3_2;
                    _b_2 = 0.47 * Math.Pow(_csdi.p / (0.00001 * _csdi.E), 0.067) * Math.Pow(_l / _csdi.D, 0.4);
                    _b = Math.Max(1.0, _b_2);
                    _s_calcr1 = 1.06 * (0.01 * _csdi.D / _b) * Math.Pow(_csdi.p / (0.00001 * _csdi.E) * (_l / _csdi.D), 0.4);
                    _s_calcr2 = 1.2 * _csdi.p * _csdi.D / (2 * _csdi.sigma_d - _csdi.p);
                    _s_calcr = Math.Max(_s_calcr1, _s_calcr2);
                    _s_calc = _s_calcr + _c;
                    if (_csdi.s == 0.0)
                    {
                        _p_dp = 2 * _csdi.sigma_d * (_s_calc - _c) / (_csdi.D + _s_calc - _c);
                        _B1_2 = 9.45 * (_csdi.D / _l) * Math.Sqrt(_csdi.D / (100 * (_s_calc - _c)));
                        _B1 = Math.Min(1.0, _B1_2);
                        _p_de = 2.08 * 0.00001 * _csdi.E / (_csdi.ny * _B1) * (_csdi.D / _l) * Math.Pow(100 * (_s_calc - _c) / _csdi.D, 2.5);
                    }
                    else if (_csdi.s >= _s_calc)
                    {
                        _p_dp = 2 * _csdi.sigma_d * (_csdi.s - _c) / (_csdi.D + _csdi.s - _c);
                        _B1_2 = 9.45 * (_csdi.D / _l) * Math.Sqrt(_csdi.D / (100 * (_csdi.s - _c)));
                        _B1 = Math.Min(1.0, _B1_2);
                        _p_de = 2.08 * 0.00001 * _csdi.E / (_csdi.ny * _B1) * (_csdi.D / _l) * Math.Pow(100 * (_csdi.s - _c) / _csdi.D, 2.5);
                    }
                    else
                    {
                        isCriticalError = true;
                        err.Add("Принятая толщина меньше расчетной");
                    }
                    _p_d = _p_dp / Math.Sqrt(1 + Math.Pow(_p_dp / _p_de, 2));
                }
                if (_p_d < _csdi.p && _csdi.s != 0)
                {
                    isError = true;
                    err.Add("[p] меньше p");
                }
            }

            if (_csdi.F > 0)
            {
                _s_calcrf = _csdi.F / (Math.PI * _csdi.D * _csdi.sigma_d * _csdi.fit);
                _s_calcf = _s_calcrf + _c;
                if (_csdi.isFTensile)
                {
                    _F_d = Math.PI * (_csdi.D + _csdi.s - _c) * (_csdi.s - _c) * _csdi.sigma_d * _csdi.fit;
                }
                else
                {
                    _F_dp = Math.PI * (_csdi.D + _csdi.s - _c) * (_csdi.s - _c) * _csdi.sigma_d;
                    _F_de1 = 0.000031 * _csdi.E / _csdi.ny * Math.Pow(_csdi.D, 2) * Math.Pow(100 * (_csdi.s - _c) / _csdi.D, 2.5);

                    const int L_MORE_THEN_D = 10;
                    bool isLMoreThenD = _csdi.l / _csdi.D > L_MORE_THEN_D;

                    if (isLMoreThenD)
                    {
                        switch (_csdi.FCalcSchema)
                        {
                            case 1:
                                _lpr = _csdi.l;
                                break;
                            case 2:
                                _lpr = 2 * _csdi.l;
                                break;
                            case 3:
                                _lpr = 0.7 * _csdi.l;
                                break;
                            case 4:
                                _lpr = 0.5 * _csdi.l;
                                break;
                            case 5:
                                _F = _csdi.q * _csdi.l;
                                _lpr = 1.12 * _csdi.l;
                                break;
                            case 6:
                                double fDivl6 = _csdi.F / _csdi.l;
                                fDivl6 *= 10;
                                fDivl6 = Math.Round(fDivl6 / 2);
                                fDivl6 *= 0.2;
                                switch (fDivl6)
                                {
                                    case 0:
                                        _lpr = 2 * _csdi.l;
                                        break;
                                    case 0.2:
                                        _lpr = 1.73 * _csdi.l;
                                        break;
                                    case 0.4:
                                        _lpr = 1.47 * _csdi.l;
                                        break;
                                    case 0.6:
                                        _lpr = 1.23 * _csdi.l;
                                        break;
                                    case 0.8:
                                        _lpr = 1.06 * _csdi.l;
                                        break;
                                    case 1:
                                        _lpr = _csdi.l;
                                        break;
                                }
                                break;
                            case 7:
                                double fDivl7 = _csdi.F / _csdi.l;
                                fDivl7 *= 10;
                                fDivl7 = Math.Round(fDivl7 / 2);
                                fDivl7 *= 0.2;
                                switch (fDivl7)
                                {
                                    case 0:
                                        _lpr = 2 * _csdi.l;
                                        break;
                                    case 0.2:
                                        _lpr = 1.7 * _csdi.l;
                                        break;
                                    case 0.4:
                                        _lpr = 1.4 * _csdi.l;
                                        break;
                                    case 0.6:
                                        _lpr = 1.11 * _csdi.l;
                                        break;
                                    case 0.8:
                                        _lpr = 0.85 * _csdi.l;
                                        break;
                                    case 1:
                                        _lpr = 0.7 * _csdi.l;
                                        break;
                                }
                                break;

                        }
                        lamda = 2.83 * _lpr / (_csdi.D + _csdi.s - _c);
                        _F_de2 = Math.PI * (_csdi.D + _csdi.s - _c) * (_csdi.s - _c) * _csdi.E / _csdi.ny *
                                        Math.Pow(Math.PI / lamda, 2);
                        _F_de = Math.Min(_F_de1, _F_de2);
                    }
                    else
                    {
                        _F_de = _F_de1;
                    }

                    _F_d = _F_dp / Math.Sqrt(1 + Math.Pow(_F_dp / _F_de, 2));
                }
            }

            if (_csdi.M > 0)
            {
                _M_dp = Math.PI / 4 * _csdi.D * (_csdi.D + _csdi.s - _c) * (_csdi.s - _c) * _csdi.sigma_d;
                _M_de = 0.000089 * _csdi.E / _csdi.ny * Math.Pow(_csdi.D, 3) * Math.Pow(100 * (_csdi.s - _c) / _csdi.D, 2.5);
                _M_d = _M_dp / Math.Sqrt(1 + Math.Pow(_M_dp / _M_de, 2));
            }

            if (_csdi.Q > 0)
            {
                _Q_dp = 0.25 * _csdi.sigma_d * Math.PI * _csdi.D * (_csdi.s - _c);
                _Q_de = 2.4 * _csdi.E * Math.Pow(_csdi.s - _c, 2) / _csdi.ny *
                    (0.18 + 3.3 * _csdi.D * (_csdi.s - _c) / Math.Pow(_csdi.l, 2));
                _Q_d = _Q_dp / Math.Sqrt(1 + Math.Pow(_Q_dp / _Q_de, 2));
            }

            if ((_csdi.IsNeedpCalculate || _csdi.isNeedFCalculate) &&
                (_csdi.isNeedMCalculate || _csdi.isNeedQCalculate))
            {
                conditionYstoich = _csdi.p / _p_d + _csdi.F / _F_d + _csdi.M / _M_d +
                                        Math.Pow(_csdi.Q / _Q_d, 2);
            }
            // TODO: Проверка F для FCalcSchema == 6 F расчитывается и записывается в d_out, а не берется из _csdi
            //return d_out;
        }



        //internal bool IsConditionUseFormuls { get => isConditionUseFormuls; }

        //internal double s_calcr1 { get => _s_calcr1;  }
        //internal double s_calcr2 { get => _s_calcr2;  }
        internal double s_calcrf { get => _s_calcrf; }
        internal double s_calcf { get => _s_calcf; }



        internal double b { get => _b; }
        internal double b_2 { get => _b_2; }
        internal double b1 { get => _B1; }
        internal double b1_2 { get => _B1_2; }
        internal double p_dp { get => _p_dp; }
        //internal double p_de { get => _p_de;  }
        internal double F_d { get => _F_d; }
        internal double F_dp { get => _F_dp; }
        internal double F_de { get => _F_de; }
        internal double F_de1 { get => _F_de1; }
        internal double F_de2 { get => _F_de2; }
        internal double Lamda { get => lamda; }
        internal double M_d { get => _M_d; }
        internal double M_dp { get => _M_dp; }
        internal double M_de { get => _M_de; }
        internal double Q_d { get => _Q_d; }
        internal double Q_dp { get => _Q_dp; }
        internal double Q_de { get => _Q_de; }
        internal double ElR { get => _elR; }
        internal double Elke { get => _elke; }
        internal double Elx { get => _elx; }
        internal double Dk { get => _Dk; }
        internal double Lpr { get => _lpr; }
        internal double F1 { get => _F1; }
        internal double ConditionYstoich { get => conditionYstoich; }
        internal double L { get => _l; }


        internal CylindricalShellDataIn Csdi => _csdi;





        internal int FCalcSchema; //1-7

        internal bool isNeedMakeCalcNozzle,
                    isNeedpCalculate,
                    isPressureIn,
                    isNeedFCalculate,
                    isFTensile,
                    isNeedMCalculate,
                    isNeedQCalculate;


        internal int bibliography;



        private double _l;
        //private double _c;




        private double _s_calcrf;
        private double _s_calcf;

        private double _b;
        private double _b_2;
        private double _B1;
        private double _B1_2;


        private double _F_d;
        private double _F_dp;
        private double _F_de;
        private double _F_de1;
        private double _F_de2;
        private double lamda;
        private double _M_d;
        private double _M_dp;
        private double _M_de;
        private double _Q_d;
        private double _Q_dp;
        private double _Q_de;
        private double _elR;
        private double _elke;
        private double _elx;
        private double _Dk;
        private double _lpr;
        private double _F1;
        private double conditionYstoich;
        private double l_in;
        private double _F;
    }



}
