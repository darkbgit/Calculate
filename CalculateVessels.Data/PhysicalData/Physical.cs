﻿using CalculateVessels.Data.PhysicalData.Common;
using CalculateVessels.Data.PhysicalData.Enums;
using CalculateVessels.Data.PhysicalData.Gost6533;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using CalculateVessels.Data.PhysicalData.Gost34233_1;
using CalculateVessels.Data.PhysicalData.Gost34233_7;
using SteelForE = CalculateVessels.Data.PhysicalData.Common.SteelForE;


namespace CalculateVessels.Data.PhysicalData
{
    public static class Physical
    {
        public static class Gost34233_1
        {
            private const string TABLE_STEELS = "PhysicalData/Gost34233_1/Steels.json";
            private const string TABLE_SIGMA = "PhysicalData/Gost34233_1/SteelsSigma.json";
            private const string TABLE_RM = "PhysicalData/Gost34233_1/SteelsRm.json";
            //private const string TABLE_TYPE = "PhysicalData/Gost34233_1/SteelsType.json";

            public static IEnumerable<string> GetSteelsList()
            {
                List<PhysicalData.Gost34233_1.Steel> steels = new();

                using StreamReader file = new(TABLE_SIGMA);
                try
                {
                    var json = file.ReadToEnd();
                    steels = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_1.Steel>>(json);
                }
                catch
                {
                    return null;
                }

                IEnumerable<string> result = Enumerable.Empty<string>();
                steels?.ForEach(s => result = result.Union(s.Name));
                result = result.OrderByDescending(s => s);

                return result;
            }

            public static SteelType GetSteelType (string steelName, ref List<string> errorList)
            {
                List<PhysicalData.Gost34233_1.SteelForSteelType> steels;

                try
                {
                    using StreamReader file = new(TABLE_STEELS);
                    var json = file.ReadToEnd();
                    file.Close();
                    steels = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_1.SteelForSteelType>>(json);
                }
                catch
                {
                    errorList.Add($"Error open file for SteelType of {steelName}");
                    return SteelType.Undefined;
                }

                var steel = steels?.FirstOrDefault(s => s.Name.Equals(steelName));

                if (steel == null)
                {
                    errorList.Add($"Error find steel {steelName}");
                    return SteelType.Undefined;
                }

                return (SteelType)steel.SteelType;
            }

