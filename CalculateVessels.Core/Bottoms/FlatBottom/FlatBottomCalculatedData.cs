using System.Collections.Generic;
using CalculateVessels.Core.Interfaces;

namespace CalculateVessels.Core.Bottoms.FlatBottom
{
    public class FlatBottomCalculatedData : ICalculatedData
    {
        public FlatBottomCalculatedData(FlatBottomInputData inputData)
        {
            InputData = inputData;
            ErrorList = new List<string>();
        }

        public IInputData InputData { get; }

        public ICollection<string> ErrorList { get; }
        public double SigmaAllow { get; set; }
        public double c { get; set; }
        public double K { get; set; }
        public double Dp { get; set; }
        public bool IsConditionFixed { get; set; }
        public double K_1 { get; set; }
        public double s2p_1 { get; set; }
        public double s2p_2 { get; set; }
        public double s2 { get; set; }
        public double s2p { get; set; }
        public double K0 { get; set; }
        public double s1p { get; set; }
        public double s1 { get; set; }
        public double ConditionUseFormulas { get; set; }
        public bool IsConditionUseFormulas { get; set; }
        public double Kp { get; set; }
        public double p_d { get; set; }
    }
}