namespace Module.RSA.Exceptions;

public class FactorizationException : Exception
{
    public FactorizationException(string message) : base(message)
    {
    }

    public FactorizationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}