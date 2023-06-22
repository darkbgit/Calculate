using System;

namespace CalculateVessels.Core.Helpers;

internal static class MathHelper
{
    public static double DegreeToRadian(double degree) => degree * Math.PI / 180;

    public static double RadianToDegree(double radian) => radian * 180 / Math.PI;
}