using System;

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
