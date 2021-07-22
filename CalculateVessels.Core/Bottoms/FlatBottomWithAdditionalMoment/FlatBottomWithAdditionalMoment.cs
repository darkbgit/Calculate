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

        public List<string> ErrorList { get; private set; } = new();

        public List<string> Bibliography { get; } = new()
        {
            Data.Properties.Resources.GOST_34233_1,
            Data.Properties.Resources.GOST_34233_2
        };

        public double PressurePermissible { get => _p_d; }

        private double _Ab;

        private double _b;
        private double _b0;

        private double _c;

        private double _dkr;
        private double _Dcp;

        private double _e;

        private bool _isGasketFlat;
        private bool _isGasketMetal;

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
        private double _psi1;
        private double _Phi;
        private double _Phi_1;
        private double _Phi_2;

        private double _Xkr;
        private double _Kkr;


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


            _E20 = Physical.Gost34233_4.GetE(_fbdi.FlangeSteel, 20);
            _E = Physical.Gost34233_4.GetE(_fbdi.FlangeSteel, _tf);
            _alfaf = Physical.Gost34233_4.GetAlfa(_fbdi.FlangeSteel, _fbdi.t);

            List<string> errorList = new();
            _Ekr20 = Physical.Gost34233_1.GetE(_fbdi.CoverSteel, 20, ref errorList);
            _Ekr = Physical.Gost34233_1.GetE(_fbdi.CoverSteel, _fbdi.t, ref errorList);
            _alfakr = Physical.GetAlfa(_fbdi.CoverSteel, _fbdi.t);

            _tkr = _fbdi.t;
            _hkr = _fbdi.s2;
            _deltakr = _fbdi.s2;
            _dkr = _fbdi.Screwd;

            if (_fbdi.IsWasher)
            {
                _alfash1 = Physical.Gost34233_4.GetAlfa(_fbdi.WasherSteel, _tf);
                _alfash2 = _alfash1;
            }

            _Eb20 = Physical.Gost34233_4.GetE(_fbdi.ScrewSteel, 20);
            _Eb = Physical.Gost34233_4.GetE(_fbdi.ScrewSteel, _tb);
            _alfab = Physical.Gost34233_4.GetAlfa(_fbdi.ScrewSteel, _tb);
            _fb = Physical.Gost34233_4.Getfb(_fbdi.Screwd, _fbdi.IsScrewWithGroove);
            _sigma_dnb = Physical.Gost34233_4.GetSigma(_fbdi.ScrewSteel, _tb);


            _S0 = _fbdi.IsFlangeFlat ? _fbdi.s : _fbdi.S0;


            if (errorList.Any())
            {
                IsCriticalError = true;
                ErrorList = ErrorList.Concat(errorList).ToList();
                return;
            }

            if (_fb == 0 || _Eb20 == 0)
            {
                IsCriticalError = true;
                ErrorList.Add("Ошибка получения значений физических велечин");
                return;
            }

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
                _Dcp = _fbdi.Dnp - _b0;
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
            _Phi_2 = _Pbm / _fbdi.sigma_d;
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
                _Kp = _conditionUseFormulas <= 0.11
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

                var stream = new MemoryStream(_fbdi.IsCoverWithGroove ?
                    Data.Properties.Resources.FlatBottomWithMomentWithGroove
                    : Data.Properties.Resources.FlatBottomWithMoment);
                imagePart.FeedData(stream);

                body.AddParagraph("").AddImage(mainPart.GetIdOfPart(imagePart));
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

                var stream = Data.Properties.Resources.ResourceManager.GetObject(type + type1 + "52857");
                imagePart.FeedData(stream);

                body.AddParagraph("").AddImage(mainPart.GetIdOfPart(imagePart));
        }

//        body.AddParagraph("Исходные данные").Alignment(AlignmentType.Center);

//            //table
//            {
//                var table = body.AddTable();

//                table.AddRow()
//                    .AddCell("Внутренний диаметр обечайки, D:")
//                    .AddCell($"{_fbdi.D} мм");

//                table.AddRow()
//                    .AddCell("Толщина стенки обечайки, s:")
//                    .AddCell($"{_fbdi.s} мм");

//                table.AddRow()
//                    .AddCell("Прибавка к расчетной толщине, c:")
//                    .AddCell($"{_fbdi.c} мм");

//                table.AddRow()
//                    .AddCell("Длина обечайки, ")
//                    .AppendEquation("L_ob")
//                    .AppendText(":")
//                    .AddCell($"{_fbdi.Lob} мм");

//                table.AddRow()
//                    .AddCell("Коэффициент прочности сварного шва, φ:")
//                    .AddCell($"{_fbdi.fi}");

//                table.AddRow()
//                    .AddCell("Марка стали")
//                    .AddCell($"{_fbdi.Steel}");

//                table.AddRow()
//                    .AddCell("Ширина опоры, b:")
//                    .AddCell($"{_fbdi.b} мм");

//                table.AddRow()
//                    .AddCell("Угол охвата опоры, ")
//                    .AppendEquation("δ_1")
//                    .AppendText(":")
//                    .AddCell($"{_fbdi.delta1} °");

