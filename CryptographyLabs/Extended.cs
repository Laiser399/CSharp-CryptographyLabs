using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Globalization;

namespace CryptographyLabs
{
    public static class Extended
    {
        public static bool TryParse(string strValue, out uint value)
        {
            strValue = strValue.Replace(" ", "").Replace("_", "");

            if (strValue.Length > 2 && strValue.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    value = Convert.ToUInt32(strValue.Substring(2), 16);
                    return true;
                }
                catch
                {
                    value = 0;
                    return false;
                }
            }
            else if (strValue.Length > 2 && strValue.StartsWith("0b", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    value = Convert.ToUInt32(strValue.Substring(2), 2);
                    return true;
                }
                catch
                {
                    value = 0;
                    return false;
                }
            }
            else
            {
                return uint.TryParse(strValue, out value);
            }
        }

        public static bool TryParse(string strValue, out ulong value)
        {
            strValue = strValue.Replace(" ", "").Replace("_", "");

            if (strValue.Length > 2 && strValue.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    value = Convert.ToUInt64(strValue.Substring(2), 16);
                    return true;
                }
                catch
                {
                    value = 0;
                    return false;
                }
            }
            else if (strValue.Length > 2 && strValue.StartsWith("0b", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    value = Convert.ToUInt64(strValue.Substring(2), 2);
                    return true;
                }
                catch
                {
                    value = 0;
                    return false;
                }
            }
            else
            {
                return ulong.TryParse(strValue, out value);
            }
        }

        public static bool TryParse(string strValue, out byte[] bytes)
        {
            if (TryParse(strValue, out BigInteger value))
            {
                bytes = value.ToByteArray();
                if (bytes.Length > 1 && bytes[bytes.Length - 1] == 0)
                    Array.Resize(ref bytes, bytes.Length - 1);
                return true;
            }
            else
            {
                bytes = null;
                return false;
            }
        }

        public static bool TryParse(string strValue, out BigInteger value)
        {
            strValue = strValue.Replace(" ", "").Replace("_", "");

            if (strValue.Length > 2 && strValue.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                strValue = strValue.Substring(2, strValue.Length - 2);
                return BigInteger.TryParse(strValue, NumberStyles.HexNumber, null, out value);
            }
            else if (strValue.Length > 2 && strValue.StartsWith("0b", StringComparison.OrdinalIgnoreCase))
            {
                strValue = strValue.Substring(2, strValue.Length - 2);
                return TryParseBinary(strValue, out value);
            }
            else
                return BigInteger.TryParse(strValue, out value);
        }

        public static bool TryParseBinary(string strValue, out BigInteger value)
        {
            value = 0;
            foreach (char c in strValue)
            {
                value <<= 1;
                if (c == '1')
                    value |= 1;
                else if (c != '0')
                    return false;
            }
            return true;
        }

        public static void CopyToEx(this Stream from, Stream destination, int bufSize, 
            Action<double> progressCallback = null)
        {
            progressCallback?.Invoke(0);
            if (bufSize <= 0)
            {
                from.CopyTo(destination);
                progressCallback?.Invoke(1);
            }
            else
            {
                long bytesCount = from.Length;
                byte[] buf = new byte[bufSize];
                for (long i = 0; i < bytesCount;)
                {
                    int hasRead = from.Read(buf, 0, buf.Length);
                    destination.Write(buf, 0, hasRead);
                    i += hasRead;
                    progressCallback?.Invoke((double)i / bytesCount);
                }
            }
        }

        // записывает ровно по блокам и последний дополняет до полного (мусором)
        public static void CopyToEx(this Stream from, Stream destination, int blockSize, int bufBlocksCount,
            Action<double> progressCallback = null)
        {
            progressCallback?.Invoke(0);
            long bytesCount = from.Length;
            int bufSize = blockSize * bufBlocksCount;

            int inBuf = 0;
            byte[] buf = new byte[bufSize];
            for (long hasWrote = 0; hasWrote < bytesCount;)
            {
                int hasRead = from.Read(buf, inBuf, buf.Length - inBuf);
                inBuf += hasRead;
                int mod = inBuf % blockSize;
                if (hasRead == 0)
                {
                    inBuf += blockSize - mod;
                    mod = 0;
                }

                int toWrite = inBuf - mod;
                destination.Write(buf, 0, toWrite);
                hasWrote += toWrite;

                Array.Copy(buf, toWrite, buf, 0, mod);
                inBuf = mod;

                progressCallback?.Invoke((double)hasWrote / bytesCount);
            }
        }

    }
}
