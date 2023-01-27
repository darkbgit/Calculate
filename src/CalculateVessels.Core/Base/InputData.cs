using System.Collections.Generic;
using System.Linq;
using CalculateVessels.Core.Interfaces;

namespace CalculateVessels.Core.Base;

public abstract class InputData
{
    public bool IsDataGood => !ErrorList.Any();

    public IEnumerable<string> ErrorList { get; } = new List<string>();

    public string Name { get; } = string.Empty;
}