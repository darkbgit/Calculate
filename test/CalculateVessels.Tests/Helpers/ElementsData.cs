using System.Collections.Generic;
using CalculateVessels.Core.Elements.Base;
using CalculateVessels.Core.Elements.Bottoms.Enums;
using CalculateVessels.Core.Elements.Bottoms.FlatBottom;
using CalculateVessels.Core.Elements.Bottoms.FlatBottomWithAdditionalMoment;
using CalculateVessels.Core.Elements.HeatExchangers.Enums;
using CalculateVessels.Core.Elements.HeatExchangers.HeatExchangerStationaryTubePlates;
using CalculateVessels.Core.Elements.Shells.Conical;
using CalculateVessels.Core.Elements.Shells.Cylindrical;
using CalculateVessels.Core.Elements.Shells.Elliptical;
using CalculateVessels.Core.Elements.Shells.Enums;
using CalculateVessels.Core.Elements.Shells.Nozzle;
using CalculateVessels.Core.Elements.Shells.Nozzle.Enums;
using CalculateVessels.Core.Elements.Supports.Saddle;
using CalculateVessels.Core.Enums;
using CalculateVessels.Data.Public.Gost34233_4.Models;
using CalculateVessels.Data.Public.Models;

namespace CalculateVessels.UnitTests.Helpers;

public class ElementsData
{
    private static readonly SteelWithIdsDto steel_20 = new()
    {
        SteelId = 3,
        SteelName = "20",
        DesignResourceId = 1
    };

    private static readonly SteelWithIdsDto steel_12h18n10t = new()
    {
        SteelId = 21,
        SteelName = "12Х18Н10Т",
        DesignResourceId = 1
    };

    private static readonly SteelWithIdsDto steel_st3 = new SteelWithIdsDto()
    {
        SteelId = 1,
        SteelName = "Ст3",
        DesignResourceId = 1,
        MinMaxThicknessId = 1
    };

    #region CylindricalShell

    public static IEnumerable<object[]> GetCylindricalInputAndCalculatedData()
    {
        var (inputData1, calculatedData1) = GetCylindricalData1();
        yield return new object[] { inputData1, calculatedData1 };

        var (inputData2, calculatedData2) = GetCylindricalData2();
        yield return new object[] { inputData2, calculatedData2 };

        var (inputData3, calculatedData3) = GetCylindricalData3();
        yield return new object[] { inputData3, calculatedData3 };

        var (inputData4, calculatedData4) = GetCylindricalData4();
        yield return new object[] { inputData4, calculatedData4 };
    }

    public static IEnumerable<object[]> GetCylindricalInputData()
    {
        var (inputData1, _) = GetCylindricalData1();
        yield return new object[] { inputData1 };

        var (inputData2, _) = GetCylindricalData2();
        yield return new object[] { inputData2 };

        var (inputData3, _) = GetCylindricalData3();
        yield return new object[] { inputData3 };

        var (inputData4, _) = GetCylindricalData4();
        yield return new object[] { inputData4 };
    }

    private static (CylindricalShellInput, CylindricalShellCalculated) GetCylindricalData1()
    {
        var loadingCondition1 = new LoadingCondition
        {
            Id = 1,
            p = 0.6,
            t = 120,
            PressureType = PressureType.Inside
        };

        var inputData = new CylindricalShellInput
        {
            LoadingConditions = new List<LoadingCondition>
            {
                loadingCondition1
            },
            Name = "Тестовая цилиндрическая обечайка",
            Steel = steel_20,
            c1 = 2.0,
            D = 1200,
            c2 = 0.8,
            c3 = 0,
            s = 8,
            phi = 0.9,
            ny = 2.4
        };

        var commonData = new CylindricalShellCalculatedCommon
        {
            c = 2.8,
            IsConditionUseFormulas = true
        };

        var result1 = new CylindricalShellCalculatedOneLoading
        {
            LoadingConditionId = loadingCondition1.Id,
            s_p = 2.854,
            s = 5.654,
            p_de = 0,
            p_d = 1.091,
            SigmaAllow = 140.5,
            b = 0,
            b_2 = 0,
            B1 = 0,
            B1_2 = 0,
            ConditionStability = 0.55,
            F = 0,
            FAllow = 0,
            F_de = 0,
            F_de1 = 0,
            F_de2 = 0,
            F_dp = 0,
            l = 0,
            lambda = 0,
            lpr = 0,
            M_d = 0,
            M_de = 0,
            M_dp = 0,
            Q_d = 0,
            Q_de = 0,
            Q_dp = 0,
            s_f = 0,
            s_pf = 0,
            s_p_1 = 0,
            s_p_2 = 0,
            p_dp = 0,
            E = 189000
        };

        var calculatedData = new CylindricalShellCalculated(commonData,
            new List<CylindricalShellCalculatedOneLoading> { result1 })
        {
            InputData = inputData
        };

        return (inputData, calculatedData);
    }

