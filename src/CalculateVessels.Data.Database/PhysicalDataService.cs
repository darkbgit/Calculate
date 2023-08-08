using CalculateVessels.Data.Database.Utilities;
using CalculateVessels.Data.Public.Enums;
using CalculateVessels.Data.Public.Exceptions;
using CalculateVessels.Data.Public.Gost34233_4.Models;
using CalculateVessels.Data.Public.Gost6533.Models;
using CalculateVessels.Data.Public.Interfaces;
using CalculateVessels.Data.Public.Models;
using CalculateVessels.DataAccess.EF.PhysicalData;
using Microsoft.EntityFrameworkCore;

namespace CalculateVessels.Data.Database;

internal class PhysicalDataService : IPhysicalDataService
{
    private readonly CalculateVesselsPhysicalDataContext _context;

    private const int MinTemperature = 20;

    public PhysicalDataService(CalculateVesselsPhysicalDataContext context)
    {
        _context = context;
    }

    public double GetSigma(string steelName, double temperature, SigmaSource source, double thickness = 0, DesignResourceType? designResource = null)
    {
        switch (source)
        {
            case SigmaSource.G34233D1:
                var steel = _context.Steels
                    .Where(steel => steel.Name == steelName)
                    .Include(s => s.Sigma34233D1)
                    .ThenInclude(sigma => sigma.MinMaxThickness)
                    .FirstOrDefault()
                            ?? throw new PhysicalDataException($"Steel {steelName} not found.");

                if (temperature > steel.Sigma34233D1.Max(s => s.T))
                {
                    throw new PhysicalDataException($"{source}. Температура {temperature} °С, больше чем максимальная температура {steel.Sigma34233D1.Max(s => s.T)} °С " +
                            $"для стали {steelName} при которой определяется допускаемое напряжение.");
                }

                int? designResourceType = designResource == null ? null : (int)designResource;

                var sigmas = steel.Sigma34233D1
                    .Where(sigma => sigma.DesignResourceId == designResourceType &&
                                    ((sigma.MinMaxThickness?.Min <= thickness && sigma.MinMaxThickness.Max > thickness) || (sigma.MinMaxThickness == null && thickness == 0)))
                    .ToList();

                if (!sigmas.Any())
                {
                    throw new PhysicalDataException($"Sigma parameters for steel - {steelName}, source - {source}, temperature - {temperature}, thickness - {thickness}, design resource - {designResource} not found.");
                }

                if (temperature <= MinTemperature)
                {
                    return sigmas
                        .First(sigma => Math.Abs(sigma.T - steel.Sigma34233D1.Min(s => s.T)) < 0.00001)
                        .SigmaAllow;
                }

                var sigma = sigmas
                    .FirstOrDefault(sigma => Math.Abs(sigma.T - temperature) < 0.00001);

                if (sigma != null)
                {
                    return sigma.SigmaAllow;
                }

                var lowerSigma = sigmas
                    .Where(s => s.T < temperature)
                    .MaxBy(s => s.T);

                var biggerSigma = sigmas
                    .Where(s => s.T > temperature)
                    .MinBy(s => s.T);

                return Interpolations.Interpolate(lowerSigma, biggerSigma, temperature, RoundType.WithAccuracy05);

            case SigmaSource.G34233D4:
                throw new NotImplementedException();
            default:
                throw new ArgumentOutOfRangeException(nameof(source), source, null);
        }
    }

    public double GetSigma(string steelName, double temperature, double s = 0, int N = 1000)
    {
        throw new NotImplementedException();
    }

    public double GetE(string steelName, double temperature, ESource source)
    {
        throw new NotImplementedException();
    }

    public double GetAlpha(string steelName, double temperature, AlphaSource source)
    {
        throw new NotImplementedException();
    }

    public double GetRm(string steelName, double temperature, RmSource source)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<SteelWithParametersDto> GetSteels(SteelSource source)
    {
        switch (source)
        {
            case SteelSource.G34233D1:
                //var steels = _context.Steels
                //    .Include(s => s.Sigma34233D1)
                //    .ThenInclude(si => si.DesignResource)
                //    .SelectMany(s => s.Sigma34233D1
                //            .GroupBy(si => new { s.Name, si.MinThickness, si.MaxThickness, si.DesignResource })
                //            .Select(g => new SteelWithParametersDto
                //            {
                //                Id = s.Id,
                //                Name = s.Name,
                //                MinThickness = g.Key.MinThickness,
                //                MaxThickness = g.Key.MaxThickness,
                //                DesignResource = g.Key.DesignResource
                //            })
                //            .ToList())
                //    .ToList();

                //var query1 = from sigma in _context.Sigmas34233D1.DefaultIfEmpty()
                //             group sigma by new
                //             {
                //                 sigma.SteelId,
                //                 sigma.MinMaxThicknessId,
                //                 sigma.DesignResourceId,
                //             }
                //into sigmaGroup
                //             select new
                //             {
                //                 sigmaGroup.Key.SteelId,
                //                 sigmaGroup.Key.MinMaxThicknessId,
                //                 sigmaGroup.Key.DesignResourceId
                //             } into sigmaGroup
                //             select sigmaGroup;

                //var q = query1.ToList();

                var query = from sigma in _context.Sigmas34233D1.DefaultIfEmpty()
                            group sigma by new
                            {
                                sigma.SteelId,
                                sigma.MinMaxThicknessId,
                                sigma.DesignResourceId
                            }
                    into sigmaGroup

                            select new
                            {
                                sigmaGroup.Key.SteelId,
                                sigmaGroup.Key.MinMaxThicknessId,
                                sigmaGroup.Key.DesignResourceId
                            }
                    into sigmaGroup
                            join steel in _context.Steels on sigmaGroup.SteelId equals steel.Id
                            join dr in _context.DesignResources on sigmaGroup.DesignResourceId.Value equals dr.Id into drList
                            from dr in drList.DefaultIfEmpty()
                            join t in _context.SteelsMinMaxThickness on sigmaGroup.MinMaxThicknessId.Value equals t.Id into tList
                            from t in tList.DefaultIfEmpty()

                            select new SteelWithParametersDto
                            {
                                Id = steel.Id,
                                Name = steel.Name,
                                MinThickness = t.Min,
                                MaxThickness = t.Max,
                                DesignResource = dr
                            };

                var steels = query.ToList();

                return steels;
            case SteelSource.G34233D4Washer:
                throw new NotImplementedException();
                break;
            case SteelSource.G34233D4Screw:
                throw new NotImplementedException();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(source), source, null);
        }
    }

    public EllipsesParameters GetEllipsesParameters()
    {
        throw new NotImplementedException();
    }

    public SteelType Gost34233D1GetSteelType(string steelName)
    {
        throw new NotImplementedException();
    }

    public double Gost34233D4Get_fb(int screwD, bool isScrewWithGroove)
    {
        throw new NotImplementedException();
    }

    public Gasket Gost34233D4GetGasketParameters(string materialName)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<string> Gost34233D4GetGasketsList()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<string> Gost34233D4GetScrewDs()
    {
        throw new NotImplementedException();
    }

    public (double phi1, double phi2, double phi3) Gost34233D7GetPhi1Phi2Phi3(double omega)
    {
        throw new NotImplementedException();
    }

    public double Gost34233D7GetA(double omega, double mA)
    {
        throw new NotImplementedException();
    }

    public double Gost34233D7GetB(double omega, double nB)
    {
        throw new NotImplementedException();
    }

    public double Gost34233D7GetWd(double D)
    {
        throw new NotImplementedException();
    }
}