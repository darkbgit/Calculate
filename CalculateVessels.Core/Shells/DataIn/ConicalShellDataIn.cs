using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateVessels.Core.Shells.DataIn
{
    public class ConicalShellDataIn : ShellDataIn, IDataIn
    {
        public ConicalShellDataIn()
            : base(ShellType.Conical)
        {

        }

        public double alfa1 { get; set; }


        public double D1 { get; set; }

        public double s1 { get; set; }
        public double s2 { get; set; }
        public double sT { get; set; }

        public bool IsTorr { get; set; }

        public double r { get; set; }

        public bool IsConnectionBig { get; set; }
        public bool IsConnectionLittle { get; set; }

        public ConicalConnectionType ConnectionType { get; set; }

        public double sigma_d_1 { get; set; }
        public double sigma_d_2 { get; set; }
    }
}
