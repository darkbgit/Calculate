using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using CalculateVessels.Data.Enums;
using CalculateVessels.Data.Exceptions;
using CalculateVessels.Data.Intrpolations;
using CalculateVessels.Data.PhysicalData.Common;
using CalculateVessels.Data.PhysicalData.Gost34233_1;
using CalculateVessels.Data.Utilities;

namespace CalculateVessels.Data.PhysicalData.Gost34233_1;

public static class Gost34233_1
{
    private const string GOST_FOLDER = "Gost34233_1";
    private const string TABLE_STEELS = "Steels.json";
    private const string TABLE_SIGMA = "SteelsSigma.json";
    private const string TABLE_TYPE = "PhysicalData/Gost34233_1/SteelsType.json";
    private const string TABLE_RM = "PhysicalData/Gost34233_1/SteelsRm.json";
    //private const string TABLE_TYPE = "PhysicalData/Gost34233_1/SteelsType.json";

    public static IEnumerable<string> GetSteelsList()
    {
        List<PhysicalData.Gost34233_1.Steel>? steels;

        var fileName = $"{Constants.DataFolder}/{GOST_FOLDER}/{TABLE_SIGMA}";

        using StreamReader file = new(fileName);
        try
        {
            var json = file.ReadToEnd();
            steels = JsonSerializer.Deserialize<List<PhysicalData.Gost34233_1.Steel>>(json);
        }
        catch (Exception ex)
        {
            throw new PhysicalDataException($"Couldn't get steels from {fileName}.", ex);
        }

        var result = Enumerable.Empty<string>();
        steels?.ForEach(s => result = result.Union(s.Name));
        result = result.OrderByDescending(s => s);

        return result;
    }

    public static SteelType GetSteelType(string steelName)
    {
        List<SteelForSteelType> steels;

        try
        {
            using StreamReader file = new(TABLE_TYPE);
            var json = file.ReadToEnd();
            file.Close();
            steels = JsonSerializer.Deserialize<List<SteelForSteelType>>(json);
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
        var isBigResource = N switch
        {
            1000 => false,
            2000 => true,
            _ => true
        };

        List<SteelWithListValuesAndThickness> steels;

        try
        {
            using StreamReader file = new(TABLE_SIGMA);
            var json = file.ReadToEnd();
            file.Close();
            steels = JsonSerializer.Deserialize<List<SteelWithListValuesAndThickness>>(json);
        }
        catch
        {
            throw new PhysicalDataException($"Error open file for sigma {steelName}");
        }

        var steel = steels?.FirstOrDefault(st => st.Name.Contains(steelName)) ??
                    throw new PhysicalDataException($"Error find steel {steelName}");

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
                    $"Температура {temperature} °С, больше чем максимальная температура {ex.MaxTemperature} °С " +
                    $"для стали {steelName} при которой определяется допускаемое напряжение по ГОСТ 34233.1-2017");
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