using CalculateVessels.Data.Enums;
using CalculateVessels.Data.Exceptions;
using CalculateVessels.Data.PhysicalData.Common;
using CalculateVessels.Data.PhysicalData.Gost34233_4.Models;
using CalculateVessels.Data.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace CalculateVessels.Data.PhysicalData.Gost34233_4;

internal class Gost34233D4
{
    private const string GostName = "ГОСТ 34233.4-2017";
    private const string GostFolder = "Gost34233_4/Data";
    private const string TableD1ScrewM = "TableD1.json";
    private const string TableE = "SteelsE.json";
    private const string TableSigma = "SteelsSigma.json";
    private const string TableGasket = "Gaskets.json";
    private const string TableAlpha = "SteelsAlpha.json";

    public static double Getfb(int M, bool isGroove)
    {
        var fileName = Path.Combine(Constants.DataFolder, GostFolder, TableD1ScrewM);

        Dictionary<int, Fb>? fbs;

        try
        {
            using StreamReader file = new(fileName);
            var json = file.ReadToEnd();
            file.Close();
            fbs = JsonSerializer.Deserialize<Dictionary<int, Fb>>(json);
        }
        catch
        {
            throw new PhysicalDataException($"{GostName}. Couldn't open file {fileName} for fb.");
        }

        if (fbs == null || !fbs.ContainsKey(M))
        {
            throw new PhysicalDataException($"{GostName}. Fb value for M {M} wasn't found in file {fileName}.");
        }

        return isGroove ? fbs[M].fbGroove : fbs[M].fb;
    }

    public static IEnumerable<string> GetScrewDs()
    {
        var fileName = Path.Combine(Constants.DataFolder, GostFolder, TableD1ScrewM);

        Dictionary<int, Fb>? fbs;

        try
        {
            using StreamReader file = new(fileName);
            var json = file.ReadToEnd();
            file.Close();
            fbs = JsonSerializer.Deserialize<Dictionary<int, Fb>>(json);
        }
        catch
        {
            throw new PhysicalDataException($"{GostName}. Couldn't open file {fileName} for screw ds.");
        }

        if (fbs == null || !fbs.Any())
        {
            throw new PhysicalDataException($"{GostName}. Screw ds weren't found in {TableD1ScrewM}.");
        }

        return fbs.Keys.Select(f => f.ToString()).AsEnumerable();
    }


    public static Gasket GetGasketParameters(string materialName)
    {
        var fileName = Path.Combine(Constants.DataFolder, GostFolder, TableGasket);

        List<Gasket>? gaskets;

        try
        {
            using StreamReader file = new(fileName);
            var json = file.ReadToEnd();
            file.Close();
            gaskets = JsonSerializer.Deserialize<List<Gasket>>(json);
        }
        catch
        {
            throw new PhysicalDataException($"{GostName}. Couldn't open file {fileName} for gasket parameters.");
        }

        return gaskets?.FirstOrDefault(g => g.Material == materialName) ??
                     throw new PhysicalDataException($"{GostName}. Couldn't find gasket parameters for material {materialName}.");
    }

    public static double GetSigma(string steelName, double temperature)
    {
        var fileName = Path.Combine(Constants.DataFolder, GostFolder, TableSigma);

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
            throw new PhysicalDataException($"{GostName}. Couldn't open file {fileName} for sigma.");
        }

        var steel = steels?.FirstOrDefault(s => s.Name.Contains(steelName)) ??
                    throw new PhysicalDataException($"{GostName}. Steel \"{steelName}\" wasn't found.");

        try
        {
            var sigmaAllow = Interpolations.InterpolationForParameters(steel.Values, temperature, RoundType.WithAccuracy05);
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
        var fileName = Path.Combine(Constants.DataFolder, GostFolder, TableGasket);

        List<Gasket>? gaskets;

        try
        {
            using StreamReader file = new(fileName);
            var json = file.ReadToEnd();
            file.Close();
            gaskets = JsonSerializer.Deserialize<List<Gasket>>(json);
        }
        catch
        {
            throw new PhysicalDataException($"{GostName}. Couldn't open file {fileName} for gasket parameters.");
        }

        if (gaskets == null || !gaskets.Any())
        {
            throw new PhysicalDataException($"{GostName}. Gaskets weren't found.");
        }

        return gaskets.Select(g => g.Material);
    }

    public static IEnumerable<string> GetSteelsList(SteelSource source)
    {
        var table = source switch
        {
            SteelSource.G34233D4Screw => TableSigma,
            SteelSource.G34233D4Washer => TableE,
            _ => throw new PhysicalDataException($"{GostName}. Type {source} for steels list is wrong.")
        };

        var fileName = Path.Combine(Constants.DataFolder, GostFolder, table);

        List<Steel> steels;

        try
        {
            using StreamReader file = new(fileName);
            var json = file.ReadToEnd();
            file.Close();
            steels = JsonSerializer.Deserialize<List<Steel>>(json) ?? throw new InvalidOperationException();
        }
        catch
        {
            throw new PhysicalDataException($"{GostName}. Couldn't open file {fileName} for steels list.");
        }

        List<string> result = new();
        steels.ForEach(s => result = result.Union(s.Name).ToList());

        return result;
    }

    public static double GetE(string steelName, double temperature)
    {
        var fileName = Path.Combine(Constants.DataFolder, GostFolder, TableE);

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
                    ?? throw new PhysicalDataException($"{GostName}. Steel \"{steelName}\" wasn't found.");

        try
        {
            return Interpolations.InterpolationForParameters(EList.Values, temperature, RoundType.Integer);
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

    public static double GetAlpha(string steelName, double temperature)
    {
        var fileName = Path.Combine(Constants.DataFolder, GostFolder, TableAlpha);

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
            throw new PhysicalDataException($"{GostName}. Couldn't open file {fileName} for alpha.");
        }

        var alphaList = steels
                            ?.FirstOrDefault(s => s.Name.Contains(steelName))
                        ?? throw new PhysicalDataException($"{GostName}. Couldn't find alpha values for steel \"{steelName}\".");

        if (!alphaList.Values.Keys.Any(k => k >= temperature))
            throw new PhysicalDataException(
                $"{GostName}. Couldn't find alpha value for steel \"{steelName}\" temperature \"{temperature}\".");

        var key = alphaList.Values.Keys.First(k => k >= temperature);
        return alphaList.Values[key];
    }
}