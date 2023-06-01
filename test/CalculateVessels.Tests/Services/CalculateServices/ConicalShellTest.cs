using CalculateVessels.Core.Interfaces;
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
        var inputData1 = new ConicalShellInput
        {
            Name = "Тестовая коническая обечайка",
            Steel = "20",
            c1 = 2.0,
            c2 = 0.8,
            c3 = 0,
            D = 1200,
            D1 = 738,
            L = 400,
            p = 0.6,
            t = 120,
            s = 8,
            phi = 0.9,
            phi_t = 1,
            phi_k = 0,
            ny = 2.4,
            IsPressureIn = true,
            ConnectionType = ConicalConnectionType.WithoutConnection,
            sT = 0,
            IsConnectionWithLittle = false,
            r = 0,
        };

        var calculatedData1 = new ConicalShellCalculated
        {
            InputData = inputData1,
            Ak = 0,
            alpha1 = 0.523,
            a1p = 59.42,
            a2p = 0,
            a1p_l = 0,
            a2p_l = 0,
            B1 = 0,
            B1_1 = 0,
            B2 = 0,
            B3 = 0,
            beta = 0,
            beta_0 = 0,
            beta_1 = 0,
            beta_2 = 0,
            beta_3 = 0,
            beta_4 = 0,
            beta_a = 0,
            beta_H = 0,
            beta_t = 0,
            ConditionForBetaH = 0,
            ConditionUseFormulas = 0.005,
            c = 2.8,
            chi_1Little = 0,
            DE = 0,
            DE_1 = 0,
            DE_2 = 0,
            Dk = 1158.397,
            E = 189000,
            IsConditionUseFormulas = true,
            lE = 0,
            SigmaAllow = 140.5,
            SigmaAllowC = 0,
            s = 5.981,
            s_tp = 0,
            s_p = 3.181,
            s_p_1 = 0,
            s_p_2 = 0,
            s_2pLittle = 0,
            p_d = 0.979,
            p_dp = 0,
            p_dBig = 0,
            p_dLittle = 0,
        };

        yield return new object[] { inputData1, calculatedData1 };
        // Assert.Equal(3.18, result.s_p, 2);
        // Assert.Equal(0.98, result.p_d, 2);
        // Assert.Equal(189000, result.E, 0);
        // Assert.Equal(140.5, result.SigmaAllow, 1);
    }
}

#endregion