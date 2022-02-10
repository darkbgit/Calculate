using System;
using CalculateVessels.Core.Exceptions;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Enums;
using CalculateVessels.Data.PhysicalData;

namespace CalculateVessels.Core.Shells.Elliptical
{
    internal class EllipticalShellCalculateProvider : ICalculateProvider
    {
        public ICalculatedData Calculate(IInputData inputData)
        {
            if (inputData is not EllipticalShellInputData dataIn)
                throw new CalculateException("Error. Input data wrong type.");

            var data = new EllipticalShellCalculatedData(dataIn);

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

            switch (data.InputData.EllipticalBottomType)
            {
                case EllipticalBottomType.Elliptical:
                case EllipticalBottomType.Hemispherical:
                {
                    const double CONDITION_USE_FORMULAS_1_MIN = 0.002,
                        CONDITION_USE_FORMULAS_1_MAX = 0.1,
                        CONDITION_USE_FORMULAS_2_MIN = 0.2,
                        CONDITION_USE_FORMULAS_2_MAX = 0.5;

                    data.IsConditionUseFormulas =
                        (data.InputData.s - data.c) / data.InputData.D <= CONDITION_USE_FORMULAS_1_MAX &
                        (data.InputData.s - data.c) / data.InputData.D >= CONDITION_USE_FORMULAS_1_MIN &
                        data.InputData.EllipseH / data.InputData.D < CONDITION_USE_FORMULAS_2_MAX &
                        data.InputData.EllipseH / data.InputData.D >= CONDITION_USE_FORMULAS_2_MIN |
                        data.InputData.s == 0;

                    if (!data.IsConditionUseFormulas)
                    {
                        data.ErrorList.Add("Условие применения формул не выполняется");
                    }

                    data.EllipseR = Math.Pow(data.InputData.D, 2) / (4.0 * data.InputData.EllipseH);

                    if (data.InputData.IsPressureIn)
                    {
                        data.s_p = data.InputData.p * data.EllipseR /
                                   (2.0 * data.InputData.SigmaAllow * data.InputData.fi - 0.5 * data.InputData.p);
                        data.s = data.s_p + data.c;

                        if (data.InputData.s != 0.0)
                        {
                            if (data.s < data.InputData.s)
                                throw new CalculateException("Принятая толщина меньше расчетной.");

                            data.p_d = 2.0 * data.InputData.SigmaAllow * data.InputData.fi *
                                       (data.InputData.s - data.c) /
                                       (data.EllipseR + 0.5 * (data.InputData.s - data.c));
                        }
                    }
                    else
                    {
                        data.EllipseKePrev = data.InputData.EllipticalBottomType switch
                        {
                            EllipticalBottomType.Elliptical => 0.9,
                            EllipticalBottomType.Hemispherical => 1.0,
                            _ => throw new CalculateException("Неверный тип днища.")
                        };

                        data.s_p_1 = data.EllipseKePrev * data.EllipseR / 161 *
                                     Math.Sqrt(data.InputData.ny * data.InputData.p / (0.00001 * data.E));
                        data.s_p_2 = 1.2 * data.InputData.p * data.EllipseR / (2.0 * data.InputData.SigmaAllow);

                        data.s_p = Math.Max(data.s_p_1, data.s_p_2);
                        data.s = data.s_p + data.c;

                        if (data.InputData.s != 0.0)
                        {
                            if (data.s < data.InputData.s)
                                throw new CalculateException("Принятая толщина меньше расчетной.");

                            data.p_dp = 2.0 * data.InputData.SigmaAllow * (data.InputData.s - data.c) /
                                        (data.EllipseR + 0.5 * (data.InputData.s - data.c));
                            data.Ellipsex = 10.0 * ((data.InputData.s - data.c) / data.InputData.D) *
                                            (data.InputData.D / (2.0 * data.InputData.EllipseH) -
                                             2.0 * data.InputData.EllipseH / data.InputData.D);
                            data.EllipseKe = (1.0 + (2.4 + 8.0 * data.Ellipsex) * data.Ellipsex) /
                                             (1.0 + (3.0 + 10.0 * data.Ellipsex) * data.Ellipsex);
                            data.p_de = 2.6 * 0.00001 * data.E / data.InputData.ny *
                                        Math.Pow(100 * (data.InputData.s - data.c) / (data.EllipseKe * data.EllipseR),
                                            2);
                            data.p_d = data.p_dp / Math.Sqrt(1.0 + Math.Pow(data.p_dp / data.p_de, 2));
                        }
                    }

                    break;
                }
                case EllipticalBottomType.Torospherical:
                {
                    //TODO: Add calculate torospherical bottom
                    break;
                }

                case EllipticalBottomType.SphericalUnflanged:
                {
                    //TODO Add calculate spherical unflanged bottom
                    break;
                }

                default:
                {
                    throw new CalculateException("Неверный тип днища.");
                }
            }

            return data;
        }
    }
}
