using System.Collections.Generic;
using CalculateVessels.Core.Elements.Bottoms.Enums;
using CalculateVessels.Core.Elements.Bottoms.FlatBottom;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.PhysicalData;
using FluentAssertions;
using Xunit;

namespace CalculateVessels.UnitTests.Services.CalculateServices;

public class FlatBottomTest
{
    private readonly ICalculateService<FlatBottomInput> _calculateService;

    public FlatBottomTest()
    {
        _calculateService = new FlatBottomCalculateService(new PhysicalDataService());
    }

    [Theory]
    [MemberData(nameof(FlatBottomTestData.GetData), MemberType = typeof(FlatBottomTestData))]
    public void FlatBottom(FlatBottomInput inputData, FlatBottomCalculated calculatedData)
    {
        //Act
        var result = _calculateService.Calculate(inputData) as FlatBottomCalculated;

        //Assert
        var precision = 0.001;
        result.Should().BeEquivalentTo(calculatedData, options => options
            .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, precision))
            .WhenTypeIs<double>());
    }
}

#region testData

public class FlatBottomTestData
{
    public static IEnumerable<object[]> GetData()
    {
        var inputData1 = new FlatBottomInput
        {
            Name = "Тестовая плоское днище",
            Steel = "20",
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
            fi = 1,
            gamma = 0,
            Hole = HoleInFlatBottom.WithoutHole,
            h1 = 0,
            p = 0.6,
            r = 0,
            SigmaAllow = 0,
            s = 8,
            s1 = 16,
            s2 = 0,
            FlatBottomType = 2,
            t = 50
        };

        var calculatedData1 = new FlatBottomCalculated
        {
            InputData = inputData1,
            ConditionUseFormulas = 0.033,
            c = 2.8,
            Dp = 400,
            IsConditionFixed = false,
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

        yield return new object[] { inputData1, calculatedData1 };
        // Assert.Equal(3.18, result.s_p, 2);
        // Assert.Equal(0.98, result.p_d, 2);
        // Assert.Equal(189000, result.E, 0);
        // Assert.Equal(140.5, result.SigmaAllow, 1);
    }
}

#endregion