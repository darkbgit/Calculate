using CalculateVessels.Core.Elements.Shells.Base;
using System.Collections.Generic;

namespace CalculateVessels.Core.Elements.Shells.Conical;

#pragma warning disable IDE1006
// ReSharper disable InconsistentNaming

public class ConicalShellCalculatedOneLoading : ShellCalculatedOneLoadingData
{
    public bool IsConditionUseFormulas { get; set; }
    public bool IsConditionUseFormulasBigConnection { get; set; }
    public bool IsConditionUseFormulasLittleConnection { get; set; }

    public double Ak { get; set; }
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
    public double E { get; set; }
    public ICollection<string> ErrorList { get; set; } = new List<string>();
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
}