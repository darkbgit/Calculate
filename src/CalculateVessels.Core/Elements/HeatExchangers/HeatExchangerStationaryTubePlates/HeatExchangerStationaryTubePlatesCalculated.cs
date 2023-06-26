using CalculateVessels.Core.Elements.Base;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.Properties;

namespace CalculateVessels.Core.Elements.HeatExchangers.HeatExchangerStationaryTubePlates;

#pragma warning disable IDE1006
// ReSharper disable InconsistentNaming

public class HeatExchangerStationaryTubePlatesCalculated : CalculatedElement, ICalculatedElement
{
    public HeatExchangerStationaryTubePlatesCalculated()
    {
        Bibliography = new[]
        {
            Resources.GOST_34233_1,
            Resources.GOST_34233_6,
            Resources.GOST_34233_7
        };
    }

    public override string Type => nameof(HeatExchangerStationaryTubePlatesInput);

    public double ET { get; set; }
    public double EK { get; set; }
    public double ED { get; set; }
    public double ED2 { get; set; }
    public double Ep { get; set; }
    public double Ep2 { get; set; }
    public double E1 { get; set; }
    public double E12 { get; set; }
    public double E2 { get; set; }
    public double E22 { get; set; }
    public double Ekom { get; set; }
    public double sigma_dp { get; set; }
    public double sigma_dp2 { get; set; }
    public double sigma_dK { get; set; }
    public double sigma_dT { get; set; }
    public double alfaK { get; set; }
    public double alfaT { get; set; }
    public double Rmp { get; set; }
    public double Rmp2 { get; set; }
    public double nNForSigmaa { get; set; }
    public double nsigmaForSigmaa { get; set; }
    public double CtForSigmaa { get; set; }
    public double AForSigmaap { get; set; }
    public double BForSigmaa { get; set; }
    public double sigmaa_dp { get; set; }
    public double AForSigmaap2 { get; set; }
    public double sigmaa_dp2 { get; set; }
    public double AForSigmaaK { get; set; }
    public double sigmaa_dK { get; set; }
    public double AForSigmaaT { get; set; }
    public double sigmaa_dT { get; set; }
    public double Ksigma { get; set; }
    public double a { get; set; }
    public double mn { get; set; }
    public double etaM { get; set; }
    public double etaT { get; set; }
    public double Ky { get; set; }
    public double ro { get; set; }
    public double Kqz { get; set; }
    public double Kpz { get; set; }
    public double betakom { get; set; }
    public double Xkom { get; set; }
    public double Ykom { get; set; }
    public double Cf { get; set; }
    public double Akom { get; set; }
    public double Kkom { get; set; }
    public double betap { get; set; }
    public double Ap { get; set; }
    public double Kpac { get; set; }
    public double Ap1 { get; set; }
    public double Ap2 { get; set; }
    public double Bp1 { get; set; }
    public double Bp2 { get; set; }
    public double Kq { get; set; }
    public double Kp { get; set; }
    public double psi0 { get; set; }
    public double beta { get; set; }
    public double omega { get; set; }
    public double b1 { get; set; }
    public double fip { get; set; }
    public double R1 { get; set; }
    public double b2 { get; set; }
    public double R2 { get; set; }
    public double beta1 { get; set; }
    public double beta2 { get; set; }
    public double K1 { get; set; }
    public double K2 { get; set; }
    public double KF1 { get; set; }
    public double KF2 { get; set; }
    public double KF { get; set; }
    public double mcp { get; set; }
    public double p0 { get; set; }
    public double ro1 { get; set; }
    public double t { get; set; }
    public double T1 { get; set; }
    public double T2 { get; set; }
    public double T3 { get; set; }
    public double Phi1 { get; set; }
    public double Phi2 { get; set; }
    public double Phi3 { get; set; }
    public double m1 { get; set; }
    public double m2 { get; set; }
    public double p1 { get; set; }
    public double MP { get; set; }
    public double QP { get; set; }
    public double Ma { get; set; }
    public double Qa { get; set; }
    public double NT { get; set; }
    public double JT { get; set; }
    public double lpr { get; set; }
    public double MT { get; set; }
    public double QK { get; set; }
    public double MK { get; set; }
    public double F { get; set; }
    public double sigmap1 { get; set; }
    public double taup1 { get; set; }
    public double mA { get; set; }
    public double nB { get; set; }
    public double pfip { get; set; }
    public double sigmap2 { get; set; }
    public double Mmax { get; set; }
    public double taup2 { get; set; }
    public double sigmaMX { get; set; }
    public double sigmaIX { get; set; }
    public double sigmaMfi { get; set; }
    public double sigmaIfi { get; set; }
    public double sigma1T { get; set; }
    public double sigma1 { get; set; }
    public double sigma2T { get; set; }
    public double ConditionStaticStressForTubePlate1 { get; set; }
    public double ConditionStaticStressForTubePlate2 { get; set; }
    public bool IsConditionStaticStressForTubePlate { get; set; }
    public double deltasigma1pk { get; set; }
    public double deltasigma2pk { get; set; }
    public double deltasigma3pk { get; set; }
    public double sigmaa_5_2_4k { get; set; }
    public double deltasigma1pp { get; set; }
    public double deltasigma2pp { get; set; }
    public double sigmaa_5_2_4p { get; set; }
    public double deltasigma3pp { get; set; }
    public double spp { get; set; }
    public double sp { get; set; }
    public double snp1 { get; set; }
    public double snp2 { get; set; }
    public double snp { get; set; }
    public double sn { get; set; }
    public double W { get; set; }
    public double ConditionStaticStressForShell2 { get; set; }
    public bool IsConditionStaticStressForShell { get; set; }
    public double deltasigma1K { get; set; }
    public double deltasigma2K { get; set; }
    public double deltasigma3K { get; set; }
    public double sigmaaK { get; set; }
    public double ConditionStaticStressForTube1 { get; set; }
    public bool IsConditionStaticStressForTube { get; set; }
    public double deltasigma1T { get; set; }
    public double deltasigma2T { get; set; }
    public double deltasigma3T { get; set; }
    public double sigmaaT { get; set; }
    public double KT { get; set; }
    public double lR { get; set; }
    public double lambda { get; set; }
    public double phiT { get; set; }
    public double ConditionStabilityForTube2 { get; set; }
    public bool IsConditionStabilityForTube { get; set; }
    public double lambday { get; set; }
    public double Ay { get; set; }
    public double Y { get; set; }
    public double Ntp_d { get; set; }
    public double phiC2 { get; set; }
    public double phiC { get; set; }
    public double tau { get; set; }
    public bool IsConditionStressBracingTube { get; set; }
    public double ConditionStressBracingTube2 { get; set; }
    public double ConditionStressBracingTube11 { get; set; }
    public double ConditionStressBracingTube12 { get; set; }
    public double pp { get; set; }
    public double spp_5_5_1 { get; set; }
    public double sp_5_5_1 { get; set; }
    public double A { get; set; }
    public double B { get; set; }
    public double W_d { get; set; }

    public override string ToString() => $"Теплообменный аппарат с неподвижными трубными решетками {InputData.Name}";
}