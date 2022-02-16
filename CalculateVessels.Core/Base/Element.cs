using System;
using System.Collections.Generic;
using CalculateVessels.Core.Exceptions;
using CalculateVessels.Core.Interfaces;

namespace CalculateVessels.Core.Base
{
    public abstract class Element
    {
        private readonly IInputData _inputData;
        private readonly ICalculateProvider _calculateProvider;
        private readonly IWordProvider _wordProvider;
        
        private ICalculatedData _calculatedData;

        protected Element(IInputData inputData, ICalculateProvider calculateProvider, IWordProvider wordProvider)
        {
            _inputData = inputData;
            _calculateProvider = calculateProvider;
            _wordProvider = wordProvider;
        }

        public bool IsCalculated { get; private set; }

        public ICalculatedData CalculatedData
        {
            get => IsCalculated ? _calculatedData : null;
            private set => _calculatedData = value;
        }

        public void Calculate()
        {
            if (!_inputData.IsDataGood)
                throw new CalculateException("Error. Wrong input data.");

            CalculatedData = _calculateProvider.Calculate(_inputData);
            IsCalculated = true;
        }

        public void MakeWord(string filePath)
        {
            if (!IsCalculated)
                throw new MakeWordException("Element didn't calculate.");

            _wordProvider.MakeWord(filePath, CalculatedData);
        }

        public IEnumerable<string> Bibliography { get; init; }
    }
}