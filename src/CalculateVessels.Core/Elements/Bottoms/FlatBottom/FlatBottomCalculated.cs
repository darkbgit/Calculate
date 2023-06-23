using CalculateVessels.Core.Elements.Base;
using CalculateVessels.Core.Interfaces;
using System.Text;

namespace CalculateVessels.Core.Elements.Bottoms.FlatBottom;

#pragma warning disable IDE1006
// ReSharper disable InconsistentNaming

public class FlatBottomCalculated : CalculatedElement, ICalculatedElement
{
    public FlatBottomCalculated()
    {
        Bibliography = new[]
        {
            Data.Properties.Resources.GOST_34233_1,
            Data.Properties.Resources.GOST_34233_2
        };
    }

    public double ConditionUseFormulas { get; set; }
    public double c { get; set; }
    public double Dp { get; set; }
    public bool IsConditionFixed { get; set; }
    public bool IsConditionUseFormulas { get; set; }
    public double K { get; set; }
    public double K0 { get; set; }
    public double K_1 { get; set; }
    public double Kp { get; set; }
    public double SigmaAllow { get; set; }
    public double s1 { get; set; }
    public double s1p { get; set; }
    public double s2 { get; set; }
    public double s2p { get; set; }
    public double s2p_1 { get; set; }
    public double s2p_2 { get; set; }
    public double p_d { get; set; }

    public override string ToString()
    {
        if (InputData is not FlatBottomInput inputData) return "Flat bottom";

        var builder = new StringBuilder();
        builder.Append("Flat bottom - ");
        builder.Append($" pressure {inputData.p} MPa");
        builder.Append($" D - {inputData.D} mm");

        return builder.ToString();
    }
}