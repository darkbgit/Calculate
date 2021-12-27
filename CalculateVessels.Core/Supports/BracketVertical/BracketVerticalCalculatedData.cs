using System.Collections.Generic;
using CalculateVessels.Core.Interfaces;

namespace CalculateVessels.Core.Supports.BracketVertical
{
    public class BracketVerticalCalculatedData : ICalculatedData
    {
        public BracketVerticalCalculatedData(BracketVerticalInputData inputData)
        {
            InputData = inputData;
        }

        internal BracketVerticalInputData InputData { get; }
        public double SigmaAlloy { get; internal set; }
        public double E { get; internal set; }
        public double ny { get; internal set; }
        public double Dp { get; internal set; }
        public double e1 { get; internal set; }
        public double F1 { get; internal set; }
        public double Q1 { get; internal set; }
        public double e1e { get; internal set; }
        public double F1Alloy { get; internal set; }
        public double sigmaid { get; internal set; }
        public double K7 { get; internal set; }
        public double x { get; internal set; }
        public double y { get; internal set; }
        public double z { get; internal set; }
        public double K71 { get; internal set; }
        public double K72 { get; internal set; }
        public double K1 { get; internal set; }
        public double K2 { get; internal set; }
        public double v1 { get; internal set; }
        public double v2 { get; internal set; }
        public double sigma_m { get; internal set; }
        public double y1 { get; internal set; }
        public double K81 { get; internal set; }
        public double K82 { get; internal set; }
        public double K8 { get; internal set; }
        public ICollection<string> ErrorList { get; internal set; }
        public bool IsConditionUseFormulas { get; internal set; }
        public double ConditionUseFormulas1 { get; internal set; }
        public double ConditionUseFormulas2 { get; internal set; }
        public double ConditionUseFormulas3 { get; internal set; }
        public double ConditionUseFormulas4 { get; internal set; }
        public double ConditionUseFormulas5 { get; internal set; }
        public double ConditionUseFormulas6 { get; internal set; }
        public double ConditionUseFormulas7 { get; internal set; }
        public double ConditionUseFormulas8 { get; internal set; }

    }
}