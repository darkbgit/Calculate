using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Shells.Base;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculateVessels.Core.Shells.Elliptical;

public class EllipticalShellCalculated : ShellCalculated, ICalculatedElement
{
    public EllipticalShellCalculated(EllipticalShellCalculatedCommon commonData,
        IEnumerable<EllipticalShellCalculatedOneLoading> results)
    {
        Bibliography = new[]
        {
            Data.Properties.Resources.GOST_34233_1,
            Data.Properties.Resources.GOST_34233_2
        };
        CommonData = commonData;
        Results = results;
    }

    public override string Type => nameof(EllipticalShellCalculated);

    public override EllipticalShellCalculatedCommon CommonData { get; }

    public override IEnumerable<EllipticalShellCalculatedOneLoading> Results { get; } =
        new List<EllipticalShellCalculatedOneLoading>();

    public override string ToString()
    {
        if (InputData is not EllipticalShellInput dataIn) return "Elliptical shell";

        var builder = new StringBuilder();
        builder.Append("Elliptical shell - ");
        builder.Append($" D={dataIn.D} mm");
        dataIn.LoadingConditions.ToList()
            .ForEach(lc =>
            {
                builder.Append($" p={lc.p} MPa");
                builder.Append(lc.IsPressureIn ? "(inside)" : "(outside)");
                builder.Append($" t={lc.t} C");
            });

        return builder.ToString();
    }
}