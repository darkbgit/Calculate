using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Conical;
using FluentAssertions;
using Xunit;

namespace CalculateVessels.UnitTests.Services.CalculateServices
{
    public class ConicalShellTest
    {
        private readonly ICalculateService<ConicalShellInput> _calculateService;

        public ConicalShellTest()
        {
            _calculateService = new ConicalShellCalculateService();
        }

        [Theory]
        [MemberData(nameof(ConicalShellTestData.GetData), MemberType = typeof(ConicalShellTestData))]
        public void ConicalShell(ConicalShellInput inputData, ConicalShellCalculated calculatedData)
        {
            //Act
            var result = _calculateService.Calculate(inputData) as ConicalShellCalculated;

            //Assert
            var precision = 0.001;
            result.Should().BeEquivalentTo(calculatedData, options => options
                .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, precision))
                .WhenTypeIs<double>());
        }
    }
}