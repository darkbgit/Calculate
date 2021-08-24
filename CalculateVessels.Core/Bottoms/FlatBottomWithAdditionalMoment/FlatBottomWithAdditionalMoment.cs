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
using System.Runtime.Serialization.Formatters.Binary;
using DocumentFormat.OpenXml.Wordprocessing;

namespace CalculateVessels.Core.Bottoms.FlatBottomWithAdditionalMoment
{
    public class FlatBottomWithAdditionalMoment : IElement
    {
        private readonly FlatBottomWithAdditionalMomentDataIn _fbdi;

        public FlatBottomWithAdditionalMoment(FlatBottomWithAdditionalMomentDataIn flatBottomWithAdditionalMomentDataIn)
        {
            _fbdi = flatBottomWithAdditionalMomentDataIn;
        }

        public bool IsCriticalError { get; private set; }
        public bool IsError { get; private set; }
        public List<string> ErrorList { get => _errorList; private set => _errorList = value; }
        public List<string> Bibliography { get; } = new()
        {
            Data.Properties.Resources.GOST_34233_1,
            Data.Properties.Resources.GOST_34233_2
        };
        public double PressurePermissible => _p_d;

        private double _Ab;

        private double _b;
        private double _b0;

        private double _c;

        private double _dkr;
        private double _Dcp;

        private double _e;

        private bool _isGasketFlat;
        private bool _isGasketMetal;
        private bool _isConditionUseFormulas;

        private double _K;
        private double _K_1;
        private double _K0;
        private double _Kp;
        private double _K6;
        private double _K7Fors2;
        private double _K7Fors3;

        private double _Dp;

        private double _hkr;

        private double _Qd;
        private double _QFM;
        private double _Qt;

        private double _s1p;
        private double _s1;
        private double _s2p;
        private double _s2;
        private double _s2p_1;
        private double _s2p_2;
        private double _s3p;
        private double _s3;
        private double _s3p_1;
        private double _s3p_2;
        private double _S0;
        private double _Se;

        private double _conditionUseFormulas;
        private double _p_d;
        private double _Pbp;
        private double _Pbm;
        private double _Pb1;
        private double _Pb1_1;
        private double _Pb1_2;
        private double _Pb2;
        private double _Pb2_2;
        private double _Pobj;

        private double _Rp;

        private double _tb;
        private double _tf;
        private double _tkr;

        private double _x;
        
        private double _yp;
        private double _yb;
        private double _yF;
        private double _ykr;
        private double _yfn;

        private double _alfa;
        private double _alfa_m;
        private double _alfab;
        private double _alfaf;
        private double _alfakr;
        private double _alfash1;
        private double _alfash2;
        private double _zeta;

        
        private double _Lb;
        private double _fb;

        private double _m;
        private double _qobj;
        private double _q_d;
        private double _Kobj;

        private double _E;
        private double _Ep;
        private double _Eb;
        private double _Eb20;
        private double _E20;
        private double _Ekr;
        private double _Ekr20;

        private double _l0;
        private double _KGost34233_4;
        private double _beta;
        private double _betaT;
        private double _betaU;
        private double _betaY;
        private double _betaZ;
        private double _betaF;
        private double _betaV;
        private double _gamma;
        private double _f;
        private double _lambda;
        private double _deltakr;

        private double _sigma_dnb;
        private double _sigma_d_krm;
        private double _psi1;
        private double _Phi;
        private double _Phi_1;
        private double _Phi_2;

        private double _Xkr;
        private double _Kkr;
        private List<string> _errorList = new();

        public override string ToString() => $"Плоская крышка с дополнительным краевым моментом {_fbdi.Name}";

