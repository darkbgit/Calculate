using CalculateVessels.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace CalculateVessels.Core.Base;

public abstract class CalculatedElement
{
    public IEnumerable<string> Bibliography { get; set; } = Enumerable.Empty<string>();
    public ICollection<string> ErrorList { get; set; } = new List<string>();
    public required IInputData InputData { get; init; }
}