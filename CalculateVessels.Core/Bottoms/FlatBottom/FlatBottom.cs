using CalculateVessels.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private double _Dp;
        private double _Qd;


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
                        ErrorList.Add("Условие закрепления не выполняется");
                    }
                    _K_1 = 0.41 * (1.0 - 0.23 * ((_fbdi.s - _c) / (_fbdi.s1 - _c)));
                    _K = Math.Max(_K_1, 0.35);
                    break;
                case 10:
                    if (_fbdi.gamma < 30 || _fbdi.gamma > 90 ||
                        _fbdi.r < 0.25 * _fbdi.s1 || _fbdi.r > (_fbdi.s1 - _fbdi.s2))
                    {
                        ErrorList.Add("Условие закрепления не выполняется");
                    }
                    goto case 4;
                case 11:
                    goto case 4;
                case 12:
                    _K = 0.4;
                    _Dp = _fbdi.D3;
                    break;
                case 13:
                    _K = 0.41;
                    _Dp = _fbdi.Dcp;
                    break;
                case 14:
                case 15:
                    _Dp = _fbdi.Dcp;
                    //_Pbp = 
                    _Qd = 0.785 * _fbdi.p * Math.Pow(_fbdi.Dcp, 2);
                    _psi1 = _Pbp / _Qd;
                    _K6 = 0.41 * Math.Sqrt((1 + 3 * _psi1 * (_fbdi.D3 / _fbdi.Dsp - 1)) / (_fbdi.D3 / _fbdi.Dsp));
                    break;
            }
            // UNDONE: доделать расчет плоского днища
            switch (_fbdi.otv)
            {
                case 0:
                    _K0 = 1;
                    break;

                case 1:
                    _K0 = Math.Sqrt(1 + _fbdi.d / _Dp + Math.Pow(_fbdi.d / _Dp, 2));
                    break;

                case 2:
                    if (_fbdi.di > 0.7 * _Dp)
                    {
                        ErrorList += "Слишком много отверстий\n";
                    }
                    _K0 = Math.Sqrt((1 - Math.Pow(_fbdi.di / _Dp, 3)) / (1 - _fbdi.di / _Dp));
                    break;
            }

            _s1_calcr = _K * _K0 * _Dp * Math.Sqrt(_fbdi.p / (_fbdi.fi * _fbdi.sigma_d));
            _s1_calc = _s1_calcr + _c;



            if (_fbdi.s1 != 0 && _fbdi.s1 >= _s1_calc)
            {
                _ypfzn = (_fbdi.s1 - _c) / _Dp;
                if (_ypfzn <= 0.11)
                {
                    _ypf = true;

                }
            }
            else if (_fbdi.s1 != 0 && _fbdi.s1 < _s1_calc)
            {
                ErrorList += "Принятая толщина меньше расчетной\n";
            }


            return d_out;

        }

        public void MakeWord(string filename)
        {
            throw new NotImplementedException();
        }
    }
}
