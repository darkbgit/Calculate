using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using AutoFixture;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Data.PhysicalData;
using CalculateVessels.Forms.Base;
using CalculateVessels.Helpers;
using CalculateVessels.UnitTests.Helpers;
using FluentValidation;
using Moq;

namespace CalculateVessels.UnitTests.Forms;

public abstract class BaseFormTest<T, T1> : IDisposable
    where T : BaseCalculateForm<T1>, IBaseForm, IDisposable
    where T1 : class, IInputData
{
    protected readonly Mock<IEnumerable<ICalculateService<T1>>> CalculateServicesMock = new();
    protected readonly IPhysicalDataService PhysicalDataService = new PhysicalDataService();
    protected IValidator<T1> Validator;
    protected readonly Mock<IFormFactory> FormFactoryMock = new();
    protected T Form;
    protected readonly IFixture Fixture;

    protected BaseFormTest()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-en");

        Fixture = FixtureFactory.GetFixture();

        var calculateServiceMock = new Mock<ICalculateService<T1>>();

        calculateServiceMock
            .SetupGet(x => x.Name)
            .Returns(Fixture.Create<string>());

        var calculateServices = new List<ICalculateService<T1>> { calculateServiceMock.Object };
        CalculateServicesMock
            .Setup(x => x.GetEnumerator())
            .Returns(calculateServices.GetEnumerator());

        CalculateServicesMock
            .As<IEnumerable>()
            .Setup(x => x.GetEnumerator())
            .Returns(calculateServices.GetEnumerator());
    }

    public void Dispose()
    {
        Form.Dispose();
    }
}