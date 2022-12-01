using CalculateVessels.Core.Base;
using CalculateVessels.Core.Interfaces;
using System.Collections.Generic;

namespace CalculateVessels.Core.Shells.Elliptical
{
    public class EllipticalShell : Element, IElement
    {
        public EllipticalShell(IInputData inputData)
        : base(inputData,
            new EllipticalShellCalculateProvider(),
            new EllipticalShellWordProvider())
        {
            Bibliography = new[]
            {
                Data.Properties.Resources.GOST_34233_1,
                Data.Properties.Resources.GOST_34233_2
            };
        }

        public override string ToString() => $"Эллиптическая обечайка {CalculatedData.InputData.Name}";
    }
}
