namespace CalculateVessels.DataAccess.EF.PhysicalData.Entities;

public class MinMaxThickness
{
    public int Id { get; set; }
    public int Min { get; set; }
    public int Max { get; set; }

    public override string ToString() => $"{Min} - {Max}";
}