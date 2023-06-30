using CalculateVessels.Core.Elements.Shells.Elliptical;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.PhysicalData;
using CalculateVessels.UnitTests.Helpers;
using FluentAssertions;
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
    [MemberData(nameof(ElementsData.GetEllipticalInputAndCalculatedData), MemberType = typeof(ElementsData))]
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