using CalculateVessels.Core.Bottoms.Enums;
using CalculateVessels.Core.HeatExchanger.Enums;
using CalculateVessels.Core.Interfaces;
using CalculateVessels.Core.Word;
using CalculateVessels.Core.Word.Enums;
using CalculateVessels.Data.PhysicalData;
using CalculateVessels.Data.PhysicalData.Enums;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageMagick;
using System.Security.Cryptography;
using CalculateVessels.Core.Exceptions;


namespace CalculateVessels.Core.HeatExchanger
{
    public class HeatExchanger : IElement
    {
        private readonly IInputData _inputData;
        private readonly ICalculateProvider _calculateProvider;
        private readonly IWordProvider _wordProvider;

        private bool _isCalculated;

        public HeatExchanger(IInputData inputData)
        {
            _inputData = inputData;
            _calculateProvider = new HeatExchangerCalculateProvider();
            _wordProvider = new HeatExchangerWordProvider();
        }


        private ICalculatedData _calculatedData;

        public bool IsCalculated => _isCalculated;


        public ICalculatedData CalculatedData
        {
            get => _isCalculated ? _calculatedData : null;
            private set => _calculatedData = value;
        }

        public void Calculate()
        {
            if (!_inputData.IsDataGood)
                throw new CalculateException("Error. Wrong input data.");

            CalculatedData = _calculateProvider.Calculate(_inputData);
            _isCalculated = true;
        }

        public void MakeWord(string filePath)
        {
            if (!IsCalculated)
                throw new ArgumentException();

            _wordProvider.MakeWord(filePath, CalculatedData);
        }

        public override string ToString() => $"Теплообменный аппарат с неподвижными трубными решетками {_inputData.Name}";
 
        public IEnumerable<string> Bibliography { get; } = new []
        {
            Data.Properties.Resources.GOST_34233_1,
            Data.Properties.Resources.GOST_34233_6,
            Data.Properties.Resources.GOST_34233_7
        };
    }
}
