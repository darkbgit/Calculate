using CalculateVessels.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateVessels.Models
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
