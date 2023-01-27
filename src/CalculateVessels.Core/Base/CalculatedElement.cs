using CalculateVessels.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace CalculateVessels.Core.Base;

public abstract class CalculatedElement
{
    public IEnumerable<string> Bibliography { get; } = Enumerable.Empty<string>();
    public ICollection<string> ErrorList { get; set; } = new List<string>();
    public IInputData InputData { get; init; }
}