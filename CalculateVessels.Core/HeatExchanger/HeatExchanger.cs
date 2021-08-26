using CalculateVessels.Core.Bottoms.Enums;
using CalculateVessels.Core.HeatExchanger.Enums;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Word;
using CalculateVessels.Core.Word.Enums;
using CalculateVessels.Data.PhysicalData;
using CalculateVessels.Data.PhysicalData.Enums;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageMagick;
using System.Security.Cryptography;


namespace CalculateVessels.Core.HeatExchanger
{
    public class HeatExchanger : IElement
    {
        public HeatExchanger(HeatExchangerDataIn heatExchangerDataIn)
        {
            _hedi = heatExchangerDataIn;
        }

        private readonly HeatExchangerDataIn _hedi;

        private bool _isConditionStabilityForTube;
        private bool _isConditionStaticStressForShell;
        private bool _isConditionStaticStressForTube;
        private bool _isConditionStaticStressForTubePlate;
        private bool _isConditionStressBracingTube;
        private double _A;
        private double _AForSigmaaK;
        private double _AForSigmaap;
        private double _AForSigmaaT;
        private double _Akom;
        private double _alfaK;
        private double _alfaT;
        private double _Ap;
        private double _Ap1;
        private double _Ap2;
        private double _Ay;
        private double _B;
        private double _b1;
        private double _b2;
        private double _beta;
        private double _beta1;
        private double _beta2;
        private double _betakom;
        private double _betap;
        private double _BForSigmaa;
        private double _Bp1;
        private double _Bp2;
        private double _Cf;
        private double _conditionStabilityForTube2;
        private double _conditionStaticStressForShell2;
        private double _conditionStaticStressForTube1;
        private double _conditionStaticStressForTubePlate1;
        private double _conditionStaticStressForTubePlate2;
        private double _conditionStressBracingTube11;
        private double _conditionStressBracingTube12;
        private double _conditionStressBracingTube2;
        private double _CtForSigmaa;
        private double _deltasigma1K;
        private double _deltasigma1T;
        private double _deltasigma2K;
        private double _deltasigma2T;
        private double _deltasigma3K;
        private double _deltasigma3T;
        private double _E1;
        private double _E2;
        private double _ED;
        private double _EK;
        private double _Ekom;
        private double _Ep;
        private double _Ep2;
        private double _ET;
        private double _etaM;
        private double _etaT;
        private double _F;
        private double _fip;
        private double _JT;
        private double _K1;
        private double _K2;
        private double _KF;
        private double _KF1;
        private double _KF2;
        private double _Kkom;
        private double _Kp;
        private double _Kpac;
        private double _Kpz;
        private double _Kq;
        private double _Kqz;
        private double _Ksigma;
        private double _KT;
        private double _Ky;
        private double _lambda;
        private double _lambday;
        private double _lpr;
        private double _lR;
        private double _m1;
        private double _m2;
        private double _mA;
        private double _Ma;
        private double _mcp;
        private double _MK;
        private double _Mmax;
        private double _mn;
        private double _MP;
        private double _MT;
        private double _nB;
        private double _nNForSigmaa;
        private double _nsigmaForSigmaa;
        private double _NT;
        private double _Ntp_d;
        private double _omega;
        private double _p0;
        private double _p1;
        private double _pfip;
        private double _Phi1;
        private double _Phi2;
        private double _Phi3;
        private double _phiC2;
        private double _phiC;
        private double _phiT;
        private double _psi0;
        private double _Qa;
        private double _QK;
        private double _QP;
        private double _R1;
        private double _R2;
        private double _Rmp;
        private double _ro;
        private double _ro1;
        private double _sigma_dK;
        private double _sigma_dp;
        private double _sigma_dT;
        private double _sigma1;
        private double _sigma1T;
        private double _sigma2T;
        private double _sigmaa_5_2_4k;
        private double _sigmaa_5_2_4p;
        private double _sigmaa_dK;
        private double _sigmaa_dp;
        private double _sigmaa_dT;
        private double _a;
        private double _sigmaaK;
        private double _sigmaaT;
        private double _sigmaIfi;
        private double _sigmaIX;
        private double _sigmaMfi;
        private double _sigmaMX;
        private double _sigmap1;
        private double _sigmap2;
        private double _sn;
        private double _snp;
        private double _snp1;
        private double _snp2;
        private double _sp;
        private double _sp_5_5_1;
        private double _spp;
        private double _pp;
        private double _spp_5_5_1;
        private double _t;
        private double _T1;
        private double _T2;
        private double _T3;
        private double _tau;
        private double _taup1;
        private double _taup2;
        private double _W;
        private double _W_d;
        private double _Xkom;
        private double _Y;
        private double _Ykom;
        private List<string> _errorList = new();
        private double _deltasigma1pk;
        private double _deltasigma2pk;
        private double _deltasigma3pk;
        private double _deltasigma1pp;
        private double _deltasigma2pp;
        private double _deltasigma3pp;
        private double _fper;
        private double _sper;
        private double _sigma_dper;
        private double _ED2;
        private double _E12;
        private double _E22;
        private double _sigma_dp2;
        private double _Rmp2;
        private double _AForSigmaap2;
        private double _sigmaa_dp2;

        public bool IsCriticalError { get; private set; }

        public bool IsError { get; private set; }

        public List<string> ErrorList { get => _errorList; private set => _errorList = value; }
        public List<string> Bibliography { get; } = new()
        {
            Data.Properties.Resources.GOST_34233_1,
            Data.Properties.Resources.GOST_34233_6,
            Data.Properties.Resources.GOST_34233_7
        };

        private static double DegToRad(double degree) => degree * Math.PI / 180;

