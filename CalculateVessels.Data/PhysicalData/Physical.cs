﻿using CalculateVessels.Data.PhysicalData.Enums;
using CalculateVessels.Data.PhysicalData.Gost6533;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;


namespace CalculateVessels.Data.PhysicalData
{
    public static class Physical
    {
        public static class Gost34233_1
        {
            public static string[] GetSteelsList()
            {
                using StreamReader file = new("PhysicalData/Gost34233_1/Steels.json");
                try
                {
                    var json = file.ReadToEnd();
                    var steels = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_1.SteelForSigma>>(json);
                    return steels.Select(l => l.Name).ToArray();
                }
                catch
                {
                    return null;
                }
            }

            public static SteelType GetSteelType (string steelName, ref List<string> errorList)
            {
                var steels = new List<PhysicalData.Gost34233_1.SteelForSteelType>();

                try
                {
                    using StreamReader file = new("PhysicalData/Gost34233_1/Steels.json");
                    var json = file.ReadToEnd();
                    file.Close();
                    steels = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_1.SteelForSteelType>>(json);
                }
                catch
                {
                    errorList.Add($"Error open file for SteelType of {steelName}");
                    return default;
                }

                var steel = steels.FirstOrDefault(s => s.Name.Equals(steelName));

                if (steel == null)
                {
                    errorList.Add($"Error find stell {steelName}");
                    return default;
                }

                return (SteelType)steel.SteelType;
            }

            public static double GetSigma(string steelName, double temperature,
                ref List<string> errorList,
                bool isBigThickness = false, bool isBigResource = false)
            {
                var steels = new List<PhysicalData.Gost34233_1.SteelForSigma>();

                try
                {
                    using StreamReader file = new("PhysicalData/Gost34233_1/Steels.json");
                    var json = file.ReadToEnd();
                    file.Close();
                    steels = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_1.SteelForSigma>>(json);
                }
                catch
                {
                    errorList.Add($"Error open file for sigma {steelName}");
                    return default;
                }

                var accessI = 0;

                if (isBigResource & !isBigThickness)
                {
                    accessI = 1;
                }
                else if (!isBigResource & isBigThickness)
                {
                    accessI = 2;
                }
                else if (isBigResource & isBigThickness)
                {
                    accessI = 3;
                }

                var steel = steels.FirstOrDefault(s => s.Name.Equals(steelName));

                double sigma_d, sigmaLittle = 0, sigmaBig = 0;
                double tempLittle = 0, tempBig = 0;

                for (var i = 0; i < steel.Values.Count; i++)
                {
                    if ((i == 0 && steel.Values[i].Temperature > temperature) ||
                        steel.Values[i].Temperature == temperature)
                    {
                        sigma_d = steel.Values[i].SigmaValue[accessI];

                        return sigma_d;
                    }
                    else if (steel.Values[i].Temperature > temperature)
                    {
                        tempLittle = steel.Values[i].Temperature;
                        sigmaBig = steel.Values[i].SigmaValue[accessI];
                        break;
                    }
                    else if (i == steel.Values.Count - 1)
                    {
                        errorList.Add($"Температура {temperature} °С, больше чем максимальная температура {tempBig} °С " +
                                      $"для стали {steelName} при которой определяется допускаемое напряжение по ГОСТ 34233.1-2017");
                        return default;
                    }
                    else
                    {
                        tempBig = steel.Values[i].Temperature;
                        sigmaLittle = steel.Values[i].SigmaValue[accessI];
                    }
                }

                sigma_d = sigmaBig - ((sigmaBig - sigmaLittle) * (temperature - tempLittle) / (tempBig - tempLittle));
                sigma_d *= 10;
                sigma_d = Math.Truncate(sigma_d / 5);
                sigma_d *= 0.5;

                return sigma_d;
            }


