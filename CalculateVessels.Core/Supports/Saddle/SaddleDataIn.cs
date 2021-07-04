using CalculateVessels.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using CalculateVessels.Core.Supports.Enums;

namespace CalculateVessels.Core.Supports.Saddle
{
    public class SaddleDataIn : IDataIn
    {
        public bool IsDataGood => !ErrorList.Any();

        public List<string> ErrorList { get; } = new ();

        public double G { get; set; }
        public double L { get; set; }
        public double Lob { get; set; }
        public double H { get; set; }
        public double e { get; set; }
        public double a { get; set; }
        public double p { get; set; }
        public double D { get; set; }
        public double s { get; set; }
        public double s2 { get; set; }
        public double c { get; set; }
        public double fi { get; set; }
        public double sigma_d { get; set; }
        public double E { get; set; }
        public double ny { get; set; }
        public SaddleType Type { get; set; }
        public double b { get; set; }
        public double b2 { get; set; }
        public double delta1 { get; set; }
        public double delta2 { get; set; }
        public string Name { get; set; }
        public string NameShell { get; set; }
        public double Temp { get; set; }
        public double l0 { get; set; }
        public string Steel { get; set; }
        public bool IsPressureIn { get; set; }
        public bool IsAssembly { get; set; }
        public double Ak { get; set; }
    }
}
