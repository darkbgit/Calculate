using CalculateVessels.Core.HeatExchanger.Enums;
using CalculateVessels.Core.HeatExchangers.HeatExchangerStationaryTubePlates;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Properties;
using CalculateVessels.Output.Word.Core;
using CalculateVessels.Output.Word.Enums;
using CalculateVessels.Output.Word.Helpers;
using CalculateVessels.Output.Word.Interfaces;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace CalculateVessels.Output.Word.Elements;

internal class HeatExchangerStationaryTubePlatesWordOutput : IWordOutputElement<HeatExchangerStationaryTubePlatesCalculated>
{
    public void MakeWord(string filePath, ICalculatedElement calculatedData)
    {
        if (calculatedData is not HeatExchangerStationaryTubePlatesCalculated data)
            throw new NullReferenceException();

        var dataIn = (HeatExchangerStationaryTubePlatesInput)data.InputData;

        filePath = WordHelpers.CheckFilePath(filePath);

        using var package = WordprocessingDocument.Open(filePath, true);

        var mainPart = package.MainDocumentPart ?? throw new InvalidOperationException();
        var body = mainPart.Document.Body ?? throw new InvalidOperationException();

        InsertHeader(body, dataIn);

        InsertImage(mainPart, dataIn);

        InsertInputDataTable(body, dataIn, data);

        InsertDataCalculated(mainPart, body, dataIn, data);

        package.Close();
    }

