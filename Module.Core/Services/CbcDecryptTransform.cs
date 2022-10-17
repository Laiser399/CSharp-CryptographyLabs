using System.Security.Cryptography;
using Module.Core.Exceptions;
using Module.Core.Services.Abstract;

namespace Module.Core.Services;

public class CbcDecryptTransform : ICryptoTransform
{
    public bool CanReuseTransform => false;
    public bool CanTransformMultipleBlocks => true;
    public int InputBlockSize => _blockCryptoTransform.InputBlockSize;
    public int OutputBlockSize => _blockCryptoTransform.OutputBlockSize;

    private readonly IBlockCryptoTransform _blockCryptoTransform;
    private readonly IXorService _xorService;
    private readonly byte[] _xorVector;

    private byte[]? _prevInputBlock;

    /// <exception cref="ArgumentException">Invalid transform block sizes or initial vector size.</exception>
    public CbcDecryptTransform(IBlockCryptoTransform blockCryptoTransform, byte[] initialVector, IXorService xorService)
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

        var outputBlockOffset = _prevInputBlock is null
            ? 0
            : 1;

        if (_prevInputBlock is null)
        {
            _prevInputBlock = new byte[InputBlockSize];
        }
        else
        {
            _blockCryptoTransform.Transform(
                _prevInputBlock,
                new Span<byte>(outputBuffer, outputOffset, InputBlockSize)
            );
        }

        for (var i = 0; i < blockCount - 1; i++)
        {
            var input = new Span<byte>(inputBuffer, inputOffset + i * InputBlockSize, InputBlockSize);
            var output = new Span<byte>(
                outputBuffer,
                outputOffset + (i + outputBlockOffset) * InputBlockSize,
                InputBlockSize
            );

            _blockCryptoTransform.Transform(input, output);

            _xorService.Xor(output, _xorVector, output);

            input.CopyTo(_xorVector);
        }

        Array.Copy(
            inputBuffer,
            inputOffset + (blockCount - 1) * InputBlockSize,
            _prevInputBlock,
            0,
            InputBlockSize
        );

        return (blockCount - 1 + outputBlockOffset) * InputBlockSize;
    }

    public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
    {
        if (inputCount != 0)
        {
            throw new CryptoTransformException("Invalid final block state.");
        }

        if (_prevInputBlock is null)
        {
            return Array.Empty<byte>();
        }

        var output = new byte[InputBlockSize];

        _blockCryptoTransform.Transform(_prevInputBlock, output);

        _xorService.Xor(output, _xorVector, output);

        var finalBlockSize = output[^1];
        if (finalBlockSize >= InputBlockSize)
        {
            throw new CryptoTransformException("Invalid final block state.");
        }

        Array.Resize(ref output, finalBlockSize);

        return output;
    }

    public void Dispose()
    {
    }
}