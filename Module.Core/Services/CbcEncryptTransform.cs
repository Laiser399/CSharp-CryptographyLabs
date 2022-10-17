using System.Security.Cryptography;
using Module.Core.Services.Abstract;

namespace Module.Core.Services;

public class CbcEncryptTransform : ICryptoTransform
{
    public bool CanReuseTransform => false;
    public bool CanTransformMultipleBlocks => true;
    public int InputBlockSize => _blockCryptoTransform.InputBlockSize;
    public int OutputBlockSize => _blockCryptoTransform.OutputBlockSize;

    private readonly IBlockCryptoTransform _blockCryptoTransform;
    private readonly IXorService _xorService;
    private readonly byte[] _xorVector;

    /// <exception cref="ArgumentException">Invalid transform block sizes or initial vector size.</exception>
    public CbcEncryptTransform(IBlockCryptoTransform blockCryptoTransform, byte[] initialVector, IXorService xorService)
    {
        if (blockCryptoTransform.InputBlockSize != blockCryptoTransform.OutputBlockSize)
        {
            throw new ArgumentException(
                "Transform with different input and output block sizes is not supported",
                nameof(blockCryptoTransform)
            );
        }

        if (blockCryptoTransform.InputBlockSize != initialVector.Length)
        {
            throw new ArgumentException(
                "Initial vector size is not equal transform block size.",
                nameof(initialVector)
            );
        }

        _blockCryptoTransform = blockCryptoTransform;
        _xorService = xorService;

        _xorVector = new byte[InputBlockSize];
        Array.Copy(initialVector, _xorVector, InputBlockSize);
    }

    public int TransformBlock(
        byte[] inputBuffer,
        int inputOffset,
        int inputCount,
        byte[] outputBuffer,
        int outputOffset)
    {
        var blockCount = inputCount / InputBlockSize;
        if (blockCount == 0)
        {
            return 0;
        }

        var buffer = new byte[InputBlockSize];

        for (var i = 0; i < blockCount; i++)
        {
            var input = new Span<byte>(inputBuffer, inputOffset + i * InputBlockSize, InputBlockSize);
            _xorService.Xor(input, _xorVector, buffer);

            var output = new Span<byte>(outputBuffer, outputOffset + i * InputBlockSize, InputBlockSize);
            _blockCryptoTransform.Transform(buffer, output);

            output.CopyTo(_xorVector);
        }

        return blockCount * InputBlockSize;
    }

    public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
    {
        inputBuffer[inputOffset + InputBlockSize - 1] = (byte)inputCount;

        var input = new Span<byte>(inputBuffer, inputOffset, InputBlockSize);
        var output = new byte[InputBlockSize];

        _xorService.Xor(input, _xorVector, input);

        _blockCryptoTransform.Transform(input, output);

        return output;
    }

    public void Dispose()
    {
    }
}