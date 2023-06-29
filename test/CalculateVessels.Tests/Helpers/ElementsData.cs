using System.Collections.Generic;
using CalculateVessels.Core.Elements.Base;
using CalculateVessels.Core.Elements.Shells.Conical;
using CalculateVessels.Core.Elements.Shells.Cylindrical;
using CalculateVessels.Core.Elements.Shells.Elliptical;
using CalculateVessels.Core.Elements.Shells.Enums;
using CalculateVessels.Core.Enums;

namespace CalculateVessels.UnitTests.Helpers;

public class ElementsData
{
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
            OrdinalNumber = 1,
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
            Steel = "20",
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
            LoadingCondition = loadingCondition1,
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
            OrdinalNumber = 1,
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
            Steel = "20",
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
            LoadingCondition = loadingCondition1,
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
            OrdinalNumber = 1,
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
            Steel = "20",
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
            LoadingCondition = loadingCondition1,
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
            OrdinalNumber = 1,
            p = 0.6,
            t = 150,
            PressureType = PressureType.Inside
        };

        var loadingCondition2 = new LoadingCondition
        {
            OrdinalNumber = 2,
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
            Steel = "12Х18Н10Т",
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
            LoadingCondition = loadingCondition1,
            s_p = 1.073,
            s = 3.873,
            p_d = 3.984,
            SigmaAllow = 168,
            ConditionStability = 0.15,
            E = 199000
        };

        var result2 = new CylindricalShellCalculatedOneLoading
        {
            LoadingCondition = loadingCondition2,
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
            OrdinalNumber = 1,
            p = 0.6,
            t = 120,
            PressureType = PressureType.Inside
        };

        var inputData = new ConicalShellInput
        {
            LoadingConditions = new List<LoadingCondition> { loadingCondition1 },
            Name = "Тестовая коническая обечайка",
            Steel = "20",
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
            LoadingCondition = loadingCondition1,
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
            OrdinalNumber = 1,
            p = 0.6,
            t = 120,
            PressureType = PressureType.Inside
        };

        var inputData = new EllipticalShellInput
        {
            LoadingConditions = new List<LoadingCondition> { loadingCondition1 },
            Name = "Тестовая эллиптическая обечайка",
            Steel = "Ст3",
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
            LoadingCondition = loadingCondition1,
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
            OrdinalNumber = 1,
            p = 0.6,
            t = 120,
            PressureType = PressureType.Outside
        };

        var inputData = new EllipticalShellInput
        {
            LoadingConditions = new List<LoadingCondition> { loadingCondition1 },
            Name = "Тестовая эллиптическая обечайка",
            Steel = "Ст3",
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
            LoadingCondition = loadingCondition1,
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
            OrdinalNumber = 1,
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
            Steel = "Ст3",
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
            LoadingCondition = loadingCondition1,
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
}
