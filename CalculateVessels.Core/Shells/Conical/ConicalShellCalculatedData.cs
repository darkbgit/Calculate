using System.Collections.Generic;
using CalculateVessels.Core.Interfaces;

namespace CalculateVessels.Core.Shells.Conical
{
    public class ConicalShellCalculatedData : ICalculatedData
    {
        public ConicalShellCalculatedData(ConicalShellInputData inputData)
        {
            InputData = inputData;
            ErrorList = new List<string>();
        }

        public IInputData InputData { get; }
        public ICollection<string> ErrorList { get; }

        private double _cosAlfa1;
        private double _sinAlfa1;
        private double _tgAlfa1;

        private int switch1;

        private double _s_2plit;


        private double _beta_a;


        public double c { get; set; }
        internal double Dk { get; set; }
        public bool IsConditionUseFormulas { get; set; }
        public double a1p { get; set; }
        public double a2p { get; set; }
        public double a1p_l { get; set; }
        public double a2p_l { get; set; }
        public double s_p { get; set; }
        public double s { get; set; }
        public double p_d { get; set; }
        public double SigmaAllow { get; set; }
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
        public double p_de { get; set; }
        public double chi_1 { get; set; }
        public double beta { get; set; }
        public double SigmaAllow1 { get; set; }
        public double SigmaAllow2 { get; set; }
        public double beta_1 { get; set; }
        public double s_2p { get; set; }
        public double p_dBig { get; set; }
        public double beta_a { get; set; }
        public double Ak { get; set; }
        public double SigmaAllowk { get; set; }
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
        public double s_2plit { get; set; }
        public double cosAlfa1 { get; set; }
        public double tgAlfa1 { get; set; }
        public double sinAlfa1 { get; set; }
    }
}