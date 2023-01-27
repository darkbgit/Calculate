using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalculateVessels.Data.PhysicalData.Common.Base;

namespace CalculateVessels.Data.PhysicalData.Common;

internal class SteelWithValues
{
    public List<string> Name { get; set; }

    //public List<T> Values { get; set; }
    public Dictionary<double, double> Values { get; set; }
}