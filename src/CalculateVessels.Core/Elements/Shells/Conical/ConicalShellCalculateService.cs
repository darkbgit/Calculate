using CalculateVessels.Core.Elements.Base;
using CalculateVessels.Core.Elements.Shells.Enums;
using CalculateVessels.Core.Exceptions;
using CalculateVessels.Core.Helpers;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Interfaces;
using System;
using System.Linq;

namespace CalculateVessels.Core.Elements.Shells.Conical;

internal class ConicalShellCalculateService : ICalculateService<ConicalShellInput>
{
    private readonly IPhysicalDataService _physicalData;

    public ConicalShellCalculateService(IPhysicalDataService physicalData)
    {
        _physicalData = physicalData;
        Name = "GOST 34233.2-2017";
    }

    public string Name { get; }

    public ICalculatedElement Calculate(ConicalShellInput dataIn)
    {
        var commonData = CalculateCommonData(dataIn);
        var results = dataIn.LoadingConditions
            .Select(lc => CalculateOneLoadingCondition(dataIn, commonData, lc))
            .ToList();

        var data = new ConicalShellCalculated(commonData, results)
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

    private ConicalShellCalculatedOneLoading CalculateOneLoadingCondition(ConicalShellInput dataIn,
        ConicalShellCalculatedCommon cdc,
        LoadingCondition loadingCondition)
    {
        var data = new ConicalShellCalculatedOneLoading
        {
            LoadingCondition = loadingCondition,
            SigmaAllow =
                PhysicalHelper.GetSigmaIfZero(dataIn.SigmaAllow, dataIn.Steel, loadingCondition.t, _physicalData),
            E = PhysicalHelper.GetEIfZero(loadingCondition.EAllow, dataIn.Steel, loadingCondition.t, _physicalData),
        };

        switch (dataIn.ConnectionType)
        {
            case ConicalConnectionType.WithoutConnection:
                break;
            case ConicalConnectionType.Simply:
                data.SigmaAllow1Big = PhysicalHelper.GetSigmaIfZero(dataIn.SigmaAllow1Big, dataIn.Steel1Big,
                    loadingCondition.t, _physicalData);
                data.SigmaAllow2Big = PhysicalHelper.GetSigmaIfZero(dataIn.SigmaAllow2Big, dataIn.Steel2Big,
                    loadingCondition.t, _physicalData);
                break;
            case ConicalConnectionType.WithRingPicture25b:
            case ConicalConnectionType.WithRingPicture29:
                data.SigmaAllow1Big = PhysicalHelper.GetSigmaIfZero(dataIn.SigmaAllow1Big, dataIn.Steel1Big,
                    loadingCondition.t, _physicalData);
                data.SigmaAllow2Big = PhysicalHelper.GetSigmaIfZero(dataIn.SigmaAllow2Big, dataIn.Steel2Big,
                    loadingCondition.t, _physicalData);
                data.SigmaAllowC = PhysicalHelper.GetSigmaIfZero(dataIn.SigmaAllowC, dataIn.SteelC, loadingCondition.t,
                    _physicalData);
                break;
            case ConicalConnectionType.Toroidal:
                data.SigmaAllowT = PhysicalHelper.GetSigmaIfZero(dataIn.SigmaAllowT, dataIn.SteelT, loadingCondition.t,
                    _physicalData);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (dataIn.IsConnectionWithLittle)
        {
            data.SigmaAllow1Little = PhysicalHelper.GetSigmaIfZero(dataIn.SigmaAllow1Little, dataIn.Steel1Little,
                loadingCondition.t, _physicalData);
            data.SigmaAllow2Little = PhysicalHelper.GetSigmaIfZero(dataIn.SigmaAllow2Little, dataIn.Steel2Little,
                loadingCondition.t, _physicalData);
        }

        if (loadingCondition.p > 0)
        {
            // Condition use formals
            const double conditionUseFormulasFrom = 0.001;
            const double conditionUseFormulasTo = 0.05;
            const int angleCondition = 70;

            data.ConditionUseFormulas = dataIn.s * Math.Cos(cdc.alpha1) / dataIn.D;

            data.IsConditionUseFormulas =
                data.ConditionUseFormulas is >= conditionUseFormulasFrom and <= conditionUseFormulasTo &&
                MathHelper.RadianToDegree(cdc.alpha1) <= angleCondition;

            if (!data.IsConditionUseFormulas)
            {
                data.ErrorList.Add("Условие применения формул не выполняется.");
            }

            if (loadingCondition.IsPressureIn)
            {
                data.s_p = loadingCondition.p * cdc.Dk / (2 * data.SigmaAllow * dataIn.phi - loadingCondition.p)
                           * (1 / Math.Cos(cdc.alpha1));
                data.s = data.s_p + cdc.c;

                if (dataIn.s != 0)
                {
                    if (data.s > dataIn.s)
                        throw new CalculateException("Принятая толщина меньше расчетной.");

                    data.p_d = 2 * data.SigmaAllow * dataIn.phi * (dataIn.s - cdc.c)
                               / (cdc.Dk / Math.Cos(cdc.alpha1) + (dataIn.s - cdc.c));
                }
            }
            else
            {
                data.lE = (dataIn.D - dataIn.D1) / (2 * Math.Sin(cdc.alpha1));
                data.DE_1 = (dataIn.D + dataIn.D1) / (2 * Math.Cos(cdc.alpha1));
                data.DE_2 = dataIn.D / Math.Cos(cdc.alpha1) - 0.3 * (dataIn.D + dataIn.D1)
                                                                   * Math.Sqrt((dataIn.D + dataIn.D1) *
                                                                               Math.Tan(cdc.alpha1) /
                                                                               ((dataIn.s - cdc.c) * 100));
                data.DE = Math.Max(data.DE_1, data.DE_2);
                data.B1_1 = 9.45 * data.DE / data.lE * Math.Sqrt(data.DE / (100 * (dataIn.s - cdc.c)));
                data.B1 = Math.Min(1.0, data.B1_1);

                data.s_p_1 = 1.06 * (0.01 * data.DE / data.B1)
                                  * Math.Pow(loadingCondition.p / (0.00001 * data.E) * (data.lE / data.DE), 0.4);
                data.s_p_2 = 1.2 * loadingCondition.p * cdc.Dk /
                             (2 * dataIn.phi * data.SigmaAllow - loadingCondition.p)
                             * (1 / Math.Cos(cdc.alpha1));
                data.s_p = Math.Max(data.s_p_1, data.s_p_2);
                data.s = data.s_p + cdc.c;

                if (dataIn.s != 0)
                {
                    if (data.s > dataIn.s)
                        throw new CalculateException("Принятая толщина меньше расчетной.");

                    data.p_dp = 2 * data.SigmaAllow * (dataIn.s - cdc.c)
                                / (cdc.Dk / Math.Cos(cdc.alpha1) + dataIn.s - cdc.c);
                    data.p_de = 2.08 * 0.00001 * data.E / (dataIn.ny * data.B1) * (data.DE / data.lE)
                        * Math.Pow(100 * (dataIn.s - cdc.c) / data.DE, 2.5);
                    data.p_d = data.p_dp / Math.Sqrt(1 + Math.Pow(data.p_dp / data.p_de, 2));
                }
            }

            if (data.p_d < loadingCondition.p && dataIn.s != 0)
            {
                data.ErrorList.Add("[p] меньше p");
            }

            //big connection
            switch (dataIn.ConnectionType)
            {
                case ConicalConnectionType.WithoutConnection:
                    break;
                case ConicalConnectionType.Simply:
                    data.IsConditionUseFormulasBigConnection = dataIn.s1Big - cdc.c >= dataIn.s2Big - cdc.c &&
                                                               MathHelper.RadianToDegree(cdc.alpha1) <= 70;
                    if (!data.IsConditionUseFormulasBigConnection)
                    {
                        data.ErrorList.Add(
                            "Узел соединения с обечайкой большего диаметра. Условие применения формул не выполняется");
                    }

                    data.chi_1Big = data.SigmaAllow1Big / data.SigmaAllow2Big;
                    data.beta = 0.4 * Math.Sqrt(dataIn.D / (dataIn.s2Big - cdc.c)) * Math.Tan(cdc.alpha1)
                        / (1 + Math.Sqrt((1 + data.chi_1Big
                                             * Math.Pow((dataIn.s1Big - cdc.c) / (dataIn.s2Big - cdc.c), 2))
                                         / (2 * Math.Cos(cdc.alpha1)) * data.chi_1Big * (dataIn.s1Big - cdc.c) /
                                         (dataIn.s2Big - cdc.c))) - 0.25;
                    data.beta_1 = Math.Max(0.5, data.beta);
                    data.s_2pBig = loadingCondition.p * dataIn.D * data.beta_1
                                   / (2 * data.SigmaAllow2Big * dataIn.phi - loadingCondition.p);

                    data.s_2Big = data.s_2pBig + cdc.c;

                    if (data.s_2Big > dataIn.s2Big)
                    {
                        data.ErrorList.Add(
                            "Узел соединения с обечайкой большего диаметра. Принятая толщина стенки обечайки меньше расчетной.");
                    }

                    data.s_1Big = (dataIn.s1Big - cdc.c) / (dataIn.s2Big - cdc.c) * data.s_2pBig + cdc.c;

                    if (data.s_1Big > dataIn.s1Big)
                    {
                        data.ErrorList.Add(
                            "Узел соединения с обечайкой большего диаметра. Принятая толщина стенки конуса меньше расчетной.");
                    }

                    data.p_dBig = 2 * data.SigmaAllow2Big * dataIn.phi * (dataIn.s2Big - cdc.c)
                                  / (dataIn.D * data.beta_1 + (dataIn.s2Big - cdc.c));

                    break;

                case ConicalConnectionType.WithRingPicture25b:
                    data.IsConditionUseFormulasBigConnection = dataIn.s1Big - cdc.c >= dataIn.s2Big - cdc.c &&
                                                               MathHelper.RadianToDegree(cdc.alpha1) <= 70;
                    if (!data.IsConditionUseFormulasBigConnection)
                    {
                        data.ErrorList.Add(
                            "Узел соединения с обечайкой большего диаметра. Условие применения формул не выполняется");
                    }

                    data.chi_1Big = data.SigmaAllow1Big / data.SigmaAllow2Big;
                    data.beta = 0.4 * Math.Sqrt(dataIn.D / (dataIn.s2Big - cdc.c)) * Math.Tan(cdc.alpha1)
                        / (1 + Math.Sqrt((1 + data.chi_1Big
                                             * Math.Pow((dataIn.s1Big - cdc.c) / (dataIn.s2Big - cdc.c), 2))
                                         / (2 * Math.Cos(cdc.alpha1)) * data.chi_1Big * (dataIn.s1Big - cdc.c) /
                                         (dataIn.s2Big - cdc.c))) - 0.25;
                    data.beta_a = (2 * data.SigmaAllow2Big * dataIn.phi / loadingCondition.p - 1) *
                        (dataIn.s2Big - cdc.c) / dataIn.D;
                    data.Ak = loadingCondition.p * Math.Pow(dataIn.D, 2) * Math.Tan(cdc.alpha1) /
                              (8 * data.SigmaAllowC * dataIn.phi_k)
                              * (1 - (data.beta_a + 0.25) / (data.beta + 0.25));
                    if (data.Ak > 0 && data.Ak < dataIn.Ak)
                    {
                        data.ErrorList.Add(
                            "Узел соединения с обечайкой большего диаметра. Площадь укрепляющего кольца недостаточна.");
                    }

                    data.B2 = 1.6 * data.Ak / ((dataIn.s2Big - cdc.c) * Math.Sqrt(dataIn.D * (dataIn.s2Big - cdc.c)))
                        * data.SigmaAllowC * dataIn.phi_k / (data.SigmaAllow2Big * dataIn.phi_t);
                    data.B3 = 0.25;
                    data.beta_0 = 0.4 * Math.Sqrt(dataIn.D / (dataIn.s2Big - cdc.c)) * Math.Tan(cdc.alpha1) -
                                  data.B3 *
                                  (1 + Math.Sqrt((1 + data.chi_1Big *
                                                     Math.Pow((dataIn.s1Big - cdc.c) / (dataIn.s2Big - cdc.c), 2)) /
                                                 (2 * Math.Cos(cdc.alpha1)) * data.chi_1Big * (dataIn.s1Big - cdc.c) /
                                                 (dataIn.s2Big - cdc.c))) /
                                  (data.B2 + (1 + Math.Sqrt(
                                      (1 + data.chi_1Big * Math.Pow((dataIn.s1Big - cdc.c) / (dataIn.s2Big - cdc.c),
                                          2)) /
                                      (2 * Math.Cos(cdc.alpha1)) * data.chi_1Big * (dataIn.s1Big - cdc.c) /
                                      (dataIn.s2Big - cdc.c))));
                    data.beta_2 = Math.Max(0.5, data.beta_0);
                    data.p_dBig = 2 * data.SigmaAllow2Big * dataIn.phi * (dataIn.s2Big - cdc.c) /
                                  (dataIn.D * data.beta_2 + (dataIn.s2Big - cdc.c));

                    break;
                case ConicalConnectionType.WithRingPicture29:
                    data.IsConditionUseFormulasBigConnection = MathHelper.RadianToDegree(cdc.alpha1) <= 70;
                    if (!data.IsConditionUseFormulasBigConnection)
                    {
                        data.ErrorList.Add(
                            "Узел соединения с обечайкой большего диаметра. Условие применения формул не выполняется");
                    }

                    data.Ak = loadingCondition.p * Math.Pow(dataIn.D, 2) * Math.Tan(cdc.alpha1) /
                              (8 * data.SigmaAllowC * dataIn.phi_k);


                    if (data.Ak > 0 && data.Ak < dataIn.Ak)
                    {
                        data.ErrorList.Add(
                            "Узел соединения с обечайкой большего диаметра. Площадь укрепляющего кольца недостаточна.");
                    }

                    data.p_dBig = data.Ak * 8 * data.SigmaAllowC * dataIn.phi_k /
                                  (Math.Pow(dataIn.D, 2) * Math.Tan(cdc.alpha1));

                    break;
                case ConicalConnectionType.Toroidal:
                    data.ConditionUseFormulasToroidal = dataIn.r / dataIn.D;
                    data.IsConditionUseFormulasBigConnection = data.ConditionUseFormulasToroidal is >= 0.0 and < 0.3 &&
                                                               MathHelper.RadianToDegree(cdc.alpha1) <= 70;
                    if (!data.IsConditionUseFormulasBigConnection)
                    {
                        data.ErrorList.Add(
                            "Узел соединения с обечайкой большего диаметра. Условие применения формул не выполняется.");
                    }

                    data.beta = 0.4 * Math.Sqrt(dataIn.D / (dataIn.s2Big - cdc.c)) * Math.Tan(cdc.alpha1)
                        / (1 + Math.Sqrt(1 / Math.Cos(cdc.alpha1))) - 0.25;
                    data.beta_t = 1 / (1 + 0.028 * MathHelper.RadianToDegree(cdc.alpha1) * dataIn.r / dataIn.D *
                                            Math.Sqrt(dataIn.D / (dataIn.sT - cdc.c)) /
                        (1 / Math.Sqrt(Math.Cos(cdc.alpha1)) + 1));
                    //TODO: Check alpha1 in beta_t in degree or in radians
                    data.beta_3_2 = data.beta * data.beta_t;
                    data.beta_3 = Math.Max(0.5, data.beta_3_2);
                    data.s_tp = loadingCondition.p * dataIn.D * data.beta_3 /
                                (2 * dataIn.phi * data.SigmaAllowT - loadingCondition.p);
                    data.s_t = data.s_tp + cdc.c;

                    if (data.s_t > dataIn.sT)
                    {
                        data.ErrorList.Add(
                            "Узел соединения с обечайкой большего диаметра. Принятая толщина перехода меньше расчетной.");
                    }

                    data.p_dBig = 2 * data.SigmaAllowT * dataIn.phi * (dataIn.sT - cdc.c) /
                                  (dataIn.D * data.beta_3 + (dataIn.sT - cdc.c));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (dataIn.ConnectionType != ConicalConnectionType.WithoutConnection && data.p_dBig < loadingCondition.p &&
                dataIn.s != 0)
            {
                data.ErrorList.Add("Узел соединения с обечайкой большего диаметра. [p] для переходной части меньше p.");
            }

            //little connection
            if (dataIn.IsConnectionWithLittle)
            {
                data.IsConditionUseFormulasLittleConnection = MathHelper.RadianToDegree(cdc.alpha1) <= 70;
                if (!data.IsConditionUseFormulasLittleConnection)
                {
                    data.ErrorList.Add(
                        "Узел соединения с обечайкой меньшего диаметра. Условие применения формул не выполняется.");
                }

                data.chi_1Little = data.SigmaAllow1Little / data.SigmaAllow2Little;
                data.ConditionForBetaH = Math.Pow((dataIn.s1Little - cdc.c) / (dataIn.s2Little - cdc.c), 2);
                if (data.ConditionForBetaH >= 1)
                {
                    data.betaLittle = 0.4 * Math.Sqrt(dataIn.D1 / (dataIn.s2Little - cdc.c)) * Math.Tan(cdc.alpha1)
                        / (1 + Math.Sqrt((1 + data.chi_1Little
                                             * Math.Pow((dataIn.s1Little - cdc.c) / (dataIn.s2Little - cdc.c), 2))
                                         / (2 * Math.Cos(cdc.alpha1)) * data.chi_1Little * (dataIn.s1Little - cdc.c) /
                                         (dataIn.s2Little - cdc.c))) - 0.25;
                    data.beta_H = data.betaLittle + 0.75;
                }
                else
                {
                    data.beta_H = 0.4 * Math.Sqrt(dataIn.D1 / (dataIn.s2Little - cdc.c)) * Math.Tan(cdc.alpha1)
                        / (data.chi_1Little * (dataIn.s1Little - cdc.c) / (dataIn.s2Little - cdc.c) * Math.Sqrt(
                            (dataIn.s1Little - cdc.c) /
                            ((dataIn.s2Little - cdc.c) * Math.Cos(cdc.alpha1))) + Math.Sqrt((1 + data.chi_1Little
                                * Math.Pow((dataIn.s1Little - cdc.c) / (dataIn.s2Little - cdc.c), 2))
                            / 2)) + 0.5;
                }

                data.beta_4 = Math.Max(1, data.beta_H);
                data.s_2pLittle = loadingCondition.p * dataIn.D1 * data.beta_4 /
                                  (2 * dataIn.phi * data.SigmaAllow2Little - loadingCondition.p);
                data.s_2Little = data.s_2pLittle + cdc.c;

                if (data.s_2Little > dataIn.s2Little)
                {
                    data.ErrorList.Add(
                        "Узел соединения с обечайкой меньшего диаметра. Принятая толщина обечайки меньше расчетной.");
                }

                data.s_1Little = (dataIn.s1Little - cdc.c) / (dataIn.s2Little - cdc.c) * data.s_2pLittle + cdc.c;

                if (data.s_1Little > dataIn.s1Little)
                {
                    data.ErrorList.Add(
                        "Узел соединения с обечайкой меньшего диаметра. Принятая толщина стенки конуса меньше расчетной.");
                }

                data.p_dLittle = 2 * data.SigmaAllow2Little * dataIn.phi * (dataIn.s2Little - cdc.c) /
                                 (dataIn.D1 * data.beta_4 + (dataIn.s2Little - cdc.c));

                if (data.p_dLittle < loadingCondition.p && dataIn.s != 0)
                {
                    data.ErrorList.Add(
                        "Узел соединения с обечайкой меньшего диаметра. [p] для переходной части меньше p.");
                }
            }
        }

        return data;
    }

    private static ConicalShellCalculatedCommon CalculateCommonData(ConicalShellInput dataIn)
    {
        var data = new ConicalShellCalculatedCommon
        {
            c = dataIn.c1 + dataIn.c2 + dataIn.c3,
            alpha1 = Math.Atan((dataIn.D / 2 - dataIn.D1 / 2) / dataIn.L)
        };

        switch (dataIn.ConnectionType)
        {
            case ConicalConnectionType.Simply:
            case ConicalConnectionType.WithRingPicture25b:
            case ConicalConnectionType.WithRingPicture29:
                data.a1p = 0.7 * Math.Sqrt(dataIn.D * (dataIn.s1Big - data.c) / Math.Cos(data.alpha1));
                data.a2p = 0.7 * Math.Sqrt(dataIn.D * (dataIn.s2Big - data.c));
                break;
            case ConicalConnectionType.Toroidal:
                data.a1p = 0.7 * Math.Sqrt(dataIn.D * (dataIn.sT - data.c) / Math.Cos(data.alpha1));
                data.a2p = 0.5 * Math.Sqrt(dataIn.D * (dataIn.sT - data.c));
                break;
            case ConicalConnectionType.WithoutConnection:
                data.a1p = 0.7 * Math.Sqrt(dataIn.D * (dataIn.s - data.c) / Math.Cos(data.alpha1));
                break;
            default:
                throw new CalculateException("Conical connection type error.");
        }

        if (dataIn.IsConnectionWithLittle)
        {
            data.a1p_l = Math.Sqrt(dataIn.D1 * (dataIn.s1Little - data.c) / Math.Cos(data.alpha1));
            data.a2p_l = 1.25 * Math.Sqrt(dataIn.D1 * (dataIn.s2Little - data.c));
        }

        data.Dk = dataIn.ConnectionType == ConicalConnectionType.Toroidal
            ? dataIn.D - 2 * (dataIn.r * (1 - Math.Cos(data.alpha1)) + 0.7 * data.a1p * Math.Sin(data.alpha1))
            : dataIn.D - 1.4 * data.a1p * Math.Sin(data.alpha1);

        return data;
    }
}