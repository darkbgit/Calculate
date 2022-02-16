using System;
using CalculateVessels.Core.Exceptions;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.PhysicalData;

namespace CalculateVessels.Core.Shells.Cylindrical
{
    internal class CylindricalShellCalculateProvider : ICalculateProvider
    {
        public ICalculatedData Calculate(IInputData inputData)
        {
            if (inputData is not CylindricalShellInputData dataIn)
                throw new CalculateException("Error. Input data wrong type.");

            var data = new CylindricalShellCalculatedData(dataIn);

            if (dataIn.SigmaAllow == 0)
            {
                try
                {
                    data.SigmaAllow = Physical.Gost34233_1.GetSigma(dataIn.Steel, dataIn.t);
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
                    throw new CalculateException("Error get E.", e);
                }
            }
            else
            {
                data.E = dataIn.E;
            }

            data.c = dataIn.c1 + dataIn.c2 + dataIn.c3;

            if (dataIn.p > 0)
            {
                if (dataIn.IsPressureIn)
                {
                    data.s_p = dataIn.p * dataIn.D / (2 * data.SigmaAllow * dataIn.fi - dataIn.p);
                    data.s = data.s_p + data.c;

                    if (dataIn.s != 0.0)
                    {
                        if (data.s > dataIn.s)
                            throw new CalculateException("Принятая толщина меньше расчетной.");

                        data.p_d = 2 * data.SigmaAllow * dataIn.fi * (dataIn.s - data.c) /
                                   (dataIn.D + dataIn.s - data.c);
                    }
                }
                else
                {
                    data.l = dataIn.l + dataIn.l3;
                    data.b_2 = 0.47 * Math.Pow(dataIn.p / (0.00001 * data.E), 0.067) * Math.Pow(data.l / dataIn.D, 0.4);
                    data.b = Math.Max(1.0, data.b_2);
                    data.s_p_1 = 1.06 * (0.01 * dataIn.D / data.b) *
                                 Math.Pow(dataIn.p / (0.00001 * data.E) * (data.l / dataIn.D), 0.4);
                    data.s_p_2 = 1.2 * dataIn.p * dataIn.D / (2 * data.SigmaAllow - dataIn.p);
                    data.s_p = Math.Max(data.s_p_1, data.s_p_2);
                    data.s = data.s_p + data.c;

                    if (dataIn.s != 0.0)
                    {
                        if (data.s > dataIn.s)
                            throw new CalculateException("Принятая толщина меньше расчетной.");


                        data.p_dp = 2 * data.SigmaAllow * (dataIn.s - data.c) /
                                    (dataIn.D + dataIn.s - data.c);
                        data.B1_2 = 9.45 * (dataIn.D / data.l) *
                                    Math.Sqrt(dataIn.D / (100 * (dataIn.s - data.c)));
                        data.B1 = Math.Min(1.0, data.B1_2);
                        data.p_de = 2.08 * 0.00001 * data.E / (dataIn.ny * data.B1) * (dataIn.D / data.l) *
                                    Math.Pow(100 * (dataIn.s - data.c) / dataIn.D, 2.5);
                        data.p_d = data.p_dp / Math.Sqrt(1 + Math.Pow(data.p_dp / data.p_de, 2));

                    }
                }

                if (data.p_d < dataIn.p && dataIn.s != 0)
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
                    data.s_f = data.s_pf + data.c;

                    if (data.s_f > dataIn.s)
                        throw new CalculateException(
                            "Принятая толщина меньше расчетной от нагрузки осевым сжимающим усилием.");


                    data.FAllow = Math.PI * (dataIn.D + dataIn.s - data.c) *
                                  (dataIn.s - data.c) * data.SigmaAllow * dataIn.fi_t;

                }
                else
                {
                    data.F_dp = Math.PI * (dataIn.D + dataIn.s - data.c) * (dataIn.s - data.c) * data.SigmaAllow;
                    data.F_de1 = 0.000031 * data.E / dataIn.ny * Math.Pow(dataIn.D, 2) *
                                 Math.Pow(100 * (dataIn.s - data.c) / dataIn.D, 2.5);

                    const int L_MORE_THEN_D = 10;
                    bool isLMoreThenD = dataIn.l / dataIn.D > L_MORE_THEN_D;

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

                        data.lambda = 2.83 * data.lpr / (dataIn.D + dataIn.s - data.c);
                        data.F_de2 = Math.PI * (dataIn.D + dataIn.s - data.c) * (dataIn.s - data.c) * data.E /
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
                data.M_dp = Math.PI / 4 * dataIn.D * (dataIn.D + dataIn.s - data.c) * (dataIn.s - data.c) *
                            data.SigmaAllow;
                data.M_de = 0.000089 * data.E / dataIn.ny * Math.Pow(dataIn.D, 3) *
                            Math.Pow(100 * (dataIn.s - data.c) / dataIn.D, 2.5);
                data.M_d = data.M_dp / Math.Sqrt(1 + Math.Pow(data.M_dp / data.M_de, 2));
            }

            if (dataIn.Q > 0)
            {
                data.Q_dp = 0.25 * data.SigmaAllow * Math.PI * dataIn.D * (dataIn.s - data.c);
                data.Q_de = 2.4 * data.E * Math.Pow(dataIn.s - data.c, 2) / dataIn.ny *
                            (0.18 + 3.3 * dataIn.D * (dataIn.s - data.c) / Math.Pow(dataIn.l, 2));
                data.Q_d = data.Q_dp / Math.Sqrt(1 + Math.Pow(data.Q_dp / data.Q_de, 2));
            }

            data.ConditionStability = dataIn.p / data.p_d +
                                      (dataIn.FCalcSchema == 5 ? data.F : dataIn.F) / data.FAllow +
                                      dataIn.M / data.M_d +
                                      Math.Pow(dataIn.Q / data.Q_d, 2);
            if (data.ConditionStability > 1)
            {
                data.ErrorList.Add("Условие устойчивости для совместного действия усилий не выполняется");
            }

            // Condition use formulas
            const int DIAMETER_BIG_LITTLE_BORDER = 200;
            const double CONDITION_USE_FORMALS_BIG_DIAMETER = 0.1;
            const double CONDITION_USE_FORMALS_LITTLE_DIAMETER = 0.3;
            data.IsConditionUseFormulas = DIAMETER_BIG_LITTLE_BORDER < dataIn.D
                ? (dataIn.s - data.c) / dataIn.D <= CONDITION_USE_FORMALS_BIG_DIAMETER
                : (dataIn.s - data.c) / dataIn.D <= CONDITION_USE_FORMALS_LITTLE_DIAMETER;

            if (!data.IsConditionUseFormulas)
            {
                data.ErrorList.Add("Условие применения формул не выполняется.");
            }

            return data;
        }
    }
}
