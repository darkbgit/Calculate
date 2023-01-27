using CalculateVessels.Data.Enums;
using CalculateVessels.Data.Exceptions;
using CalculateVessels.Data.PhysicalData.Common;
using CalculateVessels.Data.PhysicalData.Gost34233_4;
using CalculateVessels.Data.PhysicalData.Gost34233_7;
using CalculateVessels.Data.PhysicalData.Gost6533;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace CalculateVessels.Data.PhysicalData;

public static class Physical
{
    static Physical()
    {

    }

    public static class Gost6533
    {
        private const string ELLIPTICAL_BOTTOM_TABLE = "PhysicalData/Gost6533/EllipticalBottom.json";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="PhysicalDataException"></exception>
        public static IEnumerable<string> GetEllipseDiameters(EllipticalBottomGostType type)
        {
            EllipsesList ellipsesList;

            try
            {
                using StreamReader file = new(ELLIPTICAL_BOTTOM_TABLE);
                var json = file.ReadToEnd();
                file.Close();
                ellipsesList = JsonSerializer.Deserialize<EllipsesList>(json) ?? throw new InvalidOperationException();
            }
            catch
            {
                throw new PhysicalDataException($"Error open file {ELLIPTICAL_BOTTOM_TABLE} for ellipses diameters.");
            }

            var ellipses = type switch
            {
                EllipticalBottomGostType.Ell025In => ellipsesList.Ell025In.Keys,
                EllipticalBottomGostType.Ell025Out => ellipsesList.Ell025Out.Keys,
                EllipticalBottomGostType.Ell020In => throw new NotImplementedException(),
                _ => throw new PhysicalDataException($"Error open file {ELLIPTICAL_BOTTOM_TABLE} for ellipses diameters.")
            };

            if (!ellipses.Any())
            {
                throw new PhysicalDataException($"Ellipses diameters weren't found in {ELLIPTICAL_BOTTOM_TABLE}.");
            }

            return ellipses.Select(i => i.ToString(CultureInfo.CurrentCulture)).ToList();
        }

