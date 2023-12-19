using CalculateVessels.Core.Elements.Supports.Saddle;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.UnitTests.Helpers;
using FluentAssertions;
using Xunit;

namespace CalculateVessels.UnitTests.Services.CalculateServices;

public class SaddleTest : CalculateServicesBaseTest
{
    private readonly ICalculateService<SaddleInput> _calculateService;

    public SaddleTest()
    {
        _calculateService = new SaddleCalculateService(PhysicalDataService);
    }

    [Theory]
    [MemberData(nameof(ElementsData.GetSaddleInputAndCalculatedData), MemberType = typeof(ElementsData))]
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