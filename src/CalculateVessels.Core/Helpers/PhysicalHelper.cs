using CalculateVessels.Core.Exceptions;
using CalculateVessels.Data.Enums;
using CalculateVessels.Data.Exceptions;
using CalculateVessels.Data.Interfaces;

namespace CalculateVessels.Core.Helpers;

internal static class PhysicalHelper
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

    public static double GetRm(string steel, double temperature, IPhysicalDataService service, RmSource source = RmSource.G34233D1)
    {
        try
        {
            return service.GetRm(steel, temperature, source);
        }
        catch (PhysicalDataException e)
        {
            throw new CalculateException($"Couldn't get alpha. {e.Message}", e);
        }
    }

    public static SteelType GetSteelType(string steelName, IPhysicalDataService service)
    {
        try
        {
            return service.Gost34233D1GetSteelType(steelName);
        }
        catch (PhysicalDataException e)
        {
            throw new CalculateException("Couldn't identify steel type.", e);
        }
    }

    public static (double phi1, double phi2, double phi3) GetPhi1Phi2Phi3(double omega, IPhysicalDataService service)
    {
        try
        {
            return service.Gost34233D7GetPhi1Phi2Phi3(omega);
        }
        catch (PhysicalDataException e)
        {
            throw new CalculateException("Couldn't get Phi1, Phi2, Phi3", e);
        }
    }

    public static double GetA(double omega, double mA, IPhysicalDataService service)
    {
        try
        {
            return service.Gost34233D7GetA(omega, mA);
        }
        catch (PhysicalDataException e)
        {
            throw new CalculateException("Couldn't get A.", e);
        }
    }

    public static double GetB(double omega, double nB, IPhysicalDataService service)
    {
        try
        {
            return service.Gost34233D7GetB(omega, nB);
        }
        catch (PhysicalDataException e)
        {
            throw new CalculateException("Couldn't get B.", e);
        }
    }

    public static double GetWd(double D, IPhysicalDataService service)
    {
        try
        {
            return service.Gost34233D7GetWd(D);
        }
        catch (PhysicalDataException e)
        {
            throw new CalculateException("Couldn't get [W].", e);
        }
    }
}