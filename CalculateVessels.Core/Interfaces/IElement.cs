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

        List<string> ErrorList { get; }

        List<string> Bibliography { get; }
    }

}
