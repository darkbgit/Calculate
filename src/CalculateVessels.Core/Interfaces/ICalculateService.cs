namespace CalculateVessels.Core.Interfaces;

public interface ICalculateService<T>
    where T : class, IInputData
{
    ICalculatedElement<T> Calculate(T inputData);

    string Name { get; }
}
