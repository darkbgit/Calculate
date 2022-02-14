using CalculateVessels.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalculateVessels.Core.Bottoms.Enums;
using CalculateVessels.Data.PhysicalData;
using DocumentFormat.OpenXml.Packaging;
using CalculateVessels.Core.Word;
using CalculateVessels.Core.Word.Enums;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using CalculateVessels.Core.Exceptions;
using DocumentFormat.OpenXml.Wordprocessing;

namespace CalculateVessels.Core.Bottoms.FlatBottomWithAdditionalMoment
{
    public class FlatBottomWithAdditionalMoment : IElement
    {
        private readonly IInputData _inputData;
        private readonly ICalculateProvider _calculateProvider;
        private readonly IWordProvider _wordProvider;

        private bool _isCalculated;

        public FlatBottomWithAdditionalMoment(IInputData inputData)
        {
            _inputData = inputData;
            _calculateProvider = new FlatBottomWithAdditionalMomentCalculateProvider();
            _wordProvider = new FlatBottomWithAdditionalMomentWordProvider();
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

        public override string ToString() => $"Плоское днище с дополнительным краевым моментом {_inputData.Name}";

        public IEnumerable<string> Bibliography { get; } = new []
        {
            Data.Properties.Resources.GOST_34233_1,
            Data.Properties.Resources.GOST_34233_2
        };
    }
}
