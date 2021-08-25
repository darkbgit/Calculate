using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.DataIn;
using CalculateVessels.Core.Shells.Enums;
using CalculateVessels.Core.Shells.Nozzle.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using CalculateVessels.Core.Word;
using CalculateVessels.Core.Word.Enums;
using DocumentFormat.OpenXml.Wordprocessing;


namespace CalculateVessels.Core.Shells.Nozzle
{
    public class Nozzle : IElement
    {
        public Nozzle(IElement element, NozzleDataIn nozzleDataIn)
        {
            _nozzleDataIn = nozzleDataIn;
            //this.shellData = (shell as Shell).ShellDataIn;
            _element = element;
        }

        private readonly NozzleDataIn _nozzleDataIn;
        private readonly IElement _element;


        


        private string _name;
        private string _steel1;
        private string _steel2;
        private string _steel3;
        private string _steel4;

        public string Name { get => _name; set => _name = value; }
        public string Steel1 { get => _steel1; set => _steel1 = value; }
        public string Steel2 { get => _steel2; set => _steel2 = value; }
        public string Steel3 { get => _steel3; set => _steel3 = value; }
        public string Steel4 { get => _steel4; set => _steel4 = value; }
        public bool IsCriticalError { get; private set; }
        public bool IsError { get; private set; }
        public List<string> ErrorList { get; private set; } = new();
        public double p_d { get => _p_d; }
        public double d0 { get => _d0; }
        public double b { get => _b; }

        public List<string> Bibliography { get; } = new()
        {
            Data.Properties.Resources.GOST_34233_3
        };

        private bool isConditionUseFormulas;
        private double _alfa1;
        private double _b;
        private double _B1n;
        private double _c;
        private double _conditionUseFormulas1;
        private double _conditionUseFormulas2;
        private double _conditionUseFormulas2_2;
        private double _d0;
        private double _d01;
        private double _d02;
        private double _d0p;
        private double _Dk;
        private double _dmax;
        private double _Dp;
        private double _dp;
        private double _E2;
        private double _E3;
        private double _E4;
        private double _ellH;
        private double _L0;
        private double _l1p;
        private double _l1p2;
        private double _l2p;
        private double _l2p2;
        private double _l3p;
        private double _l3p2;
        private double _lp;
        private double _p_d;
        private double _p_de;
        private double _p_deShell;
        private double _p_dp;
        private double _pen;
        private double _ppn;
        private double _psi1;
        private double _psi2;
        private double _psi3;
        private double _psi4 = 1;
        private double _s1p;
        private double _sp;
        private double _spn;
        private double _V;
        private double _V1;
        private double _V2;
        private double _conditionStrengthening1;
        private double _conditionStrengthening2;
        private int _K1;
        private List<string> errorList = new();



