using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CryptographyLabs.Crypto.BlockCouplingModes
{
    public abstract class BaseCTRTransform : ICryptoTransform
    {
        protected ICryptoTransform _baseTransform;

        public BaseCTRTransform(ICryptoTransform transform)
        {
            if (transform.InputBlockSize != transform.OutputBlockSize)
                throw new CryptographicException("OFB transform does not support different block sizes.");

            _baseTransform = transform;
        }

        #region ICryptoTransform interface

        public int InputBlockSize => _baseTransform.InputBlockSize;
        public int OutputBlockSize => _baseTransform.OutputBlockSize;
        public bool CanTransformMultipleBlocks => false;
        public bool CanReuseTransform => false;

        public void Dispose()
        {
            _baseTransform.Dispose();
        }

        public abstract int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset);

        public abstract byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount);

        #endregion
    }

    public class CTREncryptTransform : BaseCTRTransform
    {
        public CTREncryptTransform(ICryptoTransform transform) : base(transform) { }

        #region ICryptoTransform interface

        public override int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            throw new NotImplementedException();
        }

        public override byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class CTRDecryptTransform : BaseCTRTransform
    {
        public CTRDecryptTransform(ICryptoTransform transform) : base(transform) { }

        #region ICryptoTransform interface

        public override int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            throw new NotImplementedException();
        }

        public override byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            throw new NotImplementedException();
        }

        #endregion
    }



    //public class CounterModeTransofrm : ICryptoTransform
    //{
    //    public int InputBlockSize => _insideEncryptTransform.InputBlockSize;

    //    public int OutputBlockSize => _insideEncryptTransform.OutputBlockSize;

    //    public bool CanTransformMultipleBlocks => true;

    //    public bool CanReuseTransform => false;

    //    private ICryptoTransform _insideEncryptTransform;
    //    private BlockTransformFunc _transformFunc;
    //    private ulong _counter = 0x97_9c_2c_da_e0_8a_6c_25;
    //    private byte _maskLen = 10;
    //    private ulong _mask = 0b11_11111111;

    //    public CounterModeTransofrm(ICryptoTransform encryptTransform)
    //    {
    //        _insideEncryptTransform = encryptTransform;

    //        if (InputBlockSize > 8)
    //            _transformFunc = TransformSimpleBlockLong;
    //        else
    //            _transformFunc = TransformSimpleBlockShort;
    //    }

    //    public void Dispose()
    //    {
    //        _insideEncryptTransform.Dispose();
    //    }

    //    public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
    //    {
    //        for (int i = 0; i < inputCount / InputBlockSize; ++i)
    //        {
    //            _transformFunc(inputBuffer, inputOffset + i * InputBlockSize, outputBuffer, outputOffset + i * InputBlockSize);
    //            _counter++;
    //        }
    //        return inputCount;
    //    }

    //    public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
    //    {
    //        byte[] result = new byte[inputCount];
    //        TransformBlock(inputBuffer, inputOffset, inputCount, result, 0);
    //        return result;
    //    }

    //    private void TransformSimpleBlockLong(byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset)
    //    {
    //        ulong newCounter = (_counter << _maskLen) | ((_counter + 1) & _mask);
    //        byte[] buf = BitConverter.GetBytes(newCounter);

    //        int prevLength = buf.Length;
    //        Array.Resize(ref buf, InputBlockSize);
    //        for (int j = prevLength; j < buf.Length; ++j)
    //            buf[j] = (byte)(buf[j % prevLength] + j / prevLength);

    //        _insideEncryptTransform.TransformBlock(buf, 0, InputBlockSize, outputBuffer, outputOffset);
    //        for (int j = 0; j < InputBlockSize; ++j)
    //            outputBuffer[outputOffset + j] ^= inputBuffer[inputOffset + j];
    //    }

    //    private void TransformSimpleBlockShort(byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset)
    //    {
    //        ulong newCounter = (_counter << _maskLen) | ((_counter + 1) & _mask);
    //        byte[] buf = BitConverter.GetBytes(newCounter);
    //        _insideEncryptTransform.TransformBlock(buf, 0, InputBlockSize, outputBuffer, outputOffset);
    //        for (int j = 0; j < InputBlockSize; ++j)
    //            outputBuffer[outputOffset + j] ^= inputBuffer[inputOffset + j];
    //    }
    //}
}
