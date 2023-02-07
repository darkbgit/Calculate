using System.Collections.Generic;
using System.Globalization;

namespace CalculateVessels.Helpers;

internal static class Parameters
{
    public static T GetParam<T>(string? paramValue, string paramName, ref List<string> errorList, NumberStyles numberStyles = NumberStyles.AllowDecimalPoint)
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

        }

        errorList.Add($"{paramName} неверный ввод");
        return default;
    }
}