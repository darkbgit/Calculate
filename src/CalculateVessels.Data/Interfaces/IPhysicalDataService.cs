using CalculateVessels.Data.Enums;
using CalculateVessels.Data.Exceptions;
using CalculateVessels.Data.PhysicalData.Gost34233_4.Models;
using CalculateVessels.Data.PhysicalData.Gost6533.Models;
using System.Collections.Generic;

namespace CalculateVessels.Data.Interfaces;

public interface IPhysicalDataService
{
    /// <summary>
    /// Get sigma from
    /// </summary>
    /// <exception cref="PhysicalDataException"></exception>
    double GetSigma(string steelName, double temperature, SigmaSource source);

    /// <exception cref="PhysicalDataException"></exception>
    double GetSigma(string steelName, double temperature, double s = 0, int N = 1000);

    /// <exception cref="PhysicalDataException"></exception>
    double GetE(string steelName, double temperature, ESource source);

    /// <exception cref="PhysicalDataException"></exception>
    double GetAlpha(string steelName, double temperature, AlphaSource source);

    /// <exception cref="PhysicalDataException"></exception>
    IEnumerable<string> GetSteels(SteelSource source);

    /// <exception cref="PhysicalDataException"></exception>
    EllipsesParameters GetEllipsesParameters();

    /// <exception cref="PhysicalDataException"></exception>
    double Gost34233D4Get_fb(int screwD, bool isScrewWithGroove);

    /// <exception cref="PhysicalDataException"></exception>
    Gasket Gost34233D4GetGasketParameters(string materialName);

    /// <exception cref="PhysicalDataException"></exception>
    IEnumerable<string> Gost34233D4GetGasketsList();

    /// <exception cref="PhysicalDataException"></exception>
    IEnumerable<string> Gost34233D4GetScrewDs();

}