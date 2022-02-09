using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalculateVessels.Core.Exceptions;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.PhysicalData;

namespace CalculateVessels.Core.Shells.CylindricalShell
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
                    data.SigmaAllow = Physical.Gost34233_1.GetSigma(data.InputData.Steel, data.InputData.t);
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
                    data.E = Physical.GetE(data.InputData.Steel, data.InputData.t);
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



            data.c = data.InputData.c1 + data.InputData.c2 + data.InputData.c3;

            // Condition use formulas
            const int DIAMETER_BIG_LITTLE_BORDER = 200;
            const double CONDITION_USE_FORMALS_BIG_DIAMETER = 0.1;
            const double CONDITION_USE_FORMALS_LITTLE_DIAMETER = 0.3;

            data.IsConditionUseFormulas = DIAMETER_BIG_LITTLE_BORDER < data.InputData.D
                ? (data.InputData.s - data.c) / data.InputData.D <= CONDITION_USE_FORMALS_BIG_DIAMETER
                : (data.InputData.s - data.c) / data.InputData.D <= CONDITION_USE_FORMALS_LITTLE_DIAMETER;

            if (!data.IsConditionUseFormulas)
            {
                data.ErrorList.Add("Условие применения формул не выполняется.");
            }

            if (data.InputData.p > 0)
            {
                if (data.InputData.IsPressureIn)
                {
                    data.s_p = data.InputData.p * data.InputData.D / (2 * data.SigmaAllow * data.InputData.fi - data.InputData.p);
                    data.s = data.s_p + data.c;

                    if (data.InputData.s != 0.0)
                    {
                        if (data.s < data.InputData.s)
                            throw new CalculateException("Принятая толщина меньше расчетной.");

                        data.p_d = 2 * data.SigmaAllow * data.InputData.fi * (data.InputData.s - data.c) /
                                   (data.InputData.D + data.InputData.s - data.c);
                    }
                }
                else
                {
                    data.l = data.InputData.l + data.InputData.l3;
                    data.b_2 = 0.47 * Math.Pow(data.InputData.p / (0.00001 * data.E), 0.067) * Math.Pow(data.l / data.InputData.D, 0.4);
                    data.b = Math.Max(1.0, data.b_2);
                    data.s_p_1 = 1.06 * (0.01 * data.InputData.D / data.b) * Math.Pow(data.InputData.p / (0.00001 * data.E) * (data.l / data.InputData.D), 0.4);
                    data.s_p_2 = 1.2 * data.InputData.p * data.InputData.D / (2 * data.SigmaAllow - data.InputData.p);
                    data.s_p = Math.Max(data.s_p_1, data.s_p_2);
                    data.s = data.s_p + data.c;

                    if (data.InputData.s != 0.0)
                    {
                        if (data.s < data.InputData.s)
                            throw new CalculateException("Принятая толщина меньше расчетной.");


                        data.p_dp = 2 * data.SigmaAllow * (data.InputData.s - data.c) /
                                    (data.InputData.D + data.InputData.s - data.c);
                        data.B1_2 = 9.45 * (data.InputData.D / data.l) *
                                    Math.Sqrt(data.InputData.D / (100 * (data.InputData.s - data.c)));
                        data.B1 = Math.Min(1.0, data.B1_2);
                        data.p_de = 2.08 * 0.00001 * data.E / (data.InputData.ny * data.B1) * (data.InputData.D / data.l) *
                                    Math.Pow(100 * (data.InputData.s - data.c) / data.InputData.D, 2.5);
                        data.p_d = data.p_dp / Math.Sqrt(1 + Math.Pow(data.p_dp / data.p_de, 2));

                    }
                }

                if (data.p_d < data.InputData.p && data.InputData.s != 0)
                {
                    data.ErrorList.Add("[p] меньше p");
                }
            }

            if (data.InputData.F > 0 && data.InputData.s != 0.0)
            {
                if (data.InputData.IsFTensile)
                {
                    data.s_pf = data.InputData.F / (Math.PI * data.InputData.D * data.SigmaAllow * data.InputData.fi_t);
                    data.s_f = data.s_pf + data.c;

                    if (data.s_f < data.InputData.s)
                        throw new CalculateException(
                            "Принятая толщина меньше расчетной от нагрузки осевым сжимающим усилием.");


                    data.FAllow = Math.PI * (data.InputData.D + data.InputData.s - data.c) *
                                  (data.InputData.s - data.c) * data.SigmaAllow * data.InputData.fi_t;

                }
                else
                {
                    data.F_dp = Math.PI * (data.InputData.D + data.InputData.s - data.c) * (data.InputData.s - data.c) * data.SigmaAllow;
                    data.F_de1 = 0.000031 * data.E / data.InputData.ny * Math.Pow(data.InputData.D, 2) *
                             Math.Pow(100 * (data.InputData.s - data.c) / data.InputData.D, 2.5);

                    const int L_MORE_THEN_D = 10;
                    bool isLMoreThenD = data.InputData.l / data.InputData.D > L_MORE_THEN_D;

                    if (isLMoreThenD || data.InputData.ConditionForCalcF5341)
                    {
                        switch (data.InputData.FCalcSchema)
                        {
                            case 1:
                                data.lpr = data.InputData.l;
                                break;
                            case 2:
                                data.lpr = 2 * data.InputData.l;
                                break;
                            case 3:
                                data.lpr = 0.7 * data.InputData.l;
                                break;
                            case 4:
                                data.lpr = 0.5 * data.InputData.l;
                                break;
                            case 5:
                                data.F = data.InputData.q * data.InputData.l;
                                data.lpr = 1.12 * data.InputData.l;
                                break;
                            case 6:
                                double fDivl6 = data.InputData.f / data.InputData.l;
                                fDivl6 *= 10;
                                fDivl6 = Math.Round(fDivl6 / 2.0);
                                fDivl6 *= 0.2;
                                data.lpr = fDivl6 switch
                                {
                                    0 => 2 * data.InputData.l,
                                    0.2 => 1.73 * data.InputData.l,
                                    0.4 => 1.47 * data.InputData.l,
                                    0.6 => 1.23 * data.InputData.l,
                                    0.8 => 1.06 * data.InputData.l,
                                    1 => data.InputData.l,
                                    _ => data.lpr
                                };
                                break;
                            case 7:
                                double fDivl7 = data.InputData.f / data.InputData.l;
                                fDivl7 *= 10;
                                fDivl7 = Math.Round(fDivl7 / 2);
                                fDivl7 *= 0.2;
                                data.lpr = fDivl7 switch
                                {
                                    0 => 2 * data.InputData.l,
                                    0.2 => 1.7 * data.InputData.l,
                                    0.4 => 1.4 * data.InputData.l,
                                    0.6 => 1.11 * data.InputData.l,
                                    0.8 => 0.85 * data.InputData.l,
                                    1 => 0.7 * data.InputData.l,
                                    _ => data.lpr
                                };
                                break;

                        }

                        data.lambda = 2.83 * data.lpr / (data.InputData.D + data.InputData.s - data.c);
                        data.F_de2 = Math.PI * (data.InputData.D + data.InputData.s - data.c) * (data.InputData.s - data.c) * data.E / data.InputData.ny *
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

            if (data.InputData.M > 0 && data.InputData.s != 0.0)
            {

                data.M_dp = Math.PI / 4 * data.InputData.D * (data.InputData.D + data.InputData.s - data.c) * (data.InputData.s - data.c) * data.SigmaAllow;
                data.M_de = 0.000089 * data.E / data.InputData.ny * Math.Pow(data.InputData.D, 3) * Math.Pow(100 * (data.InputData.s - data.c) / data.InputData.D, 2.5);
                data.M_d = data.M_dp / Math.Sqrt(1 + Math.Pow(data.M_dp / data.M_de, 2));

            }

            if (data.InputData.Q > 0 && data.InputData.s != 0.0)
            {
                data.Q_dp = 0.25 * data.SigmaAllow * Math.PI * data.InputData.D * (data.InputData.s - data.c);
                data.Q_de = 2.4 * data.E * Math.Pow(data.InputData.s - data.c, 2) / data.InputData.ny *
                        (0.18 + 3.3 * data.InputData.D * (data.InputData.s - data.c) / Math.Pow(data.InputData.l, 2));
                data.Q_d = data.Q_dp / Math.Sqrt(1 + Math.Pow(data.Q_dp / data.Q_de, 2));
            }

            data.ConditionStability = data.InputData.p / data.p_d +
                                  (data.InputData.FCalcSchema == 5 ? data.F : data.InputData.F) / data.FAllow +
                                  data.InputData.M / data.M_d +
                                  Math.Pow(data.InputData.Q / data.Q_d, 2);
            if (data.ConditionStability > 1)
            {
                data.ErrorList.Add("Условие устойчивости для совместного действия усилий не выполняется");
            }

            return data;
        }
    }
}
