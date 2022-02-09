using System.Collections.Generic;

namespace CalculateVessels.Core.Interfaces
{
    public interface ICalculatedData
    {
        ICollection<string> ErrorList { get; }
    }
}
