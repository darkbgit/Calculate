using System.Collections.Generic;
using System.Linq;
using CalculateVessels.Core.Interfaces;

namespace CalculateVessels.Core.Elements.Base;

public abstract class CalculatedElement
{
    private readonly List<string> _errorList = new();

    public virtual string Type => string.Empty;

    public IEnumerable<string> Bibliography { get; init; } = Enumerable.Empty<string>();

    public ICollection<string> ErrorList => _errorList;

    //public required IInputData InputData { get;  init; }

    public IInputData InputData { get; init; }

    internal void AddErrors(IEnumerable<string> errors)
    {
        _errorList.AddRange(errors);
    }
}