using System.Collections.Generic;
using System.Text.Json.Serialization;
using CalculateVessels.Core.Persistance.Json;

namespace CalculateVessels.Core.Interfaces;

[JsonConverter(typeof(CustomJsonConverter<ICalculatedElement>))]
public interface ICalculatedElement
{
    ICollection<string> ErrorList { get; }
    IInputData InputData { get; init; }
    IEnumerable<string> Bibliography { get; }
}