﻿using CalculateVessels.Data.Enums;
using CalculateVessels.Data.Exceptions;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Data.PhysicalData.Common;
using CalculateVessels.Data.PhysicalData.Gost34233_1;
using CalculateVessels.Data.PhysicalData.Gost34233_4;
using CalculateVessels.Data.PhysicalData.Gost34233_4.Models;
using CalculateVessels.Data.PhysicalData.Gost34233_7;
using CalculateVessels.Data.PhysicalData.Gost6533.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace CalculateVessels.Data.PhysicalData;

internal class PhysicalDataService : IPhysicalDataService
{
    public double GetSigma(string steelName, double temperature, SigmaSource source = SigmaSource.G34233D1)
    {
        return source switch
        {
            SigmaSource.G34233D1 => Gost34233D1.GetSigma(steelName, temperature),
            SigmaSource.G34233D4 => Gost34233D4.GetSigma(steelName, temperature),
            _ => throw new PhysicalDataException($"{source} isn't supported.")
        };
    }

    public double GetSigma(string steelName, double temperature, double s = 0, int N = 1000)
    {
        return Gost34233D1.GetSigma(steelName, temperature, s, N);
    }

    public double GetE(string steelName, double temperature, ESource source)
    {
        return source switch
        {
            ESource.G34233D1 => Gost34233D1.GetE(steelName, temperature),
            ESource.G34233D4 => Gost34233D4.GetE(steelName, temperature),
            _ => throw new PhysicalDataException($"{source} isn't supported.")
        };
    }

    public double GetAlpha(string steelName, double temperature, AlphaSource source)
    {
        return source switch
        {
            AlphaSource.G34233D1 => Gost34233D1.GetAlpha(steelName, temperature),
            AlphaSource.G34233D4 => Gost34233D4.GetAlpha(steelName, temperature),
            _ => throw new PhysicalDataException($"{source} isn't supported.")
        };
    }

    public IEnumerable<string> GetSteels(SteelSource source)
    {
        return source switch
        {
            SteelSource.G34233D1 => Gost34233D1.GetSteelsList(),
            SteelSource.G34233D4Washer => Gost34233D4.GetSteelsList(source),
            SteelSource.G34233D4Screw => Gost34233D4.GetSteelsList(source),
            _ => throw new PhysicalDataException($"{source} isn't supported.")
        };
    }

    public EllipsesParameters GetEllipsesParameters()
    {
        return Gost6533.Gost6533.GetEllipsesParameters();
    }

    public double Gost34233D4Get_fb(int screwD, bool isScrewWithGroove)
    {
        return Gost34233D4.Getfb(screwD, isScrewWithGroove);
    }

    public Gasket Gost34233D4GetGasketParameters(string materialName)
    {
        return Gost34233D4.GetGasketParameters(materialName);
    }

    public IEnumerable<string> Gost34233D4GetGasketsList()
    {
        return Gost34233D4.GetGasketsList();
    }

    public IEnumerable<string> Gost34233D4GetScrewDs()
    {
        return Gost34233D4.GetScrewDs();
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
                psiList = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_7.Psi>>(json)
                          ?? throw new InvalidOperationException();
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