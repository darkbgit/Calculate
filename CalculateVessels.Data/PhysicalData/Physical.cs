using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace CalculateVessels.Data.PhysicalData
{
    public static class Physical
    {
        public static string[] GetSteelsList()
        {
            using StreamReader file = new("PhysicalData/SteelsSigma.json");
            try
            {
                var json = file.ReadToEnd();
                var steels = JsonSerializer.Deserialize<List<SteelForSigma>>(json);
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
            var steels = new List<SteelForSigma>();

            try
            {
                using StreamReader file = new("PhysicalData/SteelsSigma.json");
                var json = file.ReadToEnd();
                file.Close();
                steels = JsonSerializer.Deserialize<List<SteelForSigma>>(json);
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
            
            var steel = steels.Where(s => s.Name.Equals(steelName)).FirstOrDefault();

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
            var steels = new List<SteelForE>();

            try
            {
                using StreamReader file = new("PhysicalData/SteelsSigma.json");
                var json = file.ReadToEnd();
                file.Close();
                steels = JsonSerializer.Deserialize<List<SteelForE>>(json);
            }
            catch
            {
                return 0;
            }

            var steel = steels.Where(s => s.Name.Equals(steelName)).FirstOrDefault();

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
}
