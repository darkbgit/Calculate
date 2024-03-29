﻿using CalculateVessels.Core.Bottoms.Enums;
using CalculateVessels.Core.Exceptions;
using CalculateVessels.Core.Helpers;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Enums;
using CalculateVessels.Data.Exceptions;
using CalculateVessels.Data.Interfaces;
using System;

namespace CalculateVessels.Core.Bottoms.FlatBottomWithAdditionalMoment;

internal class FlatBottomWithAdditionalMomentCalculateService : ICalculateService<FlatBottomWithAdditionalMomentInput>
{
    private readonly IPhysicalDataService _physicalData;
    public FlatBottomWithAdditionalMomentCalculateService(IPhysicalDataService physicalData)
    {
        _physicalData = physicalData;
        Name = "GOST 34233.2-2017";
    }

    public string Name { get; }

    public ICalculatedElement Calculate(FlatBottomWithAdditionalMomentInput dataIn)
    {
        var data = new FlatBottomWithAdditionalMomentCalculated
        {
            InputData = dataIn,
            SigmaAllow = PhysicalHelper.GetSigmaIfZero(dataIn.SigmaAllow, dataIn.CoverSteel, dataIn.t, _physicalData),
            c = dataIn.c1 + dataIn.c2 + dataIn.c3
        };


        if (dataIn.IsFlangeIsolated)
        {
            data.tf = dataIn.t;
            data.tb = dataIn.t * 0.97;
        }
        else
        {
            data.tf = dataIn.t * 0.96;
            data.tb = dataIn.t * 0.95;
        }

        data.E = PhysicalHelper.GetE(dataIn.FlangeSteel, data.tf, _physicalData);
        data.E20 = PhysicalHelper.GetE(dataIn.FlangeSteel, 20, _physicalData);
        data.alpha_f = PhysicalHelper.GetAlpha(dataIn.FlangeSteel, dataIn.t, _physicalData);

        data.Ekr = PhysicalHelper.GetEIfZero(dataIn.E, dataIn.CoverSteel, dataIn.t, _physicalData);
        data.Ekr20 = PhysicalHelper.GetE(dataIn.CoverSteel, 20, _physicalData);
        data.alpha_kr = PhysicalHelper.GetAlpha(dataIn.CoverSteel, dataIn.t, _physicalData);


        // Gost 34233.4 add G
        data.sigma_d_krm = data.SigmaAllow * 1.5 / 1.1;

        data.tkr = dataIn.t;
        data.hkr = dataIn.s2;
        data.delta_kr = dataIn.s2;
        data.dkr = dataIn.Screwd;

        if (dataIn.IsWasher)
        {
            data.alpha_sh1 = PhysicalHelper.GetAlpha(dataIn.WasherSteel, data.tf, _physicalData, AlphaSource.G34233D4);
            data.alpha_sh2 = data.alpha_sh1;
        }

        data.Eb = PhysicalHelper.GetE(dataIn.ScrewSteel, data.tb, _physicalData, ESource.G34233D4);
        data.Eb20 = PhysicalHelper.GetE(dataIn.ScrewSteel, 20, _physicalData, ESource.G34233D4);
        data.alpha_b = PhysicalHelper.GetAlpha(dataIn.ScrewSteel, data.tb, _physicalData, AlphaSource.G34233D4);
        data.sigma_dnb = PhysicalHelper.GetSigma(dataIn.ScrewSteel, data.tb, _physicalData, SigmaSource.G34233D4);

        try
        {
            data.fb = _physicalData.Gost34233D4Get_fb(dataIn.Screwd, dataIn.IsScrewWithGroove);
        }
        catch (PhysicalDataException e)
        {
            throw new CalculateException($"Couldn't get fb. {e.Message}", e);
        }

        data.S0 = dataIn.IsFlangeFlat ? dataIn.s : dataIn.S0;

        try
        {
            data.Gasket = _physicalData.Gost34233D4GetGasketParameters(dataIn.GasketType);
        }
        catch (PhysicalDataException e)
        {
            throw new CalculateException($"Couldn't gasket parameters. {e.Message}", e);
        }

        if (!data.Gasket.IsMetal)
        {
            data.Ep = data.Gasket.Ep switch
            {
                -1 => 0.00003 * (1 + dataIn.bp / (2 * dataIn.hp)),
                -2 => 0.00004 * (1 + dataIn.bp / (2 * dataIn.hp)),
                _ => data.Gasket.Ep
            };
        }

        if (data.Gasket.IsFlat)
        {
            data.b0 = dataIn.bp <= 15 ? dataIn.bp : 3.8 * Math.Sqrt(dataIn.bp);
            data.Dcp = dataIn.Dcp + dataIn.bp - data.b0;
        }
        else
        {
            data.b0 = dataIn.bp / 4;
            data.Dcp = dataIn.Dcp;
        }

        data.Pobj = 0.5 * Math.PI * data.Dcp * data.b0 * data.Gasket.qobj;
        data.Rp = dataIn.IsPressureIn ? Math.PI * data.Dcp * data.b0 * data.Gasket.m * Math.Abs(dataIn.p) : 0.0;

        data.Ab = dataIn.n * data.fb;


        data.Qd = 0.785 * dataIn.p * Math.Pow(data.Dcp, 2);


        data.QFM = dataIn.F + 4 * Math.Abs(dataIn.M) / data.Dcp;


        data.b = 0.5 * (dataIn.Db - data.Dcp);

        data.l0 = Math.Sqrt(dataIn.D * data.S0);
        data.beta = dataIn.S1 / data.S0;
        data.x = dataIn.l / data.l0;

        data.zeta = 1 + (data.beta - 1) * data.x / (data.x + (1 + data.beta) / 4.0);
        data.Se = 0.5 * (data.zeta * data.S0);


        data.yp = data.Gasket.IsMetal
            ? 0
            : dataIn.hp * data.Gasket.Kobj / (data.Ep * Math.PI * data.Dcp * dataIn.bp);

        data.Lb = dataIn.Lb0 + (dataIn.IsStud ? 0.56 : 0.28) * dataIn.Screwd;
        data.yb = data.Lb / (data.Eb20 * data.fb * dataIn.n);

        data.KGost34233_4 = dataIn.Dn / dataIn.D;

        data.betaT = (Math.Pow(data.KGost34233_4, 2) * (1 + 8.55 * Math.Log(data.KGost34233_4)) - 1) /
                     (1.05 + 1.945 * Math.Pow(data.KGost34233_4, 2) * (data.KGost34233_4 - 1));
        data.betaU = (Math.Pow(data.KGost34233_4, 2) * (1 + 8.55 * Math.Log(data.KGost34233_4)) - 1) /
                     (1.36 * (Math.Pow(data.KGost34233_4, 2) - 1) * (data.KGost34233_4 - 1));
        data.betaY = 1 / (data.KGost34233_4 - 1) *
                     (0.69 + 5.72 * Math.Pow(data.KGost34233_4, 2) * Math.Log(data.KGost34233_4) /
                         (Math.Pow(data.KGost34233_4, 2) - 1));
        data.betaZ = (Math.Pow(data.KGost34233_4, 2) + 1) / (Math.Pow(data.KGost34233_4, 2) - 1);


        data.betaF = 0.91;
        data.betaV = 0.55;
        data.f = 1.0;
        //TODO:data.betaF,data.betaV,data.f take values from diagram. how?

        data.lambda = data.betaF * dataIn.h + data.l0 / (data.betaT * data.l0) +
                      data.betaV * Math.Pow(dataIn.h, 3) / (data.betaU * data.l0 * Math.Pow(data.S0, 2));
        data.yF = 0.91 * data.betaV / (data.E20 * data.lambda * Math.Pow(data.S0, 2) * data.l0);

        data.yfn = Math.Pow(Math.PI / 4, 3) * dataIn.Db / (data.E20 * dataIn.Dn * Math.Pow(dataIn.h, 3));

        if (dataIn.IsCoverFlat)
        {
            data.Kkr = dataIn.Dn / data.Dcp;
            data.Xkr = 0.67 * (Math.Pow(data.Kkr, 2) * (1 + 8.55 * Math.Log(data.Kkr)) - 1) /
                       ((data.Kkr - 1) * (Math.Pow(data.Kkr, 2) - 1 + (1.857 * Math.Pow(data.Kkr, 2) + 1) *
                           Math.Pow(data.hkr, 3) / Math.Pow(data.dkr, 3)));
            data.ykr = data.Xkr / (data.Ekr20 * Math.Pow(data.delta_kr, 3));
        }
        else
        {
            //TODO: Ad
        }

        data.gamma = 1 / (data.yp + data.yb * data.Eb20 / data.Eb +
                          (data.yF * data.E20 / data.E + data.ykr * data.Ekr20 / data.Ekr) * Math.Pow(data.b, 2));

        data.Qt = data.gamma * ((data.alpha_f * dataIn.h + data.alpha_sh1 * dataIn.hsh) * (data.tf - 20) +
                                (data.alpha_kr * data.hkr + data.alpha_sh2 * dataIn.hsh) * (data.tkr - 20) -
                                data.alpha_b * (dataIn.h + data.hkr) * (data.tb - 20));

        data.e = 0.5 * (data.Dcp - dataIn.D - data.Se);

        data.alpha = 1 - (data.yp - (data.yF * data.e + data.ykr * data.b) * data.b) /
            (data.yp + data.yb + (data.yF + data.ykr) * Math.Pow(data.b, 2));

        data.alpha_m = (data.yb + 2 * data.yfn * data.b * (data.b + data.e - Math.Pow(data.e, 2) / data.Dcp)) /
                      (data.yb + data.yp * Math.Pow(dataIn.Db / data.Dcp, 2) + 2 * data.yfn * Math.Pow(data.b, 2));



        data.Pb1_1 = data.alpha * (data.Qd + dataIn.F) + data.Rp + 4 * data.alpha_m * Math.Abs(dataIn.M) / data.Dcp;
        data.Pb1_2 = data.alpha * (data.Qd + dataIn.F) + data.Rp + 4 * data.alpha_m * Math.Abs(dataIn.M) / data.Dcp -
                     data.Qt;
        data.Pb1 = Math.Max(data.Pb1_1, data.Pb1_2);

        data.Pb2_2 = 0.4 * data.Ab * data.sigma_dnb;
        data.Pb2 = Math.Max(data.Pobj, data.Pb2_2);

        data.Pbm = Math.Max(data.Pb1, data.Pb2);

        data.Pbp = data.Pbm + (1 - data.alpha) * (data.Qd + dataIn.F) + data.Qt +
                   4 * (1 - data.alpha_m) * Math.Abs(dataIn.M) / data.Dcp;

        data.psi1 = data.Pbp / data.Qd;

        data.K6 = dataIn.IsCoverWithGroove
            ? 0.41 * Math.Sqrt(
                (1.0 + 3.0 * data.psi1 * (dataIn.D3 / data.Dcp - 1) +
                 9.6 * dataIn.D3 / data.Dcp * dataIn.s4 / data.Dcp) / (dataIn.D3 / data.Dcp))
            : 0.41 * Math.Sqrt((1.0 + 3.0 * data.psi1 * (dataIn.D3 / data.Dcp - 1)) / (dataIn.D3 / data.Dcp));


        data.Dp = data.Dcp;
        switch (dataIn.Hole)
        {
            case HoleInFlatBottom.WithoutHole:
                data.K0 = 1;
                break;
            case HoleInFlatBottom.OneHole:
                data.K0 = Math.Sqrt(1.0 + dataIn.d / data.Dp + Math.Pow(dataIn.d / data.Dp, 2));
                break;
            case HoleInFlatBottom.MoreThenOneHole:
                if (dataIn.di > 0.7 * data.Dp)
                {
                    data.ErrorList.Add("Слишком много отверстий.");
                }

                data.K0 = Math.Sqrt((1 - Math.Pow(dataIn.di / data.Dp, 3)) / (1 - dataIn.di / data.Dp));
                break;
            default:
                throw new CalculateException("Ошибка определения количества отверстий.");
        }

        data.s1p = data.K0 * data.K6 * data.Dp * Math.Sqrt(dataIn.p / (dataIn.fi * data.SigmaAllow));
        data.s1 = data.s1p + data.c;

        data.Phi_1 = data.Pbp / data.SigmaAllow;
        data.Phi_2 = data.Pbm / data.sigma_d_krm;
        data.Phi = Math.Max(data.Phi_1, data.Phi_2);

        data.K7_s2 = 0.8 * Math.Sqrt(dataIn.D3 / data.Dcp - 1);

        data.s2p_1 = data.K7_s2 * Math.Sqrt(data.Phi);
        data.s2p_2 = 0.6 / data.Dcp * data.Phi;
        data.s2p = Math.Max(data.s2p_1, data.s2p_2);
        data.s2 = data.s2p + data.c;

        data.K7_s3 = 0.8 * Math.Sqrt(dataIn.D3 / dataIn.D2 - 1);

        data.s3p_1 = data.K7_s3 * Math.Sqrt(data.Phi);
        data.s3p_2 = 0.6 / data.Dcp * data.Phi;
        data.s3p = Math.Max(data.s3p_1, data.s3p_2);
        data.s3 = data.s3p + data.c;


        if (dataIn.s1 < data.s1)
        {
            throw new CalculateException($"Принятая толщина s1 {dataIn.s1} меньше расчетной {data.s1}");
        }

        if (dataIn.s2 < data.s2)
        {
            throw new CalculateException($"Принятая толщина s1 {dataIn.s2} меньше расчетной {data.s2}");
        }

        if (dataIn.s3 < data.s3)
        {
            throw new CalculateException($"Принятая толщина s1 {dataIn.s3} меньше расчетной {data.s3}");
        }


        data.ConditionUseFormulas = (dataIn.s1 - data.c) / data.Dp;
        data.IsConditionUseFormulas = data.ConditionUseFormulas <= 0.11;
        data.Kp = data.IsConditionUseFormulas
            ? 1
            : 2.2 / (1 + Math.Sqrt(1 + Math.Pow(6 * (dataIn.s1 - data.c) / data.Dp, 2)));

        data.p_d = Math.Pow((dataIn.s1 - data.c) / (data.K0 * data.K6 * data.Dp), 2) * data.SigmaAllow * dataIn.fi;
        if (data.Kp * data.p_d < dataIn.p)
        {
            data.ErrorList.Add("Допускаемое давление меньше расчетного.");
        }

        return data;
    }
}