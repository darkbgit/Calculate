using CalculateVessels.Data.Enums;
using CalculateVessels.Data.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculateVessels.Data.Utilities;

internal static class Interpolations
{
    public static double InterpolationForParametersWithList(Dictionary<double, List<double>> values, double temperature, int accessIndex, RoundType round)
    {
        var maxTemperature = values.Keys.Max();

        if (temperature > maxTemperature)
            throw new PhysicalDataException(maxTemperature);

        var minTemperature = values.Keys.Min();

        if (temperature <= minTemperature)
        {
            return values[minTemperature][accessIndex];
        }

        if (values.TryGetValue(temperature, out List<double> valuesList))
        {
            return valuesList[accessIndex];
        }

        var temperatureBig = values.Keys.First(k => k > temperature);

        var temperatureLittle = values.Keys.Last(k => k < temperature);


        var value = Interpolation((temperatureBig, values[temperatureBig][accessIndex]), (temperatureLittle, values[temperatureLittle][accessIndex]), temperature, round);

        return value;
    }

    public static double InterpolationForParameters(Dictionary<double, double> values, double temperature, RoundType round)
    {
        var maxTemperature = values.Keys.Max();

        if (temperature > maxTemperature)
            throw new PhysicalDataException(maxTemperature);

        var minTemperature = values.Keys.Min();

        if (temperature <= minTemperature)
        {
            return values[minTemperature];
        }

        if (values.TryGetValue(temperature, out double value))
        {
            return value;
        }

        var temperatureBig = values.Keys.First(k => k > temperature);

        var temperatureLittle = values.Keys.Last(k => k < temperature);

        if (Math.Abs(values[temperatureBig] - values[temperatureLittle]) < 0.000001)
        {
            return values[temperatureBig];
        }

        var result = Interpolation((temperatureBig, values[temperatureBig]), (temperatureLittle, values[temperatureLittle]), temperature, round);

        return result;
    }

    private static double Interpolation((double x, double y) first, (double x, double y) second, double interpolateFor, RoundType round)
    {
        if (Math.Abs(first.x - second.x) < 0.000001 || Math.Abs(first.y - second.y) < 0.000001)
            throw new PhysicalDataException($"Couldn't interpolate values {first} - {second}.");

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