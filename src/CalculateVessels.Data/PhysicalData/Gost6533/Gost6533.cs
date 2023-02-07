using CalculateVessels.Data.Exceptions;
using CalculateVessels.Data.PhysicalData.Gost6533.Models;
using CalculateVessels.Data.Utilities;
using System;
using System.IO;
using System.Text.Json;

namespace CalculateVessels.Data.PhysicalData.Gost6533;

internal class Gost6533
{
    private const string GostName = "ГОСТ 6533";
    private const string GostFolder = "Gost6533";
    private const string EllipticalBottomTable = "EllipticalBottom.json";

    public static EllipsesParameters GetEllipsesParameters()
    {
        const string fileName = $"{Constants.DataFolder}/{GostFolder}/Data/{EllipticalBottomTable}";

        try
        {
            using StreamReader file = new(fileName);
            var json = file.ReadToEnd();
            file.Close();
            return JsonSerializer.Deserialize<EllipsesParameters>(json) ?? throw new InvalidOperationException();
        }
        catch
        {
            throw new PhysicalDataException($"{GostName}. Couldn't open file {fileName} for ellipses diameters.");
        }
    }
}