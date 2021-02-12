using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Document.NET;

namespace calcNet.Calculate
{
    class Nozzle : IElement, IDataIn
    {
        public Nozzle (IElement shell)
        {
            this.shell = shell;
        }
        public void CheckData()
        {
            if (ErrorList?.Count > 0)
            {
                IsDataGood = false;
            }
            else
            {
                IsDataGood = true;
            }
        }

        public bool IsDataGood { get; set; }
        public List<string> ErrorStringList { get => err; }
        public List<string> ErrorList { get => errorList; }

        public string Error
        {
            get => error;
            set
            {
                error += value;
            }
        }

        private readonly ShellType shellType;
        private readonly IElement shell;
        private ShellDataIn shellData; 

        private string error;
        private List<string> err = new List<string>();
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



        internal double sigma_d1,
                        sigma_d2,
                        sigma_d3,
                        sigma_d4;

        public double E1,
                        E2,
                        E3,
                        E4;
        internal double D,
                        d1,
                        d2,
                        r;


        internal double s0,
                        s1,
                        s2,
                        s3;

        internal double cs;
        internal double cs1;

        internal int l;
        internal int l1;
        internal int l2;
        internal int l3;

        internal double fi,
                        fi1;

        internal int delta;
        internal int delta1;
        internal int delta2;
        internal double elx,
                        b;
        internal NozzleKind nozzleKind;

        internal int temp;


        internal double ny = 2.4;
        internal double t;
        internal double gamma;
        internal double omega;

        internal bool isOval;

        public void SetValue(string name, double value)
        {
            var field = typeof(DataNozzle_in).GetField(name);
            field.SetValue(this, value);
        }

        public void GetValue(string name, ref string value)
        {
            var field = typeof(DataNozzle_in).GetField(name);
            value = field.GetValue(this).ToString();
        }

        internal double p_d, p_dp, p_de;
        
        private double _Dp;
        internal double dp;
        internal double d0p;
        internal double s1p;
        internal double d0;
        internal double d01;
        internal double d02;
        internal double dmax;
        internal double sp;
        internal double spn;
        internal double ppn;
        internal int K1;
        internal double lp;
        internal double l1p;
        internal double l1p2;
        internal double l2p;
        internal double l2p2;
        internal double l3p;
        internal double l3p2;
        internal double L0;
        internal double psi1;
        internal double psi2;
        internal double psi3;
        internal double psi4 = 1;
        internal double b;
        internal double V;
        internal double V1;
        internal double V2;
        internal double yslyk1;
        internal double yslyk2;
        internal double B1n;
        internal double pen;
        //internal string err;
        internal bool ypf;
        internal double ypf1;
        internal double ypf2;

