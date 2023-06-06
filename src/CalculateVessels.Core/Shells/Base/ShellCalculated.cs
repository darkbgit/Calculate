using CalculateVessels.Core.Base;
using System.Collections.Generic;

namespace CalculateVessels.Core.Shells.Base;

public abstract class ShellCalculated : CalculatedElement
{
    public virtual ShellCalculatedCommonData CommonData { get; }

    public virtual IEnumerable<ShellCalculatedOneLoadingData> Results { get; }
}