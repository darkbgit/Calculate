using CalculateVessels.Data.Enums;
using CalculateVessels.Data.Exceptions;
using CalculateVessels.Data.Interfaces;
using CalculateVessels.Data.PhysicalData.Common;
using CalculateVessels.Data.PhysicalData.Gost34233_1;
using CalculateVessels.Data.PhysicalData.Gost34233_4;
using CalculateVessels.Data.PhysicalData.Gost34233_4.Models;
using CalculateVessels.Data.PhysicalData.Gost6533.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using CalculateVessels.Data.PhysicalData.Gost34233_7;

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

    public double GetRm(string steelName, double temperature, RmSource source)
    {
        return source switch
        {
            RmSource.G34233D1 => Gost34233D1.GetRm(steelName, temperature),
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

    public SteelType Gost34233D1GetSteelType(string steelName)
    {
        return Gost34233D1.GetSteelType(steelName);
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

    public (double phi1, double phi2, double phi3) Gost34233D7GetPhi1Phi2Phi3(double omega)
    {
        return Gost34233D7.GetPhi1Phi2Phi3(omega);
    }

    public double Gost34233D7GetA(double omega, double mA)
    {
        return Gost34233D7.GetA(omega, mA);
    }

    public double Gost34233D7GetB(double omega, double nB)
    {
        return Gost34233D7.GetB(omega, nB);
    }

    public double Gost34233D7GetWd(double D)
    {
        return Gost34233D7.GetWd(D);
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