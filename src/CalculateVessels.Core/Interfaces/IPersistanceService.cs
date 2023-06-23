using System.Collections.Generic;
using CalculateVessels.Core.Persistance.Enums;

namespace CalculateVessels.Core.Interfaces;

public interface IPersistanceService
{
    void Save(IEnumerable<ICalculatedElement> calculatedElements, string filePath, SaveType saveType);

    IEnumerable<ICalculatedElement> Open(string filePath, SaveType saveType);
}
