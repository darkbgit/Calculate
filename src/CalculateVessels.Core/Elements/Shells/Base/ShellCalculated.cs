using System.Collections.Generic;
using CalculateVessels.Core.Elements.Base;

namespace CalculateVessels.Core.Elements.Shells.Base;

public abstract class ShellCalculated : CalculatedElement
{
    public abstract ShellCalculatedCommonData CommonData { get; }

    public abstract IEnumerable<ShellCalculatedOneLoadingData> Results { get; }
}