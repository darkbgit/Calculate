using CalculateVessels.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalculateVessels.Core.Supports.Enums;
using DocumentFormat.OpenXml.Packaging;
using CalculateVessels.Core.Word;
using CalculateVessels.Core.Word.Enums;

namespace CalculateVessels.Core.Supports.Saddle
{
    public class Saddle : IElement
    {
        private readonly SaddleDataIn _saddleDataIn;

        public Saddle(SaddleDataIn saddleDataIn)
        {
            _saddleDataIn = saddleDataIn;
        }

        public bool IsCriticalError { get; private set; }

        public bool IsError {get; private set; }

        public List<string> ErrorList { get; private set; } = new ();

        public bool IsConditionUseFormulas { get; private set; }

        public List<string> Bibliograhy { get; } = new()
        {
            Data.Properties.Resources.GOST_34233_1,
            Data.Properties.Resources.GOST_34233_5
        };

        private double _q;
        private double _M0;
        private double _p_d;
        private double _F1;
        private double _F2;
        private double _F_d;
        private double _M1;
        private double _M2;
        private double _M12;
        private double _M_d;
        private double _Q1;
        private double _Q2;
        private double _Q_d;
        private double _B1;
        private double _B1_2;
        private double _conditionStrength1_1;
        private double _conditionStrength1_2;
        private double _conditionStrength2;
        private double _conditionStability1;
        private double _conditionStability2;
        private double _K9;
        private double _K9_1;
        private double _y;
        private double _x;
        private double _gamma;
        private double _beta1;
        private double _K10;
        private double _K10_1;
        private double _K11;
        private double _K12;
        private double _K13;
        private double _K14;
        private double _K15;
        private double _K15_2;
        private double _K16;
        private double _K17;
        private double _sigma_mx;
        private double _F_d2;
        private double _F_d3;
        private double _v1_2;
        private double _v1_3;
        private double _v21_2;
        private double _v21_3 = 0;
        private double _v22_2;
        private double _v22_3;
        private double _K2;
        private double _K1_2;
        private double _K1_2For_v21;
        private double _K1_2For_v22;
        private double _K1_3;
        private double _K1_3For_v21;
        private double _K1_3For_v22;
        private double _sigmai2;
        private double _sigmai2_1;
        private double _sigmai2_2;
        private double _sigmai3;
        private double _sigmai3_1;
        private double _sigmai3_2;
        private double _Fe;
        private double _sef;
        private double _Ak;

        

