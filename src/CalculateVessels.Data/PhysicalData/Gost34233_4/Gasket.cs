using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateVessels.Data.PhysicalData.Gost34233_4;

public class Gasket
{
    public string Material { get; set; }
    public double m { get; set; }
    public double qobj { get; set; }
    public double q_d { get; set; }
    public double Kobj { get; set; }
    public double Ep { get; set; }
    public bool IsFlat { get; set; }
    public bool IsMetal { get; set; }
}