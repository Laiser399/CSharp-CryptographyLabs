namespace Module.Rijndael.Services.Abstract;

public interface IRijndaelBlockTransformService
{
    void Encrypt(Span<byte> input, Span<byte> output);

    void Decrypt(Span<byte> input, Span<byte> output);
}