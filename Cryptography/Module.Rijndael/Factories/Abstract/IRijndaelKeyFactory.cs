using Module.Rijndael.Entities.Abstract;

namespace Module.Rijndael.Factories.Abstract;

public interface IRijndaelKeyFactory
{
    /// <exception cref="ArgumentException">Invalid length of key.</exception>
    IRijndaelKey Create(IReadOnlyList<byte> key);
}