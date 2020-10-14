using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Crypto
{
    // CBC
    public class CipherBlockChainingTransform : ICryptoTransform
    {
        public int InputBlockSize => _insideCryptoTransform.InputBlockSize;

        public int OutputBlockSize => _insideCryptoTransform.OutputBlockSize;

        public bool CanTransformMultipleBlocks => true;

        public bool CanReuseTransform => false;

        private delegate void BlockTransformFunc(byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset);

        private ICryptoTransform _insideCryptoTransform;
        private byte[] _prevBlock;
        private BlockTransformFunc _transformFunc;

        // TODO bool -> enum
        public CipherBlockChainingTransform(ICryptoTransform blockCryptoTransform, bool encryption = true)
        {
            _insideCryptoTransform = blockCryptoTransform;

            _prevBlock = new byte[InputBlockSize];// TODO fill with something
            for (int i = 0; i < InputBlockSize; ++i)// TODO del mb
                _prevBlock[i] = 0;

            if (encryption)
                _transformFunc = EncryptBlock;
            else
                _transformFunc = DecryptBlock;
        }

        public void Dispose()
        {

        }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            for (int i = 0; i < inputCount / InputBlockSize; ++i)
                _transformFunc(inputBuffer, inputOffset + i * InputBlockSize, outputBuffer, outputOffset + i * InputBlockSize);
            return inputCount;
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            byte[] result = new byte[inputCount];
            TransformBlock(inputBuffer, inputOffset, inputCount, result, 0);
            return result;
        }

        private void EncryptBlock(byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset)
        {
            for (int j = 0; j < InputBlockSize; ++j)
                _prevBlock[j] ^= inputBuffer[inputOffset + j];
            _insideCryptoTransform.TransformBlock(_prevBlock, 0, InputBlockSize, outputBuffer, outputOffset);
            Array.Copy(outputBuffer, outputOffset, _prevBlock, 0, InputBlockSize);
        }

        private void DecryptBlock(byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset)
        {
            _insideCryptoTransform.TransformBlock(inputBuffer, inputOffset, InputBlockSize, outputBuffer, outputOffset);
            for (int j = 0; j < InputBlockSize; ++j)
                outputBuffer[outputOffset + j] ^= _prevBlock[j];
            Array.Copy(inputBuffer, inputOffset, _prevBlock, 0, InputBlockSize);
        }
    }

    // TODO
    //CFB
    public class CipherFeedbackTransform : ICryptoTransform
    {
        public int InputBlockSize => throw new NotImplementedException();

        public int OutputBlockSize => throw new NotImplementedException();

        public bool CanTransformMultipleBlocks => throw new NotImplementedException();

        public bool CanReuseTransform => throw new NotImplementedException();

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            throw new NotImplementedException();
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            throw new NotImplementedException();
        }
    }
}
