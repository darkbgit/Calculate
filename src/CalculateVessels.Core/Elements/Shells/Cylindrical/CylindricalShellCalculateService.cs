using System;
using System.Linq;
using CalculateVessels.Core.Elements.Base;
using CalculateVessels.Core.Enums;
using CalculateVessels.Core.Exceptions;
using CalculateVessels.Core.Helpers;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Interfaces;

namespace CalculateVessels.Core.Elements.Shells.Cylindrical;

internal class CylindricalShellCalculateService : ICalculateService<CylindricalShellInput>
{
    private readonly IPhysicalDataService _physicalData;
    public CylindricalShellCalculateService(IPhysicalDataService physicalData)
    {
        _physicalData = physicalData;
        Name = "GOST 34233.2-2017";
    }

    public string Name { get; }

    public ICalculatedElement Calculate(CylindricalShellInput dataIn)
    {
        var commonData = CalculateCommonData(dataIn);
        var results = dataIn.LoadingConditions
            .Select(lc => CalculateOneLoadingCondition(dataIn, commonData, lc))
            .ToList();

        var data = new CylindricalShellCalculated(commonData, results)
        {
            InputData = dataIn,
            //CommonData = commonData,
            //Results = dataIn.LoadingConditions
            //    .Select(lc => CalculateOneLoadingCondition(dataIn, commonData, lc))
            //    .ToList()
        };

        if (data.Results.Any(r => r.ErrorList.Any()))
        {
            data.AddErrors(data.Results.SelectMany(r => r.ErrorList));
        }

        return data;
    }

    private static CylindricalShellCalculatedCommon CalculateCommonData(CylindricalShellInput dataIn)
    {
        var data = new CylindricalShellCalculatedCommon
        {
            c = dataIn.c1 + dataIn.c2 + dataIn.c3
        };

        // Condition use formulas
        const int diameterBigLittleBorder = 200;
        const double conditionUseFormalsBigDiameter = 0.1;
        const double conditionUseFormalsLittleDiameter = 0.3;
        data.IsConditionUseFormulas = diameterBigLittleBorder < dataIn.D
            ? (dataIn.s - data.c) / dataIn.D <= conditionUseFormalsBigDiameter
            : (dataIn.s - data.c) / dataIn.D <= conditionUseFormalsLittleDiameter;

        if (!data.IsConditionUseFormulas)
        {
            data.ErrorList.Add("Условие применения формул не выполняется.");
        }

        return data;
    }

