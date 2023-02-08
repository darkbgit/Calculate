using CalculateVessels.Data.Enums;
using CalculateVessels.Data.Exceptions;
using CalculateVessels.Data.PhysicalData.Common;
using CalculateVessels.Data.PhysicalData.Gost34233_1.Models;
using CalculateVessels.Data.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace CalculateVessels.Data.PhysicalData.Gost34233_1;

internal static class Gost34233D1
{
    private const string GostName = "ГОСТ 34233.1-2017";
    private const string GostFolder = "Gost34233_1";
    private const string TableSigma = "SteelsSigma.json";
    private const string TableE = "SteelsE.json";
    private const string TABLE_TYPE = "PhysicalData/Gost34233_1/SteelsType.json";
    private const string TABLE_RM = "PhysicalData/Gost34233_1/SteelsRm.json";

    public static IEnumerable<string> GetSteelsList()
    {
        List<SteelWithName> steels;

        const string fileName = $"{Constants.DataFolder}/{GostFolder}/Data/{TableSigma}";

        using StreamReader file = new(fileName);
        try
        {
            var json = file.ReadToEnd();
            steels = JsonSerializer.Deserialize<List<SteelWithName>>(json)
                ?? throw new InvalidOperationException();
        }
        catch (Exception ex)
        {
            throw new PhysicalDataException($"{GostName}. Couldn't get steels.", ex);
        }

        var result = Enumerable.Empty<string>();
        steels.ForEach(s => result = result.Union(s.Name));
        result = result.OrderByDescending(s => s);

        return result;
    }

    public static SteelType GetSteelType(string steelName)
    {
        List<SteelWithNameAndSteelType> steels;

        const string fileName = $"{Constants.DataFolder}/{GostFolder}/{TABLE_TYPE}";

        try
        {
            using StreamReader file = new(fileName);
            var json = file.ReadToEnd();
            file.Close();
            steels = JsonSerializer.Deserialize<List<SteelWithNameAndSteelType>>(json);
        }
        catch
        {
            throw new PhysicalDataException($"Error open file for SteelType of {steelName}");
        }

        var steel = steels?.FirstOrDefault(s => s.Name.Contains(steelName)) ??
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
        const string fileName = $"{Constants.DataFolder}/{GostFolder}/Data/{TableSigma}";

        var isBigResource = N switch
        {
            <= 1000 => false,
            >= 2000 => true,
            _ => true
        };

        List<SteelWithListValuesAndThickness>? steels;

        try
        {
            using StreamReader file = new(fileName);
            var json = file.ReadToEnd();
            file.Close();
            steels = JsonSerializer.Deserialize<List<SteelWithListValuesAndThickness>>(json);
        }
        catch
        {
            throw new PhysicalDataException($"{GostName}. Couldn't open file {fileName} for sigma.");
        }

        var steel = steels?.FirstOrDefault(st => st.Name.Contains(steelName)) ??
                    throw new PhysicalDataException($"{GostName}. Steel {steelName} wasn't found.");

        var isBigThickness = steel.IsCouldBigThickness && steel.BigThickness < s;

        var accessIndex = 0;

        if (isBigResource & !isBigThickness)
        {
            accessIndex = 1;
        }
        else if (!isBigResource & isBigThickness)
        {
            accessIndex = 2;
        }
        else if (isBigResource & isBigThickness)
        {
            accessIndex = 3;
        }

        try
        {
            var sigmaAllow = Interpolations.InterpolationForParametersWithList(steel.Values, temperature, accessIndex,
                RoundType.WithAccuracy05);
            return sigmaAllow;
        }
        catch (PhysicalDataException ex)
        {
            if (ex.MaxTemperatureError)
            {
                throw new PhysicalDataException(
                    $"{GostName}. Температура {temperature} °С, больше чем максимальная температура {ex.MaxTemperature} °С " +
                    $"для стали {steelName} при которой определяется допускаемое напряжение.");
            }

            throw;
        }
    }

    public static double GetE(string steelName, double temperature)
    {
        const string fileName = $"{Constants.DataFolder}/{GostFolder}/Data/{TableE}";

        List<SteelWithValues>? steels;

        try
        {
            using StreamReader file = new(fileName);
            var json = file.ReadToEnd();
            file.Close();
            steels = JsonSerializer.Deserialize<List<SteelWithValues>>(json);
        }
        catch
        {
            throw new PhysicalDataException($"{GostName}. Couldn't open file {fileName} for E.");
        }

        var EList = steels?.FirstOrDefault(s => s.Name.Contains(steelName))
                    ?? throw new PhysicalDataException($"{GostName}. Steel {steelName} wasn't found.");

        try
        {
            var E = Interpolations.InterpolationForParameters(EList.Values, temperature, RoundType.Integer);
            return E;
        }
        catch (PhysicalDataException ex)
        {
            if (ex.MaxTemperatureError)
            {
                throw new PhysicalDataException(
                    $"{GostName}. Температура {temperature} °С, больше чем максимальная температура {ex.MaxTemperature} °С " +
                    $"для стали {steelName} при которой определяется модуль продольной упругости.");
            }

            throw;
        }
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
        List<SteelWithListValuesAndThickness> steels;

        try
        {
            using StreamReader file = new(TABLE_RM);
            var json = file.ReadToEnd();
            file.Close();
            steels = JsonSerializer.Deserialize<List<SteelWithListValuesAndThickness>>(json);
        }
        catch
        {
            throw new PhysicalDataException($"Error open file for Rm {steelName}");
        }

        var steel = steels?.FirstOrDefault(st => st.Name.Contains(steelName)) ??
                    throw new PhysicalDataException($"Error find steel {steelName}");

        var accessIndex = steel.IsCouldBigThickness && steel.BigThickness < s ? 1 : 0;

        try
        {
            var Rm = Interpolations.InterpolationForParametersWithList(steel.Values, temperature, accessIndex,
                RoundType.Integer);
            return Rm;
        }
        catch (PhysicalDataException ex)
        {
            if (ex.MaxTemperatureError)
            {
                throw new PhysicalDataException(
                    $"Температура {temperature} °С, больше чем максимальная температура {ex.MaxTemperature} °С " +
                    $"для стали {steelName} при которой определяется значение временного сопротивления по ГОСТ 34233.1-2017");
            }

            throw;
        }
    }
}