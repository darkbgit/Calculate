using System.Collections.Generic;
using System.Linq;

namespace CalculateVessels.Core.Interfaces
{
    public interface IDataIn
    {
        public bool IsDataGood => !ErrorList.Any();

        public IEnumerable<string> ErrorList { get; }
    }

}
