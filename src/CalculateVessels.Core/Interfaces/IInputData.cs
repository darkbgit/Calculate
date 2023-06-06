using System.Collections.Generic;

namespace CalculateVessels.Core.Interfaces;

public interface IInputData
{
    bool IsDataGood { get; }
    ICollection<string> ErrorList { get; }
    string Name { get; }

    ///// <summary>
    ///// Check if input data consist any errors.
    ///// </summary>
    ///// <returns></returns>
    //IEnumerable<string> GetInputDataErrors();
}