            public static bool TryGetSigma(string steelName, double temperature,
                ref double sigma_d, ref List<string> errorList,
                double s = 0, int N = 1000)
            {
                bool isBigThickness = false;


                var isBigResource = N switch
                {
                    1000 => false,
                    2000 => true,
                    _ => true
                };


                List<PhysicalData.Gost34233_1.SteelForSigma> steels;

                try
                {
                    using StreamReader file = new(TABLE_SIGMA);
                    var json = file.ReadToEnd();
                    file.Close();
                    steels = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_1.SteelForSigma>>(json);
                }
                catch
                {
                    errorList.Add($"Error open file for sigma {steelName}");
                    return false;
                }

               

                var steel = steels?.FirstOrDefault(st => st.Name.Contains(steelName));

                if (steel == null)
                {
                    errorList.Add($"Error find steel {steelName}");
                    return false;
                }


                if (steel.IsCouldBigThickness)
                {
                    isBigThickness = steel.BigThickness < s;
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

                double sigmaLittle = 0, sigmaBig = 0;
                double tempLittle = 0, tempBig = 0;

                for (var i = 0; i < steel.Values.Count; i++)
                {
                    if ((i == 0 && steel.Values[i].Temperature > temperature) ||
                        steel.Values[i].Temperature == temperature)
                    {
                        sigma_d = steel.Values[i].SigmaValue[accessI];
                        return true;
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
                        return false;
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

                return true;
            }

            public static bool TryGetRm(string steelName, double temperature,
                ref double Rm, ref List<string> errorList,
                double s = 0)
            {
                List<SteelForRm> steels;

                try
                {
                    using StreamReader file = new(TABLE_RM);
                    var json = file.ReadToEnd();
                    file.Close();
                    steels = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_1.SteelForRm>>(json);
                }
                catch
                {
                    errorList.Add($"Error open file for Rm {steelName}");
                    return false;
                }

                

                var steel = steels?.FirstOrDefault(st => st.Name.Contains(steelName));

                if (steel == null)
                {
                    errorList.Add($"Error find steel {steelName}");
                    return false;
                }

                var accessI = 0;

                if (steel.IsCouldBigThickness && steel.BigThickness < s)
                {
                    accessI = 1;
                }
                

                double RmLittle = 0, RmBig = 0;
                double tempLittle = 0, tempBig = 0;

                for (var i = 0; i < steel.Values.Count; i++)
                {
                    if ((i == 0 && steel.Values[i].Temperature > temperature) ||
                        steel.Values[i].Temperature == temperature)
                    {
                        Rm = steel.Values[i].RmValue[accessI];
                        return true;
                    }
                    else if (steel.Values[i].Temperature > temperature)
                    {
                        tempLittle = steel.Values[i].Temperature;
                        RmBig = steel.Values[i].RmValue[accessI];
                        break;
                    }
                    else if (i == steel.Values.Count - 1)
                    {
                        errorList.Add($"Температура {temperature} °С, больше чем максимальная температура {tempBig} °С " +
                                      $"для стали {steelName} при которой определяется значение временного сопротивления по ГОСТ 34233.1-2017");
                        return false;
                    }
                    else
                    {
                        tempBig = steel.Values[i].Temperature;
                        RmLittle = steel.Values[i].RmValue[accessI];
                    }
                }

                Rm = RmBig - ((RmBig - RmLittle) * (temperature - tempLittle) / (tempBig - tempLittle));
                Rm = Math.Round(Rm);
                return true;
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
            private const string TABLE_D1_SCREW_M = "PhysicalData/Gost34233_4/TableD1.json";
            private const string TABLE_E = "PhysicalData/Gost34233_4/SteelsE.json";
            private const string TABLE_SIGMA = "PhysicalData/Gost34233_4/SteelsSigma.json";
            private const string TABLE_GASKET = "PhysicalData/Gost34233_4/Gaskets.json";
            private const string TABLE_ALFA = "PhysicalData/Gost34233_4/SteelsAlfa.json";


            public static bool TryGetfb(int M, bool isGroove, ref double fb, ref List<string> errorList)
            {
                var fbs = new List<PhysicalData.Gost34233_4.Fb>();

                try
                {
                    using StreamReader file = new(TABLE_D1_SCREW_M);
                    var json = file.ReadToEnd();
                    file.Close();
                    fbs = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_4.Fb>>(json);
                }
                catch
                {
                    errorList.Add($"Error open file {TABLE_D1_SCREW_M} for fb");
                    return false;
                }

                var fbValue = fbs.FirstOrDefault(f => f.M == M);

                if (fbValue == null)
                {
                    errorList.Add($"Error find value for fb for M {M} in file {TABLE_D1_SCREW_M}");
                    return false;
                }

                fb = isGroove ? fbValue.fb_groove : fbValue.fb;
                return true;
            }

            public static IEnumerable<string> GetScrewDs()
            {
                var fbs = new List<PhysicalData.Gost34233_4.Fb>();

                try
                {
                    using StreamReader file = new(TABLE_D1_SCREW_M);
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
                    using StreamReader file = new(TABLE_GASKET);
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

            public static bool TryGetSigma(string steelName, double temperature, ref double sigma_d, ref List<string> errorList)
            {
                var steels = new List<PhysicalData.Gost34233_4.SteelForSigma>();

                try
                {
                    using StreamReader file = new(TABLE_SIGMA);
                    var json = file.ReadToEnd();
                    file.Close();
                    steels = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_4.SteelForSigma>>(json);
                }
                catch
                {
                    errorList.Add($"Error open file {TABLE_SIGMA} for sigma of stell {steelName}");
                    return false;
                }

                var steel = steels.FirstOrDefault(s => s.Name.Contains(steelName));

                if (steel == null)
                {
                    errorList.Add($"Error find steel {steelName} in file {TABLE_SIGMA}");
                    return false;
                }

                steel.Values = steel.Values.Where(v => v.SigmaValue != 0).ToList();

                double sigmaLittle = 0, sigmaBig = 0;
                double tempLittle = 0, tempBig = 0;

                for (var i = 0; i < steel.Values.Count; i++)
                {
                    if ((i == 0 && steel.Values[i].Temperature > temperature) ||
                        steel.Values[i].Temperature == temperature)
                    {
                        sigma_d = steel.Values[i].SigmaValue;
                        return true;
                    }
                    else if (steel.Values[i].Temperature > temperature)
                    {
                        tempLittle = steel.Values[i].Temperature;
                        sigmaBig = steel.Values[i].SigmaValue;
                        break;
                    }
                    else if (i == steel.Values.Count - 1)
                    {
                        errorList.Add($"Температура {temperature} °С, больше чем максимальная температура {tempBig} °С " +
                            $"для стали {steelName} при которой определяется допускаемое напряжениу по ГОСТ 34233.4-2017");
                        return false;
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

                return true;
            }

            public static IEnumerable<string> GetGasketsList()
            {
                List<PhysicalData.Gost34233_4.Gasket> gaskets;

                try
                {
                    using StreamReader file = new(TABLE_GASKET);
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

            public static IEnumerable<string> GetSteelsList(string type)
            {
                string table;

                switch (type)
                {
                    case "screw":
                        table = TABLE_SIGMA;
                        break;
                    case "washer":
                        table = TABLE_E;
                        break;
                    default:
                        return null;
                }

                List<PhysicalData.Gost34233_4.Steel> steels;

                try
                {
                    using StreamReader file = new(table);
                    var json = file.ReadToEnd();
                    file.Close();
                    steels = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_4.Steel>>(json);
                }
                catch
                {
                    return null;
                }

                List<string> result = new();
                steels?.ForEach(s => result = result.Union(s.Name).ToList());

                return result;
            }
        }


        public static class Gost34233_7
        {
            private const string TABLE_1 = "PhysicalData/Gost34233_7/Table1.json";
            private const string TABLE_2 = "PhysicalData/Gost34233_7/Table2.json";
            private const string TABLE_B1 = "PhysicalData/Gost34233_7/TableB1.json";
            private const string TABLE_G1 = "PhysicalData/Gost34233_7/TableG1.json";
            private const string TABLE_G23 = "PhysicalData/Gost34233_7/TableG23.json";

            public static bool TryGetpsi0(double etaT,ref double psi0, ref List<string> errorList)
            {
                List<PhysicalData.Gost34233_7.Psi> psiList;

                try
                {
                    using StreamReader file = new(TABLE_B1);
                    var json = file.ReadToEnd();
                    file.Close();
                    psiList = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_7.Psi>>(json);
                }
                catch
                {
                    errorList.Add($"Error open file {TABLE_B1} for psi0");
                    return false;
                }

                var etaTRound = Math.Round(etaT * 20) / 20;

                var result = psiList?.FirstOrDefault(p => Math.Abs(p.etaT - etaTRound) < 0.00001);

                if (result == null)
                {
                    errorList.Add($"Coudnt find value of psi0 for etaT={etaT}");
                    return false;
                }

                psi0 = result.psi0;
                return true;
            }

            public static bool TryGetpW_d(double D, ref double W_d, ref List<string> errorList)
            {
                List<PhysicalData.Gost34233_7.W> Ws;

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
                    return false;
                }

                var result = Ws?.LastOrDefault(w => w.D <= D);

                if (result == null)
                {
                    errorList.Add($"Coudnt find value of [W] for D={D}");
                    return false;
                }

                W_d = result.W_d;
                return true;
            }

            public static bool TryGetT1T2T3(double omega, double mn,
                ref double T1, ref double T2, ref double T3,
                ref List<string> errorList)
            {
                if (omega is < 0 or > 10)
                {
                    errorList.Add($"Error input value for omega {omega} value must be in range 0-10");
                    return false;
                }

                if (mn is < 1 or > 1.54)
                {
                    errorList.Add($"Error input value for mn {mn} value must be in range 1.0-1.5");
                    return false;
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
                    return false;
                }


                var result = omegaList
                    .FirstOrDefault(o => Math.Abs(o.Omega - omegaRound) < 0.00001)
                    ?.MnList
                    .FirstOrDefault(m => Math.Abs(m.mn - mnRound) < 0.00001);

                if (result == null)
                {
                    errorList.Add($"Coudnt find value of T1, T2, T3 for omega={omega} and mn={mn}");
                    return false;
                }

                T1 = result.T1;
                T2 = result.T2;
                T3 = result.T3;
                return true;
            }

            public static bool TryGetPhi1Phi2Phi3(double omega, 
                ref double Phi1, ref double Phi2, ref double Phi3,
                ref List<string> errorList)
            {
                if (omega is < 0 or > 11.5)
                {
                    errorList.Add($"Error input value for omega {omega} value must be in range 0-11");
                    return false;
                }

                var omegaRound = omega <= 4
                    ? Math.Round(omega * 2.0) / 2.0
                    : Math.Round(omega);

                if (omegaRound == 11)
                {
                    Phi1 = Math.Sqrt(2) * omega;
                    Phi2 = omega;
                    Phi3 = Math.Sqrt(2) * omega;
                    return true;
                }
 
                List<PhysicalData.Gost34233_7.OmegaForPhi1Phi2Phi3> omegaList;

                try
                {
                    using StreamReader file = new(TABLE_1);
                    var json = file.ReadToEnd();
                    file.Close();
                    omegaList = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_7.OmegaForPhi1Phi2Phi3>>(json);
                }
                catch
                {
                    errorList.Add("Error open file for Phi1, Phi2, Phi3");
                    return false;
                }


                var result = omegaList
                    ?.FirstOrDefault(o => Math.Abs(o.Omega - omegaRound) < 0.00001);

                if (result == null)
                {
                    errorList.Add($"Coudnt find value of Phi1, Phi2, Phi3 for omega={omega}");
                    return false;
                }

                Phi1 = result.Phi1;
                Phi2 = result.Phi2;
                Phi3 = result.Phi3;
                return true;
            }

            public static bool TryGetA(double omega, double mA, ref double A, ref List<string> errorList)
            {
                if (omega is < 0 or > 10)
                {
                    errorList.Add($"Error input value for omega {omega} value must be in range 0-10");
                    return false;
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
                    return false;
                }


                var result = omegaList
                    .FirstOrDefault(o => Math.Abs(o.Omega - omegaRound) < 0.00001)
                    ?.mAnBList
                    .FirstOrDefault(m => Math.Abs(m.Value - mARound) < 0.00001);

                if (result == null)
                {
                    errorList.Add($"Coudnt find value of A for omega={omega} and mA={mA}");
                    return false;
                }

                A = result.A;
                return true;
            }

            public static bool TryGetB(double omega, double nB, ref double B, ref List<string> errorList)
            {
                if (omega is < 0 or > 10)
                {
                    errorList.Add($"Error input value for omega {omega} value must be in range 0-10");
                    return false;
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
                    return false;
                }


                var result = omegaList
                    .FirstOrDefault(o => Math.Abs(o.Omega - omegaRound) < 0.00001)
                    ?.mAnBList
                    .FirstOrDefault(n => Math.Abs(n.Value - nBRound) < 0.00001);

                if (result == null)
                {
                    errorList.Add($"Coudnt find value of B for omega={omega} and nB={nB}");
                    return false;
                }

                B = result.B;
                return true;
            }

        }


        public static bool TryGetAlfa(string steelName, double temperature,
            ref double alfa, ref List<string> errorList, string gost = "Gost34233_1")
        {
            List<SteelForAlfa> steels = new();

            try
            {
                using StreamReader file = new($"PhysicalData/{gost}/SteelsAlfa.json");
                var json = file.ReadToEnd();
                file.Close();
                steels = JsonSerializer.Deserialize<List<SteelForAlfa>>(json);
            }
            catch
            {
                errorList.Add($"Can't open file for alfa for steel {steelName} in GOST {gost}");
                return false;
            }

            var alfaList = steels
                .FirstOrDefault(s => s.Name.Contains(steelName))
                ?.Values
                .Where(v => v.AlfaValue != 0).ToList();

            if (alfaList == null)
            {
                errorList.Add($"Couldn't find alfa values for steel={steelName} in GOST {gost}");
                return false;
            }

            for (var i = 0; i < alfaList.Count; i++)
            {
                if (alfaList[i].Temperature >= temperature)
                {
                    alfa = alfaList[i].AlfaValue;
                    return true;
                }
            }

            errorList.Add($"Couldn't find alfa value for steel={steelName} on temperature {temperature} in GOST {gost}");
            return false;
        }

        public static bool TryGetE(string steelName, double temperature, ref double E, ref List<string> errorList, string gost = "Gost34233_1")
        {
            List<SteelForE> steels;

            try
            {
                using StreamReader file = new($"PhysicalData/{gost}/SteelsE.json");
                var json = file.ReadToEnd();
                file.Close();
                steels = JsonSerializer.Deserialize<List<SteelForE>>(json);
            }
            catch
            {
                errorList.Add($"Can't open file for E for steel {steelName} in GOST {gost}");
                return false;
            }

            var Elist = steels
                ?.FirstOrDefault(s => s.Name.Contains(steelName))
                ?.Values
                ?.Where(v => v.EValue != 0).ToList();

            if (Elist == null)
            {
                errorList.Add($"Error find steel {steelName}");
                return false;
            }

            double ELittle = 0, EBig = 0;
            double tempLittle = 0, tempBig = 0;

            for (var i = 0; i < Elist.Count; i++)
            {
                if ((i == 0 && Elist[i].Temperature > temperature) ||
                    Elist[i].Temperature == temperature)
                {
                    E = Elist[i].EValue;
                    return true;
                }
                else if (Elist[i].Temperature > temperature)
                {
                    tempLittle = Elist[i].Temperature;
                    EBig = Elist[i].EValue;
                    break;
                }
                else if (i == Elist.Count - 1)
                {
                    errorList.Add($"Температура {temperature} °С, больше чем максимальная температура {tempBig} °С " +
                      $"для стали {steelName} при которой определяется модуль продольной упругости по {gost}-2017");
                    return false;
                }
                else
                {
                    tempBig = Elist[i].Temperature;
                    ELittle = Elist[i].EValue;
                }
            }

            E = EBig - ((EBig - ELittle) * (temperature - tempLittle) / (tempBig - tempLittle));
            E = Math.Truncate(E);

            return true;
        }


    }
}
