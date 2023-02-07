using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Cylindrical;
using CalculateVessels.Data.PhysicalData;
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
    [MemberData(nameof(CylindricalShellTestData.GetData), MemberType = typeof(CylindricalShellTestData))]
    public void CylindricalShell(CylindricalShellInput inputData, CylindricalShellCalculated calculatedData)
    {
        //Act
        var result = _calculateService.Calculate(inputData) as CylindricalShellCalculated;

        //Assert
        var precision = 0.001;
        result.Should().BeEquivalentTo(calculatedData, options => options
            .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, precision))
            .WhenTypeIs<double>());
    }
}