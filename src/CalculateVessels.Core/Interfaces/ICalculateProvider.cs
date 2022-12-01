using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateVessels.Core.Interfaces
{
    public interface ICalculateProvider
    {
        ICalculatedData Calculate(IInputData inputData);
    }
}
