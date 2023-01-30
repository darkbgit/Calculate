using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Base;
using System.Text;

namespace CalculateVessels.Core.Shells.Elliptical;

#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming

public class EllipticalShellCalculated : ShellCalculatedData, ICalculatedElement
{
    public EllipticalShellCalculated()
    {
        Bibliography = new[]
        {
            Data.Properties.Resources.GOST_34233_1,
            Data.Properties.Resources.GOST_34233_2
        };
    }

    public double b { get; set; }
    public double b_2 { get; set; }
    public double B1 { get; set; }
    public double B1_2 { get; set; }
    public double ConditionStability { get; set; }
    //public double c { get; set; }
    //public bool IsConditionUseFormulas { get; set; }
    public double l { get; set; }
    //public double SigmaAllow { get; set; }
    //public double s { get; set; }
    //public double s_p { get; set; }
    public double s_p_1 { get; set; }
    public double s_p_2 { get; set; }
    //public double p_d { get; set; }
    public double p_dp { get; set; }
    //public double p_de { get; set; }
    public double E { get; set; }
    public double EllipseR { get; set; }
    public double EllipseKePrev { get; set; }
    public double Ellipsex { get; set; }
    public double EllipseKe { get; set; }

    public override string ToString()
    {
        if (InputData is not EllipticalShellInput dataIn) return "Elliptical shell";

        var builder = new StringBuilder();
        builder.Append("Elliptical shell - ");
        builder.Append(dataIn.IsPressureIn ? "inside" : "outside");
        builder.Append($" pressure {dataIn.p} MPa");
        builder.Append($" D - {dataIn.D} mm");

        return builder.ToString();
    }
}