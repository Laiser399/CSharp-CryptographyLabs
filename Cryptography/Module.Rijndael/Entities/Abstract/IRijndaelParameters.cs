using Module.Rijndael.Enums;

namespace Module.Rijndael.Entities.Abstract;

public interface IRijndaelParameters
{
    IReadOnlyList<byte> Key { get; }
    RijndaelSize BlockSize { get; }
}