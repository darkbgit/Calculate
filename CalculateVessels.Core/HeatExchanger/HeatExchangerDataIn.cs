﻿using CalculateVessels.Core.Bottoms.Enums;
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
        //public bool IsDataGood => !ErrorList.Any();

        public IEnumerable<string> ErrorList => _errorList;

        private List<string> _errorList = new();

        public string Name { get; set; } //+
        public bool IsOneGo { get; set; }//+
        public bool IsWorkCondition { get; set; }//+
        public double t0 { get; set; } //+
        public int N { get; set; } //+

        //public double a { get; set; }
        public double a1 { get; set; }//+

        //tube
        public bool IsNeedCheckHardnessTube { get; set; }//+
        public double dT { get; set; }//+
        public double l { get; set; } //+ UNDONE: l - half long tube
        public double pT { get; set; } //+
        public double sT { get; set; }//+
        public double tT { get; set; } //+
        public double TT { get; set; } //+
        public int i { get; set; }//+
        public string SteelT { get; set; }//+
        

        //shell
        public double cK { get; set; } //+
        public double D { get; set; } //+
        public double pM { get; set; } //+
        public double sK { get; set; } //+
        public double tK { get; set; } //+
        public double TK { get; set; } //+
        public string SteelK { get; set; } //+

        //compensator
        public bool IsNeedKcompensatorCalculate { get; set; }
        public CompensatorType CompensatorType { get; set; }//+
        public double beta0 { get; set; }
        public double deltakom { get; set; }
        public double Dkom { get; set; }
        public double dkom { get; set; }
        public double Kkom { get; set; }
        public double rkom { get; set; }
        public int nkom { get; set; }
        public string Steelkom { get; set; }

        //extender
        public double D1 { get; set; }
        public double deltap { get; set; }
        public double Lpac { get; set; }

        //tube plate
        public bool IsDifferentTubePlate { get; set; }//+
        
        

        public double d0 { get; set; }//+
        public double DE { get; set; }//+

        public double tp { get; set; }//+
        public double tP { get; set; }//+ distance over hole both side from hole

        public ConnectionTubePlate FirstTubePlate { get; set; } = new();
        public ConnectionTubePlate SecondTubePlate { get; set; } = new();
         

        //partitions
        public bool IsWithPartitions { get; set; } //+
        public double l1R { get; set; } //+
        public double l2R { get; set; } //+

        public int Bper { get; set; }
        public int Lper { get; set; }
        public double cper { get; set; }
        public double sper { get; set; }
    }

    public class ConnectionTubePlate
    {
        public bool IsNeedCheckHardnessTubePlate { get; set; }//+
        public TubePlateType TubePlateType{ get; set; }
        public FixTubeInTubePlateType FixTubeInTubePlate { get; set; }
        public TubeRollingType TubeRolling { get; set; }

        //Tube plate
        public string Steelp { get; set; } 
        
        public double BP { get; set; }//+
        public double c { get; set; }//+

        public double fiP { get; set; }
        
        public double pp { get; set; }
        
        public double s1p { get; set; }//+
        
        public double sn { get; set; }//+
        public double sp { get; set; }//+
        public bool IsWithGroove {  get; set; }//+


        //shell for tube plate
        public double s1 { get; set; } //+


        //flange for tube plate
        public string Steel1 { get; set; } //+
        public double h1 { get; set; } //+
        public double DH { get; set; }//+

        //shell for chamber
        public string SteelD { get; set; }
        public double s2 { get; set; } //+

        //flange for chamber
        public string Steel2 { get; set; } //+
        public double h2 { get; set; } //+
        public bool IsChamberFlangeSkirt { get; set; }//+
        public FlangeFaceType FlangeFace {  get; set; }

        //tube
        public double lB { get; set; }//+
        public double delta { get; set; }//+

    }
}
