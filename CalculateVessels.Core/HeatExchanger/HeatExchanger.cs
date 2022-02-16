using CalculateVessels.Core.Base;
using CalculateVessels.Core.Interfaces;


namespace CalculateVessels.Core.HeatExchanger
{
    public class HeatExchanger : Element, IElement
    {
        public HeatExchanger(IInputData inputData)
        : base(inputData,
            new HeatExchangerCalculateProvider(),
            new HeatExchangerWordProvider())
        {
            Bibliography = new[]
            {
                Data.Properties.Resources.GOST_34233_1,
                Data.Properties.Resources.GOST_34233_6,
                Data.Properties.Resources.GOST_34233_7
            };
        }

        public override string ToString() => $"Теплообменный аппарат с неподвижными трубными решетками {CalculatedData.InputData.Name}";
    }
}
