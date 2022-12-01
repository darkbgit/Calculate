using CalculateVessels.Core.Base;
using CalculateVessels.Core.Interfaces;

namespace CalculateVessels.Core.Bottoms.FlatBottomWithAdditionalMoment
{
    public class FlatBottomWithAdditionalMoment : Element, IElement
    {
        public FlatBottomWithAdditionalMoment(IInputData inputData)
        : base(inputData,
            new FlatBottomWithAdditionalMomentCalculateProvider(),
            new FlatBottomWithAdditionalMomentWordProvider())
        {
            Bibliography = new[]
            {
                Data.Properties.Resources.GOST_34233_1,
                Data.Properties.Resources.GOST_34233_2
            };
        }

        public override string ToString() => $"Плоское днище с дополнительным краевым моментом {CalculatedData.InputData.Name}";
    }
}
