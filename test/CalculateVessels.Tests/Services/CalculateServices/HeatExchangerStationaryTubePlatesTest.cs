using CalculateVessels.Core.Enums;
using CalculateVessels.Core.HeatExchanger.Enums;
using CalculateVessels.Core.HeatExchangers.HeatExchangerStationaryTubePlates;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.PhysicalData;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace CalculateVessels.UnitTests.Services.CalculateServices;

public class HeatExchangerStationaryTubePlatesTest
{
    private readonly ICalculateService<HeatExchangerStationaryTubePlatesInput> _calculateService;

    public HeatExchangerStationaryTubePlatesTest()
    {
        _calculateService = new HeatExchangerStationaryTubePlatesCalculateService(new PhysicalDataService());
    }

    [Theory]
    [MemberData(nameof(HeatExchangerStationaryTubePlatesTestData.GetData), MemberType = typeof(HeatExchangerStationaryTubePlatesTestData))]
    public void HeatExchangerStationaryTubePlates(HeatExchangerStationaryTubePlatesInput inputData, HeatExchangerStationaryTubePlatesCalculated calculatedData)
    {
        //Act
        var result = _calculateService.Calculate(inputData) as HeatExchangerStationaryTubePlatesCalculated;

        //Assert
        const double precision = 0.001;
        result.Should().BeEquivalentTo(calculatedData, options => options
            .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, precision))
            .WhenTypeIs<double>());
    }
}

#region TestData

public class HeatExchangerStationaryTubePlatesTestData
{
    public static IEnumerable<object[]> GetData()
    {
        var (inputData1, calculatedData1) = GetData1();
        yield return new object[] { inputData1, calculatedData1 };
    }

