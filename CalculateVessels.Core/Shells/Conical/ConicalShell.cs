using CalculateVessels.Core.Base;
using CalculateVessels.Core.Interfaces;
using System.Collections.Generic;

namespace CalculateVessels.Core.Shells.Conical
{
    public class ConicalShell : Element, IElement
    {
        public ConicalShell(IInputData inputData)
        : base(inputData,
            new ConicalShellCalculateProvider(),
            new ConicalShellWordProvider())
        {
            Bibliography = new[]
            {
                Data.Properties.Resources.GOST_34233_1,
                Data.Properties.Resources.GOST_34233_2
            };
        }

        public override string ToString() => $"Коническая обечайка {CalculatedData.InputData.Name}";
    }
}
