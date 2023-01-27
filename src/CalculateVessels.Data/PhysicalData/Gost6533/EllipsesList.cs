using System.Collections.Generic;

namespace CalculateVessels.Data.PhysicalData.Gost6533;

public class EllipsesList
{
    public Dictionary<double, List<SValue>> Ell025In { get; set; }
    public Dictionary<double, List<SValue>> Ell025Out { get; set; }
}