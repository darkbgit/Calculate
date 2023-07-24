﻿using System;

namespace CalculateVessels.Core.Helpers;

internal static class MathHelper
{
    public static double DegreeToRadians(double degree) => degree * Math.PI / 180;

    public static double RadiansToDegree(double radian) => radian * 180 / Math.PI;
}