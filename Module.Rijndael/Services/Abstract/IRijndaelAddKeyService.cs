namespace Module.Rijndael.Services.Abstract;

public interface IRijndaelAddKeyService
{
    void AddKey(Span<byte> state, ReadOnlySpan<byte> key);
}