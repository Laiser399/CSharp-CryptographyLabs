namespace Module.RSA.Exceptions;

public class CryptoTransformException : Exception
{
    public CryptoTransformException(string message) : base(message)
    {
    }

    public CryptoTransformException(string message, Exception innerException) : base(message, innerException)
    {
    }
}