            public static double GetE(string steelName, double temperature, ref List<string> dataInErr)
            {
                var steels = new List<PhysicalData.Gost34233_1.SteelForE>();

                try
                {
                    using StreamReader file = new("PhysicalData/Gost34233_1/Steels.json");
                    var json = file.ReadToEnd();
                    file.Close();
                    steels = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_1.SteelForE>>(json);
                }
                catch
                {
                    return 0;
                }

                var steel = steels.FirstOrDefault(s => s.Name.Equals(steelName));

                steel.Values = steel.Values.Where(v => v.EValue != 0).ToList();

                double E, ELittle = 0, EBig = 0;
                double tempLittle = 0, tempBig = 0;

                for (var i = 0; i < steel.Values.Count; i++)
                {
                    if ((i == 0 && steel.Values[i].Temperature > temperature) ||
                        steel.Values[i].Temperature == temperature)
                    {
                        return steel.Values[i].EValue;
                    }
                    else if (steel.Values[i].Temperature > temperature)
                    {
                        tempLittle = steel.Values[i].Temperature;
                        EBig = steel.Values[i].EValue;
                        break;
                    }
                    else if (i == steel.Values.Count - 1)
                    {
                        dataInErr.Add($"Температура {temperature} °С, больше чем максимальная температура {tempBig} °С " +
                                      $"для стали {steelName} при которой определяется модуль  продольной упругости по ГОСТ 34233.1-2017");
                        return 0;
                    }
                    else
                    {
                        tempBig = steel.Values[i].Temperature;
                        ELittle = steel.Values[i].EValue;
                    }
                }

                E = EBig - ((EBig - ELittle) * (temperature - tempLittle) / (tempBig - tempLittle));
                E = Math.Truncate(E);

                return E;
            }
        }


        public static class Gost6533
        {
            
            public static List<string> GetEllipseDiameters(EllipticalBottomGostType type)
            {
                EllipsesList ellipsesList;

                try
                {
                    using StreamReader file = new("PhysicalData/Gost6533/EllipticalBottom.json");
                    var json = file.ReadToEnd();
                    file.Close();
                    ellipsesList = JsonSerializer.Deserialize<EllipsesList>(json);
                }
                catch
                {
                    return null;
                }

                var ellipses = type switch
                {
                    EllipticalBottomGostType.Ell025In => ellipsesList?.Ell025In,
                    EllipticalBottomGostType.Ell025Out => ellipsesList?.Ell025Out,
                    _ => null
                };

                return ellipses?.Select(e => e.Diameter.ToString(CultureInfo.CurrentCulture)).ToList();
            }

            public static EllipsesList GetEllipsesList()
            {
                try
                {
                    using StreamReader file = new("PhysicalData/Gost6533/EllipticalBottom.json");
                    var json = file.ReadToEnd();
                    file.Close();
                    return JsonSerializer.Deserialize<EllipsesList>(json);
                }
                catch
                {
                    return null;
                }
            }
        }


        public static class Gost34233_4
        {
            public static double Getfb(int M, bool isGroove = false)
            {
                var fbs = new List<PhysicalData.Gost34233_4.Fb>();

                try
                {
                    using StreamReader file = new("PhysicalData/Gost34233_4/ScrewSquare.json");
                    var json = file.ReadToEnd();
                    file.Close();
                    fbs = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_4.Fb>>(json);
                }
                catch
                {
                    return 0;
                }

                var fb = fbs.FirstOrDefault(f => f.M == M);

                if (fb == null)
                {
                    return 0;
                }
                else
                {
                    return isGroove ? fb.fb_groove : fb.fb;
                }
            }

            public static IEnumerable<string> GetScrewDs()
            {
                var fbs = new List<PhysicalData.Gost34233_4.Fb>();

                try
                {
                    using StreamReader file = new("PhysicalData/Gost34233_4/ScrewSquare.json");
                    var json = file.ReadToEnd();
                    file.Close();
                    fbs = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_4.Fb>>(json);
                }
                catch
                {
                    return null;
                }

                return fbs?.Select(f => f.M.ToString());

            }

            public static (double m, double qobj, double q_d, double Kobj, double Ep, bool isFlat, bool isMetall) GetGasketParameters(string name)
            {
                var gaskets = new List<PhysicalData.Gost34233_4.Gasket>();

                try
                {
                    using StreamReader file = new("PhysicalData/Gost34233_4/Gaskets.json");
                    var json = file.ReadToEnd();
                    file.Close();
                    gaskets = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_4.Gasket>>(json);
                }
                catch
                {
                    return default;
                }

                var gasket = gaskets.FirstOrDefault(g => g.Material == name);

                if (gasket == null)
                {
                    return default;
                }
                else
                {
                    return (gasket.m, gasket.qobj, gasket.q_d, gasket.Kobj, gasket.Ep, gasket.IsFlat, gasket.IsMetal);
                }
            }

