﻿using CalculateVessels.Core.Bottoms.Enums;
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
        private bool _isConditionStressBracingTube2;
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
        private double _Ep1;
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
        private double _PhiC;
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
            //TODO: Get phisycal parameters
            //ET
            if (!Physical.TryGetE(_hedi.SteelT, _hedi.tT, ref _ET, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }

            //EK
            if (!Physical.TryGetE(_hedi.SteelK, _hedi.tK, ref _EK, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }

            //ED
            if (!Physical.TryGetE(_hedi.SteelD, _hedi.tT, ref _ED, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }

            //Ep
            if (_hedi.IsDifferentTubePlate)
            {
                if (!Physical.TryGetE(_hedi.Steelp1, _hedi.tK, ref _Ep1, ref _errorList))
                {
                    IsCriticalError = true;
                    return;
                }

                if (!Physical.TryGetE(_hedi.Steelp2, _hedi.tK, ref _Ep2, ref _errorList))
                {
                    IsCriticalError = true;
                    return;
                }
            }
            else
            {
                if (!Physical.TryGetE(_hedi.Steelp, _hedi.tK, ref _Ep, ref _errorList))
                {
                    IsCriticalError = true;
                    return;
                }
            }

            //TODO: Get E1 for condition if two different tube piate and flange not with tube plate
            //E1
            if (true)
            {
                _E1 = _Ep;
            }
            else
            {
                if (!Physical.TryGetE(_hedi.Steel1, _hedi.tK, ref _E1, ref _errorList))
                {
                    IsCriticalError = true;
                    return;
                }
            }

            //E2         
            if (!Physical.TryGetE(_hedi.Steel2, _hedi.tK, ref _E2, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }

            //Ekom
            if (_hedi.IsNeedKcompensatorCalculate)
            {
                if (!Physical.TryGetE(_hedi.Steelkom, _hedi.tK, ref _Ekom, ref _errorList))
                {
                    IsCriticalError = true;
                    return;
                }
            }

            //[]p
            if (Physical.Gost34233_1.TryGetSigma(_hedi.Steelp, _hedi.tK, ref _sigma_dp, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }

            //[]K
            if (Physical.Gost34233_1.TryGetSigma(_hedi.SteelK, _hedi.tK, ref _sigma_dK, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }

            //[]T
            if (Physical.Gost34233_1.TryGetSigma(_hedi.SteelT, _hedi.tT, ref _sigma_dT, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }

            //alfap
            if (Physical.TryGetAlfa(_hedi.SteelK, _hedi.tK, ref _alfaK, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }

            //alfaT
            if (Physical.TryGetAlfa(_hedi.SteelT, _hedi.tT, ref _alfaT, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }

            if (Physical.Gost34233_1.TryGetRm(_hedi.Steelp, _hedi.tK, ref _Rmp, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }

            //TODO: sigmaa for different matireals Al,Cu, Tt

            _nNForSigmaa = 10.0;
            _nsigmaForSigmaa = 2.0;

            _CtForSigmaa = (2300.0 - _hedi.tp) / 2300.0;

            _AForSigmaap = (Physical.Gost34233_1.GetSteelType(_hedi.Steelp, ref _errorList) is (SteelType.Carbon or SteelType.Austenitic) and not SteelType.Undefined) ? 60000.0 : 45000.0;

            _BForSigmaa = 0.4 * _Rmp;

            _sigmaa_dp = (_CtForSigmaa * _AForSigmaap / Math.Sqrt(_nNForSigmaa * _hedi.N)) + (_BForSigmaa / _nsigmaForSigmaa);

            _AForSigmaaK = (Physical.Gost34233_1.GetSteelType(_hedi.SteelK, ref _errorList) is (SteelType.Carbon or SteelType.Austenitic) and not SteelType.Undefined) ? 60000.0 : 45000.0;
            _sigmaa_dK = (_CtForSigmaa * _AForSigmaaK / Math.Sqrt(_nNForSigmaa * _hedi.N)) + (_BForSigmaa / _nsigmaForSigmaa);

            _AForSigmaaT = (Physical.Gost34233_1.GetSteelType(_hedi.SteelT, ref _errorList) is (SteelType.Carbon or SteelType.Austenitic) and not SteelType.Undefined) ? 60000.0 : 45000.0;
            _sigmaa_dT = (_CtForSigmaa * _AForSigmaaT / Math.Sqrt(_nNForSigmaa * _hedi.N)) + (_BForSigmaa / _nsigmaForSigmaa);

            _mn = _hedi.a / _hedi.a1;
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

                    _Kqz = Math.PI * _hedi.a * _EK * _hedi.sK / (_hedi.l * _Kkom);
                    _Kpz = Math.PI * (Math.Pow(_hedi.Dkom, 2) - Math.Pow(_hedi.dkom, 2)) * _EK * _hedi.sK /
                        (4.8 * _hedi.l * _hedi.a * _Kkom);
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
                        _Kqz = _hedi.a * _hedi.sK / _hedi.l *
                            ((Math.PI * _EK / _Kpac) + (_hedi.Lpac / (_hedi.deltap * _hedi.D1)));
                        _Kpz = _hedi.a * _hedi.sK / (_hedi.l * Math.Pow(_betap, 2)) *
                            (((1 - Math.Pow(_betap, 2)) / 4.8 * ((Math.PI * _EK / _Kpac) + (_hedi.Lpac / (_hedi.deltap * _hedi.D1)))) - (0.5 * Math.PI * _hedi.Lpac / (_hedi.deltap * _hedi.D1)));
                    }
                    else if (_hedi.beta0 >= 15 && _hedi.beta0 <= 60)
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
                        _Kqz = ((_hedi.a * (_Ap1 + (_Ap2 * Math.Sqrt(_hedi.D1 / _hedi.sK)))) -
                            (0.5 * (1 - _betap) * _hedi.Lpac)) / _hedi.l;
                        _Kpz = (_Bp1 + (_Bp2 * Math.Sqrt(_hedi.D1 / _hedi.sK))) * _hedi.a / _hedi.l;
                    }
                    break;
                case CompensatorType.CompensatorOnExpander:
                    //TODO: Make calculation compensator on expeander 
                    break;
            }

            _Kq = 1 + _Kqz;
            _Kp = 1 + _Kpz;

            _psi0 = Math.Pow(_etaT, 7.0 / 3.0);

            _beta = _hedi.IsDifferentTubePlate
                ? 1.53 * Math.Pow((_Ky / _psi0) * ((1 / (_Ep1 * Math.Pow(_hedi.sp1, 3))) +
                (1 / (_Ep2 * Math.Pow(_hedi.sp2, 3)))), 1.0 / 4.0)
                : 1.82 / _hedi.sp * Math.Pow(_Ky * _hedi.sp / (_psi0 * _Ep), 1.0 / 4.0);

            _omega = _beta * _hedi.a1;

            _fip = 1 - (_hedi.d0 / _hedi.tp);

            _b1 = (_hedi.DH - _hedi.D) / 2.0;
            _R1 = (_hedi.DH - _hedi.D) / 4.0;
            _b2 = (_hedi.DH - _hedi.D) / 2.0;
            _R2 = (_hedi.DH - _hedi.D) / 4.0;

            //UNDONE: Check calculation for _b2, _R2 fo different tube plate

            _beta1 = 1.3 / Math.Pow(_hedi.a * _hedi.s1, 0.5);
            _beta2 = 1.3 / Math.Pow(_hedi.a * _hedi.s2, 0.5);
            _K1 = _beta1 * _hedi.a * _EK * Math.Pow(_hedi.s1, 3) / (5.5 * _R1);
            _K2 = _beta2 * _hedi.a * _ED * Math.Pow(_hedi.s2, 3) / (5.5 * _R2);
            _KF1 = (_E1 * Math.Pow(_hedi.h1, 3) * _b1 / (12.0 * Math.Pow(_R1, 2))) +
                (_K1 * (1.0 + (_beta1 * _hedi.h1 / 2.0)));
            _KF2 = (_E2 * Math.Pow(_hedi.h2, 3) * _b2 / (12.0 * Math.Pow(_R2, 2))) +
                (_K2 * (1.0 + (_beta2 * _hedi.h2 / 2.0)));
            _KF = _KF1 + _KF2;

            _mcp = 0.15 * _hedi.i * Math.Pow(_hedi.dT - _hedi.sT, 2) / Math.Pow(_hedi.a1, 2);

            _p0 = (((_alfaK * (_hedi.tK - _hedi.t0)) - (_alfaT * (_hedi.tT - _hedi.t0))) * _Ky * _hedi.l) +
                ((_etaT - 1.0 + _mcp + (_mn * (_mn + (0.5 * _ro * _Kq)))) * _hedi.pT) -
                ((_etaM - 1.0 + _mcp + (_mn * (_mn + (0.3 * _ro * _Kp)))) * _hedi.pM);

            _ro1 = _Ky * _hedi.a * _hedi.a1 / (Math.Pow(_beta, 2) * _KF * _R1);

            if (!Physical.Gost34233_7.TryGetPhi1Phi2Phi3(_omega, ref _Phi1, ref _Phi2, ref _Phi3, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }

            //TODO: Calculete values of _Phi1, _Phi2, _Phi3

            _t = 1 + (1.4 * _omega * (_mn - 1));
            _T1 = _Phi1 * (_mn + (0.5 * (1 + (_mn * _t)) * (_t - 1)));
            _T2 = _Phi2 * _t;
            _T3 = _Phi3 * _mn;

            //if (!Physical.Gost34233_7.TryGetT1T2T3(_omega, _mn, ref _T1, ref _T2, ref _T3, ref _errorList))
            //{
            //    IsCriticalError = true;
            //    return;
            //}

            _m1 = (1 + (_beta1 * _hedi.h1)) / (2 * Math.Pow(_beta1, 2));
            _m2 = (1 + (_beta2 * _hedi.h2)) / (2 * Math.Pow(_beta2, 2));
            _p1 = _Ky / (_beta * _KF) * ((_m1 * _hedi.pM) - (_m2 * _hedi.pT));

            _MP = _hedi.a1 / _beta * ((_p1 * (_T1 + (_ro * _Kq))) - (_p0 * _T2)) /
                (((_T1 + (_ro * _Kq)) * (_T3 + _ro1)) - Math.Pow(_T2, 2));

            _QP = _hedi.a1 * ((_p0 * (_T3 + _ro1)) - (_p1 * _T2)) /
                (((_T1 + (_ro * _Kq)) * (_T3 + _ro1)) - Math.Pow(_T2, 2));

            _Ma = _MP + ((_hedi.a - _hedi.a1) * _QP);
            _Qa = _mn * _QP;

            _NT = Math.PI * _hedi.a1 / _hedi.i * ((((_etaM * _hedi.pM) - (_etaT * _hedi.pT)) * _hedi.a1) +
                (_Phi1 * _Qa) + (_Phi2 * _beta * _Ma));
            _JT = Math.PI * (Math.Pow(_hedi.dT, 4) - Math.Pow(_hedi.dT - (2 * _hedi.sT), 4)) / 64;
            _lpr = _hedi.IsWithPartitions ? _hedi.l1R / 3.0 : _hedi.l;
            _MT = _ET * _JT * _beta / (_Ky * _hedi.a1 * _lpr) *
                ((_Phi2 * _Qa) + (_Phi3 * _beta * _Ma));

            _QK = (_hedi.a / 2.0 * _hedi.pT) - _QP;
            _MK = (_K1 / (_ro1 * _KF * _beta) * ((_T2 * _QP) + (_T3 * _beta * _MP))) -
                (_hedi.pM / (2 * Math.Pow(_beta1, 2)));

            _F = Math.PI * _hedi.D * _QK;

            _sigmap1 = 6 * Math.Abs(_MP) / Math.Pow(_hedi.s1p - _hedi.c, 2);
            _taup1 = Math.Abs(_QP) / (_hedi.s1p - _hedi.c);

            
            _mA = _beta * _Ma / _Qa;

            if (_mA >= -1.0 & _mA <= 1.0)
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

            _sigmap2 = 6 * _Mmax / _pfip * Math.Pow(_hedi.sp - _hedi.c, 2);
            _taup2 = Math.Abs(_Qa) / _pfip * (_hedi.sp - _hedi.c);

            _sigmaMX = Math.Abs(_QK) / (_hedi.s1 - _hedi.cK);
            _sigmaIX = 6 * Math.Abs(_MK) / Math.Pow(_hedi.s1 - _hedi.cK, 2);

            _sigmaMfi = Math.Abs(_hedi.pM) * _hedi.a / (_hedi.s1 - _hedi.cK);
            _sigmaIfi = 0.3 * _sigmaIX;

            _sigma1T = Math.Abs(_NT) / (Math.PI * (_hedi.dT - _hedi.sT) * _hedi.sT);
            _sigma1 = _sigma1T + (_hedi.dT * Math.Abs(_MT) / (2 * _JT));

            _sigma2T = (_hedi.dT - _hedi.sT) * Math.Max(Math.Abs(_hedi.pT - _hedi.pM), Math.Max(_hedi.pT, _hedi.pM));

            _conditionStaticStressForTubePlate1 = Math.Max(_taup1, _taup2);
            _conditionStaticStressForTubePlate2 = 0.8 * _sigma_dp;

            _isConditionStaticStressForTubePlate = _conditionStaticStressForTubePlate1 <= _conditionStaticStressForTubePlate2;
            if (!_isConditionStaticStressForTubePlate)
            {
                IsError = true;
                _errorList.Add("Условие статической прочности трубной решетки не выполняется");
            }

            _sigmaa_5_2_4k = CalculateSigmaa(_Ksigma, _sigmap1, 0, 0);
             
            if (_sigmaa_5_2_4k > _sigmaa_dp)
            {
                IsError = true;
                _errorList.Add("Условие малоцикловой прочности трубной решетки в месте соединения с кожухом не выполняется");
            }

            _sigmaa_5_2_4p = CalculateSigmaa(1, _sigmap2, 0, 0);

            if (_sigmaa_5_2_4p > _sigmaa_dp)
            {
                IsError = true;
                _errorList.Add("Условие малоцикловой прочности трубной решетки в перфорированной части не выполняется");
            }

            if (!_hedi.IsOneGo)
            {
                _spp = (_hedi.sp - _hedi.c) * _sigma_dp / _sigmaa_dp;
                _sp = _spp + _hedi.c;
                if (_sp > _hedi.sp)
                {
                    IsError = true;
                    _errorList.Add("Толщина трубной решетки меньше расчетной");
                }

                _snp1 = 1 - Math.Sqrt(_hedi.d0 / _hedi.BP * (_hedi.tP / _hedi.tp - 1.0));
                _snp2 = Math.Sqrt(_fip);
                _snp = (_hedi.sp - _hedi.c) * Math.Max(_snp1, _snp2);
                _sn = _snp + _hedi.c;

                if (_sn > _hedi.sn)
                {
                    IsError = true;
                    _errorList.Add("Толщина трубной решетки в месте паза под перегородку меньше расчетной");
                }
            }

            if (_hedi.IsNeedCheckHardnessTubePlate)
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

            //TODO: Make check dont need calculate for construction on picture 9
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
                _lambda = _KT * Math.Sqrt(_sigmaa_dT / _ET) * _lR / (_hedi.dT - _hedi.sT);
                _phiT = 1 / Math.Sqrt(1 + Math.Pow(_lambda, 4));

                _conditionStabilityForTube2 =_phiT * _sigmaa_dp;

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

            switch (_hedi.TubeRolling)
            {
                case TubeRollingType.RollingWithoutGroove:
                    _Ntp_d = 0.5 * Math.PI * _hedi.sT * (_hedi.dT - _hedi.sT) * Math.Min(_hedi.lB / _hedi.dT, 1.6) *
                        Math.Min(_sigma_dT, _sigma_dp);
                    break;
                case TubeRollingType.RollingWithOneGroove:
                    var x = Math.Min(_hedi.lB / _hedi.dT, 1.6);
                    _Ntp_d = 0.6 * Math.PI * _hedi.sT * (_hedi.dT - _hedi.sT) * Math.Min(_sigma_dT, _sigma_dp) *
                        (x > 1.2 ? x : 1.0);
                    break;
                case TubeRollingType.RollingWithMoreThenOneGroove:
                    _Ntp_d = 0.8 * Math.PI * _hedi.sT * (_hedi.dT - _hedi.sT) * Math.Min(_sigma_dT, _sigma_dp);
                    break;
            }

            _isConditionStressBracingTube = Math.Abs(_NT) <= _Ntp_d;
            if (!_isConditionStressBracingTube)
            {
                IsError = true;
                _errorList.Add("Условие прочности крепления трубы в решетке не выполняется");
            }

            _PhiC = Math.Min(0.5, 0.95 - (0.2 * Math.Log(_hedi.N)));

            if (_hedi.IsTubeOnlyWelding)
            {
                _tau = ((Math.Abs(_NT) * _hedi.dT) + (4 * Math.Abs(_MT))) /
                    (Math.PI * Math.Pow(_hedi.dT, 2) * _hedi.delta);
                _conditionStressBracingTube2 = _PhiC * Math.Min(_sigma_dT, _sigmaa_dp);
                _isConditionStressBracingTube2 = _tau <= _conditionStressBracingTube2;
            }
            else
            {
                _conditionStressBracingTube11 = _PhiC * Math.Min(_sigma_dT, _sigmaa_dp) / _tau +
                    0.6 * _Ntp_d / Math.Abs(_NT);
                _conditionStressBracingTube12 = _Ntp_d / Math.Abs(_NT);

                _isConditionStressBracingTube2 = Math.Max(_conditionStressBracingTube11, _conditionStressBracingTube12) <= 1.0;
            }

            if (!_isConditionStressBracingTube2)
            {
                IsError = true;
                _errorList.Add("Условие прочности крепления трубы в решетке не выполняется");
            }

            _spp_5_5_1 = 0.5 * _hedi.DE * Math.Sqrt(_hedi.pp / _sigmaa_dp);
            _sp_5_5_1 = _spp_5_5_1 + _hedi.c;

            if (_sp_5_5_1 > _hedi.sp)
            {
                IsError = true;
                _errorList.Add("Толщина трубной решетки меньше расчетной");
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

            body.AddParagraph("");

            {
                var imagePart = mainPart.AddImagePart(ImagePartType.Gif);

                var a = ((int)_hedi.FirstTubePlate.TubePlateType).ToString();

                var b = _hedi.IsDifferentTubePlate
                    ? ((int)_hedi.SecondTubePlate.TubePlateType).ToString() : a;

                var c = _hedi.CompensatorType switch
                {
                    CompensatorType.No => "",
                    CompensatorType.Compensator => " Syl",
                    CompensatorType.Expander => " Exp",
                    CompensatorType.CompensatorOnExpander => " Exp",
                    _ => "",
                };

                var pic = (byte[])Data.Properties.Resources.ResourceManager.GetObject("Fixed " + a + "-" + b + c);

                if (pic != null)
                {
                    imagePart.FeedData(new MemoryStream(pic));

                    body.AddParagraph("").AddImage(mainPart.GetIdOfPart(imagePart), pic);
                }
            }

            //TODO: Add image flange
            {
                var imagePart = mainPart.AddImagePart(ImagePartType.Gif);


                //var type = _fbdi.IsFlangeFlat ? "f21_" : "fl1_";

                //string type1 = "";

                //switch (_fbdi.FlangeFace)
                //{
                //    case FlangeFaceType.Flat:
                //        type1 = "a";
                //        break;
                //    case FlangeFaceType.MaleFemale:
                //        type1 = "b";
                //        break;
                //    case FlangeFaceType.TongueGroove:
                //        type1 = "c";
                //        break;
                //    case FlangeFaceType.Ring:
                //        type1 = "d";
                //        break;
                //}

                //var b = (byte[])Data.Properties.Resources.ResourceManager.GetObject(type + type1);
                //var stream = new MemoryStream(b);
                //imagePart.FeedData(stream);

                //body.Elements<Paragraph>().LastOrDefault().AddImage(mainPart.GetIdOfPart(imagePart));
            }



            body.AddParagraph("Исходные данные").Alignment(AlignmentType.Center);

            //table
            //{
            //    body.AddParagraph("Крышка");

            //    var table = body.AddTable();

            //    //table.AddRow().
            //    //  AddCell("");

            //    table.AddRow()
            //        .AddCell("Марка стали")
            //        .AddCell($"{_fbdi.CoverSteel}");

            //    table.AddRow()
            //        .AddCell("Коэффициент прочности сварного шва, φ:")
            //        .AddCell($"{_fbdi.fi}");

            //    table.AddRow()
            //        .AddCell("Прибавка на коррозию, ")
            //        .AppendEquation("c_1")
            //        .AppendText(":")
            //        .AddCell($"{_fbdi.c1} мм");

            //    table.AddRow()
            //        .AddCell("Прибавка для компенсации минусового допуска, ")
            //        .AppendEquation("c_2")
            //        .AppendText(":")
            //        .AddCell($"{_fbdi.c2} мм");

            //    if (_fbdi.c3 > 0)
            //    {
            //        table.AddRow()
            //            .AddCell("Технологическая прибавка, ")
            //            .AppendEquation("c_3")
            //            .AppendText(":")
            //            .AddCell($"{_fbdi.c3} мм");
            //    }

            //    table.AddRow()
            //        .AddCell(_fbdi.IsCoverWithGroove
            //        ? "Исполнительная толщина плоской крышки в месте паза для перегородки, "
            //        : "Исполнительная толщина крышки, ")
            //        .AppendEquation("s_1")
            //        .AppendText(":")
            //        .AddCell($"{_fbdi.s1} мм");

            //    table.AddRow()
            //        .AddCell("Исполнительная толщина плоской крышки в зоне уплотнения, ")
            //        .AppendEquation("s_2")
            //        .AppendText(":")
            //        .AddCell($"{_fbdi.s2} мм");

            //    table.AddRow()
            //        .AddCell("Толщина крышки вне уплотнения, ")
            //        .AppendEquation("s_3")
            //        .AppendText(":")
            //        .AddCell($"{_fbdi.s3} мм");

            //    if (_fbdi.IsCoverWithGroove)
            //    {
            //        table.AddRow()
            //        .AddCell("Ширина паза под перегородку, ")
            //        .AppendEquation("s_4")
            //        .AppendText(":")
            //        .AddCell($"{_fbdi.s4} мм");
            //    }

            //    table.AddRow()
            //        .AddCell("Наименьший диаметр наружной утоненной части плоской крышки, ")
            //        .AppendEquation("D_2")
            //        .AppendText(":")
            //        .AddCell($"{_fbdi.D2} мм");

            //    table.AddRow()
            //        .AddCell("Диаметр болтовой окружности, ")
            //        .AppendEquation("D_3")
            //        .AppendText(":")
            //        .AddCell($"{_fbdi.D3} мм");

            //    table.AddRow()
            //        .AddCell("Отверстия в крышке")
            //        .AddCell(_fbdi.Hole == HoleInFlatBottom.WithoutHole ? "нет" : "есть");

            //    switch (_fbdi.Hole)
            //    {
            //        case HoleInFlatBottom.OneHole:
            //            table.AddRow()
            //                .AddCell("Диаметр отверстия в крышке, d:")
            //                .AddCell($"{_fbdi.d} мм");
            //            break;
            //        case HoleInFlatBottom.MoreThenOneHole:
            //            table.AddRow()
            //                .AddCell("Диаметр отверстий в крышке, ")
            //                .AppendEquation("d_i")
            //                .AppendText(":")
            //                .AddCell($"{_fbdi.di} мм");
            //            break;
            //    }
            //    body.InsertTable(table);

            //}

            body.AddParagraph("Результаты расчета").Alignment(AlignmentType.Center);

            body.AddParagraph("Вспомогательные величины").Alignment(AlignmentType.Center);

            body.AddParagraph("Относительную характеристику беструбного края трубной решетки вычисляют по формуле");

            body.AddParagraph("")
                .AppendEquation($"m_n=a/a_1={_hedi.a}/{_hedi.a1}={_mn:f2}");

            body.AddParagraph("Коэффициенты влияния давления на трубную решетку вычисляют по формулам:");
            body.AddParagraph("- со стороны межтрубного пространства");
            body.AddParagraph("")
                .AppendEquation($"η_M=1-(i∙d_T^2)/(4∙a_1^2)=1-({_hedi.i}∙{_hedi.dT}^2)/(4∙{_hedi.a1}^2)={_etaM:f2}");
            body.AddParagraph("- со стороны трубного пространства");
            body.AddParagraph("")
                .AppendEquation($"η_T=1-(i∙(d_T-2∙s_T)^2)/(4∙a_1^2)=1-({_hedi.i}∙({_hedi.dT}-{_hedi.sT})^2)/(4∙{_hedi.a1}^2)={_etaT:f2}");
            body.AddParagraph("Основные характеристики жесткости элементов теплообменного аппарата");
            body.AddParagraph("Модуль упругости основания(системы труб) вычисляют по формуле");
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
                    //TODO: Make calculation compensator on expeander 
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
                    .AppendEquation($"β=1.82/s_p∙∜((K_y∙s_p)/(ψ_0∙E_p))=1.82/{_hedi.sp}∙∜(({_Ky:f2}∙{_hedi.sp})/({_psi0:f2}∙{_Ep}))={_beta:f2}");
            }
            else
            {
                body.AddParagraph("")
                    .AppendEquation($"β=1.53∙∜(K_y/ψ_0∙(1/(E_p1∙s_p1^3)+1/(E_p2∙s_p2^3))" +
                    $"=1.53∙∜({_Ky:f2}/{_psi0:f2}∙(1/({_Ep1}∙{_hedi.sp1}^3)+1/({_Ep2}∙{_hedi.sp2}^3))={_beta:f2}");
            }

            body.AddParagraph("Безразмерный параметр системы решетка — трубы вычисляют по формуле");
            body.AddParagraph("")
                .AppendEquation($"ω=β∙a_1={_beta:f2}∙{_hedi.a1}={_omega:f2}");

            body.AddParagraph("Коэффициент системы кожух — решетка");
            body.AddParagraph("")
                .AppendEquation($"β_1=1.3/√(a∙s_1)=1.3/√({_hedi.a}∙{_hedi.s1})={_beta1:f2}");

            body.AddParagraph("Коэффициент системы обечайка — фланец камеры");
            body.AddParagraph("")
                .AppendEquation($"β_2=1.3/√(a∙s_2)=1.3/√({_hedi.a}∙{_hedi.s2})={_beta2:f2}");

            body.AddParagraph("Коэффициент жесткости фланцевого соединения при изгибе");
            body.AddParagraph("")
                .AppendEquation("K_Φ=K_Φ1+K_Φ2");

            body.AddParagraph("")
                .AppendEquation("K_Φ1=(E_1∙h_1^3∙b_1)/(12∙R_1^2)+K_1∙(1+(β_1∙h_1)/2)");

            body.AddParagraph("")
                .AppendEquation($"K_1=(β_1∙a∙E_K∙s_1^3)/(5.5∙R_1)=({_beta1:f2}∙{_hedi.a}∙{_EK}∙{_hedi.s1}^3)/(5.5∙{_R1:f2})={_K1:f2}");

            body.AddParagraph("")
                .AppendEquation($"K_Φ1=({_E1}∙{_hedi.h1}^3∙{_b1:f2})/(12∙{_R1:f2}^2)+{_K1:f2}∙(1+({_beta1:f2}∙{_hedi.h1:f2})/2)={_KF1:f2}");

            body.AddParagraph("")
                .AppendEquation("K_Φ2=(E_2∙h_2^3∙b_2)/(12∙R_2^2)+K_2∙(1+(β_2∙h_2)/2)");

            body.AddParagraph("")
                .AppendEquation($"K_2=(β_2∙a∙E_D∙s_2^3)/(5.5∙R_2)=({_beta2:f2}∙{_hedi.a}∙{_ED}∙{_hedi.s2}^3)/(5.5∙{_R2:f2})={_K2:f2}");

            body.AddParagraph("")
                .AppendEquation($"K_Φ1=({_E2}∙{_hedi.h2}^3∙{_b2:f2})/(12∙{_R2:f2}^2)+{_K2:f2}∙(1+({_beta2:f2}∙{_hedi.h2:f2})/2)={_KF2:f2}");

            body.AddParagraph("")
                .AppendEquation($"K_Φ={_KF1:f2}+{_KF2:f2}={_KF:f2}");

            body.AddParagraph("Определение усилий в элементах теплообменного аппарата");

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
                .AppendEquation($"ρ_1=(K_y∙a∙a_1)/(β_2∙K_Φ∙R_1)=({_Ky:f2}∙{_hedi.a}∙{_hedi.a1})/({_beta2:f2}∙{_KF:f2}∙{_R1:f2})={_ro1:f2}");

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
                .AppendEquation($"m_1=(1-β_1∙h_1)/(2∙β_1^2)=(1-{_beta1:f2}∙{_hedi.h1})/(2∙{_beta1:f2}^2)={_m1:f2}");

            body.AddParagraph("")
                .AppendEquation($"m_2=(1-β_2∙h_2)/(2∙β_2^2)=(1-{_beta2:f2}∙{_hedi.h2})/(2∙{_beta2:f2}^2)={_m2:f2}");

            body.AddParagraph("")
                .AppendEquation($"p_1={_Ky:f2}/({_beta:f2}∙{_KF:f2})∙({_m1:f2}∙{_hedi.pM}-{_m2:f2}∙{_hedi.pT})={_p1:f2}");

            body.AddParagraph("")
                .AppendEquation($"M_Π=({_hedi.a1}/{_beta:f2})∙({_p1:f2}∙({_T1:f2}+{_ro:f2}∙{_Kq:f2})-{_p0:f2}∙{_T2:f2})/(({_T1:f2}+{_ro:f2}∙{_Kq:f2})∙({_T3:f2}+{_ro1:f2})-{_T2:f2}^2)={_MP:f2} (Н∙мм)/мм");

            body.AddParagraph("")
                .AppendEquation($"Q_Π={_hedi.a1}∙({_p0:f2}∙({_T3:f2}+{_ro1:f2})-{_p1:f2}∙{_T2:f2})/(({_T1:f2}+{_ro:f2}∙{_Kq:f2})∙({_T3:f2}+{_ro1:f2})-{_T2:f2}^2)={_QP:f2} H");

            body.AddParagraph("Изгибающий момент и перерезывающие силы, распределенные по периметру перфорированной зоны решетки, вычисляют по формулам:");
            body.AddParagraph("- для изгибающего момента");
            body.AddParagraph("")
                .AppendEquation($"M_a=M_Π+(a-a_1)∙Q_Π={_MP:f2}+({_hedi.a}-{_hedi.a1})={_Ma:f2} (Н∙мм)/мм");

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
                .AppendEquation($"Q_K=a/2∙p_T-Q_Π={_hedi.a}/2∙{_hedi.pT}-{_QP:f2}={_QK:f2} H");

            body.AddParagraph("- изгибающий момент, распределенный по периметру кожуха");
            body.AddParagraph("")
                .AppendEquation("M_K=K_1/(ρ_1∙K_Φ∙β)∙(T_2∙Q_Π+T_3∙β∙M_Π)-p_M/(2∙β_1^2) +" +
                $"{_K1:f2}/({_ro1:f2}∙{_KF:f2}∙{_beta:f2})∙({_T2:f2}∙{_QP:f2}+{_T3:f2}∙{_beta:f2}∙{_MP:f2})-{_hedi.pM}/(2∙{_beta1:f2}^2)={_MK:f2} (Н∙мм)/мм");

            body.AddParagraph("- суммарная осевая сила, действующая на кожух");
            body.AddParagraph("")
                .AppendEquation($"F=π∙D∙Q_K=π∙{_hedi.D}∙{_QK:f2}={_F:f2} H");

        }

        private static double CalculateSigmaa(double Ksigma,
                                              double deltasigma1,
                                              double deltasigma2,
                                              double deltasigma3) => Ksigma * Math.Max(Math.Abs(deltasigma1 - deltasigma2),
                Math.Max(Math.Abs(deltasigma2 - deltasigma3), Math.Abs(deltasigma1 - deltasigma3)));




    }
}