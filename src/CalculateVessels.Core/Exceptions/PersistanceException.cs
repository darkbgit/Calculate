using System;

namespace CalculateVessels.Core;

public class PersistanceException : Exception
{
    public PersistanceException()
    {

    }

    public PersistanceException(string massage)
        : base(massage)
    {

    }

    public PersistanceException(string message, Exception innerException)
        : base(message, innerException)
    {

    }
}