        public void Calculate()
        {
            //TODO: Get physical parameters
            //ET
            if (!Physical.TryGetE(_hedi.SteelT, _hedi.TT, ref _ET, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }

            //EK
            if (!Physical.TryGetE(_hedi.SteelK, _hedi.TK, ref _EK, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }

            //ED
            if (!Physical.TryGetE(_hedi.FirstTubePlate.SteelD, _hedi.TT, ref _ED, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }

            if (_hedi.IsDifferentTubePlate)
            {
                if (!Physical.TryGetE(_hedi.SecondTubePlate.SteelD, _hedi.TT, ref _ED2, ref _errorList))
                {
                    IsCriticalError = true;
                    return;
                }
            }

            //Ep
            if (!Physical.TryGetE(_hedi.FirstTubePlate.Steelp, _hedi.TK, ref _Ep, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }
            if (_hedi.IsDifferentTubePlate)
            {
                if (!Physical.TryGetE(_hedi.SecondTubePlate.Steelp, _hedi.TK, ref _Ep2, ref _errorList))
                {
                    IsCriticalError = true;
                    return;
                }
            }


            //TODO: Get E1 for condition if two different tube plate and flange not with tube plate
            //E1
            if (true)
            {
                _E1 = _Ep;
            }
            else
            {
                if (!Physical.TryGetE(_hedi.FirstTubePlate.Steel1, _hedi.TK, ref _E1, ref _errorList))
                {
                    IsCriticalError = true;
                    return;
                }
            }
            if (_hedi.IsDifferentTubePlate)
            {
                if (true)
                {
                    _E12 = _Ep2;
                }
                else
                {
                    if (!Physical.TryGetE(_hedi.SecondTubePlate.Steel1, _hedi.TK, ref _E1, ref _errorList))
                    {
                        IsCriticalError = true;
                        return;
                    }
                }
            }

            //E2         
            if (!Physical.TryGetE(_hedi.FirstTubePlate.Steel2, _hedi.TK, ref _E2, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }
            if (_hedi.IsDifferentTubePlate)
            {
                if (!Physical.TryGetE(_hedi.SecondTubePlate.Steel2, _hedi.TK, ref _E22, ref _errorList))
                {
                    IsCriticalError = true;
                    return;
                }
            }

            //Ekom
            if (_hedi.IsNeedKcompensatorCalculate)
            {
                if (!Physical.TryGetE(_hedi.Steelkom, _hedi.TK, ref _Ekom, ref _errorList))
                {
                    IsCriticalError = true;
                    return;
                }
            }

            //[]p
            if (!Physical.Gost34233_1.TryGetSigma(_hedi.FirstTubePlate.Steelp, _hedi.TK, ref _sigma_dp, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }
            if (_hedi.IsDifferentTubePlate)
            {
                if (!Physical.Gost34233_1.TryGetSigma(_hedi.SecondTubePlate.Steelp, _hedi.TK, ref _sigma_dp2, ref _errorList))
                {
                    IsCriticalError = true;
                    return;
                }
            }

            //[]K
            if (!Physical.Gost34233_1.TryGetSigma(_hedi.SteelK, _hedi.TK, ref _sigma_dK, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }

            //[]T
            if (!Physical.Gost34233_1.TryGetSigma(_hedi.SteelT, _hedi.TT, ref _sigma_dT, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }

            //alfaK
            if (!Physical.TryGetAlfa(_hedi.SteelK, _hedi.TK, ref _alfaK, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }

            //alfaT
            if (!Physical.TryGetAlfa(_hedi.SteelT, _hedi.TT, ref _alfaT, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }

            if (!Physical.Gost34233_1.TryGetRm(_hedi.FirstTubePlate.Steelp, _hedi.TK, ref _Rmp, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }
            if (_hedi.IsDifferentTubePlate)
            {
                if (!Physical.Gost34233_1.TryGetRm(_hedi.SecondTubePlate.Steelp, _hedi.TK, ref _Rmp2, ref _errorList))
                {
                    IsCriticalError = true;
                    return;
                }
            }

            //TODO: sigmaa for different materials Al,Cu, Tt

            _nNForSigmaa = 10.0;
            _nsigmaForSigmaa = 2.0;

            _CtForSigmaa = (2300.0 - _hedi.TK) / 2300.0;

            _AForSigmaap = (Physical.Gost34233_1.GetSteelType(_hedi.FirstTubePlate.Steelp, ref _errorList) is (SteelType.Carbon or SteelType.Austenitiс) and not SteelType.Undefined) ? 60000.0 : 45000.0;

            _BForSigmaa = 0.4 * _Rmp;

            _sigmaa_dp = (_CtForSigmaa * _AForSigmaap / Math.Sqrt(_nNForSigmaa * _hedi.N)) + (_BForSigmaa / _nsigmaForSigmaa);

            if (_hedi.IsDifferentTubePlate)
            {
                _AForSigmaap2 = (Physical.Gost34233_1.GetSteelType(_hedi.FirstTubePlate.Steelp, ref _errorList) is (SteelType.Carbon or SteelType.Austenitiс) and not SteelType.Undefined) ? 60000.0 : 45000.0;

                _sigmaa_dp2 = (_CtForSigmaa * _AForSigmaap2 / Math.Sqrt(_nNForSigmaa * _hedi.N)) + (_BForSigmaa / _nsigmaForSigmaa);
            }

            _AForSigmaaK = (Physical.Gost34233_1.GetSteelType(_hedi.SteelK, ref _errorList) is (SteelType.Carbon or SteelType.Austenitiс) and not SteelType.Undefined) ? 60000.0 : 45000.0;
            _sigmaa_dK = (_CtForSigmaa * _AForSigmaaK / Math.Sqrt(_nNForSigmaa * _hedi.N)) + (_BForSigmaa / _nsigmaForSigmaa);

            _AForSigmaaT = (Physical.Gost34233_1.GetSteelType(_hedi.SteelT, ref _errorList) is (SteelType.Carbon or SteelType.Austenitiс) and not SteelType.Undefined) ? 60000.0 : 45000.0;
            _sigmaa_dT = (_CtForSigmaa * _AForSigmaaT / Math.Sqrt(_nNForSigmaa * _hedi.N)) + (_BForSigmaa / _nsigmaForSigmaa);

            _Ksigma = _hedi.FirstTubePlate.TubePlateType switch
            {
                TubePlateType.WeldedInShell => 1.7,
                TubePlateType.SimplyFlange => 1.7,
                TubePlateType.FlangeWithFlanging => 1.2,
                TubePlateType.SimplyFlangeWithShell => 1.7,
                TubePlateType.WeldedInFlange => 1.7,
                TubePlateType.BetweenFlange => 1.7,
                _ => throw new NotImplementedException(),
            };

            _a = _hedi.D / 2.0; 

            _mn = _a / _hedi.a1;
            _etaM = 1 - (_hedi.i * Math.Pow(_hedi.dT, 2) / (4 * Math.Pow(_hedi.a1, 2)));
            _etaT = 1 - (_hedi.i * Math.Pow(_hedi.dT - (2 * _hedi.sT), 2) / (4 * Math.Pow(_hedi.a1, 2)));

            _Ky = _ET * (_etaT - _etaM) / _hedi.l;
            _ro = _Ky * _hedi.a1 * _hedi.l / (_EK * _hedi.sK);

            switch (_hedi.CompensatorType)
            {
                case CompensatorType.No:
                    _Kqz = 0.0;
                    _Kpz = 0.0;
                    break;
                case CompensatorType.Compensator:
                    if (_hedi.IsNeedKcompensatorCalculate)
                    {
                        _betakom = _hedi.dkom / _hedi.Dkom;
                        _Xkom = 4 * _hedi.rkom * _betakom / (_hedi.dkom * (1 - _betakom));
                        _Ykom = 2.57 * _hedi.rkom /
                            Math.Sqrt(_hedi.dkom * _hedi.deltakom * (1 + 1 / _betakom));
                        _Cf = 1.0; //TODO: Get Cf value from chart
                        _Akom = 6.8 * _betakom * (1 + _betakom) / (_Cf * Math.Pow(1 - _betakom, 3));
                        _Kkom = _Ekom * Math.Pow(_hedi.deltakom, 3) * _Akom /
                            (_hedi.nkom * Math.Pow(_hedi.dkom, 2));
                    }
                    else
                    {
                        _Kkom = _hedi.Kkom;
                    }

                    _Kqz = Math.PI * _a * _EK * _hedi.sK / (_hedi.l * _Kkom);
                    _Kpz = Math.PI * (Math.Pow(_hedi.Dkom, 2) - Math.Pow(_hedi.dkom, 2)) * _EK * _hedi.sK /
                        (4.8 * _hedi.l * _a * _Kkom);
                    break;
                case CompensatorType.Expander:
                    _betap = _hedi.D / _hedi.D1;

                    if (_hedi.beta0 == 90)
                    {
                        _Ap = _betap <= 0.9
                            ? 9.2 * (Math.Pow(_betap, 2) * (1 - Math.Pow(_betap, 2))) /
                            (Math.Pow(1 - Math.Pow(_betap, 2), 2) - 4 * Math.Pow(_betap, 2) * Math.Log(_betap))
                            : 13.8 / Math.Pow(1 - _betap, 3) *
                            (1 - 5.0 / 2.0 * (1 - _betap) + 61.0 / 30.0 * Math.Pow(1 - _betap, 2) -
                            11.0 / 20.0 * Math.Pow(1 - _betap, 3));
                        _Kpac = _EK * Math.Pow(_hedi.deltap, 3) * _Ap / Math.Pow(_hedi.D, 2);
                        _Kqz = _a * _hedi.sK / _hedi.l *
                            ((Math.PI * _EK / _Kpac) + (_hedi.Lpac / (_hedi.deltap * _hedi.D1)));
                        _Kpz = _a * _hedi.sK / (_hedi.l * Math.Pow(_betap, 2)) *
                            (((1 - Math.Pow(_betap, 2)) / 4.8 * ((Math.PI * _EK / _Kpac) + (_hedi.Lpac / (_hedi.deltap * _hedi.D1)))) - (0.5 * Math.PI * _hedi.Lpac / (_hedi.deltap * _hedi.D1)));
                    }
                    else if (_hedi.beta0 is >= 15 and <= 60)
                    {
                        _Ap1 = 2 /
                            (Math.Sin(DegToRad(_hedi.beta0)) * Math.Pow(Math.Cos(DegToRad(_hedi.beta0)), 2)) *
                            Math.Log(1.0 / _betap);
                        _Ap2 = 1.82 * Math.Pow(Math.Sin(DegToRad(_hedi.beta0)), 2) *
                            (1 + Math.Sqrt(_betap)) / Math.Pow(Math.Cos(DegToRad(_hedi.beta0)), 3.0 / 2.0);
                        _Bp1 = -1.06 /
                            (Math.Sin(DegToRad(_hedi.beta0)) * Math.Pow(Math.Cos(DegToRad(_hedi.beta0)), 2)) *
                            (Math.Log(1 / _betap) + (((1.0 / Math.Pow(_betap, 2)) - 1) *
                            ((0.3 * Math.Pow(Math.Cos(DegToRad(_hedi.beta0)), 4)) +
                            (1.5 * Math.Pow(Math.Sin(DegToRad(_hedi.beta0)), 2)) -
                            (0.5 * Math.Pow(Math.Cos(DegToRad(_hedi.beta0)), 2)) +
                            Math.Pow(Math.Sin(DegToRad(_hedi.beta0)), 4))));
                        _Bp2 = 0.965 * Math.Pow(Math.Sin(DegToRad(_hedi.beta0)), 2) /
                            Math.Pow(Math.Cos(DegToRad(_hedi.beta0)), 3.0 / 2.0 *
                            ((1.0 / Math.Pow(_betap, 2)) - 1));
                        _Kqz = ((_a * (_Ap1 + (_Ap2 * Math.Sqrt(_hedi.D1 / _hedi.sK)))) -
                            (0.5 * (1 - _betap) * _hedi.Lpac)) / _hedi.l;
                        _Kpz = (_Bp1 + (_Bp2 * Math.Sqrt(_hedi.D1 / _hedi.sK))) * _a / _hedi.l;
                    }
                    break;
                case CompensatorType.CompensatorOnExpander:
                    //TODO: Make calculation compensator on expander 
                    break;
            }

            _Kq = 1 + _Kqz;
            _Kp = 1 + _Kpz;

            _psi0 = Math.Pow(_etaT, 7.0 / 3.0);

            _beta = _hedi.IsDifferentTubePlate
                ? 1.53 * Math.Pow((_Ky / _psi0) * ((1 / (_Ep * Math.Pow(_hedi.FirstTubePlate.sp, 3))) +
                (1 / (_Ep2 * Math.Pow(_hedi.SecondTubePlate.sp, 3)))), 1.0 / 4.0)
                : 1.82 / _hedi.FirstTubePlate.sp * Math.Pow(_Ky * _hedi.FirstTubePlate.sp / (_psi0 * _Ep), 1.0 / 4.0);

            _omega = _beta * _hedi.a1;

            _fip = 1 - (_hedi.d0 / _hedi.tp);

            _b1 = (_hedi.FirstTubePlate.DH - _hedi.D) / 2.0;
            _R1 = (_hedi.FirstTubePlate.DH - _hedi.D) / 4.0;
            _b2 = (_hedi.FirstTubePlate.DH - _hedi.D) / 2.0;
            _R2 = (_hedi.FirstTubePlate.DH - _hedi.D) / 4.0;

            _beta1 = 1.3 / Math.Sqrt(_a * _hedi.FirstTubePlate.s1);
            _beta2 = 1.3 / Math.Sqrt(_a * _hedi.FirstTubePlate.s2);
            _K1 = _beta1 * _a * _EK * Math.Pow(_hedi.FirstTubePlate.s1, 3) / (5.5 * _R1);
            _K2 = _beta2 * _a * _ED * Math.Pow(_hedi.FirstTubePlate.s2, 3) / (5.5 * _R2);
            _KF1 = (_E1 * Math.Pow(_hedi.FirstTubePlate.h1, 3) * _b1 / (12.0 * Math.Pow(_R1, 2))) +
                (_K1 * (1.0 + (_beta1 * _hedi.FirstTubePlate.h1 / 2.0)));
            _KF2 = (_E2 * Math.Pow(_hedi.FirstTubePlate.h2, 3) * _b2 / (12.0 * Math.Pow(_R2, 2))) +
                (_K2 * (1.0 + (_beta2 * _hedi.FirstTubePlate.h2 / 2.0)));
            _KF = _KF1 + _KF2;

            _mcp = 0.15 * _hedi.i * Math.Pow(_hedi.dT - _hedi.sT, 2) / Math.Pow(_hedi.a1, 2);

            _p0 = (((_alfaK * (_hedi.tK - _hedi.t0)) - (_alfaT * (_hedi.tT - _hedi.t0))) * _Ky * _hedi.l) +
                ((_etaT - 1.0 + _mcp + (_mn * (_mn + (0.5 * _ro * _Kq)))) * _hedi.pT) -
                ((_etaM - 1.0 + _mcp + (_mn * (_mn + (0.3 * _ro * _Kp)))) * _hedi.pM);

            _ro1 = _Ky * _a * _hedi.a1 / (Math.Pow(_beta, 2) * _KF * _R1);

            if (!Physical.Gost34233_7.TryGetPhi1Phi2Phi3(_omega, ref _Phi1, ref _Phi2, ref _Phi3, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }

            //TODO: Calculate values of _Phi1, _Phi2, _Phi3

            _t = 1 + (1.4 * _omega * (_mn - 1));
            _T1 = _Phi1 * (_mn + (0.5 * (1 + (_mn * _t)) * (_t - 1)));
            _T2 = _Phi2 * _t;
            _T3 = _Phi3 * _mn;

            //if (!Physical.Gost34233_7.TryGetT1T2T3(_omega, _mn, ref _T1, ref _T2, ref _T3, ref _errorList))
            //{
            //    IsCriticalError = true;
            //    return;
            //}

            _m1 = (1 + (_beta1 * _hedi.FirstTubePlate.h1)) / (2 * Math.Pow(_beta1, 2));
            _m2 = (1 + (_beta2 * _hedi.FirstTubePlate.h2)) / (2 * Math.Pow(_beta2, 2));
            _p1 = _Ky / (_beta * _KF) * ((_m1 * _hedi.pM) - (_m2 * _hedi.pT));

            _MP = _hedi.a1 / _beta * ((_p1 * (_T1 + (_ro * _Kq))) - (_p0 * _T2)) /
                (((_T1 + (_ro * _Kq)) * (_T3 + _ro1)) - Math.Pow(_T2, 2));

            _QP = _hedi.a1 * ((_p0 * (_T3 + _ro1)) - (_p1 * _T2)) /
                (((_T1 + (_ro * _Kq)) * (_T3 + _ro1)) - Math.Pow(_T2, 2));

            _Ma = _MP + ((_a - _hedi.a1) * _QP);
            _Qa = _mn * _QP;

            _NT = Math.PI * _hedi.a1 / _hedi.i * ((((_etaM * _hedi.pM) - (_etaT * _hedi.pT)) * _hedi.a1) +
                (_Phi1 * _Qa) + (_Phi2 * _beta * _Ma));
            _JT = Math.PI * (Math.Pow(_hedi.dT, 4) - Math.Pow(_hedi.dT - (2 * _hedi.sT), 4)) / 64;
            _lpr = _hedi.IsWithPartitions ? _hedi.l1R / 3.0 : _hedi.l;
            _MT = _ET * _JT * _beta / (_Ky * _hedi.a1 * _lpr) *
                ((_Phi2 * _Qa) + (_Phi3 * _beta * _Ma));

            _QK = (_a / 2.0 * _hedi.pT) - _QP;
            _MK = (_K1 / (_ro1 * _KF * _beta) * ((_T2 * _QP) + (_T3 * _beta * _MP))) -
                (_hedi.pM / (2 * Math.Pow(_beta1, 2)));

            _F = Math.PI * _hedi.D * _QK;

            _sigmap1 = 6 * Math.Abs(_MP) / Math.Pow(_hedi.FirstTubePlate.s1p - _hedi.FirstTubePlate.c, 2);
            _taup1 = Math.Abs(_QP) / (_hedi.FirstTubePlate.s1p - _hedi.FirstTubePlate.c);

            _mA = _beta * _Ma / _Qa;

            if (_mA >= -1.0 && _mA <= 1.0)
            {
                if (!Physical.Gost34233_7.TryGetA(_omega, _mA, ref _A, ref _errorList))
                {
                    IsCriticalError = true;
                    return;
                }
                _Mmax = _A * Math.Abs(_Qa) / _beta;
            }
            else
            {
                _nB = 1.0 / _mA;
                if (!Physical.Gost34233_7.TryGetB(_omega, _nB, ref _B, ref _errorList))
                {
                    IsCriticalError = true;
                    return;
                }
                _Mmax = _B * Math.Abs(_Ma);
            }

            _pfip = 1 - (_hedi.d0 / _hedi.tp);

            _sigmap2 = 6 * _Mmax / (_pfip * Math.Pow(_hedi.FirstTubePlate.sp - _hedi.FirstTubePlate.c, 2));
            _taup2 = Math.Abs(_Qa) / (_pfip * (_hedi.FirstTubePlate.sp - _hedi.FirstTubePlate.c));

            _sigmaMX = Math.Abs(_QK) / (_hedi.FirstTubePlate.s1 - _hedi.cK);
            _sigmaIX = 6 * Math.Abs(_MK) / Math.Pow(_hedi.FirstTubePlate.s1 - _hedi.cK, 2);

            _sigmaMfi = Math.Abs(_hedi.pM) * _a / (_hedi.FirstTubePlate.s1 - _hedi.cK);
            _sigmaIfi = 0.3 * _sigmaIX;

            _sigma1T = Math.Abs(_NT) / (Math.PI * (_hedi.dT - _hedi.sT) * _hedi.sT);
            _sigma1 = _sigma1T + (_hedi.dT * Math.Abs(_MT) / (2 * _JT));

            _sigma2T = (_hedi.dT - _hedi.sT) * Math.Max(Math.Abs(_hedi.pT - _hedi.pM), Math.Max(_hedi.pT, _hedi.pM)) /
                (2 * _hedi.sT);

            _conditionStaticStressForTubePlate1 = Math.Max(_taup1, _taup2);
            _conditionStaticStressForTubePlate2 = 0.8 * _sigma_dp;

            _isConditionStaticStressForTubePlate = _conditionStaticStressForTubePlate1 <= _conditionStaticStressForTubePlate2;
            if (!_isConditionStaticStressForTubePlate)
            {
                IsError = true;
                _errorList.Add("Условие статической прочности трубной решетки не выполняется");
            }

            _deltasigma1pk = _sigmap1;
            _deltasigma2pk = 0.0;
            _deltasigma3pk = 0.0;
            _sigmaa_5_2_4k = CalculateSigmaa(_Ksigma, _deltasigma1pk, _deltasigma2pk, _deltasigma3pk);

            if (_sigmaa_5_2_4k > _sigmaa_dp)
            {
                IsError = true;
                _errorList.Add("Условие малоцикловой прочности трубной решетки в месте соединения с кожухом не выполняется");
            }

            _deltasigma1pp = _sigmap2;
            _deltasigma2pp = 0.0;
            _deltasigma3pp = 0.0;
            _sigmaa_5_2_4p = CalculateSigmaa(1, _deltasigma1pp, _deltasigma2pp, _deltasigma3pp);

            if (_sigmaa_5_2_4p > _sigmaa_dp)
            {
                IsError = true;
                _errorList.Add("Условие малоцикловой прочности трубной решетки в перфорированной части не выполняется");
            }

            if (!_hedi.IsOneGo)
            {
                _spp = (_hedi.FirstTubePlate.sp - _hedi.FirstTubePlate.c) * _sigma_dp / (2 * _sigmaa_dp);
                _sp = _spp + _hedi.FirstTubePlate.c;
                if (_sp > _hedi.FirstTubePlate.sp)
                {
                    IsError = true;
                    _errorList.Add("Толщина трубной решетки меньше расчетной");
                }

                _snp1 = 1 - Math.Sqrt(_hedi.d0 / _hedi.FirstTubePlate.BP * (_hedi.tP / _hedi.tp - 1.0));
                _snp2 = Math.Sqrt(_fip);
                _snp = (_hedi.FirstTubePlate.sp - _hedi.FirstTubePlate.c) * Math.Max(_snp1, _snp2);
                _sn = _snp + _hedi.FirstTubePlate.c;

                if (_sn > _hedi.FirstTubePlate.sn)
                {
                    IsError = true;
                    _errorList.Add("Толщина трубной решетки в месте паза под перегородку меньше расчетной");
                }
            }

            if (_hedi.FirstTubePlate.IsNeedCheckHardnessTubePlate)
            {
                _W = 1.2 / (_Ky * _hedi.a1) * Math.Abs((_T1 * _QP) + (_T2 * _beta * _MP));

                if (!Physical.Gost34233_7.TryGetpW_d(_hedi.D, ref _W_d, ref _errorList))
                {
                    IsCriticalError = true;
                    return;
                }

                if (_W > _W_d)
                {
                    IsError = true;
                    _errorList.Add("Превышен допустимый прогиб трубной решетки");
                }
            }

            if (_hedi.FirstTubePlate.TubePlateType != TubePlateType.WeldedInFlange)
            {
                _conditionStaticStressForShell2 = 1.3 * _sigma_dK;

                _isConditionStaticStressForShell = _sigmaMX <= _conditionStaticStressForShell2;
                if (!_isConditionStaticStressForShell)
                {
                    IsError = true;
                    _errorList.Add("Условие статической прочности кожуха в месте присоединения к трубной решетке не выполняется");
                }

                _deltasigma1K = _sigmaMX + _sigmaIX;
                _deltasigma2K = _sigmaMfi + _sigmaIfi;
                _deltasigma3K = 0.0;
                _sigmaaK = CalculateSigmaa(_Ksigma, _deltasigma1K, _deltasigma2K, _deltasigma3K);

                if (_sigmaaK > _sigmaa_dK)
                {
                    IsError = true;
                    _errorList.Add("Условие малоцикловой прочности кожуха в месте присоединения к трубной решетке не выполняется");
                }
            }

            if (_F < 0)
            {
                IsError = true;
                _errorList.Add("Нужен расчет обечайки от сжимающей силы F");
                //TODO: Make stress calculate shell for F
            }

            _conditionStaticStressForTube1 = Math.Max(_sigma1T, _sigma2T);

            _isConditionStaticStressForTube = _conditionStaticStressForTube1 <= _sigma_dT;
            if (!_isConditionStaticStressForTube)
            {
                IsError = true;
                _errorList.Add("Условие статической прочности труб не выполняется");
            }

            _deltasigma1T = _sigma1;
            _deltasigma2T = 0.0;
            _deltasigma3T = 0.0;
            _sigmaaT = CalculateSigmaa(1, _deltasigma1T, _deltasigma2T, _deltasigma3T);

            if (_sigmaaT > _sigmaa_dT)
            {
                IsError = true;
                _errorList.Add("Условие малоцикловой прочности труб не выполняется");
            }

            if (_NT < 0)
            {
                _KT = _hedi.IsWorkCondition ? 1.3 : 1.126;
                _lR = _hedi.IsWithPartitions ? Math.Max(_hedi.l2R, 0.7 * _hedi.l1R) : _hedi.l;
                _lambda = _KT * Math.Sqrt(_sigma_dT / _ET) * _lR / (_hedi.dT - _hedi.sT);
                _phiT = 1 / Math.Sqrt(1 + Math.Pow(_lambda, 4));

                _conditionStabilityForTube2 = _phiT * _sigma_dT;

                _isConditionStabilityForTube = _sigma1T <= _conditionStabilityForTube2;
                if (!_isConditionStabilityForTube)
                {
                    IsError = true;
                    _errorList.Add("Условие устойчивости труб не выполняется");
                }

                if (_hedi.IsNeedCheckHardnessTube)
                {
                    _lambday = Math.Abs(_NT) * Math.Pow(_lpr, 2) / (_ET * _JT);
                    _Ay = (1 - Math.Cos(Math.Sqrt(_lambday))) / Math.Cos(Math.Sqrt(_lambday));
                    _Y = _Ay * Math.Abs(_MT) / Math.Abs(_NT);

                    if (_Y >= _hedi.tp - _hedi.dT)
                    {
                        IsError = true;
                        _errorList.Add("Прогиб трубы превышает зазор между трубами в пучке и приводит к их соприкосновению");
                    }
                }
            }

            if (_hedi.FirstTubePlate.FixTubeInTubePlate != FixTubeInTubePlateType.OnlyWelding)
            {
                switch (_hedi.FirstTubePlate.TubeRolling)
                {
                    case TubeRollingType.RollingWithoutGroove:
                        _Ntp_d = 0.5 * Math.PI * _hedi.sT * (_hedi.dT - _hedi.sT) * Math.Min(_hedi.FirstTubePlate.lB / _hedi.dT, 1.6) *
                            Math.Min(_sigma_dT, _sigma_dp);
                        break;
                    case TubeRollingType.RollingWithOneGroove:
                        var x = Math.Min(_hedi.FirstTubePlate.lB / _hedi.dT, 1.6);
                        _Ntp_d = 0.6 * Math.PI * _hedi.sT * (_hedi.dT - _hedi.sT) * Math.Min(_sigma_dT, _sigma_dp) *
                            (x > 1.2 ? x : 1.0);
                        break;
                    case TubeRollingType.RollingWithMoreThenOneGroove:
                        _Ntp_d = 0.8 * Math.PI * _hedi.sT * (_hedi.dT - _hedi.sT) * Math.Min(_sigma_dT, _sigma_dp);
                        break;
                }
            }

            _phiC2 = 0.95 - (0.2 * Math.Log(_hedi.N));
            _phiC = Math.Min(0.5, _phiC2);
            _tau = ((Math.Abs(_NT) * _hedi.dT) + (4 * Math.Abs(_MT))) /
                    (Math.PI * Math.Pow(_hedi.dT, 2) * _hedi.FirstTubePlate.delta);

            switch (_hedi.FirstTubePlate.FixTubeInTubePlate)
            {
                case FixTubeInTubePlateType.OnlyRolling:

                    _isConditionStressBracingTube = Math.Abs(_NT) <= _Ntp_d;
                    break;

                case FixTubeInTubePlateType.OnlyWelding:

                    _conditionStressBracingTube2 = _phiC * Math.Min(_sigma_dT, _sigma_dp);
                    _isConditionStressBracingTube = _tau <= _conditionStressBracingTube2;
                    break;

                case FixTubeInTubePlateType.RollingWithWelding:

                    _conditionStressBracingTube11 = _phiC * Math.Min(_sigma_dT, _sigma_dp) / _tau +
                        0.6 * _Ntp_d / Math.Abs(_NT);
                    _conditionStressBracingTube12 = _Ntp_d / Math.Abs(_NT);
                    _isConditionStressBracingTube = Math.Max(_conditionStressBracingTube11, _conditionStressBracingTube12) >= 1.0;
                    break;
            }

            if (!_isConditionStressBracingTube)
            {
                IsError = true;
                _errorList.Add("Условие прочности крепления трубы в решетке не выполняется");
            }

            _pp = Math.Max(_hedi.pM, Math.Max(_hedi.pT, Math.Abs(_hedi.pM - _hedi.pT)));
            _spp_5_5_1 = 0.5 * _hedi.DE * Math.Sqrt(_pp / _sigma_dp);
            _sp_5_5_1 = _spp_5_5_1 + _hedi.FirstTubePlate.c;

            if (_sp_5_5_1 > _hedi.FirstTubePlate.sp)
            {
                IsError = true;
                _errorList.Add("Толщина трубной решетки меньше расчетной");
            }

            if (!_hedi.IsOneGo && _hedi.FirstTubePlate.IsWithGroove)
            {
                //_fper = 1 / (1 + (_hedi.Bper / _hedi.Lper) + Math.Pow(_hedi.Bper / _hedi.Lper, 2));
                //_sper = (0.71 * _hedi.Bper * Math.Sqrt(_hedi.deltap * _fper / _sigma_dper)) + _hedi.cper;
                //if (_sper > _hedi.sper)
                //{
                //    IsError = true;
                //    _errorList.Add("Толщина перегородки в кожухе меньше расчетной");
                //}
            }
        }

        public void MakeWord(string filename)
        {
            if (filename == null)
            {
                const string DEFAULT_FILE_NAME = "temp.docx";
                filename = DEFAULT_FILE_NAME;
            }

            using WordprocessingDocument package = WordprocessingDocument.Open(filename, true);

            var mainPart = package.MainDocumentPart;
            var body = mainPart?.Document.Body;

            if (body == null) return;

            body.AddParagraph($"Расчет на прочность теплообменного аппарата с неподвижными трубными решетками {_hedi.Name}")
                .Heading(HeadingType.Heading1)
                .Alignment(AlignmentType.Center);


            {
                var imagePart = mainPart.AddImagePart(ImagePartType.Gif);

                var a = _hedi.FirstTubePlate.TubePlateType switch
                {
                    TubePlateType.WeldedInShell => "1",
                    TubePlateType.SimplyFlange => "2",
                    TubePlateType.SimplyFlangeWithShell => "2",
                    TubePlateType.FlangeWithFlanging => "2",
                    TubePlateType.WeldedInFlange => "2",
                    TubePlateType.BetweenFlange => "3",
                    _ => "1",
                };

                var b = _hedi.IsDifferentTubePlate
                    ? _hedi.FirstTubePlate.TubePlateType switch
                    {
                        TubePlateType.WeldedInShell => "1",
                        TubePlateType.SimplyFlange => "2",
                        TubePlateType.SimplyFlangeWithShell => "2",
                        TubePlateType.FlangeWithFlanging => "2",
                        TubePlateType.WeldedInFlange => "2",
                        TubePlateType.BetweenFlange => "3",
                        _ => "1",
                    } : a;

                var c = _hedi.CompensatorType switch
                {
                    CompensatorType.No => "",
                    CompensatorType.Compensator => "_Syl",
                    CompensatorType.Expander => "_Exp",
                    CompensatorType.CompensatorOnExpander => "_Exp",
                    _ => "",
                };

                var pic = (byte[])Data.Properties.Resources.ResourceManager.GetObject("Fixed_" + a + "_" + b + c);

                if (pic != null)
                {
                    imagePart.FeedData(new MemoryStream(pic));

                    body.AddParagraph("").AddImage(mainPart.GetIdOfPart(imagePart), pic);
                }
            }


            {
                var imagePart = mainPart.AddImagePart(ImagePartType.Gif);

                string type1 = ((int)_hedi.FirstTubePlate.TubePlateType).ToString();

                var type2 = _hedi.FirstTubePlate.IsChamberFlangeSkirt ? "_Butt" : "_Flat";

                string type3 = ((int)_hedi.FirstTubePlate.FlangeFace + 1).ToString();

                var b = (byte[])Data.Properties.Resources.ResourceManager.GetObject("ConnToFlange" + type1 + type2 + type3);

                if (b != null)
                {
                    imagePart.FeedData(new MemoryStream(b));

                    body.AddParagraph("").AddImage(mainPart.GetIdOfPart(imagePart), b);
                }
            }

            {
                var imagePart = mainPart.AddImagePart(ImagePartType.Gif);

                string s = "";

                switch (_hedi.FirstTubePlate.FixTubeInTubePlate)
                {
                    case FixTubeInTubePlateType.OnlyRolling:
                        switch (_hedi.FirstTubePlate.TubeRolling)
                        {
                            case TubeRollingType.RollingWithoutGroove:
                                s = "1";
                                break;
                            case TubeRollingType.RollingWithOneGroove:
                                s = "2_52857";
                                break;
                            case TubeRollingType.RollingWithMoreThenOneGroove:
                                s = "3_52857";
                                break;
                        }
                        break;
                    case FixTubeInTubePlateType.OnlyWelding:
                        s = "4";
                        break;
                    case FixTubeInTubePlateType.RollingWithWelding:
                        switch (_hedi.FirstTubePlate.TubeRolling)
                        {
                            case TubeRollingType.RollingWithoutGroove:
                                s = "1Weld";
                                break;
                            case TubeRollingType.RollingWithOneGroove:
                                s = "2Weld_52857";
                                break;
                            case TubeRollingType.RollingWithMoreThenOneGroove:
                                s = "3Weld_52857";
                                break;
                        }
                        break;
                }
                
                var pic = (byte[])Data.Properties.Resources.ResourceManager.GetObject("FixType" + s);

                if (pic != null)
                {
                    imagePart.FeedData(new MemoryStream(pic));

                    body.Elements<Paragraph>().LastOrDefault().AddImage(mainPart.GetIdOfPart(imagePart), pic);
                }
            }

            body.AddParagraph("Исходные данные").Alignment(AlignmentType.Center);

            //table
            {
                body.AddParagraph("");

                var table = body.AddTable();

                //table.AddRow().
                //  AddCell("");

                table.AddRow()
                    .AddCell("Колличество ходов")
                    .AddCell(_hedi.IsOneGo ? "Одноходовой" : "Многоходовой");


                table.AddRowWithOneCell("Кожух");

                table.AddRow()
                    .AddCell("Материал:")
                    .AddCell($"{_hedi.SteelK}");

                table.AddRow()
                    .AddCell("Внутренний диаметр кожуха, D:")
                    .AddCell($"{_hedi.D}");

                table.AddRow()
                    .AddCell("Толщина стенки кожуха, ")
                    .AppendEquation("s_K")
                    .AppendText(":")
                    .AddCell($"{_hedi.sK} мм");

                table.AddRow()
                    .AddCell("Расчетная прибавка к толщине стенки кожуха, ")
                    .AppendEquation("c_K")
                    .AppendText(":")
                    .AddCell($"{_hedi.cK} мм");

                table.AddRow()
                    .AddCell("Перегородки в межтрубном пространстве")
                    .AddCell(_hedi.IsWithPartitions ? "Есть" : "Нет");

                if (_hedi.IsWithPartitions)
                {
                    table.AddRow()
                        .AddCell("Максимальный пролет трубы между решеткой и перегородкой, ")
                        .AppendEquation("l_1R")
                        .AppendText(":")
                        .AddCell($"{_hedi.l1R} мм");

                    table.AddRow()
                        .AddCell("Максимальный пролет трубы между перегородками, ")
                        .AppendEquation("l_2R")
                        .AppendText(":")
                        .AddCell($"{_hedi.l2R} мм");

                }

                table.AddRowWithOneCell("Трубная решетка");

                table.AddRow()
                        .AddCell("Расстояние от оси кожуха до оси наиболее удаленной трубы, ")
                        .AppendEquation("a_1")
                        .AppendText(":")
                        .AddCell($"{_hedi.a1} мм");

                table.AddRow()
                        .AddCell("Диаметр окружности, вписанной в максимальную беструбную площадь, ")
                        .AppendEquation("D_E")
                        .AppendText(":")
                        .AddCell($"{_hedi.DE} мм");

                table.AddRow()
                        .AddCell("Диаметр отверстия в решетке, ")
                        .AppendEquation("d_0")
                        .AppendText(":")
                        .AddCell($"{_hedi.d0} мм");

                table.AddRow()
                        .AddCell("Шаг расположения отверстий в решетке, ")
                        .AppendEquation("t_p")
                        .AppendText(":")
                        .AddCell($"{_hedi.tp} мм");

                if (_hedi.FirstTubePlate.IsWithGroove || _hedi.SecondTubePlate.IsWithGroove)
                {
                    table.AddRow()
                        .AddCell("Расстояние между осями рядов отверстий с двух сторон от паза, ")
                        .AppendEquation("t_П")
                        .AppendText(":")
                        .AddCell($"{_hedi.tP} мм");
                }

                table.AddRow()
                    .AddCell("Первая трубная решетка")
                    .AddCell("");
                
                table.AddRow()
                    .AddCell("Материал:")
                    .AddCell($"{_hedi.FirstTubePlate.Steelp}");

                table.AddRow()
                    .AddCell("Тип")
                    .AddCell(_hedi.FirstTubePlate.TubePlateType switch
                    {
                        TubePlateType.WeldedInShell => "Решетка вварена в кожух",
                        TubePlateType.SimplyFlange => "Решетка-фланец приварена к кожуху",
                        TubePlateType.FlangeWithFlanging => "Решетка-фланец с отбортовкой приварена к кожуху",
                        TubePlateType.SimplyFlangeWithShell => "Решетка-фланец приварена к концевой обечайке",
                        TubePlateType.WeldedInFlange => "Решетка вварена в фланец",
                        TubePlateType.BetweenFlange => "Решетка вварена между фланцем и кожухом",
                    });

                table.AddRow()
                        .AddCell("Эффективный коэффициент концентрации напряжения, ")
                        .AppendEquation("K_σ")
                        .AppendText(":")
                        .AddCell($"{_Ksigma} мм");


                if (!_hedi.IsOneGo)
                {
                    table.AddRow()
                        .AddCell("Паз под перегородку в трубном пространстве")
                        .AddCell(_hedi.FirstTubePlate.IsWithGroove ? "Есть" : "Нет");

                    if (_hedi.FirstTubePlate.IsWithGroove)
                    {
                        table.AddRow()
                            .AddCell("Ширина канавки под прокладку в многоходовом аппарате, ")
                            .AppendEquation("B_П")
                            .AppendText(":")
                            .AddCell($"{_hedi.FirstTubePlate.BP} мм");

                        table.AddRow()
                            .AddCell("Толщина трубной решетки в сечении канавки под перегородку, ")
                            .AppendEquation("s_n")
                            .AppendText(":")
                            .AddCell($"{_hedi.FirstTubePlate.sn} мм");
                    }
                }

                table.AddRow()
                            .AddCell("Толщина трубной решетки, ")
                            .AppendEquation("s_p")
                            .AppendText(":")
                            .AddCell($"{_hedi.FirstTubePlate.sp} мм");

                table.AddRow()
                    .AddCell("Расчетная прибавка к толщине трубной решетки, ")
                    .AppendEquation("c")
                    .AppendText(":")
                    .AddCell($"{_hedi.FirstTubePlate.c} мм");

                table.AddRow()
                    .AddCell("")
                    .AddCell("Фланец кожуха");

                if (_hedi.FirstTubePlate.TubePlateType == TubePlateType.WeldedInFlange ||
                    _hedi.FirstTubePlate.TubePlateType == TubePlateType.BetweenFlange)
                {
                    table.AddRow()
                    .AddCell("Материал:")
                    .AddCell($"{_hedi.FirstTubePlate.Steel1}");
                }

                table.AddRow()
                            .AddCell("Наружный диаметр фланца, ")
                            .AppendEquation("D_H")
                            .AppendText(":")
                            .AddCell($"{_hedi.FirstTubePlate.DH} мм");

                table.AddRow()
                            .AddCell("Толщина тарелки фланца кожуха, ")
                            .AppendEquation("h_1")
                            .AppendText(":")
                            .AddCell($"{_hedi.FirstTubePlate.h1} мм");

                table.AddRow()
                            .AddCell("Толщина стенки кожуха в месте соединения с трубной решеткой, ")
                            .AppendEquation("s_1")
                            .AppendText(":")
                            .AddCell($"{_hedi.FirstTubePlate.s1} мм");

                table.AddRow()
                    .AddCell("")
                    .AddCell("Фланец камеры");

                table.AddRow()
                .AddCell("Материал фланца камеры:")
                .AddCell($"{_hedi.FirstTubePlate.Steel2}");

                table.AddRow()
                            .AddCell("Толщина тарелки фланца камеры, ")
                            .AppendEquation("h_2")
                            .AppendText(":")
                            .AddCell($"{_hedi.FirstTubePlate.h2} мм");

                table.AddRow()
                            .AddCell("Толщина стенки камеры в месте соединения с трубной решеткой, ")
                            .AppendEquation("s_2")
                            .AppendText(":")
                            .AddCell($"{_hedi.FirstTubePlate.s2} мм");

                table.AddRow()
                .AddCell("Материал стенки камеры:")
                .AddCell($"{_hedi.FirstTubePlate.SteelD}");

                table.AddRow()
                        .AddCell("Проверка жесткости трубных решеток")
                        .AddCell(_hedi.FirstTubePlate.IsNeedCheckHardnessTubePlate ? "Да" : "Нет");


                table.AddRowWithOneCell("Трубы");

                table.AddRow()
                    .AddCell("Материал:")
                    .AddCell($"{_hedi.SteelT}");

                table.AddRow()
                        .AddCell("Наружный диаметр трубы, ")
                        .AppendEquation("d_T")
                        .AppendText(":")
                        .AddCell($"{_hedi.dT} мм");

                table.AddRow()
                        .AddCell("Толщина стенки трубы, ")
                        .AppendEquation("s_T")
                        .AppendText(":")
                        .AddCell($"{_hedi.sT} мм");

                table.AddRow()
                        .AddCell("Число труб, i:")
                        .AddCell($"{_hedi.i} мм");

                table.AddRow()
                        .AddCell("Половина длины трубы теплообменного аппарата, l:")
                        .AddCell($"{_hedi.i} мм");

                table.AddRow()
                        .AddCell("Тип крепления трубы в трубной решетке")
                        .AddCell(_hedi.FirstTubePlate.FixTubeInTubePlate switch
                        {
                            FixTubeInTubePlateType.OnlyRolling => "Развальцовка",
                            FixTubeInTubePlateType.OnlyWelding => "Сварка",
                            FixTubeInTubePlateType.RollingWithWelding => "Развальцовка с обваркой",
                        });

                if (_hedi.FirstTubePlate.FixTubeInTubePlate == FixTubeInTubePlateType.OnlyRolling ||
                    _hedi.FirstTubePlate.FixTubeInTubePlate == FixTubeInTubePlateType.RollingWithWelding)
                {
                    table.AddRow()
                        .AddCell("Вид развальцовки")
                        .AddCell(_hedi.FirstTubePlate.TubeRolling switch
                        {
                            TubeRollingType.RollingWithoutGroove => "Гладкозавальцованные трубы",
                            TubeRollingType.RollingWithOneGroove => "Трубы завальцованные в пазы (один паз)",
                            TubeRollingType.RollingWithMoreThenOneGroove => "Трубы завальцованные в пазы (два и более паза)",
                        });
                }

                if ((_hedi.FirstTubePlate.FixTubeInTubePlate == FixTubeInTubePlateType.OnlyRolling ||
                    _hedi.FirstTubePlate.FixTubeInTubePlate == FixTubeInTubePlateType.RollingWithWelding) &&
                    (_hedi.FirstTubePlate.TubeRolling == TubeRollingType.RollingWithoutGroove ||
                    _hedi.FirstTubePlate.TubeRolling == TubeRollingType.RollingWithOneGroove))
                {
                    table.AddRow()
                        .AddCell("Глубина развальцовки труб, ")
                        .AppendEquation("l_B")
                        .AppendText(":")
                        .AddCell($"{_hedi.FirstTubePlate.lB} мм");
                }

                if (_hedi.FirstTubePlate.FixTubeInTubePlate == FixTubeInTubePlateType.OnlyWelding ||
                    _hedi.FirstTubePlate.FixTubeInTubePlate == FixTubeInTubePlateType.RollingWithWelding)
                {
                    table.AddRow()
                        .AddCell("Высота сварного шва в месте приварки трубы к решетке, ")
                        .AppendEquation("δ")
                        .AppendText(":")
                        .AddCell($"{_hedi.FirstTubePlate.delta} мм");
                }


                table.AddRowWithOneCell("Условия нагружения");


                table.AddRow()
                    .AddCell("Межтрубное пространство")
                    .AddCell("");

                table.AddRow()
                    .AddCell("Расчетное давление, ")
                    .AppendEquation("p_M")
                    .AppendText(":")
                    .AddCell($"{_hedi.pM} МПа");

                table.AddRow()
                    .AddCell("Расчетна температура, ")
                    .AppendEquation("T_K")
                    .AppendText(":")
                    .AddCell($"{_hedi.TK} °C");

                table.AddRow()
                    .AddCell("Средняя температура, ")
                    .AppendEquation("t_K")
                    .AppendText(":")
                    .AddCell($"{_hedi.tK} °C");

                table.AddRow()
                    .AddCell("Трубное пространство")
                    .AddCell("");

                table.AddRow()
                    .AddCell("Расчетное давление, ")
                    .AppendEquation("p_T")
                    .AppendText(":")
                    .AddCell($"{_hedi.pT} МПа");

                table.AddRow()
                    .AddCell("Расчетна температура, ")
                    .AppendEquation("T_T")
                    .AppendText(":")
                    .AddCell($"{_hedi.TT} °C");

                table.AddRow()
                    .AddCell("Средняя температура, ")
                    .AppendEquation("t_T")
                    .AppendText(":")
                    .AddCell($"{_hedi.tT} °C");

                table.AddRow()
                    .AddCell("Температура сборки аппарата, ")
                    .AppendEquation("t_0")
                    .AppendText(":")
                    .AddCell($"{_hedi.t0} °C");

                table.AddRow()
                    .AddCell("Число циклов нагружения за расчетный срок службы, N:")
                    .AddCell($"{_hedi.N}");



                table.AddRowWithOneCell("Прочностные характеристики материалов");


                table.AddRow()
                    .AddCell("Кожух")
                    .AddCell("");

                table.AddRow()
                    .AddCell("Материал:")
                    .AddCell($"{_hedi.SteelK}");

                table.AddRow()
                    .AddCell("Допускаемое напряжение при расчетной температуре, ")
                    .AppendEquation("[σ]_K")
                    .AppendText(":")
                    .AddCell($"{_sigma_dK} МПа");

                table.AddRow()
                    .AddCell("Модуль продольной упругости при расчетной температуре, ")
                    .AppendEquation("E_K")
                    .AppendText(":")
                    .AddCell($"{_EK} МПа");

                table.AddRow()
                    .AddCell("Коэффициент линейного расширения, ")
                    .AppendEquation("α_K")
                    .AppendText(":")
                    .AddCell($"{_alfaK} МПа")
                    .AppendEquation("°C^-1");

                table.AddRow()
                    .AddCell("Допускаемая амплитуда напряжений, ")
                    .AppendEquation("[σ_a]_K")
                    .AppendText(":")
                    .AddCell($"{_sigmaa_dK:f2} МПа");


                table.AddRow()
                    .AddCell("Трубы")
                    .AddCell("");

                table.AddRow()
                    .AddCell("Материал:")
                    .AddCell($"{_hedi.SteelT}");

                table.AddRow()
                    .AddCell($"Допускаемое напряжение при расчетной температуре, ")
                    .AppendEquation("[σ]_T")
                    .AppendText(":")
                    .AddCell($"{_sigma_dT} МПа");

                table.AddRow()
                    .AddCell("Модуль продольной упругости при расчетной температуре, ")
                    .AppendEquation("E_T")
                    .AppendText(":")
                    .AddCell($"{_ET} МПа");

                table.AddRow()
                    .AddCell("Коэффициент линейного расширения, ")
                    .AppendEquation("α_T")
                    .AppendText(":")
                    .AddCell($"{_alfaT} МПа")
                    .AppendEquation("°C^-1");

                table.AddRow()
                    .AddCell("Допускаемая амплитуда напряжений, ")
                    .AppendEquation("[σ_a]_T")
                    .AppendText(":")
                    .AddCell($"{_sigmaa_dT:f2} МПа");

                table.AddRow()
                    .AddCell("Первая трубная решетка")
                    .AddCell("");

                table.AddRow()
                    .AddCell("Материал:")
                    .AddCell($"{_hedi.FirstTubePlate.Steelp}");

                table.AddRow()
                    .AddCell($"Допускаемое напряжение при расчетной температуре, ")
                    .AppendEquation("[σ]_p")
                    .AppendText(":")
                    .AddCell($"{_sigma_dp} МПа");

                table.AddRow()
                    .AddCell("Модуль продольной упругости при расчетной температуре, ")
                    .AppendEquation("E_p")
                    .AppendText(":")
                    .AddCell($"{_Ep} МПа");

                table.AddRow()
                    .AddCell("Допускаемая амплитуда напряжений, ")
                    .AppendEquation("[σ_a]_p")
                    .AppendText(":")
                    .AddCell($"{_sigmaa_dp:f2} МПа");

                table.AddRow()
                    .AddCell("")
                    .AddCell("Фланец кожуха");

                table.AddRow()
                    .AddCell("Материал:")
                    .AddCell($"{_hedi.FirstTubePlate.Steel1}");

                table.AddRow()
                    .AddCell("Модуль продольной упругости при расчетной температуре, ")
                    .AppendEquation("E_1")
                    .AppendText(":")
                    .AddCell($"{_E1} МПа");

                table.AddRow()
                    .AddCell("")
                    .AddCell("Фланец камеры");

                table.AddRow()
                    .AddCell("Материал:")
                    .AddCell($"{_hedi.FirstTubePlate.Steel2}");

                table.AddRow()
                    .AddCell("Модуль продольной упругости при расчетной температуре, ")
                    .AppendEquation("E_2")
                    .AppendText(":")
                    .AddCell($"{_E2} МПа");

                table.AddRow()
                    .AddCell("")
                    .AddCell("Смежный элемент фланца камеры");

                table.AddRow()
                    .AddCell("Материал:")
                    .AddCell($"{_hedi.FirstTubePlate.SteelD}");

                table.AddRow()
                    .AddCell("Модуль продольной упругости при расчетной температуре, ")
                    .AppendEquation("E_D")
                    .AppendText(":")
                    .AddCell($"{_ED} МПа");
                



                body.InsertTable(table);
            }



            body.AddParagraph("Результаты расчета").Alignment(AlignmentType.Center);

            body.AddParagraph("Вспомогательные величины").Alignment(AlignmentType.Center);

            body.AddParagraph("Относительную характеристику беструбного края трубной решетки вычисляют по формуле");

            body.AddParagraph("")
                .AppendEquation($"m_n=a/a_1={_a}/{_hedi.a1}={_mn:f2}");

            body.AddParagraph("Коэффициенты влияния давления на трубную решетку вычисляют по формулам:");
            body.AddParagraph("- со стороны межтрубного пространства");
            body.AddParagraph("")
                .AppendEquation($"η_M=1-(i∙d_T^2)/(4∙a_1^2)=1-({_hedi.i}∙{_hedi.dT}^2)/(4∙{_hedi.a1}^2)={_etaM:f2}");
            body.AddParagraph("- со стороны трубного пространства");
            body.AddParagraph("")
                .AppendEquation($"η_T=1-(i∙(d_T-2∙s_T)^2)/(4∙a_1^2)=1-({_hedi.i}∙({_hedi.dT}-{_hedi.sT})^2)/(4∙{_hedi.a1}^2)={_etaT:f2}");
            body.AddParagraph("Основные характеристики жесткости элементов теплообменного аппарата");
            body.AddParagraph("Модуль упругости основания (системы труб) вычисляют по формуле");
            body.AddParagraph("")
                .AppendEquation($"K_y=(E_T∙(η_T-η_M))/l=({_ET}∙({_etaT:f2}-{_etaM:f2}))/{_hedi.l}={_Ky:f2}");
            body.AddParagraph("Приведенное отношение жесткости труб к жесткости кожуха вычисляют по формуле");
            body.AddParagraph("")
                .AppendEquation($"ρ=(K_y∙a_1∙l)/(E_K∙s_K)=({_Ky:f2}∙{_hedi.a1}∙{_hedi.l})/({_EK}∙{_hedi.sK})={_ro:f2}");
            body.AddParagraph("Коэффициенты изменения жесткости системы трубы — кожух вычисляют по формулам:");
            body.AddParagraph("")
                .AppendEquation("K_q=1+K_q^*");
            body.AddParagraph("")
                .AppendEquation("К_р=1+К_p^*");

            switch (_hedi.CompensatorType)
            {
                case CompensatorType.No:
                    body.AddParagraph("Для аппаратов с неподвижными трубными решетками")
                        .AppendEquation("K_q^*=K_p^*=0");
                    break;
                case CompensatorType.Compensator:
                    body.AddParagraph("Для аппаратов с компенсатором на кожухе");
                    body.AddParagraph("")
                        .AppendEquation($"K_q^*=(π∙a∙E_K∙s_K)/(l∙K_ком)={_Kqz:f2}");
                    body.AddParagraph("")
                        .AppendEquation($"K_p^*=(π∙(D_ком^2-d_ком^2)∙E_K∙s_K)/(4.8∙l∙a∙K_ком)={_Kpz:f2}");

                    if (_hedi.IsNeedKcompensatorCalculate)
                    {
                        //TODO: Get calculate Kkom
                    }
                    break;
                case CompensatorType.Expander:
                    //TODO:
                    if (_hedi.beta0 == 90)
                    {

                    }
                    else if (_hedi.beta0 >= 15 && _hedi.beta0 <= 60)
                    {

                    }
                    break;
                case CompensatorType.CompensatorOnExpander:
                    //TODO: Make calculation compensator on expander 
                    break;
            }

            body.AddParagraph("")
                .AppendEquation($"K_q=1+{_Kqz:f2}={_Kq:f2}");
            body.AddParagraph("")
                .AppendEquation($"К_р=1+{_Kpz:f2}={_Kp:f2}");

            body.AddParagraph("Коэффициент ослабления трубной решетки при расчете кожухотрубчатых теплообменных аппаратов с неподвижными трубными решетками и компенсатором на кожухе вычисляют по формуле");
            body.AddParagraph("")
                .AppendEquation($"φ_p=1-d_0/t_p=1-{_hedi.d0}/{_hedi.tp}={_fip:f2}");

            body.AddParagraph("Коэффициент жесткости перфорированной плиты вычисляют по формуле");
            body.AddParagraph("")
                .AppendEquation($"ψ_0=η_T^(7/3)={_psi0:f2}");

            body.AddParagraph("Коэффициент системы решетка — трубы" +
                (_hedi.IsDifferentTubePlate ? " для теплообменных аппаратов с двумя отличающимися друг от друга по толщине или модулю упругости решетками" : " ") +
                "вычисляют по формуле");

            if (!_hedi.IsDifferentTubePlate)
            {
                body.AddParagraph("")
                    .AppendEquation($"β=1.82/s_p∙∜((K_y∙s_p)/(ψ_0∙E_p))=1.82/{_hedi.FirstTubePlate.sp}∙∜(({_Ky:f2}∙{_hedi.FirstTubePlate.sp})/({_psi0:f2}∙{_Ep}))={_beta:f2}");
            }
            else
            {
                body.AddParagraph("")
                    .AppendEquation($"β=1.53∙∜(K_y/ψ_0∙(1/(E_p1∙s_p1^3)+1/(E_p2∙s_p2^3))" +
                    $"=1.53∙∜({_Ky:f2}/{_psi0:f2}∙(1/({_Ep}∙{_hedi.FirstTubePlate.sp}^3)+1/({_Ep2}∙{_hedi.SecondTubePlate.sp}^3))={_beta:f2}");
            }

            body.AddParagraph("Безразмерный параметр системы решетка — трубы вычисляют по формуле");
            body.AddParagraph("")
                .AppendEquation($"ω=β∙a_1={_beta:f2}∙{_hedi.a1}={_omega:f2}");

            body.AddParagraph("Коэффициент системы кожух — решетка");
            body.AddParagraph("")
                .AppendEquation($"β_1=1.3/√(a∙s_1)=1.3/√({_a}∙{_hedi.FirstTubePlate.s1})={_beta1:f2}");

            body.AddParagraph("Коэффициент системы обечайка — фланец камеры");
            body.AddParagraph("")
                .AppendEquation($"β_2=1.3/√(a∙s_2)=1.3/√({_a}∙{_hedi.FirstTubePlate.s2})={_beta2:f2}");

            body.AddParagraph("Коэффициент жесткости фланцевого соединения при изгибе");
            body.AddParagraph("")
                .AppendEquation("K_Φ=K_Φ1+K_Φ2");

            body.AddParagraph("")
                .AppendEquation("K_Φ1=(E_1∙h_1^3∙b_1)/(12∙R_1^2)+K_1∙(1+(β_1∙h_1)/2)");

            body.AddParagraph("")
                .AppendEquation($"K_1=(β_1∙a∙E_K∙s_1^3)/(5.5∙R_1)=({_beta1:f2}∙{_a}∙{_EK}∙{_hedi.FirstTubePlate.s1}^3)/(5.5∙{_R1:f2})={_K1:f2}");

            body.AddParagraph("")
                .AppendEquation($"K_Φ1=({_E1}∙{_hedi.FirstTubePlate.h1}^3∙{_b1:f2})/(12∙{_R1:f2}^2)+{_K1:f2}∙(1+({_beta1:f2}∙{_hedi.FirstTubePlate.h1:f2})/2)={_KF1:f2}");

            body.AddParagraph("")
                .AppendEquation("K_Φ2=(E_2∙h_2^3∙b_2)/(12∙R_2^2)+K_2∙(1+(β_2∙h_2)/2)");

            body.AddParagraph("")
                .AppendEquation($"K_2=(β_2∙a∙E_D∙s_2^3)/(5.5∙R_2)=({_beta2:f2}∙{_a}∙{_ED}∙{_hedi.FirstTubePlate.s2}^3)/(5.5∙{_R2:f2})={_K2:f2}");

            body.AddParagraph("")
                .AppendEquation($"K_Φ1=({_E2}∙{_hedi.FirstTubePlate.h2}^3∙{_b2:f2})/(12∙{_R2:f2}^2)+{_K2:f2}∙(1+({_beta2:f2}∙{_hedi.FirstTubePlate.h2:f2})/2)={_KF2:f2}");

            body.AddParagraph("")
                .AppendEquation($"K_Φ={_KF1:f2}+{_KF2:f2}={_KF:f2}");

            body.AddParagraph("");

            body.AddParagraph("Определение усилий в элементах теплообменного аппарата").Alignment(AlignmentType.Center);

            body.AddParagraph("Приведенное давление ")
                .AppendEquation("p_0")
                .AddRun(" вычисляют по формуле");

            body.AddParagraph("")
                .AppendEquation("p_0=[α_K∙(t_K-t_0)-α_T∙(t_T-t_0)∙K_y∙l+[η_T-1+m_cp+m_n∙(m_n+0.5∙ρ∙K_q)]∙p_T-[η_M-1+m_cp+m_n∙(m_n+0.3∙ρ∙K_p)]∙p_M]");

            body.AddParagraph("где ")
                .AppendEquation("m_cp")
                .AddRun(" - коэффициент влияния давления на продольную деформацию труб");

            body.AddParagraph("")
                .AppendEquation($"m_cp=0.15∙(i∙(d_T-s_T)^2)/a_1^2=0.15∙({_hedi.i}∙({_hedi.dT}-{_hedi.sT})^2)/{_hedi.a1}^2={_mcp:f2}");

            body.AddParagraph("")
                .AppendEquation($"p_0=[{_alfaK}∙({_hedi.tK}-{_hedi.t0})-{_alfaT}∙({_hedi.tT}-{_hedi.t0})∙{_Ky:f2}∙{_hedi.l}+[{_etaT:f2}-1+{_mcp:f2}+{_mn:f2}∙({_mn:f2}+0.5∙{_ro:f2}∙{_Kq:f2})]∙{_hedi.pT}-[{_etaM:f2}-1+{_mcp:f2}+{_mn:f2}∙({_mn:f2}+0.3∙{_ro:f2}∙{_Kp:f2})]∙{_hedi.pM}]={_p0:f2} МПа");

            body.AddParagraph("Приведенное отношение жесткости труб к жесткости фланцевого соединения вычисляют по формуле");

            body.AddParagraph("")
                .AppendEquation($"ρ_1=(K_y∙a∙a_1)/(β_2∙K_Φ∙R_1)=({_Ky:f2}∙{_a}∙{_hedi.a1})/({_beta2:f2}∙{_KF:f2}∙{_R1:f2})={_ro1:f2}");

            body.AddParagraph("Коэффициенты, учитывающие поддерживающее влияние труб ")
                .AppendEquation($"Φ_1={_Phi1}, Φ_2={_Phi2}, Φ_3={_Phi3}")
                .AddRun(" определяют по таблице 1 ГОСТ 34233.7");

            body.AddParagraph("")
                .AppendEquation("T_1=Φ_1∙[m_n+0.5∙(1+m_n∙t)(t-1)]");
            body.AddParagraph("")
                .AppendEquation("T_2=Φ_2∙t");
            body.AddParagraph("")
                .AppendEquation("T_3=Φ_3∙m_n");

            body.AddParagraph("")
                .AppendEquation($"t=1+1.4∙ω∙(m_n-1)=1+1.4∙{_omega:f2}∙({_mn:f2}-1)={_t:f2}");

            body.AddParagraph("")
                .AppendEquation($"T_1={_Phi1:f2}∙[{_mn:f2}+0.5∙(1+{_mn:f2}∙{_t:f2})({_t:f2}-1)]={_T1:f2}");
            body.AddParagraph("")
                .AppendEquation($"T_2={_Phi2:f2}∙{_t:f2}={_T2:f2}");
            body.AddParagraph("")
                .AppendEquation($"T_3={_Phi3:f2}∙{_mn:f2}={_T3:f2}");

            body.AddParagraph("Изгибающий момент и перерезывающую силу, распределенные по краю трубной решетки, вычисляют по формулам:");
            body.AddParagraph("- для изгибающего момента");

            body.AddParagraph("")
                .AppendEquation("M_Π=(a_1/β)∙(p_1∙(T_1+ρ∙K_q)-p_0∙T_2)/((T_1+ρ∙K_q)∙(T_3+ρ_1)-T_2^2)");

            body.AddParagraph("- для перерезывающей силы");

            body.AddParagraph("")
                .AppendEquation("Q_Π=a_1∙(p_0∙(T_3+ρ_1)-p_1∙T_2)/((T_1+ρ∙K_q)∙(T_3+ρ_1)-T_2^2)");

            body.AddParagraph("где");

            body.AddParagraph("")
                .AppendEquation("p_1=K_y/(β∙K_Φ)∙(m_1∙p_M-m_2∙p_T)");

            body.AddParagraph("")
                .AppendEquation($"m_1=(1-β_1∙h_1)/(2∙β_1^2)=(1-{_beta1:f2}∙{_hedi.FirstTubePlate.h1})/(2∙{_beta1:f2}^2)={_m1:f2}");

            body.AddParagraph("")
                .AppendEquation($"m_2=(1-β_2∙h_2)/(2∙β_2^2)=(1-{_beta2:f2}∙{_hedi.FirstTubePlate.h2})/(2∙{_beta2:f2}^2)={_m2:f2}");

            body.AddParagraph("")
                .AppendEquation($"p_1={_Ky:f2}/({_beta:f2}∙{_KF:f2})∙({_m1:f2}∙{_hedi.pM}-{_m2:f2}∙{_hedi.pT})={_p1:f2}");

            body.AddParagraph("")
                .AppendEquation($"M_Π=({_hedi.a1}/{_beta:f2})∙({_p1:f2}∙({_T1:f2}+{_ro:f2}∙{_Kq:f2})-{_p0:f2}∙{_T2:f2})/(({_T1:f2}+{_ro:f2}∙{_Kq:f2})∙({_T3:f2}+{_ro1:f2})-{_T2:f2}^2)={_MP:f2} (Н∙мм)/мм");

            body.AddParagraph("")
                .AppendEquation($"Q_Π={_hedi.a1}∙({_p0:f2}∙({_T3:f2}+{_ro1:f2})-{_p1:f2}∙{_T2:f2})/(({_T1:f2}+{_ro:f2}∙{_Kq:f2})∙({_T3:f2}+{_ro1:f2})-{_T2:f2}^2)={_QP:f2} H");

            body.AddParagraph("Изгибающий момент и перерезывающие силы, распределенные по периметру перфорированной зоны решетки, вычисляют по формулам:");
            body.AddParagraph("- для изгибающего момента");
            body.AddParagraph("")
                .AppendEquation($"M_a=M_Π+(a-a_1)∙Q_Π={_MP:f2}+({_a}-{_hedi.a1})={_Ma:f2} (Н∙мм)/мм");

            body.AddParagraph("- для перерезывающей силы");
            body.AddParagraph("")
                .AppendEquation($"Q_a={_mn:f2}∙{_QP:f2}={_Qa:f2} H");

            body.AddParagraph("Осевую силу и изгибающий момент, действующие на трубу, вычисляют по формулам:");

            body.AddParagraph("- для осевой силы");
            body.AddParagraph("")
                .AppendEquation("N_T=(π∙a_1)/i∙[(η_M∙p_M-η_T∙p_T)∙a_1+Φ_1∙Q_a+Φ_2∙β∙M_a]" +
                $"=(π∙{_hedi.a1})/{_hedi.i}∙[({_etaM:f2}∙{_hedi.pM}-{_etaT:f2}∙{_hedi.pT})∙{_hedi.a1}+{_Phi1:f2}∙{_Qa:f2}+{_Phi2:f2}∙{_beta:f2}∙{_Ma:f2}]={_NT:f2} H");

            body.AddParagraph("- для изгибающего момента");
            body.AddParagraph("")
                .AppendEquation("M_T=(E_T∙J_T∙β)/(K_y∙a_1∙l_пр)∙(Φ_2∙Q_a+Φ_3∙β∙M_a)");

            body.AddParagraph("где ")
                .AppendEquation("l_пр")
                .AddRun(_hedi.IsWithPartitions ? " для аппаратов с перегородками в кожухе" : " для аппаратов без перегородок в кожухе");

            if (!_hedi.IsWithPartitions)
            {
                body.AddParagraph("")
                .AppendEquation($"l_пр=l={_hedi.l} мм");
            }
            else
            {
                body.AddParagraph("")
                .AppendEquation($"l_пр=L_1R/3={_hedi.l1R}/3={_lpr:f2} мм");
            }

            body.AddParagraph("")
                .AppendEquation("J_T")
                .AddRun(" - момент инерции поперечного сечения трубы");

            body.AddParagraph("")
                .AppendEquation($"J_T=(π∙d_T^4-(d_T-2∙s_T)^4)/64=(π∙{_hedi.dT}^4-({_hedi.dT}-2∙{_hedi.sT})^4)/64={_JT:f2} мм^4");

            body.AddParagraph("")
                .AppendEquation($"M_T=({_ET}∙{_JT:f2}∙{_beta:f2})/({_Ky:f2}∙{_hedi.a1}∙{_lpr:f2})∙({_Phi2:f2}∙{_Qa:f2}+{_Phi3:f2}∙{_beta:f2}∙{_Ma:f2})={_MT:f2} (Н∙мм)/мм");

            body.AddParagraph("Нагрузки на кожух вычисляют по формулам:");

            body.AddParagraph("- усилие, распределенное по периметру кожуха");
            body.AddParagraph("")
                .AppendEquation($"Q_K=a/2∙p_T-Q_Π={_a}/2∙{_hedi.pT}-{_QP:f2}={_QK:f2} H");

            body.AddParagraph("- изгибающий момент, распределенный по периметру кожуха");
            body.AddParagraph("")
                .AppendEquation("M_K=K_1/(ρ_1∙K_Φ∙β)∙(T_2∙Q_Π+T_3∙β∙M_Π)-p_M/(2∙β_1^2) +" +
                $"{_K1:f2}/({_ro1:f2}∙{_KF:f2}∙{_beta:f2})∙({_T2:f2}∙{_QP:f2}+{_T3:f2}∙{_beta:f2}∙{_MP:f2})-{_hedi.pM}/(2∙{_beta1:f2}^2)={_MK:f2} (Н∙мм)/мм");

            body.AddParagraph("- суммарная осевая сила, действующая на кожух");
            body.AddParagraph("")
                .AppendEquation($"F=π∙D∙Q_K=π∙{_hedi.D}∙{_QK:f2}={_F:f2} H");

            body.AddParagraph("");

            body.AddParagraph("Расчетные напряжения в элементах конструкции").Alignment(AlignmentType.Center);

            body.AddParagraph("Крепления трубной решетки к кожуху или фланцу");


            {
                var imagePart = mainPart.AddImagePart(ImagePartType.Gif);

                string type1 = ((int)_hedi.FirstTubePlate.TubePlateType).ToString();

                var b = (byte[])Data.Properties.Resources.ResourceManager.GetObject("ConnToFlange" + type1 + "Dim");

                if (b != null)
                {
                    imagePart.FeedData(new MemoryStream(b));

                    body.AddParagraph("").AddImage(mainPart.GetIdOfPart(imagePart), b);
                }
            }

            body.AddParagraph("Расчетные напряжения в трубных решетках").Alignment(AlignmentType.Center);

            body.AddParagraph("Напряжения в трубной решетке в месте соединения с кожухом вычисляют по формулам:");

            body.AddParagraph("- изгибные");
            body.AddParagraph("")
                .AppendEquation($"σ_p1=(6∙|M_Π|)/(s_1p-c)^2=(6∙|{_MP:f2}|)/({_hedi.FirstTubePlate.s1p}-{_hedi.FirstTubePlate.c})^2={_sigmap1:f2} МПа");

            body.AddParagraph("- касательные");
            body.AddParagraph("")
                .AppendEquation($"τ_p1=|Q_Π|/(s_1p-c)=|{_QP:f2}|/({_hedi.FirstTubePlate.s1p}-{_hedi.FirstTubePlate.c})={_taup1:f2} МПа");

            body.AddParagraph("Напряжения в перфорированной части трубной решетки вычисляют по формулам:");

            body.AddParagraph("- изгибные");
            body.AddParagraph("")
                .AppendEquation($"σ_p2=(6∙M_max)/(φ_p∙(s_1p-c)^2)");

            body.AddParagraph("где ")
                .AppendEquation("M_max")
                .AddRun(" максимальный расчетный изгибающий момент в перфорированной части трубной решетки");

            if (_mA >= -1.0 && _mA <= 1.0)
            {
                body.AddParagraph("При");

                body.AddParagraph("")
                    .AppendEquation($"-1.0≤(β∙M_a)/Q_a=({_beta:f2}∙{_Ma:f2})/{_Qa:f2}={_mA:f2}≤1.0");

                body.AddParagraph("")
                    .AppendEquation("M_max=A∙|Q_a|/β");

                body.AddParagraph($"где A={_A} - коэффициент, определяемый по таблице Г.2 ГОСТ 34233.7");

                body.AddParagraph("")
                    .AppendEquation($"M_max={_A}∙|{_Qa:f2}|/{_beta:f2}={_Mmax:f2} (Н∙мм)/мм");
            }
            else
            {
                body.AddParagraph("При");

                body.AddParagraph("")
                    .AppendEquation($"-(β∙M_a)/Q_a=({_beta:f2}∙{_Ma:f2})/{_Qa:f2}={_mA:f2}" +
                    (_mA < -1.0 ? "<-1.0" : ">1.0"));

                body.AddParagraph("")
                    .AppendEquation("M_max=B∙|M_a|");

                body.AddParagraph($"где B={_B} - коэффициент, определяемый по таблице Г.2 ГОСТ 34233.7");

                body.AddParagraph("")
                    .AppendEquation($"M_max={_B}∙|{_Ma:f2}|/{_beta:f2}={_Mmax:f2} (Н∙мм)/мм");
            }

            body.AddParagraph("")
                .AppendEquation($"σ_p2=(6∙{_Mmax:f2})/({_fip:f2}∙({_hedi.FirstTubePlate.s1p}-{_hedi.FirstTubePlate.c})^2)={_sigmap2:f2} МПа");

            body.AddParagraph("- касательные");
            body.AddParagraph("")
                .AppendEquation($"τ_p2=|Q_a|/(φ_p∙(s_1p-c))=|{_QP:f2}|/({_fip:f2}∙({_hedi.FirstTubePlate.s1p}-{_hedi.FirstTubePlate.c}))={_taup2:f2} МПа");

            body.AddParagraph("Напряжения в кожухе в месте присоединения к решетке вычисляют по формулам:");

            body.AddParagraph("- в меридиональном направлении:");

            body.AddParagraph("мембранные");
            body.AddParagraph("")
                .AppendEquation($"σ_MX=|Q_K|/(s_1-c_K)=|{_QK:f2}|/({_hedi.FirstTubePlate.s1}-{_hedi.cK})={_sigmaMX:f2} МПа");

            body.AddParagraph("изгибные");
            body.AddParagraph("")
                .AppendEquation($"σ_ИX=(6∙|M_K|)/(s_1-c_K)^2=(6∙|{_MK:f2}|)/({_hedi.FirstTubePlate.s1}-{_hedi.cK})^2={_sigmaIX:f2} МПа");

            body.AddParagraph("- в окружном направлении:");

            body.AddParagraph("мембранные");
            body.AddParagraph("")
                .AppendEquation($"σ_Mφ=(|p_M|∙a)/(s_1-c_K)=(|{_hedi.pM}|∙{_a})/({_hedi.FirstTubePlate.s1}-{_hedi.cK})={_sigmaMfi:f2} МПа");

            body.AddParagraph("изгибные");
            body.AddParagraph("")
                .AppendEquation($"σ_Иφ=0.3∙σ_ИX=0.3∙{_sigmaIX:f2}={_sigmaIfi:f2} МПа");

            body.AddParagraph("Напряжения в трубах вычисляют по формулам:");

            body.AddParagraph("- в осевом направлении:");

            body.AddParagraph("мембранные");
            body.AddParagraph("")
                .AppendEquation($"σ_1T=|N_T|/(π∙(d_T-s_T)∙s_T)=|{_NT:f2}|/(π∙({_hedi.dT}-{_hedi.sT})∙{_hedi.sT})={_sigma1T:f2} МПа");

            body.AddParagraph("суммарные");
            body.AddParagraph("")
                .AppendEquation($"σ_1=σ_1T+(d_T∙|M_T|)/(2∙J_T)={_sigma1T:f2}+({_hedi.dT}∙|{_MT:f2}|)/(2∙{_JT:f2})={_sigma1:f2} МПа");

            body.AddParagraph("- в окружном направлении");
            body.AddParagraph("")
                .AppendEquation($"σ_2T=((d_T-s_T)∙max{{|p_T|;|p_M|;|p_T-p_M|}})/(2∙s_T)=(({_hedi.dT}-{_hedi.sT})∙max{{{_hedi.pT};{_hedi.pM};{Math.Abs(_hedi.pT - _hedi.pM)}}})/(2∙{_hedi.sT})={_sigma2T:f2} МПа");

            body.AddParagraph("");

            body.AddParagraph("Проверка прочности трубных решеток").Alignment(AlignmentType.Center);

            body.AddParagraph("Проверку статической прочности проводят по формуле");

            body.AddParagraph("")
                .AppendEquation("max{τ_p1;τ_p2}≤0.8∙[σ]_p");

            body.AddParagraph("")
                .AppendEquation($"max{{{_taup1:f2};{_taup2:f2}}}≤0.8∙{_sigma_dp}");

            body.AddParagraph("")
                .AppendEquation($"{_conditionStaticStressForTubePlate1:f2}≤{_conditionStaticStressForTubePlate2}");

            if (_isConditionStaticStressForTubePlate)
            {
                body.AddParagraph("Условие прочности выполняется")
                    .Bold();
            }
            else
            {
                body.AddParagraph("Условие прочности не выполняется")
                    .Bold()
                    .Color(System.Drawing.Color.Red);
            }

            body.AddParagraph("Проверку трубной решетки на малоцикловую прочность проводят по ГОСТ 34233.6");

            body.AddParagraph("- в месте соединения с кожухом");

            body.AddParagraph("")
                .AppendEquation($"Δσ_1=σ_p1={_sigmap1:f2} МПа, Δσ_2=Δσ_3=0, K_σ={_Ksigma}");

            MakeWordSigmaa(_deltasigma1pk, _deltasigma2pk, _deltasigma3pk, _Ksigma, _sigmaa_dp, _sigmaa_5_2_4k, ref body);

            body.AddParagraph("- в перфорированной части");

            body.AddParagraph("")
                .AppendEquation($"Δσ_1=σ_p2={_sigmap2:f2} МПа, Δσ_2=Δσ_3=0, K_σ=1");

            MakeWordSigmaa(_deltasigma1pp, _deltasigma2pp, _deltasigma3pp, 1, _sigmaa_dp, _sigmaa_5_2_4p, ref body);

            if (!_hedi.IsOneGo && _hedi.FirstTubePlate.IsWithGroove)
            {
                body.AddParagraph("Для многоходовых по трубному пространству теплообменных аппаратов прочность трубных решеток в зоне паза под перегородку проверяют по формуле");

                body.AddParagraph("")
                    .AppendEquation("s_n≥(s_p-c)∙max{[1-√(d_0/B_Π∙(t_Π/t_p-1))];√(φ_p)}+c");

                body.AddParagraph("")
                    .AppendEquation($"1-√(d_0/B_Π∙(t_Π/t_p-1))=1-√({_hedi.d0}/{_hedi.FirstTubePlate.BP}∙({_hedi.tP}/{_hedi.tp}-1))={_snp1:f2}");

                body.AddParagraph("")
                    .AppendEquation($"√(φ_p)=√({_fip:f2})={_snp2:f2}");


                body.AddParagraph("")
                    .AppendEquation($"s_n=({_hedi.FirstTubePlate.sp}-{_hedi.FirstTubePlate.c})∙max{{{_snp1:f2};{_snp2:f2}}}+{_hedi.FirstTubePlate.c}={_sn:f2} мм");

                if (_sn >= _hedi.FirstTubePlate.sn)
                {
                    body.AddParagraph("Принятая толщина ")
                        .Bold()
                        .AppendEquation($"s_n={_hedi.FirstTubePlate.sn} мм");
                }
                else
                {
                    body.AddParagraph("Принятая толщина ")
                        .Bold()
                        .Color(System.Drawing.Color.Red)
                        .AppendEquation($"s_n={_hedi.FirstTubePlate.sn} мм");
                }
            }

            body.AddParagraph("");

            if (_hedi.FirstTubePlate.IsNeedCheckHardnessTubePlate)
            {
                body.AddParagraph("Проверка жесткости трубных решеток").Alignment(AlignmentType.Center);

                body.AddParagraph("Проверку проводят, когда к жесткости трубных решеток предъявляются какие - либо дополнительные требования, например для аппаратов со стекающей пленкой, с перегородками по трубному пространству, если недопустим переток между ходами.");

                body.AddParagraph("Условие жесткости:");
                body.AddParagraph("")
                    .AppendEquation("W≤[W]");

                body.AddParagraph("")
                    .AppendEquation($"W=1.2/(K_y∙a_1)∙|T_1∙Q_Π+T_2∙β∙M_Π|=1.2/({_Ky:f2}∙{_hedi.a1})∙|{_T1:f2}∙{_QP:f2}+{_T2:f2}∙{_beta:f2}∙{_MP:f2}|={_W:f2} мм");

                body.AddParagraph($"[W]={_W_d} мм - допустимоее значение прогиба трубной решетки по таблице 2 ГОСТ 34233.7");

                body.AddParagraph("")
                    .AppendEquation($"{_W:f2} мм ≤{_W_d} мм");

                if (_W <= _W_d)
                {
                    body.AddParagraph("Условие жесткости выполняется")
                        .Bold();
                }
                else
                {
                    body.AddParagraph("Условие жесткости не выполняется")
                        .Bold()
                        .Color(System.Drawing.Color.Red);
                }
            }

            if (_hedi.FirstTubePlate.TubePlateType != TubePlateType.WeldedInFlange &
                _hedi.SecondTubePlate.TubePlateType != TubePlateType.WeldedInFlange)
            {
                body.AddParagraph("Расчет прочности и устойчивости кожуха").Alignment(AlignmentType.Center);

                body.AddParagraph("Условие статической прочности кожуха в месте присоединения к решетке:");

                body.AddParagraph("")
                    .AppendEquation("σ_MX≤1.3∙[σ]_K");
                body.AddParagraph("")
                    .AppendEquation($"{_sigmaMX:f2}≤1.3∙{_sigma_dK}={_conditionStaticStressForShell2:f2}");

                if (_isConditionStaticStressForShell)
                {
                    body.AddParagraph("Условие прочности выполняется")
                        .Bold();
                }
                else
                {
                    body.AddParagraph("Условие прочности не выполняется")
                        .Bold()
                        .Color(System.Drawing.Color.Red);
                }
            }

            //TODO: Make calculate for F<0

            body.AddParagraph("Проверку кожуха на малоцикловую прочность в месте присоединения к решетке проводят по ГОСТ 34233.6");

            body.AddParagraph("")
                .AppendEquation($"Δσ_1=σ_MX+σ_ИX={_sigmaMX:f2}+{_sigmaIX:f2}={_deltasigma1K:f2} МПа");
            body.AddParagraph("")
                .AppendEquation($"Δσ_2=σ_Mφ+σ_Иφ={_sigmaMfi:f2}+{_sigmaIfi:f2}={_deltasigma2K:f2} МПа");
            body.AddParagraph("")
                .AppendEquation($"Δσ_3={_deltasigma3K:f2} МПа");
            body.AddParagraph("")
                .AppendEquation($"K_σ={_Ksigma}");

            MakeWordSigmaa(_deltasigma1K, _deltasigma2K, _deltasigma3K, _Ksigma, _sigmaa_dK, _sigmaaK, ref body);

            body.AddParagraph("");

            body.AddParagraph("Расчет труб на прочность, устойчивость и жесткость и расчет крепления труб в решетке")
                .Alignment(AlignmentType.Center);

            body.AddParagraph("Условие статической прочности труб:");

            body.AddParagraph("")
                    .AppendEquation("max{σ_1T;σ_2T}≤[σ]_T");
            body.AddParagraph("")
                .AppendEquation($"max{{{_sigma1T:f2};{_sigma2T:f2}}}={_conditionStaticStressForTube1:f2}≤{_sigma_dT}");

            if (_isConditionStaticStressForTube)
            {
                body.AddParagraph("Условие прочности выполняется")
                    .Bold();
            }
            else
            {
                body.AddParagraph("Условие прочности не выполняется")
                    .Bold()
                    .Color(System.Drawing.Color.Red);
            }

            body.AddParagraph("Проверку труб на малоцикловую прочность проводят по ГОСТ 34233.6");

            body.AddParagraph("")
                .AppendEquation($"Δσ_1=σ_1={_deltasigma1T:f2} МПа, Δσ_2=Δσ_3=0 МПа, K_σ=1");

            MakeWordSigmaa(_deltasigma1T, _deltasigma2T, _deltasigma3T, _Ksigma, _sigmaa_dT, _sigmaaT, ref body);

            body.AddParagraph("Проверку труб на устойчивость" + (_NT < 0 ? " " : " не ") + "проводят, т.к. ")
                .AppendEquation($"N_T={_NT:f2}<0");

            if (_NT < 0)
            {
                body.AddParagraph("Условие устойчивости:");

                body.AddParagraph("")
                    .AppendEquation("σ_1T≤φ_T∙[σ]_T");

                body.AddParagraph("где ")
                    .AppendEquation("φ_T")
                    .AddRun(" — коэффициент уменьшения допускаемого напряжения при продольном изгибе");

                body.AddParagraph("")
                    .AppendEquation("φ_T=1/√(1+λ^4)");

                body.AddParagraph("")
                    .AppendEquation("λ=K_T∙√([σ]_T/E_T)∙l_R/((d_T-s_T))");

                body.AddParagraph("где ")
                    .AppendEquation($"K_T={_KT}")
                    .AddRun(_hedi.IsWorkCondition ? " — для рабочих условий" : " - для условий гидроиспытания");

                if (_hedi.IsWithPartitions)
                {
                    body.AddParagraph("")
                    .AppendEquation($"l_R=max{{l_2R;0.7∙l_1R}}=max{{{_hedi.l2R};0.7∙{_hedi.l1R}}}={_lR:f2} мм")
                    .AddRun(" — для аппаратов с перегородками");
                }
                else
                {
                    body.AddParagraph("")
                    .AppendEquation($"l_R=l={_lR} мм")
                    .AddRun(" — для аппаратов без перегородок");
                }


                body.AddParagraph("")
                    .AppendEquation($"λ=K_T∙√([σ]_T/E_T)∙l_R/((d_T-s_T))={_KT}∙√({_sigma_dT}/{_ET})∙{_lR}/(({_hedi.dT}-{_hedi.sT}))={_lambda:f2}");

                body.AddParagraph("")
                    .AppendEquation($"φ_T=1/√(1+{_lambda:f2}^4)={_phiT:f2}");

                body.AddParagraph("")
                    .AppendEquation($"{_sigma1T:f2}≤{_phiT:f2}∙{_sigma_dT}={_conditionStabilityForTube2:f2}");

                if (_isConditionStabilityForTube)
                {
                    body.AddParagraph("Условие устойчивости выполняется")
                        .Bold();
                }
                else
                {
                    body.AddParagraph("Условие устойчивости не выполняется")
                        .Bold()
                        .Color(System.Drawing.Color.Red);
                }

                if (_hedi.IsNeedCheckHardnessTube)
                {
                    body.AddParagraph("");

                    body.AddParagraph("Проверка жесткости труб").Alignment(AlignmentType.Center);

                    body.AddParagraph("Проверку проводят, когда к жесткости труб предъявляют какие-либо дополнительные требования");

                    body.AddParagraph("Прогиб трубы вычисляют по формуле");

                    body.AddParagraph("")
                        .AppendEquation("Y=A_y∙|M_T|/|N_T|");

                    body.AddParagraph("")
                        .AppendEquation("A_y=(1-cos√(λ_y))/(1-cos√(λ_y))");

                    body.AddParagraph("")
                        .AppendEquation($"λ_y=(|N_T|∙l_пр^2/(E_T∙J_T)=(|{_NT:f2}|∙{_lpr:f2}^2/({_ET}∙{_JT:f2})={_lambday:f2}");

                    body.AddParagraph("")
                        .AppendEquation($"A_y=(1-cos√({_lambday:f2}))/(1-cos√({_lambday:f2}))={_Ay:f2}");

                    body.AddParagraph("")
                        .AppendEquation($"Y={_Ay:f2}∙|{_MT:f2}|/|{_NT:f2}|={_Y:f2} мм");

                    body.AddParagraph("о всех случаях прогиб трубы не должен превышать зазор между трубами в пучке и приводить к их соприкосновению.");
                    //TODO: Check Y for compare with something
                }

            }

            body.AddParagraph("");

            body.AddParagraph("Проверка прочности крепления трубы в решетке").Alignment(AlignmentType.Center);

            body.AddParagraph("Допускаемую нагрузку на соединение трубы с решеткой ")
                .AppendEquation("[N]_TP")
                .AddRun(" определяют на основании испытаний или по нормативным документам. При отсутствии данных о прочности вальцовочного соединения вычисляем значение по формуле");

            switch (_hedi.FirstTubePlate.TubeRolling)
            {
                case TubeRollingType.RollingWithoutGroove:
                    body.AddParagraph("- для гладкозавальцованных труб");

                    body.AddParagraph("")
                        .AppendEquation($"[N]_TP=0.5∙π∙s_T∙(d_T-s_T)∙min{{l_B/d_T;1.6}}∙min{{[σ]_T;[σ]_p}}={_Ntp_d:f2} H");
                    break;
                case TubeRollingType.RollingWithOneGroove:
                    body.AddParagraph("- для труб, завальцованных в пазы при наличии одного паза");

                    body.AddParagraph("")
                        .AppendEquation($"[N]_TP=0.6∙π∙s_T∙(d_T-s_T)∙min{{[σ]_T;[σ]_p}}={_Ntp_d:f2} H");
                    break;
                case TubeRollingType.RollingWithMoreThenOneGroove:
                    body.AddParagraph("- для труб, завальцованных в пазы с двумя или более пазами");

                    body.AddParagraph("")
                        .AppendEquation($"[N]_TP=0.8∙π∙s_T∙(d_T-s_T)∙min{{[σ]_T;[σ]_p}}={_Ntp_d:f2} H");
                    break;
            }

            switch (_hedi.FirstTubePlate.FixTubeInTubePlate)
            {
                case FixTubeInTubePlateType.OnlyRolling:
                    body.AddParagraph("Если трубы крепятся в решетке с помощью развальцовки, должно выполняться условие");

                    body.AddParagraph("")
                        .AppendEquation("|N_T|≤[N]_TP");

                    body.AddParagraph("")
                        .AppendEquation($"|{_NT:f2}|≤{_Ntp_d:f2}");

                    break;

                case FixTubeInTubePlateType.OnlyWelding:
                    body.AddParagraph("Если трубы крепятся к решетке способом приварки или приварки с подвальцовкой, должно выполняться условие");

                    body.AddParagraph("")
                        .AppendEquation("(|N_T|∙d_T+4∙M_T)/π∙d_T^2∙δ≤φ_C∙min{[σ]_T;[σ]_p}");

                    body.AddParagraph("")
                        .AppendEquation($"φ_C=min{{0.5;(0.95-0.2∙lgN)}}=min{{0.5;(0.95-0.2∙lg{_hedi.N})}}=min{{0.5;{_phiC2:f2})}}={_phiC:f2}");

                    body.AddParagraph("")
                        .AppendEquation($"(|N_T|∙d_T+4∙M_T)/π∙d_T^2∙δ≤φ_C∙min{{[σ]_T;[σ]_p}}=(|{_NT:f2}|∙{_hedi.dT}+4∙{_MT:f2})/π∙{_hedi.dT}^2∙{_hedi.FirstTubePlate.delta}={_tau:f2}");

                    body.AddParagraph("")
                        .AppendEquation($"φ_C∙min{{[σ]_T;[σ]_p}}={_phiC:f2}∙min{{{_sigma_dT};{_sigma_dp}}}={_conditionStressBracingTube2:f2}");

                    body.AddParagraph("")
                        .AppendEquation($"{_tau:f2}≤{_conditionStressBracingTube2:f2}");

                    break;

                case FixTubeInTubePlateType.RollingWithWelding:
                    body.AddParagraph("В случае крепления труб к решетке способом развальцовки с обваркой должно выполняться условие");

                    body.AddParagraph("")
                        .AppendEquation("max{(φ_C∙min{[σ]_T;[σ]_p})/τ+0.6∙[N]_TP/|N_T|;[N]_TP/|N_T|}≥1");

                    body.AddParagraph("")
                        .AppendEquation("τ=(|N_T|∙d_T+4∙M_T)/π∙d_T^2∙δ≤φ_C∙min{[σ]_T;[σ]_p}");

                    body.AddParagraph("")
                        .AppendEquation($"φ_C=min{{0.5;(0.95-0.2∙lgN)}}=min{{0.5;(0.95-0.2∙lg{_hedi.N})}}=min{{0.5;{_phiC2:f2}}}={_phiC:f2}");

                    body.AddParagraph("")
                        .AppendEquation($"τ=(|{_NT:f2}|∙{_hedi.dT}+4∙{_MT:f2})/π∙{_hedi.dT}^2∙{_hedi.FirstTubePlate.delta}={_tau:f2}");

                    body.AddParagraph("")
                        .AppendEquation($"(φ_C∙min{{[σ]_T;[σ]_p}})/τ+0.6∙[N]_TP/|N_T|=({_phiC:f2}∙min{{{_sigma_dT};{_sigma_dp}}})/{_tau:f2}+0.6∙{_Ntp_d:f2}/|{_NT:f2}|={_conditionStressBracingTube11:f2}");

                    body.AddParagraph("")
                        .AppendEquation($"[N]_TP/|N_T|={_Ntp_d:f2}/|{_NT:f2}|={_conditionStressBracingTube12:f2}");

                    body.AddParagraph("")
                        .AppendEquation($"max{{{_conditionStressBracingTube11:f2};{_conditionStressBracingTube12:f2}}}={Math.Max(_conditionStressBracingTube11, _conditionStressBracingTube12):f2}≥1");
                    break;
            }

            if (_isConditionStressBracingTube)
            {
                body.AddParagraph("Условие прочности выполняется")
                    .Bold();
            }
            else
            {
                body.AddParagraph("Условие прочности не выполняется")
                    .Bold()
                    .Color(System.Drawing.Color.Red);
            }

            body.AddParagraph("");
            body.AddParagraph("При наличии беструбной зоны принятая толщина трубной решетки должна дополнительно удовлетворять условию");

            body.AddParagraph("")
                .AppendEquation("s_p≥0.5∙D_E∙√(p_p/[σ]_p)+c");

            body.AddParagraph("где ")
                .AppendEquation($"p_p={_pp} МПа")
                .AddRun(" - расчетное давление, действующее на решетку кожухотрубчатого теплообменного аппа­рата. Принимается равным максимально возможному перепаду давлений, действую­щих на решетку");

            body.AddParagraph("")
                .AppendEquation($"s_p=0.5∙{_hedi.DE}∙√({_pp}/{_sigma_dp})+{_hedi.FirstTubePlate.c}={_sp_5_5_1:f2} мм");

            if (_sp_5_5_1 >= _hedi.FirstTubePlate.sp)
            {
                body.AddParagraph("Принятая толщина ")
                    .Bold()
                    .AppendEquation($"s_p={_hedi.FirstTubePlate.sp} мм");
            }
            else
            {
                body.AddParagraph("Принятая толщина ")
                    .Bold()
                    .Color(System.Drawing.Color.Red)
                    .AppendEquation($"s_p={_hedi.FirstTubePlate.sp} мм");
            }


            body.AddParagraph("Для трубных решеток, выполненных заодно с фланцем, принятая толщина должна быть не менее толщины кольца ответного фланца.Допускается уменьшение толщины решетки по сравнению с толщиной ответного фланца при условии подтверждения плотности и прочности фланцевого соединения специальным расчетом.");

            if (!_hedi.IsOneGo)
            {
                body.AddParagraph("Перегородки между ходами по трубному пространству кожухотрубчатых теплообменных аппаратов");

                body.AddParagraph("Толщина перегородки должна отвечать условию");
            }

        }

        private static void MakeWordSigmaa(double ds1, double ds2, double ds3, double Ks, double sigmaa_d, double sigmaa, ref Body body)
        {
            body.AddParagraph("Амплитуду напряжений для каждого цикла вычисляют по формуле");
            body.AddParagraph("")
                .AppendEquation("σ_a=K_σ/2∙max{|Δσ_1-Δσ_2|;|Δσ_2-Δσ_3|;|Δσ_1-Δσ_3|}"
                + $"={Ks}/2∙max{{|{ds1:f2}-{ds2:f2}|;|{ds2:f2}-{ds3:f2}|;|{ds1:f2}-{ds3:f2}|}}" +
                $"={Ks}/2∙max{{|{ds1 - ds2:f2}|;|{ds2 - ds3:f2}|;|{ds1 - ds3:f2}|}}={sigmaa:f2}");

            body.AddParagraph("").AppendEquation("[σ_a]≥σ_a");
            body.AddParagraph("")
                .AppendEquation($"{sigmaa_d:f2}≥{sigmaa:f2}");
            if (sigmaa_d > sigmaa)
            {
                body.AddParagraph("Условие малоцикловой прочности выполняется")
                    .Bold();
            }
            else
            {
                body.AddParagraph("Условие малоцикловой прочности не выполняется")
                    .Bold()
                    .Color(System.Drawing.Color.Red);
            }
        }

        private static double CalculateSigmaa(double Ksigma,
                                              double deltasigma1,
                                              double deltasigma2,
                                              double deltasigma3) => Ksigma / 2 * Math.Max(Math.Abs(deltasigma1 - deltasigma2),
                Math.Max(Math.Abs(deltasigma2 - deltasigma3), Math.Abs(deltasigma1 - deltasigma3)));




    }
}
