using CalculateVessels.Core.Elements.Base;
using CalculateVessels.Core.Enums;
using CalculateVessels.Output.Word.Core;
using DocumentFormat.OpenXml.Wordprocessing;

namespace CalculateVessels.Output.Word.Helpers;

internal static class WordHelpers
{
    public static void CheckCalculatedPressure(double input, double calculated, Body body)
    {
        body.AddParagraph()
            .AppendEquation("[p]≥p");
        body.AddParagraph()
            .AppendEquation($"{calculated:f2}≥{input}");

        if (calculated >= input)
        {
            body.AddParagraph("Условие прочности выполняется")
                .Bold();
        }
        else
        {
            body.AddParagraph("Условие прочности не выполняется")
                .Bold()
                .Color(System.Drawing.Color.Red);
        }
    }



    /// <param name="name">String that pasting in a equation.</param>
    public static void CheckCalculatedThickness(string name, double input, double calculated, Body body)
    {
        if (input > calculated)
        {
            body.AddParagraph("Принятая толщина ")
                .Bold()
                .AppendEquation(name)
                .AddRun($"={input} мм");
        }
        else
        {
            body.AddParagraph("Принятая толщина ")
                .Bold()
                .Color(System.Drawing.Color.Red)
                .AppendEquation(name)
                .AddRun($"={input} мм");
        }
    }

    public static void AddLoadingConditionsInTableForShells(IEnumerable<LoadingCondition> loadingConditions, Table table)
    {
        loadingConditions
            .ToList()
            .ForEach(lc => AddLoadingConditionInputData(table, lc));
    }

    /// <summary>
    /// S
    /// </summary>
    /// <param name="results">IEnumerable of { Id, SigmaAllow, E }.</param>
    public static void AddMaterialCharacteristicsInTableForShell(string steel, IEnumerable<dynamic> results, IEnumerable<LoadingCondition> loadingConditions, Table table)
    {
        table.AddRowWithOneCell($"Характеристики материала {steel}");

        var loadingConditionsWithUniqTemperatures = loadingConditions
            .DistinctBy(lc => lc.t)
            .ToList();

        loadingConditionsWithUniqTemperatures
            .ForEach(lc => AddMaterialCharacteristic(table, lc, results
                .First(r => r.LoadingConditionId == lc.Id)));
    }

    public static string CheckFilePath(string filePath)
    {
        if (!string.IsNullOrWhiteSpace(filePath)) return filePath;

        const string defaultFileName = "temp.docx";
        return defaultFileName;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="table"></param>
    /// <param name="loadingCondition"></param>
    private static void AddLoadingConditionInputData(Table table, LoadingCondition loadingCondition)
    {
        table.AddRowWithOneCell($"Условия нагружения #{loadingCondition.Id}");

        table.AddRow()
            .AddCell("Расчетная температура, Т:")
            .AddCell($"{loadingCondition.t} °С");

        table.AddRow()
            .AddCell("Расчетное " + (loadingCondition.PressureType == PressureType.Inside ? "внутреннее избыточное" : "наружное")
                                  + " давление, p:")
            .AddCell($"{loadingCondition.p} МПа");
    }

    private static void AddMaterialCharacteristic(Table table, LoadingCondition loadingCondition, dynamic result)
    {
        //table.AddRowWithOneCell($"Характеристики материала для условий нагружения #{string.Join(", ", temperatureTuple.Item2)}");

        table.AddRow()
            .AddCell($"Допускаемое напряжение при расчетной температуре {loadingCondition.t} °С, [σ]:")
            .AddCell($"{result.SigmaAllow} МПа");

        table.AddRow()
            .AddCell($"Модуль продольной упругости при расчетной температуре {loadingCondition.t} °С, E:")
            .AddCell($"{result.E} МПа");
    }

    private static void AddMaterialCharacteristic(Table table, LoadingCondition loadingCondition, double sigmaAllow, double EAllow)
    {
        //table.AddRowWithOneCell($"Характеристики материала для условий нагружения #{string.Join(", ", temperatureTuple.Item2)}");

        table.AddRow()
            .AddCell($"Допускаемое напряжение при расчетной температуре {loadingCondition.t} °С, [σ]:")
            .AddCell($"{sigmaAllow} МПа");

        table.AddRow()
            .AddCell($"Модуль продольной упругости при расчетной температуре {loadingCondition.t} °С, E:")
            .AddCell($"{EAllow} МПа");
    }
}