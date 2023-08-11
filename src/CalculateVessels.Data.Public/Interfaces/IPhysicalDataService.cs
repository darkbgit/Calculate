using CalculateVessels.Data.Public.Enums;
using CalculateVessels.Data.Public.Exceptions;
using CalculateVessels.Data.Public.Gost34233_4.Models;
using CalculateVessels.Data.Public.Gost6533.Models;
using CalculateVessels.Data.Public.Models;
using EllipticalBottom6533Type = CalculateVessels.Data.Public.Enums.EllipticalBottom6533Type;
using SteelType = CalculateVessels.Data.Public.Enums.SteelType;

namespace CalculateVessels.Data.Public.Interfaces;

public interface IPhysicalDataService
{
    /// <summary>
    /// Get sigma from
    /// </summary>
    /// <exception cref="PhysicalDataException"></exception>
    double GetSigma(string steelName, double temperature, SigmaSource source, double thickness = 0, DesignResourceType designResource = DesignResourceType.Standard);

    /// <exception cref="PhysicalDataException"></exception>
    Task<double> GetSigmaAsync(SteelWithIdsDto steel, double temperature, SigmaSource source);

    /// <exception cref="PhysicalDataException"></exception>
    double GetE(string steelName, double temperature, ESource source);

    /// <exception cref="PhysicalDataException"></exception>
    Task<double> GetEAsync(string steelName, double temperature, ESource source);

    /// <exception cref="PhysicalDataException"></exception>
    double GetAlpha(string steelName, double temperature, AlphaSource source);

    /// <exception cref="PhysicalDataException"></exception>
    double GetRm(string steelName, double temperature, RmSource source);

    /// <exception cref="PhysicalDataException"></exception>
    IEnumerable<SteelWithParametersDto> GetSteels(SteelSource source);

    /// <exception cref="PhysicalDataException"></exception>
    EllipsesParameters GetEllipsesParameters();

    /// <exception cref="PhysicalDataException"></exception>
    Task<IEnumerable<EllipticalBottom6533Type>> GetEllipsesTypes6533Async();

    /// <exception cref="PhysicalDataException"></exception>
    Task<IEnumerable<double>> GetEllipsesDiameters6533Async(EllipticalBottom6533Type type);

    /// <exception cref="PhysicalDataException"></exception>
    Task<IEnumerable<double>> GetEllipsesThickness6533Async(EllipticalBottom6533Type type, double diameter);

    /// <exception cref="PhysicalDataException"></exception>
    Task<EllipticalBottom6533Parameters?> GetEllipsesParameters6533Async(EllipticalBottom6533Type type, double diameter, double thickness);

    /// <exception cref="PhysicalDataException"></exception>
    SteelType Gost34233D1GetSteelType(string steelName);

    /// <exception cref="PhysicalDataException"></exception>
    double Gost34233D4Get_fb(int screwD, bool isScrewWithGroove);

    /// <exception cref="PhysicalDataException"></exception>
    Gasket Gost34233D4GetGasketParameters(string materialName);

    /// <exception cref="PhysicalDataException"></exception>
    IEnumerable<string> Gost34233D4GetGasketsList();

    /// <exception cref="PhysicalDataException"></exception>
    IEnumerable<string> Gost34233D4GetScrewDs();

    /// <exception cref="PhysicalDataException"></exception>
    (double phi1, double phi2, double phi3) Gost34233D7GetPhi1Phi2Phi3(double omega);

    /// <exception cref="PhysicalDataException"></exception>
    double Gost34233D7GetA(double omega, double mA);

    /// <exception cref="PhysicalDataException"></exception>
    double Gost34233D7GetB(double omega, double nB);

    /// <exception cref="PhysicalDataException"></exception>
    double Gost34233D7GetWd(double D);
}