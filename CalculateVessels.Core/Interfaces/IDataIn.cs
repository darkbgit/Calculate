using System.Collections.Generic;

namespace CalculateVessels.Core.Interfaces
{
    public interface IDataIn
    {
        bool CheckData();

        public bool IsDataGood { get; }

        public List<string> ErrorList { get; }
    }

}
