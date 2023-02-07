using System.Collections.Generic;

namespace CalculateVessels.Data.PhysicalData.Gost34233_1.Models;

public class SteelWithNameAndSteelType
{
    public List<string> Name { get; set; } = new List<string>();
    public int SteelType { get; set; }
}