    private static (CylindricalShellInput, CylindricalShellCalculated) GetCylindricalData2()
    {
        var loadingCondition1 = new LoadingCondition
        {
            Id = 1,
            p = 0.6,
            t = 120,
            PressureType = PressureType.Outside
        };

        var inputData = new CylindricalShellInput
        {
            LoadingConditions = new List<LoadingCondition>
            {
                loadingCondition1
            },
            Name = "Тестовая цилиндрическая обечайка",
            Steel = steel_20,
            c1 = 2.0,
            D = 1200,
            c2 = 0.8,
            c3 = 0,
            s = 12,
            phi = 0.9,
            ny = 2.4,
            l = 1500,
        };

        var commonData = new CylindricalShellCalculatedCommon
        {
            c = 2.8,
            IsConditionUseFormulas = true
        };


        var result1 = new CylindricalShellCalculatedOneLoading
        {
            LoadingConditionId = loadingCondition1.Id,
            s_p = 8.789,
            s = 11.589,
            p_de = 0.674,
            p_d = 0.643,
            SigmaAllow = 140.5,
            b = 1,
            b_2 = 0.476,
            B1 = 1,
            B1_2 = 8.634,
            ConditionStability = 0.933,
            F = 0,
            FAllow = 0,
            F_de = 0,
            F_de1 = 0,
            F_de2 = 0,
            F_dp = 0,
            l = 1500,
            lambda = 0,
            lpr = 0,
            M_d = 0,
            M_de = 0,
            M_dp = 0,
            Q_d = 0,
            Q_de = 0,
            Q_dp = 0,
            s_f = 0,
            s_pf = 0,
            s_p_1 = 8.789,
            s_p_2 = 3.081,
            p_dp = 2.138,
            E = 189000
        };

        var calculatedData = new CylindricalShellCalculated(commonData,
            new List<CylindricalShellCalculatedOneLoading> { result1 })
        {
            InputData = inputData
        };

        return (inputData, calculatedData);
    }

    private static (CylindricalShellInput, CylindricalShellCalculated) GetCylindricalData3()
    {
        var loadingCondition1 = new LoadingCondition
        {
            Id = 1,
            p = 0.6,
            t = 120,
            PressureType = PressureType.Outside,
            EAllow = 189000,
            SigmaAllow = 140.5
        };

        var inputData = new CylindricalShellInput
        {
            LoadingConditions = new List<LoadingCondition>
            {
                loadingCondition1
            },
            Name = "Тестовая цилиндрическая обечайка",
            Steel = steel_20,
            c1 = 2.0,
            D = 1200,
            c2 = 0.8,
            c3 = 0,
            s = 12,
            phi = 0.9,
            ny = 2.4,
            F = 1000,
            q = 0,
            M = 0,
            Q = 0,
            l = 1500,
            l3 = 0,
            fi_t = 0,
            ConditionForCalcF5341 = false,
            FCalcSchema = 1,
            f = 0,
            IsFTensile = false
        };

        var commonData = new CylindricalShellCalculatedCommon
        {
            c = 2.8,
            IsConditionUseFormulas = true
        };

        var result1 = new CylindricalShellCalculatedOneLoading
        {
            LoadingConditionId = loadingCondition1.Id,
            s_p = 8.789,
            s = 11.589,
            p_de = 0.674,
            p_d = 0.643,
            SigmaAllow = 140.5,
            b = 1,
            b_2 = 0.476,
            B1 = 1,
            B1_2 = 8.634,
            ConditionStability = 0.933,
            F = 0,
            FAllow = 1697652.271,
            F_de = 1809219.257,
            F_de1 = 1809219.257,
            F_de2 = 0,
            F_dp = 4910346.765,
            l = 1500,
            lambda = 0,
            lpr = 0,
            M_d = 0,
            M_de = 0,
            M_dp = 0,
            Q_d = 0,
            Q_de = 0,
            Q_dp = 0,
            s_f = 0,
            s_pf = 0,
            s_p_1 = 8.789,
            s_p_2 = 3.081,
            p_dp = 2.138,
            E = 189000
        };

        var calculatedData = new CylindricalShellCalculated(commonData,
            new List<CylindricalShellCalculatedOneLoading> { result1 })
        {
            InputData = inputData
        };

        return (inputData, calculatedData);
    }

    private static (CylindricalShellInput, CylindricalShellCalculated) GetCylindricalData4()
    {
        var loadingCondition1 = new LoadingCondition
        {
            Id = 1,
            p = 0.6,
            t = 150,
            PressureType = PressureType.Inside
        };

        var loadingCondition2 = new LoadingCondition
        {
            Id = 2,
            p = 0.8,
            t = 120,
            PressureType = PressureType.Outside
        };

        var inputData = new CylindricalShellInput
        {
            LoadingConditions = new List<LoadingCondition>
            {
                loadingCondition1, loadingCondition2
            },
            Name = "Тестовая цилиндрическая обечайка",
            Steel = steel_12h18n10t,
            c1 = 2.0,
            D = 600,
            l = 1000,
            c2 = 0.8,
            c3 = 0,
            s = 10,
            phi = 1,
            ny = 2.4,
        };

        var commonData = new CylindricalShellCalculatedCommon
        {
            c = 2.8,
            IsConditionUseFormulas = true
        };

        var result1 = new CylindricalShellCalculatedOneLoading
        {
            LoadingConditionId = loadingCondition1.Id,
            s_p = 1.073,
            s = 3.873,
            p_d = 3.984,
            SigmaAllow = 168,
            ConditionStability = 0.15,
            E = 199000
        };

        var result2 = new CylindricalShellCalculatedOneLoading
        {
            LoadingConditionId = loadingCondition2.Id,
            B1 = 1.0,
            B1_2 = 5.175,
            ConditionStability = 0.526,
            E = 199600.0,
            //ErrorList = { empty },
            SigmaAllow = 171.5,
            b = 1.0,
            b_2 = 0.542,
            l = 1000.0,
            p_d = 1.518,
            p_de = 1.637,
            p_dp = 4.067,
            s = 8.212,
            s_p = 5.412,
            s_p_1 = 5.412,
            s_p_2 = 1.683,
        };

        var calculatedData = new CylindricalShellCalculated(commonData,
            new List<CylindricalShellCalculatedOneLoading> { result1, result2 })
        {
            InputData = inputData
        };

        return (inputData, calculatedData);
    }

