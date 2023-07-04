using System.Globalization;
using CalculateVessels.Core.Enums;

namespace CalculateVessels.Helpers;

internal static class Parameters
{
    public static T GetParam<T>(string? paramValue, string paramName, List<string> errorList, NumberStyles numberStyles = NumberStyles.Float)
        where T : struct
    {
        if (string.IsNullOrEmpty(paramValue))
        {
            return default;
        }

        var paramValueWithDot = paramValue.Replace(',', '.');


        switch (typeof(T))
        {
            case var value when value == typeof(double):
                if (double.TryParse(paramValueWithDot, numberStyles,
                        CultureInfo.InvariantCulture, out var paramDoubleValue))
                {
                    return (T)(object)paramDoubleValue;
                }
                break;
            case var value when value == typeof(int):
                if (int.TryParse(paramValueWithDot, numberStyles, CultureInfo.InvariantCulture, out var paramIntValue))
                {
                    return (T)(object)paramIntValue;
                }
                break;
            case var value when value == typeof(BracketVerticalType):
                if (Enum.TryParse(paramValueWithDot, out BracketVerticalType paramBracketVerticalType))
                {
                    return (T)(object)paramBracketVerticalType;
                }
                break;
        }

        errorList.Add($"{paramName} неверный ввод");
        return default;
    }
}