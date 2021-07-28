using CalculateVessels.Core.HeatExchanger.Enums;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.PhysicalData;
using CalculateVessels.Data.PhysicalData.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _ET = 0.0;
            _EK = 0.0;
            _ED = 0.0;
            _E1 = 0.0;
            _E2 = 0.0;

            if (_hedi.IsDifferentTubePlate)
            {
                _Ep1 = 0.0;
                _Ep2 = 0.0;
            }
            else
            {
                _Ep = 0.0;
            }
            
            if (_hedi.IsNeedKcompensatorCalculate)
            {
                _Ekom = 0.0;
            }

            _sigma_dp = 0.0;
            _sigma_dK = 0.0;
            _sigma_dT = 0.0;

            if (Physical.TryGetAlfa(_hedi.StellK, _hedi.tK, ref _alfaK, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }

            if (Physical.TryGetAlfa(_hedi.StellT, _hedi.tT, ref _alfaT, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }

            _Rmp = 0.0;

            //TODO: sigmaa for different matireals Al,Cu, Tt

            _nNForSigmaa = 10.0;
            _nsigmaForSigmaa = 2.0;

            _CtForSigmaa = (2300.0 - _hedi.tp) / 2300.0;

            _AForSigmaap = (Physical.Gost34233_1.GetSteelType(_hedi.Stellp, ref _errorList) is SteelType.Carbon or SteelType.Austenitic) ? 60000.0 : 45000.0;

            _BForSigmaa = 0.4 * _Rmp;

            _sigmaa_dp = (_CtForSigmaa * _AForSigmaap / Math.Sqrt(_nNForSigmaa * _hedi.N)) + (_BForSigmaa / _nsigmaForSigmaa);

            _AForSigmaaK = (Physical.Gost34233_1.GetSteelType(_hedi.StellK, ref _errorList) is SteelType.Carbon or SteelType.Austenitic) ? 60000.0 : 45000.0;
            _sigmaa_dK = (_CtForSigmaa * _AForSigmaaK / Math.Sqrt(_nNForSigmaa * _hedi.N)) + (_BForSigmaa / _nsigmaForSigmaa);

            _AForSigmaaT = (Physical.Gost34233_1.GetSteelType(_hedi.StellT, ref _errorList) is SteelType.Carbon or SteelType.Austenitic) ? 60000.0 : 45000.0;
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

            //_Phi1 = 0.0;
            //_Phi2 = 0.0;
            //_Phi3 = 0.0;
            ////TODO: Get values to _Phi1, _Phi2, _Phi3 from tables or calculete them

            //_t = 1 + (1.4 * _omega * (_mn - 1));
            //_T1 = _Phi1 * (_mn + (0.5 * (1 + (_mn * _t)) * (_t - 1)));
            //_T2 = _Phi2 * _t;
            //_T3 = _Phi3 * _mn;

            if (!Physical.Gost34233_7.TryGetT1T2T3(_omega, _mn, ref _T1, ref _T2, ref _T3, ref _errorList))
            {
                IsCriticalError = true;
                return;
            }

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
            throw new NotImplementedException();
        }

        private static double CalculateSigmaa(double Ksigma,
                                              double deltasigma1,
                                              double deltasigma2,
                                              double deltasigma3) => Ksigma * Math.Max(Math.Abs(deltasigma1 - deltasigma2),
                Math.Max(Math.Abs(deltasigma2 - deltasigma3), Math.Abs(deltasigma1 - deltasigma3)));




    }
}
