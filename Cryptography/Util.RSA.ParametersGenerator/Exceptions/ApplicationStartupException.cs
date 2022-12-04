namespace Util.RSA.ParametersGenerator.Exceptions;

public class ApplicationStartupException : Exception
{
    public ApplicationStartupException(string? message) : base(message)
    {
    }

    public ApplicationStartupException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}