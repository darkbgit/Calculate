using CalculateVessels.Core.HeatExchanger.Enums;
using CalculateVessels.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateVessels.Core.HeatExchanger
{
    public class HeatExchangerDataIn : IDataIn
    {
        public bool IsDataGood => !ErrorList.Any();

        public List<string> ErrorList { get; private set; } = new();

        public double a { get; set; }
        public double a1 { get; set; }

        //tube
        public int i { get; set; }
        public double dT { get; set; }
        public double sT { get; set; }
        public double l { get; set; } //UNDONE: l - half long tube

        //shell
        
        public double sK { get; set; }
        public double D { get; internal set; }



        //compensator
        public bool IsNeedKcompensatorCalculate { get; set; }
        public CompensatorType CompensatorType { get; set; }
        public double dkom { get; set; }
        public double Dkom { get; set; }
        public double rkom { get; set; }
        public double deltakom { get; set; }
        public int nkom { get; internal set; }
        public double Kkom { get; internal set; }
        public double beta0 { get; internal set; }

        //extender
        public double D1 { get; internal set; }
        public double deltap { get; internal set; }
        public double Lpac { get; internal set; }

        //tube plate
        public bool IsDifferentTubePlate { get; set; }
        public double sp { get; internal set; }
        public double sp1 { get; internal set; }
        public double sp2 { get; internal set; }
        public double d0 { get; internal set; }
        public double tp { get; internal set; }
        public double s1 { get; internal set; }
        public double s2 { get; internal set; }
        public double DH { get; internal set; }
        public double h1 { get; internal set; }
        public double h2 { get; internal set; }
        public double tK { get; internal set; }
        public double t0 { get; internal set; }
        public double tT { get; internal set; }
        public double pT { get; internal set; }
        public double pM { get; internal set; }
        public bool IsWithPartitions { get; internal set; }
        public double l1R { get; internal set; }
    }
}