        public void Calculate()
        {
            _c = _fbdi.c1 + _fbdi.c2 + _fbdi.c3;

            if (_fbdi.IsFlangeIsolated)
            {
                _tf = _fbdi.t;
                _tb = _fbdi.t * 0.97;
            }
            else
            {
                _tf = _fbdi.t * 0.96;
                _tb = _fbdi.t * 0.95;
            }


            if (!Physical.TryGetE(_fbdi.FlangeSteel, 20, ref _E20, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }

            if (!Physical.TryGetE(_fbdi.FlangeSteel, _tf, ref _E, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }

            if (!Physical.TryGetAlfa(_fbdi.FlangeSteel, _fbdi.t, ref _alfaf, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }

            if (!Physical.TryGetE(_fbdi.CoverSteel, 20, ref _Ekr20, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }

            if (!Physical.TryGetE(_fbdi.CoverSteel, _fbdi.t, ref _Ekr, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }

            if (!Physical.TryGetAlfa(_fbdi.CoverSteel, _fbdi.t, ref _alfakr, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }


            // GOST 34233.4 add G
            _sigma_d_krm = _fbdi.sigma_d * 1.5 / 1.1;

            _tkr = _fbdi.t;
            _hkr = _fbdi.s2;
            _deltakr = _fbdi.s2;
            _dkr = _fbdi.Screwd;

            if (_fbdi.IsWasher)
            {
                if (!Physical.TryGetAlfa(_fbdi.WasherSteel, _tf, ref _alfash1, ref _errorList, "Gost34233_4"))
                {
                    IsCriticalError = true;
                    return;
                }
                _alfash2 = _alfash1;
            }

            if (!Physical.TryGetE(_fbdi.ScrewSteel, 20, ref _Eb20, ref _errorList, "GOST34233_4"))
            {
                IsCriticalError = true;
                return;
            }

            if (!Physical.TryGetE(_fbdi.ScrewSteel, _tb, ref _Eb, ref _errorList, "GOST34233_4"))
            {
                IsCriticalError = true;
                return;
            }

            if (!Physical.TryGetAlfa(_fbdi.ScrewSteel, _tb, ref _alfab, ref _errorList, "Gost34233_4"))
            {
                IsCriticalError = true;
                return;
            }

            if (!Physical.Gost34233_4.TryGetfb(_fbdi.Screwd, _fbdi.IsScrewWithGroove, ref _fb, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }

            if (!Physical.Gost34233_4.TryGetSigma(_fbdi.ScrewSteel, _tb, ref _sigma_dnb, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }



            _S0 = _fbdi.IsFlangeFlat ? _fbdi.s : _fbdi.S0;


            if (_isGasketMetal)
            {
                (_m, _qobj, _, _, _, _isGasketFlat, _isGasketMetal) = Physical.Gost34233_4.GetGasketParameters(_fbdi.GasketType);
            }
            else
            {
                (_m, _qobj, _q_d, _Kobj, _Ep, _isGasketFlat, _isGasketMetal) = Physical.Gost34233_4.GetGasketParameters(_fbdi.GasketType);

                if (_Ep == -1)
                {
                    _Ep = 0.00003 * (1 + _fbdi.bp / (2 * _fbdi.hp));
                }
                else if (_Ep == -2)
                {
                    _Ep = 0.00004 * (1 + _fbdi.bp / (2 * _fbdi.hp));
                }
            }

            if (_isGasketFlat)
            {
                _b0 = _fbdi.bp <= 15 ? _fbdi.bp : 3.8 * Math.Sqrt(_fbdi.bp);
                _Dcp = _fbdi.Dcp + _fbdi.bp - _b0;
            }
            else
            {
                _b0 = _fbdi.bp / 4;
                _Dcp = _fbdi.Dcp;
            }

            _Pobj = 0.5 * Math.PI * _Dcp * _b0 * _qobj;
            _Rp = _fbdi.IsPressureIn ? Math.PI * _Dcp * _b0 * _m * Math.Abs(_fbdi.p) : 0.0;

            _Ab = _fbdi.n * _fb;


            _Qd = 0.785 * _fbdi.p * Math.Pow(_Dcp, 2);


            _QFM = _fbdi.F + 4 * Math.Abs(_fbdi.M) / _Dcp;


            _b = 0.5 * (_fbdi.Db - _Dcp);

            _l0 = Math.Sqrt(_fbdi.D * _S0);
            _beta = _fbdi.S1 / _S0;
            _x = _fbdi.l / _l0;

            _zeta = 1 + (_beta - 1) * _x / (_x + (1 + _beta) / 4.0);
            _Se = 0.5 * (_zeta * _S0);


            _yp = _isGasketMetal
                ? 0 : _fbdi.hp * _Kobj / (_Ep * Math.PI * _Dcp * _fbdi.bp);

            _Lb = _fbdi.Lb0 + (_fbdi.IsStud ? 0.56 : 0.28) * _fbdi.Screwd;
            _yb = _Lb / (_Eb20 * _fb * _fbdi.n);

            _KGost34233_4 = _fbdi.Dn / _fbdi.D;

            _betaT = (Math.Pow(_KGost34233_4, 2) * (1 + 8.55 * Math.Log(_KGost34233_4)) - 1) /
                     (1.05 + 1.945 * Math.Pow(_KGost34233_4, 2) * (_KGost34233_4 - 1));
            _betaU = (Math.Pow(_KGost34233_4, 2) * (1 + 8.55 * Math.Log(_KGost34233_4)) - 1) /
                     (1.36 * (Math.Pow(_KGost34233_4, 2) - 1) * (_KGost34233_4 - 1));
            _betaY = 1 / (_KGost34233_4 - 1) *
                     (0.69 + 5.72 * Math.Pow(_KGost34233_4, 2) * Math.Log(_KGost34233_4) /
                         (Math.Pow(_KGost34233_4, 2) - 1));
            _betaZ = (Math.Pow(_KGost34233_4, 2) + 1) / (Math.Pow(_KGost34233_4, 2) - 1);


            _betaF = 0.91;
            _betaV = 0.55;
            _f = 1.0;
            //TODO: _betaF, _betaV, _f take values from diagram. how?

            _lambda = _betaF * _fbdi.h + _l0 / (_betaT * _l0) +
                      _betaV * Math.Pow(_fbdi.h, 3) / (_betaU * _l0 * Math.Pow(_S0, 2));
            _yF = 0.91 * _betaV / (_E20 * _lambda * Math.Pow(_S0, 2) * _l0);

            _yfn = Math.Pow(Math.PI / 4, 3) * _fbdi.Db / (_E20 * _fbdi.Dn * Math.Pow(_fbdi.h, 3));

            if (_fbdi.IsCoverFlat)
            {
                _Kkr = _fbdi.Dn / _Dcp;
                _Xkr = 0.67 * (Math.Pow(_Kkr, 2) * (1 + 8.55 * Math.Log(_Kkr)) - 1) /
                    ((_Kkr - 1) * (Math.Pow(_Kkr, 2) - 1 + (1.857 * Math.Pow(_Kkr, 2) + 1) * Math.Pow(_hkr, 3) / Math.Pow(_dkr, 3)));
                _ykr = _Xkr / (_Ekr20 * Math.Pow(_deltakr, 3));
            }
            else
            {
                //TODO: Ad
            }

            _gamma = 1 / (_yp + _yb * _Eb20 / _Eb + (_yF * _E20 / _E + _ykr * _Ekr20 / _Ekr) * Math.Pow(_b, 2));

            _Qt = _gamma * ((_alfaf * _fbdi.h + _alfash1 * _fbdi.hsh) * (_tf - 20) +
                (_alfakr * _hkr + _alfash2 * _fbdi.hsh) * (_tkr - 20) -
                _alfab * (_fbdi.h + _hkr) * (_tb - 20));


            _alfa = 1 - (_yp - (_yF * _e + _ykr * _b) * _b) /
                (_yp + _yb + (_yF + _ykr) * Math.Pow(_b, 2));

            _alfa_m = (_yb + 2 * _yfn * _b * (_b + _e - Math.Pow(_e, 2) / _Dcp)) /
                (_yb + _yp * Math.Pow(_fbdi.Db / _Dcp, 2) + 2 * _yfn * Math.Pow(_b, 2));



            _Pb1_1 = _alfa * (_Qd + _fbdi.F) + _Rp + 4 * _alfa_m * Math.Abs(_fbdi.M) / _Dcp;
            _Pb1_1 = _alfa * (_Qd + _fbdi.F) + _Rp + 4 * _alfa_m * Math.Abs(_fbdi.M) / _Dcp - _Qt;
            _Pb1 = Math.Max(_Pb1_1, _Pb1_2);

            _Pb2_2 = 0.4 * _Ab * _sigma_dnb;
            _Pb2 = Math.Max(_Pobj, _Pb2_2);

            _Pbm = Math.Max(_Pb1, _Pb2);

            _Pbp = _Pbm + (1 - _alfa) * (_Qd + _fbdi.F) + _Qt + 4 * (1 - _alfa_m) * Math.Abs(_fbdi.M) / _Dcp;

            _psi1 = _Pbp / _Qd;

            _K6 = _fbdi.IsCoverWithGroove
                ? 0.41 * Math.Sqrt((1.0 + 3.0 * _psi1 * (_fbdi.D3 / _Dcp - 1) + 9.6 * _fbdi.D3 / _Dcp * _fbdi.s4 / _Dcp) / (_fbdi.D3 / _Dcp))
                : 0.41 * Math.Sqrt((1.0 + 3.0 * _psi1 * (_fbdi.D3 / _Dcp - 1)) / (_fbdi.D3 / _Dcp));


            _Dp = _Dcp;
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
                        ErrorList.Add("Слишком много отверстий");
                    }
                    _K0 = Math.Sqrt((1 - Math.Pow(_fbdi.di / _Dp, 3)) / (1 - _fbdi.di / _Dp));
                    break;
                default:
                    IsError = true;
                    ErrorList.Add("Ошибка определения колличества отверстий");
                    break;
            }

            _s1p = _K0 * _K6 * _Dp * Math.Sqrt(_fbdi.p / (_fbdi.fi * _fbdi.sigma_d));
            _s1 = _s1p + _c;

            _Phi_1 = _Pbp / _fbdi.sigma_d;
            _Phi_2 = _Pbm / _sigma_d_krm;
            _Phi = Math.Max(_Phi_1, _Phi_2);

            _K7Fors2 = 0.8 * Math.Sqrt(_fbdi.D3 / _Dcp - 1);

            _s2p_1 = _K7Fors2 * Math.Sqrt(_Phi);
            _s2p_2 = 0.6 / _Dcp * _Phi;
            _s2p = Math.Max(_s2p_1, _s2p_2);
            _s2 = _s2p + _c;

            _K7Fors3 = 0.8 * Math.Sqrt(_fbdi.D3 / _fbdi.D2 - 1);

            _s3p_1 = _K7Fors3 * Math.Sqrt(_Phi);
            _s3p_2 = 0.6 / _Dcp * _Phi;
            _s3p = Math.Max(_s3p_1, _s3p_2);
            _s3 = _s3p + _c;



            if (_fbdi.s1 >= _s1 && _fbdi.s2 >= _s2 && _fbdi.s3 >= _s3)
            {
                _conditionUseFormulas = (_fbdi.s1 - _c) / _Dp;
                _isConditionUseFormulas = _conditionUseFormulas <= 0.11;
                _Kp = _isConditionUseFormulas
                    ? 1
                    : 2.2 / (1 + Math.Sqrt(1 + Math.Pow(6 * (_fbdi.s1 - _c) / _Dp, 2)));

                _p_d = Math.Pow((_fbdi.s1 - _c) / (_K0 * _K6 * _Dp), 2) * _fbdi.sigma_d * _fbdi.fi;
                if (_Kp * _p_d < _fbdi.p)
                {
                    IsError = true;
                    ErrorList.Add("Допускаемое давление меньше расчетного");
                }
            }
            else
            {
                IsCriticalError = true;
                ErrorList.Add("Принятая толщина s1, s2, s3 меньше расчетной");
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

            body.AddParagraph($"Расчет на прочность плоской круглой крышки {_fbdi.Name} с дополнительным краевым моментом")
                .Heading(HeadingType.Heading1)
                .Alignment(AlignmentType.Center);

            body.AddParagraph("");

            {
                var imagePart = mainPart.AddImagePart(ImagePartType.Gif);

                byte[] bytes = _fbdi.IsCoverWithGroove
                    ? Data.Properties.Resources.FlatBottomWithMomentWithGroove
                    : Data.Properties.Resources.FlatBottomWithMoment;

                imagePart.FeedData(new MemoryStream(bytes));

                body.AddParagraph("").AddImage(mainPart.GetIdOfPart(imagePart), bytes);
            }

            {
                var imagePart = mainPart.AddImagePart(ImagePartType.Gif);

                var type = _fbdi.IsFlangeFlat ? "f21_" : "fl1_";

                string type1 = "";

                switch (_fbdi.FlangeFace)
                {
                    case FlangeFaceType.Flat:
                        type1 = "a";
                        break;
                    case FlangeFaceType.MaleFemale:
                        type1 = "b";
                        break;
                    case FlangeFaceType.TongueGroove:
                        type1 = "c";
                        break;
                    case FlangeFaceType.Ring:
                        type1 = "d";
                        break;
                }

                var bytes =(byte[])Data.Properties.Resources.ResourceManager.GetObject(type + type1);

                if (bytes != null)
                {
                    imagePart.FeedData(new MemoryStream(bytes));
                    body.Elements<Paragraph>().LastOrDefault().AddImage(mainPart.GetIdOfPart(imagePart), bytes);
                }
            }

            body.AddParagraph("Исходные данные").Alignment(AlignmentType.Center);

            //table
            {
                var table = body.AddTable();

                table.AddRowWithOneCell("Крышка", gridSpanCount: 2);

                table.AddRow()
                    .AddCell("Марка стали")
                    .AddCell($"{_fbdi.CoverSteel}");

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
                    .AddCell(_fbdi.IsCoverWithGroove
                    ? "Исполнительная толщина плоской крышки в месте паза для перегородки, "
                    : "Исполнительная толщина крышки, ")
                    .AppendEquation("s_1")
                    .AppendText(":")
                    .AddCell($"{_fbdi.s1} мм");

                table.AddRow()
                    .AddCell("Исполнительная толщина плоской крышки в зоне уплотнения, ")
                    .AppendEquation("s_2")
                    .AppendText(":")
                    .AddCell($"{_fbdi.s2} мм");

                table.AddRow()
                    .AddCell("Толщина крышки вне уплотнения, ")
                    .AppendEquation("s_3")
                    .AppendText(":")
                    .AddCell($"{_fbdi.s3} мм");

                if (_fbdi.IsCoverWithGroove)
                {
                    table.AddRow()
                    .AddCell("Ширина паза под перегородку, ")
                    .AppendEquation("s_4")
                    .AppendText(":")
                    .AddCell($"{_fbdi.s4} мм");
                }

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
                    .AddCell("Отверстия в крышке")
                    .AddCell(_fbdi.Hole == HoleInFlatBottom.WithoutHole ? "нет" : "есть");

                switch (_fbdi.Hole)
                {
                    case HoleInFlatBottom.OneHole:
                        table.AddRow()
                            .AddCell("Диаметр отверстия в крышке, d:")
                            .AddCell($"{_fbdi.d} мм");
                        break;
                    case HoleInFlatBottom.MoreThenOneHole:
                        table.AddRow()
                            .AddCell("Диаметр отверстий в крышке, ")
                            .AppendEquation("d_i")
                            .AppendText(":")
                            .AddCell($"{_fbdi.di} мм");
                        break;
                }


                table.AddRowWithOneCell("Фланец");

                table.AddRow()
                    .AddCell("Марка стали")
                    .AddCell($"{_fbdi.FlangeSteel}");

                table.AddRow()
                    .AddCell("Тип фланца")
                    .AddCell(_fbdi.IsFlangeFlat
                    ? "Плоский приварной"
                    : "Приварной встык");

                switch (_fbdi.FlangeFace)
                {
                    case FlangeFaceType.Flat:
                        table.AddRow()
                            .AddCell("Тип уплотнительной поверхности")
                            .AddCell("Плоская");
                        break;
                    case FlangeFaceType.MaleFemale:
                        table.AddRow()
                            .AddCell("Тип уплотнительной поверхности")
                            .AddCell("Выступ - впадина");
                        break;
                    case FlangeFaceType.TongueGroove:
                        table.AddRow()
                            .AddCell("Тип уплотнительной поверхности")
                            .AddCell("Шип - паз");
                        break;
                    case FlangeFaceType.Ring:
                        table.AddRow()
                            .AddCell("Тип уплотнительной поверхности")
                            .AddCell("Под прокладку овального сечения");
                        break;
                }

                table.AddRow()
                    .AddCell("Наружный диаметр фланца, ")
                    .AppendEquation("D_н")
                    .AppendText(":")
                    .AddCell($"{_fbdi.Dn} мм");

                table.AddRow()
                    .AddCell("Диаметр болтовой окружности, ")
                    .AppendEquation("D_б")
                    .AppendText(":")
                    .AddCell($"{_fbdi.Db} мм");

                table.AddRow()
                    .AddCell("Внутренний диаметр фланца, D:")
                    .AddCell($"{_fbdi.D} мм");

                if (_fbdi.IsFlangeFlat)
                {
                    table.AddRow()
                        .AddCell("Толщина стенки обечайки, s:")
                        .AddCell($"{_fbdi.s} мм");
                }
                else
                {
                    table.AddRow()
                    .AddCell("Толщина втулки приварного встык фланца в месте приварки к обечайке, ")
                    .AppendEquation("S_0")
                    .AppendText(":")
                    .AddCell($"{_fbdi.S0} мм");

                    table.AddRow()
                    .AddCell("Толщина втулки приварного встык фланца в месте присоединения к тарелке, ")
                    .AppendEquation("S_1")
                    .AppendText(":")
                    .AddCell($"{_fbdi.S1} мм");

                    table.AddRow()
                    .AddCell("Длина конической втулки приварного встык фланца, l:")
                    .AddCell($"{_fbdi.l} мм");
                }


                table.AddRowWithOneCell("Прокладка");


                table.AddRow()
                    .AddCell("Материал")
                    .AddCell($"{_fbdi.GasketType}");

                table.AddRow()
                    .AddCell("Ширина прокладки, ")
                    .AppendEquation("b_п")
                    .AppendText(":")
                    .AddCell($"{_fbdi.bp} мм");

                table.AddRow()
                .AddCell("Толщина прокладки, ")
                .AppendEquation("h_п")
                .AppendText(":")
                .AddCell($"{_fbdi.hp} мм");

                table.AddRow()
                .AddCell("Средний диаметр прокладки, ")
                .AppendEquation("D_сп")
                .AppendText(":")
                .AddCell($"{_fbdi.Dcp} мм");


                table.AddRowWithOneCell("Крепеж");


                table.AddRow()
                    .AddCell("Материал")
                    .AddCell($"{_fbdi.ScrewSteel}");

                table.AddRow()
                    .AddCell("Наружный диаметр крепежа, d:")
                    .AddCell($"{_fbdi.Screwd} мм");

                table.AddRow()
                    .AddCell("Колличество, n:")
                    .AddCell($"{_fbdi.n} мм");

                table.AddRow()
                    .AddCell("Вид крепежа:")
                    .AddCell(_fbdi.IsStud ? "Шпилька" : "Болт");

                table.AddRow()
                    .AddCell("Проточка:")
                    .AddCell(_fbdi.IsScrewWithGroove ? "Есть" : "Нет");

                table.AddRow()
                    .AddCell("Шайба:")
                    .AddCell(_fbdi.IsWasher ? "Есть" : "Нет");

                if (_fbdi.IsWasher)
                {
                    table.AddRow()
                    .AddCell("Материал")
                    .AddCell($"{_fbdi.WasherSteel}");

                    table.AddRow()
                        .AddCell("Толщина шайбы, ")
                        .AppendEquation("h_ш")
                        .AppendText(":")
                        .AddCell($"{_fbdi.hsh} мм");
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
                    .AddCell("Расчетная температура, Т:")
                    .AddCell($"{_fbdi.t} °С");

                table.AddRow()
                    .AddCell("Расчетное " +
                             (_fbdi.IsPressureIn
                             ? "внутреннее избыточное"
                             : "наружное") + " давление, p:")
                    .AddCell($"{_fbdi.p} МПа");

                body.InsertTable(table);
            }

            body.AddParagraph("");
            body.AddParagraph("Физические параметры материала")
                .Alignment(AlignmentType.Center);

            //table
            {
                var table = body.AddTable();

                table.AddRowWithOneCell("Крышка", gridSpanCount: 2);

                table.AddRow()
                    .AddCell($"Допускаемое напряжение для материала {_fbdi.CoverSteel} при расчетной температуре, [σ]:")
                    .AddCell($"{_fbdi.sigma_d} МПа");

                table.AddRow()
                    .AddCell("Модуль продольной упругости при температуре 20°C, ")
                    .AppendEquation("E^20")
                    .AppendText(":")
                    .AddCell($"{_Ekr:f2} МПа");

                table.AddRow()
                    .AddCell("Модуль продольной упругости при расчетной температуре, E:")
                    .AddCell($"{_Ekr:f2} МПа");

                table.AddRow()
                    .AddCell("Коэффициент линейного расширения, ")
                    .AppendEquation("α")
                    .AppendText(":")
                    .AddCell($"{_alfakr:f7} ")
                    .AppendEquation("°C^-1");


                table.AddRowWithOneCell("Фланец");

                table.AddRow()
                    .AddCell("Модуль продольной упругости при температуре 20°C, ")
                    .AppendEquation("E^20_ф")
                    .AppendText(":")
                    .AddCell($"{_E20:f2} МПа");

                table.AddRow()
                    .AddCell("Модуль продольной упругости при расчетной температуре, ")
                    .AppendEquation("E_ф")
                    .AppendText(":")
                    .AddCell($"{_E:f2} МПа");

                table.AddRow()
                    .AddCell("Коэффициент линейного расширения, ")
                    .AppendEquation("α_ф")
                    .AppendText(":")
                    .AddCell($"{_alfa:f7} ")
                    .AppendEquation("°C^-1");


                table.AddRowWithOneCell("Крепеж");

                table.AddRow()
                    .AddCell($"Допускаемое напряжение при расчетной температуре, ")
                    .AppendEquation("[σ]_б")
                    .AppendText(":")
                    .AddCell($"{_sigma_dnb:f1} МПа");

                table.AddRow()
                    .AddCell("Модуль продольной упругости при температуре 20C°, ")
                    .AppendEquation("E^20_б")
                    .AppendText(":")
                    .AddCell($"{_Eb20:f2} МПа");

                table.AddRow()
                    .AddCell("Модуль продольной упругости при расчетной температуре, ")
                    .AppendEquation("E_б")
                    .AppendText(":")
                    .AddCell($"{_Eb:f2} МПа");

                table.AddRow()
                    .AddCell("Коэффициент линейного расширения,")
                    .AppendEquation("α_б")
                    .AppendText(":")
                    .AddCell($"{_alfab:f7} ")
                    .AppendEquation("°C^-1");

                if (_fbdi.IsWasher)
                {
                    table.AddRowWithOneCell("Шайба");

                    table.AddRow()
                        .AddCell("Коэффициент линейного расширения,")
                        .AppendEquation("α_ш")
                        .AppendText(":")
                        .AddCell($"{_alfash1:f7} ")
                        .AppendEquation("°C^-1");
                }

                body.InsertTable(table);
            }

            body.AddParagraph("");
            body.AddParagraph("Результаты расчета").Alignment(AlignmentType.Center);
            body.AddParagraph("");

            body.AddParagraph("Толщину плоской круглой крышки с дополнительным краевым моментом под действием внутреннего давления вычисляют по формуле");
            body.AddParagraph("").AppendEquation("s_1≥s_1p+c");
            body.AddParagraph("где ");
            body.AddParagraph("").AppendEquation("s_1p=K_0∙K_6∙D_p∙√(p/(φ∙[σ]))");
            body.AddParagraph("где ");

            body.AddParagraph("").AppendEquation($"D_p=D_сп={_Dcp:f2} мм");

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
                       .AddRun(" - для крышекек, имеющих одно отверстие, вычисляют по формул");
                    body.AddParagraph("")
                        .AppendEquation("K_0=√(1+d/D_p+(d/D_p)^2)" +
                        $"=√(1+{_fbdi.d}/{_Dp:f2}+({_fbdi.d}/{_Dp:f2})^2)={_K0:f2}");
                    break;
                case HoleInFlatBottom.MoreThenOneHole:
                    body.AddParagraph("Коэффициент ")
                       .AppendEquation("K_0")
                       .AddRun(" - для крышекек, имеющих несколько отверстий, вычисляют по формул");
                    body.AddParagraph("")
                        .AppendEquation("K_0=√((1-(Σd_i/D_p)^3)/(1-(Σd_i/D_p)))" +
                        $"=√((1-({_fbdi.di}/{_Dp:f2})^3)/(1-({_fbdi.di}/{_Dp:f2})))={_K0:f2}");
                    break;
            }

            body.AddParagraph("Коэффициент ")
                        .AppendEquation("K_6")
                        .AddRun(" вычисляют по формуле");
            if (_fbdi.IsCoverWithGroove)
            {
                body.AddParagraph("")
                            .AppendEquation("K_6=0.41∙√((1+3∙ψ_1∙(D_3/D_сп-1)+9.6∙D_3/D_сп∙s_4/D_сп)/(D_3/D_сп))");
            }
            else
            {
                body.AddParagraph("")
                            .AppendEquation("K_6=0,41∙√((1+3∙ψ_1∙(D_3/D_сп-1))/(D_3/D_сп))");
            }
            
            body.AddParagraph("Значение ")
                        .AppendEquation("ψ_1")
                        .AddRun(" вычисляют по формуле");
            body.AddParagraph("")
                        .AppendEquation("ψ_1=P_б^p/Q_д");
            body.AddParagraph("")
                        .AppendEquation($"P_б^p={_Pbp:f2} H")
                        .AddRun(" расчетная нагрузка на " +
                        (_fbdi.IsStud ? "шпильки" : "болты") +
                        " фланцевых соединений, определяют по ГОСТ 34233.4 для рабочих условий");
            body.AddParagraph("")
                        .AppendEquation($"Q_д=0.785∙p∙D_сп^2=0.785∙{_fbdi.p}∙{_Dcp:f2}^2={_Qd:f2} H");

            body.AddParagraph("")
                        .AppendEquation($"ψ_1={_Pbp:f2}/{_Qd:f2}={_psi1:f2}");

            if (_fbdi.IsCoverWithGroove)
            {
                body.AddParagraph("")
                        .AppendEquation($"K_6=0,41∙√((1+3∙{_psi1:f2}({_fbdi.D3}/{_Dcp:f2}-1)+9.6∙{_fbdi.D3}/{_Dcp:f2}∙{_fbdi.s4}/{_Dcp:f2})/({_fbdi.D3}/{_Dcp:f2}))={_K6:f2}");
            }
            else
            {
                body.AddParagraph("")
                        .AppendEquation($"K_6=0,41∙√((1+3∙{_psi1:f2}({_fbdi.D3}/{_Dcp:f2}-1))/({_fbdi.D3}/{_Dcp:f2}))={_K6:f2}");
            }

            body.AddParagraph("").AppendEquation($"s_1p={_K0:f2}∙{_K6:f2}∙{_Dp:f2}∙√({_fbdi.p}/({_fbdi.fi}∙{_fbdi.sigma_d}))={_s1p:f2} мм");

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


            body.AddParagraph("Толщину плоской круглой крышки с дополнительным краевым моментом в месте уплотнения вычисляют по формуле");
            body.AddParagraph("").AppendEquation("s_2≥s_2p+c");
            body.AddParagraph("где ");
            body.AddParagraph("").AppendEquation("s_2p=max{K_7∙√(Φ);0.6/D_сп∙Φ}");
            body.AddParagraph("где ");

            body.AddParagraph("").AppendEquation("Φ=max{P_б^p/[σ]^p;P_б^м/[σ]^м}" +
                $"=max{{{_Pbp:f2}/{_fbdi.sigma_d};{_Pbm:f2}/{_sigma_d_krm:f1}}}" +
                $"=max{{{_Phi_1:f2};{_Phi_2:f2}}}={_Phi:f2}");

            body.AddParagraph("Коэффициент ")
                        .AppendEquation("K_7")
                        .AddRun(" вычисляют по формуле");

            body.AddParagraph("")
                        .AppendEquation($"K_7=0.8∙√(D_3/D_сп-1)=0.8∙√({_fbdi.D3}/{_Dcp:f2}-1)={_K7Fors2:f2}");

            body.AddParagraph("").AppendEquation($"K_7∙√(Φ)={_K7Fors2:f2}∙√({_Phi:f2})={_s2p_1:f2}");
            body.AddParagraph("").AppendEquation($"0.6/D_сп∙Φ=0.6/{_Dcp:f2}∙{_Phi:f2}={_s2p_2:f2}");

            body.AddParagraph("")
                .AppendEquation($"s_2p=max{{{_s2p_1:f2};{_s2p_2:f2}}}={_s2p:f2} мм");


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


            body.AddParagraph("Толщину плоской круглой крышки с дополнительным краевым моментом вне зоны уплотнения вычисляют по формуле");
            body.AddParagraph("").AppendEquation("s_3≥s_3p+c");
            body.AddParagraph("где ");
            body.AddParagraph("").AppendEquation("s_3p=max{K_7∙√(Φ);0.6/D_2∙Φ}");

            body.AddParagraph("Коэффициент ")
                        .AppendEquation("K_7")
                        .AddRun(" вычисляют по формуле");

            body.AddParagraph("")
                        .AppendEquation($"K_7=0.8∙√(D_3/D_2-1)=0.8∙√({_fbdi.D3}/{_fbdi.D2}-1)={_K7Fors3:f2}");

            body.AddParagraph("").AppendEquation($"K_7∙√(Φ)={_K7Fors3:f2}∙√({_Phi:f2})={_s3p_1:f2}");
            body.AddParagraph("").AppendEquation($"0.6/D_2∙Φ=0.6/{_fbdi.D2}∙{_Phi:f2}={_s3p_2:f2}");

            body.AddParagraph("")
                .AppendEquation($"s_2p=max{{{_s3p_1:f2};{_s3p_2:f2}}}={_s3p:f2} мм");


            body.AddParagraph("").AppendEquation($"s_2={_s3p:f2}+{_c:f2}={_s3:f2} мм");

            if (_fbdi.s3 > _s3)
            {
                body.AddParagraph("Принятая толщина ")
                    .AppendEquation($"s_3={_fbdi.s3} мм")
                    .Bold();
            }
            else
            {
                body.AddParagraph($"Принятая толщина ")
                    .AppendEquation($"s_3={_fbdi.s3} мм")
                    .Bold()
                    .Color(System.Drawing.Color.Red);
            }

            body.AddParagraph("Допускаемое давление вычисляют по формуле:");
            body.AddParagraph("")
                .AppendEquation("[p]=((s_1-c)/(K_0∙K_6∙D_p))^2∙[σ]∙φ"
                                + $"=(({_fbdi.s1}-{_c:f2})/({_K0:f2}∙{_K6:f2}∙{_Dp:f2}))^2∙{_fbdi.sigma_d}∙{_fbdi.fi}"
                                + $"={_p_d:f2} МПа");
            
            body.AddParagraph("Условия применения расчетных формул ");
            body.AddParagraph("")
                .AppendEquation($"(s_1-c)/D_p=({_fbdi.s1}-{_c:f2})/{_Dp:f2}={_conditionUseFormulas}≤0.11");
            if (_isConditionUseFormulas)
            {
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

            package.Close();
        }
    }
}
