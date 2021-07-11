using System.Collections.Generic;

namespace CalculateVessels.Data.PhysicalData.Gost34233_1
{
    public class SteelForSigma
	{
		public string Name { get; set; }
		public bool IsCouldBigThickness { get; set; }
		public double BigThickness { get; set; }

		public List<Sigma> Values { get; set; }
    }
}
