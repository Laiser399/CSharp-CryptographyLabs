using System.Security.Cryptography;
using Module.Core.Cryptography.Abstract;
using Module.Core.Exceptions;
using Module.Core.Services.Abstract;

namespace Module.Core.Cryptography;

public class CfbDecryptTransform : ICryptoTransform
{
    public bool CanReuseTransform => false;
    public bool CanTransformMultipleBlocks => false;
    public int InputBlockSize => _blockCryptoTransform.InputBlockSize;
    public int OutputBlockSize => _blockCryptoTransform.OutputBlockSize;

    private readonly IBlockCryptoTransform _blockCryptoTransform;
    private readonly IXorService _xorService;
    private readonly byte[] _encryptionVector;

    private byte[]? _prevInputBlock;

    public CfbDecryptTransform(IBlockCryptoTransform blockCryptoTransform, byte[] initialVector, IXorService xorService)
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

        _encryptionVector = new byte[InputBlockSize];
        initialVector.CopyTo(_encryptionVector, 0);
    }

    public int TransformBlock(
        byte[] inputBuffer,
        int inputOffset,
        int inputCount,
        byte[] outputBuffer,
        int outputOffset)
    {
        if (inputCount != InputBlockSize)
        {
            return 0;
        }

        var input = new Span<byte>(inputBuffer, inputOffset, InputBlockSize);

        if (_prevInputBlock is null)
        {
            _prevInputBlock = new byte[InputBlockSize];
            input.CopyTo(_prevInputBlock);
            return 0;
        }

        var output = new Span<byte>(outputBuffer, outputOffset, InputBlockSize);

        _blockCryptoTransform.Transform(_encryptionVector, output);

        _xorService.Xor(output, _prevInputBlock, output);

        _prevInputBlock.CopyTo(_encryptionVector, 0);

        input.CopyTo(_prevInputBlock);

        return InputBlockSize;
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

        _blockCryptoTransform.Transform(_encryptionVector, output);

        _xorService.Xor(output, _prevInputBlock, output);

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