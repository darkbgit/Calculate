using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Base;
using CalculateVessels.Core.Shells.Enums;

namespace CalculateVessels.Core.Shells.Conical
{
    public class ConicalShellInputData : ShellInputData, IInputData
    {
        public ConicalShellInputData()
            : base(ShellType.Conical)
        {

        }


        public double alfa1 { get; set; }
        public double s1 { get; set; }
        public ConicalConnectionType ConnectionType { get; set; }
        public double s2 { get; set; }
        public double sT { get; set; }
        public bool IsConnectionWithLittle { get; set; }
        public double D1 { get; set; }
        public double r { get; set; }
        public double fi_k { get; set; }
        public double fi_t { get; set; }
    }
}