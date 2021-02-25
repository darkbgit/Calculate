using System.Collections.Generic;

namespace calcNet
{
    public interface IElement
    {
        //void CheckInputData();

        void Calculate();

        void MakeWord(string filename);

        bool IsCriticalError { get; }

        bool IsError { get; }

        List<string> ErrorList { get; }
    }

}