//                table.AddRow()
//                    .AddCell("Длина свободно выступающей части, e:")
//                    .AddCell($"{_fbdi.e} мм");

//                table.AddRow()
//                    .AddCell("Длина выступающей цилиндрической части сосуда, включая отбортовку днища, a")
//                    .AddCell($"{_fbdi.a} мм");

//                table.AddRow()
//                    .AddCell("Высота опоры, Н")
//                    .AddCell($"{_fbdi.H} мм");
//                if (_fbdi.Type == SaddleType.SaddleWithoutRingWithSheet)
//                {
//                    table.AddRow()
//                        .AddCell("Толщина подкладного листа, ")
//                        .AppendEquation("s_2")
//                        .AppendText(":")
//                        .AddCell($"{_fbdi.s2} мм");

//                    table.AddRow()
//                        .AddCell("Ширина подкладного листа, мм")
//                        .AppendEquation("b_2")
//                        .AppendText(":")
//                        .AddCell($"{_fbdi.b2} мм");

//                    table.AddRow()
//                        .AddCell("Угол охвата подкладного листа, ")
//                        .AppendEquation("δ_2")
//                        .AppendText(":")
//                        .AddCell($"{_fbdi.delta2} °");
//    }
//    body.InsertTable(table);
//            }

//body.AddParagraph("");
//            body.AddParagraph("Условия нагружения")
//                .Alignment(AlignmentType.Center);

//            //table
//            {
//                var table = body.AddTable();

//                table.AddRow()
//                    .AddCell("Собственный вес с содержимым, G:")
//                    .AddCell($"{_fbdi.G} H");

//                table.AddRow()
//                    .AddCell("Расчетная температура, Т:")
//                    .AddCell($"{_fbdi.Temp} °С");

//                table.AddRow()
//                    .AddCell("Расчетное " +
//                             (_fbdi.IsPressureIn
//                             ? "внутреннее избыточное"
//                             : "наружное") + " давление, p:")
//                    .AddCell($"{_fbdi.p} МПа");

//                table.AddRow()
//                    .AddCell($"Допускаемое напряжение для материала {_fbdi.Steel} при расчетной температуре, [σ]:")
//                    .AddCell($"{_fbdi.sigma_d} МПа");

//                if (!_fbdi.IsPressureIn)
//                {
//                    table.AddRow()
//                        .AddCell("Модуль продольной упругости при расчетной температуре, E:")
//                        .AddCell($"{_fbdi.E} МПа");
//                }
//                body.InsertTable(table);
//            }

//            body.AddParagraph("");
//            body.AddParagraph("Результаты расчета").Alignment(AlignmentType.Center);
//            body.AddParagraph("");


//            body.AddParagraph("Расчетные параметры").Alignment(AlignmentType.Center);
//            body.AddParagraph("");

//            body.AddParagraph("Распределенная весовая нагрузка");
//            body.AddParagraph("")
//                .AppendEquation("q=G/(L+4/3∙H)" +
//                    $"={_fbdi.G}/({_fbdi.L}+4/3∙{_fbdi.H})={_q:f2} Н/мм");

//            body.AddParagraph("Расчетный изгибающий момент, действующий на консольную часть обечайки");
//            body.AddParagraph("")
//                .AppendEquation("M_0=q∙D^2/16" +
//                                $"={_q:f2}∙{_fbdi.D}^2/16={_M0:f2} Н∙мм");

//            body.AddParagraph("Опорное усилие");
//            body.AddParagraph("")
//                .AppendEquation("F_1=F_2=G/2" +
//                                $"={_fbdi.G}/2={_F1:f2} H");

//            body.AddParagraph("Изгибающий момент над опорами");
//            body.AddParagraph("")
//                .AppendEquation("M_1=M_2=(q∙e^2)/2-M_0" +
//                                $"=({_q:f2}∙{_fbdi.e:f2}^2)/2-{_M0:f2}={_M1:f2} Н∙мм");

//            body.AddParagraph("Максимальный изгибающий момент между опорами");
//            body.AddParagraph("")
//                .AppendEquation("M_12=M_0+F_1∙(L/2-a)-q/2∙(L/2+2/3∙H)^2" +
//                                $"={_M0:f2}+{_F1:f2}∙({_fbdi.L}/2-{_fbdi.a})-{_q:f2}/2∙({_fbdi.L}/2+2/3∙{_fbdi.H})^2={_M12:f2} Н∙мм");

//            body.AddParagraph("Поперечное усилие в сечении оболочки над опорой");
//            body.AddParagraph("")
//                .AppendEquation("Q_1=Q_2=(L-2∙a)/(L+4/3∙H)∙F_1" +
//                                $"=({_fbdi.L}-2∙{_fbdi.a})/({_fbdi.L}+4/3∙{_fbdi.H})∙{_F1:f2}={_Q1:f2} H");

