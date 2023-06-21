using CalculateVessels.Core.Base;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Cylindrical;
using CalculateVessels.Core.Shells.Nozzle;
using CalculateVessels.Core.Shells.Nozzle.Enums;
using CalculateVessels.Data.PhysicalData;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace CalculateVessels.UnitTests.Services.CalculateServices;

public class NozzleTest
{
    private readonly ICalculateService<NozzleInput> _calculateService;

    public NozzleTest()
    {
        _calculateService = new NozzleCalculateService(new PhysicalDataService());
    }

    [Theory]
    [MemberData(nameof(NozzleTestData.GetData), MemberType = typeof(NozzleTestData))]
    public void NozzleInShellTest(NozzleInput nozzleInput, NozzleCalculated nozzleCalculated)
    {
        //Act
        var result = _calculateService.Calculate(nozzleInput) as NozzleCalculated;

        //Assert
        const double precision = 0.001;
        result.Should().BeEquivalentTo(nozzleCalculated, options => options
            .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, precision))
            .WhenTypeIs<double>());
    }
}

#region TestData

public class NozzleTestData
{
    public static IEnumerable<object[]> GetData()
    {
        var (inputData1, calculatedData1) = GetData1();
        yield return new object[] { inputData1, calculatedData1 };
    }

    private static (NozzleInput, NozzleCalculated) GetData1()
    {
        var loadingCondition1 = new LoadingCondition
        {
            OrdinalNumber = 1,
            p = 0.6,
            t = 150,
            IsPressureIn = true
        };

        var shellInput = new CylindricalShellInput
        {
            LoadingConditions = new List<LoadingCondition> { loadingCondition1 },
            Name = "Тестовая цилиндрическая обечайка",
            Steel = "12Х18Н10Т",
            c1 = 2.0,
            D = 600,
            c2 = 0.8,
            c3 = 0,
            s = 8,
            phi = 1,
            ny = 2.4,
        };

        var shellCommon = new CylindricalShellCalculatedCommon
        {
            c = 2.8,
            IsConditionUseFormulas = true
        };

        var shellResult1 = new CylindricalShellCalculatedOneLoading
        {
            LoadingCondition = loadingCondition1,
            s_p = 1.073,
            s = 3.873,
            p_d = 2.887,
            SigmaAllow = 168,
            ConditionStability = 0.208,
            E = 199000
        };

        var shellCalculated = new CylindricalShellCalculated(shellCommon,
            new List<CylindricalShellCalculatedOneLoading> { shellResult1 })
        {
            InputData = shellInput
        };

        var nozzleInput = new NozzleInput(shellCalculated)
        {
            steel1 = "12Х18Н10Т",
            SigmaAllow1 = 0,
            E1 = 0,
            E2 = 0,
            E3 = 0,
            E4 = 0,
            d = 77,
            s1 = 6,
            s2 = 0,
            s3 = 6,
            cs = 2.9,
            cs1 = 2.0,
            l = 0,
            l1 = 130,
            l2 = 0,
            l3 = 5,
            NozzleKind = NozzleKind.PassWithoutRing,
            phi = 1,
            phi1 = 1,
            delta = 6,
            delta1 = 6,
            delta2 = 6,
            steel2 = "",
            steel3 = "12Х18Н10Т",
            steel4 = "",
            Location = NozzleLocation.LocationAccordingToParagraph_5_2_2_1,
            omega = 0,
            tTransversely = 0,
            ellx = 0,
            gamma = 0,
            Name = "Тестовый штуцер",
            IsOval = false,
            d1 = 0,
            d2 = 0,
            r = 0,
            s0 = 0,
            SigmaAllow2 = 0,
            SigmaAllow3 = 0,
            SigmaAllow4 = 0
        };

        var nozzleCommon = new NozzleCalculatedCommon
        {
            alpha1 = 0,
            b = 111.714,
            c = 2.8,
            ConditionUseFormulas1 = 0.128,
            ConditionUseFormulas2 = 0.009,
            ConditionUseFormulas2_2 = 0,
            d0p = 22.343,
            Dk = 0,
            dmax = 600,
            dp = 82.8,
            Dp = 600,
            EllipseH = 0,
            IsConditionUseFormulas = true,
            K1 = 1,
            L0 = 55.857,
            l1p = 20.027,
            l1p2 = 20.027,
            l2p = 0,
            l2p2 = 55.857,
            l3p = 4.772,
            l3p2 = 4.772,
            lp = 55.857,
        };

        var nozzleResult1 = new NozzleCalculatedOneLoading
        {
            LoadingCondition = loadingCondition1,

            ConditionStrengthening1 = 0,
            ConditionStrengthening2 = 0,

            d0 = 452.02,// 451.85,
            d01 = 452.02,
            d02 = 605.8,
            E1 = 199000,
            E2 = 0,
            E3 = 199000,
            E4 = 0,
            p_d = 2.887,
            p_de = 0,
            p_deShell = 0,
            p_dp = 0,
            pen = 0,
            ppn = 0,
            psi1 = 1,
            psi2 = 0,
            psi3 = 1,
            psi4 = 1,
            SigmaAllowShell = 168,
            SigmaAllow1 = 168,
            SigmaAllow2 = 0,
            SigmaAllow3 = 168,
            SigmaAllow4 = 0,
            s1p = 0.148,
            sp = 1.073,
            spn = 1.073,
            V = 1,
            V1 = 1,
            V2 = 4.569,
        };

        var nozzleCalculated = new NozzleCalculated
        {
            InputData = nozzleInput,
            CommonData = nozzleCommon,
            Results = new List<NozzleCalculatedOneLoading> { nozzleResult1 }
        };

        return (nozzleInput, nozzleCalculated);
    }
}

#endregion