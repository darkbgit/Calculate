using CalculateVessels.Core.Base;
using CalculateVessels.Core.Interfaces;

namespace CalculateVessels.Core.Bottoms.FlatBottom
{
    public class FlatBottom : Element, IElement
    {
        public FlatBottom(IInputData inputData)
        : base(inputData,
            new FlatBottomCalculateProvider(),
            new FlatBottomWordProvider())
        {

            Bibliography = new[]
            {
                Data.Properties.Resources.GOST_34233_1,
                Data.Properties.Resources.GOST_34233_2
            };
        }

        public override string ToString() => $"Плоское днище {CalculatedData.InputData.Name}";
    }
}
