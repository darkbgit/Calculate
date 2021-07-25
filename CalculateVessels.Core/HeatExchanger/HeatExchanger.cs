using CalculateVessels.Core.HeatExchanger.Enums;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.PhysicalData;
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

        private double _mn;
        private double _ET;
        private double _etaM;
        private double _etaT;
        private double _Ky;
        private double _ro;
        private double _EK;
        private double _ED;
        private double _E1;
        private double _E2;
        private double _Kqz;
        private double _Kpz;
        private double _betakom;
        private double _Xkom;
        private double _Ykom;
        private double _Cf;
        private double _Akom;
        private double _Kkom;
        private double _Ekom;
        private double _Kpac;
        private double _betap;
        private double _Ap;
        private double _Ap1;
        private double _Ap2;
        private double _Bp1;
        private double _Bp2;
        private double _Kq;
        private double _Kp;
        private double _psi0;
        private double _Ep1;
        private double _Ep2;
        private double _Ep;
        private List<string> _errorList = new();
        private double _beta;
        private double _omega;
        private double _fip;
        private double _b1;
        private double _R1;
        private double _b2;
        private double _R2;
        private double _beta1;
        private double _beta2;
        private double _K1;
        private double _K2;
        private double _KF1;
        private double _KF2;
        private double _KF;
        private double _mcp;
        private double _p0;
        private double _ro1;
        private double _Phi1;
        private double _Phi2;
        private double _Phi3;
        private double _T1;
        private double _T2;
        private double _T3;
        private double _m1;
        private double _m2;
        private double _p1;
        private double _MP;
        private double _QP;
        private double _Ma;
        private double _Qa;
        private double _NT;
        private double _JT;
        private double _lpr;
        private double _MT;
        private double _QK;
        private double _MK;
        private double _F;
        private double _alfaK;
        private double _alfaT;
        private double _t;

        public bool IsCriticalError { get; private set; }

        public bool IsError { get; private set; }

        public List<string> ErrorList { get => _errorList; private set => _errorList = value; }
        public List<string> Bibliography { get; } = new()
        {
            Data.Properties.Resources.GOST_34233_1,
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

            _alfaK = 0.0;
            _alfaT = 0.0;



            if (ErrorList.Any())
            {
                IsCriticalError = true;
                return;
            }

            if (_hedi.IsNeedKcompensatorCalculate)
            {
                _Ekom = 0.0;
            }

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

            _Phi1 = 0.0;
            _Phi2 = 0.0;
            _Phi3 = 0.0;
            //TODO: Get values to _Phi1, _Phi2, _Phi3 from tables or calculete them

            _t = 1 + (1.4 * _omega * (_mn - 1));
            _T1 = _Phi1 * (_mn + (0.5 * (1 + (_mn * _t)) * (_t - 1)));
            _T2 = _Phi2 * _t;
            _T3 = _Phi3 * _mn;

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

            //elif _hedi.type == 'plav':



            //    if True:
            //        _dE = _hedi.d0 - 2 * _hedi.sT
            //    else:
            //        _dE = _hedi.d0 - _hedi.sT

            //    _fiE = 1 - _dE / _hedi.tp



            //elif _hedi.type == 'U':
            //    pass
        }

        public void MakeWord(string filename)
        {
            throw new NotImplementedException();
        }
    }
}
