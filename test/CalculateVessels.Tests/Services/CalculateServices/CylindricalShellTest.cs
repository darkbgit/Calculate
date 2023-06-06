using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Base;
using CalculateVessels.Core.Shells.Cylindrical;
using CalculateVessels.Data.PhysicalData;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace CalculateVessels.UnitTests.Services.CalculateServices;

public class CylindricalShellTest
{
    private readonly ICalculateService<CylindricalShellInput> _calculateService;

    public CylindricalShellTest()
    {
        _calculateService = new CylindricalShellCalculateService(new PhysicalDataService());
    }



    [Theory]
    [MemberData(nameof(CylindricalShellTestData.GetData), MemberType = typeof(CylindricalShellTestData))]
    public void CylindricalShell(CylindricalShellInput inputData, CylindricalShellCalculated calculatedData)
    {
        //Act
        var result = _calculateService.Calculate(inputData) as CylindricalShellCalculated;

        //Assert
        const double precision = 0.001;
        result.Should().BeEquivalentTo(calculatedData, options => options
            .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, precision))
            .WhenTypeIs<double>());
    }
}

#region TestData

public class CylindricalShellTestData
{
    public static IEnumerable<object[]> GetData()
    {
        var (inputData1, calculatedData1) = GetData1();
        yield return new object[] { inputData1, calculatedData1 };

        var (inputData2, calculatedData2) = GetData2();
        yield return new object[] { inputData2, calculatedData2 };

        var (inputData3, calculatedData3) = GetData3();
        yield return new object[] { inputData3, calculatedData3 };

        var (inputData4, calculatedData4) = GetData4();
        yield return new object[] { inputData4, calculatedData4 };
    }

    private static (CylindricalShellInput, CylindricalShellCalculated) GetData1()
    {
        var loadingCondition1 = new LoadingCondition
        {
            OrdinalNumber = 1,
            p = 0.6,
            t = 120,
            IsPressureIn = true
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

    private static (CylindricalShellInput, CylindricalShellCalculated) GetData2()
    {
        var loadingCondition1 = new LoadingCondition
        {
            OrdinalNumber = 1,
            p = 0.6,
            t = 120,
            IsPressureIn = false
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

    private static (CylindricalShellInput, CylindricalShellCalculated) GetData3()
    {
        var loadingCondition1 = new LoadingCondition
        {
            OrdinalNumber = 1,
            p = 0.6,
            t = 120,
            IsPressureIn = false,
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
            F = 0,
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

    private static (CylindricalShellInput, CylindricalShellCalculated) GetData4()
    {
        var loadingCondition1 = new LoadingCondition
        {
            OrdinalNumber = 1,
            p = 0.6,
            t = 150,
            IsPressureIn = true
        };

        var loadingCondition2 = new LoadingCondition
        {
            OrdinalNumber = 2,
            p = 0.8,
            t = 120,
            IsPressureIn = false
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
}

#endregion