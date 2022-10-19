using System;

namespace CryptographyLabs.Crypto
{
    public static partial class Rijndael_
    {
        /// <exception cref="ArgumentException">Wrong key length</exception>
        public static INiceCryptoTransform GetNice(byte[] key, Size stateSize, CryptoDirection direction)
        {
            if (!IsValidKeyLength(key))
                throw new ArgumentException("Wrong key length.");

            if (direction == CryptoDirection.Encrypt)
                return new RijndaelEncryptTransform(stateSize, key);
            else
                return new RijndaelDecryptTransform(stateSize, key);
        }

        private static bool IsValidKeyLength(byte[] key)
        {
            return key.Length == 16 || key.Length == 24 || key.Length == 32;
        }
    }
}