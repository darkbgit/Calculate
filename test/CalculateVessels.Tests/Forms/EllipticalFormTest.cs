using CalculateVessels.Core.Elements.Shells.Elliptical;
using CalculateVessels.Forms;
using CalculateVessels.UnitTests.Helpers;
using FluentAssertions;
using ReflectionMagic;
using Xunit;

namespace CalculateVessels.UnitTests.Forms;

public class EllipticalFormTest : BaseFormTest<EllipticalShellForm, EllipticalShellInput>
{
    public EllipticalFormTest()
    {
        Validator = new EllipticalShellInputValidator();

        Form = new EllipticalShellForm(CalculateServicesMock.Object,
            PhysicalDataService,
            Validator,
            FormFactoryMock.Object);

        Form.Show();
    }


    [Theory]
    [MemberData(nameof(ElementsData.GetEllipticalInputData), MemberType = typeof(ElementsData))]
    public void LoadAndCollectInputData(EllipticalShellInput inputData)
    {
        Form.AsDynamic().LoadInputData(inputData);

        Form.AsDynamic().TryCollectInputData(out EllipticalShellInput result);

        //Assert
        const double precision = 0.001;
        result.Should().BeEquivalentTo(inputData, options => options
            .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, precision))
            .WhenTypeIs<double>());
    }
}