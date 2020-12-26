using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CryptographyLabs.Crypto.BlockCouplingModes
{
    public static class ECB
    {
        /// <exception cref="ArgumentException">Length of text if empty.</exception>
        public static async Task<byte[]> TransformFinalAsync(byte[] text, INiceCryptoTransform transform, int threadCount)
        {
            return await TransformFinalAsync(text, 0, text.Length, transform, threadCount);
        }

        /// <exception cref="ArgumentException">Length of text if empty.</exception>
        public static async Task<byte[]> TransformFinalAsync(byte[] text, int offset, int count,
            INiceCryptoTransform transform, int threadCount)
        {
            if (count == 0)
                throw new ArgumentException("Length of text if empty.");

            int remains = count % transform.InputBlockSize;
            int blocksCount = count / transform.InputBlockSize;
            byte[] final;
            if (remains == 0)
            {
                blocksCount--;
                // TODO Task.Run
                final = transform.NiceFinalTransform(text, offset + blocksCount * transform.InputBlockSize,
                    transform.InputBlockSize);
            }
            else
            {
                final = transform.NiceFinalTransform(text, offset + blocksCount * transform.InputBlockSize, remains);
            }

            byte[] result = new byte[blocksCount * transform.OutputBlockSize + final.Length];
            Array.Copy(final, 0, result, blocksCount * transform.OutputBlockSize, final.Length);

            await TransformAsync(text, offset, result, 0, blocksCount, transform, threadCount);
            return result;
        }

        /// <exception cref="ArgumentException">Wrong length of text</exception>
        public static async Task<byte[]> TransformAsync(byte[] text, INiceCryptoTransform transform, int threadCount)
        {
            if (text.Length % transform.InputBlockSize != 0)
                throw new ArgumentException("Wrong length of text.");

            int blocksCount = text.Length / transform.InputBlockSize;
            byte[] result = new byte[blocksCount * transform.OutputBlockSize];
            await TransformAsync(text, 0, result, 0, blocksCount, transform, threadCount);
            return result;
        }

        /// <exception cref="ArgumentException">Wrong length of text</exception>
        public static async Task<byte[]> TransformAsync(byte[] text, int offset, int count, 
            INiceCryptoTransform transform, int threadCount)
        {
            if (count % transform.InputBlockSize != 0)
                throw new ArgumentException("Wrong length of text.");

            int blocksCount = count / transform.InputBlockSize;
            byte[] result = new byte[blocksCount * transform.OutputBlockSize];
            await TransformAsync(text, offset, result, 0, blocksCount, transform, threadCount);
            return result;
        }

        private static async Task TransformAsync(byte[] intputBuffer, int inputOffset, 
            byte[] outputBuffer, int outputOffset, int blocksCount,
            INiceCryptoTransform transform, int threadCount)
        {
            int blocksPerThread = blocksCount / threadCount;
            int blocksRemains = blocksCount % threadCount;
            int blocksOffset = 0;

            Task[] transformTasks = new Task[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                int currentBlocksCount = blocksPerThread;
                if (i < blocksRemains)
                    currentBlocksCount++;

                transformTasks[i] = TransformTask(intputBuffer, inputOffset + blocksOffset * transform.InputBlockSize,
                    outputBuffer, outputOffset + blocksOffset * transform.OutputBlockSize, currentBlocksCount, transform);

                blocksOffset += currentBlocksCount;
            }

            await Task.WhenAll(transformTasks);
        }

        private static Task TransformTask(byte[] inputBuffer, int inputOffset,
            byte[] outputBuffer, int outputOffset, int blocksCount, INiceCryptoTransform transform)
        {
            return Task.Run(() =>
            {
                transform.NiceTransform(inputBuffer, inputOffset,
                    outputBuffer, outputOffset, blocksCount);
            });
        }

        
    }
}
