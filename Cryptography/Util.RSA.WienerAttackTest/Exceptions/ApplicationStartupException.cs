namespace Util.RSA.WienerAttackTest.Exceptions;

public class ApplicationStartupException : Exception
{
    public ApplicationStartupException(string? message) : base(message)
    {
    }

    public ApplicationStartupException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}