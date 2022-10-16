using Module.Rijndael.Entities.Abstract;
using Module.Rijndael.Enums;

namespace Module.Rijndael.Factories.Abstract;

public interface IRijndaelParametersFactory
{
    IRijndaelParameters Create(IRijndaelKey key, RijndaelSize blockSize);
}