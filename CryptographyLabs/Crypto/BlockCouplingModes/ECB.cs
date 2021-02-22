using CryptographyLabs.Extensions;
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
        /// <exception cref="ArgumentException">data is empty.</exception>
        /// <exception cref="OperationCanceledException">canceled</exception>
        public static async Task<byte[]> TransformAsync(byte[] data, INiceCryptoTransform transform, 
            CancellationToken token, int threadsCount = 4, Action<double> progressCallback = null)
        {
            if (data.Length == 0)
                throw new ArgumentException("Length of text if empty.");

            int blocksCount = data.Length / transform.InputBlockSize;
            int lastBlockSize = data.Length % transform.InputBlockSize;
            if (lastBlockSize == 0)
            {
                blocksCount--;
                lastBlockSize = transform.InputBlockSize;
            }

            byte[] result = new byte[blocksCount * transform.OutputBlockSize];

            int blocksPerThread = blocksCount / threadsCount;
            Task[] transformTasks = new Task[threadsCount];
            double[] progresses = new double[threadsCount];
            for (int i = 0; i < threadsCount; i++)
            {
                int currentBlocksCount = i == threadsCount - 1
                    ? blocksPerThread + blocksCount % threadsCount
                    : blocksPerThread;

                int i_ = i;
                transformTasks[i] = MakeTransformTask(transform, data, i * blocksPerThread * transform.InputBlockSize,
                    result, i * blocksPerThread * transform.OutputBlockSize, currentBlocksCount, token,
                    (progress) =>
                    {
                        progresses[i_] = progress;
                        progressCallback?.Invoke(MathEx.Sum(progresses) / threadsCount);
                    });
            }

            Task<byte[]> finalTask = Task.Run(() 
                => transform.NiceFinalTransform(data, blocksCount * transform.InputBlockSize, lastBlockSize));

            await Task.WhenAll(transformTasks);
            byte[] final = await finalTask;

            Array.Resize(ref result, result.Length + final.Length);
            Array.Copy(final, 0, result, blocksCount * transform.OutputBlockSize, final.Length);
            return result;
        }

        /// <summary>
        /// wrap for argumets
        /// </summary>
        /// <exception cref="OperationCanceledException">Task with this exception</exception>
        private static Task MakeTransformTask(INiceCryptoTransform transform, byte[] inBuf, int inOffset,
            byte[] outBuf, int outOffset, int blocksCount, CancellationToken token, Action<double> progressCallback = null)
        {
            return Task.Run(() =>
            {
                transform.NiceTransform(inBuf, inOffset, outBuf, outOffset, blocksCount, token, progressCallback);
            });
        }


        /// <exception cref="ArgumentException">data is empty.</exception>
        public static async Task<byte[]> TransformAsync(byte[] data, INiceCryptoTransform transform,
            int threadsCount = 4, Action<double> progressCallback = null)
        {
            if (data.Length == 0)
                throw new ArgumentException("Length of text if empty.");

            int blocksCount = data.Length / transform.InputBlockSize;
            int lastBlockSize = data.Length % transform.InputBlockSize;
            if (lastBlockSize == 0)
            {
                blocksCount--;
                lastBlockSize = transform.InputBlockSize;
            }

            byte[] result = new byte[blocksCount * transform.OutputBlockSize];

            int blocksPerThread = blocksCount / threadsCount;
            Task[] transformTasks = new Task[threadsCount];
            double[] progresses = new double[threadsCount];
            for (int i = 0; i < threadsCount; i++)
            {
                int currentBlocksCount = i == threadsCount - 1
                    ? blocksPerThread + blocksCount % threadsCount
                    : blocksPerThread;

                int i_ = i;
                transformTasks[i] = MakeTransformTask(transform, data, i * blocksPerThread * transform.InputBlockSize,
                    result, i * blocksPerThread * transform.OutputBlockSize, currentBlocksCount,
                    (progress) =>
                    {
                        progresses[i_] = progress;
                        progressCallback?.Invoke(MathEx.Sum(progresses) / threadsCount);
                    });
            }

            Task<byte[]> finalTask = Task.Run(()
                => transform.NiceFinalTransform(data, blocksCount * transform.InputBlockSize, lastBlockSize));

            await Task.WhenAll(transformTasks);
            byte[] final = await finalTask;

            Array.Resize(ref result, result.Length + final.Length);
            Array.Copy(final, 0, result, blocksCount * transform.OutputBlockSize, final.Length);
            return result;
        }

        private static Task MakeTransformTask(INiceCryptoTransform transform, byte[] inBuf, int inOffset, 
            byte[] outBuf, int outOffset, int blocksCount, Action<double> progressCallback = null)
        {
            return Task.Run(() =>
            {
                transform.NiceTransform(inBuf, inOffset, outBuf, outOffset, blocksCount, progressCallback);
            });
        }

        // TODO delete

        ///// <exception cref="ArgumentException">Length of text if empty.</exception>
        //public static async Task<byte[]> TransformFinalAsync(byte[] text, INiceCryptoTransform transform, int threadCount)
        //{
        //    return await TransformFinalAsync(text, 0, text.Length, transform, threadCount);
        //}

        ///// <exception cref="ArgumentException">Length of text if empty.</exception>
        //public static async Task<byte[]> TransformFinalAsync(byte[] text, int offset, int count,
        //    INiceCryptoTransform transform, int threadCount)
        //{
        //    if (count == 0)
        //        throw new ArgumentException("Length of text if empty.");

        //    int remains = count % transform.InputBlockSize;
        //    int blocksCount = count / transform.InputBlockSize;
        //    byte[] final;
        //    if (remains == 0)
        //    {
        //        blocksCount--;
        //        final = await Task.Run(() => transform.NiceFinalTransform(text, 
        //            offset + blocksCount * transform.InputBlockSize, transform.InputBlockSize));
        //    }
        //    else
        //    {
        //        final = await Task.Run(() => transform.NiceFinalTransform(text, 
        //            offset + blocksCount * transform.InputBlockSize, remains));
        //    }

        //    byte[] result = new byte[blocksCount * transform.OutputBlockSize + final.Length];
        //    Array.Copy(final, 0, result, blocksCount * transform.OutputBlockSize, final.Length);

        //    await TransformAsync(text, offset, result, 0, blocksCount, transform, threadCount);
        //    return result;
        //}

        ///// <exception cref="ArgumentException">Wrong length of text</exception>
        //public static async Task<byte[]> TransformAsync(byte[] text, INiceCryptoTransform transform, int threadCount)
        //{
        //    if (text.Length % transform.InputBlockSize != 0)
        //        throw new ArgumentException("Wrong length of text.");

        //    int blocksCount = text.Length / transform.InputBlockSize;
        //    byte[] result = new byte[blocksCount * transform.OutputBlockSize];
        //    await TransformAsync(text, 0, result, 0, blocksCount, transform, threadCount);
        //    return result;
        //}

        ///// <exception cref="ArgumentException">Wrong length of text</exception>
        //public static async Task<byte[]> TransformAsync(byte[] text, int offset, int count, 
        //    INiceCryptoTransform transform, int threadCount)
        //{
        //    if (count % transform.InputBlockSize != 0)
        //        throw new ArgumentException("Wrong length of text.");

        //    int blocksCount = count / transform.InputBlockSize;
        //    byte[] result = new byte[blocksCount * transform.OutputBlockSize];
        //    await TransformAsync(text, offset, result, 0, blocksCount, transform, threadCount);
        //    return result;
        //}

        //private static async Task TransformAsync(byte[] intputBuffer, int inputOffset, 
        //    byte[] outputBuffer, int outputOffset, int blocksCount,
        //    INiceCryptoTransform transform, int threadCount)
        //{
        //    int blocksPerThread = blocksCount / threadCount;
        //    int blocksRemains = blocksCount % threadCount;
        //    int blocksOffset = 0;

        //    Task[] transformTasks = new Task[threadCount];
        //    for (int i = 0; i < threadCount; i++)
        //    {
        //        int currentBlocksCount = blocksPerThread;
        //        if (i < blocksRemains)
        //            currentBlocksCount++;

        //        transformTasks[i] = TransformTask(intputBuffer, inputOffset + blocksOffset * transform.InputBlockSize,
        //            outputBuffer, outputOffset + blocksOffset * transform.OutputBlockSize, currentBlocksCount, transform);

        //        blocksOffset += currentBlocksCount;
        //    }

        //    await Task.WhenAll(transformTasks);
        //}

        //private static Task TransformTask(byte[] inputBuffer, int inputOffset,
        //    byte[] outputBuffer, int outputOffset, int blocksCount, INiceCryptoTransform transform)
        //{
        //    return Task.Run(() =>
        //    {
        //        transform.NiceTransform(inputBuffer, inputOffset,
        //            outputBuffer, outputOffset, blocksCount);
        //    });
        //}

        
    }
}
