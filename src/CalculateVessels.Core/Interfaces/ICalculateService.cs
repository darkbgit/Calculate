namespace CalculateVessels.Core.Interfaces;

public interface ICalculateService<in T>
    where T : class, IInputData
{
    ICalculatedElement Calculate(T inputData);

    string Name { get; }
}
