using CalculateVessels.Core.Exceptions;
using CalculateVessels.Core.Interfaces;
using System;
using System.Collections.Generic;
using CalculateVessels.Core.Base;


namespace CalculateVessels.Core.Shells.Nozzle
{
    public class Nozzle : Element, IElement
    {
        public Nozzle(IInputData inputData)
        : base(inputData,
            new NozzleCalculateProvider(),
            new NozzleWordProvider())
        {
            Bibliography = new[]
            {
                Data.Properties.Resources.GOST_34233_3
            };
        }

        public override string ToString() => $"Штуцер {CalculatedData.InputData.Name}";
    }
}
