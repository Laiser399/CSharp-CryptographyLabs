using Module.RSA.Entities.Abstract;

namespace Module.RSA.Entities;

public class RSAKeyPair : IRSAKeyPair
{
    public IRSAKey Public { get; }
    public IRSAKey Private { get; }

    public RSAKeyPair(IRSAKey publicKey, IRSAKey privateKey)
    {
        Public = publicKey;
        Private = privateKey;
    }
}