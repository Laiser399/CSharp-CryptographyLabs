using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Crypto
{
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
