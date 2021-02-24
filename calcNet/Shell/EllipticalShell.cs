using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Document.NET;

namespace calcNet
{
    enum EllipticalBottomType
    {
        Elliptical,
        Hemispherical
    }

    class EllipticalShell : Shell, IElement
    {
        public EllipticalShell(EllipticalShellDataIn ellipticalShellDataIn)
        {
            _esdi = ellipticalShellDataIn;
        }

        private readonly EllipticalShellDataIn _esdi;

        private double _ellR;
        private double _ellKe;
        private double _ellx;

        //public bool IsCriticalError { get => isCriticalError; }

        public void Calculate()
        {
            //Data_out d_out = new Data_out { err = "" };
            _c = _esdi.c1 + _esdi.c2 + _esdi.c3;

            //Condition use formuls
            {
                const double CONDITION_USE_FORMULS_1_MIN = 0.002,
                            CONDITION_USE_FORMULS_1_MAX = 0.1,
                            CONDITION_USE_FORMULS_2_MIN = 0.2,
                            CONDITION_USE_FORMULS_2_MAX = 0.5;

                if ((((_esdi.s - _c) / _esdi.D <= CONDITION_USE_FORMULS_1_MAX) &
                    ((_esdi.s - _c) / _esdi.D >= CONDITION_USE_FORMULS_1_MIN) &
                    (_esdi.ellH / _esdi.D < CONDITION_USE_FORMULS_2_MAX) &
                    (_esdi.ellH / _esdi.D >= CONDITION_USE_FORMULS_2_MIN)) |
                    _esdi.s == 0)
                {
                    isConditionUseFormuls = true;
                }
                else
                {
                    isError = true;
                    isConditionUseFormuls = false;
                    err.Add("Условие применения формул не выполняется");
                }
            }
            _ellR = Math.Pow(_esdi.D, 2) / (4 * _esdi.ellH);
            if (_esdi.IsPressureIn)
            {
                _s_calcr = _esdi.p * _ellR / ((2 * _esdi.sigma_d * _esdi.fi) - 0.5 * _esdi.p);
                _s_calc = _s_calcr + _c;

                if (_esdi.s == 0.0)
                {
                    _p_d = 2 * _esdi.sigma_d * _esdi.fi * (_s_calc - _c) / (_ellR + 0.5 * (_s_calc - _c));
                }
                else if (_esdi.s >= _s_calc)
                {
                    _p_d = 2 * _esdi.sigma_d * _esdi.fi * (_esdi.s - _c) / (_ellR + 0.5 * (_s_calc - _c));
                }
                else
                {
                    isCriticalError = true;
                    err.Add("Принятая толщина меньше расчетной");
                }
            }
            else
            {
                _s_calcr2 = 1.2 * _esdi.p * _ellR / (2 * _esdi.sigma_d);

                switch (_esdi.EllipticalBottomType)
                {
                    case EllipticalBottomType.Elliptical:
                        _ellKe = 0.9;
                        break;
                    case EllipticalBottomType.Hemispherical:
                        _ellKe = 1;
                        break;
                }
                _s_calcr1 = _ellKe * _ellR / 161 * Math.Sqrt(_esdi.ny * _esdi.p / (0.00001 * _esdi.E));
                _s_calcr = Math.Max(_s_calcr1, _s_calcr2);
                _s_calc = _s_calcr + _c;
                if (_esdi.s == 0.0)
                {
                    //_elke = 0.9; // # добавить ке для полусферических =1
                    _s_calcr1 = (_ellKe * _ellR) / 161 * Math.Sqrt((_esdi.ny * _esdi.p) / (0.00001 * _esdi.E));
                    _s_calcr = Math.Max(_s_calcr1, _s_calcr2);
                    //#_p_dp = 2*_esdi.sigma_d*(_s_calc-_c)/(_elR + 0.5 * (_s_calc-_c))
                    //#_elx = 10 * ((_esdi.s-_c)/_esdi.D)*(_esdi.D/(2*_esdi.elH)-(2*_esdi.elH)/_esdi.D)
                    //_elke = (1 + (2.4 + 8 * _elx)*_elx)/(1+(3.0+10*_elx)*_elx)
                    //#_p_de = (2.6*0.00001*_esdi.E)/_esdi.ny*Math.Pow(100*(_s-_c)/(_elke*_elR,2))
                    //#_p_d = _p_dp/Math.Sqrt(1+Math.Pow(_p_dp/_p_de,2))
                }
                else if (_esdi.s >= _s_calc)
                {
                    _p_dp = 2 * _esdi.sigma_d * (_esdi.s - _c) / (_ellR + 0.5 * (_esdi.s - _c));
                    _ellx = 10 * ((_esdi.s - _c) / _esdi.D) * (_esdi.D / (2 * _esdi.ellH) - (2 * _esdi.ellH) / _esdi.D);
                    _ellKe = (1 + (2.4 + 8 * _ellx) * _ellx) / (1 + (3.0 + 10 * _ellx) * _ellx);
                    _p_de = (2.6 * 0.00001 * _esdi.E) / _esdi.ny * Math.Pow(100 * (_esdi.s - _c) / (_ellKe * _ellR), 2);
                    _p_d = _p_dp / Math.Sqrt(1 + Math.Pow(_p_dp / _p_de, 2));
                }
                else
                {
                    isCriticalError = true;
                    err.Add("Принятая толщина меньше расчетной");
                }
            }

        }

