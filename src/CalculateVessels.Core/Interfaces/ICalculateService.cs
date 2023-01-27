using System;
using CalculateVessels.Core.Base;

namespace CalculateVessels.Core.Interfaces;

public interface ICalculateService<T>
    where T : class, IInputData
{
    ICalculatedElement Calculate(T inputData);
}
