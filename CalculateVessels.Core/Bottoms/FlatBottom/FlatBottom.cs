using CalculateVessels.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalculateVessels.Core.Bottoms.Enums;
using CalculateVessels.Data.PhysicalData;
using DocumentFormat.OpenXml.Packaging;
using CalculateVessels.Core.Word;
using CalculateVessels.Core.Word.Enums;
using System.IO;

namespace CalculateVessels.Core.Bottoms.FlatBottom
{
    public class FlatBottom : IElement
    {
        private readonly FlatBottomDataIn _fbdi;

        public FlatBottom(FlatBottomDataIn fbdi)
        {
            _fbdi = fbdi;
        }

        public bool IsCriticalError { get; private set; }

        public bool IsError { get; private set; }

        public IEnumerable<string> ErrorList => _errorList;

        public IEnumerable<string> Bibliography { get; } = new List<string>()
        {
            Data.Properties.Resources.GOST_34233_1,
            Data.Properties.Resources.GOST_34233_2
        };

        public double PressurePermissible => _p_d;
        public double S1Calculated => _s1;

        private double _c;
        private double _K;
        private double _K_1;
        private double _K0;
        private double _Kp;
        private double _Dp;
        private double _s1p;
        private double _s1;
        private double _s2;
        private double _s2p_1;
        private double _s2p_2;
        private double _s2p;
        private double _conditionUseFormulas;
        private double _p_d;
        private double _sigmaAllow;

        private List<string> _errorList = new();
        private bool _isConditionUseFormulas;
        private bool _isConditionFixed = true;

        public override string ToString() => $"Плоское донышко {_fbdi.Name}";

