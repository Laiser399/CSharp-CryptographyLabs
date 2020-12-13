using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CryptographyLabs
{
    public static class StreamEx
    {
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

        public static void EncryptTo(this Stream input, Stream output, ICryptoTransform transform)
        {
            int blocksCount = 1000;
            byte[] inputBuf, outputBuf;
            if (transform.CanTransformMultipleBlocks)
            {
                inputBuf = new byte[transform.InputBlockSize * blocksCount];
                outputBuf = new byte[transform.OutputBlockSize * blocksCount];
            }
            else
            {
                inputBuf = new byte[transform.InputBlockSize];
                outputBuf = new byte[transform.OutputBlockSize];
            }

            int inBuf = 0;
            while (true)
            {
                int hasRead = input.Read(inputBuf, inBuf, inputBuf.Length - inBuf);
                int transformed = transform.TransformBlock(inputBuf, 0, hasRead, outputBuf, 0);
            }
        }
    }
}
