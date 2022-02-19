using System;

namespace CalculateVessels.Data.PhysicalData
{
    public class PhysicalDataException : Exception
    {
        public PhysicalDataException()
        {

        }

        public PhysicalDataException(string massage)
            : base(massage)
        {

        }

        public PhysicalDataException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public PhysicalDataException(double maxTemperature)
        {
            MaxTemperatureError = true;
            MaxTemperature = maxTemperature;
        }

        public double MaxTemperature { get; }
        public bool MaxTemperatureError { get; }
    }
}