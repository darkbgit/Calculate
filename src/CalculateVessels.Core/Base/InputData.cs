using System.Collections.Generic;
using System.Linq;

namespace CalculateVessels.Core.Base;

public abstract class InputData
{
    public virtual bool IsDataGood => !ErrorList.Any();

    public ICollection<string> ErrorList { get; } = new List<string>();

    public string Name { get; set; } = string.Empty;
}