using System;

namespace CalculateVessels.Core.Exceptions
{
    public class MakeWordException : Exception
    {
        public MakeWordException()
        {

        }

        public MakeWordException(string massage)
            : base(massage)
        {

        }

        public MakeWordException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}