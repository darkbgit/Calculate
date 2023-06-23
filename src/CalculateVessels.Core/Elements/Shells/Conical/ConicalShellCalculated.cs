using CalculateVessels.Core.Elements.Shells.Base;
using CalculateVessels.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculateVessels.Core.Elements.Shells.Conical;

public class ConicalShellCalculated : ShellCalculated, ICalculatedElement
{
    public ConicalShellCalculated(ConicalShellCalculatedCommon commonData,
        IEnumerable<ConicalShellCalculatedOneLoading> results)
    {
        Bibliography = new[]
        {
            Data.Properties.Resources.GOST_34233_1,
            Data.Properties.Resources.GOST_34233_2
        };

        CommonData = commonData;
        Results = results;
    }

    public override string Type => nameof(ConicalShellCalculated);

    public override ConicalShellCalculatedCommon CommonData { get; }

    public override IEnumerable<ConicalShellCalculatedOneLoading> Results { get; } =
        new List<ConicalShellCalculatedOneLoading>();

    public override string ToString()
    {
        if (InputData is not ConicalShellInput dataIn) return "Conical shell";

        var builder = new StringBuilder();
        builder.Append("Conical shell - ");
        builder.Append($" D={dataIn.D} mm");
        builder.Append($" D1={dataIn.D1} mm");
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