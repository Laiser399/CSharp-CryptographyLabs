using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace Crypto
{
    delegate void BlockTransformFunc(byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset);

    // CBC
    public class CipherBlockChainingTransform : ICryptoTransform
    {
        public int InputBlockSize => _insideCryptoTransform.InputBlockSize;

        public int OutputBlockSize => _insideCryptoTransform.OutputBlockSize;

        public bool CanTransformMultipleBlocks => true;

        public bool CanReuseTransform => false;

        
        private ICryptoTransform _insideCryptoTransform;
        private byte[] _prevBlock;
        private BlockTransformFunc _transformFunc;

        // TODO bool -> enum
        public CipherBlockChainingTransform(ICryptoTransform blockCryptoTransform, CryptoDirection mode)
        {
            _insideCryptoTransform = blockCryptoTransform;

            _prevBlock = new byte[InputBlockSize];// TODO fill with something
            for (int i = 0; i < InputBlockSize; ++i)// TODO del mb
                _prevBlock[i] = 0;

            if (mode == CryptoDirection.Encrypt)
                _transformFunc = EncryptBlock;
            else
                _transformFunc = DecryptBlock;
        }

        public void Dispose()
        {
            _insideCryptoTransform.Dispose();
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

    //CFB
    public class CipherFeedbackTransform : ICryptoTransform
    {
        public int InputBlockSize => _insideEncryptTransform.InputBlockSize;

        public int OutputBlockSize => _insideEncryptTransform.OutputBlockSize;

        public bool CanTransformMultipleBlocks => true;

        public bool CanReuseTransform => false;

        private ICryptoTransform _insideEncryptTransform;
        private byte[] _prevBlock;
        private BlockTransformFunc _transformFunc;

        public CipherFeedbackTransform(ICryptoTransform encryptTransform, CryptoDirection mode)
        {
            _insideEncryptTransform = encryptTransform;

            _prevBlock = new byte[InputBlockSize];// TODO fill with something
            for (int i = 0; i < InputBlockSize; ++i)// TODO del mb
                _prevBlock[i] = 0;

            if (mode == CryptoDirection.Encrypt)
                _transformFunc = EncryptBlock;
            else
                _transformFunc = DecryptBlock;
        }

        public void Dispose()
        {
            _insideEncryptTransform.Dispose();
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
            _insideEncryptTransform.TransformBlock(_prevBlock, 0, InputBlockSize, outputBuffer, outputOffset);
            for (int j = 0; j < InputBlockSize; ++j)
                outputBuffer[outputOffset + j] ^= inputBuffer[inputOffset + j];
            Array.Copy(outputBuffer, outputOffset, _prevBlock, 0, InputBlockSize);
        }

        private void DecryptBlock(byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset)
        {
            _insideEncryptTransform.TransformBlock(_prevBlock, 0, InputBlockSize, outputBuffer, outputOffset);
            Array.Copy(inputBuffer, inputOffset, _prevBlock, 0, InputBlockSize);
            for (int j = 0; j < InputBlockSize; ++j)
                outputBuffer[outputOffset + j] ^= inputBuffer[inputOffset + j];
        }
    }

    //OFB
    public class OutputFeedbackTransform : ICryptoTransform
    {
        public int InputBlockSize => _insideEncryptTransform.InputBlockSize;

        public int OutputBlockSize => _insideEncryptTransform.OutputBlockSize;

        public bool CanTransformMultipleBlocks => true;

        public bool CanReuseTransform => false;

        private ICryptoTransform _insideEncryptTransform;
        private byte[] _prevBlock;

        public OutputFeedbackTransform(ICryptoTransform encryptTransform)
        {
            _insideEncryptTransform = encryptTransform;

            _prevBlock = new byte[InputBlockSize];// TODO fill with something
            for (int i = 0; i < InputBlockSize; ++i)// TODO del mb
                _prevBlock[i] = 0;
        }

        public void Dispose()
        {
            _insideEncryptTransform.Dispose();
        }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            for (int i = 0; i < inputCount / InputBlockSize; ++i)
                TransformSimpleBlock(inputBuffer, inputOffset + i * InputBlockSize, outputBuffer, outputOffset + i * InputBlockSize);
            return inputCount;
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            byte[] result = new byte[inputCount];
            TransformBlock(inputBuffer, inputOffset, inputCount, result, 0);
            return result;
        }

        private void TransformSimpleBlock(byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset)
        {
            _insideEncryptTransform.TransformBlock(_prevBlock, 0, InputBlockSize, outputBuffer, outputOffset);
            Array.Copy(outputBuffer, outputOffset, _prevBlock, 0, InputBlockSize);
            for (int j = 0; j < InputBlockSize; ++j)
                outputBuffer[outputOffset + j] ^= inputBuffer[inputOffset + j];
        }
    }

    //CTR
    public class CounterModeTransofrm : ICryptoTransform
    {
        public int InputBlockSize => _insideEncryptTransform.InputBlockSize;

        public int OutputBlockSize => _insideEncryptTransform.OutputBlockSize;

        public bool CanTransformMultipleBlocks => true;

        public bool CanReuseTransform => false;

        private ICryptoTransform _insideEncryptTransform;
        private BlockTransformFunc _transformFunc;
        private ulong _counter = 0x97_9c_2c_da_e0_8a_6c_25;
        private byte _maskLen = 10;
        private ulong _mask = 0b11_11111111;

        public CounterModeTransofrm(ICryptoTransform encryptTransform)
        {
            _insideEncryptTransform = encryptTransform;

            if (InputBlockSize > 8)
                _transformFunc = TransformSimpleBlockLong;
            else
                _transformFunc = TransformSimpleBlockShort;
        }

        public void Dispose()
        {
            _insideEncryptTransform.Dispose();
        }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            for (int i = 0; i < inputCount / InputBlockSize; ++i)
            {
                _transformFunc(inputBuffer, inputOffset + i * InputBlockSize, outputBuffer, outputOffset + i * InputBlockSize);
                _counter++;
            }
            return inputCount;
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            byte[] result = new byte[inputCount];
            TransformBlock(inputBuffer, inputOffset, inputCount, result, 0);
            return result;
        }

        private void TransformSimpleBlockLong(byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset)
        {
            ulong newCounter = (_counter << _maskLen) | ((_counter + 1) & _mask);
            byte[] buf = BitConverter.GetBytes(newCounter);
            
            int prevLength = buf.Length;
            Array.Resize(ref buf, InputBlockSize);
            for (int j = prevLength; j < buf.Length; ++j)
                buf[j] = (byte)(buf[j % prevLength] + j / prevLength);

            _insideEncryptTransform.TransformBlock(buf, 0, InputBlockSize, outputBuffer, outputOffset);
            for (int j = 0; j < InputBlockSize; ++j)
                outputBuffer[outputOffset + j] ^= inputBuffer[inputOffset + j];
        }

        private void TransformSimpleBlockShort(byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset)
        {
            ulong newCounter = (_counter << _maskLen) | ((_counter + 1) & _mask);
            byte[] buf = BitConverter.GetBytes(newCounter);
            _insideEncryptTransform.TransformBlock(buf, 0, InputBlockSize, outputBuffer, outputOffset);
            for (int j = 0; j < InputBlockSize; ++j)
                outputBuffer[outputOffset + j] ^= inputBuffer[inputOffset + j];
        }
    }
}
