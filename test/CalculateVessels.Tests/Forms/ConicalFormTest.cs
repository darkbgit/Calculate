using CalculateVessels.Core.Elements.Shells.Conical;
using CalculateVessels.Forms;
using CalculateVessels.UnitTests.Helpers;
using FluentAssertions;
using ReflectionMagic;
using Xunit;

namespace CalculateVessels.UnitTests.Forms;

public class ConicalFormTest : BaseFormTest<ConicalShellForm, ConicalShellInput>
{
    public ConicalFormTest()
    {
        Validator = new ConicalShellInputValidator();

        Form = new ConicalShellForm(CalculateServicesMock.Object,
            PhysicalDataService,
            Validator,
            FormFactoryMock.Object);

        Form.Show();
    }


    [Theory]
    [MemberData(nameof(ElementsData.GetConicalInputData), MemberType = typeof(ElementsData))]
    public void LoadAndCollectInputData(ConicalShellInput inputData)
    {
        Form.AsDynamic().LoadInputData(inputData);

        Form.AsDynamic().TryCollectInputData(out ConicalShellInput result);

        //Assert
        const double precision = 0.001;
        result.Should().BeEquivalentTo(inputData, options => options
            .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, precision))
            .WhenTypeIs<double>());
    }
}