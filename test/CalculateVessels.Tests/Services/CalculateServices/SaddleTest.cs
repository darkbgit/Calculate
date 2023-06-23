using CalculateVessels.Core.Elements.Supports.Saddle;
using CalculateVessels.Core.Enums;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.PhysicalData;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace CalculateVessels.UnitTests.Services.CalculateServices;

public class SaddleTest
{
    private readonly ICalculateService<SaddleInput> _calculateService;

    public SaddleTest()
    {
        _calculateService = new SaddleCalculateService(new PhysicalDataService());
    }

    [Theory]
    [MemberData(nameof(SaddleTestData.GetData), MemberType = typeof(SaddleTestData))]
    public void Saddle(SaddleInput inputData, SaddleCalculated calculatedData)
    {
        //Act
        var result = _calculateService.Calculate(inputData) as SaddleCalculated;

        //Assert
        const double precision = 0.001;
        result.Should().BeEquivalentTo(calculatedData, options => options
            .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, precision))
            .WhenTypeIs<double>());
    }
}

#region TestData

public class SaddleTestData
{
    public static IEnumerable<object[]> GetData()
    {
        var (inputData1, calculatedData1) = GetData1();
        yield return new object[] { inputData1, calculatedData1 };
    }

    private static (SaddleInput inputData, SaddleCalculated calculatedData) GetData1()
    {
        var inputData = new SaddleInput
        {
            IsPressureIn = true,
            D = 600,
            s = 8,
            c = 2.8,
            fi = 1,
            Steel = "20",
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
}

#endregion