        public void MakeWord(string filename)
        {
            if (filename == null)
            {
                const string DEFAULT_FILE_NAME = "temp.docx";
                filename = DEFAULT_FILE_NAME;
            }
            var doc = Xceed.Words.NET.DocX.Load(filename);
            // TODO: добавить полусферическое

            doc.InsertParagraph().InsertPageBreakAfterSelf();
            doc.InsertParagraph($"Расчет на прочность эллиптического днища {_esdi.Name}, нагруженного ")
                .Heading(HeadingType.Heading1)
                .Alignment = Alignment.center;

            doc.Paragraphs.Last().Append(_esdi.IsPressureIn ? "внутренним избыточным давлением"
                    : "наружным давлением");

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
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{_esdi.Steel}");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Внутренний диаметр днища, D:");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{_esdi.D} мм");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Высота выпуклой части, H:");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{_esdi.ellH} мм");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Длина отбортовки ").AppendEquation("h_1").Append(":");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{_esdi.ellh1}");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Прибавка на коррозию, ").AppendEquation("c_1").Append(":");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{_esdi.c1} мм");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Прибавка для компенсации минусового допуска, ")
                                                    .AppendEquation("c_2")
                                                    .Append(":");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{_esdi.c2} мм");

                if (_esdi.c3 > 0)
                {
                    table.InsertRow(++i);
                    table.Rows[i].Cells[0].Paragraphs[0].Append("Технологическая прибавка, ").AppendEquation("c_3").Append(":");
                    table.Rows[i].Cells[1].Paragraphs[0].Append($"{_esdi.c3}");
                }
                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Коэффициент прочности сварного шва, ").AppendEquation("φ_p");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{_esdi.fi}");
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
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{_esdi.t} °С");

                table.InsertRow(++i);

                table.Rows[i].Cells[0].Paragraphs[0].Append(_esdi.IsPressureIn ? "Расчетное внутреннее избыточное давление, p:" 
                    : "Расчетное наружное давление, p:");

                table.Rows[i].Cells[1].Paragraphs[0].Append($"{_esdi.p} МПа");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append($"Допускаемое напряжение для материала {_esdi.Steel} " +
                                                            "при расчетной температуре, [σ]:");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{_esdi.sigma_d} МПа");

                if (!_esdi.IsPressureIn)
                {
                    table.InsertRow(++i);
                    table.Rows[i].Cells[0].Paragraphs[0].Append("Модуль продольной упругости при расчетной температуре, E:");
                    table.Rows[i].Cells[1].Paragraphs[0].Append($"{_esdi.E} МПа");
                }

                doc.InsertParagraph().InsertTableAfterSelf(table);
            }

            doc.InsertParagraph();
            doc.InsertParagraph("Результаты расчета").Alignment = Alignment.center;
            doc.InsertParagraph();
            doc.InsertParagraph("Толщину стенки вычисляют по формуле:");
            doc.InsertParagraph().AppendEquation("s_1≥s_1p+c");
            doc.InsertParagraph("где ").AppendEquation("s_1p").Append(" - расчетная толщина стенки днища");
            if (_esdi.IsPressureIn)
            {
                doc.InsertParagraph().AppendEquation("s_1p=(p∙R)/(2∙[σ]∙φ-0.5∙p)");
            }
            else
            {
                doc.InsertParagraph().AppendEquation("s_1p=max{(K_Э∙R)/(161)∙√((n_y∙p)/(10^-5∙E));(1.2∙p∙R)/(2∙[σ])}");
            }
            doc.InsertParagraph("где R - радиус кривизны в вершине днища");
            // добавить расчет R для разных ситуаций
            if (_esdi.D == _ellR)
            {
                doc.InsertParagraph($"R=D={_esdi.D} мм - для эллиптичекских днищ с H=0.25D");
            }
            else
            {
                doc.InsertParagraph().AppendEquation("R=D^2/(4∙H)");
                doc.InsertParagraph().AppendEquation($"R={_esdi.D}^2/(4∙{_esdi.ellH})={_ellR} мм");
            }
            if (_esdi.IsPressureIn)
            {
                doc.InsertParagraph().AppendEquation($"s_p=({_esdi.p}∙{_ellR})/(2∙{_esdi.sigma_d}∙{_esdi.fi}-0.5{_esdi.p})={_s_calcr:f2} мм");
            }
            else
            {
                doc.InsertParagraph("Для предварительного расчета ").AppendEquation("К_Э=0.9").Append(" для эллиптических днищ");
                doc.InsertParagraph().AppendEquation($"(0.9∙{_ellR})/(161)∙√(({_esdi.ny}∙{_esdi.p})/(10^-5∙{_esdi.E}))=" +
                                                    "{_s_calcr1:f2}");
                doc.InsertParagraph().AppendEquation($"(1.2∙{_esdi.p}∙{_ellR})/(2∙{_esdi.sigma_d})={_s_calcr2:f2}");
                doc.InsertParagraph().AppendEquation($"s_1p=max({_s_calcr1:f2};{_s_calcr2:f2})={_s_calcr:f2} мм");
            }
            doc.InsertParagraph("c - сумма прибавок к расчетной толщине");
            doc.InsertParagraph().AppendEquation("c=c_1+c_2+c_3");
            doc.InsertParagraph().AppendEquation($"c={_esdi.c1}+{_esdi.c2}+{_esdi.c3}={_c:f2} мм");

            doc.InsertParagraph().AppendEquation($"s={_s_calcr:f2}+{_c:f2}={_s_calc:f2} мм");

            if (_esdi.s >= _s_calc)
            {
                doc.InsertParagraph("Принятая толщина ").Bold().AppendEquation($"s_1={_esdi.s} мм");
            }
            else
            {
                doc.InsertParagraph($"Принятая толщина s={_esdi.s} мм").Bold().Color(System.Drawing.Color.Red);
            }
            doc.InsertParagraph("Допускаемое внутреннее избыточное давление вычисляют по формуле:");
            doc.InsertParagraph().AppendEquation("[p]=(2∙[σ]∙φ∙(s_1-c))/(R+0.5∙(s-c))");
            doc.InsertParagraph().AppendEquation($"[p]=(2∙{_esdi.sigma_d}∙{_esdi.fi}∙({_esdi.s}-{_c:f2}))/" +
                                                "({_elR}+0.5∙({_esdi.s}-{_c:f2}))={_p_d:f2} МПа");
            doc.InsertParagraph().AppendEquation("[p]≥p");
            doc.InsertParagraph().AppendEquation($"{_p_d:f2}≥{_esdi.p}");
            if (_p_d > _esdi.p)
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
                doc.InsertParagraph().AppendEquation("Границы применения формул. Условие не выполняется").Bold().Color(System.Drawing.Color.Red);
            }
            //# эллептические днища
            doc.InsertParagraph().AppendEquation("0.002≤(s_1-c)/(D)≤0.1");
            doc.InsertParagraph().AppendEquation($"0.002≤({_esdi.s}-{_c:f2})/({_esdi.D})={(_esdi.s - _c) / _esdi.D:f3}≤0.1");
            doc.InsertParagraph().AppendEquation("0.2≤H/D≤0.5");
            doc.InsertParagraph().AppendEquation($"0.2≤{_esdi.ellH}/{_esdi.D}={_esdi.ellH / _esdi.D:f3}<0.5");


            doc.Save();
        }
    }
}
