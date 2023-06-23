using CalculateVessels.Core.Enums;
using CalculateVessels.Core.Helpers;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Interfaces;
using System;

namespace CalculateVessels.Core.Elements.Supports.Saddle;

internal class SaddleCalculateService : ICalculateService<SaddleInput>
{
    private readonly IPhysicalDataService _physicalData;
    public SaddleCalculateService(IPhysicalDataService physicalData)
    {
        _physicalData = physicalData;
        Name = "GOST 34233.5-2017";
    }

    public string Name { get; }

    public ICalculatedElement Calculate(SaddleInput dataIn)
    {
        var data = new SaddleCalculated
        {
            InputData = dataIn,
            SigmaAllow = PhysicalHelper.GetSigmaIfZero(dataIn.SigmaAllow, dataIn.Steel, dataIn.t, _physicalData),
            E = PhysicalHelper.GetEIfZero(dataIn.E, dataIn.Steel, dataIn.t, _physicalData),
            ny = dataIn.IsAssembly ? 1.8 : 2.4
        };

        if (dataIn.IsPressureIn)
        {
            data.p_d = 2 * data.SigmaAllow * dataIn.fi * (dataIn.s - dataIn.c) /
                       (dataIn.D + (dataIn.s - dataIn.c));
        }
        else
        {
            data.B1_2 = 9.45 * (dataIn.D / dataIn.L) *
                        Math.Sqrt(dataIn.D / (100 * (dataIn.s - dataIn.c)));
            data.B1 = Math.Min(1, data.B1_2);
            var p_de = 0.0000208 * data.E / (data.ny * data.B1) * (dataIn.D / dataIn.L) *
                       Math.Pow(100 * (dataIn.s - dataIn.c) / dataIn.D, 2.5);
            var p_dp = 2 * data.SigmaAllow * (dataIn.s - dataIn.c) /
                       (dataIn.D + (dataIn.s - dataIn.c));
            data.p_d = p_dp / Math.Sqrt(1 + Math.Pow(p_dp / p_de, 2));
        }

        //UNDONE: проверить формулу для расчета [F]
        if (true)
        {
            data.F_d = Math.PI * (dataIn.D + (dataIn.s - dataIn.c)) *
                       (dataIn.s - dataIn.c) * data.SigmaAllow * dataIn.fi;
        }
        else
        {
            var F_dp = Math.PI * (dataIn.D + (dataIn.s - dataIn.c)) *
                       (dataIn.s - dataIn.c) * data.SigmaAllow;
            var F_de = 0.00031 * data.E / data.ny * Math.Pow(dataIn.D, 2) *
                       Math.Pow(100 * (dataIn.s - dataIn.c) / dataIn.D, 2.5);
            if (dataIn.L > dataIn.D * 10)
            {

            }
            data.F_d = F_dp / Math.Sqrt(1 + Math.Pow(F_dp / F_de, 2));
        }

        data.M_dp = Math.PI / 4.0 * dataIn.D * (dataIn.D + (dataIn.s - dataIn.c)) *
                    (dataIn.s - dataIn.c) * data.SigmaAllow;
        data.M_de = 0.000089 * data.E / data.ny * Math.Pow(dataIn.D, 3) *
                    Math.Pow(100 * (dataIn.s - dataIn.c) / dataIn.D, 2.5);

        data.M_d = data.M_dp / Math.Sqrt(1 + Math.Pow(data.M_dp / data.M_de, 2));

        data.Q_dp = 0.25 * data.SigmaAllow * Math.PI * dataIn.D * (dataIn.s - dataIn.c);

        data.Q_de = 2.4 * data.E * Math.Pow(dataIn.s - dataIn.c, 2) / data.ny *
                    (0.18 + 3.3 * dataIn.D * (dataIn.s - dataIn.c) / Math.Pow(dataIn.L, 2));

        data.Q_d = data.Q_dp / Math.Sqrt(1 + Math.Pow(data.Q_dp / data.Q_de, 2));



        data.q = dataIn.G / (dataIn.L + 4.0 / 3.0 * dataIn.H);
        data.M0 = data.q * (Math.Pow(dataIn.D, 2) / 16.0);
        // UNDONE: Make calculate for non symmetrical saddle
        data.F1 = dataIn.G / 2.0;
        data.F2 = data.F1;
        data.M1 = data.q * Math.Pow(dataIn.e, 2) / 2.0 - data.M0;
        data.M2 = data.M1;
        data.M12 = data.M0 + data.F1 * (dataIn.L / 2.0 - dataIn.a) -
                   data.q / 2.0 * Math.Pow(dataIn.L / 2.0 + 2.0 / 3.0 * dataIn.H, 2);
        data.Q1 = (dataIn.L - 2.0 * dataIn.a) * data.F1 /
                  (dataIn.L + 4.0 / 3.0 * dataIn.H);
        data.Q2 = data.Q1;
        if (data.M12 > data.M1)
        {
            data.y = dataIn.D / (dataIn.s - dataIn.c);
            data.x = dataIn.L / dataIn.D;
            data.K9_1 = 1.6 - 0.20924 * (data.x - 1) + 0.028702 * data.x * (data.x - 1) + 0.0004795 * data.y * (data.x - 1) -
                        0.0000002391 * data.x * data.y * (data.x - 1) - 0.0029936 * (data.x - 1) * Math.Pow(data.x, 2) -
                        0.00000085692 * (data.x - 1) * Math.Pow(data.y, 2) + 0.00000088174 * Math.Pow(data.x, 2) * (data.x - 1) * data.y -
                        0.0000000075955 * Math.Pow(data.y, 2) * (data.x - 1) * data.x + 0.000082748 * (data.x - 1) * Math.Pow(data.x, 3) +
                        0.00000000048168 * (data.x - 1) * Math.Pow(data.y, 3);
            data.K9 = Math.Max(data.K9_1, 1);
            data.ConditionStrength1_1 = dataIn.p * dataIn.D /
                                        (4 * (dataIn.s - dataIn.c)) +
                                        4 * data.M12 * data.K9 / (Math.PI * Math.Pow(dataIn.D, 2) * (dataIn.s - dataIn.c));
            // UNDONE: if phi<1 check condition
            // Если расстояние между расчетным сечением и ближайшим кольцевым сварным швом более Sqr(D*s),
            //то в формуле(41) принимают(р равным 1

            data.ConditionStrength1_2 = data.SigmaAllow * dataIn.fi;
            if (data.ConditionStrength1_1 > data.ConditionStrength1_2)
            {
                data.ErrorList.Add("Несущая способность обечайки в сечении между опорами. Условие прочности не выполняется.");
            }
            data.ConditionStability1 =
                dataIn.IsPressureIn ? Math.Abs(data.M12) / data.M_d :
                    dataIn.p / data.p_d + Math.Abs(data.M12) / data.M_d;
            if (data.ConditionStability1 > 1)
            {
                data.ErrorList.Add("Несущая способность обечайки в сечении между опорами. Условие устойчивости не выполняется");
            }
        }
        switch (dataIn.SaddleType)
        {
            case SaddleType.SaddleWithoutRingWithoutSheet:
                data.gamma = 2.83 * (dataIn.a / dataIn.D) *
                             Math.Sqrt((dataIn.s - dataIn.c) / dataIn.D);
                data.beta1 = 0.91 * dataIn.b /
                             Math.Sqrt(dataIn.D * (dataIn.s - dataIn.c));
                data.K10_1 = Math.Exp(-data.beta1) * Math.Sin(data.beta1) / data.beta1;
                data.K10 = Math.Max(data.K10_1, 0.25);
                data.K11 = (1.0 - Math.Exp(-data.beta1) * Math.Cos(data.beta1)) / data.beta1;
                data.K12 = (1.15 - 0.1432 * MathHelper.DegreeToRadian(dataIn.delta1)) /
                           Math.Sin(0.5 * MathHelper.DegreeToRadian(dataIn.delta1));
                data.K13 = Math.Max(1.7 - 2.1 * MathHelper.DegreeToRadian(dataIn.delta1) / Math.PI, 0) /
                           Math.Sin(0.5 * MathHelper.DegreeToRadian(dataIn.delta1));
                data.K14 = (1.45 - 0.43 * MathHelper.DegreeToRadian(dataIn.delta1)) /
                           Math.Sin(0.5 * MathHelper.DegreeToRadian(dataIn.delta1));
                data.K15_2 = (0.8 * Math.Sqrt(data.gamma) + 6 * data.gamma) / MathHelper.DegreeToRadian(dataIn.delta1);
                data.K15 = Math.Min(1, data.K15_2);
                data.K16 = 1.0 - 0.65 / (1 + Math.Pow(6 * data.gamma, 2)) *
                    Math.Sqrt(Math.PI / (3.0 * MathHelper.DegreeToRadian(dataIn.delta1)));
                data.K17 = 1.0 / (1.0 + 0.6 *
                    Math.Pow(dataIn.D / (dataIn.s - dataIn.c), 1.0 / 3.0) *
                    (dataIn.b / dataIn.D) * MathHelper.DegreeToRadian(dataIn.delta1));
                data.sigma_mx = 4 * data.M1 /
                                (Math.PI * Math.Pow(dataIn.D, 2) * (dataIn.s - dataIn.c));

                data.v1_2 = -0.23 * data.K13 * data.K15 / (data.K12 * data.K10);
                data.v1_3 = -0.53 * data.K11 /
                            (data.K14 * data.K16 * data.K17 * Math.Sin(0.5 * MathHelper.DegreeToRadian(dataIn.delta1)));

                data.K2 = dataIn.IsAssembly ? 1.05 : 1.25;

                data.v21_2 = -data.sigma_mx / (data.K2 * data.SigmaAllow);
                data.v21_3 = 0;

                data.v22_2 = (dataIn.p * dataIn.D /
                              (4 * (dataIn.s - dataIn.c)) -
                              data.sigma_mx) / (data.K2 * data.SigmaAllow);
                data.v22_3 = dataIn.p * dataIn.D /
                              (2 * (dataIn.s - dataIn.c)) /
                             (data.K2 * data.SigmaAllow);

                data.K1_2For_v21 = K1(data.v1_2, data.v21_2);
                data.K1_2For_v22 = K1(data.v1_2, data.v22_2);
                data.K1_2 = Math.Min(data.K1_2For_v21, data.K1_2For_v22);

                data.K1_3For_v21 = K1(data.v1_3, data.v21_3);
                data.K1_3For_v22 = K1(data.v1_3, data.v22_3);
                data.K1_3 = Math.Min(data.K1_3For_v21, data.K1_3For_v22);

                data.sigmai2_1 = data.K1_2For_v21 * data.K2 * data.SigmaAllow;
                data.sigmai2_2 = data.K1_2For_v22 * data.K2 * data.SigmaAllow;
                data.sigmai2 = Math.Min(data.sigmai2_1, data.sigmai2_2);

                data.sigmai3_1 = data.K1_3For_v21 * data.K2 * data.SigmaAllow;
                data.sigmai3_2 = data.K1_3For_v22 * data.K2 * data.SigmaAllow;
                data.sigmai3 = Math.Min(data.sigmai3_1, data.sigmai3_2);

                data.F_d2 = 0.7 * data.sigmai2 * (dataIn.s - dataIn.c) *
                    Math.Sqrt(dataIn.D * (dataIn.s - dataIn.c)) / (data.K10 * data.K12);
                data.F_d3 = 0.9 * data.sigmai3 * (dataIn.s - dataIn.c) *
                    Math.Sqrt(dataIn.D * (dataIn.s - dataIn.c)) / (data.K14 * data.K16 * data.K17);

                data.ConditionStrength2 = Math.Min(data.F_d2, data.F_d3);

                if (data.F1 > data.ConditionStrength2)
                {
                    data.ErrorList.Add("Несущая способность обечайки, не укрепленной кольцами жесткости в области опорного узла. Условие прочности не выполняется");
                }


                data.Fe = data.F1 * (Math.PI / 4.0) * data.K13 * data.K15 *
                          Math.Sqrt(dataIn.D / (dataIn.s - dataIn.c));

                data.ConditionStability2 = dataIn.IsPressureIn
                    ? Math.Abs(data.M1) / data.M_d + data.Fe / data.F_d + Math.Pow(data.Q1 / data.Q_d, 2)
                    : dataIn.p / data.p_d + Math.Abs(data.M1) / data.M_d + data.Fe / data.F_d + Math.Pow(data.Q1 / data.Q_d, 2);
                if (data.ConditionStability2 > 1)
                {
                    data.ErrorList.Add("Несущая способность обечайки, не укрепленной кольцами жесткости в области опорного узла. Условие устойчивости не выполняется");
                }
                break;
            case SaddleType.SaddleWithoutRingWithSheet:

                data.sef = (dataIn.s - dataIn.c) *
                           Math.Sqrt(1 + Math.Pow(dataIn.s2 / (dataIn.s - dataIn.c), 2));
                data.gamma = 2.83 * (dataIn.a / dataIn.D) *
                             Math.Sqrt(data.sef / dataIn.D);
                data.beta1 = 0.91 * dataIn.b2 / Math.Sqrt(dataIn.D * data.sef);
                data.K10_1 = Math.Exp(-data.beta1) * Math.Sin(data.beta1) / data.beta1;
                data.K10 = Math.Max(data.K10_1, 0.25);
                data.K11 = (1 - Math.Exp(-data.beta1) * Math.Cos(data.beta1)) / data.beta1;
                data.K12 = (1.15 - 0.1432 * MathHelper.DegreeToRadian(dataIn.delta2)) /
                           Math.Sin(0.5 * MathHelper.DegreeToRadian(dataIn.delta2));
                data.K13 = Math.Max(1.7 - 2.1 * MathHelper.DegreeToRadian(dataIn.delta2) / Math.PI, 0) /
                           Math.Sin(0.5 * MathHelper.DegreeToRadian(dataIn.delta2));
                data.K14 = (1.45 - 0.43 * MathHelper.DegreeToRadian(dataIn.delta2)) /
                           Math.Sin(0.5 * MathHelper.DegreeToRadian(dataIn.delta2));
                data.K15_2 = (0.8 * Math.Sqrt(data.gamma) + 6 * data.gamma) / MathHelper.DegreeToRadian(dataIn.delta2);
                data.K15 = Math.Min(1, data.K15_2);
                data.K16 = 1 - 0.65 / (1 + Math.Pow(6 * data.gamma, 2)) *
                    Math.Sqrt(Math.PI / (3 * MathHelper.DegreeToRadian(dataIn.delta2)));
                data.K17 = 1.0 / (1.0 + 0.6 * Math.Pow(dataIn.D / data.sef, 1.0 / 3.0) *
                    (dataIn.b2 / dataIn.D) * MathHelper.DegreeToRadian(dataIn.delta2));
                data.sigma_mx = 4 * data.M1 / (Math.PI * Math.Pow(dataIn.D, 2) * data.sef);

                data.v1_2 = -0.23 * data.K13 * data.K15 / (data.K12 * data.K10);
                data.v1_3 = -0.53 * data.K11 / (data.K14 * data.K16 * data.K17 * Math.Sin(0.5 * MathHelper.DegreeToRadian(dataIn.delta2)));

                data.K2 = dataIn.IsAssembly ? 1.05 : 1.25;

                data.v21_2 = -data.sigma_mx / (data.K2 * data.SigmaAllow);
                data.v21_3 = 0;

                data.v22_2 = (dataIn.p * dataIn.D / (4 * data.sef) - data.sigma_mx) /
                             (data.K2 * data.SigmaAllow);
                data.v22_3 = dataIn.p * dataIn.D / (2 * data.sef) /
                             (data.K2 * data.SigmaAllow);

                data.K1_2For_v21 = K1(data.v1_2, data.v21_2);
                data.K1_2For_v22 = K1(data.v1_2, data.v22_2);
                data.K1_2 = Math.Min(data.K1_2For_v21, data.K1_2For_v22);

                data.K1_3For_v21 = K1(data.v1_3, data.v21_3);
                data.K1_3For_v22 = K1(data.v1_3, data.v22_3);
                data.K1_3 = Math.Min(data.K1_3For_v21, data.K1_3For_v22);

                data.sigmai2_1 = data.K1_2For_v21 * data.K2 * data.SigmaAllow;
                data.sigmai2_2 = data.K1_2For_v22 * data.K2 * data.SigmaAllow;
                data.sigmai2 = Math.Min(data.sigmai2_1, data.sigmai2_2);

                data.sigmai3_1 = data.K1_3For_v21 * data.K2 * data.SigmaAllow;
                data.sigmai3_2 = data.K1_3For_v22 * data.K2 * data.SigmaAllow;
                data.sigmai3 = Math.Min(data.sigmai3_1, data.sigmai3_2);

                data.F_d2 = 0.7 * data.sigmai2 * data.sef * Math.Sqrt(dataIn.D * data.sef) / (data.K10 * data.K12);
                data.F_d3 = 0.9 * data.sigmai3 * data.sef * Math.Sqrt(dataIn.D * data.sef) / (data.K14 * data.K16 * data.K17);

                data.ConditionStrength2 = Math.Min(data.F_d2, data.F_d3);

                if (data.F1 > data.ConditionStrength2)
                {
                    data.ErrorList.Add("Несущая способность обечайки, не укрепленной кольцами жесткости в области опорного узла. Условие прочности не выполняется.");
                }

                data.Fe = data.F1 * (Math.PI / 4.0) * data.K13 * data.K15 * Math.Sqrt(dataIn.D / data.sef);

                data.ConditionStability2 = dataIn.IsPressureIn
                    ? Math.Abs(data.M1) / data.M_d + data.Fe / data.F_d + Math.Pow(data.Q1 / data.Q_d, 2)
                    : dataIn.p / data.p_d + Math.Abs(data.M1) / data.M_d + data.Fe / data.F_d + Math.Pow(data.Q1 / data.Q_d, 2);
                if (data.ConditionStability2 > 1)
                {
                    data.ErrorList.Add("Несущая способность обечайки, не укрепленной кольцами жесткости в области опорного узла с подкладным листом. Условие устойчивости не выполняется.");
                }
                break;
            case SaddleType.SaddleWithRingInNoRibs:
            case SaddleType.SaddleWithRingIn1Rib:
            case SaddleType.SaddleWithRingIn3Rib:
            case SaddleType.SaddleWithRingOutRib:
                //TODO: опора с укрепляющим кольцом
                break;
        }

        data.IsConditionUseFormulas = true;

        if (dataIn.delta1 is <= 60 or >= 180)
        {
            data.IsConditionUseFormulas = false;
            data.ErrorList.Add("delta1 должно быть в пределах 60-180.");
        }
        if ((dataIn.s - dataIn.c) / dataIn.D > 0.05)
        {
            data.IsConditionUseFormulas = false;
            data.ErrorList.Add("Условие применения формул не выполняется.");
        }

        switch (dataIn.SaddleType)
        {
            case SaddleType.SaddleWithoutRingWithSheet:
                if (dataIn.delta2 < dataIn.delta1 + 20)
                {
                    data.IsConditionUseFormulas = false;
                    data.ErrorList.Add("Угол обхвата подкладного листа должен быть delta2>=delta1+20.");
                }

                if (dataIn.s2 < dataIn.s)
                {
                    data.IsConditionUseFormulas = false;
                    data.ErrorList.Add("Толщина подкладного листа должна быть s2>=s.");
                }

                break;
            case SaddleType.SaddleWithRingInNoRibs:
            case SaddleType.SaddleWithRingIn1Rib:
            case SaddleType.SaddleWithRingIn3Rib:
            case SaddleType.SaddleWithRingOutRib:
                data.Ak = (dataIn.s - dataIn.c) * Math.Sqrt(dataIn.D * (dataIn.s - dataIn.c));
                if (dataIn.Ak < data.Ak)
                {
                    data.IsConditionUseFormulas = false;
                    data.ErrorList.Add("Условие применения формул не выполняется.");
                }

                break;
        }

        return data;
    }

    private static double K1(double v1, double v2) =>
        (1 - Math.Pow(v2, 2)) /
        (1.0 / 3.0 + v1 * v2 + Math.Sqrt(Math.Pow(1.0 / 3.0 + v1 * v2, 2) +
                                         (1 - Math.Pow(v2, 2)) * Math.Pow(v1, 2)));
}