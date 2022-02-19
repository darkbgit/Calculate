using System.Collections.Generic;

namespace CalculateVessels.Data.PhysicalData.Common.Base
{
    internal class PhysicalParameterWithListValues
    {
        public double Temperature { get; set; }
        public List<double> Value { get; set; }
    }
}