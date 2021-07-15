using CalculateVessels.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalculateVessels.Core.Bottoms.Enums;

namespace CalculateVessels.Core.Bottoms.FlatBottom
{
    public class FlatBottom : IElement
    {
        private readonly FlatBottomDataIn _fbdi;

        public FlatBottom(FlatBottomDataIn fbdi)
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
                        _fbdi.r < 0.25 * _fbdi.s1 || _fbdi.r > (_fbdi.s1 - _fbdi.s2))
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
                    _Dp = _fbdi.Dcp;
                    _yp = _fbdi.BWM.IsMetall
                        ? 0 : _fbdi.BWM.hp * _fbdi.BWM.Kobj / (_fbdi.BWM.Ep * Math.PI * _fbdi.Dcp * _fbdi.BWM.bp);

                    _Lb = _fbdi.BWM.Lb0 + (_fbdi.BWM.IsStud ? 0.56 : 0.28) * _fbdi.BWM.d;
                    _yb = _Lb / (_Eb20 * _fb * _n);
                    _alfa = 1 - (_yp - (_yf * _e + _ykp * _b) * _b) /
                        (_yp + _yb + (_yf + _ykp) * Math.Pow(_b, 2));
                    _Qd = 0.785 * _fbdi.p * Math.Pow(_fbdi.Dcp, 2);
                    _Pb1_1 = _alfa * (_Qd + _fbdi.BWM.F) + _Rp + 4 * alfa_m * Math.Abs(_M) / _Dcp;
                    _Pb1 = Math.Max(_Pb1_1, _Pb1_2);
                    _Pbm = Math.Max(_Pb1, _Pb2);
                    _Pbp = _Pbm + (1 - alfa)(_Qd + F) + Q_1 + 4 * (1 - alfa_m) * Math.Abs(_M) / _Dcp;
                    _psi1 = _Pbp / _Qd;
                    _K6 = 0.41 * Math.Sqrt((1 + 3 * _psi1 * (_fbdi.D3 / _fbdi.Dsp - 1)) / (_fbdi.D3 / _fbdi.Dsp));
                    break;
            }
            // UNDONE: доделать расчет плоского днища
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
