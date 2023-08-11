namespace CalculateVessels.DataAccess.EF.PhysicalData.Entities;

public class DesignResource
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int MaxWorkingHours { get; set; }

    public override string ToString() => Name;
}