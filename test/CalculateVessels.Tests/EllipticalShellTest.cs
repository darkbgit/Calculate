using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Elliptical;
using CalculateVessels.Core.Shells.Enums;
using FluentAssertions;
using Xunit;

namespace CalculateVessels.Tests;

public class EllipticalShellTest
{
    private readonly ICalculateService<EllipticalShellInput> _calculateService;

    public EllipticalShellTest()
    {
        _calculateService = new EllipticalShellCalculateService();
    }

    [Fact]
    public void PressureIn()
    {
        var inputData = new EllipticalShellInput
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

        var calculatedData = new EllipticalShellCalculated
        {
            InputData = inputData,
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

        //Act
        var result = _calculateService.Calculate(inputData) as EllipticalShellCalculated;

        //Assert
        var precision = 0.001;
        result.Should().BeEquivalentTo(calculatedData, options => options
            .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, precision))
            .WhenTypeIs<double>());
    }

    [Fact]
    public void PressureOut()
    {
        var inputData = new EllipticalShellInput
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

        var calculatedData = new EllipticalShellCalculated
        {
            InputData = inputData,
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

        //Act
        var result = _calculateService.Calculate(inputData) as EllipticalShellCalculated;

        //Assert
        var precision = 0.001;
        result.Should().BeEquivalentTo(calculatedData, options => options
            .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, precision))
            .WhenTypeIs<double>());
    }

    [Fact]
    public void PressureOutWithHandleSigmaAndE()
    {
        var inputData = new EllipticalShellInput
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

        var calculatedData = new EllipticalShellCalculated
        {
            InputData = inputData,
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

        //Act
        var result = _calculateService.Calculate(inputData) as EllipticalShellCalculated;

        //Assert
        var precision = 0.001;
        result.Should().BeEquivalentTo(calculatedData, options => options
            .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, precision))
            .WhenTypeIs<double>());
    }
}