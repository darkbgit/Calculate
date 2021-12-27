using System.Collections.Generic;

namespace CalculateVessels.Core.Interfaces
{
    public interface IElement
    {
        //void CheckInputData();

        void Calculate();

        void MakeWord(string filePath);

        bool IsCalculated { get; }

        //bool IsCriticalError { get; }

        //bool IsError { get; }

        //IEnumerable<string> ErrorList { get; }

        IEnumerable<string> Bibliography { get; }


    }

}
