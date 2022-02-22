using System.Collections.Generic;

namespace CalculateVessels.Data.PhysicalData.Gost6533
{
    public class EllipsesList
    {
        //public List<EllipticalBottom> Ell025In { get; set; }
        //public List<EllipticalBottom> Ell025Out { get; set; }
        public Dictionary<double, List<SValue>> Ell025In { get; set; }
        public Dictionary<double, List<SValue>> Ell025Out { get; set; }
    }
}
