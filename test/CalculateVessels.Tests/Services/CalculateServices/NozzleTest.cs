using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Nozzle;
using CalculateVessels.Data.PhysicalData;
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
    [MemberData(nameof(NozzleTestData.GetData), MemberType = typeof(NozzleTestData))]
    public void NozzleInShellTest(NozzleInput nozzleInput, NozzleCalculated nozzleCalculated)
    {
        //Act
        var result = _calculateService.Calculate(nozzleInput) as NozzleCalculated;

        //Assert
        var precision = 0.001;
        result.Should().BeEquivalentTo(nozzleCalculated, options => options
            .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, precision))
            .WhenTypeIs<double>());
    }
}