            public static double GetE(string steelName, double temperature)
            {
                var steels = new List<PhysicalData.Gost34233_4.SteelForE>();

                try
                {
                    using StreamReader file = new("PhysicalData/Gost34233_4/Steels.json");
                    var json = file.ReadToEnd();
                    file.Close();
                    steels = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_4.SteelForE>>(json);
                }
                catch
                {
                    return 0;
                }

                var steel = steels.FirstOrDefault(s => s.Name.Equals(steelName));

                steel.Values = steel.Values.Where(v => v.EValue != 0).ToList();

                double E, ELittle = 0, EBig = 0;
                double tempLittle = 0, tempBig = 0;

                for (var i = 0; i < steel.Values.Count; i++)
                {
                    if ((i == 0 && steel.Values[i].Temperature > temperature) ||
                        steel.Values[i].Temperature == temperature)
                    {
                        return steel.Values[i].EValue;
                    }
                    else if (steel.Values[i].Temperature > temperature)
                    {
                        tempLittle = steel.Values[i].Temperature;
                        EBig = steel.Values[i].EValue;
                        break;
                    }
                    else if (i == steel.Values.Count - 1)
                    {
                        //dataInErr.Add($"Температура {temperature} °С, больше чем максимальная температура {tempBig} °С " +
                        //  $"для стали {steelName} при которой определяется модуль  продольной упругости по ГОСТ 34233.1-2017");
                        return 0;
                    }
                    else
                    {
                        tempBig = steel.Values[i].Temperature;
                        ELittle = steel.Values[i].EValue;
                    }
                }

                E = EBig - ((EBig - ELittle) * (temperature - tempLittle) / (tempBig - tempLittle));
                E = Math.Truncate(E);

                return E;
            }

            public static double GetAlfa(string steelName, double temperature)
            {
                var steels = new List<PhysicalData.Gost34233_4.SteelForAlfa>();

                try
                {
                    using StreamReader file = new("PhysicalData/Gost34233_4/Steels.json");
                    var json = file.ReadToEnd();
                    file.Close();
                    steels = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_4.SteelForAlfa>>(json);
                }
                catch
                {
                    return 0;
                }

                var steel = steels.FirstOrDefault(s => s.Name.Equals(steelName));

                steel.Values = steel.Values.Where(v => v.AlfaValue != 0).ToList();

                for (var i = 0; i < steel.Values.Count; i++)
                {
                    if (steel.Values[i].Temperature >= temperature)
                    {
                        return steel.Values[i].AlfaValue;
                    }
                }

                return 0;
            }

            public static double GetSigma(string steelName, double temperature)
            {
                var steels = new List<PhysicalData.Gost34233_4.SteelForSigma>();

                try
                {
                    using StreamReader file = new("PhysicalData/Gost34233_4/Steels.json");
                    var json = file.ReadToEnd();
                    file.Close();
                    steels = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_4.SteelForSigma>>(json);
                }
                catch
                {
                    return 0;
                }

                var steel = steels.FirstOrDefault(s => s.Name.Equals(steelName));

                steel.Values = steel.Values.Where(v => v.SigmaValue != 0).ToList();

                double sigma_d, sigmaLittle = 0, sigmaBig = 0;
                double tempLittle = 0, tempBig = 0;

                for (var i = 0; i < steel.Values.Count; i++)
                {
                    if ((i == 0 && steel.Values[i].Temperature > temperature) ||
                        steel.Values[i].Temperature == temperature)
                    {
                        return steel.Values[i].SigmaValue;
                    }
                    else if (steel.Values[i].Temperature > temperature)
                    {
                        tempLittle = steel.Values[i].Temperature;
                        sigmaBig = steel.Values[i].SigmaValue;
                        break;
                    }
                    else if (i == steel.Values.Count - 1)
                    {
                        //dataInErr.Add($"Температура {temperature} °С, больше чем максимальная температура {tempBig} °С " +
                        //  $"для стали {steelName} при которой определяется модуль  продольной упругости по ГОСТ 34233.1-2017");
                        return 0;
                    }
                    else
                    {
                        tempBig = steel.Values[i].Temperature;
                        sigmaLittle = steel.Values[i].SigmaValue;
                    }
                }

                sigma_d = sigmaBig - ((sigmaBig - sigmaLittle) * (temperature - tempLittle) / (tempBig - tempLittle));
                sigma_d *= 10;
                sigma_d = Math.Truncate(sigma_d / 5);
                sigma_d *= 0.5;

                return sigma_d;
            }