        public void Calculate()
        {
            if (!_saddleDataIn.IsDataGood)
            {
                IsCriticalError = true;
                ErrorList.Add("Входные данные не верны");
                return;
            }

            _M_d = (0.0000089 * _saddleDataIn.E) / _saddleDataIn.ny * Math.Pow(_saddleDataIn.D, 3) *
                   Math.Pow((100 * (_saddleDataIn.s - _saddleDataIn.c)) / _saddleDataIn.D, 2.5);
            //UNDONE: проверить формулу для расчета [F]
            _F_d = (0.0000031 * _saddleDataIn.E) / _saddleDataIn.ny * Math.Pow(_saddleDataIn.D, 2) *
                   Math.Pow((100 * (_saddleDataIn.s - _saddleDataIn.c)) / _saddleDataIn.D, 2.5);
            _Q_d = (2.4 * _saddleDataIn.E * Math.Pow(_saddleDataIn.s - _saddleDataIn.c, 2)) / _saddleDataIn.ny *
                (0.18 + 3.3 * _saddleDataIn.D * (_saddleDataIn.s - _saddleDataIn.c)) / Math.Pow(_saddleDataIn.L, 2);
            _B1_2 = 9.45 * (_saddleDataIn.D / _saddleDataIn.L) * Math.Sqrt(_saddleDataIn.D / (100 * (_saddleDataIn.s - _saddleDataIn.c)));
            _B1 = Math.Min(1, _B1_2);
            _p_d = (0.00000208 * _saddleDataIn.E) / (_saddleDataIn.ny * _B1) * (_saddleDataIn.D / _saddleDataIn.L) *
                   Math.Pow(100 * (_saddleDataIn.s - _saddleDataIn.c) / _saddleDataIn.D, 2.5);


            _q = _saddleDataIn.G / (_saddleDataIn.L + (4.0 / 3.0) * _saddleDataIn.H);
            _M0 = _q * (Math.Pow(_saddleDataIn.D, 2) / 16.0);
            // UNDONE: Make calculate for non symmetrical saddle
            _F1 = _saddleDataIn.G / 2.0;
            _F2 = _F1;
            _M1 = _q * Math.Pow(_saddleDataIn.e, 2) / 2.0 - _M0;
            _M2 = _M1;
            _M12 = _M0 + _F1 * (_saddleDataIn.L / 2.0 - _saddleDataIn.a) -
                   (_q / 2.0) * Math.Pow(_saddleDataIn.L / 2.0 + (2.0 / 3.0) * _saddleDataIn.H, 2);
            _Q1 = (_saddleDataIn.L - 2.0 * _saddleDataIn.a) * _F1 /
                  (_saddleDataIn.L + (4.0 / 3.0) * _saddleDataIn.H);
            _Q2 = _Q1;
            if (_M12 > _M1)
            {
                _y = _saddleDataIn.D / (_saddleDataIn.s - _saddleDataIn.c);
                _x = _saddleDataIn.L / _saddleDataIn.D;
                _K9_1 = 1.6 - 0.20924 * (_x - 1) + 0.028702 * _x * (_x - 1) + 0.0004795 * _y * (_x - 1) -
                        0.0000002391 * _x * _y * (_x - 1) - 0.0029936 * (_x - 1) * Math.Pow(_x, 2) -
                        0.00000085692 * (_x - 1) * Math.Pow(_y, 2) + 0.00000088174 * Math.Pow(_x, 2) * (_x - 1) * _y -
                        0.0000000075955 * Math.Pow(_y, 2) * (_x - 1) * _x + 0.000082748 * (_x - 1) * Math.Pow(_x, 3) +
                        0.00000000048168 * (_x - 1) * Math.Pow(_y, 3);
                _K9 = Math.Max(_K9_1, 1);
                _conditionStrength1_1 = _saddleDataIn.p * _saddleDataIn.D /
                                        (4 * (_saddleDataIn.s - _saddleDataIn.c)) +
                                        4 * _M12 * _K9 / (Math.PI * Math.Pow(_saddleDataIn.D, 2) * (_saddleDataIn.s - _saddleDataIn.c));
                // UNDONE: if fi<1 check condition
                _conditionStrength1_2 = _saddleDataIn.sigma_d * _saddleDataIn.fi;
                if (_conditionStrength1_1 > _conditionStrength1_2)
                {
                    ErrorList.Add("Несущая способность обечайки в сечении между опорами. Условие прочности не выполняется");
                    IsError = true;
                }
                _conditionStability1 =
                    _saddleDataIn.IsPressureIn ? Math.Abs(_M12) / _M_d : 
                        _saddleDataIn.p / _p_d + Math.Abs(_M12) / _M_d;
                if (_conditionStability1 > 1)
                {
                    ErrorList.Add("Несущая способность обечайки в сечении между опорами. Условие устойчивости не выполняется");
                    IsError = true;
                }
            }
            switch (_saddleDataIn.Type)
            {
                case SaddleType.SaddleWithoutRingWithoutSheet:
                {
                    _gamma = 2.83 * (_saddleDataIn.a / _saddleDataIn.D) *
                             Math.Sqrt((_saddleDataIn.s - _saddleDataIn.c) / _saddleDataIn.D);
                    _beta1 = 0.91 * _saddleDataIn.b /
                             Math.Sqrt(_saddleDataIn.D * (_saddleDataIn.s - _saddleDataIn.c));
                    _K10_1 = Math.Exp(-_beta1) * Math.Sin(_beta1) / _beta1;
                    _K10 = Math.Max(_K10_1, 0.25);
                    _K11 = (1.0 - Math.Exp(-_beta1) * Math.Cos(_beta1)) / _beta1;
                    _K12 = (1.15 - 0.1432 * DegToRad(_saddleDataIn.delta1)) /
                           Math.Sin(0.5 * DegToRad(_saddleDataIn.delta1));
                    _K13 = Math.Max(1.7 - 2.1 * DegToRad(_saddleDataIn.delta1) / Math.PI, 0) /
                           Math.Sin(0.5 * DegToRad(_saddleDataIn.delta1));
                    _K14 = (1.45 - 0.43 * DegToRad(_saddleDataIn.delta1)) /
                           Math.Sin(0.5 * DegToRad(_saddleDataIn.delta1));
                    _K15_2 = (0.8 * Math.Sqrt(_gamma) + 6 * _gamma) / DegToRad(_saddleDataIn.delta1);
                    _K15 = Math.Min(1, _K15_2);
                    _K16 = 1.0 - 0.65 / (1 + Math.Pow(6 * _gamma, 2)) *
                        Math.Sqrt(Math.PI / (3.0 * DegToRad(_saddleDataIn.delta1)));
                    _K17 = 1.0 / (1.0 + 0.6 *
                        Math.Pow(_saddleDataIn.D / (_saddleDataIn.s - _saddleDataIn.c), 1.0 / 3.0) *
                        (_saddleDataIn.b / _saddleDataIn.D) * DegToRad(_saddleDataIn.delta1));
                    _sigma_mx = 4 * _M1 /
                                (Math.PI * Math.Pow(_saddleDataIn.D, 2) * (_saddleDataIn.s - _saddleDataIn.c));

                    _v1_2 = -0.23 * _K13 * _K15 / (_K12 * _K10);
                    _v1_3 = -0.53 * _K11 /
                            (_K14 * _K16 * _K17 * Math.Sin(0.5 * DegToRad(_saddleDataIn.delta1)));

                    _K2 = _saddleDataIn.IsAssembly ? 1.05 : 1.25;

                    _v21_2 = - _sigma_mx / (_K2 * _saddleDataIn.sigma_d);
                    _v21_3 = 0;

                    _v22_2 = (_saddleDataIn.p * _saddleDataIn.D /
                              (4 * (_saddleDataIn.s - _saddleDataIn.c)) -
                              _sigma_mx) / (_K2 * _saddleDataIn.sigma_d);
                    _v22_3 = (_saddleDataIn.p * _saddleDataIn.D /
                              (2 * (_saddleDataIn.s - _saddleDataIn.c))) /
                             (_K2 * _saddleDataIn.sigma_d);

                    _K1_2For_v21 = K1(_v1_2, _v21_2); 
                    _K1_2For_v22 = K1(_v1_2, _v22_2);
                    _K1_2 = Math.Min(_K1_2For_v21, _K1_2For_v22);

                    _K1_3For_v21 = K1(_v1_3, _v21_3);
                    _K1_3For_v22 = K1(_v1_3, _v22_3);
                    _K1_3 = Math.Min(_K1_3For_v21, _K1_3For_v22);

                    _sigmai2_1 = _K1_2For_v21 * _K2 * _saddleDataIn.sigma_d;
                    _sigmai2_2 = _K1_2For_v22 * _K2 * _saddleDataIn.sigma_d;
                    _sigmai2 = Math.Min(_sigmai2_1, _sigmai2_2);

                    _sigmai3_1 = _K1_3For_v21 * _K2 * _saddleDataIn.sigma_d;
                    _sigmai3_2 = _K1_3For_v22 * _K2 * _saddleDataIn.sigma_d;
                    _sigmai3 = Math.Min(_sigmai3_1, _sigmai3_2);

                    _F_d2 = 0.7 * _sigmai2 * (_saddleDataIn.s - _saddleDataIn.c) *
                        Math.Sqrt(_saddleDataIn.D * (_saddleDataIn.s - _saddleDataIn.c)) / (_K10 * _K12);
                    _F_d3 = 0.9 * _sigmai3 * (_saddleDataIn.s - _saddleDataIn.c) *
                        Math.Sqrt(_saddleDataIn.D * (_saddleDataIn.s - _saddleDataIn.c)) / (_K14 * _K16 * _K17);

                    _conditionStrength2 = Math.Min(_F_d2, _F_d3);

                    if (_F1 > _conditionStrength2)
                    {
                        ErrorList.Add("Несущая способность обечайки, не укрепленной кольцами жесткости в области опорного узла. Условие прочности не выполняется");
                        IsError = true;
                    }


                    _Fe = _F1 * (Math.PI / 4.0) * _K13 * _K15 *
                          Math.Sqrt(_saddleDataIn.D / (_saddleDataIn.s - _saddleDataIn.c));

                    _conditionStability2 = _saddleDataIn.IsPressureIn
                        ? Math.Abs(_M1) / _M_d + _Fe / _F_d + Math.Pow(_Q1 / _Q_d, 2)
                        : _saddleDataIn.p / _p_d + Math.Abs(_M1) / _M_d + _Fe / _F_d + Math.Pow(_Q1 / _Q_d, 2);
                    if (_conditionStability2 > 1)
                    {
                        ErrorList.Add("Несущая способность обечайки, не укрепленной кольцами жесткости в области опорного узла. Условие устойчивости не выполняется");
                        IsError = true;
                    }
                    break;
                }
                case SaddleType.SaddleWithoutRingWithSheet:
                {
                    _sef = (_saddleDataIn.s - _saddleDataIn.c) *
                           Math.Sqrt(1 + Math.Pow(_saddleDataIn.s2 / (_saddleDataIn.s - _saddleDataIn.c), 2));
                    _gamma = 2.83 * (_saddleDataIn.a / _saddleDataIn.D) *
                             Math.Sqrt(_sef / _saddleDataIn.D);
                    _beta1 = 0.91 * _saddleDataIn.b2 / Math.Sqrt(_saddleDataIn.D * _sef);
                    _K10_1 = Math.Exp(-_beta1) * Math.Sin(_beta1) / _beta1;
                    _K10 = Math.Max(_K10_1, 0.25);
                    _K11 = (1 - Math.Exp(-_beta1) * Math.Cos(_beta1)) / _beta1;
                    _K12 = (1.15 - 0.1432 * DegToRad(_saddleDataIn.delta2)) /
                           Math.Sin(0.5 * DegToRad(_saddleDataIn.delta2));
                    _K13 = Math.Max(1.7 - 2.1 * DegToRad(_saddleDataIn.delta2) / Math.PI, 0) /
                           Math.Sin(0.5 * DegToRad(_saddleDataIn.delta2));
                    _K14 = (1.45 - 0.43 * DegToRad(_saddleDataIn.delta2)) /
                           Math.Sin(0.5 * DegToRad(_saddleDataIn.delta2));
                    _K15_2 = (0.8 * Math.Sqrt(_gamma) + 6 * _gamma) / DegToRad(_saddleDataIn.delta2);
                    _K15 = Math.Min(1, _K15_2);
                    _K16 = 1 - 0.65 / (1 + Math.Pow(6 * _gamma, 2)) *
                        Math.Sqrt(Math.PI / (3 * DegToRad(_saddleDataIn.delta2)));
                    _K17 = 1.0 / (1.0 + 0.6 * Math.Pow(_saddleDataIn.D / _sef, 1.0 / 3.0) *
                        (_saddleDataIn.b2 / _saddleDataIn.D) * DegToRad(_saddleDataIn.delta2));
                    _sigma_mx = 4 * _M1 / (Math.PI * Math.Pow(_saddleDataIn.D, 2) * _sef);

                    _v1_2 = -0.23 * _K13 * _K15 / (_K12 * _K10);
                    _v1_3 = -0.53 * _K11 / (_K14 * _K16 * _K17 * Math.Sin(0.5 * DegToRad(_saddleDataIn.delta2)));

                    _K2 = _saddleDataIn.IsAssembly ? 1.05 : 1.25;

                    _v21_2 = -_sigma_mx / (_K2 * _saddleDataIn.sigma_d);
                    _v21_3 = 0;

                    _v22_2 = (_saddleDataIn.p * _saddleDataIn.D / (4 * _sef) - _sigma_mx) /
                             (_K2 * _saddleDataIn.sigma_d);
                    _v22_3 = (_saddleDataIn.p * _saddleDataIn.D / (2 * _sef)) /
                             (_K2 * _saddleDataIn.sigma_d);

                    _K1_2For_v21 = K1(_v1_2, _v21_2);
                    _K1_2For_v22 = K1(_v1_2, _v22_2);
                    _K1_2 = Math.Min(_K1_2For_v21, _K1_2For_v22);

                    _K1_3For_v21 = K1(_v1_3, _v21_3);
                    _K1_3For_v22 = K1(_v1_3, _v22_3);
                    _K1_3 = Math.Min(_K1_3For_v21, _K1_3For_v22);

                    _sigmai2_1 = _K1_2For_v21 * _K2 * _saddleDataIn.sigma_d;
                    _sigmai2_2 = _K1_2For_v22 * _K2 * _saddleDataIn.sigma_d;
                    _sigmai2 = Math.Min(_sigmai2_1, _sigmai2_2);

                    _sigmai3_1 = _K1_3For_v21 * _K2 * _saddleDataIn.sigma_d;
                    _sigmai3_2 = _K1_3For_v22 * _K2 * _saddleDataIn.sigma_d;
                    _sigmai3 = Math.Min(_sigmai3_1, _sigmai3_2);

                    _F_d2 = 0.7 * _sigmai2 * _sef * Math.Sqrt(_saddleDataIn.D * _sef) / (_K10 * _K12);
                    _F_d3 = 0.9 * _sigmai3 * _sef * Math.Sqrt(_saddleDataIn.D * _sef) / (_K14 * _K16 * _K17);
                           
                    _conditionStrength2 = Math.Min(_F_d2, _F_d3);

                    if (_F1 > _conditionStrength2)
                    {
                        ErrorList.Add("Несущая способность обечайки, не укрепленной кольцами жесткости в области опорного узла. Условие прочности не выполняется");
                        IsError = true;
                    }

                    _Fe = _F1 * (Math.PI / 4.0) * _K13 * _K15 * Math.Sqrt(_saddleDataIn.D / _sef);

                    _conditionStability2 = _saddleDataIn.IsPressureIn
                        ? Math.Abs(_M1) / _M_d + _Fe / _F_d + Math.Pow(_Q1 / _Q_d, 2)
                        : _saddleDataIn.p / _p_d + Math.Abs(_M1) / _M_d + _Fe / _F_d + Math.Pow(_Q1 / _Q_d, 2);
                    if (_conditionStability2 > 1)
                    {
                        ErrorList.Add("Несущая способность обечайки, не укрепленной кольцами жесткости в области опорного узла с подкладным листом. Условие устойчивости не выполняется");
                        IsError = true;
                    }
                    break;
                }
                case SaddleType.SaddleWithRing:
                {
                    //TODO: опора с укрепляющим кольцом
                    break;
                }
            }


            IsConditionUseFormulas = true;

            if (_saddleDataIn.delta1 <= 60 || _saddleDataIn.delta1 >= 180)
            {
                IsConditionUseFormulas = false;
                ErrorList.Add("delta1 должно быть в пределах 60-180");
            }
            if ((_saddleDataIn.s - _saddleDataIn.c) / _saddleDataIn.D > 0.05)
            {
                IsConditionUseFormulas = false;
                ErrorList.Add("Условие применения формул не выполняется");
            }

            switch (_saddleDataIn.Type)
            {
                case SaddleType.SaddleWithoutRingWithSheet:
                {
                    if (_saddleDataIn.delta2 < _saddleDataIn.delta1 + 20)
                    {
                        IsConditionUseFormulas = false;
                        ErrorList.Add("Угол обхвата подкладного листа должен быть delta2>=delta1+20");
                    }

                    if (_saddleDataIn.s2 < _saddleDataIn.s)
                    {
                        IsConditionUseFormulas = false;
                        ErrorList.Add("Толщина подкладного листа должна быть s2>=s");
                    }

                    break;
                }
                case SaddleType.SaddleWithRing:
                {
                    _Ak = (_saddleDataIn.s - _saddleDataIn.c) * Math.Sqrt(_saddleDataIn.D * (_saddleDataIn.s - _saddleDataIn.c));
                    if (_saddleDataIn.Ak < _Ak)
                    {
                        IsConditionUseFormulas = false;
                        ErrorList.Add("Условие применения формул не выполняется");
                    }

                    break;
                }
            }
        }

