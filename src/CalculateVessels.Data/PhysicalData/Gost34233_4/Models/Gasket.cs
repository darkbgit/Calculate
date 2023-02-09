namespace CalculateVessels.Data.PhysicalData.Gost34233_4.Models;

public class Gasket
{
    public string Material { get; set; } = string.Empty;
    public double m { get; set; }
    public double qobj { get; set; }
    public double q_d { get; set; }
    public double Kobj { get; set; }
    public double Ep { get; set; }
    public bool IsFlat { get; set; }
    public bool IsMetal { get; set; }
}