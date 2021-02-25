using System.Collections.Generic;

namespace calcNet
{
    public interface IDataIn
    {
        void CheckData();

        public bool IsDataGood { get;  }

        public List<string> ErrorList { get; }
    }

}