//            body.AddParagraph("Несущую способность обечайки в сечении между опорами следует проверять при условии");
//            body.AddParagraph("").AppendEquation("max{M_12}>max{M_1}");
//            body.AddParagraph("").AppendEquation($"{_M12:f2} Н∙мм > {_M1:f2} Н∙мм");
//            if (_M12 > _M1)
//            {
//                body.AddParagraph("Проверка несущей способности обечайки в сечении между опорами");
//                body.AddParagraph("Условие прочности");
//                body.AddParagraph("").AppendEquation("(p∙D)/(4∙(s-c))+(4∙M_12∙K_9)/(π∙D^2∙(s-c))≤[σ]∙φ");
//                body.AddParagraph("где ")
//                    .AppendEquation("K_9")
//                    .AddRun(" - коэффициент, учитывающий частичное заполнение жидкостью");
//                body.AddParagraph("")
//                    .AppendEquation("K_9=max{[1.6-0.20924∙(x-1)+0.028702∙x∙(x-1)+0.4795∙10^3∙y∙(x-1)-0.2391∙10^-6∙x∙y∙(x-1)-0.29936∙10^-2∙(x-1)∙x^2-0.85692∙10^-6∙(х-1)∙у^2+0.88174∙10^-6∙х^2∙(х-1)∙у-0.75955∙10^-8∙у^2∙(х-1)∙х+0.82748∙10^-4∙(х-1)∙х^3+0.48168∙10^-9∙(х-1)∙у^3];1}");
//                body.AddParagraph("где ").AppendEquation("y=D/(s-c);x=L/D");
//                body.AddParagraph("")
//                    .AppendEquation($"y={_fbdi.D}/({_fbdi.s}-{_fbdi.c})={_y:f2}");
//                body.AddParagraph("")
//                    .AppendEquation($"x={_fbdi.L}/{_fbdi.D}={_x:f2}");

//                body.AddParagraph("").AppendEquation($"K_9=max({_K9_1:f2};1)={_K9:f2}");

//                body.AddParagraph("")
//                    .AppendEquation($"(p∙D)/(4∙(s-c))+(4∙M_12∙K_9)/(π∙D^2∙(s-c))=({_fbdi.p}∙{_fbdi.D})/(4∙({_fbdi.s}-{_fbdi.c}))+(4∙{_M12:f2}∙{_K9:f2})/(π∙{_fbdi.D}^2∙({_fbdi.s}-{_fbdi.c}))={_conditionStrength1_1:f2}");
//                body.AddParagraph("")
//                    .AppendEquation($"[σ]∙φ={_fbdi.sigma_d}∙{_fbdi.fi}={_conditionStrength1_2:f2}");
//                body.AddParagraph("")
//                    .AppendEquation($"{_conditionStrength1_1:f2}≤{_conditionStrength1_2:f2}");
//                if (_conditionStrength1_1 <= _conditionStrength1_2)
//                {
//                    body.AddParagraph("Условие прочности выполняется");
//                }
//                else
//                {
//                    body.AddParagraph("Условие прочности не выполняется")
//                        .Bold()
//                        .Color(System.Drawing.Color.Red);
//                }
//                body.AddParagraph("Условие устойчивости");
//                body.AddParagraph("").AppendEquation("|M_12|/[M]≤1");

//                body.AddParagraph("где [M] - допускаемый изгибающий момент из условия устойчивости");
//                body.AddParagraph("")
//                    .AppendEquation("[M]=(8.9∙10^-5∙E)/n_y∙D^3∙[(100∙(s-c))/D]^2.5" +
//                                    $"=(8.9∙10^-5∙{_fbdi.E})/{_fbdi.ny}∙{_fbdi.D}^3∙[(100∙({_fbdi.s}-{_fbdi.c}))/{_fbdi.D}]^2.5={_M_d:f2} Н∙мм");
//                body.AddParagraph("").AppendEquation($"|{_M12:f2}|/{_M_d:f2}={_conditionStability1:f2}≤1");

//                if (_conditionStability1 <= 1)
//                {
//                    body.AddParagraph("Условие устойчивости выполняется");
//                }
//                else
//                {
//                    body.AddParagraph("Условие устойчивости не выполняется")
//                        .Bold()
//                        .Color(System.Drawing.Color.Red);
//                }
//            }
//            else
//            {
//                body.AddParagraph("Проверка несущей способности обечайки в сечении между опорами не требуется");
//            }

//            switch (_fbdi.Type)
//            {
//                case SaddleType.SaddleWithoutRingWithoutSheet:
//                    {
//                        body.AddParagraph("Проверка несущей способности обечайки, не укрепленной кольцами жесткости в области опорного узла и без подкладного листа в месте опоры");
//                        body.AddParagraph("Вспомогательные параметры и коэффициенты");
//                        body.AddParagraph("Параметр, определяемый расстоянием от середины опоры до днища");
//                        body.AddParagraph("")
//                            .AppendEquation("γ=2.83∙a/D∙√((s-c)/D)" +
//                                            $"=2.83∙{_fbdi.a}/{_fbdi.D}∙√(({_fbdi.s}-{_fbdi.c})/{_fbdi.D})={_gamma:f2}");

//                        body.AddParagraph("Параметр, определяемый шириной пояса опоры");
//                        body.AddParagraph("")
//                            .AppendEquation("β_1=0.91∙b/√(D∙(s-c))" +
//                                            $"=0.91∙{_fbdi.b}/√({_fbdi.D}∙({_fbdi.s}-{_fbdi.c}))={_beta1:f2}");

