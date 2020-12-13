using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CryptographyLabs.Crypto.BlockCouplingModes
{
    public static class CBC
    {
        public static ICryptoTransform Get(INiceCryptoTransform transform, CryptoDirection direction)
        {
            if (direction == CryptoDirection.Encrypt)
                return new CBCEncryptTransform(transform);
            else
                return new CBCDecryptTransform(transform);
        }
    }

    public class CBCEncryptTransform : BaseEncryptTransform
    {
        private byte[] _initVector;

        public CBCEncryptTransform(INiceCryptoTransform transform) : base(transform)
        {
            _initVector = new byte[InputBlockSize];// TODO fill with something
            for (int i = 0; i < InputBlockSize; ++i)// TODO del mb
                _initVector[i] = 0;
        }

        #region BaseEncryptTransform

        protected override void Transform(byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset)
        {
            for (int i = 0; i < InputBlockSize; i++)
                inputBuffer[inputOffset + i] ^= _initVector[i];
            _baseTransform.NiceTransform(inputBuffer, inputOffset, outputBuffer, outputOffset, 1);
            Array.Copy(outputBuffer, outputOffset, _initVector, 0, InputBlockSize);
        }

        #endregion
    }

    public class CBCDecryptTransform : BaseDecryptTransform
    {
        private byte[] _initVector;

        public CBCDecryptTransform(INiceCryptoTransform transform) : base(transform)
        {
            _initVector = new byte[InputBlockSize];// TODO fill with something
            for (int i = 0; i < InputBlockSize; ++i)// TODO del mb
                _initVector[i] = 0;
        }

        #region ICryptoTransform interface

        protected override void Transform(byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset)
        {
            _baseTransform.NiceTransform(inputBuffer, inputOffset, outputBuffer, outputOffset, 1);
            for (int i = 0; i < InputBlockSize; i++)
                outputBuffer[outputOffset + i] ^= _initVector[i];
            Array.Copy(inputBuffer, inputOffset, _initVector, 0, InputBlockSize);
        }

        #endregion
    }

}