        public void Calculate()
        {
            _c = (_element as Shell).c;

            if (!_nozzleDataIn.ShellDataIn.IsPressureIn) _p_deShell = (_element as Shell).p_de;

            switch (_nozzleDataIn.ShellDataIn.ShellType)
            {
                case ShellType.Conical:
                    _Dk = (_element as ConicalShell).Dk;
                    _alfa1 = (_nozzleDataIn.ShellDataIn as ConicalShellDataIn).alfa1;
                    break;
                case ShellType.Elliptical:
                    _ellH = (_nozzleDataIn.ShellDataIn as EllipticalShellDataIn).ellH;
                    break;
            }

            // расчет Dp, dp
            switch (_nozzleDataIn.ShellDataIn.ShellType)
            {
                case ShellType.Cylindrical:
                    {
                        _Dp = _nozzleDataIn.ShellDataIn.D;
                        break;
                    }
                case ShellType.Conical:
                    {
                        _Dp = _Dk / Math.Cos(Math.PI * 100 * _alfa1);
                        break;
                    }
                case ShellType.Elliptical:
                    {
                        if (Math.Abs(_ellH * 100 - _nozzleDataIn.ShellDataIn.D * 25) < 0.000001)
                        {
                            _Dp = _nozzleDataIn.ShellDataIn.D * 2 * Math.Sqrt(1.0 - 3.0 * Math.Pow(_nozzleDataIn.ellx / _nozzleDataIn.ShellDataIn.D, 2));
                        }
                        else
                        {
                            _Dp = Math.Pow(_nozzleDataIn.ShellDataIn.D, 2) / (_ellH * 2) *
                                Math.Sqrt(1.0 - 4.0 * (Math.Pow(_nozzleDataIn.ShellDataIn.D, 2) - 4 *
                                Math.Pow(_ellH, 2)) * Math.Pow(_nozzleDataIn.ellx, 2) /
                                    Math.Pow(_nozzleDataIn.ShellDataIn.D, 4));
                        }
                        break;
                    }
                case ShellType.Spherical:
                case ShellType.Torospherical:
                    {
                        //_Dp = 2 * _nozzleDataIn.ShellDataIn.R;
                        break;
                    }
                default:
                {
                    IsCriticalError = true;
                    ErrorList.Add("Ошибка вида укрепляемого элемента");
                    break;
                }
            }

            if (_nozzleDataIn.ShellDataIn.ShellType == ShellType.Elliptical && _nozzleDataIn.ShellDataIn.IsPressureIn)
            {
                _sp = _nozzleDataIn.ShellDataIn.p * _Dp / (4.0 * _nozzleDataIn.ShellDataIn.fi * _nozzleDataIn.ShellDataIn.sigma_d - _nozzleDataIn.ShellDataIn.p);
            }
            else
            {
                _sp = (_element as Shell).s_p;
            }

            if (!_nozzleDataIn.IsOval)
            {
                _s1p = _nozzleDataIn.ShellDataIn.p * (_nozzleDataIn.d + 2 * _nozzleDataIn.cs) / (2.0 * _nozzleDataIn.fi * _nozzleDataIn.sigma_d1 - _nozzleDataIn.ShellDataIn.p);
            }
            else
            {
                _s1p = _nozzleDataIn.ShellDataIn.p * (_nozzleDataIn.d1 + 2 * _nozzleDataIn.cs) / (2.0 * _nozzleDataIn.fi * _nozzleDataIn.sigma_d1 - _nozzleDataIn.ShellDataIn.p);
            }

            switch (_nozzleDataIn.Location)
            {
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_1:
                    {
                        _dp = _nozzleDataIn.d + 2 * _nozzleDataIn.cs; //dp = d + 2cs
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_2:
                    {
                        _dp = Math.Max(_nozzleDataIn.d, 0.5 * _nozzleDataIn.tTransversely) + 2.0 * _nozzleDataIn.cs; //dp =max{d; 0,5t} + 2cs
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_3:
                    {
                        _dp = (_nozzleDataIn.d + 2.0 * _nozzleDataIn.cs) / Math.Sqrt(1 + Math.Pow(2 * _nozzleDataIn.ellx / _Dp, 2));
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_4:
                    {
                        _dp = (_nozzleDataIn.d + 2.0 * _nozzleDataIn.cs) * (1 + Math.Pow(Math.Tan(Math.PI * 180 * _nozzleDataIn.gamma), 2) *
                            Math.Pow(Math.Cos(Math.PI * 180 * _nozzleDataIn.omega), 2));
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_5:
                    {
                        _dp = (_nozzleDataIn.d + 2.0 * _nozzleDataIn.cs) / Math.Pow(Math.Cos(Math.PI * 180 * _nozzleDataIn.gamma), 2);
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_6:
                    {
                        _dp = (_nozzleDataIn.d2 + 2.0 * _nozzleDataIn.cs) *
                            (Math.Pow(Math.Sin(Math.PI * 180 * _nozzleDataIn.omega), 2) +
                            (_nozzleDataIn.d1 + 2 * _nozzleDataIn.cs) *
                            (_nozzleDataIn.d1 + _nozzleDataIn.d2 + 4 * _nozzleDataIn.cs) /
                            (2 * Math.Pow(_nozzleDataIn.d2 + 2 * _nozzleDataIn.cs, 2)) *
                            Math.Pow(Math.Cos(Math.PI * 180 * _nozzleDataIn.omega), 2));
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_7:
                    {
                        _dp = _nozzleDataIn.d + 1.5 * (_nozzleDataIn.r - _sp) + 2.0 * _nozzleDataIn.cs;
                        break;
                    }
                default:
                    {
                        IsCriticalError = true;
                        ErrorList.Add("Ошибка места расположения штуцера");
                        break;
                    }
            }

            // l1p, l3p, l2p
            {

                double d = !_nozzleDataIn.IsOval ? _nozzleDataIn.d : _nozzleDataIn.d2;

                _l1p2 = 1.25 * Math.Sqrt((d + 2.0 * _nozzleDataIn.cs) * (_nozzleDataIn.s1 - _nozzleDataIn.cs));
                _l1p = Math.Min(_nozzleDataIn.l1, _l1p2);
                if (_nozzleDataIn.s3 == 0)
                {
                    _l3p = 0;
                }
                else
                {
                    _l3p2 = 0.5 * Math.Sqrt((d + 2 * _nozzleDataIn.cs) * (_nozzleDataIn.s3 - _nozzleDataIn.cs - _nozzleDataIn.cs1));
                    _l3p = Math.Min(_nozzleDataIn.l3, _l3p2);
                }
            }

            _L0 = Math.Sqrt(_Dp * (_nozzleDataIn.ShellDataIn.s - _c));

            switch (_nozzleDataIn.NozzleKind)
            {
                case NozzleKind.ImpassWithoutRing:
                case NozzleKind.PassWithoutRing:
                case NozzleKind.ImpassWithRing:
                case NozzleKind.PassWithRing:
                case NozzleKind.WithRingAndInPart:
                case NozzleKind.WithFlanging:
                    _lp = _L0;
                    break;
                case NozzleKind.WithTorusshapedInsert:
                case NozzleKind.WithWealdedRing:
                    _lp = Math.Min(_nozzleDataIn.l, _L0);
                    break;
            }

            _l2p2 = Math.Sqrt(_Dp * (_nozzleDataIn.s2 + _nozzleDataIn.ShellDataIn.s - _c));
            _l2p = Math.Min(_nozzleDataIn.l2, _l2p2);

            switch (_nozzleDataIn.NozzleKind)
            {
                case NozzleKind.ImpassWithoutRing:
                case NozzleKind.PassWithoutRing:
                case NozzleKind.ImpassWithRing:
                case NozzleKind.PassWithRing:
                case NozzleKind.WithRingAndInPart:
                case NozzleKind.WithFlanging:
                    _nozzleDataIn.s0 = _nozzleDataIn.ShellDataIn.s;
                    //_nozzleDataIn.steel4 = _nozzleDataIn.steel1;
                    break;
            }


            _psi1 = Math.Min(1, _nozzleDataIn.sigma_d1 / _nozzleDataIn.ShellDataIn.sigma_d);
            _psi2 = Math.Min(1, _nozzleDataIn.sigma_d2 / _nozzleDataIn.ShellDataIn.sigma_d);
            _psi3 = Math.Min(1, _nozzleDataIn.sigma_d3 / _nozzleDataIn.ShellDataIn.sigma_d);

            _psi4 = _nozzleDataIn.NozzleKind switch
            {
                NozzleKind.WithTorusshapedInsert or NozzleKind.WithWealdedRing =>
                Math.Min(1, _nozzleDataIn.sigma_d4 / _nozzleDataIn.ShellDataIn.sigma_d),
                _ => 1,
            };

            _d0p = 0.4 * Math.Sqrt(_Dp * (_nozzleDataIn.ShellDataIn.s - _c));

            _b = Math.Sqrt(_Dp * (_nozzleDataIn.ShellDataIn.s - _c)) + Math.Sqrt(_Dp * (_nozzleDataIn.ShellDataIn.s - _c));

            switch (_nozzleDataIn.ShellDataIn.ShellType)
            {
                case ShellType.Cylindrical:
                    {
                        _dmax = _nozzleDataIn.ShellDataIn.D;
                        break;
                    }
                case ShellType.Conical:
                    {
                        _dmax = _Dk;
                        break;
                    }
                case ShellType.Elliptical:
                case ShellType.Spherical:
                case ShellType.Torospherical:
                    {
                        _dmax = 0.6 * _nozzleDataIn.ShellDataIn.D;
                        break;
                    }
            }

            switch (_nozzleDataIn.ShellDataIn.ShellType)
            {
                case ShellType.Cylindrical:
                case ShellType.Conical:
                    {
                        _K1 = 1;
                        break;
                    }
                case ShellType.Elliptical:
                case ShellType.Spherical:
                case ShellType.Torospherical:
                    {
                        _K1 = 2;
                        break;
                    }
            }

            if (_nozzleDataIn.ShellDataIn.IsPressureIn)
            {
                _spn = _sp;
            }
            else
            {

                //_B1n = Math.Min(1, 9.45 * (_nozzleDataIn.ShellDataIn.D / d_out.l) * Math.Sqrt(_nozzleDataIn.ShellDataIn.D / (100 * (_nozzleDataIn.ShellDataIn.s - _c))));
                //_pen = 2.08 * 0.00001 * _nozzleDataIn.ShellDataIn.E / (_nozzleDataIn.ny * _B1n) * (_nozzleDataIn.ShellDataIn.D / d_out.l) * Math.Pow(100 * (_nozzleDataIn.ShellDataIn.s - _c) / _nozzleDataIn.ShellDataIn.D, 2.5);
                _pen = _p_deShell;
                _ppn = _nozzleDataIn.ShellDataIn.p / Math.Sqrt(1.0 - Math.Pow(_nozzleDataIn.ShellDataIn.p / _pen, 2));
                _spn = _ppn * _Dp / (2.0 * _K1 * _nozzleDataIn.ShellDataIn.sigma_d - _ppn);
            }


            _d01 = 2 * ((_nozzleDataIn.ShellDataIn.s - _c) / _spn - 0.8) * Math.Sqrt(_Dp * (_nozzleDataIn.ShellDataIn.s - _c));
            _d02 = _dmax + 2 * _nozzleDataIn.cs;
            _d0 = Math.Min(_d01, _d02);

            if (_dp > _d0)
            {
                _conditionStrengthening1 = _l1p * (_nozzleDataIn.s1 - _s1p - _nozzleDataIn.cs) * _psi1 + _l2p * _nozzleDataIn.s2 * _psi2 + _l3p * (_nozzleDataIn.s3 - _nozzleDataIn.cs - _nozzleDataIn.cs1) * _psi3 + _lp * (_nozzleDataIn.ShellDataIn.s - _sp - _c) * _psi4;
                _conditionStrengthening2 = 0.5 * (_dp - _d0p) * _sp;
                if (_conditionStrengthening1 < _conditionStrengthening2)
                {
                    IsError = true;
                    ErrorList.Add("Условие укрепления одиночного отверстия не выполняется");
                }
            }



            _V1 = (_nozzleDataIn.s0 - _c) / (_nozzleDataIn.ShellDataIn.s - _c);
            _V2 = (_psi4 + (_l1p * (_nozzleDataIn.s1 - _nozzleDataIn.cs) * _psi1 + _l2p * _nozzleDataIn.s2 * _psi2 + _l3p * (_nozzleDataIn.s3 - _nozzleDataIn.cs - _nozzleDataIn.cs1) * _psi3) / _lp * (_nozzleDataIn.ShellDataIn.s - _c)) / 
                  (1.0 + 0.5 * (_dp - _d0p) / _lp + _K1 * (_nozzleDataIn.d + 2 * _nozzleDataIn.cs) / _Dp * (_nozzleDataIn.fi / _nozzleDataIn.fi1) * (_l1p / _lp));
            _V = Math.Min(_V1, _V2);

            if (_nozzleDataIn.ShellDataIn.IsPressureIn)
            {
                _p_d = 2 * _K1 * _nozzleDataIn.fi * _nozzleDataIn.ShellDataIn.sigma_d * (_nozzleDataIn.ShellDataIn.s - _c) * _V /
                       (_Dp + (_nozzleDataIn.ShellDataIn.s - _c) * _V);
            }
            else
            {
                _p_dp = 2 * _K1 * _nozzleDataIn.fi * _nozzleDataIn.ShellDataIn.sigma_d * (_nozzleDataIn.ShellDataIn.s - _c) * _V /
                        (_Dp + (_nozzleDataIn.ShellDataIn.s - _c) * _V);
                _p_de = _p_deShell;
                _p_d = _p_dp / Math.Sqrt(1 + Math.Pow(_p_dp / _p_de, 2));
            }
            if (_p_d < _nozzleDataIn.ShellDataIn.p)
            {
                IsError = true;
                ErrorList.Add("Допускаемое давление меньше расчетного");
            }

            switch (_nozzleDataIn.ShellDataIn.ShellType)
            {
                case ShellType.Cylindrical:
                    {
                        _conditionUseFormulas1 = (_dp - 2 * _nozzleDataIn.cs) / _nozzleDataIn.ShellDataIn.D;
                        _conditionUseFormulas2 = (_nozzleDataIn.ShellDataIn.s - _c) / _nozzleDataIn.ShellDataIn.D;
                        isConditionUseFormulas = _conditionUseFormulas1 <= 1 & _conditionUseFormulas2 <= 0.1;
                        break;
                    }
                case ShellType.Conical:
                    {
                        _conditionUseFormulas1 = (_dp - 2 * _nozzleDataIn.cs) / _Dk;
                        _conditionUseFormulas2 = (_nozzleDataIn.ShellDataIn.s - _c) / _Dk;
                        _conditionUseFormulas2_2 = 0.1 / Math.Cos(Math.PI * 180 * _alfa1);
                        isConditionUseFormulas = _conditionUseFormulas1 <= 1 &
                                                 _conditionUseFormulas2 <= _conditionUseFormulas2_2;
                        break;
                    }
                case ShellType.Elliptical:
                case ShellType.Spherical:
                case ShellType.Torospherical:
                    {
                        _conditionUseFormulas1 = (_dp - 2 * _nozzleDataIn.cs) / _nozzleDataIn.ShellDataIn.D;
                        _conditionUseFormulas2 = (_nozzleDataIn.ShellDataIn.s - _c) / _nozzleDataIn.ShellDataIn.D;
                        isConditionUseFormulas = _conditionUseFormulas1 <= 0.6 & _conditionUseFormulas2 <= 0.1;
                        break;
                    }
            }
            if (!isConditionUseFormulas)
            {
                IsError = true;
                ErrorList.Add("Условие применения формул не выполняется");
            }

        }

        public void MakeWord(string filename)
        {
            if (filename == null)
            {
                const string DEFAULT_FILE_NAME = "temp.docx";
                filename = DEFAULT_FILE_NAME;
            }

            using var package = WordprocessingDocument.Open(filename, true);

            var mainPart = package.MainDocumentPart;
            var body = mainPart?.Document.Body;

            if (body == null) return;



            body.AddParagraph($"Расчет на прочность узла врезки штуцера {_nozzleDataIn.Name} в ")
                .Heading(HeadingType.Heading1)
                .Alignment(AlignmentType.Center);
            switch (_nozzleDataIn.ShellDataIn.ShellType)
            {
                case ShellType.Cylindrical:
                    body.Elements<Paragraph>().Last()
                        .AddRun($"обечайку {_nozzleDataIn.ShellDataIn.Name}, нагруженную ");
                    break;
                case ShellType.Conical:
                    body.Elements<Paragraph>().Last()
                        .AddRun($"коническую обечайку {_nozzleDataIn.ShellDataIn.Name}, нагруженную ");
                    break;
                case ShellType.Elliptical:
                    body.Elements<Paragraph>().Last()
                        .AddRun($"эллиптическое днище {_nozzleDataIn.ShellDataIn.Name}, нагруженное ");
                    break;
            }

            body.Elements<Paragraph>().Last()
                .AddRun(_nozzleDataIn.ShellDataIn.IsPressureIn
                ? "внутренним избыточным давлением"
                : "наружным давлением");
            body.AddParagraph("");
            body.AddParagraph("Исходные данные").Alignment(AlignmentType.Center);

            //table
            {
                var table = body.AddTable();

                table.AddRow()
                    .AddCell("Элемент:")
                    .AddCell($"Штуцер {_nozzleDataIn.Name}");

                table.AddRow()
                    .AddCell("Элемент несущий штуцер:")
                    .AddCell($"{_nozzleDataIn.ShellDataIn.Name}");

                table.AddRow()
                    .AddCell("Тип элемента, несущего штуцер:");
                switch (_nozzleDataIn.ShellDataIn.ShellType)
                {
                    case ShellType.Cylindrical:
                        table.Elements<TableRow>().Last().AddCell("Обечайка цилиндрическая");
                        break;
                    case ShellType.Conical:
                        table.Elements<TableRow>().Last().AddCell("Обечайка коническая");
                        break;
                    case ShellType.Elliptical:
                        table.Elements<TableRow>().Last().AddCell("Днище эллиптическое");
                        break;
                }

                
                table.AddRow()
                    .AddCell("Тип штуцера:");
                switch (_nozzleDataIn.NozzleKind)
                {
                    case NozzleKind.ImpassWithoutRing:
                        table.Elements<TableRow>().Last().AddCell("Непроходящий без укрепления");
                        break;
                    case NozzleKind.PassWithoutRing:
                        table.Elements<TableRow>().Last().AddCell("Проходящий без укрепления");
                        break;
                    case NozzleKind.ImpassWithRing:
                        table.Elements<TableRow>().Last().AddCell("Непроходящий с накладным кольцом");
                        break;
                    case NozzleKind.PassWithRing:
                        table.Elements<TableRow>().Last().AddCell("Проходящий с накладным кольцом");
                        break;
                    case NozzleKind.WithRingAndInPart:
                        table.Elements<TableRow>().Last().AddCell("С накладным кольцом и внутренней частью");
                        break;
                    case NozzleKind.WithFlanging:
                        table.Elements<TableRow>().Last().AddCell("С отбортовкой");
                        break;
                    case NozzleKind.WithTorusshapedInsert:
                        table.Elements<TableRow>().Last().AddCell("С торовой вставкой");
                        break;
                    case NozzleKind.WithWealdedRing:
                        table.Elements<TableRow>().Last().AddCell("С вварным кольцом");
                        break;
                }
                body.InsertTable(table);
            }

            var imagePart = mainPart.AddImagePart(ImagePartType.Gif);

            byte[] bytes = (byte[])Data.Properties.Resources.ResourceManager
                .GetObject($"Nozzle{(int)_nozzleDataIn.NozzleKind}");

            if (bytes != null)
            {
                imagePart.FeedData(new MemoryStream(bytes));
                body.AddParagraph("").AddImage(mainPart.GetIdOfPart(imagePart), bytes);
            }

            //table
            {
                var table = body.AddTable();

                //table.

                table.AddRow()
                    .AddCell("Материал несущего элемента:")
                    .AddCell($"{_nozzleDataIn.ShellDataIn.Steel}");

                table.AddRow()
                    .AddCell("Толщина стенки несущего элемента, s:")
                    .AddCell($"{_nozzleDataIn.ShellDataIn.s} мм");

                table.AddRow()
                    .AddCell("Сумма прибавок к стенке несущего элемента, c:")
                    .AddCell($"{_c:f2} мм");

                table.AddRow()
                    .AddCell("Материал штуцера")
                    .AddCell($"{_nozzleDataIn.steel1}");

                table.AddRow()
                    .AddCell("Внутренний диаметр штуцера, d:")
                    .AddCell($"{_nozzleDataIn.d} мм");

                table.AddRow()
                    .AddCell("Толщина стенки штуцера, ")
                    .AppendEquation("s_1")
                    .AppendText(":")
                    .AddCell($"{_nozzleDataIn.s1} мм");

                table.AddRow()
                    .AddCell("Длина наружной части штуцера, ")
                    .AppendEquation("s_1")
                    .AppendText(":")
                    .AddCell($"{_nozzleDataIn.l1} мм");

                table.AddRow()
                    .AddCell("Сумма прибавок к толщине стенки штуцера, ")
                    .AppendEquation("c_s")
                    .AppendText(":")
                    .AddCell($"{_nozzleDataIn.cs} мм");

                table.AddRow()
                    .AddCell("Прибавка на коррозию, ")
                    .AppendEquation("c_s1")
                    .AppendText(":")
                    .AddCell($"{_nozzleDataIn.cs1} мм");

                switch (_nozzleDataIn.NozzleKind)
                {
                    case NozzleKind.ImpassWithoutRing:
                        {
                            break;
                        }
                    case NozzleKind.PassWithoutRing:
                        {
                            table.AddRow()
                                .AddCell("Длина внутренней части штуцера, ")
                                .AppendEquation("l_3")
                                .AppendText(":")
                                .AddCell($"{_nozzleDataIn.l3} мм");

                            table.AddRow()
                                .AddCell("Толщина внутренней части штуцера, ")
                                .AppendEquation("s_3")
                                .AppendText(":")
                                .AddCell($"{_nozzleDataIn.s3} мм");

                            break;
                        }
                    case NozzleKind.ImpassWithRing:
                        {
                            table.AddRow()
                                .AddCell("Ширина накладного кольца, ")
                                .AppendEquation("l_2")
                                .AppendText(":")
                                .AddCell($"{_nozzleDataIn.l2} мм");

                            table.AddRow()
                                .AddCell("Толщина накладного кольца, ")
                                .AppendEquation("s_2")
                                .AppendText(":")
                                .AddCell($"{_nozzleDataIn.s2} мм");

                            break;
                        }
                    case NozzleKind.PassWithRing:
                    case NozzleKind.WithRingAndInPart:
                        {
                            table.AddRow()
                                .AddCell("Ширина накладного кольца, ")
                                .AppendEquation("l_2")
                                .AppendText(":")
                                .AddCell($"{_nozzleDataIn.l2} мм");

                            table.AddRow()
                                .AddCell("Толщина накладного кольца, ")
                                .AppendEquation("s_2")
                                .AppendText(":")
                                .AddCell($"{_nozzleDataIn.s2} мм");

                            table.AddRow()
                                .AddCell("Длина внутренней части штуцера, ")
                                .AppendEquation("l_3")
                                .AppendText(":")
                                .AddCell($"{_nozzleDataIn.l3} мм");

                            table.AddRow()
                                .AddCell("Толщина внутренней части штуцера, ")
                                .AppendEquation("s_3")
                                .AppendText(":")
                                .AddCell($"{_nozzleDataIn.s3} мм");

                            break;
                        }
                    case NozzleKind.WithFlanging:
                        {
                            table.AddRow()
                                .AddCell("Радиус отбортовки, r:")
                                .AddCell($"{_nozzleDataIn.r} мм");

                            break;
                        }
                    case NozzleKind.WithTorusshapedInsert: //UNDONE:
                        break;
                    case NozzleKind.WithWealdedRing:
                        break;
                }

                table.AddRow()
                    .AddCell("Минимальный размер сварного шва, Δ:")
                    .AddCell($"{_nozzleDataIn.delta} мм");

                table.AddRowWithOneCell("Коэффициенты прочности сварных швов");

                table.AddRow()
                    .AddCell("Продольный шов штуцера ")
                    .AppendEquation("φ_1")
                    .AppendText(":")
                    .AddCell($"{_nozzleDataIn.fi1}");

                table.AddRow()
                    .AddCell("Шов обечайки в зоне врезки штуцера ")
                    .AppendEquation("φ")
                    .AppendText(":")
                    .AddCell($"{_nozzleDataIn.fi}");

                table.AddRowWithOneCell("Условия нагружения");


                table.AddRow()
                        .AddCell("Расчетная температура, Т:")
                        .AddCell($"{_nozzleDataIn.ShellDataIn.t} °С");

                table.AddRow()
                    .AddCell(_nozzleDataIn.ShellDataIn.IsPressureIn
                        ? "Расчетное внутреннее избыточное давление, p:"
                        : "Расчетное наружное давление, p:")
                    .AddCell($"{_nozzleDataIn.ShellDataIn.p} МПа");

                table.AddRowWithOneCell($"Характеристики материала {_nozzleDataIn.steel1}");

                table.AddRow()
                    .AddCell("Допускаемое напряжение при расчетной температуре, ")
                    .AppendEquation("[σ]_1")
                    .AppendText(":")
                    .AddCell($"{_nozzleDataIn.sigma_d1} МПа");

                if (!_nozzleDataIn.ShellDataIn.IsPressureIn)
                {
                    table.AddRow()
                        .AddCell("Модуль продольной упругости при расчетной температуре, ")
                        .AppendEquation("E_1")
                        .AppendText(":")
                        .AddCell($"{_nozzleDataIn.E1} МПа");
                }

                if (!string.IsNullOrEmpty(_nozzleDataIn.steel2) && _nozzleDataIn.steel1 != _nozzleDataIn.steel2)
                {
                    table.AddRowWithOneCell($"Характеристики материала {_nozzleDataIn.steel2}");

                    table.AddRow()
                        .AddCell(
                            "Допускаемое напряжение для при расчетной температуре, ")
                        .AppendEquation("[σ]_2")
                        .AppendText(":")
                        .AddCell($"{_nozzleDataIn.sigma_d2} МПа");

                    if (!_nozzleDataIn.ShellDataIn.IsPressureIn)
                    {
                        table.AddRow()
                            .AddCell("Модуль продольной упругости при расчетной температуре, ")
                            .AppendEquation("E_2")
                            .AppendText(":")
                            .AddCell($"{_nozzleDataIn.E2} МПа");
                    }
                }

                if (!string.IsNullOrEmpty(_nozzleDataIn.steel3) && _nozzleDataIn.steel1 != _nozzleDataIn.steel3)
                {
                    table.AddRowWithOneCell($"Характеристики материала {_nozzleDataIn.steel3}");

                    table.AddRow()
                        .AddCell("Допускаемое напряжение при расчетной температуре, ")
                        .AppendEquation("[σ]_3")
                        .AppendText(":")
                        .AddCell($"{_nozzleDataIn.sigma_d3} МПа");

                    if (!_nozzleDataIn.ShellDataIn.IsPressureIn)
                    {
                        table.AddRow()
                            .AddCell("Модуль продольной упругости при расчетной температуре, ")
                            .AppendEquation("E_3")
                            .AppendText(":")
                            .AddCell($"{_nozzleDataIn.E3} МПа");
                    }
                }

                if (!string.IsNullOrEmpty(_nozzleDataIn.steel4) && _nozzleDataIn.steel1 != _nozzleDataIn.steel4)
                {
                    table.AddRowWithOneCell($"Характеристики материала {_nozzleDataIn.steel4}");

                    table.AddRow()
                        .AddCell("Допускаемое напряжение при расчетной температуре, ")
                        .AppendEquation("[σ]_4")
                        .AppendText(":")
                        .AddCell($"{_nozzleDataIn.sigma_d4} МПа");

                    if (!_nozzleDataIn.ShellDataIn.IsPressureIn)
                    {
                        table.AddRow()
                            .AddCell("Модуль продольной упругости при расчетной температуре, ")
                            .AppendEquation("E_4")
                            .AppendText(":")
                            .AddCell($"{_nozzleDataIn.E4} МПа");
                    }
                }

                body.InsertTable(table);
            }

            

            body.AddParagraph("");
            body.AddParagraph("Результаты расчета").Alignment(AlignmentType.Center);
            body.AddParagraph("");


            body.AddParagraph("Расчетные параметры").Alignment(AlignmentType.Center);
            body.AddParagraph("");


            body.AddParagraph("Расчетный диаметр укрепляемого элемента ");

            switch (_nozzleDataIn.ShellDataIn.ShellType)
            {
                case ShellType.Cylindrical:
                    {
                        body.Elements<Paragraph>().Last()
                            .AddRun("(для цилиндрической обечайки)");
                        body.AddParagraph("")
                            .AppendEquation($"D_p=D={_nozzleDataIn.ShellDataIn.D} мм");
                        break;
                    }
                case ShellType.Conical:
                    {
                        body.Elements<Paragraph>().Last()
                            .AddRun("(для конической обечайки, перехода или днища)");
                        body.AddParagraph("")
                            .AppendEquation("D_p=D_k/cos(α)");
                        break;
                    }
                case ShellType.Elliptical:
                    {
                        if ((_nozzleDataIn.ShellDataIn as EllipticalShellDataIn).ellH * 100 == _nozzleDataIn.ShellDataIn.D * 25)
                        {
                            body.Elements<Paragraph>().Last()
                                .AddRun("(для эллиптического днища при H=0.25D)");
                            body.AddParagraph("")
                                .AppendEquation("D_p=2∙D∙√(1-3∙(x/D)^2)" +
                                                $"=2∙{_nozzleDataIn.ShellDataIn.D}∙√(1-3∙({_nozzleDataIn.ellx}/{_nozzleDataIn.ShellDataIn.D})^2)={_Dp:f2} мм");
                        }
                        else
                        {
                            body.Elements<Paragraph>().Last()
                                .AddRun("для эллиптического днища");
                            body.AddParagraph("")
                                .AppendEquation("D_p=D^2/(2∙H)∙√(1-(D^2-4∙H^2)/D^4∙x^2)" +
                                                $"={_nozzleDataIn.ShellDataIn.D}^2/(2∙{(_nozzleDataIn.ShellDataIn as EllipticalShellDataIn).ellH})∙√(1-({_nozzleDataIn.ShellDataIn.D}^2-4∙{(_nozzleDataIn.ShellDataIn as EllipticalShellDataIn).ellH}^2)/{_nozzleDataIn.ShellDataIn.D}^4∙{_nozzleDataIn.ellx}^2)={_Dp:f2} мм");
                        }
                        break;
                    }
                case ShellType.Spherical:
                case ShellType.Torospherical:
                    {
                        body.Elements<Paragraph>().Last()
                            .AddRun("(для сферических и торосферических днищ вне зоны отбортовки)");
                        body.AddParagraph("")
                            .AppendEquation("D_p=2∙R" +
                                            $"=2∙{(_element as EllipticalShell).EllR}={_Dp:f2}");
                        break;
                    }
            }

            switch (_nozzleDataIn.Location)
            {
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_1:
                    {
                        body.AddParagraph("Расчетный диаметр отверстия в стенке цилиндрической обечайки, конического перехода или выпуклого днища при наличии штуцера с круглым поперечным сечением, ось которого совпадает с нормалью к поверхности в центре отверстия");
                        body.AddParagraph("")
                            .AppendEquation("d_p=d+2∙c_s" +
                                            $"={_nozzleDataIn.d}+2∙{_nozzleDataIn.cs}={_dp:f2} мм");
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_2:
                    body.AddParagraph("Расчетный диаметр отверстия в стенке цилиндрической обечайки или конической обечайки при наличии наклонного штуцера, ось которого лежит в плоскости поперечного сечения укрепляемой обечайки");
                    body.AddParagraph("")
                        .AppendEquation("d_p=max{d;0.5∙t}+2∙c_s");
                    //TODO Add parametr t
                    //body.AddParagraph().AppendEquation($"d_p={_nozzleDataIn.d}+2∙{_nozzleDataIn.cs}={_dp:f2} мм");
                    break;
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_3:
                    {
                        body.AddParagraph("Расчетный диаметр отверстия в стенке эллиптического днища при наличии смещенного штуцера, ось которого параллельна оси днища");
                        body.AddParagraph("")
                            .AppendEquation("d_p=(d+2∙c_s)/√(1-((2∙x)/D_p)^2)");
                        //TODO: Add parametr x 
                        //body.AddParagraph().AppendEquation($"d_p={_nozzleDataIn.d}+2∙{_nozzleDataIn.cs}={_dp:f2} мм");
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_4:
                    {
                        body.AddParagraph("Расчетный диаметр отверстия при наличии наклонного штуцера с круглым поперечным сечением, когда максимальная ось симметрии отверстия некруглой формы составляет угол ω с образующей цилиндрической обечайки или с проекцией образующей конической обечайки на плоскость продольного сечения обечайки");
                        body.AddParagraph("")
                            .AppendEquation("d_p=(d+2∙c_s)(1+tg^2 γ∙cos^2 ω)");
                        //TODO
                        //body.AddParagraph().AppendEquation($"d_p={_nozzleDataIn.d}+2∙{_nozzleDataIn.cs}={_dp:f2} мм");
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_5:
                    body.AddParagraph("Расчетный диаметр отверстия для цилиндрической и конической обечаек, когда ось наклонного штуцера лежит в плоскости продольного сечения обечайки, а также для всех отверстий в сферическом и торосферическом днищах при наличии смещенного штуцера");
                    body.AddParagraph("")
                        .AppendEquation("d_p=(d+2∙c_s)/(cos^2 γ)");
                    //TODO
                    //body.AddParagraph().AppendEquation($"d_p={_nozzleDataIn.d}+2∙{_nozzleDataIn.cs}={_dp:f2} мм");
                    break;
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_6:
                    {
                        body.AddParagraph("Расчетный диаметр овального отверстия для перпендикулярно расположенного к поверхности обечайки штуцера с овальным поперечным сечением");
                        body.AddParagraph("")
                            .AppendEquation("d_p=(d+2∙c_s)[sin^2 ω +((d_1+2∙c_s)(d_1+d_2+4∙c_s))/(2(d_2+2∙c_s)^2)cos^2 ω");
                        //TODO
                        //body.AddParagraph().AppendEquation($"d_p={_nozzleDataIn.d}+2∙{_nozzleDataIn.cs}={_dp:f2} мм");
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_7:
                    {
                        body.AddParagraph("Расчетный диаметр отверстия для перпендикулярно расположенного к поверхности обечайки или днища штуцера с круглым поперечным сечением при наличии отбортовки или торообразной вставки");
                        body.AddParagraph("")
                            .AppendEquation("d_p=в+1.5(r-s_p)+2∙c_s");
                        //TODO
                        //body.AddParagraph().AppendEquation($"d_p={_nozzleDataIn.d}+2∙{_nozzleDataIn.cs}={_dp:f2} мм");
                        break;
                    }
            }

            body.AddParagraph("Расчетная толщина стенки укрепляемого элемента");
            if (_nozzleDataIn.ShellDataIn.ShellType == ShellType.Elliptical && _nozzleDataIn.ShellDataIn.IsPressureIn)
            {
                body.AddParagraph("")
                    .AppendEquation("s_p=(p∙D_p)/(4∙φ∙[σ]-p)" + 
                                    $"=({_nozzleDataIn.ShellDataIn.p}∙{_Dp:f2})/(4∙{_nozzleDataIn.ShellDataIn.fi}∙{_nozzleDataIn.ShellDataIn.sigma_d}-{_nozzleDataIn.ShellDataIn.p})={_sp:f2} мм");
            }
            else
            {
                body.Elements<Paragraph>().Last().AddRun(" определяется в соответствии с ГОСТ 34233.2");
                body.AddParagraph("").AppendEquation($"s_p={_sp:f2} мм");
            }
            body.AddParagraph("Расчетная толщина стенки штуцера с круглым поперечным сечением");
            body.AddParagraph("")
                .AppendEquation("s_1p=(p(d+2∙c_s))/(2∙φ_1∙[σ]_1-p)" +
                                $"=({_nozzleDataIn.ShellDataIn.p}({_nozzleDataIn.d}+2∙{_nozzleDataIn.cs}))/(2∙{_nozzleDataIn.fi1}∙{_nozzleDataIn.sigma_d1}-{_nozzleDataIn.ShellDataIn.p})={_s1p:f2} мм");

            body.AddParagraph("Расчетная длина внешней части штуцера");
            body.AddParagraph("").AppendEquation("l_1p=min{l_1;1.25√((d+2∙c_s)(s_1-c_s))}");
            body.AddParagraph("")
                .AppendEquation($"1.25√((d+2∙c_s)(s_1-c_s))=1.25√(({_nozzleDataIn.d}+2∙{_nozzleDataIn.cs})({_nozzleDataIn.s1}-{_nozzleDataIn.cs}))={_l1p2:f2} мм");
            body.AddParagraph("").AppendEquation($"l_1p=min({_nozzleDataIn.l1};{_l1p2:f2})={_l1p:f2} мм");

            if (_nozzleDataIn.l3 > 0)
            {
                body.AddParagraph("Расчетная длина внутренней части штуцера");
                body.AddParagraph("").AppendEquation("l_3p=min{l_3;0.5√((d+2∙c_s)(s_3-c_s-c_s1))}");
                body.AddParagraph("")
                    .AppendEquation($"0.5√((d+2∙c_s)(s_3-c_s-c_s1))=0.5√(({_nozzleDataIn.d}+2∙{_nozzleDataIn.cs})({_nozzleDataIn.s3}-{_nozzleDataIn.cs}-{_nozzleDataIn.cs1}))={_l3p2:f2} мм");
                body.AddParagraph("").AppendEquation($"l_3p=min({_nozzleDataIn.l3};{_l3p2:f2})={_l3p:f2} мм");
            }

            body.AddParagraph("Ширина зоны укрепления отверстия в цилиндрической обечайке");
            body.AddParagraph("")
                .AppendEquation("L_0=√(D_p∙(s-c))" +
                                $"=√({_Dp}∙({_nozzleDataIn.ShellDataIn.s}-{_c:f2}))={_L0:f2}");

            body.AddParagraph("Расчетная ширина зоны укрепления отверстия в стенке цилиндрической обечайки");

            switch (_nozzleDataIn.NozzleKind)
            {
                case NozzleKind.ImpassWithoutRing:
                case NozzleKind.PassWithoutRing:
                case NozzleKind.ImpassWithRing:
                case NozzleKind.PassWithRing:
                case NozzleKind.WithRingAndInPart:
                case NozzleKind.WithFlanging:
                    body.AddParagraph("").AppendEquation($"l_p=L_0={_lp:f2} мм");
                    break;
                case NozzleKind.WithTorusshapedInsert:
                case NozzleKind.WithWealdedRing:
                    body.AddParagraph("")
                        .AppendEquation($"l_p=min{{l;L_0}}=min{{{_nozzleDataIn.l};{_L0:f2}}}={_lp:f2} мм");
                    break;
            }

            if (_nozzleDataIn.l2 > 0)
            {
                body.AddParagraph("Расчетная ширина накладного кольца");
                body.AddParagraph("").AppendEquation("l_2p=min{l_2;√(D_p∙(s_2+s-c))}");
                body.AddParagraph("")
                    .AppendEquation($"√(D_p∙(s_2+s-c))=√({_Dp:f2}∙({_nozzleDataIn.s2}+{_nozzleDataIn.ShellDataIn.s}-{_c:f2}))={_l2p2:f2} мм");
                body.AddParagraph("").AppendEquation($"l_2p=min({_nozzleDataIn.l2};{_l2p2:f2})={_l2p:f2} мм");
            }

            if ((_psi1 is not (1 or 0)) | (_psi2 is not (1 or 0)) | (_psi3 is not (1 or 0)) | (_psi4 is not (1 or 0)))
            {
                body.AddParagraph("Учет применения различного материального исполнения");
            }
            if (!string.IsNullOrEmpty(_nozzleDataIn.steel1) && _nozzleDataIn.ShellDataIn.Steel != _nozzleDataIn.steel1)
            {
                body.AddParagraph("- для внешней части штуцера")
                    .AppendEquation($"χ_1=min(1;[σ]_1/[σ])=min(1;{_nozzleDataIn.sigma_d1}/{_nozzleDataIn.ShellDataIn.sigma_d})={_psi1:f2}");
            }
            if (!string.IsNullOrEmpty(_nozzleDataIn.steel2) && _nozzleDataIn.ShellDataIn.Steel != _nozzleDataIn.steel2)
            {
                body.AddParagraph("- для накладного кольца")
                    .AppendEquation($"χ_2=min(1;[σ]_2/[σ])=min(1;{_nozzleDataIn.sigma_d2}/{_nozzleDataIn.ShellDataIn.sigma_d})={_psi2:f2}");
            }
            if (!string.IsNullOrEmpty(_nozzleDataIn.steel3) && _nozzleDataIn.ShellDataIn.Steel != _nozzleDataIn.steel3)
            {
                body.AddParagraph("- для внутренней части штуцера")
                    .AppendEquation($"χ_3=min(1;[σ]_3/[σ])=min(1;{_nozzleDataIn.sigma_d3}/{_nozzleDataIn.ShellDataIn.sigma_d})={_psi3:f2}");
            }
            if (!string.IsNullOrEmpty(_nozzleDataIn.steel4) && _nozzleDataIn.ShellDataIn.Steel != _nozzleDataIn.steel4)
            {
                body.AddParagraph("- для торообразной вставки или вварного кольца")
                    .AppendEquation($"χ_4=min(1;[σ]_4/[σ])=min(1;{_nozzleDataIn.sigma_d4}/{_nozzleDataIn.ShellDataIn.sigma_d})={_psi4:f2}");
            }

            body.AddParagraph("Расчетный диаметр отверстия, не требующий укрепления в стенке цилиндрической обечайки при отсутствии избыточной толщины стенки сосуда и при наличии штуцера");
            body.AddParagraph("")
                .AppendEquation("d_0p=0,4√(D_p∙(s-c))" +
                                $"=0.4√({_Dp}∙({_nozzleDataIn.ShellDataIn.s}-{_c:f2}))={_d0p:f2} мм");

            body.AddParagraph("Проверка условия необходимости проведения расчета укрепления отверстий");
            body.AddParagraph("").AppendEquation("d_p≤d_0");

            body.AddParagraph("")
                .AppendEquation("d_0")
                .AddRun(" - наибольший допустимый диаметр одиночного отверстия, не требующего дополнительного укрепления при наличии избыточной толщины стенки сосуда");
            body.AddParagraph("")
                .AppendEquation("d_0=min{2∙((s-c)/s_pn-0.8)∙√(D_p∙(s-c));d_max+2∙c_s} ");
            body.AddParagraph("где - ")
                .AppendEquation("d_max")
                .AddRun(" - максимальный диаметр отверстия ");

            switch (_nozzleDataIn.ShellDataIn.ShellType)
            {
                case ShellType.Cylindrical:
                    {
                        body.AddParagraph("")
                            .AppendEquation($"d_max=D={_nozzleDataIn.ShellDataIn.D} мм")
                            .AddRun(" - для отверстий в цилиндрических обечайках");
                        break;
                    }
                case ShellType.Conical:
                    {
                        body.AddParagraph("")
                            .AppendEquation($"d_max=D_K={_dmax:f2} мм")
                            .AddRun(" - для отверстий в конических обечайках");
                        break;
                    }
                case ShellType.Elliptical:
                case ShellType.Spherical:
                case ShellType.Torospherical:
                    {
                        body.AddParagraph("")
                            .AppendEquation($"d_max=0.6∙D={_dmax:f2} мм")
                            .AddRun(" - для отверстий в выпуклых днищах");
                        break;
                    }
            }


            if (_nozzleDataIn.ShellDataIn.IsPressureIn)
            {
                body.AddParagraph("")
                    .AppendEquation($"s_pn=s_p={_sp:f2} мм")
                    .AddRun(" - в случае внутреннего давления");
            }
            else
            {
                body.AddParagraph("")
                    .AppendEquation("s_pn=(p_pn∙D_p)/(2∙K_1∙[σ]-p_pn)");
                switch (_nozzleDataIn.ShellDataIn.ShellType)
                {
                    case ShellType.Cylindrical:
                    case ShellType.Conical:
                        {
                            body.AddParagraph("")
                                .AppendEquation($"K_1={_K1}")
                                .AddRun(" - для цилиндрических и конических обечаек");
                            break;
                        }
                    case ShellType.Elliptical:
                    case ShellType.Spherical:
                    case ShellType.Torospherical:
                        {
                            body.AddParagraph("")
                                .AppendEquation($"K_1={_K1}")
                                .AddRun(" - для отверстий в выпуклых днищах");
                            break;
                        }
                }
                body.AddParagraph("").AppendEquation("p_pn=p/√(1-(p/[p]_E)^2)");
                body.AddParagraph("")
                    .AppendEquation("[p]_E")
                    .AddRun(" -  допускаемое наружное давление из условия устойчивости в пределах упругости, определяемое по ГОСТ 34233.2 для обечайки без отверстий");
                body.AddParagraph("")
                    .AppendEquation($"p_pn={_nozzleDataIn.ShellDataIn.p}/√(1-({_nozzleDataIn.ShellDataIn.p}/{_pen:f2})^2)={_ppn:f2} МПа");
                body.AddParagraph("")
                    .AppendEquation($"s_pn=({_ppn:f2}∙{_Dp:f2})/(2∙{_K1}∙{_nozzleDataIn.ShellDataIn.sigma_d}-{_ppn:f2})={_spn:f2} мм");
            }

            body.AddParagraph("")
                .AppendEquation($"2∙((s-c)/s_pn-0.8)∙√(D_p∙(s-c))=2∙(({_nozzleDataIn.ShellDataIn.s}-{_c:f2})/{_spn:f2}-0.8)∙√({_Dp:f2}∙({_nozzleDataIn.ShellDataIn.s}-{_c:f2}))={_d01:f2}");
            body.AddParagraph("")
                .AppendEquation($"d_max+2∙c_s={_dmax:f2}+2∙{_nozzleDataIn.cs}={_d02:f2}");
            body.AddParagraph("")
                .AppendEquation($"d_0=min({_d01:f2};{_d02:f2})={_d0:f2} мм");

            body.AddParagraph("").AppendEquation($"{_dp:f2}≤{_d0:f2}");
            if (_dp <= _d0)
            {
                body.AddParagraph("Условие прочности выполняется").Bold();
                body.Elements<Paragraph>().Last()
                    .AddRun(", следовательно дальнейших расчетов укрепления отверстия не требуется");
            }
            else
            {
                body.AddParagraph("Условие прочности не выполняется").Bold();
                body.Elements<Paragraph>().Last()
                    .AddRun(", следовательно необходим дальнейший расчет укрепления отверстия");
                body.AddParagraph("В случае укрепления отверстия утолщением стенки сосуда или штуцера, или накладным кольцом, или вварным кольцом, или торообразной вставкой, или отбортовкой должно выполняться условие");
                body.AddParagraph("")
                    .AppendEquation("l_1p∙(s_1-s_1p-c_s)∙χ_1+l_2p∙s_2∙χ_2+l_3p∙(s_3-c_s-c_s1)∙χ_3+l1p∙(s-s_p-c)∙χ_4≥0.5∙(d_p-d_0p)∙s_p");
                body.AddParagraph("")
                    .AppendEquation("l_1p∙(s_1-s_1p-c_s)∙χ_1+l_2p∙s_2∙χ_2+l_3p∙(s_3-c_s-c_s1)∙χ_3+l1p∙(s-s_p-c)∙χ_4=");
                body.AddParagraph("")
                    .AppendEquation($"{_l1p:f2}∙({_nozzleDataIn.s1}-{_s1p:f2}-{_nozzleDataIn.cs})∙{_psi1:f2}+{_l2p:f2}∙{_nozzleDataIn.s2}∙{_psi2:f2}+{_l3p:f2}∙({_nozzleDataIn.s3}-{_nozzleDataIn.cs}-{_nozzleDataIn.cs1})∙{_psi3:f2}+{_lp:f2}∙({_nozzleDataIn.ShellDataIn.s}-{_sp:f2}-{_c:f2})∙{_psi4:f2}={_conditionStrengthening1:f2}");
                body.AddParagraph("")
                    .AppendEquation($"0.5∙(d_p-d_0p)∙s_p=0.5∙({_dp:f2}-{_d0p:f2})∙{_sp:f2}={_conditionStrengthening2:f2}");
                body.AddParagraph("")
                    .AppendEquation($"{_conditionStrengthening1:f2}≥{_conditionStrengthening2:f2}");
                if (_conditionStrengthening1 >= _conditionStrengthening2)
                {
                    body.AddParagraph("Условие прочности выполняется").Bold();
                }
                else
                {
                    body.AddParagraph("Условие прочности не выполняется")
                        .Bold()
                        .Color(System.Drawing.Color.Red);
                }
            }

            body.AddParagraph("");

            if (_nozzleDataIn.ShellDataIn.IsPressureIn)
            {
                body.AddParagraph("Допускаемое внутреннее избыточное давление элемента сосуда с учетом ослабления стенки отверстием вычисляют по формуле");
                body.AddParagraph("").AppendEquation("[p]=(2∙K_1∙φ∙[σ]∙(s-c)∙V)/(D_p+(s-c)∙V)");
            }
            else 
            {
                body.AddParagraph("Допускаемое наружное давление элемента сосуда с учетом ослабления стенки отверстием вычисляют по формуле");
                body.AddParagraph("").AppendEquation("[p]=[p]_П/√(1+([p]_П/[p]_E)^2)");
                body.AddParagraph("где ")
                    .AppendEquation("[p]_П")
                    .AddRun(" - допускаемое наружное давление в пределах пластичности определяется как допускаемое внутреннее избыточное давление для сосуда или аппарата с отверстием при φ=1");
                body.AddParagraph("").AppendEquation("[p]_П=(2∙K_1∙φ∙[σ]∙(s-c)∙V)/(D_p+(s-c)∙V)");
            }

            switch (_nozzleDataIn.ShellDataIn.ShellType)
            {
                case ShellType.Cylindrical:
                case ShellType.Conical:
                    {
                        body.AddParagraph("")
                            .AppendEquation($"K_1={_K1}")
                            .AddRun(" - для цилиндрических и конических обечаек");
                        break;
                    }
                case ShellType.Elliptical:
                case ShellType.Spherical:
                case ShellType.Torospherical:
                    {
                        body.AddParagraph("")
                            .AppendEquation($"K_1={_K1}")
                            .AddRun(" - для отверстий в выпуклых днищах");
                        break;
                    }
            }

            body.AddParagraph("Коэффициент снижения прочности сосуда, ослабленного одиночным отверстием, вычисляют по формуле");
            body.AddParagraph("")
                .AppendEquation("V=min{(s_0-c)/(s-c);(χ_4+(l_1p∙(s_1-c_s)∙χ_1+l_2p∙s_2∙χ_2+l_3p∙(s_3-c_s-c_s1)∙χ_3)/(l_p∙(s-c)))/(1+0.5∙(d_p-d_0p)/l_p+K_1∙(d+2∙c_s)/D_p∙(φ/φ_1)∙(l_1p/l_p))}");

            switch (_nozzleDataIn.NozzleKind)
            {
                case NozzleKind.ImpassWithoutRing:
                case NozzleKind.PassWithoutRing:
                case NozzleKind.WithFlanging:
                    {
                        body.AddParagraph("При отсутствии накладного кольца и укреплении отверстия штуцером ")
                            .AppendEquation("s_2=0 , s_0=s , χ_4=1");
                        break;
                    }
                case NozzleKind.ImpassWithRing:
                case NozzleKind.PassWithRing:
                case NozzleKind.WithRingAndInPart:
                    {
                        body.AddParagraph("При отсутствии вварного кольца или торообразной вставки ")
                            .AppendEquation("s_0=s , χ_4=1");
                        break;
                    }
            }

            body.AddParagraph("")
                .AppendEquation($"(s_0-c)/(s-c)=({_nozzleDataIn.s0}-{_c:f2})/({_nozzleDataIn.ShellDataIn.s}-{_c:f2})={_V1:f2}");
            body.AddParagraph("")
                .AppendEquation("(χ_4+(l_1p∙(s_1-c_s)∙χ_1+l_2p∙s_2∙χ_2+l_3p∙(s_3-c_s-c_s1)∙χ_3)/(l_p∙(s-c)))/(1+0.5∙(d_p-d_0p)/l_p+K_1∙(d+2∙c_s)/D_p∙(φ/φ_1)∙(l_1p/l_p))=");
            body.AddParagraph("")
                .AppendEquation($"({_psi4:f2}+({_l1p:f2}∙({_nozzleDataIn.s1}-{_nozzleDataIn.cs})∙{_psi1:f2}+{_l2p:f2}∙{_nozzleDataIn.s2}∙{_psi2:f2}+{_l3p:f2}∙({_nozzleDataIn.s3}-{_nozzleDataIn.cs}-{_nozzleDataIn.cs1})∙{_psi3:f2})/({_lp:f2}∙({_nozzleDataIn.ShellDataIn.s}-{_c:f2})))/(1+0.5∙({_dp:f2}-{_d0p:f2})/{_lp:f2}+{_K1}∙({_nozzleDataIn.d}+2∙{_nozzleDataIn.cs})/{_Dp:f2}∙({_nozzleDataIn.fi}/{_nozzleDataIn.fi1})∙({_l1p:f2}/{_lp:f2}))={_V2:f2}");

            body.AddParagraph("").AppendEquation($"V=min({_V1:f2};{_V2:f2})={_V:f2} ");

            if (_nozzleDataIn.ShellDataIn.IsPressureIn)
            {
                body.AddParagraph("")
                    .AppendEquation($"[p]=(2∙{_K1}∙{_nozzleDataIn.fi}∙{_nozzleDataIn.ShellDataIn.sigma_d}∙({_nozzleDataIn.ShellDataIn.s}-{_c:f2})∙{_V:f2})/({_Dp:f2}+({_nozzleDataIn.ShellDataIn.s}-{_c:f2})∙{_V:f2})={_p_d:f2} МПа");
            }
            else 
            {
                body.AddParagraph("")
                    .AppendEquation($"[p]_p=(2∙{_K1}∙{_nozzleDataIn.fi}∙{_nozzleDataIn.ShellDataIn.sigma_d}∙({_nozzleDataIn.ShellDataIn.s}-{_c:f2})∙{_V:f2})/({_Dp}+({_nozzleDataIn.ShellDataIn.s}-{_c:f2})∙{_V:f2})={_p_dp:f2} МПа");
                body.AddParagraph("")
                    .AppendEquation("[p]_E")
                    .AddRun(" - допускаемое наружное давление из условия устойчивости в пределах упругости, определяемое по ГОСТ 34233.2 для соответствующих обечайки и днища без отверстий");
                body.AddParagraph("").AppendEquation($"[p]_E={_p_de:f2} МПа)");
                body.AddParagraph("")
                    .AppendEquation($"[p]={_p_dp:f2}/√(1+({_p_dp:f2}/{_p_de:f2})^2)={_p_d:f2} МПа");
            }
            body.AddParagraph("").AppendEquation("[p]≥p");
            body.AddParagraph("").AppendEquation($"{_p_d:f2} МПа >= {_nozzleDataIn.ShellDataIn.p} МПа");
            if (_p_d >= _nozzleDataIn.ShellDataIn.p)
            {
                body.AddParagraph("Условие прочности выполняется").Bold();
            }
            else
            {
                body.AddParagraph("Условие прочности не выполняется")
                    .Bold()
                    .Color(System.Drawing.Color.Red);
            }

            body.AddParagraph("Условия применения расчетных формул");
            switch (_nozzleDataIn.ShellDataIn.ShellType)
            {
                case ShellType.Cylindrical:
                    {
                        body.AddParagraph("")
                            .AppendEquation("(d_p-2∙c_s)/D" +
                                            $"=({_dp:f2}-2∙{_nozzleDataIn.cs})/{_nozzleDataIn.ShellDataIn.D}={_conditionUseFormulas1:f2}≤1");
                        body.AddParagraph("")
                            .AppendEquation("(s-c)/D" +
                                            $"=({_nozzleDataIn.ShellDataIn.s}-{_c:f2})/({_nozzleDataIn.ShellDataIn.D})={_conditionUseFormulas2:f2}≤0.1");
                        break;
                    }
                case ShellType.Conical:
                    {
                        body.AddParagraph("")
                            .AppendEquation("(d_p-2∙c_s)/D_K" +
                                            $"=({_dp:f2}-2∙{_nozzleDataIn.cs})/{_Dk}={_conditionUseFormulas1:f2}≤1");
                        body.AddParagraph("")
                            .AppendEquation("(s-c)/D_K≤0.1/cosα");
                        body.AddParagraph("")
                            .AppendEquation($"({_nozzleDataIn.ShellDataIn.s}-{_c:f2})/({_Dk})={_conditionUseFormulas2:f2}");
                        body.AddParagraph("")
                            .AppendEquation($"0.1/cos{_alfa1}");
                        break;
                    }
                case ShellType.Elliptical:
                case ShellType.Spherical:
                case ShellType.Torospherical:
                    {
                        body.AddParagraph("")
                            .AppendEquation("(d_p-2∙c_s)/D " +
                                            $"=({_dp:f2}-2∙{_nozzleDataIn.cs})/{_nozzleDataIn.ShellDataIn.D}={_conditionUseFormulas1:f2}≤0.6");
                        body.AddParagraph("")
                            .AppendEquation("(s-c)/D" +
                                            $"=({_nozzleDataIn.ShellDataIn.s}-{_c:f2})/({_nozzleDataIn.ShellDataIn.D})={_conditionUseFormulas2:f2}≤0.1");
                        break;
                    }
            }
            package.Close();
        }
    }
}
