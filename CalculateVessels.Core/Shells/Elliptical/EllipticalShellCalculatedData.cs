using System.Collections.Generic;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Base;

namespace CalculateVessels.Core.Shells.Elliptical
{
    public class EllipticalShellCalculatedData : ShellCalculatedData, ICalculatedData
    {


        public EllipticalShellCalculatedData(EllipticalShellInputData inputData)
        {
            InputData = inputData;
            ErrorList = new List<string>();
        }

        public IInputData InputData { get; }

        public double b { get; set; }
        public double b_2 { get; set; }
        public double B1 { get; set; }
        public double B1_2 { get; set; }
        public double ConditionStability { get; set; }

        public double l { get; set; }

        public double s_p { get; set; }
        public double s_p_1 { get; set; }
        public double s_p_2 { get; set; }
        public double s { get; set; }

        public double p_d { get; set; }
        public double p_dp { get; set; }
        public double p_de { get; set; }
        public double E { get; set; }
        public double SigmaAllow { get; set; }
        public double c { get; set; }
        public bool IsConditionUseFormulas { get; set; }

        public ICollection<string> ErrorList { get; }
        public double EllipseR { get; internal set; }
        public double EllipseKePrev { get; internal set; }
        public double Ellipsex { get; internal set; }
        public double EllipseKe { get; set; }
    }
}