    private CylindricalShellCalculatedOneLoading CalculateOneLoadingCondition(CylindricalShellInput dataIn, CylindricalShellCalculatedCommon cdc, LoadingCondition loadingCondition)
    {
        var data = new CylindricalShellCalculatedOneLoading
        {
            LoadingConditionId = loadingCondition.Id,
            SigmaAllow = PhysicalHelper.GetSigmaIfZero(loadingCondition.SigmaAllow, dataIn.Steel, loadingCondition.t, _physicalData),
            E = PhysicalHelper.GetEIfZero(loadingCondition.EAllow, dataIn.Steel, loadingCondition.t, _physicalData),
        };

        if (loadingCondition.p > 0)
        {
            switch (loadingCondition.PressureType)
            {
                case PressureType.Inside:
                    data.s_p = loadingCondition.p * dataIn.D / (2 * data.SigmaAllow * dataIn.phi - loadingCondition.p);
                    data.s = data.s_p + cdc.c;

                    if (dataIn.s != 0.0)
                    {
                        if (data.s > dataIn.s)
                            throw new CalculateException("Принятая толщина меньше расчетной.", loadingCondition);

                        data.p_d = 2 * data.SigmaAllow * dataIn.phi * (dataIn.s - cdc.c) /
                                   (dataIn.D + dataIn.s - cdc.c);
                    }

                    break;
                case PressureType.Outside:
                    data.l = dataIn.l + dataIn.l3;
                    data.b_2 = 0.47 * Math.Pow(loadingCondition.p / (0.00001 * data.E), 0.067) * Math.Pow(data.l / dataIn.D, 0.4);
                    data.b = Math.Max(1.0, data.b_2);
                    data.s_p_1 = 1.06 * (0.01 * dataIn.D / data.b) *
                                 Math.Pow(loadingCondition.p / (0.00001 * data.E) * (data.l / dataIn.D), 0.4);
                    data.s_p_2 = 1.2 * loadingCondition.p * dataIn.D / (2 * data.SigmaAllow - loadingCondition.p);
                    data.s_p = Math.Max(data.s_p_1, data.s_p_2);
                    data.s = data.s_p + cdc.c;

                    if (dataIn.s != 0.0)
                    {
                        if (data.s > dataIn.s)
                            throw new CalculateException("Принятая толщина меньше расчетной.", loadingCondition);


                        data.p_dp = 2 * data.SigmaAllow * (dataIn.s - cdc.c) /
                                    (dataIn.D + dataIn.s - cdc.c);
                        data.B1_2 = 9.45 * (dataIn.D / data.l) *
                                    Math.Sqrt(dataIn.D / (100 * (dataIn.s - cdc.c)));
                        data.B1 = Math.Min(1.0, data.B1_2);
                        data.p_de = 2.08 * 0.00001 * data.E / (dataIn.ny * data.B1) * (dataIn.D / data.l) *
                                    Math.Pow(100 * (dataIn.s - cdc.c) / dataIn.D, 2.5);
                        data.p_d = data.p_dp / Math.Sqrt(1 + Math.Pow(data.p_dp / data.p_de, 2));
                    }
                    break;
            }

            if (data.p_d < loadingCondition.p && dataIn.s != 0)
            {
                data.ErrorList.Add("[p] меньше p");
            }
        }

        if (dataIn.s == 0)
            return data;

        if (dataIn.F > 0)
        {
            if (dataIn.IsFTensile)
            {
                data.s_pf = dataIn.F / (Math.PI * dataIn.D * data.SigmaAllow * dataIn.fi_t);
                data.s_f = data.s_pf + cdc.c;

                if (data.s_f > dataIn.s)
                    throw new CalculateException("Принятая толщина меньше расчетной от нагрузки осевым сжимающим усилием.", loadingCondition);

                data.FAllow = Math.PI * (dataIn.D + dataIn.s - cdc.c) *
                              (dataIn.s - cdc.c) * data.SigmaAllow * dataIn.fi_t;
            }
            else
            {
                data.F_dp = Math.PI * (dataIn.D + dataIn.s - cdc.c) * (dataIn.s - cdc.c) * data.SigmaAllow;
                data.F_de1 = 0.000031 * data.E / dataIn.ny * Math.Pow(dataIn.D, 2) *
                             Math.Pow(100 * (dataIn.s - cdc.c) / dataIn.D, 2.5);

                const int lMoreThenD = 10;
                var isLMoreThenD = dataIn.l / dataIn.D > lMoreThenD;

                if (isLMoreThenD || dataIn.ConditionForCalcF5341)
                {
                    switch (dataIn.FCalcSchema)
                    {
                        case 1:
                            data.lpr = dataIn.l;
                            break;
                        case 2:
                            data.lpr = 2 * dataIn.l;
                            break;
                        case 3:
                            data.lpr = 0.7 * dataIn.l;
                            break;
                        case 4:
                            data.lpr = 0.5 * dataIn.l;
                            break;
                        case 5:
                            data.F = dataIn.q * dataIn.l;
                            data.lpr = 1.12 * dataIn.l;
                            break;
                        case 6:
                            double fDivl6 = dataIn.f / dataIn.l;
                            fDivl6 *= 10;
                            fDivl6 = Math.Round(fDivl6 / 2.0);
                            fDivl6 *= 0.2;
                            data.lpr = fDivl6 switch
                            {
                                0 => 2 * dataIn.l,
                                0.2 => 1.73 * dataIn.l,
                                0.4 => 1.47 * dataIn.l,
                                0.6 => 1.23 * dataIn.l,
                                0.8 => 1.06 * dataIn.l,
                                1 => dataIn.l,
                                _ => data.lpr
                            };
                            break;
                        case 7:
                            double fDivl7 = dataIn.f / dataIn.l;
                            fDivl7 *= 10;
                            fDivl7 = Math.Round(fDivl7 / 2);
                            fDivl7 *= 0.2;
                            data.lpr = fDivl7 switch
                            {
                                0 => 2 * dataIn.l,
                                0.2 => 1.7 * dataIn.l,
                                0.4 => 1.4 * dataIn.l,
                                0.6 => 1.11 * dataIn.l,
                                0.8 => 0.85 * dataIn.l,
                                1 => 0.7 * dataIn.l,
                                _ => data.lpr
                            };
                            break;

                    }

                    data.lambda = 2.83 * data.lpr / (dataIn.D + dataIn.s - cdc.c);
                    data.F_de2 = Math.PI * (dataIn.D + dataIn.s - cdc.c) * (dataIn.s - cdc.c) * data.E /
                                 dataIn.ny *
                                 Math.Pow(Math.PI / data.lambda, 2);
                    data.F_de = Math.Min(data.F_de1, data.F_de2);
                }
                else
                {
                    data.F_de = data.F_de1;
                }

                data.FAllow = data.F_dp / Math.Sqrt(1 + Math.Pow(data.F_dp / data.F_de, 2));
            }
        }

        if (dataIn.M > 0)
        {
            data.M_dp = Math.PI / 4 * dataIn.D * (dataIn.D + dataIn.s - cdc.c) * (dataIn.s - cdc.c) *
                        data.SigmaAllow;
            data.M_de = 0.000089 * data.E / dataIn.ny * Math.Pow(dataIn.D, 3) *
                        Math.Pow(100 * (dataIn.s - cdc.c) / dataIn.D, 2.5);
            data.M_d = data.M_dp / Math.Sqrt(1 + Math.Pow(data.M_dp / data.M_de, 2));
        }

        if (dataIn.Q > 0)
        {
            data.Q_dp = 0.25 * data.SigmaAllow * Math.PI * dataIn.D * (dataIn.s - cdc.c);
            data.Q_de = 2.4 * data.E * Math.Pow(dataIn.s - cdc.c, 2) / dataIn.ny *
                        (0.18 + 3.3 * dataIn.D * (dataIn.s - cdc.c) / Math.Pow(dataIn.l, 2));
            data.Q_d = data.Q_dp / Math.Sqrt(1 + Math.Pow(data.Q_dp / data.Q_de, 2));
        }

        data.ConditionStability = loadingCondition.p / data.p_d +
                                  (dataIn.F > 0 ? (dataIn.FCalcSchema == 5 ? data.F : dataIn.F) / data.FAllow : 0) +
                                  (dataIn.M > 0 ? dataIn.M / data.M_d : 0) +
                                  (dataIn.Q > 0 ? Math.Pow(dataIn.Q / data.Q_d, 2) : 0);
        if (data.ConditionStability > 1)
        {
            data.ErrorList.Add("Условие устойчивости для совместного действия усилий не выполняется.");
        }

        return data;
    }
}
