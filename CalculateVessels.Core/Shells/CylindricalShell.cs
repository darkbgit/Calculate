using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.DataIn;
using System;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using CalculateVessels.Core.Word;
using CalculateVessels.Core.Word.Enums;


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

        internal CylindricalShellDataIn Csdi { get => _csdi; }

        private double _l;
        private double _s_pf;
        private double _s_f;
        private double _b;
        private double _b_2;
        private double _B1;
        private double _B1_2;
        private double _F_d;
        private double _F_dp;
        private double _F_de;
        private double _F_de1;
        private double _F_de2;
        private double _lambda;
        private double _M_d;
        private double _M_dp;
        private double _M_de;
        private double _Q_d;
        private double _Q_dp;
        private double _Q_de;
        private double _lpr;
        private double _conditionStability;
        private double _F;

        public void Calculate()
        {
            _c = _csdi.c1 + _csdi.c2 + _csdi.c3;

            // Condition use formulas
            const int DIAMETER_BIG_LITTLE_BORDER = 200;
            const double CONDITION_USE_FORMALS_BIG_DIAMETER = 0.1;
            const double CONDITION_USE_FORMALS_LITTLE_DIAMETER = 0.3;

            IsConditionUseFormulas = DIAMETER_BIG_LITTLE_BORDER < _csdi.D
                ? (_csdi.s - _c) / _csdi.D <= CONDITION_USE_FORMALS_BIG_DIAMETER
                : (_csdi.s - _c) / _csdi.D <= CONDITION_USE_FORMALS_LITTLE_DIAMETER;

            if (!IsConditionUseFormulas)
            {
                IsError = true;
                ErrorList.Add("Условие применения формул не выполняется");
            }

            if (_csdi.p > 0)
            {
                if (_csdi.IsPressureIn)
                {

                    _s_p = _csdi.p * _csdi.D / (2 * _csdi.sigma_d * _csdi.fi - _csdi.p);
                    _s = _s_p + _c;

                    if (_csdi.s != 0.0)
                    {
                        if (_csdi.s >= _s)
                        {
                            _p_d = 2 * _csdi.sigma_d * _csdi.fi * (_csdi.s - _c) / (_csdi.D + _csdi.s - _c);
                        }
                        else
                        {
                            IsCriticalError = true;
                            ErrorList.Add("Принятая толщина меньше расчетной");
                        }
                    }
                }
                else
                {
                    _l = _csdi.l + _csdi.l3;
                    _b_2 = 0.47 * Math.Pow(_csdi.p / (0.00001 * _csdi.E), 0.067) * Math.Pow(_l / _csdi.D, 0.4);
                    _b = Math.Max(1.0, _b_2);
                    _s_p_1 = 1.06 * (0.01 * _csdi.D / _b) * Math.Pow(_csdi.p / (0.00001 * _csdi.E) * (_l / _csdi.D), 0.4);
                    _s_p_2 = 1.2 * _csdi.p * _csdi.D / (2 * _csdi.sigma_d - _csdi.p);
                    _s_p = Math.Max(_s_p_1, _s_p_2);
                    _s = _s_p + _c;

                    if (_csdi.s != 0.0)
                    {
                        if (_csdi.s >= _s)
                        {
                            _p_dp = 2 * _csdi.sigma_d * (_csdi.s - _c) / (_csdi.D + _csdi.s - _c);
                            _B1_2 = 9.45 * (_csdi.D / _l) * Math.Sqrt(_csdi.D / (100 * (_csdi.s - _c)));
                            _B1 = Math.Min(1.0, _B1_2);
                            _p_de = 2.08 * 0.00001 * _csdi.E / (_csdi.ny * _B1) * (_csdi.D / _l) * Math.Pow(100 * (_csdi.s - _c) / _csdi.D, 2.5);
                        }
                        else
                        {
                            IsCriticalError = true;
                            ErrorList.Add("Принятая толщина меньше расчетной");
                        }
                    }
                    _p_d = _p_dp / Math.Sqrt(1 + Math.Pow(_p_dp / _p_de, 2));
                }
                if (_p_d < _csdi.p && _csdi.s != 0)
                {
                    IsError = true;
                    ErrorList.Add("[p] меньше p");
                }
            }

            if (_csdi.F > 0 && _csdi.s != 0.0)
            {
                if (_csdi.IsFTensile)
                {
                    _s_pf = _csdi.F / (Math.PI * _csdi.D * _csdi.sigma_d * _csdi.fi_t);
                    _s_f = _s_pf + _c;

                    if (_csdi.s >= _s_f)
                    {
                        _F_d = Math.PI * (_csdi.D + _csdi.s - _c) * (_csdi.s - _c) * _csdi.sigma_d * _csdi.fi_t;
                    }
                    else
                    {
                        IsCriticalError = true;
                        ErrorList.Add("Принятая толщина меньше расчетной от нагрузки осевым сжимающим усилием");
                    }
                }
                else
                {
                    _F_dp = Math.PI * (_csdi.D + _csdi.s - _c) * (_csdi.s - _c) * _csdi.sigma_d;
                    _F_de1 = 0.000031 * _csdi.E / _csdi.ny * Math.Pow(_csdi.D, 2) * Math.Pow(100 * (_csdi.s - _c) / _csdi.D, 2.5);

                    const int L_MORE_THEN_D = 10;
                    bool isLMoreThenD = _csdi.l / _csdi.D > L_MORE_THEN_D;

                    if (isLMoreThenD || _csdi.ConditionForCalcF5341)
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
                                double fDivl6 = _csdi.f / _csdi.l;
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
                                double fDivl7 = _csdi.f / _csdi.l;
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
                        _lambda = 2.83 * _lpr / (_csdi.D + _csdi.s - _c);
                        _F_de2 = Math.PI * (_csdi.D + _csdi.s - _c) * (_csdi.s - _c) * _csdi.E / _csdi.ny *
                                        Math.Pow(Math.PI / _lambda, 2);
                        _F_de = Math.Min(_F_de1, _F_de2);
                    }
                    else
                    {
                        _F_de = _F_de1;
                    }

                    _F_d = _F_dp / Math.Sqrt(1 + Math.Pow(_F_dp / _F_de, 2));
                }
            }

            if (_csdi.M > 0 && _csdi.s != 0.0)
            {

                _M_dp = Math.PI / 4 * _csdi.D * (_csdi.D + _csdi.s - _c) * (_csdi.s - _c) * _csdi.sigma_d;
                _M_de = 0.000089 * _csdi.E / _csdi.ny * Math.Pow(_csdi.D, 3) * Math.Pow(100 * (_csdi.s - _c) / _csdi.D, 2.5);
                _M_d = _M_dp / Math.Sqrt(1 + Math.Pow(_M_dp / _M_de, 2));

            }

            if (_csdi.Q > 0 && _csdi.s != 0.0)
            {
                _Q_dp = 0.25 * _csdi.sigma_d * Math.PI * _csdi.D * (_csdi.s - _c);
                _Q_de = 2.4 * _csdi.E * Math.Pow(_csdi.s - _c, 2) / _csdi.ny *
                    (0.18 + 3.3 * _csdi.D * (_csdi.s - _c) / Math.Pow(_csdi.l, 2));
                _Q_d = _Q_dp / Math.Sqrt(1 + Math.Pow(_Q_dp / _Q_de, 2));
            }

            _conditionStability = _csdi.p / _p_d + 
                    (_csdi.FCalcSchema == 5 ? _F : _csdi.F) / _F_d + 
                    _csdi.M / _M_d +
                    Math.Pow(_csdi.Q / _Q_d, 2);
            if (_conditionStability > 1)
            {
                IsError = true;
                ErrorList.Add("Условие устойчивости для совместного действия усилий не выполняется");
            }
        }

        public void MakeWord(string filename)
        {

            using WordprocessingDocument package = WordprocessingDocument.Open(filename, true);

            var mainPart = package.MainDocumentPart;
            var body = mainPart?.Document.Body;

            if (body == null) return;

            body.AddParagraph($"Расчет на прочность обечайки {_csdi.Name}, нагруженной " +
                              (_csdi.IsPressureIn ? "внутренним избыточным давлением" : "наружным давлением")).Heading(HeadingType.Heading1);
            body.AddParagraph("");

            var imagePart = mainPart.AddImagePart(ImagePartType.Gif);

            using MemoryStream stream = new(Data.Properties.Resources.Cil);
            imagePart.FeedData(stream);

            body.AddParagraph("").AddImage(mainPart.GetIdOfPart(imagePart));

            body.AddParagraph("Исходные данные").Alignment(AlignmentType.Center);

            //table
            {
                var table = body.AddTable();
                //table.SetWidths(new float[] { 300, 100 });
                //int i = 0;
                table.AddRow()
                    .AddCell("Материал обечайки")
                    .AddCell($"{_csdi.Steel}");

                table.AddRow()
                    .AddCell("Внутренний диаметр обечайки, D:")
                    .AddCell($"{_csdi.D} мм");

                if (!_csdi.IsPressureIn)
                {
                    table.AddRow()
                        .AddCell("Длина обечайки, l:")
                        .AddCell($"{_csdi.l} мм");
                }

                table.AddRow()
                    .AddCell("Прибавка на коррозию, ")
                    .AppendEquation("c_1")
                    .AppendText(":")
                    .AddCell($"{_csdi.c1} мм");


                table.AddRow()
                    .AddCell("Прибавка для компенсации минусового допуска, ")
                    .AppendEquation("c_2")
                    .AppendText(":")
                    .AddCell($"{_csdi.c2} мм");

                if (_csdi.c3 > 0)
                {
                    table.AddRow()
                        .AddCell("Технологическая прибавка, ")
                        .AppendEquation("c_3")
                        .AppendText(":")
                        .AddCell($"{_csdi.c3} мм");
                }
                    
                table.AddRow()
                    .AddCell("Коэффициент прочности сварного шва, ")
                    .AppendEquation("φ_p")
                    .AppendText(":")
                    .AddCell($"{_csdi.fi}");

                body.InsertTable(table);
            }

            body.AddParagraph("");
            body.AddParagraph("Условия нагружения").Alignment(AlignmentType.Center);

            //table
            {
                var table = body.AddTable();

                table.AddRow()
                    .AddCell("Расчетная температура, Т:")
                    .AddCell($"{_csdi.t} °С");

                table.AddRow()
                    .AddCell("Расчетное " + (_csdi.IsPressureIn ? "внутреннее избыточное" : "наружное")
                                          + " давление, p:")
                    .AddCell($"{_csdi.p} МПа");

                table.AddRow()
                    .AddCell($"Допускаемое напряжение для материала {_csdi.Steel} при расчетной температуре, [σ]:")
                    .AddCell($"{_csdi.sigma_d} МПа");

                if (!_csdi.IsPressureIn)
                {
                    table.AddRow()
                        .AddCell("Модуль продольной упругости при расчетной температуре, E:")
                        .AddCell($"{_csdi.E} МПа");
                }
                body.InsertTable(table);
            }

            body.AddParagraph("");
            body.AddParagraph("Результаты расчета").Alignment(AlignmentType.Center);
            body.AddParagraph("");
            body.AddParagraph("Толщину стенки вычисляют по формуле:");
            body.AddParagraph("").AppendEquation("s≥s_p+c");
            body.AddParagraph("где ").AppendEquation("s_p").AddRun(" - расчетная толщина стенки обечайки");

            if (_csdi.IsPressureIn)
            {
                body.AddParagraph("")
                    .AppendEquation("s_p=(p∙D)/(2∙[σ]∙φ_p-p)" +
                                    $"=({_csdi.p}∙{_csdi.D})/(2∙{_csdi.sigma_d}∙{_csdi.fi}-{_csdi.p})=" +
                                    $"{_s_p:f2} мм");
            }
            else
            {
                body.AddParagraph("")
                    .AppendEquation("s_p=max{1.06∙(10^-2∙D)/(B)∙(p/(10^-5∙E)∙l/D)^0.4;(1.2∙p∙D)/(2∙[σ]-p)}");
                body.AddParagraph("Коэффициент B вычисляют по формуле:");
                body.AddParagraph("")
                    .AppendEquation("B=max{1;0.47∙(p/(10^-5∙E))^0.067∙(l/D)^0.4}");
                body.AddParagraph("")
                    .AppendEquation($"0.47∙({_csdi.p}/(10^-5∙{_csdi.E}))^0.067∙({_l}/{_csdi.D})^0.4={_b_2:f2}");
                body.AddParagraph("")
                    .AppendEquation($"B=max(1;{_b_2:f2})={_b:f2}");
                body.AddParagraph("")
                    .AppendEquation($"1.06∙(10^-2∙{_csdi.D})/({_b:f2})∙({_csdi.p}/(10^-5∙{_csdi.E})∙{_l}/{_csdi.D})^0.4={_s_p_1:f2}");
                body.AddParagraph("")
                    .AppendEquation($"(1.2∙{_csdi.p}∙{_csdi.D})/(2∙{_csdi.sigma_d}-{_csdi.p})={_s_p_2:f2}");
                body.AddParagraph("")
                    .AppendEquation($"s_p=max({_s_p_1:f2};{_s_p_2:f2})={_s_p:f2} мм");
            }

            body.AddParagraph("c - сумма прибавок к расчетной толщине");
            body.AddParagraph("")
                .AppendEquation($"c=c_1+c_2+c_3={_csdi.c1}+{_csdi.c2}+{_csdi.c3}={_c:f2} мм");

            body.AddParagraph("").AppendEquation($"s={_s_p:f2}+{_c:f2}={_s:f2} мм");

            if (_csdi.s > _s)
            {
                body.AddParagraph($"Принятая толщина s={_csdi.s} мм").Bold();
            }
            else
            {
                body.AddParagraph($"Принятая толщина s={_csdi.s} мм")
                    .Bold()
                    .Color(System.Drawing.Color.Red);
            }
            if (_csdi.IsPressureIn)
            {
                body.AddParagraph("Допускаемое внутреннее избыточное давление вычисляют по формуле:");
                body.AddParagraph("")
                    .AppendEquation("[p]=(2∙[σ]∙φ_p∙(s-c))/(D+s-c)"
                                    + $"=(2∙{_csdi.sigma_d}∙{_csdi.fi}∙({_csdi.s}-{_c:f2}))/"
                                    + $"({_csdi.D}+{_csdi.s}-{_c:f2})={_p_d:f2} МПа");
            }
            else
            {
                body.AddParagraph("Допускаемое наружное давление вычисляют по формуле:");
                body.AddParagraph("")
                    .AppendEquation("[p]=[p]_П/√(1+([p]_П/[p]_E)^2)");
                body.AddParagraph("допускаемое давление из условия прочности вычисляют по формуле:");
                body.AddParagraph("").AppendEquation("[p]_П=(2∙[σ]∙(s-c))/(D+s-c)" + 
                                                     $"=(2∙{_csdi.sigma_d}∙({_csdi.s}-{_c:f2}))/({_csdi.D}+{_csdi.s}-{_c:f2})={_p_dp:f2} МПа");
                body.AddParagraph("допускаемое давление из условия устойчивости в пределах упругости вычисляют по формуле:");
                body.AddParagraph("")
                    .AppendEquation("[p]_E=(2.08∙10^-5∙E)/(n_y∙B_1)∙D/l∙[(100∙(s-c))/D]^2.5");
                body.AddParagraph("коэффициент ")
                    .AppendEquation("B_1")
                    .AddRun(" вычисляют по формуле");
                body.AddParagraph("")
                    .AppendEquation("B_1=min{1;9.45∙D/l∙√(D/(100∙(s-c)))}");
                body.AddParagraph("")
                    .AppendEquation($"9.45∙{_csdi.D}/{_l}∙√({_csdi.D}/(100∙({_csdi.s}-{_c:f2})))={_B1_2:f2}");
                body.AddParagraph("")
                    .AppendEquation($"B_1=min(1;{_B1_2:f2})={_B1:f1}");
                body.AddParagraph("")
                    .AppendEquation($"[p]_E=(2.08∙10^-5∙{_csdi.E})/({_csdi.ny}∙{_B1:f2})∙{_csdi.D}/" +
                                    $"{_l}∙[(100∙({_csdi.s}-{_c:f2}))/{_csdi.D}]^2.5={_p_de:f2} МПа");
                body.AddParagraph("")
                    .AppendEquation($"[p]={_p_dp:f2}/√(1+({_p_dp:f2}/{_p_de:f2})^2)={_p_d:f2} МПа");
            }

            body.AddParagraph("").AppendEquation("[p]≥p");
            body.AddParagraph("")
                .AppendEquation($"{_p_d:f2}≥{_csdi.p}");
            if (_p_d > _csdi.p)
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

            const int DIAMETER_BIG_LITTLE_BORDER = 200;
            body.AddParagraph("Условия применения расчетных формул ")
                .AddRun(_csdi.D >= DIAMETER_BIG_LITTLE_BORDER ?
                    "при D ≥ 200 мм" : "при D < 200 мм");


            body.AddParagraph("")
                .AppendEquation(_csdi.D >= DIAMETER_BIG_LITTLE_BORDER ?
                    $"(s-c)/(D)=({_csdi.s}-{_c:f2})/({_csdi.D})={(_csdi.s - _c) / _csdi.D:f3}≤0.1" :
                    $"(s-c)/(D)=({_csdi.s}-{_c:f2})/({_csdi.D})={(_csdi.s - _c) / _csdi.D:f3}≤0.3");

            if (!IsConditionUseFormulas)
            {
                body.AddParagraph("Условия применения расчетных формул не выполняется ")
                    .Bold()
                    .Color(System.Drawing.Color.Red);
            }

            package.Close();
        }
    }
}