//                        body.AddParagraph("Коэффициенты, учитывающие влияние ширины пояса опоры");
//                        body.AddParagraph("")
//                            .AppendEquation("K_10=max{(exp(-β_1)∙sin(β_1))/β_1;0.25}" +
//                                            $"=max{{(exp(-{_beta1:f2})∙sin({_beta1:f2}))/{_beta1:f2};0.25}}" +
//                                            $"=max({_K10_1:f2};0.25)={_K10:f2}");

//                        body.AddParagraph("")
//                            .AppendEquation("K_11=(1-exp(-β_1)∙cos(β_1))/β_1" +
//                                            $"=(1-exp(-{_beta1:f2})∙cos({_beta1:f2}))/{_beta1:f2}={_K11:f2}");

//                        body.AddParagraph("Коэффициенты, учитывающие влияние угла охвата");
//                        body.AddParagraph("")
//                            .AppendEquation("K_12=(1.15-0.1432∙δ_1)/sin(0.5∙δ_1)" +
//                                            $"=(1.15-0.1432∙{DegToRad(_fbdi.delta1):f2})/sin(0.5∙{DegToRad(_fbdi.delta1):f2})={_K12:f2}");

//                        body.AddParagraph("")
//                            .AppendEquation("K_13=(max{1.7-(2.1∙δ_1)/π;0})/sin(0.5∙δ_1)" +
//                                            $"=(max{{1.7 - (2.1∙{DegToRad(_fbdi.delta1):f2})/π;0}})/sin(0.5∙{DegToRad(_fbdi.delta1):f2})={_K13:f2}");

//                        body.AddParagraph("")
//                            .AppendEquation("K_14=(1.45-0.43∙δ_1)/sin(0.5∙δ_1)" +
//                                            $"=(1.45-0.43∙{DegToRad(_fbdi.delta1):f2})/sin(0.5∙{DegToRad(_fbdi.delta1):f2})={_K14:f2}");

//                        body.AddParagraph("Коэффициенты, учитывающие влияние расстояния от середины опоры до днища и угла охвата");
//                        body.AddParagraph("")
//                            .AppendEquation("K_15=min{1.0;(0.8∙√γ+6∙γ)/δ_1}" +
//                                            $"min{{1.0;(0.8∙√{_gamma:f2}+6∙{_gamma:f2})/{DegToRad(_fbdi.delta1):f2}}}=min{{1.0;{_K15_2:f2}}}={_K15:f2}");

//                        body.AddParagraph("")
//                            .AppendEquation("K_16=1-0.65/(1+(6∙γ)^2)∙√(π/(3∙δ_1))"
//                                            + $"=1-0.65/(1+(6∙{_gamma:f2})^2)∙√(π/(3∙{DegToRad(_fbdi.delta1):f2}))={_K16:f2}");

//                        body.AddParagraph("Коэффициенты, учитывающие влияние ширины пояса опоры и угла охвата");
//                        body.AddParagraph("")
//                            .AppendEquation("K_17=1/(1+0.6∙∛(D/(s-c))∙(b/D)∙δ_1)" +
//                                            $"=1/(1+0.6∙∛({_fbdi.D}/({_fbdi.s}-{_fbdi.c}))∙({_fbdi.b}/{_fbdi.D})∙{DegToRad(_fbdi.delta1):f2})={_K17:f2}");

//                        body.AddParagraph("Общее мембранное меридиональное напряжение изгиба от весовых нагрузок, действующее в области опорного узла");
//                        body.AddParagraph("")
//                            .AppendEquation("σ_mx=4∙M_i/(π∙D^2∙(s-c))" +
//                                            $"=4∙{_M1}/(π∙{_fbdi.D}^2∙({_fbdi.s}-{_fbdi.c}))={_sigma_mx:f2}");

//                        body.AddParagraph("Условие прочности");
//                        body.AddParagraph("").AppendEquation("F_1≤min{[F]_2;[F]_3}");
//                        body.AddParagraph("где ")
//                            .AppendEquation("[F]_2")
//                            .AddRun(" - допускаемое опорное усилие от нагружения в меридиональном направлении");
//                        body.AddParagraph("").AppendEquation("[F]_2=(0.7∙[σ_i]_2∙(s-c)∙√(D∙(s-c)))/(K_10∙K_12)");

//                        body.AddParagraph("\t")
//                            .AppendEquation("[F]_3")
//                            .AddRun(" - допускаемое опорное усилие от нагружения в окружном направлении");

//                        body.AddParagraph("")
//                            .AppendEquation("[F]_3=(0.9∙[σ_i]_3∙(s-c)∙√(D∙(s-c)))/(K_14∙K_16∙K_17)");

//                        body.AddParagraph("где ")
//                            .AppendEquation("[σ_i]_2, [σ_i]_2")
//                            .AddRun(" - предельные напряжения изгиба в меридиональном и окружном направлениях");

//                        body.AddParagraph("")
//                            .AppendEquation("[σ_i]=K_1∙K_2∙[σ]");

//                        body.AddParagraph("")
//                            .AppendEquation("K_1=(1-ϑ_2^2)/((1/3+ϑ_1∙ϑ_2)+√((1/3+ϑ_1∙ϑ_2)^2+(1-ϑ_2^2)∙ϑ_1^2))");

