using CalculateVessels.Core.Interfaces;
using System.Collections.Generic;

namespace CalculateVessels.Core.Shells.CylindricalShell
{
    public class CylindricalShellCalculatedData : ICalculatedData
    {


        public CylindricalShellCalculatedData(CylindricalShellInputData inputData)
        {
            InputData = inputData;
            ErrorList = new List<string>();
        }

        public CylindricalShellInputData InputData { get; }

        public double b { get; set; }
        public double b_2 { get; set; }
        public double B1 { get; set; }
        public double B1_2 { get; set; }
        public double ConditionStability { get; set; }
        public double F { get; set; }
        public double FAllow { get; set; }
        public double F_de { get; set; }
        public double F_de1 { get; set; }
        public double F_de2 { get; set; }
        public double F_dp { get; set; }
        public double l { get; set; }
        public double lambda { get; set; }
        public double lpr { get; set; }
        public double M_d { get; set; }
        public double M_de { get; set; }
        public double M_dp { get; set; }
        public double Q_d { get; set; }
        public double Q_de { get; set; }
        public double Q_dp { get; set; }
        public double s_f { get; set; }
        public double s_pf { get; set; }
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
    }
}
