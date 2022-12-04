namespace Module.RSA.Entities.Abstract;

public interface IRSAKeyPair
{
    IRSAKey Public { get; }
    IRSAKey Private { get; }
}