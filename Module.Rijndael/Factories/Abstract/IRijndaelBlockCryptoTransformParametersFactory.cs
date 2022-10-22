using Module.Rijndael.Entities.Abstract;
using Module.Rijndael.Enums;

namespace Module.Rijndael.Factories.Abstract;

public interface IRijndaelBlockCryptoTransformParametersFactory
{
    IRijndaelBlockCryptoTransformParameters Create(IRijndaelKey key, RijndaelSize blockSize);
}