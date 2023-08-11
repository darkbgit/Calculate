using System.Text;
using CalculateVessels.Data.Public.Enums;
using CalculateVessels.DataAccess.EF.PhysicalData.Entities;

namespace CalculateVessels.Data.Public.Models;

public class SteelWithParametersDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public MinMaxThickness? MinMaxThickness { get; set; }
    public DesignResourceType? DesignResource { get; set; }

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.Append(Name);
        if (MinMaxThickness != null)
        {
            builder.Append($"({MinMaxThickness.Min}-{MinMaxThickness.Max})");
        }
        if (DesignResource != null)
        {
            builder.Append($" {DesignResource}");
        }

        return builder.ToString();
    }
}