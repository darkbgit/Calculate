using CalculateVessels.Core.Base;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Data.PhysicalData.Gost34233_4.Models;
using System.Text;

namespace CalculateVessels.Core.Bottoms.FlatBottomWithAdditionalMoment;

#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming

public class FlatBottomWithAdditionalMomentCalculated : CalculatedElement, ICalculatedElement
{
    public FlatBottomWithAdditionalMomentCalculated()
    {
        Bibliography = new[]
        {
            Data.Properties.Resources.GOST_34233_1,
            Data.Properties.Resources.GOST_34233_2
        };
    }

    public bool IsConditionUseFormulas { get; set; }
    public double Ab { get; set; }
    public double alpha { get; set; }
    public double alpha_m { get; set; }
    public double alpha_b { get; set; }
    public double alpha_f { get; set; }
    public double alpha_kr { get; set; }
    public double alpha_sh1 { get; set; }
    public double alpha_sh2 { get; set; }
    public double b { get; set; }
    public double b0 { get; set; }
    public double beta { get; set; }
    public double betaF { get; set; }
    public double betaT { get; set; }
    public double betaU { get; set; }
    public double betaV { get; set; }
    public double betaY { get; set; }
    public double betaZ { get; set; }
    public double c { get; set; }
    public double ConditionUseFormulas { get; set; }
    public double Dcp { get; set; }
    public double delta_kr { get; set; }
    public double Dp { get; set; }
    public double e { get; set; }
    public double E { get; set; }
    public double E20 { get; set; }
    public double Eb { get; set; }
    public double Eb20 { get; set; }
    public double Ekr { get; set; }
    public double Ekr20 { get; set; }
    public double Ep { get; set; }
    public double f { get; set; }
    public double fb { get; set; }
    public Gasket Gasket { get; set; }
    public double gamma { get; set; }
    public double hkr { get; set; }
    public double K0 { get; set; }
    public double K6 { get; set; }
    public double K7_s2 { get; set; }
    public double K7_s3 { get; set; }
    public double KGost34233_4 { get; set; }
    public double Kkr { get; set; }
    public double Kp { get; set; }
    public double l0 { get; set; }
    public double lambda { get; set; }
    public double Lb { get; set; }
    public double p_d { get; set; }
    public double Pb1 { get; set; }
    public double Pb1_1 { get; set; }
    public double Pb1_2 { get; set; }
    public double Pb2 { get; set; }
    public double Pb2_2 { get; set; }
    public double Pbm { get; set; }
    public double Pbp { get; set; }
    public double Phi { get; set; }
    public double Phi_1 { get; set; }
    public double Phi_2 { get; set; }
    public double Pobj { get; set; }
    public double psi1 { get; set; }
    public double Qd { get; set; }
    public double QFM { get; set; }
    public double Qt { get; set; }
    public double Rp { get; set; }
    public double S0 { get; set; }
    public double s1 { get; set; }
    public double s1p { get; set; }
    public double s2 { get; set; }
    public double s2p { get; set; }
    public double s2p_1 { get; set; }
    public double s2p_2 { get; set; }
    public double s3 { get; set; }
    public double s3p { get; set; }
    public double s3p_1 { get; set; }
    public double s3p_2 { get; set; }
    public double Se { get; set; }
    public double SigmaAllow { get; set; }
    public double sigma_d_krm { get; set; }
    public double sigma_dnb { get; set; }
    public double tb { get; set; }
    public double tf { get; set; }
    public double tkr { get; set; }
    public double x { get; set; }
    public double Xkr { get; set; }
    public double yb { get; set; }
    public double yF { get; set; }
    public double yfn { get; set; }
    public double ykr { get; set; }
    public double yp { get; set; }
    public double zeta { get; set; }
    public int dkr { get; set; }

    public override string ToString()
    {
        if (InputData is not FlatBottomWithAdditionalMomentInput inputData) return "Flat bottom with additional moment";

        var builder = new StringBuilder();
        builder.Append("Flat bottom with additional moment - ");
        builder.Append($" pressure {inputData.p} MPa");
        builder.Append($" D - {inputData.D} mm");

        return builder.ToString();
    }
}