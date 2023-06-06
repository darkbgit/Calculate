using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Base;
using CalculateVessels.Core.Shells.Elliptical;
using CalculateVessels.Core.Shells.Enums;
using CalculateVessels.Data.PhysicalData;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace CalculateVessels.UnitTests.Services.CalculateServices;

public class EllipticalShellTest
{
    private readonly ICalculateService<EllipticalShellInput> _calculateService;

    public EllipticalShellTest()
    {
        _calculateService = new EllipticalShellCalculateService(new PhysicalDataService());
    }

    [Theory]
    [MemberData(nameof(EllipticalShellTestData.GetData), MemberType = typeof(EllipticalShellTestData))]
    public void EllipticalShell(EllipticalShellInput inputData, EllipticalShellCalculated calculatedData)
    {
        //Act
        var result = _calculateService.Calculate(inputData) as EllipticalShellCalculated;

        //Assert
        const double precision = 0.001;
        result.Should().BeEquivalentTo(calculatedData, options => options
            .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, precision))
            .WhenTypeIs<double>());
    }
}

#region TestData

public class EllipticalShellTestData
{
    public static IEnumerable<object[]> GetData()
    {
        var (inputData1, calculatedData1) = GetData1();
        yield return new object[] { inputData1, calculatedData1 };

        var (inputData2, calculatedData2) = GetData2();
        yield return new object[] { inputData2, calculatedData2 };

        var (inputData3, calculatedData3) = GetData3();
        yield return new object[] { inputData3, calculatedData3 };
    }

    private static (EllipticalShellInput, EllipticalShellCalculated) GetData1()
    {
        var loadingCondition1 = new LoadingCondition
        {
            OrdinalNumber = 1,
            p = 0.6,
            t = 120,
            IsPressureIn = true
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

    private static (EllipticalShellInput, EllipticalShellCalculated) GetData2()
    {
        var loadingCondition1 = new LoadingCondition
        {
            OrdinalNumber = 1,
            p = 0.6,
            t = 120,
            IsPressureIn = false
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

    private static (EllipticalShellInput, EllipticalShellCalculated) GetData3()
    {
        var loadingCondition1 = new LoadingCondition
        {
            OrdinalNumber = 1,
            p = 0.6,
            t = 120,
            EAllow = 189000,
            SigmaAllow = 147,
            IsPressureIn = false
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
}

#endregion