using CalculateVessels.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalculateVessels.Core.Bottoms.Enums;

namespace CalculateVessels.Core.Bottoms.FlatBottomWithAdditionalMoment
{
    public class FlatBottomWithAdditionalMomentDataIn : IDataIn
    {
        public bool IsDataGood => !ErrorList.Any();

        public List<string> ErrorList { get; private set; } = new();

        //shell
        public double D { get; set; }
        public double s { get; set; }
        public double c1 { get; set; }
        public double c2 { get; set; }
        public double c3 { get; set; }

        //cover
        public double D2 { get; set; }
        public double D3 { get; set; }
        public double s1 { get; set; }
        public double s2 { get; set; }
        public double s3 { get; set; }
        public double s4 { get; set; }

        public string CoverSteel { get; set; }

        public bool IsCoverFlat { get; set; }
        public bool IsCoverWithGroove { get; set; }

        //stress condition
        public double p { get; set; }
        public double t { get; set; }
        public double F { get; set; }
        public double M { get; set; }
        public bool IsPressureIn { get; set; }
        public double fi { get; set; }
        public double sigma_d { get; set; }

        //screw
        public bool IsStud { get; set; }
        public bool IsScrewWithGroove { get; set; }
        public string ScrewSteel { get; set; }
        public double Lb0 { get; set; }
        public int Screwd { get; set; }
        public int n { get; set; }

        //gasket
        public string GasketType { get; set; }
        public double Dcp { get; set; }
        public double bp { get; set; }
        public double hp { get; set; }

        //flange
        public double Db { get; set; }
        public string FlangeSteel { get; set; }
        public double h { get; set; }
        public bool IsFlangeIsolated { get; set; }
        public bool IsFlangeFlat { get; set; }
        public double S0 { get; set; }
        public double S1 { get; set; }
        public double l { get; set; }
        public double Dn { get; set; }
        public FlangeFaceType FlangeFace { get; set; }

        //washer
        public bool IsWasher { get; set; }
        public string WasherSteel { get; set; }
        public double hsh { get; set; }

        //hole
        public HoleInFlatBottom Hole { get; set; }

        public double di { get; set; }
        public double d { get; set; }


        public string Name { get; set; }
    }
 }
