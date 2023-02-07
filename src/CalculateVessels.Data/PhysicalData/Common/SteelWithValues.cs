using System.Collections.Generic;

namespace CalculateVessels.Data.PhysicalData.Common;

internal class SteelWithValues
{
    public List<string> Name { get; set; } = new List<string>();
    public Dictionary<double, double> Values { get; set; } = new Dictionary<double, double>();
}