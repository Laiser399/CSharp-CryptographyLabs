using Module.RSA.Entities.Abstract;
using Module.RSA.Exceptions;

namespace Module.RSA.Services.Abstract;

public interface IRSATransformService
{
    /// <summary>
    /// Подразумевается, что данные могут поступить любые.
    /// </summary>
    /// <param name="data">Шифруемые данные</param>
    /// <param name="key">Ключ шифрования</param>
    /// <param name="progressCallback">Прогресс в диапазоне от 0 до 1</param>
    Task<byte[]> EncryptAsync(byte[] data, IRSAKey key, Action<double>? progressCallback = null);

    /// <summary>
    /// Подразумевается, что сюда поступают данные, возвращенные из <see cref="EncryptAsync"/>
    /// </summary>
    /// <param name="data">Дешифруемые данные</param>
    /// <param name="key">Ключ дешифрования</param>
    /// <param name="progressCallback">Прогресс в диапазоне от 0 до 1</param>
    /// <exception cref="CryptoTransformException">State of input file is invalid.</exception>
    Task<byte[]> DecryptAsync(byte[] data, IRSAKey key, Action<double>? progressCallback = null);
}