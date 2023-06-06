using CalculateVessels.Core.Exceptions;
using CalculateVessels.Core.Helpers;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Base;
using CalculateVessels.Core.Shells.Enums;
using CalculateVessels.Data.Interfaces;
using System;
using System.Linq;

namespace CalculateVessels.Core.Shells.Elliptical;

internal class EllipticalShellCalculateService : ICalculateService<EllipticalShellInput>
{
    private readonly IPhysicalDataService _physicalData;

    public EllipticalShellCalculateService(IPhysicalDataService physicalData)
    {
        _physicalData = physicalData;
        Name = "GOST 34233.2-2017";
    }
    public string Name { get; }

    public ICalculatedElement Calculate(EllipticalShellInput dataIn)
    {
        var commonData = CalculateCommonData(dataIn);
        var results = dataIn.LoadingConditions
            .Select(lc => CalculateOneLoadingCondition(dataIn, commonData, lc))
            .ToList();

        var data = new EllipticalShellCalculated(commonData, results)
        {
            InputData = dataIn,
            //CommonData = commonData,
            //Results = dataIn.LoadingConditions
            //    .Select(lc => CalculateOneLoadingCondition(dataIn, commonData, lc))
            //    .ToList()
        };

        if (data.CommonData.ErrorList.Any())
        {
            data.AddErrors(data.CommonData.ErrorList);
        }

        if (data.Results.Any(r => r.ErrorList.Any()))
        {
            data.AddErrors(data.Results.SelectMany(r => r.ErrorList));
        }

        return data;
    }

    private static EllipticalShellCalculatedCommon CalculateCommonData(EllipticalShellInput dataIn)
    {
        var data = new EllipticalShellCalculatedCommon
        {
            c = dataIn.c1 + dataIn.c2 + dataIn.c3,
            EllipseR = Math.Pow(dataIn.D, 2) / (4.0 * dataIn.EllipseH)
        };

        if (dataIn.s != 0)
        {
            const double conditionUseFormulas1Min = 0.002,
                conditionUseFormulas1Max = 0.1,
                conditionUseFormulas2Min = 0.2,
                conditionUseFormulas2Max = 0.5;

            data.IsConditionUseFormulas =
                (dataIn.s - data.c) / dataIn.D <= conditionUseFormulas1Max &
                (dataIn.s - data.c) / dataIn.D >= conditionUseFormulas1Min &
                dataIn.EllipseH / dataIn.D < conditionUseFormulas2Max &
                dataIn.EllipseH / dataIn.D >= conditionUseFormulas2Min;

            if (!data.IsConditionUseFormulas)
            {
                data.ErrorList.Add("Условие применения формул не выполняется");
            }
        }

        return data;
    }

    private EllipticalShellCalculatedOneLoading CalculateOneLoadingCondition(EllipticalShellInput dataIn,
        EllipticalShellCalculatedCommon cdc,
        LoadingCondition loadingCondition)
    {
        var data = new EllipticalShellCalculatedOneLoading
        {
            LoadingCondition = loadingCondition,
            SigmaAllow = PhysicalHelper.GetSigmaIfZero(loadingCondition.SigmaAllow, dataIn.Steel, loadingCondition.t, _physicalData),
            E = PhysicalHelper.GetEIfZero(loadingCondition.EAllow, dataIn.Steel, loadingCondition.t, _physicalData),
        };

        switch (dataIn.EllipticalBottomType)
        {
            case EllipticalBottomType.Elliptical:
            case EllipticalBottomType.Hemispherical:

                if (loadingCondition.IsPressureIn)
                {
                    data.s_p = loadingCondition.p * cdc.EllipseR /
                               (2.0 * data.SigmaAllow * dataIn.phi - 0.5 * loadingCondition.p);
                    data.s = data.s_p + cdc.c;

                    if (dataIn.s != 0.0)
                    {
                        if (data.s > dataIn.s)
                            throw new CalculateException("Принятая толщина меньше расчетной.", loadingCondition);

                        data.p_d = 2.0 * data.SigmaAllow * dataIn.phi *
                                   (dataIn.s - cdc.c) /
                                   (cdc.EllipseR + 0.5 * (dataIn.s - cdc.c));
                    }
                }
                else
                {
                    data.EllipseKePrev = dataIn.EllipticalBottomType switch
                    {
                        EllipticalBottomType.Elliptical => 0.9,
                        EllipticalBottomType.Hemispherical => 1.0,
                        _ => throw new CalculateException("Неверный тип днища.", loadingCondition)
                    };

                    data.s_p_1 = data.EllipseKePrev * cdc.EllipseR / 161 *
                                 Math.Sqrt(dataIn.ny * loadingCondition.p / (0.00001 * data.E));
                    data.s_p_2 = 1.2 * loadingCondition.p * cdc.EllipseR / (2.0 * data.SigmaAllow);

                    data.s_p = Math.Max(data.s_p_1, data.s_p_2);
                    data.s = data.s_p + cdc.c;

                    if (dataIn.s != 0.0)
                    {
                        if (data.s > dataIn.s)
                            throw new CalculateException("Принятая толщина меньше расчетной.", loadingCondition);

                        data.p_dp = 2.0 * data.SigmaAllow * (dataIn.s - cdc.c) /
                                    (cdc.EllipseR + 0.5 * (dataIn.s - cdc.c));
                        data.Ellipsex = 10.0 * ((dataIn.s - cdc.c) / dataIn.D) *
                                        (dataIn.D / (2.0 * dataIn.EllipseH) -
                                         2.0 * dataIn.EllipseH / dataIn.D);
                        data.EllipseKe = (1.0 + (2.4 + 8.0 * data.Ellipsex) * data.Ellipsex) /
                                         (1.0 + (3.0 + 10.0 * data.Ellipsex) * data.Ellipsex);
                        data.p_de = 2.6 * 0.00001 * data.E / dataIn.ny *
                                    Math.Pow(100 * (dataIn.s - cdc.c) / (data.EllipseKe * cdc.EllipseR),
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