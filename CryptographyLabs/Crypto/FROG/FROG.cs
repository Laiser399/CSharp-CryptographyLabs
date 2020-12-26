using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using CryptographyLabs.Extensions;

namespace CryptographyLabs.Crypto
{
    public partial class FROGProvider
    {
        private byte[] _key;
        private byte[][][] _encryptRoundKeys;
        private byte[][][] _decryptRoundKeys;

        public FROGProvider(byte[] key)
        {
            if (key.Length < MinKeyLength || key.Length > MaxKeyLength)
                throw new ArgumentException("Wrong key length.");

            _key = key;
        }

        public ICryptoTransform Create(CryptoDirection direction)
        {
            if (direction == CryptoDirection.Encrypt)
            {
                if (_encryptRoundKeys is null)
                    _encryptRoundKeys = GenerateKey(_key, CryptoDirection.Encrypt);
                return new FROGEncryptTransform(_encryptRoundKeys);
            }
            else
            {
                if (_decryptRoundKeys is null)
                    _decryptRoundKeys = GenerateKey(_key, CryptoDirection.Decrypt);
                return new FROGDecryptTransform(_decryptRoundKeys);
            }
        }

        public INiceCryptoTransform CreateNice(CryptoDirection direction)
        {
            throw new NotImplementedException();
        }

        // static
        public static int MinKeyLength => 5;
        public static int MaxKeyLength => 125;
        private const int _blockSize = 16;
        private static byte[] _masterKey = new byte[]
        {
            113,  21, 232,  18, 113,  92,  63, 157, 124, 193, 166, 197, 126,  56, 229, 229,
            156, 162,  54,  17, 230,  89, 189,  87, 169,   0,  81, 204,   8,  70, 203, 225,
            160,  59, 167, 189, 100, 157,  84,  11,   7, 130,  29,  51,  32,  45, 135, 237,
            139,  33,  17, 221,  24,  50,  89,  74,  21, 205, 191, 242,  84,  53,   3, 230,
            231, 118,  15,  15, 107,   4,  21,  34,   3, 156,  57,  66,  93, 255, 191,   3,
             85, 135, 205, 200, 185, 204,  52,  37,  35,  24,  68, 185, 201,  10, 224, 234,
              7, 120, 201, 115, 216, 103,  57, 255,  93, 110,  42, 249,  68,  14,  29,  55,
            128,  84,  37, 152, 221, 137,  39,  11, 252,  50, 144,  35, 178, 190,  43, 162,
            103, 249, 109,   8, 235,  33, 158, 111, 252, 205, 169,  54,  10,  20, 221, 201,
            178, 224,  89, 184, 182,  65, 201,  10,  60,   6, 191, 174,  79,  98,  26, 160,
            252,  51,  63,  79,   6, 102, 123, 173,  49,   3, 110, 233,  90, 158, 228, 210,
            209, 237,  30,  95,  28, 179, 204, 220,  72, 163,  77, 166, 192,  98, 165,  25,
            145, 162,  91, 212,  41, 230, 110,   6, 107, 187, 127,  38,  82,  98,  30,  67,
            225,  80, 208, 134,  60, 250, 153,  87, 148,  60,  66, 165,  72,  29, 165,  82,
            211, 207,   0, 177, 206,  13,   6,  14,  92, 248,  60, 201, 132,  95,  35, 215,
            118, 177, 121, 180,  27,  83, 131,  26,  39,  46,  12
        };

        private static T[] Expand<T>(T[] array, int newLength)
        {
            T[] result = new T[newLength];
            for (int i = 0; i < newLength; i++)
                result[i] = array[i % array.Length];
            return result;
        }

        private static byte[][][] GenerateKey(byte[] key, CryptoDirection direction)
        {
            // 1
            byte[] keyExpanded = Expand(key, 2304);
            // 2
            byte[] masterKeyExpanded = Expand(_masterKey, 2304);
            // 3
            for (int i = 0; i < 2304; i++)
                keyExpanded[i] = (byte)(keyExpanded[i] ^ masterKeyExpanded[i]);
            // 4
            return FormatExpandedKey(keyExpanded, direction);
        }

        private static byte[][][] FormatExpandedKey(byte[] expandedKey, CryptoDirection direction)
        {
            int bytesInKey = 288;// 16 + 256 + 16
            byte[][][] result = new byte[8][][];
            for (int i = 0; i < 8; i++)
            {
                byte[] key1 = new byte[16];
                byte[] key2 = new byte[256];
                byte[] key3 = new byte[16];

                Array.Copy(expandedKey, i * bytesInKey, key1, 0, 16);
                Array.Copy(expandedKey, i * bytesInKey + 16, key2, 0, 256);
                Array.Copy(expandedKey, i * bytesInKey + 272, key3, 0, 16);

                // 4.1
                Format(key2);
                if (direction == CryptoDirection.Decrypt)
                    key2 = Invert(key2);

                // 4.4
                Format(key3);
                // 4.5
                MakeSingleCycle(key3);
                // 4.6
                for (int j = 0; j < 16; j++)
                    if (key3[j] == j + 1)
                        key3[j] = (byte)((j + 2) % 16);

                result[i] = new byte[3][]
                {
                    key1, key2, key3
                };
            }
            return result;
        }

        private static void Format(byte[] values)
        {
            List<byte> U = new List<byte>(values.Length);
            for (int i = 0; i < values.Length; i++)
                U.Add((byte)i);

            int prevIndex = 0;
            for (int i = 0; i < values.Length; i++)
            {
                int currentIndex = (prevIndex + values[i]) % U.Count;
                prevIndex = currentIndex;
                values[i] = U[currentIndex];
                U.RemoveAt(currentIndex);
            }
        }

        private static byte[] Invert(byte[] values)
        {
            byte[] result = new byte[values.Length];
            for (int i = 0; i < values.Length; i++)
                result[values[i]] = (byte)i;
            return result;
        }

        private static void MakeSingleCycle(byte[] cycles)
        {
            BitArray used = new BitArray(16, false);

            int startIndex = 0;
            byte index = (byte)startIndex;
            used[index] = true;
            while (cycles[index] != startIndex)
            {
                index = cycles[index];
                used[index] = true;
            }

            while (true)
            {
                int nextStartIndex = used.FirstIndexOf(false);
                if (nextStartIndex == -1)
                {
                    cycles[index] = 0;
                    break;
                }
                cycles[index] = (byte)nextStartIndex;
                index = cycles[index];
                used[index] = true;
                do
                {
                    index = cycles[index];
                    used[index] = true;
                } while (cycles[index] != nextStartIndex);
            }
        }

    }

}