    private static void InsertDataCalculated(MainDocumentPart mainPart, Body body, HeatExchangerStationaryTubePlatesInput dataIn, HeatExchangerStationaryTubePlatesCalculated data)
    {
        body.AddParagraph("Результаты расчета")
            .Alignment(AlignmentType.Center);

        body.AddParagraph("Вспомогательные величины")
            .Alignment(AlignmentType.Center);

        body.AddParagraph("Относительную характеристику беструбного края трубной решетки вычисляют по формуле");

        body.AddParagraph()
            .AppendEquation($"m_n=a/a_1={data.a}/{dataIn.a1}={data.mn:f2}");

        body.AddParagraph("Коэффициенты влияния давления на трубную решетку вычисляют по формулам:");
        body.AddParagraph("- со стороны межтрубного пространства");
        body.AddParagraph()
            .AppendEquation($"η_M=1-(i∙d_T^2)/(4∙a_1^2)=1-({dataIn.i}∙{dataIn.dT}^2)/(4∙{dataIn.a1}^2)={data.etaM:f2}");
        body.AddParagraph("- со стороны трубного пространства");
        body.AddParagraph()
            .AppendEquation($"η_T=1-(i∙(d_T-2∙s_T)^2)/(4∙a_1^2)=1-({dataIn.i}∙({dataIn.dT}-{dataIn.sT})^2)/(4∙{dataIn.a1}^2)={data.etaT:f2}");
        body.AddParagraph("Основные характеристики жесткости элементов теплообменного аппарата");
        body.AddParagraph("Модуль упругости основания (системы труб) вычисляют по формуле");
        body.AddParagraph()
            .AppendEquation($"K_y=(E_T∙(η_T-η_M))/l=({data.ET}∙({data.etaT:f2}-{data.etaM:f2}))/{dataIn.l}={data.Ky:f2}");
        body.AddParagraph("Приведенное отношение жесткости труб к жесткости кожуха вычисляют по формуле");
        body.AddParagraph()
            .AppendEquation($"ρ=(K_y∙a_1∙l)/(E_K∙s_K)=({data.Ky:f2}∙{dataIn.a1}∙{dataIn.l})/({data.EK}∙{dataIn.sK})={data.ro:f2}");
        body.AddParagraph("Коэффициенты изменения жесткости системы трубы — кожух вычисляют по формулам:");
        body.AddParagraph()
            .AppendEquation("K_q=1+K_q^*");
        body.AddParagraph()
            .AppendEquation("К_р=1+К_p^*");

        switch (dataIn.CompensatorType)
        {
            case CompensatorType.No:
                body.AddParagraph("Для аппаратов с неподвижными трубными решетками")
                    .AppendEquation("K_q^*=K_p^*=0");
                break;
            case CompensatorType.Compensator:
                body.AddParagraph("Для аппаратов с компенсатором на кожухе");
                body.AddParagraph()
                    .AppendEquation($"K_q^*=(π∙a∙E_K∙s_K)/(l∙K_ком)={data.Kqz:f2}");
                body.AddParagraph()
                    .AppendEquation($"K_p^*=(π∙(D_ком^2-d_ком^2)∙E_K∙s_K)/(4.8∙l∙a∙K_ком)={data.Kpz:f2}");

                if (dataIn.IsNeedCompensatorCalculate)
                {
                    //TODO: Get calculate Kkom
                }
                break;
            case CompensatorType.Expander:
                //TODO:
                if (Math.Abs(dataIn.beta0 - 90) < 0.000001)
                {

                }
                else if (dataIn.beta0 is >= 15 and <= 60)
                {

                }
                break;
            case CompensatorType.CompensatorOnExpander:
                //TODO: Make calculation compensator on expander 
                break;
        }

        body.AddParagraph()
            .AppendEquation($"K_q=1+{data.Kqz:f2}={data.Kq:f2}");
        body.AddParagraph()
            .AppendEquation($"К_р=1+{data.Kpz:f2}={data.Kp:f2}");

        body.AddParagraph("Коэффициент ослабления трубной решетки при расчете кожухотрубчатых теплообменных аппаратов с неподвижными трубными решетками и компенсатором на кожухе вычисляют по формуле");
        body.AddParagraph()
            .AppendEquation($"φ_p=1-d_0/t_p=1-{dataIn.d0}/{dataIn.tp}={data.fip:f2}");

        body.AddParagraph("Коэффициент жесткости перфорированной плиты вычисляют по формуле");
        body.AddParagraph()
            .AppendEquation($"ψ_0=η_T^(7/3)={data.psi0:f2}");

        body.AddParagraph("Коэффициент системы решетка — трубы" +
                          (dataIn.IsDifferentTubePlate ? " для теплообменных аппаратов с двумя отличающимися друг от друга по толщине или модулю упругости решетками" : " ") +
                          "вычисляют по формуле");

        if (!dataIn.IsDifferentTubePlate)
        {
            body.AddParagraph()
                .AppendEquation($"β=1.82/s_p∙∜((K_y∙s_p)/(ψ_0∙E_p))=1.82/{dataIn.FirstTubePlate.sp}∙∜(({data.Ky:f2}∙{dataIn.FirstTubePlate.sp})/({data.psi0:f2}∙{data.Ep}))={data.beta:f2}");
        }
        else
        {
            body.AddParagraph()
                .AppendEquation($"β=1.53∙∜(K_y/ψ_0∙(1/(E_p1∙s_p1^3)+1/(E_p2∙s_p2^3))" +
                                $"=1.53∙∜({data.Ky:f2}/{data.psi0:f2}∙(1/({data.Ep}∙{dataIn.FirstTubePlate.sp}^3)+1/({data.Ep2}∙{dataIn.SecondTubePlate.sp}^3))={data.beta:f2}");
        }

        body.AddParagraph("Безразмерный параметр системы решетка — трубы вычисляют по формуле");
        body.AddParagraph()
            .AppendEquation($"ω=β∙a_1={data.beta:f2}∙{dataIn.a1}={data.omega:f2}");

        body.AddParagraph("Коэффициент системы кожух — решетка");
        body.AddParagraph()
            .AppendEquation($"β_1=1.3/√(a∙s_1)=1.3/√({data.a}∙{dataIn.FirstTubePlate.s1})={data.beta1:f2}");

        body.AddParagraph("Коэффициент системы обечайка — фланец камеры");
        body.AddParagraph()
            .AppendEquation($"β_2=1.3/√(a∙s_2)=1.3/√({data.a}∙{dataIn.FirstTubePlate.s2})={data.beta2:f2}");

        body.AddParagraph("Коэффициент жесткости фланцевого соединения при изгибе");
        body.AddParagraph()
            .AppendEquation("K_Φ=K_Φ1+K_Φ2");

        body.AddParagraph()
            .AppendEquation("K_Φ1=(E_1∙h_1^3∙b_1)/(12∙R_1^2)+K_1∙(1+(β_1∙h_1)/2)");

        body.AddParagraph()
            .AppendEquation($"K_1=(β_1∙a∙E_K∙s_1^3)/(5.5∙R_1)=({data.beta1:f2}∙{data.a}∙{data.EK}∙{dataIn.FirstTubePlate.s1}^3)/(5.5∙{data.R1:f2})={data.K1:f2}");

        body.AddParagraph()
            .AppendEquation($"K_Φ1=({data.E1}∙{dataIn.FirstTubePlate.h1}^3∙{data.b1:f2})/(12∙{data.R1:f2}^2)+{data.K1:f2}∙(1+({data.beta1:f2}∙{dataIn.FirstTubePlate.h1:f2})/2)={data.KF1:f2}");

        body.AddParagraph()
            .AppendEquation("K_Φ2=(E_2∙h_2^3∙b_2)/(12∙R_2^2)+K_2∙(1+(β_2∙h_2)/2)");

        body.AddParagraph()
            .AppendEquation($"K_2=(β_2∙a∙E_D∙s_2^3)/(5.5∙R_2)=({data.beta2:f2}∙{data.a}∙{data.ED}∙{dataIn.FirstTubePlate.s2}^3)/(5.5∙{data.R2:f2})={data.K2:f2}");

        body.AddParagraph()
            .AppendEquation($"K_Φ1=({data.E2}∙{dataIn.FirstTubePlate.h2}^3∙{data.b2:f2})/(12∙{data.R2:f2}^2)+{data.K2:f2}∙(1+({data.beta2:f2}∙{dataIn.FirstTubePlate.h2:f2})/2)={data.KF2:f2}");

        body.AddParagraph()
            .AppendEquation($"K_Φ={data.KF1:f2}+{data.KF2:f2}={data.KF:f2}");

        body.AddParagraph();

        body.AddParagraph("Определение усилий в элементах теплообменного аппарата").Alignment(AlignmentType.Center);

        body.AddParagraph("Приведенное давление ")
            .AppendEquation("p_0")
            .AddRun(" вычисляют по формуле");

        body.AddParagraph()
            .AppendEquation("p_0=[α_K∙(t_K-t_0)-α_T∙(t_T-t_0)∙K_y∙l+[η_T-1+m_cp+m_n∙(m_n+0.5∙ρ∙K_q)]∙p_T-[η_M-1+m_cp+m_n∙(m_n+0.3∙ρ∙K_p)]∙p_M]");

        body.AddParagraph("где ")
            .AppendEquation("m_cp")
            .AddRun(" - коэффициент влияния давления на продольную деформацию труб");

        body.AddParagraph()
            .AppendEquation($"m_cp=0.15∙(i∙(d_T-s_T)^2)/a_1^2=0.15∙({dataIn.i}∙({dataIn.dT}-{dataIn.sT})^2)/{dataIn.a1}^2={data.mcp:f2}");

        body.AddParagraph()
            .AppendEquation($"p_0=[{data.alfaK}∙({dataIn.tK}-{dataIn.t0})-{data.alfaT}∙({dataIn.tT}-{dataIn.t0})∙{data.Ky:f2}∙{dataIn.l}+[{data.etaT:f2}-1+{data.mcp:f2}+{data.mn:f2}∙({data.mn:f2}+0.5∙{data.ro:f2}∙{data.Kq:f2})]∙{dataIn.pT}-[{data.etaM:f2}-1+{data.mcp:f2}+{data.mn:f2}∙({data.mn:f2}+0.3∙{data.ro:f2}∙{data.Kp:f2})]∙{dataIn.pM}]={data.p0:f2} МПа");

        body.AddParagraph("Приведенное отношение жесткости труб к жесткости фланцевого соединения вычисляют по формуле");

        body.AddParagraph()
            .AppendEquation($"ρ_1=(K_y∙a∙a_1)/(β_2∙K_Φ∙R_1)=({data.Ky:f2}∙{data.a}∙{dataIn.a1})/({data.beta2:f2}∙{data.KF:f2}∙{data.R1:f2})={data.ro1:f2}");

        body.AddParagraph("Коэффициенты, учитывающие поддерживающее влияние труб ")
            .AppendEquation($"Φ_1={data.Phi1}, Φ_2={data.Phi2}, Φ_3={data.Phi3}")
            .AddRun(" определяют по таблице 1 ГОСТ 34233.7");

        body.AddParagraph()
            .AppendEquation("T_1=Φ_1∙[m_n+0.5∙(1+m_n∙t)(t-1)]");
        body.AddParagraph()
            .AppendEquation("T_2=Φ_2∙t");
        body.AddParagraph()
            .AppendEquation("T_3=Φ_3∙m_n");

        body.AddParagraph()
            .AppendEquation($"t=1+1.4∙ω∙(m_n-1)=1+1.4∙{data.omega:f2}∙({data.mn:f2}-1)={data.t:f2}");

        body.AddParagraph()
            .AppendEquation($"T_1={data.Phi1:f2}∙[{data.mn:f2}+0.5∙(1+{data.mn:f2}∙{data.t:f2})({data.t:f2}-1)]={data.T1:f2}");
        body.AddParagraph()
            .AppendEquation($"T_2={data.Phi2:f2}∙{data.t:f2}={data.T2:f2}");
        body.AddParagraph()
            .AppendEquation($"T_3={data.Phi3:f2}∙{data.mn:f2}={data.T3:f2}");

        body.AddParagraph("Изгибающий момент и перерезывающую силу, распределенные по краю трубной решетки, вычисляют по формулам:");
        body.AddParagraph("- для изгибающего момента");

        body.AddParagraph()
            .AppendEquation("M_Π=(a_1/β)∙(p_1∙(T_1+ρ∙K_q)-p_0∙T_2)/((T_1+ρ∙K_q)∙(T_3+ρ_1)-T_2^2)");

        body.AddParagraph("- для перерезывающей силы");

        body.AddParagraph()
            .AppendEquation("Q_Π=a_1∙(p_0∙(T_3+ρ_1)-p_1∙T_2)/((T_1+ρ∙K_q)∙(T_3+ρ_1)-T_2^2)");

        body.AddParagraph("где");

        body.AddParagraph()
            .AppendEquation("p_1=K_y/(β∙K_Φ)∙(m_1∙p_M-m_2∙p_T)");

        body.AddParagraph()
            .AppendEquation($"m_1=(1-β_1∙h_1)/(2∙β_1^2)=(1-{data.beta1:f2}∙{dataIn.FirstTubePlate.h1})/(2∙{data.beta1:f2}^2)={data.m1:f2}");

        body.AddParagraph()
            .AppendEquation($"m_2=(1-β_2∙h_2)/(2∙β_2^2)=(1-{data.beta2:f2}∙{dataIn.FirstTubePlate.h2})/(2∙{data.beta2:f2}^2)={data.m2:f2}");

        body.AddParagraph()
            .AppendEquation($"p_1={data.Ky:f2}/({data.beta:f2}∙{data.KF:f2})∙({data.m1:f2}∙{dataIn.pM}-{data.m2:f2}∙{dataIn.pT})={data.p1:f2}");

        body.AddParagraph()
            .AppendEquation($"M_Π=({dataIn.a1}/{data.beta:f2})∙({data.p1:f2}∙({data.T1:f2}+{data.ro:f2}∙{data.Kq:f2})-{data.p0:f2}∙{data.T2:f2})/(({data.T1:f2}+{data.ro:f2}∙{data.Kq:f2})∙({data.T3:f2}+{data.ro1:f2})-{data.T2:f2}^2)={data.MP:f2} (Н∙мм)/мм");

        body.AddParagraph()
            .AppendEquation($"Q_Π={dataIn.a1}∙({data.p0:f2}∙({data.T3:f2}+{data.ro1:f2})-{data.p1:f2}∙{data.T2:f2})/(({data.T1:f2}+{data.ro:f2}∙{data.Kq:f2})∙({data.T3:f2}+{data.ro1:f2})-{data.T2:f2}^2)={data.QP:f2} H");

        body.AddParagraph("Изгибающий момент и перерезывающие силы, распределенные по периметру перфорированной зоны решетки, вычисляют по формулам:");
        body.AddParagraph("- для изгибающего момента");
        body.AddParagraph()
            .AppendEquation($"M_a=M_Π+(a-a_1)∙Q_Π={data.MP:f2}+({data.a}-{dataIn.a1})={data.Ma:f2} (Н∙мм)/мм");

        body.AddParagraph("- для перерезывающей силы");
        body.AddParagraph()
            .AppendEquation($"Q_a={data.mn:f2}∙{data.QP:f2}={data.Qa:f2} H");

        body.AddParagraph("Осевую силу и изгибающий момент, действующие на трубу, вычисляют по формулам:");

        body.AddParagraph("- для осевой силы");
        body.AddParagraph()
            .AppendEquation("N_T=(π∙a_1)/i∙[(η_M∙p_M-η_T∙p_T)∙a_1+Φ_1∙Q_a+Φ_2∙β∙M_a]" +
                            $"=(π∙{dataIn.a1})/{dataIn.i}∙[({data.etaM:f2}∙{dataIn.pM}-{data.etaT:f2}∙{dataIn.pT})∙{dataIn.a1}+{data.Phi1:f2}∙{data.Qa:f2}+{data.Phi2:f2}∙{data.beta:f2}∙{data.Ma:f2}]={data.NT:f2} H");

        body.AddParagraph("- для изгибающего момента");
        body.AddParagraph()
            .AppendEquation("M_T=(E_T∙J_T∙β)/(K_y∙a_1∙l_пр)∙(Φ_2∙Q_a+Φ_3∙β∙M_a)");

        body.AddParagraph("где ")
            .AppendEquation("l_пр")
            .AddRun(dataIn.IsWithPartitions ? " для аппаратов с перегородками в кожухе" : " для аппаратов без перегородок в кожухе");

        body.AddParagraph()
            .AppendEquation(!dataIn.IsWithPartitions
                ? $"l_пр=l={dataIn.l} мм"
                : $"l_пр=L_1R/3={dataIn.l1R}/3={data.lpr:f2} мм");

        body.AddParagraph()
            .AppendEquation("J_T")
            .AddRun(" - момент инерции поперечного сечения трубы");

        body.AddParagraph()
            .AppendEquation($"J_T=(π∙d_T^4-(d_T-2∙s_T)^4)/64=(π∙{dataIn.dT}^4-({dataIn.dT}-2∙{dataIn.sT})^4)/64={data.JT:f2} мм^4");

        body.AddParagraph()
            .AppendEquation($"M_T=({data.ET}∙{data.JT:f2}∙{data.beta:f2})/({data.Ky:f2}∙{dataIn.a1}∙{data.lpr:f2})∙({data.Phi2:f2}∙{data.Qa:f2}+{data.Phi3:f2}∙{data.beta:f2}∙{data.Ma:f2})={data.MT:f2} (Н∙мм)/мм");

        body.AddParagraph("Нагрузки на кожух вычисляют по формулам:");

        body.AddParagraph("- усилие, распределенное по периметру кожуха");
        body.AddParagraph()
            .AppendEquation($"Q_K=a/2∙p_T-Q_Π={data.a}/2∙{dataIn.pT}-{data.QP:f2}={data.QK:f2} H");

        body.AddParagraph("- изгибающий момент, распределенный по периметру кожуха");
        body.AddParagraph()
            .AppendEquation("M_K=K_1/(ρ_1∙K_Φ∙β)∙(T_2∙Q_Π+T_3∙β∙M_Π)-p_M/(2∙β_1^2) +" +
                            $"{data.K1:f2}/({data.ro1:f2}∙{data.KF:f2}∙{data.beta:f2})∙({data.T2:f2}∙{data.QP:f2}+{data.T3:f2}∙{data.beta:f2}∙{data.MP:f2})-{dataIn.pM}/(2∙{data.beta1:f2}^2)={data.MK:f2} (Н∙мм)/мм");

        body.AddParagraph("- суммарная осевая сила, действующая на кожух");
        body.AddParagraph()
            .AppendEquation($"F=π∙D∙Q_K=π∙{dataIn.D}∙{data.QK:f2}={data.F:f2} H");

        body.AddParagraph();

        body.AddParagraph("Расчетные напряжения в элементах конструкции").Alignment(AlignmentType.Center);

        body.AddParagraph("Крепления трубной решетки к кожуху или фланцу");


        var tubePlateTypeNumber = ((int)dataIn.FirstTubePlate.TubePlateType).ToString();

        var connectionWithFlangeView = (byte[])(Resources.ResourceManager.GetObject("ConnToFlange" + tubePlateTypeNumber + "Dim")
                ?? throw new InvalidOperationException());

        mainPart.InsertImage(connectionWithFlangeView, ImagePartType.Gif);


        body.AddParagraph("Расчетные напряжения в трубных решетках").Alignment(AlignmentType.Center);

        body.AddParagraph("Напряжения в трубной решетке в месте соединения с кожухом вычисляют по формулам:");

        body.AddParagraph("- изгибные");
        body.AddParagraph()
            .AppendEquation($"σ_p1=(6∙|M_Π|)/(s_1p-c)^2=(6∙|{data.MP:f2}|)/({dataIn.FirstTubePlate.s1p}-{dataIn.FirstTubePlate.c})^2={data.sigmap1:f2} МПа");

        body.AddParagraph("- касательные");
        body.AddParagraph()
            .AppendEquation($"τ_p1=|Q_Π|/(s_1p-c)=|{data.QP:f2}|/({dataIn.FirstTubePlate.s1p}-{dataIn.FirstTubePlate.c})={data.taup1:f2} МПа");

        body.AddParagraph("Напряжения в перфорированной части трубной решетки вычисляют по формулам:");

        body.AddParagraph("- изгибные");
        body.AddParagraph()
            .AppendEquation("σ_p2=(6∙M_max)/(φ_p∙(s_1p-c)^2)");

        body.AddParagraph("где ")
            .AppendEquation("M_max")
            .AddRun(" максимальный расчетный изгибающий момент в перфорированной части трубной решетки");

        if (data.mA is >= -1.0 and <= 1.0)
        {
            body.AddParagraph("При");

            body.AddParagraph()
                .AppendEquation($"-1.0≤(β∙M_a)/Q_a=({data.beta:f2}∙{data.Ma:f2})/{data.Qa:f2}={data.mA:f2}≤1.0");

            body.AddParagraph()
                .AppendEquation("M_max=A∙|Q_a|/β");

            body.AddParagraph($"где A={data.A} - коэффициент, определяемый по таблице Г.2 ГОСТ 34233.7");

            body.AddParagraph()
                .AppendEquation($"M_max={data.A}∙|{data.Qa:f2}|/{data.beta:f2}={data.Mmax:f2} (Н∙мм)/мм");
        }
        else
        {
            body.AddParagraph("При");

            body.AddParagraph()
                .AppendEquation($"-(β∙M_a)/Q_a=({data.beta:f2}∙{data.Ma:f2})/{data.Qa:f2}={data.mA:f2}" +
                                (data.mA < -1.0 ? "<-1.0" : ">1.0"));

            body.AddParagraph()
                .AppendEquation("M_max=B∙|M_a|");

            body.AddParagraph($"где B={data.B} - коэффициент, определяемый по таблице Г.2 ГОСТ 34233.7");

            body.AddParagraph()
                .AppendEquation($"M_max={data.B}∙|{data.Ma:f2}|/{data.beta:f2}={data.Mmax:f2} (Н∙мм)/мм");
        }

        body.AddParagraph()
            .AppendEquation($"σ_p2=(6∙{data.Mmax:f2})/({data.fip:f2}∙({dataIn.FirstTubePlate.s1p}-{dataIn.FirstTubePlate.c})^2)={data.sigmap2:f2} МПа");

        body.AddParagraph("- касательные");
        body.AddParagraph()
            .AppendEquation($"τ_p2=|Q_a|/(φ_p∙(s_1p-c))=|{data.QP:f2}|/({data.fip:f2}∙({dataIn.FirstTubePlate.s1p}-{dataIn.FirstTubePlate.c}))={data.taup2:f2} МПа");

        body.AddParagraph("Напряжения в кожухе в месте присоединения к решетке вычисляют по формулам:");

        body.AddParagraph("- в меридиональном направлении:");

        body.AddParagraph("мембранные");
        body.AddParagraph()
            .AppendEquation($"σ_MX=|Q_K|/(s_1-c_K)=|{data.QK:f2}|/({dataIn.FirstTubePlate.s1}-{dataIn.cK})={data.sigmaMX:f2} МПа");

        body.AddParagraph("изгибные");
        body.AddParagraph()
            .AppendEquation($"σ_ИX=(6∙|M_K|)/(s_1-c_K)^2=(6∙|{data.MK:f2}|)/({dataIn.FirstTubePlate.s1}-{dataIn.cK})^2={data.sigmaIX:f2} МПа");

        body.AddParagraph("- в окружном направлении:");

        body.AddParagraph("мембранные");
        body.AddParagraph()
            .AppendEquation($"σ_Mφ=(|p_M|∙a)/(s_1-c_K)=(|{dataIn.pM}|∙{data.a})/({dataIn.FirstTubePlate.s1}-{dataIn.cK})={data.sigmaMfi:f2} МПа");

        body.AddParagraph("изгибные");
        body.AddParagraph()
            .AppendEquation($"σ_Иφ=0.3∙σ_ИX=0.3∙{data.sigmaIX:f2}={data.sigmaIfi:f2} МПа");

        body.AddParagraph("Напряжения в трубах вычисляют по формулам:");

        body.AddParagraph("- в осевом направлении:");

        body.AddParagraph("мембранные");
        body.AddParagraph()
            .AppendEquation($"σ_1T=|N_T|/(π∙(d_T-s_T)∙s_T)=|{data.NT:f2}|/(π∙({dataIn.dT}-{dataIn.sT})∙{dataIn.sT})={data.sigma1T:f2} МПа");

        body.AddParagraph("суммарные");
        body.AddParagraph()
            .AppendEquation($"σ_1=σ_1T+(d_T∙|M_T|)/(2∙J_T)={data.sigma1T:f2}+({dataIn.dT}∙|{data.MT:f2}|)/(2∙{data.JT:f2})={data.sigma1:f2} МПа");

        body.AddParagraph("- в окружном направлении");
        body.AddParagraph()
            .AppendEquation($"σ_2T=((d_T-s_T)∙max{{|p_T|;|p_M|;|p_T-p_M|}})/(2∙s_T)=(({dataIn.dT}-{dataIn.sT})∙max{{{dataIn.pT};{dataIn.pM};{Math.Abs(dataIn.pT - dataIn.pM)}}})/(2∙{dataIn.sT})={data.sigma2T:f2} МПа");

        body.AddParagraph();

        body.AddParagraph("Проверка прочности трубных решеток")
            .Alignment(AlignmentType.Center);

        body.AddParagraph("Проверку статической прочности проводят по формуле");

        body.AddParagraph()
            .AppendEquation("max{τ_p1;τ_p2}≤0.8∙[σ]_p");

        body.AddParagraph()
            .AppendEquation($"max{{{data.taup1:f2};{data.taup2:f2}}}≤0.8∙{data.sigma_dp}");

        body.AddParagraph()
            .AppendEquation($"{data.ConditionStaticStressForTubePlate1:f2}≤{data.ConditionStaticStressForTubePlate2}");

        if (data.IsConditionStaticStressForTubePlate)
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

        body.AddParagraph("Проверку трубной решетки на малоцикловую прочность проводят по ГОСТ 34233.6");

        body.AddParagraph("- в месте соединения с кожухом");

        body.AddParagraph()
            .AppendEquation($"Δσ_1=σ_p1={data.sigmap1:f2} МПа, Δσ_2=Δσ_3=0, K_σ={data.Ksigma}");

        MakeWordSigmaa(data.deltasigma1pk, data.deltasigma2pk, data.deltasigma3pk, data.Ksigma, data.sigmaa_dp, data.sigmaa_5_2_4k, ref body);

        body.AddParagraph("- в перфорированной части");

        body.AddParagraph()
            .AppendEquation($"Δσ_1=σ_p2={data.sigmap2:f2} МПа, Δσ_2=Δσ_3=0, K_σ=1");

        MakeWordSigmaa(data.deltasigma1pp, data.deltasigma2pp, data.deltasigma3pp, 1, data.sigmaa_dp, data.sigmaa_5_2_4p, ref body);

        if (dataIn is { IsOneGo: false, FirstTubePlate.IsWithGroove: true })
        {
            body.AddParagraph("Для многоходовых по трубному пространству теплообменных аппаратов прочность трубных решеток в зоне паза под перегородку проверяют по формуле");

            body.AddParagraph()
                .AppendEquation("s_n≥(s_p-c)∙max{[1-√(d_0/B_Π∙(t_Π/t_p-1))];√(φ_p)}+c");

            body.AddParagraph()
                .AppendEquation($"1-√(d_0/B_Π∙(t_Π/t_p-1))=1-√({dataIn.d0}/{dataIn.FirstTubePlate.BP}∙({dataIn.tP}/{dataIn.tp}-1))={data.snp1:f2}");

            body.AddParagraph()
                .AppendEquation($"√(φ_p)=√({data.fip:f2})={data.snp2:f2}");


            body.AddParagraph()
                .AppendEquation($"s_n=({dataIn.FirstTubePlate.sp}-{dataIn.FirstTubePlate.c})∙max{{{data.snp1:f2};{data.snp2:f2}}}+{dataIn.FirstTubePlate.c}={data.sn:f2} мм");

            if (data.sn >= dataIn.FirstTubePlate.sn)
            {
                body.AddParagraph("Принятая толщина ")
                    .Bold()
                    .AppendEquation($"s_n={dataIn.FirstTubePlate.sn} мм");
            }
            else
            {
                body.AddParagraph("Принятая толщина ")
                    .Bold()
                    .Color(System.Drawing.Color.Red)
                    .AppendEquation($"s_n={dataIn.FirstTubePlate.sn} мм");
            }
        }

        body.AddParagraph();

        if (dataIn.FirstTubePlate.IsNeedCheckHardnessTubePlate)
        {
            body.AddParagraph("Проверка жесткости трубных решеток").Alignment(AlignmentType.Center);

            body.AddParagraph("Проверку проводят, когда к жесткости трубных решеток предъявляются какие - либо дополнительные требования, например для аппаратов со стекающей пленкой, с перегородками по трубному пространству, если недопустим переток между ходами.");

            body.AddParagraph("Условие жесткости:");
            body.AddParagraph()
                .AppendEquation("W≤[W]");

            body.AddParagraph()
                .AppendEquation($"W=1.2/(K_y∙a_1)∙|T_1∙Q_Π+T_2∙β∙M_Π|=1.2/({data.Ky:f2}∙{dataIn.a1})∙|{data.T1:f2}∙{data.QP:f2}+{data.T2:f2}∙{data.beta:f2}∙{data.MP:f2}|={data.W:f2} мм");

            body.AddParagraph($"[W]={data.W_d} мм - допустимое значение прогиба трубной решетки по таблице 2 ГОСТ 34233.7");

            body.AddParagraph()
                .AppendEquation($"{data.W:f2} мм ≤{data.W_d} мм");

            if (data.W <= data.W_d)
            {
                body.AddParagraph("Условие жесткости выполняется")
                    .Bold();
            }
            else
            {
                body.AddParagraph("Условие жесткости не выполняется")
                    .Bold()
                    .Color(System.Drawing.Color.Red);
            }
        }

        if (dataIn.FirstTubePlate.TubePlateType != TubePlateType.WeldedInFlange &
            dataIn.SecondTubePlate.TubePlateType != TubePlateType.WeldedInFlange)
        {
            body.AddParagraph("Расчет прочности и устойчивости кожуха").Alignment(AlignmentType.Center);

            body.AddParagraph("Условие статической прочности кожуха в месте присоединения к решетке:");

            body.AddParagraph()
                .AppendEquation("σ_MX≤1.3∙[σ]_K");
            body.AddParagraph()
                .AppendEquation($"{data.sigmaMX:f2}≤1.3∙{data.sigma_dK}={data.ConditionStaticStressForShell2:f2}");

            if (data.IsConditionStaticStressForShell)
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

        //TODO: Make calculate for F<0

        body.AddParagraph("Проверку кожуха на малоцикловую прочность в месте присоединения к решетке проводят по ГОСТ 34233.6");

        body.AddParagraph()
            .AppendEquation($"Δσ_1=σ_MX+σ_ИX={data.sigmaMX:f2}+{data.sigmaIX:f2}={data.deltasigma1K:f2} МПа");
        body.AddParagraph()
            .AppendEquation($"Δσ_2=σ_Mφ+σ_Иφ={data.sigmaMfi:f2}+{data.sigmaIfi:f2}={data.deltasigma2K:f2} МПа");
        body.AddParagraph()
            .AppendEquation($"Δσ_3={data.deltasigma3K:f2} МПа");
        body.AddParagraph()
            .AppendEquation($"K_σ={data.Ksigma}");

        MakeWordSigmaa(data.deltasigma1K, data.deltasigma2K, data.deltasigma3K, data.Ksigma, data.sigmaa_dK, data.sigmaaK, ref body);

        body.AddParagraph();

        body.AddParagraph("Расчет труб на прочность, устойчивость и жесткость и расчет крепления труб в решетке")
            .Alignment(AlignmentType.Center);

        body.AddParagraph("Условие статической прочности труб:");

        body.AddParagraph()
            .AppendEquation("max{σ_1T;σ_2T}≤[σ]_T");
        body.AddParagraph()
            .AppendEquation($"max{{{data.sigma1T:f2};{data.sigma2T:f2}}}={data.ConditionStaticStressForTube1:f2}≤{data.sigma_dT}");

        if (data.IsConditionStaticStressForTube)
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

        body.AddParagraph("Проверку труб на малоцикловую прочность проводят по ГОСТ 34233.6");

        body.AddParagraph()
            .AppendEquation($"Δσ_1=σ_1={data.deltasigma1T:f2} МПа, Δσ_2=Δσ_3=0 МПа, K_σ=1");

        MakeWordSigmaa(data.deltasigma1T, data.deltasigma2T, data.deltasigma3T, data.Ksigma, data.sigmaa_dT, data.sigmaaT, ref body);

        body.AddParagraph("Проверку труб на устойчивость" + (data.NT < 0 ? " " : " не ") + "проводят, т.к. ")
            .AppendEquation($"N_T={data.NT:f2}<0");

        if (data.NT < 0)
        {
            body.AddParagraph("Условие устойчивости:");

            body.AddParagraph()
                .AppendEquation("σ_1T≤φ_T∙[σ]_T");

            body.AddParagraph("где ")
                .AppendEquation("φ_T")
                .AddRun(" — коэффициент уменьшения допускаемого напряжения при продольном изгибе");

            body.AddParagraph()
                .AppendEquation("φ_T=1/√(1+λ^4)");

            body.AddParagraph()
                .AppendEquation("λ=K_T∙√([σ]_T/E_T)∙l_R/((d_T-s_T))");

            body.AddParagraph("где ")
                .AppendEquation($"K_T={data.KT}")
                .AddRun(dataIn.IsWorkCondition ? " — для рабочих условий" : " - для условий гидроиспытания");

            if (dataIn.IsWithPartitions)
            {
                body.AddParagraph()
                    .AppendEquation($"l_R=max{{l_2R;0.7∙l_1R}}=max{{{dataIn.l2R};0.7∙{dataIn.l1R}}}={data.lR:f2} мм")
                    .AddRun(" — для аппаратов с перегородками");
            }
            else
            {
                body.AddParagraph()
                    .AppendEquation($"l_R=l={data.lR} мм")
                    .AddRun(" — для аппаратов без перегородок");
            }


            body.AddParagraph()
                .AppendEquation($"λ=K_T∙√([σ]_T/E_T)∙l_R/((d_T-s_T))={data.KT}∙√({data.sigma_dT}/{data.ET})∙{data.lR}/(({dataIn.dT}-{dataIn.sT}))={data.lambda:f2}");

            body.AddParagraph()
                .AppendEquation($"φ_T=1/√(1+{data.lambda:f2}^4)={data.phiT:f2}");

            body.AddParagraph()
                .AppendEquation($"{data.sigma1T:f2}≤{data.phiT:f2}∙{data.sigma_dT}={data.ConditionStabilityForTube2:f2}");

            if (data.IsConditionStabilityForTube)
            {
                body.AddParagraph("Условие устойчивости выполняется")
                    .Bold();
            }
            else
            {
                body.AddParagraph("Условие устойчивости не выполняется")
                    .Bold()
                    .Color(System.Drawing.Color.Red);
            }

            if (dataIn.IsNeedCheckHardnessTube)
            {
                body.AddParagraph();

                body.AddParagraph("Проверка жесткости труб").Alignment(AlignmentType.Center);

                body.AddParagraph("Проверку проводят, когда к жесткости труб предъявляют какие-либо дополнительные требования");

                body.AddParagraph("Прогиб трубы вычисляют по формуле");

                body.AddParagraph()
                    .AppendEquation("Y=A_y∙|M_T|/|N_T|");

                body.AddParagraph()
                    .AppendEquation("A_y=(1-cos√(λ_y))/(1-cos√(λ_y))");

                body.AddParagraph()
                    .AppendEquation($"λ_y=(|N_T|∙l_пр^2/(E_T∙J_T)=(|{data.NT:f2}|∙{data.lpr:f2}^2/({data.ET}∙{data.JT:f2})={data.lambday:f2}");

                body.AddParagraph()
                    .AppendEquation($"A_y=(1-cos√({data.lambday:f2}))/(1-cos√({data.lambday:f2}))={data.Ay:f2}");

                body.AddParagraph()
                    .AppendEquation($"Y={data.Ay:f2}∙|{data.MT:f2}|/|{data.NT:f2}|={data.Y:f2} мм");

                body.AddParagraph("о всех случаях прогиб трубы не должен превышать зазор между трубами в пучке и приводить к их соприкосновению.");
                //TODO: Check Y for compare with something
            }

        }

        body.AddParagraph();

        body.AddParagraph("Проверка прочности крепления трубы в решетке").Alignment(AlignmentType.Center);

        body.AddParagraph("Допускаемую нагрузку на соединение трубы с решеткой ")
            .AppendEquation("[N]_TP")
            .AddRun(" определяют на основании испытаний или по нормативным документам. При отсутствии данных о прочности вальцовочного соединения вычисляем значение по формуле");

        switch (dataIn.FirstTubePlate.TubeRolling)
        {
            case TubeRollingType.RollingWithoutGroove:
                body.AddParagraph("- для гладкозавальцованных труб");

                body.AddParagraph()
                    .AppendEquation($"[N]_TP=0.5∙π∙s_T∙(d_T-s_T)∙min{{l_B/d_T;1.6}}∙min{{[σ]_T;[σ]_p}}={data.Ntp_d:f2} H");
                break;
            case TubeRollingType.RollingWithOneGroove:
                body.AddParagraph("- для труб, завальцованных в пазы при наличии одного паза");

                body.AddParagraph()
                    .AppendEquation($"[N]_TP=0.6∙π∙s_T∙(d_T-s_T)∙min{{[σ]_T;[σ]_p}}={data.Ntp_d:f2} H");
                break;
            case TubeRollingType.RollingWithMoreThenOneGroove:
                body.AddParagraph("- для труб, завальцованных в пазы с двумя или более пазами");

                body.AddParagraph()
                    .AppendEquation($"[N]_TP=0.8∙π∙s_T∙(d_T-s_T)∙min{{[σ]_T;[σ]_p}}={data.Ntp_d:f2} H");
                break;
        }

        switch (dataIn.FirstTubePlate.FixTubeInTubePlate)
        {
            case FixTubeInTubePlateType.OnlyRolling:
                body.AddParagraph("Если трубы крепятся в решетке с помощью развальцовки, должно выполняться условие");

                body.AddParagraph()
                    .AppendEquation("|N_T|≤[N]_TP");

                body.AddParagraph()
                    .AppendEquation($"|{data.NT:f2}|≤{data.Ntp_d:f2}");

                break;

            case FixTubeInTubePlateType.OnlyWelding:
                body.AddParagraph("Если трубы крепятся к решетке способом приварки или приварки с подвальцовкой, должно выполняться условие");

                body.AddParagraph()
                    .AppendEquation("(|N_T|∙d_T+4∙M_T)/π∙d_T^2∙δ≤φ_C∙min{[σ]_T;[σ]_p}");

                body.AddParagraph()
                    .AppendEquation($"φ_C=min{{0.5;(0.95-0.2∙lgN)}}=min{{0.5;(0.95-0.2∙lg{dataIn.N})}}=min{{0.5;{data.phiC2:f2})}}={data.phiC:f2}");

                body.AddParagraph()
                    .AppendEquation($"(|N_T|∙d_T+4∙M_T)/π∙d_T^2∙δ≤φ_C∙min{{[σ]_T;[σ]_p}}=(|{data.NT:f2}|∙{dataIn.dT}+4∙{data.MT:f2})/π∙{dataIn.dT}^2∙{dataIn.FirstTubePlate.delta}={data.tau:f2}");

                body.AddParagraph()
                    .AppendEquation($"φ_C∙min{{[σ]_T;[σ]_p}}={data.phiC:f2}∙min{{{data.sigma_dT};{data.sigma_dp}}}={data.ConditionStressBracingTube2:f2}");

                body.AddParagraph()
                    .AppendEquation($"{data.tau:f2}≤{data.ConditionStressBracingTube2:f2}");

                break;

            case FixTubeInTubePlateType.RollingWithWelding:
                body.AddParagraph("В случае крепления труб к решетке способом развальцовки с обваркой должно выполняться условие");

                body.AddParagraph()
                    .AppendEquation("max{(φ_C∙min{[σ]_T;[σ]_p})/τ+0.6∙[N]_TP/|N_T|;[N]_TP/|N_T|}≥1");

                body.AddParagraph()
                    .AppendEquation("τ=(|N_T|∙d_T+4∙M_T)/π∙d_T^2∙δ≤φ_C∙min{[σ]_T;[σ]_p}");

                body.AddParagraph()
                    .AppendEquation($"φ_C=min{{0.5;(0.95-0.2∙lgN)}}=min{{0.5;(0.95-0.2∙lg{dataIn.N})}}=min{{0.5;{data.phiC2:f2}}}={data.phiC:f2}");

                body.AddParagraph()
                    .AppendEquation($"τ=(|{data.NT:f2}|∙{dataIn.dT}+4∙{data.MT:f2})/π∙{dataIn.dT}^2∙{dataIn.FirstTubePlate.delta}={data.tau:f2}");

                body.AddParagraph()
                    .AppendEquation($"(φ_C∙min{{[σ]_T;[σ]_p}})/τ+0.6∙[N]_TP/|N_T|=({data.phiC:f2}∙min{{{data.sigma_dT};{data.sigma_dp}}})/{data.tau:f2}+0.6∙{data.Ntp_d:f2}/|{data.NT:f2}|={data.ConditionStressBracingTube11:f2}");

                body.AddParagraph()
                    .AppendEquation($"[N]_TP/|N_T|={data.Ntp_d:f2}/|{data.NT:f2}|={data.ConditionStressBracingTube12:f2}");

                body.AddParagraph()
                    .AppendEquation($"max{{{data.ConditionStressBracingTube11:f2};{data.ConditionStressBracingTube12:f2}}}={Math.Max(data.ConditionStressBracingTube11, data.ConditionStressBracingTube12):f2}≥1");
                break;
        }

        if (data.IsConditionStressBracingTube)
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

        body.AddParagraph();
        body.AddParagraph("При наличии беструбной зоны принятая толщина трубной решетки должна дополнительно удовлетворять условию");

        body.AddParagraph()
            .AppendEquation("s_p≥0.5∙D_E∙√(p_p/[σ]_p)+c");

        body.AddParagraph("где ")
            .AppendEquation($"p_p={data.pp} МПа")
            .AddRun(" - расчетное давление, действующее на решетку кожухотрубчатого теплообменного аппарата. Принимается равным максимально возможному перепаду давлений, действую­щих на решетку");

        body.AddParagraph()
            .AppendEquation($"s_p=0.5∙{dataIn.DE}∙√({data.pp}/{data.sigma_dp})+{dataIn.FirstTubePlate.c}={data.sp_5_5_1:f2} мм");

        if (data.sp_5_5_1 >= dataIn.FirstTubePlate.sp)
        {
            body.AddParagraph("Принятая толщина ")
                .Bold()
                .AppendEquation($"s_p={dataIn.FirstTubePlate.sp} мм");
        }
        else
        {
            body.AddParagraph("Принятая толщина ")
                .Bold()
                .Color(System.Drawing.Color.Red)
                .AppendEquation($"s_p={dataIn.FirstTubePlate.sp} мм");
        }


        body.AddParagraph("Для трубных решеток, выполненных заодно с фланцем, принятая толщина должна быть не менее толщины кольца ответного фланца.Допускается уменьшение толщины решетки по сравнению с толщиной ответного фланца при условии подтверждения плотности и прочности фланцевого соединения специальным расчетом.");

        if (!dataIn.IsOneGo)
        {
            body.AddParagraph("Перегородки между ходами по трубному пространству кожухотрубчатых теплообменных аппаратов");

            body.AddParagraph("Толщина перегородки должна отвечать условию");
        }
    }

    private static void InsertInputDataTable(Body body, HeatExchangerStationaryTubePlatesInput dataIn, HeatExchangerStationaryTubePlatesCalculated data)
    {
        body.AddParagraph("Исходные данные").Alignment(AlignmentType.Center);

        body.AddParagraph();

        var table = body.AddTable();

        //table.AddRow().
        //  AddCell("");

        table.AddRow()
            .AddCell("Количество ходов")
            .AddCell(dataIn.IsOneGo ? "Одноходовой" : "Многоходовой");

        table.AddRowWithOneCell("Кожух");

        table.AddRow()
            .AddCell("Материал:")
            .AddCell($"{dataIn.SteelK}");

        table.AddRow()
            .AddCell("Внутренний диаметр кожуха, D:")
            .AddCell($"{dataIn.D}");

        table.AddRow()
            .AddCell("Толщина стенки кожуха, ")
            .AppendEquation("s_K")
            .AppendText(":")
            .AddCell($"{dataIn.sK} мм");

        table.AddRow()
            .AddCell("Расчетная прибавка к толщине стенки кожуха, ")
            .AppendEquation("c_K")
            .AppendText(":")
            .AddCell($"{dataIn.cK} мм");

        table.AddRow()
            .AddCell("Перегородки в межтрубном пространстве")
            .AddCell(dataIn.IsWithPartitions ? "Есть" : "Нет");

        if (dataIn.IsWithPartitions)
        {
            table.AddRow()
                .AddCell("Максимальный пролет трубы между решеткой и перегородкой, ")
                .AppendEquation("l_1R")
                .AppendText(":")
                .AddCell($"{dataIn.l1R} мм");

            table.AddRow()
                .AddCell("Максимальный пролет трубы между перегородками, ")
                .AppendEquation("l_2R")
                .AppendText(":")
                .AddCell($"{dataIn.l2R} мм");
        }

        table.AddRowWithOneCell("Трубная решетка");

        table.AddRow()
            .AddCell("Расстояние от оси кожуха до оси наиболее удаленной трубы, ")
            .AppendEquation("a_1")
            .AppendText(":")
            .AddCell($"{dataIn.a1} мм");

        table.AddRow()
            .AddCell("Диаметр окружности, вписанной в максимальную беструбную площадь, ")
            .AppendEquation("D_E")
            .AppendText(":")
            .AddCell($"{dataIn.DE} мм");

        table.AddRow()
            .AddCell("Диаметр отверстия в решетке, ")
            .AppendEquation("d_0")
            .AppendText(":")
            .AddCell($"{dataIn.d0} мм");

        table.AddRow()
            .AddCell("Шаг расположения отверстий в решетке, ")
            .AppendEquation("t_p")
            .AppendText(":")
            .AddCell($"{dataIn.tp} мм");

        if (dataIn.FirstTubePlate.IsWithGroove || dataIn.SecondTubePlate.IsWithGroove)
        {
            table.AddRow()
                .AddCell("Расстояние между осями рядов отверстий с двух сторон от паза, ")
                .AppendEquation("t_П")
                .AppendText(":")
                .AddCell($"{dataIn.tP} мм");
        }

        table.AddRow()
            .AddCell("Первая трубная решетка")
            .AddCell();

        table.AddRow()
            .AddCell("Материал:")
            .AddCell($"{dataIn.FirstTubePlate.Steelp}");

        table.AddRow()
            .AddCell("Тип")
            .AddCell(dataIn.FirstTubePlate.TubePlateType switch
            {
                TubePlateType.WeldedInShell => "Решетка вварена в кожух",
                TubePlateType.SimplyFlange => "Решетка-фланец приварена к кожуху",
                TubePlateType.FlangeWithFlanging => "Решетка-фланец с отбортовкой приварена к кожуху",
                TubePlateType.SimplyFlangeWithShell => "Решетка-фланец приварена к концевой обечайке",
                TubePlateType.WeldedInFlange => "Решетка вварена в фланец",
                TubePlateType.BetweenFlange => "Решетка вварена между фланцем и кожухом",
            });

        table.AddRow()
            .AddCell("Эффективный коэффициент концентрации напряжения, ")
            .AppendEquation("K_σ")
            .AppendText(":")
            .AddCell($"{data.Ksigma} мм");


        if (!dataIn.IsOneGo)
        {
            table.AddRow()
                .AddCell("Паз под перегородку в трубном пространстве")
                .AddCell(dataIn.FirstTubePlate.IsWithGroove ? "Есть" : "Нет");

            if (dataIn.FirstTubePlate.IsWithGroove)
            {
                table.AddRow()
                    .AddCell("Ширина канавки под прокладку в многоходовом аппарате, ")
                    .AppendEquation("B_П")
                    .AppendText(":")
                    .AddCell($"{dataIn.FirstTubePlate.BP} мм");

                table.AddRow()
                    .AddCell("Толщина трубной решетки в сечении канавки под перегородку, ")
                    .AppendEquation("s_n")
                    .AppendText(":")
                    .AddCell($"{dataIn.FirstTubePlate.sn} мм");
            }
        }

        table.AddRow()
            .AddCell("Толщина трубной решетки, ")
            .AppendEquation("s_p")
            .AppendText(":")
            .AddCell($"{dataIn.FirstTubePlate.sp} мм");

        table.AddRow()
            .AddCell("Расчетная прибавка к толщине трубной решетки, ")
            .AppendEquation("c")
            .AppendText(":")
            .AddCell($"{dataIn.FirstTubePlate.c} мм");

        table.AddRow()
            .AddCell()
            .AddCell("Фланец кожуха");

        if (dataIn.FirstTubePlate.TubePlateType is TubePlateType.WeldedInFlange or TubePlateType.BetweenFlange)
        {
            table.AddRow()
                .AddCell("Материал:")
                .AddCell($"{dataIn.FirstTubePlate.Steel1}");
        }

        table.AddRow()
            .AddCell("Наружный диаметр фланца, ")
            .AppendEquation("D_H")
            .AppendText(":")
            .AddCell($"{dataIn.FirstTubePlate.DH} мм");

        table.AddRow()
            .AddCell("Толщина тарелки фланца кожуха, ")
            .AppendEquation("h_1")
            .AppendText(":")
            .AddCell($"{dataIn.FirstTubePlate.h1} мм");

        table.AddRow()
            .AddCell("Толщина стенки кожуха в месте соединения с трубной решеткой, ")
            .AppendEquation("s_1")
            .AppendText(":")
            .AddCell($"{dataIn.FirstTubePlate.s1} мм");

        table.AddRow()
            .AddCell()
            .AddCell("Фланец камеры");

        table.AddRow()
            .AddCell("Материал фланца камеры:")
            .AddCell($"{dataIn.FirstTubePlate.Steel2}");

        table.AddRow()
            .AddCell("Толщина тарелки фланца камеры, ")
            .AppendEquation("h_2")
            .AppendText(":")
            .AddCell($"{dataIn.FirstTubePlate.h2} мм");

        table.AddRow()
            .AddCell("Толщина стенки камеры в месте соединения с трубной решеткой, ")
            .AppendEquation("s_2")
            .AppendText(":")
            .AddCell($"{dataIn.FirstTubePlate.s2} мм");

        table.AddRow()
            .AddCell("Материал стенки камеры:")
            .AddCell($"{dataIn.FirstTubePlate.SteelD}");

        table.AddRow()
            .AddCell("Проверка жесткости трубных решеток")
            .AddCell(dataIn.FirstTubePlate.IsNeedCheckHardnessTubePlate ? "Да" : "Нет");


        table.AddRowWithOneCell("Трубы");

        table.AddRow()
            .AddCell("Материал:")
            .AddCell($"{dataIn.SteelT}");

        table.AddRow()
            .AddCell("Наружный диаметр трубы, ")
            .AppendEquation("d_T")
            .AppendText(":")
            .AddCell($"{dataIn.dT} мм");

        table.AddRow()
            .AddCell("Толщина стенки трубы, ")
            .AppendEquation("s_T")
            .AppendText(":")
            .AddCell($"{dataIn.sT} мм");

        table.AddRow()
            .AddCell("Число труб, i:")
            .AddCell($"{dataIn.i} мм");

        table.AddRow()
            .AddCell("Половина длины трубы теплообменного аппарата, l:")
            .AddCell($"{dataIn.l} мм");

        table.AddRow()
            .AddCell("Тип крепления трубы в трубной решетке")
            .AddCell(dataIn.FirstTubePlate.FixTubeInTubePlate switch
            {
                FixTubeInTubePlateType.OnlyRolling => "Развальцовка",
                FixTubeInTubePlateType.OnlyWelding => "Сварка",
                FixTubeInTubePlateType.RollingWithWelding => "Развальцовка с обваркой",
            });

        if (dataIn.FirstTubePlate.FixTubeInTubePlate is FixTubeInTubePlateType.OnlyRolling or FixTubeInTubePlateType.RollingWithWelding)
        {
            table.AddRow()
                .AddCell("Вид развальцовки")
                .AddCell(dataIn.FirstTubePlate.TubeRolling switch
                {
                    TubeRollingType.RollingWithoutGroove => "Гладкозавальцованные трубы",
                    TubeRollingType.RollingWithOneGroove => "Трубы завальцованные в пазы (один паз)",
                    TubeRollingType.RollingWithMoreThenOneGroove => "Трубы завальцованные в пазы (два и более паза)",
                });
        }

        if (dataIn.FirstTubePlate.FixTubeInTubePlate is FixTubeInTubePlateType.OnlyRolling or FixTubeInTubePlateType.RollingWithWelding &&
            dataIn.FirstTubePlate.TubeRolling is TubeRollingType.RollingWithoutGroove or TubeRollingType.RollingWithOneGroove)
        {
            table.AddRow()
                .AddCell("Глубина развальцовки труб, ")
                .AppendEquation("l_B")
                .AppendText(":")
                .AddCell($"{dataIn.FirstTubePlate.lB} мм");
        }

        if (dataIn.FirstTubePlate.FixTubeInTubePlate is FixTubeInTubePlateType.OnlyWelding or FixTubeInTubePlateType.RollingWithWelding)
        {
            table.AddRow()
                .AddCell("Высота сварного шва в месте приварки трубы к решетке, ")
                .AppendEquation("δ")
                .AppendText(":")
                .AddCell($"{dataIn.FirstTubePlate.delta} мм");
        }

        table.AddRowWithOneCell("Условия нагружения");

        table.AddRow()
            .AddCell("Межтрубное пространство")
            .AddCell();

        table.AddRow()
            .AddCell("Расчетное давление, ")
            .AppendEquation("p_M")
            .AppendText(":")
            .AddCell($"{dataIn.pM} МПа");

        table.AddRow()
            .AddCell("Расчетна температура, ")
            .AppendEquation("T_K")
            .AppendText(":")
            .AddCell($"{dataIn.TK} °C");

        table.AddRow()
            .AddCell("Средняя температура, ")
            .AppendEquation("t_K")
            .AppendText(":")
            .AddCell($"{dataIn.tK} °C");

        table.AddRow()
            .AddCell("Трубное пространство")
            .AddCell();

        table.AddRow()
            .AddCell("Расчетное давление, ")
            .AppendEquation("p_T")
            .AppendText(":")
            .AddCell($"{dataIn.pT} МПа");

        table.AddRow()
            .AddCell("Расчетна температура, ")
            .AppendEquation("T_T")
            .AppendText(":")
            .AddCell($"{dataIn.TT} °C");

        table.AddRow()
            .AddCell("Средняя температура, ")
            .AppendEquation("t_T")
            .AppendText(":")
            .AddCell($"{dataIn.tT} °C");

        table.AddRow()
            .AddCell("Температура сборки аппарата, ")
            .AppendEquation("t_0")
            .AppendText(":")
            .AddCell($"{dataIn.t0} °C");

        table.AddRow()
            .AddCell("Число циклов нагружения за расчетный срок службы, N:")
            .AddCell($"{dataIn.N}");

        table.AddRowWithOneCell("Прочностные характеристики материалов");

        table.AddRow()
            .AddCell("Кожух")
            .AddCell();

        table.AddRow()
            .AddCell("Материал:")
            .AddCell($"{dataIn.SteelK}");

        table.AddRow()
            .AddCell("Допускаемое напряжение при расчетной температуре, ")
            .AppendEquation("[σ]_K")
            .AppendText(":")
            .AddCell($"{data.sigma_dK} МПа");

        table.AddRow()
            .AddCell("Модуль продольной упругости при расчетной температуре, ")
            .AppendEquation("E_K")
            .AppendText(":")
            .AddCell($"{data.EK} МПа");

        table.AddRow()
            .AddCell("Коэффициент линейного расширения, ")
            .AppendEquation("α_K")
            .AppendText(":")
            .AddCell($"{data.alfaK} МПа")
            .AppendEquation("°C^-1");

        table.AddRow()
            .AddCell("Допускаемая амплитуда напряжений, ")
            .AppendEquation("[σ_a]_K")
            .AppendText(":")
            .AddCell($"{data.sigmaa_dK:f2} МПа");

        table.AddRow()
            .AddCell("Трубы")
            .AddCell();

        table.AddRow()
            .AddCell("Материал:")
            .AddCell($"{dataIn.SteelT}");

        table.AddRow()
            .AddCell("Допускаемое напряжение при расчетной температуре, ")
            .AppendEquation("[σ]_T")
            .AppendText(":")
            .AddCell($"{data.sigma_dT} МПа");

        table.AddRow()
            .AddCell("Модуль продольной упругости при расчетной температуре, ")
            .AppendEquation("E_T")
            .AppendText(":")
            .AddCell($"{data.ET} МПа");

        table.AddRow()
            .AddCell("Коэффициент линейного расширения, ")
            .AppendEquation("α_T")
            .AppendText(":")
            .AddCell($"{data.alfaT} МПа")
            .AppendEquation("°C^-1");

        table.AddRow()
            .AddCell("Допускаемая амплитуда напряжений, ")
            .AppendEquation("[σ_a]_T")
            .AppendText(":")
            .AddCell($"{data.sigmaa_dT:f2} МПа");

        table.AddRow()
            .AddCell("Первая трубная решетка")
            .AddCell();

        table.AddRow()
            .AddCell("Материал:")
            .AddCell($"{dataIn.FirstTubePlate.Steelp}");

        table.AddRow()
            .AddCell("Допускаемое напряжение при расчетной температуре, ")
            .AppendEquation("[σ]_p")
            .AppendText(":")
            .AddCell($"{data.sigma_dp} МПа");

        table.AddRow()
            .AddCell("Модуль продольной упругости при расчетной температуре, ")
            .AppendEquation("E_p")
            .AppendText(":")
            .AddCell($"{data.Ep} МПа");

        table.AddRow()
            .AddCell("Допускаемая амплитуда напряжений, ")
            .AppendEquation("[σ_a]_p")
            .AppendText(":")
            .AddCell($"{data.sigmaa_dp:f2} МПа");

        table.AddRow()
            .AddCell()
            .AddCell("Фланец кожуха");

        table.AddRow()
            .AddCell("Материал:")
            .AddCell($"{dataIn.FirstTubePlate.Steel1}");

        table.AddRow()
            .AddCell("Модуль продольной упругости при расчетной температуре, ")
            .AppendEquation("E_1")
            .AppendText(":")
            .AddCell($"{data.E1} МПа");

        table.AddRow()
            .AddCell()
            .AddCell("Фланец камеры");

        table.AddRow()
            .AddCell("Материал:")
            .AddCell($"{dataIn.FirstTubePlate.Steel2}");

        table.AddRow()
            .AddCell("Модуль продольной упругости при расчетной температуре, ")
            .AppendEquation("E_2")
            .AppendText(":")
            .AddCell($"{data.E2} МПа");

        table.AddRow()
            .AddCell()
            .AddCell("Смежный элемент фланца камеры");

        table.AddRow()
            .AddCell("Материал:")
            .AddCell($"{dataIn.FirstTubePlate.SteelD}");

        table.AddRow()
            .AddCell("Модуль продольной упругости при расчетной температуре, ")
            .AppendEquation("E_D")
            .AppendText(":")
            .AddCell($"{data.ED} МПа");

        body.InsertTable(table);

    }

    private static void InsertImage(MainDocumentPart mainPart, HeatExchangerStationaryTubePlatesInput dataIn)
    {
        var firstTubePlateNumber = dataIn.FirstTubePlate.TubePlateType switch
        {
            TubePlateType.WeldedInShell => "1",
            TubePlateType.SimplyFlange => "2",
            TubePlateType.SimplyFlangeWithShell => "2",
            TubePlateType.FlangeWithFlanging => "2",
            TubePlateType.WeldedInFlange => "2",
            TubePlateType.BetweenFlange => "3",
            _ => "1",
        };

        var secondTubePlateNumber = dataIn.IsDifferentTubePlate
            ? dataIn.FirstTubePlate.TubePlateType switch
            {
                TubePlateType.WeldedInShell => "1",
                TubePlateType.SimplyFlange => "2",
                TubePlateType.SimplyFlangeWithShell => "2",
                TubePlateType.FlangeWithFlanging => "2",
                TubePlateType.WeldedInFlange => "2",
                TubePlateType.BetweenFlange => "3",
                _ => "1",
            } : firstTubePlateNumber;

        var compensatorType = dataIn.CompensatorType switch
        {
            CompensatorType.No => "",
            CompensatorType.Compensator => "_Syl",
            CompensatorType.Expander => "_Exp",
            CompensatorType.CompensatorOnExpander => "_Exp",
            _ => "",
        };

        var commonHeatExchangerView = (byte[])(Resources.ResourceManager
            .GetObject("Fixed_" + firstTubePlateNumber + "_" + secondTubePlateNumber + compensatorType)
            ?? throw new InvalidOperationException());

        mainPart.InsertImage(commonHeatExchangerView, ImagePartType.Gif);


        var type1 = ((int)dataIn.FirstTubePlate.TubePlateType).ToString();

        var type2 = dataIn.FirstTubePlate.IsChamberFlangeSkirt ? "_Butt" : "_Flat";

        var type3 = ((int)dataIn.FirstTubePlate.FlangeFace + 1).ToString();

        var connectionWithFlangeView = (byte[])(Resources.ResourceManager.GetObject("ConnToFlange" + type1 + type2 + type3)
            ?? throw new InvalidOperationException());

        mainPart.InsertImage(connectionWithFlangeView, ImagePartType.Gif);


        var fixType = string.Empty;

        fixType = dataIn.FirstTubePlate.FixTubeInTubePlate switch
        {
            FixTubeInTubePlateType.OnlyRolling => dataIn.FirstTubePlate.TubeRolling switch
            {
                TubeRollingType.RollingWithoutGroove => "1",
                TubeRollingType.RollingWithOneGroove => "2_52857",
                TubeRollingType.RollingWithMoreThenOneGroove => "3_52857",
                _ => fixType
            },
            FixTubeInTubePlateType.OnlyWelding => "4",
            FixTubeInTubePlateType.RollingWithWelding => dataIn.FirstTubePlate.TubeRolling switch
            {
                TubeRollingType.RollingWithoutGroove => "1Weld",
                TubeRollingType.RollingWithOneGroove => "2Weld_52857",
                TubeRollingType.RollingWithMoreThenOneGroove => "3Weld_52857",
                _ => fixType
            },
            _ => throw new ArgumentOutOfRangeException()
        };

        var tubeFixView = (byte[])(Resources.ResourceManager.GetObject("FixType" + fixType)
                                   ?? throw new InvalidOperationException());

        mainPart.InsertImage(tubeFixView, ImagePartType.Gif);
    }

    private static void InsertHeader(Body body, HeatExchangerStationaryTubePlatesInput dataIn)
    {
        body.AddParagraph($"Расчет на прочность теплообменного аппарата с неподвижными трубными решетками {dataIn.Name}")
            .Heading(HeadingType.Heading1)
            .Alignment(AlignmentType.Center);
    }

    private static void MakeWordSigmaa(double ds1, double ds2, double ds3, double Ks, double sigmaa_d, double sigmaa, ref Body body)
    {
        body.AddParagraph("Амплитуду напряжений для каждого цикла вычисляют по формуле");
        body.AddParagraph()
            .AppendEquation("σ_a=K_σ/2∙max{|Δσ_1-Δσ_2|;|Δσ_2-Δσ_3|;|Δσ_1-Δσ_3|}"
                            + $"={Ks}/2∙max{{|{ds1:f2}-{ds2:f2}|;|{ds2:f2}-{ds3:f2}|;|{ds1:f2}-{ds3:f2}|}}" +
                            $"={Ks}/2∙max{{|{ds1 - ds2:f2}|;|{ds2 - ds3:f2}|;|{ds1 - ds3:f2}|}}={sigmaa:f2}");

        body.AddParagraph().AppendEquation("[σ_a]≥σ_a");
        body.AddParagraph()
            .AppendEquation($"{sigmaa_d:f2}≥{sigmaa:f2}");
        if (sigmaa_d > sigmaa)
        {
            body.AddParagraph("Условие малоцикловой прочности выполняется")
                .Bold();
        }
        else
        {
            body.AddParagraph("Условие малоцикловой прочности не выполняется")
                .Bold()
                .Color(System.Drawing.Color.Red);
        }
    }
}