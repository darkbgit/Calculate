using CalculateVessels.Core.Elements.Bottoms.FlatBottomWithAdditionalMoment;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.UnitTests.Helpers;
using FluentAssertions;
using Xunit;

namespace CalculateVessels.UnitTests.Services.CalculateServices;

public class FlatBottomTestWithAdditionalMoment : CalculateServicesBaseTest
{
    private readonly ICalculateService<FlatBottomWithAdditionalMomentInput> _calculateService;

    public FlatBottomTestWithAdditionalMoment()
    {
        _calculateService = new FlatBottomWithAdditionalMomentCalculateService(PhysicalDataService);
    }

    [Theory]
    [MemberData(nameof(ElementsData.GetFlatBottomWithAdditionalMomentInputAndCalculatedData), MemberType = typeof(ElementsData))]
    public void FlatBottom(FlatBottomWithAdditionalMomentInput inputData, FlatBottomWithAdditionalMomentCalculated calculatedData)
    {
        //Act
        var result = _calculateService.Calculate(inputData) as FlatBottomWithAdditionalMomentCalculated;

        //Assert
        var precision = 0.001;
        result.Should().BeEquivalentTo(calculatedData, options => options
            .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, precision))
            .WhenTypeIs<double>());
    }
}
