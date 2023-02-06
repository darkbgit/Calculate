using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Elliptical;
using FluentAssertions;
using Xunit;

namespace CalculateVessels.UnitTests.Services.CalculateServices;

public class EllipticalShellTest
{
    private readonly ICalculateService<EllipticalShellInput> _calculateService;

    public EllipticalShellTest()
    {
        _calculateService = new EllipticalShellCalculateService();
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