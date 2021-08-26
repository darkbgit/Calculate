using System.Collections.Generic;

namespace CalculateVessels.Data.PhysicalData.Gost34233_1
{
    public class SteelForRm
	{
		public List<string> Name { get; set; }
        public bool IsCouldBigThickness { get; set; }
        public double BigThickness { get; set; }
		public List<Rm> Values { get; set; }
    }
}
