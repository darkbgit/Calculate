using System;
using System.Linq;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Interfaces;

namespace CalculateVessels.Core.ColumnSkirt;

internal class ColumnSkirtCalculateService : ICalculateService<ColumnSkirtInput>
{
    private readonly IPhysicalDataService _physicalData;

    public ColumnSkirtCalculateService(IPhysicalDataService physicalData)
    {
        _physicalData = physicalData;
        Name = "GOST 34233.9-2017";
    }

    public string Name { get; }

    public ICalculatedElement Calculate(ColumnSkirtInput dataIn)
    {

        var data = new ColumnSkirtCalculated
        {
            InputData = dataIn
        };

        //    if (dataIn.SigmaAllow == 0)
        //    {
        //        try
        //        {
        //            data.SigmaAllow = Physical.Gost34233_1.GetSigma(dataIn.Steel, dataIn.t);
        //        }
        //        catch (PhysicalDataException e)
        //        {
        //            throw new CalculateException("Error get sigma.", e);
        //        }
        //    }
        //    else
        //    {
        //        data.SigmaAllow = dataIn.SigmaAllow;
        //    }

        //    if (dataIn.E == 0)
        //    {
        //        try
        //        {
        //            data.E = Physical.GetE(dataIn.Steel, dataIn.t);
        //        }
        //        catch (PhysicalDataException e)
        //        {
        //            throw new CalculateException("Error get E.", e);
        //        }
        //    }
        //    else
        //    {
        //        data.E = dataIn.E;
        //    }

        //    data.WeldWithBottomStrengthCondition = (4 * data.M_d / dataIn.D0 + data.F) / (Math.PI * dataIn.D0 * dataIn.a);

        //    data.IsWeldWithBottomStrengthCondition = data.WeldWithBottomStrengthCondition <= Math.Min(data.SigmaAlloy0, data.SigmaAlloy);

        //    data.AreaOfHoleStrengthCondition =

        //        data.c = dataIn.c1 + dataIn.c2 + dataIn.c3;

        return data;
    }

    private static double sigma_x1(double p, double D, double s, double c, double F, double M) =>
        p * (D + s) / (4 * (s - c)) - F / (Math.PI * D * (s - c)) + 4 * M / (Math.PI * Math.Pow(D, 2) * (s - c));

    private static double sigma_x2(double p, double D, double s, double c, double F, double M) =>
        p * (D + s) / (4 * (s - c)) - F / (Math.PI * D * (s - c)) - 4 * M / (Math.PI * Math.Pow(D, 2) * (s - c));

    private static double sigma_y(double p, double D, double s, double c) =>
        p * (D + s) / (2 * (s - c));

    private static double sigma_E1(double sigma_x1, double sigma_y) =>
        new[] { Math.Abs(sigma_x1 - sigma_y), Math.Abs(sigma_y), Math.Abs(sigma_x1) }.Max();

    private static double sigma_E2(double sigma_x2, double sigma_y) =>
        new[] { Math.Abs(sigma_x2 - sigma_y), Math.Abs(sigma_y), Math.Abs(sigma_x2) }.Max();
}