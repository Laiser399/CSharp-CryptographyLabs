using System.Security.Cryptography;
using Module.Core.Services.Abstract;

namespace Module.Core.Services;

public class EcbEncryptTransform : ICryptoTransform
{
    public bool CanReuseTransform => true;
    public bool CanTransformMultipleBlocks => true;
    public int InputBlockSize => _blockCryptoTransform.InputBlockSize;
    public int OutputBlockSize => _blockCryptoTransform.OutputBlockSize;

    private readonly IBlockCryptoTransform _blockCryptoTransform;

    public EcbEncryptTransform(IBlockCryptoTransform blockCryptoTransform)
    {
        _blockCryptoTransform = blockCryptoTransform;
    }

    public int TransformBlock(
        byte[] inputBuffer,
        int inputOffset,
        int inputCount,
        byte[] outputBuffer,
        int outputOffset)
    {
        var blockCount = inputCount / InputBlockSize;

        for (var i = 0; i < blockCount; i++)
        {
            _blockCryptoTransform.Transform(
                new Span<byte>(inputBuffer, inputOffset + i * InputBlockSize, InputBlockSize),
                new Span<byte>(outputBuffer, outputOffset + i * OutputBlockSize, OutputBlockSize)
            );
        }

        return blockCount * OutputBlockSize;
    }

    public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
    {
        inputBuffer[InputBlockSize - 1] = (byte)inputCount;
        var output = new byte[OutputBlockSize];
        _blockCryptoTransform.Transform(
            new Span<byte>(inputBuffer, inputOffset, InputBlockSize),
            new Span<byte>(output)
        );
        return output;
    }

    public void Dispose()
    {
    }
}