    #endregion

    #region ConicalShell

    public static IEnumerable<object[]> GetConicalInputAndCalculatedData()
    {
        var (inputData1, calculatedData1) = GetConicalData1();
        yield return new object[] { inputData1, calculatedData1 };
    }

    public static IEnumerable<object[]> GetConicalInputData()
    {
        var (inputData1, _) = GetConicalData1();
        yield return new object[] { inputData1 };
    }

    private static (ConicalShellInput, ConicalShellCalculated) GetConicalData1()
    {
        var loadingCondition1 = new LoadingCondition
        {
            Id = 1,
            p = 0.6,
            t = 120,
            PressureType = PressureType.Inside
        };

        var inputData = new ConicalShellInput
        {
            LoadingConditions = new List<LoadingCondition> { loadingCondition1 },
            Name = "Тестовая коническая обечайка",
            Steel = steel_20,
            c1 = 2.0,
            c2 = 0.8,
            c3 = 0,
            D = 1200,
            D1 = 738,
            L = 400,
            s = 8,
            phi = 0.9,
            phi_t = 1,
            phi_k = 0,
            ny = 2.4,
            ConnectionType = ConicalConnectionType.WithoutConnection,
            sT = 0,
            IsConnectionWithLittle = false,
            r = 0,
        };

        var commonData = new ConicalShellCalculatedCommon
        {
            alpha1 = 0.523,
            a1p = 59.42,
            c = 2.8,
            Dk = 1158.397,
        };

        var result1 = new ConicalShellCalculatedOneLoading
        {
            LoadingConditionId = loadingCondition1.Id,
            ConditionUseFormulas = 0.005,
            E = 189000,
            IsConditionUseFormulas = true,
            SigmaAllow = 140.5,
            s = 5.981,
            s_p = 3.181,
            p_d = 0.979
        };

        var calculatedData = new ConicalShellCalculated(commonData,
            new List<ConicalShellCalculatedOneLoading> { result1 })
        {
            InputData = inputData
        };

        return (inputData, calculatedData);
    }

    #endregion

    #region EllipticalShell

    public static IEnumerable<object[]> GetEllipticalInputAndCalculatedData()
    {
        var (inputData1, calculatedData1) = GetEllipticalData1();
        yield return new object[] { inputData1, calculatedData1 };

        var (inputData2, calculatedData2) = GetEllipticalData2();
        yield return new object[] { inputData2, calculatedData2 };

        var (inputData3, calculatedData3) = GetEllipticalData3();
        yield return new object[] { inputData3, calculatedData3 };
    }

    public static IEnumerable<object[]> GetEllipticalInputData()
    {
        var (inputData1, _) = GetEllipticalData1();
        yield return new object[] { inputData1 };

        var (inputData2, _) = GetEllipticalData2();
        yield return new object[] { inputData2 };

        var (inputData3, _) = GetEllipticalData3();
        yield return new object[] { inputData3 };
    }

    private static (EllipticalShellInput, EllipticalShellCalculated) GetEllipticalData1()
    {
        var loadingCondition1 = new LoadingCondition
        {
            Id = 1,
            p = 0.6,
            t = 120,
            PressureType = PressureType.Inside
        };

        var inputData = new EllipticalShellInput
        {
            LoadingConditions = new List<LoadingCondition> { loadingCondition1 },
            Name = "Тестовая эллиптическая обечайка",
            Steel = steel_st3,
            c1 = 2.0,
            D = 1000,
            c2 = 0.8,
            c3 = 1.2,
            s = 8,
            phi = 1.0,
            ny = 2.4,
            EllipseH = 250,
            Ellipseh1 = 25,
            EllipticalBottomType = EllipticalBottomType.Elliptical,
        };

        var commonData = new EllipticalShellCalculatedCommon
        {
            c = 4,
            IsConditionUseFormulas = true,
            EllipseR = 1000
        };

        var result1 = new EllipticalShellCalculatedOneLoading
        {
            LoadingConditionId = loadingCondition1.Id,
            s_p = 2.043,
            s = 6.043,
            p_de = 0,
            p_d = 1.174,
            SigmaAllow = 147,
            b = 0,
            b_2 = 0,
            B1 = 0,
            B1_2 = 0,
            ConditionStability = 0,
            l = 0,
            s_p_1 = 0,
            s_p_2 = 0,
            p_dp = 0,
            E = 189000
        };

        var calculatedData = new EllipticalShellCalculated(commonData,
            new List<EllipticalShellCalculatedOneLoading> { result1 })
        {
            InputData = inputData
        };

        return (inputData, calculatedData);
    }

