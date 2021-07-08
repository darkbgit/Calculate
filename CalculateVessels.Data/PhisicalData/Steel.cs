using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateVessels.Data.PhisicalData
{
    public class Steel
	{
		public string Name { get; set; }
		public bool IsCouldBigThinckness { get; set; }
		public double BigThickness { get; set; }

		public List<Sigma> Value { get; set; }
    }
}
