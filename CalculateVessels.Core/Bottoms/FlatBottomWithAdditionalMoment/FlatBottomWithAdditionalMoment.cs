using CalculateVessels.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalculateVessels.Core.Bottoms.Enums;
using CalculateVessels.Data.PhysicalData;


namespace CalculateVessels.Core.Bottoms.FlatBottomWithAdditionalMoment
{
    public class FlatBottomWithAdditionalMoment : IElement
    {
        private readonly FlatBottomWithAdditionalMomentDataIn _fbdi;

        public FlatBottomWithAdditionalMoment(FlatBottomWithAdditionalMomentDataIn flatBottomWithAdditionalMomentDataIn)
        {
            _fbdi = flatBottomWithAdditionalMomentDataIn;
        }

        public bool IsCriticalError { get; private set; }

        public bool IsError { get; private set; }

        public List<string> ErrorList { get; private set; } = new();

        public List<string> Bibliography { get; } = new()
        {
            Data.Properties.Resources.GOST_34233_1,
            Data.Properties.Resources.GOST_34233_2
        };


        private double _Ab;

        private double _b;
        private double _b0;

        private double _c;

        private double _dkr;
        private double _Dcp;

        private double _e;

        private double _K;
        private double _K_1;
        private double _K0;
        private double _Kp;
        private double _K6;
        private double _K7Fors2;
        private double _K7Fors3;

        private double _Dp;

        private double _hkr;

        private double _Qd;
        private double _QFM;
        private double _Qt;

        private double _s1p;
        private double _s1;
        private double _s2p;
        private double _s2;
        private double _s2p_1;
        private double _s2p_2;
        private double _s3p;
        private double _s3;
        private double _s3p_1;
        private double _s3p_2;
        private double _S0;
        private double _Se;

        private double _conditionUseFormulas;
        private double _p_d;
        private double _Pbp;
        private double _Pbm;
        private double _Pb1;
        private double _Pb1_1;
        private double _Pb1_2;
        private double _Pb2;
        private double _Pb2_2;
        private double _Pobj;

        private double _Rp;

        private double _tb;
        private double _tf;
        private double _tkr;

        private double _x;
        
        private double _yp;
        private double _yb;
        private double _yF;
        private double _ykr;
        private double _yfn;

        private double _alfa;
        private double _alfa_m;
        private double _alfab;
        private double _alfaf;
        private double _alfakr;
        private double _alfash1;
        private double _alfash2;
        private double _zeta;

        
        private double _Lb;
        private double _fb;

        private double _m;
        private double _qobj;
        private double _q_d;
        private double _Kobj;

        private double _E;
        private double _Ep;
        private double _Eb;
        private double _Eb20;
        private double _E20;
        private double _Ekr;
        private double _Ekr20;

        private double _l0;
        private double _KGost34233_4;
        private double _beta;
        private double _betaT;
        private double _betaU;
        private double _betaY;
        private double _betaZ;
        private double _betaF;
        private double _betaV;
        private double _gamma;
        private double _f;
        private double _lambda;
        private double _deltakr;

        private double _sigma_dnb;
        private double _psi1;
        private double _Phi;
        private double _Phi_1;
        private double _Phi_2;

        private double _Xkr;
        private double _Kkr;


