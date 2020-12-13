using CryptographyLabs.Crypto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CryptographyLabs;
using CryptographyLabs.Crypto.BlockCouplingModes;

namespace UnitTests
{
    [TestClass]
    public class DESTest
    {
        private byte[] _text;
        private ulong _key;

        public DESTest()
        {
            Random random = new Random(992);
            int bytesCount = 800_001;
            _text = new byte[bytesCount];
            random.NextBytes(_text);

            byte[] keyTm = new byte[8];
            random.NextBytes(keyTm);
            _key = BitConverter.ToUInt64(keyTm, 0);
        }

        // TODO vernam tests
        [TestMethod]
        public void Test1_DESCryptoTransform()
        {
            var enTransform = DES_.Get(_key, DES_.Mode.ECB, CryptoDirection.Encrypt);
            var deTransform = DES_.Get(_key, DES_.Mode.ECB, CryptoDirection.Decrypt);

            Check(enTransform, deTransform);
        }

        [TestMethod]
        public void Test2_CBC()
        {
            var enTransform = DES_.Get(_key, DES_.Mode.CBC, CryptoDirection.Encrypt);
            var deTransform = DES_.Get(_key, DES_.Mode.CBC, CryptoDirection.Decrypt);

            Check(enTransform, deTransform);
        }

        [TestMethod]
        public void Test3_CFB()
        {
            var enTransform = DES_.Get(_key, DES_.Mode.CFB, CryptoDirection.Encrypt);
            var deTransform = DES_.Get(_key, DES_.Mode.CFB, CryptoDirection.Decrypt);

            Check(enTransform, deTransform);
        }

        [TestMethod]
        public void Test4_OFB()
        {
            var enTransform = DES_.Get(_key, DES_.Mode.OFB, CryptoDirection.Encrypt);
            var deTransform = DES_.Get(_key, DES_.Mode.OFB, CryptoDirection.Decrypt);

            Check(enTransform, deTransform);
        }

        [TestMethod]
        public void Test5_CTR()
        {
            // TODO crt test
            //var enTransform = DES_.CTRTransform(_key);
            //var deTransform = DES_.CTRTransform(_key);

            //Check(enTransform, deTransform);
        }

        private void Check(ICryptoTransform enTransform, ICryptoTransform deTransform)
        {
            byte[] encrypted = Transform(_text, enTransform);
            byte[] decrypted = Transform(encrypted, deTransform);

            Assert.AreEqual(_text.Length, decrypted.Length);
            for (int i = 0; i < _text.Length; ++i)
                Assert.AreEqual(_text[i], decrypted[i]);
        }

        private byte[] Transform(byte[] text, ICryptoTransform transform)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                using (MemoryStream inStream = new MemoryStream(text))
                using (CryptoStream crStream = new CryptoStream(inStream, transform, CryptoStreamMode.Read))
                {
                    crStream.CopyTo(outStream);
                }
                return outStream.ToArray();
            }
        }
    }
}
