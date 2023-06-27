using System.Text;
using CalculateVessels.Core.Elements.Base;
using CalculateVessels.Core.Interfaces;

namespace CalculateVessels.Core.Elements.Supports.BracketVertical;

#pragma warning disable IDE1006
// ReSharper disable InconsistentNaming

public class BracketVerticalCalculated : CalculatedElement, ICalculatedElement
{
    public BracketVerticalCalculated()
    {
        Bibliography = new[]
        {
            Data.Properties.Resources.GOST_34233_1,
            Data.Properties.Resources.GOST_34233_5
        };
    }

    public override string Type => nameof(BracketVerticalCalculated);

    public bool IsConditionUseFormulas { get; internal set; }
    public double ConditionUseFormulas1 { get; internal set; }
    public double ConditionUseFormulas2 { get; internal set; }
    public double ConditionUseFormulas3 { get; internal set; }
    public double ConditionUseFormulas4 { get; internal set; }
    public double ConditionUseFormulas5 { get; internal set; }
    public double ConditionUseFormulas6 { get; internal set; }
    public double ConditionUseFormulas7 { get; internal set; }
    public double ConditionUseFormulas8 { get; internal set; }
    public double Dp { get; internal set; }
    public double E { get; internal set; }
    public double e1 { get; internal set; }
    public double e1e { get; internal set; }
    public double F1 { get; internal set; }
    public double F1Allow { get; internal set; }
    public double K1 { get; internal set; }
    public double K2 { get; internal set; }
    public double K7 { get; internal set; }
    public double K71 { get; internal set; }
    public double K72 { get; internal set; }
    public double K8 { get; internal set; }
    public double K81 { get; internal set; }
    public double K82 { get; internal set; }
    public double ny { get; internal set; }
    public double Q1 { get; internal set; }
    public double sigma_m { get; internal set; }
    public double SigmaAllow { get; internal set; }
    public double sigmaid { get; internal set; }
    public double v1 { get; internal set; }
    public double v2 { get; internal set; }
    public double x { get; internal set; }
    public double y { get; internal set; }
    public double y1 { get; internal set; }
    public double z { get; internal set; }

    public override string ToString()
    {
        if (InputData is not BracketVerticalInput inputData) return "Bracket vertical";

        var builder = new StringBuilder();
        builder.Append("Bracket vertical - ");
        builder.Append($" pressure {inputData.p} MPa");
        builder.Append($" D - {inputData.D} mm");

        return builder.ToString();
    }
}