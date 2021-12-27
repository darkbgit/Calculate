using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Supports.Enums;
using System.Collections.Generic;
using System.Linq;

namespace CalculateVessels.Core.Supports.BracketVertical
{
    public class BracketVerticalInputData : IInputData
    {
        public IEnumerable<string> ErrorList => _errorList;

        private List<string> _errorList = new();

        public bool IsAssembly { get; set; }
        public bool IsPressureIn { get; set; }
        public double a { get; set; }
        public double Ak { get; set; }
        public double b { get; set; }
        public double b2 { get; set; }
        public double c { get; set; }
        public double D { get; set; }
        public double delta1 { get; set; }
        public double delta2 { get; set; }
        public double e { get; set; }
        public double fi { get; set; }
        public double G { get; set; }
        public double H { get; set; }
        public double L { get; set; }
        public double p { get; set; }
        public double s { get; set; }
        public double s2 { get; set; }
        public string Name { get; set; }
        public string NameShell { get; set; }
        public string Steel { get; set; }
        public double t { get; set; }
        public int N { get; set; }

        public int n { get; set; }

        public double M { get; set; }

        public double e1 { get; set; }

        public double l1 { get; set; }

        public bool PreciseMontage { get; set; }

        public bool ReinforceingPad { get; set; }

        public double Q { get; set; }

        public double h { get; set; }

        public double h1 { get; set; }

        public double g { get; set; }
        public BracketVerticalType Type { get; set; }
        public double b4 { get; set; }
        public double b3 { get; set; }
      
    }
}
