using System.Collections.Generic;

namespace CalculateVessels.Core.Interfaces
{
    public interface IDataIn
    {

        public bool IsDataGood { get; }

        public List<string> ErrorList { get; }
    }

}
