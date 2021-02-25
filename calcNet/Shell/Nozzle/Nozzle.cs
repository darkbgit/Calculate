using System;
using System.Collections.Generic;
using System.Linq;
using Xceed.Document.NET;

namespace calcNet
{
    class Nozzle : IElement
    {
        public Nozzle(IElement element, NozzleDataIn nozzleDataIn)
        {
            this.nozzleDataIn = nozzleDataIn;
            //this.shellData = (shell as Shell).ShellDataIn;
            this.element = element;
        }

        private readonly NozzleDataIn nozzleDataIn;
        private readonly IElement element;


        private List<string> errorList = new List<string>();

        private NozzleLocation location;

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

        public bool IsCriticalError { get => isCriticalError; }
        public bool IsError { get => isError; }
        public List<string> ErrorList { get => errorList; }
        public double p_d { get => _p_d; }
        public double d0 { get => _d0; }
        public double b { get => _b; }

        private double _E2;
        private double _E3;
        private double _E4;






        internal NozzleKind nozzleKind;





        //public void SetValue(string name, double value)
        //{
        //    var field = typeof(DataNozzle_in).GetField(name);
        //    field.SetValue(this, value);
        //}

        //public void GetValue(string name, ref string value)
        //{
        //    var field = typeof(DataNozzle_in).GetField(name);
        //    value = field.GetValue(this).ToString();
        //}

        private double _p_d;
        private double _p_dp;
        private double _p_de;

        private double _Dp;
        private double _dp;
        private double _d0p;
        private double _s1p;
        private double _b;
        private double _d0;
        private double _d01;
        private double _d02;
        private double _dmax;
        private double _sp;
        private double _spn;
        private double _ppn;
        private double _pen;
        private int _K1;
        private double _lp;
        private double _l1p;
        private double _l1p2;
        private double _l2p;
        private double _l2p2;
        private double _l3p;
        private double _l3p2;
        private double _L0;
        private double _psi1;
        private double _psi2;
        private double _psi3;
        private double _psi4 = 1;

        private double _V;
        private double _V1;
        private double _V2;
        private double _yslyk1;
        private double _yslyk2;
        private double _B1n;
        

        private bool isConditionUseFormuls;
        private double _ypf1;
        private double _ypf2;


        private double _c;
        private double _s_calcr;
        private double _Dk;
        private double _p_deShell;


        private bool isCriticalError;
        private bool isError;




