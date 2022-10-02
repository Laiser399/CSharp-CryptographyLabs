using Module.RSA.Entities.Abstract;

namespace Module.RSA.Services.Abstract;

public interface IRSATransformService
{
    /// <summary>
    /// Подразумевается, что данные могут поступить любые.
    /// </summary>
    byte[] Encrypt(byte[] data, IRSAKey key);

    /// <summary>
    /// Подразумевается, что сюда поступают данные, возвращенные из <see cref="Encrypt"/>
    /// </summary>
    byte[] Decrypt(byte[] data, IRSAKey key);
}