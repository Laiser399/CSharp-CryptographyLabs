namespace Module.RSA.Exceptions;

public class CryptographyAttackException : Exception
{
    public CryptographyAttackException(string message) : base(message)
    {
    }

    public CryptographyAttackException(string message, Exception innerException) : base(message, innerException)
    {
    }
}