//                        body.AddParagraph("")
//                            .AppendEquation($"K_2={_K2}")
//                            .AddRun(_fbdi.IsAssembly
//                            ? " - для условий испытания и монтажа"
//                            : " - для рабочих условий");

//                        body.AddParagraph("для ").AppendEquation("[σ_i]_2");
//                        body.AddParagraph("")
//                            .AppendEquation("ϑ_1=-(0,23∙K_13∙K_15)/(K_12∙K_10)" +
//                                            $"={_v1_2:f2}");

//                        body.AddParagraph("")
//                            .AppendEquation("ϑ_(2,1)=- ̅σ_mx∙1/(K_2∙[σ])" +
//                                            $"={_v21_2:f2}");
//                        body.AddParagraph("")
//                            .AppendEquation("ϑ_(2,2)=[(p∙D)/(4∙(s-c))- ̅σ_mx]∙1/(K_2∙[σ])" +
//                                            $"={_v22_2:f2}");

//                        body.AddParagraph("Для ")
//                            .AppendEquation("ϑ_2")
//                            .AddRun("принимают одно из значений ")
//                            .AppendEquation("ϑ_(2,1)")
//                            .AddRun(" или ")
//                            .AppendEquation("ϑ_(2,2)")
//                            .AddRun(", для которого предельное напряжение изгибабудет наименьшим.");

//                        body.AddParagraph("")
//                            .AppendEquation(_K1_2For_v21 < _K1_2For_v22
//                            ? $"ϑ_2=ϑ_(2,1)={_v21_2:f2}"
//                            : $"ϑ_2=ϑ_(2,2)={_v22_2:f2}");

//                        body.AddParagraph("").AppendEquation($"K_1={_K1_2:f2}");

//                        body.AddParagraph("")
//                            .AppendEquation($"[σ_i]_2={_K1_2:f2}∙{_K2:f2}∙{_fbdi.sigma_d}={_sigmai2:f2}");

//                        body.AddParagraph("")
//                            .AppendEquation($"[F]_2=(0.7∙{_sigmai2:f2}∙({_fbdi.s}-{_fbdi.c})∙√({_fbdi.D}∙({_fbdi.s}-{_fbdi.c})))/({_K10:f2}∙{_K12:f2})={_F_d2:f2}");

//                        body.AddParagraph("для ").AppendEquation("[σ_i]_3");
//                        body.AddParagraph("")
//                            .AppendEquation("ϑ_1=-(0,53∙K_11)/(K_14∙K_16∙K_17∙sin(0.5∙δ_1))" +
//                                            $"={_v1_3:f2}");

//                        body.AddParagraph("").AppendEquation("ϑ_(2,1)=0");

//                        body.AddParagraph("")
//                            .AppendEquation("ϑ_(2,2)=(p∙D)/(2∙(s-c))∙1/(K_2∙[σ])" + $"={_v22_3:f2}");

//                        body.AddParagraph("Для ")
//                            .AppendEquation("ϑ_2")
//                            .AddRun("принимают одно из значений ")
//                            .AppendEquation("ϑ_(2,1)")
//                            .AddRun(" или ")
//                            .AppendEquation("ϑ_(2,2)")
//                            .AddRun(", для которого предельное напряжение изгибабудет наименьшим.");

//                        body.AddParagraph("").AppendEquation(_K1_3For_v21 < _K1_3For_v22
//                            ? $"ϑ_2=ϑ_(2,1)={_v21_3:f2}"
//                            : $"ϑ_2=ϑ_(2,2)={_v22_3:f2}");

//                        body.AddParagraph("").AppendEquation($"K_1={_K1_3:f2}");

//                        body.AddParagraph("")
//                            .AppendEquation($"[σ_i]_3={_K1_3:f2}∙{_K2:f2}∙{_fbdi.sigma_d}={_sigmai3:f2}");

//                        body.AddParagraph("")
//                            .AppendEquation($"[F]_3=(0.9∙{_sigmai2:f2}∙({_fbdi.s}-{_fbdi.c})∙√({_fbdi.D}∙({_fbdi.s}-{_fbdi.c})))/({_K14:f2}∙{_K16:f2}∙{_K17:f2})={_F_d3:f2}");

//                        body.AddParagraph("")
//                            .AppendEquation($"{_F1:f2}≤min{{{_F_d2:f2};{_F_d3:f2}}}");

//                        if (_F1 <= Math.Min(_F_d2, _F_d3))
//                        {
//                            body.AddParagraph("Условие прочности выполняется");
//                        }
//                        else
//                        {
//                            body.AddParagraph("Условие прочности не выполняется")
//                                .Bold()
//                                .Color(System.Drawing.Color.Red);
//                        }

//                        body.AddParagraph("Условие устойчивости");

//                        body.AddParagraph("").AppendEquation("|p|/[p]+|M_i|/[M]+|F_e|/[F]+(Q_i/[Q])^2≤1");

//                        body.AddParagraph("где p - расчетное наружное давление (для сосудов, работающих под внутренним избыточным давлением, р=0");

//                        body.AddParagraph("где ")
//                            .AppendEquation("F_e")
//                            .AddRun(" - эффективное осевое усилие от местных мембранных напряжений, действующих в области опоры");

