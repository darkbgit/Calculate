using System.Collections.Generic;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Base;

namespace CalculateVessels.Core.Shells.Conical
{
    public class ConicalShellCalculatedData : ShellCalculatedData, ICalculatedData
    {
        public ConicalShellCalculatedData(ConicalShellInputData inputData)
        {
            InputData = inputData;
        }

        public IInputData InputData { get; }

        public double Dk { get; set; }
        public double a1p { get; set; }
        public double a2p { get; set; }
        public double a1p_l { get; set; }
        public double a2p_l { get; set; }
        public double E { get; set; }
        public double lE { get; set; }
        public double DE_1 { get; set; }
        public double DE_2 { get; set; }
        public double DE { get; set; }
        public double B1_1 { get; set; }
        public double B1 { get; set; }
        public double s_p_1 { get; set; }
        public double s_p_2 { get; set; }
        public double p_dp { get; set; }
        public double chi_1 { get; set; }
        public double beta { get; set; }
        public double SigmaAllow1 { get; set; }
        public double SigmaAllow2 { get; set; }
        public double beta_1 { get; set; }
        public double s_2p { get; set; }
        public double p_dBig { get; set; }
        public double beta_a { get; set; }
        public double Ak { get; set; }
        public double SigmaAllowC { get; set; }
        public double B2 { get; set; }
        public double B3 { get; set; }
        public double beta_0 { get; set; }
        public double beta_2 { get; set; }
        public double beta_t { get; set; }
        public double beta_3 { get; set; }
        public double s_tp { get; set; }
        public double ConditionForBetan { get; set; }
        public double beta_n { get; set; }
        public double beta_4 { get; set; }
        public double p_dLittle { get; set; }
        public double s_2pLittle { get; set; }
        public double cosAlpha1 { get; set; }
        public double tgAlpha1 { get; set; }
        public double sinAlpha1 { get; set; }
    }
}