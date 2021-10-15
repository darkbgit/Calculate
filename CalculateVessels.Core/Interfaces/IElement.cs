using System.Collections.Generic;

namespace CalculateVessels.Core.Interfaces
{
    public interface IElement
    {
        //void CheckInputData();

        void Calculate();

        void MakeWord(string filename);

        string ToString();

        bool IsCriticalError { get; }

        bool IsError { get; }

        IEnumerable<string> ErrorList { get; }

        IEnumerable<string> Bibliography { get; }


    }

}
