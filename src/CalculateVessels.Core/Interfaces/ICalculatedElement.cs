using System.Collections.Generic;

namespace CalculateVessels.Core.Interfaces;

public interface ICalculatedElement
{
    ICollection<string> ErrorList { get; }

    IInputData InputData { get; init; }
    IEnumerable<string> Bibliography { get; }

    //Type GetElementType();
}