        public void Calculate()
        {
            _c = _fbdi.c1 + _fbdi.c2 + _fbdi.c3;
            
            if (_fbdi.IsFlangeIsolated)
            {
                _tf = _fbdi.t;
                _tb = _fbdi.t * 0.97;
            }
            else
            {
                _tf = _fbdi.t * 0.96;
                _tb = _fbdi.t * 0.95;
            }

                  
            _E20 = Physical.Gost34233_4.GetE(_fbdi.FlangeSteel, 20);
            _E = Physical.Gost34233_4.GetE(_fbdi.FlangeSteel, _tf);
            _alfaf = Physical.Gost34233_4.GetAlfa(_fbdi.FlangeSteel, _fbdi.t);

            List<string> errorList = new();
            _Ekr20 = Physical.Gost34233_1.GetE(_fbdi.CoverSteel, 20, ref errorList);
            _Ekr = Physical.Gost34233_1.GetE(_fbdi.CoverSteel, _fbdi.t, ref errorList);
            

            _alfakr = Physical.Gost34233_4.GetAlfa(_fbdi.CoverSteel, _fbdi.t);
            _tkr = _fbdi.t;
            _hkr = _fbdi.s2;
            _deltakr = _fbdi.s2;
            _dkr = _fbdi.Screwd;

            _alfash1 = Physical.Gost34233_4.GetAlfa(_fbdi.WasherSteel, _tf);
            _alfash2 = _alfash1;

            _Eb20 = Physical.Gost34233_4.GetE(_fbdi.ScrewSteel, 20);
            _Eb = Physical.Gost34233_4.GetE(_fbdi.ScrewSteel, _tb);
            _alfab = Physical.Gost34233_4.GetAlfa(_fbdi.ScrewSteel, _tb);
            _fb = Physical.Gost34233_4.Getfb(_fbdi.Screwd, _fbdi.IsScrewWithGroove);
            _sigma_dnb = Physical.Gost34233_4.GetSigma(_fbdi.ScrewSteel, _tb);


            _S0 = _fbdi.IsFlangeFlat ? _fbdi.s : _fbdi.s0;


            if (errorList.Any())
            {
                IsCriticalError = true;
                ErrorList = ErrorList.Concat(errorList).ToList();
                return;
            }

            if (_fb == 0 || _Eb20 == 0)
            {
                IsCriticalError = true;
                ErrorList.Add("Ошибка получения значений физических велечин");
                return;
            }

            if (_fbdi.IsGasketMetal)
            {
                (_m, _qobj, _, _, _) = Physical.Gost34233_4.GetGasketParameters(_fbdi.GasketType);
            }
            else
            {
                (_m, _qobj, _q_d, _Kobj, _Ep) = Physical.Gost34233_4.GetGasketParameters(_fbdi.GasketType);
            }

            if (_fbdi.IsGasketFlat)
            {
                _b0 = _fbdi.bp <= 15 ? _fbdi.bp : 3.8 * Math.Sqrt(_fbdi.bp);
                _Dcp = _fbdi.Dnp - _b0;
            }
            else
            {
                _b0 = _fbdi.bp / 4;
                _Dcp = _fbdi.Dcp;
            }

            _Pobj = 0.5 * Math.PI * _Dcp * _b0 * _qobj;
            _Rp = _fbdi.IsPressureIn ? Math.PI * _fbdi.Dcp * _b0 * _m * Math.Abs(_fbdi.p) : 0.0;

            _Ab = _fbdi.n * _fb;


            _Qd = 0.785 * _fbdi.p * Math.Pow(_Dcp, 2);


            _QFM = _fbdi.F + 4 * Math.Abs(_fbdi.M) / _Dcp;


            _b = 0.5 * (_fbdi.Db - _Dcp);

            _l0 = Math.Sqrt(_fbdi.D * _S0);
            _beta = _fbdi.S1 / _S0;
            _x = _fbdi.l / _l0;

            _zeta = 1 + (_beta - 1) * _x / (_x + (1 + _beta) / 4.0);
            _Se = 0.5 * (_zeta * _S0);
            
            
            _yp = _fbdi.IsGasketMetal
                ? 0 : _fbdi.hp * _Kobj / (_Ep * Math.PI * _Dcp * _fbdi.bp);
            
            _Lb = _fbdi.Lb0 + (_fbdi.IsStud ? 0.56 : 0.28) * _fbdi.Screwd;
            _yb = _Lb / (_Eb20 * _fb * _fbdi.n);
            
            _KGost34233_4 = _fbdi.Dn / _fbdi.D;

            _betaT = (Math.Pow(_KGost34233_4, 2) * (1 + 8.55 * Math.Log(_KGost34233_4)) - 1) /
                     (1.05 + 1.945 * Math.Pow(_KGost34233_4, 2) * (_KGost34233_4 - 1));
            _betaU = (Math.Pow(_KGost34233_4, 2) * (1 + 8.55 * Math.Log(_KGost34233_4)) - 1) /
                     (1.36 * (Math.Pow(_KGost34233_4, 2) - 1) * (_KGost34233_4 - 1));
            _betaY = 1 / (_KGost34233_4 - 1) *
                     (0.69 + 5.72 * Math.Pow(_KGost34233_4, 2) * Math.Log(_KGost34233_4) /
                         (Math.Pow(_KGost34233_4, 2) - 1));
            _betaZ = (Math.Pow(_KGost34233_4, 2) + 1) / (Math.Pow(_KGost34233_4, 2) - 1);


            _betaF = 0.91;
            _betaV = 0.55;
            _f = 1.0;
            //TODO: _betaF, _betaV, _f take values from diagram. how?

            _lambda = _betaF * _fbdi.h + _l0 / (_betaT * _l0) +
                      _betaV * Math.Pow(_fbdi.h, 3) / (_betaU * _l0 * Math.Pow(_S0, 2));
            _yF = 0.91 * _betaV / (_E20 * _lambda * Math.Pow(_S0, 2) * _l0);

            _yfn = Math.Pow(Math.PI / 4, 3) * _fbdi.Db / (_E20 * _fbdi.Dn * Math.Pow(_fbdi.h, 3));

            if (_fbdi.IsCoverFlat)
            {
                _Kkr = _fbdi.Dn / _Dcp;
                _Xkr = 0.67 * (Math.Pow(_Kkr, 2) * (1 + 8.55 * Math.Log(_Kkr)) - 1) /
                    ((_Kkr - 1) * (Math.Pow(_Kkr, 2) - 1 + (1.857 * Math.Pow(_Kkr, 2) + 1) * Math.Pow(_hkr, 3) / Math.Pow(_dkr, 3)));
                _ykr = _Xkr / (_Ekr20 * Math.Pow(_deltakr, 3));
            }
            else
            {
                //TODO: Ad
            }

            _gamma = 1 / (_yp + _yb * _Eb20 / _Eb + (_yF * _E20 / _E + _ykr * _Ekr20 / _Ekr) * Math.Pow(_b, 2));

            _Qt = _gamma * ((_alfaf * _fbdi.h + _alfash1 * _fbdi.hsh) * (_tf - 20) +
                (_alfakr * _hkr + _alfash2 * _fbdi.hsh) * (_tkr - 20) -
                _alfab * (_fbdi.h + _hkr) * (_tb - 20)); 
            

            _alfa = 1 - (_yp - (_yF * _e + _ykr * _b) * _b) /
                (_yp + _yb + (_yF + _ykr) * Math.Pow(_b, 2));

            _alfa_m = (_yb + 2 * _yfn * _b * (_b + _e - Math.Pow(_e, 2) / _Dcp)) /
                (_yb + _yp * Math.Pow(_fbdi.Db / _Dcp, 2)  + 2 * _yfn * Math.Pow(_b, 2));



            _Pb1_1 = _alfa * (_Qd + _fbdi.F) + _Rp + 4 * _alfa_m * Math.Abs(_fbdi.M) / _Dcp;
            _Pb1_1 = _alfa * (_Qd + _fbdi.F) + _Rp + 4 * _alfa_m * Math.Abs(_fbdi.M) / _Dcp - _Qt;
            _Pb1 = Math.Max(_Pb1_1, _Pb1_2);

            _Pb2_2 = 0.4 * _Ab * _sigma_dnb;
            _Pb2 = Math.Max(_Pobj, _Pb2_2);

            _Pbm = Math.Max(_Pb1, _Pb2);

            _Pbp = _Pbm + (1 - _alfa) * (_Qd + _fbdi.F) + _Qt + 4 * (1 - _alfa_m) * Math.Abs(_fbdi.M) / _Dcp;

            _psi1 = _Pbp / _Qd;

            _K6 = _fbdi.IsCoverWithGroove
                ? 0.41 * Math.Sqrt((1.0 + 3.0 * _psi1 * (_fbdi.D3 / _Dcp - 1) + 9.6 * _fbdi.D3 / _Dcp * _fbdi.s4 / _Dcp) / (_fbdi.D3 / _Dcp))
                : 0.41 * Math.Sqrt((1.0 + 3.0 * _psi1 * (_fbdi.D3 / _Dcp - 1)) / (_fbdi.D3 / _Dcp));


            _Dp = _Dcp;
            switch (_fbdi.Hole)
            {
                case HoleInFlatBottom.WithoutHole:
                    _K0 = 1;
                    break;
                case HoleInFlatBottom.OneHole:
                    _K0 = Math.Sqrt(1.0 + _fbdi.d / _Dp + Math.Pow(_fbdi.d / _Dp, 2));
                    break;
                case HoleInFlatBottom.MoreThenOneHole:
                    if (_fbdi.di > 0.7 * _Dp)
                    {
                        IsError = true;
                        ErrorList.Add("Слишком много отверстий");
                    }
                    _K0 = Math.Sqrt((1 - Math.Pow(_fbdi.di / _Dp, 3)) / (1 - _fbdi.di / _Dp));
                    break;
                default:
                    IsError = true;
                    ErrorList.Add("Ошибка определения колличества отверстий");
                    break;
            }

            _s1p = _K0 * _K6 * _Dp * Math.Sqrt(_fbdi.p / (_fbdi.fi * _fbdi.sigma_d));
            _s1 = _s1p + _c;

            _Phi_1 = _Pbp / _fbdi.sigma_d;
            _Phi_2 = _Pbm / _fbdi.sigma_d;
            _Phi = Math.Max(_Phi_1, _Phi_2);

            _K7Fors2 = 0.8 * Math.Sqrt(_fbdi.D3 / _Dcp - 1);

            _s2p_1 = _K7Fors2 * Math.Sqrt(_Phi);
            _s2p_2 = 0.6 / _Dcp * _Phi;
            _s2p = Math.Max(_s2p_1, _s2p_2);
            _s2 = _s2p + _c;

            _K7Fors3 = 0.8 * Math.Sqrt(_fbdi.D3 / _fbdi.D2 - 1);

            _s3p_1 = _K7Fors3 * Math.Sqrt(_Phi);
            _s3p_2 = 0.6 / _Dcp * _Phi;
            _s3p = Math.Max(_s3p_1, _s3p_2);
            _s3 = _s3p + _c;


            if (_fbdi.s != 0.0)
            {
                if (_fbdi.s1 >= _s1 && _fbdi.s2 >= _s2 && _fbdi.s3 >= _s3)
                {
                    _conditionUseFormulas = (_fbdi.s1 - _c) / _Dp;
                    _Kp = _conditionUseFormulas <= 0.11
                        ? 1
                        : 2.2 / (1 + Math.Sqrt(1 + Math.Pow(6 * (_fbdi.s1 - _c) / _Dp, 2)));

                    _p_d = Math.Pow((_fbdi.s1 - _c) / (_K0 * _K6 * _Dp), 2) * _fbdi.sigma_d * _fbdi.fi;
                    if (_Kp * _p_d < _fbdi.p)
                    {
                        IsError = true;
                        ErrorList.Add("Допускаемое давление меньше расчетного");
                    }
                }
                else
                {
                    IsCriticalError = true;
                    ErrorList.Add("Принятая толщина s1, s2, s3 меньше расчетной");
                }
            }
        }

        public void MakeWord(string filename)
        {
            throw new NotImplementedException();
        }
    }
}
