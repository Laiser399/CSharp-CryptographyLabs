using Module.Rijndael.Enums;

namespace Module.Rijndael.Entities.Abstract;

public interface IRijndaelKey
{
    RijndaelSize Size { get; }
    ReadOnlySpan<byte> Key { get; }
}