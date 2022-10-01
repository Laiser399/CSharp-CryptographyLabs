using Module.RSA.Entities.Abstract;

namespace Module.RSA.Services.Abstract;

public interface IRSATransformService
{
    byte[] Transform(byte[] data, IRSAKey key);
}