    private static (HeatExchangerStationaryTubePlatesInput inputData, HeatExchangerStationaryTubePlatesCalculated calculatedData) GetData1()
    {
        var firstTubePlate = new ConnectionTubePlate
        {

            IsNeedCheckHardnessTubePlate = false,
            TubePlateType = TubePlateType.WeldedInFlange,
            FixTubeInTubePlate = FixTubeInTubePlateType.RollingWithWelding,

            TubeRolling = TubeRollingType.RollingWithoutGroove,

            //Tube plate
            Steelp = "Ст3",

            BP = 10,

            c = 2,


            fiP = 1,

            //pp { get; set; }

            s1p = 40,

            sn = 40,
            sp = 45,
            IsWithGroove = true,


            //shell for tube plate
            s1 = 10,


            //flange for tube plate
            Steel1 = "Ст3", //+
            h1 = 34,
            DH = 740,


            //shell for chamber
            SteelD = "Ст3",
            s2 = 8, //+

            //flange for chamber
            Steel2 = "Ст3", //+
            h2 = 32, //+
            IsChamberFlangeSkirt = false,
            FlangeFace = FlangeFaceType.Flat,

            //tube
            lB = 40,
            delta = 2
        };

        var inputData = new HeatExchangerStationaryTubePlatesInput
        {
            Name = "Name",
            IsOneGo = true,
            IsWorkCondition = true,
            t0 = 20,
            N = 1000,
            // a =
            a1 = 279,

            //tube
            IsNeedCheckHardnessTube = false,
            dT = 25,//+
            l = 3000,//+ UNDONE: l - half long tube
            pT = 0.6,//+
            sT = 2,//+
            tT = 75,//+
            TT = 150,//+
            i = 248,//+
            SteelT = "Ст3",//+

            //shell
            cK = 2,//+
            D = 600,//+
            pM = 2.5,//+
            sK = 8,//+
            tK = 25,//+
            TK = 150,//+
            SteelK = "Ст3",//+

            //compensator
            IsNeedCompensatorCalculate = false,
            CompensatorType = default,//+
                                      // beta0 =
                                      // deltakom =
                                      // Dkom =
                                      // dkom =
                                      // Kkom =
                                      // rkom =
                                      //public int nkom =
                                      //public string Steelkom =

            //extender
            //D1 =
            //deltap =
            //Lpac =

            //tube plate
            IsDifferentTubePlate = false,//+



            d0 = 25.35,//+
            DE = 70,//+

            tp = 32,//+
            tP = 55,//+ distance over hole both side from hole

            FirstTubePlate = firstTubePlate,
            //SecondTubePlate = = new();


            //partitions
            IsWithPartitions = true,//+
            l1R = 700,//+
            l2R = 592,//+

            //public int Bper =
            //public int Lper =
            cper = 2,
            sper = 8
        };

        var calculatedData = new HeatExchangerStationaryTubePlatesCalculated
        {
            InputData = inputData,

            ET = 186000.0,
            EK = 186000.0,
            ED = 186000.0,
            Ep = 186000.0,
            E1 = 186000.0,
            E2 = 186000.0,
            sigma_dp = 145.0,
            sigma_dK = 145.0,
            sigma_dT = 145.0,
            Rmp = 460.0,
            nNForSigmaa = 10.0,
            nsigmaForSigmaa = 2.0,
            CtForSigmaa = 0.9347826086956522,
            AForSigmaap = 60000.0,
            BForSigmaa = 184.0,
            sigmaa_dp = 652.8695652173914,
            AForSigmaaK = 60000.0,
            sigmaa_dK = 652.8695652173914,
            AForSigmaaT = 60000.0,
            sigmaa_dT = 652.8695652173914,
            Ksigma = 1.7,
            a = 300.0,
            mn = 1.075268817204301,
            etaM = 0.5021903624054161,
            etaT = 0.6487455197132617,
            Ky = 9.086419753086423,
            ro = 5.1111111111111125,
            Kq = 1.0,
            Kp = 1.0,
            psi0 = 0.3643400109605112,
            beta = 0.011272110790902762,
            omega = 3.1449189106618705,
            b1 = 70.0,
            fip = 0.20781249999999996,
            R1 = 35.0,
            b2 = 70.0,
            R2 = 35.0,
            beta1 = 0.023734644158557198,
            beta2 = 0.0265361388801511,
            K1 = 6879964.384662295,
            K2 = 3938321.420001895,
            KF1 = 44468068.28076619,
            KF2 = 34633532.64078111,
            KF = 79101600.9215473,
            mcp = 0.2528076463560334,
            p0 = -21.289800490743954,
            ro1 = 2.1619937717916575,
            t = 1.3314000572525408,
            T1 = 6.651842197849333,
            T2 = 3.91431616832247,
            T3 = 5.0,
            Phi1 = 4.5,
            Phi2 = 2.94,
            Phi3 = 4.65,
            m1 = 1603.8265396961049,
            m2 = 1313.0104928981075,
            p1 = 0.03283185839571015,
            MP = 30065.048902714567,
            QP = -617.7362469045031,
            Ma = 17092.587717720002,
            Qa = -664.2325235532292,
            NT = -7708.007967411455,
            JT = 9628.19608508932,
            lpr = 233.33333333333334,
            MT = -36069.12956317753,
            QK = 707.7362469045031,
            MK = -4801.189597342522,
            F = 1334051.3963726393,
            sigmap1 = 97.56100238847344,
            taup1 = 14.365959230337282,
            mA = -0.2900633974179733,
            pfip = 0.20781249999999996,
            sigmap2 = 294.44727435326945,
            Mmax = 18856.66415810754,
            taup2 = 74.33271814549165,
            sigmaMX = 88.46703086306289,
            sigmaIX = 450.1115247508614,
            sigmaMfi = 93.75,
            sigmaIfi = 135.0334574252584,
            sigma1T = 53.337720408923744,
            sigma1 = 100.16519624709748,
            sigma2T = 14.375,
            ConditionStaticStressForTubePlate1 = 74.33271814549165,
            ConditionStaticStressForTubePlate2 = 116.0,
            IsConditionStaticStressForTubePlate = true,
            deltasigma1pk = 97.56100238847344,
            sigmaa_5_2_4k = 82.92685203020243,
            deltasigma1pp = 294.44727435326945,
            sigmaa_5_2_4p = 147.22363717663472,
            ConditionStaticStressForTube1 = 53.337720408923744,
            IsConditionStaticStressForTube = true,
            deltasigma1T = 100.16519624709748,
            sigmaaT = 50.08259812354874,
            KT = 1.3,
            lR = 592.0,
            lambda = 0.9342535397879407,
            phiT = 0.7533864361093159,
            ConditionStabilityForTube2 = 109.24103323585081,
            IsConditionStabilityForTube = true,
            Ntp_d = 16763.538399555135,
            phiC2 = -0.4315510557964275,
            phiC = -0.4315510557964275,
            tau = 85.81041645942085,
            IsConditionStressBracingTube = true,
            ConditionStressBracingTube11 = 0.5756699682625417,
            ConditionStressBracingTube12 = 2.1748211042891223,
            pp = 2.5,
            spp_5_5_1 = 4.595725150090289,
            sp_5_5_1 = 6.595725150090289,
            A = 0.32,
        };

        return (inputData, calculatedData);
    }
}

#endregion