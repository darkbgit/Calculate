using CalculateVessels.Core.Elements.Bottoms.FlatBottom;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.UnitTests.Helpers;
using FluentAssertions;
using Xunit;

namespace CalculateVessels.UnitTests.Services.CalculateServices;

public class FlatBottomTest : CalculateServicesBaseTest
{
    private readonly ICalculateService<FlatBottomInput> _calculateService;

    public FlatBottomTest()
    {
        _calculateService = new FlatBottomCalculateService(PhysicalDataService);
    }

    [Theory]
    [MemberData(nameof(ElementsData.GetFlatBottomInputAndCalculatedData), MemberType = typeof(ElementsData))]
    public void FlatBottom(FlatBottomInput inputData, FlatBottomCalculated calculatedData)
    {
        //Act
        var result = _calculateService.Calculate(inputData) as FlatBottomCalculated;

        //Assert
        var precision = 0.001;
        result.Should().BeEquivalentTo(calculatedData, options => options
            .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, precision))
            .WhenTypeIs<double>());
    }
}
