using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.DataIn;
using CalculateVessels.Core.Shells.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalculateVessels.Core.Word;
using CalculateVessels.Core.Word.Enums;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace CalculateVessels.Core.Shells
{
    public class ConicalShell : Shell, IElement
    {
        internal readonly ConicalShellDataIn _csdi;

        public ConicalShell(ConicalShellDataIn csdi)
        {
            _csdi = csdi;
            _cosAlfa1 = Math.Cos(_csdi.alfa1 * Math.PI / 180);
            _tgAlfa1 = Math.Tan(_csdi.alfa1 * Math.PI / 180);
            _sinAlfa1 = Math.Sin(_csdi.alfa1 * Math.PI / 180);
        }

        private readonly double _cosAlfa1;
        private readonly double _sinAlfa1;
        private readonly double _tgAlfa1;

        private int switch1;

        private double _Dk;

        private double _a1p;
        private double _a2p;

        private double _a1p_l;
        private double _a2p_l;

        private double _lE;
        private double _DE_1;
        private double _DE_2;
        private double _DE;
        private double _B1_1;
        private double _B1;
        private double _beta_4;
        private double _s_2plit;
        private double _s_2p;
        private double _s_tp;
        private double _beta;
        private double _beta_0;
        private double _beta_1;
        private double _beta_2;
        private double _beta_3;
        private double _beta_a;
        private double _beta_t;
        private double _beta_n;

        private double _chi_1;
        private double _conditionForBetan;
        private double _Ak;

        private double _B2;
        private double _B3;

        private double _p_dBig;
        private double _p_dLittle;
        

        internal double Dk { get => _Dk; }

        public override string ToString() => $"Коническая обечайка {_csdi.Name}";

        public void Calculate()
        {
            _c = _csdi.c1 + _csdi.c2 + _csdi.c3;

            // Condition use formuls
            {
                const double CONDITION_USE_FORMULS_FROM = 0.001;
                const double CONDITION_USE_FORMULS_TO = 0.05;

                IsConditionUseFormulas = _csdi.s1 * _cosAlfa1 / _csdi.D >= CONDITION_USE_FORMULS_FROM
                    && _csdi.s1 * _cosAlfa1 / _csdi.D <= CONDITION_USE_FORMULS_TO;

                if (!IsConditionUseFormulas)
                {
                    IsError = true;
                    ErrorList.Add("Условие применения формул не выполняется");
                }
            }

            switch (_csdi.ConnectionType)
            {
                case ConicalConnectionType.Simply:
                case ConicalConnectionType.WithRingPicture25b:
                case ConicalConnectionType.WithRingPicture29:
                    _a1p = 0.7 * Math.Sqrt(_csdi.D * (_csdi.s1 - _c) / _cosAlfa1);
                    _a2p = 0.7 * Math.Sqrt(_csdi.D * (_csdi.s2 - _c));
                    break;
                case ConicalConnectionType.Toroidal:
                    _a1p = 0.7 * Math.Sqrt(_csdi.D * (_csdi.sT - _c) / _cosAlfa1);
                    _a2p = 0.5 * Math.Sqrt(_csdi.D * (_csdi.sT - _c));
                    break;
            }

            if (_csdi.IsConnectionWithLittle)
            {
                _a1p_l = Math.Sqrt(_csdi.D1 * (_csdi.s1 - _c) / _cosAlfa1);
                _a2p_l = 1.25 * Math.Sqrt(_csdi.D1 * (_csdi.s2 - _c));
            }

            _Dk = _csdi.ConnectionType == ConicalConnectionType.Toroidal
                ? _csdi.D - 2 * (_csdi.r * (1 - _cosAlfa1) + 0.7 * _a1p * _sinAlfa1)
                : _csdi.D - 1.4 * _a1p * _sinAlfa1;


            if (_csdi.p > 0)
            {
                if (_csdi.IsPressureIn)
                {

                    _s_p = _csdi.p * _Dk / (2 * _csdi.sigma_d * _csdi.fi - _csdi.p)
                        * (1 / _cosAlfa1);
                    _s = _s_p + _c;
                    if (_csdi.s == 0)
                    {
                        _p_d = 2 * _csdi.sigma_d * _csdi.fi * _s_p / (_Dk / _cosAlfa1 + _s_p);
                    }
                    else if (_csdi.s >= _s)
                    {
                        _p_d = 2 * _csdi.sigma_d * _csdi.fi * (_csdi.s - _c)
                            / (_Dk / _cosAlfa1 + (_csdi.s - _c));
                    }
                    else
                    {
                        IsCriticalError = true;
                        ErrorList.Add("Принятая толщина меньше расчетной");
                    }
                }
                else
                {
                    _lE = (_csdi.D - _csdi.D1) / (2 * _sinAlfa1);
                    _DE_1 = (_csdi.D + _csdi.D1) / (2 * _cosAlfa1);
                    _DE_2 = _csdi.D / _cosAlfa1 - 0.3 * (_csdi.D + _csdi.D1)
                        * Math.Sqrt((_csdi.D + _csdi.D1) / ((_csdi.s - _c) * 100)) * _tgAlfa1;
                    _DE = Math.Max(_DE_1, _DE_2);
                    _B1_1 = 9.45 * _DE / _lE * Math.Sqrt(_DE / (100 * (_csdi.s - _c)));
                    _B1 = Math.Min(1.0, _B1_1);

                    _s_p_1 = 1.06 * (0.01 * _DE / _B1)
                        * Math.Pow(_csdi.p / (0.00001 * _csdi.E) * (_lE / _DE), 0.4);
                    _s_p_2 = 1.2 * _csdi.p * _Dk / (2 * _csdi.fi * _csdi.sigma_d - _csdi.p)
                        * (1 / _cosAlfa1);
                    _s_p = Math.Max(_s_p_1, _s_p_2);
                    _s = _s_p + _c;
                    if (_csdi.s == 0)
                    {
                        _p_dp = 2 * _csdi.sigma_d * _s_p / (_Dk / _cosAlfa1 + _s_p);
                        _p_de = 2.08 * 0.00001 * _csdi.E / (_csdi.ny * _B1) * (_DE / _lE)
                            * Math.Pow(100 * _s_p / _DE, 2.5);
                        _p_d = _p_dp / Math.Sqrt(1 + Math.Pow(_p_dp / _p_de, 2));
                    }
                    else if (_csdi.s >= _s)
                    {
                        _p_dp = 2 * _csdi.sigma_d * (_csdi.s - _c)
                            / (_Dk / _cosAlfa1 + _csdi.s - _c);
                        _p_de = 2.08 * 0.00001 * _csdi.E / (_csdi.ny * _B1) * (_DE / _lE)
                            * Math.Pow(100 * (_csdi.s - _c) / _DE, 2.5);
                        _p_d = _p_dp / Math.Sqrt(1 + Math.Pow(_p_dp / _p_de, 2));
                    }
                    else
                    {
                        IsCriticalError = true;
                        ErrorList.Add("Принятая толщина меньше расчетной");
                    }
                }
                if (_p_d < _csdi.p && _csdi.s != 0)
                {
                    IsError = true;
                    ErrorList.Add("[p] меньше p");
                }
                if (_csdi.ConnectionType != ConicalConnectionType.WithoutConnection)
                {
                    if (_csdi.alfa1 > 70)
                    {
                        IsConditionUseFormulas = false;
                        IsError = true;
                        ErrorList.Add("Угол должен быть меньше либо равен 70 градусам");
                    }
                    switch (_csdi.ConnectionType)
                    {
                        case ConicalConnectionType.Simply:
                            if((_csdi.s1 - _c) < (_csdi.s2 - _c))
                            {
                                IsConditionUseFormulas = false;
                                IsError = true;
                                ErrorList.Add("Условие присменения формул не выполняется");
                            }
                            _chi_1 = _csdi.sigma_d_1 / _csdi.sigma_d_2;
                            _beta = 0.4 * Math.Sqrt(_csdi.D / (_csdi.s2 - _c)) * _tgAlfa1
                                / (1 + Math.Sqrt((1 + _chi_1
                                * Math.Pow((_csdi.s1 - _c) / (_csdi.s2 - _c), 2))
                                / (2 * _cosAlfa1) * _chi_1 * (_csdi.s1 - _c) / (_csdi.s2 - _c))) - 0.25;
                            _beta_1 = Math.Max(0.5, _beta);
                            _s_2p = _csdi.p * _csdi.D * _beta_1
                                / (2 * _csdi.sigma_d_2 * _csdi.fi - _csdi.p);
                            if (_csdi.s2 >= _s_2p + _c)
                            {
                                _p_dBig = 2 * _csdi.sigma_d_2 * _csdi.fi * (_csdi.s2 - _c)
                                    / (_csdi.D * _beta_1 + (_csdi.s2 - _c));
                            }
                            else
                            {
                                IsCriticalError = true;
                                ErrorList.Add("Принятая толщина переходной зоны меньше расчетной");
                            }
                            break;
                        case ConicalConnectionType.WithRingPicture25b:
                            if ((_csdi.s1 - _c) < (_csdi.s2 - _c))
                            {
                                IsConditionUseFormulas = false;
                                IsError = true;
                                ErrorList.Add("Условие присменения формул не выполняется");
                            }
                            _chi_1 = _csdi.sigma_d_1 / _csdi.sigma_d_2;
                            _beta = 0.4 * Math.Sqrt(_csdi.D / (_csdi.s2 - _c)) * _tgAlfa1
                                / (1 + Math.Sqrt((1 + _chi_1
                                * Math.Pow((_csdi.s1 - _c) / (_csdi.s2 - _c), 2))
                                / (2 * _cosAlfa1) * _chi_1 * (_csdi.s1 - _c) / (_csdi.s2 - _c))) - 0.25;
                            _beta_a = (2 * _csdi.sigma_d_2 * _csdi.fi / _csdi.p - 1) * (_csdi.s2 - _c) / _csdi.D;
                            _Ak = _csdi.p * Math.Pow(_csdi.D, 2) * _tgAlfa1 / (8 * _csdi.sigma_d_k * _csdi.fi_k)
                                * (1 - (_beta_a + 0.25) / (_beta + 0.25));
                            _B2 = 1.6 * _Ak / ((_csdi.s2 - _c) * Math.Sqrt(_csdi.D * (_csdi.s2 - _c)))
                                * _csdi.sigma_d_k * _csdi.fi_k / (_csdi.sigma_d_2 * _csdi.fi_t);
                            _B3 = 0.25;
                            _beta_0 = 0.4 * Math.Sqrt(_csdi.D / (_csdi.s2 - _c)) * _tgAlfa1 - _B3 *
                                (1 + Math.Sqrt((1 + _chi_1 * Math.Pow((_csdi.s1 - _c) / (_csdi.s2 - _c), 2)) /
                                (2 * _cosAlfa1) * _chi_1 * (_csdi.s1 - _c) / (_csdi.s2 - _c))) /
                                (_B2 + (1 + Math.Sqrt((1 + _chi_1 * Math.Pow((_csdi.s1 - _c) / (_csdi.s2 - _c), 2)) /
                                (2 * _cosAlfa1) * _chi_1 * (_csdi.s1 - _c) / (_csdi.s2 - _c))));
                            _beta_2 = Math.Max(0.5, _beta_0);
                            _p_dBig = 2 * _csdi.sigma_d_2 * _csdi.fi * (_csdi.s2 - _c) / (_csdi.D * _beta_2 + (_csdi.s2 - _c));
                            break;
                        case ConicalConnectionType.WithRingPicture29:
                            _Ak = _csdi.p * Math.Pow(_csdi.D, 2) * _tgAlfa1 / (8 * _csdi.sigma_d_k * _csdi.fi_k);
                            _p_dBig = _Ak * 8 * _csdi.sigma_d_k * _csdi.fi_k / (Math.Pow(_csdi.D, 2) * _tgAlfa1);
                            //TODO: Check conical shell with ring picture 29
                            break;
                        case ConicalConnectionType.Toroidal:
                            if (_csdi.r / _csdi.D >= 0.0
                                && _csdi.r / _csdi.D < 0.3)
                            {
                                IsConditionUseFormulas = false;
                                IsError = true;
                                ErrorList.Add("Условие применения формул не выполняется");
                            }
                            _chi_1 = _csdi.sigma_d_1 / _csdi.sigma_d_2;
                            _beta = 0.4 * Math.Sqrt(_csdi.D / (_csdi.s2 - _c)) * _tgAlfa1
                                / (1 + Math.Sqrt((1 + _chi_1
                                * Math.Pow((_csdi.s1 - _c) / (_csdi.s2 - _c), 2))
                                / (2 * _cosAlfa1) * _chi_1 * (_csdi.s1 - _c) / (_csdi.s2 - _c))) - 0.25;
                            _beta_t = 1 / (1 + (0.028 * _csdi.alfa1 * _csdi.r / _csdi.D *
                                Math.Sqrt(_csdi.D / (_csdi.sT - c))) /
                                (1 / Math.Sqrt(_cosAlfa1) + 1));
                            //TODO: Check alfa1 in beta_t in degreee or in radians
                            _beta_3 = Math.Max(0.5, Math.Max(_beta, _beta_t));
                            _s_tp = _csdi.p * _csdi.D * _beta_3 / (2 * _csdi.fi * _csdi.sigma_d - _csdi.p);
                            _p_dBig = 2 * _csdi.sigma_d * _csdi.fi * (_csdi.sT - _c) /
                                (_csdi.D * _beta_3 + (_csdi.sT - _c));
                            break;
                    }
                    if (_p_dBig < _csdi.p && _csdi.s != 0)
                    {
                        IsError = true;
                        ErrorList.Add("[p] для переходной части меньше p");
                    }
                    if(_csdi.IsConnectionWithLittle)
                    {
                        _chi_1 = _csdi.sigma_d_1 / _csdi.sigma_d_2;
                        _conditionForBetan = Math.Pow((_csdi.s1 - _c) / (_csdi.s2 - _c), 2);
                        if(_conditionForBetan >= 1)
                        {
                            _beta = 0.4 * Math.Sqrt(_csdi.D1 / (_csdi.s2 - _c)) * _tgAlfa1
                            / (1 + Math.Sqrt((1 + _chi_1
                            * Math.Pow((_csdi.s1 - _c) / (_csdi.s2 - _c), 2))
                            / (2 * _cosAlfa1) * _chi_1 * (_csdi.s1 - _c) / (_csdi.s2 - _c))) - 0.25;
                            _beta_n = _beta + 0.75;
                        }
                        else
                        {
                            _beta_n = 0.4 * Math.Sqrt(_csdi.D1 / (_csdi.s2 - _c)) * _tgAlfa1
                            / (_chi_1 * (_csdi.s1 - _c)/(_csdi.s2 - _c) * Math.Sqrt((_csdi.s1 - _c) /
                            ((_csdi.s2 - _c) * _cosAlfa1)) + Math.Sqrt((1 + _chi_1
                            * Math.Pow((_csdi.s1 - _c) / (_csdi.s2 - _c), 2))
                            / 2)) + 0.5;
                        }
                        _beta_4 = Math.Max(1, _beta_n);
                        _s_2plit = _csdi.p * _csdi.D1 * _beta_4 / (2 * _csdi.fi * _csdi.sigma_d_2 - _csdi.p);
                        _p_dLittle = 2 * _csdi.sigma_d_2 * _csdi.fi * (_csdi.s2 - _c) /
                            (_csdi.D1 * _beta_4 + (_csdi.s2 - _c));
                        
                    }
                }

            }

            //if (_csdi.F > 0)
            //{
            //    _s_calcrf = _csdi.F / (Math.PI * _csdi.D * _csdi.sigma_d * _csdi.fit);
            //    _s_calcf = _s_calcrf + _c;
            //    if (_csdi.isFTensile)
            //    {
            //        _F_d = Math.PI * (_csdi.D + _csdi.s - _c) * (_csdi.s - _c) * _csdi.sigma_d * _csdi.fit;
            //    }
            //    else
            //    {
            //        _F_dp = Math.PI * (_csdi.D + _csdi.s - _c) * (_csdi.s - _c) * _csdi.sigma_d;
            //        _F_de1 = 0.000031 * _csdi.E / _csdi.ny * Math.Pow(_csdi.D, 2) * Math.Pow(100 * (_csdi.s - _c) / _csdi.D, 2.5);

            //        const int L_MORE_THEN_D = 10;
            //        bool isLMoreThenD = _csdi.l / _csdi.D > L_MORE_THEN_D;

            //        if (isLMoreThenD)
            //        {
            //            switch (_csdi.FCalcSchema)
            //            {
            //                case 1:
            //                    _lpr = _csdi.l;
            //                    break;
            //                case 2:
            //                    _lpr = 2 * _csdi.l;
            //                    break;
            //                case 3:
            //                    _lpr = 0.7 * _csdi.l;
            //                    break;
            //                case 4:
            //                    _lpr = 0.5 * _csdi.l;
            //                    break;
            //                case 5:
            //                    _F = _csdi.q * _csdi.l;
            //                    _lpr = 1.12 * _csdi.l;
            //                    break;
            //                case 6:
            //                    double fDivl6 = _csdi.F / _csdi.l;
            //                    fDivl6 *= 10;
            //                    fDivl6 = Math.Round(fDivl6 / 2);
            //                    fDivl6 *= 0.2;
            //                    switch (fDivl6)
            //                    {
            //                        case 0:
            //                            _lpr = 2 * _csdi.l;
            //                            break;
            //                        case 0.2:
            //                            _lpr = 1.73 * _csdi.l;
            //                            break;
            //                        case 0.4:
            //                            _lpr = 1.47 * _csdi.l;
            //                            break;
            //                        case 0.6:
            //                            _lpr = 1.23 * _csdi.l;
            //                            break;
            //                        case 0.8:
            //                            _lpr = 1.06 * _csdi.l;
            //                            break;
            //                        case 1:
            //                            _lpr = _csdi.l;
            //                            break;
            //                    }
            //                    break;
            //                case 7:
            //                    double fDivl7 = _csdi.F / _csdi.l;
            //                    fDivl7 *= 10;
            //                    fDivl7 = Math.Round(fDivl7 / 2);
            //                    fDivl7 *= 0.2;
            //                    switch (fDivl7)
            //                    {
            //                        case 0:
            //                            _lpr = 2 * _csdi.l;
            //                            break;
            //                        case 0.2:
            //                            _lpr = 1.7 * _csdi.l;
            //                            break;
            //                        case 0.4:
            //                            _lpr = 1.4 * _csdi.l;
            //                            break;
            //                        case 0.6:
            //                            _lpr = 1.11 * _csdi.l;
            //                            break;
            //                        case 0.8:
            //                            _lpr = 0.85 * _csdi.l;
            //                            break;
            //                        case 1:
            //                            _lpr = 0.7 * _csdi.l;
            //                            break;
            //                    }
            //                    break;

            //            }
            //            lamda = 2.83 * _lpr / (_csdi.D + _csdi.s - _c);
            //            _F_de2 = Math.PI * (_csdi.D + _csdi.s - _c) * (_csdi.s - _c) * _csdi.E / _csdi.ny *
            //                            Math.Pow(Math.PI / lamda, 2);
            //            _F_de = Math.Min(_F_de1, _F_de2);
            //        }
            //        else
            //        {
            //            _F_de = _F_de1;
            //        }

            //        _F_d = _F_dp / Math.Sqrt(1 + Math.Pow(_F_dp / _F_de, 2));
            //    }
            //}

            //if (_csdi.M > 0)
            //{
            //    _M_dp = Math.PI / 4 * _csdi.D * (_csdi.D + _csdi.s - _c) * (_csdi.s - _c) * _csdi.sigma_d;
            //    _M_de = 0.000089 * _csdi.E / _csdi.ny * Math.Pow(_csdi.D, 3) * Math.Pow(100 * (_csdi.s - _c) / _csdi.D, 2.5);
            //    _M_d = _M_dp / Math.Sqrt(1 + Math.Pow(_M_dp / _M_de, 2));
            //}

            //if (_csdi.Q > 0)
            //{
            //    _Q_dp = 0.25 * _csdi.sigma_d * Math.PI * _csdi.D * (_csdi.s - _c);
            //    _Q_de = 2.4 * _csdi.E * Math.Pow(_csdi.s - _c, 2) / _csdi.ny *
            //        (0.18 + 3.3 * _csdi.D * (_csdi.s - _c) / Math.Pow(_csdi.l, 2));
            //    _Q_d = _Q_dp / Math.Sqrt(1 + Math.Pow(_Q_dp / _Q_de, 2));
            //}

            //if ((_csdi.IsNeedpCalculate || _csdi.isNeedFCalculate) &&
            //    (_csdi.isNeedMCalculate || _csdi.isNeedQCalculate))
            //{
            //    conditionYstoich = _csdi.p / _p_d + _csdi.F / _F_d + _csdi.M / _M_d +
            //                            Math.Pow(_csdi.Q / _Q_d, 2);
            //}
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

            body.AddParagraph($"Расчет на прочность конической обечайки {_csdi.Name}, нагруженной " +
                              (_csdi.IsPressureIn ? "внутренним избыточным давлением" : "наружным давлением")).Heading(HeadingType.Heading1);
            body.AddParagraph("");

            var imagePart = mainPart.AddImagePart(ImagePartType.Gif);

            using MemoryStream stream = new(Data.Properties.Resources.ConeElemBottom);
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

                //if (!_csdi.IsPressureIn)
                //{
                //    table.AddRow()
                //        .AddCell("Длина обечайки, l:")
                //        .AddCell($"{_csdi.l} мм");
                //}

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
            body.AddParagraph("Расчетные параметры").Alignment(AlignmentType.Center);
            body.AddParagraph("");

            body.AddParagraph("Расчетные длины переходных частей. В првом приближении принимаем ")
                .AppendEquation($"s_1={_csdi.s1} мм");
                        
            switch (_csdi.ConnectionType)
            {
                case ConicalConnectionType.Simply:
                case ConicalConnectionType.WithRingPicture25b:
                    body.Elements<Paragraph>().Last()
                        .AppendEquation($"s_2={_csdi.s2} мм");
                    body.AddParagraph("- для конических и цилиндрических обечаек");
                    body.AddParagraph("")
                        .AppendEquation("a_1p=0.7√(D/cosα_1∙(s_1-c))" +
                                        $"=0.7√({_csdi.D}/cos{_csdi.alfa1}({_csdi.s1}-{_c:f2}))={_a1p:f2}");
                    body.AddParagraph("")
                        .AppendEquation("a_2p=0.7√(D∙(s_2-c))" +
                                        $"=0.7√({_csdi.D}∙({_csdi.s2}-{_c:f2}))={_a2p:f2}");
                    break;
                case ConicalConnectionType.Toroidal:
                    body.Elements<Paragraph>().Last()
                        .AppendEquation($"s_T={_csdi.sT} мм");
                    body.AddParagraph("- для конических и цилиндрических обечаек");
                    body.AddParagraph("")
                        .AppendEquation("a_1p=0.7√(D/cosα_1∙(s_T-c))" +
                                        $"=0.7√({_csdi.D}/cos{_csdi.alfa1}({_csdi.sT}-{_c:f2}))={_a1p:f2}");
                    body.AddParagraph("")
                        .AppendEquation("a_2p=0.5√(D∙(s_T-c))" +
                                        $"=0.5√({_csdi.D}∙({_csdi.sT}-{_c:f2}))={_a2p:f2}");
                    break;
            }

            if (_csdi.IsConnectionWithLittle)
            {
                body.AddParagraph("- для конических и цилиндрических обечаек или штуцера")
                    .AppendEquation("a_1p=√(D_1/cosα_1∙(s_1-c))" +
                                    $"=√({_csdi.D1}/cos{_csdi.alfa1}({_csdi.s1}-{_c:f2}))={_a1p_l:f2}");
                body.AddParagraph("")
                    .AppendEquation("a_2p=1.25√(D_1∙(s_2-c))" +
                                    $"=1.25√({_csdi.D1}∙({_csdi.s2}-{_c:f2}))={_a2p_l:f2}");
            }

            body.AddParagraph("Расчетный диаметр гладкой конической обечайки");
            if(_csdi.ConnectionType != ConicalConnectionType.Toroidal)
            {
                body.Elements<Paragraph>().Last()
                    .AddRun(" без тороидального перехода");
                body.AddParagraph("")
                    .AppendEquation("D_k=D-1.4∙a_1p∙sinα_1" +
                    $"={_csdi.D}-1.4∙{_a1p:f2}∙sin{_csdi.alfa1}={_Dk:f2} мм");
            }
            else
            {
                body.Elements<Paragraph>().Last()
                    .AddRun(" с тороидальным переходом");
                body.AddParagraph("")
                    .AppendEquation("D_k=D-2[r(1-cosα_1)+0.7∙a_1p∙sinα_1]" +
                    $"={_csdi.D}-2[{_csdi.r}(1-cos{_csdi.alfa1})+0.7∙{_a1p:f2}∙sin{_csdi.alfa1}]={_Dk:f2} мм");
            }

            body.AddParagraph("Толщину стенки гладкой конической обечайки вычисляют по формуле:");
            body.AddParagraph("").AppendEquation("s_k≥s_k.p+c");
            body.AddParagraph("где ").AppendEquation("s_k.p").AddRun(" - расчетная толщина стенки конической обечайки");

            if (_csdi.IsPressureIn)
            {
                body.AddParagraph("")
                    .AppendEquation("s_k.p=(p∙D_k)/(2∙φ_p∙[σ]-p)(1/cosα_1)" +
                                    $"=({_csdi.p}∙{_Dk:f2})/(2∙{_csdi.fi}∙{_csdi.sigma_d}-{_csdi.p})(1/cos{_csdi.alfa1})={_s_p:f2} мм");
            }
            else
            {
                body.AddParagraph("")
                    .AppendEquation("s_k.p=max{1.06∙(10^-2∙D_E)/(B_1)∙(p/(10^-5∙E)∙l_E/D_E)^0.4;(1.2∙p∙D_k)/(2∙φ_p∙[σ]-p)(1/cosα_1)}");
                body.AddParagraph("Эффективные размеры конической обечайки вычисляют по формулам (предварительно принимаем ")
                    .AppendEquation($"s_k={_csdi.s} мм")
                    .AddRun("):");
                body.AddParagraph("")
                    .AppendEquation("l_E=(D-D_1)/(2∙sinα_1)" + $"=({_csdi.D}-{_csdi.D1})/(2∙sin{_csdi.alfa1})={_lE:f2} мм");
                body.AddParagraph("")
                    .AppendEquation("D_E=max{(D+D_1)/(2∙cosα_1);D/cosα_1-0.3∙(D+D_1)∙√((D+D_1)/((s_k-c)∙100)∙tgα_1}");
                body.AddParagraph("")
                    .AppendEquation("(D+D_1)/(2∙cosα_1)" + $"=({_csdi.D}+{_csdi.D1})/(2∙cos{_csdi.alfa1})={_DE_1:f2}");
                body.AddParagraph("")
                    .AppendEquation("D/cosα_1-0.3∙(D+D_1)∙√((D+D_1)/((s_k-c)∙100)∙tgα_1" +
                    $"{_csdi.D}/cos{_csdi.alfa1}-0.3∙({_csdi.D}+{_csdi.D1})∙√(({_csdi.D}+{_csdi.D1})/(({_csdi.s}-{_c:f2})∙100)∙tg{_csdi.alfa1}={_DE_2:f2}");
                body.AddParagraph("")
                    .AppendEquation($"D_E=max{{({_DE_1:f2};{_DE_2:f2}}}={_DE:f2} мм");
                body.AddParagraph("Коэффициент ")
                    .AppendEquation("B_1")
                    .AddRun(" вычисляют по формуле:");
                body.AddParagraph("")
                    .AppendEquation("B_1=min{1.0;9.45∙D_E/l_E∙√(D_E/((s_k-c)∙100))}");
                body.AddParagraph("")
                    .AppendEquation("9.45∙D_E/l_E∙√(D_E/((s_k-c)∙100))" +
                    $"9.45∙{_DE:F2}/{_lE:f2}∙√({_DE:f2}/(({_csdi.s}-{_c:f2})∙100))={_B1_1:f2}");
                body.AddParagraph("")
                    .AppendEquation($"B=max(1;{_B1_1:f2})={_B1:f2}");
                body.AddParagraph("")
                    .AppendEquation($"1.06∙(10^-2∙{_DE:f2})/{_B1:f2}∙({_csdi.p}/(10^-5∙{_csdi.E})∙{_lE:f2}/{_DE:f2})^0.4={_s_p_1:f2}");
                body.AddParagraph("")
                    .AppendEquation($"(1.2∙{_csdi.p}∙{_Dk:f2})/(2∙{_csdi.fi}∙{_csdi.sigma_d}-{_csdi.p})(1/cos{_csdi.alfa1})={_s_p_2:f2}");
                body.AddParagraph("")
                    .AppendEquation($"s_p=max{{{_s_p_1:f2};{_s_p_2:f2}}}={_s_p:f2} мм");
            }

            body.AddParagraph("c - сумма прибавок к расчетной толщине");
            body.AddParagraph("")
                .AppendEquation($"c=c_1+c_2+c_3={_csdi.c1}+{_csdi.c2}+{_csdi.c3}={_c:f2} мм");

            body.AddParagraph("").AppendEquation($"s_k={_s_p:f2}+{_c:f2}={_s:f2} мм");

            if (_csdi.s > _s)
            {
                body.AddParagraph($"Принятая толщина s_k={_csdi.s} мм").Bold();
            }
            else
            {
                body.AddParagraph($"Принятая толщина s_k={_csdi.s} мм")
                    .Bold()
                    .Color(System.Drawing.Color.Red);
            }
            if (_csdi.IsPressureIn)
            {
                body.AddParagraph("Допускаемое внутреннее избыточное давление вычисляют по формуле:");
                body.AddParagraph("")
                    .AppendEquation("[p]=(2∙[σ]∙φ_p∙(s_k-c))/(D_k/cosα_1+(s_k-c))"
                                    + $"=(2∙{_csdi.sigma_d}∙{_csdi.fi}∙({_csdi.s}-{_c:f2}))/({_Dk:f2}/cos{_csdi.alfa1}+({_csdi.s}-{_c:f2}))={_p_d:f2} МПа");
            }
            else
            {
                body.AddParagraph("Допускаемое наружное давление вычисляют по формуле:");
                body.AddParagraph("")
                    .AppendEquation("[p]=[p]_П/√(1+([p]_П/[p]_E)^2)");
                body.AddParagraph("допускаемое давление из условия прочности вычисляют по формуле:");
                body.AddParagraph("")
                    .AppendEquation("[p]_П=(2∙[σ]∙φ_p∙(s_k-c))/(D_k/cosα_1+(s_k-c))" +
                                    $"=(2∙{_csdi.sigma_d}∙{_csdi.fi}∙({_csdi.s}-{_c:f2}))/({_Dk:f2}/cos{_csdi.alfa1}+({_csdi.s}-{_c:f2}))={_p_dp:f2} МПа");
                body.AddParagraph("допускаемое давление из условия устойчивости в пределах упругости вычисляют по формуле:");
                body.AddParagraph("")
                    .AppendEquation("[p]_E=(2.08∙10^-5∙E)/(n_y∙B_1)∙D_E/l_E∙[(100∙(s_k-c))/D_E]^2.5" +
                    $"=(2.08∙10^-5∙{_csdi.E})/({_csdi.ny}∙{_B1:f2})∙{_DE:f2}/{_lE:f2}∙[(100∙({_csdi.s}-{_c:f2}))/{_DE:f2}]^2.5={_p_de:f2} МПа");
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

            if(_csdi.ConnectionType != ConicalConnectionType.WithoutConnection)
            {
                switch (_csdi.ConnectionType)
                {
                    case ConicalConnectionType.Simply:
                        body.AddParagraph("Соединение обечайки без тороидального перехода");
                        body.AddParagraph("Толщину стенки из условия прочности переходной зоны вычисляют по формулам: ");
                        body.AddParagraph("").AppendEquation("s_2≥s_2p+c");
                        body.AddParagraph("")
                            .AppendEquation("s_2p=(p∙D∙β_1)/(2∙[σ]_2∙φ_p-p)");
                        body.AddParagraph("где ")
                            .AppendEquation("β_1")
                            .AddRun(" - коэффициент формы вычисляют по формуле:");
                        body.AddParagraph("")
                            .AppendEquation("β_1=max{0.5;β}");
                        body.AddParagraph("")
                            .AppendEquation("β=0.4√(D/(s_2-c))∙tgα_1/(1+√((1+χ_1((s_1-c)/(s_2-c))^2)/(2∙cosα_1)χ_1(s_1-c)/(s_2-c)))-0.25");
                        body.AddParagraph("")
                            .AppendEquation("χ_1=[σ]_1/[σ]_2" + 
                            $"={_csdi.sigma_d_1}/{_csdi.sigma_d_2}={_chi_1:f2}");
                        body.AddParagraph("")
                            .AppendEquation($"β=0.4√({_csdi.D}/({_csdi.s2}-{_c:f2}))∙tg{_csdi.alfa1}/(1+√((1+{_chi_1:f2} (({_csdi.s1}-{_c:f2})/({_csdi.s2}-{_c:f2}))^2)/(2∙cos{_csdi.alfa1}){_chi_1:f2}({_csdi.s1}-{_c:f2})/({_csdi.s2}-{_c:f2})))-0.25={_beta:f2}");
                        body.AddParagraph("")
                            .AppendEquation($"β_1=max{{0.5;{_beta:f2}}}={_beta_1:f2}");
                        body.AddParagraph("")
                            .AppendEquation($"s_2p=({_csdi.p}∙{_csdi.D}∙{_beta_1:f2})/(2∙{_csdi.sigma_d}∙{_csdi.fi}-{_csdi.p})={_s_2p:f2} мм");
                        body.AddParagraph("Допускаемое " +
                            (_csdi.IsPressureIn ? "внутреннее избыточное" : "наружное") +
                            "давление из условия прочности переходной части вычисляют по формуле");
                        body.AddParagraph("")
                            .AppendEquation("[p]=(2∙[σ]_2∙φ_p∙(s_2-c))/(D∙β_1+(s_2-c))" +
                            $"=(2∙{_csdi.sigma_d_2}∙{_csdi.fi}∙({_csdi.s2}-{_c:f2}))/({_csdi.D}∙{_beta_1:f2}+({_csdi.s2}-{_c:f2}))={_p_dBig:f2} МПа");
                            break;
                }

                body.AddParagraph("").AppendEquation("[p]≥p");
                body.AddParagraph("")
                    .AppendEquation($"{_p_dBig:f2}≥{_csdi.p}");
                if (_p_dBig > _csdi.p)
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

            }
            //UNDONE: Make word conocal shell

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
