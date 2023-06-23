using CalculateVessels.Core.Elements.Bottoms.Enums;
using CalculateVessels.Core.Elements.Bottoms.FlatBottomWithAdditionalMoment;
using CalculateVessels.Core.Enums;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.PhysicalData;
using CalculateVessels.Data.PhysicalData.Gost34233_4.Models;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace CalculateVessels.UnitTests.Services.CalculateServices;

public class FlatBottomTestWithAdditionalMoment
{
    private readonly ICalculateService<FlatBottomWithAdditionalMomentInput> _calculateService;

    public FlatBottomTestWithAdditionalMoment()
    {
        _calculateService = new FlatBottomWithAdditionalMomentCalculateService(new PhysicalDataService());
    }

    [Theory]
    [MemberData(nameof(FlatBottomWithAdditionalMomentTestData.GetData), MemberType = typeof(FlatBottomWithAdditionalMomentTestData))]
    public void FlatBottom(FlatBottomWithAdditionalMomentInput inputData, FlatBottomWithAdditionalMomentCalculated calculatedData)
    {
        //Act
        var result = _calculateService.Calculate(inputData) as FlatBottomWithAdditionalMomentCalculated;

        //Assert
        var precision = 0.001;
        result.Should().BeEquivalentTo(calculatedData, options => options
            .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, precision))
            .WhenTypeIs<double>());
    }
}

#region testData

public class FlatBottomWithAdditionalMomentTestData
{
    public static IEnumerable<object[]> GetData()
    {
        var inputData1 = new FlatBottomWithAdditionalMomentInput
        {
            Name = "Тестовая плоское днище",

            c1 = 2,
            c2 = 0.8,
            c3 = 0,
            D = 600,
            s = 40,

            //cover
            CoverSteel = "20",
            D2 = 633,
            D3 = 700,
            IsCoverFlat = true,
            IsCoverWithGroove = false,
            s1 = 36,
            s2 = 31,
            s3 = 25,
            s4 = 0,

            //stress condition
            F = 0,
            fi = 1,
            IsPressureIn = true,
            M = 0,
            p = 0.6,
            t = 150,
            SigmaAllow = 0,

            //screw
            IsStud = true,
            IsScrewWithGroove = false,
            Lb0 = 63,
            n = 28,
            ScrewSteel = "35",
            Screwd = 10,

            //gasket
            bp = 13,
            Dcp = 620,
            GasketType = "Резина по ГОСТ 7338 с твердостью по Шору А до 65 единиц",
            hp = 2,

            //flange
            Db = 700,
            Dn = 740,
            FlangeFace = FlangeFaceType.Flat,
            FlangeSteel = "Ст3",
            h = 32,
            IsFlangeIsolated = false,
            IsFlangeFlat = true,
            l = 0,
            S0 = 0,
            S1 = 0,

            //washer
            IsWasher = false,
            hsh = 0,
            WasherSteel = string.Empty,

            //hole
            di = 0,
            d = 0,
            E = 0,
            Hole = HoleInFlatBottom.WithoutHole,

        };

        var calculatedData1 = new FlatBottomWithAdditionalMomentCalculated
        {
            InputData = inputData1,
            IsConditionUseFormulas = true,
            Ab = 1461.6,
            alpha = 0,
            alpha_m = 0,
            alpha_b = 0,
            alpha_f = 0,
            alpha_kr = 0,
            alpha_sh1 = 0,
            alpha_sh2 = 0,
            b = 40,
            b0 = 13,
            beta = 0,
            betaF = 0.91,
            betaT = 1.867,
            betaU = 19.645,
            betaV = 0.55,
            betaY = 17.964,
            betaZ = 4.838,
            c = 2.8,
            ConditionUseFormulas = 0.054,
            Dcp = 620,
            delta_kr = 31,
            Dp = 620,
            e = 0,
            E = 186600,
            E20 = 199000,
            Eb = 204900,
            Eb20 = 213000,
            Ekr = 186000,
            Ekr20 = 199000,
            Ep = 0,
            f = 1,
            fb = 52.2,
            Gasket = new Gasket
            {
                Material = inputData1.GasketType,
                m = 0.5,
                qobj = 2,
                q_d = 18,
                Kobj = 0.04,
                Ep = -1,
                IsFlat = true,
                IsMetal = false
            },
            gamma = 40.355,
            hkr = 31,
            K0 = 1,
            K6 = 0.479,
            K7_s2 = 0.287,
            K7_s3 = 0.26,
            KGost34233_4 = 1.233,
            Kkr = 1.194,
            Kp = 1,
            l0 = 154.919,
            lambda = 29.659,
            Lb = 68.6,
            p_d = 1.737,
            Pb1 = 7598.308,
            Pb1_1 = 7598.308,
            Pb1_2 = 7597.947,
            Pb2 = 71910.72,
            Pb2_2 = 71910.72,
            Pbm = 71910.72,
            Pbp = 252961.544,
            Phi = 1819.867,
            Phi_1 = 1819.867,
            Phi_2 = 379.385,
            Pobj = 25321.237,
            psi1 = 1.397,
            Qd = 181052.4,
            QFM = 0,
            Qt = 0.361,
            Rp = 7596.371,
            S0 = 40,
            s1 = 22.311,
            s1p = 19.511,
            s2 = 15.059,
            s2p = 12.259,
            s2p_1 = 12.259,
            s2p_2 = 1.761,
            s3 = 13.903,
            s3p = 11.103,
            s3p_1 = 11.103,
            s3p_2 = 1.761,
            Se = 20,
            SigmaAllow = 139,
            sigma_d_krm = 189.545,
            sigma_dnb = 123,
            tb = 142.5,
            tf = 144,
            tkr = 150,
            x = 0,
            Xkr = 0.082,
            yb = 0,
            yF = 0,
            yfn = 0,
            ykr = 0,
            yp = 0.025,
            zeta = 1,
            dkr = 10,
        };

        yield return new object[] { inputData1, calculatedData1 };
    }
}

#endregion

