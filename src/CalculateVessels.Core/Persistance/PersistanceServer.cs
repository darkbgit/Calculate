using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Persistance.Enums;

namespace CalculateVessels.Core;

internal class PersistanceServer : IPersistanceService
{
    public IEnumerable<ICalculatedElement> Open(string filePath, SaveType saveType)
    {
        switch (saveType)
        {
            case SaveType.Json:
                return ReadFromJson(filePath);
            default:
                throw new PersistanceException($"{saveType} is not supported.");
        }
    }

    public void Save(IEnumerable<ICalculatedElement> calculatedElements, string filePath, SaveType saveType)
    {
        switch (saveType)
        {
            case SaveType.Json:
                WriteToJsonFile(filePath, calculatedElements);
                break;
            default:
                throw new PersistanceException($"{saveType} is not supported.");
        }
    }

    private static IEnumerable<ICalculatedElement> ReadFromJson(string filePath)
    {
        string json = string.Empty;

        try
        {
            using var reader = new StreamReader(filePath);
            json = reader.ReadToEnd();
        }
        catch (Exception ex)
        {
            throw new PersistanceException(ex.Message);
        }


        try
        {
            var result = JsonSerializer.Deserialize<List<ICalculatedElement>>(json,
                new JsonSerializerOptions { WriteIndented = true }) ??
                     throw new NullReferenceException();

            return result;
        }
        catch (Exception ex)
        {
            throw new PersistanceException(ex.Message);
        }
    }

    private static void WriteToJsonFile(string filePath, object objectToWrite, bool append = false)
    {
        var json = JsonSerializer.Serialize(objectToWrite, new JsonSerializerOptions { WriteIndented = true });

        try
        {
            using var writer = new StreamWriter(filePath, append, Encoding.UTF8, 10);
            writer.Write(Regex.Unescape(json));
        }
        catch (Exception ex)
        {
            throw new PersistanceException(ex.Message);
        }
    }
}
