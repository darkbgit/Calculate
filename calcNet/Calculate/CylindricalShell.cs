using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calcNet
{
    public interface IElement
    {
        //void CheckInputData();

        void Calculate();

        void MakeWord(string filename);
    }

    interface IDataIn
    {
        public bool IsDataGood { get;  }

        public string Error { get; }
    }

    interface ICheckedElement
    {
        void Calculate();
    }

    interface ICalculatedElement
    {
        void MakeWord();
    }


    public abstract class Shell 
    {
        public Shell(ShellType shellType)
        {
            this.shellType = shellType;
        }

        private readonly ShellType shellType;
        public ShellType ShellType { get => shellType; }
    }

    class ShellDataIn 
    {
        //public ShellDataIn()
        //{

        //}
        private bool isError;

        private List<string> errorList;

        private string name;
        private string steel;
        private ShellType shellType;


        public string Name { get => name; set => name = value; }
        public string Steel { get => steel; set => steel = value; }
        public ShellType ShellType { get => shellType; set => shellType = value; }
        public double c1
        {
            get => _c1; 
            set
            {
                if (value > 0)
                {
                    _c1 = value;
                }
                else
                {
                    isError = true;
                    errorList.Add("c1 должно быть больше 0");
                }
            }
        }
        public double D
        {
            get => _D;
            set
            {
                if (value > 0)
                {
                    _D = value;
                }
                else
                {
                    isError = true;
                    errorList.Add("D должен быть больше 0");
                }
            }
        }
        public double c2
        {
            get => _c2;
            set
            {
                if (value >= 0)
                {
                    _c2 = value;
                }
                else
                {
                    isError = true;
                    errorList.Add("c1 должно быть больше либо равно 0");
                }
            }
        }
        public double c3 { get => _c3; set => _c3 = value; }
        public double p
        {
            get => _p; 
            set
            {
                if (value > 0)
                {
                    _p = value;
                }
                else
                {
                    isError = true;
                    errorList.Add("p должно быть больше 0");
                }
            }
        }
        public double t
        {
            get => _t;
            set
            {
                const int MIN_TEMPERATURE = 20,
                            MAX_TEMPERATURE = 1000;
                if (value >= MIN_TEMPERATURE && t < MAX_TEMPERATURE)
                {
                    _t = value;
                }
                else
                {
                    isError = true;
                    errorList.Add($"T должна быть в диапазоне {MIN_TEMPERATURE} - {MAX_TEMPERATURE}");
                }
            }
        }
        public double E
        {
            get => _E;
            set
            {
                if (value > 0)
                {
                    _E = value;
                }
                else
                {
                    isError = true;
                    errorList.Add("E должно быть больше 0");
                }
            }
        }
        public double s { get => _s; set => _s = value; }
        public bool IsNeedpCalculate { get => isNeedpCalculate; set => isNeedpCalculate = value; }
        public double sigma_d
        {
            get => _sigma_d;
            set
            {
                {
                    if (value > 0)
                    {
                        _sigma_d = value;
                    }
                    else
                    {
                        isError = true;
                        errorList.Add("[σ] должно быть больше 0");
                    }
                }
            }
        }
        public double fi
        {
            get => _fi;
            set
            {
                const int MIN_FI = 0,
                            MAX_FI = 1;
                if (value >= MIN_FI && t <= MAX_FI)
                    if (value > 0)
                {
                    _fi = value;
                }
                else
                {
                    isError = true;
                    errorList.Add($"φ должен быть в диапазоне {MIN_FI} - {MAX_FI}");
                }
            }
        }
        public double l 
        {
            get => _l;
            set
            {
                if (value > 0)
                {
                    _l = value;
                }
                else
                {
                    isError = true;
                    errorList.Add("l должно быть больше 0");
                }
            } 
        }
        public double l3_1 { get => _l3_1; set => _l3_1 = value; }
        public double l3_2 { get => _l3_2; set => _l3_2 = value; }
        public double ny { get => _ny; set => _ny = value; }
        public double fit { get => _fit; set => _fit = value; }
        public double F { get => _F; set => _F = value; }
        public double q { get => _q; set => _q = value; }
        public double M { get => _M; set => _M = value; }
        public double Q { get => _Q; set => _Q = value; }
        public bool IsInError { get => isInError; set => isInError = value; }
        internal bool IsPressureIn { get => isPressureIn; set => isPressureIn = value; }
        public List<string> ErrorList { get => errorList; }

        internal EllipticalBottomType ellipticalBottomType;

        

        public double                                                                                                                         elH_f,
                        elh1_f,
                        f_f,
                        alfa_f,
                        R_f; //Spherical

        internal int FCalcSchema; //1-7

        internal bool             isNeedMakeCalcNozzle,
                    isNeedFCalculate,
                    isFTensile,
                    isNeedMCalculate,
                    isNeedQCalculate;


        internal int bibliography;
        private double _c1;
        private double _D;
        private double _c2;
        private double _c3;
        private double _p;
        private double _E;
        private double _t;
        private double _s;
        private bool isNeedpCalculate;
        private double _sigma_d;
        private double _fi;
        private double _l;
        private double _l3_1;
        private double _l3_2;
        private double _ny;
        private double _fit;
        private double _F;
        private double _q;
        private double _M;
        private double _Q;
        private bool isInError;
        private bool isPressureIn;

        public void SetValue(string name, double value)
        {
            var field = typeof(ShellDataIn).GetProperty(name);
            try
            {
                field.SetValue(this, value);
            }
            catch (System.Reflection.TargetInvocationException ex)
            {
                throw ex.InnerException; // System.Windows.Forms.MessageBox.Show(ex.InnerException.Message);
            }
            //catch (ArgumentOutOfRangeException)
            //{
            //    ;
            //}
        }

        public void SetValue(string name, string value)
        {
            var field = typeof(ShellDataIn).GetProperty(name);
            field.SetValue(this, value);
        }

        //public void GetType


    }

    class CylindricalShellDataIn : ShellDataIn, IDataIn
    {

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

        public string Error
        {
            get => error; 
            set
            {
                error += value;
            }
        }

        private string error;

    }

    //class Shell
    //{
    //    private string name;
    //    private string steel;

    //    internal string Name { get => name; set => name = value; }
    //    internal string Steel { get => steel; set => steel = value; }
    //    internal ShellType ShellType { get => shellType; set => shellType = value; }

    //    public Shell(ShellType shellType)
    //    {
    //        ShellType = shellType;
    //    }

    //    internal EllipticalBottomType EllipticalBottomType { get => ellipticalBottomType; set => ellipticalBottomType = value; }
    //    public double T { get => t; set => t = value; }

    //    private ShellType shellType;

    //    private EllipticalBottomType ellipticalBottomType;



    //    public double p,
    //                    E,
    //                    F,
    //                    M,
    //                    Q,
    //                    sigma_d,
    //                    D,
    //                    c1,
    //                    c2,
    //                    c3 = 0,
    //                    fi,
    //                    fit, // кольцевого сварного шва
    //                    s,
    //                    l,
    //                    l3_1,
    //                    l3_2,
    //                    ny = 2.4,
    //                    elH,
    //                    elh1,
    //                    q,
    //                    f,
    //                    alfa,
    //                    R; //Spherical

    //    internal int FCalcSchema; //1-7

    //    internal bool isNeedMakeCalcNozzle,
    //                isNeedpCalculate,
    //                isPressureIn,
    //                isNeedFCalculate,
    //                isFTensile,
    //                isNeedMCalculate,
    //                isNeedQCalculate;


    //    internal int bibliography;
    //    private double t;

    //    public void SetValue(string name, double value)
    //    {
    //        var field = typeof(Data_in).GetField(name);
    //        field.SetValue(this, value);
    //    }
    //}

    class CylindricalShell : Shell, IElement
    {
        public CylindricalShell(CylindricalShellDataIn cylindricalShellDataIn)
            : base(ShellType.Cylindrical)
        {
            //_t = cylindricalShellDataIn.t;
            csdi = cylindricalShellDataIn;
            //if (dataShellIn.ShellType == ShellType.Cylindrical)
            //{
            //    this._dataCylinderIn = dataShellIn;
            //    CalculateCylindricalShell(_dataCylinderIn);
            //}
        }

        public void Calculate()
        {

        }



        public void MakeWord(string filename)
        {
        //    if (filename == null)
        //    {
        //        const string DEFAULT_FILE_NAME = "temp.docx"; 
        //        filename = DEFAULT_FILE_NAME;
        //    }

        //    var doc = Xceed.Words.NET.DocX.Load(filename);

        //    doc.InsertParagraph().InsertPageBreakAfterSelf();
        //    doc.InsertParagraph($"Расчет на прочность обечайки {_name}, нагруженной ")
        //        .Heading(HeadingType.Heading1)
        //        .Alignment = Alignment.center;
        //    if (d_in.isPressureIn)
        //    {
        //        doc.Paragraphs.Last().Append("внутренним избыточным давлением");
        //    }
        //    else
        //    {
        //        doc.Paragraphs.Last().Append("наружным давлением");
        //    }
        //    doc.InsertParagraph().Alignment = Alignment.center;

        //    var image = doc.AddImage("pic/ObCil.gif");
        //    var picture = image.CreatePicture();
        //    doc.InsertParagraph().AppendPicture(picture);
        //    doc.InsertParagraph("Исходные данные").Alignment = Alignment.center;

        //    //table
        //    {
        //        var table = doc.AddTable(1, 2);
        //        table.SetWidths(new float[] { 300, 100 });
        //        int i = 0;
        //        //table.InsertRow(i);
        //        table.Rows[i].Cells[0].Paragraphs[0].Append("Материал обечайки");
        //        table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.Steel}");

        //        table.InsertRow(++i);
        //        table.Rows[i].Cells[0].Paragraphs[0].Append("Внутренний диаметр обечайки, D:");
        //        table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.D} мм");

        //        if (d_in.isPressureIn)
        //        {
        //            table.InsertRow(++i);
        //            table.Rows[i].Cells[0].Paragraphs[0].Append("Длина обечайки, l:");
        //            table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.l} мм");
        //        }
        //        table.InsertRow(++i);
        //        table.Rows[i].Cells[0].Paragraphs[0].Append("Прибавка на коррозию, ")
        //                                            .AppendEquation("c_1")
        //                                            .Append(":");
        //        table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.c1} мм");

        //        table.InsertRow(++i);
        //        table.Rows[i].Cells[0].Paragraphs[0].Append("Прибавка для компенсации минусового допуска, ")
        //                                            .AppendEquation("c_2")
        //                                            .Append(":");
        //        table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.c2} мм");

        //        if (d_in.c3 > 0)
        //        {
        //            table.InsertRow(++i);
        //            table.Rows[i].Cells[0].Paragraphs[0].Append("Технологическая прибавка, ")
        //                                                .AppendEquation("c_3")
        //                                                .Append(":");
        //            table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.c3} мм");
        //        }
        //        table.InsertRow(++i);
        //        table.Rows[i].Cells[0].Paragraphs[0].Append("Коэффициент прочности сварного шва, ")
        //                                            .AppendEquation("φ_p")
        //                                            .Append(":");
        //        table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.fi} мм");

        //        doc.InsertParagraph().InsertTableAfterSelf(table);
        //    }

        //    doc.InsertParagraph();
        //    doc.InsertParagraph("Условия нагружения").Alignment = Alignment.center;

        //    //table
        //    {
        //        var table = doc.AddTable(1, 2);
        //        table.SetWidths(new float[] { 300, 100 });
        //        int i = 0;
        //        table.Rows[i].Cells[0].Paragraphs[0].Append("Расчетная температура, Т:");
        //        table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.temp} °С");

        //        table.InsertRow(++i);
        //        if (d_in.isPressureIn)
        //        {
        //            table.Rows[i].Cells[0].Paragraphs[0].Append("Расчетное внутреннее избыточное давление, p:");
        //        }
        //        else
        //        {
        //            table.Rows[i].Cells[0].Paragraphs[0].Append("Расчетное наружное давление, p:");
        //        }
        //        table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.p} МПа");

        //        table.InsertRow(++i);
        //        table.Rows[i].Cells[0].Paragraphs[0].Append($"Допускаемое напряжение для материала {d_in.Steel} при расчетной температуре, [σ]:");
        //        table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.sigma_d} МПа");
        //        if (!d_in.isPressureIn)
        //        {
        //            table.InsertRow(++i);
        //            table.Rows[i].Cells[0].Paragraphs[0].Append("Модуль продольной упругости при расчетной температуре, E:");
        //            table.Rows[i].Cells[1].Paragraphs[0].Append($"{d_in.E} МПа");
        //        }
        //        doc.InsertParagraph().InsertTableAfterSelf(table);
        //    }

        //    doc.InsertParagraph("");
        //    doc.InsertParagraph("Результаты расчета").Alignment = Alignment.center;
        //    doc.InsertParagraph("");
        //    doc.InsertParagraph("Толщину стенки вычисляют по формуле:");
        //    doc.InsertParagraph().AppendEquation("s≥s_p+c");
        //    doc.InsertParagraph("где ").AppendEquation("s_p").Append(" - расчетная толщина стенки обечайки");
        //    if (d_in.isPressureIn)
        //    {
        //        doc.InsertParagraph().AppendEquation("s_p=(p∙D)/(2∙[σ]∙φ_p-p)");
        //        doc.InsertParagraph().AppendEquation($"s_p=({d_in.p}∙{d_in.D})/(2∙{d_in.sigma_d}∙{d_in.fi}-{d_in.p})=" +
        //                                            "{d_out.s_calcr:f2} мм");
        //    }
        //    else
        //    {
        //        doc.InsertParagraph().AppendEquation("s_p=max{1.06∙(10^-2∙D)/(B)∙(p/(10^-5∙E)∙l/D)^0.4;(1.2∙p∙D)/(2∙[σ]-p)}");
        //        doc.InsertParagraph("Коэффициент B вычисляют по формуле:");
        //        doc.InsertParagraph().AppendEquation("B=max{1;0.47∙(p/(10^-5∙E))^0.067∙(l/D)^0.4}");
        //        doc.InsertParagraph().AppendEquation($"0.47∙({d_in.p}/(10^-5∙{d_in.E}))^0.067∙({d_out.l}/{d_in.D})^0.4={d_out.b_2:f2}");
        //        doc.InsertParagraph().AppendEquation($"B=max(1;{d_out.b_2:f2})={d_out.b:f2}");
        //        doc.InsertParagraph().AppendEquation($"1.06∙(10^-2∙{d_in.D})/({d_out.b:f2})∙({d_in.p}/(10^-5∙{d_in.E})∙{d_out.l}/{d_in.D})^0.4={d_out.s_calcr1:f2}");
        //        doc.InsertParagraph().AppendEquation($"(1.2∙{d_in.p}∙{d_in.D})/(2∙{d_in.sigma_d}-{d_in.p})={d_out.s_calcr2:f2}");
        //        doc.InsertParagraph().AppendEquation($"s_p=max({d_out.s_calcr1:f2};{d_out.s_calcr2:f2})={d_out.s_calcr:f2} мм");
        //    }

        //    doc.InsertParagraph("c - сумма прибавок к расчетной толщине");
        //    doc.InsertParagraph().AppendEquation("c=c_1+c_2+c_3");
        //    doc.InsertParagraph().AppendEquation($"c={d_in.c1}+{d_in.c2}+{d_in.c3}={d_out.c:f2} мм");

        //    doc.InsertParagraph().AppendEquation($"s={d_out.s_calcr:f2}+{d_out.c:f2}={d_out.s_calc:f2} мм");
        //    if (d_in.s > d_out.s_calc)
        //    {
        //        doc.InsertParagraph($"Принятая толщина s={d_in.s} мм").Bold();
        //    }
        //    else
        //    {
        //        doc.InsertParagraph($"Принятая толщина s={d_in.s} мм").Bold().Color(System.Drawing.Color.Red);
        //    }
        //    if (d_in.isPressureIn)
        //    {
        //        doc.InsertParagraph("Допускаемое внутреннее избыточное давление вычисляют по формуле:");
        //        doc.InsertParagraph().AppendEquation("[p]=(2∙[σ]∙φ_p∙(s-c))/(D+s-c)");
        //        doc.InsertParagraph().AppendEquation($"[p]=(2∙{d_in.sigma_d}∙{d_in.fi}∙({d_in.s}-{d_out.c:f2}))/" +
        //                                            "({d_in.D}+{d_in.s}-{d_out.c:f2})={d_out.p_d:f2} МПа");
        //    }
        //    else
        //    {
        //        doc.InsertParagraph("Допускаемое наружное давление вычисляют по формуле:");
        //        doc.InsertParagraph().AppendEquation("[p]=[p]_П/√(1+([p]_П/[p]_E)^2)");
        //        doc.InsertParagraph("допускаемое давление из условия прочности вычисляют по формуле:");
        //        doc.InsertParagraph().AppendEquation("[p]_П=(2∙[σ]∙(s-c))/(D+s-c)");
        //        doc.InsertParagraph().AppendEquation($"[p]_П=(2∙{d_in.sigma_d}∙({d_in.s}-{d_out.c:f2}))/" +
        //                                            "({d_in.D}+{d_in.s}-{d_out.c:f2})={d_out.p_dp:f2} МПа");
        //        doc.InsertParagraph("допускаемое давление из условия устойчивости в пределах упругости вычисляют по формуле:");
        //        doc.InsertParagraph().AppendEquation("[p]_E=(2.08∙10^-5∙E)/(n_y∙B_1)∙D/l∙[(100∙(s-c))/D]^2.5");
        //        doc.InsertParagraph("коэффициент ").AppendEquation("B_1").Append(" вычисляют по формуле");
        //        doc.InsertParagraph().AppendEquation("B_1=min{1;9.45∙D/l∙√(D/(100∙(s-c)))}");
        //        doc.InsertParagraph().AppendEquation($"9.45∙{d_in.D}/{d_out.l}∙√({d_in.D}/(100∙({d_in.s}-{d_out.c:f2})))=" +
        //                                            "{d_out.b1_2:f2}");
        //        doc.InsertParagraph().AppendEquation($"B_1=min(1;{d_out.b1_2:f2})={d_out.b1:f1}");
        //        doc.InsertParagraph().AppendEquation($"[p]_E=(2.08∙10^-5∙{d_in.E})/({d_in.ny}∙{d_out.b1:f2})∙{d_in.D}/" +
        //                                            "{d_out.l}∙[(100∙({d_in.s}-{d_out.c:f2}))/{d_in.D}]^2.5={d_out.p_de:f2} МПа");
        //        doc.InsertParagraph().AppendEquation($"[p]={d_out.p_dp:f2}/√(1+({d_out.p_dp:f2}/{d_out.p_de:f2})^2)={d_out.p_d:f2} МПа");
        //    }

        //    doc.InsertParagraph().AppendEquation("[p]≥p");
        //    doc.InsertParagraph().AppendEquation($"{d_out.p_d:f2}≥{d_in.p}");
        //    if (d_out.p_d > d_in.p)
        //    {
        //        doc.InsertParagraph("Условие прочности выполняется").Bold();
        //    }
        //    else
        //    {
        //        doc.InsertParagraph("Условие прочности не выполняется").Bold().Color(System.Drawing.Color.Red);
        //    }
        //    if (d_out.isConditionUseFormuls)
        //    {
        //        doc.InsertParagraph("Границы применения формул ");
        //    }
        //    else
        //    {
        //        doc.InsertParagraph("Границы применения формул. Условие не выполняется ").Bold().Color(System.Drawing.Color.Red);
        //    }
        //    const int DIAMETR_BIG_LITTLE_BORDER = 200;
        //    if (d_in.D >= DIAMETR_BIG_LITTLE_BORDER)
        //    {
        //        doc.Paragraphs.Last().Append("при D ≥ 200 мм");
        //        doc.InsertParagraph().AppendEquation("(s-c)/(D)≤0.1");
        //        doc.InsertParagraph().AppendEquation($"({d_in.s}-{d_out.c:f2})/({d_in.D})={(d_in.s - d_out.c) / d_in.D:f3}≤0.1");
        //    }
        //    else
        //    {
        //        doc.Paragraphs.Last().Append("при D < 200 мм");
        //        doc.InsertParagraph().AppendEquation("(s-c)/(D)≤0.3");
        //        doc.InsertParagraph().AppendEquation($"({d_in.s}-{d_out.c:f2})/({d_in.D})={(d_in.s - d_out.c) / d_in.D:f3}≤0.3");
        //    }

        //    doc.SaveAs(filename);

        }

        private void CalculateCylindricalShell()
        {

            _c = csdi.c1 + csdi.c2 + csdi.c3;

            // Condition use formuls
            {
                const int DIAMETR_BIG_LITTLE_BORDER = 200;
                bool isDiametrBig = DIAMETR_BIG_LITTLE_BORDER < csdi.D;

                //bool isConditionUseFormuls;

                if (isDiametrBig)
                {
                    const double CONDITION_USE_FORMULS_BIG_DIAMETR = 0.1;
                    isConditionUseFormuls = ((csdi.s - _c) / csdi.D) <= CONDITION_USE_FORMULS_BIG_DIAMETR;
                }
                else
                {
                    const double CONDITION_USE_FORMULS_LITTLE_DIAMETR = 0.3;
                    isConditionUseFormuls = ((csdi.s - _c) / csdi.D) <= CONDITION_USE_FORMULS_LITTLE_DIAMETR;
                }

                if (!isConditionUseFormuls)
                {
                    isError = true;
                    err += "Условие применения формул не выполняется\n";
                }

            }

            if (csdi.p > 0)
            {
                if (csdi.IsPressureIn)
                {

                    _s_calcr = csdi.p * csdi.D / (2 * csdi.sigma_d * csdi.fi - csdi.p);
                    _s_calc = _s_calcr + _c;
                    if (csdi.s == 0.0)
                    {
                        _p_d = 2 * csdi.sigma_d * csdi.fi * (_s_calc - _c) / (csdi.D + _s_calc - _c);
                    }
                    else if (csdi.s >= _s_calc)
                    {
                        _p_d = 2 * csdi.sigma_d * csdi.fi * (csdi.s - _c) / (csdi.D + csdi.s - _c);
                    }
                    else
                    {
                        throw new Exception("");
                    }
                }
                else
                {
                    _l = csdi.l + csdi.l3_1 + csdi.l3_2;
                    _b_2 = 0.47 * Math.Pow(csdi.p / (0.00001 * csdi.E), 0.067) * Math.Pow(_l / csdi.D, 0.4);
                    _b = Math.Max(1.0, _b_2);
                    _s_calcr1 = 1.06 * (0.01 * csdi.D / _b) * Math.Pow(csdi.p / (0.00001 * csdi.E) * (_l / csdi.D), 0.4);
                    _s_calcr2 = 1.2 * csdi.p * csdi.D / (2 * csdi.sigma_d - csdi.p);
                    _s_calcr = Math.Max(_s_calcr1, _s_calcr2);
                    _s_calc = _s_calcr + _c;
                    if (csdi.s == 0.0)
                    {
                        _p_dp = 2 * csdi.sigma_d * (_s_calc - _c) / (csdi.D + _s_calc - _c);
                        _b1_2 = 9.45 * (csdi.D / _l) * Math.Sqrt(csdi.D / (100 * (_s_calc - _c)));
                        _b1 = Math.Min(1.0, _b1_2);
                        _p_de = 2.08 * 0.00001 * csdi.E / csdi.ny * _b1 * (csdi.D / _l) * Math.Pow(100 * (_s_calc - _c) / csdi.D, 2.5);
                    }
                    else if (csdi.s >= _s_calc)
                    {
                        _p_dp = 2 * csdi.sigma_d * (csdi.s - _c) / (csdi.D + csdi.s - _c);
                        _b1_2 = 9.45 * (csdi.D / _l) * Math.Sqrt(csdi.D / (100 * (csdi.s - _c)));
                        _b1 = Math.Min(1.0, _b1_2);
                        _p_de = 2.08 * 0.00001 * csdi.E / csdi.ny * _b1 * (csdi.D / _l) * Math.Pow(100 * (csdi.s - _c) / csdi.D, 2.5);
                    }
                    else
                    {
                        throw new Exception("Принятая толщина меньше расчетной\nрасчет не выполнен");
                    }
                    _p_d = _p_dp / Math.Sqrt(1 + Math.Pow(_p_dp / _p_de, 2));
                }
                if (_p_d < csdi.p && csdi.s != 0)
                {
                    isError = true;
                    err += "[p] меньше p";
                }
            }

            if (csdi.F > 0)
            {
                _s_calcrf = csdi.F / (Math.PI * csdi.D * csdi.sigma_d * csdi.fit);
                _s_calcf = _s_calcrf + _c;
                if (csdi.isFTensile)
                {
                    _F_d = Math.PI * (csdi.D + csdi.s - _c) * (csdi.s - _c) * csdi.sigma_d * csdi.fit;
                }
                else
                {
                    _F_dp = Math.PI * (csdi.D + csdi.s - _c) * (csdi.s - _c) * csdi.sigma_d;
                    _F_de1 = 0.000031 * csdi.E / csdi.ny * Math.Pow(csdi.D, 2) * Math.Pow(100 * (csdi.s - _c) / csdi.D, 2.5);

                    const int L_MORE_THEN_D = 10;
                    bool isLMoreThenD = csdi.l / csdi.D > L_MORE_THEN_D;

                    if (isLMoreThenD)
                    {
                        switch (csdi.FCalcSchema)
                        {
                            case 1:
                                _lpr = csdi.l;
                                break;
                            case 2:
                                _lpr = 2 * csdi.l;
                                break;
                            case 3:
                                _lpr = 0.7 * csdi.l;
                                break;
                            case 4:
                                _lpr = 0.5 * csdi.l;
                                break;
                            case 5:
                                _F = csdi.q * csdi.l;
                                _lpr = 1.12 * csdi.l;
                                break;
                            case 6:
                                double fDivl6 = csdi.F / csdi.l;
                                fDivl6 *= 10;
                                fDivl6 = Math.Round(fDivl6 / 2);
                                fDivl6 *= 0.2;
                                switch (fDivl6)
                                {
                                    case 0:
                                        _lpr = 2 * csdi.l;
                                        break;
                                    case 0.2:
                                        _lpr = 1.73 * csdi.l;
                                        break;
                                    case 0.4:
                                        _lpr = 1.47 * csdi.l;
                                        break;
                                    case 0.6:
                                        _lpr = 1.23 * csdi.l;
                                        break;
                                    case 0.8:
                                        _lpr = 1.06 * csdi.l;
                                        break;
                                    case 1:
                                        _lpr = csdi.l;
                                        break;
                                }
                                break;
                            case 7:
                                double fDivl7 = csdi.F / csdi.l;
                                fDivl7 *= 10;
                                fDivl7 = Math.Round(fDivl7 / 2);
                                fDivl7 *= 0.2;
                                switch (fDivl7)
                                {
                                    case 0:
                                        _lpr = 2 * csdi.l;
                                        break;
                                    case 0.2:
                                        _lpr = 1.7 * csdi.l;
                                        break;
                                    case 0.4:
                                        _lpr = 1.4 * csdi.l;
                                        break;
                                    case 0.6:
                                        _lpr = 1.11 * csdi.l;
                                        break;
                                    case 0.8:
                                        _lpr = 0.85 * csdi.l;
                                        break;
                                    case 1:
                                        _lpr = 0.7 * csdi.l;
                                        break;
                                }
                                break;

                        }
                        lamda = 2.83 * _lpr / (csdi.D + csdi.s - _c);
                        _F_de2 = Math.PI * (csdi.D + csdi.s - _c) * (csdi.s - _c) * csdi.E / csdi.ny *
                                        Math.Pow(Math.PI / lamda, 2);
                        _F_de = Math.Min(_F_de1, _F_de2);
                    }
                    else
                    {
                        _F_de = _F_de1;
                    }

                    _F_d = _F_dp / Math.Sqrt(1 + Math.Pow(_F_dp / _F_de, 2));
                }
            }

            if (csdi.M > 0)
            {
                _M_dp = Math.PI / 4 * csdi.D * (csdi.D + csdi.s - _c) * (csdi.s - _c) * csdi.sigma_d;
                _M_de = 0.000089 * csdi.E / csdi.ny * Math.Pow(csdi.D, 3) * Math.Pow(100 * (csdi.s - _c) / csdi.D, 2.5);
                _M_d = _M_dp / Math.Sqrt(1 + Math.Pow(_M_dp / _M_de, 2));
            }

            if (csdi.Q > 0)
            {
                _Q_dp = 0.25 * csdi.sigma_d * Math.PI * csdi.D * (csdi.s - _c);
                _Q_de = 2.4 * csdi.E * Math.Pow(csdi.s - _c, 2) / csdi.ny *
                    (0.18 + 3.3 * csdi.D * (csdi.s - _c) / Math.Pow(csdi.l, 2));
                _Q_d = _Q_dp / Math.Sqrt(1 + Math.Pow(_Q_dp / _Q_de, 2));
            }

            if ((csdi.IsNeedpCalculate || csdi.isNeedFCalculate) &&
                (csdi.isNeedMCalculate || csdi.isNeedQCalculate))
            {
                conditionYstoich = csdi.p / _p_d + csdi.F / _F_d + csdi.M / _M_d +
                                        Math.Pow(csdi.Q / _Q_d, 2);
            }
            // TODO: Проверка F для FCalcSchema == 6 F расчитывается и записывается в d_out, а не берется из d_in
            //return d_out;
        }

        private readonly CylindricalShellDataIn csdi;



        //internal string Name { get => _dataCylinderIn.Name; }
        //internal string Steel { get => _dataCylinderIn.Steel; }
        ////internal ShellType ShellType { get => shellType; }
        //public double t { get => _dataCylinderIn.t; }
        //public double L_in { get => _dataCylinderIn.L; }
        //public double ny { get => _dataCylinderIn.ny; }
        //public double F { get => _dataCylinderIn.F; }

        //internal EllipticalBottomType EllipticalBottomType { get => ellipticalBottomType; set => ellipticalBottomType = value; }
        
        internal double c { get => _c; }
        internal bool IsConditionUseFormuls { get => isConditionUseFormuls; }
        internal bool IsError { get => isError; }
        internal string ErrorString { get => err; }
        internal double s_calcr { get => _s_calcr; }
        internal double s_calc { get => _s_calc; }
        internal double s_calcr1 { get => _s_calcr1;  }
        internal double s_calcr2 { get => _s_calcr2;  }
        internal double s_calcrf { get => _s_calcrf;  }
        internal double s_calcf { get => _s_calcf;  }
        internal double p_d { get => _p_d;  }
        internal double b { get => _b;  }
        internal double b_2 { get => _b_2;  }
        internal double b1 { get => _b1;  }
        internal double b1_2 { get => _b1_2; }
        internal double p_dp { get => _p_dp;  }
        internal double p_de { get => _p_de;  }
        internal double F_d { get => _F_d; }
        internal double F_dp { get => _F_dp; }
        internal double F_de { get => _F_de;  }
        internal double F_de1 { get => _F_de1;  }
        internal double F_de2 { get => _F_de2;  }
        internal double Lamda { get => lamda;  }
        internal double M_d { get => _M_d; }
        internal double M_dp { get => _M_dp; }
        internal double M_de { get => _M_de;  }
        internal double Q_d { get => _Q_d;  }
        internal double Q_dp { get => _Q_dp;  }
        internal double Q_de { get => _Q_de;  }
        internal double ElR { get => _elR;  }
        internal double Elke { get => _elke;  }
        internal double Elx { get => _elx;  }
        internal double Dk { get => _Dk; }
        internal double Lpr { get => _lpr; }
        internal double F1 { get => _F1;  }
        internal double ConditionYstoich { get => conditionYstoich;  }
        internal double L { get => _l; }
        

        private EllipticalBottomType ellipticalBottomType;

        private double _t;

        public double p,
                        E,
                        M,
                        Q,
                        sigma_d,
                        D,
                        c1,
                        c2,
                        c3 = 0,
                        fi,
                        fit, // кольцевого сварного шва
                        s,
                        l3_1,
                        l3_2,

                        elH,
                        elh1,
                        q,
                        f,
                        alfa,
                        R; //Spherical

        internal int FCalcSchema; //1-7

        internal bool isNeedMakeCalcNozzle,
                    isNeedpCalculate,
                    isPressureIn,
                    isNeedFCalculate,
                    isFTensile,
                    isNeedMCalculate,
                    isNeedQCalculate;


        internal int bibliography;


        public void SetValue(string name, double value)
        {
            var field = typeof(Data_in).GetField(name);
            field.SetValue(this, value);
        }






        private double _l;

        private string err;
        internal bool _isCriticalError;
        private double _c;
        private bool isConditionUseFormuls;
        private bool isError;
        private double _s_calcr;
        private double _s_calc;
        private double _s_calcr1;
        private double _s_calcr2;
        private double _s_calcrf;
        private double _s_calcf;
        private double _p_d;
        private double _b;
        private double _b_2;
        private double _b1;
        private double _b1_2;
        private double _p_dp;
        private double _p_de;
        private double _F_d;
        private double _F_dp;
        private double _F_de;
        private double _F_de1;
        private double _F_de2;
        private double lamda;
        private double _M_d;
        private double _M_dp;
        private double _M_de;
        private double _Q_d;
        private double _Q_dp;
        private double _Q_de;
        private double _elR;
        private double _elke;
        private double _elx;
        private double _Dk;
        private double _lpr;
        private double _F1;
        private double conditionYstoich;
        private double l_in;
        private double _F;
    }

}