        private double K1(double v1, double v2) =>
            (1 - Math.Pow(v2, 2)) /
        (1.0 / 3.0 + v1 * v2 + Math.Sqrt(Math.Pow(1.0 / 3.0 + v1 * v2, 2) +
                                         (1 - Math.Pow(v2, 2)) * Math.Pow(v1, 2)));

        private static double DegToRad(double degree) => degree * Math.PI / 180;


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

            body.AddParagraph($"Расчет на прочность обечайки {_saddleDataIn.NameShell} от воздействия опорных нагрузок. Седловые опоры")
                .Heading(HeadingType.Heading1)
                .Alignment(AlignmentType.Center);

            body.AddParagraph("");

            var imagePart = mainPart.AddImagePart(ImagePartType.Gif);

            var stream = _saddleDataIn.Type switch
            {
                SaddleType.SaddleWithoutRingWithoutSheet =>
                    new MemoryStream(Data.Properties.Resources.SaddleNothingElem),
                SaddleType.SaddleWithoutRingWithSheet =>
                    new MemoryStream(Data.Properties.Resources.SaddleSheetElem),
                _ => new MemoryStream(Data.Properties.Resources.SaddleNothingElem)
            };
            imagePart.FeedData(stream);

            body.AddParagraph("").AddImage(mainPart.GetIdOfPart(imagePart));

