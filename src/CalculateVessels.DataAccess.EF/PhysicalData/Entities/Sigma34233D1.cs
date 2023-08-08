namespace CalculateVessels.DataAccess.EF.PhysicalData.Entities;

public class Sigma34233D1
{
    public int SteelId { get; set; }
    public double T { get; set; }
    public double SigmaAllow { get; set; }
    public int? MinMaxThicknessId { get; set; }
    public MinMaxThickness? MinMaxThickness { get; set; }
    public int? DesignResourceId { get; set; }
    public DesignResource? DesignResource { get; set; }
}