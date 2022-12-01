using System.Collections.Generic;
using CalculateVessels.Core.Interfaces;

namespace CalculateVessels.Core.Bottoms.FlatBottomWithAdditionalMoment
{
    public class FlatBottomWithAdditionalMomentCalculatedData : ICalculatedData
    {
        public FlatBottomWithAdditionalMomentCalculatedData(FlatBottomWithAdditionalMomentInputData inputData)
        {
            InputData = inputData;
            ErrorList = new List<string>();
        }

        public IInputData InputData { get; }
        public ICollection<string> ErrorList { get; }
        public double c { get; set; }
        public double tf { get; set; }
        public double tb { get; set; }
        public double sigma_d { get; set; }
        public double E20 { get; set; }
        public double E { get; set; }
        public double Ekr20 { get; set; }
        public double Ekr { get; set; }
        public double alfaf { get; set; }
        public double alfakr { get; set; }
        public double sigma_d_krm { get; set; }
        public double tkr { get; set; }
        public double hkr { get; set; }
        public double deltakr { get; set; }
        public int dkr { get; set; }
        public double alfash1 { get; set; }
        public double alfash2 { get; set; }
        public double Eb20 { get; set; }
        public double Eb { get; set; }
        public double alfab { get; set; }
        public double sigma_dnb { get; set; }
        public double fb { get; set; }
        public double S0 { get; set; }
        public double m { get; set; }
        public double qobj { get; set; }
        public double q_d { get; set; }
        public double Kobj { get; set; }
        public double Ep { get; set; }
        public bool IsGasketFlat { get; set; }
        public bool IsGasketMetal { get; set; }
        public double b0 { get; set; }
        public double Dcp { get; set; }
        public double Pobj { get; set; }
        public double Rp { get; set; }
        public double Ab { get; set; }
        public double Qd { get; set; }
        public double QFM { get; set; }
        public double b { get; set; }
        public double l0 { get; set; }
        public double beta { get; set; }
        public double x { get; set; }
        public double zeta { get; set; }
        public double Se { get; set; }
        public double yp { get; set; }
        public double Lb { get; set; }
        public double yb { get; set; }
        public double KGost34233_4 { get; set; }
        public double betaT { get; set; }
        public double betaU { get; set; }
        public double betaY { get; set; }
        public double betaZ { get; set; }
        public double betaF { get; set; }
        public double betaV { get; set; }
        public double f { get; set; }
        public double lambda { get; set; }
        public double yF { get; set; }
        public double yfn { get; set; }
        public double gamma { get; set; }
        public double Kkr { get; set; }
        public double Xkr { get; set; }
        public double ykr { get; set; }
        public double Qt { get; set; }
        public double alfa { get; set; }
        public double alfa_m { get; set; }
        public double Pb1_1 { get; set; }
        public double Pb1 { get; set; }
        public double Pb1_2 { get; set; }
        public double Pb2_2 { get; set; }
        public double Pb2 { get; set; }
        public double Pbm { get; set; }
        public double Pbp { get; set; }
        public double psi1 { get; set; }
        public double K6 { get; set; }
        public double Dp { get; set; }
        public double K0 { get; set; }
        public double s1p { get; set; }
        public double s1 { get; set; }
        public double Phi_1 { get; set; }
        public double Phi_2 { get; set; }
        public double Phi { get; set; }
        public double K7Fors2 { get; set; }
        public double s2p_1 { get; set; }
        public double s2p_2 { get; set; }
        public double s2p { get; set; }
        public double s2 { get; set; }
        public double K7Fors3 { get; set; }
        public double s3p_1 { get; set; }
        public double s3p_2 { get; set; }
        public double s3p { get; set; }
        public double s3 { get; set; }
        public double ConditionUseFormulas { get; set; }
        public bool IsConditionUseFormulas { get; set; }
        public double Kp { get; set; }
        public double p_d { get; set; }
        public double e { get; set; }
    }
}