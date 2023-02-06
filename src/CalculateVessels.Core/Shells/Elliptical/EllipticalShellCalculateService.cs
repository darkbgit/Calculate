using CalculateVessels.Core.Exceptions;
using CalculateVessels.Core.Helpers;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Enums;
using System;

namespace CalculateVessels.Core.Shells.Elliptical;

internal class EllipticalShellCalculateService : ICalculateService<EllipticalShellInput>
{
    public EllipticalShellCalculateService()
    {
        Name = "GOST 34233.2-2017";
    }
    public string Name { get; }

    public ICalculatedElement Calculate(EllipticalShellInput dataIn)
    {
        var data = new EllipticalShellCalculated
        {
            InputData = dataIn,
            SigmaAllow = PhysicalHelper.GetSigmaIfZero(dataIn.SigmaAllow, dataIn.Steel, dataIn.t),
            E = PhysicalHelper.GetEIfZero(dataIn.E, dataIn.Steel, dataIn.t),
            c = dataIn.c1 + dataIn.c2 + dataIn.c3
        };

        switch (dataIn.EllipticalBottomType)
        {
            case EllipticalBottomType.Elliptical:
            case EllipticalBottomType.Hemispherical:
                if (dataIn.s != 0)
                {
                    const double CONDITION_USE_FORMULAS_1_MIN = 0.002,
                        CONDITION_USE_FORMULAS_1_MAX = 0.1,
                        CONDITION_USE_FORMULAS_2_MIN = 0.2,
                        CONDITION_USE_FORMULAS_2_MAX = 0.5;

                    data.IsConditionUseFormulas =
                        (dataIn.s - data.c) / dataIn.D <= CONDITION_USE_FORMULAS_1_MAX &
                        (dataIn.s - data.c) / dataIn.D >= CONDITION_USE_FORMULAS_1_MIN &
                        dataIn.EllipseH / dataIn.D < CONDITION_USE_FORMULAS_2_MAX &
                        dataIn.EllipseH / dataIn.D >= CONDITION_USE_FORMULAS_2_MIN;

                    if (!data.IsConditionUseFormulas)
                    {
                        data.ErrorList.Add("Условие применения формул не выполняется");
                    }
                }

                data.EllipseR = Math.Pow(dataIn.D, 2) / (4.0 * dataIn.EllipseH);

                if (dataIn.IsPressureIn)
                {
                    data.s_p = dataIn.p * data.EllipseR /
                               (2.0 * data.SigmaAllow * dataIn.fi - 0.5 * dataIn.p);
                    data.s = data.s_p + data.c;

                    if (dataIn.s != 0.0)
                    {
                        if (data.s > dataIn.s)
                            throw new CalculateException("Принятая толщина меньше расчетной.");

                        data.p_d = 2.0 * data.SigmaAllow * dataIn.fi *
                                   (dataIn.s - data.c) /
                                   (data.EllipseR + 0.5 * (dataIn.s - data.c));
                    }
                }
                else
                {
                    data.EllipseKePrev = dataIn.EllipticalBottomType switch
                    {
                        EllipticalBottomType.Elliptical => 0.9,
                        EllipticalBottomType.Hemispherical => 1.0,
                        _ => throw new CalculateException("Неверный тип днища.")
                    };

                    data.s_p_1 = data.EllipseKePrev * data.EllipseR / 161 *
                                 Math.Sqrt(dataIn.ny * dataIn.p / (0.00001 * data.E));
                    data.s_p_2 = 1.2 * dataIn.p * data.EllipseR / (2.0 * data.SigmaAllow);

                    data.s_p = Math.Max(data.s_p_1, data.s_p_2);
                    data.s = data.s_p + data.c;

                    if (dataIn.s != 0.0)
                    {
                        if (data.s > dataIn.s)
                            throw new CalculateException("Принятая толщина меньше расчетной.");

                        data.p_dp = 2.0 * data.SigmaAllow * (dataIn.s - data.c) /
                                    (data.EllipseR + 0.5 * (dataIn.s - data.c));
                        data.Ellipsex = 10.0 * ((dataIn.s - data.c) / dataIn.D) *
                                        (dataIn.D / (2.0 * dataIn.EllipseH) -
                                         2.0 * dataIn.EllipseH / dataIn.D);
                        data.EllipseKe = (1.0 + (2.4 + 8.0 * data.Ellipsex) * data.Ellipsex) /
                                         (1.0 + (3.0 + 10.0 * data.Ellipsex) * data.Ellipsex);
                        data.p_de = 2.6 * 0.00001 * data.E / dataIn.ny *
                                    Math.Pow(100 * (dataIn.s - data.c) / (data.EllipseKe * data.EllipseR),
                                        2);
                        data.p_d = data.p_dp / Math.Sqrt(1.0 + Math.Pow(data.p_dp / data.p_de, 2));
                    }
                }

                break;
            case EllipticalBottomType.Torospherical:
                //TODO: Add calculate torospherical bottom
                throw new NotImplementedException();
            case EllipticalBottomType.SphericalUnflanged:
                //TODO Add calculate spherical unflanged bottom
                throw new NotImplementedException();
            default:
                throw new CalculateException("Неверный тип днища.");
        }

        return data;
    }
}