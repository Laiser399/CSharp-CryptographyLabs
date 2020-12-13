using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using CryptographyLabs;

namespace CryptographyLabs.Crypto
{
    public static class Vernam
    {

        public static void EncryptFile(string path)
        {
            EncryptFile(path, 0);
        }

        public static Task EncryptFileAsync(string path, Action<double> progressCallback = null)
        {
            return Task.Run(() =>
            {
                EncryptFile(path, 100000, progressCallback);
            });
        }

        private static void EncryptFile(string path, int bufSize, Action<double> progressCallback = null)
        {
            string encryptPath = path + ".v399";
            string keyPath = path + ".vkey399";
            long bytesCount = new FileInfo(path).Length;

            if (progressCallback is null)
                GenerateKeyFile(keyPath, bytesCount);
            else
                GenerateKeyFile(keyPath, bytesCount, progress => progressCallback(progress / 2));

            using (FileStream inStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (FileStream inKeyStream = new FileStream(keyPath, FileMode.Open, FileAccess.Read))
            using (FileStream outStream = new FileStream(encryptPath, FileMode.OpenOrCreate, FileAccess.Write))
            using (CryptoStream outCrypto = new CryptoStream(outStream, new VernamCryptoTransform(inKeyStream), CryptoStreamMode.Write))
            {
                if (progressCallback is object)
                    inStream.CopyToEx(outCrypto, bufSize, progress => progressCallback(progress / 2 + 0.5));
                else
                    inStream.CopyToEx(outCrypto, bufSize);
            }
        }

        private static void GenerateKeyFile(string path, long bytesCount, Action<double> progressCallback = null)
        {
            using (FileStream outStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                Random random = new Random((int)DateTime.Now.Ticks);
                int bufSize = 80000;
                byte[] buff = new byte[bufSize];
                for (long i = 0; i < bytesCount;)
                {
                    random.NextBytes(buff);
                    int toWrite = (int)Math.Min(bufSize, bytesCount - i);
                    outStream.Write(buff, 0, toWrite);
                    i += toWrite;
                    progressCallback?.Invoke((double)i / bytesCount);
                }
            }
        }

        public static void DecryptFile(string encryptedPath, string keyPath, string decryptPath = null)
        {
            DecryptFile(encryptedPath, keyPath, decryptPath, 0, null);
        }

        public static Task DecryptFileAsync(string encryptedPath, string keyPath,
            Action<double> progressCallback = null, string decryptPath = null)
        {
            return Task.Run(() =>
            {
                DecryptFile(encryptedPath, keyPath, decryptPath, 100000, progressCallback);
            });
        }

        private static void DecryptFile(string encryptedPath, string keyPath, string decryptPath, 
            int bufSize, Action<double> progressCallback = null)
        {
            if (decryptPath is null)
            {
                if (encryptedPath.EndsWith(".v399"))
                {
                    decryptPath = encryptedPath.Substring(0, encryptedPath.Length - 5);
                }
                else
                {
                    string dirName = Path.GetDirectoryName(encryptedPath);
                    decryptPath = Path.Combine(dirName, "decrypted");
                }
            }

            using (FileStream inStream = new FileStream(encryptedPath, FileMode.Open, FileAccess.Read))
            using (FileStream inKeyStream = new FileStream(keyPath, FileMode.Open, FileAccess.Read))
            using (FileStream outStream = new FileStream(decryptPath, FileMode.OpenOrCreate, FileAccess.Write))
            using (CryptoStream outCrypto = new CryptoStream(outStream, new VernamCryptoTransform(inKeyStream), CryptoStreamMode.Write))
            {
                inStream.CopyToEx(outCrypto, bufSize, progressCallback);
            }
        }
        
    }

    public class VernamCryptoTransform : ICryptoTransform
    {
        public int InputBlockSize => 1;

        public int OutputBlockSize => 1;

        public bool CanTransformMultipleBlocks => true;

        public bool CanReuseTransform => false;

        private BufferedStream _keyStream;

        public VernamCryptoTransform(Stream keyStream)
        {
            _keyStream = new BufferedStream(keyStream);
        }

        public void Dispose()
        {
            _keyStream.Dispose();
        }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            byte[] keyPart = new byte[inputCount];
            int hasRead = _keyStream.Read(keyPart, 0, inputCount);
            if (hasRead < inputCount)
            {
                Console.WriteLine("parasha");
                throw new Exception("Can't read enough bytes.");// TODO exc
            }

            for (int i = 0; i < inputCount; ++i)
            {
                outputBuffer[outputOffset + i] = (byte)(inputBuffer[inputOffset + i] ^ keyPart[i]);// TODO think
            }
            return inputCount;
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            byte[] result = new byte[inputCount];
            TransformBlock(inputBuffer, inputOffset, inputCount, result, 0);
            return result;
        }
    }

}
