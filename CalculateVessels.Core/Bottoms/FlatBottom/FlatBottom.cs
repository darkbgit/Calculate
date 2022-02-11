using CalculateVessels.Core.Exceptions;
using CalculateVessels.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace CalculateVessels.Core.Bottoms.FlatBottom
{
    public class FlatBottom : IElement
    {
        private readonly IInputData _inputData;
        private readonly ICalculateProvider _calculateProvider;
        private readonly IWordProvider _wordProvider;

        private bool _isCalculated;

        public FlatBottom(IInputData inputData)
        {
            _inputData = inputData;
            _calculateProvider = new FlatBottomCalculateProvider();
            _wordProvider = new FlatBottomWordProvider();

            Bibliography = new[]
            {
                Data.Properties.Resources.GOST_34233_1,
                Data.Properties.Resources.GOST_34233_2
            };
        }

        private ICalculatedData _calculatedData;

        public bool IsCalculated => _isCalculated;


        public ICalculatedData CalculatedData
        {
            get => _isCalculated ? _calculatedData : null;
            private set => _calculatedData = value;
        }

        public IEnumerable<string> Bibliography { get; }

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

        public override string ToString() => $"Плоское днище {_inputData.Name}";
    }
}