        public void Calculate()
        {
            // расчет Dp, dp
            switch ((shell as Shell).ShellType)
            {
                case ShellType.Cylindrical:
                    {
                        _Dp = (shell as CylindricalShell).ShellDataIn.D;
                        break;
                    }
                case ShellType.Conical:
                    {
                        //_Dp = (shell as ConicalShell).ShellDataIn.D / Math.Cos(Math.PI * 100 * (shell as ConicalShell).ShellDataIn.alfa);
                        break;
                    }
                case ShellType.Elliptical:
                    {
                        if ((shell as Ellid_in.elH * 100 == d_in.D * 25)
                        {
                            dN_out.Dp = d_in.D * 2 * Math.Sqrt(1 - 3 * Math.Pow(dN_in.elx / d_in.D, 2));
                        }
                        else
                        {
                            dN_out.Dp = Math.Pow(d_in.D, 2) / (d_in.elH * 2) * Math.Sqrt(1 - (4 * (Math.Pow(d_in.D, 2) - 4 * Math.Pow(d_in.elH, 2)) * Math.Pow(dN_in.elx, 2)) / Math.Pow(d_in.D, 4));
                        }
                        break;
                    }
                case ShellType.Spherical:
                case ShellType.Torospherical:
                    {
                        dN_out.Dp = 2 * d_in.R;
                        break;
                    }
                default:
                    {
                        dN_out.err += "Ошибка вида укрепляемого элемента\n";
                        break;
                    }
            }

            if ((shell as Shell).ShellType == ShellType.Elliptical && d_in.isPressureIn)
            {
                dN_out.sp = d_in.p * dN_out.Dp / (4 * d_in.fi * d_in.sigma_d - d_in.p);
            }
            else
            {
                dN_out.sp = d_out.s_calcr;
            }

            if (!dN_in.isOval)
            {
                dN_out.s1p = d_in.p * (dN_in.D + 2 * dN_in.cs) / (2 * dN_in.fi * dN_in.sigma_d1 - d_in.p);
            }
            else
            {
                dN_out.s1p = d_in.p * (dN_in.d1 + 2 * dN_in.cs) / (2 * dN_in.fi * dN_in.sigma_d1 - d_in.p);
            }

            switch (dN_in.location)
            {
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_1:
                    {
                        dN_out.dp = dN_in.D + 2 * dN_in.cs; //dp = d + 2cs
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_2:
                    {
                        dN_out.dp = Math.Max(dN_in.D, 0.5 * dN_in.t) + (2 * dN_in.cs); //dp =max{d; 0,5t} + 2cs
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_3:
                    {
                        dN_out.dp = (dN_in.D + 2 * dN_in.cs) / Math.Sqrt(1 + Math.Pow(2 * dN_in.elx / dN_out.Dp, 2));
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_4:
                    {
                        dN_out.dp = (dN_in.D + 2 * dN_in.cs) * (1 + Math.Pow(Math.Tan(Math.PI * 180 * dN_in.gamma), 2) *
                            Math.Pow(Math.Cos(Math.PI * 180 * dN_in.omega), 2));
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_5:
                    {
                        dN_out.dp = (dN_in.D + 2 * dN_in.cs) / Math.Pow(Math.Cos(Math.PI * 180 * dN_in.gamma), 2);
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_6:
                    {
                        dN_out.dp = (dN_in.d2 + 2 * dN_in.cs) *
                            (Math.Pow(Math.Sin(Math.PI * 180 * dN_in.omega), 2) +
                            (dN_in.d1 + 2 * dN_in.cs) *
                            (dN_in.d1 + dN_in.d2 + 4 * dN_in.cs) /
                            (2 * Math.Pow(dN_in.d2 + 2 * dN_in.cs, 2)) *
                            Math.Pow(Math.Cos(Math.PI * 180 * dN_in.omega), 2));
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_7:
                    {
                        dN_out.dp = dN_in.D + 1.5 * (dN_in.r - dN_out.sp) + (2 * dN_in.cs);
                        break;
                    }
                default:
                    {
                        dN_out.err += "Ошибка места расположения штуцера\n";
                        break;
                    }
            }

            // l1p, l3p, l2p
            {
                double d;
                if (!dN_in.isOval)
                {
                    d = dN_in.D;
                }
                else
                {
                    d = dN_in.d2;
                }

                dN_out.l1p2 = 1.25 * Math.Sqrt((d + 2 * dN_in.cs) * (dN_in.s1 - dN_in.cs));
                dN_out.l1p = Math.Min(dN_in.l1, dN_out.l1p2);
                if (dN_in.s3 == 0)
                {
                    dN_out.l3p = 0;
                }
                else
                {
                    dN_out.l3p2 = 0.5 * Math.Sqrt((d + 2 * dN_in.cs) * (dN_in.s3 - dN_in.cs - dN_in.cs1));
                    dN_out.l3p = Math.Min(dN_in.l3, dN_out.l3p2);
                }
            }

            dN_out.L0 = Math.Sqrt(dN_out.Dp * (d_in.s - d_out.c));

            switch (dN_in.nozzleKind)
            {
                case NozzleKind.ImpassWithoutRing:
                case NozzleKind.PassWithoutRing:
                case NozzleKind.ImpassWithRing:
                case NozzleKind.PassWithRing:
                case NozzleKind.WithRingAndInPart:
                case NozzleKind.WithFlanging:
                    dN_out.lp = dN_out.L0;
                    break;
                case NozzleKind.WithTorusshapedInsert:
                case NozzleKind.WithWealdedRing:
                    dN_out.lp = Math.Min(dN_in.l, dN_out.L0);
                    break;
            }

            dN_out.l2p2 = Math.Sqrt(dN_out.Dp * (dN_in.s2 + d_in.s - d_out.c));
            dN_out.l2p = Math.Min(dN_in.l2, dN_out.l2p2);

            switch (dN_in.nozzleKind)
            {
                case NozzleKind.ImpassWithoutRing:
                case NozzleKind.PassWithoutRing:
                case NozzleKind.ImpassWithRing:
                case NozzleKind.PassWithRing:
                case NozzleKind.WithRingAndInPart:
                case NozzleKind.WithFlanging:
                    dN_in.s0 = d_in.s;
                    dN_in.steel4 = dN_in.steel1;
                    break;
            }

            //dN_in.sigma_d2 = GetSigma(dN_in.steel2, d_in.temp);
            //dN_in.sigma_d3 = GetSigma(dN_in.steel3, d_in.temp);
            //dN_in.sigma_d4 = GetSigma(dN_in.steel4, d_in.temp);

            dN_out.psi1 = Math.Min(1, dN_in.sigma_d1 / d_in.sigma_d);
            dN_out.psi2 = Math.Min(1, dN_in.sigma_d2 / d_in.sigma_d);
            dN_out.psi3 = Math.Min(1, dN_in.sigma_d3 / d_in.sigma_d);
            dN_out.psi4 = Math.Min(1, dN_in.sigma_d4 / d_in.sigma_d);

            dN_out.d0p = 0.4 * Math.Sqrt(dN_out.Dp * (d_in.s - d_out.c));

            dN_out.b = Math.Sqrt(dN_out.Dp * (d_in.s - d_out.c)) + Math.Sqrt(dN_out.Dp * (d_in.s - d_out.c));

            switch (d_in.shellType)
            {
                case ShellType.Cylindrical:
                    {
                        dN_out.dmax = d_in.D;
                        break;
                    }
                case ShellType.Conical:
                    {
                        dN_out.dmax = d_out.Dk;
                        break;
                    }
                case ShellType.Elliptical:
                case ShellType.Spherical:
                case ShellType.Torospherical:
                    {
                        dN_out.dmax = 0.6 * d_in.D;
                        break;
                    }
            }

            switch (d_in.shellType)
            {
                case ShellType.Cylindrical:
                case ShellType.Conical:
                    {
                        dN_out.K1 = 1;
                        break;
                    }
                case ShellType.Elliptical:
                case ShellType.Spherical:
                case ShellType.Torospherical:
                    {
                        dN_out.K1 = 2;
                        break;
                    }
            }

            if (d_in.isPressureIn)
            {
                dN_out.spn = dN_out.sp;
            }
            else
            {

                //dN_out.B1n = Math.Min(1, 9.45 * (d_in.D / d_out.l) * Math.Sqrt(d_in.D / (100 * (d_in.s - d_out.c))));
                //dN_out.pen = 2.08 * 0.00001 * d_in.E / (dN_in.ny * dN_out.B1n) * (d_in.D / d_out.l) * Math.Pow(100 * (d_in.s - d_out.c) / d_in.D, 2.5);
                dN_out.pen = d_out.p_de;
                dN_out.ppn = d_in.p / Math.Sqrt(1 - Math.Pow(d_in.p / dN_out.pen, 2));
                dN_out.spn = dN_out.ppn * dN_out.Dp / (2 * dN_out.K1 * d_in.sigma_d - dN_out.ppn);
            }


            dN_out.d01 = 2 * ((d_in.s - d_out.c) / dN_out.spn - 0.8) * Math.Sqrt(dN_out.Dp * (d_in.s - d_out.c));
            dN_out.d02 = dN_out.dmax + 2 * dN_in.cs;
            dN_out.d0 = Math.Min(dN_out.d01, dN_out.d02);

            if (dN_out.dp > dN_out.d0)
            {
                dN_out.yslyk1 = dN_out.l1p * (dN_in.s1 - dN_out.s1p - dN_in.cs) * dN_out.psi1 + dN_out.l2p * dN_in.s2 * dN_out.psi2 + dN_out.l3p * (dN_in.s3 - dN_in.cs - dN_in.cs1) * dN_out.psi3 + dN_out.lp * (d_in.s - d_out.s_calcr - d_out.c) * dN_out.psi4;
                dN_out.yslyk2 = 0.5 * (dN_out.dp - dN_out.d0p) * d_out.s_calcr;
                if (dN_out.yslyk1 < dN_out.yslyk2)
                {
                    dN_out.err += "Условие укрепления одиночного отверстия не выполняется";
                }
            }



            dN_out.V1 = (dN_in.s0 - d_out.c) / (d_in.s - d_out.c);
            dN_out.V2 = (dN_out.psi4 + (dN_out.l1p * (dN_in.s1 - dN_in.cs) * dN_out.psi1 + dN_out.l2p * dN_in.s2 * dN_out.psi2 + dN_out.l3p * (dN_in.s3 - dN_in.cs - dN_in.cs1) * dN_out.psi3) / dN_out.lp * (d_in.s - d_out.c)) / (1 + 0.5 * (dN_out.dp - dN_out.d0p) / dN_out.lp + dN_out.K1 * (dN_in.D + 2 * dN_in.cs) / dN_out.Dp * (dN_in.fi / dN_in.fi1) * (dN_out.l1p / dN_out.lp));
            dN_out.V = Math.Min(dN_out.V1, dN_out.V2);

            if (d_in.isPressureIn)
            {
                dN_out.p_d = 2 * dN_out.K1 * dN_in.fi * d_in.sigma_d * (d_in.s - d_out.c) * dN_out.V / (dN_out.Dp + (d_in.s - d_out.c) * dN_out.V);
            }
            else
            {
                dN_out.p_dp = 2 * dN_out.K1 * dN_in.fi * d_in.sigma_d * (d_in.s - d_out.c) * dN_out.V / (dN_out.Dp + (d_in.s - d_out.c) * dN_out.V);
                dN_out.p_de = d_out.p_de;
                dN_out.p_d = dN_out.p_dp / Math.Sqrt(1 + Math.Pow(dN_out.p_dp / dN_out.p_de, 2));
            }
            if (dN_out.p_d < d_in.p)
            {
                dN_out.err += "Допускаемое давление меньше расчетного\n";
            }

            switch (d_in.shellType)
            {
                case ShellType.Cylindrical:
                    {
                        dN_out.ypf1 = (dN_out.dp - 2 * dN_in.cs) / d_in.D;
                        dN_out.ypf2 = (d_in.s - d_out.c) / d_in.D;
                        if (dN_out.ypf1 <= 1 & dN_out.ypf2 <= 0.1)
                        {
                            dN_out.ypf = true;
                        }
                        else
                        {
                            dN_out.ypf = false;
                        }
                        break;
                    }
                case ShellType.Conical:
                    {
                        dN_out.ypf1 = (dN_out.dp - 2 * dN_in.cs) / d_out.Dk;
                        dN_out.ypf2 = (d_in.s - d_out.c) / d_out.Dk;
                        if (dN_out.ypf1 <= 1 & dN_out.ypf2 <= 0.1 / Math.Cos(Math.PI * 180 * d_in.alfa))
                        {
                            dN_out.ypf = true;
                        }
                        else
                        {
                            dN_out.ypf = false;
                        }
                        break;
                    }
                case ShellType.Elliptical:
                case ShellType.Spherical:
                case ShellType.Torospherical:
                    {
                        dN_out.ypf1 = (dN_out.dp - 2 * dN_in.cs) / d_in.D;
                        dN_out.ypf2 = (d_in.s - d_out.c) / d_in.D;
                        if (dN_out.ypf1 <= 0.6 & dN_out.ypf2 <= 0.1)
                        {
                            dN_out.ypf = true;
                        }
                        else
                        {
                            dN_out.ypf = false;
                        }
                        break;
                    }
            }
            if (dN_out.ypf == false)
            {
                dN_out.err += "Условие применения формул не выполняется";
            }

            return dN_out;
        }

        public void MakeWord(string filename)
        {
            var doc = Xceed.Words.NET.DocX.Load(filename);
            doc.InsertParagraph().InsertPageBreakAfterSelf();
            doc.InsertParagraph($"Расчет на прочность узла врезки штуцера {dN_in.name} в ").Heading(HeadingType.Heading1).Alignment = Alignment.center;
            switch (d_in.shellType)
            {
                case ShellType.Cylindrical:
                    doc.Paragraphs.Last().Append($"обечайку {d_in.Name}, нагруженную ");
                    break;
                case ShellType.Conical:
                    doc.Paragraphs.Last().Append($"коническую обечайку {d_in.Name}, нагруженную ");
                    break;
                case ShellType.Elliptical:
                    doc.Paragraphs.Last().Append($"эллиптическое днище {d_in.Name}, нагруженное ");
                    break;
            }
            if (d_in.isPressureIn)
            {
                doc.Paragraphs.Last().Append("внутренним избыточным давлением");
            }
            else if (!d_in.isPressureIn)
            {
                doc.Paragraphs.Last().Append("наружным давлением");
            }
            doc.InsertParagraph();
            doc.InsertParagraph("Исходные данные").Alignment = Alignment.center;

            //table
            {
                var table = doc.AddTable(1, 2);
                table.SetWidths(new float[] { 200, 200 });

                table.Rows[0].Cells[0].Paragraphs[0].Append("Элемент:");
                table.Rows[0].Cells[1].Paragraphs[0].Append($"Штуцер {dN_in.name}");

                table.InsertRow();
                table.Rows[1].Cells[0].Paragraphs[0].Append("Элемент несущий штуцер:");
                table.Rows[1].Cells[1].Paragraphs[0].Append($"{d_in.Name}");

                table.InsertRow();
                table.Rows[2].Cells[0].Paragraphs[0].Append("Тип элемента, несущего штуцер:");
                switch (d_in.shellType)
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
                switch (dN_in.nozzleKind)
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

            var image = doc.AddImage($"pic/Nozzle/Nozzle{dN_in.nozzleKind}.gif");
            var picture = image.CreatePicture();
            doc.InsertParagraph().AppendPicture(picture);

            //table
            {
                var table = doc.AddTable(1, 2);
                table.SetWidths(new float[] { 300, 100 });

                int i = 0;
                table.Rows[i].Cells[0].Paragraphs[0].Append("Материал несущего элемента:");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.Steel}");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Толщина стенки несущего элемента, s:");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.s} мм");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Сумма прибавок к стенке несущего элемента, c:");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_out.c:f2} мм");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Материал штуцера");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.steel1}");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Внутренний диаметр штуцера, d:");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.D} мм");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Толщина стенки штуцера, ")
                                                    .AppendEquation("s_1")
                                                    .Append(":");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.s1} мм");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Длина наружной части штуцера, ")
                                                    .AppendEquation("s_1")
                                                    .Append(":");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.l1} мм");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Сумма прибавок к толщине стенки штуцера, ")
                                                    .AppendEquation("c_s")
                                                    .Append(":");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.cs} мм");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append("Прибавка на коррозию, ")
                                                    .AppendEquation("c_s1")
                                                    .Append(":");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.cs1} мм");

                switch (dN_in.nozzleKind)
                {
                    case NozzleKind.ImpassWithoutRing:
                        {
                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Минимальный размер сварного шва, Δ:");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.delta} мм");
                            break;
                        }
                    case NozzleKind.PassWithoutRing:
                        {
                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Длина внутренней части штуцера, ")
                                                                .AppendEquation("l_3")
                                                                .Append(":");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.l3} мм");

                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Толщина внутренней части штуцера, ")
                                                                .AppendEquation("s_3")
                                                                .Append(":");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.s3} мм");

                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Минимальный размер сварного шва, Δ:");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.delta} мм");
                            break;
                        }
                    case NozzleKind.ImpassWithRing:
                        {
                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Ширина накладного кольца, ")
                                                                .AppendEquation("l_2")
                                                                .Append(":");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.l2} мм");

                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Толщина накладного кольца, ")
                                                                .AppendEquation("s_2")
                                                                .Append(":");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.s2} мм");

                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Минимальный размер сварного шва, Δ:");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.delta} мм");
                            break;
                        }
                    case NozzleKind.PassWithRing:
                        {
                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Ширина накладного кольца, ")
                                                                .AppendEquation("l_2")
                                                                .Append(":");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.l2} мм");

                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Толщина накладного кольца, ")
                                                                .AppendEquation("s_2")
                                                                .Append(":");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.s2} мм");

                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Длина внутренней части штуцера, ")
                                                                .AppendEquation("l_3")
                                                                .Append(":");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.l3} мм");

                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Толщина внутренней части штуцера, ")
                                                                .AppendEquation("s_3")
                                                                .Append(":");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.s3} мм");

                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Минимальный размер сварного шва, Δ:");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.delta} мм");
                            break;
                        }
                    case NozzleKind.WithRingAndInPart:
                        {
                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Ширина накладного кольца, ")
                                                                .AppendEquation("l_2")
                                                                .Append(":");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.l2} мм");

                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Толщина накладного кольца, ")
                                                                .AppendEquation("s_2")
                                                                .Append(":");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.s2} мм");

                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Длина внутренней части штуцера, ")
                                                                .AppendEquation("l_3")
                                                                .Append(":");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.l3} мм");

                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Толщина внутренней части штуцера, ")
                                                                .AppendEquation("s_3")
                                                                .Append(":");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.s3} мм");

                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Минимальный размер сварного шва, Δ:");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.delta} мм");
                            break;
                        }
                    case NozzleKind.WithFlanging:
                        {
                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Радиус отбортовки, r:");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.r} мм");

                            table.InsertRow(++i);
                            table.Rows[i].Cells[0].Paragraphs[0].Append("Минимальный размер сварного шва, Δ:");
                            table.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.delta} мм");
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
            doc.InsertParagraph("Продольный шов штуцера ").AppendEquation($"φ_1={dN_in.fi1}");
            doc.InsertParagraph("Шов обечайки в зоне врезки штуцера ").AppendEquation($"φ={dN_in.fi}");

            doc.InsertParagraph();
            doc.InsertParagraph("Условия нагружения").Alignment = Alignment.center;

            //table
            {
                var table = doc.AddTable(1, 2);
                table.SetWidths(new float[] { 300, 100 });

                int i = 0;
                table.Rows[i].Cells[0].Paragraphs[0].Append("Расчетная температура, Т:");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.temp} °С");

                table.InsertRow(++i);
                if (d_in.isPressureIn)
                {
                    table.Rows[i].Cells[0].Paragraphs[0].Append("Расчетное внутреннее избыточное давление, p:");
                }
                else
                {
                    table.Rows[i].Cells[0].Paragraphs[0].Append("Расчетное наружное давление, p:");
                }
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.p} МПа");

                table.InsertRow(++i);
                table.Rows[i].Cells[0].Paragraphs[0].Append($"Допускаемое напряжение для материала {dN_in.steel1} при расчетной температуре, ")
                                                    .AppendEquation("[σ]_1")
                                                    .Append(":");
                table.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.sigma_d1} МПа");

                if (!d_in.isPressureIn)
                {
                    table.InsertRow(++i);
                    table.Rows[i].Cells[0].Paragraphs[0].Append("Модуль продольной упругости при расчетной температуре, ")
                                                        .AppendEquation("E_1")
                                                        .Append(":");
                    table.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.E1} МПа");
                }
                if (dN_in.steel1 != dN_in.steel2)
                {
                    table.InsertRow(++i);
                    table.Rows[i].Cells[0].Paragraphs[0].Append($"Допускаемое напряжение для материала {dN_in.steel2} при расчетной температуре, ")
                                                        .AppendEquation("[σ]_2")
                                                        .Append(":");
                    table.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.sigma_d2} МПа");

                    if (!d_in.isPressureIn)
                    {
                        table.InsertRow(++i);
                        table.Rows[i].Cells[0].Paragraphs[0].Append("Модуль продольной упругости при расчетной температуре, ")
                                                            .AppendEquation("E_2")
                                                            .Append(":");
                        table.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.E2} МПа");
                    }
                }
                if (dN_in.steel1 != dN_in.steel3)
                {
                    table.InsertRow(++i);
                    table.Rows[i].Cells[0].Paragraphs[0].Append($"Допускаемое напряжение для материала {dN_in.steel3} при расчетной температуре, ")
                                                        .AppendEquation("[σ]_3")
                                                        .Append(":");
                    table.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.sigma_d3} МПа");

                    if (!d_in.isPressureIn)
                    {
                        table.InsertRow(++i);
                        table.Rows[i].Cells[0].Paragraphs[0].Append("Модуль продольной упругости при расчетной температуре, ")
                                                            .AppendEquation("E_3")
                                                            .Append(":");
                        table.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.E3} МПа");
                    }
                }
                if (dN_in.steel1 != dN_in.steel4)
                {
                    table.InsertRow(++i);
                    table.Rows[i].Cells[0].Paragraphs[0].Append($"Допускаемое напряжение для материала {dN_in.steel4} при расчетной температуре, ")
                                                        .AppendEquation("[σ]_4")
                                                        .Append(":");
                    table.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.sigma_d4} МПа");

                    if (!d_in.isPressureIn)
                    {
                        table.InsertRow(++i);
                        table.Rows[i].Cells[0].Paragraphs[0].Append("Модуль продольной упругости при расчетной температуре, ")
                                                            .AppendEquation("E_4")
                                                            .Append(":");
                        table.Rows[i].Cells[1].Paragraphs[0].Append($"{dN_in.E4} МПа");
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

            switch (d_in.shellType)
            {
                case ShellType.Cylindrical:
                    {
                        doc.Paragraphs.Last().Append("(для цилиндрической обечайки)");
                        doc.InsertParagraph().AppendEquation($"D_p=D={d_in.D} мм");
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
                        if (d_in.elH * 100 == d_in.D * 25)
                        {
                            doc.InsertParagraph("(для эллиптического днища при H=0.25D)");
                            doc.InsertParagraph().AppendEquation("D_p=2∙D∙√(1-3∙(x/D)^2)");
                            doc.InsertParagraph().AppendEquation($"D_p=2∙{d_in.D}∙√(1-3∙({dN_in.elx}/{d_in.D})^2)={dN_out.Dp:f2} мм");
                        }
                        else
                        {
                            doc.InsertParagraph("(для эллиптического днища)");
                            doc.InsertParagraph().AppendEquation("D_p=D^2/(2∙H)∙√(1-(D^2-4∙H^2)/D^4∙x^2)");
                            doc.InsertParagraph().AppendEquation($"D_p={d_in.D}^2/(2∙{d_in.elH})∙√(1-({d_in.D}^2-4∙{d_in.elH}^2)/{d_in.D}^4∙{dN_in.elx}^2)={dN_out.Dp:f2} мм");
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

            switch (dN_in.location)
            {
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_1:
                    {
                        doc.InsertParagraph("Расчетный диаметр отверстия в стенке цилиндрической обечайки, конического перехода или выпуклого днища при наличии штуцера с круглым поперечным сечением, ось которого совпадает с нормалью к поверхности в центре отверстия");
                        doc.InsertParagraph().AppendEquation("d_p=d+2∙c_s");
                        doc.InsertParagraph().AppendEquation($"d_p={dN_in.D}+2∙{dN_in.cs}={dN_out.dp:f2} мм");
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_2:
                    doc.InsertParagraph("Расчетный диаметр отверстия в стенке цилиндрической обечайки или конической обечайки при наличии наклонного штуцера, ось которого лежит в плоскости поперечного сечения укрепляемой обечайки");
                    doc.InsertParagraph().AppendEquation("d_p=max{d;0.5∙t}+2∙c_s");
                    //doc.InsertParagraph().AppendEquation($"d_p={dN_in.D}+2∙{dN_in.cs}={dN_out.dp:f2} мм");
                    break;
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_3:
                    {
                        doc.InsertParagraph("Расчетный диаметр отверстия в стенке эллиптического днища при наличии смещенного штуцера, ось которого параллельна оси днища");
                        doc.InsertParagraph().AppendEquation("d_p=(d+2∙c_s)/√(1-((2∙x)/D_p)^2)");
                        //doc.InsertParagraph().AppendEquation($"d_p={dN_in.D}+2∙{dN_in.cs}={dN_out.dp:f2} мм");
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_4:
                    {
                        doc.InsertParagraph("Расчетный диаметр отверстия при наличии наклонного штуцера с круглым поперечным сечением, когда максимальная ось симметрии отверстия некруглой формы составляет угол ω с образующей цилиндрической обечайки или с проекцией образующей конической обечайки на плоскость продольного сечения обечайки");
                        doc.InsertParagraph().AppendEquation("d_p=(d+2∙c_s)(1+tg^2 γ∙cos^2 ω)");
                        //doc.InsertParagraph().AppendEquation($"d_p={dN_in.D}+2∙{dN_in.cs}={dN_out.dp:f2} мм");
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_5:
                    doc.InsertParagraph("Расчетный диаметр отверстия для цилиндрической и конической обечаек, когда ось наклонного штуцера лежит в плоскости продольного сечения обечайки, а также для всех отверстий в сферическом и торосферическом днищах при наличии смещенного штуцера");
                    doc.InsertParagraph().AppendEquation("d_p=(d+2∙c_s)/(cos^2 γ)");
                    //doc.InsertParagraph().AppendEquation($"d_p={dN_in.D}+2∙{dN_in.cs}={dN_out.dp:f2} мм");
                    break;
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_6:
                    {
                        doc.InsertParagraph("Расчетный диаметр овального отверстия для перпендикулярно расположенного к поверхности обечайки штуцера с овальным поперечным сечением");
                        doc.InsertParagraph().AppendEquation("d_p=(d+2∙c_s)[sin^2 ω +((d_1+2∙c_s)(d_1+d_2+4∙c_s))/(2(d_2+2∙c_s)^2)cos^2 ω");
                        //doc.InsertParagraph().AppendEquation($"d_p={dN_in.D}+2∙{dN_in.cs}={dN_out.dp:f2} мм");
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_7:
                    {
                        doc.InsertParagraph("Расчетный диаметр отверстия для перпендикулярно расположенного к поверхности обечайки или днища штуцера с круглым поперечным сечением при наличии отбортовки или торообразной вставки");
                        doc.InsertParagraph().AppendEquation("d_p=в+1.5(r-s_p)+2∙c_s");
                        //doc.InsertParagraph().AppendEquation($"d_p={dN_in.D}+2∙{dN_in.cs}={dN_out.dp:f2} мм");
                        break;
                    }
            }

            doc.InsertParagraph("Расчетная толщина стенки укрепляемого элемента");
            if (d_in.shellType == ShellType.Elliptical && d_in.isPressureIn)
            {
                doc.InsertParagraph().AppendEquation("s_p=(p∙D_p)/(4∙φ∙[σ]-p)");
                doc.InsertParagraph().AppendEquation($"s_p=({d_in.p}∙{dN_out.Dp:f2})/(4∙{d_in.fi}∙{d_in.sigma_d}-{d_in.p})={dN_out.sp:f2} мм");
            }
            else
            {
                doc.Paragraphs.Last().Append(" определяется в соответствии с ГОСТ 34233.2");
                doc.InsertParagraph().AppendEquation($"s_p={dN_out.sp:f2} мм");
            }
            doc.InsertParagraph("Расчетная толщина стенки штуцера с круглым поперечным сечением");
            doc.InsertParagraph().AppendEquation("s_1p=(p(d+2∙c_s))/(2∙φ_1∙[σ]_1-p)");
            doc.InsertParagraph().AppendEquation($"s_1p=({d_in.p}({dN_in.D}+2∙{dN_in.cs}))/(2∙{dN_in.fi1}∙{dN_in.sigma_d1}-{d_in.p})={dN_out.s1p:f2} мм");

            doc.InsertParagraph("Расчетная длина внешней части штуцера");
            doc.InsertParagraph().AppendEquation("l_1p=min{l_1;1.25√((d+2∙c_s)(s_1-c_s))}");
            doc.InsertParagraph().AppendEquation($"1.25√((d+2∙c_s)(s_1-c_s))=1.25√(({dN_in.D}+2∙{dN_in.cs})({dN_in.s1}-{dN_in.cs}))={dN_out.l1p2:f2} мм");
            doc.InsertParagraph().AppendEquation($"l_1p=min({dN_in.l1};{dN_out.l1p2:f2})={dN_out.l1p:f2} мм");

            if (dN_in.l3 > 0)
            {
                doc.InsertParagraph("Расчетная длина внутренней части штуцера");
                doc.InsertParagraph().AppendEquation("l_3p=min{l_3;0.5√((d+2∙c_s)(s_3-c_s-c_s1))}");
                doc.InsertParagraph().AppendEquation($"0.5√((d+2∙c_s)(s_3-c_s-c_s1))=0.5√(({dN_in.D}+2∙{dN_in.cs})({dN_in.s3}-{dN_in.cs}-{dN_in.cs1}))={dN_out.l3p2:f2} мм");
                doc.InsertParagraph().AppendEquation($"l_3p=min({dN_in.l3};{dN_out.l3p2:f2})={dN_out.l3p:f2} мм");
            }

            doc.InsertParagraph("Ширина зоны укрепления отверстия в цилиндрической обечайке");
            doc.InsertParagraph().AppendEquation("L_0=√(D_p∙(s-c))");
            doc.InsertParagraph().AppendEquation($"L_0=√({dN_out.Dp}∙({d_in.s}-{d_out.c:f2}))={dN_out.L0:f2}");

            doc.InsertParagraph("Расчетная ширина зоны укрепления отверстия в стенке цилиндрической обечайки");

            switch (dN_in.nozzleKind)
            {
                case NozzleKind.ImpassWithoutRing:
                case NozzleKind.PassWithoutRing:
                case NozzleKind.ImpassWithRing:
                case NozzleKind.PassWithRing:
                case NozzleKind.WithRingAndInPart:
                case NozzleKind.WithFlanging:
                    doc.InsertParagraph().AppendEquation($"l_p=L_0={dN_out.lp:f2} мм");
                    break;
                case NozzleKind.WithTorusshapedInsert:
                case NozzleKind.WithWealdedRing:
                    doc.InsertParagraph().AppendEquation("l_p=min{l;L_0}");
                    doc.InsertParagraph().AppendEquation($"l_p=min({dN_in.l};{dN_out.L0:f2})={dN_out.lp:f2} мм");
                    break;
            }

            if (dN_in.l2 > 0)
            {
                doc.InsertParagraph("Расчетная ширина накладного кольца");
                doc.InsertParagraph().AppendEquation("l_2p=min{l_2;√(D_p∙(s_2+s-c))}");
                doc.InsertParagraph().AppendEquation($"√(D_p∙(s_2+s-c))=√({dN_out.Dp}∙({dN_in.s2}+{d_in.s}-{d_out.c:f2}))={dN_out.l2p2:f2} мм");
                doc.InsertParagraph().AppendEquation($"l_2p=min({dN_in.l2};{dN_out.l2p2:f2})={dN_out.l2p:f2} мм");
            }

            if (dN_out.psi1 != 1 | dN_out.psi2 != 1 | dN_out.psi3 != 1 | dN_out.psi4 != 1)
            {
                doc.InsertParagraph("Учет применения различного материального исполнения");
            }
            if (d_in.Steel != dN_in.steel1)
            {
                doc.InsertParagraph("- для внешней части штуцера").AppendEquation($"χ_1=min(1;[σ]_1/[σ])=min(1;{dN_in.sigma_d1}/{d_in.sigma_d})={dN_out.psi1:f2}");
            }
            if (d_in.Steel != dN_in.steel2)
            {
                doc.InsertParagraph("- для накладного кольца").AppendEquation($"χ_2=min(1;[σ]_2/[σ])=min(1;{dN_in.sigma_d2}/{d_in.sigma_d})={dN_out.psi2:f2}");
            }
            if (d_in.Steel != dN_in.steel3)
            {
                doc.InsertParagraph("- для внутренней части штуцера").AppendEquation($"χ_3=min(1;[σ]_3/[σ])=min(1;{dN_in.sigma_d3}/{d_in.sigma_d})={dN_out.psi3:f2}");
            }
            if (d_in.Steel != dN_in.steel4)
            {
                doc.InsertParagraph("- для торообразной вставки или вварного кольца").AppendEquation($"χ_4=min(1;[σ]_4/[σ])=min(1;{dN_in.sigma_d4}/{d_in.sigma_d})={dN_out.psi4:f2}");
            }

            doc.InsertParagraph("Расчетный диаметр отверстия, не требующий укрепления в стенке цилиндрической обечайки при отсутствии избыточной толщины стенки сосуда и при наличии штуцера");
            doc.InsertParagraph().AppendEquation("d_0p=0,4√(D_p∙(s-c))");
            doc.InsertParagraph().AppendEquation($"d_0p=0.4√({dN_out.Dp}∙({d_in.s}-{d_out.c:f2}))={dN_out.d0p:f2} мм");


            doc.InsertParagraph("Проверка условия необходимости проведения расчета укрепления отверстий");
            doc.InsertParagraph().AppendEquation("d_p≤d_0");

            doc.InsertParagraph().AppendEquation("d_0").Append(" - наибольший допустимый диаметр одиночного отверстия, не требующего дополнительного укрепления при наличии избыточной толщины стенки сосуда");
            doc.InsertParagraph().AppendEquation("d_0=min{2∙((s-c)/s_pn-0.8)∙√(D_p∙(s-c));d_max+2∙c_s} ");
            doc.InsertParagraph("где - ").AppendEquation("d_max").Append(" - максимальный диаметр отверстия ");

            switch (d_in.shellType)
            {
                case ShellType.Cylindrical:
                    {
                        doc.InsertParagraph().AppendEquation($"d_max=D={d_in.D} мм").AppendLine(" - для отверстий в цилиндрических обечайках");
                        break;
                    }
                case ShellType.Conical:
                    {
                        doc.InsertParagraph().AppendEquation($"d_max=D_K={dN_out.dmax:f2} мм").AppendLine(" - для отверстий в конических обечайках");
                        break;
                    }
                case ShellType.Elliptical:
                case ShellType.Spherical:
                case ShellType.Torospherical:
                    {
                        doc.InsertParagraph().AppendEquation($"d_max=0.6∙D={dN_out.dmax:f2} мм").AppendLine(" - для отверстий в выпуклых днищах");
                        break;
                    }
            }


            if (d_in.isPressureIn)
            {
                doc.InsertParagraph().AppendEquation($"s_pn=s_p={dN_out.sp:f2} мм").AppendEquation(" - в случае внутреннего давления");
            }
            else if (!d_in.isPressureIn)
            {
                doc.InsertParagraph().AppendEquation("s_pn=(p_pn∙D_p)/(2∙K_1∙[σ]-p_pn)");
                switch (d_in.shellType)
                {
                    case ShellType.Cylindrical:
                    case ShellType.Conical:
                        {
                            doc.InsertParagraph().AppendEquation($"K_1={dN_out.K1}").Append(" - для цилиндрических и конических обечаек");
                            break;
                        }
                    case ShellType.Elliptical:
                    case ShellType.Spherical:
                    case ShellType.Torospherical:
                        {
                            doc.InsertParagraph().AppendEquation($"K_1={dN_out.K1}").Append(" - для отверстий в выпуклых днищах");
                            break;
                        }
                }
                doc.InsertParagraph().AppendEquation("p_pn=p/√(1-(p/[p]_E)^2)");
                doc.InsertParagraph().AppendEquation("[p]_E").Append(" -  допускаемое наружное давление из условия устойчивости в пределах упругости, определяемое по ГОСТ 34233.2 для обечайки без отверстий");
                doc.InsertParagraph().AppendEquation($"p_pn={d_in.p}/√(1-({d_in.p}/{dN_out.pen:f2})^2)={dN_out.ppn:f2} МПа");
                doc.InsertParagraph().AppendEquation($"s_pn=({dN_out.ppn:f2}∙{dN_out.Dp:f2})/(2∙{dN_out.K1}∙{d_in.sigma_d}-{dN_out.ppn:f2})={dN_out.spn:f2} мм");
            }

            doc.InsertParagraph().AppendEquation($"2∙((s-c)/s_pn-0.8)∙√(D_p∙(s-c))=2∙(({d_in.s}-{d_out.c:f2})/{dN_out.spn:f2}-0.8)∙√({dN_out.Dp:f2}∙({d_in.s}-{d_out.c:f2}))={dN_out.d01:f2}");
            doc.InsertParagraph().AppendEquation($"d_max+2∙c_s={dN_out.dmax:f2}+2∙{dN_in.cs}={dN_out.d02:f2}");
            doc.InsertParagraph().AppendEquation($"d_0=min({dN_out.d01:f2};{dN_out.d02:f2})={dN_out.d0:f2} мм");

            doc.InsertParagraph().AppendEquation($"{dN_out.dp:f2}≤{dN_out.d0:f2}");
            if (dN_out.dp <= dN_out.d0)
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
                doc.InsertParagraph().AppendEquation($"{dN_out.l1p:f2}∙({dN_in.s1}-{dN_out.s1p:f2}-{dN_in.cs})∙{dN_out.psi1}+{dN_out.l2p:f2}∙{dN_in.s2}∙{dN_out.psi2}+{dN_out.l3p:f2}∙({dN_in.s3}-{dN_in.cs}-{dN_in.cs1})∙{dN_out.psi3:f2}+{dN_out.lp:f2}∙({d_in.s}-{dN_out.sp:f2}-{d_out.c:f2})∙{dN_out.psi4}={dN_out.yslyk1:f2}");
                doc.InsertParagraph().AppendEquation($"0.5∙(d_p-d_0p)∙s_p=0.5∙({dN_out.dp:f2}-{dN_out.d0p:f2})∙{dN_out.sp:f2}={dN_out.yslyk2:f2}");
                doc.InsertParagraph().AppendEquation($"{dN_out.yslyk1:f2}≥{dN_out.yslyk2:f2}");
                if (dN_out.yslyk1 >= dN_out.yslyk2)
                {
                    doc.InsertParagraph("Условие прочности выполняется").Bold();
                }
                else
                {
                    doc.InsertParagraph("Условие прочности не выполняется").Bold().Color(System.Drawing.Color.Red);
                }
            }

            doc.InsertParagraph();


            if (d_in.isPressureIn)
            {
                doc.InsertParagraph("Допускаемое внутреннее избыточное давление элемента сосуда с учетом ослабления стенки отверстием вычисляют по формуле");
                doc.InsertParagraph().AppendEquation("[p]=(2∙K_1∙φ∙[σ]∙(s-c)∙V)/(D_p+(s-c)∙V)");
            }
            else if (!d_in.isPressureIn)
            {
                doc.InsertParagraph("Допускаемое наружное давление элемента сосуда с учетом ослабления стенки отверстием вычисляют по формуле");
                doc.InsertParagraph().AppendEquation("[p]=[p]_П/√(1+([p]_П/[p]_E)^2)");
                doc.InsertParagraph("где ").AppendEquation("[p]_П").Append(" - допускаемое наружное давление в пределах пластичности определяется как допускаемое внутреннее избыточное давление для сосуда или аппарата с отверстием при φ=1");
                doc.InsertParagraph().AppendEquation("[p]_П=(2∙K_1∙φ∙[σ]∙(s-c)∙V)/(D_p+(s-c)∙V)");
            }

            switch (d_in.shellType)
            {
                case ShellType.Cylindrical:
                case ShellType.Conical:
                    {
                        doc.InsertParagraph().AppendEquation($"K_1={dN_out.K1}").Append(" - для цилиндрических и конических обечаек");
                        break;
                    }
                case ShellType.Elliptical:
                case ShellType.Spherical:
                case ShellType.Torospherical:
                    {
                        doc.InsertParagraph().AppendEquation($"K_1={dN_out.K1}").Append(" - для отверстий в выпуклых днищах");
                        break;
                    }
            }

            doc.InsertParagraph("Коэффициент снижения прочности сосуда, ослабленного одиночным отверстием, вычисляют по формуле");
            doc.InsertParagraph().AppendEquation("V=min{(s_0-c)/(s-c);(χ_4+(l_1p∙(s_1-c_s)∙χ_1+l_2p∙s_2∙χ_2+l_3p∙(s_3-c_s-c_s1)∙χ_3)/(l_p∙(s-c)))/(1+0.5∙(d_p-d_0p)/l_p+K_1∙(d+2∙c_s)/D_p∙(φ/φ_1)∙(l_1p/l_p))}").Alignment = Alignment.center;

            switch (dN_in.nozzleKind)
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

            doc.InsertParagraph().AppendEquation($"(s_0-c)/(s-c)=({dN_in.s0}-{d_out.c:f2})/({d_in.s}-{d_out.c:f2})={dN_out.V1:f2}");
            doc.InsertParagraph().AppendEquation("(χ_4+(l_1p∙(s_1-c_s)∙χ_1+l_2p∙s_2∙χ_2+l_3p∙(s_3-c_s-c_s1)∙χ_3)/(l_p∙(s-c)))/(1+0.5∙(d_p-d_0p)/l_p+K_1∙(d+2∙c_s)/D_p∙(φ/φ_1)∙(l_1p/l_p))=");
            doc.InsertParagraph().AppendEquation($"({dN_out.psi4}+({dN_out.l1p:f2}∙({dN_in.s1}-{dN_in.cs})∙{dN_out.psi1}+{dN_out.l2p:f2}∙{dN_in.s2}∙{dN_out.psi2}+{dN_out.l3p:f2}∙({dN_in.s3}-{dN_in.cs}-{dN_in.cs1})∙{dN_out.psi3:f2})/({dN_out.lp:f2}∙({d_in.s}-{d_out.c:f2})))/(1+0.5∙({dN_out.dp:f2}-{dN_out.d0p:f2})/{dN_out.lp:f2}+{dN_out.K1}∙({dN_in.D}+2∙{dN_in.cs})/{dN_out.Dp}∙({dN_in.fi}/{dN_in.fi1})∙({dN_out.l1p:f2}/{dN_out.lp:f2}))={dN_out.V2:f2}");

            doc.InsertParagraph().AppendEquation($"V=min({dN_out.V1:f2};{dN_out.V2:f2})={dN_out.V:f2} ");

            if (d_in.isPressureIn)
            {
                doc.InsertParagraph().AppendEquation($"[p]=(2∙{dN_out.K1}∙{dN_in.fi}∙{d_in.sigma_d}∙({d_in.s}-{d_out.c:f2})∙{dN_out.V:f2})/({dN_out.Dp}+({d_in.s}-{d_out.c:f2})∙{dN_out.V:f2})={dN_out.p_d:f2} МПа");
            }
            else if (!d_in.isPressureIn)
            {
                doc.InsertParagraph().AppendEquation($"[p]_p=(2∙{dN_out.K1}∙{dN_in.fi}∙{d_in.sigma_d}∙({d_in.s}-{d_out.c:f2})∙{dN_out.V:f2})/({dN_out.Dp}+({d_in.s}-{d_out.c:f2})∙{dN_out.V:f2})={dN_out.p_dp:f2} МПа");
                doc.InsertParagraph().AppendEquation("[p]_E").Append(" - допускаемое наружное давление из условия устойчивости в пределах упругости, определяемое по ГОСТ 34233.2 для соответствующих обечайки и днища без отверстий");
                doc.InsertParagraph().AppendEquation($"[p]_E={dN_out.p_de:f2} МПа)");
                doc.InsertParagraph().AppendEquation($"[p]={dN_out.p_dp:f2}/√(1+({dN_out.p_dp:f2}/{dN_out.p_de:f2})^2)={dN_out.p_d:f2} МПа");
            }
            doc.InsertParagraph().AppendEquation("[p]≥p");
            doc.InsertParagraph().AppendEquation($"{dN_out.p_d:f2} МПа >= {d_in.p} МПа");
            if (dN_out.p_d >= d_in.p)
            {
                doc.InsertParagraph("Условие прочности выполняется").Bold();
            }
            else
            {
                doc.InsertParagraph("Условие прочности не выполняется").Bold().Color(System.Drawing.Color.Red);
            }

            doc.InsertParagraph("Границы применения формул");
            switch (d_in.shellType)
            {
                case ShellType.Cylindrical:
                    {
                        doc.InsertParagraph().AppendEquation("(d_p-2∙c_s)/D≤1");
                        doc.InsertParagraph().AppendEquation($"({dN_out.dp:f2}-2∙{dN_in.cs})/{d_in.D}={dN_out.ypf1:f2}≤1");
                        doc.InsertParagraph().AppendEquation("(s-c)/D≤0.1");
                        doc.InsertParagraph().AppendEquation($"({d_in.s}-{d_out.c:f2})/({d_in.D})={dN_out.ypf2:f2}≤0.1");
                        break;
                    }
                case ShellType.Conical:
                    {
                        doc.InsertParagraph().AppendEquation("(d_p-2∙c_s)/D_K≤1");
                        //doc.InsertParagraph().AppendEquation($"({dN_out.dp:f2}-2∙{dN_in.cs})/{d_in.DK}={dN_out.ypf1:f2}≤1");
                        doc.InsertParagraph().AppendEquation("(s-c)/D_K≤0.1/cosα");
                        //doc.InsertParagraph().AppendEquation($"({d_in.s}-{d_out.c:f2})/({d_in.DK})={dN_out.ypf2:f2}≤0.1/cos{d_in.alfa}");
                        break;
                    }
                case ShellType.Elliptical:
                case ShellType.Spherical:
                case ShellType.Torospherical:
                    {
                        doc.InsertParagraph().AppendEquation("(d_p-2∙c_s)/D≤0.6");
                        doc.InsertParagraph().AppendEquation($"({dN_out.dp:f2}-2∙{dN_in.cs})/{d_in.D}={dN_out.ypf1:f2}≤0.6");
                        doc.InsertParagraph().AppendEquation("(s-c)/D≤0.1");
                        doc.InsertParagraph().AppendEquation($"({d_in.s}-{d_out.c:f2})/({d_in.D})={dN_out.ypf2:f2}≤0.1");
                        break;
                    }
            }
            doc.Save();
        }
    }
}
