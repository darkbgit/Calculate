using CalculateVessels.Core.Exceptions;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Base;
using CalculateVessels.Core.Shells.Conical;
using CalculateVessels.Core.Shells.Elliptical;
using CalculateVessels.Core.Shells.Enums;
using CalculateVessels.Core.Shells.Nozzle.Enums;
using CalculateVessels.Data.Exceptions;
using CalculateVessels.Data.PhysicalData;
using CalculateVessels.Data.PhysicalData.Gost34233_1;
using System;

namespace CalculateVessels.Core.Shells.Nozzle
{
    internal class NozzleCalculateProvider : ICalculateProvider
    {
        public ICalculatedData Calculate(IInputData inputData)
        {
            if (inputData is not NozzleInputData dataIn)
                throw new CalculateException("Error. Input data wrong type.");

            var data = new NozzleCalculatedData(dataIn);


            if (dataIn.SigmaAllow1 == 0)
            {
                try
                {
                    data.SigmaAllow1 = Gost34233_1.GetSigma(dataIn.steel1, dataIn.t);
                }
                catch (PhysicalDataException e)
                {
                    throw new CalculateException("Error get sigma.", e);
                }
            }
            else
            {
                data.SigmaAllow1 = dataIn.SigmaAllow1;
            }

            if (dataIn.E1 == 0)
            {
                try
                {
                    data.E1 = Physical.GetE(dataIn.steel1, dataIn.t);
                }
                catch (PhysicalDataException e)
                {
                    throw new CalculateException("Error get E.", e);
                }
            }
            else
            {
                data.E1 = dataIn.E1;
            }

            if (dataIn.NozzleKind is NozzleKind.ImpassWithRing or NozzleKind.PassWithRing
                or NozzleKind.WithRingAndInPart)
            {

                try
                {
                    data.SigmaAllow2 = Gost34233_1.GetSigma(dataIn.steel2, dataIn.t);
                }
                catch (PhysicalDataException e)
                {
                    throw new CalculateException("Error get sigma.", e);
                }

                try
                {
                    data.E2 = Physical.GetE(dataIn.steel2, dataIn.t);
                }
                catch (PhysicalDataException e)
                {
                    throw new CalculateException("Error get E.", e);
                }
            }

            if (dataIn.NozzleKind is NozzleKind.PassWithoutRing or NozzleKind.PassWithRing or NozzleKind.WithRingAndInPart)
            {
                try
                {
                    data.SigmaAllow3 = Gost34233_1.GetSigma(dataIn.steel3, dataIn.t);
                }
                catch (PhysicalDataException e)
                {
                    throw new CalculateException("Error get sigma.", e);
                }

                try
                {
                    data.E3 = Physical.GetE(dataIn.steel3, dataIn.t);
                }
                catch (PhysicalDataException e)
                {
                    throw new CalculateException("Error get E.", e);
                }
            }

            //TODO: steel4

            var shellDataIn = (ShellInputData)dataIn.ShellCalculatedData.InputData;

            data.c = ((ShellCalculatedData)dataIn.ShellCalculatedData).c;

            data.SigmaAllowShell = ((ShellCalculatedData)dataIn.ShellCalculatedData).SigmaAllow;

            if (!shellDataIn.IsPressureIn) data.p_deShell = ((ShellCalculatedData)dataIn.ShellCalculatedData).p_de;

            switch (shellDataIn.ShellType)
            {
                case ShellType.Conical:
                    data.Dk = ((ConicalShellCalculatedData)dataIn.ShellCalculatedData).Dk;
                    data.alfa1 = ((ConicalShellInputData)shellDataIn).alfa1;
                    break;
                case ShellType.Elliptical:
                    data.EllipseH = ((EllipticalShellInputData)shellDataIn).EllipseH;
                    break;
            }

            // расчет Dp, dp
            switch (shellDataIn.ShellType)
            {
                case ShellType.Cylindrical:
                    {
                        data.Dp = shellDataIn.D;
                        break;
                    }
                case ShellType.Conical:
                    {
                        data.Dp = data.Dk / Math.Cos(Math.PI * 100 * data.alfa1);
                        break;
                    }
                case ShellType.Elliptical:
                    {
                        if (Math.Abs(data.EllipseH * 100 - shellDataIn.D * 25) < 0.000001)
                        {
                            data.Dp = shellDataIn.D * 2 * Math.Sqrt(1.0 - 3.0 * Math.Pow(dataIn.ellx / shellDataIn.D, 2));
                        }
                        else
                        {
                            data.Dp = Math.Pow(shellDataIn.D, 2) / (data.EllipseH * 2) *
                                Math.Sqrt(1.0 - 4.0 * (Math.Pow(shellDataIn.D, 2) - 4 *
                                Math.Pow(data.EllipseH, 2)) * Math.Pow(dataIn.ellx, 2) /
                                    Math.Pow(shellDataIn.D, 4));
                        }
                        break;
                    }
                case ShellType.Spherical:
                case ShellType.Torospherical:
                    {
                        //data.Dp = 2 * shellDataIn.R;
                        break;
                    }
                default:
                    {
                        throw new CalculateException("Ошибка вида укрепляемого элемента.");
                    }
            }

            if (shellDataIn.ShellType == ShellType.Elliptical && shellDataIn.IsPressureIn)
            {
                data.sp = shellDataIn.p * data.Dp / (4.0 * shellDataIn.fi * data.SigmaAllowShell - shellDataIn.p);
            }
            else
            {
                data.sp = ((ShellCalculatedData)dataIn.ShellCalculatedData).s_p;
            }

            if (!dataIn.IsOval)
            {
                data.s1p = shellDataIn.p * (dataIn.d + 2 * dataIn.cs) / (2.0 * dataIn.fi * data.SigmaAllow1 - shellDataIn.p);
            }
            else
            {
                data.s1p = shellDataIn.p * (dataIn.d1 + 2 * dataIn.cs) / (2.0 * dataIn.fi * data.SigmaAllow1 - shellDataIn.p);
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

            data.L0 = Math.Sqrt(data.Dp * (shellDataIn.s - data.c));

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

            data.l2p2 = Math.Sqrt(data.Dp * (dataIn.s2 + shellDataIn.s - data.c));
            data.l2p = Math.Min(dataIn.l2, data.l2p2);

            switch (dataIn.NozzleKind)
            {
                case NozzleKind.ImpassWithoutRing:
                case NozzleKind.PassWithoutRing:
                case NozzleKind.ImpassWithRing:
                case NozzleKind.PassWithRing:
                case NozzleKind.WithRingAndInPart:
                case NozzleKind.WithFlanging:
                    dataIn.s0 = shellDataIn.s;
                    //dataIn.steel4 = dataIn.steel1;
                    break;
            }


            data.psi1 = Math.Min(1, data.SigmaAllow1 / data.SigmaAllowShell);
            data.psi2 = Math.Min(1, data.SigmaAllow2 / data.SigmaAllowShell);
            data.psi3 = Math.Min(1, data.SigmaAllow3 / data.SigmaAllowShell);

            data.psi4 = dataIn.NozzleKind switch
            {
                NozzleKind.WithTorusshapedInsert or NozzleKind.WithWealdedRing =>
                Math.Min(1, data.SigmaAllow4 / data.SigmaAllowShell),
                _ => 1,
            };

            data.d0p = 0.4 * Math.Sqrt(data.Dp * (shellDataIn.s - data.c));

            data.b = Math.Sqrt(data.Dp * (shellDataIn.s - data.c)) + Math.Sqrt(data.Dp * (shellDataIn.s - data.c));

            switch (shellDataIn.ShellType)
            {
                case ShellType.Cylindrical:
                    {
                        data.dmax = shellDataIn.D;
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
                        data.dmax = 0.6 * shellDataIn.D;
                        break;
                    }
            }

            switch (shellDataIn.ShellType)
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

            if (shellDataIn.IsPressureIn)
            {
                data.spn = data.sp;
            }
            else
            {

                //data.B1n = Math.Min(1, 9.45 * (shellDataIn.D / ddata.out.l) * Math.Sqrt(shellDataIn.D / (100 * (shellDataIn.s - data.c))));
                //data.pen = 2.08 * 0.00001 * shellDataIn.E / (dataIn.ny * data.B1n) * (shellDataIn.D / ddata.out.l) * Math.Pow(100 * (shellDataIn.s - data.c) / shellDataIn.D, 2.5);
                data.pen = data.p_deShell;
                data.ppn = shellDataIn.p / Math.Sqrt(1.0 - Math.Pow(shellDataIn.p / data.pen, 2));
                data.spn = data.ppn * data.Dp / (2.0 * data.K1 * data.SigmaAllowShell - data.ppn);
            }


            data.d01 = 2 * ((shellDataIn.s - data.c) / data.spn - 0.8) * Math.Sqrt(data.Dp * (shellDataIn.s - data.c));
            data.d02 = data.dmax + 2 * dataIn.cs;
            data.d0 = Math.Min(data.d01, data.d02);

            if (data.dp > data.d0)
            {
                data.ConditionStrengthening1 = data.l1p * (dataIn.s1 - data.s1p - dataIn.cs) * data.psi1 + data.l2p * dataIn.s2 * data.psi2 + data.l3p * (dataIn.s3 - dataIn.cs - dataIn.cs1) * data.psi3 + data.lp * (shellDataIn.s - data.sp - data.c) * data.psi4;
                data.ConditionStrengthening2 = 0.5 * (data.dp - data.d0p) * data.sp;
                if (data.ConditionStrengthening1 < data.ConditionStrengthening2)
                {
                    data.ErrorList.Add("Условие укрепления одиночного отверстия не выполняется.");
                }
            }


            data.V1 = (dataIn.s0 - data.c) / (shellDataIn.s - data.c);
            data.V2 = (data.psi4 + (data.l1p * (dataIn.s1 - dataIn.cs) * data.psi1 + data.l2p * dataIn.s2 * data.psi2 + data.l3p * (dataIn.s3 - dataIn.cs - dataIn.cs1) * data.psi3) / data.lp * (shellDataIn.s - data.c)) /
                  (1.0 + 0.5 * (data.dp - data.d0p) / data.lp + data.K1 * (dataIn.d + 2 * dataIn.cs) / data.Dp * (dataIn.fi / dataIn.fi1) * (data.l1p / data.lp));
            data.V = Math.Min(data.V1, data.V2);

            if (shellDataIn.IsPressureIn)
            {
                data.p_d = 2 * data.K1 * dataIn.fi * data.SigmaAllowShell * (shellDataIn.s - data.c) * data.V /
                       (data.Dp + (shellDataIn.s - data.c) * data.V);
            }
            else
            {
                data.p_dp = 2 * data.K1 * dataIn.fi * data.SigmaAllowShell * (shellDataIn.s - data.c) * data.V /
                        (data.Dp + (shellDataIn.s - data.c) * data.V);
                data.p_de = data.p_deShell;
                data.p_d = data.p_dp / Math.Sqrt(1 + Math.Pow(data.p_dp / data.p_de, 2));
            }
            if (data.p_d < shellDataIn.p)
            {
                data.ErrorList.Add("Допускаемое давление меньше расчетного.");
            }

            switch (shellDataIn.ShellType)
            {
                case ShellType.Cylindrical:
                    {
                        data.ConditionUseFormulas1 = (data.dp - 2 * dataIn.cs) / shellDataIn.D;
                        data.ConditionUseFormulas2 = (shellDataIn.s - data.c) / shellDataIn.D;
                        data.IsConditionUseFormulas = data.ConditionUseFormulas1 <= 1 & data.ConditionUseFormulas2 <= 0.1;
                        break;
                    }
                case ShellType.Conical:
                    {
                        data.ConditionUseFormulas1 = (data.dp - 2 * dataIn.cs) / data.Dk;
                        data.ConditionUseFormulas2 = (shellDataIn.s - data.c) / data.Dk;
                        data.ConditionUseFormulas2_2 = 0.1 / Math.Cos(Math.PI * 180 * data.alfa1);
                        data.IsConditionUseFormulas = data.ConditionUseFormulas1 <= 1 &
                                                 data.ConditionUseFormulas2 <= data.ConditionUseFormulas2_2;
                        break;
                    }
                case ShellType.Elliptical:
                case ShellType.Spherical:
                case ShellType.Torospherical:
                    {
                        data.ConditionUseFormulas1 = (data.dp - 2 * dataIn.cs) / shellDataIn.D;
                        data.ConditionUseFormulas2 = (shellDataIn.s - data.c) / shellDataIn.D;
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
