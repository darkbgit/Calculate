using CalculateVessels.Core.Elements.Shells.Cylindrical;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.PhysicalData;
using CalculateVessels.UnitTests.Helpers;
using FluentAssertions;
using Xunit;

namespace CalculateVessels.UnitTests.Services.CalculateServices;

public class CylindricalShellTest
{
    private readonly ICalculateService<CylindricalShellInput> _calculateService;

    public CylindricalShellTest()
    {
        _calculateService = new CylindricalShellCalculateService(new PhysicalDataService());
    }



    [Theory]
    [MemberData(nameof(ElementsData.GetCylindricalInputAndCalculatedData), MemberType = typeof(ElementsData))]
    public void CylindricalShell(CylindricalShellInput inputData, CylindricalShellCalculated calculatedData)
    {
        //Act
        var result = _calculateService.Calculate(inputData) as CylindricalShellCalculated;

        //Assert
        const double precision = 0.001;
        result.Should().BeEquivalentTo(calculatedData, options => options
            .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, precision))
            .WhenTypeIs<double>());
    }
}