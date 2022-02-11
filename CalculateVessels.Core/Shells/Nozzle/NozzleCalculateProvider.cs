using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalculateVessels.Core.Exceptions;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Base;
using CalculateVessels.Core.Shells.Conical;
using CalculateVessels.Core.Shells.Elliptical;
using CalculateVessels.Core.Shells.Enums;
using CalculateVessels.Core.Shells.Nozzle.Enums;

namespace CalculateVessels.Core.Shells.Nozzle
{
    internal class NozzleCalculateProvider : ICalculateProvider
    {
        public ICalculatedData Calculate(IInputData inputData)
        {
            if (inputData is not NozzleInputData dataIn)
                throw new CalculateException("Error. Input data wrong type.");

            var data = new NozzleCalculatedData(dataIn);


            data.c = ((Shell)dataIn.Element).c;

            data.SigmaAllowShell = ((Shell)dataIn.Element).SigmaAllow;

            if (!dataIn.ShellDataIn.IsPressureIn) data.p_deShell = ((Shell)dataIn.Element).p_de;

            switch (dataIn.ShellDataIn.ShellType)
            {
                case ShellType.Conical:
                    data.Dk = ((ConicalShellCalculatedData)dataIn.Element.CalculatedData).Dk;
                    data.alfa1 = ((ConicalShellInputData)dataIn.ShellDataIn).alfa1;
                    break;
                case ShellType.Elliptical:
                    data.EllipseH = ((EllipticalShellInputData)dataIn.ShellDataIn).EllipseH;
                    break;
            }

            // расчет Dp, dp
            switch (dataIn.ShellDataIn.ShellType)
            {
                case ShellType.Cylindrical:
                    {
                        data.Dp = dataIn.ShellDataIn.D;
                        break;
                    }
                case ShellType.Conical:
                    {
                        data.Dp = data.Dk / Math.Cos(Math.PI * 100 * data.alfa1);
                        break;
                    }
                case ShellType.Elliptical:
                    {
                        if (Math.Abs(data.EllipseH * 100 - dataIn.ShellDataIn.D * 25) < 0.000001)
                        {
                            data.Dp = dataIn.ShellDataIn.D * 2 * Math.Sqrt(1.0 - 3.0 * Math.Pow(dataIn.ellx / dataIn.ShellDataIn.D, 2));
                        }
                        else
                        {
                            data.Dp = Math.Pow(dataIn.ShellDataIn.D, 2) / (data.EllipseH * 2) *
                                Math.Sqrt(1.0 - 4.0 * (Math.Pow(dataIn.ShellDataIn.D, 2) - 4 *
                                Math.Pow(data.EllipseH, 2)) * Math.Pow(dataIn.ellx, 2) /
                                    Math.Pow(dataIn.ShellDataIn.D, 4));
                        }
                        break;
                    }
                case ShellType.Spherical:
                case ShellType.Torospherical:
                    {
                        //data.Dp = 2 * dataIn.ShellDataIn.R;
                        break;
                    }
                default:
                {
                    throw new CalculateException("Ошибка вида укрепляемого элемента.");
                }
            }

            if (dataIn.ShellDataIn.ShellType == ShellType.Elliptical && dataIn.ShellDataIn.IsPressureIn)
            {
                data.sp = dataIn.ShellDataIn.p * data.Dp / (4.0 * dataIn.ShellDataIn.fi * data.SigmaAllowShell - dataIn.ShellDataIn.p);
            }
            else
            {
                data.sp = ((Shell)dataIn.Element).s_p;
            }

            if (!dataIn.IsOval)
            {
                data.s1p = dataIn.ShellDataIn.p * (dataIn.d + 2 * dataIn.cs) / (2.0 * dataIn.fi * dataIn.sigma_d1 - dataIn.ShellDataIn.p);
            }
            else
            {
                data.s1p = dataIn.ShellDataIn.p * (dataIn.d1 + 2 * dataIn.cs) / (2.0 * dataIn.fi * dataIn.sigma_d1 - dataIn.ShellDataIn.p);
            }

            switch (dataIn.Location)
            {
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_1:
                    {
                        data.dp = dataIn.d + 2 * dataIn.cs; //dp = d + 2cs
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_2:
                    {
                        data.dp = Math.Max(dataIn.d, 0.5 * dataIn.tTransversely) + 2.0 * dataIn.cs; //dp =max{d; 0,5t} + 2cs
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_3:
                    {
                        data.dp = (dataIn.d + 2.0 * dataIn.cs) / Math.Sqrt(1 + Math.Pow(2 * dataIn.ellx / data.Dp, 2));
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_4:
                    {
                        data.dp = (dataIn.d + 2.0 * dataIn.cs) * (1 + Math.Pow(Math.Tan(Math.PI * 180 * dataIn.gamma), 2) *
                            Math.Pow(Math.Cos(Math.PI * 180 * dataIn.omega), 2));
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_5:
                    {
                        data.dp = (dataIn.d + 2.0 * dataIn.cs) / Math.Pow(Math.Cos(Math.PI * 180 * dataIn.gamma), 2);
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_6:
                    {
                        data.dp = (dataIn.d2 + 2.0 * dataIn.cs) *
                            (Math.Pow(Math.Sin(Math.PI * 180 * dataIn.omega), 2) +
                            (dataIn.d1 + 2 * dataIn.cs) *
                            (dataIn.d1 + dataIn.d2 + 4 * dataIn.cs) /
                            (2 * Math.Pow(dataIn.d2 + 2 * dataIn.cs, 2)) *
                            Math.Pow(Math.Cos(Math.PI * 180 * dataIn.omega), 2));
                        break;
                    }
                case NozzleLocation.LocationAccordingToParagraph_5_2_2_7:
                    {
                        data.dp = dataIn.d + 1.5 * (dataIn.r - data.sp) + 2.0 * dataIn.cs;
                        break;
                    }
                default:
                    {
                        throw new CalculateException("Ошибка места расположения штуцера.");
                    }
            }

            // l1p, l3p, l2p
            {

                var d = !dataIn.IsOval ? dataIn.d : dataIn.d2;

                data.l1p2 = 1.25 * Math.Sqrt((d + 2.0 * dataIn.cs) * (dataIn.s1 - dataIn.cs));
                data.l1p = Math.Min(dataIn.l1, data.l1p2);
                if (dataIn.s3 == 0)
                {
                    data.l3p = 0;
                }
                else
                {
                    data.l3p2 = 0.5 * Math.Sqrt((d + 2 * dataIn.cs) * (dataIn.s3 - dataIn.cs - dataIn.cs1));
                    data.l3p = Math.Min(dataIn.l3, data.l3p2);
                }
            }

            data.L0 = Math.Sqrt(data.Dp * (dataIn.ShellDataIn.s - data.c));

            switch (dataIn.NozzleKind)
            {
                case NozzleKind.ImpassWithoutRing:
                case NozzleKind.PassWithoutRing:
                case NozzleKind.ImpassWithRing:
                case NozzleKind.PassWithRing:
                case NozzleKind.WithRingAndInPart:
                case NozzleKind.WithFlanging:
                    data.lp = data.L0;
                    break;
                case NozzleKind.WithTorusshapedInsert:
                case NozzleKind.WithWealdedRing:
                    data.lp = Math.Min(dataIn.l, data.L0);
                    break;
            }

            data.l2p2 = Math.Sqrt(data.Dp * (dataIn.s2 + dataIn.ShellDataIn.s - data.c));
            data.l2p = Math.Min(dataIn.l2, data.l2p2);

            switch (dataIn.NozzleKind)
            {
                case NozzleKind.ImpassWithoutRing:
                case NozzleKind.PassWithoutRing:
                case NozzleKind.ImpassWithRing:
                case NozzleKind.PassWithRing:
                case NozzleKind.WithRingAndInPart:
                case NozzleKind.WithFlanging:
                    dataIn.s0 = dataIn.ShellDataIn.s;
                    //dataIn.steel4 = dataIn.steel1;
                    break;
            }


            data.psi1 = Math.Min(1, dataIn.sigma_d1 / data.SigmaAllowShell);
            data.psi2 = Math.Min(1, dataIn.sigma_d2 / data.SigmaAllowShell);
            data.psi3 = Math.Min(1, dataIn.sigma_d3 / data.SigmaAllowShell);

            data.psi4 = dataIn.NozzleKind switch
            {
                NozzleKind.WithTorusshapedInsert or NozzleKind.WithWealdedRing =>
                Math.Min(1, dataIn.sigma_d4 / data.SigmaAllowShell),
                _ => 1,
            };

            data.d0p = 0.4 * Math.Sqrt(data.Dp * (dataIn.ShellDataIn.s - data.c));

            data.b = Math.Sqrt(data.Dp * (dataIn.ShellDataIn.s - data.c)) + Math.Sqrt(data.Dp * (dataIn.ShellDataIn.s - data.c));

            switch (dataIn.ShellDataIn.ShellType)
            {
                case ShellType.Cylindrical:
                    {
                        data.dmax = dataIn.ShellDataIn.D;
                        break;
                    }
                case ShellType.Conical:
                    {
                        data.dmax = data.Dk;
                        break;
                    }
                case ShellType.Elliptical:
                case ShellType.Spherical:
                case ShellType.Torospherical:
                    {
                        data.dmax = 0.6 * dataIn.ShellDataIn.D;
                        break;
                    }
            }

            switch (dataIn.ShellDataIn.ShellType)
            {
                case ShellType.Cylindrical:
                case ShellType.Conical:
                    {
                        data.K1 = 1;
                        break;
                    }
                case ShellType.Elliptical:
                case ShellType.Spherical:
                case ShellType.Torospherical:
                    {
                        data.K1 = 2;
                        break;
                    }
            }

            if (dataIn.ShellDataIn.IsPressureIn)
            {
                data.spn = data.sp;
            }
            else
            {

                //data.B1n = Math.Min(1, 9.45 * (dataIn.ShellDataIn.D / ddata.out.l) * Math.Sqrt(dataIn.ShellDataIn.D / (100 * (dataIn.ShellDataIn.s - data.c))));
                //data.pen = 2.08 * 0.00001 * dataIn.ShellDataIn.E / (dataIn.ny * data.B1n) * (dataIn.ShellDataIn.D / ddata.out.l) * Math.Pow(100 * (dataIn.ShellDataIn.s - data.c) / dataIn.ShellDataIn.D, 2.5);
                data.pen = data.p_deShell;
                data.ppn = dataIn.ShellDataIn.p / Math.Sqrt(1.0 - Math.Pow(dataIn.ShellDataIn.p / data.pen, 2));
                data.spn = data.ppn * data.Dp / (2.0 * data.K1 * data.SigmaAllowShell - data.ppn);
            }


            data.d01 = 2 * ((dataIn.ShellDataIn.s - data.c) / data.spn - 0.8) * Math.Sqrt(data.Dp * (dataIn.ShellDataIn.s - data.c));
            data.d02 = data.dmax + 2 * dataIn.cs;
            data.d0 = Math.Min(data.d01, data.d02);

            if (data.dp > data.d0)
            {
                data.ConditionStrengthening1 = data.l1p * (dataIn.s1 - data.s1p - dataIn.cs) * data.psi1 + data.l2p * dataIn.s2 * data.psi2 + data.l3p * (dataIn.s3 - dataIn.cs - dataIn.cs1) * data.psi3 + data.lp * (dataIn.ShellDataIn.s - data.sp - data.c) * data.psi4;
                data.ConditionStrengthening2 = 0.5 * (data.dp - data.d0p) * data.sp;
                if (data.ConditionStrengthening1 < data.ConditionStrengthening2)
                {
                    data.ErrorList.Add("Условие укрепления одиночного отверстия не выполняется");
                }
            }


            data.V1 = (dataIn.s0 - data.c) / (dataIn.ShellDataIn.s - data.c);
            data.V2 = (data.psi4 + (data.l1p * (dataIn.s1 - dataIn.cs) * data.psi1 + data.l2p * dataIn.s2 * data.psi2 + data.l3p * (dataIn.s3 - dataIn.cs - dataIn.cs1) * data.psi3) / data.lp * (dataIn.ShellDataIn.s - data.c)) /
                  (1.0 + 0.5 * (data.dp - data.d0p) / data.lp + data.K1 * (dataIn.d + 2 * dataIn.cs) / data.Dp * (dataIn.fi / dataIn.fi1) * (data.l1p / data.lp));
            data.V = Math.Min(data.V1, data.V2);

            if (dataIn.ShellDataIn.IsPressureIn)
            {
                data.p_d = 2 * data.K1 * dataIn.fi * data.SigmaAllowShell * (dataIn.ShellDataIn.s - data.c) * data.V /
                       (data.Dp + (dataIn.ShellDataIn.s - data.c) * data.V);
            }
            else
            {
                data.p_dp = 2 * data.K1 * dataIn.fi * data.SigmaAllowShell * (dataIn.ShellDataIn.s - data.c) * data.V /
                        (data.Dp + (dataIn.ShellDataIn.s - data.c) * data.V);
                data.p_de = data.p_deShell;
                data.p_d = data.p_dp / Math.Sqrt(1 + Math.Pow(data.p_dp / data.p_de, 2));
            }
            if (data.p_d < dataIn.ShellDataIn.p)
            {
                data.ErrorList.Add("Допускаемое давление меньше расчетного.");
            }

            switch (dataIn.ShellDataIn.ShellType)
            {
                case ShellType.Cylindrical:
                    {
                        data.ConditionUseFormulas1 = (data.dp - 2 * dataIn.cs) / dataIn.ShellDataIn.D;
                        data.ConditionUseFormulas2 = (dataIn.ShellDataIn.s - data.c) / dataIn.ShellDataIn.D;
                        data.IsConditionUseFormulas = data.ConditionUseFormulas1 <= 1 & data.ConditionUseFormulas2 <= 0.1;
                        break;
                    }
                case ShellType.Conical:
                    {
                        data.ConditionUseFormulas1 = (data.dp - 2 * dataIn.cs) / data.Dk;
                        data.ConditionUseFormulas2 = (dataIn.ShellDataIn.s - data.c) / data.Dk;
                        data.ConditionUseFormulas2_2 = 0.1 / Math.Cos(Math.PI * 180 * data.alfa1);
                        data.IsConditionUseFormulas = data.ConditionUseFormulas1 <= 1 &
                                                 data.ConditionUseFormulas2 <= data.ConditionUseFormulas2_2;
                        break;
                    }
                case ShellType.Elliptical:
                case ShellType.Spherical:
                case ShellType.Torospherical:
                    {
                        data.ConditionUseFormulas1 = (data.dp - 2 * dataIn.cs) / dataIn.ShellDataIn.D;
                        data.ConditionUseFormulas2 = (dataIn.ShellDataIn.s - data.c) / dataIn.ShellDataIn.D;
                        data.IsConditionUseFormulas = data.ConditionUseFormulas1 <= 0.6 & data.ConditionUseFormulas2 <= 0.1;
                        break;
                    }
            }
            if (!data.IsConditionUseFormulas)
            {
                data.ErrorList.Add("Условие применения формул не выполняется.");
            }

            return data;
        }
    }
}
