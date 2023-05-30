using CalculateVessels.Core.Interfaces;
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
        var precision = 0.001;
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
        var inputData1 = new EllipticalShellInput
        {
            Name = "Тестовая эллиптическая обечайка",
            Steel = "Ст3",
            c1 = 2.0,
            D = 1000,
            c2 = 0.8,
            c3 = 1.2,
            p = 0.6,
            t = 120,
            s = 8,
            fi = 1.0,
            ny = 2.4,
            EllipseH = 250,
            Ellipseh1 = 25,
            EllipticalBottomType = EllipticalBottomType.Elliptical,
            IsPressureIn = true
        };

        var calculatedData1 = new EllipticalShellCalculated
        {
            InputData = inputData1,
            c = 4,
            s_p = 2.043,
            s = 6.043,
            p_de = 0,
            p_d = 1.174,
            SigmaAllow = 147,
            //ErrorList => _errorList;
            IsConditionUseFormulas = true,
            //_errorList = new();
            b = 0,
            b_2 = 0,
            B1 = 0,
            B1_2 = 0,
            ConditionStability = 0,
            l = 0,
            s_p_1 = 0,
            s_p_2 = 0,
            p_dp = 0,
            E = 189000,
            EllipseR = 1000
        };

        var inputData2 = new EllipticalShellInput
        {
            Name = "Тестовая эллиптическая обечайка",
            Steel = "Ст3",
            c1 = 2.0,
            D = 1000,
            c2 = 0.8,
            c3 = 1.2,
            p = 0.6,
            t = 120,
            //E = 189000,
            s = 10,
            //SigmaAllow = 1,
            fi = 1.0,
            ny = 2.4,
            IsPressureIn = false,
            EllipseH = 250,
            Ellipseh1 = 25,
            EllipticalBottomType = EllipticalBottomType.Elliptical
        };

        var calculatedData2 = new EllipticalShellCalculated
        {
            InputData = inputData2,
            c = 4,
            s_p = 4.879,
            s = 8.879,
            p_de = 0.82,
            p_d = 0.743,
            SigmaAllow = 147,
            //ErrorList => _errorList;
            IsConditionUseFormulas = true,
            //_errorList = new();
            b = 0,
            b_2 = 0,
            B1 = 0,
            B1_2 = 0,
            ConditionStability = 0,
            l = 0,
            s_p_1 = 4.879,
            s_p_2 = 2.449,
            p_dp = 1.759,
            E = 189000,
            EllipseR = 1000,
            EllipseKePrev = 0.9,
            Ellipsex = 0.09,
            EllipseKe = 0.948
        };


        var inputData3 = new EllipticalShellInput
        {
            Name = "Тестовая эллиптическая обечайка",
            Steel = "Ст3",
            c1 = 2.0,
            D = 1000,
            c2 = 0.8,
            c3 = 1.2,
            p = 0.6,
            t = 120,
            E = 189000,
            s = 10,
            SigmaAllow = 147,
            fi = 1.0,
            ny = 2.4,
            IsPressureIn = false,
            EllipseH = 250,
            Ellipseh1 = 25,
            EllipticalBottomType = EllipticalBottomType.Elliptical
        };

        var calculatedData3 = new EllipticalShellCalculated
        {
            InputData = inputData3,
            c = 4,
            s_p = 4.879,
            s = 8.879,
            p_de = 0.82,
            p_d = 0.743,
            SigmaAllow = 147,
            //ErrorList => _errorList;
            IsConditionUseFormulas = true,
            //_errorList = new();
            b = 0,
            b_2 = 0,
            B1 = 0,
            B1_2 = 0,
            ConditionStability = 0,
            l = 0,
            s_p_1 = 4.879,
            s_p_2 = 2.449,
            p_dp = 1.759,
            E = 189000,
            EllipseR = 1000,
            EllipseKePrev = 0.9,
            Ellipsex = 0.09,
            EllipseKe = 0.948
        };

        yield return new object[] { inputData1, calculatedData1 };
        yield return new object[] { inputData2, calculatedData2 };
        yield return new object[] { inputData3, calculatedData3 };
    }
}

#endregion