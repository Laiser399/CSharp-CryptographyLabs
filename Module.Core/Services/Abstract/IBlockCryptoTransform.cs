namespace Module.Core.Services.Abstract;

public interface IBlockCryptoTransform
{
    int InputBlockSize { get; }
    int OutputBlockSize { get; }

    /// <exception cref="ArgumentException">Invalid size of input or output.</exception>
    void Transform(Span<byte> input, Span<byte> output);
}