    private static (EllipticalShellInput, EllipticalShellCalculated) GetEllipticalData2()
    {
        var loadingCondition1 = new LoadingCondition
        {
            Id = 1,
            p = 0.6,
            t = 120,
            PressureType = PressureType.Outside
        };

        var inputData = new EllipticalShellInput
        {
            LoadingConditions = new List<LoadingCondition> { loadingCondition1 },
            Name = "Тестовая эллиптическая обечайка",
            Steel = steel_st3,
            c1 = 2.0,
            D = 1000,
            c2 = 0.8,
            c3 = 1.2,
            s = 10,
            phi = 1.0,
            ny = 2.4,
            EllipseH = 250,
            Ellipseh1 = 25,
            EllipticalBottomType = EllipticalBottomType.Elliptical
        };

        var commonData = new EllipticalShellCalculatedCommon
        {
            c = 4,
            IsConditionUseFormulas = true,
            EllipseR = 1000
        };

        var result1 = new EllipticalShellCalculatedOneLoading()
        {
            LoadingConditionId = loadingCondition1.Id,
            s_p = 4.879,
            s = 8.879,
            p_de = 0.82,
            p_d = 0.743,
            SigmaAllow = 147,
            s_p_1 = 4.879,
            s_p_2 = 2.449,
            p_dp = 1.759,
            E = 189000,
            EllipseKePrev = 0.9,
            Ellipsex = 0.09,
            EllipseKe = 0.948
        };

        var calculatedData = new EllipticalShellCalculated(commonData,
            new List<EllipticalShellCalculatedOneLoading> { result1 })
        {
            InputData = inputData
        };

        return (inputData, calculatedData);
    }

    private static (EllipticalShellInput, EllipticalShellCalculated) GetEllipticalData3()
    {
        var loadingCondition1 = new LoadingCondition
        {
            Id = 1,
            p = 0.6,
            t = 120,
            EAllow = 189000,
            SigmaAllow = 147,
            PressureType = PressureType.Outside
        };

        var inputData = new EllipticalShellInput
        {
            LoadingConditions = new List<LoadingCondition> { loadingCondition1 },
            Name = "Тестовая эллиптическая обечайка",
            Steel = steel_st3,
            c1 = 2.0,
            D = 1000,
            c2 = 0.8,
            c3 = 1.2,
            s = 10,
            phi = 1.0,
            ny = 2.4,
            EllipseH = 250,
            Ellipseh1 = 25,
            EllipticalBottomType = EllipticalBottomType.Elliptical
        };

        var commonData = new EllipticalShellCalculatedCommon
        {
            c = 4,
            IsConditionUseFormulas = true,
            EllipseR = 1000
        };

        var result1 = new EllipticalShellCalculatedOneLoading
        {
            LoadingConditionId = loadingCondition1.Id,
            s_p = 4.879,
            s = 8.879,
            p_de = 0.82,
            p_d = 0.743,
            SigmaAllow = 147,
            s_p_1 = 4.879,
            s_p_2 = 2.449,
            p_dp = 1.759,
            E = 189000,
            EllipseKePrev = 0.9,
            Ellipsex = 0.09,
            EllipseKe = 0.948
        };

        var calculatedData = new EllipticalShellCalculated(commonData,
            new List<EllipticalShellCalculatedOneLoading> { result1 })
        {
            InputData = inputData
        };

        return (inputData, calculatedData);
    }

    #endregion

    #region Nozzle

    public static IEnumerable<object[]> GetNozzleInputAndCalculatedData()
    {
        var (inputData1, calculatedData1) = GetNozzleData1();
        yield return new object[] { inputData1, calculatedData1 };
    }

