using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Crypto;
using System.IO;
using System.Reflection;

namespace ConsoleTests
{
    public class SimpleCryptoTransform : ICryptoTransform
    {
        public int InputBlockSize => 1;

        public int OutputBlockSize => 1;

        public bool CanTransformMultipleBlocks => true;

        public bool CanReuseTransform => false;// TODO think

        private byte[] _key;
        private int _key_index = 0;
        private bool _flag;

        public SimpleCryptoTransform(byte[] key, bool flag = false)
        {
            if (key.Length < 1)
                throw new ArgumentException("Length of key must be >= 1.");
            _key = key;
            _flag = flag;
        }

        public void Dispose()
        {

        }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            if (_flag)
            {
                for (int i = 0; i < inputCount; ++i)
                {
                    outputBuffer[outputOffset + i] = (byte)(inputBuffer[inputOffset + i] ^ _key[_key_index]);
                    _key_index = (_key_index + 1) % _key.Length;
                }
                return inputCount;
            }
            else
            {
                int counter = 0;
                uint tmData = 0;
                uint tmKey = 0;
                for (int i = 0; i < inputCount; ++i)
                {
                    if (counter == 4)
                    {
                        byte[] aga = BitConverter.GetBytes(tmData ^ tmKey);
                        Array.Copy(aga, 0, outputBuffer, outputOffset + i - 4, 4);
                        counter = 0;
                    }

                    tmData <<= 8;
                    tmData |= inputBuffer[inputOffset + i];
                    tmKey <<= 8;
                    tmKey |= _key[_key_index];
                    _key_index = (_key_index + 1) % _key.Length;
                    counter++;
                }

                if (counter > 0)
                {
                    byte[] aga = BitConverter.GetBytes(tmData ^ tmKey);
                    Array.Copy(aga, 0, outputBuffer, outputOffset + inputCount - counter, counter);
                }
                return inputCount;
            }
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            byte[] result = new byte[inputCount];
            TransformBlock(inputBuffer, inputOffset, inputCount, result, 0);
            return result;
        }
    }

    public class TestCryptoTransform : ICryptoTransform
    {
        public int InputBlockSize => 8;

        public int OutputBlockSize => 8;

        public bool CanTransformMultipleBlocks => true;

        public bool CanReuseTransform => false;

        public void Dispose()
        {

        }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            Console.WriteLine($"Bytes count: {inputCount}");
            Array.Copy(inputBuffer, inputOffset, outputBuffer, outputOffset, inputCount);
            return inputCount;
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            byte[] result = new byte[inputCount];
            Array.Copy(inputBuffer, inputOffset, result, 0, inputCount);
            return result;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Type DESCrTransType = typeof(Crypto.DES).GetNestedType("DESCryptoTransform", BindingFlags.NonPublic);
            ulong[] masks = (ulong[])DESCrTransType.GetField("_IPPermMasks",
                BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
            foreach (ulong mask in masks)
                Console.WriteLine("{0:X}", mask);

            Console.WriteLine();
            Console.WriteLine("Press...");
            Console.ReadKey();
        }

        private static ulong[] CalcPermMasks64(byte[] myTransposition)
        {
            if (myTransposition.Length > 64)
                throw new Exception("chet ne to");
            else if (myTransposition.Length < 64)
            {
                // дополнение до 64 бит
                byte[] addedTransp = new byte[64];
                HashSet<byte> indices = new HashSet<byte>();
                for (byte i = 0; i < 64; ++i)
                    indices.Add(i);
                foreach (byte usedIdex in myTransposition)
                    indices.Remove(usedIdex);

                for (int i = 0; i < 64 - myTransposition.Length; ++i)
                {
                    addedTransp[i] = indices.First();
                    indices.Remove(addedTransp[i]);
                }
                for (int i = 0; i < myTransposition.Length; ++i)
                    addedTransp[63 - i] = myTransposition[myTransposition.Length - 1 - i];
                myTransposition = addedTransp;
            }

            byte[] trueTranspose = new byte[64];
            for (int i = 0; i < 64; ++i)
                trueTranspose[myTransposition[63 - i]] = (byte)i;
            var network = new PermutationNetwork(trueTranspose);

            //tests
            Random random = new Random(123);
            byte[] buf = new byte[8];
            for (int i = 0; i < 10000; ++i)
            {
                random.NextBytes(buf);
                ulong value = BitConverter.ToUInt64(buf, 0);

                ulong expected = Bitops.BitPermutation(value, myTransposition);
                ulong actualNetwork = network.Permute(value);

                ulong actual = value;
                byte[] deltas64 = new byte[] { 1, 2, 4, 8, 16, 32, 16, 8, 4, 2, 1 };
                for (int j = 0; j < deltas64.Length; ++j)
                {
                    ulong mask = network.Masks[j];
                    if (mask != 0)
                        actual = Bitops.SwapBitsMask(actual, deltas64[j], mask);
                }

                if (expected != actualNetwork || expected != actual)
                    throw new Exception($"ne: {i}");
            }

            ulong[] masks = new ulong[11];
            for (int i = 0; i < 11; ++i)
                masks[i] = network.Masks[i];
            return masks;
        }

        private static void DESTest()
        {
            string dir = @"C:\Users\Sergey\Desktop\testDir";
            string inFilename = Path.Combine(dir, @"Akira Yamaoka - Theme Of Laura (Reprise) (DJ Saban D'N'B Mix).mp3");
            Random random = new Random(123);
            byte[] tm = new byte[8];
            random.NextBytes(tm);
            ulong key = BitConverter.ToUInt64(tm, 0);
            string encryptedFilename = Path.Combine(dir, "encrypted.mp3");
            string decryptedFilename = Path.Combine(dir, "decrypted.mp3");
            long size = new FileInfo(inFilename).Length;

            DateTime start = DateTime.Now;
            // encryption
            using (FileStream inStream = new FileStream(inFilename, FileMode.Open, FileAccess.Read))
            using (FileStream outStream = new FileStream(encryptedFilename, FileMode.OpenOrCreate, FileAccess.Write))
            using (ICryptoTransform transform = Crypto.DES.ECBTransform(key, CryptoMode.Encrypt))
            using (CryptoStream outCrStream = new CryptoStream(outStream, transform, CryptoStreamMode.Write))
            {
                int bytesCount = 80000;
                byte[] buf = new byte[bytesCount];
                while (true)
                {
                    int hasRead = inStream.Read(buf, 0, bytesCount);
                    if (hasRead == 0)
                        break;
                    outCrStream.Write(buf, 0, hasRead);
                }
            }
            Console.WriteLine($"Encrypted in {DateTime.Now - start}");

            // decryption
            using (FileStream inStream = new FileStream(encryptedFilename, FileMode.Open, FileAccess.Read))
            using (FileStream outStream = new FileStream(decryptedFilename, FileMode.OpenOrCreate, FileAccess.Write))
            using (ICryptoTransform transform = Crypto.DES.ECBTransform(key, CryptoMode.Decrypt))
            using (CryptoStream outCrStream = new CryptoStream(outStream, transform, CryptoStreamMode.Write))
            {
                int bytesCount = 80000;
                byte[] buf = new byte[bytesCount];
                while (true)
                {
                    int hasRead = inStream.Read(buf, 0, bytesCount);
                    if (hasRead == 0)
                        break;
                    outCrStream.Write(buf, 0, hasRead);
                }
            }

            // check
            byte[] b1 = File.ReadAllBytes(inFilename);
            byte[] b2 = File.ReadAllBytes(decryptedFilename);
            bool compareRes = Enumerable.SequenceEqual(b1, b2);
            Console.WriteLine($"Compare res: {compareRes}");
        }

        private static void VernamTest()
        {
            string dir = @"C:\Users\Sergey\Desktop\testDir";
            string inFilename = Path.Combine(dir, @"Akira Yamaoka - Theme Of Laura (Reprise) (DJ Saban D'N'B Mix).mp3");
            string keyFilename = Path.Combine(dir, "key");
            string encryptedFilename = Path.Combine(dir, "encrypted.mp3");
            string decryptedFilename = Path.Combine(dir, "decrypted.mp3");
            long size = new FileInfo(inFilename).Length;

            // generate key file
            using (FileStream outStream = new FileStream(keyFilename, FileMode.OpenOrCreate, FileAccess.Write))
            {
                Random r = new Random(123);
                int bufSize = 80000;
                byte[] buff = new byte[bufSize];
                for (long i = 0; i < size;)
                {
                    r.NextBytes(buff);
                    int toWrite = (int)Math.Min(bufSize, size - i);
                    outStream.Write(buff, 0, toWrite);
                    i += toWrite;
                }
            }

            // encryption
            using (FileStream inStream = new FileStream(inFilename, FileMode.Open, FileAccess.Read))
            using (FileStream inKeyStream = new FileStream(keyFilename, FileMode.Open, FileAccess.Read))
            using (FileStream outStream = new FileStream(encryptedFilename, FileMode.OpenOrCreate, FileAccess.Write))
            using (CryptoStream outCrypto = new CryptoStream(outStream, new VernamCryptoTransform(inKeyStream), CryptoStreamMode.Write))
            {
                inStream.CopyTo(outCrypto);
            }

            // decryption
            using (FileStream inStream = new FileStream(encryptedFilename, FileMode.Open, FileAccess.Read))
            using (FileStream inKeyStream = new FileStream(keyFilename, FileMode.Open, FileAccess.Read))
            using (FileStream outStream = new FileStream(decryptedFilename, FileMode.OpenOrCreate, FileAccess.Write))
            using (CryptoStream outCrypto = new CryptoStream(outStream, new VernamCryptoTransform(inKeyStream), CryptoStreamMode.Write))
            {
                inStream.CopyTo(outCrypto);
            }

            // comparison
            byte[] b1 = File.ReadAllBytes(inFilename);
            byte[] b2 = File.ReadAllBytes(decryptedFilename);
            bool compareRes = Enumerable.SequenceEqual(b1, b2);
            Console.WriteLine($"Compare res: {compareRes}");


        }

        private static void SimpleTest()
        {
            string inFilename = @"D:\Downloads\Akira Yamaoka - Theme Of Laura (Reprise) (DJ Saban D'N'B Mix).mp3";
            string encryptedFilename = @"D:\Downloads\encrypted.mp3";
            string decryptedFilename = @"D:\Downloads\decrypted.mp3";
            byte[] key = new byte[1024];
            Random r = new Random(123);
            r.NextBytes(key);

            DateTime startTime = DateTime.Now;
            using (FileStream inStream = new FileStream(inFilename, FileMode.Open, FileAccess.Read))
            using (FileStream outStream = new FileStream(encryptedFilename, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (CryptoStream crStream = new CryptoStream(outStream, new SimpleCryptoTransform(key, true), CryptoStreamMode.Write))
                {
                    inStream.CopyTo(crStream);
                }
            }
            Console.WriteLine($"Encrypt done in {DateTime.Now - startTime}");

            startTime = DateTime.Now;
            using (FileStream inStream = new FileStream(inFilename, FileMode.Open, FileAccess.Read))
            using (FileStream outStream = new FileStream(encryptedFilename, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (CryptoStream crStream = new CryptoStream(outStream, new SimpleCryptoTransform(key, false), CryptoStreamMode.Write))
                {
                    inStream.CopyTo(crStream);
                }
            }
            Console.WriteLine($"Encrypt done in {DateTime.Now - startTime}");



            byte[] b1 = File.ReadAllBytes(inFilename);
            byte[] b2 = File.ReadAllBytes(decryptedFilename);
            bool compareRes = Enumerable.SequenceEqual(b1, b2);
            Console.WriteLine($"Compare res: {compareRes}");

        }

    }



}
