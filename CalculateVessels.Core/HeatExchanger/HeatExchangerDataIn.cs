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
        public double D { get; set; }



        //compensator
        public bool IsNeedKcompensatorCalculate { get; set; }
        public CompensatorType CompensatorType { get; set; }
        public double dkom { get; set; }
        public double Dkom { get; set; }
        public double rkom { get; set; }
        public double deltakom { get; set; }
        public int nkom { get; set; }
        public double Kkom { get; set; }
        public double beta0 { get; set; }

        //extender
        public double D1 { get; set; }
        public double deltap { get; set; }
        public double Lpac { get; set; }

        //tube plate
        public bool IsDifferentTubePlate { get; set; }
        public double sp { get; set; }
        public double sp1 { get; set; }
        public double sp2 { get; set; }
        public double d0 { get; set; }
        public double tp { get; set; }
        public double tP { get; internal set; }
        public double s1 { get; set; }
        public double s2 { get; set; }
        public double DH { get; set; }
        public double h1 { get; set; }
        public double h2 { get; set; }
        public double tK { get; set; }
        public double t0 { get; set; }
        public double tT { get; set; }
        public double pT { get; set; }
        public double pM { get; set; }
        public bool IsWithPartitions { get; set; }
        public double l1R { get; set; }
        public double s1p { get; set; }
        public double c { get; set; }
        public double fip { get; set; }
        public double cK { get; internal set; }
        public int N { get; internal set; }


        public string Stellp { get; set; }
        public bool IsOneGo { get; internal set; }
        public double BP { get; internal set; }
        public double sn { get; internal set; }
        public bool IsNeedCheckHardnessTubePlate { get; internal set; }
        public string StellK { get; internal set; }
        public string StellT { get; internal set; }
        public bool IsWorkCondition { get; internal set; }
        public byte l2R { get; internal set; }
        public bool IsNeedCheckHardnessTube { get; internal set; }
        public TubeRollingType TubeRolling { get; internal set; }
        public double lB { get; internal set; }
        public bool IsTubeOnlyWelding { get; internal set; }
        public double delta { get; internal set; }
        public double pp { get; internal set; }
    }
}
