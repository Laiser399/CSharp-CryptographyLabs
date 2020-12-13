using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CryptographyLabs.Crypto.BlockCouplingModes
{

    public static class OFB
    {
        public static ICryptoTransform Get(INiceCryptoTransform transform, CryptoDirection direction)
        {
            if (direction == CryptoDirection.Encrypt)
                return new OFBEncryptTransform(transform);
            else
                return new OFBDecryptTransform(transform);
        }
    }

    public class OFBEncryptTransform : BaseEncryptTransform
    {
        private byte[] _initVector;

        public OFBEncryptTransform(INiceCryptoTransform transform) : base(transform)
        {
            _initVector = new byte[InputBlockSize];// TODO fill with something
            for (int i = 0; i < InputBlockSize; ++i)// TODO del mb
                _initVector[i] = 0;
        }

        #region BaseDecryptTransform

        protected override void Transform(byte[] inputBuffer, int inputOffset,byte[] outputBuffer, int outputOffset)
        {
            _baseTransform.NiceTransform(_initVector, 0, outputBuffer, outputOffset, 1);
            Array.Copy(outputBuffer, outputOffset, _initVector, 0, InputBlockSize);
            for (int i = 0; i < InputBlockSize; i++)
                outputBuffer[outputOffset + i] ^= inputBuffer[inputOffset + i];
        }

        #endregion

    }

    public class OFBDecryptTransform : BaseDecryptTransform
    {
        private byte[] _initVector;

        public OFBDecryptTransform(INiceCryptoTransform transform) : base(transform)
        {
            _initVector = new byte[InputBlockSize];// TODO fill with something
            for (int i = 0; i < InputBlockSize; ++i)// TODO del mb
                _initVector[i] = 0;
        }

        #region BaseDecryptTransform

        protected override void Transform(byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset)
        {
            _baseTransform.NiceTransform(_initVector, 0, outputBuffer, outputOffset, 1);
            Array.Copy(outputBuffer, outputOffset, _initVector, 0, InputBlockSize);
            for (int i = 0; i < InputBlockSize; i++)
                outputBuffer[outputOffset + i] ^= inputBuffer[inputOffset + i];
        }

        #endregion
    }

}