//                        body.AddParagraph("")
//                            .AppendEquation("F_e=F_i∙π/4∙K_13∙K_15∙√(D/(s-c))" +
//                                            $"={_F1:f2}∙π/4∙{_K13:f2}∙{_K15:f2}∙√({_fbdi.D}/({_fbdi.s}-{_fbdi.c}))={_Fe:f2}");

//                        body.AddParagraph("")
//                            .AppendEquation(_fbdi.IsPressureIn
//                        ? "0"
//                        : $"{_fbdi.p}/{_p_d}" +
//                          $"+{_M1:f2}/{_M_d:f2}+{_Fe:f2}/{_F_d:f2}+({_Q1:f2}/{_Q_d:f2})^2={_conditionStability2:f2}≤1");

//                        if (_conditionStability2 <= 1)
//                        {
//                            body.AddParagraph("Условие устойчивости выполняется");
//                        }
//                        else
//                        {
//                            body.AddParagraph("Условие устойчивости не выполняется")
//                                .Bold()
//                                .Color(System.Drawing.Color.Red);
//                        }
//                        break;
//                    }
//                case SaddleType.SaddleWithoutRingWithSheet:
//                    {
//                        body.AddParagraph("Проверка несущей способности обечайки, не укрепленной кольцами жесткости в области опорного узла с подкладным листом в месте опоры");
//                        body.AddParagraph("Вспомогательные параметры и коэффициенты");

//                        body.AddParagraph("")
//                            .AppendEquation("s_ef=(s-c)∙√(1+(s_2/(s-c))^2)" +
//                                            $"=({_fbdi.s}-{_fbdi.c})∙√(1+({_fbdi.s2}/({_fbdi.s}-{_fbdi.c}))^2)={_sef:f2}");

//                        body.AddParagraph("Параметр, определяемый расстоянием от середины опоры до днища");
//                        body.AddParagraph("")
//                            .AppendEquation("γ=2.83∙a/D∙√(s_ef/D)" +
//                                            $"=2.83∙{_fbdi.a}/{_fbdi.D}∙√({_sef:f2})/{_fbdi.D})={_gamma:f2}");

//                        body.AddParagraph("Параметр, определяемый шириной пояса опоры");
//                        body.AddParagraph("")
//                            .AppendEquation("β_1=0.91∙b_2/√(D∙s_ef)" +
//                                             $"=0.91∙{_fbdi.b}/√({_fbdi.D}∙{_sef:f2})={_beta1:f2}");

//                        body.AddParagraph("Коэффициенты, учитывающие влияние ширины пояса опоры");
//                        body.AddParagraph("")
//                            .AppendEquation("K_10=max{(exp(-β_1)∙sin(β_1))/β_1;0.25}" +
//                                            $"=max{{(exp(-{_beta1:f2})∙sin({_beta1:f2}))/{_beta1:f2};0.25}}" +
//                                            $"=max({_K10_1:f2};0.25)={_K10:f2}");

//                        body.AddParagraph("")
//                            .AppendEquation("K_11=(1-exp(-β_1)∙cos(β_1))/β_1" +
//                                            $"=(1-exp(-{_beta1:f2})∙cos({_beta1:f2}))/{_beta1:f2}={_K11:f2}");

//                        body.AddParagraph("Коэффициенты, учитывающие влияние угла охвата");
//                        body.AddParagraph("")
//                            .AppendEquation("K_12=(1.15-0.1432∙δ_2)/sin(0.5∙δ_2)" +
//                                            $"=(1.15-0.1432∙{DegToRad(_fbdi.delta2):f2})/sin(0.5∙{DegToRad(_fbdi.delta2):f2})={_K12:f2}");

//                        body.AddParagraph("")
//                            .AppendEquation("K_13=(max{1.7-(2.1∙δ_2)/π;0})/sin(0.5∙δ_2)" +
//                                            $"=(max{{1.7 - (2.1∙{DegToRad(_fbdi.delta2):f2})/π;0}})/sin(0.5∙{DegToRad(_fbdi.delta2):f2})={_K13:f2}");

//                        body.AddParagraph("")
//                            .AppendEquation("K_14=(1.45-0.43∙δ_2)/sin(0.5∙δ_2)" +
//                                            $"=(1.45-0.43∙{DegToRad(_fbdi.delta2):f2})/sin(0.5∙{DegToRad(_fbdi.delta2):f2})={_K14:f2}");

//                        body.AddParagraph("Коэффициенты, учитывающие влияние расстояния от середины опоры до днища и угла охвата");
//                        body.AddParagraph("")
//                            .AppendEquation("K_15=min{1.0;(0.8∙√γ+6∙γ)/δ_2}" +
//                                            $"min{{1.0;(0.8∙√{_gamma:f2}+6∙{_gamma:f2})/{DegToRad(_fbdi.delta2):f2}}}=min{{1.0;{_K15_2:f2}}}={_K15:f2}");

//                        body.AddParagraph("")
//                            .AppendEquation("K_16=1-0.65/(1+(6∙γ)^2)∙√(π/(3∙δ_2))"
//                                            + $"=1-0.65/(1+(6∙{_gamma:f2})^2)∙√(π/(3∙{DegToRad(_fbdi.delta2):f2}))={_K16:f2}");

