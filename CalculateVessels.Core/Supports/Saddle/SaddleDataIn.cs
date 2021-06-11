using CalculateVessels.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateVessels.Core.Supports.Saddle
{
    public class SaddleDataIn : IDataIn
    {
        public bool IsDataGood => throw new NotImplementedException();

        public List<string> ErrorList => throw new NotImplementedException();

        public bool CheckData() => ErrorList.Any();

        

       

        public double G { get; set; }
        public double L { get; set; }
        public double Lob { get; set; }
        public double H
        {
            get; set;
        }
        public double e
        {
            get; set;
        }
        public double a
        {
            get; set;
        }
        public double p
        {
            get; set;
        }
        public double D
        {
            get; set;
        }
        public double s
        {
            get; set;
        }
        public double s2
        {
            get; set;
        }
        public double c
        {
            get; set;
        }
        public double fi
        {
            get; set;
        }
        public double sigma_d
        {
            get; set;
        }
        public double E
        {
            get; set;
        }
        public double ny
        {
            get; set;
        }
        internal int type;
        public double b
        {
            get; set;
        }
        public double b2
        {
            get; set;
        }
        public double delta1
        {
            get; set;
        }
        public double delta2
        {
            get; set;
        }
        internal string name;
        internal string nameob;
        public double temp
        {
            get; set;
        }
        public double l0
        {
            get; set;
        }
        internal string steel;

        internal bool isPressureIn;

    }
}
