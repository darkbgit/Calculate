using CalculateVessels.Data.PhysicalData.Common;
using CalculateVessels.Data.PhysicalData.Enums;
using CalculateVessels.Data.PhysicalData.Gost6533;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using CalculateVessels.Data.PhysicalData.Gost34233_1;
using CalculateVessels.Data.PhysicalData.Gost34233_4;
using CalculateVessels.Data.PhysicalData.Gost34233_7;
using SteelForE = CalculateVessels.Data.PhysicalData.Common.SteelForE;
using SteelForSigma = CalculateVessels.Data.PhysicalData.Gost34233_4.SteelForSigma;


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
                List<PhysicalData.Gost34233_1.Steel> steels;

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

                var result = Enumerable.Empty<string>();
                steels?.ForEach(s => result = result.Union(s.Name));
                result = result.OrderByDescending(s => s);

                return result;
            }

            public static SteelType GetSteelType (string steelName)
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
                    throw new PhysicalDataException($"Error open file for SteelType of {steelName}");
                }

                var steel = steels?.FirstOrDefault(s => s.Name.Equals(steelName)) ??
                            throw new PhysicalDataException($"Error find steel {steelName}");

                return (SteelType)steel.SteelType;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="steelName"></param>
            /// <param name="temperature"></param>
            /// <param name="s"></param>
            /// <param name="N"></param>
            /// <returns></returns>
            /// <exception cref="PhysicalDataException"></exception>
            public static double GetSigma(string steelName, double temperature, double s = 0, int N = 1000)
            {
                bool isBigThickness = false;

                double sigmaAlloy;

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
                    throw new PhysicalDataException($"Error open file for sigma {steelName}");
                }

                var steel = steels?.FirstOrDefault(st => st.Name.Contains(steelName)) ??
                            throw new PhysicalDataException($"Error find steel {steelName}");

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
                        sigmaAlloy = steel.Values[i].SigmaValue[accessI];
                        return sigmaAlloy;
                    }
                    else if (steel.Values[i].Temperature > temperature)
                    {
                        tempLittle = steel.Values[i].Temperature;
                        sigmaBig = steel.Values[i].SigmaValue[accessI];
                        break;
                    }
                    else if (i == steel.Values.Count - 1)
                    {
                        throw new  PhysicalDataException($"Температура {temperature} °С, больше чем максимальная температура {tempBig} °С " +
                                      $"для стали {steelName} при которой определяется допускаемое напряжение по ГОСТ 34233.1-2017");
                    }
                    else
                    {
                        tempBig = steel.Values[i].Temperature;
                        sigmaLittle = steel.Values[i].SigmaValue[accessI];
                    }
                }

                sigmaAlloy = sigmaBig - ((sigmaBig - sigmaLittle) * (temperature - tempLittle) / (tempBig - tempLittle));
                sigmaAlloy *= 10;
                sigmaAlloy = Math.Truncate(sigmaAlloy / 5);
                sigmaAlloy *= 0.5;

                return sigmaAlloy;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="steelName"></param>
            /// <param name="temperature"></param>
            /// <param name="s"></param>
            /// <returns></returns>
            /// <exception cref="PhysicalDataException"></exception>
            public static double GetRm(string steelName, double temperature, double s = 0)
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
                    throw new PhysicalDataException($"Error open file for Rm {steelName}");
                }

                var steel = steels?.FirstOrDefault(st => st.Name.Contains(steelName)) ??
                            throw new PhysicalDataException($"Error find steel {steelName}");

                var accessI = 0;

                if (steel.IsCouldBigThickness && steel.BigThickness < s)
                {
                    accessI = 1;
                }

                double Rm;

                double RmLittle = 0, RmBig = 0;
                double tempLittle = 0, tempBig = 0;

                for (var i = 0; i < steel.Values.Count; i++)
                {
                    if ((i == 0 && steel.Values[i].Temperature > temperature) ||
                        steel.Values[i].Temperature == temperature)
                    {
                        Rm = steel.Values[i].RmValue[accessI];
                        return Rm;
                    }
                    else if (steel.Values[i].Temperature > temperature)
                    {
                        tempLittle = steel.Values[i].Temperature;
                        RmBig = steel.Values[i].RmValue[accessI];
                        break;
                    }
                    else if (i == steel.Values.Count - 1)
                    {
                        throw new PhysicalDataException($"Температура {temperature} °С, больше чем максимальная температура {tempBig} °С " +
                                                                      $"для стали {steelName} при которой определяется значение временного сопротивления по ГОСТ 34233.1-2017");
                    }
                    else
                    {
                        tempBig = steel.Values[i].Temperature;
                        RmLittle = steel.Values[i].RmValue[accessI];
                    }
                }

                Rm = RmBig - ((RmBig - RmLittle) * (temperature - tempLittle) / (tempBig - tempLittle));
                Rm = Math.Round(Rm);
                return Rm;
            }

        }


        public static class Gost6533
        {
            private const string ELLIPTICAL_BOTTOM_TABLE = "PhysicalData/Gost6533/EllipticalBottom.json";
            public static List<string> GetEllipseDiameters(EllipticalBottomGostType type)
            {
                EllipsesList ellipsesList;

                try
                {
                    using StreamReader file = new(ELLIPTICAL_BOTTOM_TABLE);
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
                    using StreamReader file = new(ELLIPTICAL_BOTTOM_TABLE);
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
            private const string TABLE_ALPHA = "PhysicalData/Gost34233_4/SteelsAlfa.json";


            /// <summary>
            /// 
            /// </summary>
            /// <param name="M"></param>
            /// <param name="isGroove"></param>
            /// <returns></returns>
            /// <exception cref="PhysicalDataException"></exception>
            public static double Getfb(int M, bool isGroove)
            {
                List<Fb> fbs;

                try
                {
                    using StreamReader file = new(TABLE_D1_SCREW_M);
                    var json = file.ReadToEnd();
                    file.Close();
                    fbs = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_4.Fb>>(json);
                }
                catch
                {
                    throw new PhysicalDataException($"Error open file {TABLE_D1_SCREW_M} for fb");
                }

                var fbValue = fbs?.FirstOrDefault(f => f.M == M) ?? 
                              throw new PhysicalDataException($"Error find value for fb for M {M} in file {TABLE_D1_SCREW_M}");

                return isGroove ? fbValue.fb_groove : fbValue.fb;
            }

            public static IEnumerable<string> GetScrewDs()
            {
                List<Fb> fbs;

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

            /// <summary>
            /// 
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            /// <exception cref="PhysicalDataException"></exception>
            public static (double m, double qobj, double q_d, double Kobj, double Ep, bool isFlat, bool isMetall) GetGasketParameters(string name)
            {
                List<Gasket> gaskets;

                try
                {
                    using StreamReader file = new(TABLE_GASKET);
                    var json = file.ReadToEnd();
                    file.Close();
                    gaskets = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_4.Gasket>>(json);
                }
                catch
                {
                    throw new PhysicalDataException($"Error open file {TABLE_GASKET} for gasket parameters");
                }

                var gasket = gaskets?.FirstOrDefault(g => g.Material == name) ??
                             throw new PhysicalDataException($"Couldn't find gasket parameters for material {name}");

                return (gasket.m, gasket.qobj, gasket.q_d, gasket.Kobj, gasket.Ep, gasket.IsFlat, gasket.IsMetal);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="steelName"></param>
            /// <param name="temperature"></param>
            /// <returns></returns>
            /// <exception cref="PhysicalDataException"></exception>
            public static double GetSigma(string steelName, double temperature)
            {
                List<SteelForSigma> steels;

                try
                {
                    using StreamReader file = new(TABLE_SIGMA);
                    var json = file.ReadToEnd();
                    file.Close();
                    steels = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_4.SteelForSigma>>(json);
                }
                catch
                {
                    throw new PhysicalDataException($"Error open file {TABLE_SIGMA} for sigma of steel {steelName}");
                }

                var steel = steels?.FirstOrDefault(s => s.Name.Contains(steelName)) ??
                            throw new PhysicalDataException($"Error find steel {steelName} in file {TABLE_SIGMA}");


                steel.Values = steel.Values.Where(v => v.SigmaValue != 0).ToList();

                double sigmaLittle = 0, sigmaBig = 0;
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
                        throw new PhysicalDataException($"Температура {temperature} °С, больше чем максимальная температура {tempBig} °С " +
                                                                      $"для стали {steelName} при которой определяется допускаемое напряжение по ГОСТ 34233.4-2017");
                    }
                    else
                    {
                        tempBig = steel.Values[i].Temperature;
                        sigmaLittle = steel.Values[i].SigmaValue;
                    }
                }

                var sigmaAlloy = sigmaBig - (sigmaBig - sigmaLittle) * (temperature - tempLittle) / (tempBig - tempLittle);
                sigmaAlloy *= 10;
                sigmaAlloy = Math.Truncate(sigmaAlloy / 5);
                sigmaAlloy *= 0.5;

                return sigmaAlloy;
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

            /// <summary>
            /// 
            /// </summary>
            /// <param name="etaT"></param>
            /// <returns></returns>
            /// <exception cref="PhysicalDataException"></exception>
            public static double Getpsi0(double etaT)
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
                    throw new PhysicalDataException($"Error open file {TABLE_B1} for psi0");
                }

                var etaTRound = Math.Round(etaT * 20) / 20;

                var result = psiList?.FirstOrDefault(p => Math.Abs(p.etaT - etaTRound) < 0.00001) ??
                             throw new PhysicalDataException($"Couldn't find value of psi0 for etaT={etaT}");

                return result.psi0;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="D"></param>
            /// <returns></returns>
            /// <exception cref="PhysicalDataException"></exception>
            public static double GetpW_d(double D)
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
                    throw new PhysicalDataException($"Error open table {TABLE_2} for [W]");
                }

                var result = Ws?.LastOrDefault(w => w.D <= D) ??
                             throw new PhysicalDataException($"Couldn't find value of [W] for D={D}");

                return result.W_d;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="omega"></param>
            /// <param name="mn"></param>
            /// <returns></returns>
            /// <exception cref="PhysicalDataException"></exception>
            public static (double T1, double T2, double T3) TryGetT1T2T3(double omega, double mn)
            {
                if (omega is < 0 or > 10)
                {
                    throw new PhysicalDataException($"Error input value for omega {omega} value must be in range 0-10");
                }

                if (mn is < 1 or > 1.54)
                {
                    throw new PhysicalDataException($"Error input value for mn {mn} value must be in range 1.0-1.5");
                }

                var omegaRound = omega <= 4
                        ? Math.Round(omega * 2.0) / 2.0
                        : Math.Round(omega);
                var mnRound = Math.Round(mn * 10.0) / 10.0;

                List<OmegaForT1T2T3> omegaList;

                try
                {
                    using StreamReader file = new(TABLE_G1);
                    var json = file.ReadToEnd();
                    file.Close();
                    omegaList = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_7.OmegaForT1T2T3>>(json);
                }
                catch
                {
                    throw new PhysicalDataException("Error open file for T1, T2, T3");
                }


                var result = omegaList
                    ?.FirstOrDefault(o => Math.Abs(o.Omega - omegaRound) < 0.00001)
                    ?.MnList
                    .FirstOrDefault(m => Math.Abs(m.mn - mnRound) < 0.00001) ??
                             throw new PhysicalDataException($"Couldn't find value of T1, T2, T3 for omega={omega} and mn={mn}");
                return (result.T1, result.T2, result.T3);
            }


            /// <summary>
            /// 
            /// </summary>
            /// <param name="omega"></param>
            /// <returns></returns>
            /// <exception cref="PhysicalDataException"></exception>
            public static (double Phi1, double Phi2, double Phi3) TryGetPhi1Phi2Phi3(double omega)
            {
                if (omega is < 0 or > 11.5)
                {
                    throw new PhysicalDataException($"Error input value for omega {omega} value must be in range 0-11");
                }

                var omegaRound = omega <= 4
                    ? Math.Round(omega * 2.0) / 2.0
                    : Math.Round(omega);

                if (omegaRound == 11)
                {
                    return (Math.Sqrt(2) * omega, omega, Math.Sqrt(2) * omega);
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
                    throw new PhysicalDataException("Error open file for Phi1, Phi2, Phi3");
                }


                var result = omegaList
                    ?.FirstOrDefault(o => Math.Abs(o.Omega - omegaRound) < 0.00001) ??
                             throw new PhysicalDataException($"Couldn't find value of Phi1, Phi2, Phi3 for omega={omega}");

                return (result.Phi1, result.Phi2, result.Phi3);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="omega"></param>
            /// <param name="mA"></param>
            /// <returns></returns>
            /// <exception cref="PhysicalDataException"></exception>
            public static double GetA(double omega, double mA)
            {
                if (omega is < 0 or > 10)
                {
                    throw new PhysicalDataException($"Error input value for omega {omega} value must be in range 0-10");
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

                List<OmegaForAB> omegaList;

                try
                {
                    using StreamReader file = new(TABLE_G23);
                    var json = file.ReadToEnd();
                    file.Close();
                    omegaList = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_7.OmegaForAB>>(json);
                }
                catch
                {
                    throw new PhysicalDataException("Error open file for A");
                }


                var result = omegaList
                    ?.FirstOrDefault(o => Math.Abs(o.Omega - omegaRound) < 0.00001)
                    ?.mAnBList
                    .FirstOrDefault(m => Math.Abs(m.Value - mARound) < 0.00001) ??
                             throw new PhysicalDataException($"Couldn't find value of A for omega={omega} and mA={mA}");
                return result.A;
            }


            /// <summary>
            /// 
            /// </summary>
            /// <param name="omega"></param>
            /// <param name="nB"></param>
            /// <returns></returns>
            /// <exception cref="PhysicalDataException"></exception>
            public static double GetB(double omega, double nB)
            {
                if (omega is < 0 or > 10)
                {
                    throw new PhysicalDataException($"Error input value for omega {omega} value must be in range 0-10");
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

                List<OmegaForAB> omegaList;

                try
                {
                    using StreamReader file = new(TABLE_G23);
                    var json = file.ReadToEnd();
                    file.Close();
                    omegaList = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_7.OmegaForAB>>(json);
                }
                catch
                {
                    throw new PhysicalDataException("Error open file for B");
                }


                var result = omegaList
                    ?.FirstOrDefault(o => Math.Abs(o.Omega - omegaRound) < 0.00001)
                    ?.mAnBList
                    .FirstOrDefault(n => Math.Abs(n.Value - nBRound) < 0.00001) ??
                             throw new PhysicalDataException($"Couldn't find value of B for omega={omega} and nB={nB}");

                return result.B;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="steelName"></param>
        /// <param name="temperature"></param>
        /// <param name="gost"></param>
        /// <returns></returns>
        /// <exception cref="PhysicalDataException"></exception>
        public static double GetAlpha(string steelName, double temperature, string gost = "Gost34233_1")
        {
            List<SteelForAlpha> steels;

            try
            {
                using StreamReader file = new($"PhysicalData/{gost}/SteelsAlfa.json");
                var json = file.ReadToEnd();
                file.Close();
                steels = JsonSerializer.Deserialize<List<SteelForAlpha>>(json);
            }
            catch
            {
                throw new PhysicalDataException($"Can't open file for alpha for steel {steelName} in GOST {gost}");
            }

            var alphaList = steels
                ?.FirstOrDefault(s => s.Name.Contains(steelName))
                ?.Values
                .Where(v => v.AlphaValue != 0).ToList() ??
                           throw new PhysicalDataException($"Couldn't find alpha values for steel={steelName} in GOST {gost}");


            foreach (var alpha in alphaList.Where(alpha => alpha.Temperature >= temperature))
            {
                return alpha.AlphaValue;
            }

            throw new PhysicalDataException($"Couldn't find alpha value for steel={steelName} on temperature {temperature} in GOST {gost}");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="steelName"></param>
        /// <param name="temperature"></param>
        /// <param name="gost"></param>
        /// <returns></returns>
        /// <exception cref="PhysicalDataException"></exception>
        public static double GetE(string steelName, double temperature, string gost = "Gost34233_1")
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
                throw new PhysicalDataException($"Can't open file for E for steel {steelName} in GOST {gost}");
            }

            var Elist = steels
                ?.FirstOrDefault(s => s.Name.Contains(steelName))
                ?.Values
                ?.Where(v => v.EValue != 0).ToList() ??
                        throw new PhysicalDataException($"Error find steel {steelName}");

            double ELittle = 0, EBig = 0;
            double tempLittle = 0, tempBig = 0;

            for (var i = 0; i < Elist.Count; i++)
            {
                if ((i == 0 && Elist[i].Temperature > temperature) ||
                    Elist[i].Temperature == temperature)
                {
                    return Elist[i].EValue;
                }
                else if (Elist[i].Temperature > temperature)
                {
                    tempLittle = Elist[i].Temperature;
                    EBig = Elist[i].EValue;
                    break;
                }
                else if (i == Elist.Count - 1)
                {
                    throw new PhysicalDataException($"Температура {temperature} °С, больше чем максимальная температура {tempBig} °С " +
                                                                  $"для стали {steelName} при которой определяется модуль продольной упругости по {gost}-2017");
                }
                else
                {
                    tempBig = Elist[i].Temperature;
                    ELittle = Elist[i].EValue;
                }
            }

            var E = EBig - (EBig - ELittle) * (temperature - tempLittle) / (tempBig - tempLittle);
            E = Math.Truncate(E);

            return E;
        }
    }
}