//                        body.AddParagraph("Коэффициенты, учитывающие влияние ширины пояса опоры и угла охвата");
//                        body.AddParagraph("")
//                            .AppendEquation("K_17=1/(1+0.6∙∛(D/s_ef)∙(b_2/D)∙δ_2)" +
//                                            $"=1/(1+0.6∙∛({_fbdi.D}/{_sef:f2})∙({_fbdi.b2}/{_fbdi.D})∙{DegToRad(_fbdi.delta2):f2})={_K17:f2}");

//                        body.AddParagraph("Общее мембранное меридиональное напряжение изгиба от весовых нагрузок, действующее в области опорного узла");
//                        body.AddParagraph("")
//                            .AppendEquation("σ_mx=4∙M_i/(π∙D^2∙s_ef)" +
//                                            $"=4∙{_M1}/(π∙{_fbdi.D}^2∙{_sef:f2})={_sigma_mx:f2}");

//                        body.AddParagraph("Условие прочности");
//                        body.AddParagraph("").AppendEquation("F_1≤min{[F]_2;[F]_3}");
//                        body.AddParagraph("где ")
//                            .AppendEquation("[F]_2")
//                            .AddRun(" - допускаемое опорное усилие от нагружения в меридиональном направлении");
//                        body.AddParagraph("").AppendEquation("[F]_2=(0.7∙[σ_i]_2∙s_ef∙√(D∙s_ef))/(K_10∙K_12)");

//                        body.AddParagraph("\t")
//                            .AppendEquation("[F]_3")
//                            .AddRun(" - допускаемое опорное усилие от нагружения в окружном направлении");

//                        body.AddParagraph("")
//                            .AppendEquation("[F]_3=(0.9∙[σ_i]_3∙s_ef∙√(D∙s_ef))/(K_14∙K_16∙K_17)");

//                        body.AddParagraph("где ")
//                            .AppendEquation("[σ_i]_2, [σ_i]_2")
//                            .AddRun(" - предельные напряжения изгиба в меридиональном и окружном направлениях");

//                        body.AddParagraph("")
//                            .AppendEquation("[σ_i]=K_1∙K_2∙[σ]");

//                        body.AddParagraph("")
//                            .AppendEquation("K_1=(1-ϑ_2^2)/((1/3+ϑ_1∙ϑ_2)+√((1/3+ϑ_1∙ϑ_2)^2+(1-ϑ_2^2)∙ϑ_1^2))");

//                        body.AddParagraph("")
//                            .AppendEquation($"K_2={_K2}")
//                            .AddRun(_fbdi.IsAssembly
//                            ? " - для условий испытания и монтажа"
//                            : " - для рабочих условий");

//                        body.AddParagraph("для ").AppendEquation("[σ_i]_2");
//                        body.AddParagraph("")
//                            .AppendEquation("ϑ_1=-(0,23∙K_13∙K_15)/(K_12∙K_10)" +
//                                            $"={_v1_2:f2}");

//                        body.AddParagraph("")
//                            .AppendEquation("ϑ_(2,1)=- ̅σ_mx∙1/(K_2∙[σ])" +
//                                            $"={_v21_2:f2}");
//                        body.AddParagraph("")
//                            .AppendEquation("ϑ_(2,2)=[(p∙D)/(4∙s_ef)- ̅σ_mx]∙1/(K_2∙[σ])" +
//                                            $"={_v22_2:f2}");

//                        body.AddParagraph("Для ")
//                            .AppendEquation("ϑ_2")
//                            .AddRun("принимают одно из значений ")
//                            .AppendEquation("ϑ_(2,1)")
//                            .AddRun(" или ")
//                            .AppendEquation("ϑ_(2,2)")
//                            .AddRun(", для которого предельное напряжение изгибабудет наименьшим.");

//                        body.AddParagraph("")
//                            .AppendEquation(_K1_2For_v21 < _K1_2For_v22
//                            ? $"ϑ_2=ϑ_(2,1)={_v21_2:f2}"
//                            : $"ϑ_2=ϑ_(2,2)={_v22_2:f2}");

//                        body.AddParagraph("").AppendEquation($"K_1={_K1_2:f2}");

//                        body.AddParagraph("")
//                            .AppendEquation($"[σ_i]_2={_K1_2:f2}∙{_K2:f2}∙{_fbdi.sigma_d}={_sigmai2:f2}");

//                        body.AddParagraph("")
//                            .AppendEquation($"[F]_2=(0.7∙{_sigmai2:f2}∙{_sef:f2}∙√({_fbdi.D}∙{_sef:f2}))/({_K10:f2}∙{_K12:f2})={_F_d2:f2}");

//                        body.AddParagraph("для ").AppendEquation("[σ_i]_3");
//                        body.AddParagraph("")
//                            .AppendEquation("ϑ_1=-(0,53∙K_11)/(K_14∙K_16∙K_17∙sin(0.5∙δ_2))" +
//                                            $"={_v1_3:f2}");

//                        body.AddParagraph("").AppendEquation("ϑ_(2,1)=0");

//                        body.AddParagraph("")
//                            .AppendEquation("ϑ_(2,2)=(p∙D)/(2∙s_ef)∙1/(K_2∙[σ])" + $"={_v22_3:f2}");

