using System.Collections.Generic;
using CalculateVessels.Core.Elements.Supports.BracketVertical;
using CalculateVessels.Core.Enums;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.PhysicalData;
using FluentAssertions;
using Xunit;

namespace CalculateVessels.UnitTests.Services.CalculateServices;

public class BracketVerticalTest
{
    private readonly ICalculateService<BracketVerticalInput> _calculateService;

    public BracketVerticalTest()
    {
        _calculateService = new BracketVerticalCalculateService(new PhysicalDataService());
    }

    [Theory]
    [MemberData(nameof(BracketVerticalTestData.GetData), MemberType = typeof(BracketVerticalTestData))]
    public void FlatBottom(BracketVerticalInput inputData, BracketVerticalCalculated calculatedData)
    {
        //Act
        var result = _calculateService.Calculate(inputData) as BracketVerticalCalculated;

        //Assert
        const double precision = 0.001;
        result.Should().BeEquivalentTo(calculatedData, options => options
            .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, precision))
            .WhenTypeIs<double>());
    }
}

#region testData

public class BracketVerticalTestData
{
    public static IEnumerable<object[]> GetData()
    {
        var inputData1 = new BracketVerticalInput
        {
            IsAssembly = false,
            PressureType = PressureType.Outside,
            PreciseMontage = false,
            ReinforcingPad = false,
            BracketVerticalType = BracketVerticalType.A,
            b2 = 0,
            b3 = 0,
            b4 = 150,
            c = 2.8,
            D = 800,
            e1 = 0,
            g = 80,
            G = 20000,
            h = 0,
            h1 = 200,
            l1 = 100,
            M = 0,
            p = 0.6,
            phi = 1,
            Q = 0,
            s = 8,
            s2 = 0,
            SigmaAllow = 0,
            t = 50,
            n = 2,
            N = 1000,
            Name = "Тестовая плоское днище",
            Steel = "20"
        };

        var calculatedData1 = new BracketVerticalCalculated()
        {
            InputData = inputData1,
            IsConditionUseFormulas = true,
            ConditionUseFormulas1 = 0.006,
            ConditionUseFormulas2 = 40,
            ConditionUseFormulas3 = 0.25,
            ConditionUseFormulas4 = 0.187,
            ConditionUseFormulas5 = 0,
            ConditionUseFormulas6 = 0,
            ConditionUseFormulas7 = 0,
            ConditionUseFormulas8 = 0,
            Dp = 800,
            e1 = 83.333,
            e1e = 83.333,
            F1 = 10000,
            F1Allow = 21419.183,
            K1 = 1.451,
            K2 = 1.25,
            K7 = 0.717,
            K71 = 0,
            K72 = 0,
            K8 = 0,
            K81 = 0,
            K82 = 0,
            sigma_m = 46.153,
            SigmaAllow = 145,
            sigmaid = 263,
            v1 = 0.3,
            v2 = -0.254,
            x = 4.342,
            y = -1.386,
            z = -1.673
        };

        yield return new object[] { inputData1, calculatedData1 };
    }
}

#endregion