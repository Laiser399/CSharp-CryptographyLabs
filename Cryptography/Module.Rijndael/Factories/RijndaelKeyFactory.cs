using Module.Rijndael.Entities.Abstract;
using Module.Rijndael.Enums;
using Module.Rijndael.Factories.Abstract;

namespace Module.Rijndael.Factories;

public class RijndaelKeyFactory : IRijndaelKeyFactory
{
    public IRijndaelKey Create(IReadOnlyList<byte> key)
    {
        return key.Count switch
        {
            16 => new RijndaelKey(RijndaelSize.S128, key),
            24 => new RijndaelKey(RijndaelSize.S192, key),
            32 => new RijndaelKey(RijndaelSize.S256, key),
            _ => throw new ArgumentException("Invalid length of key.", nameof(key))
        };
    }

    private class RijndaelKey : IRijndaelKey
    {
        public RijndaelSize Size { get; }
        public ReadOnlySpan<byte> Key => _key;

        private readonly byte[] _key;

        public RijndaelKey(RijndaelSize size, IReadOnlyList<byte> key)
        {
            Size = size;
            _key = key.ToArray();
        }
    }
}