using CalculateVessels.Core.Interfaces;
using System;
using System.Collections.Generic;
using CalculateVessels.Core.Exceptions;

namespace CalculateVessels.Core.Supports.BracketVertical
{
    public class BracketVertical : IElement
    {
        private readonly IInputData _inputData;

        private readonly ICalculateProvider _calculateProvider;

        private readonly IWordProvider _wordProvider;

        public BracketVertical(IInputData inputData)
        {
            _inputData = inputData;

            _calculateProvider = new BracketVerticalCalculateProvider();

            _wordProvider = new BracketVerticalWordProvider();

            Bibliography = new[]
            {
                Data.Properties.Resources.GOST_34233_1,
                Data.Properties.Resources.GOST_34233_5
            };
        }

        public ICalculatedData CalculatedData { get; private set; }
        public IEnumerable<string> Bibliography { get; }

        public bool IsCalculated { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="CalculateException"></exception>
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
                throw new ArgumentException();

            _wordProvider.MakeWord(filePath, CalculatedData);
        }
    }
}
