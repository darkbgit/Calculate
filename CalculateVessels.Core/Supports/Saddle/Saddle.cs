using CalculateVessels.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalculateVessels.Core.Supports.Enums;

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
        private double _AkForCUF;

        

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
            // UNDONE: проверить формулу для расчета [F]
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
                    _AkForCUF = (_saddleDataIn.s - _saddleDataIn.c) * Math.Sqrt(_saddleDataIn.D * (_saddleDataIn.s - _saddleDataIn.c));
                    if (_saddleDataIn.Ak < _AkForCUF)
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
            //var doc = Xceed.Words.NET.DocX.Load(Docum);
            //doc.InsertParagraph().InsertPageBreakAfterSelf();
            //doc.InsertParagraph($"Расчет на прочность обечайки {_saddleDataIn.nameob} от воздействия опорных нагрузок").Heading(HeadingType.Heading1).Alignment = Alignment.center;
            //doc.InsertParagraph();


            //Xceed.Document.NET.Image image;
            //Xceed.Document.NET.Picture picture;

            //if (_saddleDataIn.type == 1)
            //{
            //    image = doc.AddImage("pic/Saddle/SaddleNothingElem.gif");
            //    picture = image.CreatePicture();
            //    doc.InsertParagraph().AppendPicture(picture);
            //}
            //else if (_saddleDataIn.type == 2)
            //{
            //    image = doc.AddImage("pic/Saddle/SaddleSheetElem.gif");
            //    picture = image.CreatePicture();
            //    doc.InsertParagraph().AppendPicture(picture);
            //}

            //doc.InsertParagraph("Исходные данные").Alignment = Alignment.center;

            //var table = doc.AddTable(1, 2);
            //table.SetWidths(new float[] { 300, 100 });
            ////int i = 0;
            ////table.InsertRow(i);
            //table.Rows[0].Cells[0].Paragraphs[0].Append("Внутренний диаметр обечайки, D:");
            //table.Rows[0].Cells[1].Paragraphs[0].Append($"{_saddleDataIn.D} мм");
            //int i = 1;
            //table.InsertRow(i);
            //table.Rows[i].Cells[0].Paragraphs[0].Append("Толщина стенки обечайки, s:");
            //table.Rows[i].Cells[1].Paragraphs[0].Append($"{_saddleDataIn.s} мм");
            //i++;
            //table.InsertRow(i);
            //table.Rows[i].Cells[0].Paragraphs[0].Append("Прибавка к расчетной толщине, c:");
            //table.Rows[i].Cells[1].Paragraphs[0].Append($"{_saddleDataIn.c} мм");
            //i++;
            //table.InsertRow(i);
            //table.Rows[i].Cells[0].Paragraphs[0].Append("Длина обечайки, Lob:");
            //table.Rows[i].Cells[1].Paragraphs[0].Append($"{_saddleDataIn.Lob} мм");
            //i++;
            //table.InsertRow(i);
            //table.Rows[i].Cells[0].Paragraphs[0].Append("Коэффициент прочности сварного шва, φ:");
            //table.Rows[i].Cells[1].Paragraphs[0].Append($"{_saddleDataIn.fi}");
            //i++;
            //table.InsertRow(i);
            //table.Rows[i].Cells[0].Paragraphs[0].Append("Марка стали");
            //table.Rows[i].Cells[1].Paragraphs[0].Append($"{_saddleDataIn.steel}");
            //i++;
            //table.InsertRow(i);
            //table.Rows[i].Cells[0].Paragraphs[0].Append("Собственный вес с содержимым, G:");
            //table.Rows[i].Cells[1].Paragraphs[0].Append($"{_saddleDataIn.G} H");
            //i++;
            //table.InsertRow(i);
            //table.Rows[i].Cells[0].Paragraphs[0].Append("Ширина опоры, b:");
            //table.Rows[i].Cells[1].Paragraphs[0].Append($"{_saddleDataIn.b} мм");
            //i++;
            //table.InsertRow(i);
            //table.Rows[i].Cells[0].Paragraphs[0].Append("Угол охвата опоры, ").AppendEquation("δ_1");
            //table.Rows[i].Cells[1].Paragraphs[0].Append($"{_saddleDataIn.delta1} °");
            //i++;
            //table.InsertRow(i);
            //table.Rows[i].Cells[0].Paragraphs[0].Append("Длина свободно выступающей части, e:");
            //table.Rows[i].Cells[1].Paragraphs[0].Append($"{_saddleDataIn.e} мм");
            //i++;
            //table.InsertRow(i);
            //table.Rows[i].Cells[0].Paragraphs[0].Append("Длина выступающей цилиндрической части сосуда, включая отбортовку днища, a");
            //table.Rows[i].Cells[1].Paragraphs[0].Append($"{_saddleDataIn.a} мм");
            //i++;
            //table.InsertRow(i);
            //table.Rows[i].Cells[0].Paragraphs[0].Append("Высота опоры, Н");
            //table.Rows[i].Cells[1].Paragraphs[0].Append($"{_saddleDataIn.H} мм");
            //if (_saddleDataIn.type == 2)
            //{
            //    i++;
            //    table.InsertRow(i);
            //    table.Rows[i].Cells[0].Paragraphs[0].Append("Толщина подкладного листа, ").AppendEquation("s_2");
            //    table.Rows[i].Cells[1].Paragraphs[0].Append($"{_saddleDataIn.s2} мм");
            //    i++;
            //    table.InsertRow(i);
            //    table.Rows[i].Cells[0].Paragraphs[0].Append("Ширина подкладного листа, мм").AppendEquation("b_2");
            //    table.Rows[i].Cells[1].Paragraphs[0].Append($"{_saddleDataIn.b2} мм");
            //    i++;
            //    table.InsertRow(i);
            //    table.Rows[i].Cells[0].Paragraphs[0].Append("Угол охвата подкладного листа, ").AppendEquation("δ_2");
            //    table.Rows[i].Cells[1].Paragraphs[0].Append($"{_saddleDataIn.delta2} °");
            //}

            //doc.InsertParagraph().InsertTableAfterSelf(table);

            //doc.InsertParagraph();
            //doc.InsertParagraph("Условия нагружения").Alignment = Alignment.center;
            //var table1 = doc.AddTable(1, 2);
            //table1.SetWidths(new float[] { 300, 100 });

            //table1.Rows[0].Cells[0].Paragraphs[0].Append("Расчетная температура, Т:");
            //table1.Rows[0].Cells[1].Paragraphs[0].Append($"{_saddleDataIn.temp} °С");
            //i = 1;
            //table1.InsertRow(i);
            //if (_saddleDataIn.isPressureIn)
            //{
            //    table1.Rows[i].Cells[0].Paragraphs[0].Append("Расчетное внутреннее избыточное давление, p:");
            //}
            //else
            //{
            //    table1.Rows[i].Cells[0].Paragraphs[0].Append("Расчетное наружное давление, p:");
            //}
            //table1.Rows[i].Cells[1].Paragraphs[0].Append($"{_saddleDataIn.p} МПа");
            //i++;
            //table1.InsertRow(i);
            //table1.Rows[i].Cells[0].Paragraphs[0].Append($"Допускаемое напряжение для материала {_saddleDataIn.steel} при расчетной температуре, [σ]:");
            //table1.Rows[i].Cells[1].Paragraphs[0].Append($"{_saddleDataIn.sigma_d} МПа");
            //i++;
            //if (!_saddleDataIn.isPressureIn)
            //{
            //    table1.InsertRow(i);
            //    table1.Rows[i].Cells[0].Paragraphs[0].Append("Модуль продольной упругости при расчетной температуре, E:");
            //    table1.Rows[i].Cells[1].Paragraphs[0].Append($"{_saddleDataIn.E} МПа");
            //}

            //doc.InsertParagraph().InsertTableAfterSelf(table1);

            //doc.InsertParagraph();
            //doc.InsertParagraph("Результаты расчета").Alignment = Alignment.center;
            //doc.InsertParagraph();


            //doc.InsertParagraph("Расчетные параметры").Alignment = Alignment.center;
            //doc.InsertParagraph();

            //doc.InsertParagraph("Распределенная весовая нагрузка");
            //doc.InsertParagraph().AppendEquation("q=G/(L+4/3∙H)");
            //doc.InsertParagraph().AppendEquation($"q={_saddleDataIn.G}/({_saddleDataIn.L}+4/3∙{_saddleDataIn.H})={_q:f2} Н/мм");

            //doc.InsertParagraph("Расчетный изгибающий момент, действующий на консольную часть обечайки");
            //doc.InsertParagraph().AppendEquation("M_0=q∙D^2/16");
            //doc.InsertParagraph().AppendEquation($"M_0={_q:f2}∙{_saddleDataIn.D}^2/16={_M0:f2} Н∙мм");

            //doc.InsertParagraph("Опорное усилие");
            //doc.InsertParagraph().AppendEquation("F_1=F_2=G/2");
            //doc.InsertParagraph().AppendEquation($"F_1=F_2={_saddleDataIn.G}/2={_F1:f2} H");

            //doc.InsertParagraph("Изгибающий момент над опорами");
            //doc.InsertParagraph().AppendEquation("M_1=M_2=(q∙e^2)/2-M_0");
            //doc.InsertParagraph().AppendEquation($"M_1=M_2=({_q:f2}∙{_saddleDataIn.e:f2}^2)/2-{_M0:f2}={_M1:f2} Н∙мм");

            //doc.InsertParagraph("Максимальный изгибающий момент между опорами");
            //doc.InsertParagraph().AppendEquation("M_12=M_0+F_1∙(L/2-a)-q/2∙(L/2+2/3∙H)^2");
            //doc.InsertParagraph().AppendEquation($"M_12={_M0:f2}+{_F1:f2}∙({_saddleDataIn.L}/2-{_saddleDataIn.a})-{_q:f2}/2∙({_saddleDataIn.L}/2+2/3∙{_saddleDataIn.H})^2={_M12:f2} Н∙мм");

            //doc.InsertParagraph("Поперечное усилие в сечении оболочки над опорой");
            //doc.InsertParagraph().AppendEquation("Q_1=Q_2=(L-2∙a)/(L+4/3∙H)∙F_1");
            //doc.InsertParagraph().AppendEquation($"Q_1=Q_2=({_saddleDataIn.L}-2∙{_saddleDataIn.a})/({_saddleDataIn.L}+4/3∙{_saddleDataIn.H})∙{_F1:f2}={_Q1:f2} H");

            //doc.InsertParagraph("Несущую способность обечайки в сечении между опорами следует проверять при условии");
            //doc.InsertParagraph().AppendEquation("max{M_12}>max{M_1}");
            //doc.InsertParagraph().AppendEquation($"{_M12:f2} Н∙мм > {_M1:f2} Н∙мм");
            //if (_M12 > _M1)
            //{
            //    doc.InsertParagraph("Проверка несущей способности обечайки в сечении между опорами");
            //    doc.InsertParagraph("Условие прочности");
            //    doc.InsertParagraph().AppendEquation("(p∙D)/(4∙(s-c))+(4∙M_12∙K_9)/(π∙D^2∙(s-c))≤[σ]∙φ");
            //    doc.InsertParagraph("где ").AppendEquation("K_9").Append(" - коэффициент, учитывающий частичное заполнение жидкостью");
            //    doc.InsertParagraph().AppendEquation("K_9=max{[1.6-0.20924∙(x-1)+0.028702∙x∙(x-1)+0.4795∙10^3∙y∙(x-1)-0.2391∙10^-6∙x∙y∙(x-1)-0.29936∙10^-2∙(x-1)∙x^2-0.85692∙10^-6∙(х-1)∙у^2+0.88174∙10^-6∙х^2∙(х-1)∙у-0.75955∙10^-8∙у^2∙(х-1)∙х+0.82748∙10^-4∙(х-1)∙х^3+0.48168∙10^-9∙(х-1)∙у^3];1}");
            //    doc.InsertParagraph("где ").AppendEquation("y=D/(s-c);x=L/D");
            //    doc.InsertParagraph().AppendEquation($"y={_saddleDataIn.D}/({_saddleDataIn.s}-{_saddleDataIn.c})={_y:f2}");
            //    doc.InsertParagraph().AppendEquation($"x={_saddleDataIn.L}/{_saddleDataIn.D}={_x:f2}");

            //    doc.InsertParagraph().AppendEquation($"K_9=max({_K9:f2};1)={_K9:f2}");

            //    doc.InsertParagraph().AppendEquation($"(p∙D)/(4∙(s-c))+(4∙M_12∙K_9)/(π∙D^2∙(s-c))=({_saddleDataIn.p}∙{_saddleDataIn.D})/(4∙({_saddleDataIn.s}-{_saddleDataIn.c}))+(4∙{_M12:f2}∙{_K9:f2})/(π∙{_saddleDataIn.D}^2∙({_saddleDataIn.s}-{_saddleDataIn.c}))={_yslproch1_1:f2}");
            //    doc.InsertParagraph().AppendEquation($"[σ]∙φ={_saddleDataIn.sigma_d}∙{_saddleDataIn.fi}={_yslproch1_2:f2}");
            //    doc.InsertParagraph().AppendEquation($"{_yslproch1_1:f2}≤{_yslproch1_2:f2}");
            //    if (_yslproch1_1 <= _yslproch1_2)
            //    {
            //        doc.InsertParagraph("Условие прочности выполняется");
            //    }
            //    else
            //    {
            //        doc.InsertParagraph("Условие прочности не выполняется").Bold().Color(System.Drawing.Color.Red);
            //    }
            //    doc.InsertParagraph("Условие устойчивости");
            //    doc.InsertParagraph().AppendEquation("|M_12|/[M]≤1");

            //    doc.InsertParagraph("где [M] - допускаемый изгибающий момент из условия устойчивости");
            //    doc.InsertParagraph().AppendEquation("[M]=(8.9∙10^-5∙E)/n_y∙D^3∙[(100∙(s-c))/D]^2.5");

            //    doc.InsertParagraph().AppendEquation($"[M]=(8.9∙10^-5∙{_saddleDataIn.E})/{_saddleDataIn.ny}∙{_saddleDataIn.D}^3∙[(100∙({_saddleDataIn.s}-{_saddleDataIn.c}))/{_saddleDataIn.D}]^2.5={_M_d:f2} Н∙мм");
            //    doc.InsertParagraph().AppendEquation($"|{_M12:f2}|/{_M_d:f2}={_yslystoich1:f2}≤1");

            //    if (_yslystoich1 <= 1)
            //    {
            //        doc.InsertParagraph("Условие устойчивости выполняется");
            //    }
            //    else
            //    {
            //        doc.InsertParagraph("Условие устойчивости не выполняется").Bold().Color(System.Drawing.Color.Red);
            //    }
            //}
            //else
            //{
            //    doc.InsertParagraph("Проверка несущей способности обечайки в сечении между опорами не требуется");
            //}

            //if (_saddleDataIn.type == 1)
            //{
            //    doc.InsertParagraph("Проверка несущей способности обечайки, не укрепленной кольцами жесткости в области опорного узла и без подкладного листа в месте опоры");
            //    doc.InsertParagraph("Вспомогательные параметры и коэффициенты");
            //    doc.InsertParagraph("Параметр, определяемый расстоянием от середины опоры до днища");
            //    doc.InsertParagraph().AppendEquation("γ=2.83∙a/D∙√((s-c)/D)");
            //    doc.InsertParagraph().AppendEquation($"γ={_gamma:f2}");

            //    doc.InsertParagraph("Параметр, определяемый шириной пояса опоры");
            //    doc.InsertParagraph().AppendEquation("β_1=0.91∙b/√(D∙(s-c))");
            //    doc.InsertParagraph().AppendEquation($"β_1={_beta1:f2}");

            //    doc.InsertParagraph("Коэффициенты, учитывающие влияние ширины пояса опоры");
            //    doc.InsertParagraph().AppendEquation("K_10=max{(exp(-β_1)∙sin(β_1))/β_1;0.25}");
            //    doc.InsertParagraph().AppendEquation($"K_10=max({_K10_1:f2};0.25)={_K10:f2}");

            //    doc.InsertParagraph().AppendEquation("K_11=(1-exp(-β_1)∙cos(β_1))/β_1");
            //    doc.InsertParagraph().AppendEquation($"K_11={_K11:f2}");

            //    doc.InsertParagraph("Коэффициенты, учитывающие влияние угла охвата");
            //    doc.InsertParagraph().AppendEquation("K_12=(1.15-0.1432∙δ_1)/sin(0.5∙δ_1)");
            //    doc.InsertParagraph().AppendEquation($"K_12={_K12:f2}");

            //    doc.InsertParagraph().AppendEquation("K_13=(max{1.7-(2.1∙δ_1)/π;0})/sin(0.5∙δ_1)");
            //    doc.InsertParagraph().AppendEquation($"K_13={_K13:f2}");

            //    doc.InsertParagraph().AppendEquation("K_14=(1.45-0.43∙δ_1)/sin(0.5∙δ_1)");
            //    doc.InsertParagraph().AppendEquation($"K_14={_K14:f2}");

            //    doc.InsertParagraph("Коэффициенты, учитывающие влияние расстояния от середины опоры до днища и угла охвата");
            //    doc.InsertParagraph().AppendEquation("K_15=min{1.0;(0.8∙√γ+6∙γ)/δ_1}");
            //    doc.InsertParagraph().AppendEquation($"K_15=min(1.0;{_K15_2:f2})={_K15:f2}");

            //    doc.InsertParagraph().AppendEquation("K_16=1-0.65/(1+(6∙γ)^2)∙√(π/(3∙δ_1))");
            //    doc.InsertParagraph().AppendEquation($"K_16={_K16:f2}");

            //    doc.InsertParagraph("Коэффициенты, учитывающие влияние ширины пояса опоры и угла охвата");
            //    doc.InsertParagraph().AppendEquation("K_17=1/(1+0.6∙∛(D/(s-c))∙(b/D)∙δ_1)");
            //    doc.InsertParagraph().AppendEquation($"K_17={_K17:f2}");

            //    doc.InsertParagraph("Общее мембранное меридиональное напряжение изгиба от весовых нагрузок, действующее в области опорного узла");
            //    doc.InsertParagraph().AppendEquation("σ ̅_mx=4∙M_i/(π∙D^2∙(s-c))");
            //    doc.InsertParagraph().AppendEquation($"σ ̅_mx={_sigma_mx:f2}");

            //    doc.InsertParagraph("Условие прочности");
            //    doc.InsertParagraph().AppendEquation("F_1≤min{[F]_2;[F]_3}");
            //    doc.InsertParagraph("где ").AppendEquation("[F]_2").Append(" - допускаемое опорное усилие от нагружения в меридиональном направлении");
            //    doc.InsertParagraph().AppendEquation("[F]_2=(0.7∙[σ_i]_2∙(s-c)∙√(D∙(s-c)))/(K_10∙K_12)");

            //    doc.InsertParagraph("\t").AppendEquation("[F]_3").Append(" - допускаемое опорное усилие от нагружения в окружном направлении");
            //    doc.InsertParagraph().AppendEquation("[F]_3=(0.9∙[σ_i]_3∙(s-c)∙√(D∙(s-c)))/(K_14∙K_16∙K_17)");

            //    doc.InsertParagraph("где ").AppendEquation("[σ_i]_2, [σ_i]_2").Append(" - предельные напряжения изгиба в меридиональном и окружном направлениях");

            //    doc.InsertParagraph().AppendEquation("[σ_i]=K_1∙K_2∙[σ]");

            //    doc.InsertParagraph().AppendEquation("K_1=(1-ϑ_2^2)/((1/3+ϑ_1∙ϑ_2)+√((1/3+ϑ_1∙ϑ_2)^2+(1-ϑ_2^2)∙ϑ_1^2))");

            //    // TODO: для условий монтажа 
            //    doc.InsertParagraph().AppendEquation("K_2=1.25").Append(" - для рабочих условий");

            //    doc.InsertParagraph("для ").AppendEquation("[σ_i]_2");
            //    doc.InsertParagraph().AppendEquation("ϑ_1=-(0,23∙K_13∙K_15)/(K_12∙K_10)");
            //    doc.InsertParagraph().AppendEquation($"ϑ_1={_v1_2:f2}");

            //    doc.InsertParagraph().AppendEquation("ϑ_(2,1)=- ̅σ_mx∙1/(K_2∙[σ])");
            //    doc.InsertParagraph().AppendEquation($"ϑ_(2,1)={_v21_2:f2}");
            //    doc.InsertParagraph().AppendEquation("ϑ_(2,2)=[(p∙D)/(4∙(s-c))- ̅σ_mx]∙1/(K_2∙[σ])");
            //    doc.InsertParagraph().AppendEquation($"ϑ_(2,2)={_v22_2:f2}");

            //    doc.InsertParagraph("Для ").AppendEquation("ϑ_2").Append("принимают одно из значений ").AppendEquation("ϑ_(2,1)").Append(" или ").AppendEquation("ϑ_(2,2)").Append(", для которого предельное напряжение изгибабудет наименьшим.");
            //    if (_K1_2For_v21 < _K1_2For_v22)
            //    {
            //        doc.InsertParagraph().AppendEquation($"ϑ_2=ϑ_(2,1)={_v21_2:f2}");
            //    }
            //    else
            //    {
            //        doc.InsertParagraph().AppendEquation($"ϑ_2=ϑ_(2,2)={_v22_2:f2}");
            //    }

            //    doc.InsertParagraph().AppendEquation($"K_1={_K1_2:f2}");

            //    doc.InsertParagraph().AppendEquation($"[σ_i]_2={_K1_2:f2}∙{_K2:f2}∙{_saddleDataIn.sigma_d}={_sigmai2:f2}");

            //    doc.InsertParagraph().AppendEquation($"[F]_2=(0.7∙{_sigmai2:f2}∙({_saddleDataIn.s}-{_saddleDataIn.c})∙√({_saddleDataIn.D}∙({_saddleDataIn.s}-{_saddleDataIn.c})))/({_K10:f2}∙{_K12:f2})={_F_d2:f2}");


            //    doc.InsertParagraph("для ").AppendEquation("[σ_i]_3");
            //    doc.InsertParagraph().AppendEquation("ϑ_1=-(0,53∙K_11)/(K_14∙K_16∙K_17∙sin(0.5∙δ_1))");
            //    doc.InsertParagraph().AppendEquation($"ϑ_1={_v1_3:f2}");

            //    doc.InsertParagraph().AppendEquation("ϑ_(2,1)=0");
            //    doc.InsertParagraph().AppendEquation($"ϑ_(2,1)={_v21_3:f2}");
            //    doc.InsertParagraph().AppendEquation("ϑ_(2,2)=(p∙D)/(2∙(s-c))∙1/(K_2∙[σ])");
            //    doc.InsertParagraph().AppendEquation($"ϑ_(2,2)={_v22_3:f2}");

            //    doc.InsertParagraph("Для ").AppendEquation("ϑ_2").Append("принимают одно из значений ").AppendEquation("ϑ_(2,1)").Append(" или ").AppendEquation("ϑ_(2,2)").Append(", для которого предельное напряжение изгибабудет наименьшим.");
            //    if (_K1_3For_v21 < _K1_3For_v22)
            //    {
            //        doc.InsertParagraph().AppendEquation($"ϑ_2=ϑ_(2,1)={_v21_3:f2}");
            //    }
            //    else
            //    {
            //        doc.InsertParagraph().AppendEquation($"ϑ_2=ϑ_(2,2)={_v22_3:f2}");
            //    }

            //    doc.InsertParagraph().AppendEquation($"K_1={_K1_3:f2}");

            //    doc.InsertParagraph().AppendEquation($"[σ_i]_3={_K1_3:f2}∙{_K2:f2}∙{_saddleDataIn.sigma_d}={_sigmai3:f2}");

            //    doc.InsertParagraph().AppendEquation($"[F]_3=(0.9∙{_sigmai2:f2}∙({_saddleDataIn.s}-{_saddleDataIn.c})∙√({_saddleDataIn.D}∙({_saddleDataIn.s}-{_saddleDataIn.c})))/({_K14:f2}∙{_K16:f2}∙{_K17:f2})={_F_d3:f2}");

            //    doc.InsertParagraph().AppendEquation($"{_F1:f2}≤min({_F_d2:f2};{_F_d3:f2})");
            //    if (_F1 <= Math.Min(_F_d2, _F_d3))
            //    {
            //        doc.InsertParagraph("Условие прочности выполняется");
            //    }
            //    else
            //    {
            //        doc.InsertParagraph("Условие прочности не выполняется").Bold().Color(System.Drawing.Color.Red);
            //    }

            //    doc.InsertParagraph("Условие устойчивости");
            //    // TODO: добавить наружное давление
            //    doc.InsertParagraph().AppendEquation("|p|/[p]+|M_i|/[M]+|F_e|/[F]+(Q_i/[Q])^2≤1");

            //    doc.InsertParagraph("где ").AppendEquation("F_e").Append(" - эффективное осевое усилие от местных мембранных напряжений, действующих в области опоры");
            //    doc.InsertParagraph().AppendEquation("F_e=F_i∙π/4∙K_13∙K_15∙√(D/(s-c))");
            //    doc.InsertParagraph().AppendEquation($"F_e={_F1:f2}∙π/4∙{_K13:f2}∙{_K15:f2}∙√({_saddleDataIn.D}/({_saddleDataIn.s}-{_saddleDataIn.c}))={_Fe:f2}");
            //    doc.InsertParagraph().AppendEquation($"{_M1:f2}/{_M_d:f2}+{_Fe:f2}/{_F_d:f2}+({_Q1:f2}/{_Q_d:f2})^2={_yslystoich2:f2}≤1");

            //    if (_yslystoich2 <= 1)
            //    {
            //        doc.InsertParagraph("Условие устойчивости выполняется");
            //    }
            //    else
            //    {
            //        doc.InsertParagraph("Условие устойчивости не выполняется").Bold().Color(System.Drawing.Color.Red);
            //    }
            //}
            //else if (_saddleDataIn.type == 2)
            //{
            //    doc.InsertParagraph("Проверка несущей способности обечайки, не укрепленной кольцами жесткости в области опорного узла с подкладным листом в месте опоры");
            //    doc.InsertParagraph("Вспомогательные параметры и коэффициенты");

            //    doc.InsertParagraph().AppendEquation("s_ef=(s-c)∙√(1+(s_2/(s-c))^2)");
            //    doc.InsertParagraph().AppendEquation($"s_ef={_sef:f2}");

            //    doc.InsertParagraph("Параметр, определяемый расстоянием от середины опоры до днища");
            //    doc.InsertParagraph().AppendEquation("γ=2.83∙a/D∙√(s_ef)/D");
            //    doc.InsertParagraph().AppendEquation($"γ={_gamma:f2}");

            //    doc.InsertParagraph("Параметр, определяемый шириной пояса опоры");
            //    doc.InsertParagraph().AppendEquation("β_1=0.91∙b_2/√(D∙(s_ef))");
            //    doc.InsertParagraph().AppendEquation($"β_1={_beta1:f2}");

            //    doc.InsertParagraph("Коэффициенты, учитывающие влияние ширины пояса опоры");
            //    doc.InsertParagraph().AppendEquation("K_10=max{(exp(-β_1)∙sin(β_1))/β_1;0.25}");
            //    doc.InsertParagraph().AppendEquation($"K_10=max({_K10_1:f2};0.25)={_K10:f2}");

            //    doc.InsertParagraph().AppendEquation("K_11=(1-exp(-β_1)∙cos(β_1))/β_1");
            //    doc.InsertParagraph().AppendEquation($"K_11={_K11:f2}");

            //    doc.InsertParagraph("Коэффициенты, учитывающие влияние угла охвата");
            //    doc.InsertParagraph().AppendEquation("K_12=(1.15-0.1432∙δ_1)/sin(0.5∙δ_1)");
            //    doc.InsertParagraph().AppendEquation($"K_12={_K12:f2}");

            //    doc.InsertParagraph().AppendEquation("K_13=(max{1.7-(2.1∙δ_1)/π;0})/sin(0.5∙δ_1)");
            //    doc.InsertParagraph().AppendEquation($"K_13={_K13:f2}");

            //    doc.InsertParagraph().AppendEquation("K_14=(1.45-0.43∙δ_1)/sin(0.5∙δ_1)");
            //    doc.InsertParagraph().AppendEquation($"K_14={_K14:f2}");

            //    doc.InsertParagraph("Коэффициенты, учитывающие влияние расстояния от середины опоры до днища и угла охвата");
            //    doc.InsertParagraph().AppendEquation("K_15=min{1.0;(0.8∙√γ+6∙γ)/δ_1}");
            //    doc.InsertParagraph().AppendEquation($"K_15=min(1.0;{_K15_2:f2})={_K15:f2}");

            //    doc.InsertParagraph().AppendEquation("K_16=1-0.65/(1+(6∙γ)^2)∙√(π/(3∙δ_1))");
            //    doc.InsertParagraph().AppendEquation($"K_16={_K16:f2}");

            //    doc.InsertParagraph("Коэффициенты, учитывающие влияние ширины пояса опоры и угла охвата");
            //    doc.InsertParagraph().AppendEquation("K_17=1/(1+0.6∙∛(D/(s_ef))∙(b_2/D)∙δ_1)");
            //    doc.InsertParagraph().AppendEquation($"K_17={_K17:f2}");

            //    doc.InsertParagraph("Общее мембранное меридиональное напряжение изгиба от весовых нагрузок, действующее в области опорного узла");
            //    doc.InsertParagraph().AppendEquation("σ ̅_mx=4∙M_i/(π∙D^2∙(s_ef))");
            //    doc.InsertParagraph().AppendEquation($"σ ̅_mx={_sigma_mx:f2}");

            //    doc.InsertParagraph("Условие прочности");
            //    doc.InsertParagraph().AppendEquation("F_1≤min{[F]_2;[F]_3}");
            //    doc.InsertParagraph("где ").AppendEquation("[F]_2").Append(" - допускаемое опорное усилие от нагружения в меридиональном направлении");
            //    doc.InsertParagraph().AppendEquation("[F]_2=(0.7∙[σ_i]_2∙(s_ef)∙√(D∙(s_ef)))/(K_10∙K_12)");

            //    doc.InsertParagraph("\t").AppendEquation("[F]_3").Append(" - допускаемое опорное усилие от нагружения в окружном направлении");
            //    doc.InsertParagraph().AppendEquation("[F]_3=(0.9∙[σ_i]_3∙(s_ef)∙√(D∙(s_ef)))/(K_14∙K_16∙K_17)");

            //    doc.InsertParagraph("где ").AppendEquation("[σ_i]_2, [σ_i]_2").Append(" - предельные напряжения изгиба в меридиональном и окружном направлениях");

            //    doc.InsertParagraph().AppendEquation("[σ_i]=K_1∙K_2∙[σ]");

            //    doc.InsertParagraph().AppendEquation("K_1=(1-ϑ_2^2)/((1/3+ϑ_1∙ϑ_2)+√((1/3+ϑ_1∙ϑ_2)^2+(1-ϑ_2^2)∙ϑ_1^2))");

            //    // TODO: для условий монтажа 
            //    doc.InsertParagraph().AppendEquation("K_2=1.25").Append(" - для рабочих условий");

            //    doc.InsertParagraph("для ").AppendEquation("[σ_i]_2");
            //    doc.InsertParagraph().AppendEquation("ϑ_1=-(0,23∙K_13∙K_15)/(K_12∙K_10)");
            //    doc.InsertParagraph().AppendEquation($"ϑ_1={_v1_2:f2}");

            //    doc.InsertParagraph().AppendEquation("ϑ_(2,1)=- ̅σ_mx∙1/(K_2∙[σ])");
            //    doc.InsertParagraph().AppendEquation($"ϑ_(2,1)={_v21_2:f2}");
            //    doc.InsertParagraph().AppendEquation("ϑ_(2,2)=[(p∙D)/(4∙(s_ef))- ̅σ_mx]∙1/(K_2∙[σ])");
            //    doc.InsertParagraph().AppendEquation($"ϑ_(2,2)={_v22_2:f2}");

            //    doc.InsertParagraph("Для ").AppendEquation("ϑ_2").Append(" принимают одно из значений ").AppendEquation("ϑ_(2,1)").Append(" или ").AppendEquation("ϑ_(2,2)").Append(", для которого предельное напряжение изгибабудет наименьшим.");
            //    if (_K1_2For_v21 < _K1_2For_v22)
            //    {
            //        doc.InsertParagraph().AppendEquation($"ϑ_2=ϑ_(2,1)={_v21_2:f2}");
            //    }
            //    else
            //    {
            //        doc.InsertParagraph().AppendEquation($"ϑ_2=ϑ_(2,2)={_v22_2:f2}");
            //    }

            //    doc.InsertParagraph().AppendEquation($"K_1={_K1_2:f2}");

            //    doc.InsertParagraph().AppendEquation($"[σ_i]_2={_K1_2:f2}∙{_K2:f2}∙{_saddleDataIn.sigma_d}={_sigmai2:f2}");

            //    doc.InsertParagraph().AppendEquation($"[F]_2=(0.7∙{_sigmai2:f2}∙({_sef:f2})∙√({_saddleDataIn.D}∙({_sef:f2})))/({_K10:f2}∙{_K12:f2})={_F_d2:f2}");


            //    doc.InsertParagraph("для ").AppendEquation("[σ_i]_3");
            //    doc.InsertParagraph().AppendEquation("ϑ_1=-(0,53∙K_11)/(K_14∙K_16∙K_17∙sin(0.5∙δ_1))");
            //    doc.InsertParagraph().AppendEquation($"ϑ_1={_v1_3:f2}");

            //    doc.InsertParagraph().AppendEquation("ϑ_(2,1)=0");
            //    doc.InsertParagraph().AppendEquation($"ϑ_(2,1)={_v21_3:f2}");
            //    doc.InsertParagraph().AppendEquation("ϑ_(2,2)=(p∙D)/(2∙(s_ef))∙1/(K_2∙[σ])");
            //    doc.InsertParagraph().AppendEquation($"ϑ_(2,2)={_v22_3:f2}");

            //    doc.InsertParagraph("Для ").AppendEquation("ϑ_2").AppendEquation(" принимают одно из значений ").AppendEquation("ϑ_(2,1)").Append(" или ").AppendEquation("ϑ_(2,2)").Append(", для которого предельное напряжение изгибабудет наименьшим.");
            //    if (_K1_3For_v21 < _K1_3For_v22)
            //    {
            //        doc.InsertParagraph().AppendEquation($"ϑ_2=ϑ_(2,1)={_v21_3:f2}");
            //    }
            //    else
            //    {
            //        doc.InsertParagraph().AppendEquation($"ϑ_2=ϑ_(2,2)={_v22_3:f2}");
            //    }

            //    doc.InsertParagraph().AppendEquation($"K_1={_K1_3:f2}");

            //    doc.InsertParagraph().AppendEquation($"[σ_i]_3={_K1_3:f2}∙{_K2:f2}∙{_saddleDataIn.sigma_d}={_sigmai3:f2}");

            //    doc.InsertParagraph().AppendEquation($"[F]_3=(0.9∙{_sigmai2:f2}∙({_sef:f2})∙√({_saddleDataIn.D}∙({_sef:f2})))/({_K14:f2}∙{_K16:f2}∙{_K17:f2})={_F_d3:f2}");

            //    doc.InsertParagraph().AppendEquation($"{_F1:f2}≤min({_F_d2:f2};{_F_d3:f2})");
            //    if (_F1 <= Math.Min(_F_d2, _F_d3))
            //    {
            //        doc.InsertParagraph("Условие прочности выполняется");
            //    }
            //    else
            //    {
            //        doc.InsertParagraph("Условие прочности не выполняется").Bold().Color(System.Drawing.Color.Red);
            //    }

            //    doc.InsertParagraph("Условие устойчивости");
            //    // TODO: добавить наружное давление
            //    doc.InsertParagraph().AppendEquation("|p|/[p]+|M_i|/[M]+|F_e|/[F]+(Q_i/[Q])^2≤1");

            //    doc.InsertParagraph("где ").AppendEquation("F_e").Append(" - эффективное осевое усилие от местных мембранных напряжений, действующих в области опоры");
            //    doc.InsertParagraph().AppendEquation("F_e=F_i∙π/4∙K_13∙K_15∙√(D/(s_ef))");
            //    doc.InsertParagraph().AppendEquation($"F_e={_F1:f2}∙π/4∙{_K13:f2}∙{_K15:f2}∙√({_saddleDataIn.D}/({_sef:f2}))={_Fe:f2}");
            //    doc.InsertParagraph().AppendEquation($"{_M1:f2}/{_M_d:f2}+{_Fe:f2}/{_F_d:f2}+({_Q1:f2}/{_Q_d:f2})^2={_yslystoich2:f2}≤1");

            //    if (_yslystoich2 <= 1)
            //    {
            //        doc.InsertParagraph("Условие устойчивости выполняется");
            //    }
            //    else
            //    {
            //        doc.InsertParagraph("Условие устойчивости не выполняется").Bold().Color(System.Drawing.Color.Red);
            //    }
            //}

            //if (_isConditionUseFormuls)
            //{
            //    doc.InsertParagraph("Условия применения формул");
            //}
            //else
            //{
            //    doc.InsertParagraph("Условия применения формул не выполняются").Bold().Color(System.Drawing.Color.Red);
            //}
            //doc.InsertParagraph().AppendEquation("60°≤δ_1≤180°");
            //doc.InsertParagraph($"60°≤{_saddleDataIn.delta1}°≤180°");
            //doc.InsertParagraph().AppendEquation("(s-c)/D≤0.05");
            //doc.InsertParagraph().AppendEquation($"({_saddleDataIn.s}-{_saddleDataIn.c})/{_saddleDataIn.D}={(_saddleDataIn.s - _saddleDataIn.c) / _saddleDataIn.D:f2}≤0.05");
            //if (_saddleDataIn.type == 2)
            //{
            //    doc.InsertParagraph().AppendEquation("s_2≥s");
            //    doc.InsertParagraph().AppendEquation($"{_saddleDataIn.s2} мм ≥ {_saddleDataIn.s} мм");
            //    doc.InsertParagraph().AppendEquation("δ_2≥δ_1+20°");
            //    doc.InsertParagraph().AppendEquation($"{_saddleDataIn.delta2}°≥{_saddleDataIn.delta1}°+20°={_saddleDataIn.delta1 + 20}°");
            //    doc.InsertParagraph().AppendEquation("A_k≥(s-c)√(D∙(s-c))");
            //    doc.InsertParagraph().AppendEquation($"{_Ak:f2}≥({_saddleDataIn.s}-{_saddleDataIn.c})√({_saddleDataIn.D}∙({_saddleDataIn.s}-{_saddleDataIn.c}))={_Akypf:f2}");
            //}

            //doc.Save();
        }

    }
}
