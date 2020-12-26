using CryptographyLabs.Crypto;
using CryptographyLabs.Crypto.BlockCouplingModes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    
    public class MultithreadTransformTest
    {
        [Test]
        public async Task Test()
        {
            Random random = new Random(126);
            byte[] tmKey = new byte[8];
            random.NextBytes(tmKey);
            ulong key56 = BitConverter.ToUInt64(tmKey);

            byte[] text = new byte[800_003];
            random.NextBytes(text);

            INiceCryptoTransform encryptor = DES_.GetNice(key56, CryptoDirection.Encrypt);
            INiceCryptoTransform decryptor = DES_.GetNice(key56, CryptoDirection.Decrypt);

            byte[] encrypted;

            int bytesPerIter = 100 * encryptor.InputBlockSize;
            using (MemoryStream outStream = new MemoryStream())
            {
                int blocksCount = text.Length / bytesPerIter;
                int remains = text.Length % bytesPerIter;
                if (remains == 0)
                {
                    blocksCount--;
                    remains = bytesPerIter;
                }
                byte[] tm;
                for (int i = 0; i < blocksCount; i++)
                {
                    tm = await ECB.TransformAsync(text, i * bytesPerIter, bytesPerIter, encryptor, 4);
                    outStream.Write(tm);
                }
                tm = await ECB.TransformFinalAsync(text, blocksCount * bytesPerIter, remains, encryptor, 4);
                outStream.Write(tm);

                encrypted = outStream.ToArray();
            }

            byte[] decrypted;

            bytesPerIter = 100 * decryptor.InputBlockSize;
            using (MemoryStream outStream = new MemoryStream())
            {
                int blocksCount = encrypted.Length / bytesPerIter;
                int remains = encrypted.Length % bytesPerIter;
                if (remains == 0)
                {
                    blocksCount--;
                    remains = bytesPerIter;
                }

                byte[] tm;
                for (int i = 0; i < blocksCount; i++)
                {
                    tm = await ECB.TransformAsync(encrypted, i * bytesPerIter, bytesPerIter, decryptor, 4);
                    outStream.Write(tm);
                }
                tm = await ECB.TransformFinalAsync(encrypted, blocksCount * bytesPerIter, 
                    remains, decryptor, 4);
                outStream.Write(tm);

                decrypted = outStream.ToArray();
            }

            Assert.AreEqual(text.Length, decrypted.Length);
            for (int i = 0; i < text.Length; i++)
            {
                Assert.AreEqual(text[i], decrypted[i]);
            }
        }
    }
}
