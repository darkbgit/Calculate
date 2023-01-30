using CalculateVessels.Core.Interfaces;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace CalculateVessels.UnitTests.Services.CalculateServices;

public abstract class BaseCalculateServiceTest<T, T1, T2>
    where T : class, IInputData, new()
    where T1 : class, ICalculateService<T>, new()
    where T2 : class
{
    private readonly ICalculateService<T> _calculateService;

    protected BaseCalculateServiceTest()
    {
        _calculateService = new T1();
    }

    [Theory]
    [MemberData(nameof(Data))]
    public void CylindricalShell(T inputData, T2 calculatedData)
    {
        //Act
        var result = _calculateService.Calculate(inputData) as T2;

        //Assert
        var precision = 0.001;
        result.Should().BeEquivalentTo(calculatedData, options => options
            .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, precision))
            .WhenTypeIs<double>());
    }

    public static abstract IEnumerable<object[]> Data { get; }
}