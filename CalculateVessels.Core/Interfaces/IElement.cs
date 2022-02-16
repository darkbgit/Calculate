using System.Collections.Generic;
using CalculateVessels.Core.Exceptions;

namespace CalculateVessels.Core.Interfaces
{
    public interface IElement
    {
        /// <summary>
        /// Calculate element.
        /// </summary>
        /// <exception cref="CalculateException"></exception>
        void Calculate();

        /// <summary>
        /// Make word file on filePath if element was calculated.
        /// </summary>
        /// <param name="filePath"></param>
        /// <exception cref="MakeWordException"></exception>
        void MakeWord(string filePath);

        bool IsCalculated { get; }
        
        /// <summary>
        /// Returns null if element isn't calculated.
        /// </summary>
        ICalculatedData CalculatedData { get; }

        IEnumerable<string> Bibliography { get; }
    }
}