            public static IEnumerable<string> GetGasketsList()
            {
                List<PhysicalData.Gost34233_4.Gasket> gaskets;

                try
                {
                    using StreamReader file = new("PhysicalData/Gost34233_4/Gaskets.json");
                    var json = file.ReadToEnd();
                    file.Close();
                    gaskets = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_4.Gasket>>(json);
                }
                catch
                {
                    return null;
                }

                return gaskets?.Select(g => g.Material);
            }

            public static IEnumerable<string> GetSteelsList()
            {
                List<PhysicalData.Gost34233_4.Steel> steels;

                try
                {
                    using StreamReader file = new("PhysicalData/Gost34233_4/Steels.json");
                    var json = file.ReadToEnd();
                    file.Close();
                    steels = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_4.Steel>>(json);
                }
                catch
                {
                    return null;
                }

                return steels?.Select(s => s.Name);
            }
        }

        public static double GetAlfa(string steelName, double temperature, string gost = "Gost34233_1")
        {
            var steels = new List<PhysicalData.Gost34233_4.SteelForAlfa>();

            try
            {
                using StreamReader file = new($"PhysicalData/{gost}/Steels.json");
                var json = file.ReadToEnd();
                file.Close();
                steels = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_4.SteelForAlfa>>(json);
            }
            catch
            {
                return 0;
            }

            var steel = steels.FirstOrDefault(s => s.Name.Equals(steelName));

            steel.Values = steel.Values.Where(v => v.AlfaValue != 0).ToList();

            for (var i = 0; i < steel.Values.Count; i++)
            {
                if (steel.Values[i].Temperature >= temperature)
                {
                    return steel.Values[i].AlfaValue;
                }
            }

            return 0;
        }

        public static class Gost34233_7
        {
            private const string TABLE_2 = "PhysicalData/Gost34233_7/Table2.json";
            private const string TABLE_B1 = "PhysicalData/Gost34233_7/TableB1.json";
            private const string TABLE_G1 = "PhysicalData/Gost34233_7/TableG1.json";
            private const string TABLE_G23 = "PhysicalData/Gost34233_7/TableG23.json";

            public static double Getpsi0(double etaT, ref List<string> errorList)
            {
                var psiList = new List<PhysicalData.Gost34233_7.Psi>();

                try
                {
                    using StreamReader file = new(TABLE_B1);
                    var json = file.ReadToEnd();
                    file.Close();
                    psiList = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_7.Psi>>(json);
                }
                catch
                {
                    errorList.Add("Error open file for psi0");
                    return default;
                }

                var etaTRound = Math.Round(etaT * 20) / 20;

                var result = psiList.FirstOrDefault(p => Math.Abs(p.etaT - etaTRound) < 0.00001);

                if (result == null)
                {
                    errorList.Add($"Coudnt find value of psi0 for etaT={etaT}");
                    return default;
                }

                return result.psi0;
            }

            public static double GetpW_d(double D, ref List<string> errorList)
            {
                var Ws = new List<PhysicalData.Gost34233_7.W>();

                try
                {
                    using StreamReader file = new(TABLE_2);
                    var json = file.ReadToEnd();
                    file.Close();
                    Ws = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_7.W>>(json);
                }
                catch
                {
                    errorList.Add("Error open file for [W]");
                    return default;
                }

                var result = Ws.LastOrDefault(w => w.D <= D);

                if (result == null)
                {
                    errorList.Add($"Coudnt find value of [W] for D={D}");
                    return default;
                }

                return result.W_d;
            }

