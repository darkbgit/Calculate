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
        var precision = 0.001;
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

        var shellInput1 = new CylindricalShellInput
        {
            Name = "Тестовая цилиндрическая обечайка",
            Steel = "12Х18Н10Т",
            c1 = 2.0,
            D = 600,
            c2 = 0.8,
            c3 = 0,
            p = 0.6,
            t = 150,
            s = 8,
            phi = 1,
            ny = 2.4,
            IsPressureIn = true
        };

        var shellCalculated1 = new CylindricalShellCalculated()
        {
            InputData = shellInput1,
            c = 2.8,
            s_p = 1.073,
            s = 3.873,
            p_de = 0,
            p_d = 2.887,
            SigmaAllow = 168,
            IsConditionUseFormulas = true,
            b = 0,
            b_2 = 0,
            B1 = 0,
            B1_2 = 0,
            ConditionStability = 0.208,
            F = 0,
            FAllow = 0,
            F_de = 0,
            F_de1 = 0,
            F_de2 = 0,
            F_dp = 0,
            l = 0,
            lambda = 0,
            lpr = 0,
            M_d = 0,
            M_de = 0,
            M_dp = 0,
            Q_d = 0,
            Q_de = 0,
            Q_dp = 0,
            s_f = 0,
            s_pf = 0,
            s_p_1 = 0,
            s_p_2 = 0,
            p_dp = 0,
            E = 199000
        };

        var nozzleInput1 = new NozzleInput(shellCalculated1)
        {
            t = 150,
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
            fi = 1,
            fi1 = 1,
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

        var nozzleCalculated1 = new NozzleCalculated
        {
            InputData = nozzleInput1,
            alpha1 = 0,
            b = 111.714,
            c = 2.8,
            ConditionStrengthening1 = 0,
            ConditionStrengthening2 = 0,
            ConditionUseFormulas1 = 0.128,
            ConditionUseFormulas2 = 0.009,
            ConditionUseFormulas2_2 = 0,
            d0 = 452.02,// 451.85,
            d01 = 452.02,
            d02 = 605.8,
            d0p = 22.343,
            Dk = 0,
            dmax = 600,
            dp = 82.8,
            Dp = 600,
            E1 = 199000,
            E2 = 0,
            E3 = 199000,
            E4 = 0,
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

        yield return new object[] { nozzleInput1, nozzleCalculated1 };
    }
}

#endregion