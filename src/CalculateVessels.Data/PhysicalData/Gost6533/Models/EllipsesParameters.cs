using System.Collections.Generic;

namespace CalculateVessels.Data.PhysicalData.Gost6533.Models;

public class EllipsesParameters
{
    public Dictionary<double, List<EllipseSValueParameters>> EllipticalBottomInsideDiameter025 { get; set; } = new Dictionary<double, List<EllipseSValueParameters>>();

    public Dictionary<double, List<EllipseSValueParameters>> EllipticalBottomInsideDiameter02 { get; set; } =
        new Dictionary<double, List<EllipseSValueParameters>>();
}