        public void Calculate()
        {
            _c = (element as Shell).c;
            _s_calcr = (element as Shell).s_calcr;
            if (!nozzleDataIn.ShellDataIn.IsPressureIn) _p_deShell = (element as Shell).p_de;
            if (nozzleDataIn.ShellDataIn.ShellType == ShellType.Conical) _Dk = (element as ConicalShell).Dk;

            // расчет Dp, dp
            switch (nozzleDataIn.ShellDataIn.ShellType)
            {
                case ShellType.Cylindrical:
                    {
                        _Dp = nozzleDataIn.ShellDataIn.D;
                        break;
                    }
                case ShellType.Conical:
                    {
                        _Dp = nozzleDataIn.ShellDataIn.D / Math.Cos(Math.PI * 100 * (nozzleDataIn.ShellDataIn as ConicalShellDataIn).alfa);
                        break;
                    }
                case ShellType.Elliptical:
                    {
                        if ((nozzleDataIn.ShellDataIn as EllipticalShellDataIn).ellH * 100 ==
                            nozzleDataIn.ShellDataIn.D * 25)
                        {
                            _Dp = nozzleDataIn.ShellDataIn.D * 2 * Math.Sqrt(1 - 3 * Math.Pow(nozzleDataIn.ellx / nozzleDataIn.ShellDataIn.D, 2));
                        }
                        else
                        {
                            _Dp = Math.Pow(nozzleDataIn.ShellDataIn.D, 2) /
                                ((nozzleDataIn.ShellDataIn as EllipticalShellDataIn).ellH * 2) *
                                Math.Sqrt(1 - (4 * (Math.Pow(nozzleDataIn.ShellDataIn.D, 2) - 4 *
                                Math.Pow((nozzleDataIn.ShellDataIn as EllipticalShellDataIn).ellH, 2)) *
                                               Math.Pow(nozzleDataIn.ellx, 2)) /
                                    Math.Pow(nozzleDataIn.ShellDataIn.D, 4));
                        }
                        break;
                    }
                case ShellType.Spherical:
                case ShellType.Torospherical:
                    {
                        //_Dp = 2 * nozzleDataIn.ShellDataIn.R;
                        break;
                    }
                default:
                    {
                        errorList.Add("Ошибка вида укрепляемого элемента");
                        break;
                    }
            }

            if (nozzleDataIn.ShellDataIn.ShellType == ShellType.Elliptical && nozzleDataIn.ShellDataIn.IsPressureIn)
            {
                _sp = nozzleDataIn.ShellDataIn.p * _Dp / (4 * nozzleDataIn.ShellDataIn.fi * nozzleDataIn.ShellDataIn.sigma_d - nozzleDataIn.ShellDataIn.p);
            }
            else
            {
                _sp = (element as Shell).s_calcr;
            }

            if (!nozzleDataIn.IsOval)
            {
                _s1p = nozzleDataIn.ShellDataIn.p * (nozzleDataIn.d + 2 * nozzleDataIn.cs) / (2 * nozzleDataIn.fi * nozzleDataIn.sigma_d1 - nozzleDataIn.ShellDataIn.p);
            }
            else
            {
                _s1p = nozzleDataIn.ShellDataIn.p * (nozzleDataIn.d1 + 2 * nozzleDataIn.cs) / (2 * nozzleDataIn.fi * nozzleDataIn.sigma_d1 - nozzleDataIn.ShellDataIn.p);
            }

            switch (nozzleDataIn.Location)
            {
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_1:
                    {
                        _dp = nozzleDataIn.d + 2 * nozzleDataIn.cs; //dp = d + 2cs
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_2:
                    {
                        _dp = Math.Max(nozzleDataIn.d, 0.5 * nozzleDataIn.tTransversely) + (2 * nozzleDataIn.cs); //dp =max{d; 0,5t} + 2cs
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_3:
                    {
                        _dp = (nozzleDataIn.d + 2 * nozzleDataIn.cs) / Math.Sqrt(1 + Math.Pow(2 * nozzleDataIn.ellx / _Dp, 2));
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_4:
                    {
                        _dp = (nozzleDataIn.d + 2 * nozzleDataIn.cs) * (1 + Math.Pow(Math.Tan(Math.PI * 180 * nozzleDataIn.gamma), 2) *
                            Math.Pow(Math.Cos(Math.PI * 180 * nozzleDataIn.omega), 2));
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_5:
                    {
                        _dp = (nozzleDataIn.d + 2 * nozzleDataIn.cs) / Math.Pow(Math.Cos(Math.PI * 180 * nozzleDataIn.gamma), 2);
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_6:
                    {
                        _dp = (nozzleDataIn.d2 + 2 * nozzleDataIn.cs) *
                            (Math.Pow(Math.Sin(Math.PI * 180 * nozzleDataIn.omega), 2) +
                            (nozzleDataIn.d1 + 2 * nozzleDataIn.cs) *
                            (nozzleDataIn.d1 + nozzleDataIn.d2 + 4 * nozzleDataIn.cs) /
                            (2 * Math.Pow(nozzleDataIn.d2 + 2 * nozzleDataIn.cs, 2)) *
                            Math.Pow(Math.Cos(Math.PI * 180 * nozzleDataIn.omega), 2));
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_7:
                    {
                        _dp = nozzleDataIn.d + 1.5 * (nozzleDataIn.r - _sp) + (2 * nozzleDataIn.cs);
                        break;
                    }
                default:
                    {
                        isCriticalError = true;
                        errorList.Add("Ошибка места расположения штуцера");
                        break;
                    }
            }

            // l1p, l3p, l2p
            {
                double d;
                if (!nozzleDataIn.IsOval)
                {
                    d = nozzleDataIn.d;
                }
                else
                {
                    d = nozzleDataIn.d2;
                }

                _l1p2 = 1.25 * Math.Sqrt((d + 2 * nozzleDataIn.cs) * (nozzleDataIn.s1 - nozzleDataIn.cs));
                _l1p = Math.Min(nozzleDataIn.l1, _l1p2);
                if (nozzleDataIn.s3 == 0)
                {
                    _l3p = 0;
                }
                else
                {
                    _l3p2 = 0.5 * Math.Sqrt((d + 2 * nozzleDataIn.cs) * (nozzleDataIn.s3 - nozzleDataIn.cs - nozzleDataIn.cs1));
                    _l3p = Math.Min(nozzleDataIn.l3, _l3p2);
                }
            }

            _L0 = Math.Sqrt(_Dp * (nozzleDataIn.ShellDataIn.s - _c));

            switch (nozzleDataIn.NozzleKind)
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
                    _lp = Math.Min(nozzleDataIn.l, _L0);
                    break;
            }

            _l2p2 = Math.Sqrt(_Dp * (nozzleDataIn.s2 + nozzleDataIn.ShellDataIn.s - _c));
            _l2p = Math.Min(nozzleDataIn.l2, _l2p2);

            switch (nozzleDataIn.NozzleKind)
            {
                case NozzleKind.ImpassWithoutRing:
                case NozzleKind.PassWithoutRing:
                case NozzleKind.ImpassWithRing:
                case NozzleKind.PassWithRing:
                case NozzleKind.WithRingAndInPart:
                case NozzleKind.WithFlanging:
                    nozzleDataIn.s0 = nozzleDataIn.ShellDataIn.s;
                    nozzleDataIn.steel4 = nozzleDataIn.steel1;
                    break;
            }


            _psi1 = Math.Min(1, nozzleDataIn.sigma_d1 / nozzleDataIn.ShellDataIn.sigma_d);
            _psi2 = Math.Min(1, nozzleDataIn.sigma_d2 / nozzleDataIn.ShellDataIn.sigma_d);
            _psi3 = Math.Min(1, nozzleDataIn.sigma_d3 / nozzleDataIn.ShellDataIn.sigma_d);
            _psi4 = Math.Min(1, nozzleDataIn.sigma_d4 / nozzleDataIn.ShellDataIn.sigma_d);

            _d0p = 0.4 * Math.Sqrt(_Dp * (nozzleDataIn.ShellDataIn.s - _c));

            _b = Math.Sqrt(_Dp * (nozzleDataIn.ShellDataIn.s - _c)) + Math.Sqrt(_Dp * (nozzleDataIn.ShellDataIn.s - _c));

            switch (nozzleDataIn.ShellDataIn.ShellType)
            {
                case ShellType.Cylindrical:
                    {
                        _dmax = nozzleDataIn.ShellDataIn.D;
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
                        _dmax = 0.6 * nozzleDataIn.ShellDataIn.D;
                        break;
                    }
            }

            switch (nozzleDataIn.ShellDataIn.ShellType)
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

            if (nozzleDataIn.ShellDataIn.IsPressureIn)
            {
                _spn = _sp;
            }
            else
            {

                //_B1n = Math.Min(1, 9.45 * (nozzleDataIn.ShellDataIn.D / d_out.l) * Math.Sqrt(nozzleDataIn.ShellDataIn.D / (100 * (nozzleDataIn.ShellDataIn.s - _c))));
                //_pen = 2.08 * 0.00001 * nozzleDataIn.ShellDataIn.E / (nozzleDataIn.ny * _B1n) * (nozzleDataIn.ShellDataIn.D / d_out.l) * Math.Pow(100 * (nozzleDataIn.ShellDataIn.s - _c) / nozzleDataIn.ShellDataIn.D, 2.5);
                _pen = _p_deShell;
                _ppn = nozzleDataIn.ShellDataIn.p / Math.Sqrt(1 - Math.Pow(nozzleDataIn.ShellDataIn.p / _pen, 2));
                _spn = _ppn * _Dp / (2 * _K1 * nozzleDataIn.ShellDataIn.sigma_d - _ppn);
            }


            _d01 = 2 * ((nozzleDataIn.ShellDataIn.s - _c) / _spn - 0.8) * Math.Sqrt(_Dp * (nozzleDataIn.ShellDataIn.s - _c));
            _d02 = _dmax + 2 * nozzleDataIn.cs;
            _d0 = Math.Min(_d01, _d02);

            if (_dp > _d0)
            {
                _yslyk1 = _l1p * (nozzleDataIn.s1 - _s1p - nozzleDataIn.cs) * _psi1 + _l2p * nozzleDataIn.s2 * _psi2 + _l3p * (nozzleDataIn.s3 - nozzleDataIn.cs - nozzleDataIn.cs1) * _psi3 + _lp * (nozzleDataIn.ShellDataIn.s - _s_calcr - _c) * _psi4;
                _yslyk2 = 0.5 * (_dp - _d0p) * _s_calcr;
                if (_yslyk1 < _yslyk2)
                {
                    isError = true;
                    errorList.Add("Условие укрепления одиночного отверстия не выполняется");
                }
            }



            _V1 = (nozzleDataIn.s0 - _c) / (nozzleDataIn.ShellDataIn.s - _c);
            _V2 = (_psi4 + (_l1p * (nozzleDataIn.s1 - nozzleDataIn.cs) * _psi1 + _l2p * nozzleDataIn.s2 * _psi2 + _l3p * (nozzleDataIn.s3 - nozzleDataIn.cs - nozzleDataIn.cs1) * _psi3) / _lp * (nozzleDataIn.ShellDataIn.s - _c)) / (1 + 0.5 * (_dp - _d0p) / _lp + _K1 * (nozzleDataIn.d + 2 * nozzleDataIn.cs) / _Dp * (nozzleDataIn.fi / nozzleDataIn.fi1) * (_l1p / _lp));
            _V = Math.Min(_V1, _V2);

            if (nozzleDataIn.ShellDataIn.IsPressureIn)
            {
                _p_d = 2 * _K1 * nozzleDataIn.fi * nozzleDataIn.ShellDataIn.sigma_d * (nozzleDataIn.ShellDataIn.s - _c) * _V / (_Dp + (nozzleDataIn.ShellDataIn.s - _c) * _V);
            }
            else
            {
                _p_dp = 2 * _K1 * nozzleDataIn.fi * nozzleDataIn.ShellDataIn.sigma_d * (nozzleDataIn.ShellDataIn.s - _c) * _V / (_Dp + (nozzleDataIn.ShellDataIn.s - _c) * _V);
                _p_de = _p_deShell;
                _p_d = _p_dp / Math.Sqrt(1 + Math.Pow(_p_dp / _p_de, 2));
            }
            if (_p_d < nozzleDataIn.ShellDataIn.p)
            {
                isError = true;
                errorList.Add("Допускаемое давление меньше расчетного");
            }

            switch (nozzleDataIn.ShellDataIn.ShellType)
            {
                case ShellType.Cylindrical:
                    {
                        _ypf1 = (_dp - 2 * nozzleDataIn.cs) / nozzleDataIn.ShellDataIn.D;
                        _ypf2 = (nozzleDataIn.ShellDataIn.s - _c) / nozzleDataIn.ShellDataIn.D;
                        isConditionUseFormuls = _ypf1 <= 1 & _ypf2 <= 0.1;
                        break;
                    }
                case ShellType.Conical:
                    {
                        _ypf1 = (_dp - 2 * nozzleDataIn.cs) / _Dk;
                        _ypf2 = (nozzleDataIn.ShellDataIn.s - _c) / _Dk;
                        isConditionUseFormuls = _ypf1 <= 1 & _ypf2 <= 0.1 / Math.Cos(Math.PI * 180 * (nozzleDataIn.ShellDataIn as ConicalShellDataIn).alfa);
                         break;
                    }
                case ShellType.Elliptical:
                case ShellType.Spherical:
                case ShellType.Torospherical:
                    {
                        _ypf1 = (_dp - 2 * nozzleDataIn.cs) / nozzleDataIn.ShellDataIn.D;
                        _ypf2 = (nozzleDataIn.ShellDataIn.s - _c) / nozzleDataIn.ShellDataIn.D;
                        isConditionUseFormuls = _ypf1 <= 0.6 & _ypf2 <= 0.1;
                        break;
                    }
            }
            if (!isConditionUseFormuls)
            {
                isError = true;
                errorList.Add( "Условие применения формул не выполняется");
            }

        }

        public void MakeWord(string filename)
        {
            var doc = Xceed.Words.NET.DocX.Load(filename);
            doc.InsertParagraph().InsertPageBreakAfterSelf();
            doc.InsertParagraph($"Расчет на прочность узла врезки штуцера {nozzleDataIn.Name} в ").Heading(HeadingType.Heading1).Alignment = Alignment.center;
            switch (nozzleDataIn.ShellDataIn.ShellType)
            {
                case ShellType.Cylindrical:
                    doc.Paragraphs.Last().Append($"обечайку {nozzleDataIn.ShellDataIn.Name}, нагруженную ");
                    break;
                case ShellType.Conical:
                    doc.Paragraphs.Last().Append($"коническую обечайку {nozzleDataIn.ShellDataIn.Name}, нагруженную ");
                    break;
                case ShellType.Elliptical:
                    doc.Paragraphs.Last().Append($"эллиптическое днище {nozzleDataIn.ShellDataIn.Name}, нагруженное ");
                    break;
            }

            doc.Paragraphs.Last().Append(nozzleDataIn.ShellDataIn.IsPressureIn
                ? "внутренним избыточным давлением"
                : "наружным давлением");
            doc.InsertParagraph();
            doc.InsertParagraph("Исходные данные").Alignment = Alignment.center;

            //table
            {
                var table = doc.AddTable(1, 2);
                table.SetWidths(new float[] { 200, 200 });

                table.Rows[0].Cells[0].Paragraphs[0].Append("Элемент:");
                table.Rows[0].Cells[1].Paragraphs[0].Append($"Штуцер {nozzleDataIn.Name}");

                table.InsertRow();
                table.Rows[1].Cells[0].Paragraphs[0].Append("Элемент несущий штуцер:");
                table.Rows[1].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.ShellDataIn.Name}");

                table.InsertRow();
                table.Rows[2].Cells[0].Paragraphs[0].Append("Тип элемента, несущего штуцер:");
                switch (nozzleDataIn.ShellDataIn.ShellType)
                {
                    case ShellType.Cylindrical:
                        table.Rows[2].Cells[1].Paragraphs[0].Append("Обечайка цилиндрическая");
                        break;
                    case ShellType.Conical:
                        table.Rows[2].Cells[1].Paragraphs[0].Append("Обечайка коническая");
                        break;
                    case ShellType.Elliptical:
                        table.Rows[2].Cells[1].Paragraphs[0].Append("Днище эллиптическое");
                        break;
                }
                table.InsertRow();
                table.Rows[3].Cells[0].Paragraphs[0].Append("Тип штуцера:");
                switch (nozzleDataIn.NozzleKind)
                {
                    case NozzleKind.ImpassWithoutRing:
                        table.Rows[3].Cells[1].Paragraphs[0].Append("Непроходящий без укрепления");
                        break;
                    case NozzleKind.PassWithoutRing:
                        table.Rows[3].Cells[1].Paragraphs[0].Append("Проходящий без укрепления");
                        break;
                    case NozzleKind.ImpassWithRing:
                        table.Rows[3].Cells[1].Paragraphs[0].Append("Непроходящий с накладным кольцом");
                        break;
                    case NozzleKind.PassWithRing:
                        table.Rows[3].Cells[1].Paragraphs[0].Append("Проходящий с накладным кольцом");
                        break;
                    case NozzleKind.WithRingAndInPart:
                        table.Rows[3].Cells[1].Paragraphs[0].Append("С накладным кольцом и внутренней частью");
                        break;
                    case NozzleKind.WithFlanging:
                        table.Rows[3].Cells[1].Paragraphs[0].Append("С отбортовкой");
                        break;
                    case NozzleKind.WithTorusshapedInsert:
                        table.Rows[3].Cells[1].Paragraphs[0].Append("С торовой вставкой");
                        break;
                    case NozzleKind.WithWealdedRing:
                        table.Rows[3].Cells[1].Paragraphs[0].Append("С вварным кольцом");
                        break;
                }
                doc.InsertParagraph().InsertTableAfterSelf(table);
            }

            var image = doc.AddImage($"pic/Nozzle/Nozzle{nozzleDataIn.NozzleKind}.gif");
            var picture = image.CreatePicture();
            doc.InsertParagraph().AppendPicture(picture);

            //table
            {
                var table = doc.AddTable(1, 2);
                table.SetWidths(new float[] { 300, 100 });

                int i = 0;
                table.Rows[i].Cells[0].Paragraphs[0].Append("Материал несущего элемента:");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.ShellDataIn.Steel}");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Толщина стенки несущего элемента, s:");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.ShellDataIn.s} мм");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Сумма прибавок к стенке несущего элемента, c:");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{_c:f2} мм");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Материал штуцера");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.steel1}");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Внутренний диаметр штуцера, d:");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.d} мм");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Толщина стенки штуцера, ")
                                                    .AppendEquation("s_1")
                                                    .Append(":");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.s1} мм");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Длина наружной части штуцера, ")
                                                    .AppendEquation("s_1")
                                                    .Append(":");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.l1} мм");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Сумма прибавок к толщине стенки штуцера, ")
                                                    .AppendEquation("c_s")
                                                    .Append(":");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.cs} мм");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Прибавка на коррозию, ")
                                                    .AppendEquation("c_s1")
                                                    .Append(":");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.cs1} мм");

                switch (nozzleDataIn.NozzleKind)
                {
                    case NozzleKind.ImpassWithoutRing:
                        {
                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Минимальный размер сварного шва, Δ:");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.delta} мм");
                            break;
                        }
                    case NozzleKind.PassWithoutRing:
                        {
                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Длина внутренней части штуцера, ")
                                                                .AppendEquation("l_3")
                                                                .Append(":");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.l3} мм");

                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Толщина внутренней части штуцера, ")
                                                                .AppendEquation("s_3")
                                                                .Append(":");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.s3} мм");

                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Минимальный размер сварного шва, Δ:");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.delta} мм");
                            break;
                        }
                    case NozzleKind.ImpassWithRing:
                        {
                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Ширина накладного кольца, ")
                                                                .AppendEquation("l_2")
                                                                .Append(":");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.l2} мм");

                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Толщина накладного кольца, ")
                                                                .AppendEquation("s_2")
                                                                .Append(":");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.s2} мм");

                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Минимальный размер сварного шва, Δ:");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.delta} мм");
                            break;
                        }
                    case NozzleKind.PassWithRing:
                        {
                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Ширина накладного кольца, ")
                                                                .AppendEquation("l_2")
                                                                .Append(":");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.l2} мм");

                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Толщина накладного кольца, ")
                                                                .AppendEquation("s_2")
                                                                .Append(":");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.s2} мм");

                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Длина внутренней части штуцера, ")
                                                                .AppendEquation("l_3")
                                                                .Append(":");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.l3} мм");

                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Толщина внутренней части штуцера, ")
                                                                .AppendEquation("s_3")
                                                                .Append(":");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.s3} мм");

                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Минимальный размер сварного шва, Δ:");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.delta} мм");
                            break;
                        }
                    case NozzleKind.WithRingAndInPart:
                        {
                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Ширина накладного кольца, ")
                                                                .AppendEquation("l_2")
                                                                .Append(":");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.l2} мм");

                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Толщина накладного кольца, ")
                                                                .AppendEquation("s_2")
                                                                .Append(":");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.s2} мм");

                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Длина внутренней части штуцера, ")
                                                                .AppendEquation("l_3")
                                                                .Append(":");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.l3} мм");

                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Толщина внутренней части штуцера, ")
                                                                .AppendEquation("s_3")
                                                                .Append(":");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.s3} мм");

                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Минимальный размер сварного шва, Δ:");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.delta} мм");
                            break;
                        }
                    case NozzleKind.WithFlanging:
                        {
                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Радиус отбортовки, r:");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.r} мм");

                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Минимальный размер сварного шва, Δ:");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.delta} мм");
                            break;
                        }
                    case NozzleKind.WithTorusshapedInsert: //UNDONE:
                        break;
                    case NozzleKind.WithWealdedRing:
                        break;
                }
                doc.InsertParagraph().InsertTableAfterSelf(table);
            }
            doc.InsertParagraph();

            doc.InsertParagraph("Коэффициенты прочности сварных швов:");
            doc.InsertParagraph("Продольный шов штуцера ").AppendEquation($"φ_1={nozzleDataIn.fi1}");
            doc.InsertParagraph("Шов обечайки в зоне врезки штуцера ").AppendEquation($"φ={nozzleDataIn.fi}");

            doc.InsertParagraph();
            doc.InsertParagraph("Условия нагружения").Alignment = Alignment.center;

            //table
            {
                var table = doc.AddTable(1, 2);
                table.SetWidths(new float[] { 300, 100 });

                int i = 0;
                table.Rows[i].Cells[0].Paragraphs[0].Append("Расчетная температура, Т:");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.ShellDataIn.t} °С");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append(nozzleDataIn.ShellDataIn.IsPressureIn
                    ? "Расчетное внутреннее избыточное давление, p:"
                    : "Расчетное наружное давление, p:");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.ShellDataIn.p} МПа");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append($"Допускаемое напряжение для материала {nozzleDataIn.steel1} при расчетной температуре, ")
                                                    .AppendEquation("[σ]_1")
                                                    .Append(":");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.sigma_d1} МПа");

                if (!nozzleDataIn.ShellDataIn.IsPressureIn)
                {
                    table.InsertRow(++i);
                    table.Rows[i].Cells[0].Paragraphs[0].Append("Модуль продольной упругости при расчетной температуре, ")
                                                        .AppendEquation("E_1")
                                                        .Append(":");
                    table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.E1} МПа");
                }
                if (nozzleDataIn.steel1 != nozzleDataIn.steel2)
                {
                    table.InsertRow(++i);
                    table.Rows[i].Cells[0].Paragraphs[0].Append($"Допускаемое напряжение для материала {nozzleDataIn.steel2} при расчетной температуре, ")
                                                        .AppendEquation("[σ]_2")
                                                        .Append(":");
                    table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.sigma_d2} МПа");

                    if (!nozzleDataIn.ShellDataIn.IsPressureIn)
                    {
                        table.InsertRow(++i);
                        table.Rows[i].Cells[0].Paragraphs[0].Append("Модуль продольной упругости при расчетной температуре, ")
                                                            .AppendEquation("E_2")
                                                            .Append(":");
                        table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.E2} МПа");
                    }
                }
                if (nozzleDataIn.steel1 != nozzleDataIn.steel3)
                {
                    table.InsertRow(++i);
                    table.Rows[i].Cells[0].Paragraphs[0].Append($"Допускаемое напряжение для материала {nozzleDataIn.steel3} при расчетной температуре, ")
                                                        .AppendEquation("[σ]_3")
                                                        .Append(":");
                    table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.sigma_d3} МПа");

                    if (!nozzleDataIn.ShellDataIn.IsPressureIn)
                    {
                        table.InsertRow(++i);
                        table.Rows[i].Cells[0].Paragraphs[0].Append("Модуль продольной упругости при расчетной температуре, ")
                                                            .AppendEquation("E_3")
                                                            .Append(":");
                        table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.E3} МПа");
                    }
                }
                if (nozzleDataIn.steel1 != nozzleDataIn.steel4)
                {
                    table.InsertRow(++i);
                    table.Rows[i].Cells[0].Paragraphs[0].Append($"Допускаемое напряжение для материала {nozzleDataIn.steel4} при расчетной температуре, ")
                                                        .AppendEquation("[σ]_4")
                                                        .Append(":");
                    table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.sigma_d4} МПа");

                    if (!nozzleDataIn.ShellDataIn.IsPressureIn)
                    {
                        table.InsertRow(++i);
                        table.Rows[i].Cells[0].Paragraphs[0].Append("Модуль продольной упругости при расчетной температуре, ")
                                                            .AppendEquation("E_4")
                                                            .Append(":");
                        table.Rows[i].Cells[1].Paragraphs[0].Append($"{nozzleDataIn.E4} МПа");
                    }
                }
                doc.InsertParagraph().InsertTableAfterSelf(table);
            }

            doc.InsertParagraph();
            doc.InsertParagraph("Результаты расчета").Alignment = Alignment.center;
            doc.InsertParagraph();


            doc.InsertParagraph("Расчетные параметры").Alignment = Alignment.center;
            doc.InsertParagraph();


            doc.InsertParagraph("Расчетный диаметр укрепляемого элемента ");

            switch (nozzleDataIn.ShellDataIn.ShellType)
            {
                case ShellType.Cylindrical:
                    {
                        doc.Paragraphs.Last().Append("(для цилиндрической обечайки)");
                        doc.InsertParagraph().AppendEquation($"D_p=D={nozzleDataIn.ShellDataIn.D} мм");
                        break;
                    }
                case ShellType.Conical:
                    {
                        doc.Paragraphs.Last().Append("(для конической обечайки, перехода или днища)");
                        doc.InsertParagraph().AppendEquation("D_p=D_k/cos(α)");
                        break;
                    }
                case ShellType.Elliptical:
                    {
                        if ((nozzleDataIn.ShellDataIn as EllipticalShellDataIn).ellH * 100 == nozzleDataIn.ShellDataIn.D * 25)
                        {
                            doc.InsertParagraph("(для эллиптического днища при H=0.25D)");
                            doc.InsertParagraph().AppendEquation("D_p=2∙D∙√(1-3∙(x/D)^2)");
                            doc.InsertParagraph().AppendEquation($"D_p=2∙{nozzleDataIn.ShellDataIn.D}∙√(1-3∙({nozzleDataIn.ellx}/{nozzleDataIn.ShellDataIn.D})^2)={_Dp:f2} мм");
                        }
                        else
                        {
                            doc.InsertParagraph("(для эллиптического днища)");
                            doc.InsertParagraph().AppendEquation("D_p=D^2/(2∙H)∙√(1-(D^2-4∙H^2)/D^4∙x^2)");
                            doc.InsertParagraph().AppendEquation($"D_p={nozzleDataIn.ShellDataIn.D}^2/(2∙{(nozzleDataIn.ShellDataIn as EllipticalShellDataIn).ellH})∙√(1-({nozzleDataIn.ShellDataIn.D}^2-4∙{(nozzleDataIn.ShellDataIn as EllipticalShellDataIn).ellH}^2)/{nozzleDataIn.ShellDataIn.D}^4∙{nozzleDataIn.ellx}^2)={_Dp:f2} мм");
                        }
                        break;
                    }
                case ShellType.Spherical:
                case ShellType.Torospherical:
                    {
                        doc.Paragraphs.Last().Append("(для сферических и торосферических днищ вне зоны отбортовки)");
                        doc.InsertParagraph().AppendEquation("D_p=2∙R");
                        break;
                    }
            }

            switch (nozzleDataIn.Location)
            {
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_1:
                    {
                        doc.InsertParagraph("Расчетный диаметр отверстия в стенке цилиндрической обечайки, конического перехода или выпуклого днища при наличии штуцера с круглым поперечным сечением, ось которого совпадает с нормалью к поверхности в центре отверстия");
                        doc.InsertParagraph().AppendEquation("d_p=d+2∙c_s");
                        doc.InsertParagraph().AppendEquation($"d_p={nozzleDataIn.d}+2∙{nozzleDataIn.cs}={_dp:f2} мм");
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_2:
                    doc.InsertParagraph("Расчетный диаметр отверстия в стенке цилиндрической обечайки или конической обечайки при наличии наклонного штуцера, ось которого лежит в плоскости поперечного сечения укрепляемой обечайки");
                    doc.InsertParagraph().AppendEquation("d_p=max{d;0.5∙t}+2∙c_s");
                    //doc.InsertParagraph().AppendEquation($"d_p={nozzleDataIn.d}+2∙{nozzleDataIn.cs}={_dp:f2} мм");
                    break;
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_3:
                    {
                        doc.InsertParagraph("Расчетный диаметр отверстия в стенке эллиптического днища при наличии смещенного штуцера, ось которого параллельна оси днища");
                        doc.InsertParagraph().AppendEquation("d_p=(d+2∙c_s)/√(1-((2∙x)/D_p)^2)");
                        //doc.InsertParagraph().AppendEquation($"d_p={nozzleDataIn.d}+2∙{nozzleDataIn.cs}={_dp:f2} мм");
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_4:
                    {
                        doc.InsertParagraph("Расчетный диаметр отверстия при наличии наклонного штуцера с круглым поперечным сечением, когда максимальная ось симметрии отверстия некруглой формы составляет угол ω с образующей цилиндрической обечайки или с проекцией образующей конической обечайки на плоскость продольного сечения обечайки");
                        doc.InsertParagraph().AppendEquation("d_p=(d+2∙c_s)(1+tg^2 γ∙cos^2 ω)");
                        //doc.InsertParagraph().AppendEquation($"d_p={nozzleDataIn.d}+2∙{nozzleDataIn.cs}={_dp:f2} мм");
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_5:
                    doc.InsertParagraph("Расчетный диаметр отверстия для цилиндрической и конической обечаек, когда ось наклонного штуцера лежит в плоскости продольного сечения обечайки, а также для всех отверстий в сферическом и торосферическом днищах при наличии смещенного штуцера");
                    doc.InsertParagraph().AppendEquation("d_p=(d+2∙c_s)/(cos^2 γ)");
                    //doc.InsertParagraph().AppendEquation($"d_p={nozzleDataIn.d}+2∙{nozzleDataIn.cs}={_dp:f2} мм");
                    break;
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_6:
                    {
                        doc.InsertParagraph("Расчетный диаметр овального отверстия для перпендикулярно расположенного к поверхности обечайки штуцера с овальным поперечным сечением");
                        doc.InsertParagraph().AppendEquation("d_p=(d+2∙c_s)[sin^2 ω +((d_1+2∙c_s)(d_1+d_2+4∙c_s))/(2(d_2+2∙c_s)^2)cos^2 ω");
                        //doc.InsertParagraph().AppendEquation($"d_p={nozzleDataIn.d}+2∙{nozzleDataIn.cs}={_dp:f2} мм");
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_7:
                    {
                        doc.InsertParagraph("Расчетный диаметр отверстия для перпендикулярно расположенного к поверхности обечайки или днища штуцера с круглым поперечным сечением при наличии отбортовки или торообразной вставки");
                        doc.InsertParagraph().AppendEquation("d_p=в+1.5(r-s_p)+2∙c_s");
                        //doc.InsertParagraph().AppendEquation($"d_p={nozzleDataIn.d}+2∙{nozzleDataIn.cs}={_dp:f2} мм");
                        break;
                    }
            }

            doc.InsertParagraph("Расчетная толщина стенки укрепляемого элемента");
            if (nozzleDataIn.ShellDataIn.ShellType == ShellType.Elliptical && nozzleDataIn.ShellDataIn.IsPressureIn)
            {
                doc.InsertParagraph().AppendEquation("s_p=(p∙D_p)/(4∙φ∙[σ]-p)");
                doc.InsertParagraph().AppendEquation($"s_p=({nozzleDataIn.ShellDataIn.p}∙{_Dp:f2})/(4∙{nozzleDataIn.ShellDataIn.fi}∙{nozzleDataIn.ShellDataIn.sigma_d}-{nozzleDataIn.ShellDataIn.p})={_sp:f2} мм");
            }
            else
            {
                doc.Paragraphs.Last().Append(" определяется в соответствии с ГОСТ 34233.2");
                doc.InsertParagraph().AppendEquation($"s_p={_sp:f2} мм");
            }
            doc.InsertParagraph("Расчетная толщина стенки штуцера с круглым поперечным сечением");
            doc.InsertParagraph().AppendEquation("s_1p=(p(d+2∙c_s))/(2∙φ_1∙[σ]_1-p)");
            doc.InsertParagraph().AppendEquation($"s_1p=({nozzleDataIn.ShellDataIn.p}({nozzleDataIn.d}+2∙{nozzleDataIn.cs}))/(2∙{nozzleDataIn.fi1}∙{nozzleDataIn.sigma_d1}-{nozzleDataIn.ShellDataIn.p})={_s1p:f2} мм");

            doc.InsertParagraph("Расчетная длина внешней части штуцера");
            doc.InsertParagraph().AppendEquation("l_1p=min{l_1;1.25√((d+2∙c_s)(s_1-c_s))}");
            doc.InsertParagraph().AppendEquation($"1.25√((d+2∙c_s)(s_1-c_s))=1.25√(({nozzleDataIn.d}+2∙{nozzleDataIn.cs})({nozzleDataIn.s1}-{nozzleDataIn.cs}))={_l1p2:f2} мм");
            doc.InsertParagraph().AppendEquation($"l_1p=min({nozzleDataIn.l1};{_l1p2:f2})={_l1p:f2} мм");

            if (nozzleDataIn.l3 > 0)
            {
                doc.InsertParagraph("Расчетная длина внутренней части штуцера");
                doc.InsertParagraph().AppendEquation("l_3p=min{l_3;0.5√((d+2∙c_s)(s_3-c_s-c_s1))}");
                doc.InsertParagraph().AppendEquation($"0.5√((d+2∙c_s)(s_3-c_s-c_s1))=0.5√(({nozzleDataIn.d}+2∙{nozzleDataIn.cs})({nozzleDataIn.s3}-{nozzleDataIn.cs}-{nozzleDataIn.cs1}))={_l3p2:f2} мм");
                doc.InsertParagraph().AppendEquation($"l_3p=min({nozzleDataIn.l3};{_l3p2:f2})={_l3p:f2} мм");
            }

            doc.InsertParagraph("Ширина зоны укрепления отверстия в цилиндрической обечайке");
            doc.InsertParagraph().AppendEquation("L_0=√(D_p∙(s-c))");
            doc.InsertParagraph().AppendEquation($"L_0=√({_Dp}∙({nozzleDataIn.ShellDataIn.s}-{_c:f2}))={_L0:f2}");

            doc.InsertParagraph("Расчетная ширина зоны укрепления отверстия в стенке цилиндрической обечайки");

            switch (nozzleDataIn.NozzleKind)
            {
                case NozzleKind.ImpassWithoutRing:
                case NozzleKind.PassWithoutRing:
                case NozzleKind.ImpassWithRing:
                case NozzleKind.PassWithRing:
                case NozzleKind.WithRingAndInPart:
                case NozzleKind.WithFlanging:
                    doc.InsertParagraph().AppendEquation($"l_p=L_0={_lp:f2} мм");
                    break;
                case NozzleKind.WithTorusshapedInsert:
                case NozzleKind.WithWealdedRing:
                    doc.InsertParagraph().AppendEquation("l_p=min{l;L_0}");
                    doc.InsertParagraph().AppendEquation($"l_p=min({nozzleDataIn.l};{_L0:f2})={_lp:f2} мм");
                    break;
            }

            if (nozzleDataIn.l2 > 0)
            {
                doc.InsertParagraph("Расчетная ширина накладного кольца");
                doc.InsertParagraph().AppendEquation("l_2p=min{l_2;√(D_p∙(s_2+s-c))}");
                doc.InsertParagraph().AppendEquation($"√(D_p∙(s_2+s-c))=√({_Dp}∙({nozzleDataIn.s2}+{nozzleDataIn.ShellDataIn.s}-{_c:f2}))={_l2p2:f2} мм");
                doc.InsertParagraph().AppendEquation($"l_2p=min({nozzleDataIn.l2};{_l2p2:f2})={_l2p:f2} мм");
            }

            if (_psi1 != 1 | _psi2 != 1 | _psi3 != 1 | _psi4 != 1)
            {
                doc.InsertParagraph("Учет применения различного материального исполнения");
            }
            if (nozzleDataIn.ShellDataIn.Steel != nozzleDataIn.steel1)
            {
                doc.InsertParagraph("- для внешней части штуцера").AppendEquation($"χ_1=min(1;[σ]_1/[σ])=min(1;{nozzleDataIn.sigma_d1}/{nozzleDataIn.ShellDataIn.sigma_d})={_psi1:f2}");
            }
            if (nozzleDataIn.ShellDataIn.Steel != nozzleDataIn.steel2)
            {
                doc.InsertParagraph("- для накладного кольца").AppendEquation($"χ_2=min(1;[σ]_2/[σ])=min(1;{nozzleDataIn.sigma_d2}/{nozzleDataIn.ShellDataIn.sigma_d})={_psi2:f2}");
            }
            if (nozzleDataIn.ShellDataIn.Steel != nozzleDataIn.steel3)
            {
                doc.InsertParagraph("- для внутренней части штуцера").AppendEquation($"χ_3=min(1;[σ]_3/[σ])=min(1;{nozzleDataIn.sigma_d3}/{nozzleDataIn.ShellDataIn.sigma_d})={_psi3:f2}");
            }
            if (nozzleDataIn.ShellDataIn.Steel != nozzleDataIn.steel4)
            {
                doc.InsertParagraph("- для торообразной вставки или вварного кольца").AppendEquation($"χ_4=min(1;[σ]_4/[σ])=min(1;{nozzleDataIn.sigma_d4}/{nozzleDataIn.ShellDataIn.sigma_d})={_psi4:f2}");
            }

            doc.InsertParagraph("Расчетный диаметр отверстия, не требующий укрепления в стенке цилиндрической обечайки при отсутствии избыточной толщины стенки сосуда и при наличии штуцера");
            doc.InsertParagraph().AppendEquation("d_0p=0,4√(D_p∙(s-c))");
            doc.InsertParagraph().AppendEquation($"d_0p=0.4√({_Dp}∙({nozzleDataIn.ShellDataIn.s}-{_c:f2}))={_d0p:f2} мм");


            doc.InsertParagraph("Проверка условия необходимости проведения расчета укрепления отверстий");
            doc.InsertParagraph().AppendEquation("d_p≤d_0");

            doc.InsertParagraph().AppendEquation("d_0").Append(" - наибольший допустимый диаметр одиночного отверстия, не требующего дополнительного укрепления при наличии избыточной толщины стенки сосуда");
            doc.InsertParagraph().AppendEquation("d_0=min{2∙((s-c)/s_pn-0.8)∙√(D_p∙(s-c));d_max+2∙c_s} ");
            doc.InsertParagraph("где - ").AppendEquation("d_max").Append(" - максимальный диаметр отверстия ");

            switch (nozzleDataIn.ShellDataIn.ShellType)
            {
                case ShellType.Cylindrical:
                    {
                        doc.InsertParagraph().AppendEquation($"d_max=D={nozzleDataIn.ShellDataIn.D} мм").AppendLine(" - для отверстий в цилиндрических обечайках");
                        break;
                    }
                case ShellType.Conical:
                    {
                        doc.InsertParagraph().AppendEquation($"d_max=D_K={_dmax:f2} мм").AppendLine(" - для отверстий в конических обечайках");
                        break;
                    }
                case ShellType.Elliptical:
                case ShellType.Spherical:
                case ShellType.Torospherical:
                    {
                        doc.InsertParagraph().AppendEquation($"d_max=0.6∙D={_dmax:f2} мм").AppendLine(" - для отверстий в выпуклых днищах");
                        break;
                    }
            }


            if (nozzleDataIn.ShellDataIn.IsPressureIn)
            {
                doc.InsertParagraph().AppendEquation($"s_pn=s_p={_sp:f2} мм").AppendEquation(" - в случае внутреннего давления");
            }
            else
            {
                doc.InsertParagraph().AppendEquation("s_pn=(p_pn∙D_p)/(2∙K_1∙[σ]-p_pn)");
                switch (nozzleDataIn.ShellDataIn.ShellType)
                {
                    case ShellType.Cylindrical:
                    case ShellType.Conical:
                        {
                            doc.InsertParagraph().AppendEquation($"K_1={_K1}").Append(" - для цилиндрических и конических обечаек");
                            break;
                        }
                    case ShellType.Elliptical:
                    case ShellType.Spherical:
                    case ShellType.Torospherical:
                        {
                            doc.InsertParagraph().AppendEquation($"K_1={_K1}").Append(" - для отверстий в выпуклых днищах");
                            break;
                        }
                }
                doc.InsertParagraph().AppendEquation("p_pn=p/√(1-(p/[p]_E)^2)");
                doc.InsertParagraph().AppendEquation("[p]_E").Append(" -  допускаемое наружное давление из условия устойчивости в пределах упругости, определяемое по ГОСТ 34233.2 для обечайки без отверстий");
                doc.InsertParagraph().AppendEquation($"p_pn={nozzleDataIn.ShellDataIn.p}/√(1-({nozzleDataIn.ShellDataIn.p}/{_pen:f2})^2)={_ppn:f2} МПа");
                doc.InsertParagraph().AppendEquation($"s_pn=({_ppn:f2}∙{_Dp:f2})/(2∙{_K1}∙{nozzleDataIn.ShellDataIn.sigma_d}-{_ppn:f2})={_spn:f2} мм");
            }

            doc.InsertParagraph().AppendEquation($"2∙((s-c)/s_pn-0.8)∙√(D_p∙(s-c))=2∙(({nozzleDataIn.ShellDataIn.s}-{_c:f2})/{_spn:f2}-0.8)∙√({_Dp:f2}∙({nozzleDataIn.ShellDataIn.s}-{_c:f2}))={_d01:f2}");
            doc.InsertParagraph().AppendEquation($"d_max+2∙c_s={_dmax:f2}+2∙{nozzleDataIn.cs}={_d02:f2}");
            doc.InsertParagraph().AppendEquation($"d_0=min({_d01:f2};{_d02:f2})={_d0:f2} мм");

            doc.InsertParagraph().AppendEquation($"{_dp:f2}≤{_d0:f2}");
            if (_dp <= _d0)
            {
                doc.InsertParagraph("Условие прочности выполняется").Bold();
                doc.Paragraphs.Last().Append(", следовательно дальнейших расчетов укрепления отверстия не требуется").Bold(false);
            }
            else
            {
                doc.InsertParagraph("Условие прочности не выполняется").Bold();
                doc.Paragraphs.Last().Append(", следовательно необходим дальнейший расчет укрепления отверстия").Bold(false);
                doc.InsertParagraph("В случае укрепления отверстия утолщением стенки сосуда или штуцера, или накладным кольцом, или вварным кольцом, или торообразной вставкой, или отбортовкой должно выполняться условие");
                doc.InsertParagraph().AppendEquation("l_1p∙(s_1-s_1p-c_s)∙χ_1+l_2p∙s_2∙χ_2+l_3p∙(s_3-c_s-c_s1)∙χ_3+l1p∙(s-s_p-c)∙χ_4≥0.5∙(d_p-d_0p)∙s_p");
                doc.InsertParagraph().AppendEquation("l_1p∙(s_1-s_1p-c_s)∙χ_1+l_2p∙s_2∙χ_2+l_3p∙(s_3-c_s-c_s1)∙χ_3+l1p∙(s-s_p-c)∙χ_4=");
                doc.InsertParagraph().AppendEquation($"{_l1p:f2}∙({nozzleDataIn.s1}-{_s1p:f2}-{nozzleDataIn.cs})∙{_psi1}+{_l2p:f2}∙{nozzleDataIn.s2}∙{_psi2}+{_l3p:f2}∙({nozzleDataIn.s3}-{nozzleDataIn.cs}-{nozzleDataIn.cs1})∙{_psi3:f2}+{_lp:f2}∙({nozzleDataIn.ShellDataIn.s}-{_sp:f2}-{_c:f2})∙{_psi4}={_yslyk1:f2}");
                doc.InsertParagraph().AppendEquation($"0.5∙(d_p-d_0p)∙s_p=0.5∙({_dp:f2}-{_d0p:f2})∙{_sp:f2}={_yslyk2:f2}");
                doc.InsertParagraph().AppendEquation($"{_yslyk1:f2}≥{_yslyk2:f2}");
                if (_yslyk1 >= _yslyk2)
                {
                    doc.InsertParagraph("Условие прочности выполняется").Bold();
                }
                else
                {
                    doc.InsertParagraph("Условие прочности не выполняется").Bold().Color(System.Drawing.Color.Red);
                }
            }

            doc.InsertParagraph();


            if (nozzleDataIn.ShellDataIn.IsPressureIn)
            {
                doc.InsertParagraph("Допускаемое внутреннее избыточное давление элемента сосуда с учетом ослабления стенки отверстием вычисляют по формуле");
                doc.InsertParagraph().AppendEquation("[p]=(2∙K_1∙φ∙[σ]∙(s-c)∙V)/(D_p+(s-c)∙V)");
            }
            else if (!nozzleDataIn.ShellDataIn.IsPressureIn)
            {
                doc.InsertParagraph("Допускаемое наружное давление элемента сосуда с учетом ослабления стенки отверстием вычисляют по формуле");
                doc.InsertParagraph().AppendEquation("[p]=[p]_П/√(1+([p]_П/[p]_E)^2)");
                doc.InsertParagraph("где ").AppendEquation("[p]_П").Append(" - допускаемое наружное давление в пределах пластичности определяется как допускаемое внутреннее избыточное давление для сосуда или аппарата с отверстием при φ=1");
                doc.InsertParagraph().AppendEquation("[p]_П=(2∙K_1∙φ∙[σ]∙(s-c)∙V)/(D_p+(s-c)∙V)");
            }

            switch (nozzleDataIn.ShellDataIn.ShellType)
            {
                case ShellType.Cylindrical:
                case ShellType.Conical:
                    {
                        doc.InsertParagraph().AppendEquation($"K_1={_K1}").Append(" - для цилиндрических и конических обечаек");
                        break;
                    }
                case ShellType.Elliptical:
                case ShellType.Spherical:
                case ShellType.Torospherical:
                    {
                        doc.InsertParagraph().AppendEquation($"K_1={_K1}").Append(" - для отверстий в выпуклых днищах");
                        break;
                    }
            }

            doc.InsertParagraph("Коэффициент снижения прочности сосуда, ослабленного одиночным отверстием, вычисляют по формуле");
            doc.InsertParagraph().AppendEquation("V=min{(s_0-c)/(s-c);(χ_4+(l_1p∙(s_1-c_s)∙χ_1+l_2p∙s_2∙χ_2+l_3p∙(s_3-c_s-c_s1)∙χ_3)/(l_p∙(s-c)))/(1+0.5∙(d_p-d_0p)/l_p+K_1∙(d+2∙c_s)/D_p∙(φ/φ_1)∙(l_1p/l_p))}").Alignment = Alignment.center;

            switch (nozzleDataIn.NozzleKind)
            {
                case NozzleKind.ImpassWithoutRing:
                case NozzleKind.PassWithoutRing:
                case NozzleKind.WithFlanging:
                    {
                        doc.InsertParagraph("При отсутствии накладного кольца и укреплении отверстия штуцером ").AppendEquation("s_2=0 , s_0=s , χ_4=1");
                        break;
                    }
                case NozzleKind.ImpassWithRing:
                case NozzleKind.PassWithRing:
                case NozzleKind.WithRingAndInPart:
                    {
                        doc.InsertParagraph("При отсутствии вварного кольца или торообразной вставки ").AppendEquation("s_0=s , χ_4=1");
                        break;
                    }
            }

            doc.InsertParagraph().AppendEquation($"(s_0-c)/(s-c)=({nozzleDataIn.s0}-{_c:f2})/({nozzleDataIn.ShellDataIn.s}-{_c:f2})={_V1:f2}");
            doc.InsertParagraph().AppendEquation("(χ_4+(l_1p∙(s_1-c_s)∙χ_1+l_2p∙s_2∙χ_2+l_3p∙(s_3-c_s-c_s1)∙χ_3)/(l_p∙(s-c)))/(1+0.5∙(d_p-d_0p)/l_p+K_1∙(d+2∙c_s)/D_p∙(φ/φ_1)∙(l_1p/l_p))=");
            doc.InsertParagraph().AppendEquation($"({_psi4}+({_l1p:f2}∙({nozzleDataIn.s1}-{nozzleDataIn.cs})∙{_psi1}+{_l2p:f2}∙{nozzleDataIn.s2}∙{_psi2}+{_l3p:f2}∙({nozzleDataIn.s3}-{nozzleDataIn.cs}-{nozzleDataIn.cs1})∙{_psi3:f2})/({_lp:f2}∙({nozzleDataIn.ShellDataIn.s}-{_c:f2})))/(1+0.5∙({_dp:f2}-{_d0p:f2})/{_lp:f2}+{_K1}∙({nozzleDataIn.d}+2∙{nozzleDataIn.cs})/{_Dp}∙({nozzleDataIn.fi}/{nozzleDataIn.fi1})∙({_l1p:f2}/{_lp:f2}))={_V2:f2}");

            doc.InsertParagraph().AppendEquation($"V=min({_V1:f2};{_V2:f2})={_V:f2} ");

            if (nozzleDataIn.ShellDataIn.IsPressureIn)
            {
                doc.InsertParagraph().AppendEquation($"[p]=(2∙{_K1}∙{nozzleDataIn.fi}∙{nozzleDataIn.ShellDataIn.sigma_d}∙({nozzleDataIn.ShellDataIn.s}-{_c:f2})∙{_V:f2})/({_Dp}+({nozzleDataIn.ShellDataIn.s}-{_c:f2})∙{_V:f2})={_p_d:f2} МПа");
            }
            else if (!nozzleDataIn.ShellDataIn.IsPressureIn)
            {
                doc.InsertParagraph().AppendEquation($"[p]_p=(2∙{_K1}∙{nozzleDataIn.fi}∙{nozzleDataIn.ShellDataIn.sigma_d}∙({nozzleDataIn.ShellDataIn.s}-{_c:f2})∙{_V:f2})/({_Dp}+({nozzleDataIn.ShellDataIn.s}-{_c:f2})∙{_V:f2})={_p_dp:f2} МПа");
                doc.InsertParagraph().AppendEquation("[p]_E").Append(" - допускаемое наружное давление из условия устойчивости в пределах упругости, определяемое по ГОСТ 34233.2 для соответствующих обечайки и днища без отверстий");
                doc.InsertParagraph().AppendEquation($"[p]_E={_p_de:f2} МПа)");
                doc.InsertParagraph().AppendEquation($"[p]={_p_dp:f2}/√(1+({_p_dp:f2}/{_p_de:f2})^2)={_p_d:f2} МПа");
            }
            doc.InsertParagraph().AppendEquation("[p]≥p");
            doc.InsertParagraph().AppendEquation($"{_p_d:f2} МПа >= {nozzleDataIn.ShellDataIn.p} МПа");
            if (_p_d >= nozzleDataIn.ShellDataIn.p)
            {
                doc.InsertParagraph("Условие прочности выполняется").Bold();
            }
            else
            {
                doc.InsertParagraph("Условие прочности не выполняется").Bold().Color(System.Drawing.Color.Red);
            }

            doc.InsertParagraph("Границы применения формул");
            switch (nozzleDataIn.ShellDataIn.ShellType)
            {
                case ShellType.Cylindrical:
                    {
                        doc.InsertParagraph().AppendEquation("(d_p-2∙c_s)/D≤1");
                        doc.InsertParagraph().AppendEquation($"({_dp:f2}-2∙{nozzleDataIn.cs})/{nozzleDataIn.ShellDataIn.D}={_ypf1:f2}≤1");
                        doc.InsertParagraph().AppendEquation("(s-c)/D≤0.1");
                        doc.InsertParagraph().AppendEquation($"({nozzleDataIn.ShellDataIn.s}-{_c:f2})/({nozzleDataIn.ShellDataIn.D})={_ypf2:f2}≤0.1");
                        break;
                    }
                case ShellType.Conical:
                    {
                        doc.InsertParagraph().AppendEquation("(d_p-2∙c_s)/D_K≤1");
                        //doc.InsertParagraph().AppendEquation($"({_dp:f2}-2∙{nozzleDataIn.cs})/{nozzleDataIn.ShellDataIn.DK}={_ypf1:f2}≤1");
                        doc.InsertParagraph().AppendEquation("(s-c)/D_K≤0.1/cosα");
                        //doc.InsertParagraph().AppendEquation($"({nozzleDataIn.ShellDataIn.s}-{_c:f2})/({nozzleDataIn.ShellDataIn.DK})={_ypf2:f2}≤0.1/cos{nozzleDataIn.ShellDataIn.alfa}");
                        break;
                    }
                case ShellType.Elliptical:
                case ShellType.Spherical:
                case ShellType.Torospherical:
                    {
                        doc.InsertParagraph().AppendEquation("(d_p-2∙c_s)/D≤0.6");
                        doc.InsertParagraph().AppendEquation($"({_dp:f2}-2∙{nozzleDataIn.cs})/{nozzleDataIn.ShellDataIn.D}={_ypf1:f2}≤0.6");
                        doc.InsertParagraph().AppendEquation("(s-c)/D≤0.1");
                        doc.InsertParagraph().AppendEquation($"({nozzleDataIn.ShellDataIn.s}-{_c:f2})/({nozzleDataIn.ShellDataIn.D})={_ypf2:f2}≤0.1");
                        break;
                    }
            }
            doc.Save();
        }
    }
}
