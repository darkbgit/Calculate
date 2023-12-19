using CalculateVessels.Core.Elements.HeatExchangers.HeatExchangerStationaryTubePlates;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.UnitTests.Helpers;
using FluentAssertions;
using Xunit;

namespace CalculateVessels.UnitTests.Services.CalculateServices;

public class HeatExchangerStationaryTubePlatesTest : CalculateServicesBaseTest
{
    private readonly ICalculateService<HeatExchangerStationaryTubePlatesInput> _calculateService;

    public HeatExchangerStationaryTubePlatesTest()
    {
        _calculateService = new HeatExchangerStationaryTubePlatesCalculateService(PhysicalDataService);
    }

    [Theory]
    [MemberData(nameof(ElementsData.GetHeatExchangerStationaryTubePlatesInputAndCalculatedData), MemberType = typeof(ElementsData))]
    public void HeatExchangerStationaryTubePlates(HeatExchangerStationaryTubePlatesInput inputData, HeatExchangerStationaryTubePlatesCalculated calculatedData)
    {
        //Act
        var result = _calculateService.Calculate(inputData) as HeatExchangerStationaryTubePlatesCalculated;

        //Assert
        const double precision = 0.001;
        result.Should().BeEquivalentTo(calculatedData, options => options
            .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, precision))
            .WhenTypeIs<double>());
    }
}