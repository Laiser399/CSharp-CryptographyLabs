﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CryptographyLabs.Crypto.BlockCouplingModes
{
    public static class CFB
    {
        public static ICryptoTransform Get(INiceCryptoTransform transform, CryptoDirection direction)
        {
            if (direction == CryptoDirection.Encrypt)
                return new CFBEncryptTransform(transform);
            else
                return new CFBDecryptTransform(transform);
        }

        private class CFBEncryptTransform : BaseEncryptTransform
        {
            private byte[] _initVector;

            public CFBEncryptTransform(INiceCryptoTransform transform) : base(transform)
            {
                _initVector = new byte[InputBlockSize];
            }

            #region BaseEncryptTransform

            protected override void Transform(byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset)
            {
                _baseTransform.NiceTransform(_initVector, 0, outputBuffer, outputOffset, 1);
                for (int i = 0; i < InputBlockSize; i++)
                    outputBuffer[outputOffset + i] ^= inputBuffer[inputOffset + i];
                Array.Copy(outputBuffer, outputOffset, _initVector, 0, InputBlockSize);
            }

            #endregion
        }

        private class CFBDecryptTransform : BaseDecryptTransform
        {
            private byte[] _initVector;

            public CFBDecryptTransform(INiceCryptoTransform transform) : base(transform)
            {
                _initVector = new byte[InputBlockSize];
            }

            #region BaseEncryptTransform

            protected override void Transform(byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset)
            {
                _baseTransform.NiceTransform(_initVector, 0, outputBuffer, outputOffset, 1);
                Array.Copy(inputBuffer, inputOffset, _initVector, 0, InputBlockSize);
                for (int i = 0; i < InputBlockSize; i++)
                    outputBuffer[outputOffset + i] ^= inputBuffer[inputOffset + i];
            }

            #endregion

        }
    }

    

    

}
