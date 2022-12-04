using System.Security.Cryptography;
using Module.Core.Cryptography.Abstract;
using Module.Core.Exceptions;

namespace Module.Core.Cryptography;

public class EcbDecryptParallelTransform : ICryptoTransform
{
    public bool CanReuseTransform => false;
    public bool CanTransformMultipleBlocks => true;
    public int InputBlockSize => _blockCryptoTransform.InputBlockSize;
    public int OutputBlockSize => _blockCryptoTransform.OutputBlockSize;

    private readonly IBlockCryptoTransform _blockCryptoTransform;

    private byte[]? _prevInputBlock;

    public EcbDecryptParallelTransform(IBlockCryptoTransform blockCryptoTransform)
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
                new Span<byte>(outputBuffer, outputOffset, OutputBlockSize)
            );
        }

        Parallel.For(
            0,
            blockCount - 1,
            new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount
            },
            i =>
            {
                _blockCryptoTransform.Transform(
                    new Span<byte>(
                        inputBuffer,
                        inputOffset + i * InputBlockSize,
                        InputBlockSize
                    ),
                    new Span<byte>(
                        outputBuffer,
                        outputOffset + (i + outputBlockOffset) * OutputBlockSize,
                        OutputBlockSize
                    )
                );
            }
        );

        Array.Copy(
            inputBuffer,
            inputOffset + (blockCount - 1) * InputBlockSize,
            _prevInputBlock,
            0,
            InputBlockSize
        );

        return (blockCount - 1 + outputBlockOffset) * OutputBlockSize;
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

        var output = new byte[OutputBlockSize];
        _blockCryptoTransform.Transform(_prevInputBlock, output);
        var finalBlockSize = output[^1];
        if (finalBlockSize >= output.Length)
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