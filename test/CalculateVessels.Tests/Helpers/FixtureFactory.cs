using AutoFixture;
using AutoFixture.AutoMoq;

namespace CalculateVessels.UnitTests.Helpers;

internal static class FixtureFactory
{
    public static IFixture GetFixture()
    {
        return new Fixture().Customize(new AutoMoqCustomization());
    }
}