//                        body.AddParagraph("Для ")
//                            .AppendEquation("ϑ_2")
//                            .AddRun("принимают одно из значений ")
//                            .AppendEquation("ϑ_(2,1)")
//                            .AddRun(" или ")
//                            .AppendEquation("ϑ_(2,2)")
//                            .AddRun(", для которого предельное напряжение изгибабудет наименьшим.");

//                        body.AddParagraph("").AppendEquation(_K1_3For_v21 < _K1_3For_v22
//                            ? $"ϑ_2=ϑ_(2,1)={_v21_3:f2}"
//                            : $"ϑ_2=ϑ_(2,2)={_v22_3:f2}");

//                        body.AddParagraph("").AppendEquation($"K_1={_K1_3:f2}");

//                        body.AddParagraph("")
//                            .AppendEquation($"[σ_i]_3={_K1_3:f2}∙{_K2:f2}∙{_fbdi.sigma_d}={_sigmai3:f2}");

//                        body.AddParagraph("")
//                            .AppendEquation($"[F]_3=(0.9∙{_sigmai2:f2}∙{_sef:f2}∙√({_fbdi.D}∙{_sef:f2}))/({_K14:f2}∙{_K16:f2}∙{_K17:f2})={_F_d3:f2}");

//                        body.AddParagraph("")
//                            .AppendEquation($"{_F1:f2}≤min{{{_F_d2:f2};{_F_d3:f2}}}");

//                        if (_F1 <= Math.Min(_F_d2, _F_d3))
//                        {
//                            body.AddParagraph("Условие прочности выполняется");
//                        }
//                        else
//                        {
//                            body.AddParagraph("Условие прочности не выполняется")
//                                .Bold()
//                                .Color(System.Drawing.Color.Red);
//                        }

//                        body.AddParagraph("Условие устойчивости");

//                        body.AddParagraph("").AppendEquation("|p|/[p]+|M_i|/[M]+|F_e|/[F]+(Q_i/[Q])^2≤1");

//                        body.AddParagraph("где p - расчетное наружное давление (для сосудов, работающих под внутренним избыточным давлением, р=0");

//                        body.AddParagraph("где ")
//                            .AppendEquation("F_e")
//                            .AddRun(" - эффективное осевое усилие от местных мембранных напряжений, действующих в области опоры");

//                        body.AddParagraph("")
//                            .AppendEquation("F_e=F_i∙π/4∙K_13∙K_15∙√(D/s_ef)" +
//                                            $"={_F1:f2}∙π/4∙{_K13:f2}∙{_K15:f2}∙√({_fbdi.D}/{_sef:f2})={_Fe:f2}");

//                        body.AddParagraph("")
//                            .AppendEquation(_fbdi.IsPressureIn
//                        ? "0"
//                        : $"{_fbdi.p}/{_p_d}" +
//                            $"+{_M1:f2}/{_M_d:f2}+{_Fe:f2}/{_F_d:f2}+({_Q1:f2}/{_Q_d:f2})^2={_conditionStability2:f2}≤1");

//                        if (_conditionStability2 <= 1)
//                        {
//                            body.AddParagraph("Условие устойчивости выполняется");
//                        }
//                        else
//                        {
//                            body.AddParagraph("Условие устойчивости не выполняется")
//                                .Bold()
//                                .Color(System.Drawing.Color.Red);
//                        }
//                        break;
//                    }
//            }

//            body.AddParagraph("Условия применения расчетных формул ");

//            if (IsConditionUseFormulas)
//            {
//                body.AddParagraph("Условия применения формул");
//            }
//            else
//            {
//                body.AddParagraph("Условия применения формул не выполняются").Bold().Color(System.Drawing.Color.Red);
//            }
//            body.AddParagraph("")
//                .AppendEquation($"60°≤δ_1={_fbdi.delta1}°≤180°");
//            body.AddParagraph("")
//                .AppendEquation($"(s-c)/D=({_fbdi.s}-{_fbdi.c})/{_fbdi.D}={(_fbdi.s - _fbdi.c) / _fbdi.D:f2}≤0.05");
//            if (_fbdi.Type == SaddleType.SaddleWithoutRingWithSheet)
//            {
//                body.AddParagraph("").AppendEquation("s_2≥s");
//                body.AddParagraph("").AppendEquation($"{_fbdi.s2} мм ≥ {_fbdi.s} мм");
//                body.AddParagraph("").AppendEquation("δ_2≥δ_1+20°");
//                body.AddParagraph("")
//                    .AppendEquation(
//                        $"{_fbdi.delta2}°≥{_fbdi.delta1}°+20°={_fbdi.delta1 + 20}°");
//            }

//            if (_fbdi.Type == SaddleType.SaddleWithRing)
//            {
//                body.AddParagraph("").AppendEquation("A_k≥(s-c)√(D∙(s-c))");
//                body.AddParagraph("").AppendEquation($"{_fbdi.Ak:f2}≥({_fbdi.s}-{_fbdi.c})√({_fbdi.D}∙({_fbdi.s}-{_fbdi.c}))={_Ak:f2}");
//            }

            package.Close();
        }
    }
}
