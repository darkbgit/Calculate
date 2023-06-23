using System;
using CalculateVessels.Core.Elements.Base;

namespace CalculateVessels.Core.Exceptions;

public class CalculateException : Exception
{
    public CalculateException()
    {

    }

    public CalculateException(string massage)
        : base(massage)
    {

    }

    public CalculateException(string massage, LoadingCondition loadingCondition)
        : base(EnrichWithLoadingConditions(loadingCondition) + massage)
    {

    }

    public CalculateException(string message, Exception innerException)
        : base(message, innerException)
    {

    }

    private static string EnrichWithLoadingConditions(LoadingCondition loadingCondition) =>
        (loadingCondition.IsPressureIn ? "inside " : "outside ") + loadingCondition.p + " MPa " + loadingCondition.t + " C. ";

}