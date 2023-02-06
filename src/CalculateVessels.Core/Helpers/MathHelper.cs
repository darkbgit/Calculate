using System;

namespace CalculateVessels.Core.Helpers;

internal static class MathHelper
{
    public static double DegreeToRadian(double degree) => degree * Math.PI / 180;
}