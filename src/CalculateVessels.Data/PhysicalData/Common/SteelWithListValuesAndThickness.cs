using System.Collections.Generic;

namespace CalculateVessels.Data.PhysicalData.Common;

internal class SteelWithListValuesAndThickness
{
    public List<string> Name { get; set; }
    public Dictionary<double, List<double>> Values { get; set; }
    public bool IsCouldBigThickness { get; set; }
    public double BigThickness { get; set; }
}