using CalculateVessels.Core.Base;
using CalculateVessels.Core.Interfaces;

namespace CalculateVessels.Core.Supports.Saddle
{
    public class Saddle : Element, IElement
    {
        public Saddle(IInputData inputData)
        : base(inputData,
            new SaddleCalculateProvider(),
            new SaddleWordProvider())
        {
            Bibliography = new[]
            {
                Data.Properties.Resources.GOST_34233_1,
                Data.Properties.Resources.GOST_34233_5
            };
        }

        public override string ToString() => $"Седловая опора {CalculatedData.InputData.Name}";
    }
}
