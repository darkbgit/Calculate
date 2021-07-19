﻿using CalculateVessels.Data.PhysicalData.Gost6533;
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

            public static double GetSigma(string steelName, double temperature,
                ref List<string> dataInErr,
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
                    return 0;
                }

                var accessI = 0;

                if (isBigResource == true & isBigThickness == false)
                {
                    accessI = 1;
                }
                else if (isBigResource == false & isBigThickness == true)
                {
                    accessI = 2;
                }
                else if (isBigResource == true & isBigThickness == true)
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
                        dataInErr.Add($"Температура {temperature} °С, больше чем максимальная температура {tempBig} °С " +
                                      $"для стали {steelName} при которой определяется допускаемое напряжение по ГОСТ 34233.1-2017");
                        return 0;
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

            public static (double m, double qobj, double q_d, double Kobj, double Ep) GetGasketParameters(string name)
            {
                var gaskets = new List<PhysicalData.Gost34233_4.Gasket>();

                try
                {
                    using StreamReader file = new("PhysicalData/Gost34233_4/ScrewSquare.json");
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
                    return (gasket.m, gasket.qobj, gasket.q_d, gasket.Kobj, gasket.Ep);
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
                    if ((i == 0 && steel.Values[i].Temperature > temperature) ||
                        steel.Values[i].Temperature == temperature)
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

        }
        
    }

    
}
