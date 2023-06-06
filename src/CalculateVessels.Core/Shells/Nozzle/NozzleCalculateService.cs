using CalculateVessels.Core.Exceptions;
using CalculateVessels.Core.Helpers;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Base;
using CalculateVessels.Core.Shells.Conical;
using CalculateVessels.Core.Shells.Cylindrical;
using CalculateVessels.Core.Shells.Elliptical;
using CalculateVessels.Core.Shells.Enums;
using CalculateVessels.Core.Shells.Nozzle.Enums;
using CalculateVessels.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculateVessels.Core.Shells.Nozzle;

internal class NozzleCalculateService : ICalculateService<NozzleInput>
{
    private readonly IPhysicalDataService _physicalData;

    public NozzleCalculateService(IPhysicalDataService physicalData)
    {
        _physicalData = physicalData;
        Name = "GOST 34233.3-2017";
    }

    public string Name { get; }

    public ICalculatedElement Calculate(NozzleInput dataIn)
    {
        var commonData = CalculateCommonData(dataIn);
        var results = CalculateResults(dataIn, commonData);

        var data = new NozzleCalculated
        {
            InputData = dataIn,
            CommonData = commonData,
            Results = results
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

    private IEnumerable<NozzleCalculatedOneLoading> CalculateResults(NozzleInput dataIn, NozzleCalculatedCommon commonData)
    {
        var results = ((ShellInputData)dataIn.ShellCalculatedData.InputData).LoadingConditions
            .Select(lc => CalculateOneLoadingCondition(dataIn, commonData, lc))
            .ToList();

        return results;
    }

    private NozzleCalculatedOneLoading CalculateOneLoadingCondition(NozzleInput dataIn, NozzleCalculatedCommon cdc, LoadingCondition lc)
    {
        var data = new NozzleCalculatedOneLoading
        {
            LoadingCondition = lc,
            SigmaAllow1 = PhysicalHelper.GetSigmaIfZero(dataIn.SigmaAllow1, dataIn.steel1, lc.t, _physicalData),
            E1 = PhysicalHelper.GetEIfZero(dataIn.E1, dataIn.steel1, lc.t, _physicalData)
        };

        if (dataIn.NozzleKind is NozzleKind.ImpassWithRing or NozzleKind.PassWithRing
            or NozzleKind.WithRingAndInPart)
        {
            data.SigmaAllow2 = PhysicalHelper.GetSigmaIfZero(dataIn.SigmaAllow2, dataIn.steel2, lc.t, _physicalData);
            data.E2 = PhysicalHelper.GetEIfZero(dataIn.E2, dataIn.steel2, lc.t, _physicalData);
        }

        if (dataIn.NozzleKind is NozzleKind.PassWithoutRing or NozzleKind.PassWithRing or NozzleKind.WithRingAndInPart)
        {
            data.SigmaAllow3 = PhysicalHelper.GetSigmaIfZero(dataIn.SigmaAllow3, dataIn.steel3, lc.t, _physicalData);
            data.E3 = PhysicalHelper.GetEIfZero(dataIn.E3, dataIn.steel3, lc.t, _physicalData);
        }

        //TODO: steel4

        var shellDataIn = (ShellInputData)dataIn.ShellCalculatedData.InputData;


        var shellResult = ((ShellCalculated)dataIn.ShellCalculatedData).Results
            .First(r => r.LoadingCondition.OrdinalNumber == lc.OrdinalNumber);

        data.SigmaAllowShell = shellResult.SigmaAllow;

        if (!lc.IsPressureIn)
        {
            data.p_deShell = shellResult.p_de;
        }

        if (shellDataIn is EllipticalShellInput { ShellType: ShellType.Elliptical } ellipticalDataIn && lc.IsPressureIn)
        {
            data.sp = lc.p * cdc.Dp /
                      (4.0 * ellipticalDataIn.phi * data.SigmaAllowShell - lc.p);
        }
        else
        {
            data.sp = shellResult.s_p;
        }

        if (!dataIn.IsOval)
        {
            data.s1p = lc.p * (dataIn.d + 2 * dataIn.cs) / (2.0 * dataIn.phi * data.SigmaAllow1 - lc.p);
        }
        else
        {
            data.s1p = lc.p * (dataIn.d1 + 2 * dataIn.cs) / (2.0 * dataIn.phi * data.SigmaAllow1 - lc.p);
        }

        if (dataIn.Location == NozzleLocation.LocationAccordingToParagraph_5_2_2_7)
        {
            cdc.dp = dataIn.d + 1.5 * (dataIn.r - data.sp) + 2.0 * dataIn.cs;
        }

        switch (dataIn.NozzleKind)
        {
            case NozzleKind.ImpassWithoutRing:
            case NozzleKind.PassWithoutRing:
            case NozzleKind.ImpassWithRing:
            case NozzleKind.PassWithRing:
            case NozzleKind.WithRingAndInPart:
            case NozzleKind.WithFlanging:
                dataIn.s0 = shellDataIn.s;
                //dataIn.steel4 = dataIn.steel1;
                break;
        }


        data.psi1 = Math.Min(1, data.SigmaAllow1 / data.SigmaAllowShell);
        data.psi2 = Math.Min(1, data.SigmaAllow2 / data.SigmaAllowShell);
        data.psi3 = Math.Min(1, data.SigmaAllow3 / data.SigmaAllowShell);

        data.psi4 = dataIn.NozzleKind switch
        {
            NozzleKind.WithTorusshapedInsert or NozzleKind.WithWealdedRing =>
                Math.Min(1, data.SigmaAllow4 / data.SigmaAllowShell),
            _ => 1,
        };


        //switch (shellDataIn.ShellType)
        //{
        //    case ShellType.Cylindrical:
        //    case ShellType.Conical:
        //        data.K1 = 1;
        //        break;
        //    case ShellType.Elliptical:
        //    case ShellType.Spherical:
        //    case ShellType.Torospherical:
        //        data.K1 = 2;
        //        break;
        //}

        if (lc.IsPressureIn)
        {
            data.spn = data.sp;
        }
        else
        {

            //data.B1n = Math.Min(1, 9.45 * (shellDataIn.D / ddata.out.l) * Math.Sqrt(shellDataIn.D / (100 * (shellDataIn.s - data.c))));
            //data.pen = 2.08 * 0.00001 * shellDataIn.E / (dataIn.ny * data.B1n) * (shellDataIn.D / ddata.out.l) * Math.Pow(100 * (shellDataIn.s - data.c) / shellDataIn.D, 2.5);
            data.pen = data.p_deShell;
            data.ppn = lc.p / Math.Sqrt(1.0 - Math.Pow(lc.p / data.pen, 2));
            data.spn = data.ppn * cdc.Dp / (2.0 * cdc.K1 * data.SigmaAllowShell - data.ppn);
        }


        data.d01 = 2 * ((shellDataIn.s - cdc.c) / data.spn - 0.8) * Math.Sqrt(cdc.Dp * (shellDataIn.s - cdc.c));
        data.d02 = cdc.dmax + 2 * dataIn.cs;
        data.d0 = Math.Min(data.d01, data.d02);

        if (cdc.dp > data.d0)
        {
            data.ConditionStrengthening1 = cdc.l1p * (dataIn.s1 - data.s1p - dataIn.cs) * data.psi1 + cdc.l2p * dataIn.s2 * data.psi2 + cdc.l3p * (dataIn.s3 - dataIn.cs - dataIn.cs1) * data.psi3 + cdc.lp * (shellDataIn.s - data.sp - cdc.c) * data.psi4;
            data.ConditionStrengthening2 = 0.5 * (cdc.dp - cdc.d0p) * data.sp;
            if (data.ConditionStrengthening1 < data.ConditionStrengthening2)
            {
                data.ErrorList.Add("Условие укрепления одиночного отверстия не выполняется.");
            }
        }


        data.V1 = (dataIn.s0 - cdc.c) / (shellDataIn.s - cdc.c);
        data.V2 = (data.psi4 + (cdc.l1p * (dataIn.s1 - dataIn.cs) * data.psi1 + cdc.l2p * dataIn.s2 * data.psi2 + cdc.l3p * (dataIn.s3 - dataIn.cs - dataIn.cs1) * data.psi3) / cdc.lp * (shellDataIn.s - cdc.c)) /
                  (1.0 + 0.5 * (cdc.dp - cdc.d0p) / cdc.lp + cdc.K1 * (dataIn.d + 2 * dataIn.cs) / cdc.Dp * (dataIn.phi / dataIn.phi1) * (cdc.l1p / cdc.lp));
        data.V = Math.Min(data.V1, data.V2);

        if (lc.IsPressureIn)
        {
            data.p_d = 2 * cdc.K1 * dataIn.phi * data.SigmaAllowShell * (shellDataIn.s - cdc.c) * data.V /
                       (cdc.Dp + (shellDataIn.s - cdc.c) * data.V);
        }
        else
        {
            data.p_dp = 2 * cdc.K1 * dataIn.phi * data.SigmaAllowShell * (shellDataIn.s - cdc.c) * data.V /
                        (cdc.Dp + (shellDataIn.s - cdc.c) * data.V);
            data.p_de = data.p_deShell;
            data.p_d = data.p_dp / Math.Sqrt(1 + Math.Pow(data.p_dp / data.p_de, 2));
        }
        if (data.p_d < lc.p)
        {
            data.ErrorList.Add("Допускаемое давление меньше расчетного.");
        }

        //switch (shellDataIn.ShellType)
        //{
        //    case ShellType.Cylindrical:
        //        data.ConditionUseFormulas1 = (cdc.dp - 2 * dataIn.cs) / shellDataIn.D;
        //        data.ConditionUseFormulas2 = (shellDataIn.s - cdc.c) / shellDataIn.D;
        //        data.IsConditionUseFormulas = data.ConditionUseFormulas1 <= 1 & data.ConditionUseFormulas2 <= 0.1;
        //        break;
        //    case ShellType.Conical:
        //        data.ConditionUseFormulas1 = (cdc.dp - 2 * dataIn.cs) / cdc.Dk;
        //        data.ConditionUseFormulas2 = (shellDataIn.s - cdc.c) / cdc.Dk;
        //        data.ConditionUseFormulas2_2 = 0.1 / Math.Cos(Math.PI * 180 * cdc.alpha1);
        //        data.IsConditionUseFormulas = data.ConditionUseFormulas1 <= 1 &
        //                                      data.ConditionUseFormulas2 <= data.ConditionUseFormulas2_2;
        //        break;
        //    case ShellType.Elliptical:
        //    case ShellType.Spherical:
        //    case ShellType.Torospherical:
        //        data.ConditionUseFormulas1 = (cdc.dp - 2 * dataIn.cs) / shellDataIn.D;
        //        data.ConditionUseFormulas2 = (shellDataIn.s - cdc.c) / shellDataIn.D;
        //        data.IsConditionUseFormulas = data.ConditionUseFormulas1 <= 0.6 & data.ConditionUseFormulas2 <= 0.1;
        //        break;
        //}
        //if (!data.IsConditionUseFormulas)
        //{
        //    data.ErrorList.Add("Условие применения формул не выполняется.");
        //}

        return data;
    }

    private static NozzleCalculatedCommon CalculateCommonData(NozzleInput dataIn)
    {
        var data = new NozzleCalculatedCommon();

        var shellDataIn = (ShellInputData)dataIn.ShellCalculatedData.InputData;

        switch (shellDataIn.ShellType)
        {
            case ShellType.Cylindrical:
                data.c = ((CylindricalShellCalculated)dataIn.ShellCalculatedData).CommonData.c;
                break;
            case ShellType.Conical:
                data.c = ((ConicalShellCalculated)dataIn.ShellCalculatedData).CommonData.c;
                data.Dk = ((ConicalShellCalculated)dataIn.ShellCalculatedData).CommonData.Dk;
                data.alpha1 = MathHelper.RadianToDegree(((ConicalShellCalculated)dataIn.ShellCalculatedData).CommonData.alpha1);
                break;
            case ShellType.Elliptical:
                data.c = ((EllipticalShellCalculated)dataIn.ShellCalculatedData).CommonData.c;
                data.EllipseH = ((EllipticalShellInput)shellDataIn).EllipseH;
                break;
            case ShellType.Spherical:
                break;
            case ShellType.Torospherical:
                break;
            default:
                throw new CalculateException("Ошибка вида укрепляемого элемента.");
        }

        // расчет Dp, dp
        switch (shellDataIn.ShellType)
        {
            case ShellType.Cylindrical:
                data.Dp = shellDataIn.D;
                break;
            case ShellType.Conical:
                data.Dp = data.Dk / Math.Cos(Math.PI * 100 * data.alpha1);
                break;
            case ShellType.Elliptical:
                if (Math.Abs(data.EllipseH * 100 - shellDataIn.D * 25) < 0.000001)
                {
                    data.Dp = shellDataIn.D * 2 * Math.Sqrt(1.0 - 3.0 * Math.Pow(dataIn.ellx / shellDataIn.D, 2));
                }
                else
                {
                    data.Dp = Math.Pow(shellDataIn.D, 2) / (data.EllipseH * 2) *
                              Math.Sqrt(1.0 - 4.0 * (Math.Pow(shellDataIn.D, 2) - 4 *
                                      Math.Pow(data.EllipseH, 2)) * Math.Pow(dataIn.ellx, 2) /
                                  Math.Pow(shellDataIn.D, 4));
                }
                break;
            case ShellType.Spherical:
            case ShellType.Torospherical:
                //data.Dp = 2 * shellDataIn.R;
                break;
            default:
                throw new CalculateException("Ошибка вида укрепляемого элемента.");
        }

        switch (dataIn.Location)
        {
            case NozzleLocation.LocationAccordingToParagraph_5_2_2_1:
                data.dp = dataIn.d + 2 * dataIn.cs; //dp = d + 2cs
                break;
            case NozzleLocation.LocationAccordingToParagraph_5_2_2_2:
                data.dp = Math.Max(dataIn.d, 0.5 * dataIn.tTransversely) + 2.0 * dataIn.cs; //dp =max{d; 0,5t} + 2cs
                break;
            case NozzleLocation.LocationAccordingToParagraph_5_2_2_3:
                data.dp = (dataIn.d + 2.0 * dataIn.cs) / Math.Sqrt(1 + Math.Pow(2 * dataIn.ellx / data.Dp, 2));
                break;
            case NozzleLocation.LocationAccordingToParagraph_5_2_2_4:
                data.dp = (dataIn.d + 2.0 * dataIn.cs) * (1 + Math.Pow(Math.Tan(Math.PI * 180 * dataIn.gamma), 2) *
                        Math.Pow(Math.Cos(Math.PI * 180 * dataIn.omega), 2));
                break;
            case NozzleLocation.LocationAccordingToParagraph_5_2_2_5:
                data.dp = (dataIn.d + 2.0 * dataIn.cs) / Math.Pow(Math.Cos(Math.PI * 180 * dataIn.gamma), 2);
                break;
            case NozzleLocation.LocationAccordingToParagraph_5_2_2_6:
                data.dp = (dataIn.d2 + 2.0 * dataIn.cs) *
                          (Math.Pow(Math.Sin(Math.PI * 180 * dataIn.omega), 2) +
                           (dataIn.d1 + 2 * dataIn.cs) *
                           (dataIn.d1 + dataIn.d2 + 4 * dataIn.cs) /
                           (2 * Math.Pow(dataIn.d2 + 2 * dataIn.cs, 2)) *
                           Math.Pow(Math.Cos(Math.PI * 180 * dataIn.omega), 2));
                break;
            case NozzleLocation.LocationAccordingToParagraph_5_2_2_7:
                //data.dp = dataIn.d + 1.5 * (dataIn.r - data.sp) + 2.0 * dataIn.cs;
                break;
            default:
                throw new CalculateException("Ошибка места расположения штуцера.");
        }

        // l1p, l3p, l2p
        {
            var d = !dataIn.IsOval ? dataIn.d : dataIn.d2;

            data.l1p2 = 1.25 * Math.Sqrt((d + 2.0 * dataIn.cs) * (dataIn.s1 - dataIn.cs));
            data.l1p = Math.Min(dataIn.l1, data.l1p2);
            if (dataIn.s3 == 0)
            {
                data.l3p = 0;
            }
            else
            {
                data.l3p2 = 0.5 * Math.Sqrt((d + 2 * dataIn.cs) * (dataIn.s3 - dataIn.cs - dataIn.cs1));
                data.l3p = Math.Min(dataIn.l3, data.l3p2);
            }
        }

        data.L0 = Math.Sqrt(data.Dp * (shellDataIn.s - data.c));

        switch (dataIn.NozzleKind)
        {
            case NozzleKind.ImpassWithoutRing:
            case NozzleKind.PassWithoutRing:
            case NozzleKind.ImpassWithRing:
            case NozzleKind.PassWithRing:
            case NozzleKind.WithRingAndInPart:
            case NozzleKind.WithFlanging:
                data.lp = data.L0;
                break;
            case NozzleKind.WithTorusshapedInsert:
            case NozzleKind.WithWealdedRing:
                data.lp = Math.Min(dataIn.l, data.L0);
                break;
        }

        data.l2p2 = Math.Sqrt(data.Dp * (dataIn.s2 + shellDataIn.s - data.c));
        data.l2p = Math.Min(dataIn.l2, data.l2p2);

        switch (dataIn.NozzleKind)
        {
            case NozzleKind.ImpassWithoutRing:
            case NozzleKind.PassWithoutRing:
            case NozzleKind.ImpassWithRing:
            case NozzleKind.PassWithRing:
            case NozzleKind.WithRingAndInPart:
            case NozzleKind.WithFlanging:
                dataIn.s0 = shellDataIn.s;
                //dataIn.steel4 = dataIn.steel1;
                break;
        }

        data.d0p = 0.4 * Math.Sqrt(data.Dp * (shellDataIn.s - data.c));

        data.b = Math.Sqrt(data.Dp * (shellDataIn.s - data.c)) + Math.Sqrt(data.Dp * (shellDataIn.s - data.c));

        switch (shellDataIn.ShellType)
        {
            case ShellType.Cylindrical:
                {
                    data.dmax = shellDataIn.D;
                    break;
                }
            case ShellType.Conical:
                {
                    data.dmax = data.Dk;
                    break;
                }
            case ShellType.Elliptical:
            case ShellType.Spherical:
            case ShellType.Torospherical:
                {
                    // TODO: 
                    data.dmax = 0.6 * shellDataIn.D;
                    break;
                }
        }

        switch (shellDataIn.ShellType)
        {
            case ShellType.Cylindrical:
            case ShellType.Conical:
                data.K1 = 1;
                break;
            case ShellType.Elliptical:
            case ShellType.Spherical:
            case ShellType.Torospherical:
                data.K1 = 2;
                break;
        }

        //if (shellDataIn.IsPressureIn)
        //{
        //    data.spn = data.sp;
        //}
        //else
        //{

        //    //data.B1n = Math.Min(1, 9.45 * (shellDataIn.D / ddata.out.l) * Math.Sqrt(shellDataIn.D / (100 * (shellDataIn.s - data.c))));
        //    //data.pen = 2.08 * 0.00001 * shellDataIn.E / (dataIn.ny * data.B1n) * (shellDataIn.D / ddata.out.l) * Math.Pow(100 * (shellDataIn.s - data.c) / shellDataIn.D, 2.5);
        //    data.pen = data.p_deShell;
        //    data.ppn = shellDataIn.p / Math.Sqrt(1.0 - Math.Pow(shellDataIn.p / data.pen, 2));
        //    data.spn = data.ppn * data.Dp / (2.0 * data.K1 * data.SigmaAllowShell - data.ppn);
        //}


        //data.d01 = 2 * ((shellDataIn.s - data.c) / data.spn - 0.8) * Math.Sqrt(data.Dp * (shellDataIn.s - data.c));
        //data.d02 = data.dmax + 2 * dataIn.cs;
        //data.d0 = Math.Min(data.d01, data.d02);

        //if (data.dp > data.d0)
        //{
        //    data.ConditionStrengthening1 = data.l1p * (dataIn.s1 - data.s1p - dataIn.cs) * data.psi1 + data.l2p * dataIn.s2 * data.psi2 + data.l3p * (dataIn.s3 - dataIn.cs - dataIn.cs1) * data.psi3 + data.lp * (shellDataIn.s - data.sp - data.c) * data.psi4;
        //    data.ConditionStrengthening2 = 0.5 * (data.dp - data.d0p) * data.sp;
        //    if (data.ConditionStrengthening1 < data.ConditionStrengthening2)
        //    {
        //        data.ErrorList.Add("Условие укрепления одиночного отверстия не выполняется.");
        //    }
        //}


        //data.V1 = (dataIn.s0 - data.c) / (shellDataIn.s - data.c);
        //data.V2 = (data.psi4 + (data.l1p * (dataIn.s1 - dataIn.cs) * data.psi1 + data.l2p * dataIn.s2 * data.psi2 + data.l3p * (dataIn.s3 - dataIn.cs - dataIn.cs1) * data.psi3) / data.lp * (shellDataIn.s - data.c)) /
        //          (1.0 + 0.5 * (data.dp - data.d0p) / data.lp + data.K1 * (dataIn.d + 2 * dataIn.cs) / data.Dp * (dataIn.phi / dataIn.phi1) * (data.l1p / data.lp));
        //data.V = Math.Min(data.V1, data.V2);

        //if (shellDataIn.IsPressureIn)
        //{
        //    data.p_d = 2 * data.K1 * dataIn.phi * data.SigmaAllowShell * (shellDataIn.s - data.c) * data.V /
        //               (data.Dp + (shellDataIn.s - data.c) * data.V);
        //}
        //else
        //{
        //    data.p_dp = 2 * data.K1 * dataIn.phi * data.SigmaAllowShell * (shellDataIn.s - data.c) * data.V /
        //                (data.Dp + (shellDataIn.s - data.c) * data.V);
        //    data.p_de = data.p_deShell;
        //    data.p_d = data.p_dp / Math.Sqrt(1 + Math.Pow(data.p_dp / data.p_de, 2));
        //}
        //if (data.p_d < shellDataIn.p)
        //{
        //    data.ErrorList.Add("Допускаемое давление меньше расчетного.");
        //}

        switch (shellDataIn.ShellType)
        {
            case ShellType.Cylindrical:
                data.ConditionUseFormulas1 = (data.dp - 2 * dataIn.cs) / shellDataIn.D;
                data.ConditionUseFormulas2 = (shellDataIn.s - data.c) / shellDataIn.D;
                data.IsConditionUseFormulas = data.ConditionUseFormulas1 <= 1 & data.ConditionUseFormulas2 <= 0.1;
                break;
            case ShellType.Conical:
                data.ConditionUseFormulas1 = (data.dp - 2 * dataIn.cs) / data.Dk;
                data.ConditionUseFormulas2 = (shellDataIn.s - data.c) / data.Dk;
                data.ConditionUseFormulas2_2 = 0.1 / Math.Cos(Math.PI * 180 * data.alpha1);
                data.IsConditionUseFormulas = data.ConditionUseFormulas1 <= 1 &
                                              data.ConditionUseFormulas2 <= data.ConditionUseFormulas2_2;
                break;
            case ShellType.Elliptical:
            case ShellType.Spherical:
            case ShellType.Torospherical:
                data.ConditionUseFormulas1 = (data.dp - 2 * dataIn.cs) / shellDataIn.D;
                data.ConditionUseFormulas2 = (shellDataIn.s - data.c) / shellDataIn.D;
                data.IsConditionUseFormulas = data.ConditionUseFormulas1 <= 0.6 & data.ConditionUseFormulas2 <= 0.1;
                break;
        }
        if (!data.IsConditionUseFormulas)
        {
            data.ErrorList.Add("Условие применения формул не выполняется.");
        }

        return data;
    }
}