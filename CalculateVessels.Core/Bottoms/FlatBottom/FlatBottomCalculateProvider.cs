using CalculateVessels.Core.Bottoms.Enums;
using CalculateVessels.Core.Exceptions;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.PhysicalData;
using System;

namespace CalculateVessels.Core.Bottoms.FlatBottom
{
    public class FlatBottomCalculateProvider : ICalculateProvider
    {
        public ICalculatedData Calculate(IInputData inputData)
        {
            if (inputData is not FlatBottomInputData dataIn)
                throw new CalculateException("Error. Input data wrong type.");

            var data = new FlatBottomCalculatedData(dataIn);

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


            data.c = dataIn.c1 + dataIn.c2 + dataIn.c3;

            switch (dataIn.Type)
            {
                case 1:
                    data.K = 0.53;
                    data.Dp = dataIn.D;
                    if (dataIn.a < 1.7 * dataIn.s)
                    {
                        data.IsConditionFixed = false;
                        data.ErrorList.Add("Условие закрепления не выполняется a>=1.7s");
                    }
                    break;
                case 2:
                    data.K = 0.50;
                    data.Dp = dataIn.D;
                    if (dataIn.a < 0.85 * dataIn.s)
                    {
                        data.IsConditionFixed = false;
                        data.ErrorList.Add("Условие закрепления не выполняется a>=0.85s");
                    }
                    break;
                case 3:
                    data.Dp = dataIn.D;
                    data.K = (dataIn.s - data.c) / (dataIn.s1 - data.c) < 0.25 ? 0.45 : 0.41;
                    break;
                case 4:
                    data.Dp = dataIn.D;
                    data.K = (dataIn.s - data.c) / (dataIn.s1 - data.c) < 0.5 ? 0.41 : 0.38;
                    break;
                case 5:
                    goto case 3;
                case 6:
                    goto case 2;
                case 7:
                case 8:
                    goto case 4;
                case 9:
                    data.Dp = dataIn.D - 2 * dataIn.r;
                    if (dataIn.h1 < dataIn.r ||
                        dataIn.r < Math.Max(dataIn.s, 025 * dataIn.s1) ||
                        dataIn.r > Math.Min(dataIn.s1, 0.1 * dataIn.D))
                    {
                        data.IsConditionFixed = false;
                        data.ErrorList.Add("Условие закрепления не выполняется");
                    }
                    data.K_1 = 0.41 * (1.0 - 0.23 * ((dataIn.s - data.c) / (dataIn.s1 - data.c)));
                    data.K = Math.Max(data.K_1, 0.35);
                    break;
                case 10:
                    if (dataIn.gamma < 30 || dataIn.gamma > 90 ||
                        dataIn.r < 0.25 * dataIn.s1 || dataIn.r > (dataIn.s1 - dataIn.s2))
                    {
                        data.IsConditionFixed = false;
                        data.ErrorList.Add("Условие закрепления не выполняется");
                    }

                    data.s2p_1 = 1.1 * (dataIn.s - data.c);
                    data.s2p_2 = (dataIn.s1 - data.c) /
                             (1 + (data.Dp - 2 * dataIn.r) / (1.2 * (dataIn.s1 - data.c) * Math.Sin(dataIn.gamma * Math.PI / 180)));
                    data.s2 = Math.Max(data.s2p_1, data.s2p_2) + data.c;
                    if (dataIn.s2 < data.s2)
                    {
                        data.ErrorList.Add("Принятая толщина s2 меньше расчетной");
                    }
                    goto case 4;
                case 11:
                case 12:
                    if (dataIn.Type == 11)
                    {
                        data.K = 0.4;
                        data.Dp = dataIn.D3;
                    }
                    else
                    {
                        data.K = 0.41;
                        data.Dp = dataIn.Dcp;
                    }
                    data.s2p_1 = 0.7 * (dataIn.s1 - data.c);
                    data.s2p_2 = (dataIn.s1 - data.c) * Math.Sqrt(2 * (data.Dp - dataIn.D2) * dataIn.D2 / Math.Pow(dataIn.D2, 2));
                    data.s2p = Math.Max(data.s2p_1, data.s2p_2);
                    data.s2 = data.s2p + data.c;
                    if (dataIn.s2 < data.s2)
                    {
                        data.ErrorList.Add("Принятая толщина s2 меньше расчетной");
                    }
                    break;
            }

            switch (dataIn.Hole)
            {
                case HoleInFlatBottom.WithoutHole:
                    data.K0 = 1;
                    break;
                case HoleInFlatBottom.OneHole:
                    data.K0 = Math.Sqrt(1.0 + dataIn.d / data.Dp + Math.Pow(dataIn.d / data.Dp, 2));
                    break;
                case HoleInFlatBottom.MoreThenOneHole:
                    if (dataIn.di > 0.7 * data.Dp)
                    {
                        data.ErrorList.Add("Слишком много отверстий");
                    }
                    data.K0 = Math.Sqrt((1 - Math.Pow(dataIn.di / data.Dp, 3)) / (1 - dataIn.di / data.Dp));
                    break;
                default:
                    data.ErrorList.Add("Ошибка определения количества отверстий");
                    break;
            }

            data.s1p = data.K * data.K0 * data.Dp * Math.Sqrt(dataIn.p / (dataIn.fi * data.SigmaAllow));
            data.s1 = data.s1p + data.c;

            if (dataIn.s1 != 0.0)
            {
                if (dataIn.s1 >= data.s1p)
                {
                    data.ConditionUseFormulas = (dataIn.s1 - data.c) / data.Dp;
                    data.IsConditionUseFormulas = data.ConditionUseFormulas <= 0.11;
                    data.Kp = data.IsConditionUseFormulas
                         ? 1
                         : 2.2 / (1 + Math.Sqrt(1 + Math.Pow(6 * (dataIn.s1 - data.c) / data.Dp, 2)));

                    data.p_d = Math.Pow((dataIn.s1 - data.c) / (data.K * data.K0 * data.Dp), 2) * data.SigmaAllow * dataIn.fi;
                    if (data.Kp * data.p_d < dataIn.p)
                    {
                        data.ErrorList.Add("Допускаемое давление меньше расчетного");
                    }
                }
                else
                {
                    throw new CalculateException("Принятая толщина s1 меньше расчетной");
                }
            }

            return data;
        }
    }
}