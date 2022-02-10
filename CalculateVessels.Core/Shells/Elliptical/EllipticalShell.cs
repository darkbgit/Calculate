using CalculateVessels.Core.Exceptions;
using CalculateVessels.Core.Interfaces;
using System;

namespace CalculateVessels.Core.Shells.Elliptical
{

    public class EllipticalShell : Shell, IElement
    {
        private readonly IInputData _inputData;
        private readonly ICalculateProvider _calculateProvider;
        private readonly IWordProvider _wordProvider;

        private bool _isCalculated;

        private ICalculatedData _calculatedData;

        public EllipticalShell(IInputData inputData)
        {
            _inputData = inputData;
            _calculateProvider = new EllipticalShellCalculateProvider();
            _wordProvider = new EllipticalShellWordProvider();
        }

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
    }
}
