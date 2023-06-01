using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Base;
using System.Text;

namespace CalculateVessels.Core.Shells.Conical;

#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming

public class ConicalShellCalculated : ShellCalculatedData, ICalculatedElement
{
    public ConicalShellCalculated()
    {
        Bibliography = new[]
        {
            Data.Properties.Resources.GOST_34233_1,
            Data.Properties.Resources.GOST_34233_2
        };
    }

    public bool IsConditionUseFormulasBigConnection { get; set; }
    public bool IsConditionUseFormulasLittleConnection { get; set; }

    public double Ak { get; set; }

    /// <summary>
    /// in radians
    /// </summary>
    public double alpha1 { get; set; }
    public double a1p { get; set; }
    public double a2p { get; set; }
    public double a1p_l { get; set; }
    public double a2p_l { get; set; }
    public double B1 { get; set; }
    public double B1_1 { get; set; }
    public double B2 { get; set; }
    public double B3 { get; set; }
    public double beta { get; set; }
    public double betaLittle { get; set; }
    public double beta_0 { get; set; }
    public double beta_1 { get; set; }
    public double beta_2 { get; set; }
    public double beta_3 { get; set; }
    public double beta_3_2 { get; set; }
    public double beta_4 { get; set; }
    public double beta_a { get; set; }
    public double beta_H { get; set; }
    public double beta_t { get; set; }
    public double ConditionForBetaH { get; set; }
    public double ConditionUseFormulas { get; set; }
    public double ConditionUseFormulasToroidal { get; set; }
    public double chi_1Big { get; set; }
    public double chi_1Little { get; set; }
    public double DE { get; set; }
    public double DE_1 { get; set; }
    public double DE_2 { get; set; }
    public double Dk { get; set; }
    public double E { get; set; }
    public double lE { get; set; }
    public double SigmaAllow1Big { get; set; }
    public double SigmaAllow1Little { get; set; }
    public double SigmaAllow2Big { get; set; }
    public double SigmaAllow2Little { get; set; }
    public double SigmaAllowC { get; set; }
    public double SigmaAllowT { get; set; }
    public double s_t { get; set; }
    public double s_tp { get; set; }
    public double s_p_1 { get; set; }
    public double s_p_2 { get; set; }
    public double s_1Big { get; set; }
    public double s_1Little { get; set; }
    public double s_2Big { get; set; }
    public double s_2pBig { get; set; }
    public double s_2Little { get; set; }
    public double s_2pLittle { get; set; }
    public double p_dp { get; set; }
    public double p_dBig { get; set; }
    public double p_dLittle { get; set; }

    public override string ToString()
    {
        if (InputData is not ConicalShellInput dataIn) return "Conical shell";

        var builder = new StringBuilder();
        builder.Append("Conical shell - ");
        builder.Append(dataIn.IsPressureIn ? "inside" : "outside");
        builder.Append($" pressure {dataIn.p} MPa");
        builder.Append($" D - {dataIn.D} mm");
        builder.Append($" D1 - {dataIn.D1} mm");

        return builder.ToString();
    }
}