using CalculateVessels.Core.Elements.Shells.Conical;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.PhysicalData;
using CalculateVessels.UnitTests.Helpers;
using FluentAssertions;
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
    [MemberData(nameof(ElementsData.GetConicalInputAndCalculatedData), MemberType = typeof(ElementsData))]
    public void ConicalShell(ConicalShellInput inputData, ConicalShellCalculated calculatedData)
    {
        //Act
        var result = _calculateService.Calculate(inputData) as ConicalShellCalculated;

        //Assert
        const double precision = 0.001;
        result.Should().BeEquivalentTo(calculatedData, options => options
            .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, precision))
            .WhenTypeIs<double>());
    }
}