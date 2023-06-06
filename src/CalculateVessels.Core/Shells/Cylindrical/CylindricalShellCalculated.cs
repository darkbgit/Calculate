using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Base;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculateVessels.Core.Shells.Cylindrical;

public class CylindricalShellCalculated : ShellCalculated, ICalculatedElement
{
    public CylindricalShellCalculated(CylindricalShellCalculatedCommon commonData,
        IEnumerable<CylindricalShellCalculatedOneLoading> results)
    {
        Bibliography = new[]
        {
            Data.Properties.Resources.GOST_34233_1,
            Data.Properties.Resources.GOST_34233_2
        };

        CommonData = commonData;
        Results = results;
    }

    public override CylindricalShellCalculatedCommon CommonData { get; }

    public override IEnumerable<CylindricalShellCalculatedOneLoading> Results { get; }

    public override string ToString()
    {
        if (InputData is not CylindricalShellInput dataIn) return "Cylindrical shell";

        var builder = new StringBuilder();
        builder.Append("Cylindrical shell - ");
        builder.Append($" D - {dataIn.D} mm");
        dataIn.LoadingConditions.ToList()
            .ForEach(x =>
            {
                builder.Append(x.IsPressureIn ? "inside" : "outside");
                builder.Append($" pressure {x.p} MPa");
            });

        return builder.ToString();
    }
}
