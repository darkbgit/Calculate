using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Base;
using CalculateVessels.Core.Shells.Conical;
using CalculateVessels.Core.Shells.Enums;
using CalculateVessels.Data.PhysicalData;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace CalculateVessels.UnitTests.Services.CalculateServices;

public class ConicalShellTest
{
    private readonly ICalculateService<ConicalShellInput> _calculateService;

    public ConicalShellTest()
    {
        _calculateService = new ConicalShellCalculateService(new PhysicalDataService());
    }

    [Theory]
    [MemberData(nameof(ConicalShellTestData.GetData), MemberType = typeof(ConicalShellTestData))]
    public void ConicalShell(ConicalShellInput inputData, ConicalShellCalculated calculatedData)
    {
        //Act
        var result = _calculateService.Calculate(inputData) as ConicalShellCalculated;

        //Assert
        var precision = 0.001;
        result.Should().BeEquivalentTo(calculatedData, options => options
            .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, precision))
            .WhenTypeIs<double>());
    }
}

#region testData

public class ConicalShellTestData
{
    public static IEnumerable<object[]> GetData()
    {
        var (inputData1, calculatedData1) = GetData1();
        yield return new object[] { inputData1, calculatedData1 };
    }

    private static (ConicalShellInput, ConicalShellCalculated) GetData1()
    {
        var loadingCondition1 = new LoadingCondition
        {
            OrdinalNumber = 1,
            p = 0.6,
            t = 120,
            IsPressureIn = true
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
}

#endregion