namespace Util.RSA.WienerAttackTest.Exceptions;

public class PerformAttackException : Exception
{
    public PerformAttackException(string? message) : base(message)
    {
    }

    public PerformAttackException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}