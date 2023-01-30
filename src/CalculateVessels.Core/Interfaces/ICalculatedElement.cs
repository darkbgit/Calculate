using System.Collections.Generic;

namespace CalculateVessels.Core.Interfaces;

public interface ICalculatedElement<T>
 where T : class, IInputData
{
    ICollection<string> ErrorList { get; }

    T InputData { get; init; }
    IEnumerable<string> Bibliography { get; }

    //Type GetElementType();
}