    private static (NozzleInput, NozzleCalculated) GetNozzleData1()
    {
        var loadingCondition1 = new LoadingCondition
        {
            Id = 1,
            p = 0.6,
            t = 150,
            PressureType = PressureType.Inside
        };

        var shellInput = new CylindricalShellInput
        {
            LoadingConditions = new List<LoadingCondition> { loadingCondition1 },
            Name = "Тестовая цилиндрическая обечайка",
            Steel = steel_12h18n10t,
            c1 = 2.0,
            D = 600,
            c2 = 0.8,
            c3 = 0,
            s = 8,
            phi = 1,
            ny = 2.4,
        };

        var shellCommon = new CylindricalShellCalculatedCommon
        {
            c = 2.8,
            IsConditionUseFormulas = true
        };

        var shellResult1 = new CylindricalShellCalculatedOneLoading
        {
            LoadingConditionId = loadingCondition1.Id,
            s_p = 1.073,
            s = 3.873,
            p_d = 2.887,
            SigmaAllow = 168,
            ConditionStability = 0.208,
            E = 199000
        };

        var shellCalculated = new CylindricalShellCalculated(shellCommon,
            new List<CylindricalShellCalculatedOneLoading> { shellResult1 })
        {
            InputData = shellInput
        };

        var nozzleInput = new NozzleInput(shellCalculated)
        {
            steel1 = steel_12h18n10t,
            SigmaAllow1 = 0,
            E1 = 0,
            E2 = 0,
            E3 = 0,
            E4 = 0,
            d = 77,
            s1 = 6,
            s2 = 0,
            s3 = 6,
            cs = 2.9,
            cs1 = 2.0,
            l = 0,
            l1 = 130,
            l2 = 0,
            l3 = 5,
            NozzleKind = NozzleKind.PassWithoutRing,
            phi = 1,
            phi1 = 1,
            delta = 6,
            delta1 = 6,
            delta2 = 6,
            steel2 = new SteelWithIdsDto(),
            steel3 = steel_12h18n10t,
            steel4 = new SteelWithIdsDto(),
            Location = NozzleLocation.LocationAccordingToParagraph_5_2_2_1,
            omega = 0,
            tTransversely = 0,
            ellx = 0,
            gamma = 0,
            Name = "Тестовый штуцер",
            IsOval = false,
            d1 = 0,
            d2 = 0,
            r = 0,
            s0 = 0,
            SigmaAllow2 = 0,
            SigmaAllow3 = 0,
            SigmaAllow4 = 0
        };

        var nozzleCommon = new NozzleCalculatedCommon
        {
            alpha1 = 0,
            b = 111.714,
            c = 2.8,
            ConditionUseFormulas1 = 0.128,
            ConditionUseFormulas2 = 0.009,
            ConditionUseFormulas2_2 = 0,
            d0p = 22.343,
            Dk = 0,
            dmax = 600,
            dp = 82.8,
            Dp = 600,
            EllipseH = 0,
            IsConditionUseFormulas = true,
            K1 = 1,
            L0 = 55.857,
            l1p = 20.027,
            l1p2 = 20.027,
            l2p = 0,
            l2p2 = 55.857,
            l3p = 4.772,
            l3p2 = 4.772,
            lp = 55.857,
        };

        var nozzleResult1 = new NozzleCalculatedOneLoading
        {
            LoadingConditionId = loadingCondition1.Id,

            ConditionStrengthening1 = 0,
            ConditionStrengthening2 = 0,

            d0 = 452.02,// 451.85,
            d01 = 452.02,
            d02 = 605.8,
            E1 = 199000,
            E2 = 0,
            E3 = 199000,
            E4 = 0,
            p_d = 2.887,
            p_de = 0,
            p_deShell = 0,
            p_dp = 0,
            pen = 0,
            ppn = 0,
            psi1 = 1,
            psi2 = 0,
            psi3 = 1,
            psi4 = 1,
            SigmaAllowShell = 168,
            SigmaAllow1 = 168,
            SigmaAllow2 = 0,
            SigmaAllow3 = 168,
            SigmaAllow4 = 0,
            s1p = 0.148,
            sp = 1.073,
            spn = 1.073,
            V = 1,
            V1 = 1,
            V2 = 4.569,
        };

        var nozzleCalculated = new NozzleCalculated
        {
            InputData = nozzleInput,
            CommonData = nozzleCommon,
            Results = new List<NozzleCalculatedOneLoading> { nozzleResult1 }
        };

        return (nozzleInput, nozzleCalculated);
    }

    #endregion

    #region FlatBottom
    public static IEnumerable<object[]> GetFlatBottomInputAndCalculatedData()
    {
        var (inputData1, calculatedData1) = GetFlatBottomData1();
        yield return new object[] { inputData1, calculatedData1 };
    }

    public static IEnumerable<object[]> GetFlatBottomInputData()
    {
        var (inputData1, _) = GetFlatBottomData1();
        yield return new object[] { inputData1 };
    }

    private static (FlatBottomInput, FlatBottomCalculated) GetFlatBottomData1()
    {
        var loadingCondition1 = new LoadingCondition
        {
            Id = 1,
            p = 0.6,
            t = 50,
            PressureType = PressureType.Inside
        };

        var inputData = new FlatBottomInput
        {
            Name = "Тестовая плоское днище",
            a = 10,
            c1 = 2.0,
            c2 = 0.8,
            c3 = 0,
            D = 400,
            D2 = 0,
            D3 = 0,
            Dcp = 0,
            d = 0,
            di = 0,
            E = 0,
            phi = 1,
            gamma = 0,
            Hole = HoleInFlatBottom.WithoutHole,
            h1 = 0,
            r = 0,
            SigmaAllow = 0,
            s = 8,
            s1 = 16,
            s2 = 0,
            FlatBottomType = 2,
            LoadingCondition = loadingCondition1,
            Steel = steel_20
        };


        var calculatedData = new FlatBottomCalculated()
        {
            InputData = inputData,
            ConditionUseFormulas = 0.033,
            c = 2.8,
            Dp = 400,
            IsConditionFixed = true,
            IsConditionUseFormulas = true,
            K = 0.5,
            K0 = 1,
            K_1 = 0,
            Kp = 1,
            SigmaAllow = 145,
            s1 = 15.665,
            s1p = 12.865,
            s2 = 0,
            s2p = 0,
            s2p_1 = 0,
            s2p_2 = 0,
            p_d = 0.632
        };

        return (inputData, calculatedData);
    }

    #endregion

    #region FlatBottomWithAdditionalMoment

    public static IEnumerable<object[]> GetFlatBottomWithAdditionalMomentInputAndCalculatedData()
    {
        var (inputData1, calculatedData1) = GetFlatBottomWithAdditionalMomentData1();
        yield return new object[] { inputData1, calculatedData1 };
    }

