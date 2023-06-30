using CalculateVessels.Core.Elements.Shells.Nozzle;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.PhysicalData;
using CalculateVessels.UnitTests.Helpers;
using FluentAssertions;
using Xunit;

namespace CalculateVessels.UnitTests.Services.CalculateServices;

public class NozzleTest
{
    private readonly ICalculateService<NozzleInput> _calculateService;

    public NozzleTest()
    {
        _calculateService = new NozzleCalculateService(new PhysicalDataService());
    }

    [Theory]
    [MemberData(nameof(ElementsData.GetNozzleInputAndCalculatedData), MemberType = typeof(ElementsData))]
    public void NozzleInShellTest(NozzleInput nozzleInput, NozzleCalculated nozzleCalculated)
    {
        //Act
        var result = _calculateService.Calculate(nozzleInput) as NozzleCalculated;

        //Assert
        const double precision = 0.001;
        result.Should().BeEquivalentTo(nozzleCalculated, options => options
            .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, precision))
            .WhenTypeIs<double>());
    }
}