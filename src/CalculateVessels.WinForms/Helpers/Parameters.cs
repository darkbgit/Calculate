using System;
using System.Collections.Generic;
using System.Globalization;
using CalculateVessels.Core.Enums;

namespace CalculateVessels.Helpers;

internal static class Parameters
{
    public static T GetParam<T>(string? paramValue, string paramName, List<string> errorList, NumberStyles numberStyles = NumberStyles.AllowDecimalPoint)
        where T : struct
    {
        if (string.IsNullOrEmpty(paramValue))
        {
            return default;
        }

        switch (typeof(T))
        {
            case var value when value == typeof(double):
                if (double.TryParse(paramValue, numberStyles,
                        CultureInfo.InvariantCulture, out var paramDoubleValue))
                {
                    return (T)(object)paramDoubleValue;
                }
                break;
            case var value when value == typeof(int):
                if (int.TryParse(paramValue, numberStyles, CultureInfo.InvariantCulture, out var paramIntValue))
                {
                    return (T)(object)paramIntValue;
                }
                break;
            case var value when value == typeof(BracketVerticalType):
                if (Enum.TryParse(paramValue, out BracketVerticalType paramBracketVerticalType))
                {
                    return (T)(object)paramBracketVerticalType;
                }
                break;

        }

        errorList.Add($"{paramName} неверный ввод");
        return default;
    }
}