using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.DataIn;
using CalculateVessels.Core.Shells.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateVessels.Core.Shells
{
    public class ConicalShell : Shell, IElement
    {
        internal readonly ConicalShellDataIn _csdi;

        public ConicalShell(ConicalShellDataIn csdi)
        {
            _csdi = csdi;
            _cosAlfa1 = Math.Cos(_csdi.alfa1 * Math.PI / 180);
            _tgAlfa1 = Math.Tan(_csdi.alfa1 * Math.PI / 180);
            _sinAlfa1 = Math.Sin(_csdi.alfa1 * Math.PI / 180);
        }

        private readonly double _cosAlfa1;
        private readonly double _sinAlfa1;
        private readonly double _tgAlfa1;

        private int switch1;

        private double _Dk;

        private double _a1p;
        private double _a2p;

        private double _a1p_l;
        private double _a2p_l;

        private double _lE;
        private double _DE_1;
        private double _DE_2;
        private double _DE;
        private double _B1_1;
        private double _B1;

        private double _s_2p;
        private double _beta_1;
        private double _beta;
        private double _chi_1;

        internal double Dk { get => _Dk; }

        public void Calculate()
        {
            _c = _csdi.c1 + _csdi.c2 + _csdi.c3;

            // Condition use formuls
            {
                const double CONDITION_USE_FORMULS_FROM = 0.001;
                const double CONDITION_USE_FORMULS_TO = 0.05;

                isConditionUseFormuls = _csdi.s1 * _cosAlfa1 / _csdi.D >= CONDITION_USE_FORMULS_FROM
                    && _csdi.s1 * _cosAlfa1 / _csdi.D <= CONDITION_USE_FORMULS_TO;

                if (!isConditionUseFormuls)
                {
                    isError = true;
                    err.Add("Условие применения формул не выполняется");
                }
            }

            switch (_csdi.ConnectionType)
            {
                case ConicalConnectionType.Simply:
                case ConicalConnectionType.WithRing:
                    _a1p = 0.7 * Math.Sqrt(_csdi.D * (_csdi.s1 - _c) / _cosAlfa1);
                    _a2p = 0.7 * Math.Sqrt(_csdi.D * (_csdi.s2 - _c));
                    break;
                case ConicalConnectionType.Toroidal:
                    _a1p = 0.7 * Math.Sqrt(_csdi.D * (_csdi.sT - _c) / _cosAlfa1);
                    _a2p = 0.5 * Math.Sqrt(_csdi.D * (_csdi.sT - _c));
                    break;
            }

            if (_csdi.IsConnectionLittle)
            {
                _a1p_l = Math.Sqrt(_csdi.D1 * (_csdi.s1 - _c) / _cosAlfa1);
                _a2p_l = 1.25 * Math.Sqrt(_csdi.D1 * (_csdi.s2 - _c));
            }

            _Dk = _csdi.IsTorr
                ? _csdi.D - 2 * (_csdi.r * (1 - _cosAlfa1) + 0.7 * _a1p * _sinAlfa1)
                : _csdi.D - 1.4 * _a1p * _sinAlfa1;


            if (_csdi.p > 0)
            {
                if (_csdi.IsPressureIn)
                {

                    _s_calcr = _csdi.p * _Dk / (2 * _csdi.sigma_d * _csdi.fi - _csdi.p)
                        * (1 / _cosAlfa1);
                    _s_calc = _s_calcr + _c;
                    if (_csdi.s == 0.0 || _csdi.s >= _s_calc)
                    {
                        _p_d = 2 * _csdi.sigma_d * _csdi.fi * (_csdi.s - _c)
                            / (_Dk / _cosAlfa1 + (_csdi.s - _c));
                    }
                    else
                    {
                        isCriticalError = true;
                        err.Add("Принятая толщина меньше расчетной");
                    }
                }
                else
                {
                    _lE = (_csdi.D - _csdi.D1) / (2 * _cosAlfa1);
                    _DE_1 = (_csdi.D + _csdi.D1) / (2 * _cosAlfa1);
                    _DE_2 = _csdi.D / _cosAlfa1 - 0.3 * (_csdi.D + _csdi.D1)
                        * Math.Sqrt((_csdi.D + _csdi.D1) / ((_csdi.s - _c) * 100)) * _tgAlfa1;
                    _DE = Math.Max(_DE_1, _DE_2);
                    _B1_1 = 9.45 * _DE / _lE * Math.Sqrt(_DE / (100 * (_csdi.s - _c)));
                    _B1 = Math.Min(1.0, _B1_1);

                    _s_calcr1 = 1.06 * (0.01 * _DE / _B1)
                        * Math.Pow(_csdi.p / (0.00001 * _csdi.E) * (_lE / _DE), 0.4);
                    _s_calcr2 = 1.2 * _csdi.p * _Dk / (2 * _csdi.fi * _csdi.sigma_d - _csdi.p)
                        * (1 / _cosAlfa1);
                    _s_calcr = Math.Max(_s_calcr1, _s_calcr2);
                    _s_calc = _s_calcr + _c;

                    if (_csdi.s >= _s_calc)
                    {
                        _p_dp = 2 * _csdi.sigma_d * (_csdi.s - _c)
                            / (_Dk / _cosAlfa1 + _csdi.s - _c);
                        _p_de = 2.08 * 0.00001 * _csdi.E / (_csdi.ny * _B1) * (_DE / _lE)
                            * Math.Pow(100 * (_csdi.s - _c) / _DE, 2.5);
                        _p_d = _p_dp / Math.Sqrt(1 + Math.Pow(_p_dp / _p_de, 2));
                    }
                    else
                    {
                        isCriticalError = true;
                        err.Add("Принятая толщина меньше расчетной");
                    }
                }
                if (_p_d < _csdi.p && _csdi.s != 0)
                {
                    isError = true;
                    err.Add("[p] меньше p");
                }
                if (_csdi.IsConnectionBig)
                {
                    switch (_csdi.ConnectionType)
                    {
                        case ConicalConnectionType.Simply:
                            _chi_1 = _csdi.sigma_d_1 / _csdi.sigma_d_2;
                            _beta = 0.4 * Math.Sqrt(_csdi.D / (_csdi.s2 - _c)) * _tgAlfa1
                                / (1 + Math.Sqrt((1 + _chi_1
                                * Math.Pow((_csdi.s1 - _c) / (_csdi.s2 - _c), 2))
                                / (2 * _cosAlfa1) * _chi_1 * (_csdi.s1 - _c) / (_csdi.s2 - _c))) - 0.25;
                            _beta_1 = Math.Max(0.5, _beta);
                            _s_2p = _csdi.p * _csdi.D * _beta_1
                                / (2 * _csdi.sigma_d_2 * _csdi.fi - _csdi.p);

                            if (_csdi.s2 >= _s_2p + _c)
                            {
                                _p_d = 2 * _csdi.sigma_d_2 * _csdi.fi * (_csdi.s2 - _c)
                                    / (_csdi.D * _beta_1 + (_csdi.s2 - _c));
                            }
                            else
                            {
                                isCriticalError = true;
                                err.Add("Принятая толщина переходной зоны меньше расчетной");
                            }
                            break;
                        case ConicalConnectionType.WithRing:
                            break;
                        case ConicalConnectionType.Toroidal:
                            break;
                    }
                }

            }

            //if (_csdi.F > 0)
            //{
            //    _s_calcrf = _csdi.F / (Math.PI * _csdi.D * _csdi.sigma_d * _csdi.fit);
            //    _s_calcf = _s_calcrf + _c;
            //    if (_csdi.isFTensile)
            //    {
            //        _F_d = Math.PI * (_csdi.D + _csdi.s - _c) * (_csdi.s - _c) * _csdi.sigma_d * _csdi.fit;
            //    }
            //    else
            //    {
            //        _F_dp = Math.PI * (_csdi.D + _csdi.s - _c) * (_csdi.s - _c) * _csdi.sigma_d;
            //        _F_de1 = 0.000031 * _csdi.E / _csdi.ny * Math.Pow(_csdi.D, 2) * Math.Pow(100 * (_csdi.s - _c) / _csdi.D, 2.5);

            //        const int L_MORE_THEN_D = 10;
            //        bool isLMoreThenD = _csdi.l / _csdi.D > L_MORE_THEN_D;

            //        if (isLMoreThenD)
            //        {
            //            switch (_csdi.FCalcSchema)
            //            {
            //                case 1:
            //                    _lpr = _csdi.l;
            //                    break;
            //                case 2:
            //                    _lpr = 2 * _csdi.l;
            //                    break;
            //                case 3:
            //                    _lpr = 0.7 * _csdi.l;
            //                    break;
            //                case 4:
            //                    _lpr = 0.5 * _csdi.l;
            //                    break;
            //                case 5:
            //                    _F = _csdi.q * _csdi.l;
            //                    _lpr = 1.12 * _csdi.l;
            //                    break;
            //                case 6:
            //                    double fDivl6 = _csdi.F / _csdi.l;
            //                    fDivl6 *= 10;
            //                    fDivl6 = Math.Round(fDivl6 / 2);
            //                    fDivl6 *= 0.2;
            //                    switch (fDivl6)
            //                    {
            //                        case 0:
            //                            _lpr = 2 * _csdi.l;
            //                            break;
            //                        case 0.2:
            //                            _lpr = 1.73 * _csdi.l;
            //                            break;
            //                        case 0.4:
            //                            _lpr = 1.47 * _csdi.l;
            //                            break;
            //                        case 0.6:
            //                            _lpr = 1.23 * _csdi.l;
            //                            break;
            //                        case 0.8:
            //                            _lpr = 1.06 * _csdi.l;
            //                            break;
            //                        case 1:
            //                            _lpr = _csdi.l;
            //                            break;
            //                    }
            //                    break;
            //                case 7:
            //                    double fDivl7 = _csdi.F / _csdi.l;
            //                    fDivl7 *= 10;
            //                    fDivl7 = Math.Round(fDivl7 / 2);
            //                    fDivl7 *= 0.2;
            //                    switch (fDivl7)
            //                    {
            //                        case 0:
            //                            _lpr = 2 * _csdi.l;
            //                            break;
            //                        case 0.2:
            //                            _lpr = 1.7 * _csdi.l;
            //                            break;
            //                        case 0.4:
            //                            _lpr = 1.4 * _csdi.l;
            //                            break;
            //                        case 0.6:
            //                            _lpr = 1.11 * _csdi.l;
            //                            break;
            //                        case 0.8:
            //                            _lpr = 0.85 * _csdi.l;
            //                            break;
            //                        case 1:
            //                            _lpr = 0.7 * _csdi.l;
            //                            break;
            //                    }
            //                    break;

            //            }
            //            lamda = 2.83 * _lpr / (_csdi.D + _csdi.s - _c);
            //            _F_de2 = Math.PI * (_csdi.D + _csdi.s - _c) * (_csdi.s - _c) * _csdi.E / _csdi.ny *
            //                            Math.Pow(Math.PI / lamda, 2);
            //            _F_de = Math.Min(_F_de1, _F_de2);
            //        }
            //        else
            //        {
            //            _F_de = _F_de1;
            //        }

            //        _F_d = _F_dp / Math.Sqrt(1 + Math.Pow(_F_dp / _F_de, 2));
            //    }
            //}

            //if (_csdi.M > 0)
            //{
            //    _M_dp = Math.PI / 4 * _csdi.D * (_csdi.D + _csdi.s - _c) * (_csdi.s - _c) * _csdi.sigma_d;
            //    _M_de = 0.000089 * _csdi.E / _csdi.ny * Math.Pow(_csdi.D, 3) * Math.Pow(100 * (_csdi.s - _c) / _csdi.D, 2.5);
            //    _M_d = _M_dp / Math.Sqrt(1 + Math.Pow(_M_dp / _M_de, 2));
            //}

            //if (_csdi.Q > 0)
            //{
            //    _Q_dp = 0.25 * _csdi.sigma_d * Math.PI * _csdi.D * (_csdi.s - _c);
            //    _Q_de = 2.4 * _csdi.E * Math.Pow(_csdi.s - _c, 2) / _csdi.ny *
            //        (0.18 + 3.3 * _csdi.D * (_csdi.s - _c) / Math.Pow(_csdi.l, 2));
            //    _Q_d = _Q_dp / Math.Sqrt(1 + Math.Pow(_Q_dp / _Q_de, 2));
            //}

            //if ((_csdi.IsNeedpCalculate || _csdi.isNeedFCalculate) &&
            //    (_csdi.isNeedMCalculate || _csdi.isNeedQCalculate))
            //{
            //    conditionYstoich = _csdi.p / _p_d + _csdi.F / _F_d + _csdi.M / _M_d +
            //                            Math.Pow(_csdi.Q / _Q_d, 2);
            //}
        }

        public void MakeWord(string filename)
        {
            throw new NotImplementedException();
        }
    }


}