    public static IEnumerable<object[]> GetFlatBottomWithAdditionalMomentInputData()
    {
        var (inputData1, _) = GetFlatBottomWithAdditionalMomentData1();
        yield return new object[] { inputData1 };
    }

    public static (FlatBottomWithAdditionalMomentInput, FlatBottomWithAdditionalMomentCalculated) GetFlatBottomWithAdditionalMomentData1()
    {
        var inputData = new FlatBottomWithAdditionalMomentInput
        {
            Name = "Тестовая плоское днище",

            c1 = 2,
            c2 = 0.8,
            c3 = 0,
            D = 600,
            s = 40,

            //cover
            CoverSteel = "20",
            D2 = 633,
            D3 = 700,
            IsCoverFlat = true,
            IsCoverWithGroove = false,
            s1 = 36,
            s2 = 31,
            s3 = 25,
            s4 = 0,

            //stress condition
            F = 0,
            fi = 1,
            IsPressureIn = true,
            M = 0,
            p = 0.6,
            t = 150,
            SigmaAllow = 0,

            //screw
            IsStud = true,
            IsScrewWithGroove = false,
            Lb0 = 63,
            n = 28,
            ScrewSteel = "35",
            Screwd = 10,

            //gasket
            bp = 13,
            Dcp = 620,
            GasketType = "Резина по ГОСТ 7338 с твердостью по Шору А до 65 единиц",
            hp = 2,

            //flange
            Db = 700,
            Dn = 740,
            FlangeFace = FlangeFaceType.Flat,
            FlangeSteel = "Ст3",
            h = 32,
            IsFlangeIsolated = false,
            IsFlangeFlat = true,
            l = 0,
            S0 = 0,
            S1 = 0,

            //washer
            IsWasher = false,
            hsh = 0,
            WasherSteel = string.Empty,

            //hole
            di = 0,
            d = 0,
            E = 0,
            Hole = HoleInFlatBottom.WithoutHole,

        };

        var calculatedData = new FlatBottomWithAdditionalMomentCalculated
        {
            InputData = inputData,
            IsConditionUseFormulas = true,
            Ab = 1461.6,
            alpha = 0,
            alpha_m = 0,
            alpha_b = 0,
            alpha_f = 0,
            alpha_kr = 0,
            alpha_sh1 = 0,
            alpha_sh2 = 0,
            b = 40,
            b0 = 13,
            beta = 0,
            betaF = 0.91,
            betaT = 1.867,
            betaU = 19.645,
            betaV = 0.55,
            betaY = 17.964,
            betaZ = 4.838,
            c = 2.8,
            ConditionUseFormulas = 0.054,
            Dcp = 620,
            delta_kr = 31,
            Dp = 620,
            e = 0,
            E = 186600,
            E20 = 199000,
            Eb = 204900,
            Eb20 = 213000,
            Ekr = 186000,
            Ekr20 = 199000,
            Ep = 0,
            f = 1,
            fb = 52.2,
            Gasket = new Gasket
            {
                Material = inputData.GasketType,
                m = 0.5,
                qobj = 2,
                q_d = 18,
                Kobj = 0.04,
                Ep = -1,
                IsFlat = true,
                IsMetal = false
            },
            gamma = 40.355,
            hkr = 31,
            K0 = 1,
            K6 = 0.479,
            K7_s2 = 0.287,
            K7_s3 = 0.26,
            KGost34233_4 = 1.233,
            Kkr = 1.194,
            Kp = 1,
            l0 = 154.919,
            lambda = 29.659,
            Lb = 68.6,
            p_d = 1.737,
            Pb1 = 7598.308,
            Pb1_1 = 7598.308,
            Pb1_2 = 7597.947,
            Pb2 = 71910.72,
            Pb2_2 = 71910.72,
            Pbm = 71910.72,
            Pbp = 252961.544,
            Phi = 1819.867,
            Phi_1 = 1819.867,
            Phi_2 = 379.385,
            Pobj = 25321.237,
            psi1 = 1.397,
            Qd = 181052.4,
            QFM = 0,
            Qt = 0.361,
            Rp = 7596.371,
            S0 = 40,
            s1 = 22.311,
            s1p = 19.511,
            s2 = 15.059,
            s2p = 12.259,
            s2p_1 = 12.259,
            s2p_2 = 1.761,
            s3 = 13.903,
            s3p = 11.103,
            s3p_1 = 11.103,
            s3p_2 = 1.761,
            Se = 20,
            SigmaAllow = 139,
            sigma_d_krm = 189.545,
            sigma_dnb = 123,
            tb = 142.5,
            tf = 144,
            tkr = 150,
            x = 0,
            Xkr = 0.082,
            yb = 0,
            yF = 0,
            yfn = 0,
            ykr = 0,
            yp = 0.025,
            zeta = 1,
            dkr = 10,
        };

        return (inputData, calculatedData);
    }

    #endregion

    #region Saddle

    public static IEnumerable<object[]> GetSaddleInputAndCalculatedData()
    {
        var (inputData1, calculatedData1) = GetSaddleData1();
        yield return new object[] { inputData1, calculatedData1 };
    }

    public static IEnumerable<object[]> GetSaddleInputData()
    {
        var (inputData1, _) = GetSaddleData1();
        yield return new object[] { inputData1 };
    }

