using CalculateVessels.Core.Elements.Base;
using CalculateVessels.Core.Elements.HeatExchangers.Enums;
using CalculateVessels.Core.Exceptions;
using CalculateVessels.Core.Helpers;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Enums;
using CalculateVessels.Data.Interfaces;
using System;

namespace CalculateVessels.Core.Elements.HeatExchangers.HeatExchangerStationaryTubePlates;

internal class HeatExchangerStationaryTubePlatesCalculateService : CalculateService, ICalculateService<HeatExchangerStationaryTubePlatesInput>
{
    public HeatExchangerStationaryTubePlatesCalculateService(IPhysicalDataService physicalData)
        : base(physicalData)
    {
        Name = "GOST 34233.7-2017";
    }

    public string Name { get; }

    public ICalculatedElement Calculate(HeatExchangerStationaryTubePlatesInput dataIn)
    {

        var data = new HeatExchangerStationaryTubePlatesCalculated
        {
            InputData = dataIn,
            //TODO: Get physical parameters
            ET = PhysicalHelper.GetE(dataIn.SteelT, dataIn.TT, PhysicalData),
            EK = PhysicalHelper.GetE(dataIn.SteelK, dataIn.TK, PhysicalData),
            ED = PhysicalHelper.GetE(dataIn.FirstTubePlate.SteelD, dataIn.TT, PhysicalData),
            ED2 = dataIn.IsDifferentTubePlate
                ? PhysicalHelper.GetE(dataIn.SecondTubePlate.SteelD, dataIn.TT, PhysicalData)
                : default,
            Ep = PhysicalHelper.GetE(dataIn.FirstTubePlate.Steelp, dataIn.TK, PhysicalData),
            Ep2 = dataIn.IsDifferentTubePlate
                ? PhysicalHelper.GetE(dataIn.SecondTubePlate.Steelp, dataIn.TK, PhysicalData)
                : default,

            //TODO: Get E1 for condition if two different tube plate and flange not with tube plate
            E1 = true
                ? PhysicalHelper.GetE(dataIn.FirstTubePlate.Steelp, dataIn.TK, PhysicalData)
                : PhysicalHelper.GetE(dataIn.FirstTubePlate.Steel1, dataIn.TK, PhysicalData),

            E12 = dataIn.IsDifferentTubePlate
                ? true
                    ? PhysicalHelper.GetE(dataIn.SecondTubePlate.Steelp, dataIn.TK, PhysicalData)
                    : PhysicalHelper.GetE(dataIn.SecondTubePlate.Steel1, dataIn.TK, PhysicalData)
                : default,

            E2 = PhysicalHelper.GetE(dataIn.FirstTubePlate.Steel2, dataIn.TK, PhysicalData),
            E22 = dataIn.IsDifferentTubePlate
                ? PhysicalHelper.GetE(dataIn.SecondTubePlate.Steel2, dataIn.TK, PhysicalData)
                : default,

            Ekom = dataIn.IsNeedCompensatorCalculate
                ? PhysicalHelper.GetE(dataIn.Steelkom, dataIn.TK, PhysicalData)
                : default,

            sigma_dp = PhysicalHelper.GetSigma(dataIn.FirstTubePlate.Steelp, dataIn.TK, PhysicalData),
            sigma_dp2 = dataIn.IsDifferentTubePlate
                ? PhysicalHelper.GetSigma(dataIn.SecondTubePlate.Steelp, dataIn.TK, PhysicalData)
                : default,
            sigma_dK = PhysicalHelper.GetSigma(dataIn.SteelK, dataIn.TK, PhysicalData),
            sigma_dT = PhysicalHelper.GetSigma(dataIn.SteelT, dataIn.TT, PhysicalData),
            alfaK = PhysicalHelper.GetAlpha(dataIn.SteelK, dataIn.TK, PhysicalData),
            alfaT = PhysicalHelper.GetAlpha(dataIn.SteelT, dataIn.TT, PhysicalData),
            Rmp = PhysicalHelper.GetRm(dataIn.FirstTubePlate.Steelp, dataIn.TK, PhysicalData),
            Rmp2 = dataIn.IsDifferentTubePlate
                ? PhysicalHelper.GetRm(dataIn.SecondTubePlate.Steelp, dataIn.TK, PhysicalData)
                : default,
            //TODO: sigmaa for different materials Al,Cu, Tt

            nNForSigmaa = 10.0,
            nsigmaForSigmaa = 2.0,

            CtForSigmaa = (2300.0 - dataIn.TK) / 2300.0,

            AForSigmaap = PhysicalHelper.GetSteelType(dataIn.FirstTubePlate.Steelp, PhysicalData) is SteelType.Carbon or SteelType.Austenitic ? 60000.0 : 45000.0
        };

        data.BForSigmaa = 0.4 * data.Rmp;

        data.sigmaa_dp = data.CtForSigmaa * data.AForSigmaap / Math.Sqrt(data.nNForSigmaa * dataIn.N) + data.BForSigmaa / data.nsigmaForSigmaa;

        if (dataIn.IsDifferentTubePlate)
        {
            data.AForSigmaap = PhysicalHelper.GetSteelType(dataIn.SecondTubePlate.Steelp, PhysicalData) is SteelType.Carbon or SteelType.Austenitic ? 60000.0 : 45000.0;
            data.sigmaa_dp2 = data.CtForSigmaa * data.AForSigmaap2 / Math.Sqrt(data.nNForSigmaa * dataIn.N) + data.BForSigmaa / data.nsigmaForSigmaa;
        }

        data.AForSigmaaK = PhysicalHelper.GetSteelType(dataIn.SteelK, PhysicalData) is SteelType.Carbon or SteelType.Austenitic ? 60000.0 : 45000.0;
        data.sigmaa_dK = data.CtForSigmaa * data.AForSigmaaK / Math.Sqrt(data.nNForSigmaa * dataIn.N) + data.BForSigmaa / data.nsigmaForSigmaa;


        data.AForSigmaaT = PhysicalHelper.GetSteelType(dataIn.SteelT, PhysicalData) is SteelType.Carbon or SteelType.Austenitic ? 60000.0 : 45000.0;
        data.sigmaa_dT = data.CtForSigmaa * data.AForSigmaaT / Math.Sqrt(data.nNForSigmaa * dataIn.N) + data.BForSigmaa / data.nsigmaForSigmaa;

        data.Ksigma = dataIn.FirstTubePlate.TubePlateType switch
        {
            TubePlateType.WeldedInShell => 1.7,
            TubePlateType.SimplyFlange => 1.7,
            TubePlateType.FlangeWithFlanging => 1.2,
            TubePlateType.SimplyFlangeWithShell => 1.7,
            TubePlateType.WeldedInFlange => 1.7,
            TubePlateType.BetweenFlange => 1.7,
            _ => throw new CalculateException("Undefined tube plate type.")
        };

        data.a = dataIn.D / 2.0;

        data.mn = data.a / dataIn.a1;
        data.etaM = 1 - dataIn.i * Math.Pow(dataIn.dT, 2) / (4 * Math.Pow(dataIn.a1, 2));
        data.etaT = 1 - dataIn.i * Math.Pow(dataIn.dT - 2 * dataIn.sT, 2) / (4 * Math.Pow(dataIn.a1, 2));

        data.Ky = data.ET * (data.etaT - data.etaM) / dataIn.l;
        data.ro = data.Ky * dataIn.a1 * dataIn.l / (data.EK * dataIn.sK);

        switch (dataIn.CompensatorType)
        {
            case CompensatorType.No:
                data.Kqz = 0.0;
                data.Kpz = 0.0;
                break;
            case CompensatorType.Compensator:
                if (dataIn.IsNeedCompensatorCalculate)
                {
                    data.betakom = dataIn.dkom / dataIn.Dkom;
                    data.Xkom = 4 * dataIn.rkom * data.betakom / (dataIn.dkom * (1 - data.betakom));
                    data.Ykom = 2.57 * dataIn.rkom /
                                Math.Sqrt(dataIn.dkom * dataIn.deltakom * (1 + 1 / data.betakom));
                    data.Cf = 1.0; //TODO: Get Cf value from chart
                    data.Akom = 6.8 * data.betakom * (1 + data.betakom) / (data.Cf * Math.Pow(1 - data.betakom, 3));
                    data.Kkom = data.Ekom * Math.Pow(dataIn.deltakom, 3) * data.Akom /
                                (dataIn.nkom * Math.Pow(dataIn.dkom, 2));
                }
                else
                {
                    data.Kkom = dataIn.Kkom;
                }

                data.Kqz = Math.PI * data.a * data.EK * dataIn.sK / (dataIn.l * data.Kkom);
                data.Kpz = Math.PI * (Math.Pow(dataIn.Dkom, 2) - Math.Pow(dataIn.dkom, 2)) * data.EK * dataIn.sK /
                           (4.8 * dataIn.l * data.a * data.Kkom);
                break;
            case CompensatorType.Expander:
                data.betap = dataIn.D / dataIn.D1;

                if (Math.Abs(dataIn.beta0 - 90) < 0.000001)
                {
                    data.Ap = data.betap <= 0.9
                        ? 9.2 * (Math.Pow(data.betap, 2) * (1 - Math.Pow(data.betap, 2))) /
                          (Math.Pow(1 - Math.Pow(data.betap, 2), 2) - 4 * Math.Pow(data.betap, 2) * Math.Log(data.betap))
                        : 13.8 / Math.Pow(1 - data.betap, 3) *
                          (1 - 5.0 / 2.0 * (1 - data.betap) + 61.0 / 30.0 * Math.Pow(1 - data.betap, 2) -
                           11.0 / 20.0 * Math.Pow(1 - data.betap, 3));
                    data.Kpac = data.EK * Math.Pow(dataIn.deltap, 3) * data.Ap / Math.Pow(dataIn.D, 2);
                    data.Kqz = data.a * dataIn.sK / dataIn.l *
                               (Math.PI * data.EK / data.Kpac + dataIn.Lpac / (dataIn.deltap * dataIn.D1));
                    data.Kpz = data.a * dataIn.sK / (dataIn.l * Math.Pow(data.betap, 2)) *
                               ((1 - Math.Pow(data.betap, 2)) / 4.8 * (Math.PI * data.EK / data.Kpac + dataIn.Lpac / (dataIn.deltap * dataIn.D1)) - 0.5 * Math.PI * dataIn.Lpac / (dataIn.deltap * dataIn.D1));
                }
                else if (dataIn.beta0 is >= 15 and <= 60)
                {
                    data.Ap1 = 2 /
                               (Math.Sin(MathHelper.DegreeToRadian(dataIn.beta0)) * Math.Pow(Math.Cos(MathHelper.DegreeToRadian(dataIn.beta0)), 2)) *
                               Math.Log(1.0 / data.betap);
                    data.Ap2 = 1.82 * Math.Pow(Math.Sin(MathHelper.DegreeToRadian(dataIn.beta0)), 2) *
                        (1 + Math.Sqrt(data.betap)) / Math.Pow(Math.Cos(MathHelper.DegreeToRadian(dataIn.beta0)), 3.0 / 2.0);
                    data.Bp1 = -1.06 /
                               (Math.Sin(MathHelper.DegreeToRadian(dataIn.beta0)) * Math.Pow(Math.Cos(MathHelper.DegreeToRadian(dataIn.beta0)), 2)) *
                               (Math.Log(1 / data.betap) + (1.0 / Math.Pow(data.betap, 2) - 1) *
                                   (0.3 * Math.Pow(Math.Cos(MathHelper.DegreeToRadian(dataIn.beta0)), 4) +
                                    1.5 * Math.Pow(Math.Sin(MathHelper.DegreeToRadian(dataIn.beta0)), 2) -
                                    0.5 * Math.Pow(Math.Cos(MathHelper.DegreeToRadian(dataIn.beta0)), 2) +
                                    Math.Pow(Math.Sin(MathHelper.DegreeToRadian(dataIn.beta0)), 4)));
                    data.Bp2 = 0.965 * Math.Pow(Math.Sin(MathHelper.DegreeToRadian(dataIn.beta0)), 2) /
                               Math.Pow(Math.Cos(MathHelper.DegreeToRadian(dataIn.beta0)), 3.0 / 2.0 *
                                                                          (1.0 / Math.Pow(data.betap, 2) - 1));
                    data.Kqz = (data.a * (data.Ap1 + data.Ap2 * Math.Sqrt(dataIn.D1 / dataIn.sK)) -
                                0.5 * (1 - data.betap) * dataIn.Lpac) / dataIn.l;
                    data.Kpz = (data.Bp1 + data.Bp2 * Math.Sqrt(dataIn.D1 / dataIn.sK)) * data.a / dataIn.l;
                }
                break;
            case CompensatorType.CompensatorOnExpander:
                //TODO: Make calculation compensator on expander 
                break;
        }

        data.Kq = 1 + data.Kqz;
        data.Kp = 1 + data.Kpz;

        data.psi0 = Math.Pow(data.etaT, 7.0 / 3.0);

        data.beta = dataIn.IsDifferentTubePlate
            ? 1.53 * Math.Pow(data.Ky / data.psi0 * (1 / (data.Ep * Math.Pow(dataIn.FirstTubePlate.sp, 3)) +
                                                     1 / (data.Ep2 * Math.Pow(dataIn.SecondTubePlate.sp, 3))), 1.0 / 4.0)
            : 1.82 / dataIn.FirstTubePlate.sp * Math.Pow(data.Ky * dataIn.FirstTubePlate.sp / (data.psi0 * data.Ep), 1.0 / 4.0);

        data.omega = data.beta * dataIn.a1;

        data.fip = 1 - dataIn.d0 / dataIn.tp;

        data.b1 = (dataIn.FirstTubePlate.DH - dataIn.D) / 2.0;
        data.R1 = (dataIn.FirstTubePlate.DH - dataIn.D) / 4.0;
        data.b2 = (dataIn.FirstTubePlate.DH - dataIn.D) / 2.0;
        data.R2 = (dataIn.FirstTubePlate.DH - dataIn.D) / 4.0;

        data.beta1 = 1.3 / Math.Sqrt(data.a * dataIn.FirstTubePlate.s1);
        data.beta2 = 1.3 / Math.Sqrt(data.a * dataIn.FirstTubePlate.s2);
        data.K1 = data.beta1 * data.a * data.EK * Math.Pow(dataIn.FirstTubePlate.s1, 3) / (5.5 * data.R1);
        data.K2 = data.beta2 * data.a * data.ED * Math.Pow(dataIn.FirstTubePlate.s2, 3) / (5.5 * data.R2);
        data.KF1 = data.E1 * Math.Pow(dataIn.FirstTubePlate.h1, 3) * data.b1 / (12.0 * Math.Pow(data.R1, 2)) +
                   data.K1 * (1.0 + data.beta1 * dataIn.FirstTubePlate.h1 / 2.0);
        data.KF2 = data.E2 * Math.Pow(dataIn.FirstTubePlate.h2, 3) * data.b2 / (12.0 * Math.Pow(data.R2, 2)) +
                   data.K2 * (1.0 + data.beta2 * dataIn.FirstTubePlate.h2 / 2.0);
        data.KF = data.KF1 + data.KF2;

        data.mcp = 0.15 * dataIn.i * Math.Pow(dataIn.dT - dataIn.sT, 2) / Math.Pow(dataIn.a1, 2);

        data.p0 = (data.alfaK * (dataIn.tK - dataIn.t0) - data.alfaT * (dataIn.tT - dataIn.t0)) * data.Ky * dataIn.l +
                  (data.etaT - 1.0 + data.mcp + data.mn * (data.mn + 0.5 * data.ro * data.Kq)) * dataIn.pT -
                  (data.etaM - 1.0 + data.mcp + data.mn * (data.mn + 0.3 * data.ro * data.Kp)) * dataIn.pM;

        data.ro1 = data.Ky * data.a * dataIn.a1 / (Math.Pow(data.beta, 2) * data.KF * data.R1);

        (data.Phi1, data.Phi2, data.Phi3) = PhysicalHelper.GetPhi1Phi2Phi3(data.omega, PhysicalData);

        //TODO: Calculate values of data.Phi1, data.Phi2, data.Phi3

        data.t = 1 + 1.4 * data.omega * (data.mn - 1);
        data.T1 = data.Phi1 * (data.mn + 0.5 * (1 + data.mn * data.t) * (data.t - 1));
        data.T2 = data.Phi2 * data.t;
        data.T3 = data.Phi3 * data.mn;

        //if (!Physical.Gost34233_7.TryGetT1T2T3 (data.omega, data.mn, ref data.T1, ref data.T2, ref data.T3, ref data.ErrorList))
        //{
        //    IsCriticalError = true;
        //    return;
        //}

        data.m1 = (1 + data.beta1 * dataIn.FirstTubePlate.h1) / (2 * Math.Pow(data.beta1, 2));
        data.m2 = (1 + data.beta2 * dataIn.FirstTubePlate.h2) / (2 * Math.Pow(data.beta2, 2));
        data.p1 = data.Ky / (data.beta * data.KF) * (data.m1 * dataIn.pM - data.m2 * dataIn.pT);

        data.MP = dataIn.a1 / data.beta * (data.p1 * (data.T1 + data.ro * data.Kq) - data.p0 * data.T2) /
                  ((data.T1 + data.ro * data.Kq) * (data.T3 + data.ro1) - Math.Pow(data.T2, 2));

        data.QP = dataIn.a1 * (data.p0 * (data.T3 + data.ro1) - data.p1 * data.T2) /
                  ((data.T1 + data.ro * data.Kq) * (data.T3 + data.ro1) - Math.Pow(data.T2, 2));

        data.Ma = data.MP + (data.a - dataIn.a1) * data.QP;
        data.Qa = data.mn * data.QP;

        data.NT = Math.PI * dataIn.a1 / dataIn.i * ((data.etaM * dataIn.pM - data.etaT * dataIn.pT) * dataIn.a1 +
                                                    data.Phi1 * data.Qa + data.Phi2 * data.beta * data.Ma);
        data.JT = Math.PI * (Math.Pow(dataIn.dT, 4) - Math.Pow(dataIn.dT - 2 * dataIn.sT, 4)) / 64;
        data.lpr = dataIn.IsWithPartitions ? dataIn.l1R / 3.0 : dataIn.l;
        data.MT = data.ET * data.JT * data.beta / (data.Ky * dataIn.a1 * data.lpr) *
                  (data.Phi2 * data.Qa + data.Phi3 * data.beta * data.Ma);

        data.QK = data.a / 2.0 * dataIn.pT - data.QP;
        data.MK = data.K1 / (data.ro1 * data.KF * data.beta) * (data.T2 * data.QP + data.T3 * data.beta * data.MP) -
                  dataIn.pM / (2 * Math.Pow(data.beta1, 2));

        data.F = Math.PI * dataIn.D * data.QK;

        dataIn.FirstTubePlate.s1p = dataIn.FirstTubePlate.sp; //UNDONE: сделана просто заглушка s1p зависит от типа трубной решетки

        data.sigmap1 = 6 * Math.Abs(data.MP) / Math.Pow(dataIn.FirstTubePlate.s1p - dataIn.FirstTubePlate.c, 2);
        data.taup1 = Math.Abs(data.QP) / (dataIn.FirstTubePlate.s1p - dataIn.FirstTubePlate.c);

        data.mA = data.beta * data.Ma / data.Qa;

        if (data.mA is >= -1.0 and <= 1.0)
        {
            data.A = PhysicalHelper.GetA(data.omega, data.mA, PhysicalData);
            data.Mmax = data.A * Math.Abs(data.Qa) / data.beta;
        }
        else
        {
            data.nB = 1.0 / data.mA;
            data.B = PhysicalHelper.GetB(data.omega, data.nB, PhysicalData);
            data.Mmax = data.B * Math.Abs(data.Ma);
        }

        data.pfip = 1 - dataIn.d0 / dataIn.tp;

        data.sigmap2 = 6 * data.Mmax / (data.pfip * Math.Pow(dataIn.FirstTubePlate.sp - dataIn.FirstTubePlate.c, 2));
        data.taup2 = Math.Abs(data.Qa) / (data.pfip * (dataIn.FirstTubePlate.sp - dataIn.FirstTubePlate.c));

        data.sigmaMX = Math.Abs(data.QK) / (dataIn.FirstTubePlate.s1 - dataIn.cK);
        data.sigmaIX = 6 * Math.Abs(data.MK) / Math.Pow(dataIn.FirstTubePlate.s1 - dataIn.cK, 2);

        data.sigmaMfi = Math.Abs(dataIn.pM) * data.a / (dataIn.FirstTubePlate.s1 - dataIn.cK);
        data.sigmaIfi = 0.3 * data.sigmaIX;

        data.sigma1T = Math.Abs(data.NT) / (Math.PI * (dataIn.dT - dataIn.sT) * dataIn.sT);
        data.sigma1 = data.sigma1T + dataIn.dT * Math.Abs(data.MT) / (2 * data.JT);

        data.sigma2T = (dataIn.dT - dataIn.sT) * Math.Max(Math.Abs(dataIn.pT - dataIn.pM), Math.Max(dataIn.pT, dataIn.pM)) /
                       (2 * dataIn.sT);

        data.ConditionStaticStressForTubePlate1 = Math.Max(data.taup1, data.taup2);
        data.ConditionStaticStressForTubePlate2 = 0.8 * data.sigma_dp;

        data.IsConditionStaticStressForTubePlate = data.ConditionStaticStressForTubePlate1 <= data.ConditionStaticStressForTubePlate2;
        if (!data.IsConditionStaticStressForTubePlate)
        {
            data.ErrorList.Add("Условие статической прочности трубной решетки не выполняется.");
        }

        data.deltasigma1pk = data.sigmap1;
        data.deltasigma2pk = 0.0;
        data.deltasigma3pk = 0.0;
        data.sigmaa_5_2_4k = CalculateSigmaa(data.Ksigma, data.deltasigma1pk, data.deltasigma2pk, data.deltasigma3pk);

        if (data.sigmaa_5_2_4k > data.sigmaa_dp)
        {
            data.ErrorList.Add("Условие малоцикловой прочности трубной решетки в месте соединения с кожухом не выполняется.");
        }

        data.deltasigma1pp = data.sigmap2;
        data.deltasigma2pp = 0.0;
        data.deltasigma3pp = 0.0;
        data.sigmaa_5_2_4p = CalculateSigmaa(1, data.deltasigma1pp, data.deltasigma2pp, data.deltasigma3pp);

        if (data.sigmaa_5_2_4p > data.sigmaa_dp)
        {
            data.ErrorList.Add("Условие малоцикловой прочности трубной решетки в перфорированной части не выполняется.");
        }

        if (!dataIn.IsOneGo)
        {
            data.spp = (dataIn.FirstTubePlate.sp - dataIn.FirstTubePlate.c) * data.sigma_dp / (2 * data.sigmaa_dp);
            data.sp = data.spp + dataIn.FirstTubePlate.c;
            if (data.sp > dataIn.FirstTubePlate.sp)
            {
                data.ErrorList.Add("Толщина трубной решетки меньше расчетной.");
            }

            data.snp1 = 1 - Math.Sqrt(dataIn.d0 / dataIn.FirstTubePlate.BP * (dataIn.tP / dataIn.tp - 1.0));
            data.snp2 = Math.Sqrt(data.fip);
            data.snp = (dataIn.FirstTubePlate.sp - dataIn.FirstTubePlate.c) * Math.Max(data.snp1, data.snp2);
            data.sn = data.snp + dataIn.FirstTubePlate.c;

            if (data.sn > dataIn.FirstTubePlate.sn)
            {
                data.ErrorList.Add("Толщина трубной решетки в месте паза под перегородку меньше расчетной.");
            }
        }

        if (dataIn.FirstTubePlate.IsNeedCheckHardnessTubePlate)
        {
            data.W = 1.2 / (data.Ky * dataIn.a1) * Math.Abs(data.T1 * data.QP + data.T2 * data.beta * data.MP);
            data.W_d = PhysicalHelper.GetWd(dataIn.D, PhysicalData);

            if (data.W > data.W_d)
            {
                data.ErrorList.Add("Превышен допустимый прогиб трубной решетки.");
            }
        }

        if (dataIn.FirstTubePlate.TubePlateType != TubePlateType.WeldedInFlange)
        {
            data.ConditionStaticStressForShell2 = 1.3 * data.sigma_dK;

            data.IsConditionStaticStressForShell = data.sigmaMX <= data.ConditionStaticStressForShell2;
            if (!data.IsConditionStaticStressForShell)
            {
                data.ErrorList.Add("Условие статической прочности кожуха в месте присоединения к трубной решетке не выполняется.");
            }

            data.deltasigma1K = data.sigmaMX + data.sigmaIX;
            data.deltasigma2K = data.sigmaMfi + data.sigmaIfi;
            data.deltasigma3K = 0.0;
            data.sigmaaK = CalculateSigmaa(data.Ksigma, data.deltasigma1K, data.deltasigma2K, data.deltasigma3K);

            if (data.sigmaaK > data.sigmaa_dK)
            {
                data.ErrorList.Add("Условие малоцикловой прочности кожуха в месте присоединения к трубной решетке не выполняется.");
            }
        }

        if (data.F < 0)
        {
            data.ErrorList.Add("Нужен расчет обечайки от сжимающей силы F.");
            //TODO: Make stress calculate shell for F
        }

        data.ConditionStaticStressForTube1 = Math.Max(data.sigma1T, data.sigma2T);

        data.IsConditionStaticStressForTube = data.ConditionStaticStressForTube1 <= data.sigma_dT;
        if (!data.IsConditionStaticStressForTube)
        {
            data.ErrorList.Add("Условие статической прочности труб не выполняется.");
        }

        data.deltasigma1T = data.sigma1;
        data.deltasigma2T = 0.0;
        data.deltasigma3T = 0.0;
        data.sigmaaT = CalculateSigmaa(1, data.deltasigma1T, data.deltasigma2T, data.deltasigma3T);

        if (data.sigmaaT > data.sigmaa_dT)
        {
            data.ErrorList.Add("Условие малоцикловой прочности труб не выполняется.");
        }

        if (data.NT < 0)
        {
            data.KT = dataIn.IsWorkCondition ? 1.3 : 1.126;
            data.lR = dataIn.IsWithPartitions ? Math.Max(dataIn.l2R, 0.7 * dataIn.l1R) : dataIn.l;
            data.lambda = data.KT * Math.Sqrt(data.sigma_dT / data.ET) * data.lR / (dataIn.dT - dataIn.sT);
            data.phiT = 1 / Math.Sqrt(1 + Math.Pow(data.lambda, 4));

            data.ConditionStabilityForTube2 = data.phiT * data.sigma_dT;

            data.IsConditionStabilityForTube = data.sigma1T <= data.ConditionStabilityForTube2;
            if (!data.IsConditionStabilityForTube)
            {
                data.ErrorList.Add("Условие устойчивости труб не выполняется.");
            }

            if (dataIn.IsNeedCheckHardnessTube)
            {
                data.lambday = Math.Abs(data.NT) * Math.Pow(data.lpr, 2) / (data.ET * data.JT);
                data.Ay = (1 - Math.Cos(Math.Sqrt(data.lambday))) / Math.Cos(Math.Sqrt(data.lambday));
                data.Y = data.Ay * Math.Abs(data.MT) / Math.Abs(data.NT);

                if (data.Y >= dataIn.tp - dataIn.dT)
                {
                    data.ErrorList.Add("Прогиб трубы превышает зазор между трубами в пучке и приводит к их соприкосновению.");
                }
            }
        }

        if (dataIn.FirstTubePlate.FixTubeInTubePlate != FixTubeInTubePlateType.OnlyWelding)
        {
            switch (dataIn.FirstTubePlate.TubeRolling)
            {
                case TubeRollingType.RollingWithoutGroove:
                    data.Ntp_d = 0.5 * Math.PI * dataIn.sT * (dataIn.dT - dataIn.sT) * Math.Min(dataIn.FirstTubePlate.lB / dataIn.dT, 1.6) *
                                 Math.Min(data.sigma_dT, data.sigma_dp);
                    break;
                case TubeRollingType.RollingWithOneGroove:
                    var x = Math.Min(dataIn.FirstTubePlate.lB / dataIn.dT, 1.6);
                    data.Ntp_d = 0.6 * Math.PI * dataIn.sT * (dataIn.dT - dataIn.sT) * Math.Min(data.sigma_dT, data.sigma_dp) *
                                 (x > 1.2 ? x : 1.0);
                    break;
                case TubeRollingType.RollingWithMoreThenOneGroove:
                    data.Ntp_d = 0.8 * Math.PI * dataIn.sT * (dataIn.dT - dataIn.sT) * Math.Min(data.sigma_dT, data.sigma_dp);
                    break;
            }
        }

        data.phiC2 = 0.95 - 0.2 * Math.Log(dataIn.N);
        data.phiC = Math.Min(0.5, data.phiC2);
        data.tau = (Math.Abs(data.NT) * dataIn.dT + 4 * Math.Abs(data.MT)) /
                   (Math.PI * Math.Pow(dataIn.dT, 2) * dataIn.FirstTubePlate.delta);

        switch (dataIn.FirstTubePlate.FixTubeInTubePlate)
        {
            case FixTubeInTubePlateType.OnlyRolling:

                data.IsConditionStressBracingTube = Math.Abs(data.NT) <= data.Ntp_d;
                break;

            case FixTubeInTubePlateType.OnlyWelding:

                data.ConditionStressBracingTube2 = data.phiC * Math.Min(data.sigma_dT, data.sigma_dp);
                data.IsConditionStressBracingTube = data.tau <= data.ConditionStressBracingTube2;
                break;

            case FixTubeInTubePlateType.RollingWithWelding:

                data.ConditionStressBracingTube11 = data.phiC * Math.Min(data.sigma_dT, data.sigma_dp) / data.tau +
                                                    0.6 * data.Ntp_d / Math.Abs(data.NT);
                data.ConditionStressBracingTube12 = data.Ntp_d / Math.Abs(data.NT);
                data.IsConditionStressBracingTube = Math.Max(data.ConditionStressBracingTube11, data.ConditionStressBracingTube12) >= 1.0;
                break;
        }

        if (!data.IsConditionStressBracingTube)
        {
            data.ErrorList.Add("Условие прочности крепления трубы в решетке не выполняется.");
        }

        data.pp = Math.Max(dataIn.pM, Math.Max(dataIn.pT, Math.Abs(dataIn.pM - dataIn.pT)));
        data.spp_5_5_1 = 0.5 * dataIn.DE * Math.Sqrt(data.pp / data.sigma_dp);
        data.sp_5_5_1 = data.spp_5_5_1 + dataIn.FirstTubePlate.c;

        if (data.sp_5_5_1 > dataIn.FirstTubePlate.sp)
        {
            data.ErrorList.Add("Толщина трубной решетки меньше расчетной.");
        }

        if (dataIn is { IsOneGo: false, FirstTubePlate.IsWithGroove: true })
        {
            //_fper = 1 / (1 + (dataIn.Bper / dataIn.Lper) + Math.Pow(dataIn.Bper / dataIn.Lper, 2));
            //_sper = (0.71 * dataIn.Bper * Math.Sqrt(dataIn.deltap * data.fper / data.sigma_dper)) + dataIn.cper;
            //if  (data.sper > dataIn.sper)
            //{
            //    IsError = true;
            //    data.ErrorList.Add("Толщина перегородки в кожухе меньше расчетной");
            //}
        }

        return data;
    }

    private static double CalculateSigmaa(double Ksigma,
        double deltasigma1,
        double deltasigma2,
        double deltasigma3) => Ksigma / 2 * Math.Max(Math.Abs(deltasigma1 - deltasigma2),
        Math.Max(Math.Abs(deltasigma2 - deltasigma3), Math.Abs(deltasigma1 - deltasigma3)));
}