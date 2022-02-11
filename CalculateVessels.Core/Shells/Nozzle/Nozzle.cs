using CalculateVessels.Core.Exceptions;
using CalculateVessels.Core.Interfaces;
using System;
using System.Collections.Generic;


namespace CalculateVessels.Core.Shells.Nozzle
{
    public class Nozzle : IElement
    {
        private readonly IInputData _inputData;
        private readonly ICalculateProvider _calculateProvider;
        private readonly IWordProvider _wordProvider;

        private bool _isCalculated;

        public Nozzle(IInputData inputData)
        {
            _inputData = inputData;
            _calculateProvider = new NozzleCalculateProvider();
            _wordProvider = new NozzleWordProvider();
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

        public IEnumerable<string> Bibliography { get; } = new List<string>()
        {
            Data.Properties.Resources.GOST_34233_3
        };
    }
}
