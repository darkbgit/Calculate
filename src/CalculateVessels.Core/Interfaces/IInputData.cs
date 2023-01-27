using System.Collections.Generic;
using System.Linq;

namespace CalculateVessels.Core.Interfaces;

public interface IInputData
{
    bool IsDataGood => !ErrorList.Any();

    IEnumerable<string> ErrorList { get; }

    string Name { get; }
}