using CalculateVessels.Core.Exceptions;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Exceptions;
using CalculateVessels.Data.PhysicalData;
using CalculateVessels.Data.PhysicalData.Gost34233_1;
using System;

namespace CalculateVessels.Core.Supports.BracketVertical
{
    internal class BracketVerticalCalculateProvider : ICalculateProvider
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        /// <exception cref="CalculateException"></exception>
        public ICalculatedData Calculate(IInputData inputData)
        {
            if (inputData is not BracketVerticalInputData dataIn)
                throw new CalculateException("Error. Input data wrong type.");

            var data = new BracketVerticalCalculatedData(dataIn);

            if (dataIn.SigmaAlloy == 0)
            {
                try
                {
                    data.SigmaAlloy = Gost34233_1.GetSigma(dataIn.Steel, dataIn.t);
                }
                catch (PhysicalDataException e)
                {
                    Console.WriteLine(e);
                    throw new CalculateException("Error get sigma.", e);
                }
            }
            else
            {
                data.SigmaAlloy = dataIn.SigmaAlloy;
            }

            data.Dp = dataIn.D; //TODO make Dp for different types of shells

            data.e1 = dataIn.e1 == 0 ? dataIn.e1 : 5.0 / 6.0 * dataIn.l1;

            data.F1 = dataIn.n switch
            {
                4 when dataIn.PreciseMontage => dataIn.G / 4.0 + dataIn.M /
                    (data.Dp + 2 * (data.e1 + dataIn.s + dataIn.s2)),
                2 or 4 => dataIn.G / 2.0 + dataIn.M /
                    (data.Dp + 2 * (data.e1 + dataIn.s + dataIn.s2)),
                3 => dataIn.G / 3.0 + dataIn.M / 0.75 *
                    (data.Dp + 2 * (data.e1 + dataIn.s + dataIn.s2)),
                _ => throw new CalculateException("Error number of vertical brackets.")
            };

            data.Q1 = dataIn.Q / dataIn.n;

            data.e1e = data.e1 + data.Q1 * dataIn.h / data.F1;

            data.x = Math.Log(data.Dp / (2 * (dataIn.c - dataIn.c)));

            data.y = Math.Log(dataIn.h1 / data.Dp);


            if (!dataIn.ReinforceingPad)
            {
                data.z = Math.Log(dataIn.b4 / data.Dp);

                switch (dataIn.Type)
                {
                    case BracketVerticalType.A:
                    case BracketVerticalType.C:
                        data.K7 = Math.Exp((-5.964 - 11.395 * data.x - 18.984 * data.y -
                                            2.413 * Math.Pow(data.x, 2) - 7.286 * data.x * data.y -
                                            2.042 * Math.Pow(data.y, 2) +
                                            0.1322 * Math.Pow(data.x, 3) + 0.4833 * Math.Pow(data.x, 2) * data.y +
                                            0.8469 * data.x * Math.Pow(data.y, 2) + 1.428 * Math.Pow(data.y, 3)) *
                                           0.01);
                        break;
                    case BracketVerticalType.B:
                        data.K71 = Math.Exp((-26.791 - 6.936 * data.x - 36.33 * data.y -
                                             3.503 * Math.Pow(data.x, 2) - 3.357 * data.x * data.y +
                                             2.786 * Math.Pow(data.y, 2) +
                                             0.2267 * Math.Pow(data.x, 3) + 0.2831 * Math.Pow(data.x, 2) * data.y +
                                             0.3851 * data.x * Math.Pow(data.y, 2) + 1.37 * Math.Pow(data.y, 3)) *
                                            0.01);
                        data.K72 = Math.Exp((-5.964 - 11.395 * data.x - 18.984 * data.y -
                                             2.413 * Math.Pow(data.x, 2) - 7.286 * data.x * data.y -
                                             2.042 * Math.Pow(data.y, 2) +
                                             0.1322 * Math.Pow(data.x, 3) + 0.4833 * Math.Pow(data.x, 2) * data.y +
                                             0.8469 * data.x * Math.Pow(data.y, 2) + 1.428 * Math.Pow(data.y, 3)) *
                                            0.01);
                        data.K7 = Math.Min(data.K71, data.K72);
                        break;
                    case BracketVerticalType.D:
                        data.K7 = Math.Exp((-29.532 - 45.958 * data.x - 91.759 * data.z -
                                            1.801 * Math.Pow(data.x, 2) - 12.062 * data.x * data.z -
                                            18.872 * Math.Pow(data.z, 2) +
                                            0.1551 * Math.Pow(data.x, 3) + 1.617 * Math.Pow(data.x, 2) * data.z +
                                            3.736 * data.x * Math.Pow(data.z, 2) + 1.425 * Math.Pow(data.z, 3)) *
                                           0.01);
                        break;
                    default:
                        throw new CalculateException("Error bracket vertical type.");
                }

                data.K2 = dataIn.IsAssembly ? 1.05 : 1.25;

                data.v1 = 0.3;

                switch (dataIn.Type)
                {
                    case BracketVerticalType.A:
                    case BracketVerticalType.B:
                    case BracketVerticalType.C:
                        data.sigma_m = dataIn.p * data.Dp /
                                       (2 * (dataIn.s - dataIn.c));
                        break;
                    case BracketVerticalType.D:
                        data.sigma_m = dataIn.p * data.Dp / (4 * (dataIn.s - dataIn.c)) +
                                       1 / (Math.PI * data.Dp * (dataIn.s - dataIn.c)) *
                                       (data.F1 + 4 * dataIn.M / data.Dp);
                        break;
                    default:
                        throw new CalculateException("Error bracket vertical type.");
                }

                data.v2 = -data.sigma_m / (data.K2 * data.SigmaAlloy * dataIn.fi);

                data.K1 = (1 - Math.Pow(data.v2, 2)) /
                          (1.0 / 3.0 + data.v1 * data.v2 +
                           Math.Sqrt(Math.Pow(1.0 / 3.0 + data.v1 * data.v2, 2) +
                                     (1 - Math.Pow(data.v2, 2)) * Math.Pow(data.v1, 2)));

                data.sigmaid = data.K1 * data.K2 * data.SigmaAlloy;

                data.F1Alloy = data.sigmaid * dataIn.h1 *
                    Math.Pow(dataIn.s - dataIn.c, 2) / (data.K7 * data.e1e);

                if (dataIn.g / dataIn.h1 < 0.5)
                {
                    data.F1Alloy *= 0.5 + dataIn.g / dataIn.h1;
                }
            }
            else
            {
                data.y1 = Math.Log(dataIn.b3 / data.Dp);

                data.K81 = Math.Exp((-49.919 - 39.119 * data.x - 107.01 * data.y1 -
                                        1.693 * Math.Pow(data.x, 2) - 11.92 * data.x * data.y1 -
                                        39.276 * Math.Pow(data.y1, 2) +
                                        0.237 * Math.Pow(data.x, 3) + 1.608 * Math.Pow(data.x, 2) * data.y1 +
                                        2.761 * data.x * Math.Pow(data.y1, 2) - 3.854 * Math.Pow(data.y1, 3)) *
                                    0.01);

                data.K82 = Math.Exp((-5.964 - 11.395 * data.x - 18.984 * data.y -
                                     2.413 * Math.Pow(data.x, 2) - 7.286 * data.x * data.y -
                                     2.042 * Math.Pow(data.y, 2) +
                                     0.1322 * Math.Pow(data.x, 3) + 0.4833 * Math.Pow(data.x, 2) * data.y +
                                     0.8469 * data.x * Math.Pow(data.y, 2) + 1.428 * Math.Pow(data.y, 3)) *
                                    0.01);

                data.K8 = Math.Min(data.K81, data.K82);

                data.K2 = dataIn.IsAssembly ? 1.05 : 1.25;

                data.v1 = 0.4;

                data.sigma_m = dataIn.p * data.Dp /
                               (2 * (dataIn.s - dataIn.c));

                data.v2 = -data.sigma_m / (data.K2 * data.SigmaAlloy * dataIn.fi);

                data.K1 = (1 - Math.Pow(data.v2, 2)) /
                          (1.0 / 3.0 + data.v1 * data.v2 +
                           Math.Sqrt(Math.Pow(1.0 / 3.0 + data.v1 * data.v2, 2) +
                                     (1 - Math.Pow(data.v2, 2)) * Math.Pow(data.v1, 2)));

                data.sigmaid = data.K1 * data.K2 * data.SigmaAlloy;

                data.F1Alloy = data.sigmaid * dataIn.b3 *
                    Math.Pow(dataIn.s - dataIn.c, 2) / (data.K8 * (data.e1e + dataIn.s2));

                if (dataIn.b2 / dataIn.b3 < 0.6)
                {
                    data.F1Alloy *= 0.4 + dataIn.b2 / dataIn.b3;
                }
            }

            if (data.F1 > data.F1Alloy)
            {
                data.ErrorList.Add("Несущая способность обечайки в месте приварки опорной лапы " +
                               (dataIn.ReinforceingPad ? "с подкладным листом" : "без подкладного листа.") +
                               " Условие прочности не выполняется");
            }

            data.IsConditionUseFormulas = true;

            data.ConditionUseFormulas1 = (dataIn.s - dataIn.c) / data.Dp;
            if (data.ConditionUseFormulas1 > 0.05)
            {
                data.IsConditionUseFormulas = false;
                data.ErrorList.Add("Условие применения формул не выполняется");
            }

            data.ConditionUseFormulas2 = 0.2 * dataIn.h1;
            if (data.ConditionUseFormulas2 > dataIn.g)
            {
                data.IsConditionUseFormulas = false;
                data.ErrorList.Add("Условие применения формул не выполняется");
            }

            data.ConditionUseFormulas3 = dataIn.h1 / data.Dp;
            if (data.ConditionUseFormulas3 is < 0.04 or > 0.5)
            {
                data.IsConditionUseFormulas = false;
                data.ErrorList.Add("Условие применения формул не выполняется");
            }

            data.ConditionUseFormulas4 = dataIn.b4 / data.Dp;
            if (data.ConditionUseFormulas4 is < 0.04 or > 0.5)
            {
                data.IsConditionUseFormulas = false;
                data.ErrorList.Add("Условие применения формул не выполняется");
            }

            if (dataIn.ReinforceingPad)
            {
                data.ConditionUseFormulas5 = dataIn.b3 / data.Dp;
                if (data.ConditionUseFormulas5 is < 0.04 or > 0.8)
                {
                    data.IsConditionUseFormulas = false;
                    data.ErrorList.Add("Условие применения формул не выполняется");
                }

                data.ConditionUseFormulas6 = 0.6 * dataIn.b3;
                if (data.ConditionUseFormulas6 > dataIn.b2)
                {
                    data.IsConditionUseFormulas = false;
                    data.ErrorList.Add("Условие применения формул не выполняется");
                }

                data.ConditionUseFormulas7 = 1.5 * dataIn.h1;
                if (data.ConditionUseFormulas7 < dataIn.b3)
                {
                    data.IsConditionUseFormulas = false;
                    data.ErrorList.Add("Условие применения формул не выполняется");
                }

                if (dataIn.s2 < dataIn.s)
                {
                    data.IsConditionUseFormulas = false;
                    data.ErrorList.Add("Условие применения формул не выполняется");
                }
            }

            return data;
        }
    }
}
