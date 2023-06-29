using CalculateVessels.Core.Elements.Shells.Cylindrical;
using CalculateVessels.Forms;
using CalculateVessels.UnitTests.Helpers;
using FluentAssertions;
using ReflectionMagic;
using Xunit;

namespace CalculateVessels.UnitTests.Forms;

public class CylindricalFormTest : BaseFormTest<CylindricalShellForm, CylindricalShellInput>
{
    public CylindricalFormTest()
    {
        Validator = new CylindricalShellInputValidator();

        Form = new CylindricalShellForm(CalculateServicesMock.Object,
            PhysicalDataService,
            Validator,
            FormFactoryMock.Object);

        Form.Show();
    }


    [Theory]
    [MemberData(nameof(ElementsData.GetCylindricalInputData), MemberType = typeof(ElementsData))]
    public void LoadAndCollectInputData(CylindricalShellInput inputData)
    {
        Form.AsDynamic().LoadInputData(inputData);

        Form.AsDynamic().TryCollectInputData(out CylindricalShellInput result);

        //Assert
        const double precision = 0.001;
        result.Should().BeEquivalentTo(inputData, options => options
            .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, precision))
            .WhenTypeIs<double>());
    }
}