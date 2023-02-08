using CalculateVessels.Core.Exceptions;
using CalculateVessels.Data.Enums;
using CalculateVessels.Data.Exceptions;
using CalculateVessels.Data.Interfaces;

namespace CalculateVessels.Core.Helpers;

internal class PhysicalHelper
{
    public static double GetSigmaIfZero(double sigmaAllow, string steel, double temperature, IPhysicalDataService service, SigmaSource source = SigmaSource.G34233D1)
    {
        if (sigmaAllow != 0) return sigmaAllow;

        double result;

        try
        {
            result = service.GetSigma(steel, temperature, source);
        }
        catch (PhysicalDataException e)
        {
            throw new CalculateException($"Error get sigma. {e.Message}", e);
        }

        return result;
    }

    public static double GetEIfZero(double E, string steel, double temperature, IPhysicalDataService service, ESource source = ESource.G34233D1)
    {
        if (E != 0) return E;

        double result;

        try
        {
            result = service.GetE(steel, temperature, source);
        }
        catch (PhysicalDataException e)
        {
            throw new CalculateException($"Error get E. {e.Message}", e);
        }

        return result;
    }
}