using CalculateVessels.Core.Interfaces;

namespace CalculateVessels.Core.Models
{
    public class CalculatedElement
    {
        public IElement Element { get; }

        private readonly ICalculateProvider _provider;


        public CalculatedElement(IElement element)
        {
            Element = element;
        }

        //public CalculatedElement(IElement element, ICalculateProvider calculateProvider, IDataIn dataIn)
        //{
        //    Element = element;
        //    _provider = calculateProvider;
        //    _provider.Calculate(dataIn);
        //}
    }
}
