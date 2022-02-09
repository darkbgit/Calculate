using System.Collections.Generic;
using CalculateVessels.Core.Exceptions;

namespace CalculateVessels.Core.Interfaces
{
    public interface IElement
    {
        //void CheckInputData();

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="CalculateException"></exception>
        void Calculate();

        void MakeWord(string filePath);

        bool IsCalculated { get; }
        
        /// <summary>
        /// Returns null if element isn't calculated
        /// </summary>
        ICalculatedData CalculatedData { get; }

        //bool IsCriticalError { get; }

        //bool IsError { get; }

        //IEnumerable<string> ErrorList { get; }

        IEnumerable<string> Bibliography { get; }


    }

}
