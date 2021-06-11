using CalculateVessels.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateVessels.Core.Supports.Saddle
{
    public class Saddle : IElement
    {
        public bool IsCriticalError => throw new NotImplementedException();

        public bool IsError => throw new NotImplementedException();

        public List<string> ErrorList => throw new NotImplementedException();

        public void Calculate()
        {
            throw new NotImplementedException();
        }

        public void MakeWord(string filename)
        {
            throw new NotImplementedException();
        }
    }
}
