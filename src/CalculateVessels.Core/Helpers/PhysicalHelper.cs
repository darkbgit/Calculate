using CalculateVessels.Core.Exceptions;
using CalculateVessels.Data.Exceptions;
using CalculateVessels.Data.PhysicalData;
using CalculateVessels.Data.PhysicalData.Gost34233_1;

namespace CalculateVessels.Core.Helpers;

internal static class PhysicalHelper
{
    public static double GetSigmaIfZero(double sigmaAllow, string steel, double temperature)
    {
        if (sigmaAllow != 0) return sigmaAllow;

        double result;

        try
        {
            result = Gost34233_1.GetSigma(steel, temperature);
        }
        catch (PhysicalDataException e)
        {
            throw new CalculateException("Error get sigma.", e);
        }

        return result;
    }

    public static double GetEIfZero(double E, string steel, double temperature)
    {
        if (E != 0) return E;

        double result;

        try
        {
            result = Physical.GetE(steel, temperature);
        }
        catch (PhysicalDataException e)
        {
            throw new CalculateException("Error get sigma.", e);
        }

        return result;
    }
}