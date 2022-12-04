using Module.Rijndael.Entities.Abstract;
using Module.Rijndael.Enums;

namespace Module.Rijndael.Services.Abstract;

public interface IRijndaelExtendedKeyGenerator
{
    /// <returns>
    /// На выходе вначале идет базовый ключ, затем - его расширение.
    /// Размер на выходе соответствует количеству раундов на 1 больше необходимого (включая 1-й ключ, который используется до самих раундов).
    /// </returns>
    byte[] Generate(IRijndaelKey key, RijndaelSize blockSize);
}