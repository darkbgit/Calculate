using CalculateVessels.Core.Exceptions;
using CalculateVessels.Data.Enums;
using CalculateVessels.Data.Exceptions;
using CalculateVessels.Data.Interfaces;

namespace CalculateVessels.Core.Helpers;

internal class PhysicalHelper
{
    public static double GetSigma(string steel, double temperature, IPhysicalDataService service, SigmaSource source = SigmaSource.G34233D1)
    {
        try
        {
            return service.GetSigma(steel, temperature, source);
        }
        catch (PhysicalDataException e)
        {
            throw new CalculateException($"Couldn't get sigma. {e.Message}", e);
        }
    }

    public static double GetSigmaIfZero(double sigmaAllow, string steel, double temperature, IPhysicalDataService service, SigmaSource source = SigmaSource.G34233D1)
    {
        return sigmaAllow != 0 ? sigmaAllow : GetSigma(steel, temperature, service, source);
    }

    public static double GetE(string steel, double temperature, IPhysicalDataService service, ESource source = ESource.G34233D1)
    {
        try
        {
            return service.GetE(steel, temperature, source);
        }
        catch (PhysicalDataException e)
        {
            throw new CalculateException($"Couldn't get E. {e.Message}", e);
        }
    }

    public static double GetEIfZero(double E, string steel, double temperature, IPhysicalDataService service, ESource source = ESource.G34233D1)
    {
        return E != 0 ? E : GetE(steel, temperature, service, source);
    }

    public static double GetAlpha(string steel, double temperature, IPhysicalDataService service, AlphaSource source = AlphaSource.G34233D1)
    {
        try
        {
            return service.GetAlpha(steel, temperature, source);
        }
        catch (PhysicalDataException e)
        {
            throw new CalculateException($"Couldn't get alpha. {e.Message}", e);
        }
    }
}