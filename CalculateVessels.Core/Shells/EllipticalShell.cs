using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.DataIn;
using CalculateVessels.Core.Shells.Enums;
using System;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using CalculateVessels.Core.Word;
using CalculateVessels.Core.Word.Enums;
using System.IO;

namespace CalculateVessels.Core.Shells
{

    public class EllipticalShell : Shell, IElement
    {
        public EllipticalShell(EllipticalShellDataIn ellipticalShellDataIn)
        {
            _esdi = ellipticalShellDataIn;
        }

        private readonly EllipticalShellDataIn _esdi;

        private double _ellR;
        private double _ellKe;
        private double _ellKePrev;
        private double _ellx;

        public double EllR => _ellR;
        //public bool IsCriticalError { get => isCriticalError; }

        public void Calculate()
        {
            //Data_out d_out = new Data_out { err = "" };
            _c = _esdi.c1 + _esdi.c2 + _esdi.c3;

            switch (_esdi.EllipticalBottomType)
            {
                case EllipticalBottomType.Elliptical:
                case EllipticalBottomType.Hemispherical:
                {
                    //Condition use formulas
                    {
                        const double CONDITION_USE_FORMULAS_1_MIN = 0.002,
                            CONDITION_USE_FORMULAS_1_MAX = 0.1,
                            CONDITION_USE_FORMULAS_2_MIN = 0.2,
                            CONDITION_USE_FORMULAS_2_MAX = 0.5;

                        if ((_esdi.s - _c) / _esdi.D <= CONDITION_USE_FORMULAS_1_MAX &
                            (_esdi.s - _c) / _esdi.D >= CONDITION_USE_FORMULAS_1_MIN &
                            _esdi.ellH / _esdi.D < CONDITION_USE_FORMULAS_2_MAX &
                            _esdi.ellH / _esdi.D >= CONDITION_USE_FORMULAS_2_MIN |
                            _esdi.s == 0)
                        {
                            IsConditionUseFormulas = true;
                        }
                        else
                        {
                            IsError = true;
                            IsConditionUseFormulas = false;
                            ErrorList.Add("Условие применения формул не выполняется");
                        }
                    }

                    _ellR = Math.Pow(_esdi.D, 2) / (4.0 * _esdi.ellH);
                    if (_esdi.IsPressureIn)
                    {
                        _s_p = _esdi.p * _ellR / (2.0 * _esdi.sigma_d * _esdi.fi - 0.5 * _esdi.p);
                        _s = _s_p + _c;
                        if (_esdi.s == 0.0)
                        {
                            _p_d = 2.0 * _esdi.sigma_d * _esdi.fi * _s_p /
                                   (_ellR + 0.5 * _s_p);
                        }
                        else if (_esdi.s >= _s)
                        {
                            _p_d = 2.0 * _esdi.sigma_d * _esdi.fi * (_esdi.s - _c) /
                                   (_ellR + 0.5 * (_esdi.s - _c));
                        }
                        else
                        {
                            IsCriticalError = true;
                            ErrorList.Add("Принятая толщина меньше расчетной");
                        }
                    }
                    else
                    {
                        _ellKePrev = _esdi.EllipticalBottomType switch
                        {
                            EllipticalBottomType.Elliptical => 0.9,
                            EllipticalBottomType.Hemispherical => 1.0,
                            _ => _ellKePrev
                        };

                        _s_p_1 = _ellKePrev * _ellR / 161 * Math.Sqrt(_esdi.ny * _esdi.p / (0.00001 * _esdi.E));
                        _s_p_2 = 1.2 * _esdi.p * _ellR / (2.0 * _esdi.sigma_d);

                        _s_p = Math.Max(_s_p_1, _s_p_2);
                        _s = _s_p + _c;
                        if (_esdi.s == 0.0)
                        {
                            _p_dp = 2.0 * _esdi.sigma_d * _s_p / (_ellR + 0.5 * _s_p);
                            _ellx = 10.0 * (_s_p / _esdi.D) *
                                    (_esdi.D / (2.0 * _esdi.ellH) - 2.0 * _esdi.ellH / _esdi.D);
                            _ellKe = (1.0 + (2.4 + 8.0 * _ellx) * _ellx) / (1.0 + (3.0 + 10.0 * _ellx) * _ellx);
                            _p_de = 2.6 * 0.00001 * _esdi.E / _esdi.ny *
                                    Math.Pow(100.0 * _s_p / (_ellKe * _ellR), 2);
                            _p_d = _p_dp / Math.Sqrt(1.0 + Math.Pow(_p_dp / _p_de, 2));

                        }
                        else if (_esdi.s >= _s)
                        {
                            _p_dp = 2.0 * _esdi.sigma_d * (_esdi.s - _c) / (_ellR + 0.5 * (_esdi.s - _c));
                            _ellx = 10.0 * ((_esdi.s - _c) / _esdi.D) *
                                    (_esdi.D / (2.0 * _esdi.ellH) - 2.0 * _esdi.ellH / _esdi.D);
                            _ellKe = (1.0 + (2.4 + 8.0 * _ellx) * _ellx) / (1.0 + (3.0 + 10.0 * _ellx) * _ellx);
                            _p_de = 2.6 * 0.00001 * _esdi.E / _esdi.ny *
                                    Math.Pow(100 * (_esdi.s - _c) / (_ellKe * _ellR), 2);
                            _p_d = _p_dp / Math.Sqrt(1.0 + Math.Pow(_p_dp / _p_de, 2));
                        }
                        else
                        {
                            IsCriticalError = true;
                            ErrorList.Add("Принятая толщина меньше расчетной");
                        }
                    }
                    break;
                }
                case EllipticalBottomType.Torospherical:
                {
                    //TODO: Add calculate torospherical bottom
                    break;
                }

                case EllipticalBottomType.SphericalUnflanged:
                {
                    //TODO Add calculate spherical unflanged bottom
                    break;
                }

                default:
                {
                    IsCriticalError = true;
                    ErrorList.Add("Неверный тип днища");
                    break;
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

            using WordprocessingDocument package = WordprocessingDocument.Open(filename, true);

            var mainPart = package.MainDocumentPart;
            var body = mainPart?.Document.Body;

            if (body == null) return;
            
            // TODO: добавить полусферическое

            //body.AddParagraph("").InsertPageBreakAfterSelf();

            body.AddParagraph($"Расчет на прочность эллиптического днища {_esdi.Name}, нагруженного " +
                (_esdi.IsPressureIn ? "внутренним избыточным давлением" : "наружным давлением"))
                .Heading(HeadingType.Heading1)
                .Alignment(AlignmentType.Center);

            body.AddParagraph("");

            
            var imagePart = mainPart.AddImagePart(ImagePartType.Gif);
            
            byte[] bytes = Data.Properties.Resources.Ell;

            if (bytes != null)
            {
                imagePart.FeedData(new MemoryStream(bytes));

                body.AddParagraph("").AddImage(mainPart.GetIdOfPart(imagePart), bytes);
            }

            body.AddParagraph("Исходные данные")
                .Alignment(AlignmentType.Center);

            //table
            {
                var table = body.AddTable();
                
                table.AddRow()
                    .AddCell("Материал днища")
                    .AddCell($"{_esdi.Steel}");

                table.AddRow()
                    .AddCell("Внутренний диаметр днища, D:")
                    .AddCell($"{_esdi.D} мм");

                table.AddRow()
                    .AddCell("Высота выпуклой части, H:")
                    .AddCell($"{_esdi.ellH} мм");

                table.AddRow()
                    .AddCell("Длина отбортовки ")
                    .AppendEquation("h_1")
                    .AppendText(":")
                    .AddCell($"{_esdi.ellh1}");

                table.AddRow()
                    .AddCell("Прибавка на коррозию, ")
                    .AppendEquation("c_1")
                    .AppendText(":")
                    .AddCell($"{_esdi.c1} мм");

                table.AddRow()
                    .AddCell("Прибавка для компенсации минусового допуска, ")
                    .AppendEquation("c_2")
                    .AppendText(":")
                    .AddCell($"{_esdi.c2} мм");

                if (_esdi.c3 > 0)
                {
                    table.AddRow()
                        .AddCell("Технологическая прибавка, ")
                        .AppendEquation("c_3")
                        .AppendText(":")
                        .AddCell($"{_esdi.c3}");
                }

                table.AddRow()
                    .AddCell("Коэффициент прочности сварного шва, ")
                    .AppendEquation("φ_p")
                    .AppendText(":")
                    .AddCell($"{_esdi.fi}");

                table.AddRowWithOneCell("Условия нагружения");

                table.AddRow()
                    .AddCell("Расчетная температура, Т:")
                    .AddCell($"{_esdi.t} °С");

                table.AddRow()
                    .AddCell(_esdi.IsPressureIn ? "Расчетное внутреннее избыточное давление, p:"
                        : "Расчетное наружное давление, p:")
                    .AddCell($"{_esdi.p} МПа");

                table.AddRowWithOneCell($"Характеристики материала сталь {_esdi.Steel}");

                table.AddRow()
                    .AddCell("Допускаемое напряжение при расчетной температуре, [σ]:")
                    .AddCell($"{_esdi.sigma_d} МПа");

                if (!_esdi.IsPressureIn)
                {
                    table.AddRow()
                        .AddCell("Модуль продольной упругости при расчетной температуре, E:")
                        .AddCell($"{_esdi.E} МПа");
                }

                body.InsertTable(table);
            }


            body.AddParagraph("");
            body.AddParagraph("Результаты расчета").Alignment(AlignmentType.Center);
            body.AddParagraph("");
            body.AddParagraph("Толщину стенки вычисляют по формуле:");
            body.AddParagraph("").AppendEquation("s_1≥s_1p+c");
            body.AddParagraph("где ")
                .AppendEquation("s_1p")
                .AddRun(" - расчетная толщина стенки днища");

            body.AddParagraph("")
                .AppendEquation(_esdi.IsPressureIn
                ? "s_1p=(p∙R)/(2∙[σ]∙φ-0.5∙p)"
                : "s_1p=max{(K_Э∙R)/(161)∙√((n_y∙p)/(10^-5∙E));(1.2∙p∙R)/(2∙[σ])}");
            body.AddParagraph("где R - радиус кривизны в вершине днища");

            // TODO: добавить расчет R для разных ситуаций

            if (_esdi.EllipticalBottomType == EllipticalBottomType.Elliptical &&
                Math.Abs(_esdi.D - _ellR) < 0.00001)
            {
                body.AddParagraph($"R=D={_esdi.D} мм - для эллиптических днищ с H=0.25D");
            }
            else if (_esdi.EllipticalBottomType == EllipticalBottomType.Hemispherical &&
                Math.Abs(0.5 * _esdi.D - _ellR) < 0.00001)
            {
                body.AddParagraph("")
                    .AppendEquation($"R=0.5∙D={_esdi.D} мм")
                    .AddRun(" - для полусферических днищ с H=0.5D");
            }
            else
            {
                body.AddParagraph("")
                    .AppendEquation($"R=D^2/(4∙H)={_esdi.D}^2/(4∙{_esdi.ellH})={_ellR:f2} мм");
            }

            if (_esdi.IsPressureIn)
            {
                body.AddParagraph("")
                    .AppendEquation($"s_p=({_esdi.p}∙{_ellR:f2})/(2∙{_esdi.sigma_d}∙{_esdi.fi}-0.5{_esdi.p})={_s_p:f2} мм");
            }
            else
            {
                body.AddParagraph("Для предварительного расчета ")
                    .AppendEquation($"К_Э={_ellKePrev}")
                    .AddRun(_esdi.EllipticalBottomType == EllipticalBottomType.Elliptical
                        ? " для эллиптических днищ"
                        : " для полусферических днищ");
                body.AddParagraph("")
                    .AppendEquation($"({_ellKePrev}∙{_ellR:f2})/(161)∙√(({_esdi.ny}∙{_esdi.p})/(10^-5∙{_esdi.E}))=" +
                                                    $"{_s_p_1:f2}");
                body.AddParagraph("").AppendEquation($"(1.2∙{_esdi.p}∙{_ellR:f2})/(2∙{_esdi.sigma_d})={_s_p_2:f2}");
                body.AddParagraph("").AppendEquation($"s_1p=max({_s_p_1:f2};{_s_p_2:f2})={_s_p:f2} мм");
            }
            body.AddParagraph("c - сумма прибавок к расчетной толщине");
            body.AddParagraph("")
                .AppendEquation($"c=c_1+c_2+c_3={_esdi.c1}+{_esdi.c2}+{_esdi.c3}={_c:f2} мм");

            body.AddParagraph("").AppendEquation($"s={_s_p:f2}+{_c:f2}={_s:f2} мм");

            if (_esdi.s >= _s)
            {
                body.AddParagraph("Принятая толщина ").Bold().AppendEquation($"s_1={_esdi.s} мм");
            }
            else
            {
                body.AddParagraph($"Принятая толщина ").Bold().Color(System.Drawing.Color.Red)
                    .AppendEquation($"s_1={_esdi.s} мм"); 
            }

            if (_esdi.IsPressureIn)
            {
                body.AddParagraph("Допускаемое внутреннее избыточное давление вычисляют по формуле:");
                body.AddParagraph("")
                    .AppendEquation("[p]=(2∙[σ]∙φ∙(s_1-c))/(R+0.5∙(s-c))" +
                                    $"=(2∙{_esdi.sigma_d}∙{_esdi.fi}∙({_esdi.s}-{_c:f2}))/" +
                                    $"({_ellR:f2}+0.5∙({_esdi.s}-{_c:f2}))={_p_d:f2} МПа");
            }
            else
            {
                body.AddParagraph("Допускаемое наружное давление вычисляют по формуле:");
                body.AddParagraph("")
                    .AppendEquation("[p]=[p]_П/√(1+([p]_П/[p]_E)^2)");
                body.AddParagraph("допускаемое давление из условия прочности вычисляют по формуле:");
                body.AddParagraph("")
                    .AppendEquation("[p]_П=(2∙[σ]∙(s_1-c))/(R+0.5(s_1-c))" +
                                    $"=(2∙{_esdi.sigma_d}∙({_esdi.s}-{_c:f2}))/({_ellR}+0.5({_esdi.s}-{_c:f2}))={_p_dp:f2} МПа");
                body.AddParagraph("допускаемое давление из условия устойчивости в пределах упругости вычисляют по формуле:");
                body.AddParagraph("")
                    .AppendEquation("[p]_E=(2.6∙10^-5∙E)/n_y∙[(100∙(s_1-c))/(К_Э∙R)]^2");
                body.AddParagraph("коэффициент ")
                    .AppendEquation("К_Э")
                    .AddRun(" вычисляют по формуле");
                body.AddParagraph("")
                    .AppendEquation("К_Э=(1+(2.4+8∙x)∙x)/(1+(3+10∙x)∙x)");
                body.AddParagraph("")
                    .AppendEquation($"x=10∙(s_1-c)/D∙(D/(2∙H)-(2∙H)/D)=10∙({_esdi.s-_c:f2})/{_esdi.D}∙({_esdi.D}/(2∙{_esdi.ellH})-(2∙{_esdi.ellH})/{_esdi.D})={_ellx:f2}");
                body.AddParagraph("")
                    .AppendEquation($"К_Э=(1+(2.4+8∙{_ellx})∙{_ellx})/(1+(3+10∙{_ellx})∙{_ellx}={_ellKe:f2}");
                body.AddParagraph("")
                    .AppendEquation($"[p]_E=(2.6∙10^-5∙{_esdi.E})/{_esdi.ny}∙" +
                                    $"[(100∙({_esdi.s}-{_c:f2}))/({_ellKe:f2}∙{_ellR:f2})]^2={_p_de:f2} МПа");
                body.AddParagraph("")
                    .AppendEquation($"[p]={_p_dp:f2}/√(1+({_p_dp:f2}/{_p_de:f2})^2)={_p_d:f2} МПа");
            }
            body.AddParagraph("").AppendEquation("[p]≥p");
            body.AddParagraph("").AppendEquation($"{_p_d:f2}≥{_esdi.p}");

            if (_p_d > _esdi.p)
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

            body.AddParagraph("Условия применения расчетных формул");

            // эллептические днища
            body.AddParagraph("")
                .AppendEquation("0.002≤(s_1-c)/(D)" +
                                $"=({_esdi.s}-{_c:f2})/({_esdi.D})={(_esdi.s - _c) / _esdi.D:f3}≤0.1");
            body.AddParagraph("")
                .AppendEquation($"0.2≤H/D={_esdi.ellH}/{_esdi.D}={_esdi.ellH / _esdi.D:f3}<0.5");

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
