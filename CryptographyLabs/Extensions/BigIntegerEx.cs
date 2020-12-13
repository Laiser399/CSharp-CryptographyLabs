using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CryptographyLabs
{
    public static class BigIntegerEx
    {
        public static int BytesCount(this BigInteger value)
        {
            byte[] bytes = value.ToByteArray();
            if (bytes.Length > 1 && bytes[bytes.Length - 1] == 0)
                return bytes.Length - 1;
            else
                return bytes.Length;
        }

        public static byte[] ToByteArrayWithoutZero(this BigInteger value)
        {
            byte[] bytes = value.ToByteArray();
            if (bytes.Length > 1 && bytes[bytes.Length - 1] == 0)
                Array.Resize(ref bytes, bytes.Length - 1);
            return bytes;
        }
    }
}
