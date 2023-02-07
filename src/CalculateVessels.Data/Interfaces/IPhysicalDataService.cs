using CalculateVessels.Data.Enums;
using CalculateVessels.Data.PhysicalData.Gost6533.Models;
using System.Collections.Generic;

namespace CalculateVessels.Data.Interfaces;

public interface IPhysicalDataService
{
    double GetSigma(string steelName, double temperature, SigmaSource source);
    double GetSigma(string steelName, double temperature, double s = 0, int N = 1000);

    double GetE(string steelName, double temperature, ESource source);

    IEnumerable<string> GetSteels(SteelSource source);

    EllipsesParameters GetEllipsesParameters();
}