    private static (SaddleInput inputData, SaddleCalculated calculatedData) GetSaddleData1()
    {
        var inputData = new SaddleInput
        {
            PressureType = PressureType.Inside,
            D = 600,
            s = 8,
            c = 2.8,
            fi = 1,
            Steel = steel_20,
            p = 2.5,
            t = 150,
            N = 1000,
            G = 54000,
            SaddleType = SaddleType.SaddleWithoutRingWithoutSheet,
            b = 180,
            delta1 = 112,
            H = 204,
            a = 1532,
            L = 6632,
            e = 1632
        };

        var calculatedData = new SaddleCalculated
        {
            InputData = inputData,
            IsConditionUseFormulas = true,
            beta1 = 2.932,
            ConditionStability2 = 0.131,
            ConditionStrength2 = 136214.77,
            E = 186000,
            F_d = 1374253.766,
            F_d2 = 150477.23,
            F_d3 = 136214.77,
            F1 = 27000,
            F2 = 27000,
            Fe = 108072.236,
            gamma = 0.673,
            K1_2 = 1.118,
            K1_2For_v21 = 1.118,
            K1_2For_v22 = 1.428,
            K1_3 = 0.79,
            K1_3For_v21 = 0.79,
            K1_3For_v22 = 1.335,
            K10 = 0.25,
            K10_1 = 0.004,
            K11 = 0.359,
            K12 = 1.05,
            K13 = 0.474,
            K14 = 0.735,
            K15 = 1,
            K15_2 = 2.4,
            K16 = 0.972,
            K17 = 0.369,
            K2 = 1.25,
            M_d = 202217365.417,
            M_de = 1041779227.112,
            M_dp = 206138064.974,
            M0 = 175984.936,
            M1 = 10240070.684,
            M12 = 1741984.936,
            M2 = 10240070.684,
            ny = 2.4,
            p_d = 2.389,
            q = 7.821,
            Q_d = 318845.436,
            Q_de = 906476.532,
            Q_dp = 340611.476,
            Q1 = 13953.65,
            Q2 = 13953.65,
            sigma_mx = 6.965,
            SigmaAllow = 139,
            sigmai2 = 194.185,
            sigmai2_1 = 194.185,
            sigmai2_2 = 248.128,
            sigmai3 = 137.31,
            sigmai3_1 = 137.31,
            sigmai3_2 = 231.935,
            v1_2 = -0.416,
            v1_3 = -0.87,
            v21_2 = -0.04,
            v22_2 = 0.375,
            v22_3 = 0.83
        };

        return (inputData, calculatedData);
    }

    #endregion

    #region HeatExchangerStationaryTubePlates

    public static IEnumerable<object[]> GetHeatExchangerStationaryTubePlatesInputAndCalculatedData()
    {
        var (inputData1, calculatedData1) = GetHeatExchangerStationaryTubePlatesData1();
        yield return new object[] { inputData1, calculatedData1 };
    }

    public static IEnumerable<object[]> GetHeatExchangerStationaryTubePlatesInputData()
    {
        var (inputData1, _) = GetHeatExchangerStationaryTubePlatesData1();
        yield return new object[] { inputData1 };
    }

