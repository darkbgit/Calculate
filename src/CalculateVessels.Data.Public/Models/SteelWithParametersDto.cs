using System.Text;
using CalculateVessels.DataAccess.EF.PhysicalData.Entities;

namespace CalculateVessels.Data.Public.Models;

public class SteelWithParametersDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? MinThickness { get; set; }
    public int? MaxThickness { get; set; }
    public DesignResource? DesignResource { get; set; }

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.Append(Name);
        if (MinThickness != null && MaxThickness != null)
        {
            builder.Append($"({MinThickness}-{MaxThickness})");
        }
        if (DesignResource != null)
        {
            builder.Append($" {DesignResource.Name}");
        }

        return builder.ToString();
    }
}