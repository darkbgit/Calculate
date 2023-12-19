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

internal class PhysicalDataService(CalculateVesselsPhysicalDataContext context) : IPhysicalDataService
{
    private readonly CalculateVesselsPhysicalDataContext _context = context;

    private const int MinTemperature = 20;

    public double GetSigma(string steelName, double temperature, SigmaSource source, double thickness = 0, DesignResourceType designResource = DesignResourceType.Standard)
    {
        switch (source)
        {
            case SigmaSource.G34233D1:

                var query = from sigmaTable in _context.Sigmas34233D1
                         .Where(s => s.SteelId == _context.Steels.First(st => st.Name == steelName).Id)
                            group sigmaTable by new
                            {
                                sigmaTable.SteelId,
                                sigmaTable.MinMaxThicknessId,
                                sigmaTable.DesignResourceId,
                                sigmaTable.T,
                                sigmaTable.SigmaAllow
                            }
                     into sigmaGroup
                            select new
                            {
                                sigmaGroup.Key.SteelId,
                                sigmaGroup.Key.MinMaxThicknessId,
                                sigmaGroup.Key.DesignResourceId,
                                sigmaGroup.Key.T,
                                sigmaGroup.Key.SigmaAllow
                            }
                     into sigmaGroup
                            join steel in _context.Steels on sigmaGroup.SteelId equals steel.Id
                            join dr in _context.DesignResources on sigmaGroup.DesignResourceId.Value equals dr.Id into drList
                            from dr in drList.DefaultIfEmpty()
                            join t in _context.SteelsMinMaxThickness on sigmaGroup.MinMaxThicknessId.Value equals t.Id into tList
                            from t in tList.DefaultIfEmpty()

                            where t == null || (t.Min <= thickness && t.Max > thickness)
                            where dr == null || dr.Id == (int)designResource
                            select new TemperatureWithValue
                            {
                                Temperature = sigmaGroup.T,
                                Value = sigmaGroup.SigmaAllow
                            };

                var temperaturesWithSigmas = query.ToList();

                if (!temperaturesWithSigmas.Any())
                {
                    throw new PhysicalDataException($"Sigma parameters for steel - {steelName}, source - {source}, temperature - {temperature}, thickness - {thickness}, design resource - {designResource} not found.");
                }

                if (temperature > temperaturesWithSigmas.Max(s => s.Temperature))
                {
                    throw new PhysicalDataException($"{source}. Температура {temperature} °С, больше чем максимальная температура {temperaturesWithSigmas.Max(s => s.Temperature)} °С " +
                            $"для стали {steelName} при которой определяется допускаемое напряжение.");
                }

                if (temperature <= MinTemperature)
                {
                    return temperaturesWithSigmas
                        .First(sigma => Math.Abs(sigma.Temperature - temperaturesWithSigmas.Min(s => s.Temperature)) < 0.00001)
                        .Value;
                }

                if (temperaturesWithSigmas.Any(sigma => Math.Abs(sigma.Temperature - temperature) < 0.00001))
                {
                    return temperaturesWithSigmas
                        .First(sigma => Math.Abs(sigma.Temperature - temperature) < 0.00001)
                        .Value;
                }


                var lowerSigma = temperaturesWithSigmas
                    .Where(s => s.Temperature < temperature)
                    .MaxBy(s => s.Temperature) ??
                    throw new NullReferenceException();

                var biggerSigma = temperaturesWithSigmas
                    .Where(s => s.Temperature > temperature)
                    .MinBy(s => s.Temperature) ??
                    throw new NullReferenceException();

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

    public async Task<double> GetSigmaAsync(SteelWithIdsDto steel, double temperature, SigmaSource source)
    {
        switch (source)
        {
            case SigmaSource.G34233D1:
                var query = from sigmaTable in _context.Sigmas34233D1
                         .Where(s => s.SteelId == steel.SteelId)
                                //.Where(s => (s.DesignResourceId == null && steel.DesignResourceId == null) || s.DesignResourceId == steel.DesignResourceId)
                                //.Where(s => s.MinMaxThicknessId == steel.MinMaxThicknessId)
                                //&& s.DesignResourceId == steel.DesignResourceId
                                //&& s.MinMaxThicknessId == steel.MinMaxThicknessId)
                            group sigmaTable by new
                            {
                                sigmaTable.SteelId,
                                sigmaTable.MinMaxThicknessId,
                                sigmaTable.DesignResourceId,
                                sigmaTable.T,
                                sigmaTable.SigmaAllow
                            }
                     into sigmaGroup
                            select new
                            {
                                sigmaGroup.Key.SteelId,
                                sigmaGroup.Key.MinMaxThicknessId,
                                sigmaGroup.Key.DesignResourceId,
                                sigmaGroup.Key.T,
                                sigmaGroup.Key.SigmaAllow
                            }
                     into sigmaGroup
                            //join steel in _context.Steels on sigmaGroup.SteelId equals steel.Id
                            join dr in _context.DesignResources on sigmaGroup.DesignResourceId.Value equals dr.Id into drList
                            from dr in drList.DefaultIfEmpty()
                            join t in _context.SteelsMinMaxThickness on sigmaGroup.MinMaxThicknessId.Value equals t.Id into tList
                            from t in tList.DefaultIfEmpty()

                            where dr == null || dr.Id == steel.DesignResourceId
                            where t == null || t.Id == steel.MinMaxThicknessId

                            select new TemperatureWithValue
                            {
                                Temperature = sigmaGroup.T,
                                Value = sigmaGroup.SigmaAllow
                            };

                var temperaturesWithSigmas = await query.ToListAsync();

                if (!temperaturesWithSigmas.Any())
                {
                    throw new PhysicalDataException($"Sigma parameters for steel - {steel.SteelName}, source - {source}, temperature - {temperature}, minMaxThicknessId - {steel.MinMaxThicknessId}, designResourceId - {steel.DesignResourceId} not found.");
                }

                if (temperature > temperaturesWithSigmas.Max(s => s.Temperature))
                {
                    throw new PhysicalDataException($"{source}. Температура {temperature} °С, больше чем максимальная температура {temperaturesWithSigmas.Max(s => s.Temperature)} °С " +
                            $"для стали {steel.SteelName} при которой определяется допускаемое напряжение.");
                }

                if (temperature <= MinTemperature)
                {
                    return temperaturesWithSigmas
                        .First(sigma => Math.Abs(sigma.Temperature - temperaturesWithSigmas.Min(s => s.Temperature)) < 0.00001)
                        .Value;
                }

                if (temperaturesWithSigmas.Any(sigma => Math.Abs(sigma.Temperature - temperature) < 0.00001))
                {
                    return temperaturesWithSigmas
                        .First(sigma => Math.Abs(sigma.Temperature - temperature) < 0.00001)
                        .Value;
                }

                var lowerSigma = temperaturesWithSigmas
                    .Where(s => s.Temperature < temperature)
                    .MaxBy(s => s.Temperature) ??
                    throw new NullReferenceException();

                var biggerSigma = temperaturesWithSigmas
                    .Where(s => s.Temperature > temperature)
                    .MinBy(s => s.Temperature) ??
                    throw new NullReferenceException();

                return Interpolations.Interpolate(lowerSigma, biggerSigma, temperature, RoundType.WithAccuracy05);

            case SigmaSource.G34233D4:
                throw new NotImplementedException();
            default:
                throw new ArgumentOutOfRangeException(nameof(source), source, null);
        }
    }

    public async Task<double> GetEAsync(string steelName, double temperature, ESource source)
    {
        switch (source)
        {
            case ESource.G34233D1:
                var query = from ETable in _context.E34233D1
                        .Where(e => e.SteelTypeId == _context.Steels.First(st => st.Name == steelName).SteelTypeId)
                            select new TemperatureWithValue
                            {
                                Temperature = ETable.T,
                                Value = ETable.E
                            };

                var temperaturesWithEs = await query.ToListAsync();

                if (!temperaturesWithEs.Any())
                {
                    throw new PhysicalDataException($"E for steel - {steelName}, source - {source}, temperature - {temperature} not found.");
                }

                if (temperature > temperaturesWithEs.Max(e => e.Temperature))
                {
                    throw new PhysicalDataException($"{source}. Температура {temperature} °С, больше чем максимальная температура {temperaturesWithEs.Max(s => s.Temperature)} °С " +
                            $"для стали {steelName} при которой определяется модуль продольной упругости.");
                }

                if (temperature <= MinTemperature)
                {
                    return temperaturesWithEs
                        .First(sigma => Math.Abs(sigma.Temperature - temperaturesWithEs.Min(s => s.Temperature)) < 0.00001)
                        .Value;
                }

                if (temperaturesWithEs.Any(sigma => Math.Abs(sigma.Temperature - temperature) < 0.00001))
                {
                    return temperaturesWithEs
                        .First(sigma => Math.Abs(sigma.Temperature - temperature) < 0.00001)
                        .Value;
                }


                var lower = temperaturesWithEs
                    .Where(s => s.Temperature < temperature)
                    .MaxBy(s => s.Temperature) ??
                    throw new NullReferenceException();

                var bigger = temperaturesWithEs
                    .Where(s => s.Temperature > temperature)
                    .MinBy(s => s.Temperature) ??
                    throw new NullReferenceException();

                return Interpolations.Interpolate(lower, bigger, temperature, RoundType.Integer);

            //var es = await _context.Steels
            //    .FirstOrDefaultAsync(s => s.Name == steelName);


            //var fileName = Path.Combine(Constants.DataFolder, GostFolder, TableE);

            //List<SteelWithValues>? steels;

            //try
            //{
            //    using StreamReader file = new(fileName);
            //    var json = file.ReadToEnd();
            //    file.Close();
            //    steels = JsonSerializer.Deserialize<List<SteelWithValues>>(json);
            //}
            //catch
            //{
            //    throw new PhysicalDataException($"{GostName}. Couldn't open file {fileName} for E.");
            //}

            //var EList = steels?.FirstOrDefault(s => s.Name.Contains(steelName))
            //            ?? throw new PhysicalDataException($"{GostName}. Steel \"{steelName}\" wasn't found.");

            //try
            //{
            //    return Interpolations.InterpolationForParameters(EList.Values, temperature, RoundType.Integer);
            //}
            //catch (PhysicalDataException ex)
            //{
            //    if (ex.MaxTemperatureError)
            //    {
            //        throw new PhysicalDataException(
            //            $"{GostName}. Температура {temperature} °С, больше чем максимальная температура {ex.MaxTemperature} °С " +
            //            $"для стали {steelName} при которой определяется модуль продольной упругости.");
            //    }

            //    throw;
            //}
            //break;
            case ESource.G34233D4:
                throw new NotImplementedException();
                break;
            default:
                throw new NotImplementedException();
                break;
        }
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
                                MinMaxThickness = t,
                                DesignResource = (DesignResourceType)dr.Id
                            };

                var steels = query.ToList();

                return steels;
            case SteelSource.G34233D4Washer:
                throw new NotImplementedException();
            case SteelSource.G34233D4Screw:
                throw new NotImplementedException();
            default:
                throw new ArgumentOutOfRangeException(nameof(source), source, null);
        }
    }