    private static (HeatExchangerStationaryTubePlatesInput inputData, HeatExchangerStationaryTubePlatesCalculated calculatedData) GetHeatExchangerStationaryTubePlatesData1()
    {
        var firstTubePlate = new ConnectionTubePlate
        {

            IsNeedCheckHardnessTubePlate = false,
            TubePlateType = TubePlateType.WeldedInFlange,
            FixTubeInTubePlate = FixTubeInTubePlateType.RollingWithWelding,

            TubeRolling = TubeRollingType.RollingWithoutGroove,

            //Tube plate
            Steelp = "Ст3",

            BP = 10,

            c = 2,


            fiP = 1,

            //pp { get; set; }

            s1p = 40,

            sn = 40,
            sp = 45,
            IsWithGroove = true,


            //shell for tube plate
            s1 = 10,


            //flange for tube plate
            Steel1 = "Ст3", //+
            h1 = 34,
            DH = 740,


            //shell for chamber
            SteelD = "Ст3",
            s2 = 8, //+

            //flange for chamber
            Steel2 = "Ст3", //+
            h2 = 32, //+
            IsChamberFlangeSkirt = false,
            FlangeFace = FlangeFaceType.Flat,

            //tube
            lB = 40,
            delta = 2
        };

        var inputData = new HeatExchangerStationaryTubePlatesInput
        {
            Name = "Name",
            IsOneGo = true,
            IsWorkCondition = true,
            t0 = 20,
            N = 1000,
            // a =
            a1 = 279,

            //tube
            IsNeedCheckHardnessTube = false,
            dT = 25,//+
            l = 3000,//+ UNDONE: l - half long tube
            pT = 0.6,//+
            sT = 2,//+
            tT = 75,//+
            TT = 150,//+
            i = 248,//+
            SteelT = "Ст3",//+

            //shell
            cK = 2,//+
            D = 600,//+
            pM = 2.5,//+
            sK = 8,//+
            tK = 25,//+
            TK = 150,//+
            SteelK = "Ст3",//+

            //compensator
            IsNeedCompensatorCalculate = false,
            CompensatorType = default,//+
                                      // beta0 =
                                      // deltakom =
                                      // Dkom =
                                      // dkom =
                                      // Kkom =
                                      // rkom =
                                      //public int nkom =
                                      //public string Steelkom =

            //extender
            //D1 =
            //deltap =
            //Lpac =

            //tube plate
            IsDifferentTubePlate = false,//+



            d0 = 25.35,//+
            DE = 70,//+

            tp = 32,//+
            tP = 55,//+ distance over hole both side from hole

            FirstTubePlate = firstTubePlate,
            //SecondTubePlate = = new();


            //partitions
            IsWithPartitions = true,//+
            l1R = 700,//+
            l2R = 592,//+

            //public int Bper =
            //public int Lper =
            cper = 2,
            sper = 8
        };

        var calculatedData = new HeatExchangerStationaryTubePlatesCalculated
        {
            InputData = inputData,

            ET = 186000.0,
            EK = 186000.0,
            ED = 186000.0,
            Ep = 186000.0,
            E1 = 186000.0,
            E2 = 186000.0,
            sigma_dp = 145.0,
            sigma_dK = 145.0,
            sigma_dT = 145.0,
            Rmp = 460.0,
            nNForSigmaa = 10.0,
            nsigmaForSigmaa = 2.0,
            CtForSigmaa = 0.9347826086956522,
            AForSigmaap = 60000.0,
            BForSigmaa = 184.0,
            sigmaa_dp = 652.8695652173914,
            AForSigmaaK = 60000.0,
            sigmaa_dK = 652.8695652173914,
            AForSigmaaT = 60000.0,
            sigmaa_dT = 652.8695652173914,
            Ksigma = 1.7,
            a = 300.0,
            mn = 1.075268817204301,
            etaM = 0.5021903624054161,
            etaT = 0.6487455197132617,
            Ky = 9.086419753086423,
            ro = 5.1111111111111125,
            Kq = 1.0,
            Kp = 1.0,
            psi0 = 0.3643400109605112,
            beta = 0.011272110790902762,
            omega = 3.1449189106618705,
            b1 = 70.0,
            fip = 0.20781249999999996,
            R1 = 35.0,
            b2 = 70.0,
            R2 = 35.0,
            beta1 = 0.023734644158557198,
            beta2 = 0.0265361388801511,
            K1 = 6879964.384662295,
            K2 = 3938321.420001895,
            KF1 = 44468068.28076619,
            KF2 = 34633532.64078111,
            KF = 79101600.9215473,
            mcp = 0.2528076463560334,
            p0 = -21.289800490743954,
            ro1 = 2.1619937717916575,
            t = 1.3314000572525408,
            T1 = 6.651842197849333,
            T2 = 3.91431616832247,
            T3 = 5.0,
            Phi1 = 4.5,
            Phi2 = 2.94,
            Phi3 = 4.65,
            m1 = 1603.8265396961049,
            m2 = 1313.0104928981075,
            p1 = 0.03283185839571015,
            MP = 30065.048902714567,
            QP = -617.7362469045031,
            Ma = 17092.587717720002,
            Qa = -664.2325235532292,
            NT = -7708.007967411455,
            JT = 9628.19608508932,
            lpr = 233.33333333333334,
            MT = -36069.12956317753,
            QK = 707.7362469045031,
            MK = -4801.189597342522,
            F = 1334051.3963726393,
            sigmap1 = 97.56100238847344,
            taup1 = 14.365959230337282,
            mA = -0.2900633974179733,
            pfip = 0.20781249999999996,
            sigmap2 = 294.44727435326945,
            Mmax = 18856.66415810754,
            taup2 = 74.33271814549165,
            sigmaMX = 88.46703086306289,
            sigmaIX = 450.1115247508614,
            sigmaMfi = 93.75,
            sigmaIfi = 135.0334574252584,
            sigma1T = 53.337720408923744,
            sigma1 = 100.16519624709748,
            sigma2T = 14.375,
            ConditionStaticStressForTubePlate1 = 74.33271814549165,
            ConditionStaticStressForTubePlate2 = 116.0,
            IsConditionStaticStressForTubePlate = true,
            deltasigma1pk = 97.56100238847344,
            sigmaa_5_2_4k = 82.92685203020243,
            deltasigma1pp = 294.44727435326945,
            sigmaa_5_2_4p = 147.22363717663472,
            ConditionStaticStressForTube1 = 53.337720408923744,
            IsConditionStaticStressForTube = true,
            deltasigma1T = 100.16519624709748,
            sigmaaT = 50.08259812354874,
            KT = 1.3,
            lR = 592.0,
            lambda = 0.9342535397879407,
            phiT = 0.7533864361093159,
            ConditionStabilityForTube2 = 109.24103323585081,
            IsConditionStabilityForTube = true,
            Ntp_d = 16763.538399555135,
            phiC2 = -0.4315510557964275,
            phiC = -0.4315510557964275,
            tau = 85.81041645942085,
            IsConditionStressBracingTube = true,
            ConditionStressBracingTube11 = 0.5756699682625417,
            ConditionStressBracingTube12 = 2.1748211042891223,
            pp = 2.5,
            spp_5_5_1 = 4.595725150090289,
            sp_5_5_1 = 6.595725150090289,
            A = 0.32,
        };

        return (inputData, calculatedData);
    }


    #endregion
}