            public static (double T1, double T2, double T3) GetT1T2T3(double omega, double mn, ref List<string> errorList)
            {
                if (omega is < 0 or > 10)
                {
                    errorList.Add($"Error input value for omega {omega} value must be in range 0-10");
                    return default;
                }

                if (mn is < 1 or > 1.54)
                {
                    errorList.Add($"Error input value for mn {mn} value must be in range 1.0-1.5");
                    return default;
                }

                var omegaRound = omega <= 4
                        ? Math.Round(omega * 2.0) / 2.0
                        : Math.Round(omega);
                var mnRound = Math.Round(mn * 10.0) / 10.0;

                var omegaList = new List<PhysicalData.Gost34233_7.OmegaForT1T2T3>();

                try
                {
                    using StreamReader file = new(TABLE_G1);
                    var json = file.ReadToEnd();
                    file.Close();
                    omegaList = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_7.OmegaForT1T2T3>>(json);
                }
                catch
                {
                    errorList.Add("Error open file for T1, T2, T3");
                    return default;
                }


                var result = omegaList
                    .FirstOrDefault(o => Math.Abs(o.Omega - omegaRound) < 0.00001)
                    ?.MnList
                    .FirstOrDefault(m => Math.Abs(m.mn - mnRound) < 0.00001);

                if (result == null)
                {
                    errorList.Add($"Coudnt find value of T1, T2, T3 for omega={omega} and mn={mn}");
                    return default;
                }

                return (result.T1, result.T2, result.T3);
            }

            public static double GetA(double omega, double mA, ref List<string> errorList)
            {
                if (omega is < 0 or > 10)
                {
                    errorList.Add($"Error input value for omega {omega} value must be in range 0-10");
                    return default;
                }

                double omegaRound;
                if (omega <= 2)
                {
                    omegaRound = Math.Round(omega * 2.0) / 2.0;
                }
                else if (omega <= 5)
                {
                    omegaRound = Math.Round(omega);
                }
                else
                {
                    omegaRound = omega < 7.5 ? 5 : 10;
                }

                var mARound = Math.Round(mA * 10) / 10.0;

                var omegaList = new List<PhysicalData.Gost34233_7.OmegaForAB>();

                try
                {
                    using StreamReader file = new(TABLE_G23);
                    var json = file.ReadToEnd();
                    file.Close();
                    omegaList = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_7.OmegaForAB>>(json);
                }
                catch
                {
                    errorList.Add("Error open file for A");
                    return default;
                }


                var result = omegaList
                    .FirstOrDefault(o => Math.Abs(o.Omega - omegaRound) < 0.00001)
                    ?.mAnBList
                    .FirstOrDefault(m => Math.Abs(m.Value - mARound) < 0.00001);

                if (result == null)
                {
                    errorList.Add($"Coudnt find value of A for omega={omega} and mA={mA}");
                    return default;
                }

                return result.A;
            }

            public static double GetB(double omega, double nB, ref List<string> errorList)
            {
                if (omega is < 0 or > 10)
                {
                    errorList.Add($"Error input value for omega {omega} value must be in range 0-10");
                    return default;
                }

                double omegaRound;
                if (omega <= 2)
                {
                    omegaRound = Math.Round(omega * 2.0) / 2.0;
                }
                else if (omega <= 5)
                {
                    omegaRound = Math.Round(omega);
                }
                else
                {
                    omegaRound = omega < 7.5 ? 5 : 10;
                }

                var nBRound = Math.Round(nB * 10) / 10.0;

                var omegaList = new List<PhysicalData.Gost34233_7.OmegaForAB>();

                try
                {
                    using StreamReader file = new(TABLE_G23);
                    var json = file.ReadToEnd();
                    file.Close();
                    omegaList = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_7.OmegaForAB>>(json);
                }
                catch
                {
                    errorList.Add("Error open file for B");
                    return default;
                }


                var result = omegaList
                    .FirstOrDefault(o => Math.Abs(o.Omega - omegaRound) < 0.00001)
                    ?.mAnBList
                    .FirstOrDefault(n => Math.Abs(n.Value - nBRound) < 0.00001);

                if (result == null)
                {
                    errorList.Add($"Coudnt find value of B for omega={omega} and nB={nB}");
                    return default;
                }

                return result.B;
            }

        }
    }
}
