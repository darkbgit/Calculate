using System.Collections.Generic;
using CalculateVessels.Core.Base;
using CalculateVessels.Core.Interfaces;

namespace CalculateVessels.Core;

public interface IPersistanceService
{
    void Save(IEnumerable<ICalculatedElement> calculatedElements, string filePath, SaveType saveType);

    IEnumerable<ICalculatedElement> Open(string filePath, SaveType saveType);
}
