using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptographyLabs.Crypto
{
    public static partial class Rijndael_
    {
        public static byte[] GenerateSBox()
        {
            byte[] matrix = new byte[]
            {
                0b1111_0001,
                0b1110_0011,
                0b1100_0111,
                0b1000_1111,
                0b0001_1111,
                0b0011_1110,
                0b0111_1100,
                0b1111_1000
            };

            byte[] sBox = new byte[256];
            for (int i = 0; i < 256; ++i)
            {
                byte inv = GF.Inverse((byte)i);
                for (int j = 0; j < 8; j++)
                {
                    byte conj = (byte)(matrix[j] & inv);
                    byte xorSum = Bitops.XorBits(conj);
                    sBox[i] |= (byte)(xorSum << j);
                }
                sBox[i] ^= 0x63;
            }
            return sBox;
        }

        public static byte[] GenerateInvSBox(byte[] sBox)
        {
            byte[] invSBox = new byte[256];
            for (int i = 0; i < 256; ++i)
                invSBox[sBox[i]] = (byte)i;
            return invSBox;
        }
    }
}
