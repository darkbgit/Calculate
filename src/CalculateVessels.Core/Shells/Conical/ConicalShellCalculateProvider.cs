using System;
using CalculateVessels.Core.Exceptions;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Enums;
using CalculateVessels.Data.Exceptions;
using CalculateVessels.Data.PhysicalData;
using CalculateVessels.Data.PhysicalData.Gost34233_1;

namespace CalculateVessels.Core.Shells.Conical
{
    public class ConicalShellCalculateProvider : ICalculateProvider
    {

        public ICalculatedData Calculate(IInputData inputData)
        {
            if (inputData is not ConicalShellInputData dataIn)
                throw new CalculateException("Error. Input data wrong type.");

            var data = new ConicalShellCalculatedData(dataIn);

            if (dataIn.SigmaAllow == 0)
            {
                try
                {
                    data.SigmaAllow = Gost34233_1.GetSigma(dataIn.Steel, dataIn.t);
                }
                catch (PhysicalDataException e)
                {
                    throw new CalculateException("Error get sigma.", e);
                }
            }
            else
            {
                data.SigmaAllow = dataIn.SigmaAllow;
            }

            if (dataIn.E == 0)
            {
                try
                {
                    data.E = Physical.GetE(dataIn.Steel, dataIn.t);
                }
                catch (PhysicalDataException e)
                {
                    throw new CalculateException("Error get sigma.", e);
                }
            }
            else
            {
                data.E = dataIn.E;
            }

            //TODO: 
            data.SigmaAllow1 = 0;
            data.SigmaAllow2 = 0;
            data.SigmaAllowC = 0;

            data.c = dataIn.c1 + dataIn.c2 + dataIn.c3;

            data.cosAlpha1 = Math.Cos(dataIn.alfa1 * Math.PI / 180);
            data.tgAlpha1 = Math.Tan(dataIn.alfa1 * Math.PI / 180);
            data.sinAlpha1 = Math.Sin(dataIn.alfa1 * Math.PI / 180);

            // Condition use formals
            {
                const double CONDITION_USE_FORMULAS_FROM = 0.001;
                const double CONDITION_USE_FORMULAS_TO = 0.05;

                data.IsConditionUseFormulas = dataIn.s1 * data.cosAlpha1 / dataIn.D >= CONDITION_USE_FORMULAS_FROM
                    && dataIn.s1 * data.cosAlpha1 / dataIn.D <= CONDITION_USE_FORMULAS_TO;

                if (!data.IsConditionUseFormulas)
                {
                    data.ErrorList.Add("Условие применения формул не выполняется");
                }
            }

            switch (dataIn.ConnectionType)
            {
                case ConicalConnectionType.Simply:
                case ConicalConnectionType.WithRingPicture25b:
                case ConicalConnectionType.WithRingPicture29:
                    data.a1p = 0.7 * Math.Sqrt(dataIn.D * (dataIn.s1 - data.c) / data.cosAlpha1);
                    data.a2p = 0.7 * Math.Sqrt(dataIn.D * (dataIn.s2 - data.c));
                    break;
                case ConicalConnectionType.Toroidal:
                    data.a1p = 0.7 * Math.Sqrt(dataIn.D * (dataIn.sT - data.c) / data.cosAlpha1);
                    data.a2p = 0.5 * Math.Sqrt(dataIn.D * (dataIn.sT - data.c));
                    break;
                case ConicalConnectionType.WithoutConnection:
                    data.a1p = 0.7 * Math.Sqrt(dataIn.D * (dataIn.s1 - data.c) / data.cosAlpha1);
                    break;
                default:
                    throw new CalculateException("Conical connection type error.");
            }

            if (dataIn.IsConnectionWithLittle)
            {
                data.a1p_l = Math.Sqrt(dataIn.D1 * (dataIn.s1 - data.c) / data.cosAlpha1);
                data.a2p_l = 1.25 * Math.Sqrt(dataIn.D1 * (dataIn.s2 - data.c));
            }

            data.Dk = dataIn.ConnectionType == ConicalConnectionType.Toroidal
                ? dataIn.D - 2 * (dataIn.r * (1 - data.cosAlpha1) + 0.7 * data.a1p * data.sinAlpha1)
                : dataIn.D - 1.4 * data.a1p * data.sinAlpha1;


            if (dataIn.p > 0)
            {
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
                        * Math.Sqrt((dataIn.D + dataIn.D1) / ((dataIn.s - data.c) * 100)) * data.tgAlpha1;
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

                if (dataIn.ConnectionType != ConicalConnectionType.WithoutConnection)
                {
                    if (dataIn.alfa1 > 70)
                    {
                        data.IsConditionUseFormulas = false;
                        data.ErrorList.Add("Угол должен быть меньше либо равен 70 градусам");
                    }
                    switch (dataIn.ConnectionType)
                    {
                        case ConicalConnectionType.Simply:
                            if ((dataIn.s1 - data.c) < (dataIn.s2 - data.c))
                            {
                                data.IsConditionUseFormulas = false;
                                data.ErrorList.Add("Условие применения формул не выполняется");
                            }
                            data.chi_1 = data.SigmaAllow1 / data.SigmaAllow2;
                            data.beta = 0.4 * Math.Sqrt(dataIn.D / (dataIn.s2 - data.c)) * data.tgAlpha1
                                / (1 + Math.Sqrt((1 + data.chi_1
                                * Math.Pow((dataIn.s1 - data.c) / (dataIn.s2 - data.c), 2))
                                / (2 * data.cosAlpha1) * data.chi_1 * (dataIn.s1 - data.c) / (dataIn.s2 - data.c))) - 0.25;
                            data.beta_1 = Math.Max(0.5, data.beta);
                            data.s_2p = dataIn.p * dataIn.D * data.beta_1
                                / (2 * data.SigmaAllow2 * dataIn.fi - dataIn.p);

                            if (dataIn.s2 >= data.s_2p + data.c)
                            {
                                data.p_dBig = 2 * data.SigmaAllow2 * dataIn.fi * (dataIn.s2 - data.c)
                                    / (dataIn.D * data.beta_1 + (dataIn.s2 - data.c));
                            }
                            else
                            {
                                throw new CalculateException("Принятая толщина переходной зоны меньше расчетной.");
                            }
                            break;
                        case ConicalConnectionType.WithRingPicture25b:
                            if ((dataIn.s1 - data.c) < (dataIn.s2 - data.c))
                            {
                                data.IsConditionUseFormulas = false;
                                data.ErrorList.Add("Условие применения формул не выполняется");
                            }
                            data.chi_1 = data.SigmaAllow1 / data.SigmaAllow2;
                            data.beta = 0.4 * Math.Sqrt(dataIn.D / (dataIn.s2 - data.c)) * data.tgAlpha1
                                / (1 + Math.Sqrt((1 + data.chi_1
                                * Math.Pow((dataIn.s1 - data.c) / (dataIn.s2 - data.c), 2))
                                / (2 * data.cosAlpha1) * data.chi_1 * (dataIn.s1 - data.c) / (dataIn.s2 - data.c))) - 0.25;
                            data.beta_a = (2 * data.SigmaAllow2 * dataIn.fi / dataIn.p - 1) * (dataIn.s2 - data.c) / dataIn.D;
                            data.Ak = dataIn.p * Math.Pow(dataIn.D, 2) * data.tgAlpha1 / (8 * data.SigmaAllowC * dataIn.fi_k)
                                * (1 - (data.beta_a + 0.25) / (data.beta + 0.25));
                            data.B2 = 1.6 * data.Ak / ((dataIn.s2 - data.c) * Math.Sqrt(dataIn.D * (dataIn.s2 - data.c)))
                                * data.SigmaAllowC * dataIn.fi_k / (data.SigmaAllow2 * dataIn.fi_t);
                            data.B3 = 0.25;
                            data.beta_0 = 0.4 * Math.Sqrt(dataIn.D / (dataIn.s2 - data.c)) * data.tgAlpha1 - data.B3 *
                                (1 + Math.Sqrt((1 + data.chi_1 * Math.Pow((dataIn.s1 - data.c) / (dataIn.s2 - data.c), 2)) /
                                (2 * data.cosAlpha1) * data.chi_1 * (dataIn.s1 - data.c) / (dataIn.s2 - data.c))) /
                                (data.B2 + (1 + Math.Sqrt((1 + data.chi_1 * Math.Pow((dataIn.s1 - data.c) / (dataIn.s2 - data.c), 2)) /
                                (2 * data.cosAlpha1) * data.chi_1 * (dataIn.s1 - data.c) / (dataIn.s2 - data.c))));
                            data.beta_2 = Math.Max(0.5, data.beta_0);
                            data.p_dBig = 2 * data.SigmaAllow2 * dataIn.fi * (dataIn.s2 - data.c) / (dataIn.D * data.beta_2 + (dataIn.s2 - data.c));
                            break;
                        case ConicalConnectionType.WithRingPicture29:
                            data.Ak = dataIn.p * Math.Pow(dataIn.D, 2) * data.tgAlpha1 / (8 * data.SigmaAllowC * dataIn.fi_k);
                            data.p_dBig = data.Ak * 8 * data.SigmaAllowC * dataIn.fi_k / (Math.Pow(dataIn.D, 2) * data.tgAlpha1);
                            //TODO: Check conical shell with ring picture 29
                            break;
                        case ConicalConnectionType.Toroidal:
                            if (dataIn.r / dataIn.D >= 0.0
                                && dataIn.r / dataIn.D < 0.3)
                            {
                                data.IsConditionUseFormulas = false;
                                data.ErrorList.Add("Условие применения формул не выполняется");
                            }
                            data.chi_1 = data.SigmaAllow1 / data.SigmaAllow2;
                            data.beta = 0.4 * Math.Sqrt(dataIn.D / (dataIn.s2 - data.c)) * data.tgAlpha1
                                / (1 + Math.Sqrt((1 + data.chi_1
                                * Math.Pow((dataIn.s1 - data.c) / (dataIn.s2 - data.c), 2))
                                / (2 * data.cosAlpha1) * data.chi_1 * (dataIn.s1 - data.c) / (dataIn.s2 - data.c))) - 0.25;
                            data.beta_t = 1 / (1 + (0.028 * dataIn.alfa1 * dataIn.r / dataIn.D *
                                Math.Sqrt(dataIn.D / (dataIn.sT - data.c))) /
                                (1 / Math.Sqrt(data.cosAlpha1) + 1));
                            //TODO: Check alfa1 in betadata.t in degree or in radians
                            data.beta_3 = Math.Max(0.5, Math.Max(data.beta, data.beta_t));
                            data.s_tp = dataIn.p * dataIn.D * data.beta_3 / (2 * dataIn.fi * data.SigmaAllow - dataIn.p);
                            data.p_dBig = 2 * data.SigmaAllow * dataIn.fi * (dataIn.sT - data.c) /
                                (dataIn.D * data.beta_3 + (dataIn.sT - data.c));
                            break;
                    }
                    if (data.p_dBig < dataIn.p && dataIn.s != 0)
                    {
                        data.ErrorList.Add("[p] для переходной части меньше p");
                    }
                    if (dataIn.IsConnectionWithLittle)
                    {
                        data.chi_1 = data.SigmaAllow1 / data.SigmaAllow2;
                        data.ConditionForBetan = Math.Pow((dataIn.s1 - data.c) / (dataIn.s2 - data.c), 2);
                        if (data.ConditionForBetan >= 1)
                        {
                            data.beta = 0.4 * Math.Sqrt(dataIn.D1 / (dataIn.s2 - data.c)) * data.tgAlpha1
                            / (1 + Math.Sqrt((1 + data.chi_1
                            * Math.Pow((dataIn.s1 - data.c) / (dataIn.s2 - data.c), 2))
                            / (2 * data.cosAlpha1) * data.chi_1 * (dataIn.s1 - data.c) / (dataIn.s2 - data.c))) - 0.25;
                            data.beta_n = data.beta + 0.75;
                        }
                        else
                        {
                            data.beta_n = 0.4 * Math.Sqrt(dataIn.D1 / (dataIn.s2 - data.c)) * data.tgAlpha1
                            / (data.chi_1 * (dataIn.s1 - data.c) / (dataIn.s2 - data.c) * Math.Sqrt((dataIn.s1 - data.c) /
                            ((dataIn.s2 - data.c) * data.cosAlpha1)) + Math.Sqrt((1 + data.chi_1
                            * Math.Pow((dataIn.s1 - data.c) / (dataIn.s2 - data.c), 2))
                            / 2)) + 0.5;
                        }
                        data.beta_4 = Math.Max(1, data.beta_n);
                        data.s_2pLittle = dataIn.p * dataIn.D1 * data.beta_4 / (2 * dataIn.fi * data.SigmaAllow2 - dataIn.p);
                        data.p_dLittle = 2 * data.SigmaAllow2 * dataIn.fi * (dataIn.s2 - data.c) /
                            (dataIn.D1 * data.beta_4 + (dataIn.s2 - data.c));

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

}