        public void Calculate()
        {
            //[]p
            if (_fbdi.sigma_d > 0)
            {
                _sigmaAllow = _fbdi.sigma_d;
            }
            else
            {
                if (!Physical.Gost34233_1.TryGetSigma(_fbdi.Steel, _fbdi.t, ref _sigmaAllow, ref _errorList))
                {
                    IsCriticalError = true;
                    return;
                }
            }

            _c = _fbdi.c1 + _fbdi.c2 + _fbdi.c3;

            switch (_fbdi.Type)
            {
                case 1:
                    _K = 0.53;
                    _Dp = _fbdi.D;
                    if (_fbdi.a < 1.7 * _fbdi.s)
                    {
                        _isConditionFixed = false;
                        IsError = true;
                        _errorList.Add("Условие закрепления не выполняется a>=1.7s");
                    }
                    break;
                case 2:
                    _K = 0.50;
                    _Dp = _fbdi.D;
                    if (_fbdi.a < 0.85 * _fbdi.s)
                    {
                        _isConditionFixed = false;
                        IsError = true;
                        _errorList.Add("Условие закрепления не выполняется a>=0.85s");
                    }
                    break;
                case 3:
                    _Dp = _fbdi.D;
                    _K = (_fbdi.s - _c) / (_fbdi.s1 - _c) < 0.25 ? 0.45 : 0.41;
                    break;
                case 4:
                    _Dp = _fbdi.D;
                    _K = (_fbdi.s - _c) / (_fbdi.s1 - _c) < 0.5 ? 0.41 : 0.38;
                    break;
                case 5:
                    goto case 3;
                case 6:
                    goto case 2;
                case 7:
                case 8:
                    goto case 4;
                case 9:
                    _Dp = _fbdi.D - 2 * _fbdi.r;
                    if (_fbdi.h1 < _fbdi.r ||
                        _fbdi.r < Math.Max(_fbdi.s, 025 * _fbdi.s1) ||
                        _fbdi.r > Math.Min(_fbdi.s1, 0.1 * _fbdi.D))
                    {
                        _isConditionFixed = false;
                        IsError = true;
                        _errorList.Add("Условие закрепления не выполняется");
                    }
                    _K_1 = 0.41 * (1.0 - 0.23 * ((_fbdi.s - _c) / (_fbdi.s1 - _c)));
                    _K = Math.Max(_K_1, 0.35);
                    break;
                case 10:
                    if (_fbdi.gamma < 30 || _fbdi.gamma > 90 ||
                        _fbdi.r < 0.25 * _fbdi.s1 || _fbdi.r > (_fbdi.s1 - _fbdi.s2))
                    {
                        _isConditionFixed = false;
                        IsError = true;
                        _errorList.Add("Условие закрепления не выполняется");
                    }

                    _s2p_1 = 1.1 * (_fbdi.s - _c);
                    _s2p_2 = (_fbdi.s1 - _c) /
                            (1 + (_Dp - 2 * _fbdi.r) / (1.2 * (_fbdi.s1 - _c) * Math.Sin(_fbdi.gamma * Math.PI / 180)));
                    _s2 = Math.Max(_s2p_1, _s2p_2) + _c;
                    if (_fbdi.s2 < _s2)
                    {
                        IsError = true;
                        _errorList.Add("Принятая толщина s2 меньше расчетной");
                    }
                    goto case 4;
                case 11:
                case 12:
                    if (_fbdi.Type == 11)
                    {
                        _K = 0.4;
                        _Dp = _fbdi.D3;
                    }
                    else
                    {
                        _K = 0.41;
                        _Dp = _fbdi.Dcp;
                    }
                    _s2p_1 = 0.7 * (_fbdi.s1 - _c);
                    _s2p_2 = (_fbdi.s1 - _c) * Math.Sqrt(2 * (_Dp - _fbdi.D2) * _fbdi.D2 / Math.Pow(_fbdi.D2, 2));
                    _s2p = Math.Max(_s2p_1, _s2p_2);
                    _s2 = _s2p + _c;
                    if (_fbdi.s2 < _s2)
                    {
                        IsError = true;
                        _errorList.Add("Принятая толщина s2 меньше расчетной");
                    }
                    break;
            }

            switch (_fbdi.Hole)
            {
                case HoleInFlatBottom.WithoutHole:
                    _K0 = 1;
                    break;
                case HoleInFlatBottom.OneHole:
                    _K0 = Math.Sqrt(1.0 + _fbdi.d / _Dp + Math.Pow(_fbdi.d / _Dp, 2));
                    break;
                case HoleInFlatBottom.MoreThenOneHole:
                    if (_fbdi.di > 0.7 * _Dp)
                    {
                        IsError = true;
                        _errorList.Add("Слишком много отверстий");
                    }
                    _K0 = Math.Sqrt((1 - Math.Pow(_fbdi.di / _Dp, 3)) / (1 - _fbdi.di / _Dp));
                    break;
                default:
                    IsError = true;
                    _errorList.Add("Ошибка определения колличества отверстий");
                    break;
            }

            _s1p = _K * _K0 * _Dp * Math.Sqrt(_fbdi.p / (_fbdi.fi * _sigmaAllow));
            _s1 = _s1p + _c;

            if (_fbdi.s1 != 0.0)
            {
                if (_fbdi.s1 >= _s1p)
                {
                    _conditionUseFormulas = (_fbdi.s1 - _c) / _Dp;
                    _isConditionUseFormulas = _conditionUseFormulas <= 0.11;
                    _Kp = _isConditionUseFormulas
                        ? 1
                        : 2.2 / (1 + Math.Sqrt(1 + Math.Pow(6 * (_fbdi.s1 - _c) / _Dp, 2)));

                    _p_d = Math.Pow((_fbdi.s1 - _c) / (_K * _K0 * _Dp), 2) * _sigmaAllow * _fbdi.fi;
                    if (_Kp * _p_d < _fbdi.p)
                    {
                        IsError = true;
                        _errorList.Add("Допускаемое давление меньше расчетного");
                    }
                }
                else
                {
                    IsCriticalError = true;
                    _errorList.Add("Принятая толщина s1 меньше расчетной");
                    return;
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

            body.AddParagraph($"Расчет на прочность плоской круглой крышки {_fbdi.Name}")
                .Heading(HeadingType.Heading1)
                .Alignment(AlignmentType.Center);

            body.AddParagraph("");

            {
                var imagePart = mainPart.AddImagePart(ImagePartType.Gif);

                byte[] bytes = (byte[])Data.Properties.Resources.ResourceManager.GetObject("pldn" + _fbdi.Type);

                imagePart.FeedData(new MemoryStream(bytes));

                body.AddParagraph("").AddImage(mainPart.GetIdOfPart(imagePart), bytes);
            }


            body.AddParagraph("Исходные данные").Alignment(AlignmentType.Center);

            //table
            {
                var table = body.AddTable();

                table.AddRow()
                    .AddCell("Марка стали")
                    .AddCell($"{_fbdi.Steel}");

                table.AddRow()
                    .AddCell("Коэффициент прочности сварного шва, φ:")
                    .AddCell($"{_fbdi.fi}");

                table.AddRow()
                    .AddCell("Прибавка на коррозию, ")
                    .AppendEquation("c_1")
                    .AppendText(":")
                    .AddCell($"{_fbdi.c1} мм");

                table.AddRow()
                    .AddCell("Прибавка для компенсации минусового допуска, ")
                    .AppendEquation("c_2")
                    .AppendText(":")
                    .AddCell($"{_fbdi.c2} мм");

                if (_fbdi.c3 > 0)
                {
                    table.AddRow()
                        .AddCell("Технологическая прибавка, ")
                        .AppendEquation("c_3")
                        .AppendText(":")
                        .AddCell($"{_fbdi.c3} мм");
                }

                table.AddRow()
                    .AddCell("Тип конструкции по ГОСТ 34233.2-2017 табл.4:")
                    .AddCell($"{_fbdi.Type}");

                switch (_fbdi.Type)
                {
                    case 1:
                    case 2:
                        table.AddRow()
                            .AddCell("Внутренний диаметр аппарата, D:")
                            .AddCell($"{_fbdi.D} мм");

                        table.AddRow()
                            .AddCell("Толщина стенки аппарата, s:")
                            .AddCell($"{_fbdi.s} мм");

                        table.AddRow()
                            .AddCell("Катет сварного шва, a:")
                            .AddCell($"{_fbdi.a} мм");

                        break;
                    case 3:
                    case 4:
                    case 5:
                        table.AddRow()
                            .AddCell("Внутренний диаметр аппарата, D:")
                            .AddCell($"{_fbdi.D} мм");

                        table.AddRow()
                            .AddCell("Толщина стенки аппарата, s:")
                            .AddCell($"{_fbdi.s} мм");

                        break;
                    case 6:
                        goto case 2;
                    case 7:
                    case 8:
                        goto case 5;
                    case 9:
                        table.AddRow()
                            .AddCell("Внутренний диаметр аппарата, D:")
                            .AddCell($"{_fbdi.D} мм");

                        table.AddRow()
                            .AddCell("Толщина стенки аппарата, s:")
                            .AddCell($"{_fbdi.s} мм");

                        table.AddRow()
                            .AddCell("Радиус отбортовки, r:")
                            .AddCell($"{_fbdi.r} мм");

                        table.AddRow()
                            .AddCell("Высота отбортовки, ")
                            .AppendEquation("h_1")
                            .AppendText(":")
                            .AddCell($"{_fbdi.h1} мм");

                        break;
                    case 10:
                        table.AddRow()
                            .AddCell("Внутренний диаметр аппарата, D:")
                            .AddCell($"{_fbdi.D} мм");

                        table.AddRow()
                            .AddCell("Толщина стенки аппарата, s:")
                            .AddCell($"{_fbdi.s} мм");

                        table.AddRow()
                            .AddCell("Радиус выточки, r:")
                            .AddCell($"{_fbdi.r} мм");

                        table.AddRow()
                            .AddCell("Высота отбортовки, ")
                            .AppendEquation("γ")
                            .AppendText(":")
                            .AddCell($"{_fbdi.gamma} °");

                        table.AddRow()
                            .AddCell("Толщина крышки в зоне проточки, ")
                            .AppendEquation("s_2")
                            .AppendText(":")
                            .AddCell($"{_fbdi.s2} мм");

                        break;

                    case 11:
                        table.AddRow()
                            .AddCell("Наименьший диаметр наружной утоненной части плоской крышки, ")
                            .AppendEquation("D_2")
                            .AppendText(":")
                            .AddCell($"{_fbdi.D2} мм");

                        table.AddRow()
                            .AddCell("Диаметр болтовой окружности, ")
                            .AppendEquation("D_3")
                            .AppendText(":")
                            .AddCell($"{_fbdi.D3} мм");

                        table.AddRow()
                            .AddCell("Толщина крышки в зоне уплотнения, ")
                            .AppendEquation("s_2")
                            .AppendText(":")
                            .AddCell($"{_fbdi.s2} мм");

                        break;

                    case 12:
                        table.AddRow()
                            .AddCell("Наименьший диаметр наружной утоненной части плоской крышки, ")
                            .AppendEquation("D_2")
                            .AppendText(":")
                            .AddCell($"{_fbdi.D2} мм");

                        table.AddRow()
                            .AddCell("Расчетный диаметр прокладки, ")
                            .AppendEquation("D_c.п")
                            .AppendText(":")
                            .AddCell($"{_fbdi.Dcp} мм");

                        table.AddRow()
                            .AddCell("Толщина крышки в зоне уплотнения, ")
                            .AppendEquation("s_2")
                            .AppendText(":")
                            .AddCell($"{_fbdi.s2} мм");

                        break;
                    case 13:
                    case 14:
                    case 15:
                        //TODO Make or delete flat bottom type
                        break;
                }

                table.AddRowWithOneCell("Условия нагружения");


                table.AddRow()
                    .AddCell("Расчетная температура, Т:")
                    .AddCell($"{_fbdi.t} °С");

                table.AddRow()
                    .AddCell("Расчетное давление, p:")
                    .AddCell($"{_fbdi.p} МПа");


                table.AddRowWithOneCell($"Характеристики материалов");

                table.AddRow()
                    .AddCell("Допускаемое напряжение при расчетной температуре, [σ]:")
                    .AddCell($"{_sigmaAllow} МПа");

                body.InsertTable(table);
            }


            body.AddParagraph("");
            body.AddParagraph("Результаты расчета").Alignment(AlignmentType.Center);
            body.AddParagraph("");

            body.AddParagraph("Толщину плоской круглой крышки аппарата, работающего под внутренним избыточным давлением вычисляют по формуле");
            body.AddParagraph("").AppendEquation("s_1≥s_1p+c");
            body.AddParagraph("где ");
            body.AddParagraph("").AppendEquation("s_1p=K∙K_0∙D_p∙√(p/(φ∙[σ]))");

            body.AddParagraph("Коэффициент К в зависимости от конструкции днищ и крышек определяют по таблице 4 ГОСТ 34233.2-2017.");

            switch (_fbdi.Type)
            {
                case 1:
                case 2:
                    body.AddParagraph("")
                        .AppendEquation($"D_p=D={_Dp} мм, K={_K}");
                    break;
                case 3:
                    body.AddParagraph("")
                        .AppendEquation($"D_p=D={_Dp} мм");
                    body.AddParagraph("")
                        .AppendEquation($"при (s-c)/(s_1-c)=({_fbdi.s}-{_c})/({_fbdi.s1}-{_c})={(_fbdi.s-_c)/(_fbdi.s1-_c):f2}" +
                        ((_fbdi.s - _c) / (_fbdi.s1 - _c) < 0.25 ? "<0.25" : "≥0.25") + $"  K={_K}");
                    break;
                case 4:
                    body.AddParagraph("")
                        .AppendEquation($"D_p=D={_Dp} мм");
                    body.AddParagraph("")
                        .AppendEquation($"при (s-c)/(s_1-c)=({_fbdi.s}-{_c})/({_fbdi.s1}-{_c})={(_fbdi.s - _c) / (_fbdi.s1 - _c):f2}" +
                        ((_fbdi.s - _c) / (_fbdi.s1 - _c) < 0.5 ? "<0.5" : "≥0.5") + $"  K={_K}");
                    break;
                case 5:
                    goto case 3;
                case 6:
                    goto case 2;
                case 7:
                case 8:
                    goto case 4;
                case 9:
                    body.AddParagraph("")
                        .AppendEquation($"D_p=D-2∙r={_Dp} мм");
                    body.AddParagraph("")
                        .AppendEquation($"K=max[0.41∙(1-0.23∙(s-c)/(s_1-c));0.35]={_K}");
                    break;
                case 10:
                    goto case 4;
                case 11:
                    body.AddParagraph("")
                        .AppendEquation($"D_p=D_3={_Dp} мм, K={_K}");
                    break;
                case 12:
                    body.AddParagraph("")
                        .AppendEquation($"D_p=D_c.п={_Dp} мм, K={_K}");
                    break;
            }


            switch (_fbdi.Hole)
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
                    body.AddParagraph("")
                        .AppendEquation("K_0=√(1+d/D_p+(d/D_p)^2)" +
                        $"=√(1+{_fbdi.d}/{_Dp:f2}+({_fbdi.d}/{_Dp:f2})^2)={_K0:f2}");
                    break;
                case HoleInFlatBottom.MoreThenOneHole:
                    body.AddParagraph("Коэффициент ")
                       .AppendEquation("K_0")
                       .AddRun(" - для крышек, имеющих несколько отверстий, вычисляют по формул");
                    body.AddParagraph("")
                        .AppendEquation("K_0=√((1-(Σd_i/D_p)^3)/(1-(Σd_i/D_p)))" +
                        $"=√((1-({_fbdi.di}/{_Dp:f2})^3)/(1-({_fbdi.di}/{_Dp:f2})))={_K0:f2}");
                    break;
            }


            body.AddParagraph("").AppendEquation($"s_1p={_K:f2}∙{_K0:f2}∙{_Dp:f2}∙√({_fbdi.p}/({_fbdi.fi}∙{_sigmaAllow}))={_s1p:f2} мм");

            body.AddParagraph("c - сумма прибавок к расчетной толщине");
            body.AddParagraph("")
                .AppendEquation($"c=c_1+c_2+c_3={_fbdi.c1}+{_fbdi.c2}+{_fbdi.c3}={_c:f2} мм");

            body.AddParagraph("").AppendEquation($"s_1={_s1p:f2}+{_c:f2}={_s1:f2} мм");

            if (_fbdi.s1 > _s1)
            {
                body.AddParagraph("Принятая толщина ")
                    .AppendEquation($"s_1={_fbdi.s1} мм")
                    .Bold();
            }
            else
            {
                body.AddParagraph($"Принятая толщина ")
                    .AppendEquation($"s_1={_fbdi.s1} мм")
                    .Bold()
                    .Color(System.Drawing.Color.Red);
            }

            if (_fbdi.Type is 10 or 11 or 12)
            {
                body.AddParagraph("").AppendEquation("s_2≥s_2p+c");
                body.AddParagraph("где ");
                
                switch (_fbdi.Type)
                {
                    case 10:
                        body.AddParagraph("")
                            .AppendEquation("s_2p=max{1.1∙(s-c);(s_1-c)/(1+(D_p-2∙r)/(1.2∙(s_1-c))∙sinγ)}");
                        body.AddParagraph("")
                            .AppendEquation($"1.1∙(s-c)=1.1∙({_fbdi.s}-{_c})={_s2p_1:f2}");
                        body.AddParagraph("")
                            .AppendEquation($"(s_1-c)/(1+(D_p-2∙r)/(1.2∙(s_1-c))∙sinγ)=({_fbdi.s1}-{_c})/(1+({_Dp}-2∙{_fbdi.r})/(1.2∙({_fbdi.s1}-{_c}))∙sin{_fbdi.gamma})={_s2p_2:f2}");
                        break;
                    case 11:
                    case 12:
                        body.AddParagraph("")
                            .AppendEquation("s_2p=max{0.7∙(s_1-c);(s_1-c)∙√(2∙((D_p-D_2)∙D_2)/(D_p^2))}");
                        body.AddParagraph("")
                            .AppendEquation($"0.7∙(s_1-c)=0.7∙({_fbdi.s1}-{_c})={_s2p_1:f2}");
                        body.AddParagraph("")
                            .AppendEquation($"(s_1-c)∙√(2∙((D_p-D_2)∙D_2)/(D_p^2))=({_fbdi.s1}-{_c})∙√(2∙(({_Dp}-{_fbdi.D2})∙{_fbdi.D2})/({_Dp}^2))={_s2p_2:f2}");
                        break;
                }

                body.AddParagraph("").AppendEquation($"s_2={_s2p:f2}+{_c:f2}={_s2:f2} мм");

                if (_fbdi.s2 > _s2)
                {
                    body.AddParagraph("Принятая толщина ")
                        .AppendEquation($"s_2={_fbdi.s2} мм")
                        .Bold();
                }
                else
                {
                    body.AddParagraph($"Принятая толщина ")
                        .AppendEquation($"s_2={_fbdi.s2} мм")
                        .Bold()
                        .Color(System.Drawing.Color.Red);
                }
            }
            
            body.AddParagraph("Допускаемое давление вычисляют по формуле:");
            body.AddParagraph("")
                .AppendEquation("[p]=((s_1-c)/(K∙K_0∙D_p))^2∙[σ]∙φ"
                                + $"=(({_fbdi.s1}-{_c:f2})/({_K}∙{_K0:f2}∙{_Dp:f2}))^2∙{_sigmaAllow}∙{_fbdi.fi}"
                                + $"={_p_d:f2} МПа");

            body.AddParagraph("Условия применения расчетных формул ");
            body.AddParagraph("")
                .AppendEquation($"(s_1-c)/D_p=({_fbdi.s1}-{_c:f2})/{_Dp:f2}={_conditionUseFormulas:f2}≤0.11");
            if (_isConditionUseFormulas)
            {
                body.AddParagraph("Условие прочности");
                body.AddParagraph("").AppendEquation("[p]≥p");
                body.AddParagraph("")
                    .AppendEquation($"{_p_d:f2}≥{_fbdi.p}");
            }
            else
            {
                body.AddParagraph("Т.к. условие применения формул не выполняется, то условие прочности имеет вид");
                body.AddParagraph("").AppendEquation("K_p∙[p]≥p");
                body.AddParagraph("где ")
                        .AppendEquation("K_p")
                        .AddRun("  - поправочный коэффициент");
                body.AddParagraph("")
                        .AppendEquation("K_p=2.2/(1+√(1+(6∙(s_1-c)/D_p)^2))" +
                        $"=2.2/(1+√(1+(6∙({_fbdi.s1}-{_c:f2})/{_Dp:f2})^2))={_Kp:f2}");

                body.AddParagraph("")
                    .AppendEquation($"{_Kp:f2}∙{_p_d:f2}={_Kp * _p_d:f2}≥{_fbdi.p}");
            }
            if (_p_d * _Kp > _fbdi.p)
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


            if (_fbdi.Type is 1 or 2 or 6 or 9 or 10)
            {
                body.AddParagraph("Условия закрепления");

                switch (_fbdi.Type)
                {
                    case 1:
                        body.AddParagraph("")
                            .AppendEquation($"a≥1.7∙s={_fbdi.a}≥1.7∙{_fbdi.s}={1.7 * _fbdi.s:f2}");
                        break;
                    case 2:
                    case 6:
                        body.AddParagraph("")
                            .AppendEquation($"a≥0.85∙s={_fbdi.a}≥0.85∙{_fbdi.s}={0.85 * _fbdi.s:f2}");
                        break;
                    case 9:
                        body.AddParagraph("")
                            .AppendEquation("max{s;0.25∙s_1}≤r≤min{s_1;0.1∙D}");
                        body.AddParagraph("")
                            .AppendEquation(
                                $"{Math.Max(_fbdi.s, 0.25 * _fbdi.s1):f2}≤{_fbdi.r}≤{Math.Min(_fbdi.s1, 0.1 * _fbdi.D):f2}");
                        body.AddParagraph("")
                            .AppendEquation($"h_1={_fbdi.h1}≥r={_fbdi.r}");
                        break;
                    case 10:
                        body.AddParagraph("")
                            .AppendEquation($"a≥1.7∙s={_fbdi.a}≥1.7∙{_fbdi.s}={1.7 * _fbdi.s:f2}");
                        break;
                }
            }



            if (_isConditionFixed)
            {
                body.AddParagraph("Условие закрепления выполняется")
                    .Bold();
            }
            else
            {
                body.AddParagraph("Условие закрепления не выполняется")
                    .Bold()
                    .Color(System.Drawing.Color.Red);
            }

            package.Close();
        }
    }
}