            body.AddParagraph("Исходные данные").Alignment(AlignmentType.Center);

            //table
            {
                var table = body.AddTable();

                table.AddRow()
                    .AddCell("Внутренний диаметр обечайки, D:")
                    .AddCell($"{_saddleDataIn.D} мм");

                table.AddRow()
                    .AddCell("Толщина стенки обечайки, s:")
                    .AddCell($"{_saddleDataIn.s} мм");

                table.AddRow()
                    .AddCell("Прибавка к расчетной толщине, c:")
                    .AddCell($"{_saddleDataIn.c} мм");

                table.AddRow()
                    .AddCell("Длина обечайки, ")
                    .AppendEquation("L_ob")
                    .AppendText(":")
                    .AddCell($"{_saddleDataIn.Lob} мм");

                table.AddRow()
                    .AddCell("Коэффициент прочности сварного шва, φ:")
                    .AddCell($"{_saddleDataIn.fi}");

                table.AddRow()
                    .AddCell("Марка стали")
                    .AddCell($"{_saddleDataIn.Steel}");

                table.AddRow()
                    .AddCell("Ширина опоры, b:")
                    .AddCell($"{_saddleDataIn.b} мм");

                table.AddRow()
                    .AddCell("Угол охвата опоры, ")
                    .AppendEquation("δ_1")
                    .AppendText(":")
                    .AddCell($"{_saddleDataIn.delta1} °");

                table.AddRow()
                    .AddCell("Длина свободно выступающей части, e:")
                    .AddCell($"{_saddleDataIn.e} мм");

                table.AddRow()
                    .AddCell("Длина выступающей цилиндрической части сосуда, включая отбортовку днища, a")
                    .AddCell($"{_saddleDataIn.a} мм");

                table.AddRow()
                    .AddCell("Высота опоры, Н")
                    .AddCell($"{_saddleDataIn.H} мм");
                if (_saddleDataIn.Type == SaddleType.SaddleWithoutRingWithSheet)
                {
                    table.AddRow()
                        .AddCell("Толщина подкладного листа, ")
                        .AppendEquation("s_2")
                        .AppendText(":")
                        .AddCell($"{_saddleDataIn.s2} мм");

                    table.AddRow()
                        .AddCell("Ширина подкладного листа, мм")
                        .AppendEquation("b_2")
                        .AppendText(":")
                        .AddCell($"{_saddleDataIn.b2} мм");

                    table.AddRow()
                        .AddCell("Угол охвата подкладного листа, ")
                        .AppendEquation("δ_2")
                        .AppendText(":")
                        .AddCell($"{_saddleDataIn.delta2} °");
                }
                body.InsertTable(table);
            }

            body.AddParagraph("");
            body.AddParagraph("Условия нагружения")
                .Alignment(AlignmentType.Center);

