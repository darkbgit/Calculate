using CalculateVessels.Core.Exceptions;
using CalculateVessels.Core.Helpers;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Enums;
using CalculateVessels.Data.Interfaces;
using System;

namespace CalculateVessels.Core.Shells.Conical;

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
        var data = new ConicalShellCalculated
        {
            InputData = dataIn,
            SigmaAllow = PhysicalHelper.GetSigmaIfZero(dataIn.SigmaAllow, dataIn.Steel, dataIn.t, _physicalData),
            E = PhysicalHelper.GetEIfZero(dataIn.E, dataIn.Steel, dataIn.t, _physicalData)
        };

        switch (dataIn.ConnectionType)
        {
            case ConicalConnectionType.WithoutConnection:
                break;
            case ConicalConnectionType.Simply:
                data.SigmaAllow1Big = PhysicalHelper.GetSigmaIfZero(dataIn.SigmaAllow1Big, dataIn.Steel1Big, dataIn.t, _physicalData);
                data.SigmaAllow2Big = PhysicalHelper.GetSigmaIfZero(dataIn.SigmaAllow2Big, dataIn.Steel2Big, dataIn.t, _physicalData);
                break;
            case ConicalConnectionType.WithRingPicture25b:
            case ConicalConnectionType.WithRingPicture29:
                data.SigmaAllow1Big = PhysicalHelper.GetSigmaIfZero(dataIn.SigmaAllow1Big, dataIn.Steel1Big, dataIn.t, _physicalData);
                data.SigmaAllow2Big = PhysicalHelper.GetSigmaIfZero(dataIn.SigmaAllow2Big, dataIn.Steel2Big, dataIn.t, _physicalData);
                data.SigmaAllowC = PhysicalHelper.GetSigmaIfZero(dataIn.SigmaAllowC, dataIn.SteelC, dataIn.t, _physicalData);
                break;
            case ConicalConnectionType.Toroidal:
                data.SigmaAllowT = PhysicalHelper.GetSigmaIfZero(dataIn.SigmaAllowT, dataIn.SteelT, dataIn.t, _physicalData);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (dataIn.IsConnectionWithLittle)
        {
            data.SigmaAllow1Little = PhysicalHelper.GetSigmaIfZero(dataIn.SigmaAllow1Little, dataIn.Steel1Little, dataIn.t, _physicalData);
            data.SigmaAllow2Little = PhysicalHelper.GetSigmaIfZero(dataIn.SigmaAllow2Little, dataIn.Steel2Little, dataIn.t, _physicalData);
        }

        data.c = dataIn.c1 + dataIn.c2 + dataIn.c3;

        data.cosAlpha1 = Math.Cos(MathHelper.DegreeToRadian(dataIn.alpha1));
        data.tgAlpha1 = Math.Tan(MathHelper.DegreeToRadian(dataIn.alpha1));
        data.sinAlpha1 = Math.Sin(MathHelper.DegreeToRadian(dataIn.alpha1));

        if (dataIn.p > 0)
        {
            // Condition use formals

            const double CONDITION_USE_FORMULAS_FROM = 0.001;
            const double CONDITION_USE_FORMULAS_TO = 0.05;

            data.IsConditionUseFormulas = dataIn.s * data.cosAlpha1 / dataIn.D >= CONDITION_USE_FORMULAS_FROM
                                          && dataIn.s * data.cosAlpha1 / dataIn.D <= CONDITION_USE_FORMULAS_TO;

            if (!data.IsConditionUseFormulas)
            {
                data.ErrorList.Add("Условие применения формул не выполняется.");
            }

            if (dataIn.alpha1 > 70)
            {
                data.IsConditionUseFormulas = false;
                data.ErrorList.Add("Угол должен быть меньше либо равен 70 градусам.");
            }


            switch (dataIn.ConnectionType)
            {
                case ConicalConnectionType.Simply:
                case ConicalConnectionType.WithRingPicture25b:
                case ConicalConnectionType.WithRingPicture29:
                    data.a1p = 0.7 * Math.Sqrt(dataIn.D * (dataIn.s1Big - data.c) / data.cosAlpha1);
                    data.a2p = 0.7 * Math.Sqrt(dataIn.D * (dataIn.s2Big - data.c));
                    break;
                case ConicalConnectionType.Toroidal:
                    data.a1p = 0.7 * Math.Sqrt(dataIn.D * (dataIn.sT - data.c) / data.cosAlpha1);
                    data.a2p = 0.5 * Math.Sqrt(dataIn.D * (dataIn.sT - data.c));
                    break;
                case ConicalConnectionType.WithoutConnection:
                    data.a1p = 0.7 * Math.Sqrt(dataIn.D * (dataIn.s - data.c) / data.cosAlpha1);
                    break;
                default:
                    throw new CalculateException("Conical connection type error.");
            }

            if (dataIn.IsConnectionWithLittle)
            {
                data.a1p_l = Math.Sqrt(dataIn.D1 * (dataIn.s1Little - data.c) / data.cosAlpha1);
                data.a2p_l = 1.25 * Math.Sqrt(dataIn.D1 * (dataIn.s2Little - data.c));
            }

            data.Dk = dataIn.ConnectionType == ConicalConnectionType.Toroidal
                ? dataIn.D - 2 * (dataIn.r * (1 - data.cosAlpha1) + 0.7 * data.a1p * data.sinAlpha1)
                : dataIn.D - 1.4 * data.a1p * data.sinAlpha1;



            if (dataIn.IsPressureIn)
            {
                data.s_p = dataIn.p * data.Dk / (2 * data.SigmaAllow * dataIn.fi - dataIn.p)
                           * (1 / data.cosAlpha1);
                data.s = data.s_p + data.c;

                if (dataIn.s != 0)
                {
                    if (data.s > dataIn.s)
                        throw new CalculateException("Принятая толщина меньше расчетной.");

                    data.p_d = 2 * data.SigmaAllow * dataIn.fi * (dataIn.s - data.c)
                               / (data.Dk / data.cosAlpha1 + (dataIn.s - data.c));
                }
            }
            else
            {
                data.lE = (dataIn.D - dataIn.D1) / (2 * data.sinAlpha1);
                data.DE_1 = (dataIn.D + dataIn.D1) / (2 * data.cosAlpha1);
                data.DE_2 = dataIn.D / data.cosAlpha1 - 0.3 * (dataIn.D + dataIn.D1)
                                                            * Math.Sqrt((dataIn.D + dataIn.D1) * data.tgAlpha1 / ((dataIn.s - data.c) * 100));
                data.DE = Math.Max(data.DE_1, data.DE_2);
                data.B1_1 = 9.45 * data.DE / data.lE * Math.Sqrt(data.DE / (100 * (dataIn.s - data.c)));
                data.B1 = Math.Min(1.0, data.B1_1);

                data.s_p_1 = 1.06 * (0.01 * data.DE / data.B1)
                                  * Math.Pow(dataIn.p / (0.00001 * data.E) * (data.lE / data.DE), 0.4);
                data.s_p_2 = 1.2 * dataIn.p * data.Dk / (2 * dataIn.fi * data.SigmaAllow - dataIn.p)
                             * (1 / data.cosAlpha1);
                data.s_p = Math.Max(data.s_p_1, data.s_p_2);
                data.s = data.s_p + data.c;

                if (dataIn.s != 0)
                {
                    if (data.s > dataIn.s)
                        throw new CalculateException("Принятая толщина меньше расчетной.");

                    data.p_dp = 2 * data.SigmaAllow * (dataIn.s - data.c)
                                / (data.Dk / data.cosAlpha1 + dataIn.s - data.c);
                    data.p_de = 2.08 * 0.00001 * data.E / (dataIn.ny * data.B1) * (data.DE / data.lE)
                        * Math.Pow(100 * (dataIn.s - data.c) / data.DE, 2.5);
                    data.p_d = data.p_dp / Math.Sqrt(1 + Math.Pow(data.p_dp / data.p_de, 2));
                }
            }

            if (data.p_d < dataIn.p && dataIn.s != 0)
            {
                data.ErrorList.Add("[p] меньше p");
            }

            //down connection
            switch (dataIn.ConnectionType)
            {
                case ConicalConnectionType.WithoutConnection:
                    break;
                case ConicalConnectionType.Simply:
                    if ((dataIn.s1Big - data.c) < (dataIn.s2Big - data.c))
                    {
                        data.IsConditionUseFormulas = false;
                        data.ErrorList.Add("Условие применения формул не выполняется");
                    }
                    data.chi_1Big = data.SigmaAllow1Big / data.SigmaAllow2Big;
                    data.beta = 0.4 * Math.Sqrt(dataIn.D / (dataIn.s2Big - data.c)) * data.tgAlpha1
                        / (1 + Math.Sqrt((1 + data.chi_1Big
                                * Math.Pow((dataIn.s1Big - data.c) / (dataIn.s2Big - data.c), 2))
                            / (2 * data.cosAlpha1) * data.chi_1Big * (dataIn.s1Big - data.c) / (dataIn.s2Big - data.c))) - 0.25;
                    data.beta_1 = Math.Max(0.5, data.beta);
                    data.s_2pBig = dataIn.p * dataIn.D * data.beta_1
                                / (2 * data.SigmaAllow2Big * dataIn.fi - dataIn.p);

                    data.s_2Big = data.s_2pBig + data.c;

                    if (data.s_2Big > dataIn.s2Big)
                    {
                        data.ErrorList.Add("Принятая толщина переходной зоны меньше расчетной.");
                    }

                    data.p_dBig = 2 * data.SigmaAllow2Big * dataIn.fi * (dataIn.s2Big - data.c)
                                  / (dataIn.D * data.beta_1 + (dataIn.s2Big - data.c));

                    break;

                case ConicalConnectionType.WithRingPicture25b:
                    if ((dataIn.s1Big - data.c) < (dataIn.s2Big - data.c))
                    {
                        data.IsConditionUseFormulas = false;
                        data.ErrorList.Add("Условие применения формул не выполняется");
                    }
                    data.chi_1Big = data.SigmaAllow1Big / data.SigmaAllow2Big;
                    data.beta = 0.4 * Math.Sqrt(dataIn.D / (dataIn.s2Big - data.c)) * data.tgAlpha1
                        / (1 + Math.Sqrt((1 + data.chi_1Big
                                * Math.Pow((dataIn.s1Big - data.c) / (dataIn.s2Big - data.c), 2))
                            / (2 * data.cosAlpha1) * data.chi_1Big * (dataIn.s1Big - data.c) / (dataIn.s2Big - data.c))) - 0.25;
                    data.beta_a = (2 * data.SigmaAllow2Big * dataIn.fi / dataIn.p - 1) * (dataIn.s2Big - data.c) / dataIn.D;
                    data.Ak = dataIn.p * Math.Pow(dataIn.D, 2) * data.tgAlpha1 / (8 * data.SigmaAllowC * dataIn.fi_k)
                              * (1 - (data.beta_a + 0.25) / (data.beta + 0.25));
                    if (data.Ak > 0 && data.Ak < dataIn.Ak)
                    {
                        data.ErrorList.Add("Площадь укрепляющего кольца недостаточна.");
                    }
                    data.B2 = 1.6 * data.Ak / ((dataIn.s2Big - data.c) * Math.Sqrt(dataIn.D * (dataIn.s2Big - data.c)))
                        * data.SigmaAllowC * dataIn.fi_k / (data.SigmaAllow2Big * dataIn.fi_t);
                    data.B3 = 0.25;
                    data.beta_0 = 0.4 * Math.Sqrt(dataIn.D / (dataIn.s2Big - data.c)) * data.tgAlpha1 - data.B3 *
                        (1 + Math.Sqrt((1 + data.chi_1Big * Math.Pow((dataIn.s1Big - data.c) / (dataIn.s2Big - data.c), 2)) /
                            (2 * data.cosAlpha1) * data.chi_1Big * (dataIn.s1Big - data.c) / (dataIn.s2Big - data.c))) /
                        (data.B2 + (1 + Math.Sqrt((1 + data.chi_1Big * Math.Pow((dataIn.s1Big - data.c) / (dataIn.s2Big - data.c), 2)) /
                            (2 * data.cosAlpha1) * data.chi_1Big * (dataIn.s1Big - data.c) / (dataIn.s2Big - data.c))));
                    data.beta_2 = Math.Max(0.5, data.beta_0);
                    data.p_dBig = 2 * data.SigmaAllow2Big * dataIn.fi * (dataIn.s2Big - data.c) / (dataIn.D * data.beta_2 + (dataIn.s2Big - data.c));

                    break;
                case ConicalConnectionType.WithRingPicture29:
                    data.Ak = dataIn.p * Math.Pow(dataIn.D, 2) * data.tgAlpha1 / (8 * data.SigmaAllowC * dataIn.fi_k);


                    if (data.Ak > 0 && data.Ak < dataIn.Ak)
                    {
                        data.ErrorList.Add("Площадь укрепляющего кольца недостаточна.");
                    }

                    data.p_dBig = data.Ak * 8 * data.SigmaAllowC * dataIn.fi_k / (Math.Pow(dataIn.D, 2) * data.tgAlpha1);

                    break;
                case ConicalConnectionType.Toroidal:
                    if (dataIn.r / dataIn.D >= 0.0
                        && dataIn.r / dataIn.D < 0.3)
                    {
                        data.IsConditionUseFormulas = false;
                        data.ErrorList.Add("Условие применения формул не выполняется.");
                    }

                    data.beta = 0.4 * Math.Sqrt(dataIn.D / (dataIn.s2Big - data.c)) * data.tgAlpha1
                        / (1 + Math.Sqrt(1 / data.cosAlpha1)) - 0.25;
                    data.beta_t = 1 / (1 + (0.028 * dataIn.alpha1 * dataIn.r / dataIn.D *
                                            Math.Sqrt(dataIn.D / (dataIn.sT - data.c))) /
                        (1 / Math.Sqrt(data.cosAlpha1) + 1));
                    //TODO: Check alpha1 in beta_t in degree or in radians
                    data.beta_3_2 = data.beta * data.beta_t;
                    data.beta_3 = Math.Max(0.5, data.beta_3_2);
                    data.s_tp = dataIn.p * dataIn.D * data.beta_3 / (2 * dataIn.fi * data.SigmaAllowT - dataIn.p);
                    data.s_t = data.s_tp + data.c;

                    if (data.s_t > dataIn.sT)
                    {
                        data.ErrorList.Add("Принятая толщина переходной зоны меньше расчетной.");
                    }

                    data.p_dBig = 2 * data.SigmaAllowT * dataIn.fi * (dataIn.sT - data.c) /
                                  (dataIn.D * data.beta_3 + (dataIn.sT - data.c));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (dataIn.ConnectionType != ConicalConnectionType.WithoutConnection && data.p_dBig < dataIn.p && dataIn.s != 0)
            {
                data.ErrorList.Add("[p] для переходной части меньше p");
            }

            //up connection
            if (dataIn.IsConnectionWithLittle)
            {
                data.chi_1Little = data.SigmaAllow1Little / data.SigmaAllow2Little;
                data.ConditionForBetan = Math.Pow((dataIn.s1Little - data.c) / (dataIn.s2Little - data.c), 2);
                if (data.ConditionForBetan >= 1)
                {
                    data.beta = 0.4 * Math.Sqrt(dataIn.D1 / (dataIn.s2Little - data.c)) * data.tgAlpha1
                        / (1 + Math.Sqrt((1 + data.chi_1Little
                                * Math.Pow((dataIn.s1Little - data.c) / (dataIn.s2Little - data.c), 2))
                            / (2 * data.cosAlpha1) * data.chi_1Little * (dataIn.s1Little - data.c) / (dataIn.s2Little - data.c))) - 0.25;
                    data.beta_n = data.beta + 0.75;
                }
                else
                {
                    data.beta_n = 0.4 * Math.Sqrt(dataIn.D1 / (dataIn.s2Little - data.c)) * data.tgAlpha1
                        / (data.chi_1Little * (dataIn.s1Little - data.c) / (dataIn.s2Little - data.c) * Math.Sqrt((dataIn.s1Little - data.c) /
                            ((dataIn.s2Little - data.c) * data.cosAlpha1)) + Math.Sqrt((1 + data.chi_1Little
                                * Math.Pow((dataIn.s1Little - data.c) / (dataIn.s2Little - data.c), 2))
                            / 2)) + 0.5;
                }
                data.beta_4 = Math.Max(1, data.beta_n);
                data.s_2pLittle = dataIn.p * dataIn.D1 * data.beta_4 / (2 * dataIn.fi * data.SigmaAllow2Little - dataIn.p);
                data.s_2Little = data.s_2pLittle + data.c;

                if (data.s_2Little > dataIn.s2Little)
                {
                    data.ErrorList.Add("Принятая толщина обечайки меньшего диаметра меньше расчетной.");
                }

                data.p_dLittle = 2 * data.SigmaAllow2Little * dataIn.fi * (dataIn.s2Little - data.c) /
                                 (dataIn.D1 * data.beta_4 + (dataIn.s2Little - data.c));

                if (data.p_dLittle < dataIn.p && dataIn.s != 0)
                {
                    data.ErrorList.Add("[p] для переходной части меньше p");
                }
            }
        }

        //if (dataIn.F > 0)
        //{
        //    data.sdata.calcrf = dataIn.F / (Math.PI * dataIn.D * dataIn.SigmaAllow * dataIn.fit);
        //    data.sdata.calcf = data.sdata.calcrf + data.c;
        //    if (dataIn.isFTensile)
        //    {
        //        data.Fdata.d = Math.PI * (dataIn.D + dataIn.s - data.c) * (dataIn.s - data.c) * dataIn.SigmaAllow * dataIn.fit;
        //    }
        //    else
        //    {
        //        data.Fdata.dp = Math.PI * (dataIn.D + dataIn.s - data.c) * (dataIn.s - data.c) * dataIn.SigmaAllow;
        //        data.Fdata.de1 = 0.000031 * dataIn.E / dataIn.ny * Math.Pow(dataIn.D, 2) * Math.Pow(100 * (dataIn.s - data.c) / dataIn.D, 2.5);

        //        const int Ldata.MOREdata.THENdata.D = 10;
        //        bool isLMoreThenD = dataIn.l / dataIn.D > Ldata.MOREdata.THENdata.D;

        //        if (isLMoreThenD)
        //        {
        //            switch (dataIn.FCalcSchema)
        //            {
        //                case 1:
        //                    data.lpr = dataIn.l;
        //                    break;
        //                case 2:
        //                    data.lpr = 2 * dataIn.l;
        //                    break;
        //                case 3:
        //                    data.lpr = 0.7 * dataIn.l;
        //                    break;
        //                case 4:
        //                    data.lpr = 0.5 * dataIn.l;
        //                    break;
        //                case 5:
        //                    data.F = dataIn.q * dataIn.l;
        //                    data.lpr = 1.12 * dataIn.l;
        //                    break;
        //                case 6:
        //                    double fDivl6 = dataIn.F / dataIn.l;
        //                    fDivl6 *= 10;
        //                    fDivl6 = Math.Round(fDivl6 / 2);
        //                    fDivl6 *= 0.2;
        //                    switch (fDivl6)
        //                    {
        //                        case 0:
        //                            data.lpr = 2 * dataIn.l;
        //                            break;
        //                        case 0.2:
        //                            data.lpr = 1.73 * dataIn.l;
        //                            break;
        //                        case 0.4:
        //                            data.lpr = 1.47 * dataIn.l;
        //                            break;
        //                        case 0.6:
        //                            data.lpr = 1.23 * dataIn.l;
        //                            break;
        //                        case 0.8:
        //                            data.lpr = 1.06 * dataIn.l;
        //                            break;
        //                        case 1:
        //                            data.lpr = dataIn.l;
        //                            break;
        //                    }
        //                    break;
        //                case 7:
        //                    double fDivl7 = dataIn.F / dataIn.l;
        //                    fDivl7 *= 10;
        //                    fDivl7 = Math.Round(fDivl7 / 2);
        //                    fDivl7 *= 0.2;
        //                    switch (fDivl7)
        //                    {
        //                        case 0:
        //                            data.lpr = 2 * dataIn.l;
        //                            break;
        //                        case 0.2:
        //                            data.lpr = 1.7 * dataIn.l;
        //                            break;
        //                        case 0.4:
        //                            data.lpr = 1.4 * dataIn.l;
        //                            break;
        //                        case 0.6:
        //                            data.lpr = 1.11 * dataIn.l;
        //                            break;
        //                        case 0.8:
        //                            data.lpr = 0.85 * dataIn.l;
        //                            break;
        //                        case 1:
        //                            data.lpr = 0.7 * dataIn.l;
        //                            break;
        //                    }
        //                    break;

        //            }
        //            lamda = 2.83 * data.lpr / (dataIn.D + dataIn.s - data.c);
        //            data.Fdata.de2 = Math.PI * (dataIn.D + dataIn.s - data.c) * (dataIn.s - data.c) * dataIn.E / dataIn.ny *
        //                            Math.Pow(Math.PI / lamda, 2);
        //            data.Fdata.de = Math.Min(data.Fdata.de1, data.Fdata.de2);
        //        }
        //        else
        //        {
        //            data.Fdata.de = data.Fdata.de1;
        //        }

        //        data.Fdata.d = data.Fdata.dp / Math.Sqrt(1 + Math.Pow(data.Fdata.dp / data.Fdata.de, 2));
        //    }
        //}

        //if (dataIn.M > 0)
        //{
        //    data.Mdata.dp = Math.PI / 4 * dataIn.D * (dataIn.D + dataIn.s - data.c) * (dataIn.s - data.c) * dataIn.SigmaAllow;
        //    data.Mdata.de = 0.000089 * dataIn.E / dataIn.ny * Math.Pow(dataIn.D, 3) * Math.Pow(100 * (dataIn.s - data.c) / dataIn.D, 2.5);
        //    data.Mdata.d = data.Mdata.dp / Math.Sqrt(1 + Math.Pow(data.Mdata.dp / data.Mdata.de, 2));
        //}

        //if (dataIn.Q > 0)
        //{
        //    data.Qdata.dp = 0.25 * dataIn.SigmaAllow * Math.PI * dataIn.D * (dataIn.s - data.c);
        //    data.Qdata.de = 2.4 * dataIn.E * Math.Pow(dataIn.s - data.c, 2) / dataIn.ny *
        //        (0.18 + 3.3 * dataIn.D * (dataIn.s - data.c) / Math.Pow(dataIn.l, 2));
        //    data.Qdata.d = data.Qdata.dp / Math.Sqrt(1 + Math.Pow(data.Qdata.dp / data.Qdata.de, 2));
        //}

        //if ((dataIn.IsNeedpCalculate || dataIn.isNeedFCalculate) &&
        //    (dataIn.isNeedMCalculate || dataIn.isNeedQCalculate))
        //{
        //    conditionYstoich = dataIn.p / data.pdata.d + dataIn.F / data.Fdata.d + dataIn.M / data.Mdata.d +
        //                            Math.Pow(dataIn.Q / data.Qdata.d, 2);
        //}

        return data;
    }
}