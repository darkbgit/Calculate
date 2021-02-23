using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calcNet
{
    enum EllipticalBottomType
    {
        Elliptical,
        Hemispherical
    }

    class EllipticalShell : Shell, IElement
    {
        public EllipticalShell(EllipticalShellDataIn ellipticalShellDataIn)
        {
            _esdi = ellipticalShellDataIn;
        }

        private readonly EllipticalShellDataIn _esdi;

        private double _ellR;
        private double _ellKe;
        private double _ellx;

        public bool IsCriticalError { get => isCriticalError; }

        public void Calculate()
        {
            //Data_out d_out = new Data_out { err = "" };
            _c = _esdi.c1 + _esdi.c2 + _esdi.c3;

            //Condition use formuls
            {
                const double CONDITION_USE_FORMULS_1_MIN = 0.002,
                            CONDITION_USE_FORMULS_1_MAX = 0.1,
                            CONDITION_USE_FORMULS_2_MIN = 0.2,
                            CONDITION_USE_FORMULS_2_MAX = 0.5;

                if ((((_esdi.s - _c) / _esdi.D <= CONDITION_USE_FORMULS_1_MAX) &
                    ((_esdi.s - _c) / _esdi.D >= CONDITION_USE_FORMULS_1_MIN) &
                    (_esdi.ellH / _esdi.D < CONDITION_USE_FORMULS_2_MAX) &
                    (_esdi.ellH / _esdi.D >= CONDITION_USE_FORMULS_2_MIN)) |
                    _esdi.s == 0)
                {
                    isConditionUseFormuls = true;
                }
                else
                {
                    isError = true;
                    isConditionUseFormuls = false;
                    err.Add("Условие применения формул не выполняется");
                }
            }
            _ellR = Math.Pow(_esdi.D, 2) / (4 * _esdi.ellH);
            if (_esdi.IsPressureIn)
            {
                _s_calcr = _esdi.p * _ellR / ((2 * _esdi.sigma_d * _esdi.fi) - 0.5 * _esdi.p);
                _s_calc = _s_calcr + _c;

                if (_esdi.s == 0.0)
                {
                    _p_d = 2 * _esdi.sigma_d * _esdi.fi * (_s_calc - _c) / (_ellR + 0.5 * (_s_calc - _c));
                }
                else if (_esdi.s >= _s_calc)
                {
                    _p_d = 2 * _esdi.sigma_d * _esdi.fi * (_esdi.s - _c) / (_ellR + 0.5 * (_s_calc - _c));
                }
                else
                {
                    isCriticalError = true;
                    err.Add("Принятая толщина меньше расчетной");
                }
            }
            else
            {
                _s_calcr2 = 1.2 * _esdi.p * _ellR / (2 * _esdi.sigma_d);

                switch (_esdi.ellipticalBottomType)
                {
                    case EllipticalBottomType.Elliptical:
                        _ellKe = 0.9;
                        break;
                    case EllipticalBottomType.Hemispherical:
                        _ellKe = 1;
                        break;
                }
                _s_calcr1 = _ellKe * _ellR / 161 * Math.Sqrt(_esdi.ny * _esdi.p / (0.00001 * _esdi.E));
                _s_calcr = Math.Max(_s_calcr1, _s_calcr2);
                _s_calc = _s_calcr + _c;
                if (_esdi.s == 0.0)
                {
                    //_elke = 0.9; // # добавить ке для полусферических =1
                    _s_calcr1 = (_ellKe * _ellR) / 161 * Math.Sqrt((_esdi.ny * _esdi.p) / (0.00001 * _esdi.E));
                    _s_calcr = Math.Max(_s_calcr1, _s_calcr2);
                    //#_p_dp = 2*_esdi.sigma_d*(_s_calc-_c)/(_elR + 0.5 * (_s_calc-_c))
                    //#_elx = 10 * ((_esdi.s-_c)/_esdi.D)*(_esdi.D/(2*_esdi.elH)-(2*_esdi.elH)/_esdi.D)
                    //_elke = (1 + (2.4 + 8 * _elx)*_elx)/(1+(3.0+10*_elx)*_elx)
                    //#_p_de = (2.6*0.00001*_esdi.E)/_esdi.ny*Math.Pow(100*(_s-_c)/(_elke*_elR,2))
                    //#_p_d = _p_dp/Math.Sqrt(1+Math.Pow(_p_dp/_p_de,2))
                }
                else if (_esdi.s >= _s_calc)
                {
                    _p_dp = 2 * _esdi.sigma_d * (_esdi.s - _c) / (_ellR + 0.5 * (_esdi.s - _c));
                    _ellx = 10 * ((_esdi.s - _c) / _esdi.D) * (_esdi.D / (2 * _esdi.ellH) - (2 * _esdi.ellH) / _esdi.D);
                    _ellKe = (1 + (2.4 + 8 * _ellx) * _ellx) / (1 + (3.0 + 10 * _ellx) * _ellx);
                    _p_de = (2.6 * 0.00001 * _esdi.E) / _esdi.ny * Math.Pow(100 * (_esdi.s - _c) / (_ellKe * _ellR), 2);
                    _p_d = _p_dp / Math.Sqrt(1 + Math.Pow(_p_dp / _p_de, 2));
                }
                else
                {
                    isCriticalError = true;
                    err.Add("Принятая толщина меньше расчетной");
                }
            }

        }

    }
}
