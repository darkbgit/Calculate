using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateVessels.Core.Exceptions
{
    public class CalculateException : Exception
    {
        public CalculateException()
        {

        }

        public CalculateException(string massage)
            : base(massage)
        {

        }

        public CalculateException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
