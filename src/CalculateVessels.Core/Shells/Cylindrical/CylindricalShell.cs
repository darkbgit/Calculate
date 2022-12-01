using CalculateVessels.Core.Base;
using CalculateVessels.Core.Interfaces;
using System.Collections.Generic;

namespace CalculateVessels.Core.Shells.Cylindrical
{
    public class CylindricalShell : Element, IElement
    {
        public CylindricalShell(IInputData inputData) :
            base(inputData,
                new CylindricalShellCalculateProvider(),
                new CylindricalShellWordProvider())
        {
            Bibliography = new[]
            {
                Data.Properties.Resources.GOST_34233_1,
                Data.Properties.Resources.GOST_34233_2
            };
        }

        public override string ToString() => $"Цилиндрическая обечайка {CalculatedData.InputData.Name}";
    }
}
