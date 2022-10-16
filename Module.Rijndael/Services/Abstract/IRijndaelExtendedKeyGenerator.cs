using Module.Rijndael.Entities.Abstract;
using Module.Rijndael.Enums;

namespace Module.Rijndael.Services.Abstract;

public interface IRijndaelExtendedKeyGenerator
{
    byte[] Generate(IRijndaelKey key, RijndaelSize blockSize);
}