namespace CalculateVessels.Data.Public.Models;

public class SteelWithIdsDto
{
    public int SteelId { get; set; }
    public string SteelName { get; set; } = string.Empty;
    public int? MinMaxThicknessId { get; set; }
    public int? DesignResourceId { get; set; }
}