        public static EllipsesList GetEllipsesList()
        {
            try
            {
                using StreamReader file = new(ELLIPTICAL_BOTTOM_TABLE);
                var json = file.ReadToEnd();
                file.Close();
                return JsonSerializer.Deserialize<EllipsesList>(json) ?? throw new InvalidOperationException();
            }
            catch
            {
                throw new PhysicalDataException($"Error open file {ELLIPTICAL_BOTTOM_TABLE} for ellipses diameters.");
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
            Dictionary<int, Fb> fbs;

            try
            {
                using StreamReader file = new(TABLE_D1_SCREW_M);
                var json = file.ReadToEnd();
                file.Close();
                fbs = JsonSerializer.Deserialize<Dictionary<int, Fb>>(json) ?? throw new InvalidOperationException();
            }
            catch
            {
                throw new PhysicalDataException($"Error open file {TABLE_D1_SCREW_M} for fb");
            }

            if (fbs == null || !fbs.ContainsKey(M))
            {
                throw new PhysicalDataException($"Error find value for fb for M {M} in file {TABLE_D1_SCREW_M}");
            }

            return isGroove ? fbs[M].fbGroove : fbs[M].fb;
        }

        public static IEnumerable<string> GetScrewDs()
        {
            Dictionary<int, Fb> fbs;

            try
            {
                using StreamReader file = new(TABLE_D1_SCREW_M);
                var json = file.ReadToEnd();
                file.Close();
                fbs = JsonSerializer.Deserialize<Dictionary<int, Fb>>(json) ?? throw new InvalidOperationException();
            }
            catch
            {
                throw new PhysicalDataException($"Error open file {TABLE_D1_SCREW_M} for screw ds.");
            }

            if (!fbs.Any())
            {
                throw new PhysicalDataException($"Screw ds weren't found in {TABLE_D1_SCREW_M}.");
            }

            return fbs.Keys.Select(f => f.ToString()).AsEnumerable();
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
                gaskets = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_4.Gasket>>(json) ?? throw new InvalidOperationException();
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
            List<SteelWithValues> steels;

            try
            {
                using StreamReader file = new(TABLE_SIGMA);
                var json = file.ReadToEnd();
                file.Close();
                steels = JsonSerializer.Deserialize<List<SteelWithValues>>(json) ?? throw new InvalidOperationException();
            }
            catch
            {
                throw new PhysicalDataException($"Error open file {TABLE_SIGMA} for sigma of steel {steelName}");
            }

            var steel = steels.FirstOrDefault(s => s.Name.Contains(steelName)) ??
                        throw new PhysicalDataException($"Error find steel {steelName} in file {TABLE_SIGMA}");

            try
            {
                var sigmaAllow = InterpolationForParameters(steel.Values, temperature, RoundType.WithAccuracy05);
                return sigmaAllow;
            }
            catch (PhysicalDataException ex)
            {
                if (ex.MaxTemperatureError)
                {
                    throw new PhysicalDataException(
                        $"Температура {temperature} °С, больше чем максимальная температура {ex.MaxTemperature} °С " +
                        $"для стали {steelName} при которой определяется допускаемое напряжение по ГОСТ 34233.4-2017");
                }

                throw;
            }
        }


        public static IEnumerable<string> GetGasketsList()
        {
            List<Gasket> gaskets;

            try
            {
                using StreamReader file = new(TABLE_GASKET);
                var json = file.ReadToEnd();
                file.Close();
                gaskets = JsonSerializer.Deserialize<List<Gasket>>(json) ?? throw new InvalidOperationException();
            }
            catch
            {
                throw new PhysicalDataException($"Error open file {TABLE_GASKET} for gaskets list.");
            }

            if (!gaskets.Any())
            {
                throw new PhysicalDataException($"Gaskets weren't found in file {TABLE_GASKET}.");
            }

            return gaskets.Select(g => g.Material);
        }

        public static IEnumerable<string> GetSteelsList(string type)
        {
            var table = type switch
            {
                "screw" => TABLE_SIGMA,
                "washer" => TABLE_E,
                _ => throw new PhysicalDataException("Type for steels list is wrong.")
            };

            List<PhysicalData.Gost34233_4.Steel> steels;

            try
            {
                using StreamReader file = new(table);
                var json = file.ReadToEnd();
                file.Close();
                steels = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_4.Steel>>(json) ?? throw new InvalidOperationException();
            }
            catch
            {
                throw new PhysicalDataException($"Error open file {table} for steels list.");
            }

            List<string> result = new();
            steels.ForEach(s => result = result.Union(s.Name).ToList());

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
                psiList = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_7.Psi>>(json) ?? throw new InvalidOperationException();
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
                Ws = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_7.W>>(json) ?? throw new InvalidOperationException();
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
                omegaList = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_7.OmegaForT1T2T3>>(json) ?? throw new InvalidOperationException();
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
                omegaList = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_7.OmegaForPhi1Phi2Phi3>>(json) ?? throw new InvalidOperationException();
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
                omegaList = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_7.OmegaForAB>>(json) ?? throw new InvalidOperationException();
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
                omegaList = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_7.OmegaForAB>>(json) ?? throw new InvalidOperationException();
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
        const string JSON_NAME = "SteelsAlpha.json";

        List<SteelWithValues> steels;

        try
        {
            using StreamReader file = new($"PhysicalData/{gost}/{JSON_NAME}");
            var json = file.ReadToEnd();
            file.Close();
            steels = JsonSerializer.Deserialize<List<SteelWithValues>>(json) ?? throw new InvalidOperationException();
        }
        catch
        {
            throw new PhysicalDataException($"Can't open file for alpha for steel {steelName} in GOST {gost}");
        }

        var alphaList = steels
                            ?.FirstOrDefault(s => s.Name.Contains(steelName))
                        ?? throw new PhysicalDataException(
                            $"Couldn't find alpha values for steel={steelName} in GOST {gost}");

        if (!alphaList.Values.Keys.Any(k => k >= temperature))
            throw new PhysicalDataException(
                $"Couldn't find alpha value for steel={steelName} at temperature {temperature} in GOST {gost}");

        var key = alphaList.Values.Keys.First(k => k >= temperature);
        return alphaList.Values[key];
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
        List<SteelWithValues> steels;

        try
        {
            using StreamReader file = new($"PhysicalData/{gost}/SteelsE.json");
            var json = file.ReadToEnd();
            file.Close();
            steels = JsonSerializer.Deserialize<List<SteelWithValues>>(json) ?? throw new InvalidOperationException();
        }
        catch
        {
            throw new PhysicalDataException($"Can't open file for E for steel {steelName} in GOST {gost}");
        }

        var EList = steels
            ?.FirstOrDefault(s => s.Name.Contains(steelName))
            ?? throw new PhysicalDataException($"Error find steel {steelName}");

        try
        {
            var E = InterpolationForParameters(EList.Values, temperature, RoundType.Integer);
            return E;
        }
        catch (PhysicalDataException ex)
        {
            if (ex.MaxTemperatureError)
            {
                throw new PhysicalDataException(
                    $"Температура {temperature} °С, больше чем максимальная температура {ex.MaxTemperature} °С " +
                    $"для стали {steelName} при которой определяется модуль продольной упругости по {gost}-2017");
            }

            throw;
        }
    }


    private static double InterpolationForParametersWithList(Dictionary<double, List<double>> values, double temperature, int accessIndex, RoundType round)
    {
        var maxTemperature = values.Keys.Max();

        if (temperature > maxTemperature)
            throw new PhysicalDataException(maxTemperature);

        var minTemperature = values.Keys.Min();

        if (temperature <= minTemperature)
        {
            return values[minTemperature][accessIndex];
        }

        if (values.ContainsKey(temperature))
        {
            return values[temperature][accessIndex];
        }

        var temperatureBig = values.Keys.First(k => k > temperature);

        var temperatureLittle = values.Keys.Last(k => k < temperature);


        var value = Interpolation((temperatureBig, values[temperatureBig][accessIndex]), (temperatureLittle, values[temperatureLittle][accessIndex]), temperature, round);

        return value;
    }

    private static double InterpolationForParameters(Dictionary<double, double> values, double temperature, RoundType round)
    {
        var maxTemperature = values.Keys.Max();

        if (temperature > maxTemperature)
            throw new PhysicalDataException(maxTemperature);

        var minTemperature = values.Keys.Min();

        if (temperature <= minTemperature)
        {
            return values[minTemperature];
        }

        if (values.TryGetValue(temperature, out var value))
        {
            return value;
        }

        var temperatureBig = values.Keys.First(k => k > temperature);

        var temperatureLittle = values.Keys.Last(k => k < temperature);


        var result = Interpolation((temperatureBig, values[temperatureBig]), (temperatureLittle, values[temperatureLittle]), temperature, round);

        return result;
    }

    private static double Interpolation((double x, double y) first, (double x, double y) second, double interpolateFor, RoundType round)
    {
        if (Math.Abs(first.x - second.x) < 0.000001 || Math.Abs(first.y - second.y) < 0.000001)
            throw new PhysicalDataException($"Couldn't interpolate values {first} - {second}");

        if (first.y < second.y)
        {
            (first, second) = (second, first);
        }

        var value = first.y - Math.Abs((first.x - interpolateFor) * (first.y - second.y) / (first.x - second.x));

        switch (round)
        {
            case RoundType.Integer:
                value = Math.Truncate(value);
                break;
            case RoundType.WithAccuracy05:
                value *= 10;
                value = Math.Truncate(value / 5);
                value *= 0.5;
                break;
            case RoundType.None:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(round), round, null);
        }
        return value;
    }
}