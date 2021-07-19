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

        public FlatBottomWithAdditionalMoment(FlatBottomWithAdditionalMomentDataIn fbdi)
        {
            _fbdi = fbdi;
        }

        public bool IsCriticalError { get; private set; }

        public bool IsError { get; private set; }

        public List<string> ErrorList { get; private set; } = new();

        public List<string> Bibliography { get; } = new()
        {
            Data.Properties.Resources.GOST_34233_1,
            Data.Properties.Resources.GOST_34233_2
        };

        private double _c;
        private double _K;
        private double _K_1;
        private double _K0;
        private double _Kp;
        private double _Dp;
        private double _Qd;
        private double _s1p;
        private double _s1;
        private double _s2;
        private double _s2_1;
        private double _s2_2;
        private double _conditionUseFormulas;
        private double _p_d;
        private double _Pbp;
        private double _Pbm;
        private double _Pb1;
        private double _Pb2;
        private double _Pb1_1;
        private double _Pb1_2;
        private double _alfa;
        private double _alfa_m;
        private double _yp;
        private double _yb;
        private double _Lb;
        private double _fb;

        private double _m;
        private double _qobj;
        private double _q_d;
        private double _Kobj;
        private double _Ep;
        private double _Eb20;
        private double _E20;
        private double _Ekp20;
        private double _l0;
        private double _KGost34233_4;
        private double _betaT;
        private double _betaU;
        private double _betaY;
        private double _betaZ;
        private double _betaF;
        private double _betaV;
        private double _f;
        private double _lambda;
        private double _yF;
        private double _ykp;
        private double _Xkp;
        private double _Kkp;

        private double _Rp;

        public void Calculate()
        {
            _c = _fbdi.c1 + _fbdi.c2 + _fbdi.c3;

            switch (_fbdi.Type)
            {
                case 1:
                    _K = 0.53;
                    _Dp = _fbdi.D;
                    if (_fbdi.a < 1.7 * _fbdi.s)
                    {
                        IsError = true;
                        ErrorList.Add("Условие закрепления не выполняется a>=1.7s");
                    }
                    break;
                case 2:
                    _K = 0.50;
                    _Dp = _fbdi.D;
                    if (_fbdi.a < 0.85 * _fbdi.s)
                    {
                        IsError = true;
                        ErrorList.Add("Условие закрепления не выполняется a>=0.85s");
                    }
                    break;
                case 3:
                    _Dp = _fbdi.D;
                    _K = (_fbdi.s - _c) / (_fbdi.s1 - _c) < 0.25 ? 0.45 : 0.41;
                    break;
                case 4:
                    _Dp = _fbdi.D;
                    _K = (_fbdi.s - _c) / (_fbdi.s1 - _c) < 0.5 ? 0.41 : 0.38;
                    break;
                case 5:
                    goto case 3;
                case 6:
                    goto case 2;
                case 7:
                case 8:
                    goto case 4;
                case 9:
                    _Dp = _fbdi.D - 2 * _fbdi.r;
                    if (_fbdi.h1 < _fbdi.r ||
                        _fbdi.r < Math.Max(_fbdi.s, 025 * _fbdi.s1) ||
                        _fbdi.r > Math.Min(_fbdi.s1, 0.1 * _fbdi.D))
                    {
                        IsError = true;
                        ErrorList.Add("Условие закрепления не выполняется");
                    }
                    _K_1 = 0.41 * (1.0 - 0.23 * ((_fbdi.s - _c) / (_fbdi.s1 - _c)));
                    _K = Math.Max(_K_1, 0.35);
                    break;
                case 10:
                    if (_fbdi.gamma < 30 || _fbdi.gamma > 90 ||
                        _fbdi.r < 0.25 * _fbdi.s1 || _fbdi.r > _fbdi.s1 - _fbdi.s2)
                    {
                        IsError = true;
                        ErrorList.Add("Условие закрепления не выполняется");
                    }

                    _s2_1 = 1.1 * (_fbdi.s - _c);
                    _s2_2 = (_fbdi.s1 - _c) /
                            (1 + (_Dp - 2 * _fbdi.r) / (1.2 * (_fbdi.s1 - _c) * Math.Sin(_fbdi.gamma * Math.PI / 180)));
                    _s2 = Math.Max(_s2_1, _s2_2) + _c;
                    if (_fbdi.s2 < _s2)
                    {
                        IsError = true;
                        ErrorList.Add("Принятая толщина s2 меньше расчетной");
                    }
                    goto case 4;
                case 11:
                    _K = 0.4;
                    _Dp = _fbdi.D3;
                    _s2_1 = 0.7 * (_fbdi.s1 - _c);
                    _s2_2 = (_fbdi.s1 - _c) * Math.Sqrt(2 * (_Dp - _fbdi.D2) * _fbdi.D2 / Math.Pow(_fbdi.D2, 2));
                    _s2 = Math.Max(_s2_1, _s2_2) + _c;
                    if (_fbdi.s2 < _s2)
                    {
                        IsError = true;
                        ErrorList.Add("Принятая толщина s2 меньше расчетной");
                    }
                    break;
                case 12:
                    _K = 0.41;
                    _Dp = _fbdi.Dcp;
                    break;
                case 13:
                case 14:

                    //_fb = Physical.Gost34233_4.Getfb(_fbdi.BWM.ScrewM, _fbdi.BWM.IsScrewGroove);


                    //_Eb20 = Physical.Gost34233_4.GetE(_fbdi.BWM.ScrewSteel, 20);
                    //_E20 = Physical.Gost34233_4.GetE(_fbdi.BWM.FlangeSteel, 20);

                    //List<string> errorList = new();
                    //_Ekp20 = Physical.Gost34233_1.GetE(_fbdi.BWM.CoverSteel, 20, ref errorList);

                    //if (errorList.Any())
                    //{
                    //    IsCriticalError = true;
                    //    ErrorList = ErrorList.Concat(errorList).ToList();
                    //    return;
                    //}

                    //if (_fb == 0 || _Eb20 == 0)
                    //{
                    //    IsCriticalError = true;
                    //    ErrorList.Add("Ошибка получения значений физических велечин");
                    //    return;
                    //}

                    //if (_fbdi.BWM.IsMetall)
                    //{
                    //    (_m, _qobj, _, _, _) = Physical.Gost34233_4.GetGasketParameters(_fbdi.BWM.GasketType);
                    //}
                    //else
                    //{
                    //    (_m, _qobj, _q_d, _Kobj, _Ep) = Physical.Gost34233_4.GetGasketParameters(_fbdi.BWM.GasketType);
                    //}

                    //_Dp = _fbdi.Dcp;
                    //_yp = _fbdi.BWM.IsMetall
                    //    ? 0 : _fbdi.BWM.hp * _Kobj / (_Ep * Math.PI * _fbdi.Dcp * _fbdi.BWM.bp);

                    //_Lb = _fbdi.BWM.Lb0 + (_fbdi.BWM.IsStud ? 0.56 : 0.28) * _fbdi.BWM.d;
                    //_yb = _Lb / (_Eb20 * _fb * _fbdi.BWM.n);
                    //_l0 = Math.Sqrt(_fbdi.D * _fbdi.BWM.S0);
                    //_KGost34233_4 = _fbdi.BWM.Dn / _fbdi.D;
                    //_betaT = (Math.Pow(_KGost34233_4, 2) * (1 + 8.55 * Math.Log(_KGost34233_4)) - 1) /
                    //         (1.05 + 1.945 * Math.Pow(_KGost34233_4, 2) * (_KGost34233_4 - 1));
                    //_betaU = (Math.Pow(_KGost34233_4, 2) * (1 + 8.55 * Math.Log(_KGost34233_4)) - 1) /
                    //         (1.36 * (Math.Pow(_KGost34233_4, 2) - 1) * (_KGost34233_4 - 1));
                    //_betaY = 1 / (_KGost34233_4 - 1) *
                    //         (0.69 + 5.72 * Math.Pow(_KGost34233_4, 2) * Math.Log(_KGost34233_4) /
                    //             (Math.Pow(_KGost34233_4, 2) - 1));
                    //_betaZ = (Math.Pow(_KGost34233_4, 2) + 1) / (Math.Pow(_KGost34233_4, 2) - 1);
                    //_betaF = _fbdi.BWM.S1 / _fbdi.BWM.S0;
                    //_betaF = 0.91;
                    //_betaV = 0.55;
                    //_f = 1.0;
                    ////TODO: _betaF, _betaV, _f take values from diagram. how?
                    //_lambda = _betaF * _fbdi.BWM.h + _l0 / (_betaT * _l0) +
                    //          _betaV * Math.Pow(_fbdi.BWM.h, 3) / (_betaU * _l0 * Math.Pow(_fbdi.BWM.S0, 2));
                    //_yF = 0.91 * _betaV / (_E20 * _lambda * Math.Pow(_fbdi.BWM.S0, 2) * _l0);
                    //_Kkp = _fbdi.BWM.Dn / _fbdi.Dcp;
                    //_Xkp = 0.67 * (Math.Pow(_Kkp, 2) * (1 + 8.55 * Math.Log(_Kkp)) - 1) /
                    //    ((_Kkp - 1) * (Math.Pow(_Kkp, 2) - 1 + (1.857 * Math.Pow(_Kkp, 2) + 1) * Math.Pow(_hkp, 3) / Math.Pow(_dkp, 3)));
                    //_ykp = _Xkp / (_Ekp20 * Math.Pow(_deltakp, 3));
                    //_alfa = 1 - (_yp - (_yF * _e + _ykp * _b) * _b) /
                    //    (_yp + _yb + (_yF + _ykp) * Math.Pow(_b, 2));
                    //_Qd = 0.785 * _fbdi.p * Math.Pow(_fbdi.Dcp, 2);
                    //_Rp = Math.PI * _fbdi.Dcp * _fbdi.BWM.b0 * _m * Math.Abs(_fbdi.p);
                    //_Pb1_1 = _alfa * (_Qd + _fbdi.BWM.F) + _Rp + 4 * alfa_m * Math.Abs(_M) / _Dcp;
                    //_Pb1 = Math.Max(_Pb1_1, _Pb1_2);
                    //_Pbm = Math.Max(_Pb1, _Pb2);
                    //_Pbp = _Pbm + (1 - alfa)(_Qd + F) + Q_1 + 4 * (1 - alfa_m) * Math.Abs(_M) / _Dcp;
                    //_psi1 = _Pbp / _Qd;
                    //_K6 = 0.41 * Math.Sqrt((1 + 3 * _psi1 * (_fbdi.D3 / _fbdi.Dsp - 1)) / (_fbdi.D3 / _fbdi.Dsp));
                    break;
            }
            // UNDONE: доделать расчет плоского днища
            switch (_fbdi.Hole)
            {
                case HoleInFlatBottom.WithoutHole:
                    _K0 = 1;
                    break;
                case HoleInFlatBottom.OneHole:
                    //_K0 = Math.Sqrt(1.0 + _fbdi.d / _Dp + Math.Pow(_fbdi.d / _Dp, 2));
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

            _s1p = _K * _K0 * _Dp * Math.Sqrt(_fbdi.p / (_fbdi.fi * _fbdi.sigma_d));
            _s1 = _s1p + _c;

            if (_fbdi.s != 0.0)
            {
                if (_fbdi.s1 >= _s1p)
                {
                    _conditionUseFormulas = (_fbdi.s1 - _c) / _Dp;
                    _Kp = _conditionUseFormulas <= 0.11
                        ? 1
                        : 2.2 / (1 + Math.Sqrt(1 + Math.Pow(6 * (_fbdi.s1 - _c) / _Dp, 2)));

                    _p_d = Math.Pow((_fbdi.s1 - _c) / (_K * _K0 * _Dp), 2) * _fbdi.sigma_d * _fbdi.fi;
                    if (_Kp * _p_d < _fbdi.p)
                    {
                        IsError = true;
                        ErrorList.Add("Допускаемое давление меньше расчетного");
                    }
                }
                else
                {
                    IsCriticalError = true;
                    ErrorList.Add("Принятая толщина s1 меньше расчетной");
                }
            }
        }

        public void MakeWord(string filename)
        {
            throw new NotImplementedException();
        }
    }
}
