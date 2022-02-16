using CalculateVessels.Core.Base;
using CalculateVessels.Core.Interfaces;

namespace CalculateVessels.Core.Supports.BracketVertical
{
    public class BracketVertical : Element, IElement
    {
        public BracketVertical(IInputData inputData)
        : base(inputData,
            new BracketVerticalCalculateProvider(),
            new BracketVerticalWordProvider())
        {
            Bibliography = new[]
            {
                Data.Properties.Resources.GOST_34233_1,
                Data.Properties.Resources.GOST_34233_5
            };
        }

        public override string ToString() => $"Опорные лапы вертикального аппарата {CalculatedData.InputData.Name}";
    }
}
