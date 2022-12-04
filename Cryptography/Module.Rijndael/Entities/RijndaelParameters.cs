using Module.Rijndael.Entities.Abstract;
using Module.Rijndael.Enums;

namespace Module.Rijndael.Entities;

public class RijndaelParameters : IRijndaelParameters
{
    public IReadOnlyList<byte> Key { get; }
    public RijndaelSize BlockSize { get; }

    public RijndaelParameters(IReadOnlyList<byte> key, RijndaelSize blockSize)
    {
        Key = key;
        BlockSize = blockSize;
    }
}