    public EllipsesParameters GetEllipsesParameters()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<EllipticalBottom6533Type>> GetEllipsesTypes6533Async()
    {
        return await _context.EllipticalBottom6533
            .Select(t => (EllipticalBottom6533Type)t.EllipticalBottomTypeId)
            .Distinct()
            .ToListAsync();
    }

    public async Task<IEnumerable<double>> GetEllipsesDiameters6533Async(EllipticalBottom6533Type type)
    {
        return await _context.EllipticalBottom6533
            .Where(el => el.EllipticalBottomTypeId == (int)type)
            .Select(el => el.D)
            .Distinct()
            .ToListAsync();
    }

    public async Task<IEnumerable<double>> GetEllipsesThickness6533Async(EllipticalBottom6533Type type, double diameter)
    {
        return await _context.EllipticalBottom6533
            .Where(el => el.EllipticalBottomTypeId == (int)type && Math.Abs(el.D - diameter) < 0.00001)
            .Select(el => el.s)
            .ToListAsync();
    }

    public async Task<EllipticalBottom6533Parameters?> GetEllipsesParameters6533Async(EllipticalBottom6533Type type, double diameter, double thickness)
    {
        return await _context.EllipticalBottom6533
            .Where(el => el.EllipticalBottomTypeId == (int)type && Math.Abs(el.D - diameter) < 0.00001 && Math.Abs(el.s - thickness) < 0.00001)
            .Select(el => new EllipticalBottom6533Parameters
            {
                H = el.H,
                h1 = el.h1
            })
            .FirstOrDefaultAsync();
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

    double IPhysicalDataService.GetE(string steelName, double temperature, ESource source)
    {
        throw new NotImplementedException();
    }

    SteelType IPhysicalDataService.Gost34233D1GetSteelType(string steelName)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<EllipticalBottom6533>> GetEllipses6533Async()
    {
        return await _context.EllipticalBottom6533
            .Select(e => new EllipticalBottom6533
            {
                Type = (EllipticalBottom6533Type)e.EllipticalBottomTypeId,
                D = e.D,
                h1 = e.h1,
                H = e.H,
                s = e.s,
                F = e.F,
                V = e.V,
                m = e.m
            })
            .ToListAsync();
    }
}