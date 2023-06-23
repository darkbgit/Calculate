using System.Collections.Generic;
using System.Text.Json.Serialization;
using CalculateVessels.Core.Persistance.Json;

namespace CalculateVessels.Core.Interfaces;

[JsonConverter(typeof(CustomJsonConverter<IInputData>))]
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