            //table
            {
                var table = body.AddTable();

                table.AddRow()
                    .AddCell("Собственный вес с содержимым, G:")
                    .AddCell($"{_saddleDataIn.G} H");

                table.AddRow()
                    .AddCell("Расчетная температура, Т:")
                    .AddCell($"{_saddleDataIn.Temp} °С");

                table.AddRow()
                    .AddCell("Расчетное " + 
                             (_saddleDataIn.IsPressureIn
                             ? "внутреннее избыточное"
                             : "наружное") + " давление, p:")
                    .AddCell($"{_saddleDataIn.p} МПа");

                table.AddRow()
                    .AddCell($"Допускаемое напряжение для материала {_saddleDataIn.Steel} при расчетной температуре, [σ]:")
                    .AddCell($"{_saddleDataIn.sigma_d} МПа");

                if (!_saddleDataIn.IsPressureIn)
                {
                    table.AddRow()
                        .AddCell("Модуль продольной упругости при расчетной температуре, E:")
                        .AddCell($"{_saddleDataIn.E} МПа");
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
                    $"={_saddleDataIn.G}/({_saddleDataIn.L}+4/3∙{_saddleDataIn.H})={_q:f2} Н/мм");

            body.AddParagraph("Расчетный изгибающий момент, действующий на консольную часть обечайки");
            body.AddParagraph("")
                .AppendEquation("M_0=q∙D^2/16" +
                                $"={_q:f2}∙{_saddleDataIn.D}^2/16={_M0:f2} Н∙мм");

            body.AddParagraph("Опорное усилие");
            body.AddParagraph("")
                .AppendEquation("F_1=F_2=G/2" +
                                $"={_saddleDataIn.G}/2={_F1:f2} H");

            body.AddParagraph("Изгибающий момент над опорами");
            body.AddParagraph("")
                .AppendEquation("M_1=M_2=(q∙e^2)/2-M_0" +
                                $"=({_q:f2}∙{_saddleDataIn.e:f2}^2)/2-{_M0:f2}={_M1:f2} Н∙мм");

            body.AddParagraph("Максимальный изгибающий момент между опорами");
            body.AddParagraph("")
                .AppendEquation("M_12=M_0+F_1∙(L/2-a)-q/2∙(L/2+2/3∙H)^2" + 
                                $"={_M0:f2}+{_F1:f2}∙({_saddleDataIn.L}/2-{_saddleDataIn.a})-{_q:f2}/2∙({_saddleDataIn.L}/2+2/3∙{_saddleDataIn.H})^2={_M12:f2} Н∙мм");

            body.AddParagraph("Поперечное усилие в сечении оболочки над опорой");
            body.AddParagraph("")
                .AppendEquation("Q_1=Q_2=(L-2∙a)/(L+4/3∙H)∙F_1" +
                                $"=({_saddleDataIn.L}-2∙{_saddleDataIn.a})/({_saddleDataIn.L}+4/3∙{_saddleDataIn.H})∙{_F1:f2}={_Q1:f2} H");

            body.AddParagraph("Несущую способность обечайки в сечении между опорами следует проверять при условии");
            body.AddParagraph("").AppendEquation("max{M_12}>max{M_1}");
            body.AddParagraph("").AppendEquation($"{_M12:f2} Н∙мм > {_M1:f2} Н∙мм");
            if (_M12 > _M1)
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
                    .AppendEquation($"y={_saddleDataIn.D}/({_saddleDataIn.s}-{_saddleDataIn.c})={_y:f2}");
                body.AddParagraph("")
                    .AppendEquation($"x={_saddleDataIn.L}/{_saddleDataIn.D}={_x:f2}");

                body.AddParagraph("").AppendEquation($"K_9=max({_K9_1:f2};1)={_K9:f2}");

                body.AddParagraph("")
                    .AppendEquation($"(p∙D)/(4∙(s-c))+(4∙M_12∙K_9)/(π∙D^2∙(s-c))=({_saddleDataIn.p}∙{_saddleDataIn.D})/(4∙({_saddleDataIn.s}-{_saddleDataIn.c}))+(4∙{_M12:f2}∙{_K9:f2})/(π∙{_saddleDataIn.D}^2∙({_saddleDataIn.s}-{_saddleDataIn.c}))={_conditionStrength1_1:f2}");
                body.AddParagraph("")
                    .AppendEquation($"[σ]∙φ={_saddleDataIn.sigma_d}∙{_saddleDataIn.fi}={_conditionStrength1_2:f2}");
                body.AddParagraph("")
                    .AppendEquation($"{_conditionStrength1_1:f2}≤{_conditionStrength1_2:f2}");
                if (_conditionStrength1_1 <= _conditionStrength1_2)
                {
                    body.AddParagraph("Условие прочности выполняется");
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
                                    $"=(8.9∙10^-5∙{_saddleDataIn.E})/{_saddleDataIn.ny}∙{_saddleDataIn.D}^3∙[(100∙({_saddleDataIn.s}-{_saddleDataIn.c}))/{_saddleDataIn.D}]^2.5={_M_d:f2} Н∙мм");
                body.AddParagraph("").AppendEquation($"|{_M12:f2}|/{_M_d:f2}={_conditionStability1:f2}≤1");

                if (_conditionStability1 <= 1)
                {
                    body.AddParagraph("Условие устойчивости выполняется");
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

            switch (_saddleDataIn.Type)
            {
                case SaddleType.SaddleWithoutRingWithoutSheet:
                {
                    body.AddParagraph("Проверка несущей способности обечайки, не укрепленной кольцами жесткости в области опорного узла и без подкладного листа в месте опоры");
                    body.AddParagraph("Вспомогательные параметры и коэффициенты");
                    body.AddParagraph("Параметр, определяемый расстоянием от середины опоры до днища");
                    body.AddParagraph("")
                        .AppendEquation("γ=2.83∙a/D∙√((s-c)/D)" +
                                        $"=2.83∙{_saddleDataIn.a}/{_saddleDataIn.D}∙√(({_saddleDataIn.s}-{_saddleDataIn.c})/{_saddleDataIn.D})={_gamma:f2}");

                    body.AddParagraph("Параметр, определяемый шириной пояса опоры");
                    body.AddParagraph("")
                        .AppendEquation("β_1=0.91∙b/√(D∙(s-c))" +
                                        $"=0.91∙{_saddleDataIn.b}/√({_saddleDataIn.D}∙({_saddleDataIn.s}-{_saddleDataIn.c}))={_beta1:f2}");

                    body.AddParagraph("Коэффициенты, учитывающие влияние ширины пояса опоры");
                    body.AddParagraph("")
                        .AppendEquation("K_10=max{(exp(-β_1)∙sin(β_1))/β_1;0.25}" + 
                                        $"=max{{(exp(-{_beta1:f2})∙sin({_beta1:f2}))/{_beta1:f2};0.25}}" +
                                        $"=max({_K10_1:f2};0.25)={_K10:f2}");

                    body.AddParagraph("")
                        .AppendEquation("K_11=(1-exp(-β_1)∙cos(β_1))/β_1" +
                                        $"=(1-exp(-{_beta1:f2})∙cos({_beta1:f2}))/{_beta1:f2}={_K11:f2}");

                    body.AddParagraph("Коэффициенты, учитывающие влияние угла охвата");
                    body.AddParagraph("")
                        .AppendEquation("K_12=(1.15-0.1432∙δ_1)/sin(0.5∙δ_1)" +
                                        $"=(1.15-0.1432∙{DegToRad(_saddleDataIn.delta1):f2})/sin(0.5∙{DegToRad(_saddleDataIn.delta1):f2})={_K12:f2}");

                    body.AddParagraph("")
                        .AppendEquation("K_13=(max{1.7-(2.1∙δ_1)/π;0})/sin(0.5∙δ_1)" +
                                        $"=(max{{1.7 - (2.1∙{DegToRad(_saddleDataIn.delta1):f2})/π;0}})/sin(0.5∙{DegToRad(_saddleDataIn.delta1):f2})={_K13:f2}");

                    body.AddParagraph("")
                        .AppendEquation("K_14=(1.45-0.43∙δ_1)/sin(0.5∙δ_1)" +
                                        $"=(1.45-0.43∙{DegToRad(_saddleDataIn.delta1):f2})/sin(0.5∙{DegToRad(_saddleDataIn.delta1):f2})={_K14:f2}");

                    body.AddParagraph("Коэффициенты, учитывающие влияние расстояния от середины опоры до днища и угла охвата");
                    body.AddParagraph("")
                        .AppendEquation("K_15=min{1.0;(0.8∙√γ+6∙γ)/δ_1}" +
                                        $"min{{1.0;(0.8∙√{_gamma:f2}+6∙{_gamma:f2})/{DegToRad(_saddleDataIn.delta1):f2}}}=min{{1.0;{_K15_2:f2}}}={_K15:f2}");

                    body.AddParagraph("")
                        .AppendEquation("K_16=1-0.65/(1+(6∙γ)^2)∙√(π/(3∙δ_1))" 
                                        + $"=1-0.65/(1+(6∙{_gamma:f2})^2)∙√(π/(3∙{DegToRad(_saddleDataIn.delta1):f2}))={_K16:f2}");

                    body.AddParagraph("Коэффициенты, учитывающие влияние ширины пояса опоры и угла охвата");
                    body.AddParagraph("")
                        .AppendEquation("K_17=1/(1+0.6∙∛(D/(s-c))∙(b/D)∙δ_1)" +
                                        $"=1/(1+0.6∙∛({_saddleDataIn.D}/({_saddleDataIn.s}-{_saddleDataIn.c}))∙({_saddleDataIn.b}/{_saddleDataIn.D})∙{DegToRad(_saddleDataIn.delta1):f2})={_K17:f2}");

                    body.AddParagraph("Общее мембранное меридиональное напряжение изгиба от весовых нагрузок, действующее в области опорного узла");
                    body.AddParagraph("")
                        .AppendEquation("σ_mx=4∙M_i/(π∙D^2∙(s-c))" +
                                        $"=4∙{_M1}/(π∙{_saddleDataIn.D}^2∙({_saddleDataIn.s}-{_saddleDataIn.c}))={_sigma_mx:f2}");

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
                        .AppendEquation($"K_2={_K2}")
                        .AddRun(_saddleDataIn.IsAssembly
                        ? " - для условий испытания и монтажа"
                        : " - для рабочих условий");

                    body.AddParagraph("для ").AppendEquation("[σ_i]_2");
                    body.AddParagraph("")
                        .AppendEquation("ϑ_1=-(0,23∙K_13∙K_15)/(K_12∙K_10)" +
                                        $"={_v1_2:f2}");

                    body.AddParagraph("")
                        .AppendEquation("ϑ_(2,1)=- ̅σ_mx∙1/(K_2∙[σ])" +
                                        $"={_v21_2:f2}");
                    body.AddParagraph("")
                        .AppendEquation("ϑ_(2,2)=[(p∙D)/(4∙(s-c))- ̅σ_mx]∙1/(K_2∙[σ])" +
                                        $"={_v22_2:f2}");

                    body.AddParagraph("Для ")
                        .AppendEquation("ϑ_2")
                        .AddRun("принимают одно из значений ")
                        .AppendEquation("ϑ_(2,1)")
                        .AddRun(" или ")
                        .AppendEquation("ϑ_(2,2)")
                        .AddRun(", для которого предельное напряжение изгибабудет наименьшим.");

                    body.AddParagraph("")
                        .AppendEquation(_K1_2For_v21 < _K1_2For_v22
                        ? $"ϑ_2=ϑ_(2,1)={_v21_2:f2}"
                        : $"ϑ_2=ϑ_(2,2)={_v22_2:f2}");

                    body.AddParagraph("").AppendEquation($"K_1={_K1_2:f2}");

                    body.AddParagraph("")
                        .AppendEquation($"[σ_i]_2={_K1_2:f2}∙{_K2:f2}∙{_saddleDataIn.sigma_d}={_sigmai2:f2}");

                    body.AddParagraph("")
                        .AppendEquation($"[F]_2=(0.7∙{_sigmai2:f2}∙({_saddleDataIn.s}-{_saddleDataIn.c})∙√({_saddleDataIn.D}∙({_saddleDataIn.s}-{_saddleDataIn.c})))/({_K10:f2}∙{_K12:f2})={_F_d2:f2}");

                    body.AddParagraph("для ").AppendEquation("[σ_i]_3");
                    body.AddParagraph("")
                        .AppendEquation("ϑ_1=-(0,53∙K_11)/(K_14∙K_16∙K_17∙sin(0.5∙δ_1))" +
                                        $"={_v1_3:f2}");

                    body.AddParagraph("").AppendEquation("ϑ_(2,1)=0");
                    
                    body.AddParagraph("")
                        .AppendEquation("ϑ_(2,2)=(p∙D)/(2∙(s-c))∙1/(K_2∙[σ])" + $"={_v22_3:f2}");

                    body.AddParagraph("Для ")
                        .AppendEquation("ϑ_2")
                        .AddRun("принимают одно из значений ")
                        .AppendEquation("ϑ_(2,1)")
                        .AddRun(" или ")
                        .AppendEquation("ϑ_(2,2)")
                        .AddRun(", для которого предельное напряжение изгибабудет наименьшим.");

                    body.AddParagraph("").AppendEquation(_K1_3For_v21 < _K1_3For_v22
                        ? $"ϑ_2=ϑ_(2,1)={_v21_3:f2}"
                        : $"ϑ_2=ϑ_(2,2)={_v22_3:f2}");

                    body.AddParagraph("").AppendEquation($"K_1={_K1_3:f2}");

                    body.AddParagraph("")
                        .AppendEquation($"[σ_i]_3={_K1_3:f2}∙{_K2:f2}∙{_saddleDataIn.sigma_d}={_sigmai3:f2}");

                    body.AddParagraph("")
                        .AppendEquation($"[F]_3=(0.9∙{_sigmai2:f2}∙({_saddleDataIn.s}-{_saddleDataIn.c})∙√({_saddleDataIn.D}∙({_saddleDataIn.s}-{_saddleDataIn.c})))/({_K14:f2}∙{_K16:f2}∙{_K17:f2})={_F_d3:f2}");

                    body.AddParagraph("")
                        .AppendEquation($"{_F1:f2}≤min{{{_F_d2:f2};{_F_d3:f2}}}");

                    if (_F1 <= Math.Min(_F_d2, _F_d3))
                    {
                        body.AddParagraph("Условие прочности выполняется");
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
                        .AppendEquation("F_e=F_i∙π/4∙K_13∙K_15∙√(D/(s-c))" +
                                        $"={_F1:f2}∙π/4∙{_K13:f2}∙{_K15:f2}∙√({_saddleDataIn.D}/({_saddleDataIn.s}-{_saddleDataIn.c}))={_Fe:f2}");

                    body.AddParagraph("")
                        .AppendEquation(_saddleDataIn.IsPressureIn
                    ? "0"
                    : $"{_saddleDataIn.p}/{_p_d}" + 
                      $"+{_M1:f2}/{_M_d:f2}+{_Fe:f2}/{_F_d:f2}+({_Q1:f2}/{_Q_d:f2})^2={_conditionStability2:f2}≤1");

                    if (_conditionStability2 <= 1)
                    {
                        body.AddParagraph("Условие устойчивости выполняется");
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
                                        $"=({_saddleDataIn.s}-{_saddleDataIn.c})∙√(1+({_saddleDataIn.s2}/({_saddleDataIn.s}-{_saddleDataIn.c}))^2)={_sef:f2}");

                    body.AddParagraph("Параметр, определяемый расстоянием от середины опоры до днища");
                    body.AddParagraph("")
                        .AppendEquation("γ=2.83∙a/D∙√(s_ef/D)" +
                                        $"=2.83∙{_saddleDataIn.a}/{_saddleDataIn.D}∙√({_sef:f2})/{_saddleDataIn.D})={_gamma:f2}");

                    body.AddParagraph("Параметр, определяемый шириной пояса опоры");
                    body.AddParagraph("")
                        .AppendEquation("β_1=0.91∙b_2/√(D∙s_ef)" +
                                         $"=0.91∙{_saddleDataIn.b}/√({_saddleDataIn.D}∙{_sef:f2})={_beta1:f2}");

                    body.AddParagraph("Коэффициенты, учитывающие влияние ширины пояса опоры");
                    body.AddParagraph("")
                        .AppendEquation("K_10=max{(exp(-β_1)∙sin(β_1))/β_1;0.25}" +
                                        $"=max{{(exp(-{_beta1:f2})∙sin({_beta1:f2}))/{_beta1:f2};0.25}}" +
                                        $"=max({_K10_1:f2};0.25)={_K10:f2}");

                    body.AddParagraph("")
                        .AppendEquation("K_11=(1-exp(-β_1)∙cos(β_1))/β_1" +
                                        $"=(1-exp(-{_beta1:f2})∙cos({_beta1:f2}))/{_beta1:f2}={_K11:f2}");

                    body.AddParagraph("Коэффициенты, учитывающие влияние угла охвата");
                    body.AddParagraph("")
                        .AppendEquation("K_12=(1.15-0.1432∙δ_2)/sin(0.5∙δ_2)" +
                                        $"=(1.15-0.1432∙{DegToRad(_saddleDataIn.delta2):f2})/sin(0.5∙{DegToRad(_saddleDataIn.delta2):f2})={_K12:f2}");

                    body.AddParagraph("")
                        .AppendEquation("K_13=(max{1.7-(2.1∙δ_2)/π;0})/sin(0.5∙δ_2)" +
                                        $"=(max{{1.7 - (2.1∙{DegToRad(_saddleDataIn.delta2):f2})/π;0}})/sin(0.5∙{DegToRad(_saddleDataIn.delta2):f2})={_K13:f2}");

                    body.AddParagraph("")
                        .AppendEquation("K_14=(1.45-0.43∙δ_2)/sin(0.5∙δ_2)" +
                                        $"=(1.45-0.43∙{DegToRad(_saddleDataIn.delta2):f2})/sin(0.5∙{DegToRad(_saddleDataIn.delta2):f2})={_K14:f2}");

                    body.AddParagraph("Коэффициенты, учитывающие влияние расстояния от середины опоры до днища и угла охвата");
                    body.AddParagraph("")
                        .AppendEquation("K_15=min{1.0;(0.8∙√γ+6∙γ)/δ_2}" +
                                        $"min{{1.0;(0.8∙√{_gamma:f2}+6∙{_gamma:f2})/{DegToRad(_saddleDataIn.delta2):f2}}}=min{{1.0;{_K15_2:f2}}}={_K15:f2}");

                    body.AddParagraph("")
                        .AppendEquation("K_16=1-0.65/(1+(6∙γ)^2)∙√(π/(3∙δ_2))"
                                        + $"=1-0.65/(1+(6∙{_gamma:f2})^2)∙√(π/(3∙{DegToRad(_saddleDataIn.delta2):f2}))={_K16:f2}");

                    body.AddParagraph("Коэффициенты, учитывающие влияние ширины пояса опоры и угла охвата");
                    body.AddParagraph("")
                        .AppendEquation("K_17=1/(1+0.6∙∛(D/s_ef)∙(b_2/D)∙δ_2)" +
                                        $"=1/(1+0.6∙∛({_saddleDataIn.D}/{_sef:f2})∙({_saddleDataIn.b2}/{_saddleDataIn.D})∙{DegToRad(_saddleDataIn.delta2):f2})={_K17:f2}");

                    body.AddParagraph("Общее мембранное меридиональное напряжение изгиба от весовых нагрузок, действующее в области опорного узла");
                    body.AddParagraph("")
                        .AppendEquation("σ_mx=4∙M_i/(π∙D^2∙s_ef)" +
                                        $"=4∙{_M1}/(π∙{_saddleDataIn.D}^2∙{_sef:f2})={_sigma_mx:f2}");

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
                        .AppendEquation($"K_2={_K2}")
                        .AddRun(_saddleDataIn.IsAssembly
                        ? " - для условий испытания и монтажа"
                        : " - для рабочих условий");

                    body.AddParagraph("для ").AppendEquation("[σ_i]_2");
                    body.AddParagraph("")
                        .AppendEquation("ϑ_1=-(0,23∙K_13∙K_15)/(K_12∙K_10)" +
                                        $"={_v1_2:f2}");

                    body.AddParagraph("")
                        .AppendEquation("ϑ_(2,1)=- ̅σ_mx∙1/(K_2∙[σ])" +
                                        $"={_v21_2:f2}");
                    body.AddParagraph("")
                        .AppendEquation("ϑ_(2,2)=[(p∙D)/(4∙s_ef)- ̅σ_mx]∙1/(K_2∙[σ])" +
                                        $"={_v22_2:f2}");

                    body.AddParagraph("Для ")
                        .AppendEquation("ϑ_2")
                        .AddRun("принимают одно из значений ")
                        .AppendEquation("ϑ_(2,1)")
                        .AddRun(" или ")
                        .AppendEquation("ϑ_(2,2)")
                        .AddRun(", для которого предельное напряжение изгибабудет наименьшим.");

                    body.AddParagraph("")
                        .AppendEquation(_K1_2For_v21 < _K1_2For_v22
                        ? $"ϑ_2=ϑ_(2,1)={_v21_2:f2}"
                        : $"ϑ_2=ϑ_(2,2)={_v22_2:f2}");

                    body.AddParagraph("").AppendEquation($"K_1={_K1_2:f2}");

                    body.AddParagraph("")
                        .AppendEquation($"[σ_i]_2={_K1_2:f2}∙{_K2:f2}∙{_saddleDataIn.sigma_d}={_sigmai2:f2}");

                    body.AddParagraph("")
                        .AppendEquation($"[F]_2=(0.7∙{_sigmai2:f2}∙{_sef:f2}∙√({_saddleDataIn.D}∙{_sef:f2}))/({_K10:f2}∙{_K12:f2})={_F_d2:f2}");

                    body.AddParagraph("для ").AppendEquation("[σ_i]_3");
                    body.AddParagraph("")
                        .AppendEquation("ϑ_1=-(0,53∙K_11)/(K_14∙K_16∙K_17∙sin(0.5∙δ_2))" +
                                        $"={_v1_3:f2}");

                    body.AddParagraph("").AppendEquation("ϑ_(2,1)=0");

                    body.AddParagraph("")
                        .AppendEquation("ϑ_(2,2)=(p∙D)/(2∙s_ef)∙1/(K_2∙[σ])" + $"={_v22_3:f2}");

                    body.AddParagraph("Для ")
                        .AppendEquation("ϑ_2")
                        .AddRun("принимают одно из значений ")
                        .AppendEquation("ϑ_(2,1)")
                        .AddRun(" или ")
                        .AppendEquation("ϑ_(2,2)")
                        .AddRun(", для которого предельное напряжение изгибабудет наименьшим.");

                    body.AddParagraph("").AppendEquation(_K1_3For_v21 < _K1_3For_v22
                        ? $"ϑ_2=ϑ_(2,1)={_v21_3:f2}"
                        : $"ϑ_2=ϑ_(2,2)={_v22_3:f2}");

                    body.AddParagraph("").AppendEquation($"K_1={_K1_3:f2}");

                    body.AddParagraph("")
                        .AppendEquation($"[σ_i]_3={_K1_3:f2}∙{_K2:f2}∙{_saddleDataIn.sigma_d}={_sigmai3:f2}");

                    body.AddParagraph("")
                        .AppendEquation($"[F]_3=(0.9∙{_sigmai2:f2}∙{_sef:f2}∙√({_saddleDataIn.D}∙{_sef:f2}))/({_K14:f2}∙{_K16:f2}∙{_K17:f2})={_F_d3:f2}");

                    body.AddParagraph("")
                        .AppendEquation($"{_F1:f2}≤min{{{_F_d2:f2};{_F_d3:f2}}}");

                    if (_F1 <= Math.Min(_F_d2, _F_d3))
                    {
                        body.AddParagraph("Условие прочности выполняется");
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
                                        $"={_F1:f2}∙π/4∙{_K13:f2}∙{_K15:f2}∙√({_saddleDataIn.D}/{_sef:f2})={_Fe:f2}");

                    body.AddParagraph("")
                        .AppendEquation(_saddleDataIn.IsPressureIn
                    ? "0"
                    : $"{_saddleDataIn.p}/{_p_d}" +
                        $"+{_M1:f2}/{_M_d:f2}+{_Fe:f2}/{_F_d:f2}+({_Q1:f2}/{_Q_d:f2})^2={_conditionStability2:f2}≤1");

                    if (_conditionStability2 <= 1)
                    {
                        body.AddParagraph("Условие устойчивости выполняется");
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

            if (IsConditionUseFormulas)
            {
                body.AddParagraph("Условия применения формул");
            }
            else
            {
                body.AddParagraph("Условия применения формул не выполняются").Bold().Color(System.Drawing.Color.Red);
            }
            body.AddParagraph("")
                .AppendEquation($"60°≤δ_1={_saddleDataIn.delta1}°≤180°");
            body.AddParagraph("")
                .AppendEquation($"(s-c)/D=({_saddleDataIn.s}-{_saddleDataIn.c})/{_saddleDataIn.D}={(_saddleDataIn.s - _saddleDataIn.c) / _saddleDataIn.D:f2}≤0.05");
            if (_saddleDataIn.Type == SaddleType.SaddleWithoutRingWithSheet)
            {
                body.AddParagraph("").AppendEquation("s_2≥s");
                body.AddParagraph("").AppendEquation($"{_saddleDataIn.s2} мм ≥ {_saddleDataIn.s} мм");
                body.AddParagraph("").AppendEquation("δ_2≥δ_1+20°");
                body.AddParagraph("")
                    .AppendEquation(
                        $"{_saddleDataIn.delta2}°≥{_saddleDataIn.delta1}°+20°={_saddleDataIn.delta1 + 20}°");
            }

            if (_saddleDataIn.Type == SaddleType.SaddleWithRing)
            {
                body.AddParagraph("").AppendEquation("A_k≥(s-c)√(D∙(s-c))");
                body.AddParagraph("").AppendEquation($"{_saddleDataIn.Ak:f2}≥({_saddleDataIn.s}-{_saddleDataIn.c})√({_saddleDataIn.D}∙({_saddleDataIn.s}-{_saddleDataIn.c}))={_Ak:f2}");
            }

            package.Close();
        }

    }
}
