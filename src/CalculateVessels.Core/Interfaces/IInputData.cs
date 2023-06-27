using CalculateVessels.Core.Persistance.Json;
using System.Text.Json.Serialization;

namespace CalculateVessels.Core.Interfaces;

[JsonConverter(typeof(CustomJsonConverter<IInputData>))]
public interface IInputData
{
    //    bool IsDataGood { get; }
    //    ICollection<string> ErrorList { get; }
    string Name { get; }
    public string Type { get; }
}